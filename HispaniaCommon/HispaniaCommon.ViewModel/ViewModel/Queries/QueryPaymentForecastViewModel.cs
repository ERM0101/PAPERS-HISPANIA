using HispaniaCommon.DataAccess.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HispaniaCommon.ViewModel.ViewModel.Queries
{
    /// <summary>
    /// 
    /// </summary>
    public class QueryPaymentForecastViewModel:
            ViewModelBase
    {
        #region PROPERTIES

        #region TextStartDare

        private string _TextStartDate;
        
        /// <summary>
        /// Start date text value
        /// </summary>
        public string TextStartDate
        {
            get
            {            
                return this._TextStartDate; 
            }

            set
            {                
                this._TextStartDate = value;
                OnPropertyChanged( nameof( TextStartDate ) );
            }
        }

        #endregion

        #region TextEndDate;
        
        private string _TextEndDate;

        /// <summary>
        /// End date text value
        /// </summary>
        public string TextEndDate
        {
            get
            {
                return this._TextEndDate;
            }

            set
            {
                this._TextEndDate = value;
                OnPropertyChanged( nameof( TextEndDate ) );
            }
        }

        #endregion

        #region StartDate

        private DateTime _StartDate;

        /// <summary>
        /// Start date
        /// </summary>
        public DateTime StartDate
        {
            get
            {
                return this._StartDate;
            }

            set
            {
                this._StartDate = value;
                OnPropertyChanged( nameof( StartDate ) );
                this.TextStartDate = GlobalViewModel.GetStringFromDateTimeValue( value );
                Refresh();
            }
        }

        #endregion

        #region EndDate

        private DateTime _EndDate;

        /// <summary>
        /// End date
        /// </summary>
        public DateTime EndDate
        {
            get
            {
                return this._EndDate;
            }

            set
            {
                this._EndDate = value;
                OnPropertyChanged( nameof( EndDate ) );
                this.TextEndDate = GlobalViewModel.GetStringFromDateTimeValue( value );
                Refresh();
            }
        }
        #endregion

        #region LISITEMS

        /// <summary>
        /// 
        /// </summary>
        private ObservableCollection<QueryPaymentForecastItemModel> _ListItems;
        
        /// <summary>
        /// Grid items
        /// </summary>
        public ObservableCollection<QueryPaymentForecastItemModel> ListItems
        {
            get
            {
                return this._ListItems;
            }
            set
            {
                this._ListItems = value;
                OnPropertyChanged( nameof( ListItems ) );
            }
        }

        #endregion

        #endregion

        #region COMMANDS

        #region REFRESH

        /// <summary>
        /// 
        /// </summary>
        private RelayCommand _RefreshCommand;

        /// <summary>
        /// 
        /// </summary>
        public RelayCommand RefreshCommand
        {
            get
            {
                return this._RefreshCommand ??
                  (this._RefreshCommand = new RelayCommand( obj =>
                  {
                      try
                      {
                          this._StartDate = GlobalViewModel.GetDateTimeValue( this._TextStartDate );
                          this._EndDate = GlobalViewModel.GetDateTimeValue( this._TextEndDate );

                          Refresh();

                      } catch( Exception ex )
                      {
                          Debug.WriteLine( $"Error:{ex.Message}");

                      }
                  } ));
            }
        }

        #endregion

        #region EXPORT

        /// <summary>
        /// 
        /// </summary>
        private RelayCommand _ExcelExportCommand;

        /// <summary>
        /// 
        /// </summary>
        public RelayCommand ExcelExportCommand
        {
            get
            {
                return this._ExcelExportCommand ??
                  (this._ExcelExportCommand = new RelayCommand( obj =>
                  {
                      try
                      {
                          QueryViewModel.Instance.GenerateExcelFromStreamData( this._ListItems,
                                                                               "Previsión de pago" );

                      } catch(Exception ex)
                      {

                      }
                  } ));
            }
        }

        #endregion


        #endregion

        /// <summary>
        /// CTOR
        /// </summary>
        public QueryPaymentForecastViewModel() :
                            base()
        {
            this._ListItems = new ObservableCollection<QueryPaymentForecastItemModel>();

            DateTime moment = DateTime.Now;

            this._StartDate = moment.AddMonths( -1 );
            this._EndDate = moment;

            this._TextStartDate = GlobalViewModel.GetStringFromDateTimeValue( this._StartDate );
            this._TextEndDate = GlobalViewModel.GetStringFromDateTimeValue( this._EndDate );

        }

        /// <summary>
        /// 
        /// </summary>
        public void Refresh()
        {
            StartWait();

            this._ListItems.Clear();

            try
            {
                IEnumerable<QueryPaymentForecastItemModel> items = QueryViewModel.Instance.GetQueryPaymentForecast( 
                                                                            this.StartDate, 
                                                                            this.EndDate );

                foreach(QueryPaymentForecastItemModel item in items)
                {
                    this._ListItems.Add( item );
                };

                StopWait();

            } catch(Exception ex)
            {
                StopWait();

                throw;
            }
        }

    }
}
