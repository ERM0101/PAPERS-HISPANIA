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
    
    public partial class Good
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Good()
        {
            this.CustomerOrderMovements = new HashSet<CustomerOrderMovement>();
            this.HistoCustomers = new HashSet<HistoCustomer>();
            this.HistoGoods = new HashSet<HistoGood>();
            this.HistoProviders = new HashSet<HistoProvider>();
            this.PriceRanges = new HashSet<PriceRange>();
            this.ProviderOrderMovements = new HashSet<ProviderOrderMovement>();
            this.WarehouseMovements = new HashSet<WarehouseMovement>();
        }
    
        public int Good_Id { get; set; }
        public string Good_Code { get; set; }
        public Nullable<decimal> Initial { get; set; }
        public Nullable<decimal> Initial_Fact { get; set; }
        public string Good_Description { get; set; }
        public string Cod_Fam { get; set; }
        public Nullable<decimal> Price_Cost { get; set; }
        public Nullable<decimal> Average_Price_Cost { get; set; }
        public Nullable<decimal> Previous_Average_Price_Cost { get; set; }
        public Nullable<decimal> Billing_Unit_Stocks { get; set; }
        public Nullable<decimal> Billing_Unit_Available { get; set; }
        public Nullable<decimal> Shipping_Unit_Stocks { get; set; }
        public Nullable<decimal> Shipping_Unit_Available { get; set; }
        public Nullable<decimal> Billing_Unit_Entrance { get; set; }
        public Nullable<decimal> Billing_Unit_Departure { get; set; }
        public Nullable<decimal> Shipping_Unit_Entrance { get; set; }
        public Nullable<decimal> Shipping_Unit_Departure { get; set; }
        public Nullable<decimal> Cumulative_Sales_Retail_Price_Month { get; set; }
        public Nullable<decimal> Cumulative_Sales_Retail_Price_Year { get; set; }
        public Nullable<decimal> Cumulative_Sales_Cost_Month { get; set; }
        public Nullable<decimal> Cumulative_Sales_Cost_Year { get; set; }
        public Nullable<decimal> Conversion_Factor { get; set; }
        public Nullable<decimal> Average_Billing_Unit { get; set; }
        public Nullable<int> Unit_Id { get; set; }
        public Nullable<int> Minimum { get; set; }
        public Nullable<decimal> Cumulative_Sales_Retail_Price_1 { get; set; }
        public Nullable<decimal> Cumulative_Sales_Retail_Price_2 { get; set; }
        public Nullable<decimal> Cumulative_Sales_Retail_Price_3 { get; set; }
        public Nullable<decimal> Cumulative_Sales_Retail_Price_4 { get; set; }
        public Nullable<decimal> Cumulative_Sales_Retail_Price_5 { get; set; }
        public Nullable<decimal> Cumulative_Sales_Retail_Price_6 { get; set; }
        public Nullable<decimal> Cumulative_Sales_Retail_Price_7 { get; set; }
        public Nullable<decimal> Cumulative_Sales_Retail_Price_8 { get; set; }
        public Nullable<decimal> Cumulative_Sales_Retail_Price_9 { get; set; }
        public Nullable<decimal> Cumulative_Sales_Retail_Price_10 { get; set; }
        public Nullable<decimal> Cumulative_Sales_Retail_Price_11 { get; set; }
        public Nullable<decimal> Cumulative_Sales_Retail_Price_12 { get; set; }
        public Nullable<decimal> Cumulative_Sales_Cost_1 { get; set; }
        public Nullable<decimal> Cumulative_Sales_Cost_2 { get; set; }
        public Nullable<decimal> Cumulative_Sales_Cost_3 { get; set; }
        public Nullable<decimal> Cumulative_Sales_Cost_4 { get; set; }
        public Nullable<decimal> Cumulative_Sales_Cost_5 { get; set; }
        public Nullable<decimal> Cumulative_Sales_Cost_6 { get; set; }
        public Nullable<decimal> Cumulative_Sales_Cost_7 { get; set; }
        public Nullable<decimal> Cumulative_Sales_Cost_8 { get; set; }
        public Nullable<decimal> Cumulative_Sales_Cost_9 { get; set; }
        public Nullable<decimal> Cumulative_Sales_Cost_10 { get; set; }
        public Nullable<decimal> Cumulative_Sales_Cost_11 { get; set; }
        public Nullable<decimal> Cumulative_Sales_Cost_12 { get; set; }
        public bool Revised { get; set; }
        public bool Canceled { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CustomerOrderMovement> CustomerOrderMovements { get; set; }
        public virtual Unit Unit { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistoCustomer> HistoCustomers { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistoGood> HistoGoods { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HistoProvider> HistoProviders { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PriceRange> PriceRanges { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProviderOrderMovement> ProviderOrderMovements { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<WarehouseMovement> WarehouseMovements { get; set; }
    }
}
