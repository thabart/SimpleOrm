using ORM.Mappings;
using SampleClient.Models;

namespace SampleClient.Mappings
{
    public class CustomerMapping : BaseMapping<Customer>
    {
        public CustomerMapping()
        {
            ToTable("dbo.Customers");
            Property(t => t.Id).HasColumnName("ID");
            Property(t => t.FirstName).HasColumnName("FIRST_NAME");
            Property(t => t.LastName).HasColumnName("LAST_NAME");
        }
    }
}
