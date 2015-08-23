using ORM.Mappings;
using SampleClient.Models;

namespace SampleClient.Mappings
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
