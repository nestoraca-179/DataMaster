//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DataMaster.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class saArtCompuestoReng
    {
        public string co_artc { get; set; }
        public int reng_num { get; set; }
        public string co_art { get; set; }
        public string co_uni { get; set; }
        public decimal total_art { get; set; }
        public string sco_uni { get; set; }
        public decimal stotal_art { get; set; }
        public string co_us_in { get; set; }
        public string co_sucu_in { get; set; }
        public System.DateTime fe_us_in { get; set; }
        public string co_us_mo { get; set; }
        public string co_sucu_mo { get; set; }
        public System.DateTime fe_us_mo { get; set; }
        public string revisado { get; set; }
        public string trasnfe { get; set; }
        public System.Guid rowguid { get; set; }
    
        public virtual saArtCompuesto saArtCompuesto { get; set; }
        public virtual saArtUnidad saArtUnidad { get; set; }
    }
}
