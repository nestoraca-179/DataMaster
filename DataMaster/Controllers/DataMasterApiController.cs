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
			Usuario u = (HttpContext.Current.Session["USER"] as Usuario);

			try
			{
				saPedidoVenta new_order = new Order().AddOrder(order, u.username);
				LogController.CreateLog(u.username, "PEDIDO", u.ID.ToString(), "I", null);

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

		[HttpPost]
		[Route("api/DataMasterApi/EditOrder/")]
		public DataMasterResponse EditOrder(saPedidoVenta order)
		{
			DataMasterResponse response = new DataMasterResponse();
			Usuario u = (HttpContext.Current.Session["USER"] as Usuario);

			try
			{
				saPedidoVenta edit_order = new Order().EditOrder(order, u.username);
				LogController.CreateLog(u.username, "PEDIDO", u.ID.ToString(), "M", edit_order.comentario);

				response.Status = "OK";
				response.Result = edit_order;
			}
			catch (Exception ex)
			{
				response.Status = "ERROR";
				response.Message = ex.Message;
				IncidentController.CreateIncident("ERROR MODIFICANDO PEDIDO " + order.doc_num, ex);
			}

			return response;
		}

		[HttpGet]
		[Route("api/DataMasterApi/DeleteOrder/{id}")]
		public DataMasterResponse DeleteOrder(string id)
		{
			DataMasterResponse response = new DataMasterResponse();
			Usuario u = (HttpContext.Current.Session["USER"] as Usuario);

			try
			{
				saPedidoVenta del_order = new Order().DeleteOrder(id, u.username);

				response.Status = "OK";
				response.Result = del_order.doc_num.Trim();
			}
			catch (Exception ex)
			{
				response.Status = "ERROR";
				response.Message = ex.Message;
				IncidentController.CreateIncident("ERROR ELIMINANDO PEDIDO " + id.ToString(), ex);
			}

			return response;
		}
	}
}