using System;
using System.Linq;
using System.Collections.Generic;
using DataMaster.Controllers;

namespace DataMaster.Models
{
    public class Currency
    {
        public saMoneda GetcurrencyByID(string id)
        {
            ProfitAdmEntities db = new ProfitAdmEntities();
            saMoneda currency;

            try
            {
                currency = db.saMoneda.AsNoTracking().SingleOrDefault(c => c.co_mone == id);
            }
            catch (Exception ex)
            {
                currency = null;
                IncidentController.CreateIncident("ERROR BUSCANDO MONEDA " + id, ex);
            }

            return currency;
        }

        public List<saMoneda> GetAllCurrencies()
		{
			ProfitAdmEntities db = new ProfitAdmEntities();
			List<saMoneda> currencies;

            try
            {
                currencies = db.saMoneda.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                currencies = null;
                IncidentController.CreateIncident("ERROR BUSCANDO MONEDAS", ex);
            }

            return currencies;
        }
    }
}