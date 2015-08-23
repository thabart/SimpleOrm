using ORM.Models;

namespace ORM.Mappings
{
    public class CustomerMapping : BaseMapping<Customer>
    {
        public CustomerMapping()
        {
            ToTable("dbo.Customers");
            Property(t => t.FirstName).HasColumnName("FIRST_NAME");
        }
    }
}
