#region Librerias usadas por la ventana

using HispaniaCommon.ViewModel;
using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.Theme;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

#endregion

namespace HispaniaCommon.ViewClientWPF.Windows
{
    /// <summary>
    /// Lógica de interacción para Queries.xaml
    /// </summary>
    public partial class Queries : Window
    {
        #region Constants

        /// <summary>
        /// Defines the theme for the main Hispania Application.
        /// </summary>
        private ThemeInfo HispaniaApp = new ThemeInfo("HispaniaCommon.ViewClientWPF", "component/Recursos/Themes/", "HispaniaComptabilitat");

        /// <summary>
        /// Defines the theme for the support Hispania Application.
        /// </summary>
        private ThemeInfo HispaniaSupportApp = new ThemeInfo("HispaniaCommon.ViewClientWPF", "component/Recursos/Themes/", "HispaniaHelp");

        /// <summary>
        /// Hide the ItemPannel.
        /// </summary>
        private GridLength HideComponent = new GridLength(0.0);

        /// <summary>
        /// Hide the ItemPannel.
        /// </summary>
        private GridLength HideHeaderPanel = new GridLength(40.0);

        /// <summary>
        /// Show the Params Pannel.
        /// </summary>
        private GridLength ViewOperationsBBDDPanel = new GridLength(150.0);        

        /// <summary>
        /// Show the Params Pannel.
        /// </summary>
        private GridLength ViewParamsPannel = new GridLength(175.0);

        /// <summary>
        /// Show the Params Pannel.
        /// </summary>
        private GridLength ViewExtendedParamsPannel = new GridLength(350.0);

        /// <summary>
        /// Show the Params Pannel.
        /// </summary>
        private GridLength ViewSQLPannel = new GridLength(175.0);

        #endregion

        #region Attributes

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private QueryType m_QueryType = QueryType.None;

        /// <summary>
        /// Store the Agents
        /// </summary>
        public ObservableCollection<AgentsView> m_DataList;

        #region GUI

        /// <summary>
        /// Store the background color for the search text box.
        /// </summary>
        private Brush m_AppColor = null;

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Store the type of Application with the user want open.
        /// </summary>
        public ApplicationType AppType
        {
            get;
            private set;
        }

        /// <summary>
        /// Get or Set the type of query.
        /// </summary>
        private QueryType QueryType
        {
            get
            {
                return (m_QueryType);
            }
            set
            {
                //  Actualize property value
                    m_QueryType = value;
                //  Refresh Button Bar
                    RefreshUI(true);
            }
        }

        private DateTime CurrentInitFilterData  { get; set; }

        /// <summary>
        /// Get or Set if the manager of the data change for the Good has active.
        /// </summary>
        private bool DataChangedManagerActive
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set the data to show in List of Items.
        /// </summary>
        public ObservableCollection<AgentsView> DataList
        {
            get
            {
                return (m_DataList);
            }
            set
            {
                DataChangedManagerActive = false;
                if (value != null) m_DataList = value;
                else m_DataList = new ObservableCollection<AgentsView>();
                ListItems.ItemsSource = m_DataList;
                ListItems.DataContext = this;
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Filter = UserFilter;
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                ListItems.SelectedItems.Clear();
                DataChangedManagerActive = true;
            }
        }

        #region GUI

        /// <summary>
        /// Get the ThemeInfo to apply 
        /// </summary>
        private ThemeInfo AppTheme
        {
            get
            {
                if (AppType == ApplicationType.Comptabilitat) return (HispaniaApp);
                else if (AppType == ApplicationType.Help) return (HispaniaSupportApp);
                else throw new Exception("Error, undefined app type.");
            }
        }

