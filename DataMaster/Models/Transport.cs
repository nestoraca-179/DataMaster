using System;
using System.Linq;
using System.Collections.Generic;
using DataMaster.Controllers;

namespace DataMaster.Models
{
    public class Transport
    {
        public saTransporte GetTransportByID(string id)
		{
			ProfitAdmEntities db = new ProfitAdmEntities();
			saTransporte transport;

            try
            {
                transport = db.saTransporte.AsNoTracking().SingleOrDefault(c => c.co_tran == id);
            }
            catch (Exception ex)
            {
                transport = null;
                IncidentController.CreateIncident("ERROR BUSCANDO TRANSPORTE " + id, ex);
            }

            return transport;
        }

        public List<saTransporte> GetAllTransports()
		{
			ProfitAdmEntities db = new ProfitAdmEntities();
			List<saTransporte> transports;

            try
            {
                transports = db.saTransporte.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                transports = null;
				IncidentController.CreateIncident("ERROR BUSCANDO TRANSPORTES", ex);
            }

            return transports;
        }
    }
}