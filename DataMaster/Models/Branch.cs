using System.Linq;
using System.Collections.Generic;

namespace DataMaster.Models
{
	public class Branch : ProfitAdmManager
    {
        public saSucursal GetBranchByID(string id)
        {
			return db.saSucursal.AsNoTracking().Single(c => c.co_sucur == id);
		}

        public List<saSucursal> GetAllBranchs()
        {
			return db.saSucursal.AsNoTracking().ToList();
		}
    
        public bool UseBranchs()
        {
            return db.par_emp.AsNoTracking().First().v_maneja_sucursales;
        }
    }
}