using System;
using System.Linq;
using System.Collections.Generic;
using DataMaster.Controllers;

namespace DataMaster.Models
{
    public class Storage
    {
        public saAlmacen GetAlmByID(string id)
        {
            ProfitAdmEntities db = new ProfitAdmEntities();
            saAlmacen alm;

            try
            {
                alm = db.saAlmacen.AsNoTracking().SingleOrDefault(c => c.co_alma == id);
            }
            catch (Exception ex)
            {
                alm = null;
                IncidentController.CreateIncident("ERROR BUSCANDO ALMACEN " + id, ex);
            }

            return alm;
        }

        public List<saAlmacen> GetAllAlms()
        {
			ProfitAdmEntities db = new ProfitAdmEntities();
			List<saAlmacen> alms;

            try
            {
                alms = db.saAlmacen.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                alms = null;
                IncidentController.CreateIncident("ERROR BUSCANDO ALMACENES", ex);
            }

            return alms;
        }
    }
}