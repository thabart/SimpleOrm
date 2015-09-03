using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System.Windows.Input;

using EnvDTE;
using EnvDTE80;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using System;

namespace ORM.VSPackage.ImportWindowSqlServer.ViewModels
{
    public class ImportViewModel : BindableBase
    {
        private bool _isWindowsAuthenticationEnabled;

        public ImportViewModel()
        {
            _isWindowsAuthenticationEnabled = false;
            RegisterCommands();
        }

        public ICommand EnableWindowsAuthenticationCommand { get; private set; }

        public ICommand TestCommand { get; private set; }

        public bool IsWindowsAuthenticationEnabled
        {
            get
            {
                return _isWindowsAuthenticationEnabled;
            } set
            {
                SetProperty(ref _isWindowsAuthenticationEnabled, value);
            }
        }

        private void RegisterCommands()
        {
            EnableWindowsAuthenticationCommand = new DelegateCommand(EnableWindowsAuthenticationExecute);
            TestCommand = new DelegateCommand(TestCommandExecute);
        }

        private void EnableWindowsAuthenticationExecute()
        {
            IsWindowsAuthenticationEnabled = !IsWindowsAuthenticationEnabled;
        }

        private void TestCommandExecute()
        {
            DTE2 dte2 = (DTE2)System.Runtime.InteropServices.Marshal.GetActiveObject("VisualStudio.DTE.14.0");
            Solution2 solution = (Solution2)dte2.Solution;
            Solution solution1 = dte2.Solution;

            var projectItemTemplate = solution.GetProjectItemTemplate("Class", "CSharp");
            Project project = null;
            foreach(Project proj in solution1.Projects)
            {
                if (proj.Name == "test")
                {
                    project = proj;
                }
            }
            
            var projectItem = project.ProjectItems.AddFromTemplate(projectItemTemplate, "NewClassInProj.cs");
            projectItem.Save();
            // project.ProjectItems.AddFolder("My folder");
            Trace.WriteLine("test");

            /*
            foreach (string kind in colKinds)
            {
                try
                {
                    // Add a solution folder "test"
                    project.ProjectItems.AddFolder(@"C:\Project\ORM\ORM.VSPackage.Window\Bingo", kind);
                } catch (Exception ex)
                {
                    Trace.WriteLine("coucou");
                }
            }*/
        }
    }
}