        /// <summary>
        /// Get the background color for the search text box.
        /// </summary>
        private Brush BrAppColor
        {
            get
            {
                if (m_AppColor == null)
                {
                    if (AppType == ApplicationType.Comptabilitat) m_AppColor = new SolidColorBrush(Color.FromArgb(255, 155, 33, 28));
                    else if (AppType == ApplicationType.Help)
                    {
                        m_AppColor = new SolidColorBrush(Color.FromArgb(255, 8, 70, 124));
                    }
                    else m_AppColor = new SolidColorBrush(Colors.WhiteSmoke);
                }
                return (m_AppColor);
            }
        }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Main Builder of the windows.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        public Queries(ApplicationType AppType, QueryType QueryTypeIn = QueryType.None)
        {
            InitializeComponent();
            Initialize(AppType, QueryTypeIn);
        }

        /// <summary>
        /// Method that initialize the window.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        private void Initialize(ApplicationType AppType, QueryType QueryTypeIn = QueryType.None)
        {
            //  Actualize the Window.
                ActualizeWindowComponents(AppType, QueryTypeIn);
            //  Load Data in Window components.
                LoadDataInWindowComponents();
            //  Load the managers of the controls of the Window.
                LoadManagers();
        }

        #endregion

        #region Standings

        /// <summary>
        /// Method that actualize the Window components.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        private void ActualizeWindowComponents(ApplicationType AppType, QueryType QueryTypeIn = QueryType.None)
        {
            //  Validate the QueryType.
                if (QueryTypeIn == QueryType.None)
                {
                    MsgManager.ShowMessage("Error, no s'ha seleccionat el tipus de Consulta a presentar.");
                    Close();
                }
            //  Actualize properties of this Window.
                this.AppType = AppType;
            //  Initialize Filter Components
                dtpInitData.SelectedDate = CurrentInitFilterData = DateTime.Now;
                dtpEndData.DisplayDateStart = dtpInitData.SelectedDate;
                tbInitData.Text = GlobalViewModel.GetStringFromDateTimeValue(CurrentInitFilterData);
                dtpEndData.SelectedDate = dtpInitData.SelectedDate;
                tbEndData.Text = GlobalViewModel.GetStringFromDateTimeValue(dtpEndData.SelectedDate);
            //  initialize Properties
                QueryType = QueryTypeIn;
            //  Apply Theme to window.
                ThemeManager.ActualTheme = AppTheme;
        }

        /// <summary>
        /// Method that load data in Window components.
        /// </summary>
        private void LoadDataInWindowComponents()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Set Data into the Window.
                CurrentInitFilterData = (DateTime) dtpInitData.SelectedDate;
            //  Activate managers
                DataChangedManagerActive = true;

            RefreshArticleProviderFilter();
        }

        /// <summary>
        /// Method that filter the elements that are showing in the list
        /// </summary>
        /// <param name="item">Item to test</param>
        /// <returns>true, if the item must be loaded, false, if not.</returns>
        private bool UserFilter(object item)
        {
            //  Get Acces to the object and the property name To Filter.
                AgentsView ItemData = (AgentsView)item;
            //  Calculate the Visibility value with properties values.
                return (!ItemData.Canceled);
        }

        #endregion

        #region Managers

