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
    
    public partial class InputsOutputs_Result
    {
        public string Good_Code { get; set; }
        public Nullable<System.DateTime> IO_Date { get; set; }
        public string IO_Type { get; set; }
        public Nullable<decimal> Amount_Shipping { get; set; }
        public Nullable<decimal> Amount_Billing { get; set; }
        public Nullable<decimal> Price { get; set; }
        public Nullable<int> DeliveryNote_Id { get; set; }
        public Nullable<int> DeliveryNote_Year { get; set; }
        public Nullable<int> Customer_Id { get; set; }
        public Nullable<int> Bill_Id { get; set; }
        public Nullable<int> Bill_Year { get; set; }
        public string Bill_Serie { get; set; }
        public int IO_State { get; set; }
        public string Provider { get; set; }
        public Nullable<int> Provider_Id { get; set; }
    }
}
