using DataMaster.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace DataMaster.Controllers
{
	public class AccountController : Controller
    {
		[HttpPost]
		public ActionResult Login(string username, string password)
		{
			if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
			{
				string encryptedPass = SecurityController.Encrypt(password);
				// string decryptedText = SecurityController.Decrypt("IDaB8UHxBROqKEbn9PQvkA==");

				using (DataMasterEntities db = new DataMasterEntities())
				{
					Usuario user = db.Usuario.AsNoTracking().SingleOrDefault(u => u.username == username && u.password == encryptedPass);

					if (user == null)
					{
						return RedirectToAction("Index", "Home", new { message = "Usuario o clave incorrectos" });
					}
					else
					{
						if (!user.activo)
							return RedirectToAction("Index", "Home", new { message = "Usuario inactivo" });

						FormsAuthentication.SetAuthCookie(username, false);
						Session["USER"] = user;

						if (DateTime.Compare(user.fec_camb, DateTime.Now) < 0)
							return RedirectToAction("CambiarClave", "Home");

						LogController.CreateLog(user.username, "LOGIN", user.ID.ToString(), "L", null);
						return RedirectToAction("SeleccionEmpresa", "Home");
					}
				}
			}
			else
			{
				return RedirectToAction("Index", "Home", new { message = "Debes ingresar usuario y clave" });
			}
		}

		[HttpPost]
		public ActionResult ChangePassword(string username, string old_pass, string new_pass)
		{
			if (!string.IsNullOrEmpty(old_pass) && !string.IsNullOrEmpty(new_pass))
			{
				string encryptedOldPass = SecurityController.Encrypt(old_pass);
				string encryptedNewPass = SecurityController.Encrypt(new_pass);

				using (DataMasterEntities db = new DataMasterEntities())
				{
					Usuario user = db.Usuario.AsNoTracking().SingleOrDefault(u => u.username == username && u.password == encryptedOldPass);

					if (user == null)
					{
						return RedirectToAction("CambiarClave", "Home", new { message = "Usuario o clave incorrectos" });
					}
					else
					{
						if (old_pass.Trim() == new_pass.Trim())
							return RedirectToAction("CambiarClave", "Home", new { message = "Debes ingresar una clave diferente a la anterior" });

						user.password = encryptedNewPass;
						user.fec_camb = DateTime.Now.AddMonths(3);
						db.Entry(user).State = EntityState.Modified;
						db.SaveChanges();

						LogController.CreateLog(user.username, "LOGIN", user.ID.ToString(), "C", null);
						return RedirectToAction("SeleccionEmpresa", "Home");
					}
				}
			}
			else
			{
				return RedirectToAction("CambiarClave", "Home", new { message = "Debes ingresar la clave anterior y la clave nueva" });
			}
		}

		public ActionResult Logout(string msg = "")
		{
			FormsAuthentication.SignOut();
			Session.Clear();
			Session.Abandon();
			Session.RemoveAll();

			if (Request.Cookies["ASP.NET_SessionId"] != null)
			{
				Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddDays(-1);
				Response.Cookies["ASP.NET_SessionId"].Value = string.Empty;
			}

			if (msg != "")
				return RedirectToAction("Index", "Home", new { message = msg });

			return RedirectToAction("Index", "Home");
		}
	}
}