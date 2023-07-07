#region Librerias usadas por la ventana

using HispaniaCommon.ViewClientWPF.UserControls;
using HispaniaCommon.ViewClientWPF.Windows.Common;
using HispaniaCommon.ViewModel;
using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.Theme;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

#endregion

namespace HispaniaCommon.ViewClientWPF.Windows
{
    /// <summary>
    /// Interaction logic for ProviderOrders.xaml
    /// </summary>
    public partial class ProviderOrders : Window
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
        /// Show the Button Search with for Date fields.
        /// </summary>
        private GridLength ViewDateFilterButtonSearchPannel = new GridLength(230.0);

        /// <summary>
        /// Show the Item Pannel.
        /// </summary>
        private GridLength ViewDateFilterPannel = new GridLength(1.0, GridUnitType.Star);

        /// <summary>
        /// Show the Item Pannel.
        /// </summary>
        private GridLength ViewTextFilterPannel = new GridLength(1.0, GridUnitType.Star);

        /// <summary>
        /// Show the Search Pannel button.
        /// </summary>
        private GridLength ViewButtonPannel = new GridLength(150.0);

        /// <summary>
        /// Show the Print button.
        /// </summary>
        private GridLength ViewEditButtonPannel = new GridLength(110.0);

        /// <summary>
        /// Show the Print button.
        /// </summary>
        private GridLength ViewPrintButtonPannel = new GridLength(110.0);

        /// <summary>
        /// Show the Print button.
        /// </summary>
        private GridLength ViewAddButtonPannel = new GridLength(120.0);

        /// <summary>
        /// Show the Print button.
        /// </summary>
        private GridLength ViewViewButtonPannel = new GridLength(120.0);

        /// <summary>
        /// Show the Create Report button.
        /// </summary>
        private GridLength ViewCreateReportButtonPannel = new GridLength(120.0);

        /// <summary>
        /// Show the Send by EMail button.
        /// </summary>
        private GridLength ViewSendByEMailButtonPannel = new GridLength(140.0);

        /// <summary>
        /// Show the Create Bill button.
        /// </summary>
        private GridLength ViewCreateBillButtonPannel = new GridLength(130.0);
        
        /// <summary>
        /// Show the Create Delivery Note button.
        /// </summary>
        private GridLength ViewCreateDeliveryNoteButtonPannel = new GridLength(125.0);

        /// <summary>
        /// Show the Cahnge Date button.
        /// </summary>
        private GridLength ViewChangeDateButtonPannel = new GridLength(125.0);

        /// <summary>
        /// Show the Split Provider Order button.
        /// </summary>
        private GridLength ViewSplitProviderOrderButtonPannel = new GridLength(155.0);

        /// <summary>
        /// Show the Item Pannel.
        /// </summary>
        private GridLength ViewItemPannel = new GridLength(600.0);

        /// <summary>
        /// Show the Serach Pannel.
        /// </summary>
        private GridLength ViewSearchPannel = new GridLength(30.0);

        /// <summary>
        /// Show the Operation Pannel.
        /// </summary>
        private GridLength ViewOperationPannel = new GridLength(30.0);

        /// <summary>
        /// Show the According label.
        /// </summary>
        private GridLength ViewLabelAccording = new GridLength(65.0);

        /// <summary>
        /// Show the According checkbox.
        /// </summary>
        private GridLength ViewCheckBoxAccording = new GridLength(22.0);

        /// <summary>
        /// Show the Provider Pannel.
        /// </summary>
        private GridLength ViewProviderPannel = new GridLength(80.0);

        /// <summary>
        /// Show the Bill label.
        /// </summary>
        private GridLength ViewLabelBill = new GridLength(75.0);

        /// <summary>
        /// Show the Bill checkbox.
        /// </summary>
        private GridLength ViewCheckBoxBill = new GridLength(22.0);

        /// <summary>
        /// Hide the ItemPannel.
        /// </summary>
        private GridLength HideComponent = new GridLength(0.0);


        /// <summary>
        /// Show the Print Pannel.
        /// </summary>
        private GridLength ViewPrintPannel = new GridLength(30.0);

        /// <summary>
        /// Hide the PrintPannel.
        /// </summary>
        private GridLength HidePrintPannel = new GridLength(0.0);

        #endregion

        #region Attributes

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private ObservableCollection<ProviderOrdersView> m_DataList = new ObservableCollection<ProviderOrdersView>();

        /// <summary>
        /// Store the data of the Provider.
        /// </summary>
        private ProvidersView m_Provider = null;

        /// <summary>
        /// Store the value that indicates if the Window allows impression or not.
        /// </summary>
        private bool m_Print = false;

        /// <summary>
        /// Store the value that indicates if the Window allows impression or not.
        /// </summary>
        private bool m_PrintProviderOrder = true;

        /// <summary>
        /// Store the value that indicates if the Window filter by Provider the ProviderOrders that show or not.
        /// </summary>
        private bool m_FilterByProvider = false;

        /// <summary>
        /// Store a value that indicate if the Window can show Delivery Notes.
        /// </summary>
        private bool m_CanShowDeliveryNotes = true;

        /// <summary>
        /// Store a value that indicate if the Window can change Date of the Provider Order or Delivery Notes.
        /// </summary>
        private bool m_CanChangeDate = false;

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
        /// Get or Set the data to show in List of Items.
        /// </summary>
        public ProvidersView Provider
        {
            get
            {
                return (m_Provider);
            }
            set
            {
                //  Actualize the value
                    m_Provider = value;
                    if (m_Provider != null) 
                    {
                        //  Set the information of the provider in Provider Info fields.
                            tbProviderCode.Text = GlobalViewModel.GetStringFromIntIdValue(m_Provider.Provider_Id);
                            tbProviderDescription.Text = m_Provider.Name;
                        //  Remove the option of filter by client id.
                            if (cbFieldItemToSearch.ItemsSource != null)
                            {
                                ((Dictionary<string, string>)cbFieldItemToSearch.ItemsSource).Remove("Numero de Client");
                            }
                        //  Refresh Button Bar
                            RefreshButtonBar();
                    }
            }
        }

        /// <summary>
        /// Get or Set the data to show in List of Items.
        /// </summary>
        public ObservableCollection<ProviderOrdersView> DataList
        {
            get
            {
                return (m_DataList);
            }
            set
            {
                //  Actualize the value
                    if (value != null) m_DataList = value;
                    else m_DataList = new ObservableCollection<ProviderOrdersView>();
                //  Deactivate managers
                    DataChangedManagerActive = false;
                //  Set up controls state
                    ListItems.ItemsSource = m_DataList;
                    ListItems.DataContext = this;
                    if (m_CanShowDeliveryNotes)
                    {
                        CollectionViewSource.GetDefaultView(ListItems.ItemsSource).SortDescriptions.Add(new SortDescription("DeliveryNote_Id", ListSortDirection.Descending));
                    }
                    else
                    {
                        CollectionViewSource.GetDefaultView(ListItems.ItemsSource).SortDescriptions.Add(new SortDescription("ProviderOrder_Id", ListSortDirection.Descending));
                    }
                    CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Filter = UserFilter;
                //  Initialize the filter options
                    InitDateFilter = NormalizeDateInit(dtpInitDateFilter.SelectedDate);
                    tbInitDateFilter.Text = GlobalViewModel.GetLongDateString(InitDateFilter);
                    EndDateFilter = NormalizeDateEnd(dtpEndDateFilter.SelectedDate);
                    tbEndDateFilter.Text = EndDateFilter is null ? "No seleccionda" : GlobalViewModel.GetLongDateString((DateTime)EndDateFilter);
                //  Reactivate managers
                    DataChangedManagerActive = true;
                //  Refresh Button Bar
                    RefreshButtonBar();
                //  Set the Focus at ther TextBox Search control.
                    tbItemToSearch.Focus();
            }
        }

        private DateTime InitDateFilter
        {
            get;
            set;
        }

