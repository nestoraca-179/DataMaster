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
    
    public partial class saSubLinea
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public saSubLinea()
        {
            this.saArtCaracteristicaMov = new HashSet<saArtCaracteristicaMov>();
            this.saArtCaracteristicaMov1 = new HashSet<saArtCaracteristicaMov>();
            this.saArtCaracteristicaMov2 = new HashSet<saArtCaracteristicaMov>();
            this.saArtCaracteristicaMov3 = new HashSet<saArtCaracteristicaMov>();
            this.saArtCaracteristicaMov4 = new HashSet<saArtCaracteristicaMov>();
            this.saArticulo = new HashSet<saArticulo>();
            this.saArtCrearAut = new HashSet<saArtCrearAut>();
            this.saArtCrearAut1 = new HashSet<saArtCrearAut>();
        }
    
        public string co_lin { get; set; }
        public string co_subl { get; set; }
        public string subl_des { get; set; }
        public string co_imun { get; set; }
        public string co_reten { get; set; }
        public string i_subl_des { get; set; }
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
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saArtCaracteristicaMov> saArtCaracteristicaMov { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saArtCaracteristicaMov> saArtCaracteristicaMov1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saArtCaracteristicaMov> saArtCaracteristicaMov2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saArtCaracteristicaMov> saArtCaracteristicaMov3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saArtCaracteristicaMov> saArtCaracteristicaMov4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saArticulo> saArticulo { get; set; }
        public virtual saConISLR saConISLR { get; set; }
        public virtual saLineaArticulo saLineaArticulo { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saArtCrearAut> saArtCrearAut { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saArtCrearAut> saArtCrearAut1 { get; set; }
    }
}
