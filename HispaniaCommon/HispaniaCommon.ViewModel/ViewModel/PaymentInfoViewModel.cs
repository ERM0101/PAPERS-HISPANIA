using HispaniaCommon.ViewModel.ViewModel.Paymets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HispaniaCommon.ViewModel.ViewModel
{
    public class PaymentInfoViewModel: 
                ViewModelBase
    {
        #region Visible

        private bool _Visible;

        /// <summary>
        /// Visible
        /// </summary>
        public bool Visible
        {
            get
            {
                return this._Visible;
            }
            set
            {
                if(this._Visible != value)
                {
                    this._Visible = value;
                    base.OnPropertyChanged( "Visible");
                }
            }
        }
        #endregion

        #region PaimetInfo

        public string _PaymentsInfoText;

        /// <summary>
        /// Payments info text
        /// </summary>
        public string PaymentsInfoText
        {
            get
            {
                return this._PaymentsInfoText;
            }
            set
            {
                if(string.Compare( this._PaymentsInfoText, value ) != 0)
                {
                    this._PaymentsInfoText = value;
                    base.OnPropertyChanged( "PaymentsInfoText" );
                    this.Visible = !string.IsNullOrEmpty( value );
                }
            }
        }


        #endregion

        #region DeliveryDate

        private DateTime? _DeliveryDate;

        public DateTime? DeliveryDate
        {
            get
            {
                return this._DeliveryDate;
            }
            set
            {
                if(this._DeliveryDate != value)
                {
                    this._DeliveryDate = value;
                    RebuildPaymetInfoText();
                }
            }
        }

        #endregion

        #region payment DAYS
        
        private IEnumerable<int> _PaymentDays;
        
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<int> PaymentDays
        {
            get
            {
                return this._PaymentDays;
            }

            set
            {
                this._PaymentDays = value;
                RebuildPaymetInfoText();
            }
        }

        #endregion

        /// <summary>
        /// CTOR
        /// </summary>
        public PaymentInfoViewModel()
        {
            this._Visible = true;
            this._DeliveryDate = null;
            this._PaymentDays = Enumerable.Empty<int>();

            this._PaymentsInfoText = "No payments";
        }

        /// <summary>
        /// 
        /// </summary>
        private void RebuildPaymetInfoText()
        {
            string message = null;

            if(this._DeliveryDate.HasValue && this._PaymentDays.Any() )
            {
                decimal percent = 100.0m / this._PaymentDays.Count();

                IEnumerable<PaymentDayInfo> paymets_day = this._PaymentDays.Select( d => new PaymentDayInfo( d, percent ) )
                                                               .Normalize();

                IEnumerable<PaymentDateInfo> payments_date = paymets_day.BuldPaymensDateInfo( this._DeliveryDate.Value );

                message = payments_date.BuildString();
            }

            this.PaymentsInfoText = message;
        }

    }
}
