#region Librerias usadas por la ventana

using HispaniaCommon.ViewClientWPF.UserControls;
using HispaniaCommon.ViewModel;
using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.Theme;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

#endregion

namespace HispaniaCommon.ViewClientWPF.Windows
{
    /// <summary>
    /// Interaction logic for MainWindowOp1.xaml
    /// </summary>
    public partial class Stocks : Window
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
        /// Show the Item Pannel.
        /// </summary>
        private GridLength ViewItemPannel = new GridLength(150);

        /// <summary>
        /// Show the Serach Pannel.
        /// </summary>
        private GridLength ViewSearchPannel = new GridLength(30.0);

        /// <summary>
        /// Show the Operation Pannel.
        /// </summary>
        private GridLength ViewOperationPannel = new GridLength(30.0);

        /// <summary>
        /// Hide the ItemPannel.
        /// </summary>
        private GridLength HideComponent = new GridLength(0.0);

        #endregion

        #region Attributes

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private ObservableCollection<StocksView> m_DataList = new ObservableCollection<StocksView>();

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
        /// Store the data to show in List of Items.
        /// </summary>
        public ObservableCollection<StocksView> DataList
        {
            get
            {
                return (m_DataList);
            }
            set
            {
                if (value != null)
                {
                    m_DataList = value;
                    ListItems.ItemsSource = m_DataList;
                    ListItems.DataContext = this;
                    CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Filter = UserFilter;
                    CollectionViewSource.GetDefaultView(ListItems.ItemsSource).SortDescriptions.Add(new SortDescription("Good_Code", ListSortDirection.Ascending));
                }
                else m_DataList = new ObservableCollection<StocksView>();
                if (m_DataList.Count > 0) rdSearchPannel.Height = ViewSearchPannel;
                else rdSearchPannel.Height = HideComponent;
            }
        }

        /// <summary>
        /// Get or Set if the manager of the data change for the Customer has active.
        /// </summary>
        private bool DataChangedManagerActive
        {
            get;
            set;
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
        private Brush brAppColor
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

        #region Buiders

        /// <summary>
        /// Main Builder of the windows.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        public Stocks(ApplicationType AppType)
        {
            InitializeComponent();
            Initialize(AppType);
        }

        /// <summary>
        /// Method that initialize the window.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        private void Initialize(ApplicationType AppType)
        {
            //  Actualize the Window.
                ActualizeWindowComponents(AppType);
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
        private void ActualizeWindowComponents(ApplicationType AppType)
        {
            //  Actualize properties of this Window.
                this.AppType = AppType;
            //  Apply Theme to window.
                ThemeManager.ActualTheme = AppTheme;
                StocksDataControl.AppType = AppType;
            //  Initialize state of Window components.
                rdItemPannel.Height = HideComponent;
                rdSearchPannel.Height = HideComponent;
                gsSplitter.IsEnabled = false;
                btnEdit.Visibility = btnViewData.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// Method that load data in Window components.
        /// </summary>
        private void LoadDataInWindowComponents()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Set Data into the Window.
                ListItems.ItemsSource = m_DataList;
                DataContext = DataList;
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Filter = UserFilter;
                cbFieldItemToSearch.ItemsSource = StocksView.Fields;
                cbFieldItemToSearch.DisplayMemberPath = "Key";
                cbFieldItemToSearch.SelectedValuePath = "Value";
                if (StocksView.Fields.Count > 0) cbFieldItemToSearch.SelectedIndex = 0;
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        /// <summary>
        /// Method that filter the elements that are showing in the list
        /// </summary>
        /// <param name="item">Item to test</param>
        /// <returns>true, if the item must be loaded, false, if not.</returns>
        private bool UserFilter(object item)
        {
            //  Determine if is needed aplicate one filter.
                if ((cbFieldItemToSearch.SelectedIndex == -1) || (String.IsNullOrEmpty(tbItemToSearch.Text))) return (true);
            //  Get Acces to the object and the property name To Filter.
                StocksView ItemData = (StocksView)item;
                String ProperyName = (string) cbFieldItemToSearch.SelectedValue;
            //  Apply the filter
                object valueToTest = ItemData.GetType().GetProperty(ProperyName).GetValue(ItemData);
                if (valueToTest == null) return (false);
                else return ((valueToTest.ToString().ToUpper()).Contains(tbItemToSearch.Text.ToUpper()));
        }
        
        #endregion

        #region Managers

        /// <summary>
        /// Method that define the managers needed for the user operations in the Window
        /// </summary>
        private void LoadManagers()
        {
            //  Window
                Closed += Stocks_Closed;
            //  TextBox
                tbItemToSearch.TextChanged += TbItemToSearch_TextChanged;
            //  Button Search Clients
                btnEdit.Click += BtnEdit_Click;
                btnViewData.Click += BtnViewData_Click;
                btnRefresh.Click += BtnRefresh_Click;
            //  Define ListView events to manage.
                ListItems.SelectionChanged += ListItems_SelectionChanged;   
            //  Define CustomerDataControl events to manage.
                StocksDataControl.EvAccept += StocksDataControl_evAccept;
                StocksDataControl.EvCancel += StocksDataControl_evCancel;
        }

        #region Filter

        /// <summary>
        /// Manage the search of Items in the list.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void TbItemToSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Refresh();
        }

        #endregion

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
                ActualizeStocksFromDb();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al refrescar els valors dels Extocs.", MsgManager.ExcepMsg(ex)));
            }
        }

