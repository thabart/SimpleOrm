using EnvDTE;
using EnvDTE80;

using ORM.VSPackage.Helper;
using ORM.VSPackage.ImportWindowSqlServer.Models;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORM.VSPackage.Generator
{
    public interface IGenerateMappings
    {
        Task Execute(
            Project project,
            IEnumerable<TableDefinition> tableDefinitions,
            string templatePath);
    }

    public class GenerateMappings : IGenerateMappings
    {

        public async Task Execute(
            Project project,
            IEnumerable<TableDefinition> tableDefinitions,
            string templatePath)
        {
            await Task.Run(() =>
            {
                var mappingsProjectItem = project.ProjectItems.AddFolder(NamespaceValues.Mappings);
                Generate(tableDefinitions, mappingsProjectItem, templatePath);
            });
        }

        private static void Generate(
            IEnumerable<TableDefinition> tableDefinitions,
            ProjectItem mappingsProjectItem,
            string templatePath)
        {            
            // Create mapping files
            foreach (var tableDefinition in tableDefinitions)
            {
                var mappingFileName = TableDefinitionHelper.GetMappingFileName(tableDefinition) + ".cs";
                mappingsProjectItem.ProjectItems.AddFromTemplate(templatePath, mappingFileName);
            }

            // Add mapping instructions
            foreach (ProjectItem projectItem in mappingsProjectItem.ProjectItems)
            {
                var name = projectItem.Name;
                var tableDefinition = tableDefinitions.SingleOrDefault(t => TableDefinitionHelper.GetMappingFileName(t) + ".cs" == name);
                var codeNamespace = CodeModelHelper.GetNameSpaceFromFileCode(projectItem.FileCodeModel);
                var fullNamespace = codeNamespace.FullName.Replace(NamespaceValues.Mappings, NamespaceValues.Models);

                // Add the import instructions.
                var fileCodeModel = (FileCodeModel2)projectItem.FileCodeModel;
                fileCodeModel.AddImport(fullNamespace);
                fileCodeModel.AddImport(NamespaceValues.OrmMappings);

                // Add inheritance to BaseMapping.
                var cls = CodeModelHelper.GetCodeClassFromFileCode(projectItem.FileCodeModel);
                cls.Access = vsCMAccess.vsCMAccessPublic;
                var fullyQualifiedName = string.Format("BaseMapping<{0}>", tableDefinition.TableName);
                cls.AddBase(fullyQualifiedName);

                // Modify the constructor
                cls.AddFunction(TableDefinitionHelper.GetMappingFileName(tableDefinition),
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
    }
}
