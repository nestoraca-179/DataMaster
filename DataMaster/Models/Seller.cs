using System.Linq;
using System.Collections.Generic;
using System;
using System.Globalization;

namespace DataMaster.Models
{
	public class Seller : ProfitAdmManager
    {
        public saVendedor GetSellerByID(string id)
        {
            return db.saVendedor.AsNoTracking().Single(s => s.co_ven == id);
        }

        public List<saVendedor> GetAllSellers()
		{
            return db.saVendedor.AsNoTracking().ToList();
        }

        public List<saVendedor> GetMostActiveSellers(DateTime fec_d, DateTime fec_h, int number, string sucur) 
        {
            List<saVendedor> sellers = new List<saVendedor>();
            List<saVendedor> sellers_temp = new List<saVendedor>();

            var sp = db.RepFacturaVentaxVendedor(null, null, fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, "NOT", sucur, null, null, null);
            var enumerator = sp.GetEnumerator();

			while (enumerator.MoveNext())
			{
				saVendedor seller = new saVendedor();

				seller.co_ven = enumerator.Current.co_ven.Trim();
				seller.ven_des = enumerator.Current.ven_des.Trim();
				seller.comision = enumerator.Current.anulado ? 0 : Math.Round(enumerator.Current.total_neto.Value / enumerator.Current.tasa, 2); // USADO PARA EL TOTAL

				sellers_temp.Add(seller);
			}

			decimal total = sellers_temp.Select(s => s.comision).Sum();
			foreach (saVendedor sl in sellers_temp.OrderBy(c => c.co_ven))
			{
				if (!sellers.Any(c => c.co_ven == sl.co_ven))
				{
					saVendedor new_seller = new saVendedor();
					int count_s = sellers_temp.Where(s => s.co_ven == sl.co_ven).Count();
					decimal total_s = sellers_temp.Where(s => s.co_ven == sl.co_ven).Select(s => s.comision).Sum();

					new_seller.co_ven = sl.co_ven;
					new_seller.ven_des = sl.ven_des;
					new_seller.campo1 = count_s.ToString();
					new_seller.campo2 = total_s.ToString("N2", CultureInfo.GetCultureInfo("es-ES"));
					new_seller.campo3 = Math.Round((total_s * 100) / total, 2).ToString("N2", CultureInfo.GetCultureInfo("es-ES"));
					new_seller.comision = total_s;

					sellers.Add(new_seller);
				}
			}

			sellers = sellers.OrderByDescending(c => c.comision).ToList();
			if (sellers.Count > number)
				sellers.RemoveRange(number, sellers.Count - number);

			return sellers;
        }
    }
}