using DataMaster.Controllers;
using System.Data.Entity.Core.EntityClient;
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
	}
}