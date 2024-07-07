using System.Linq;
using System.Collections.Generic;

namespace DataMaster.Models
{
	public class Cond : ProfitAdmManager
    {
        public saCondicionPago GetCondByID(string id)
        {
            return db.saCondicionPago.AsNoTracking().Single(c => c.co_cond == id);
        }

        public List<saCondicionPago> GetAllConds()
        {
            return db.saCondicionPago.AsNoTracking().ToList();
        }
    }
}