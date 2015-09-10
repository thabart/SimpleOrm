using EnvDTE;
using EnvDTE80;

using ORM.VSPackage.ImportWindowSqlServer.Models;

using System.Collections.Generic;
using System.Threading.Tasks;

namespace ORM.VSPackage.Generator
{
    public interface IModelFirstApproachGenerator
    {
        Task Execute(Project project,
            IEnumerable<TableDefinition> tableDefinitions);
    }

    public class ModelFirstApproachGenerator : IModelFirstApproachGenerator
    {
        private readonly IGenerateDbContext _generateDbContext;

        private readonly IGenerateEntities _generateEntities;

        private readonly IGenerateMappings _generateMappings;

        public ModelFirstApproachGenerator()
        {
            _generateDbContext = new GenerateDbContext();
            _generateEntities = new GenerateEntities();
            _generateMappings = new GenerateMappings();
        }

        public async Task Execute(Project project,
            IEnumerable<TableDefinition> tableDefinitions)
        {
            var solution = (Solution2)project.DTE.Solution;
            var templatePath = solution.GetProjectItemTemplate("Class", "CSharp");
            await _generateDbContext.Execute(project, tableDefinitions, templatePath);
            await _generateEntities.Execute(project, tableDefinitions, templatePath);
            await _generateMappings.Execute(project, tableDefinitions, templatePath);
        }
    }
}
