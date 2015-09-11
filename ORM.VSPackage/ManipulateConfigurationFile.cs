using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml;

using EnvDTE;

namespace ORM.VSPackage
{
    public interface IManipulateConfigurationFile
    {
        Task AddConnectionString(
            Project project,
            string connectionString);
    }

    public class ManipulateConfigurationFile : IManipulateConfigurationFile
    {
        public async Task AddConnectionString(
            Project project,
            string connectionString)
        {
            await Task.Run(() =>
            {
                var configurationFile = GetConfigurationFile(project);
                if (configurationFile == null)
                {
                    return;
                }

                var fileInfo = new FileInfo(project.FullName);
                var configurationFullPath = fileInfo.DirectoryName + "\\" + configurationFile.Name;
                var document = new XmlDocument();
                document.Load(configurationFullPath);
                if (document.DocumentElement == null)
                {
                    return;
                }

                // If the connectionStrings element doesn't exist then create it.
                var connectionStrings = document.DocumentElement.SelectNodes("connectionStrings");
                XmlNode connectionStringElement;
                if (connectionStrings.Count == 0)
                {
                    connectionStringElement = document.CreateNode(XmlNodeType.Element, "connectionStrings", null);
                    document.DocumentElement.AppendChild(connectionStringElement);
                }
                else
                {
                    connectionStringElement = connectionStrings[0];
                }

                // If the add element doesn't exist then create it.
                var nodes = document.DocumentElement.SelectNodes(string.Format("connectionStrings/add[@name='{0}']", "CustomConnectionString"));
                XmlNode node;
                var newNode = nodes.Count == 0;
                if (newNode)
                {
                    node = document.CreateNode(XmlNodeType.Element, "add", null);
                    var nameAttribute = CreateAttribute(document, "name", "CustomConnectionString");
                    var connectionStringAttribute = CreateAttribute(document, "connectionString", string.Empty);
                    var providerNameAttribute = CreateAttribute(document, "providerName", "System.Data.SqlClient");

                    node.Attributes.Append(nameAttribute);
                    node.Attributes.Append(connectionStringAttribute);
                    node.Attributes.Append(providerNameAttribute);
                }
                else
                {
                    node = nodes[0];
                }

                node.Attributes["connectionString"].Value = connectionString;
                if (newNode)
                {
                    connectionStringElement.AppendChild(node);
                }

                document.Save(configurationFullPath);
            });
        }

        private static ProjectItem GetConfigurationFile(Project project)
        {
            foreach (ProjectItem projectItem in project.ProjectItems)
            {
                if (projectItem.Name.Equals("App.config", StringComparison.InvariantCultureIgnoreCase) ||
                    projectItem.Name.Equals("Web.config", StringComparison.InvariantCultureIgnoreCase))
                {
                    return projectItem;
                }
            }

            return null;
        }

        private static XmlAttribute CreateAttribute(XmlDocument document, string attributeName, string attributeValue)
        {
            var attribute = document.CreateAttribute(attributeName);
            attribute.Value = attributeValue;
            return attribute;
        }
    }
}
