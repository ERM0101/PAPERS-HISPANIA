using HispaniaCommon.DataAccess;
using HispaniaComptabilitat.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Emit;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace HispaniaCommon.ViewModel.ViewModel.Queries
{


    /// <summary>
    /// 
    /// </summary>
    public class QueryOrdersProviderViewModel:ViewModelBase
    {
        private DateTime _StartDate;
        private DateTime _EndDate;
        private ObservableCollection<QueryOrderProviderViewModel> _ListItems;
        private ObservableCollection<ComboBoxViewModel<int>> _Providers;
        private ObservableCollection<ComboBoxViewModel<int>> _Goods;
        private int? _SelectedProvider;
        private int? _SelectedGood;

        private string _TextInitDate = string.Empty;
        private string _TextEndDate = string.Empty;

        public string TextInitDate
        {
            get
            {
                return _TextInitDate; 
            }
            set
            {
                _TextInitDate = value;
                OnPropertyChanged( nameof( TextInitDate ) );                
            }
        }

        public string TextEndDate
        {
            get
            {
                return _TextEndDate;
            }
            set
            {
                _TextEndDate = value;
                OnPropertyChanged( nameof( TextEndDate ) );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ComboBoxViewModel<int>> Providers
        {
            get
            {
                return _Providers;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<ComboBoxViewModel<int>> Goods
        {
            get
            {
                return _Goods; 
            } 
        }


        /// <summary>
        /// 
        /// </summary>
        public int? SelectedProvider
        {
            get
            {
                return _SelectedProvider;
            }
            set
            {
                this._SelectedProvider = value;
                OnPropertyChanged( nameof( SelectedProvider ) );
                Refresh();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int? SelectedGood
        {
            get
            {
                return _SelectedGood;
            }
            set
            {
                this._SelectedGood = value;
                OnPropertyChanged( nameof( SelectedGood ) );
                Refresh();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime StartDate
        {
            get
            {
                return _StartDate;
            }
            set
            {
                if(this._StartDate != value)
                {
                    this._StartDate = value;
                    OnPropertyChanged(nameof(StartDate));
                    this.TextInitDate = GlobalViewModel.GetStringFromDateTimeValue( value );
                    Refresh();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime EndDate
        {
            get
            {
                return _EndDate;
            }
            set
            {
                if(this._EndDate != value)
                {
                    this._EndDate = value;
                    OnPropertyChanged( nameof( EndDate ) );
                    this.TextEndDate = GlobalViewModel.GetStringFromDateTimeValue( value );
                    Refresh();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<QueryOrderProviderViewModel> ListItems
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

        private RelayCommand _RefreshCommand;
        
        /// <summary>
        /// 
        /// </summary>
        public RelayCommand RefreshCommand
        {
            get
            {
                return this._RefreshCommand ??
                  ( this._RefreshCommand = new RelayCommand( obj =>
                  {
                      try
                      {
                          this._StartDate = GlobalViewModel.GetDateTimeValue( this.TextInitDate );
                          this._EndDate = GlobalViewModel.GetDateTimeValue( this.TextEndDate );

                          Refresh();

                      } catch(Exception ex)
                      {

                      }
                  } ));
            }
        }

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
                                                                               "Gestió de Comandes De Proveidor" );

                      } catch(Exception ex)
                      {

                      }
                  } ));
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        public QueryOrdersProviderViewModel():
                base()
        {
            this._ListItems = new ObservableCollection<QueryOrderProviderViewModel>();



            this._StartDate = DateTime.Now.AddMonths( -1 );
            this._EndDate = DateTime.Now;

            this._TextInitDate = GlobalViewModel.GetStringFromDateTimeValue( this._StartDate );
            this._TextEndDate = GlobalViewModel.GetStringFromDateTimeValue( this._EndDate );

            this._Providers = new ObservableCollection<ComboBoxViewModel<int>>();
            this._Goods = new ObservableCollection<ComboBoxViewModel<int>>();

            // load and select            
            this._SelectedProvider = LoadProviders().Value;
            this._SelectedGood = LoadGoods().Value;
                        
            Refresh();
        }

        /// <summary>
        /// 
        /// </summary>
        private void Refresh()
        {
            
            StartWait();

            int? article_id = (this.SelectedGood.HasValue ?
                                    (this.SelectedGood.Value != 0 ? (int?)this.SelectedGood.Value : null )
                                    : null );

            int? provider_id = (this.SelectedProvider.HasValue ?
                                    (this.SelectedProvider.Value != 0 ? (int?)this.SelectedProvider.Value : null)
                                    : null);

            this._ListItems.Clear();

            IEnumerable<QueryOrderProviderViewModel> items = null;
            try
            {
                StartWait();

                items =  QueryViewModel.Instance.GetQueryOrders( this.StartDate, this.EndDate,
                                                                 article_id, provider_id );
                StopWait();

            } catch(Exception ex)
            {
                StopWait();

                throw;
            }

            foreach(QueryOrderProviderViewModel item in items )
            {
                this._ListItems.Add( item );
            };                
        }

        /// <summary>
        /// 
        /// </summary>
        private ComboBoxViewModel<int> LoadGoods()
        {
            ComboBoxViewModel<int> result = null;

            RefreshDataViewModel.Instance.RefreshData( WindowToRefresh.GoodsWindow );
            var goods = GlobalViewModel.Instance.HispaniaViewModel.Goods;

            this._Goods.Clear();

            result = new ComboBoxViewModel<int>( 0, "--- TODOS ---" );
            this._Goods.Add( result );

            foreach(ComboBoxViewModel<int> good in goods.OrderBy( i => i.Good_Description)
                                       .Select( i => new ComboBoxViewModel<int>( i.Good_Id, i.Good_Description ) ))
            {
                this._Goods.Add( good );
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        private ComboBoxViewModel<int> LoadProviders()
        {
            ComboBoxViewModel<int> result = null;

            RefreshDataViewModel.Instance.RefreshData( WindowToRefresh.ProvidersWindow );
            var providers = GlobalViewModel.Instance.HispaniaViewModel.Providers;

            this._Providers.Clear();

            result = new ComboBoxViewModel<int>( 0, "--- TODOS ---" );
            this._Providers.Add( result );

            foreach( ComboBoxViewModel<int> provider in providers.OrderBy( i => i.Name )
                                       .Select( i => new ComboBoxViewModel<int>( i.Provider_Id, i.Name ) ) )
            {
                this._Providers.Add( provider );
            }

            return result;
        }
    }
}
