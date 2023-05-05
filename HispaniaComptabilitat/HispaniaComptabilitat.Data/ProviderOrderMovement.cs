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
    
    public partial class ProviderOrderMovement
    {
        public int ProviderOrderMovement_Id { get; set; }
        public Nullable<int> ProviderOrder_Id { get; set; }
        public Nullable<int> Good_Id { get; set; }
        public string Description { get; set; }
        public Nullable<decimal> Unit_Shipping { get; set; }
        public Nullable<decimal> Unit_Billing { get; set; }
        public Nullable<decimal> RetailPrice { get; set; }
        public Nullable<decimal> Comission { get; set; }
        public string Remark { get; set; }
        public bool According { get; set; }
        public bool Comi { get; set; }
        public bool Historic { get; set; }
        public string Unit_Shipping_Definition { get; set; }
        public string Unit_Billing_Definition { get; set; }
        public string Internal_Remark { get; set; }
        public Nullable<int> RowOrder { get; set; }
    
        public virtual CustomerOrder CustomerOrder { get; set; }
        public virtual Good Good { get; set; }
        public virtual ProviderOrder ProviderOrder { get; set; }
    }
}
