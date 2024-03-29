//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HispaniaComptabilitat.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class DeliveryNote
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DeliveryNote()
        {
            this.CustomerOrders = new HashSet<CustomerOrder>();
            this.HistoCustomers = new HashSet<HistoCustomer>();
            this.HistoProviders = new HashSet<HistoProvider>();
            this.ProviderOrders = new HashSet<ProviderOrder>();
        }
    
        public int DeliveryNote_Id { get; set; }
        public decimal Year { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string FileNamePDF { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerOrder> CustomerOrders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistoCustomer> HistoCustomers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistoProvider> HistoProviders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProviderOrder> ProviderOrders { get; set; }
    }
}
