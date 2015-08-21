using ORM.Core;
using ORM.Models;

namespace ORM
{
    public class CustomDbContext : BaseDbContext
    {
        public CustomDbContext() : base("CustomConnectionString")
        {
        }

        public IDbSet<Customer> Customers { get; set; }

        protected override void Mappings()
        {

        }
    }
}
