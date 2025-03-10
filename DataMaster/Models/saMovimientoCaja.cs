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
    
    public partial class saMovimientoCaja
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public saMovimientoCaja()
        {
            this.saCobroTPReng = new HashSet<saCobroTPReng>();
            this.saDepositoBancoReng = new HashSet<saDepositoBancoReng>();
            this.saDepositoBancoReng1 = new HashSet<saDepositoBancoReng>();
            this.saDevolucionCliente = new HashSet<saDevolucionCliente>();
            this.saDevolucionProveedor = new HashSet<saDevolucionProveedor>();
            this.saMovimientoCaja1 = new HashSet<saMovimientoCaja>();
            this.saOrdenPago = new HashSet<saOrdenPago>();
            this.saPagoTPReng = new HashSet<saPagoTPReng>();
        }
    
        public string mov_num { get; set; }
        public System.DateTime fecha { get; set; }
        public string descrip { get; set; }
        public string cod_caja { get; set; }
        public string co_ban { get; set; }
        public string co_tar { get; set; }
        public string co_vale { get; set; }
        public string co_cta_ingr_egr { get; set; }
        public decimal tasa { get; set; }
        public string tipo_mov { get; set; }
        public string forma_pag { get; set; }
        public string num_pago { get; set; }
        public bool saldo_ini { get; set; }
        public decimal monto_d { get; set; }
        public decimal monto_h { get; set; }
        public string dep_num { get; set; }
        public string origen { get; set; }
        public string doc_num { get; set; }
        public bool anulado { get; set; }
        public bool depositado { get; set; }
        public bool transferido { get; set; }
        public string mov_nro { get; set; }
        public Nullable<decimal> aux01 { get; set; }
        public string aux02 { get; set; }
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
        public string dis_cen { get; set; }
        public System.DateTime fecha_che { get; set; }
    
        public virtual pvValeAlimentacion pvValeAlimentacion { get; set; }
        public virtual saBanco saBanco { get; set; }
        public virtual saCaja saCaja { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saCobroTPReng> saCobroTPReng { get; set; }
        public virtual saCuentaIngEgr saCuentaIngEgr { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saDepositoBancoReng> saDepositoBancoReng { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saDepositoBancoReng> saDepositoBancoReng1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saDevolucionCliente> saDevolucionCliente { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saDevolucionProveedor> saDevolucionProveedor { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saMovimientoCaja> saMovimientoCaja1 { get; set; }
        public virtual saMovimientoCaja saMovimientoCaja2 { get; set; }
        public virtual saTarjetaCredito saTarjetaCredito { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saOrdenPago> saOrdenPago { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<saPagoTPReng> saPagoTPReng { get; set; }
    }
}
