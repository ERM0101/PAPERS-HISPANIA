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
    
    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            this.AddressStores = new HashSet<AddressStore>();
            this.CustomerOrders = new HashSet<CustomerOrder>();
            this.HistoCumulativeCustomers = new HashSet<HistoCumulativeCustomer>();
            this.HistoCustomers = new HashSet<HistoCustomer>();
            this.RelatedCustomers = new HashSet<RelatedCustomer>();
            this.RelatedCustomers1 = new HashSet<RelatedCustomer>();
        }
    
        public int Customer_Id { get; set; }
        public string Customer_Alias { get; set; }
        public string Company_Name { get; set; }
        public string Company_Cif { get; set; }
        public string Company_Address { get; set; }
        public Nullable<int> Company_PostalCode_Id { get; set; }
        public string Company_Phone_1 { get; set; }
        public string Company_Phone_2 { get; set; }
        public string Company_Fax { get; set; }
        public string Company_MobilePhone { get; set; }
        public string Company_EMail { get; set; }
        public string Company_ContactPerson { get; set; }
        public string Company_TimeTable { get; set; }
        public string Company_NumProv { get; set; }
        public Nullable<int> DataBank_EffectType_Id { get; set; }
        public decimal DataBank_NumEffect { get; set; }
        public decimal DataBank_FirstExpirationData { get; set; }
        public decimal DataBank_ExpirationInterval { get; set; }
        public decimal DataBank_Payday_1 { get; set; }
        public decimal DataBank_Payday_2 { get; set; }
        public decimal DataBank_Payday_3 { get; set; }
        public string DataBank_Bank { get; set; }
        public string DataBank_BankAddress { get; set; }
        public string DataBank_IBAN_CountryCode { get; set; }
        public string DataBank_IBAN_BankCode { get; set; }
        public string DataBank_IBAN_OfficeCode { get; set; }
        public string DataBank_SerieCode { get; set; }
        public string DataBank_IBAN_CheckDigits { get; set; }
        public string DataBank_IBAN_AccountNumber { get; set; }
        public string DataBank_SubAccount { get; set; }
        public string BillingData_BillingType { get; set; }
        public Nullable<decimal> BillingData_Duplicate { get; set; }
        public Nullable<decimal> BillingData_COMDiscount { get; set; }
        public Nullable<decimal> BillingData_EarlyPaymentDiscount { get; set; }
        public Nullable<decimal> BillingData_ArtiDiscount { get; set; }
        public Nullable<decimal> BillingData_RiskGranted { get; set; }
        public Nullable<decimal> BillingData_CurrentRisk { get; set; }
        public Nullable<decimal> BillingData_Unpaid { get; set; }
        public Nullable<decimal> BillingData_NumUnpaid { get; set; }
        public Nullable<int> BillingData_Agent_Id { get; set; }
        public Nullable<int> BillingData_IVAType_Id { get; set; }
        public Nullable<System.DateTime> BillingData_Register { get; set; }
        public string Several_Remarks { get; set; }
        public bool BillingData_Valued { get; set; }
        public Nullable<int> BillingData_SendType_Id { get; set; }
        public Nullable<decimal> SeveralData_Acum_1 { get; set; }
        public Nullable<decimal> SeveralData_Acum_2 { get; set; }
        public Nullable<decimal> SeveralData_Acum_3 { get; set; }
        public Nullable<decimal> SeveralData_Acum_4 { get; set; }
        public Nullable<decimal> SeveralData_Acum_5 { get; set; }
        public Nullable<decimal> SeveralData_Acum_6 { get; set; }
        public Nullable<decimal> SeveralData_Acum_7 { get; set; }
        public Nullable<decimal> SeveralData_Acum_8 { get; set; }
        public Nullable<decimal> SeveralData_Acum_9 { get; set; }
        public Nullable<decimal> SeveralData_Acum_10 { get; set; }
        public Nullable<decimal> SeveralData_Acum_11 { get; set; }
        public Nullable<decimal> SeveralData_Acum_12 { get; set; }
        public bool QueryData_Active { get; set; }
        public bool QueryData_Print { get; set; }
        public bool Canceled { get; set; }
        public string Company_Email2 { get; set; }
        public string Company_Email3 { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AddressStore> AddressStores { get; set; }
        public virtual Agent Agent { get; set; }
        public virtual EffectType EffectType { get; set; }
        public virtual IVAType IVAType { get; set; }
        public virtual PostalCode PostalCode { get; set; }
        public virtual SendType SendType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerOrder> CustomerOrders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistoCumulativeCustomer> HistoCumulativeCustomers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistoCustomer> HistoCustomers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RelatedCustomer> RelatedCustomers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RelatedCustomer> RelatedCustomers1 { get; set; }
    }
}
