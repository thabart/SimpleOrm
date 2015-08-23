using ORM.Core;
using ORM.Mappings;

using SampleClient.Mappings;
using SampleClient.Models;

namespace SampleClient
{
    public class CustomDbContext : BaseDbContext
    {
        public CustomDbContext() : base("CustomConnectionString")
        {
        }

        public IDbSet<Customer> Customers { get; set; }

        protected override void Mappings(IEntityMappingContainer entityMappingContainer)
        {
            entityMappingContainer.AddMapping(new CustomerMapping());
        }
    }
}
