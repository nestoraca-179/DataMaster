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
    
    public partial class saLineaArticulo
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public saLineaArticulo()
        {
            this.saArtCaracteristica = new HashSet<saArtCaracteristica>();
            this.saArtCaracteristica1 = new HashSet<saArtCaracteristica>();
            this.saArtCaracteristica2 = new HashSet<saArtCaracteristica>();
            this.saArtCaracteristica3 = new HashSet<saArtCaracteristica>();
            this.saArtCaracteristica4 = new HashSet<saArtCaracteristica>();
            this.saComisionGeneracion = new HashSet<saComisionGeneracion>();
            this.saComisionGeneracion1 = new HashSet<saComisionGeneracion>();
            this.saComisionPrecioLinea = new HashSet<saComisionPrecioLinea>();
            this.saComisionRentabLinea = new HashSet<saComisionRentabLinea>();
            this.saDescLinea = new HashSet<saDescLinea>();
            this.saSubLinea = new HashSet<saSubLinea>();
        }
    
        public string co_lin { get; set; }
        public string lin_des { get; set; }
        public string dis_cen { get; set; }
        public string co_imun { get; set; }
        public string co_reten { get; set; }
        public decimal comi_lin { get; set; }
        public decimal comi_lin2 { get; set; }
        public string i_lin_des { get; set; }
        public bool va { get; set; }
        public bool movil { get; set; }
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
        public Nullable<System.DateTime> feccom { get; set; }
        public Nullable<int> numcom { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saArtCaracteristica> saArtCaracteristica { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saArtCaracteristica> saArtCaracteristica1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saArtCaracteristica> saArtCaracteristica2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saArtCaracteristica> saArtCaracteristica3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saArtCaracteristica> saArtCaracteristica4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saComisionGeneracion> saComisionGeneracion { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saComisionGeneracion> saComisionGeneracion1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saComisionPrecioLinea> saComisionPrecioLinea { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saComisionRentabLinea> saComisionRentabLinea { get; set; }
        public virtual saConISLR saConISLR { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saDescLinea> saDescLinea { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saSubLinea> saSubLinea { get; set; }
    }
}
