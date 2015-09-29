using ORM.Mappings;
using Debugging.Models;

namespace Debugging.Mappings {
	public class DeeeevvitMapping : BaseMapping<Deeeevvit> {
		public DeeeevvitMapping() {
			MapClassToTable("dbo.DevitTable");
			MapProperty(t => Money, "MoneyColumn");
		}
	}
}

