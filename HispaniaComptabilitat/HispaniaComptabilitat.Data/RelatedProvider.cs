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
    
    public partial class RelatedProvider
    {
        public int Provider_Id { get; set; }
        public int Provider_Canceled_Id { get; set; }
        public string Remarks { get; set; }
    
        public virtual Provider Provider { get; set; }
        public virtual Provider Provider1 { get; set; }
    }
}
