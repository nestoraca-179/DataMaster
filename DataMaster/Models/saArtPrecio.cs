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
    
    public partial class saArtPrecio
    {
        public string co_art { get; set; }
        public string co_precio { get; set; }
        public string co_alma_calculado { get; set; }
        public System.DateTime desde { get; set; }
        public Nullable<System.DateTime> hasta { get; set; }
        public string co_alma { get; set; }
        public decimal monto { get; set; }
        public Nullable<decimal> montoadi1 { get; set; }
        public Nullable<decimal> montoadi2 { get; set; }
        public Nullable<decimal> montoadi3 { get; set; }
        public Nullable<decimal> montoadi4 { get; set; }
        public Nullable<decimal> montoadi5 { get; set; }
        public bool precioOm { get; set; }
        public string co_us_in { get; set; }
        public string co_sucu_in { get; set; }
        public System.DateTime fe_us_in { get; set; }
        public string co_us_mo { get; set; }
        public string co_sucu_mo { get; set; }
        public System.DateTime fe_us_mo { get; set; }
        public string revisado { get; set; }
        public string trasnfe { get; set; }
        public byte[] validador { get; set; }
        public string co_mone { get; set; }
        public bool Inactivo { get; set; }
        public System.Guid rowguid { get; set; }
    
        public virtual saAlmacen saAlmacen { get; set; }
        public virtual saArticulo saArticulo { get; set; }
        public virtual saTipoPrecio saTipoPrecio { get; set; }
    }
}
