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
    public class PaymentDateInfo
    {
        /// <summary>
        /// POayment day
        /// </summary>
        public int Days
        {
            get;
        }

        /// <summary>
        /// Payment date
        /// </summary>
        public DateTime Date
        {
            get;
        }

        /// <summary>
        /// Percent
        /// </summary>
        public decimal Percent
        {
            get;
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="days"></param>
        /// <param name="percent"></param>
        /// <param name="date"></param>
        internal PaymentDateInfo( int days, decimal percent, DateTime date )
        {
            this.Days = days;
            this.Date = date;
            this.Percent = percent;
        }
    }
}
