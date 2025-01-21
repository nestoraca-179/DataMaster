using DataMaster.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataMaster.Models
{
	public class Budget : ProfitAdmManager
	{
        public saCotizacionCliente GetBudgetByID(string id)
        {
            saCotizacionCliente budget;

            try
            {
                budget = db.saCotizacionCliente.AsNoTracking().Include("saCotizacionClienteReng").Include("saCliente")
                    .Include("saCondicionPago").Include("saVendedor").Single(i => i.doc_num == id);

                budget.saCliente.saCotizacionCliente = null;
                budget.saVendedor.saCotizacionCliente = null;
                budget.saCondicionPago.saCotizacionCliente = null;
                foreach (saCotizacionClienteReng reng in budget.saCotizacionClienteReng)
                {
                    reng.saCotizacionCliente = null;
                }
            }
            catch (Exception ex)
            {
                budget = null;
                IncidentController.CreateIncident("ERROR BUSCANDO COTIZACION " + id, ex);
            }

            return budget;
        }

        public List<saCotizacionCliente> GetAllBudgets(int number, bool free)
        {
            List<saCotizacionCliente> budgets = new List<saCotizacionCliente>();

            try
            {
                if (free)
                {
                    budgets = db.saCotizacionCliente.AsNoTracking().Include("saCliente").Where(o => o.status.Trim() == "0")
                        .OrderByDescending(i => i.fec_emis).ThenBy(i => i.doc_num).Take(number).ToList();
                }
                else
                {
                    budgets = db.saCotizacionCliente.AsNoTracking().Include("saCotizacionClienteReng").Include("saCliente").Include("saVendedor")
                        .Include("saCondicionPago").OrderByDescending(i => i.fec_emis).ThenBy(i => i.doc_num).Take(number).ToList();
                }

                foreach (saCotizacionCliente budget in budgets)
                {
                    if (!free)
                    {
                        budget.saVendedor.saCotizacionCliente = null;
                        budget.saCondicionPago.saCotizacionCliente = null;
                    }

                    budget.saCliente.saCotizacionCliente = null;
                    foreach (saCotizacionClienteReng reng in budget.saCotizacionClienteReng)
                    {
                        reng.saCotizacionCliente = null;
                    }
                }
            }
            catch (Exception ex)
            {
                budgets = null;
                IncidentController.CreateIncident("ERROR BUSCANDO COTIZACIONES", ex);
            }

            return budgets;
        }
    }
}