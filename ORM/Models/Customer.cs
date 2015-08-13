using System;

using ORM.Core;

namespace ORM.Models
{
    public sealed class Customer : BaseEntity<Customer>
    {
        public Guid Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public override void Mappings()
        {
            LinkToTable("dbo.Customers");
            AddColumnMapping("Id", x => x.Id);
            AddColumnMapping("FirstName", x => x.FirstName);
            AddColumnMapping("LastName", x => x.LastName);
        } 
    }
}
