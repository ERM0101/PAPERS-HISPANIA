using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HispaniaCommon.ViewModel.ViewModel.Paymets
{
    public static class PaimentInfoUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_This"></param>
        /// <param name="nullOrEmptyIsZero"></param>
        /// <returns></returns>
        public static int? ToPaymentDay( this string _This, bool nullOrEmptyIsZero = false )
        {
            int? result = null;
            bool str_is_null_or_empty = string.IsNullOrEmpty( _This );

            if( nullOrEmptyIsZero && str_is_null_or_empty)
            {
                result = 0;
            }
            else
            {
                if(!str_is_null_or_empty)
                {
                    if(int.TryParse( _This, out int value ))
                    {
                        result = value;
                    }
                }
            }
            
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_This"></param>
        /// <returns></returns>
        public static IEnumerable<PaymentDayInfo> Normalize( this IEnumerable<PaymentDayInfo> _This )
        {
            if(_This is null)
            {
                throw new ArgumentNullException( nameof( _This ) );
            }

            PaymentDayInfo[] result = _This.GroupBy( i => i.Days )
                                           .Select( i=> new PaymentDayInfo( i.Key, i.Sum( e=> e.Percent ) ) )
                                           .ToArray();
            if(result.Length == 1)
            {
                // correct if one element
                result = new PaymentDayInfo[] { new PaymentDayInfo( result[ 0 ].Days, 100.0m ) };
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_This"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public static IEnumerable<PaymentDateInfo> BuldPaymensDateInfo( this IEnumerable<PaymentDayInfo> _This,
                                                                        DateTime date )
        {
            IEnumerable<PaymentDateInfo> result = _This.Select( pd=> new PaymentDateInfo( pd.Days,  
                                                                                          pd.Percent,
                                                                                          date.AddDays( pd.Days ) ) )
                                                        .ToArray();
            return result;
        }
    }
}