        private DateTime? EndDateFilter
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set the value that indicates if the Window filter by Provider the ProviderOrders that show or not.
        /// </summary>
        public bool FilterByProvider
        {
            get
            {
                return (m_FilterByProvider);
            }
            set
            {
                m_FilterByProvider = value;
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

        /// <summary>
        /// Get or Set the value that indicates if the Window allows impression or not.
        /// </summary>
        private bool Print
        {
            get
            {
                return m_Print;
            }
        }

        /// <summary>
        /// Get the value that indicates if the Window allows impression or not.
        /// </summary>
        private bool PrintProviderOrder
        {
            get
            {
                return m_PrintProviderOrder;
            }
        }

        /// <summary>
        /// Get the value that indicate if the Window can change Date of the Provider Order or Delivery Notes.
        /// </summary>
        private bool CanChangeDate
        {
            get
            {
                return m_CanChangeDate;
            }
        }


        /// <summary>
        /// Get the value that indicate if the Window can show Delivery Notes.
        /// </summary>
        private bool CanShowDeliveryNotes
        {
            get
            {
                return m_CanShowDeliveryNotes;
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

        #region Foreign Tables Info

        /// <summary>
        /// Get or Set the Parameters
        /// </summary>
        public ParametersView Parameters
        {
            set
            {
                ProviderOrderDataControl.Parameters = value;
            }
        }

        /// <summary>
        /// Get or Set the Providers 
        /// </summary>
        public Dictionary<string, ProvidersView> Providers
        {
            set
            {
                ProviderOrderDataControl.Providers = value;
            }
        }

        /// <summary>
        /// Get or Set the SendTypes
        /// </summary>
        public Dictionary<string, SendTypesView> SendTypes
        {
            set
            {
                ProviderOrderDataControl.SendTypes = value;
            }
        }

        /// <summary>
        /// Get or Set the Effect Types
        /// </summary>
        public Dictionary<string, EffectTypesView> EffectTypes
        {
            set
            {
                ProviderOrderDataControl.EffectTypes = value;
            }
        }
                

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Main Builder of the windows.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        public ProviderOrders(ApplicationType AppType, bool Print = false, bool PrintProviderOrder = true, bool CanShowDeliveryNotes = true, bool CanChangeDate = false)
        {
            InitializeComponent();
            Initialize(AppType, Print, PrintProviderOrder, CanShowDeliveryNotes, CanChangeDate);
        }

        /// <summary>
        /// Method that initialize the window.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        private void Initialize(ApplicationType AppType, bool Print, bool PrintProviderOrder, bool CanShowDeliveryNotes, bool CanChangeDate)
        {
            //  Actualize the Window.
                ActualizeWindowComponents(AppType, Print, PrintProviderOrder, CanShowDeliveryNotes, CanChangeDate);
            //  Load Data in Window components.
                LoadDataInWindowComponents(CanShowDeliveryNotes);
            //  Load the managers of the controls of the Window.
                LoadManagers();
        }

        #endregion

        #region Standings

        /// <summary>
        /// Method that actualize the Window components.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        private void ActualizeWindowComponents(ApplicationType AppType, bool Print, bool PrintProviderOrder, bool CanShowDeliveryNotes, bool CanChangeDate)
        {
            //  Actualize properties of this Window.
                this.AppType = AppType;
                m_Print = Print;
                m_PrintProviderOrder = PrintProviderOrder;
                m_CanShowDeliveryNotes = CanShowDeliveryNotes;
                m_CanChangeDate = CanChangeDate;
            //  Update Control State
                if ((m_Print) || (m_CanChangeDate)) ListItems.SelectionMode = SelectionMode.Extended;
                else ListItems.SelectionMode = SelectionMode.Single;
                ProviderOrderDataControl.rdBill.Height = HideComponent;
                if (CanShowDeliveryNotes)
                {
                    if (m_Print)
                    {
                        Title = "Impressió d'Albarans i Creació de Factures";
                        OpText.Text = "Albarans";
                        DataListText.Text = "Albarans";
                    }
                    else
                    {
                        Title = m_CanChangeDate? "Canvi de Data dels Albarans" : "Gestió d'Albarans";
                        OpText.Text = "Albarans";
                        DataListText.Text = "Albarans";
                    }
                    ListItems.View = (GridView) FindResource("GridViewForDeliveryNotes");
                    cbFieldItemToSearch.ItemsSource = ProviderOrdersView.DeliveryNoteFields;
                }
                else
                {
                    Title = m_CanChangeDate ? "Canvi de Data de les Comandes de Proveidor" : "Gestió de Comandes de Proveidor";
                    OpText.Text = "Comandes de Proveidor";
                    DataListText.Text = "Comandes de Proveidor";
                    ListItems.View = (GridView)FindResource("GridViewForProviderOrders");
                    cbFieldItemToSearch.ItemsSource = ProviderOrdersView.Fields;
                    ProviderOrderDataControl.rdDeliveryNote.Height = HideComponent;
                }
            //  Apply Theme to window.
                ThemeManager.ActualTheme = AppTheme;
                ProviderOrderDataControl.AppType = AppType;
            //  Initialize state of Window components.
                rdItemPannel.Height = HideComponent;
                rdSearchPannel.Height = HideComponent;
                gsSplitter.IsEnabled = false;
                HideOrderOrProformaPannel();
        }

        /// <summary>
        /// Method that load data in Window components.
        /// </summary>
        private void LoadDataInWindowComponents(bool CanShowDeliveryNotes)
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Set Data into the Window.
                cbFieldItemToSearch.DisplayMemberPath = "Key";
                cbFieldItemToSearch.SelectedValuePath = "Value";
                if (ProviderOrdersView.Fields.Count > 0) cbFieldItemToSearch.SelectedIndex = 0;
            //  Activate managers
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
                if (cbFieldItemToSearch.SelectedIndex == -1)  return (true);
            //  Get Acces to the object to Filter.
                ProviderOrdersView ItemData = (ProviderOrdersView)item;
            //  Validate that the Provider Order belong at the Provider.
                if (FilterByProvider)
                {
                    if ((ItemData.Provider is null) || 
                        (!(ItemData.Provider is null) && (ItemData.Provider.Provider_Id != Provider.Provider_Id)))
                    {
                        return false;
                    }
                }
            //  Get Acces to the property name To Filter.
                String ProperyName = (string) cbFieldItemToSearch.SelectedValue;
            //  Apply the filter by selected field value
                if (ProperyName == "Date_Str")
                {
                    if ((ItemData.Date < InitDateFilter) || (ItemData.Date > EndDateFilter)) return false;
                }
                else if (ProperyName == "DeliveryNote_Date_Str")
                {
                    if ((ItemData.DeliveryNote_Date < InitDateFilter) || (ItemData.DeliveryNote_Date > EndDateFilter)) return false;
                }
                else
                {
                    if (!String.IsNullOrEmpty(tbItemToSearch.Text))
                    {
                        object valueToTest = ItemData.GetType().GetProperty(ProperyName).GetValue(ItemData);
                        if ((valueToTest is null) ||
                            (!(valueToTest.ToString().ToUpper()).Contains(tbItemToSearch.Text.ToUpper())))
                        {
                            return false;
                        }
                    }
                }
            //  Calculate the Visibility value with properties values.
                return ((!ItemData.HasBill) &&
                        ((PrintProviderOrder && !ItemData.HasDeliveryNote) ||
                         (Print && ItemData.HasDeliveryNote) ||
                         (!Print && !m_CanShowDeliveryNotes && !ItemData.HasDeliveryNote) ||
                         (m_CanShowDeliveryNotes && ItemData.HasDeliveryNote)));
        }

        #endregion

        #region Managers

        /// <summary>
        /// Method that define the managers needed for the user operations in the Window
        /// </summary>
        private void LoadManagers()
        {
            //  Window
                this.Closed += Providers_Closed;
            //  TextBox
                tbItemToSearch.TextChanged += TbItemToSearch_TextChanged;
            //  Button 
                btnAdd.Click += BtnAdd_Click;
                btnCopy.Click += BtnCopy_Click;
                btnEdit.Click += BtnEdit_Click;
                btnDelete.Click += BtnDelete_Click;
                btnViewData.Click += BtnViewData_Click;
                btnPrint.Click += BtnPrint_Click;
                btnSendByEmail.Click += BtnSendByEMail_Click;
                btnCreateBill.Click += BtnCreateBill_Click;
                btnRefresh.Click += BtnRefresh_Click;
                btnChangeDate.Click += BtnChangeDate_Click;
                btnProforma.Click += BtnProformaPrint_Click;
                btnOrder.Click += BtnOrderPrint_Click;
            //  Define ComboBox events to manage.
                cbFieldItemToSearch.SelectionChanged += CbFieldItemToSearch_SelectionChanged;
            //  DatePiker
                dtpInitDateFilter.SelectedDateChanged += SelectedDateChanged;
                dtpEndDateFilter.SelectedDateChanged += SelectedDateChanged;
            //  Define ListView events to manage.
                ListItems.SelectionChanged += ListItems_SelectionChanged;   
            //  Define ProviderDataControl events to manage.
                ProviderOrderDataControl.EvAccept += ProviderOrderDataControl_evAccept;
                ProviderOrderDataControl.EvCancel += ProviderOrderDataControl_evCancel;
                btnCreateExcel.Click += BtnCreateExcel_Click;
        }

        #region Filter

        /// <summary>
        /// Select all text in sender TextBox control.
        /// </summary>
        /// <param name="sender">TextBox control that has produced the event.</param>
        /// <param name="e">Parameters associateds to the event.</param>
        private void TBGotFocus(object sender, RoutedEventArgs e)
        {
            GlobalViewModel.Instance.SelectAllTextInGotFocusEvent(sender, e);
        }

        /// <summary>
        /// Manage the user selection of filter field
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void TbItemToSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Refresh();
                RefreshButtonBar();
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Manage the user selection of filter field
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CbFieldItemToSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Refresh();
                RefreshButtonBar();
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Manage the user selection of data filter fields
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                if (sender == dtpInitDateFilter)
                {
                    DateTime InitValidityPeriod = NormalizeDateInit(dtpInitDateFilter.SelectedDate);
                    DateTime? EndValidityPeriod = NormalizeDateEnd(dtpEndDateFilter.SelectedDate);
                    if ((EndValidityPeriod != null) && (InitValidityPeriod > (DateTime)EndValidityPeriod))
                    {
                        MsgManager.ShowMessage("Error, la data d'inici del període de filtratge no pot ser major que la de final.");
                        dtpInitDateFilter.SelectedDate = InitDateFilter;
                    }
                    else
                    {
                        dtpEndDateFilter.DisplayDate = EndValidityPeriod ?? (DateTime)NormalizeDateEnd(DateTime.Now);
                        dtpEndDateFilter.DisplayDateStart = InitValidityPeriod;
                        InitDateFilter = InitValidityPeriod;
                    }
                    dtpInitDateFilter.DisplayDate = InitDateFilter;
                    tbInitDateFilter.Text = GlobalViewModel.GetLongDateString(InitDateFilter);
                }
                else if (sender == dtpEndDateFilter)
                {
                    dtpEndDateFilter.DisplayDateStart = InitDateFilter;
                    EndDateFilter = NormalizeDateEnd(dtpEndDateFilter.SelectedDate);
                    tbEndDateFilter.Text = EndDateFilter is null ? "No seleccionda" : GlobalViewModel.GetLongDateString((DateTime)EndDateFilter);
                }
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Refresh();
                RefreshButtonBar();
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region Button

        #region Change Date

        /// <summary>
        /// Manage the Date Change of Provider Order and Delivery Notes.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnChangeDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActualizeProviderOrdersFromDb();
                if (CanShowDeliveryNotes) ChangeDateDeliveryNote();
                else ChangeDateProviderOrder();
                ActualizeProviderOrdersRefreshFromDb();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al canviar la data a(ls) element(s) seleccionat(s).\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
            }
        }

        /// <summary>
        /// Manage the Date Change of Provider Order.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void ChangeDateProviderOrder()
        {
            try
            {
                if (ListItems.SelectedItems != null)
                {
                    DataSelector dsDate = new DataSelector(AppType);
                    dsDate.Show();
                    if (dsDate.Result == DataSelector.WindowResult.Accept)
                    {
                        DateTime NewDate = dsDate.Data;
                        string sQuestion = string.Format("Està segur que vol canviar la data dels elements seleccionats per la Data seleccionada: {0}", NewDate);
                        if (MsgManager.ShowQuestion(sQuestion) == MessageBoxResult.Yes)
                        {
                            StringBuilder sbInfoExecution = new StringBuilder(string.Empty);
                            foreach (ProviderOrdersView providerOrder in new ArrayList(ListItems.SelectedItems))
                            {
                                try
                                {
                                    string AddRemark = string.Format("Comanda de Proveidor canviada de la data {0} a la data {1}.\r\n", providerOrder.Date, NewDate);
                                    providerOrder.Date = NewDate;
                                    providerOrder.Remarks = AddRemark + providerOrder.Remarks;
                                    GlobalViewModel.Instance.HispaniaViewModel.UpdateProviderOrder(providerOrder);
                                    sbInfoExecution.AppendFormat("Data de la Comanda de Proveidor '{0}' actualitzada correctament.\r\n", providerOrder.ProviderOrder_Id);
                                }
                                catch (Exception)
                                {
                                    sbInfoExecution.AppendFormat("Error al actualitzar la data de la Comanda de Proveidor '{0}'.\r\n", providerOrder.ProviderOrder_Id);
                                }
                            }
                            MsgManager.ShowMessage(sbInfoExecution.ToString(), MsgType.Information);
                        }
                        else MsgManager.ShowMessage("Informació, operació cancel·lada per l'usuari.", MsgType.Information);
                    }
                    else MsgManager.ShowMessage("Informació, operació cancel·lada per l'usuari.", MsgType.Information);
                }
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage("Error, al canviar la data.\r\nDetalls: {0}.", MsgManager.ExcepMsg(ex));
            }
        }

        /// <summary>
        /// Manage the Date Change of Provider Order.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void ChangeDateDeliveryNote()
        {
            try
            {
                if (ListItems.SelectedItems != null)
                {
                    DataSelector dsDate = new DataSelector(AppType);
                    dsDate.Show();
                    if (dsDate.Result == DataSelector.WindowResult.Accept)
                    {
                        DateTime NewDate = dsDate.Data;
                        string sQuestion = string.Format("Està segur que vol canviar la data dels elements seleccionats per la Data seleccionada: {0}", NewDate);
                        if (MsgManager.ShowQuestion(sQuestion) == MessageBoxResult.Yes)
                        {
                            StringBuilder sbInfoExecution = new StringBuilder(string.Empty);
                            foreach (ProviderOrdersView providerOrder in new ArrayList(ListItems.SelectedItems))
                            {
                                try
                                {
                                    GlobalViewModel.Instance.HispaniaViewModel.UpdateProviderOrder(providerOrder, NewDate);
                                    sbInfoExecution.AppendFormat("Data de l'Albarà '{0}' actualitzat correctament.\r\n", providerOrder.DeliveryNote_Id);
                                }
                                catch (Exception)
                                {
                                    sbInfoExecution.AppendFormat("Error al actualitzar la data de l'Albarà '{0}'.\r\n", providerOrder.DeliveryNote_Id);
                                }
                            }
                            MsgManager.ShowMessage(sbInfoExecution.ToString(), MsgType.Information);
                        }
                        else MsgManager.ShowMessage("Informació, operació cancel·lada per l'usuari.", MsgType.Information);
                    }
                    else MsgManager.ShowMessage("Informació, operació cancel·lada per l'usuari.", MsgType.Information);
                }
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage("Error, al canviar la data.\r\nDetalls: {0}.", MsgManager.ExcepMsg(ex));
            }
        }

        #endregion

        #region Editing Provider Orders

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
                    ActualizeProviderOrderFromDb();
                    ProviderOrderDataControl.CtrlOperation = Operation.Show;
                    gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                    ShowItemPannel();
                }
                catch (Exception ex)
                {
                    ProviderOrdersView ProviderOrder = (ProviderOrdersView)ListItems.SelectedItem;
                    string ItemDescription = PrintProviderOrder ? string.Format("la Comanda de Proveidor '{0}'", ProviderOrder.ProviderOrder_Id)
                                                                : string.Format("l'Albarà '{0}'", ProviderOrder.DeliveryNote_Id_Str);
                    MsgManager.ShowMessage(string.Format("Error, al visualitzar les dades de {0}.\r\nDetalls:{1}", ItemDescription, MsgManager.ExcepMsg(ex)));
                }
            }
        }

        /// <summary>
        /// Manage the button for add Items in the list.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {                        
            try
            {
                ProviderOrderDataControl.CtrlOperation = Operation.Add;
                gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "EditableGroupBox");
                ShowItemPannel();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al afegir una Comanda de Proveidor.\r\nDetalls:{0}", MsgManager.ExcepMsg(ex)));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnCopy_Click( object sender, RoutedEventArgs e )
        {
            if(this.ListItems.SelectedItem != null)
            {
                ProviderOrdersView selected_order = (ProviderOrdersView)this.ListItems.SelectedItem;
                
                ProviderOrdersView new_providerorder = ProviderOrdersView.CreateCopy( selected_order.ProviderOrder_Id );
                
                this.DataList.Add( new_providerorder );

                this.ListItems.SelectedItem =  new_providerorder;
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
                    ActualizeProviderOrderFromDb();
                    if (!((ProviderOrdersView)ListItems.SelectedItem).HasBill)
                    {
                        if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(ListItems.SelectedItem, out string ErrMsg))
                        {
                            ProviderOrderDataControl.CtrlOperation = Operation.Edit;
                            gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "EditableGroupBox");
                            ShowItemPannel();
                            ProviderOrderDataControl.ShowGoodRemark();
                        }
                        else
                        {
                            MsgManager.ShowMessage(ErrMsg);
                        }
                    }
                    else
                    {
                        MsgManager.ShowMessage("Avís, no es pot modificar un albarà associat a una factura.\r\n" +
                                               "Per poder modificar-lo primer desvinculil de la factura.", MsgType.Warning);
                    }
                }
                catch (Exception ex)
                {
                    ProviderOrdersView ProviderOrder = (ProviderOrdersView)ListItems.SelectedItem;
                    string ItemDescription = PrintProviderOrder ? string.Format("la Comanda de Proveidor '{0}'", ProviderOrder.ProviderOrder_Id)
                                                                : string.Format("l'Albarà '{0}'", ProviderOrder.DeliveryNote_Id_Str);
                    MsgManager.ShowMessage(string.Format("Error, a l'editar {0}.\r\nDetalls:{1}", ItemDescription, MsgManager.ExcepMsg(ex)));
                }
            }
        }

        /// <summary>
        /// Manage the event produced when the operation in that was doing in the ProviderDataControl was Accepted.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void ProviderOrderDataControl_evAccept(ProviderOrdersView NewOrEditedProviderOrder, Guid DataManagementId)
        {
            try
            {
                ProviderOrdersView NewProviderOrder = new ProviderOrdersView(NewOrEditedProviderOrder);
                switch (ProviderOrderDataControl.CtrlOperation)
                {
                    case Operation.Add:
                         GlobalViewModel.Instance.HispaniaViewModel.CreateProviderOrder(NewProviderOrder, DataManagementId);
                         DataChangedManagerActive = false;
                         if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                         DataList.Add(NewProviderOrder);
                         DataChangedManagerActive = true;
                         ListItems.SelectedItem = NewProviderOrder;
                         ListItems.UpdateLayout();
                         gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                         HideItemPannel();
                         break;
                    case Operation.Edit:
                         GlobalViewModel.Instance.HispaniaViewModel.UpdateProviderOrder(NewProviderOrder, DataManagementId);
                         if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(ProviderOrderDataControl.ProviderOrder, out string ErrMsg))
                         {
                             MsgManager.ShowMessage(ErrMsg);
                         }
                         if (ProviderOrderDataControl.ProviderOrder.HasBill)
                         {
                             if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(ProviderOrderDataControl.ProviderOrder.Bill, out ErrMsg))
                             {
                                 MsgManager.ShowMessage(ErrMsg);
                             }
                         }
                         DataChangedManagerActive = false;
                         ProviderOrdersView SourceProviderOrder = (ProviderOrdersView)ListItems.SelectedItem;
                         if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                         DataList.Remove(SourceProviderOrder);
                         DataList.Add(GlobalViewModel.Instance.HispaniaViewModel.GetProviderOrderFromDb(NewProviderOrder));
                         DataChangedManagerActive = true;
                         ListItems.SelectedItem = NewProviderOrder;
                         ListItems.UpdateLayout();
                         gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                         HideItemPannel();
                         break;
                }
            }
            catch (Exception ex)
            {
                switch (ProviderOrderDataControl.CtrlOperation)
                {
                    case Operation.Edit:
                         string ItemDescription = PrintProviderOrder ? string.Format("la Comanda de Proveidor '{0}'", NewOrEditedProviderOrder.ProviderOrder_Id)
                                                                     : string.Format("l'Albarà '{0}'", NewOrEditedProviderOrder.DeliveryNote_Id_Str);
                         MsgManager.ShowMessage(string.Format("Error, al guardar les dades de l'edició de {0}.\r\nDetalls:{1}", ItemDescription, MsgManager.ExcepMsg(ex)));
                         break;
                    case Operation.Add:
                    default:
                         MsgManager.ShowMessage(string.Format("Error, al crear la Comanda de Proveidor.\r\nDetalls:{0}", MsgManager.ExcepMsg(ex)));
                         break;
                }
            }
        }

        /// <summary>
        /// Manage the event produced when the operation in that was doing in the ProviderDataControl was Cancelled.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void ProviderOrderDataControl_evCancel()
        {
            switch (ProviderOrderDataControl.CtrlOperation)
            {
                case Operation.Add:
                     MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                     break;
                case Operation.Edit:
                     MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                     if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(ProviderOrderDataControl.ProviderOrder, out string ErrMsg))
                     {
                         MsgManager.ShowMessage(ErrMsg);
                     }
                     if (ProviderOrderDataControl.ProviderOrder.HasBill)
                     {
                         if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(ProviderOrderDataControl.ProviderOrder.Bill, out ErrMsg))
                         {
                             MsgManager.ShowMessage(ErrMsg);
                         }
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

        #region Remove Provider Orders and Delivery Notes

        /// <summary>
        /// Manage the button for edit an Item of the list.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItem != null)
            {
                try
                {
                    ActualizeProviderOrderFromDb();
                    ProviderOrdersView ProviderOrdersToDelete = (ProviderOrdersView)ListItems.SelectedItem;
                    string ItemDescription = PrintProviderOrder ? string.Format("la Comanda de Proveidor '{0}'", ProviderOrdersToDelete.ProviderOrder_Id)
                                                                : string.Format("l'Albarà '{0}'", ProviderOrdersToDelete.DeliveryNote_Id);
                    if (MsgManager.ShowQuestion(string.Format("Està segur que vol esborrar {0} ?", ItemDescription)) == MessageBoxResult.Yes)
                    {
                        if (!ProviderOrdersToDelete.HasBill)
                        {
                            if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(ProviderOrdersToDelete, out string ErrMsg))
                            {
                                GlobalViewModel.Instance.HispaniaViewModel.DeleteProviderOrder(DataList[DataList.IndexOf(ProviderOrdersToDelete)]);
                                if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(ProviderOrdersToDelete, out ErrMsg))
                                {
                                    MsgManager.ShowMessage(ErrMsg);
                                }
                                DataChangedManagerActive = false;
                                if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                                DataList.Remove(ProviderOrdersToDelete);
                                DataChangedManagerActive = true;
                                ListItems.UpdateLayout();
                            }
                            else MsgManager.ShowMessage(ErrMsg);
                        }
                        else
                        {
                            MsgManager.ShowMessage(
                                string.Format("Error, a l'esborrar l'Albarà '{0}'.\r\nDetalls: {1}", 
                                              ProviderOrdersToDelete.ProviderOrder_Id,
                                              "L'Albarà pertany a una factura i no es pot esborrar un albarà associat a una factura. " +
                                              "Cal treure l'albarà de la factura abans de poder esborrar-lo."));
                        }
                    }
                    else MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                }
                catch (Exception ex)
                {
                    ProviderOrdersView ProviderOrdersToDelete = (ProviderOrdersView)ListItems.SelectedItem;
                    string ItemDescription = PrintProviderOrder ? string.Format("la Comanda de Proveidor '{0}'", ProviderOrdersToDelete.ProviderOrder_Id)
                                                                : string.Format("l'Albarà '{0}'", ProviderOrdersToDelete.DeliveryNote_Id);
                    MsgManager.ShowMessage(string.Format("Error, a l'esborrar {0}.\r\nDetalls: {1}", ItemDescription, MsgManager.ExcepMsg(ex)));
                }
            }
        }

        #endregion

        #region Print Report

        /// <summary>
        /// Manage the creation of the Report of selected Item and Print this Item.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            ShowOrderOrProformaPannel();           
        }

        /// <summary>
        /// Manage the creation of the Report of selected Item and Print this Item.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnOrderPrint_Click(object sender, RoutedEventArgs e)
        {            
            try
            {

                ActualizeProviderOrdersFromDb();
                if (PrintProviderOrder)
                {
                    if (ListItems.SelectionMode == SelectionMode.Single) PrintReportOneProviderOrder(false);
                    else
                    {
                        PrintReportProviderOrder(false);
                    }
                }
                else PrintReportDeliveryNote();
                DataList = GlobalViewModel.Instance.HispaniaViewModel.ProviderOrders;
                ListItems.UpdateLayout();
                HideOrderOrProformaPannel();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(
                    string.Format("Error, a l'imprimir el(s) element(s) seleccionat(s).\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
            }
        }

        /// <summary>
        /// Manage the creation of the Report of selected Item and Print this Item.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnProformaPrint_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {

                ActualizeProviderOrdersFromDb();
                if (PrintProviderOrder)
                {
                    if (ListItems.SelectionMode == SelectionMode.Single) PrintReportOneProviderOrder(true);
                    else
                    {
                        PrintReportProviderOrder(true);
                    }
                }
                else PrintReportDeliveryNote();
                DataList = GlobalViewModel.Instance.HispaniaViewModel.ProviderOrders;
                ListItems.UpdateLayout();
                HideOrderOrProformaPannel();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(
                    string.Format("Error, a l'imprimir el(s) element(s) seleccionat(s).\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
            }
        }

        

        private void PrintReportOneProviderOrder(bool isProfoma)
        {
            if (ListItems.SelectedItem != null)
            {
                ProviderOrdersView providerOrder = (ProviderOrdersView)ListItems.SelectedItem;
                {
                    if (ProviderOrdersReportView.CheckAndContinueIfExistReport(providerOrder, out string PDF_FileName, out string ErrMsg))
                    {
                        if (ProviderOrdersReportView.CreateReport(providerOrder, PDF_FileName, out ErrMsg, isProfoma))
                        {
                            if (ReportView.PrintReport(PDF_FileName, string.Format("Comanda de Proveidor '{0}'", providerOrder.ProviderOrder_Id), out ErrMsg))
                            {
                                if (ProviderOrdersReportView.UpdateProviderOrderFlag(providerOrder, ProviderOrderFlag.Print, true, out ErrMsg))
                                {
                                    DataList[DataList.IndexOf(providerOrder)].Print_ProviderOrder = true;
                                    MsgManager.ShowMessage(
                                       string.Format("Informe de la Comanda de Proveidor '{0}' enviat a impressió correctament.\r\n", 
                                                     providerOrder.ProviderOrder_Id),
                                       MsgType.Information);
                                    return;
                                }
                            }
                        }
                    }
                    MsgManager.ShowMessage(ErrMsg);
                }
            }
        }

        private void PrintReportProviderOrder(bool isProforma)
        {
            if (ListItems.SelectedItems != null)
            {
                StringBuilder sbInfoExecution = new StringBuilder(string.Empty);
                foreach (ProviderOrdersView providerOrder in new ArrayList(ListItems.SelectedItems))
                {
                    if (ProviderOrdersReportView.CheckAndContinueIfExistReport(providerOrder, out string PDF_FileName, out string ErrMsg))
                    {
                        if (ProviderOrdersReportView.CreateReport(providerOrder, PDF_FileName, out ErrMsg, isProforma))
                        {
                            if (ReportView.PrintReport(PDF_FileName, string.Format("Comanda de Proveidor '{0}'", providerOrder.ProviderOrder_Id), out ErrMsg))
                            {
                                if (ProviderOrdersReportView.UpdateProviderOrderFlag(providerOrder, ProviderOrderFlag.Print, true, out ErrMsg))
                                {
                                    DataList[DataList.IndexOf(providerOrder)].Print_ProviderOrder = true;
                                    sbInfoExecution.AppendFormat("Informe de la Comanda de Proveidor '{0}' enviat a impressió correctament.\r\n", providerOrder.ProviderOrder_Id);
                                    continue;
                                }
                            }
                        }
                    }
                    sbInfoExecution.AppendLine(ErrMsg);
                }
                MsgManager.ShowMessage(sbInfoExecution.ToString(), MsgType.Information);
            }
        }

        private void PrintReportDeliveryNote()
        {
            if (ListItems.SelectedItems != null)
            {
                StringBuilder sbInfoExecution = new StringBuilder(string.Empty);
                foreach (ProviderOrdersView providerOrder in ListItems.SelectedItems)
                {
                    if (DeliveryNotesReportView.CheckAndContinueIfExistReport(providerOrder, out string PDF_FileName, out string ErrMsg))
                    {
                        if (DeliveryNotesReportView.CreateReport(providerOrder, PDF_FileName, out ErrMsg))
                        {
                            if (ReportView.PrintReport(PDF_FileName, string.Format("Albarà '{0}'", providerOrder.DeliveryNote_Id), out ErrMsg))
                            {
                                if (DeliveryNotesReportView.UpdateDeliveryNoteFlag(providerOrder, DeliveryNoteFlag.Print, true, out ErrMsg))
                                {
                                    DataList[DataList.IndexOf(providerOrder)].Print = true;
                                    sbInfoExecution.AppendFormat("Informe de l'Albarà '{0}' enviat a impressió correctament.\r\n", providerOrder.DeliveryNote_Id);
                                    continue;
                                }
                            }
                        }
                    }
                    sbInfoExecution.AppendLine(ErrMsg);
                }
                MsgManager.ShowMessage(sbInfoExecution.ToString(), MsgType.Information);
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
            try
            {
                QueryViewModel.Instance.CreateExcelFromQuery(QueryType.ProviderConformedOrders);
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al crear l'Excel.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
            }
            finally
            {
                Mouse.OverrideCursor = Cursors.Arrow;               
            }
        }

        //private Dictionary<string, object> CreateParams()
        //{
        //    Dictionary<string, object> Params = null;         
            
        //    List<ProviderOrdersView> Orders = new List<ProviderOrdersView>();
        //    foreach (ProviderOrdersView ProviderOrder in m_DataList)
        //    {
        //        Orders.Add(new ProviderOrdersView(ProviderOrder));
        //    }
        //    Params = new Dictionary<string, object>
        //                 {
        //                    {"ProviderOrders", Orders}, 
        //                 };
        //    return Params;
        //}

        #endregion

        #region Send By Email Report

        /// <summary>
        /// Manage the creation of the Report of Items, if it's needed, and Send by EMail the Report.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnSendByEMail_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItems != null)
            {
                try
                {
                    ActualizeProviderOrdersFromDb();
                    if (PrintProviderOrder)
                    {
                        if (ListItems.SelectionMode == SelectionMode.Single) SendByEmailReportOneProviderOrder(true);
                        else
                        {
                            SendByEmailReportProviderOrder(true);
                        }
                    }
                    else SendByEmailReportDeliveryNote();
                    DataList = GlobalViewModel.Instance.HispaniaViewModel.ProviderOrders;
                    ListItems.UpdateLayout();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, a l'enviar per e-mail el(s) element(s) seleccionat(s).\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
        }

        private void SendByEmailReportOneProviderOrder(bool isProforma)
        {
            if (ListItems.SelectedItem != null)
            {
                ProviderOrdersView providerOrder = (ProviderOrdersView)ListItems.SelectedItem;
                {
                    if (ProviderOrdersReportView.CheckAndContinueIfExistReport(providerOrder, out string PDF_FileName, out string ErrMsg))
                    {
                        if (ProviderOrdersReportView.CreateReport(providerOrder, PDF_FileName, out ErrMsg, isProforma))
                        {
                            if (ProviderOrdersReportView.SendByEMail(providerOrder, PDF_FileName, out ErrMsg))
                            {
                                if (ProviderOrdersReportView.UpdateProviderOrderFlag(providerOrder, ProviderOrderFlag.SendByEMail, true, out ErrMsg))
                                {
                                    DataList[DataList.IndexOf(providerOrder)].SendByEMail_ProviderOrder = true;
                                    MsgManager.ShowMessage(string.Format("Creat correctament l'email de la comanda de proveidor '{0}'.\r\n", providerOrder.ProviderOrder_Id),
                                                           MsgType.Information);
                                    return;
                                }
                            }
                        }
                    }
                    MsgManager.ShowMessage(ErrMsg);
                }
            }
        }

        private void SendByEmailReportProviderOrder(bool isProforma)
        {
            StringBuilder sbInfoExecution = new StringBuilder(string.Empty);
            foreach (ProviderOrdersView providerOrder in ListItems.SelectedItems)
            {
                if (ProviderOrdersReportView.CheckAndContinueIfExistReport(providerOrder, out string PDF_FileName, out string ErrMsg))
                {
                    if (ProviderOrdersReportView.CreateReport(providerOrder, PDF_FileName, out ErrMsg, isProforma))
                    {
                        Process.Start(PDF_FileName);
                        if (ProviderOrdersReportView.SendByEMail(providerOrder, PDF_FileName, out ErrMsg))
                        {
                            if (ProviderOrdersReportView.UpdateProviderOrderFlag(providerOrder, ProviderOrderFlag.SendByEMail, true, out ErrMsg))
                            {
                                DataList[DataList.IndexOf(providerOrder)].SendByEMail_ProviderOrder = true;
                                sbInfoExecution.AppendFormat("Creat correctament l'email de la comanda de proveidor '{0}'.\r\n", providerOrder.ProviderOrder_Id);
                                continue;
                            }
                        }
                    }
                }
                sbInfoExecution.AppendLine(ErrMsg);
            }
            ListItems.UpdateLayout();
            MsgManager.ShowMessage(sbInfoExecution.ToString(), MsgType.Information);
        }

        private void SendByEmailReportDeliveryNote()
        {
            StringBuilder sbInfoExecution = new StringBuilder(string.Empty);
            foreach (ProviderOrdersView providerOrder in ListItems.SelectedItems)
            {
                if (DeliveryNotesReportView.CheckAndContinueIfExistReport(providerOrder, out string PDF_FileName, out string ErrMsg))
                {
                    if (DeliveryNotesReportView.CreateReport(providerOrder, PDF_FileName, out ErrMsg))
                    {
                        Process.Start(PDF_FileName);
                        if (DeliveryNotesReportView.SendByEMail(providerOrder, PDF_FileName, out ErrMsg))
                        {
                            if (DeliveryNotesReportView.UpdateDeliveryNoteFlag(providerOrder, DeliveryNoteFlag.SendByEMail, true, out ErrMsg))
                            {
                                DataList[DataList.IndexOf(providerOrder)].SendByEMail = true;
                                sbInfoExecution.AppendFormat("Creat correctament l'email  de l'albarà '{0}'.\r\n", providerOrder.DeliveryNote_Id);
                                continue;
                            }
                        }
                    }
                }
                sbInfoExecution.AppendLine(ErrMsg);
            }
            ListItems.UpdateLayout();
            MsgManager.ShowMessage(sbInfoExecution.ToString(), MsgType.Information);
        }

        #endregion

        #region Create Bill

        /// <summary>
        /// Manage the creation of the Bill associate at selected items.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnCreateBill_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItems != null)
            {
                string Question = string.Format("Està segur que vol crear factures per les comandes de proveidor seleccionades ?");
                if (MsgManager.ShowQuestion(Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        bool ExecuteCommand = true;
                        bool ProviderOrderWithBill = false;
                        LogViewModel.Instance.WriteLog("Create Bill - ActualizeProviderOrdersFromDb().");
                        ActualizeProviderOrdersFromDb();
                        SortedDictionary<int, DataForProviderBill> SourceProviderOrders = new SortedDictionary<int, DataForProviderBill>();
                        foreach (ProviderOrdersView providerOrder in ListItems.SelectedItems)
                        {
                            if (!GlobalViewModel.Instance.HispaniaViewModel.LockRegister(providerOrder, out string ErrMsg))
                            {
                                MsgManager.ShowMessage(ErrMsg);
                                ExecuteCommand = false;
                                break;
                            }
                            if (providerOrder.HasBill)
                            {
                                ProviderOrderWithBill = true;
                                ExecuteCommand = false;
                                break;
                            }
                            if (providerOrder.DeliveryNote_Year != DateTime.Now.Year)
                            {
                                MsgManager.ShowMessage(
                                   string.Format("Error, al crear la Factura de l'Albarà '{0}'.\r\n" +
                                                 "Detalls: no es pot crear una factura d'un Albarà que no pertany a l'any en curs.", 
                                                 providerOrder.DeliveryNote_Id_Str));
                                LogViewModel.Instance.WriteLog("Create Bill - DeliveryNote with Year different of now year.");
                                ExecuteCommand = false;
                                break;
                            }
                            ProvidersView Provider = providerOrder.Provider;
                            DateTime DeliveryNoteDate = providerOrder.DeliveryNote_Date;
                            if (!SourceProviderOrders.ContainsKey(providerOrder.Provider.Provider_Id))
                            {
                                List<ProviderOrdersView> ListProviderOrders = new List<ProviderOrdersView> { providerOrder };
                                SourceProviderOrders.Add(Provider.Provider_Id, new DataForProviderBill(DeliveryNoteDate, Provider, ListProviderOrders));
                            }
                            else
                            {
                                if (DeliveryNoteDate > SourceProviderOrders[Provider.Provider_Id].Bill_Date)
                                {
                                    SourceProviderOrders[Provider.Provider_Id].Bill_Date = DeliveryNoteDate;
                                }
                                SourceProviderOrders[Provider.Provider_Id].Movements.Add(providerOrder);
                            }
                        }
                        LogViewModel.Instance.WriteLog("Create Bill - SourceProviderOrders : {0}.", SourceProviderOrders.Count);
                        if (ExecuteCommand)
                        {
                            LogViewModel.Instance.WriteLog("Create Bill - CreateBillsFromProviderOrders(SourceProviderOrders).");
                            GlobalViewModel.Instance.HispaniaViewModel.CreateBillsFromProviderOrders(SourceProviderOrders);
                            DataChangedManagerActive = false;
                            if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                            DataList = GlobalViewModel.Instance.HispaniaViewModel.ProviderOrders;
                            DataChangedManagerActive = true;
                            ListItems.SelectedItem = null;
                            ListItems.UpdateLayout();
                            LogViewModel.Instance.WriteLog("Create Bill - ListItems -> Layout Updated.");
                        }
                        if (ProviderOrderWithBill)
                        {
                            LogViewModel.Instance.WriteLog("Create Bill - FilterDataListObjects().");
                            FilterDataListObjects();
                        }
                        foreach (DataForProviderBill ProviderOrders in SourceProviderOrders.Values)
                        {
                            foreach (ProviderOrdersView providerOrder in ProviderOrders.Movements)
                            {
                                if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(providerOrder, out string ErrMsg))
                                {
                                    MsgManager.ShowMessage(ErrMsg);
                                }
                            }
                        }
                        LogViewModel.Instance.WriteLog("Create Bill - UnlockRegisters.");
                    }
                    catch (Exception ex)
                    {
                        LogViewModel.Instance.WriteLog("Create Bill - Error, al crear les factures pels albarans seleccionats.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex));
                        MsgManager.ShowMessage(
                           string.Format("Error, al crear les factures pels albarans seleccionats.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                    }
                }
                else MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
            }
        }

        #endregion

        #region Create Delivery Note for Provider Order

        private void BtnCreateDeliveryNote_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItem != null)
            {
                try
                {
                    ActualizeProviderOrderFromDb();
                    ProviderOrdersView ProviderOrder = (ProviderOrdersView)ListItems.SelectedItem;
                    if (!ProviderOrder.HasDeliveryNote)
                    {
                        string Question = string.Format("Està segur que vol crear un albarà per la comanda de proveidor '{0}' ?", ProviderOrder.ProviderOrder_Id);
                        if (MsgManager.ShowQuestion(Question) == MessageBoxResult.Yes)
                        {
                            string ErrMsg;
                            try
                            {
                                if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(ListItems.SelectedItem, out ErrMsg))
                                {
                                    if (GlobalViewModel.Instance.HispaniaViewModel.CreateDeliveryNoteForProviderOrder(ProviderOrder, out ErrMsg))
                                    {
                                        GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(ListItems.SelectedItem, out ErrMsg);
                                        DataChangedManagerActive = false;
                                        if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                                        DataList.Remove(ProviderOrder);
                                        DataChangedManagerActive = true;
                                        ListItems.UpdateLayout();
                                        MsgManager.ShowMessage(string.Format("Informació, s'ha creat l'albarà '{0}' per la comanda de proveidor '{1}'.",
                                                                             ProviderOrder.DeliveryNote_Id, ProviderOrder.ProviderOrder_Id), MsgType.Information);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ErrMsg = string.Format("Error, al crear l'albarà per la Comanda de Proveidor '{0}'.\r\nDetalls: {1}",
                                                        ProviderOrder.ProviderOrder_Id, MsgManager.ExcepMsg(ex));
                            }
                            if (!string.IsNullOrEmpty(ErrMsg))
                            {
                                MsgManager.ShowMessage(ErrMsg);
                                GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(ProviderOrder, out ErrMsg);
                            }
                        }
                        else MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                    }
                    else
                    {
                        MsgManager.ShowMessage(string.Format("Ja s'ha creat un albarà per la comanda de proveidor '{0}' ?", ProviderOrder.ProviderOrder_Id),
                                                MsgType.Warning);
                        FilterDataListObjects();
                    }
                }
                catch (Exception ex)
                {
                    ProviderOrdersView ProviderOrder = (ProviderOrdersView)ListItems.SelectedItem;
                    MsgManager.ShowMessage(
                       string.Format("Error, al crear l'Albarà a partir de la Comanda de Proveidor '{0}'.\r\nDetalls: {1}",
                                     ProviderOrder.ProviderOrder_Id, MsgManager.ExcepMsg(ex)));
                }
            }
        }

        #endregion

        #region Split Provider Order for create Delivery Note

        private void BtnSplitProviderOrder_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItem != null)
            {
                try
                {
                    ActualizeProviderOrderFromDb();
                    ProviderOrdersView ProviderOrder = (ProviderOrdersView)ListItems.SelectedItem;
                    string Question = string.Format("Està segur que vol preparar la comanda de proveidor '{0}' per poder crear un Albarà ?", ProviderOrder.ProviderOrder_Id);
                    if (MsgManager.ShowQuestion(Question) == MessageBoxResult.Yes)
                    {
                        string ErrMsg;
                        try
                        {
                            if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(ListItems.SelectedItem, out ErrMsg))
                            {
                                if (GlobalViewModel.Instance.HispaniaViewModel.SplitProviderOrder(ProviderOrder, out ProviderOrdersView NewProviderOrder, out ErrMsg))
                                {
                                    GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(ListItems.SelectedItem, out ErrMsg);
                                    DataChangedManagerActive = false;
                                    if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                                    DataList.Remove(ProviderOrder);
                                    DataList.Add(GlobalViewModel.Instance.HispaniaViewModel.GetProviderOrderFromDb(ProviderOrder));
                                    DataList.Add(GlobalViewModel.Instance.HispaniaViewModel.GetProviderOrderFromDb(NewProviderOrder));
                                    DataChangedManagerActive = true;
                                    ListItems.SelectedItem = NewProviderOrder;
                                    ListItems.UpdateLayout();
                                    if (NewProviderOrder == null)
                                    {
                                        MsgManager.ShowMessage(
                                            string.Format("Informació, la comanda de proveidor '{0}' ja estava preparada per crear un Albarà.",
                                                          ProviderOrder.ProviderOrder_Id), MsgType.Information);
                                    }
                                    else
                                    {
                                        MsgManager.ShowMessage(
                                            string.Format("Informació, la comanda de proveidor '{0}' s'ha preparat per crear un Albarà.\r\n" +
                                                          "S'ha creat la comanda de proveidor '{1}' amb les línies no conformes de la comanda inicial.",
                                                          ProviderOrder.ProviderOrder_Id, NewProviderOrder.ProviderOrder_Id), MsgType.Information);
                                    }
                                }
                                else
                                {
                                    ErrMsg = string.Format("Error, al preparar la Comanda de Proveidor '{0}' per crear l'Albarà.\r\nDetalls: {1}",
                                                            ProviderOrder.ProviderOrder_Id, ErrMsg);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrMsg = string.Format("Error, al preparar la Comanda de Proveidor '{0}' per crear l'Albarà.\r\nDetalls: {1}",
                                                    ProviderOrder.ProviderOrder_Id, MsgManager.ExcepMsg(ex));
                        }
                        if (!string.IsNullOrEmpty(ErrMsg))
                        {
                            MsgManager.ShowMessage(ErrMsg);
                            GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(ProviderOrder, out ErrMsg);
                        }
                    }
                    else MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                }
                catch (Exception ex)
                {
                    ProviderOrdersView ProviderOrder = (ProviderOrdersView)ListItems.SelectedItem;
                    MsgManager.ShowMessage(
                       string.Format("Error, al separar la Comanda de Proveidor '{0}'.\r\nDetalls: {1}", 
                                     ProviderOrder.ProviderOrder_Id,
                                     MsgManager.ExcepMsg(ex)));
                }
            }
        }

        #endregion
									
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
                ActualizeProviderOrdersRefreshFromDb();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al refrescar els valors.", MsgManager.ExcepMsg(ex)));
            }
        }

        #endregion

        #endregion

        #region ListItems

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
                RefreshButtonBar();
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region Window

        private void Providers_Closed(object sender, EventArgs e)
        {
            if (ProviderOrderDataControl.CtrlOperation == Operation.Edit)
            {
                if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(ProviderOrderDataControl.ProviderOrder, out string ErrMsg))
                {
                    MsgManager.ShowMessage(ErrMsg);
                }
            }
        }

        #endregion

        #endregion
                
        #region Database Operations
		        
        private void ActualizeProviderOrdersRefreshFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                bool IsDataRefreshed = false;
                if (Print == false && PrintProviderOrder == true && m_CanShowDeliveryNotes == false)
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.ProviderOrdersWindow);
                    IsDataRefreshed = true;
                }
                else if (Print == false && PrintProviderOrder == false && m_CanShowDeliveryNotes == true)
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.DeliveryNotesWindow);
                    IsDataRefreshed = true;
                }
                else if (Print == true && PrintProviderOrder == false && m_CanShowDeliveryNotes == true)
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.DeliveryNotesPrintWindow);
                    IsDataRefreshed = true;
                }
                if (IsDataRefreshed)
                {
                    Providers = GlobalViewModel.Instance.HispaniaViewModel.ProvidersActiveDict;
                    SendTypes = GlobalViewModel.Instance.HispaniaViewModel.SendTypesDict;
                    EffectTypes = GlobalViewModel.Instance.HispaniaViewModel.EffectTypesDict;
                    Parameters = GlobalViewModel.Instance.HispaniaViewModel.Parameters;
                    DataList = GlobalViewModel.Instance.HispaniaViewModel.ProviderOrders;
                }
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        private void ActualizeProviderOrdersFromDb()
        {
            if (ListItems.SelectionMode == SelectionMode.Single) ActualizeProviderOrderFromDb();
            else ActualizeMultipleProviderOrdersFromDb();
        }

        private void ActualizeMultipleProviderOrdersFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                //List<ProviderOrdersView> ItemsInDb = new List<ProviderOrdersView>(ListItems.SelectedItems.Count);
                foreach (ProviderOrdersView providerOrder in new ArrayList(ListItems.SelectedItems))
                {
                    ProviderOrdersView providerInDb = GlobalViewModel.Instance.HispaniaViewModel.GetProviderOrderFromDb(providerOrder);
                    //ItemsInDb.Add(providerInDb);
                    int Index = DataList.IndexOf(providerOrder);
                    ListItems.SelectedItems.Remove(providerOrder);
                    DataList.Remove(providerOrder);
                    DataList.Insert(Index, providerInDb);
                    ListItems.SelectedItems.Add(providerInDb);
                }
                ListItems.UpdateLayout();
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        private void ActualizeProviderOrderFromDb()
        {
            //  Update selected item if there are one.
                if (ListItems.SelectedItem != null)
                {
                    //  Deactivate managers
                        DataChangedManagerActive = false;
                    //  Actualize Item Information From DataBase
                        ProviderOrdersView SelectedItem = (ProviderOrdersView)ListItems.SelectedItem;
                        ProviderOrdersView ItemInDb = GlobalViewModel.Instance.HispaniaViewModel.GetProviderOrderFromDb(SelectedItem);
                        int Index = ListItems.SelectedIndex;
                        ListItems.UnselectAll();
                        DataList.Remove(SelectedItem);
                        DataList.Insert(Index, ItemInDb);
                        ListItems.SelectedItem = ItemInDb;
                        ProviderOrderDataControl.ProviderOrder = ItemInDb;
                        ListItems.UpdateLayout();
                    //  Deactivate managers
                        DataChangedManagerActive = true;
                }
        }
        
        #endregion

        #region Update IU Methods

        private void FilterDataListObjects()
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Refresh();
                RefreshButtonBar();
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Method that show the Item Pannel
        /// </summary>
        private void ShowItemPannel()
        {
            gsSplitter.IsEnabled = true;
            rdItemPannel.Height = ViewItemPannel;
            rdOperationPannel.Height = HideComponent;
            RefreshButtonBar(true);
            GridList.IsEnabled = false;
        }

        /// <summary>
        /// Method that show the Item Pannel
        /// </summary>
        private void HideOrderOrProformaPannel()
        {
            rdPrintPannel.Height = HidePrintPannel;            
            gsSplitter.IsEnabled = false;
            RefreshButtonBar();
            GridList.IsEnabled = true;
        }


        /// <summary>
        /// Method that show the Item Pannel
        /// </summary>
        private void ShowOrderOrProformaPannel()
        {
            gsSplitter.IsEnabled = true;
            rdPrintPannel.Height = ViewPrintPannel;           
            RefreshButtonBar(true);
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
            RefreshButtonBar();
            GridList.IsEnabled = true;
        }

        /// <summary>
        /// Refresh Button Bar with nowadays state of the formulary.
        /// </summary>
        /// <param name="IsEditing"></param>
        private void RefreshButtonBar(bool IsEditing = false)
        {
            bool NotViewingProvider = Provider is null;
            rdProviderInfo.Height = NotViewingProvider ? HideComponent : ViewProviderPannel;
            rdSearchPannel.Height = (DataList.Count > 0) ? ViewSearchPannel : HideComponent;
            
            btnAdd.Visibility = (!Print && !IsEditing && !m_CanShowDeliveryNotes && NotViewingProvider && !CanChangeDate) ?
                                 Visibility.Visible : Visibility.Collapsed;

            bool HasItemSelected = !(ListItems.SelectedItem is null);
            ProviderOrdersView ProviderOrder = HasItemSelected ? (ProviderOrdersView)ListItems.SelectedItem : null;
            bool PrintControlVisibility = ((Print || PrintProviderOrder) && HasItemSelected && !CanChangeDate);
            
            btnPrint.Visibility = PrintControlVisibility ? Visibility.Visible : Visibility.Collapsed;

            btnSendByEmail.Visibility = (PrintControlVisibility ? 
                                Visibility.Visible : Visibility.Collapsed);

            bool CanCreateBill = Print && HasItemSelected && !ProviderOrder.HasBill && !CanChangeDate;

            btnCreateBill.Visibility= ( CanCreateBill ? Visibility.Visible : Visibility.Collapsed );

            bool CanEditOrDelete = !Print && HasItemSelected && !IsEditing && !CanChangeDate;
            
            btnEdit.Visibility  = ( CanEditOrDelete ? Visibility.Visible : Visibility.Collapsed);

            btnDelete.Visibility = ( CanEditOrDelete ? Visibility.Visible : Visibility.Collapsed);

            bool CanSplitProviderOrder = CanEditOrDelete && !ProviderOrder.HasDeliveryNote && !CanChangeDate;

            btnChangeDate.Visibility = ( CanSplitProviderOrder ? Visibility.Visible : Visibility.Collapsed);

            btnViewData.Visibility = ( ( HasItemSelected && !CanChangeDate ) ? Visibility.Visible : Visibility.Collapsed);

            bool CanCreateDeliveryNote = !Print && HasItemSelected && !ProviderOrder.HasDeliveryNote && !m_CanShowDeliveryNotes && !CanChangeDate;

            btnCreateExcel.Visibility = ( ( CanCreateDeliveryNote) ? Visibility.Visible : Visibility.Collapsed);

            string SelectedFilterField = (string)cbFieldItemToSearch.SelectedValue;
            bool IsDateFilter = SelectedFilterField == "Date_Str" || SelectedFilterField == "DeliveryNote_Date_Str";
            cdTextFilter.Width = IsDateFilter ? HideComponent : ViewTextFilterPannel;
            cdDateFilter.Width = IsDateFilter ? ViewTextFilterPannel : HideComponent;

            //cdChangeDate.Width = (CanChangeDate) ? ViewChangeDateButtonPannel : HideComponent;


            /*
            bool NotViewingProvider = Provider is null;
            rdProviderInfo.Height = NotViewingProvider ? HideComponent : ViewProviderPannel;
            rdSearchPannel.Height = (DataList.Count > 0) ? ViewSearchPannel : HideComponent;
            cdAdd.Width = (!Print && !IsEditing && !m_CanShowDeliveryNotes && NotViewingProvider && !CanChangeDate) ? ViewAddButtonPannel : HideComponent;
            bool HasItemSelected = !(ListItems.SelectedItem is null);
            ProviderOrdersView ProviderOrder = HasItemSelected ? (ProviderOrdersView)ListItems.SelectedItem : null;
            bool PrintControlVisibility = ((Print || PrintProviderOrder) && HasItemSelected && !CanChangeDate);
            cdPrint.Width = PrintControlVisibility ? ViewPrintButtonPannel : HideComponent;
            cdSendByEMail.Width = PrintControlVisibility ? ViewSendByEMailButtonPannel : HideComponent;
            bool CanCreateBill = Print && HasItemSelected && !ProviderOrder.HasBill && !CanChangeDate;
            cdCreateBill.Width = (CanCreateBill) ? ViewCreateBillButtonPannel : HideComponent;
            bool CanEditOrDelete = !Print && HasItemSelected && !IsEditing && !CanChangeDate;
            cdEdit.Width = CanEditOrDelete ? ViewEditButtonPannel : HideComponent;
            cdDelete.Width = CanEditOrDelete ? ViewEditButtonPannel : HideComponent;
            bool CanSplitProviderOrder = CanEditOrDelete && !ProviderOrder.HasDeliveryNote && !CanChangeDate;
            cdSplitProviderOrder.Width = CanSplitProviderOrder ? ViewSplitProviderOrderButtonPannel : HideComponent;
            cdView.Width = (HasItemSelected && !CanChangeDate) ? ViewViewButtonPannel : HideComponent;
            bool CanCreateDeliveryNote = !Print && HasItemSelected && !ProviderOrder.HasDeliveryNote && !m_CanShowDeliveryNotes && !CanChangeDate;
            cdCreateDeliveryNote.Width = (CanCreateDeliveryNote) ? ViewCreateDeliveryNoteButtonPannel : HideComponent;
            string SelectedFilterField = (string)cbFieldItemToSearch.SelectedValue;
            bool IsDateFilter = SelectedFilterField == "Date_Str" || SelectedFilterField == "DeliveryNote_Date_Str";
            cdTextFilter.Width = IsDateFilter ? HideComponent : ViewTextFilterPannel;
            cdDateFilter.Width = IsDateFilter ? ViewTextFilterPannel : HideComponent;
            cdChangeDate.Width = (CanChangeDate) ? ViewChangeDateButtonPannel : HideComponent;
            */
        }

        #endregion

        #region Shared Functions

        private DateTime NormalizeDateInit(DateTime? Date)
        {
            DateTime DateBase = Date is null ? DateTime.Now : (DateTime)Date;
            return new DateTime(DateBase.Year, DateBase.Month, DateBase.Day, 0, 0, 0);

        }

        private DateTime? NormalizeDateEnd(DateTime? Date)
        {
            if (Date is null) return null;
            else
            {
                DateTime DateBase = (DateTime)Date;
                return Date is null ? Date : new DateTime(DateBase.Year, DateBase.Month, DateBase.Day, 23, 59, 59);
            }
        }

        #endregion
    }
}
