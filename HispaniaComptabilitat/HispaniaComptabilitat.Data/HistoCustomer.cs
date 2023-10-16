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
    
    public partial class HistoCustomer
    {
        public int HistoCustomer_Id { get; set; }
        public int Customer_Id { get; set; }
        public Nullable<int> Bill_Id { get; set; }
        public Nullable<decimal> Bill_Year { get; set; }
        public Nullable<System.DateTime> Bill_Date { get; set; }
        public Nullable<int> Bill_Serie_Id { get; set; }
        public Nullable<int> DeliveryNote_Id { get; set; }
        public Nullable<decimal> DeliveryNote_Year { get; set; }
        public Nullable<System.DateTime> DeliveryNote_Date { get; set; }
        public Nullable<int> Good_Id { get; set; }
        public string Good_Code { get; set; }
        public string Good_Description { get; set; }
        public Nullable<decimal> Shipping_Units { get; set; }
        public Nullable<decimal> Billing_Units { get; set; }
        public Nullable<decimal> Retail_Price { get; set; }
        public Nullable<decimal> Comission { get; set; }
        public int CustomerOrderMovement_Id { get; set; }
        public string Unit_Shipping_Definition { get; set; }
        public string Unit_Billing_Definition { get; set; }
    
        public virtual Bill Bill { get; set; }
        public virtual BillingSerie BillingSerie { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual DeliveryNote DeliveryNote { get; set; }
        public virtual Good Good { get; set; }
    }
}
