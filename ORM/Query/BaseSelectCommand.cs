using System.Collections.Generic;

namespace ORM.Query
{
    public class BaseSelectCommand
    {
        public List<string> Columns { get; set; }

        public WhereCommand WhereCommands { get; set; }
    }
}
