using System;
using System.Collections.Generic;

namespace ORM.Mappings
{
    public class EntityMappingContainer : IEntityMappingContainer
    {
        private readonly Dictionary<Type, EntityMappingDefinition> _mappings;

        public EntityMappingContainer()
        {
            _mappings = new Dictionary<Type, EntityMappingDefinition>();
        }

        public void AddMapping<TSource>(BaseMapping<TSource> mapping) where TSource : class
        {
            var type = typeof(TSource);
            var entityMappingDefinition = mapping.EntityMappingDefinition;
            _mappings.Add(type, entityMappingDefinition);
        }

        public EntityMappingDefinition GetEntityMappingDefinition(Type type)
        {
            if (!_mappings.ContainsKey(type))
            {
                // TODO : returns the correct exception
            }

            return _mappings[type];
        }
    }
}
