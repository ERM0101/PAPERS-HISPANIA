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
    
    public partial class CustomerSale
    {
        public int Customer_Id { get; set; }
        public string Company_Name { get; set; }
        public string Company_Cif { get; set; }
        public string Company_Address { get; set; }
        public string Postal_Code { get; set; }
        public string City { get; set; }
        public string Company_Phone_1 { get; set; }
        public string Company_Phone_2 { get; set; }
        public string Company_EMail { get; set; }
        public Nullable<decimal> Customer_Sales_Acum { get; set; }
    }
}
