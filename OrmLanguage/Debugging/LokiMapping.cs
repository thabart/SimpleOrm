using ORM.Mappings;
using Debugging.Models;

namespace Debugging.Mappings {
	public class LokiMapping : BaseMapping<Loki> {
		public LokiMapping() {
			MapClassToTable("dbo.LokiTable");
			MapProperty(t => Name, "NameColumn");
		}
	}
}

