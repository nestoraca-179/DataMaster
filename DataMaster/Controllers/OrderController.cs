using DataMaster.Models;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace DataMaster.Controllers
{
    public class OrderController : Controller
    {
        public ActionResult Index()
        {
			if (!Request.IsAuthenticated)
				return RedirectToAction("Index", "Home", new { message = "Debes iniciar sesión" });
			else if (Session["USER"] == null)
				return RedirectToAction("Logout", "Account", new { msg = "Debes iniciar sesión" });

			JavaScriptSerializer serializer = new JavaScriptSerializer();
			serializer.MaxJsonLength = 50000000;

			ViewBag.usuario = (Session["USER"] as Usuario);
			ViewBag.orders = serializer.Serialize(new Order().GetAllOrders(30));
			ViewBag.arts = serializer.Serialize(new Product().GetAllArts());
			ViewBag.clients = new Client().GetAllClients(false);
			ViewBag.conds = new Cond().GetAllConds();
			ViewBag.sellers = new Seller().GetAllSellers();
			ViewBag.currencies = new Currency().GetAllCurrencies();
			ViewBag.transports = new Transport().GetAllTransports();
			ViewBag.alms = new Storage().GetAllAlms();
			ViewBag.obj_client = serializer.Serialize(ViewBag.clients);

			return View();
        }
    }
}