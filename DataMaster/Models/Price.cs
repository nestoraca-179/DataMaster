using System;
using System.Collections.Generic;
using System.Linq;

namespace DataMaster.Models
{
	public class Price : ProfitAdmManager
	{
		public saArtPrecio GetPriceByID(Guid id)
		{
			return db.saArtPrecio.AsNoTracking().Single(p => p.rowguid == id);
		}

		public List<saArtPrecio> GetAllPrices()
		{
			return db.saArtPrecio.AsNoTracking().ToList();
		}

		public decimal GetRateUSD()
		{
			List<saTasa> rates = db.saTasa.AsNoTracking().Where(r => r.co_mone.StartsWith("US")).ToList();
			decimal rate = rates.GroupBy(r => r.co_mone).Select(g => g.OrderByDescending(r => r.fecha).First()).ToList()[0].tasa_v;

			return rate;
		}

		public List<pObtenerFechaImpuestoSobreVenta_Result> GetAllImps()
		{
			List<pObtenerFechaImpuestoSobreVenta_Result> result = new List<pObtenerFechaImpuestoSobreVenta_Result>();

			var sp = db.pObtenerFechaImpuestoSobreVenta(DateTime.Now, true).GetEnumerator();
			while (sp.MoveNext())
				result.Add(sp.Current);

			return result;
		}
	}
}