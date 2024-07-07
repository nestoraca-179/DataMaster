using System;
using System.Linq;
using System.Collections.Generic;
using DataMaster.Controllers;

namespace DataMaster.Models
{
	public class Client : ProfitAdmManager
	{
        public saCliente GetClientByID(string id)
        {
            saCliente client;

            try
            {
                client = db.saCliente.AsNoTracking().Include("saTipoCliente").Include("saSegmento").Include("saVendedor").Include("saCuentaIngEgr")
                    .Include("saPais").Include("saZona").Include("saCondicionPago").SingleOrDefault(c => c.co_cli == id);

                client.saTipoCliente.saCliente = null;
                client.saSegmento.saCliente = null;
                client.saVendedor.saCliente = null;
                client.saCuentaIngEgr.saCliente = null;
                client.saPais.saCliente = null;
                client.saZona.saCliente = null;

                if (client.saCondicionPago != null)
                    client.saCondicionPago.saCliente = null;
            }
            catch (Exception ex)
            {
                client = null;
                IncidentController.CreateIncident("ERROR BUSCANDO CLIENTE " + id, ex);
            }

            return client;
        }

        public List<saCliente> GetAllClients()
        {
			return db.saCliente.AsNoTracking().ToList();
        }
    }
}