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
    
    public partial class CustomerOrderMovementsForReport_Result
    {
        public Nullable<int> DeliveryNoteId { get; set; }
        public Nullable<int> BillId { get; set; }
        public int CustomerId { get; set; }
        public string CustomerCompanyName { get; set; }
        public Nullable<int> GoodId { get; set; }
        public string GoodDescription { get; set; }
        public Nullable<decimal> UnitBilling { get; set; }
        public Nullable<decimal> UnitShipping { get; set; }
        public string According { get; set; }
        public string SendType { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
    }
}
