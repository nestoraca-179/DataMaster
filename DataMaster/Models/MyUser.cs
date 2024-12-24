using System.Linq;
using System.Collections.Generic;
using DataMaster.Controllers;
using System;
using System.Data.Entity;
using System.Reflection;

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

		public static Usuario Add(Usuario user, string u)
		{
			Usuario new_user;

			user.password = SecurityController.Encrypt(user.password);
			user.fec_camb = DateTime.Now.AddMinutes(-1);
			user.co_us_in = u;
			user.fe_us_in = DateTime.Now;
			user.co_us_mo = u;
			user.fe_us_mo = DateTime.Now;

			new_user = db.Usuario.Add(user);
			db.SaveChanges();

			return new_user;
		}

		public static Usuario Edit(Usuario user, string u)
		{
			Usuario old_user = GetUserByID(user.ID);

			using (DataMasterEntities context = new DataMasterEntities())
			{
				user.username = old_user.username;
				user.password = old_user.password;
				user.fec_camb = old_user.fec_camb;
				user.co_us_mo = u;
				user.fe_us_mo = DateTime.Now;
				context.Entry(user).State = EntityState.Modified;
				context.SaveChanges();
			}

			user.password = GetChanges(old_user, user);
			return user;
		}

		public static Usuario Delete(int id)
		{
			Usuario del_user = GetUserByID(id);

			db.Usuario.Attach(del_user);
			db.Usuario.Remove(del_user);
			db.SaveChanges();

			return del_user;
		}

		private static string GetChanges(Usuario user_v, Usuario user_n)
		{
			string campos = "";
			Type type = new Usuario().GetType();

			foreach (PropertyInfo prop in type.GetProperties())
			{
				if (prop.Name != "fe_us_in" && prop.Name != "fe_us_mo")
				{
					string valor1 = prop.GetValue(user_v) == null ? "" : prop.GetValue(user_v).ToString().Trim();
					string valor2 = prop.GetValue(user_n) == null ? "" : prop.GetValue(user_n).ToString().Trim();

					if (valor1 != valor2)
					{
						campos += string.Format("{0}: {1} -> {2}; ", prop.Name, valor1, valor2);
					}
				}
			}

			return campos;
		}
	}
}