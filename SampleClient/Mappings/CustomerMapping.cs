using ORM.Mappings;
using SampleClient.Models;

namespace SampleClient.Mappings
{
    public class CustomerMapping : BaseMapping<Customer>
    {
        public CustomerMapping()
        {
            MapClassToTable("dbo.CUSTOMERS");
            MapProperty(t => t.Id).ToColumn("ID");
            MapProperty(t => t.FirstName).ToColumn("FIRST_NAME");
            MapProperty(t => t.LastName).ToColumn("LAST_NAME");
        }
    }
}
