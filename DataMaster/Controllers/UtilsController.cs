using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace DataMaster.Controllers
{
	public class UtilsController
	{
		public DateTime FormatDate(string date)
		{
			int anio = int.Parse(date.Split('-')[2]);
			int mes = int.Parse(date.Split('-')[1]);
			int dia = int.Parse(date.Split('-')[0]);

			DateTime fecha = new DateTime(anio, mes, dia);

			return fecha;
		}
	}
}