using System;
using System.Linq;
using System.Collections.Generic;
using DataMaster.Controllers;

namespace DataMaster.Models
{
    public class Seller
    {
        public saVendedor GetSellerByID(string id)
        {
            ProfitAdmEntities db = new ProfitAdmEntities();
            saVendedor seller;

            try
            {
                seller = db.saVendedor.AsNoTracking().SingleOrDefault(s => s.co_ven == id);
            }
            catch (Exception ex)
            {
                seller = null;
                IncidentController.CreateIncident("ERROR BUSCANDO VENDEDOR " + id, ex);
            }

            return seller;
        }

        public List<saVendedor> GetAllSellers()
		{
			ProfitAdmEntities db = new ProfitAdmEntities();
			List<saVendedor> sellers;

            try
            {
                sellers = db.saVendedor.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                sellers = null;
				IncidentController.CreateIncident("ERROR BUSCANDO VENDEDORES", ex);
            }

            return sellers;
        }
    }
}