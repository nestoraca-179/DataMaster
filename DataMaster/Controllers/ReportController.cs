using DataMaster.Models;
using System.Web.Mvc;

namespace DataMaster.Controllers
{
	public class ReportController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RepStockArticulos()
		{
            if (!Request.IsAuthenticated)
                return RedirectToAction("Index", "Home", new { message = "Debes iniciar sesión" });
            else if (Session["USER"] == null)
                return RedirectToAction("Logout", "Account", new { msg = "Debes iniciar sesión" });

            saSucursal s = (Session["BRANCH"] as saSucursal);
            ViewBag.usuario = (Session["USER"] as Usuario);
            ViewBag.empresa = Session["NAME_CONN"];
            ViewBag.sucur = s?.sucur_des;

            return View();
		}
    }
}