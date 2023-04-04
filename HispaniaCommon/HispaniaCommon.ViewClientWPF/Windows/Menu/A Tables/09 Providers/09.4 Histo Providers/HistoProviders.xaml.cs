#region Librerias uasadas por la clase

using HispaniaCommon.ViewModel;
using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.Theme;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

#endregion

namespace HispaniaCommon.ViewClientWPF.Windows
{
    public enum HistoProvidersMode
    {
        Historic,
        ProviderOrderMovementInput,
        ProviderOrderMovementLoad
    }

    /// <summary>
    /// Interaction logic for HistoProviders.xaml
    /// </summary>
    public partial class HistoProviders : Window
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
        /// Show the Report button.
        /// </summary>
        private GridLength ViewReportButtonPannel = new GridLength(120.0);

        /// <summary>
        /// Show the Item Pannel.
        /// </summary>
        private GridLength ViewItemPannel = new GridLength(1.0, GridUnitType.Star);

        /// <summary>
        /// Show the Search Pannel.
        /// </summary>
        private GridLength ViewSearchPannel = new GridLength(30.0);

        /// <summary>
        /// Show the Provider Order Movement Pannel.
        /// </summary>
        private GridLength ViewProviderOrderMovementPannel = new GridLength(80.0);

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
        private ProvidersView m_Data = null;
            
        /// <summary>
        /// Store the Mode of the window use for work.
        /// </summary>
        private HistoProvidersMode m_Mode = HistoProvidersMode.Historic;

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private ObservableCollection<HistoProvidersView> m_DataList = new ObservableCollection<HistoProvidersView>();

        /// <summary>
        /// Get or Set the data to show in List of Items selected.
        /// </summary>
        public ObservableCollection<HistoProvidersView> m_HistoProviderSelected = new ObservableCollection<HistoProvidersView>();

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
        public ProvidersView Data
        {
            get
            {
                return (m_Data);
            }
            set
            {
                if (value != null)
                {
                    m_Data = value;
                    LoadDataInWindow(m_Data);
                }
                else throw new ArgumentNullException("Error, no s'ha trobat el Client a carregar.");
            }
        }

        /// <summary>
        /// Get or Set the data to show in List of Items.
        /// </summary>
        public ObservableCollection<HistoProvidersView> DataList
        {
            get
            {
                return (m_DataList);
            }
            set
            {
                if (value != null) m_DataList = value;
                else m_DataList = new ObservableCollection<HistoProvidersView>();
                ListItems.ItemsSource = m_DataList;
                ListItems.DataContext = this;
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Filter = UserFilter;
                if (m_DataList.Count > 0)
                {
                    rdSearchPannel.Height = ViewSearchPannel;
                    ListItems.SelectedIndex = 0;
                }
                else rdSearchPannel.Height = HideComponent;
            }
        }

        /// <summary>
        /// Stores and initialize the mode of window.
        /// </summary>
        private HistoProvidersMode Mode
        {
            get
            {
                return m_Mode;
            }
            set
            {
                m_Mode = value;
                SetFromModeUI();
            }
        }

        /// <summary>
        /// Get or Set the data to show in List of Items selected.
        /// </summary>
        public ObservableCollection<HistoProvidersView> HistoProviderSelected
        {
            get
            {
                return m_HistoProviderSelected;
            }
            private set
            {
                if (value != null) m_HistoProviderSelected = value;
                else m_HistoProviderSelected = new ObservableCollection<HistoProvidersView>();
            }        
        }

        /// <summary>
        /// Get or Set if the manager of the data change for the Good has active.
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
        public HistoProviders(ApplicationType AppType, HistoProvidersMode Mode)
        {
            InitializeComponent();
            Initialize(AppType, Mode);
        }

