using EnvDTE;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.VisualStudio;
using ORM.VSPackage.Generator;
using ORM.VSPackage.Identifiers;
using ORM.VSPackage.ImportWindowSqlServer;
using ORM.VSPackage.ImportWindowSqlServer.CustomEventArgs;
using System;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Xml;

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
        private readonly IModelFirstApproachGenerator _modelFirstApproachGenerator;

        private readonly IManipulateConfigurationFile _manipulateConfigurationFile;

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
            _modelFirstApproachGenerator = new ModelFirstApproachGenerator();
            _manipulateConfigurationFile = new ManipulateConfigurationFile();
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
            viewModel.ImportTablesEvent += (s, a) => ImportTablesEventCallback(s, a);
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

        private async void ImportTablesEventCallback(object sender, ImportTablesEventArgs args)
        {
            var tableDefinitions = args.TableDefinitions;
            var connectionString = args.ConnectionString;
            var project = GetSelectedProject();

            // 1. Modify appveyor configuration to automatically create a release : contains the ORM dll & VSIX extension.

            await _modelFirstApproachGenerator.Execute(project, tableDefinitions);

            // Install the nuget package
            InstallNugetPackage(project, "SimpleOrm");

            // Modify the configuration file
            await _manipulateConfigurationFile.AddConnectionString(project, connectionString);
        }
        
        private static void InstallNugetPackage(
            Project project, 
            string package)
        {
            var dte = GetGlobalService(typeof(DTE)) as DTE;
            var componentModel = (IComponentModel)GetGlobalService(typeof(SComponentModel));
            var vsPackageInstallerServices = componentModel.GetService<IVsPackageInstallerServices>();
            if (!vsPackageInstallerServices.IsPackageInstalled(project, package))
            {
                dte.StatusBar.Text = "Installing " + package + " Nuget package, this may takes a minute ...";
                var vsPackageInstaller = componentModel.GetService<IVsPackageInstaller>();
                vsPackageInstaller.InstallPackage(null, project, package, (Version)null, false);
                dte.StatusBar.Text = @"Finished installing the " + package + " Nuget package";
            };
        }
    }
}
