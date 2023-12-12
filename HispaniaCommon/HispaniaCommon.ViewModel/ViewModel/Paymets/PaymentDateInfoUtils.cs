using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HispaniaCommon.ViewModel.ViewModel.Paymets
{
    /// <summary>
    /// 
    /// </summary>
    public static class PaymentDateInfoUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_This"></param>
        /// <param name="dateFormat"></param>
        /// <param name="percentFormat"></param>
        /// <returns></returns>
        public static string BuildString( this IEnumerable<PaymentDateInfo> _This, 
                                          string dateFormat = null, 
                                          string percentFormat = null )
        {
            string date_format = dateFormat;
            if( string.IsNullOrWhiteSpace( date_format ) )
            {
                date_format = "dd/MM/yyyy";
            }

            string percent_format = percentFormat;
            if(string.IsNullOrWhiteSpace( percent_format ))
            {
                percent_format = "N";
            }

            StringBuilder sb = new StringBuilder();

            foreach(var payment_info in _This.OrderBy( pi=> pi.Date ) )
            {
                if(sb.Length > 0)
                {
                    sb.Append("  ");
                }
                sb.Append( $"{payment_info.Date.ToString( date_format )} ({payment_info.Percent.ToString( percent_format )} %)" );
            }

            return sb.ToString();
        }

    }
}
