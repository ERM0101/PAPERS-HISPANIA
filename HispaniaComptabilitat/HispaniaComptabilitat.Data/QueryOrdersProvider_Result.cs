//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HispaniaComptabilitat.Data
{
    using System;
    
    public partial class QueryOrdersProvider_Result
    {
        public int ProviderOrderId { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public bool According { get; set; }
        public Nullable<bool> PrevisioLliurament { get; set; }
        public Nullable<System.DateTime> PrevisioLliuramentData { get; set; }
        public int ProviderId { get; set; }
        public string ProviderAlias { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public string SendTypeDescription { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
