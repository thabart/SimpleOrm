using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.ComponentModel.Design;

using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.Shell;

using ORM.VSPackage.Identifiers;
using ORM.VSPackage.ImportWindowSqlServer;

using EnvDTE;

using Microsoft.VisualStudio;
using EnvDTE80;

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
    // This attribute registers a tool window exposed by this package.
    [ProvideToolWindow(typeof(MyToolWindow))]
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

        /// <summary>
        /// This function is called when the user clicks the menu item that shows the 
        /// tool window. See the Initialize method to see how the menu item is associated to 
        /// this function using the OleMenuCommandService service and the MenuCommand class.
        /// </summary>
        private void ShowToolWindow(object sender, EventArgs e)
        {
            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            ToolWindowPane window = this.FindToolWindow(typeof(MyToolWindow), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException("cannot create windows");
            }
            IVsWindowFrame windowFrame = (IVsWindowFrame)window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(windowFrame.Show());
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
            Debug.WriteLine (string.Format(CultureInfo.CurrentCulture, "Entering Initialize() of: {0}", this.ToString()));
            base.Initialize();

            var mcs = GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if ( null != mcs )
            {
                // Create the command for the menu item.
                var menuCommandID = new CommandID(GuidList.guidORM_VSPackageCmdSet, (int)PkgCmdIdList.ormCommand);
                var menuItem = new MenuCommand(MenuItemCallback, menuCommandID );
                mcs.AddCommand( menuItem );
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
            Project project = GetSelectedProject();
            Solution2 solution = (Solution2)project.DTE.Solution;
            string templatePath = solution.GetProjectItemTemplate("Class", "CSharp");
            ProjectItem modelProjectItem = project.ProjectItems.AddFolder("Models");
            ProjectItem dbContextProjectItem = project.ProjectItems.AddFolder("DbContext");
            modelProjectItem.ProjectItems.AddFromTemplate(templatePath, "model1.cs");
            foreach(ProjectItem projectItem in modelProjectItem.ProjectItems)
            {
                Trace.WriteLine(projectItem.Name);
                CodeElements codeElements = projectItem.FileCodeModel.CodeElements;
                foreach (CodeElement codeElement in codeElements)
                {
                    if (codeElement.Kind == vsCMElement.vsCMElementNamespace)
                    {
                        var children = codeElement.Children;
                        foreach (CodeElement childCodeElement in children)
                        {
                            if (childCodeElement.Kind == vsCMElement.vsCMElementClass)
                            {
                                CodeClass cls = (CodeClass)childCodeElement;
                                cls.Access = vsCMAccess.vsCMAccessPublic;
                                cls.AddProperty("TestProperty", "TestProperty", vsCMTypeRef.vsCMTypeRefInt, -1,
                                    vsCMAccess.vsCMAccessPublic, null);
                            }
                        }
                    }
                }
            }

            var importView = new ImportView();
            importView.Show();
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
            var monitorSelection = (IVsMonitorSelection)GetGlobalService(typeof(SVsShellMonitorSelection));
            monitorSelection.GetCurrentSelection(
                out hierarchyPointer,
                out projectItemId, 
                out multiItemSelect,
                out selectionContainerPointer);
            IVsHierarchy selectedHierarchy = Marshal.GetTypedObjectForIUnknown(
                                     hierarchyPointer,
                                     typeof(IVsHierarchy)) as IVsHierarchy;

            if (selectedHierarchy != null)
            {
                ErrorHandler.ThrowOnFailure(selectedHierarchy.GetProperty(
                                                  projectItemId,
                                                  (int)__VSHPROPID.VSHPROPID_ExtObject,
                                                  out selectedObject));
            }

            return selectedObject as Project;
        }
    }
}
