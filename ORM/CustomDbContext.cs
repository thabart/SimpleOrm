using ORM.Core;
using ORM.Models;

namespace ORM
{
    public class CustomDbContext : BaseDBContext
    {
        public CustomDbContext() : base(@"Server = DESKTOP - 1CNU397\SQLEXPRESS; Database = customer; Integrated Security = True;")
        {
        }

        public DbSet<Customer> Customers { get; set; }
    }
}
