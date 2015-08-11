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
            // Possible here to define mapping rules between table & customer.
        } 
    }
}
