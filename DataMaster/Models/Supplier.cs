using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace DataMaster.Models
{
	public class Supplier : ProfitAdmManager
	{
		public List<saProveedor> GetMostActiveSuppliers(DateTime fec_d, DateTime fec_h, int number, string sucur)
		{
			List<saProveedor> suppliers = new List<saProveedor>();

			var sp = db.RepProveedorMasCompra(fec_d, fec_h, null, null, number, null, sucur, null, null, null);
			var enumerator = sp.GetEnumerator();

			while (enumerator.MoveNext())
			{
				saProveedor supplier = new saProveedor();

				supplier.co_prov = enumerator.Current.co_prov.Trim();
				supplier.prov_des = enumerator.Current.prov_des.Trim();
				supplier.campo1 = Convert.ToDouble(enumerator.Current.Compra).ToString("N2", CultureInfo.GetCultureInfo("es-ES"));
				supplier.campo2 = Math.Round(Convert.ToDouble((enumerator.Current.Compra * 100) / enumerator.Current.Compra_total), 2).ToString("N2", CultureInfo.GetCultureInfo("es-ES"));

				suppliers.Add(supplier);
			}

			return suppliers;
		}

		public List<saProveedor> GetMostMorousSuppliers(int number)
		{
			List<saProveedor> suppliers = new List<saProveedor>();

			DateTime fec_h = DateTime.Now;
			DateTime fec_d = fec_h.AddDays(-(fec_h.Day - 1));

			var sp = db.RepEstadoCuentaProv(fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null, null, null);
			var enumerator = sp.GetEnumerator();

			while (enumerator.MoveNext())
			{
				saProveedor supplier = new saProveedor();

				supplier.co_prov = enumerator.Current.co_prov;
				supplier.prov_des = enumerator.Current.prov_des;
				supplier.campo1 = ((enumerator.Current.tot_debe ?? 0) - (enumerator.Current.tot_haber ?? 0)).ToString();

				suppliers.Add(supplier);
			}

			suppliers = (from s in suppliers
						 group s.campo1 by (s.co_prov, s.prov_des) into g
						 select new saProveedor
						 {
							 co_prov = g.Key.co_prov,
							 prov_des = g.Key.co_prov + " - " + g.Key.prov_des,
							 campo1 = Math.Round(g.Select(x => double.Parse(x)).Sum(), 2).ToString()

						 }).OrderByDescending(x => double.Parse(x.campo1)).ToList();

			if (suppliers.Count > number)
				suppliers.RemoveRange(number, suppliers.Count - number);

			return suppliers;
		}
	}
}