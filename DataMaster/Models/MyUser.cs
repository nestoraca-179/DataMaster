using System.Linq;
using System.Collections.Generic;

namespace DataMaster.Models
{
	public class MyUser
	{
		private static readonly DataMasterEntities db = new DataMasterEntities();

		public static Usuario GetUserByID(int id)
		{
			return db.Usuario.AsNoTracking().Single(u => u.ID == id);
		}

		public static List<Usuario> GetAllUsers()
		{
			return db.Usuario.AsNoTracking().ToList(); ;
		}
	}
}