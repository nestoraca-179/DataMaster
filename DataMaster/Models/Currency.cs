using System.Linq;
using System.Collections.Generic;

namespace DataMaster.Models
{
	public class Currency : ProfitAdmManager
    {
        public saMoneda GetcurrencyByID(string id)
        {
			return db.saMoneda.AsNoTracking().Single(c => c.co_mone == id);
        }

        public List<saMoneda> GetAllCurrencies()
		{
            return db.saMoneda.AsNoTracking().ToList();
        }
    }
}