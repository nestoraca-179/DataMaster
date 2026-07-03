using DataMaster.Controllers;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace DataMaster.Models
{
	public class Invoice : ProfitAdmManager
	{
		public saFacturaVenta GetSellInvoiceByID(string id)
		{
			saFacturaVenta invoice;

			try
			{
				invoice = db.saFacturaVenta.AsNoTracking().Include("saFacturaVentaReng").Include("saCliente").Include("saCondicionPago")
					.Include("saVendedor").Single(i => i.doc_num == id);

				invoice.saCliente.saFacturaVenta = null;
				invoice.saVendedor.saFacturaVenta = null;
				invoice.saCondicionPago.saFacturaVenta = null;

				foreach (saFacturaVentaReng reng in invoice.saFacturaVentaReng)
				{
					reng.saFacturaVenta = null;
				}
			}
			catch (Exception ex)
			{
				invoice = null;
				IncidentController.CreateIncident("ERROR BUSCANDO FACTURA VENTA " + id, ex);
			}

			return invoice;
		}

		public List<saFacturaVenta> GetAllSellInvoices(int number, string sucur)
		{
			List<saFacturaVenta> invoices = new List<saFacturaVenta>();

			try
			{
				invoices = db.saFacturaVenta.AsNoTracking().Where(o => o.co_sucu_in == sucur).Include("saFacturaVentaReng").Include("saCliente")
					.Include("saVendedor").Include("saCondicionPago").OrderByDescending(i => i.fec_emis).ThenByDescending(i => i.doc_num).Take(number).ToList();

				foreach (saFacturaVenta invoice in invoices)
				{
					invoice.saVendedor.saFacturaVenta = null;
					invoice.saCondicionPago.saFacturaVenta = null;
					invoice.saCliente.saFacturaVenta = null;
					foreach (saFacturaVentaReng reng in invoice.saFacturaVentaReng)
					{
						reng.saFacturaVenta = null;
					}
				}
			}
			catch (Exception ex)
			{
				invoices = null;
				IncidentController.CreateIncident("ERROR BUSCANDO FACTURAS VENTA", ex);
			}

			return invoices;
		}

		public void MarkAsVerified(string id)
		{
			saFacturaVenta invoice = GetSellInvoiceByID(id);
			invoice.campo8 = "OK";
			db.Entry(invoice).State = EntityState.Modified;
			db.SaveChanges();
		}

		public object GetStatsInvoices(DateTime fec_d, DateTime fec_h, string sucur)
		{
			int totalCountSale = 0, totalCountBuy = 0, totalCountSaleSuc = 0, totalCountBuySuc = 0;
			decimal totalAmountSale = 0, totalAmountBuy = 0, totalState;
			decimal totalAmountSaleSuc = 0, totalAmountBuySuc = 0, totalStateSuc;
			decimal totalReimbExpSale = 0, totalReimbExpSaleSuc = 0, totalReimbExpBuy = 0, totalReimbExpBuySuc = 0;

			// MONTOS USD
			decimal totalAmountSaleUSD = 0, totalAmountSaleSucUSD = 0;
			decimal totalAmountBuyUSD = 0, totalAmountBuySucUSD = 0;

			// VENTAS
			var sp1 = db.RepFacturaVentaxFecha(null, null, fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
			var enumerator1 = sp1.GetEnumerator();

			while (enumerator1.MoveNext())
			{
				decimal total_neto_v = enumerator1.Current.anulado ? 0 : enumerator1.Current.total_neto.Value;
				decimal tasa = enumerator1.Current.tasa;

				totalCountSale++;
				totalAmountSale += Math.Round(decimal.Parse(total_neto_v.ToString()), 2);
				totalAmountSaleUSD += Math.Round(total_neto_v / tasa, 2);

				if (enumerator1.Current.co_sucu_in?.Trim() == sucur)
				{
					totalCountSaleSuc++;
					totalAmountSaleSuc += Math.Round(decimal.Parse(total_neto_v.ToString()), 2);
					totalAmountSaleSucUSD += Math.Round(total_neto_v / tasa, 2);
				}
			}

			// COMPRAS
			var sp2 = db.RepCompraxFecha(null, null, fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null, null);
			var enumerator2 = sp2.GetEnumerator();

			while (enumerator2.MoveNext())
			{
				decimal total_neto_c = enumerator2.Current.anulado ? 0 : enumerator2.Current.total_neto.Value;
				decimal tasa = enumerator2.Current.tasa;

				totalCountBuy++;
				totalAmountBuy += Math.Round(decimal.Parse(total_neto_c.ToString()), 2);
				totalAmountBuyUSD += Math.Round(total_neto_c / tasa, 2);

				if (enumerator2.Current.co_sucu_in?.Trim() == sucur)
				{
					totalCountBuySuc++;
					totalAmountBuySuc += Math.Round(decimal.Parse(total_neto_c.ToString()), 2);
					totalAmountBuySucUSD += Math.Round(total_neto_c / tasa, 2);
				}
			}

			// ESTADO DE GANANCIA
			totalState = totalAmountSale - totalAmountBuy;
			totalStateSuc = totalAmountSaleSuc - totalAmountBuySuc;

			enumerator1.Dispose();
			enumerator2.Dispose();

			// OBJETO ESTADISTICAS
			var obj = new
			{
				all = new
				{
					totalCountSale,
					totalCountBuy,
					totalAmountSale,
					totalAmountSaleUSD,
					totalAmountBuy,
					totalAmountBuyUSD,
					totalState,
					totalReimbExpSale,
					totalReimbExpBuy
				},
				suc = new
				{
					totalCountSale = totalCountSaleSuc,
					totalCountBuy = totalCountBuySuc,
					totalAmountSale = totalAmountSaleSuc,
					totalAmountSaleUSD = totalAmountSaleSucUSD,
					totalAmountBuy = totalAmountBuySuc,
					totalAmountBuyUSD = totalAmountBuySucUSD,
					totalState = totalStateSuc,
					totalReimbExpSale = totalReimbExpSaleSuc,
					totalReimbExpBuy = totalReimbExpBuySuc
				},
			};

			return obj;
		}

		public object GetStatsInvoicesWithOrders(DateTime fec_d, DateTime fec_h, string sucur)
		{
			int totalCountSale = 0, totalCountBuy = 0, totalCountSaleSuc = 0, totalCountBuySuc = 0;
			decimal totalAmountSale = 0, totalAmountBuy = 0, totalState;
			decimal totalAmountSaleSuc = 0, totalAmountBuySuc = 0, totalStateSuc;
			decimal totalReimbExpSale = 0, totalReimbExpSaleSuc = 0, totalReimbExpBuy = 0, totalReimbExpBuySuc = 0;

			// MONTOS USD
			decimal totalAmountSaleUSD = 0, totalAmountSaleSucUSD = 0;
			decimal totalAmountBuyUSD = 0, totalAmountBuySucUSD = 0;

			// VENTAS
			var sp1 = db.RepFacturaVentaxFecha(null, null, fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
			var enumerator1 = sp1.GetEnumerator();

			while (enumerator1.MoveNext())
			{
				decimal total_neto_v = enumerator1.Current.anulado ? 0 : enumerator1.Current.total_neto.Value;
				decimal tasa = enumerator1.Current.tasa;

				totalCountSale++;
				totalAmountSale += Math.Round(decimal.Parse(total_neto_v.ToString()), 2);
				totalAmountSaleUSD += Math.Round(total_neto_v / tasa, 2);

				if (enumerator1.Current.co_sucu_in?.Trim() == sucur)
				{
					totalCountSaleSuc++;
					totalAmountSaleSuc += Math.Round(decimal.Parse(total_neto_v.ToString()), 2);
					totalAmountSaleSucUSD += Math.Round(total_neto_v / tasa, 2);
				}
			}

			// COMPRAS
			var sp2 = db.RepPedidoVentaxFecha(null, null, fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, "SPRO", null, null, null, null, null);
			var enumerator2 = sp2.GetEnumerator();

			while (enumerator2.MoveNext())
			{
				decimal total_neto_c = enumerator2.Current.anulado ? 0 : enumerator2.Current.total_neto.Value;
				decimal tasa = enumerator2.Current.tasa;

				totalCountBuy++;
				totalAmountBuy += Math.Round(decimal.Parse(total_neto_c.ToString()), 2);
				totalAmountBuyUSD += Math.Round(total_neto_c / tasa, 2);

				if (enumerator2.Current.co_sucu_in?.Trim() == sucur)
				{
					totalCountBuySuc++;
					totalAmountBuySuc += Math.Round(decimal.Parse(total_neto_c.ToString()), 2);
					totalAmountBuySucUSD += Math.Round(total_neto_c / tasa, 2);
				}
			}

			// ESTADO DE GANANCIA
			totalState = totalAmountSale - totalAmountBuy;
			totalStateSuc = totalAmountSaleSuc - totalAmountBuySuc;

			enumerator1.Dispose();
			enumerator2.Dispose();

			// OBJETO ESTADISTICAS
			var obj = new
			{
				all = new
				{
					totalCountSale,
					totalCountBuy,
					totalAmountSale,
					totalAmountSaleUSD,
					totalAmountBuy,
					totalAmountBuyUSD,
					totalState,
					totalReimbExpSale,
					totalReimbExpBuy
				},
				suc = new
				{
					totalCountSale = totalCountSaleSuc,
					totalCountBuy = totalCountBuySuc,
					totalAmountSale = totalAmountSaleSuc,
					totalAmountSaleUSD = totalAmountSaleSucUSD,
					totalAmountBuy = totalAmountBuySuc,
					totalAmountBuyUSD = totalAmountBuySucUSD,
					totalState = totalStateSuc,
					totalReimbExpSale = totalReimbExpSaleSuc,
					totalReimbExpBuy = totalReimbExpBuySuc
				},
			};

			return obj;
		}
	}
}