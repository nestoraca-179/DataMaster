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

			if (Session["ARTS"] == null)
				Session["ARTS"] = serializer.Serialize(new Product().GetAllArts());

			if (Session["CLIENTS"] == null)
				Session["CLIENTS"] = new Client().GetAllClients();

			if (Session["CONDS"] == null)
				Session["CONDS"] = new Cond().GetAllConds();

			if (Session["SELLERS"] == null)
				Session["SELLERS"] = new Seller().GetAllSellers();

			if (Session["CURRENCIES"] == null)
				Session["CURRENCIES"] = new Currency().GetAllCurrencies();

			if (Session["TRANSPORTS"] == null)
				Session["TRANSPORTS"] = new Transport().GetAllTransports();

			if (Session["ALMS"] == null)
				Session["ALMS"] = new Storage().GetAllAlms();

			ViewBag.arts = Session["ARTS"];
			ViewBag.clients = Session["CLIENTS"];
			ViewBag.conds = Session["CONDS"];
			ViewBag.sellers = Session["SELLERS"];
			ViewBag.currencies = Session["CURRENCIES"];
			ViewBag.transports = Session["TRANSPORTS"];
			ViewBag.alms = Session["ALMS"];
			ViewBag.obj_client = serializer.Serialize(ViewBag.clients);

			return View();
        }
    }
}