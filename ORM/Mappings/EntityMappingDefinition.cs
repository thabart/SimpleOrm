using System.Collections.Generic;

namespace ORM.Mappings
{
    public class EntityMappingDefinition
    {
        private List<ColumnDefinition> _columnDefinitions;

        public EntityMappingDefinition()
        {
            _columnDefinitions = new List<ColumnDefinition>();
        }

        public string TableName { get; set; }

        public List<ColumnDefinition> ColumnDefinitions
        {
            get
            {
                return _columnDefinitions;
            }
        }
    }
}
