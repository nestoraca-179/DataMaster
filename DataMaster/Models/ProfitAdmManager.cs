using DataMaster.Controllers;
using System;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DataMaster.Models
{
	public class ProfitAdmManager
	{
		// CADENA DE CONEXION
		private static string connect;

		// ENTIDAD PARA EF
		public static EntityConnectionStringBuilder entity;

		// CONTEXTO EF
		public static ProfitAdmEntities db;

		public ProfitAdmManager()
		{
			connect = HttpContext.Current.Session["CONNECT"].ToString();
			entity = EntityController.GetEntity(connect);
			db = new ProfitAdmEntities(entity.ToString());
		}

		public string GetNextConsec(string sucur, string serie)
		{
			string num = "";

			var sp = db.pConsecutivoProximo(sucur, serie).GetEnumerator();
			if (sp.MoveNext())
				num = sp.Current;

			sp.Dispose();
			return num;
		}

		public static decimal SaldoCajaAUnaFecha(string codCaja, DateTime fecha)
		{
			return db.Database.SqlQuery<decimal>(
				"SELECT dbo.SaldoCajaAUnaFecha(@codCaja, @fecha)",
				new SqlParameter("@codCaja", codCaja),
				new SqlParameter("@fecha", fecha)
			).FirstOrDefault();
		}

		public static decimal SaldoBancoAUnaFecha4(string codCta, DateTime fecha, string tipoSaldo)
		{
			return db.Database.SqlQuery<decimal>(
				"SELECT dbo.SaldoBancoAUnaFecha4(@codCta, @fecha, @tipoSaldo)",
				new SqlParameter("@codCta", codCta),
				new SqlParameter("@fecha", fecha),
				new SqlParameter("@tipoSaldo", tipoSaldo)
			).FirstOrDefault();
		}
	}
}