using System;
using System.Collections.Generic;

namespace ORM.Helpers
{
    public static class GenerateSqlScriptHelper
    {
        private static readonly List<Type> _listOfTypesWithQuotes = new List<Type>()
        {
            typeof(string),
            typeof(Guid),
            typeof(char)
        };

        public static string ConvertConstantIntoSqlScript(Type type, object value)
        {
            var format = "{0}";
            if (_listOfTypesWithQuotes.Contains(type))
            {
                format = "'{0}'";
            }

            return string.Format(format, value);
        }
    }
}
