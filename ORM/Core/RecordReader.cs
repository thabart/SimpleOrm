using ORM.Mappings;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace ORM.Core
{
    public class RecordReader : IRecordReader
    {

        /// <summary>
        /// Map the record values to the entity.
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="entityMappingDefinition"></param>
        /// <param name="columnNames"></param>
        /// <returns></returns>
        public object MapToEntity(SqlDataReader reader, 
            EntityMappingDefinition entityMappingDefinition,
            List<string> columnNames)
        {
            if (columnNames == null)
            {
                // TODO : Throw the appropriate exception.
            }

            var instance = Activator.CreateInstance(entityMappingDefinition.EntityType);
            for (var i = 0; i < columnNames.Count; i++)
            {
                var columnName = columnNames[i];
                var columnDefinition = entityMappingDefinition.ColumnDefinitions.FirstOrDefault(c => c.ColumnName == columnName);
                if (columnDefinition == null)
                {
                    continue;
                }

                FillInTheInstance(instance, columnName, columnDefinition, reader, i);
            }

            return instance;
        }

        private static void FillInTheInstance(
            object instance,
            string columnName,
            ColumnDefinition columnDefinition,
            SqlDataReader dataReader,
            int index)
        {
            var getPropertyInfoType = GetPropertyInfoType(instance, columnDefinition.PropertyName);
            if (columnDefinition == null)
            {
                return;
            }

            if (getPropertyInfoType == typeof(int))
            {
                var recordValue = dataReader.GetInt32(index);
                SetPropertyValue(instance, columnDefinition.PropertyName, recordValue);
            }
            else if (getPropertyInfoType == typeof(double))
            {
                var recordValue = dataReader.GetDouble(index);
                SetPropertyValue(instance, columnDefinition.PropertyName, recordValue);
            }
            else if (getPropertyInfoType == typeof(Guid))
            {
                var recordValue = dataReader.GetGuid(index);
                SetPropertyValue(instance, columnDefinition.PropertyName, recordValue);
            }
            else
            {
                var recordValue = dataReader.GetString(index);
                SetPropertyValue(instance, columnDefinition.PropertyName, recordValue);
            }
        }

        /// <summary>
        /// Get the type of the property info.
        /// </summary>
        /// <param name="instance">Current instance</param>
        /// <param name="propertyName">Name of the property</param>
        /// <returns></returns>
        private static Type GetPropertyInfoType(object instance, string propertyName)
        {
            var type = instance.GetType();
            var propertyInfo = type.GetProperty(propertyName,
                BindingFlags.Instance | BindingFlags.Public);
            if (propertyInfo == null)
            {
                // TODO : throw the appropriate exception.
            }

            return propertyInfo.PropertyType;
        }

        /// <summary>
        /// Set the property value.
        /// </summary>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="instance"></param>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        private static void SetPropertyValue<TProperty>(object instance, string propertyName, TProperty propertyValue)
        {
            var type = instance.GetType();
            var propertyInfo = type.GetProperty(propertyName,
                BindingFlags.Instance | BindingFlags.Public);
            if (propertyInfo != null && propertyInfo.CanWrite)
            {
                propertyInfo.SetValue(instance, propertyValue);
            }
        }

    }
}
