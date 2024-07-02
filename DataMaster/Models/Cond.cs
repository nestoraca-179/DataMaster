using System;
using System.Linq;
using System.Collections.Generic;
using DataMaster.Controllers;

namespace DataMaster.Models
{
    public class Cond
    {
        public saCondicionPago GetCondByID(string id)
        {
            ProfitAdmEntities db = new ProfitAdmEntities();
			saCondicionPago cond;

            try
            {
                cond = db.saCondicionPago.AsNoTracking().SingleOrDefault(c => c.co_cond == id);
            }
            catch (Exception ex)
            {
                cond = null;
                IncidentController.CreateIncident("ERROR BUSCANDO CONDICION " + id, ex);
            }

            return cond;
        }

        public List<saCondicionPago> GetAllConds()
        {
			ProfitAdmEntities db = new ProfitAdmEntities();
			List<saCondicionPago> conds;

            try
            {
                conds = db.saCondicionPago.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                conds = null;
				IncidentController.CreateIncident("ERROR BUSCANDO CONDICIONES", ex);
            }

            return conds;
        }
    }
}