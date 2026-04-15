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

		// PEDIDO VENTA

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

		// NOTA ENTREGA

		[HttpGet]
		[Route("api/DataMasterApi/GetSellNote/{id}")]
		public DataMasterResponse GetSellNote(string id)
		{
			DataMasterResponse response = new DataMasterResponse();

			try
			{
				saNotaEntregaVenta order = new Note().GetSellNoteByID(id);

				if (order == null)
				{
					response.Status = "ERROR";
					response.Message = $"No se ha encontrado la Nota de Entrega Nro. {id}";
				}
				else if (order.campo8 == "OK") // USED FOR VERIFICATION
				{
					response.Status = "ERROR";
					response.Message = $"La Nota de Entrega Nro. {id} ya ha sido verificada";
				}
				else
				{
					response.Status = "OK";
					response.Result = order;
					response.Message = null;
				}
			}
			catch (Exception ex)
			{
				response.Status = "ERROR";
				response.Message = ex.Message;
				IncidentController.CreateIncident("ERROR BUSCANDO NOTA ENTREGA " + id, ex);
			}

			return response;
		}

		[HttpGet]
		[Route("api/DataMasterApi/MarkNoteAsVerified/{id}")]
		public DataMasterResponse MarkNoteAsVerified(string id)
		{
			DataMasterResponse response = new DataMasterResponse();

			try
			{
				new Note().MarkAsVerified(id);

				response.Status = "OK";
				response.Result = id;
			}
			catch (Exception ex)
			{
				response.Status = "ERROR";
				response.Message = ex.Message;
				IncidentController.CreateIncident("ERROR MARCANDO NOTA DE ENTREGA " + id + " COMO VERIFICADA", ex);
			}

			return response;
		}

		// FACTURA VENTA

		[HttpGet]
		[Route("api/DataMasterApi/GetSellInvoice/{id}")]
		public DataMasterResponse GetSellInvoice(string id)
		{
			DataMasterResponse response = new DataMasterResponse();

			try
			{
				saFacturaVenta invoice = new Invoice().GetSellInvoiceByID(id);

				if (invoice == null)
				{
					response.Status = "ERROR";
					response.Message = $"No se ha encontrado la Factura de Venta Nro. {id}";
				}
				else if (invoice.campo8 == "OK") // USED FOR VERIFICATION 
				{
					response.Status = "ERROR";
					response.Message = $"La Factura de Venta Nro. {id} ya ha sido verificada";
				}
				else
				{
					response.Status = "OK";
					response.Result = invoice;
					response.Message = null;
				}
			}
			catch (Exception ex)
			{
				response.Status = "ERROR";
				response.Message = ex.Message;
				IncidentController.CreateIncident("ERROR BUSCANDO FACTURA VENTA " + id, ex);
			}

			return response;
		}

		[HttpGet]
		[Route("api/DataMasterApi/MarkInvoiceAsVerified/{id}")]
		public DataMasterResponse MarkInvoiceAsVerified(string id)
		{
			DataMasterResponse response = new DataMasterResponse();

			try
			{
				new Invoice().MarkAsVerified(id);

				response.Status = "OK";
				response.Result = id;
			}
			catch (Exception ex)
			{
				response.Status = "ERROR";
				response.Message = ex.Message;
				IncidentController.CreateIncident("ERROR MARCANDO FACTURA DE VENTA " + id + " COMO VERIFICADA", ex);
			}

			return response;
		}

		// USUARIO

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

		[HttpGet]
		[Route("api/DataMasterApi/ResetPass/{id}")]
		public DataMasterResponse ResetPass(int id)
		{
			DataMasterResponse response = new DataMasterResponse();

			try
			{
				MyUser.ResetPass(id);

				response.Status = "OK";
				response.Result = id;
			}
			catch (Exception ex)
			{
				response.Status = "ERROR";
				response.Message = ex.Message;
				IncidentController.CreateIncident("ERROR RESETANDO CLAVE DE USUARIO " + id.ToString(), ex);
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

				object stats = new Invoice().GetStatsInvoicesWithOrders(fecha_d, fecha_h, sucur);

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

		[HttpGet]
		[Route("api/DataMasterApi/GetMostActiveSellers/{fec_d}/{fec_h}/{number}/{suc}")]
		public DataMasterResponse GetMostActiveSellers(string fec_d, string fec_h, int number, int suc)
		{
			DataMasterResponse response = new DataMasterResponse();

			try
			{
				DateTime fecha_d = utils.FormatDate(fec_d);
				DateTime fecha_h = utils.FormatDate(fec_h);
				string sucur = HttpContext.Current.Session["BRANCH"]?.ToString();

				List<saVendedor> sellers = new Seller().GetMostActiveSellers(fecha_d, fecha_h, number, suc == 1 ? sucur : null);

				response.Status = "OK";
				response.Result = sellers;
			}
			catch (Exception ex)
			{
				response.Status = "ERROR";
				response.Message = ex.Message;
				IncidentController.CreateIncident("ERROR OBTENIENDO VENDEDORES MAS ACTIVOS", ex);
			}

			return response;
		}

		[HttpGet]
		[Route("api/DataMasterApi/GetMostSubLines/{fec_d}/{fec_h}/{number}/{suc}")]
		public DataMasterResponse GetMostSubLines(string fec_d, string fec_h, int number, int suc)
		{
			DataMasterResponse response = new DataMasterResponse();

			try
			{
				DateTime fecha_d = utils.FormatDate(fec_d);
				DateTime fecha_h = utils.FormatDate(fec_h);
				string sucur = HttpContext.Current.Session["BRANCH"]?.ToString();

				List<saSubLinea> sublines = new Product().GetMostSubLines(fecha_d, fecha_h, number, suc == 1 ? sucur : null);

				response.Status = "OK";
				response.Result = sublines;
			}
			catch (Exception ex)
			{
				response.Status = "ERROR";
				response.Message = ex.Message;
				IncidentController.CreateIncident("ERROR OBTENIENDO SUBLINEAS MAS VENDIDAS", ex);
			}

			return response;
		}

		[HttpGet]
		[Route("api/DataMasterApi/GetBalances/{date}")]
		public DataMasterResponse GetBalances(string date)
		{
			DataMasterResponse response = new DataMasterResponse();

			try
			{
				DateTime finalDate = utils.FormatDate(date);
				List<BalanceDetail> details = new BalanceDetail().GetDetailsBankAccountsAndBoxes(finalDate);

				response.Status = "OK";
				response.Result = details;
			}
			catch (Exception ex)
			{
				response.Status = "ERROR";
				response.Message = ex.Message;
				IncidentController.CreateIncident("ERROR SALDOS DE CUENTAS Y BANCOS", ex);
			}

			return response;
		}
	}
}