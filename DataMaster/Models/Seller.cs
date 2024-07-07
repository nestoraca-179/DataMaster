using System.Linq;
using System.Collections.Generic;

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
    }
}