
 using ORM.Core;
 using ORM.Mappings;

 using Debugging.Mappings;
 using Debugging.Models;

 namespace Debugging 
 {
	public class DbContext : BaseDbContext 
	{
		public DbContext() : base("CustomConnectionString")
		{
		}

		public IDbSet<Deeeevvit> Deeeevvits { get; set; }
		public IDbSet<Loki> Lokis { get; set; }

		public override void Mappings(IEntityMappingContainer entityMappingContainer)
		{
			entityMappingContainer.AddMapping(new DeeeevvitMapping());
			entityMappingContainer.AddMapping(new LokiMapping());
		}
	}
 }