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
    
    public partial class saLoteSalida
    {
        public System.Guid rowguid_reng { get; set; }
        public int reng_num { get; set; }
        public string tipo_doc { get; set; }
        public string co_art { get; set; }
        public string co_alma { get; set; }
        public string numero_lote { get; set; }
        public System.Guid Rowguid_Lote { get; set; }
        public decimal cantidad { get; set; }
        public decimal precio { get; set; }
        public string co_us_in { get; set; }
        public string co_sucu_in { get; set; }
        public System.DateTime fe_us_in { get; set; }
        public string co_us_mo { get; set; }
        public string co_sucu_mo { get; set; }
        public System.DateTime fe_us_mo { get; set; }
        public string revisado { get; set; }
        public string trasnfe { get; set; }
        public byte[] validador { get; set; }
        public System.Guid rowguid { get; set; }
    
        public virtual saAlmacen saAlmacen { get; set; }
        public virtual saArticulo saArticulo { get; set; }
    }
}
