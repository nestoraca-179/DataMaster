using System.Linq;
using System.Collections.Generic;

namespace DataMaster.Models
{
	public class Transport : ProfitAdmManager
    {
        public saTransporte GetTransportByID(string id)
		{
            return db.saTransporte.AsNoTracking().Single(c => c.co_tran == id);
        }

        public List<saTransporte> GetAllTransports()
		{
            return db.saTransporte.AsNoTracking().ToList();
        }
    }
}