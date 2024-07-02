using System;
using System.Linq;
using System.Globalization;
using System.Collections.Generic;
using DataMaster.Controllers;

namespace DataMaster.Models
{
    public class Client
    {
        public saCliente GetClientByID(string id)
        {
            ProfitAdmEntities db = new ProfitAdmEntities();
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

        public List<saCliente> GetAllClients(bool full)
        {
			ProfitAdmEntities db = new ProfitAdmEntities();
			List<saCliente> clients;

			try
			{
				clients = db.saCliente.AsNoTracking().ToList();
			}
			catch (Exception ex)
			{
				clients = null;
				IncidentController.CreateIncident("ERROR BUSCANDO CLIENTES", ex);
			}

			return clients;
        }
    }
}