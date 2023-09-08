using HispaniaCommon.DataAccess.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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

        #region FILTERNAMES
        public ObservableCollection<ComboBoxViewModel<string>> _FilterCriteriaNames = new ObservableCollection<ComboBoxViewModel<string>>();

        public ObservableCollection<ComboBoxViewModel<string>> FilterCriteriaNames
        {
            get
            {
                return this._FilterCriteriaNames;
            }
            set
            {
                this._FilterCriteriaNames = value;
                OnPropertyChanged( nameof( FilterCriteriaNames ) );
                ApplyFilter();
            }
        }

        #region SelectedFilter

        /// <summary>
        /// 
        /// </summary>
        private string _SelectedFilter = "";

        /// <summary>
        /// 
        /// </summary>
        public string SelectedFilter
        {
            get
            {
                return this._SelectedFilter;
            }
            set
            {
                this._SelectedFilter = value;
                OnPropertyChanged( nameof( SelectedFilter ) );
                ApplyFilter();
            }
        }

        #endregion


        #endregion

        #region FilterText

        /// <summary>
        /// 
        /// </summary>
        private string _FilterText = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string FilterText
        {
            get
            {
                return this._FilterText;
            }

            set
            {
                this._FilterText = value;
                OnPropertyChanged( nameof( FilterText ) );
                ApplyFilter();
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

        private Action<string, Dictionary<string, string>> _FilterAction = null;
        
        /// <summary>
        /// CTOR
        /// </summary>
        public QueryPaymentForecastViewModel( Dictionary<string, Dictionary<string, string>> gridHeadInfos = null,
                                              Action<string, Dictionary<string, string>> filterAction = null ):
                            base()
        {
            if(gridHeadInfos.TryGetValue( "dgListItem", out Dictionary<string,string> name2property ))
            {
                InitFilterCriteriaNames( name2property );
            }

            this._FilterAction = filterAction;

            this._ListItems = new ObservableCollection<QueryPaymentForecastItemModel>();

            DateTime moment = DateTime.Now;

            int month = moment.Month;
            int year = moment.Year;
            int start_day = 1;
            int end_day = 0;

            month += 1;
            if(month == 13)
            {
                month = 1;
                year++;
            }

            end_day = DateTime.DaysInMonth( year, month );
            
            this._StartDate = new DateTime( year, month, start_day );
            this._EndDate = new DateTime( year, month, end_day );

            this._TextStartDate = GlobalViewModel.GetStringFromDateTimeValue( this._StartDate );
            this._TextEndDate = GlobalViewModel.GetStringFromDateTimeValue( this._EndDate );

        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name2Prorties"></param>
        private void InitFilterCriteriaNames( Dictionary<string,string> name2Prorties )
        {
            this._FilterCriteriaNames.Clear();

            foreach(KeyValuePair<string, string> pair in name2Prorties)
            {
                this._FilterCriteriaNames.Add( new ComboBoxViewModel<string>( pair.Value, pair.Key ) );
            }
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

        /// <summary>
        /// 
        /// </summary>
        private void ApplyFilter()
        {
            if(null != this._FilterAction)
            {
                Dictionary<string, string> filter_criterias = new Dictionary<string, string>();

                try
                {
                    StartWait();

                    if(!string.IsNullOrWhiteSpace( this._SelectedFilter ) &&
                        !string.IsNullOrWhiteSpace( this._FilterText ))
                    {

                        filter_criterias.Add( this._SelectedFilter, this._FilterText.Trim() );

                        this._FilterAction( "dgListItem", filter_criterias );
                    }
                    else
                    {
                        this._FilterAction( "dgListItem", filter_criterias );
                    }
                }catch(Exception ex)
                {
                    Debug.WriteLine( $"Sort error:{ex.Message}" );
                }
                finally 
                { 
                    StopWait(); 
                }
            }
        }
    }
}
