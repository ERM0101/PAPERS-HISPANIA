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
    
    public partial class StockTaking
    {
        public string Good_Code { get; set; }
        public string Familia { get; set; }
        public bool Canceled { get; set; }
        public string Good_Description { get; set; }
        public Nullable<int> UnitType { get; set; }
        public Nullable<decimal> Price_Cost { get; set; }
        public Nullable<decimal> Average_Price_Cost { get; set; }
        public Nullable<decimal> Shipping_Unit_Stocks { get; set; }
        public Nullable<decimal> Billing_Unit_Stocks { get; set; }
        public Nullable<decimal> Expression_1 { get; set; }
        public Nullable<int> Minimum { get; set; }
        public Nullable<decimal> Shipping_Unit_Entrance { get; set; }
        public Nullable<decimal> Billing_Unit_Entrance { get; set; }
        public Nullable<decimal> Shipping_Unit_Departure { get; set; }
        public Nullable<decimal> Billing_Unit_Departure { get; set; }
    }
}
