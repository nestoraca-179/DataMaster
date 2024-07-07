using System.Collections.Generic;
using System.Linq;

namespace DataMaster.Models
{
	public class Product : ProfitAdmManager
    {
        public saArticulo GetArtByID(string id)
        {
            return db.saArticulo.AsNoTracking().Single(c => c.co_art == id);
        }

        public List<saArticulo> GetAllArts()
		{
            return db.saArticulo.AsNoTracking().ToList();
        }
    }
}