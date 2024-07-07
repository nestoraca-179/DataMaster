using System.Linq;
using System.Collections.Generic;

namespace DataMaster.Models
{
	public class Connection
	{
		private static readonly DataMasterEntities db = new DataMasterEntities();

		public static Empresa GetConnByID(int id)
		{
			return db.Empresa.AsNoTracking().Single(c => c.ID == id);
		}

		public static List<Empresa> GetAllConns()
		{
			return db.Empresa.AsNoTracking().ToList(); ;
		}
	}
}