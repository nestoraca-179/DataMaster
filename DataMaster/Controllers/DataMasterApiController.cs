using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;
using DataMaster.Models;

namespace DataMaster.Controllers
{
    public class DataMasterApiController : ApiController
    {
		// PEDIDOS

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
		public DataMasterResponse AddOrder(saPedidoVenta order)
		{
			DataMasterResponse response = new DataMasterResponse();
			Usuario u = (HttpContext.Current.Session["USER"] as Usuario);
			string s = (HttpContext.Current.Session["BRANCH"] as saSucursal)?.co_sucur;

			try
			{
				saPedidoVenta new_order = new Order().AddOrder(order, u.username, s, false);
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
		[Route("api/DataMasterApi/AddNewOrder/")]
		public DataMasterResponse AddNewOrder(saPedidoVenta order)
		{
			DataMasterResponse response = new DataMasterResponse();
			Usuario u = (HttpContext.Current.Session["USER"] as Usuario);
			string s = (HttpContext.Current.Session["BRANCH"] as saSucursal)?.co_sucur;

			try
			{
				saPedidoVenta new_order = new Order().AddOrder(order, u.username, s, true);
				LogController.CreateLog(u.username, "PEDIDO", new_order.doc_num, "I", null);

				response.Status = "OK";
				response.Result = new_order;
			}
			catch (Exception ex)
			{
				response.Status = "ERROR";
				response.Message = ex.Message;
				IncidentController.CreateIncident("ERROR AGREGANDO PEDIDO POR COTIZACION", ex);
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

		// COTIZACIONES

		[HttpGet]
		[Route("api/DataMasterApi/GetBudget/{id}")]
		public DataMasterResponse GetBudget(string id)
		{
			DataMasterResponse response = new DataMasterResponse();

			try
			{
				saCotizacionCliente budget = new Budget().GetBudgetByID(id);

				if (budget == null)
				{
					response.Status = "ERROR";
					response.Message = "0"; // COTIZACION NO EXISTE
				}
				else
				{
					response.Status = budget.status == "0" ? "OK" : "ERROR";
					response.Result = budget.status == "0" ? budget : null;
					response.Message = budget.status == "0" ? null : "1"; // COTIZACION BLOQUEADA

					//foreach (saCotizacionClienteReng reng in budget.saCotizacionClienteReng)
					//{
					//	decimal stock = Order.GetStock(reng.co_art, reng.co_alma);
					//	if (stock <= 0 || stock < reng.total_art)
					//		throw new Exception(string.Format(
					//			"No hay stock disponible del articulo {0} en el almacen {1} (DISPONIBLE {2}, SOLICITADA {3})",
					//			reng.co_art, reng.co_alma, stock, reng.total_art
					//		));
					//}
				}
			}
			catch (Exception ex)
			{
				response.Status = "ERROR";
				response.Message = ex.Message; // HA OCURRIDO UN ERROR
				IncidentController.CreateIncident("ERROR BUSCANDO COTIZACION " + id, ex);
			}

			return response;
		}

		[HttpGet]
		[Route("api/DataMasterApi/GetBudgets/{number}")]
		public DataMasterResponse GetOrders(int number)
		{
			DataMasterResponse response = new DataMasterResponse();

			try
			{
				List<saCotizacionCliente> budgets = new Budget().GetAllBudgets(number, true);

				if (budgets.Count > 0)
				{
					response.Status = "OK";
					response.Result = budgets;
				}
				else
				{
					response.Status = "ERROR";
					response.Result = "No se encontraron cotizaciones sin procesar";
				}
			}
			catch (Exception ex)
			{
				response.Status = "ERROR";
				response.Message = ex.Message;
				IncidentController.CreateIncident("ERROR BUSCANDO COTIZACIONES", ex);
			}

			return response;
		}

		// USUARIOS

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
	}
}