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
    
    public partial class saDescProntoPago
    {
        public string co_desc { get; set; }
        public string tip_cli { get; set; }
        public string des_des { get; set; }
        public Nullable<decimal> hasta1 { get; set; }
        public Nullable<decimal> hasta2 { get; set; }
        public Nullable<decimal> hasta3 { get; set; }
        public Nullable<decimal> hasta4 { get; set; }
        public Nullable<decimal> hasta5 { get; set; }
        public Nullable<decimal> porc1 { get; set; }
        public Nullable<decimal> porc2 { get; set; }
        public Nullable<decimal> porc3 { get; set; }
        public Nullable<decimal> porc4 { get; set; }
        public Nullable<decimal> porc5 { get; set; }
        public Nullable<decimal> porc6 { get; set; }
        public string campo1 { get; set; }
        public string campo2 { get; set; }
        public string campo3 { get; set; }
        public string campo4 { get; set; }
        public string campo5 { get; set; }
        public string campo6 { get; set; }
        public string campo7 { get; set; }
        public string campo8 { get; set; }
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
    
        public virtual saTipoCliente saTipoCliente { get; set; }
    }
}
