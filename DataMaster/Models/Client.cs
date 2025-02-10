using System;
using System.Linq;
using System.Collections.Generic;
using DataMaster.Controllers;
using System.Globalization;

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

		public List<saCliente> GetMostActiveClients(DateTime fec_d, DateTime fec_h, int number, string sucur)
		{
			List<saCliente> clientes = new List<saCliente>();

			var sp = db.RepClienteMasVenta(fec_d, fec_h, null, null, null, null, null, null, null, number, sucur, null, null, null);
			var enumerator = sp.GetEnumerator();

			while (enumerator.MoveNext())
			{
				saCliente cliente = new saCliente();

				cliente.co_cli = enumerator.Current.co_cli.Trim();
				cliente.cli_des = enumerator.Current.cli_des.Trim();
				cliente.campo1 = Convert.ToDouble(enumerator.Current.Venta).ToString("N2", CultureInfo.GetCultureInfo("es-ES"));
				cliente.campo2 = Math.Round(Convert.ToDouble((enumerator.Current.Venta * 100) / enumerator.Current.Venta_total), 2).ToString("N2", CultureInfo.GetCultureInfo("es-ES"));

				clientes.Add(cliente);
			}

			return clientes;
		}

		public List<saCliente> GetMostActiveClientsWithNotes(DateTime fec_d, DateTime fec_h, int number, string sucur)
		{
			List<saCliente> clientes = new List<saCliente>();
			List<saCliente> clientes_temp = new List<saCliente>();

			var sp = db.RepTotalNotaEntregaxCliente(fec_d, fec_h, null, null, null, null, null, null, null, sucur, null, null, null, null);
			var enumerator = sp.GetEnumerator();

			while (enumerator.MoveNext())
			{
				saCliente cliente = new saCliente();

				cliente.co_cli = enumerator.Current.co_cli.Trim();
				cliente.cli_des = enumerator.Current.cli_des.Trim();
				cliente.desc_glob = enumerator.Current.anulado ? 0 : enumerator.Current.total_neto; // USADO PARA EL TOTAL

				clientes_temp.Add(cliente);
			}

			decimal total = clientes_temp.Select(c => c.desc_glob).Sum();
			foreach (saCliente cl in clientes_temp.OrderBy(c => c.co_cli))
			{
				if (!clientes.Any(c => c.co_cli == cl.co_cli))
				{
					saCliente new_cliente = new saCliente();
					decimal total_c = clientes_temp.Where(c => c.co_cli == cl.co_cli).Select(c => c.desc_glob).Sum();

					new_cliente.co_cli = cl.co_cli;
					new_cliente.cli_des = cl.cli_des;
					new_cliente.campo1 = total_c.ToString("N2", CultureInfo.GetCultureInfo("es-ES"));
					new_cliente.campo2 = Math.Round((total_c * 100) / total, 2).ToString("N2", CultureInfo.GetCultureInfo("es-ES"));
					new_cliente.desc_glob = total_c;

					clientes.Add(new_cliente);
				}
			}

			return clientes.OrderByDescending(c => c.desc_glob).ToList();
		}

		public List<saCliente> GetMostMorousClients(int number)
		{
			List<saCliente> clients = new List<saCliente>();

			DateTime fec_h = DateTime.Now;
			DateTime fec_d = fec_h.AddDays(-(fec_h.Day - 1));

			var sp = db.RepEstadoCuentaCli(fec_d, fec_h, null, null, null, null, null, null, null, null, null, null, null, null, null);
			var enumerator = sp.GetEnumerator();

			while (enumerator.MoveNext())
			{
				saCliente client = new saCliente();

				client.co_cli = enumerator.Current.co_prov;
				client.cli_des = enumerator.Current.prov_des;
				client.campo1 = ((enumerator.Current.tot_debe ?? 0) - (enumerator.Current.tot_haber ?? 0)).ToString();

				clients.Add(client);
			}

			clients = (from c in clients
					   group c.campo1 by (c.co_cli, c.cli_des) into g
					   select new saCliente
					   {
						   co_cli = g.Key.co_cli,
						   cli_des = g.Key.co_cli + " - " + g.Key.cli_des,
						   campo1 = Math.Round(g.Select(x => double.Parse(x)).Sum(), 2).ToString()

					   }).OrderByDescending(x => double.Parse(x.campo1)).ToList();

			if (clients.Count > number)
				clients.RemoveRange(number, clients.Count - number);

			return clients;
		}
	}
}