using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using DataMaster.Models;

namespace DataMaster.Controllers
{
    public class DataMasterApiController : ApiController
    {
		// UTILS
		private readonly UtilsController utils = new UtilsController();

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
			string s = (HttpContext.Current.Session["BRANCH"] as saSucursal)?.co_sucur;

			try
			{
				saPedidoVenta new_order = new Order().AddOrder(order, u.username, s);
				LogController.CreateLog(u.username, "PEDIDO", new_order.doc_num, "I", null);

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
			string s = (HttpContext.Current.Session["BRANCH"] as saSucursal)?.co_sucur;

			try
			{
				saPedidoVenta edit_order = new Order().EditOrder(order, u.username, s);
				LogController.CreateLog(u.username, "PEDIDO", edit_order.doc_num, "M", edit_order.comentario);

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
			string s = (HttpContext.Current.Session["BRANCH"] as saSucursal)?.co_sucur;

			try
			{
				saPedidoVenta del_order = new Order().DeleteOrder(id, u.username, s);
				LogController.CreateLog(u.username, "PEDIDO", del_order.doc_num, "E", null);

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

		[HttpPost]
		[Route("api/DataMasterApi/AddUser/")]
		public DataMasterResponse AddUser(Usuario user)
		{
			DataMasterResponse response = new DataMasterResponse();
			Usuario u = (HttpContext.Current.Session["USER"] as Usuario);

			try
			{
				Usuario new_user = MyUser.Add(user, u.username);
				LogController.CreateLog(u.username, "USUARIO", new_user.ID.ToString(), "I", null);

				response.Status = "OK";
				response.Result = new_user.ID;
			}
			catch (Exception ex)
			{
				response.Status = "ERROR";
				response.Message = ex.Message;
				IncidentController.CreateIncident("ERROR AGREGANDO USUARIO " + user.username, ex);
			}

			return response;
		}

		[HttpPost]
		[Route("api/DataMasterApi/EditUser/")]
		public DataMasterResponse EditUser(Usuario user)
		{
			DataMasterResponse response = new DataMasterResponse();
			Usuario u = (HttpContext.Current.Session["USER"] as Usuario);

			try
			{
				Usuario edit_user = MyUser.Edit(user, u.username);
				LogController.CreateLog(u.username, "USUARIO", edit_user.ID.ToString(), "E", edit_user.password);

				response.Status = "OK";
				response.Result = edit_user.ID;
			}
			catch (Exception ex)
			{
				response.Status = "ERROR";
				response.Message = ex.Message;
				IncidentController.CreateIncident("ERROR EDITANDO USUARIO " + user.username, ex);
			}

			return response;
		}

		[HttpGet]
		[Route("api/DataMasterApi/DeleteUser/{id}/")]
		public DataMasterResponse DeleteUser(int id)
		{
			DataMasterResponse response = new DataMasterResponse();
			Usuario u = (HttpContext.Current.Session["USER"] as Usuario);

			try
			{
				Usuario del_user = MyUser.Delete(id);
				LogController.CreateLog(u.username, "USUARIO", id.ToString(), "E", null);

				response.Status = "OK";
				response.Result = del_user.ID;
			}
			catch (Exception ex)
			{
				response.Status = "ERROR";
				response.Message = ex.Message;
				IncidentController.CreateIncident("ERROR ELIMINANDO USUARIO " + id.ToString(), ex);
			}

			return response;
		}

		// ESTADISTICAS DASHBOARD ADMIN

		[HttpGet]
		[Route("api/DataMasterApi/GetStatsInvoices/{fec_d}/{fec_h}")]
		public DataMasterResponse GetStatsInvoices(string fec_d, string fec_h)
		{
			DataMasterResponse response = new DataMasterResponse();

			try
			{
				DateTime fecha_d = utils.FormatDate(fec_d);
				DateTime fecha_h = utils.FormatDate(fec_h);
				string sucur = HttpContext.Current.Session["BRANCH"]?.ToString();

				object stats = new Invoice().GetStatsInvoicesWithNotes(fecha_d, fecha_h, sucur);

				response.Status = "OK";
				response.Result = stats;
			}
			catch (Exception ex)
			{
				response.Status = "ERROR";
				response.Message = ex.Message;
				IncidentController.CreateIncident("ERROR OBTENIENDO ESTADISTICAS DE VENTAS", ex);
			}

			return response;
		}

		[HttpGet]
		[Route("api/DataMasterApi/GetMostSaleProducts/{fec_d}/{fec_h}/{number}/{suc}")]
		public DataMasterResponse GetMostSaleProducts(string fec_d, string fec_h, int number, int suc)
		{
			DataMasterResponse response = new DataMasterResponse();

			try
			{
				DateTime fecha_d = utils.FormatDate(fec_d);
				DateTime fecha_h = utils.FormatDate(fec_h);
				string sucur = HttpContext.Current.Session["BRANCH"]?.ToString();

				List<saArticulo> arts = new Product().GetMostProducts(fecha_d, fecha_h, number, true, suc == 1 ? sucur : null);

				response.Status = "OK";
				response.Result = arts;
			}
			catch (Exception ex)
			{
				response.Status = "ERROR";
				response.Message = ex.Message;
				IncidentController.CreateIncident("ERROR OBTENIENDO PRODUCTOS MAS VENDIDOS", ex);
			}

			return response;
		}

		[HttpGet]
		[Route("api/DataMasterApi/GetMostPurchaseProducts/{fec_d}/{fec_h}/{number}/{suc}")]
		public DataMasterResponse GetMostPurchaseProducts(string fec_d, string fec_h, int number, int suc)
		{
			DataMasterResponse response = new DataMasterResponse();

			try
			{
				DateTime fecha_d = utils.FormatDate(fec_d);
				DateTime fecha_h = utils.FormatDate(fec_h);
				string sucur = HttpContext.Current.Session["BRANCH"]?.ToString();

				List<saArticulo> arts = new Product().GetMostProductsWithNotes(fecha_d, fecha_h, number, false, suc == 1 ? sucur : null);

				response.Status = "OK";
				response.Result = arts;
			}
			catch (Exception ex)
			{
				response.Status = "ERROR";
				response.Message = ex.Message;
				IncidentController.CreateIncident("ERROR OBTENIENDO PRODUCTOS MAS COMPRADOS", ex);
			}

			return response;
		}

		[HttpGet]
		[Route("api/DataMasterApi/GetMostActiveClients/{fec_d}/{fec_h}/{number}/{suc}")]
		public DataMasterResponse GetMostActiveClients(string fec_d, string fec_h, int number, int suc)
		{
			DataMasterResponse response = new DataMasterResponse();

			try
			{
				DateTime fecha_d = utils.FormatDate(fec_d);
				DateTime fecha_h = utils.FormatDate(fec_h);
				string sucur = HttpContext.Current.Session["BRANCH"]?.ToString();

				List<saCliente> clients = new Client().GetMostActiveClients(fecha_d, fecha_h, number, suc == 1 ? sucur : null);

				response.Status = "OK";
				response.Result = clients;
			}
			catch (Exception ex)
			{
				response.Status = "ERROR";
				response.Message = ex.Message;
				IncidentController.CreateIncident("ERROR OBTENIENDO CLIENTES MAS ACTIVOS", ex);
			}

			return response;
		}

		[HttpGet]
		[Route("api/DataMasterApi/GetMostActiveSuppliers/{fec_d}/{fec_h}/{number}/{suc}")]
		public DataMasterResponse GetMostActiveSuppliers(string fec_d, string fec_h, int number, int suc)
		{
			DataMasterResponse response = new DataMasterResponse();

			try
			{
				DateTime fecha_d = utils.FormatDate(fec_d);
				DateTime fecha_h = utils.FormatDate(fec_h);
				string sucur = HttpContext.Current.Session["BRANCH"]?.ToString();

				// List<saProveedor> suppliers = new Supplier().GetMostActiveSuppliers(fecha_d, fecha_h, number, suc == 1 ? sucur : null);
				List<saCliente> clientes = new Client().GetMostActiveClientsWithNotes(fecha_d, fecha_h, number, suc == 1 ? sucur : null);

				response.Status = "OK";
				response.Result = clientes;
			}
			catch (Exception ex)
			{
				response.Status = "ERROR";
				response.Message = ex.Message;
				IncidentController.CreateIncident("ERROR OBTENIENDO PROVEEDORES MAS ACTIVOS", ex);
			}

			return response;
		}
	}
}