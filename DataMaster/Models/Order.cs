using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Reflection;
using DataMaster.Controllers;
using DevExpress.DataProcessing;

namespace DataMaster.Models
{
	public class Order : ProfitAdmManager
	{
		public List<saPedidoVenta> GetAllOrders(int number, string sucur)
		{
			List<saPedidoVenta> orders = new List<saPedidoVenta>();

			try
			{
				orders = db.saPedidoVenta.AsNoTracking().Where(o => o.co_sucu_in == sucur).Include("saPedidoVentaReng").Include("saCliente")
					.Include("saVendedor").Include("saCondicionPago").OrderByDescending(i => i.fec_emis).ThenBy(i => i.doc_num).Take(number).ToList();

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

		public saPedidoVenta AddOrder(saPedidoVenta order, string user, string sucur)
		{
			saPedidoVenta new_order = new saPedidoVenta();
			using (ProfitAdmEntities context = new ProfitAdmEntities(entity.ToString()))
			{
				using (DbContextTransaction tran = context.Database.BeginTransaction())
				{
					try
					{
						string n_ord = "";
						bool uso_suc = false;

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

		public saPedidoVenta EditOrder(saPedidoVenta order, string user, string sucur)
		{
			using (ProfitAdmEntities context = new ProfitAdmEntities(entity.ToString()))
			{
				using (DbContextTransaction tran = context.Database.BeginTransaction())
				{
					try
					{
						#region ANTERIOR
						//string sucur = context.saSucursal.AsNoTracking().First().co_sucur; // CAMBIAR
						//saPedidoVenta old = context.saPedidoVenta.AsNoTracking().Single(o => o.doc_num == order.doc_num);
						//old.saPedidoVentaReng = context.saPedidoVentaReng.AsNoTracking().Where(r => r.doc_num == order.doc_num).ToList();

						//foreach (saPedidoVentaReng r in old.saPedidoVentaReng)
						//{
						//	var sp_stock = context.pStockActualizar(r.co_alma, r.co_art, r.co_uni, r.total_art, "COM", false, false);
						//	sp_stock.Dispose();
						//	// var sp_del = context.pEliminarRenglonesPedidoVenta(r.doc_num, r.reng_num, user, "DATAMASTER", sucur, r.rowguid);

						//	// context.Entry(r).State = EntityState.Deleted;
						//}

						//// context.saPedidoVentaReng.RemoveRange(old.saPedidoVentaReng);
						//for (int i = old.saPedidoVentaReng.Count - 1; i >= 0; i--)
						//	context.Entry(old.saPedidoVentaReng.ElementAt(i)).State = EntityState.Deleted;
						//context.SaveChanges();
						//for (int i = old.saPedidoVentaReng.Count - 1; i >= 0; i--)
						//	context.Entry(old.saPedidoVentaReng.ElementAt(i)).State = EntityState.Detached;
						//// old.saPedidoVentaReng.ForEach(delegate(saPedidoVentaReng r) { context.Entry(r).Reload(); });

						//old.co_cli = order.co_cli;
						//old.co_ven = order.co_ven;
						//old.co_mone = order.co_mone;
						//old.co_cond = order.co_cond;
						//old.co_tran = order.co_tran;
						//old.tasa = order.tasa;
						//old.descrip = order.descrip;
						//context.Entry(old).State = EntityState.Modified;
						////var sp = context.pActualizarPedidoVenta(order.doc_num, order.doc_num, order.descrip, order.co_cli, order.co_tran, 
						////	order.co_mone, null, order.co_ven, order.co_cond, order.fec_emis, order.fec_venc, order.fec_reg, order.anulado, 
						////	order.status, order.tasa, null, null, order.porc_desc_glob, order.monto_desc_glob, order.porc_reca, order.monto_reca, 
						////	order.saldo, order.total_bruto, order.monto_imp, order.monto_imp2, order.monto_imp3, order.otros1, order.otros2, 
						////	order.otros3, order.total_neto, order.comentario, order.dir_ent, order.contrib, order.impresa, order.salestax, 
						////	order.impfis, order.impfisfac, order.ven_ter, null, order.campo1, order.campo2, order.campo3, order.campo4, 
						////	order.campo5, order.campo6, order.campo7, order.campo8, user, sucur, order.revisado, order.trasnfe, 
						////	"DATAMASTER", null, changes, order.rowguid);
						////sp.Dispose();

						//foreach (saPedidoVentaReng r in order.saPedidoVentaReng)
						//{
						//	var sp_stock = context.pStockActualizar(r.co_alma, r.co_art, r.co_uni, r.total_art, "COM", true, false);
						//	sp_stock.Dispose();

						//	//var sp_reng = context.pInsertarRenglonesPedidoVenta(r.reng_num, r.doc_num, r.co_art, r.des_art, r.co_uni, r.sco_uni,
						//	//	r.co_alma, r.co_precio, r.tipo_imp, r.tipo_imp2, r.tipo_imp3, r.total_art, r.stotal_art, r.prec_vta, r.porc_desc,
						//	//	r.monto_desc, r.reng_neto, r.pendiente, r.pendiente2, r.monto_desc_glob, r.monto_reca_glob, r.otros1_glob,
						//	//	r.otros2_glob, r.otros3_glob, r.monto_imp_afec_glob, r.monto_imp2_afec_glob, r.monto_imp3_afec_glob, r.tipo_doc,
						//	//	r.rowguid_doc, r.num_doc, r.porc_imp, r.porc_imp2, r.porc_imp3, r.monto_imp, r.monto_imp2, r.monto_imp3, r.otros, 
						//	//	r.total_dev, r.monto_dev, r.comentario, null, sucur, user, r.revisado, r.trasnfe, "DATAMASTER");
						//	// sp_reng.Dispose();
						//	context.saPedidoVentaReng.Add(r);
						//}
						#endregion

						// Obtener el pedido incluyendo los renglones
						saPedidoVenta existing = context.saPedidoVenta.Include(o => o.saPedidoVentaReng)
							.Single(o => o.doc_num == order.doc_num);

						existing.co_cli = order.co_cli;
						existing.co_ven = order.co_ven;
						existing.co_mone = order.co_mone;
						existing.co_cond = order.co_cond;
						existing.co_tran = order.co_tran;
						existing.tasa = order.tasa;
						existing.descrip = order.descrip;
						existing.total_bruto = order.total_bruto;
						existing.monto_imp = order.monto_imp;
						existing.total_neto = order.total_neto;
						existing.saldo= order.saldo;
						existing.co_us_mo = user;
						existing.co_us_mo = user;
						existing.fe_us_mo = DateTime.Now;
						context.Entry(existing).State = EntityState.Modified;

						// Actualizar los datos del encabezado del pedido
						// context.Entry(existing).CurrentValues.SetValues(order);
						// context.Entry(existing).Property(f => f.doc_num).IsModified = false;

						// Eliminar renglones que fueron eliminados
						foreach (saPedidoVentaReng reng in existing.saPedidoVentaReng.ToList())
						{
							var sp_stock = context.pStockActualizar(reng.co_alma, reng.co_art, reng.co_uni, reng.total_art,
								"COM", false, false);
							sp_stock.Dispose();

							if (!order.saPedidoVentaReng.Any(r => r.rowguid == reng.rowguid))
								context.saPedidoVentaReng.Remove(reng);
						}

						// Manejar los renglones de la factura
						foreach (saPedidoVentaReng reng in order.saPedidoVentaReng)
						{
							if (reng.co_uni == "UND") // RENGLON AGREGADO DENTRO DE LA MODIFICACION
							{
								reng.doc_num = order.doc_num;
								reng.co_uni = context.saArtUnidad.AsNoTracking().FirstOrDefault(u => u.co_art == reng.co_art && u.uni_principal).co_uni;
								reng.co_us_in = user;
								reng.co_sucu_in = sucur;
								reng.fe_us_in = DateTime.Now;
								reng.rowguid = Guid.NewGuid();
							}

							reng.co_us_mo = user;
							reng.co_sucu_mo = sucur;
							reng.fe_us_mo = DateTime.Now;

							saPedidoVentaReng e_reng = existing.saPedidoVentaReng.SingleOrDefault(r => r.rowguid == reng.rowguid);
							var sp_stock = context.pStockActualizar(reng.co_alma, reng.co_art, reng.co_uni, reng.total_art,
								"COM", true, false);
							sp_stock.Dispose();

							if (e_reng != null)
							{
								// Actualizar renglón existente
								if (e_reng.reng_num != reng.reng_num)
								{
									context.Entry(e_reng).State = EntityState.Deleted;
									context.SaveChanges();
									existing.saPedidoVentaReng.Add(reng);
								}
								else
								{
									context.Entry(e_reng).CurrentValues.SetValues(reng);
								}
							}
							else
							{
								// Agregar nuevo renglón
								existing.saPedidoVentaReng.Add(reng);
							}
						}

						// context.saPedidoVentaReng.AddRange(order.saPedidoVentaReng);
						context.SaveChanges();
						tran.Commit();
						order.comentario = GetChanges(existing, order); // CAMBIOS
						order.saPedidoVentaReng.ForEach(delegate(saPedidoVentaReng r) { r.saPedidoVenta = null; });
					}
					catch (Exception ex)
					{
						tran.Rollback();
						IncidentController.CreateIncident("ERROR INTERNO MODIFICANDO PEDIDO " + order.doc_num, ex);

						throw ex;
					}
				}
			}

			return order;
		}

		public saPedidoVenta DeleteOrder(string id, string user, string sucur)
		{
			saPedidoVenta order = new saPedidoVenta();
			using (ProfitAdmEntities context = new ProfitAdmEntities(entity.ToString()))
			{
				using (DbContextTransaction tran = context.Database.BeginTransaction()) 
				{
					try
					{
						order = context.saPedidoVenta.AsNoTracking().Single(o => o.doc_num == id);
						order.saPedidoVentaReng = context.saPedidoVentaReng.AsNoTracking().Where(r => r.doc_num == id).ToList();

						context.pEliminarPedidoVenta(order.doc_num, order.validador, "DATAMASTER", user, sucur, order.rowguid);
						foreach (saPedidoVentaReng reng in order.saPedidoVentaReng)
						{
							var sp_stock = context.pStockActualizar(reng.co_alma, reng.co_art, reng.co_uni, reng.total_art,
								"COM", false, false);
							sp_stock.Dispose();
						}

						context.SaveChanges();
						tran.Commit();
					}
					catch (Exception ex)
					{
						tran.Rollback();
						IncidentController.CreateIncident("ERROR INTERNO ELIMINANDO PEDIDO " + order.doc_num, ex);

						throw ex;
					}
				}
			}

			return order;
		}

		public static decimal GetStock(string art, string alm)
		{
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

		private static string GetChanges(saPedidoVenta ord_v, saPedidoVenta ord_n)
		{
			string campos = "";
			Type type = new saPedidoVenta().GetType();

			foreach (PropertyInfo prop in type.GetProperties())
			{
				if (prop.Name != "fe_us_in" && prop.Name != "fe_us_mo")
				{
					string valor1 = prop.GetValue(ord_v) == null ? "" : prop.GetValue(ord_v).ToString().Trim();
					string valor2 = prop.GetValue(ord_n) == null ? "" : prop.GetValue(ord_n).ToString().Trim();

					if (valor1 != valor2)
					{
						campos += string.Format("{0}: {1} -> {2}; ", prop.Name, valor1, valor2);
					}
				}
			}

			return campos;
		}
	}
}