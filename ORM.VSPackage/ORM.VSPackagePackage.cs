using System;
using System.CodeDom;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;

using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;

using ORM.VSPackage.Identifiers;
using ORM.VSPackage.ImportWindowSqlServer;

using EnvDTE;

using Microsoft.VisualStudio;
using EnvDTE80;
using ORM.VSPackage.ImportWindowSqlServer.CustomEventArgs;
using System.Collections.Generic;
using ORM.VSPackage.ImportWindowSqlServer.Models;
using CodeNamespace = EnvDTE.CodeNamespace;

namespace ORM.VSPackage
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    ///
    /// The minimum requirement for a class to be considered a valid package for Visual Studio
    /// is to implement the IVsPackage interface and register itself with the shell.
    /// This package uses the helper classes defined inside the Managed Package Framework (MPF)
    /// to do it: it derives from the Package class that provides the implementation of the 
    /// IVsPackage interface and uses the registration attributes defined in the framework to 
    /// register itself and its components with the shell.
    /// </summary>
    // This attribute tells the PkgDef creation utility (CreatePkgDef.exe) that this class is
    // a package.
    [PackageRegistration(UseManagedResourcesOnly = true)]
    // This attribute is used to register the information needed to show this package
    // in the Help/About dialog of Visual Studio.
    [InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)]
    // This attribute is needed to let the shell know that this package exposes some menus.
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideKeyBindingTable(GuidList.guidORM_VSPackageEditorFactoryString, 102)]
    // Our Editor supports Find and Replace therefore we need to declare support for LOGVIEWID_TextView.
    // This attribute declares that your EditorPane class implements IVsCodeWindow interface
    // used to navigate to find results from a "Find in Files" type of operation.
    [Guid(GuidList.guidORM_VSPackagePkgString)]
    public sealed class ORM_VSPackagePackage : Package
    {
        /// <summary>
        /// Default constructor of the package.
        /// Inside this method you can place any initialization code that does not require 
        /// any Visual Studio service because at this point the package object is created but 
        /// not sited yet inside Visual Studio environment. The place to do all the other 
        /// initialization is the Initialize method.
        /// </summary>
        public ORM_VSPackagePackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering constructor for: {0}", this.ToString()));
        }


        /////////////////////////////////////////////////////////////////////////////
        // Overridden Package Implementation

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        protected override void Initialize()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            var mcs = GetService(typeof (IMenuCommandService)) as OleMenuCommandService;
            if (null != mcs)
            {
                // Create the command for the menu item.
                var menuCommandID = new CommandID(GuidList.guidORM_VSPackageCmdSet, (int) PkgCmdIdList.ormCommand);
                var menuItem = new MenuCommand(MenuItemCallback, menuCommandID);
                mcs.AddCommand(menuItem);
            }
        }

        #endregion

        /// <summary>
        /// This function is the callback used to execute a command when the a menu item is clicked.
        /// See the Initialize method to see how the menu item is associated to this function using
        /// the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void MenuItemCallback(object sender, EventArgs e)
        {
            var importView = new ImportView();
            importView.Show();
            var viewModel = importView.GetViewModel();
            viewModel.ImportTablesEvent += ImportTablesEventCallback;
        }

        /// <summary>
        /// Get the selected project
        /// </summary>
        /// <returns>EnvDTE project</returns>
        private static Project GetSelectedProject()
        {
            IntPtr hierarchyPointer, selectionContainerPointer;
            Object selectedObject = null;
            IVsMultiItemSelect multiItemSelect;
            uint projectItemId;
            var monitorSelection = (IVsMonitorSelection) GetGlobalService(typeof (SVsShellMonitorSelection));
            monitorSelection.GetCurrentSelection(
                out hierarchyPointer,
                out projectItemId,
                out multiItemSelect,
                out selectionContainerPointer);
            var selectedHierarchy = Marshal.GetTypedObjectForIUnknown(
                hierarchyPointer,
                typeof (IVsHierarchy)) as IVsHierarchy;

            if (selectedHierarchy != null)
            {
                ErrorHandler.ThrowOnFailure(selectedHierarchy.GetProperty(
                    projectItemId,
                    (int) __VSHPROPID.VSHPROPID_ExtObject,
                    out selectedObject));
            }

            return selectedObject as Project;
        }

        private void ImportTablesEventCallback(object sender, ImportTablesEventArgs args)
        {
            var tableDefinitions = args.TableDefinitions;
            var project = GetSelectedProject();
            var solution = (Solution2) project.DTE.Solution;
            var templatePath = solution.GetProjectItemTemplate("Class", "CSharp");
            var modelProjectItem = project.ProjectItems.AddFolder("Models");
            var dbContextProjectItem = project.ProjectItems.AddFolder("DbContext");
            var mappingsProjectItem = project.ProjectItems.AddFolder("Mappings");

            // 1. Refactor this class (SR)
            // 2. Add nuget package : http://tylerhughes.info/post/installing-a-nuget-package-programmatically
            // 3. Modify the configuration file (add the connection string)
            // 4. Release the nuget package & fix release appveyor

            GenerateModels(tableDefinitions, modelProjectItem, templatePath);
            GenerateMappings(tableDefinitions, mappingsProjectItem, templatePath);
            GenerateDbContext(tableDefinitions, dbContextProjectItem, templatePath);
        }

        private static void GenerateModels(
            IEnumerable<TableDefinition> tableDefinitions,
            ProjectItem modelProjectItem,
            string templatePath)
        {
            var csFilePattern = "{0}.cs";

            // Generate the files
            foreach (var tableDefinition in tableDefinitions)
            {
                modelProjectItem.ProjectItems.AddFromTemplate(templatePath,
                    string.Format(csFilePattern, tableDefinition.TableName));
            }

            foreach (ProjectItem projectItem in modelProjectItem.ProjectItems)
            {
                var name = projectItem.Name;
                var tableDefinition =
                    tableDefinitions.SingleOrDefault(t => string.Format(csFilePattern, t.TableName) == name);
                var cls = GetCodeClassFromFileCode(projectItem.FileCodeModel);

                cls.Access = vsCMAccess.vsCMAccessPublic;

                // For each column definition we create a field
                foreach (var columnDefinition in tableDefinition.ColumnDefinitions)
                {
                    var type = GetRefTypeOfColumnDefinition(columnDefinition);

                    var fieldName = "_" + columnDefinition.ColumnName.ToLower();
                    cls.AddVariable(
                        fieldName,
                        type,
                        -1,
                        vsCMAccess.vsCMAccessPrivate);
                }

                // For each column definitions we create a property
                foreach (var columnDefinition in tableDefinition.ColumnDefinitions)
                {
                    var fieldName = "_" + columnDefinition.ColumnName.ToLower();
                    var type = GetRefTypeOfColumnDefinition(columnDefinition);
                    CodeProperty property = cls.AddProperty(columnDefinition.ColumnName,
                        columnDefinition.ColumnName,
                        type, -1,
                        vsCMAccess.vsCMAccessPublic,
                        null);

                    // For more information about how to add a property read this book :
                    // http://tinyurl.com/pao4fu5
                    var epGetter =
                        property.Getter.GetStartPoint(vsCMPart.vsCMPartBody).CreateEditPoint();
                    epGetter.Delete(property.Getter.GetEndPoint(vsCMPart.vsCMPartBody));
                    epGetter.Indent();
                    epGetter.Insert(string.Format("return {0}; \n", fieldName));
                    epGetter.Indent(Count: 3);

                    var epSetter =
                        property.Setter.GetStartPoint(vsCMPart.vsCMPartBody).CreateEditPoint();
                    epSetter.Delete(property.Setter.GetEndPoint(vsCMPart.vsCMPartBody));
                    epSetter.Indent();
                    epSetter.Insert(string.Format("{0} = value; \n", fieldName));
                    epSetter.Indent(Count: 3);
                }
            }
        }

        private static void GenerateMappings(
            IEnumerable<TableDefinition> tableDefinitions,
            ProjectItem mappingsProjectItem,
            string templatePath)
        {
            // More information about EnvDte usage at this url : http://www.techheadbrothers.com/Articles.aspx/introduction-codedom
            var mappingFilePatternName = "{0}Mapping.cs";

            // Create the files
            foreach (var tableDefinition in tableDefinitions)
            {
                var mappingFileName = string.Format(mappingFilePatternName, tableDefinition.TableName);
                mappingsProjectItem.ProjectItems.AddFromTemplate(templatePath, mappingFileName);
            }

            // Enrich the code.
            foreach (ProjectItem projectItem in mappingsProjectItem.ProjectItems)
            {
                var name = projectItem.Name;
                var tableDefinition = tableDefinitions.SingleOrDefault(t => string.Format(mappingFilePatternName, t.TableName) == name);
                var codeNamespace = GetNameSpaceFromFileCode(projectItem.FileCodeModel);
                var fullNamespace = codeNamespace.FullName.Replace("Mappings", "Models");

                // Add the import instructions.
                var fileCodeModel = (FileCodeModel2) projectItem.FileCodeModel;
                fileCodeModel.AddImport(fullNamespace);
                fileCodeModel.AddImport("ORM.Mappings");

                // Add inheritance to BaseMapping.
                var cls = GetCodeClassFromFileCode(projectItem.FileCodeModel);
                cls.Access = vsCMAccess.vsCMAccessPublic;
                var fullyQualifiedName = string.Format("BaseMapping<{0}>", tableDefinition.TableName);
                cls.AddBase(fullyQualifiedName);
                
                // Modify the constructor
                cls.AddFunction(tableDefinition.TableName + "Mapping",
                    vsCMFunction.vsCMFunctionConstructor,
                    null,
                    -1,
                    vsCMAccess.vsCMAccessPublic, null);
                var constructor = (CodeFunction)cls.Children.Item(1);
                var startPoint = constructor.GetStartPoint(vsCMPart.vsCMPartBody).CreateEditPoint();
                startPoint.StartOfLine();
                startPoint.Indent();
                startPoint.Insert("ToTable(\"" + tableDefinition.TableSchema + "." + tableDefinition.TableName + "\"); \n");
                
                foreach (var columnDefinition in tableDefinition.ColumnDefinitions)
                {
                    var statement = "Property(t => t." + columnDefinition.ColumnName + ").HasColumnName(\"" +
                                    columnDefinition.ColumnName + "\"); \n";
                    startPoint.Indent(Count: 3);
                    startPoint.Insert(statement);
                }

                startPoint.EndOfLine();
                startPoint.Indent(Count: 2);
            }
        }

        private static void GenerateDbContext(
            IEnumerable<TableDefinition> tableDefinitions,
            ProjectItem dbContextProjectItem,
            string templatePath) {
            dbContextProjectItem.ProjectItems.AddFromTemplate(templatePath, "DbContext.cs");
            var projectItem = dbContextProjectItem.ProjectItems.Item(1);
            var codeNamespace = GetNameSpaceFromFileCode(projectItem.FileCodeModel);
            var modelNamespace = codeNamespace.FullName.Replace("DbContext", "Models");
            var mappingNamespace = codeNamespace.FullName.Replace("DbContext", "Mappings");
            
            // Add the import instruction
            var fileCodeModel = (FileCodeModel2)projectItem.FileCodeModel;
            fileCodeModel.AddImport("ORM.Mappings");
            fileCodeModel.AddImport("ORM.Core");
            fileCodeModel.AddImport(modelNamespace);
            fileCodeModel.AddImport(mappingNamespace);

            // Add inheritance to BaseDbContext
            var cls = GetCodeClassFromFileCode(projectItem.FileCodeModel);
            cls.Access = vsCMAccess.vsCMAccessPublic;
            cls.AddBase("BaseDbContext");

            // Add fields
            foreach(var tableDefinition in tableDefinitions)
            {
                var fieldName = "_" + tableDefinition.TableName.ToLower();
                cls.AddVariable(
                    fieldName,
                    "IDbSet<" + tableDefinition.TableName + ">",
                    -1,
                    vsCMAccess.vsCMAccessPrivate);
            }

            // Add constructor
            var constructorStartPoint = cls.GetStartPoint(vsCMPart.vsCMPartBody).CreateEditPoint();
            constructorStartPoint.LineDown(tableDefinitions.Count());
            constructorStartPoint.Indent(Count: 1);
            constructorStartPoint.Insert("public DbContext() : base(\"CustomConnectionString\") { } \n");

            // Add properties
            foreach (var tableDefinition in tableDefinitions)
            {
                var fieldName = "_" + tableDefinition.TableName.ToLower();
                CodeProperty property = cls.AddProperty(tableDefinition.TableName+"s",
                    tableDefinition.TableName+"s", 
                    "IDbSet<" + tableDefinition.TableName + ">",
                    -1,
                    vsCMAccess.vsCMAccessPublic,
                    null);

                var epGetter =
                    property.Getter.GetStartPoint(vsCMPart.vsCMPartBody).CreateEditPoint();
                epGetter.Delete(property.Getter.GetEndPoint(vsCMPart.vsCMPartBody));
                epGetter.Indent();
                epGetter.Insert(string.Format("return {0}; \n", fieldName));
                epGetter.Indent(Count: 3);

                var epSetter =
                    property.Setter.GetStartPoint(vsCMPart.vsCMPartBody).CreateEditPoint();
                epSetter.Delete(property.Setter.GetEndPoint(vsCMPart.vsCMPartBody));
                epSetter.Indent();
                epSetter.Insert(string.Format("{0} = value; \n", fieldName));
                epSetter.Indent(Count: 3);
            }

            // Add mappings
            var mappingEndPoint = cls.GetEndPoint(vsCMPart.vsCMPartBody).CreateEditPoint();
            mappingEndPoint.Indent(Count : 1);
            mappingEndPoint.Insert("protected override void Mappings(IEntityMappingContainer entityMappingContainer) { \n");
            foreach(var tableDefinition in tableDefinitions)
            {
                var mappingName = tableDefinition.TableName + "Mapping";
                mappingEndPoint.Indent(Count: 3);
                mappingEndPoint.Insert("entityMappingContainer.AddMapping(new " + mappingName + "()); \n");
                
            }

            mappingEndPoint.Indent(Count: 2);
            mappingEndPoint.Insert("} \n");
        }

        /// <summary>
        /// Returns the code class from the file code model.
        /// </summary>
        /// <param name="fileCodeModel"></param>
        private static CodeClass GetCodeClassFromFileCode(FileCodeModel fileCodeModel)
        {
            var codeNameSpace = GetNameSpaceFromFileCode(fileCodeModel);
            foreach (CodeElement namespaceChild in codeNameSpace.Children)
            {
                if (namespaceChild.Kind == vsCMElement.vsCMElementClass)
                {
                    return (CodeClass)namespaceChild;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the namespace element from file code.
        /// </summary>
        /// <param name="fileCodeModel"></param>
        /// <returns></returns>
        private static CodeNamespace GetNameSpaceFromFileCode(FileCodeModel fileCodeModel)
        {
            var codeElements = fileCodeModel.CodeElements;
            foreach (CodeElement codeElement in codeElements)
            {
                if (codeElement.Kind == vsCMElement.vsCMElementNamespace)
                {
                    return (CodeNamespace) codeElement;
                }
            }

            return null;
        }

        private static vsCMTypeRef GetRefTypeOfColumnDefinition(ColumnDefinition columnDefinition)
        {
            var type = vsCMTypeRef.vsCMTypeRefString;
            switch (columnDefinition.ColumnType)
            {
                case "varchar":
                case "uniqueidentifier":
                    type = vsCMTypeRef.vsCMTypeRefString;
                    break;
            }

            return type;
        }
    }
}
