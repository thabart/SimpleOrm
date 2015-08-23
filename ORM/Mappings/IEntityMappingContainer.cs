using System;

namespace ORM.Mappings
{
    public interface IEntityMappingContainer
    {
        void AddMapping<TSource>(BaseMapping<TSource> mapping) where TSource : class;

        EntityMappingDefinition GetEntityMappingDefinition(Type type);
    }
}
