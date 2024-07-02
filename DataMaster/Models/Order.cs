using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using DataMaster.Controllers;

namespace DataMaster.Models
{
	public class Order
	{
		public List<saPedidoVenta> GetAllOrders(int number)
		{
			ProfitAdmEntities db = new ProfitAdmEntities();
			List<saPedidoVenta> orders = new List<saPedidoVenta>();

			try
			{
				orders = db.saPedidoVenta.AsNoTracking().Include("saPedidoVentaReng").Include("saCliente").Include("saVendedor")
					.Include("saCondicionPago").OrderByDescending(i => i.fec_emis).ThenBy(i => i.doc_num).Take(number).ToList();

				foreach (saPedidoVenta order in orders)
				{
					order.saVendedor.saPedidoVenta = null;
					order.saCondicionPago.saPedidoVenta = null;
					order.saCliente.saPedidoVenta = null;
					foreach (saPedidoVentaReng reng in order.saPedidoVentaReng)
					{
						reng.saPedidoVenta = null;
					}
				}
			}
			catch (Exception ex)
			{
				orders = null;
				IncidentController.CreateIncident("ERROR BUSCANDO PEDIDOS", ex);
			}

			return orders;
		}

		public saPedidoVenta AddOrder(saPedidoVenta order, string user)
		{
			saPedidoVenta new_order = new saPedidoVenta();

			using (ProfitAdmEntities context = new ProfitAdmEntities())
			{
				using (DbContextTransaction tran = context.Database.BeginTransaction())
				{
					try
					{
						string n_ord = "";
						bool uso_suc = false;
						string sucur = context.saSucursal.AsNoTracking().First().co_sucur;

						// SERIE USA SUCURSAL
						var sp_1 = context.pSeleccionarUsoSucursalConsecutivoTipo("PCLI_NUM").GetEnumerator();
						if (sp_1.MoveNext())
							uso_suc = sp_1.Current.UsoSucursal;
						sp_1.Dispose();

						var sp_n_ord = context.pConsecutivoProximo(uso_suc ? sucur : null, "PCLI_NUM").GetEnumerator();
						if (sp_n_ord.MoveNext())
							n_ord = sp_n_ord.Current;
						sp_n_ord.Dispose();

						// PEDIDO
						var sp = context.pInsertarPedidoVenta(n_ord, order.descrip, order.co_cli, order.co_tran, order.co_mone, null, order.co_ven,
							order.co_cond, order.fec_emis, order.fec_venc, order.fec_reg, order.anulado, order.status, order.tasa, null, null, 
							order.porc_desc_glob, order.monto_desc_glob, order.porc_reca, order.monto_reca, order.saldo, order.total_bruto, 
							order.monto_imp, order.monto_imp2, order.monto_imp3, order.otros1, order.otros2, order.otros3, order.total_neto, 
							null, order.comentario, order.dir_ent, order.contrib, order.impresa, order.salestax, order.impfis, order.impfisfac, 
							order.ven_ter, order.campo1, order.campo2, order.campo3, order.campo4, order.campo5, order.campo6, order.campo7, 
							order.campo8, user, sucur, order.revisado, order.trasnfe, "DATAMASTER");
						sp.Dispose();

						// RENGLONES
						foreach (saPedidoVentaReng r in order.saPedidoVentaReng)
						{
							r.co_uni = context.saArtUnidad.AsNoTracking().FirstOrDefault(u => u.co_art == r.co_art && u.uni_principal).co_uni;
							var sp_stock = context.pStockActualizar(r.co_alma, r.co_art, r.co_uni, r.total_art, "COM", true, false);
							sp_stock.Dispose();

							var sp_reng = context.pInsertarRenglonesPedidoVenta(r.reng_num, n_ord, r.co_art, r.des_art, r.co_uni, r.sco_uni, 
								r.co_alma, r.co_precio, r.tipo_imp, r.tipo_imp2, r.tipo_imp3, r.total_art, r.stotal_art, r.prec_vta, r.porc_desc, 
								r.monto_desc, r.reng_neto, r.pendiente, r.pendiente2, r.monto_desc_glob, r.monto_reca_glob, r.otros1_glob, 
								r.otros2_glob, r.otros3_glob, r.monto_imp_afec_glob, r.monto_imp2_afec_glob, r.monto_imp3_afec_glob, r.tipo_doc, 
								r.rowguid_doc, r.num_doc, r.porc_imp, r.porc_imp2, r.porc_imp3, r.monto_imp, r.monto_imp2, r.monto_imp3, r.otros, r.total_dev,
								r.monto_dev, r.comentario, null, sucur, user, r.revisado, r.trasnfe, "DATAMASTER");
							sp_reng.Dispose();
						}

						tran.Commit();
						new_order = context.saPedidoVenta.AsNoTracking().Single(i => i.doc_num.Trim() == n_ord.Trim());
						new_order.saPedidoVentaReng = context.saPedidoVentaReng.AsNoTracking().Where(r => r.doc_num.Trim() == n_ord.Trim()).ToList();
						new_order.saCliente = context.saCliente.AsNoTracking().Single(c => c.co_cli.Trim() == new_order.co_cli.Trim());
					}
					catch (Exception ex)
					{
						tran.Rollback();
						IncidentController.CreateIncident("ERROR INTERNO AGREGANDO PEDIDO", ex);

						throw ex;
					}
				}
			}

			return new_order;
		}

		public static decimal GetStock(string art, string alm)
		{
			ProfitAdmEntities db = new ProfitAdmEntities();

			decimal result = 0;
			decimal stock_act = 0, stock_com = 0;

			var sp_1 = db.pConsultarStockArticulo(art, alm, "ACT").GetEnumerator();
			if (sp_1.MoveNext())
				stock_act = sp_1.Current.stock;
			sp_1.Dispose();

			var sp_2 = db.pConsultarStockArticulo(art, alm, "COM").GetEnumerator();
			if (sp_2.MoveNext())
				stock_com = sp_2.Current.stock;
			sp_2.Dispose();

			result = stock_act - stock_com;

			return result;
		}
	}
}