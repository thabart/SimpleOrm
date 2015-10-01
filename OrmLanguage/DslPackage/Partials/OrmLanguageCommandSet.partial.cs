using Company.OrmLanguage.Partials;
using Company.OrmLanguage.Window;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Modeling.Shell;
using Microsoft.VisualStudio.Shell;

using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using ORM.VSPackage.ImportWindowSqlServer.CustomEventArgs;

namespace Company.OrmLanguage
{
    internal partial class OrmLanguageCommandSet
    {
        public Guid cmdViewSimpleOrmMappingRulesGuid = new Guid("EC120F9A-9E7F-469d-8D61-F4E2A97E5725");

        public Guid cmdGenerateEntitiesGuid = new Guid("0D203C5B-36AE-41B0-871D-4B4216D13815");

        public const int cmdViewSimpleOrmMappingRulesId = 0x810;

        public const int cmdGenerateEntitiesId = 0x820;

        protected override IList<MenuCommand> GetMenuCommands()
        {
            var commands = base.GetMenuCommands();

            var cmdGenerateEntities = new DynamicStatusMenuCommand(
                OnPopupMenuDisplayActionGenerateEntities,
                OnPopupMenuClickGenerateEntities,
                new CommandID(cmdGenerateEntitiesGuid, cmdGenerateEntitiesId));
            var cmdViewSimpleOrmMappingRules = new DynamicStatusMenuCommand(
                OnPopUpMenuDisplayActionMappingRule,
                OnPopUpMenuClickMappingRule,
                new CommandID(cmdViewSimpleOrmMappingRulesGuid, cmdViewSimpleOrmMappingRulesId));

            commands.Add(cmdGenerateEntities);
            commands.Add(cmdViewSimpleOrmMappingRules);

            return commands;
        }

        internal void OnPopUpMenuDisplayActionMappingRule(object sender, EventArgs args)
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

        internal void OnPopUpMenuClickMappingRule(object sender, EventArgs args)
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

        internal void OnPopupMenuDisplayActionGenerateEntities(object sender, EventArgs args)
        {
            var command = sender as MenuCommand;
            foreach (var selectedObject in CurrentSelection)
            {
                if (selectedObject is OrmLanguageDiagram)
                {
                    command.Visible = true;
                    command.Enabled = true;
                    return;
                }
            }

            command.Visible = false;
            command.Enabled = false;
        }

        internal void OnPopupMenuClickGenerateEntities(object sender, EventArgs args)
        {
            foreach (var selectedObject in CurrentSelection)
            {
                if (selectedObject is OrmLanguageDiagram)
                {
                    var entityShape = selectedObject as OrmLanguageDiagram;
                    var modelElement = entityShape.ModelElement as SampleOrmModel;
                    var singleton = ShowGenerateTablesWindowSingleton.Instanciate((o, a) =>
                    {
                        DeleteEntities(modelElement);
                        GenerateEntities(a, modelElement);
                    });

                    singleton.Show();
                    return;
                }
            }
        }

        private void DeleteEntities(SampleOrmModel ormModel)
        {
            using (var transaction = ormModel.Store.TransactionManager.BeginTransaction())
            {
                ormModel.Elements.Clear();
                transaction.Commit();
            }
        }

        private void GenerateEntities(ImportTablesEventArgs args, SampleOrmModel ormModel)
        {
            using (var transaction = ormModel.Store.TransactionManager.BeginTransaction())
            {
                foreach (var tableDefinition in args.TableDefinitions)
                {
                    var entityElement = new EntityElement(ormModel.Store, null)
                    {
                        Name = tableDefinition.TableName,
                        TableName = tableDefinition.TableName
                    };

                    foreach (var columnDefinition in tableDefinition.ColumnDefinitions)
                    {
                        var property = new Property(ormModel.Store, null)
                        {
                            ColumnName = columnDefinition.ColumnName,
                            Name = columnDefinition.ColumnName,
                            Type = TypeCode.String
                        };

                        entityElement.Properties.Add(property);
                    }

                    ormModel.Elements.Add(entityElement);
                }

                transaction.Commit();
            }
        }
    }
}
