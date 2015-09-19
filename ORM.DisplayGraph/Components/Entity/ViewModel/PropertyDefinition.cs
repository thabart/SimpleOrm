namespace ORM.DisplayGraph.Components.Entity.ViewModel
{
    public class PropertyDefinition
    {
        public string Name { get; set; }

        public string Type { get; set; }

        public bool IsPrimaryKey { get; set; }

        public bool IsReferenceKey { get; set; }
    }
}
