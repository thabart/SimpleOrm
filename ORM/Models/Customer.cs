using System;

using ORM.Core;

namespace ORM.Models
{
    public sealed class Customer : BaseEntity
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public override void Mappings()
        {
            AddColumnMapping("FirstName", () => FirstName);
            AddColumnMapping("LastName", () => LastName);
        } 
    }
}
