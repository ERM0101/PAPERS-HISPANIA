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
    
    public partial class CustomerOrder
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public CustomerOrder()
        {
            this.CustomerOrderMovements = new HashSet<CustomerOrderMovement>();
        }
    
        public int CustomerOrder_Id { get; set; }
        public int Customer_Id { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> PostalCode_Id { get; set; }
        public Nullable<int> SendType_Id { get; set; }
        public Nullable<int> EffectType_Id { get; set; }
        public decimal IVAPercent { get; set; }
        public decimal SurchargePercent { get; set; }
        public Nullable<int> Bill_Id { get; set; }
        public Nullable<decimal> Bill_Year { get; set; }
        public Nullable<int> Bill_Serie_Id { get; set; }
        public Nullable<System.DateTime> Bill_Date { get; set; }
        public Nullable<int> DeliveryNote_Id { get; set; }
        public Nullable<decimal> DeliveryNote_Year { get; set; }
        public Nullable<System.DateTime> DeliveryNote_Date { get; set; }
        public string Address { get; set; }
        public decimal NumEffect { get; set; }
        public decimal DataBank_ExpirationDays { get; set; }
        public decimal DataBank_ExpirationInterval { get; set; }
        public decimal DataBank_Paydays1 { get; set; }
        public decimal DataBank_Paydays2 { get; set; }
        public decimal DataBank_Paydays3 { get; set; }
        public string DataBank_Bank { get; set; }
        public string DataBank_BankAddress { get; set; }
        public string DataBank_IBAN_CountryCode { get; set; }
        public string DataBank_IBAN_BankCode { get; set; }
        public string DataBank_IBAN_OfficeCode { get; set; }
        public string DataBank_IBAN_CheckDigits { get; set; }
        public string DataBank_IBAN_AccountNumber { get; set; }
        public Nullable<decimal> BillingData_EarlyPaymentDiscount { get; set; }
        public Nullable<int> BillingData_Agent_Id { get; set; }
        public bool According { get; set; }
        public bool Valued { get; set; }
        public string Remarks { get; set; }
        public bool Transfer { get; set; }
        public bool Print { get; set; }
        public bool SendByEMail { get; set; }
        public bool Historic { get; set; }
        public bool Select_Bill { get; set; }
        public bool Expiration { get; set; }
        public bool Daily { get; set; }
        public Nullable<System.DateTime> Daily_Dates { get; set; }
        public bool Print_CustomerOrder { get; set; }
        public bool SendByEmail_CustomerOrder { get; set; }
        public decimal TotalAmount { get; set; }
        public string TimeTable { get; set; }
        public Nullable<bool> PrevisioLliurament { get; set; }
        public Nullable<System.DateTime> PrevisioLliuramentData { get; set; }
    
        public virtual Agent Agent { get; set; }
        public virtual Bill Bill { get; set; }
        public virtual BillingSerie BillingSerie { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual DeliveryNote DeliveryNote { get; set; }
        public virtual EffectType EffectType { get; set; }
        public virtual PostalCode PostalCode { get; set; }
        public virtual SendType SendType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerOrderMovement> CustomerOrderMovements { get; set; }
    }
}
