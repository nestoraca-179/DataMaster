using DataMaster.Models;
using System.Web.Mvc;

namespace DataMaster.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index(string message = "")
		{
			if (Request.IsAuthenticated)
				return RedirectToAction("Dashboard");

			ViewBag.usuario = new Usuario() { des_usuario = "null" };
			ViewBag.message = message;
			return View();
		}

		public ActionResult CambiarClave(string message = "")
		{
			if (!Request.IsAuthenticated)
				return RedirectToAction("Index", new { message = "Debes iniciar sesión" });

			ViewBag.usuario = (Session["USER"] as Usuario);
			ViewBag.message = message;
			return View();
		}

		[Authorize]
		public ActionResult Dashboard()
		{
			if (!Request.IsAuthenticated)
				return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
			else if (Session["USER"] == null)
				return RedirectToAction("Logout", "Account", new { msg = "Debes iniciar sesión" });

			ViewBag.usuario = (Session["USER"] as Usuario);
			return View();
		}
	}
}