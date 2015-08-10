using ORM.Core;
using ORM.Models;

namespace ORM
{
    public class CustomDbContext : BaseDBContext
    {
        public CustomDbContext() : base("CustomConnectionString")
        {
        }

        public DbSet<Customer> Customers { get; set; }
    }
}
