using EnvDTE;

using ORM.VSPackage.Helper;
using ORM.VSPackage.ImportWindowSqlServer.Models;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORM.VSPackage.Generator
{
    public interface IGenerateEntities
    {
        Task Execute(
            Project project,
            IEnumerable<TableDefinition> tableDefinitions,
            string templatePath);
    }

    public class GenerateEntities : IGenerateEntities
    {
        public async Task Execute(
            Project project, 
            IEnumerable<TableDefinition> tableDefinitions,
            string templatePath)
        {
            await Task.Run(() =>
            {
                var modelProjectItem = project.ProjectItems.AddFolder(NamespaceValues.Models);
                GenerateModels(tableDefinitions, modelProjectItem, templatePath);
            });
        }


        private static void GenerateModels(
            IEnumerable<TableDefinition> tableDefinitions,
            ProjectItem modelProjectItem,
            string templatePath)
        {
            // Generate the files
            foreach (var tableDefinition in tableDefinitions)
            {
                modelProjectItem.ProjectItems.AddFromTemplate(templatePath,
                    TableDefinitionHelper.GetModelFileName(tableDefinition) + ".cs");
            }

            foreach (ProjectItem projectItem in modelProjectItem.ProjectItems)
            {
                var name = projectItem.Name;
                var tableDefinition =
                    tableDefinitions.Single(t => TableDefinitionHelper.GetModelFileName(t) + ".cs" == name);
                var cls = CodeModelHelper.GetCodeClassFromFileCode(projectItem.FileCodeModel);
                cls.Access = vsCMAccess.vsCMAccessPublic;

                // Create the fields
                foreach (var columnDefinition in tableDefinition.ColumnDefinitions)
                {
                    var type = ColumnDefinitionHelper.GetRefTypeOfColumnDefinition(columnDefinition);
                    var fieldName = ColumnDefinitionHelper.GetPropertyName(columnDefinition);
                    cls.AddVariable(
                        fieldName,
                        type,
                        -1,
                        vsCMAccess.vsCMAccessPrivate);
                }

                // Create the properties
                foreach (var columnDefinition in tableDefinition.ColumnDefinitions)
                {
                    var type = ColumnDefinitionHelper.GetRefTypeOfColumnDefinition(columnDefinition);
                    var fieldName = ColumnDefinitionHelper.GetPropertyName(columnDefinition);
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
    }
}
