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
    
    public partial class saPlantillaCompraReqRelacion
    {
        public System.Guid rowguid_reng_req { get; set; }
        public System.Guid rowguid_reng_imp { get; set; }
        public string co_tipo_doc { get; set; }
        public bool entregado { get; set; }
        public Nullable<System.DateTime> fecha_real_entrega { get; set; }
        public Nullable<decimal> total_art { get; set; }
        public string comentario { get; set; }
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
    }
}
