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
    
    public partial class Usuario
    {
        public int ID { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string des_usuario { get; set; }
        public string email { get; set; }
        public bool activo { get; set; }
        public bool admin { get; set; }
        public System.DateTime fec_camb { get; set; }
        public string co_us_in { get; set; }
        public System.DateTime fe_us_in { get; set; }
        public string co_us_mo { get; set; }
        public System.DateTime fe_us_mo { get; set; }
        public bool seller { get; set; }
        public Nullable<int> id_emp { get; set; }
    }
}