        /// <summary>
        /// Method that initialize the window.
        /// </summary>
        /// <param name="Mode">Mode as is used the this window.</param>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        private void Initialize(ApplicationType AppType, HistoProvidersMode Mode)
        {
            //  Actualize the Window.
                ActualizeWindowComponents(AppType, Mode);
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
        private void ActualizeWindowComponents(ApplicationType AppType, HistoProvidersMode Mode)
        {
            //  Actualize properties of this Window.
                this.AppType = AppType;
                this.Mode = Mode;
            //  Apply Theme to window.
                ThemeManager.ActualTheme = AppTheme;
            //  Initialize state of Window components.
                rdSearchPannel.Height = HideComponent;
        }

        /// <summary>
        /// Method that load data in Window components.
        /// </summary>
        private void LoadDataInWindowComponents()
        {
            //  Set Data into the Window.
                ListItems.ItemsSource = m_DataList;
                DataContext = DataList;
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Filter = UserFilter;
                cbFieldItemToSearch.ItemsSource = HistoProvidersView.Fields;
                cbFieldItemToSearch.DisplayMemberPath = "Key";
                cbFieldItemToSearch.SelectedValuePath = "Value";
                if (HistoProvidersView.Fields.Count > 0) cbFieldItemToSearch.SelectedIndex = 0;
        }

        /// <summary>
        /// Load the data of the Company into the Window
        /// </summary>
        private void LoadDataInWindow(ProvidersView providerView)
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Load the data in the window.
                if (providerView == null) return;
                tbProviderCode.Text = GlobalViewModel.GetStringFromIntIdValue(providerView.Provider_Id);
                tbProviderDescription.Text = providerView.Company_Name;
                ActualizeButtonState();
                ActualizeUnitInformation();
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
            //  Get Acces to the object and the property name To Filter.
                HistoProvidersView ItemData = (HistoProvidersView)item;
                String ProperyName = (string) cbFieldItemToSearch.SelectedValue;
            //  Apply the filter
                object valueToTest = ItemData.GetType().GetProperty(ProperyName).GetValue(ItemData);
                if (valueToTest == null) return (false);
                else
                {
                    if (valueToTest is DateTime)
                    {
                        return GlobalViewModel.GetDateForUI((DateTime)valueToTest).Contains(tbStoreAddressToSearch.Text);
                    }
                    else return ((valueToTest.ToString().ToUpper()).Contains(tbStoreAddressToSearch.Text.ToUpper()));
                }
        }
        
        #endregion

        #region Managers

        /// <summary>
        /// Method that define the managers needed for the user operations in the Window
        /// </summary>
        private void LoadManagers()
        {
            //  ListView Item selected.
                ListItems.SelectionChanged += ListItems_SelectionChanged;
            //  TextBox
                tbStoreAddressToSearch.TextChanged += TbStoreAddressToSearch_TextChanged;
            //  Button Search Clients
                btnClearSelection.Click += BtnClearSelection_Click;
                btnTransfer.Click += BtnTransfer_Click;
                btnRefresh.Click += BtnRefresh_Click;
        }

        #region Filter

