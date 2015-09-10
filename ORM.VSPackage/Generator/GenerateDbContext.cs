using EnvDTE;
using EnvDTE80;

using ORM.VSPackage.Helper;
using ORM.VSPackage.ImportWindowSqlServer.Models;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORM.VSPackage.Generator
{
    public interface IGenerateDbContext
    {
        Task Execute(
            Project project,
            IEnumerable<TableDefinition> tableDefinitions,
            string templatePath);
    }

    public class GenerateDbContext : IGenerateDbContext
    {
        public async Task Execute(
            Project project,
            IEnumerable<TableDefinition> tableDefinitions,
            string templatePath)
        {
            await Task.Run(() =>
            {
                var dbContextProjectItem = project.ProjectItems.AddFolder(NamespaceValues.DbContext);
                Generate(tableDefinitions, dbContextProjectItem, templatePath);
            });
        }

        private static void Generate(
            IEnumerable<TableDefinition> tableDefinitions,
            ProjectItem dbContextProjectItem,
            string templatePath)
        {
            dbContextProjectItem.ProjectItems.AddFromTemplate(templatePath, "DbContext.cs");
            var projectItem = dbContextProjectItem.ProjectItems.Item(1);
            var codeNamespace = CodeModelHelper.GetNameSpaceFromFileCode(projectItem.FileCodeModel);
            var modelNamespace = codeNamespace.FullName.Replace(NamespaceValues.DbContext, NamespaceValues.Models);
            var mappingNamespace = codeNamespace.FullName.Replace(NamespaceValues.DbContext, NamespaceValues.Mappings);

            // Add the import instruction
            var fileCodeModel = (FileCodeModel2)projectItem.FileCodeModel;
            fileCodeModel.AddImport(NamespaceValues.OrmMappings);
            fileCodeModel.AddImport(NamespaceValues.OrmCore);
            fileCodeModel.AddImport(modelNamespace);
            fileCodeModel.AddImport(mappingNamespace);

            // Add inheritance to BaseDbContext
            var cls = CodeModelHelper.GetCodeClassFromFileCode(projectItem.FileCodeModel);
            cls.Access = vsCMAccess.vsCMAccessPublic;
            cls.AddBase("BaseDbContext");

            // Add fields
            foreach (var tableDefinition in tableDefinitions)
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
                CodeProperty property = cls.AddProperty(tableDefinition.TableName + "s",
                    tableDefinition.TableName + "s",
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
            mappingEndPoint.Indent(Count: 1);
            mappingEndPoint.Insert("protected override void Mappings(IEntityMappingContainer entityMappingContainer) { \n");
            foreach (var tableDefinition in tableDefinitions)
            {
                var mappingName = tableDefinition.TableName + "Mapping";
                mappingEndPoint.Indent(Count: 3);
                mappingEndPoint.Insert("entityMappingContainer.AddMapping(new " + mappingName + "()); \n");

            }

            mappingEndPoint.Indent(Count: 2);
            mappingEndPoint.Insert("} \n");
        }
    }
}
