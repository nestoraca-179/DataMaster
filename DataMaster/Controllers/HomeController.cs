using System.Web.Mvc;
using System.Data.SqlClient;
using DataMaster.Models;

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

			ViewBag.usuario = Session["USER"] as Usuario;
			ViewBag.message = message;

			return View();
		}

		[Authorize]
		public ActionResult SeleccionEmpresa()
		{
			if (!Request.IsAuthenticated)
				return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
			else if (Session["USER"] == null)
				return RedirectToAction("Logout", "Account", new { msg = "Debes iniciar sesión" });

			ViewBag.usuario = Session["USER"];
			ViewBag.connections = Connection.GetAllConns();

			return View();
		}

		[Authorize]
		[HttpPost]
		public ActionResult Redirect(int connect)
		{
			Empresa conn = Connection.GetConnByID(connect);
			string connectionString = string.Format("Server={0};Database={1};User Id={2};Password={3}", 
				conn.server, conn.database, conn.username, conn.password);
			SqlConnection connection = new SqlConnection(connectionString);

			Session["CONNECT"] = connectionString;
			Session["DB"] = connection.Database;
			Session["NAME_CONN"] = conn.des_con;
			Session["ID_CONN"] = conn.ID;

			bool useBranchs = new Branch().UseBranchs();

			if (useBranchs)
			{
				return RedirectToAction("SeleccionSucursal");
			}
			else
			{
				return RedirectToAction("Dashboard");
			}
		}

		[Authorize]
		public ActionResult SeleccionSucursal()
		{
			if (!Request.IsAuthenticated)
				return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
			else if (Session["USER"] == null)
				return RedirectToAction("Logout", "Account", new { msg = "Debes iniciar sesión" });

			ViewBag.usuario = Session["USER"];
			ViewBag.branchs = new Branch().GetAllBranchs();

			return View();
		}

		[Authorize]
		public ActionResult Dashboard(string sucur = "")
		{
			if (!Request.IsAuthenticated)
				return RedirectToAction("Index", new { message = "Debes iniciar sesión" });
			else if (Session["USER"] == null)
				return RedirectToAction("Logout", "Account", new { msg = "Debes iniciar sesión" });

			if (!string.IsNullOrEmpty(sucur) && Session["BRANCH"] == null)
				Session["BRANCH"] = new Branch().GetBranchByID(sucur);
			
			ViewBag.usuario = (Session["USER"] as Usuario);
			ViewBag.empresa = Session["NAME_CONN"];
			ViewBag.sucur = (Session["BRANCH"] as saSucursal)?.sucur_des;
			ViewBag.rate = new Price().GetRateUSD().ToString().Replace(",", ".");

			return View();
		}
	}
}