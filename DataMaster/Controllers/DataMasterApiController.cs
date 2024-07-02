using System;
using System.Web;
using System.Web.Http;
using DataMaster.Models;

namespace DataMaster.Controllers
{
    public class DataMasterApiController : ApiController
    {
		[HttpGet]
		[Route("api/DataMasterApi/GetStock/{art}/{alm}")]
		public DataMasterResponse GetStock(string art, string alm)
		{
			DataMasterResponse response = new DataMasterResponse();

			try
			{
				decimal result = Order.GetStock(art, alm);

				response.Status = "OK";
				response.Result = result;
			}
			catch (Exception ex)
			{
				response.Status = "ERROR";
				response.Message = ex.Message;
			}
			
			return response;
		}

		[HttpPost]
		[Route("api/DataMasterApi/AddOrder/")]
		public DataMasterResponse Addorder(saPedidoVenta order)
		{
			DataMasterResponse response = new DataMasterResponse();
			string user = (HttpContext.Current.Session["USER"] as Usuario).username;

			try
			{
				saPedidoVenta new_order = new Order().AddOrder(order, user);

				response.Status = "OK";
				response.Result = new_order;
			}
			catch (Exception ex)
			{
				response.Status = "ERROR";
				response.Message = ex.Message;
				IncidentController.CreateIncident("ERROR AGREGANDO PEDIDO", ex);
			}

			return response;
		}
	}
}