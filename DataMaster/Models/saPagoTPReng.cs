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
    
    public partial class saPagoTPReng
    {
        public int reng_num { get; set; }
        public string cob_num { get; set; }
        public string forma_pag { get; set; }
        public string cod_cta { get; set; }
        public string cod_caja { get; set; }
        public string mov_num_c { get; set; }
        public string mov_num_b { get; set; }
        public string num_doc { get; set; }
        public bool devuelto { get; set; }
        public decimal mont_doc { get; set; }
        public System.DateTime fecha_che { get; set; }
        public string co_sucu_in { get; set; }
        public string co_us_in { get; set; }
        public System.DateTime fe_us_in { get; set; }
        public string co_sucu_mo { get; set; }
        public string co_us_mo { get; set; }
        public System.DateTime fe_us_mo { get; set; }
        public string trasnfe { get; set; }
        public string revisado { get; set; }
        public System.Guid rowguid { get; set; }
    
        public virtual saCaja saCaja { get; set; }
        public virtual saCuentaBancaria saCuentaBancaria { get; set; }
        public virtual saMovimientoBanco saMovimientoBanco { get; set; }
        public virtual saMovimientoCaja saMovimientoCaja { get; set; }
        public virtual saPago saPago { get; set; }
    }
}
