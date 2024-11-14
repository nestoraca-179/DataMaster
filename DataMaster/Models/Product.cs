﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DataMaster.Models
{
	public class Product : ProfitAdmManager
    {
        public saArticulo GetArtByID(string id)
        {
            return db.saArticulo.AsNoTracking().Single(c => c.co_art == id);
        }

        public List<saArticulo> GetAllArts()
		{
            return db.saArticulo.AsNoTracking().ToList();
        }

		public List<saArticulo> GetMostProducts(DateTime fec_d, DateTime fec_h, int number, bool selling, string sucur)
		{
			List<saArticulo> prods = new List<saArticulo>();

			if (selling) // ARTICULOS VENTAS
			{
				var sp = db.RepTotalVentaxArticulo(fec_d, fec_h, null, null, null, null, null, null, null, null, null, sucur, null, null, null);
				var enumerator = sp.GetEnumerator();

				while (enumerator.MoveNext())
				{
					saArticulo articulo = new saArticulo();

					articulo.co_art = enumerator.Current.co_art.Trim();
					articulo.art_des = enumerator.Current.art_des.Trim();
					articulo.campo1 = (enumerator.Current.total_art - enumerator.Current.total_dev).ToString();

					prods.Add(articulo);
				}
			}
			else // ARTICULOS COMPRAS
			{
				var sp = db.RepTotalCompraxArticulo(fec_d, fec_h, null, null, null, null, null, null, null, null, null, sucur, null, null, null);
				var enumerator = sp.GetEnumerator();

				while (enumerator.MoveNext())
				{
					saArticulo articulo = new saArticulo();

					articulo.co_art = enumerator.Current.co_art.Trim();
					articulo.art_des = enumerator.Current.art_des.Trim();
					articulo.campo1 = (enumerator.Current.total_art - enumerator.Current.total_dev).ToString();

					prods.Add(articulo);
				}
			}

			prods = (from a in prods
					 group decimal.Parse(a.campo1) by (a.co_art, a.art_des) into g
					 select new saArticulo
					 {
						 co_art = g.Key.co_art,
						 art_des = g.Key.art_des,
						 campo1 = Math.Round(g.Sum(), 2).ToString()

					 }).OrderByDescending(x => double.Parse(x.campo1)).ToList();

			if (prods.Count > number)
				prods.RemoveRange(number, prods.Count - number);

			return prods;
		}
	}
}