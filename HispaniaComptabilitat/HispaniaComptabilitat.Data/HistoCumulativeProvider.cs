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
    
    public partial class HistoCumulativeProvider
    {
        public int Histo_Id { get; set; }
        public int Provider_Id { get; set; }
        public Nullable<decimal> Data_Year { get; set; }
        public Nullable<decimal> Cumulative_Sales_1 { get; set; }
        public Nullable<decimal> Cumulative_Sales_2 { get; set; }
        public Nullable<decimal> Cumulative_Sales_3 { get; set; }
        public Nullable<decimal> Cumulative_Sales_4 { get; set; }
        public Nullable<decimal> Cumulative_Sales_5 { get; set; }
        public Nullable<decimal> Cumulative_Sales_6 { get; set; }
        public Nullable<decimal> Cumulative_Sales_7 { get; set; }
        public Nullable<decimal> Cumulative_Sales_8 { get; set; }
        public Nullable<decimal> Cumulative_Sales_9 { get; set; }
        public Nullable<decimal> Cumulative_Sales_10 { get; set; }
        public Nullable<decimal> Cumulative_Sales_11 { get; set; }
        public Nullable<decimal> Cumulative_Sales_12 { get; set; }
    
        public virtual Provider Provider { get; set; }
    }
}
