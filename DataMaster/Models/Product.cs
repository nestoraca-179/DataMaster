using DataMaster.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataMaster.Models
{
    public class Product
    {
        public saArticulo GetArtByID(string id)
        {
            ProfitAdmEntities db = new ProfitAdmEntities();
            saArticulo art;

            try
            {
                art = db.saArticulo.AsNoTracking().SingleOrDefault(c => c.co_art == id);
            }
            catch (Exception ex)
            {
                art = null;
                IncidentController.CreateIncident("ERROR BUSCANDO ARTICULO " + id, ex);
            }

            return art;
        }

        public List<saArticulo> GetAllArts()
		{
			ProfitAdmEntities db = new ProfitAdmEntities();
			List<saArticulo> arts;

            try
            {
                arts = db.saArticulo.AsNoTracking().ToList();
            }
            catch (Exception ex)
            {
                arts = null;
				IncidentController.CreateIncident("ERROR BUSCANDO ARTICULOS", ex);
            }

            return arts;
        }

        public List<string> GetAllNameArts()
		{
			ProfitAdmEntities db = new ProfitAdmEntities();
			List<string> prods;

            try
            {
				prods = (from a in db.saArticulo.AsNoTracking()
						 select a.co_art.Trim() + "/" + a.art_des.Trim()).OrderBy(a => a).ToList();
			}
            catch (Exception ex)
            {
                prods = null;
                IncidentController.CreateIncident("ERROR BUSCANDO NOMBRES DE PRODUCTOS", ex);
            }

            return prods;
        }
    }
}