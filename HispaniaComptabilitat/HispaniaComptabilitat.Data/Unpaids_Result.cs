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
    
    public partial class Unpaids_Result
    {
        public int Agent_Id { get; set; }
        public string Agent_Name { get; set; }
        public Nullable<int> Bill_Id { get; set; }
        public Nullable<System.DateTime> Bill_Date { get; set; }
        public int Customer_Id { get; set; }
        public string Company_Name { get; set; }
        public Nullable<decimal> Base { get; set; }
        public Nullable<decimal> ComissionPercent { get; set; }
        public Nullable<decimal> Comission { get; set; }
    }
}
