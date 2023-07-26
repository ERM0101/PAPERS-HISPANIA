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
    
    public partial class WarehouseMovement
    {
        public int WarehouseMovement_Id { get; set; }
        public int Good_Id { get; set; }
        public Nullable<int> Provider_Id { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<decimal> Amount_Unit_Shipping { get; set; }
        public Nullable<decimal> Amount_Unit_Billing { get; set; }
        public Nullable<int> Type { get; set; }
        public Nullable<decimal> Price { get; set; }
        public bool According { get; set; }
    
        public virtual Good Good { get; set; }
        public virtual Provider Provider { get; set; }
    }
}
