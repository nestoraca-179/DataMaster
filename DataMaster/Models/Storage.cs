using System.Linq;
using System.Collections.Generic;

namespace DataMaster.Models
{
	public class Storage : ProfitAdmManager
    {
        public saAlmacen GetAlmByID(string id)
        {
            return db.saAlmacen.AsNoTracking().Single(c => c.co_alma == id);
        }

        public List<saAlmacen> GetAllAlms()
        {
            return db.saAlmacen.AsNoTracking().ToList();
        }
    }
}