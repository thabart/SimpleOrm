using Company.OrmLanguage.Window;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;

namespace Company.OrmLanguage
{
    internal partial class OrmLanguageCommandSet
    {
        public Guid cmdViewSimpleOrmMappingRulesGuid = new Guid("EC120F9A-9E7F-469d-8D61-F4E2A97E5725");
        public const int cmdViewSimpleOrmMappingRulesId = 0x810;

        protected override IList<MenuCommand> GetMenuCommands()
        {
            var commands = base.GetMenuCommands();
            var cmdViewSimpleOrmMappingRules = new DynamicStatusMenuCommand(
                OnPopUpMenuDisplayAction,
                OnPopUpMenuClick,
                new CommandID(cmdViewSimpleOrmMappingRulesGuid, cmdViewSimpleOrmMappingRulesId));
            commands.Add(cmdViewSimpleOrmMappingRules);
            return commands;
        }

        internal void OnPopUpMenuDisplayAction(object sender, EventArgs args)
        {
            var command = sender as MenuCommand;
            foreach (var selectedObject in CurrentSelection)
            {
                if (selectedObject is EntityShape)
                {
                    command.Visible = true;
                    command.Enabled = true;
                    return;
                }
            }

            command.Visible = false;
            command.Enabled = false;
        }

        internal void OnPopUpMenuClick(object sender, EventArgs args)
        {
            var showSimpleOrmWindow = ShowSimpleOrmWindowSingleton.Instance();
            if (showSimpleOrmWindow == null)
            {
                var package = ServiceProvider.GetService(typeof (Package)) as ModelingPackage;
                var simpleOrmMappingWindow = (SimpleOrmMappingWindow) package.FindToolWindow(typeof (SimpleOrmMappingWindow), 0, true);
                if (simpleOrmMappingWindow == null || simpleOrmMappingWindow.Frame == null)
                {
                    return;
                }

                ShowSimpleOrmWindowSingleton.Instanciate(simpleOrmMappingWindow);
            }

            var windowFrame = ShowSimpleOrmWindowSingleton.Instance().GetWindowFrame();
            var mappingWindow = ShowSimpleOrmWindowSingleton.Instance().GetOrmMappingWindow();

            foreach (var selectedObject in CurrentSelection)
            {
                if (selectedObject is EntityShape)
                {
                    var entityShape = selectedObject as EntityShape;
                    var modelElement = entityShape.ModelElement as EntityElement;
                    mappingWindow.EntityElement = modelElement;
                    ErrorHandler.ThrowOnFailure(windowFrame.Show());

                    return;
                }
            }
        }
    }
}
