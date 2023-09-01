using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HispaniaCommon.ViewModel.ViewModel.Paymets
{
    /// <summary>
    /// 
    /// </summary>
    public class PaymentDayInfo
    {
        /// <summary>
        /// Payment Day
        /// </summary>
        public int Days
        {
            get;
        }

        /// <summary>
        /// Percents
        /// </summary>
        public decimal Percent
        {
            get;
        }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="days"></param>
        /// <param name="percent"></param>
        /// <param name="decimals"></param>
        public PaymentDayInfo( int days, decimal percent = 100.0m, int decimals = 2 )
        {
            this.Days = days;
            this.Percent = Math.Round( percent, decimals );
        }
    }
}
