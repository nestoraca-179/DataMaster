using DataMaster.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataMaster.Models
{
	public class Note : ProfitAdmManager
	{
		public saNotaEntregaVenta GetSellNoteByID(string id)
		{
			saNotaEntregaVenta order;

			try
			{
				order = db.saNotaEntregaVenta.AsNoTracking().Include("saNotaEntregaVentaReng").Include("saCliente").Include("saCondicionPago")
					.Include("saVendedor").Single(i => i.doc_num == id);

				order.saCliente.saNotaEntregaVenta = null;
				order.saVendedor.saNotaEntregaVenta = null;
				order.saCondicionPago.saNotaEntregaVenta = null;

				foreach (saNotaEntregaVentaReng reng in order.saNotaEntregaVentaReng)
				{
					reng.saNotaEntregaVenta = null;
				}
			}
			catch (Exception ex)
			{
				order = null;
				IncidentController.CreateIncident("ERROR BUSCANDO NOTA ENTREGA " + id, ex);
			}

			return order;
		}

		public List<saNotaEntregaVenta> GetAllSellNotes(int number, string sucur)
		{
			List<saNotaEntregaVenta> orders = new List<saNotaEntregaVenta>();

			try
			{
				orders = db.saNotaEntregaVenta.AsNoTracking().Where(o => o.co_sucu_in == sucur).Include("saNotaEntregaVentaReng").Include("saCliente")
					.Include("saVendedor").Include("saCondicionPago").OrderByDescending(i => i.fec_emis).ThenByDescending(i => i.doc_num).Take(number).ToList();

				foreach (saNotaEntregaVenta order in orders)
				{
					order.saVendedor.saNotaEntregaVenta = null;
					order.saCondicionPago.saNotaEntregaVenta = null;
					order.saCliente.saNotaEntregaVenta = null;
					foreach (saNotaEntregaVentaReng reng in order.saNotaEntregaVentaReng)
					{
						reng.saNotaEntregaVenta = null;
					}
				}
			}
			catch (Exception ex)
			{
				orders = null;
				IncidentController.CreateIncident("ERROR BUSCANDO NOTAS ENTREGA", ex);
			}

			return orders;
		}

		public void MarkAsVerified(string id)
		{
			saNotaEntregaVenta order = GetSellNoteByID(id);
			order.campo8 = "OK";
			db.Entry(order).State = EntityState.Modified;
			db.SaveChanges();
		}
	}
}