        /// <summary>
        /// Manage the search of Items in the list.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void TbStoreAddressToSearch_TextChanged(object sender, TextChangedEventArgs e)
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
                ActualizeHistoProvidersRefreshFromDb();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al refrescar els valors dels Històrics de Client.", MsgManager.ExcepMsg(ex)));
            }
        }

        #endregion

        #region Clear Selection

        /// <summary>
        /// Clear the items selected in the list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnClearSelection_Click(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                if (Mode == HistoProvidersMode.ProviderOrderMovementInput)
                {
                    if (ListItems.SelectedItems.Count > 0)
                    {
                        ListItems.SelectedItems.Clear();
                        ActualizeButtonState();
                        ActualizeUnitInformation();
                    }
                }
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region Transfer

        /// <summary>
        /// Transfer the selected items and close the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnTransfer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActualizeHistoProvidersFromDb();
                Close();

            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al transferir els elements seleccionats a una Comanda de Client o Albarà.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
            }
        }

        #endregion

        #endregion

        #region ListView

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
                if (Mode == HistoProvidersMode.ProviderOrderMovementInput)
                {
                    if (ListItems.SelectedItems.Count > 0)
                    {
                        ActualizeButtonState();
                        try
                        {
                            ActualizeUnitInformation((HistoProvidersView)(ListItems.SelectedItems[ListItems.SelectedItems.Count - 1]));
                        }
                        catch (Exception ex)
                        {
                            MsgManager.ShowMessage(
                                string.Format("Error al carregar les dades del històric seleccionat.\r\nDetalls:{0}.",
                                              MsgManager.ExcepMsg(ex)));
                        }
                    }
                }
                else if (Mode == HistoProvidersMode.ProviderOrderMovementLoad)
                {
                    if (ListItems.SelectedItem != null)
                    {
                        ActualizeButtonState();
                        try
                        {
                            ActualizeUnitInformation((HistoProvidersView)ListItems.SelectedItem);
                        }
                        catch (Exception ex)
                        {
                            MsgManager.ShowMessage(
                                string.Format("Error al carregar les dades del històric seleccionat.\r\nDetalls:{0}.",
                                              MsgManager.ExcepMsg(ex)));
                        }
                    }
                }
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #endregion
                
        #region Database Operations
        		        
        private void ActualizeHistoProvidersRefreshFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                DataList = GlobalViewModel.Instance.HispaniaViewModel.GetHistoProviders(Data.Provider_Id);
            //  Deactivate managers
                DataChangedManagerActive = true;
        }


        private void ActualizeHistoProvidersFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                HistoProviderSelected = new ObservableCollection<HistoProvidersView>();
                if (Mode == HistoProvidersMode.ProviderOrderMovementInput)
                {
                    foreach (HistoProvidersView histoProvider in new ArrayList(ListItems.SelectedItems))
                    {
                        HistoProvidersView histoProviderInDb = GlobalViewModel.Instance.HispaniaViewModel.GetHistoProviderFromDb(histoProvider);
                        ListItems.SelectedItems.Remove(histoProvider);
                        DataList.Remove(histoProvider);
                        DataList.Add(histoProviderInDb);
                        ListItems.SelectedItems.Add(histoProviderInDb);
                        HistoProviderSelected.Add(histoProviderInDb);
                    }
                }
                else if (Mode == HistoProvidersMode.ProviderOrderMovementLoad)
                {
                    HistoProvidersView histoProvider = (HistoProvidersView)ListItems.SelectedItem;
                    HistoProvidersView histoProviderInDb = GlobalViewModel.Instance.HispaniaViewModel.GetHistoProviderFromDb(histoProvider);
                    ListItems.SelectedItem = null;
                    DataList.Remove(histoProvider);
                    DataList.Add(histoProviderInDb);
                    ListItems.SelectedItem = histoProviderInDb;
                    HistoProviderSelected.Add(histoProviderInDb);
                }
                ListItems.UpdateLayout();
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        #endregion

        #region Update UI

        private void SetFromModeUI()
        {
            switch (Mode)                 
            {
                case HistoProvidersMode.ProviderOrderMovementInput:
                     ListItems.SelectionMode = SelectionMode.Extended;
                     rdProviderOrderMovementPannel.Height = ViewProviderOrderMovementPannel;
                     break;
                case HistoProvidersMode.ProviderOrderMovementLoad:
                     ListItems.SelectionMode = SelectionMode.Single;
                     rdProviderOrderMovementPannel.Height = ViewProviderOrderMovementPannel;
                     break;
                case HistoProvidersMode.Historic:
                default:
                     ListItems.SelectionMode = SelectionMode.Single;
                     rdProviderOrderMovementPannel.Height = HideComponent;
                     break;
            }
        }

        private void ActualizeButtonState()
        {
            if (Mode == HistoProvidersMode.ProviderOrderMovementInput)
            {
                btnTransfer.IsEnabled = ListItems.SelectedItems.Count > 0;
                btnClearSelection.IsEnabled = ListItems.SelectedItems.Count > 0;
            }
            else if (Mode == HistoProvidersMode.ProviderOrderMovementLoad)
            {
                btnTransfer.IsEnabled = ListItems.SelectedItem != null;
                btnClearSelection.IsEnabled = ListItems.SelectedItem != null;
            }
        }

        private void ActualizeUnitInformation(HistoProvidersView histoCutomer = null)
        {
            if (histoCutomer == null)
            {
                tbBillingUnitAvailable.Text = string.Empty;
                tbBillingUnitStocks.Text = string.Empty;
                tbShippingUnitAvailable.Text = string.Empty;
                tbShippingUnitStocks.Text = string.Empty;
            }
            else
            {
                tbBillingUnitAvailable.Text = histoCutomer.Good_BillingUnitAvailable_Str;
                tbBillingUnitStocks.Text = histoCutomer.Good_BillingUnitStocks_Str;
                tbShippingUnitAvailable.Text = histoCutomer.Good_ShippingUnitAvailable_Str;
                tbShippingUnitStocks.Text = histoCutomer.Good_ShippingUnitStocks_Str;
            }
        }

        #endregion
    }
}
