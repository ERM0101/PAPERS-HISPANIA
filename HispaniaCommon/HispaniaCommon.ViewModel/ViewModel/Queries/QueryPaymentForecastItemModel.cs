using HispaniaCommon.DataAccess.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HispaniaCommon.ViewModel.ViewModel.Queries
{
    public class QueryPaymentForecastItemModel
    {
        /// <summary>
        /// 
        /// </summary>
        [ExcelColumnAttribute( 1, "Nº Comanda" )]
        public int OrderId
        {
            get; 
            set; 
        }

        /// <summary>
        /// 
        /// </summary>
        [ExcelColumnAttribute( 2, "Nº Proveidor" )]
        public int ProviderId
        {
            get; 
            set; 
        }

        /// <summary>
        /// 
        /// </summary>
        [ExcelColumnAttribute( 3, "Proveidor" )]
        public string ProviderName
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [ExcelColumnAttribute( 5, "Import" )]
        public decimal Total
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [ExcelColumnAttribute( 4, "Data de Venciment" )]
        public DateTime PaymentDate
        {
            get;
            set; 
        }
    }
}
