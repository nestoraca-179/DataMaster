using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataMaster.Models
{
	public class Invoice : ProfitAdmManager
	{
		public object GetStatsInvoices(DateTime fec_d, DateTime fec_h, string sucur)
		{
			int totalCountSale = 0, totalCountBuy = 0, totalCountSaleSuc = 0, totalCountBuySuc = 0;
			decimal totalAmountSale = 0, totalAmountBuy = 0, totalState;
			decimal totalAmountSaleSuc = 0, totalAmountBuySuc = 0, totalStateSuc;
			decimal totalReimbExpSale = 0, totalReimbExpSaleSuc = 0, totalReimbExpBuy = 0, totalReimbExpBuySuc = 0;

			// VENTAS
			var sp1 = db.RepFacturaVentaxFecha(null, null, fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null);
			var enumerator1 = sp1.GetEnumerator();

			while (enumerator1.MoveNext())
			{
				decimal total_neto_v = enumerator1.Current.anulado ? 0 : enumerator1.Current.total_neto.Value;

				totalCountSale++;
				totalAmountSale += Math.Round(decimal.Parse(total_neto_v.ToString()), 2);

				if (enumerator1.Current.co_sucu_in?.Trim() == sucur)
				{
					totalCountSaleSuc++;
					totalAmountSaleSuc += Math.Round(decimal.Parse(total_neto_v.ToString()), 2);
				}
			}

			// COMPRAS
			var sp2 = db.RepCompraxFecha(null, null, fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null, null);
			var enumerator2 = sp2.GetEnumerator();

			while (enumerator2.MoveNext())
			{
				decimal total_neto_c = enumerator2.Current.anulado ? 0 : enumerator2.Current.total_neto.Value;

				totalCountBuy++;
				totalAmountBuy += Math.Round(decimal.Parse(total_neto_c.ToString()), 2);

				if (enumerator2.Current.co_sucu_in?.Trim() == sucur)
				{
					totalCountBuySuc++;
					totalAmountBuySuc += Math.Round(decimal.Parse(total_neto_c.ToString()), 2);
				}
			}

			// ESTADO DE GANANCIA
			totalState = totalAmountSale - totalAmountBuy;
			totalStateSuc = totalAmountSaleSuc - totalAmountBuySuc;

			// GASTOS REEMBOLSABLES - ISH
			if (db.Database.Connection.Database == "PP2K12_ISH_ADM")
			{
				// totalReimbExpSale
				var sp3 = db.RepFacturaVentaxArt2("910101001-001", "910101004-001", fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null,
					null, null, null, null, null, null, null, null, null, null);
				var enumerator3 = sp3.GetEnumerator();

				while (enumerator3.MoveNext())
				{
					if (enumerator3.Current.anulado)
						totalReimbExpSale += 0;
					else
						totalReimbExpSale += Convert.ToDecimal(enumerator3.Current.neto);
				}
				// totalReimbExpSale

				// totalReimbExpSaleSuc
				var sp4 = db.RepFacturaVentaxArt2("910101001-001", "910101004-001", fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null,
					null, null, null, null, null, null, sucur, null, null, null);
				var enumerator4 = sp4.GetEnumerator();

				while (enumerator4.MoveNext())
				{
					if (enumerator4.Current.anulado)
						totalReimbExpSaleSuc += 0;
					else
						totalReimbExpSaleSuc += Convert.ToDecimal(enumerator4.Current.neto);
				}
				// totalReimbExpSaleSuc

				// totalReimbExpBuy
				var sp5 = db.RepCompraxArt2("920101001-001", "920101004-001", fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null,
					null, null, null, null, null, null);
				var enumerator5 = sp5.GetEnumerator();

				while (enumerator5.MoveNext())
				{
					if (enumerator5.Current.anulado)
						totalReimbExpBuy += 0;
					else
						totalReimbExpBuy += Convert.ToDecimal(enumerator5.Current.neto);
				}
				// totalReimbExpBuy

				// totalReimbExpBuySuc
				var sp6 = db.RepCompraxArt2("920101001-001", "920101004-001", fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null,
					null, null, sucur, null, null, null);
				var enumerator6 = sp6.GetEnumerator();

				while (enumerator6.MoveNext())
				{
					if (enumerator6.Current.anulado)
						totalReimbExpBuySuc += 0;
					else
						totalReimbExpBuySuc += Convert.ToDecimal(enumerator6.Current.neto);
				}
				// totalReimbExpBuySuc

				enumerator3.Dispose();
				enumerator4.Dispose();
				enumerator5.Dispose();
				enumerator6.Dispose();
			}

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
					totalAmountBuy,
					totalState,
					totalReimbExpSale,
					totalReimbExpBuy
				},
				suc = new
				{
					totalCountSale = totalCountSaleSuc,
					totalCountBuy = totalCountBuySuc,
					totalAmountSale = totalAmountSaleSuc,
					totalAmountBuy = totalAmountBuySuc,
					totalState = totalStateSuc,
					totalReimbExpSale = totalReimbExpSaleSuc,
					totalReimbExpBuy = totalReimbExpBuySuc
				},
			};

			return obj;
		}

	}
}