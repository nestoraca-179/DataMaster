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
    
    public partial class saComisionGeneracion
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public saComisionGeneracion()
        {
            this.saComisionResultado = new HashSet<saComisionResultado>();
        }
    
        public string co_generacion { get; set; }
        public System.DateTime fecha { get; set; }
        public string co_comi { get; set; }
        public string comentario { get; set; }
        public System.DateTime fecha_desde { get; set; }
        public System.DateTime fecha_hasta { get; set; }
        public string co_ven_desde { get; set; }
        public string co_ven_hasta { get; set; }
        public string tipo_ven_desde { get; set; }
        public string tipo_ven_hasta { get; set; }
        public string co_art_desde { get; set; }
        public string co_art_hasta { get; set; }
        public string co_cat_desde { get; set; }
        public string co_cat_hasta { get; set; }
        public string co_lin_desde { get; set; }
        public string co_lin_hasta { get; set; }
        public string p_adicional { get; set; }
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
    
        public virtual saArticulo saArticulo { get; set; }
        public virtual saArticulo saArticulo1 { get; set; }
        public virtual saCatArticulo saCatArticulo { get; set; }
        public virtual saCatArticulo saCatArticulo1 { get; set; }
        public virtual saComisionTipo saComisionTipo { get; set; }
        public virtual saLineaArticulo saLineaArticulo { get; set; }
        public virtual saLineaArticulo saLineaArticulo1 { get; set; }
        public virtual saVendedor saVendedor { get; set; }
        public virtual saVendedor saVendedor1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saComisionResultado> saComisionResultado { get; set; }
    }
}
