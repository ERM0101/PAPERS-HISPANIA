//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HispaniaComptabilitat.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Receipt
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Receipt()
        {
            this.BadDebts = new HashSet<BadDebt>();
        }
    
        public int Receipt_Id { get; set; }
        public int Bill_Id { get; set; }
        public decimal Bill_Year { get; set; }
        public int Bill_Serie_Id { get; set; }
        public Nullable<System.DateTime> Expiration_Date { get; set; }
        public Nullable<decimal> Amount { get; set; }
        public Nullable<bool> Paid { get; set; }
        public Nullable<bool> Returned { get; set; }
        public Nullable<bool> Expired { get; set; }
        public Nullable<bool> Print { get; set; }
        public Nullable<bool> SendByEMail { get; set; }
        public string FileNamePDF { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BadDebt> BadDebts { get; set; }
        public virtual Bill Bill { get; set; }
        public virtual BillingSerie BillingSerie { get; set; }
    }
}