        /// <summary>
        /// Method that define the managers needed for the user operations in the Window
        /// </summary>
        private void LoadManagers()
        {
            //  Button Search Clients
                btnCreateExcel.Click += BtnCreateExcel_Click;
                btnExecuteQuery.Click += BtnExecuteQuery_Click;
                btnExit.Click += BtnExit_Click;
                btnRefresh.Click += BtnRefresh_Click;
            //  DatePiker
                dtpInitData.SelectedDateChanged += DtpInitData_SelectedDateChanged;
                dtpEndData.SelectedDateChanged += DtpEndData_SelectedDateChanged;
            //  ListView
                ListItems.SelectionChanged += ListItems_SelectionChanged;
            //  Images
                imgParamQuery.MouseDown += ImgParamQuery_MouseDown;
                imgSQLQuery.MouseDown += ImgSQLQuery_MouseDown;
            
            // Article Provedores
            cbArticleFilter.SelectionChanged += CbArticleProviderFilter_SelectionChanged;
            cbProveforFilter.SelectionChanged += CbArticleProviderFilter_SelectionChanged;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbArticleProviderFilter_SelectionChanged( object sender, System.Windows.Controls.SelectionChangedEventArgs e )
        {
            UpdateDataUI();
        }

        #region TextBox

        /// <summary>
        /// Select all text in sender TextBox control.
        /// </summary>
        /// <param name="sender">TextBox control that has produced the event.</param>
        /// <param name="e">Parameters associateds to the event.</param>
        private void TBGotFocus(object sender, RoutedEventArgs e)
        {
            GlobalViewModel.Instance.SelectAllTextInGotFocusEvent(sender, e);
        }
        #endregion

        #region Button
									
        #region Refresh

        /// <summary>
        /// Manage the button for Refresh.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActualizeQueriesFromDb();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al refrescar els valors dels Representants.", MsgManager.ExcepMsg(ex)));
            }
        }

        #endregion

        #region Create Excel

        /// <summary>
        /// Manage the Button Mouse Click in the button that creates excel with the query type and params selecteds.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnCreateExcel_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            btnExit.IsEnabled = false;
            try
            {
                QueryViewModel.Instance.CreateExcelFromQuery(QueryType, CreateParams());
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al crear l'Excel.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                btnExit.IsEnabled = true;
            }
        }

        #endregion

        #region Execute Query

        private void BtnExecuteQuery_Click(object sender, RoutedEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            btnExit.IsEnabled = false;
            try
            {
                Dictionary<string, object> Params = CreateParams();
                dgData.DataContext = QueryViewModel.Instance.GetDataQuery(QueryType, Params).DefaultView;
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, a l'executar la consulta.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;
                btnExit.IsEnabled = true;
            }
        }

        #endregion

        #region Exit

        /// <summary>
        /// Manage the button for add Items in the list.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion

        #endregion

        #region DatePiker

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DtpInitData_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                DateTime InitValidityPeriod = (DateTime)dtpInitData.SelectedDate;
                DateTime? EndValidityPeriod = dtpEndData.SelectedDate;
                if ((EndValidityPeriod != null) && (InitValidityPeriod > (DateTime)EndValidityPeriod))
                {
                    MsgManager.ShowMessage("Error, la data d'inici de l'interval de filtratge no pot ser major que la de final.");
                    dtpInitData.SelectedDate = CurrentInitFilterData;
                }
                else
                {
                    dtpEndData.DisplayDate = EndValidityPeriod ?? DateTime.Now;
                    dtpEndData.DisplayDateStart = InitValidityPeriod;
                    CurrentInitFilterData = (DateTime) dtpInitData.SelectedDate;
                    UpdateDataUI();
                }
                dtpInitData.DisplayDate = CurrentInitFilterData;
                tbInitData.Text = GlobalViewModel.GetStringFromDateTimeValue(CurrentInitFilterData);
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DtpEndData_SelectedDateChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            tbEndData.Text = GlobalViewModel.GetStringFromDateTimeValue((DateTime)dtpEndData.SelectedDate);
            UpdateDataUI();
        }

        #endregion

        #region ListView

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListItems_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                UpdateDataUI();
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region Images

        private void ImgParamQuery_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rdParamsPanel.Height =
                (rdParamsPanel.Height == HideHeaderPanel) ?
                     ((QueryType == QueryType.HistoCustomerForDataAndAgent) ? ViewExtendedParamsPannel : ViewParamsPannel)
                     : HideHeaderPanel;
        }

        private void ImgSQLQuery_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rdSQLPanel.Height = (rdSQLPanel.Height == HideHeaderPanel) ? ViewSQLPannel : HideHeaderPanel;
        }

        #endregion

        #endregion

        #region Database Operations
		        
        private void ActualizeQueriesFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.QueriesWindow);
                DataList = GlobalViewModel.Instance.HispaniaViewModel.Agents;
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        #endregion

        #region Update IU Methods

        /// <summary>
        /// Refresh UI depending of QueryType.
        /// </summary>
        private void RefreshUI(bool InitWindow = false)
        {
            string QueryInfo;
            switch (m_QueryType)
            {
                case QueryType.Goods:
                     QueryInfo = "ARTICLES";
                     rdParamsPanel.Height = HideComponent;
                     rdSQLPanel.Height = HideHeaderPanel;
                     cdOperationsBBDD.Width = HideComponent;
                    filterGrid.ColumnDefinitions[ 2 ].Width = HideComponent;
                    break;
                case QueryType.Customers:
                case QueryType.Customers_Full:
                     QueryInfo = "CLIENTS";
                     rdParamsPanel.Height = HideComponent;
                     rdSQLPanel.Height = HideHeaderPanel;
                     cdOperationsBBDD.Width = HideComponent;
                    filterGrid.ColumnDefinitions[ 2 ].Width = HideComponent;
                    break;
                case QueryType.CustomerOrders:
                     QueryInfo = "COMANDES DE CLIENT";
                     rdParamsPanel.Height = HideComponent;
                     rdSQLPanel.Height = HideHeaderPanel;
                     cdOperationsBBDD.Width = HideComponent;
                     filterGrid.ColumnDefinitions[ 2 ].Width = HideComponent;
                     break;
                case QueryType.HistoCustomerForData:
                     QueryInfo = "HISTÒRIC DE CLIENTS PER DATA";
                     rdParamsPanel.Height = ViewParamsPannel;
                     rdSQLPanel.Height = HideHeaderPanel;
                     cdOperationsBBDD.Width = HideComponent;                    
                     //lblAgent.Visibility = Visibility.Collapsed;
                     //ListItems.Visibility = Visibility.Collapsed;                                          
                     filterGrid.ColumnDefinitions[ 0 ].Width = HideComponent;
                     filterGrid.ColumnDefinitions[ 1 ].Width = HideComponent;
                     filterGrid.ColumnDefinitions[ 2 ].Width = HideComponent;

                    break;
                case QueryType.HistoCustomerForDataAndAgent:
                     QueryInfo = "HISTÒRIC DE CLIENTS PER DATA I REPRESENTANT";
                     rdParamsPanel.Height = ViewExtendedParamsPannel;
                     rdSQLPanel.Height = HideHeaderPanel;
                     cdOperationsBBDD.Width = HideComponent;
                    filterGrid.ColumnDefinitions[ 2 ].Width = HideComponent;
                    break;
                case QueryType.CustomQuery:
                     QueryInfo = "CONSULTA PERSONALITZADA";
                     rdParamsPanel.Height = HideComponent;
                     rdSQLPanel.Height = ViewSQLPannel;
                     cdOperationsBBDD.Width = ViewOperationsBBDDPanel;
                     tbSQLQuery.IsReadOnly = false;
                    filterGrid.ColumnDefinitions[ 2 ].Width = HideComponent;
                    break;

                case QueryType.Providers:
                    QueryInfo = "COMANDES A PROVEIDORS";
                    rdParamsPanel.Height = ViewParamsPannel;
                    rdSQLPanel.Height = HideHeaderPanel;
                    cdOperationsBBDD.Width = HideComponent;

                    //lblAgent.Visibility = Visibility.Collapsed;
                    //ListItems.Visibility = Visibility.Collapsed;
                    filterGrid.ColumnDefinitions[1].Width = HideComponent;
                    filterGrid.ColumnDefinitions[0].Width = HideComponent;
                    //filterGrid.ColumnDefinitions[2].Width = HideComponent;

                    break;
                case QueryType.None:
                default:
                    throw new Exception("Error, tipus de consulta no reconegut.");
            }
            Title = "Consultes a Base de Dades: " + QueryInfo;
            UpdateDataUI(InitWindow);
        }

        private void UpdateDataUI(bool InitWindow = false)
        {
            try
            {
                Dictionary<string, object> Params = CreateParams();
                if (QueryType != QueryType.CustomQuery)
                {
                    tbSQLQuery.Text = QueryViewModel.Instance.GetQuerySQL_UI(QueryType, Params);
                }
                if (((QueryType == QueryType.CustomQuery) && (!InitWindow)) || (QueryType != QueryType.CustomQuery))
                {
                    DataTable table = QueryViewModel.Instance.GetDataQuery( QueryType, Params );
                    dgData.DataContext = table.DefaultView;                    
                }
            }
            catch (Exception ex)
            {
                string ErrMsg = string.Format("Error, al carregar la informació de la consulta.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex));
                if (!InitWindow) MsgManager.ShowMessage(ErrMsg);
                else throw new Exception(ErrMsg);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        private void RefreshArticleProviderFilter( )
        {
            RefreshDataViewModel.Instance.RefreshData( WindowToRefresh.GoodsWindow );
            var goods = GlobalViewModel.Instance.HispaniaViewModel.Goods;

            RefreshDataViewModel.Instance.RefreshData( WindowToRefresh.ProvidersWindow );
            var providers = GlobalViewModel.Instance.HispaniaViewModel.Providers;

            var goods_items = goods.OrderBy( i=> i.Good_Description )
                                   .Select( i => new { Text = i.Good_Description, Value = i.Good_Id, } )
                                   .ToList();
            goods_items.Insert( 0, new
            {
                Text = "--- ALL ---", Value = 0
            } );

            cbArticleFilter.ItemsSource = goods_items;
            cbArticleFilter.DisplayMemberPath = "Text";
            cbArticleFilter.SelectedValuePath = "Value";

            var providers_items = providers.OrderBy( i => i.Name )
                                            .Select( i => new { Text = i.Name, Value = i.Provider_Id, } )
                                            .ToList();
            providers_items.Insert( 0, new
            {
                Text = "--- ALL ---", Value = 0
            } );

            cbProveforFilter.ItemsSource = providers_items;
            cbProveforFilter.DisplayMemberPath = "Text";
            cbProveforFilter.SelectedValuePath = "Value";

        }



        private Dictionary<string, object> CreateParams()
        {
            Dictionary<string, object> Params = null;
            if ( ( QueryType == QueryType.HistoCustomerForData ) || 
                 ( QueryType == QueryType.HistoCustomerForDataAndAgent ) || 
                 ( QueryType == QueryType.Providers ) )
            {
                Params = new Dictionary<string, object>
                         {
                            { "DateInit", dtpInitData.SelectedDate },
                            { "DateEnd", (dtpEndData.SelectedDate is null) ? DateTime.Now : dtpEndData.SelectedDate }
                         };
                
                if(QueryType == QueryType.Providers)
                {
                    if(cbArticleFilter.SelectedIndex != -1)
                    {
                        object value = cbArticleFilter.SelectedValue;
                        if(value is int)
                        {
                            Params.Add( "article_id", (int)value );
                        }
                    }

                    if( cbProveforFilter.SelectedIndex != -1)
                    {
                        object value = cbProveforFilter.SelectedValue;
                        if(value is int)
                        {
                            Params.Add( "provider_id", (int)value );
                        }
                    }
                }
            }
            if (QueryType == QueryType.HistoCustomerForDataAndAgent)
            {
                List<AgentsView> Agents = new List<AgentsView>();
                foreach (AgentsView Agent in ListItems.SelectedItems)
                {
                    Agents.Add(new AgentsView(Agent));
                }
                Params.Add("Agents", Agents);
            }
            if (QueryType == QueryType.CustomQuery)
            {
                Params = new Dictionary<string, object>
                         {
                            { "Query", tbSQLQuery.Text },
                         };
            }
            return Params;
        }

        #endregion
    }
}
