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
    
    public partial class PriceRange
    {
        public int PriceRange_Id { get; set; }
        public int Good_Id { get; set; }
        public Nullable<decimal> Sequence { get; set; }
        public Nullable<decimal> Since { get; set; }
        public Nullable<decimal> Until { get; set; }
    
        public virtual Good Good { get; set; }
    }
}
