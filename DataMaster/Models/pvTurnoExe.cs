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
    
    public partial class pvTurnoExe
    {
        public string num_turno { get; set; }
        public string co_turno { get; set; }
        public string cod_caja { get; set; }
        public string user_caj { get; set; }
        public string user_sup { get; set; }
        public System.DateTime fecha_ini { get; set; }
        public System.DateTime fecha_fin { get; set; }
        public string status { get; set; }
        public bool restringe { get; set; }
        public decimal saldo { get; set; }
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
        public string cod_caja2 { get; set; }
        public string cod_caja3 { get; set; }
        public decimal saldo2 { get; set; }
        public decimal saldo3 { get; set; }
    
        public virtual pvTurno pvTurno { get; set; }
        public virtual saCaja saCaja { get; set; }
        public virtual saCaja saCaja1 { get; set; }
        public virtual saCaja saCaja2 { get; set; }
    }
}
