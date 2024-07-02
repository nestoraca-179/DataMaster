using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataMaster.Models
{
	public class MyUser
	{
		public static List<Usuario> GetAllUsers()
		{
			DataMasterEntities db = new DataMasterEntities();
			return db.Usuario.AsNoTracking().ToList(); ;
		}
	}
}