        #endregion

        #region Editing Stocks

        /// <summary>
        /// Manage the Button Mouse Click in one of the items of the List to show its data in User Control.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnViewData_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItem != null)
            {
                try
                {
                    ActualizeStockFromDb();
                    StocksDataControl.CtrlOperation = Operation.Show;
                    gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                    ShowItemPannel();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, al presentar la informació de l'Estoc seleccionat.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
        }

        /// <summary>
        /// Manage the button for edit an Item of the list.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItem != null)
            {
                try
                {
                    ActualizeStockFromDb();
                    if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(ListItems.SelectedItem, out string ErrMsg))
                    {
                        StocksDataControl.CtrlOperation = Operation.Edit;
                        gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "EditableGroupBox");
                        ShowItemPannel();
                    }
                    else MsgManager.ShowMessage(ErrMsg);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, a l'editar l'Estoc seleccionat.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
        }

        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void StocksDataControl_evAccept(StocksView NewOrEditedStocks)
        {
            try
            {
                StocksView NewStock = new StocksView(NewOrEditedStocks);
                switch (StocksDataControl.CtrlOperation)
                {
                    case Operation.Edit:
                         GlobalViewModel.Instance.HispaniaViewModel.UpdateGoodStocks(NewStock);
                         if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(StocksDataControl.StockData, out string ErrMsg))
                         {
                             MsgManager.ShowMessage(ErrMsg);
                         }
                         DataChangedManagerActive = false;
                         if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                         DataList = GlobalViewModel.Instance.HispaniaViewModel.Stocks;
                         DataChangedManagerActive = true;
                         ListItems.SelectedItem = NewStock;
                         ListItems.UpdateLayout();
                         gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                         HideItemPannel();
                         break;
                }
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al guardar la informació de l'Estoc editat.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
            }
        }

        /// <summary>
        /// Manage the event produced when the operation in that was doing in the IVATypesDataControl was Cancelled.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void StocksDataControl_evCancel()
        {
            switch (StocksDataControl.CtrlOperation)
            {
                case Operation.Add:
                     MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                     break;
                case Operation.Edit:
                     MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                     if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(StocksDataControl.StockData, out string ErrMsg))
                     {
                         MsgManager.ShowMessage(ErrMsg);
                     }
                     break;
                case Operation.Show:
                     break;
                default:
                     break;
            }
            HideItemPannel();
        }

        #endregion

        #endregion

        #region List View

        /// <summary>
        /// Manage the event produced when change the selection of the items list.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void ListItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                ActualizeButtonBar();
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region Window

        /// <summary>
        /// Manage the window close and free the register lock.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Stocks_Closed(object sender, EventArgs e)
        {
            if (StocksDataControl.CtrlOperation == Operation.Edit)
            {
                if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(StocksDataControl.StockData, out string ErrMsg))
                {
                    MsgManager.ShowMessage(ErrMsg);
                }
            }
        }

        #endregion

        #endregion
                
        #region Database Operations
		        
        private void ActualizeStocksFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.StocksWindow);
                DataList = GlobalViewModel.Instance.HispaniaViewModel.Stocks;
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        private void ActualizeStockFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                StocksView SelectedItem = (StocksView)ListItems.SelectedItem;
                StocksView ItemInDb = GlobalViewModel.Instance.HispaniaViewModel.GetStockFromDb(SelectedItem);
                int Index = ListItems.SelectedIndex;
                ListItems.UnselectAll();
                DataList.Remove(SelectedItem);
                DataList.Insert(Index, ItemInDb);
                ListItems.SelectedItem = ItemInDb;
                StocksDataControl.StockData = ItemInDb;
                ListItems.UpdateLayout();
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        #endregion

        #region Update UI

        /// <summary>
        /// Method that show the Item Pannel
        /// </summary>
        private void ShowItemPannel()
        {
            gsSplitter.IsEnabled = true;
            rdItemPannel.Height = ViewItemPannel;
            rdOperationPannel.Height = HideComponent;
            btnEdit.Visibility = btnViewData.Visibility = Visibility.Hidden;
            GridList.IsEnabled = false;
        }

        /// <summary>
        /// Method that show the Item Pannel
        /// </summary>
        private void HideItemPannel()
        {
            rdItemPannel.Height = HideComponent;
            rdOperationPannel.Height = ViewOperationPannel;
            gsSplitter.IsEnabled = false;
            if (DataList.Count > 0)
            {
                rdSearchPannel.Height = ViewSearchPannel;
                ActualizeButtonBar();
            }
            else rdSearchPannel.Height = HideComponent;
            GridList.IsEnabled = true;
        }

        /// <summary>
        /// Method that actualize the Button Bar look
        /// </summary>
        private void ActualizeButtonBar()
        {
            if (ListItems.SelectedItem != null)
            {
                btnEdit.Visibility = btnViewData.Visibility = Visibility.Visible;
            }
            else btnEdit.Visibility = btnViewData.Visibility = Visibility.Hidden;
        }

        #endregion
    }
}
