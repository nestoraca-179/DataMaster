using DataMaster.Models;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace DataMaster.Controllers
{
	[Authorize]
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

			saSucursal s = (Session["BRANCH"] as saSucursal);
			ViewBag.usuario = (Session["USER"] as Usuario);
			ViewBag.empresa = Session["NAME_CONN"];
			ViewBag.sucur = s?.sucur_des;
			ViewBag.orders = serializer.Serialize(new Order().GetAllOrders(30, s?.co_sucur));

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

			if (Session["PRICES"] == null)
				Session["PRICES"] = new Price().GetAllPrices();

			if (Session["IMPS"] == null)
				Session["IMPS"] = new Price().GetAllImps();

			ViewBag.arts = Session["ARTS"];
			ViewBag.clients = Session["CLIENTS"];
			ViewBag.conds = Session["CONDS"];
			ViewBag.sellers = Session["SELLERS"];
			ViewBag.currencies = Session["CURRENCIES"];
			ViewBag.transports = Session["TRANSPORTS"];
			ViewBag.alms = Session["ALMS"];
			ViewBag.prices = serializer.Serialize(Session["PRICES"]);
			ViewBag.imps = Session["IMPS"];
			ViewBag.rate = new Price().GetRateUSD().ToString().Replace(",", ".");
			ViewBag.obj_client = serializer.Serialize(ViewBag.clients);

			return View();
        }
    }
}