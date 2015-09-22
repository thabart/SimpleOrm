using System;

namespace Company.OrmLanguage
{
    public partial class Entry  
    {
        public Guid GetGuidValue()
        {
            return Guid.NewGuid();
        }
    }
}
