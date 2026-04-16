using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DataMaster.Models
{
	public class BalanceDetail : ProfitAdmManager
	{
		public string code { get; set; }
		public string name { get; set; }
		public bool isBox { get; set; }
		public decimal initBalance { get; set; }
		public decimal totalIncomes { get; set; }
		public decimal totalOutcomes { get; set; }
		public decimal totalBalance { get; set; }

		public List<BalanceDetail> GetDetailsBankAccountsAndBoxes(DateTime date)
		{
			List<BalanceDetail> details = new List<BalanceDetail>();

			List<saCaja> boxes = db.saCaja.AsNoTracking().Where(b => !b.inactivo).ToList();
			List<saCuentaBancaria> banks = db.saCuentaBancaria.AsNoTracking().Where(b => !b.inactivo).ToList();
			List<saMovimientoCaja> boxTransactions = db.saMovimientoCaja.AsNoTracking()
				.Where(m => DbFunctions.TruncateTime(m.fecha) == DbFunctions.TruncateTime(date))
				.Where(m => !m.anulado).ToList();
			List<saMovimientoBanco> bankTransactions = db.saMovimientoBanco.AsNoTracking()
				.Where(m => DbFunctions.TruncateTime(m.fecha) == DbFunctions.TruncateTime(date))
				.Where(m => !m.anulado).ToList();

			foreach (saCaja box in boxes)
			{
				if (!details.Any(d => d.code == box.cod_caja.Trim() && d.isBox))
				{
					BalanceDetail detail = new BalanceDetail();
					List<saMovimientoCaja> boxMoves = boxTransactions.Where(mc => mc.cod_caja == box.cod_caja).ToList();

					detail.code = box.cod_caja;
					detail.name = db.saCaja.AsNoTracking().SingleOrDefault(c => c.cod_caja == box.cod_caja).descrip;
					detail.isBox = true;
					detail.initBalance = SaldoCajaAUnaFecha(box.cod_caja, DateTime.Now.AddDays(-1));
					detail.totalIncomes = boxMoves.Where(mc => mc.tipo_mov.Trim() == "I").Select(mc => mc.monto_h).Sum();
					detail.totalOutcomes = boxMoves.Where(mc => mc.tipo_mov.Trim() == "E").Select(mc => mc.monto_d).Sum();
					detail.totalBalance = detail.totalIncomes - detail.totalOutcomes;
					
					details.Add(detail);
				}
			}

			foreach (saCuentaBancaria bank in banks)
			{
				if (!details.Any(d => d.code == bank.cod_cta.Trim() && !d.isBox))
				{
					BalanceDetail detail = new BalanceDetail();
					List<saMovimientoBanco> bankMoves = bankTransactions.Where(mb => mb.cod_cta == bank.cod_cta).ToList();

					detail.code = bank.cod_cta;
					detail.name = db.saCuentaBancaria.AsNoTracking().SingleOrDefault(cb => cb.cod_cta == bank.cod_cta).num_cta;
					detail.isBox = false;
					detail.initBalance = SaldoBancoAUnaFecha4(bank.cod_cta, DateTime.Now.AddDays(-1), "2");
					detail.totalIncomes = bankMoves.Where(mc => mc.monto_h > 0 && mc.monto_d == 0).Select(mc => mc.monto_h).Sum();
					detail.totalOutcomes = bankMoves.Where(mc => mc.monto_h == 0 && mc.monto_d > 0).Select(mc => mc.monto_d).Sum();
					detail.totalBalance = detail.totalIncomes - detail.totalOutcomes;

					details.Add(detail);
				}
			}

			return details;
		}
	}
}