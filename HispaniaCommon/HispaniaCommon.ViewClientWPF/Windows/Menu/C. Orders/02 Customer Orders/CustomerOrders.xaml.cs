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
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

#endregion

namespace HispaniaCommon.ViewClientWPF.Windows
{
    /// <summary>
    /// Interaction logic for CustomerOrders.xaml
    /// </summary>
    public partial class CustomerOrders : Window
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
        /// Show the Split Customer Order button.
        /// </summary>
        private GridLength ViewSplitCustomerOrderButtonPannel = new GridLength(155.0);

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
        /// Show the Customer Pannel.
        /// </summary>
        private GridLength ViewCustomerPannel = new GridLength(80.0);

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
        private ObservableCollection<CustomerOrdersView> m_DataList = new ObservableCollection<CustomerOrdersView>();

        /// <summary>
        /// Store the data of the Customer.
        /// </summary>
        private CustomersView m_Customer = null;

        /// <summary>
        /// Store the value that indicates if the Window allows impression or not.
        /// </summary>
        private bool m_Print = false;

        /// <summary>
        /// Store the value that indicates if the Window allows impression or not.
        /// </summary>
        private bool m_PrintCustomerOrder = true;

        /// <summary>
        /// Store the value that indicates if the Window filter by Customer the CustomerOrders that show or not.
        /// </summary>
        private bool m_FilterByCustomer = false;

        /// <summary>
        /// Store a value that indicate if the Window can show Delivery Notes.
        /// </summary>
        private bool m_CanShowDeliveryNotes = true;

        /// <summary>
        /// Store a value that indicate if the Window can change Date of the Customer Order or Delivery Notes.
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
        public CustomersView Customer
        {
            get
            {
                return (m_Customer);
            }
            set
            {
                //  Actualize the value
                    m_Customer = value;
                    if (m_Customer != null) 
                    {
                        //  Set the information of the customer in Customer Info fields.
                            tbCustomerCode.Text = GlobalViewModel.GetStringFromIntIdValue(m_Customer.Customer_Id);
                            tbCustomerDescription.Text = m_Customer.Company_Name;
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
        public ObservableCollection<CustomerOrdersView> DataList
        {
            get
            {
                return (m_DataList);
            }
            set
            {
                //  Actualize the value
                    if (value != null) m_DataList = value;
                    else m_DataList = new ObservableCollection<CustomerOrdersView>();
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
                        CollectionViewSource.GetDefaultView(ListItems.ItemsSource).SortDescriptions.Add(new SortDescription("CustomerOrder_Id", ListSortDirection.Descending));
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
        /// Get or Set the value that indicates if the Window filter by Customer the CustomerOrders that show or not.
        /// </summary>
        public bool FilterByCustomer
        {
            get
            {
                return (m_FilterByCustomer);
            }
            set
            {
                m_FilterByCustomer = value;
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
        private bool PrintCustomerOrder
        {
            get
            {
                return m_PrintCustomerOrder;
            }
        }

        /// <summary>
        /// Get the value that indicate if the Window can change Date of the Customer Order or Delivery Notes.
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
                CustomerOrderDataControl.Parameters = value;
            }
        }

        /// <summary>
        /// Get or Set the Customers 
        /// </summary>
        public Dictionary<string, CustomersView> Customers
        {
            set
            {
                CustomerOrderDataControl.Customers = value;
            }
        }

        /// <summary>
        /// Get or Set the SendTypes
        /// </summary>
        public Dictionary<string, SendTypesView> SendTypes
        {
            set
            {
                CustomerOrderDataControl.SendTypes = value;
            }
        }

        /// <summary>
        /// Get or Set the Effect Types
        /// </summary>
        public Dictionary<string, EffectTypesView> EffectTypes
        {
            set
            {
                CustomerOrderDataControl.EffectTypes = value;
            }
        }

        /// <summary>
        /// Get or Set the Agents
        /// </summary>
        public Dictionary<string, AgentsView> Agents
        {
            set
            {
                CustomerOrderDataControl.Agents = value;
            }
        }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Main Builder of the windows.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        public CustomerOrders(ApplicationType AppType, bool Print = false, bool PrintCustomerOrder = true, bool CanShowDeliveryNotes = true, bool CanChangeDate = false)
        {
            InitializeComponent();
            Initialize(AppType, Print, PrintCustomerOrder, CanShowDeliveryNotes, CanChangeDate);
        }

        /// <summary>
        /// Method that initialize the window.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        private void Initialize(ApplicationType AppType, bool Print, bool PrintCustomerOrder, bool CanShowDeliveryNotes, bool CanChangeDate)
        {
            //  Actualize the Window.
                ActualizeWindowComponents(AppType, Print, PrintCustomerOrder, CanShowDeliveryNotes, CanChangeDate);
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
        private void ActualizeWindowComponents(ApplicationType AppType, bool Print, bool PrintCustomerOrder, bool CanShowDeliveryNotes, bool CanChangeDate)
        {
            //  Actualize properties of this Window.
                this.AppType = AppType;
                m_Print = Print;
                m_PrintCustomerOrder = PrintCustomerOrder;
                m_CanShowDeliveryNotes = CanShowDeliveryNotes;
                m_CanChangeDate = CanChangeDate;
            //  Update Control State
                if ((m_Print) || (m_CanChangeDate)) ListItems.SelectionMode = SelectionMode.Extended;
                else ListItems.SelectionMode = SelectionMode.Single;
                CustomerOrderDataControl.rdBill.Height = HideComponent;
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
                    cbFieldItemToSearch.ItemsSource = CustomerOrdersView.DeliveryNoteFields;
                }
                else
                {
                    Title = m_CanChangeDate ? "Canvi de Data de les Comandes de Client" : "Gestió de Comandes de Client";
                    OpText.Text = "Comandes de Client";
                    DataListText.Text = "Comandes de Client";
                    ListItems.View = (GridView)FindResource("GridViewForCustomerOrders");
                    cbFieldItemToSearch.ItemsSource = CustomerOrdersView.Fields;
                    CustomerOrderDataControl.rdDeliveryNote.Height = HideComponent;
                }
            //  Apply Theme to window.
                ThemeManager.ActualTheme = AppTheme;
                CustomerOrderDataControl.AppType = AppType;
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
                if (CustomerOrdersView.Fields.Count > 0) cbFieldItemToSearch.SelectedIndex = 0;
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
                CustomerOrdersView ItemData = (CustomerOrdersView)item;
            //  Validate that the Customer Order belong at the Customer.
                if (FilterByCustomer)
                {
                    if ((ItemData.Customer is null) || 
                        (!(ItemData.Customer is null) && (ItemData.Customer.Customer_Id != Customer.Customer_Id)))
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
                        ((PrintCustomerOrder && !ItemData.HasDeliveryNote) ||
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
                this.Closed += Customers_Closed;
            //  TextBox
                tbItemToSearch.TextChanged += TbItemToSearch_TextChanged;
            //  Button 
                btnAdd.Click += BtnAdd_Click;
                btnEdit.Click += BtnEdit_Click;
                btnDelete.Click += BtnDelete_Click;
                btnViewData.Click += BtnViewData_Click;
                btnPrint.Click += BtnPrint_Click;
                btnSendByEmail.Click += BtnSendByEMail_Click;
                btnCreateBill.Click += BtnCreateBill_Click;
                btnCreateDeliveryNote.Click += BtnCreateDeliveryNote_Click;
                btnSplitCustomerOrder.Click += BtnSplitCustomerOrder_Click;
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
            //  Define CustomerDataControl events to manage.
                CustomerOrderDataControl.EvAccept += CustomerOrderDataControl_evAccept;
                CustomerOrderDataControl.EvCancel += CustomerOrderDataControl_evCancel;
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
        /// Manage the Date Change of Customer Order and Delivery Notes.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnChangeDate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActualizeCustomerOrdersFromDb();
                if (CanShowDeliveryNotes) ChangeDateDeliveryNote();
                else ChangeDateCustomerOrder();
                ActualizeCustomerOrdersRefreshFromDb();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al canviar la data a(ls) element(s) seleccionat(s).\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
            }
        }

        /// <summary>
        /// Manage the Date Change of Customer Order.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void ChangeDateCustomerOrder()
        {
            try
            {
                if (ListItems.SelectedItems != null)
                {
                    DataSelector dsDate = new DataSelector(AppType);
                    dsDate.ShowDialog();
                    if (dsDate.Result == DataSelector.WindowResult.Accept)
                    {
                        DateTime NewDate = dsDate.Data;
                        string sQuestion = string.Format("Està segur que vol canviar la data dels elements seleccionats per la Data seleccionada: {0}", NewDate);
                        if (MsgManager.ShowQuestion(sQuestion) == MessageBoxResult.Yes)
                        {
                            StringBuilder sbInfoExecution = new StringBuilder(string.Empty);
                            foreach (CustomerOrdersView customerOrder in new ArrayList(ListItems.SelectedItems))
                            {
                                try
                                {
                                    string AddRemark = string.Format("Comanda de Client canviada de la data {0} a la data {1}.\r\n", customerOrder.Date, NewDate);
                                    customerOrder.Date = NewDate;
                                    customerOrder.Remarks = AddRemark + customerOrder.Remarks;
                                    GlobalViewModel.Instance.HispaniaViewModel.UpdateCustomerOrder(customerOrder);
                                    sbInfoExecution.AppendFormat("Data de la Comanda de Client '{0}' actualitzada correctament.\r\n", customerOrder.CustomerOrder_Id);
                                }
                                catch (Exception)
                                {
                                    sbInfoExecution.AppendFormat("Error al actualitzar la data de la Comanda de Client '{0}'.\r\n", customerOrder.CustomerOrder_Id);
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
        /// Manage the Date Change of Customer Order.
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
                    dsDate.ShowDialog();
                    if (dsDate.Result == DataSelector.WindowResult.Accept)
                    {
                        DateTime NewDate = dsDate.Data;
                        string sQuestion = string.Format("Està segur que vol canviar la data dels elements seleccionats per la Data seleccionada: {0}", NewDate);
                        if (MsgManager.ShowQuestion(sQuestion) == MessageBoxResult.Yes)
                        {
                            StringBuilder sbInfoExecution = new StringBuilder(string.Empty);
                            foreach (CustomerOrdersView customerOrder in new ArrayList(ListItems.SelectedItems))
                            {
                                try
                                {
                                    GlobalViewModel.Instance.HispaniaViewModel.UpdateCustomerOrder(customerOrder, NewDate);
                                    sbInfoExecution.AppendFormat("Data de l'Albarà '{0}' actualitzat correctament.\r\n", customerOrder.DeliveryNote_Id);
                                }
                                catch (Exception)
                                {
                                    sbInfoExecution.AppendFormat("Error al actualitzar la data de l'Albarà '{0}'.\r\n", customerOrder.DeliveryNote_Id);
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

        #region Editing Customer Orders

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
                    ActualizeCustomerOrderFromDb();
                    CustomerOrderDataControl.CtrlOperation = Operation.Show;
                    gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                    ShowItemPannel();
                }
                catch (Exception ex)
                {
                    CustomerOrdersView CustomerOrder = (CustomerOrdersView)ListItems.SelectedItem;
                    string ItemDescription = PrintCustomerOrder ? string.Format("la Comanda de Client '{0}'", CustomerOrder.CustomerOrder_Id)
                                                                : string.Format("l'Albarà '{0}'", CustomerOrder.DeliveryNote_Id_Str);
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
                CustomerOrderDataControl.CtrlOperation = Operation.Add;
                gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "EditableGroupBox");
                ShowItemPannel();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al afegir una Comanda de Client.\r\nDetalls:{0}", MsgManager.ExcepMsg(ex)));
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
                    ActualizeCustomerOrderFromDb();
                    if (!((CustomerOrdersView)ListItems.SelectedItem).HasBill)
                    {
                        if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(ListItems.SelectedItem, out string ErrMsg))
                        {
                            CustomerOrderDataControl.CtrlOperation = Operation.Edit;
                            gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "EditableGroupBox");
                            ShowItemPannel();
                            CustomerOrderDataControl.ShowGoodRemark();
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
                    CustomerOrdersView CustomerOrder = (CustomerOrdersView)ListItems.SelectedItem;
                    string ItemDescription = PrintCustomerOrder ? string.Format("la Comanda de Client '{0}'", CustomerOrder.CustomerOrder_Id)
                                                                : string.Format("l'Albarà '{0}'", CustomerOrder.DeliveryNote_Id_Str);
                    MsgManager.ShowMessage(string.Format("Error, a l'editar {0}.\r\nDetalls:{1}", ItemDescription, MsgManager.ExcepMsg(ex)));
                }
            }
        }

        /// <summary>
        /// Manage the event produced when the operation in that was doing in the CustomerDataControl was Accepted.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CustomerOrderDataControl_evAccept(CustomerOrdersView NewOrEditedCustomerOrder, Guid DataManagementId)
        {
            try
            {
                CustomerOrdersView NewCustomerOrder = new CustomerOrdersView(NewOrEditedCustomerOrder);
                switch (CustomerOrderDataControl.CtrlOperation)
                {
                    case Operation.Add:
                         GlobalViewModel.Instance.HispaniaViewModel.CreateCustomerOrder(NewCustomerOrder, DataManagementId);
                         DataChangedManagerActive = false;
                         if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                         DataList.Add(NewCustomerOrder);
                         DataChangedManagerActive = true;
                         ListItems.SelectedItem = NewCustomerOrder;
                         ListItems.UpdateLayout();
                         gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                         HideItemPannel();
                         break;
                    case Operation.Edit:
                         GlobalViewModel.Instance.HispaniaViewModel.UpdateCustomerOrder(NewCustomerOrder, DataManagementId);
                         if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(CustomerOrderDataControl.CustomerOrder, out string ErrMsg))
                         {
                             MsgManager.ShowMessage(ErrMsg);
                         }
                         if (CustomerOrderDataControl.CustomerOrder.HasBill)
                         {
                             if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(CustomerOrderDataControl.CustomerOrder.Bill, out ErrMsg))
                             {
                                 MsgManager.ShowMessage(ErrMsg);
                             }
                         }
                         DataChangedManagerActive = false;
                         CustomerOrdersView SourceCustomerOrder = (CustomerOrdersView)ListItems.SelectedItem;
                         if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                         DataList.Remove(SourceCustomerOrder);
                         DataList.Add(GlobalViewModel.Instance.HispaniaViewModel.GetCustomerOrderFromDb(NewCustomerOrder));
                         DataChangedManagerActive = true;
                         ListItems.SelectedItem = NewCustomerOrder;
                         ListItems.UpdateLayout();
                         gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                         HideItemPannel();
                         break;
                }
            }
            catch (Exception ex)
            {
                switch (CustomerOrderDataControl.CtrlOperation)
                {
                    case Operation.Edit:
                         string ItemDescription = PrintCustomerOrder ? string.Format("la Comanda de Client '{0}'", NewOrEditedCustomerOrder.CustomerOrder_Id)
                                                                     : string.Format("l'Albarà '{0}'", NewOrEditedCustomerOrder.DeliveryNote_Id_Str);
                         MsgManager.ShowMessage(string.Format("Error, al guardar les dades de l'edició de {0}.\r\nDetalls:{1}", ItemDescription, MsgManager.ExcepMsg(ex)));
                         break;
                    case Operation.Add:
                    default:
                         MsgManager.ShowMessage(string.Format("Error, al crear la Comanda de Client.\r\nDetalls:{0}", MsgManager.ExcepMsg(ex)));
                         break;
                }
            }
        }

        /// <summary>
        /// Manage the event produced when the operation in that was doing in the CustomerDataControl was Cancelled.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CustomerOrderDataControl_evCancel()
        {
            switch (CustomerOrderDataControl.CtrlOperation)
            {
                case Operation.Add:
                     MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                     break;
                case Operation.Edit:
                     MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                     if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(CustomerOrderDataControl.CustomerOrder, out string ErrMsg))
                     {
                         MsgManager.ShowMessage(ErrMsg);
                     }
                     if (CustomerOrderDataControl.CustomerOrder.HasBill)
                     {
                         if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(CustomerOrderDataControl.CustomerOrder.Bill, out ErrMsg))
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

        #region Remove Customer Orders and Delivery Notes

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
                    ActualizeCustomerOrderFromDb();
                    CustomerOrdersView CustomerOrdersToDelete = (CustomerOrdersView)ListItems.SelectedItem;
                    string ItemDescription = PrintCustomerOrder ? string.Format("la Comanda de Client '{0}'", CustomerOrdersToDelete.CustomerOrder_Id)
                                                                : string.Format("l'Albarà '{0}'", CustomerOrdersToDelete.DeliveryNote_Id);
                    if (MsgManager.ShowQuestion(string.Format("Està segur que vol esborrar {0} ?", ItemDescription)) == MessageBoxResult.Yes)
                    {
                        if (!CustomerOrdersToDelete.HasBill)
                        {
                            if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(CustomerOrdersToDelete, out string ErrMsg))
                            {
                                GlobalViewModel.Instance.HispaniaViewModel.DeleteCustomerOrder(DataList[DataList.IndexOf(CustomerOrdersToDelete)]);
                                if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(CustomerOrdersToDelete, out ErrMsg))
                                {
                                    MsgManager.ShowMessage(ErrMsg);
                                }
                                DataChangedManagerActive = false;
                                if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                                DataList.Remove(CustomerOrdersToDelete);
                                DataChangedManagerActive = true;
                                ListItems.UpdateLayout();
                            }
                            else MsgManager.ShowMessage(ErrMsg);
                        }
                        else
                        {
                            MsgManager.ShowMessage(
                                string.Format("Error, a l'esborrar l'Albarà '{0}'.\r\nDetalls: {1}", 
                                              CustomerOrdersToDelete.CustomerOrder_Id,
                                              "L'Albarà pertany a una factura i no es pot esborrar un albarà associat a una factura. " +
                                              "Cal treure l'albarà de la factura abans de poder esborrar-lo."));
                        }
                    }
                    else MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                }
                catch (Exception ex)
                {
                    CustomerOrdersView CustomerOrdersToDelete = (CustomerOrdersView)ListItems.SelectedItem;
                    string ItemDescription = PrintCustomerOrder ? string.Format("la Comanda de Client '{0}'", CustomerOrdersToDelete.CustomerOrder_Id)
                                                                : string.Format("l'Albarà '{0}'", CustomerOrdersToDelete.DeliveryNote_Id);
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

                ActualizeCustomerOrdersFromDb();
                if (PrintCustomerOrder)
                {
                    if (ListItems.SelectionMode == SelectionMode.Single) PrintReportOneCustomerOrder(false);
                    else
                    {
                        PrintReportCustomerOrder(false);
                    }
                }
                else PrintReportDeliveryNote();
                DataList = GlobalViewModel.Instance.HispaniaViewModel.CustomerOrders;
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

                ActualizeCustomerOrdersFromDb();
                if (PrintCustomerOrder)
                {
                    if (ListItems.SelectionMode == SelectionMode.Single) PrintReportOneCustomerOrder(true);
                    else
                    {
                        PrintReportCustomerOrder(true);
                    }
                }
                else PrintReportDeliveryNote();
                DataList = GlobalViewModel.Instance.HispaniaViewModel.CustomerOrders;
                ListItems.UpdateLayout();
                HideOrderOrProformaPannel();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(
                    string.Format("Error, a l'imprimir el(s) element(s) seleccionat(s).\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
            }
        }

        

        private void PrintReportOneCustomerOrder(bool isProfoma)
        {
            if (ListItems.SelectedItem != null)
            {
                CustomerOrdersView customerOrder = (CustomerOrdersView)ListItems.SelectedItem;
                {
                    if (CustomerOrdersReportView.CheckAndContinueIfExistReport(customerOrder, out string PDF_FileName, out string ErrMsg))
                    {
                        if (CustomerOrdersReportView.CreateReport(customerOrder, PDF_FileName, out ErrMsg, isProfoma))
                        {
                            if (ReportView.PrintReport(PDF_FileName, string.Format("Comanda de Client '{0}'", customerOrder.CustomerOrder_Id), out ErrMsg))
                            {
                                if (CustomerOrdersReportView.UpdateCustomerOrderFlag(customerOrder, CustomerOrderFlag.Print, true, out ErrMsg))
                                {
                                    DataList[DataList.IndexOf(customerOrder)].Print_CustomerOrder = true;
                                    MsgManager.ShowMessage(
                                       string.Format("Informe de la Comanda de Client '{0}' enviat a impressió correctament.\r\n", 
                                                     customerOrder.CustomerOrder_Id),
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

        private void PrintReportCustomerOrder(bool isProforma)
        {
            if (ListItems.SelectedItems != null)
            {
                StringBuilder sbInfoExecution = new StringBuilder(string.Empty);
                foreach (CustomerOrdersView customerOrder in new ArrayList(ListItems.SelectedItems))
                {
                    if (CustomerOrdersReportView.CheckAndContinueIfExistReport(customerOrder, out string PDF_FileName, out string ErrMsg))
                    {
                        if (CustomerOrdersReportView.CreateReport(customerOrder, PDF_FileName, out ErrMsg, isProforma))
                        {
                            if (ReportView.PrintReport(PDF_FileName, string.Format("Comanda de Client '{0}'", customerOrder.CustomerOrder_Id), out ErrMsg))
                            {
                                if (CustomerOrdersReportView.UpdateCustomerOrderFlag(customerOrder, CustomerOrderFlag.Print, true, out ErrMsg))
                                {
                                    DataList[DataList.IndexOf(customerOrder)].Print_CustomerOrder = true;
                                    sbInfoExecution.AppendFormat("Informe de la Comanda de Client '{0}' enviat a impressió correctament.\r\n", customerOrder.CustomerOrder_Id);
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
                foreach (CustomerOrdersView customerOrder in ListItems.SelectedItems)
                {
                    if (DeliveryNotesReportView.CheckAndContinueIfExistReport(customerOrder, out string PDF_FileName, out string ErrMsg))
                    {
                        if (DeliveryNotesReportView.CreateReport(customerOrder, PDF_FileName, out ErrMsg))
                        {
                            if (ReportView.PrintReport(PDF_FileName, string.Format("Albarà '{0}'", customerOrder.DeliveryNote_Id), out ErrMsg))
                            {
                                if (DeliveryNotesReportView.UpdateDeliveryNoteFlag(customerOrder, DeliveryNoteFlag.Print, true, out ErrMsg))
                                {
                                    DataList[DataList.IndexOf(customerOrder)].Print = true;
                                    sbInfoExecution.AppendFormat("Informe de l'Albarà '{0}' enviat a impressió correctament.\r\n", customerOrder.DeliveryNote_Id);
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
                    ActualizeCustomerOrdersFromDb();
                    if (PrintCustomerOrder)
                    {
                        if (ListItems.SelectionMode == SelectionMode.Single) SendByEmailReportOneCustomerOrder(true);
                        else
                        {
                            SendByEmailReportCustomerOrder(true);
                        }
                    }
                    else SendByEmailReportDeliveryNote();
                    DataList = GlobalViewModel.Instance.HispaniaViewModel.CustomerOrders;
                    ListItems.UpdateLayout();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, a l'enviar per e-mail el(s) element(s) seleccionat(s).\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
        }

        private void SendByEmailReportOneCustomerOrder(bool isProforma)
        {
            if (ListItems.SelectedItem != null)
            {
                CustomerOrdersView customerOrder = (CustomerOrdersView)ListItems.SelectedItem;
                {
                    if (CustomerOrdersReportView.CheckAndContinueIfExistReport(customerOrder, out string PDF_FileName, out string ErrMsg))
                    {
                        if (CustomerOrdersReportView.CreateReport(customerOrder, PDF_FileName, out ErrMsg, isProforma))
                        {
                            if (CustomerOrdersReportView.SendByEMail(customerOrder, PDF_FileName, out ErrMsg))
                            {
                                if (CustomerOrdersReportView.UpdateCustomerOrderFlag(customerOrder, CustomerOrderFlag.SendByEMail, true, out ErrMsg))
                                {
                                    DataList[DataList.IndexOf(customerOrder)].SendByEMail_CustomerOrder = true;
                                    MsgManager.ShowMessage(string.Format("Creat correctament l'email de la comanda de client '{0}'.\r\n", customerOrder.CustomerOrder_Id),
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

        private void SendByEmailReportCustomerOrder(bool isProforma)
        {
            StringBuilder sbInfoExecution = new StringBuilder(string.Empty);
            foreach (CustomerOrdersView customerOrder in ListItems.SelectedItems)
            {
                if (CustomerOrdersReportView.CheckAndContinueIfExistReport(customerOrder, out string PDF_FileName, out string ErrMsg))
                {
                    if (CustomerOrdersReportView.CreateReport(customerOrder, PDF_FileName, out ErrMsg, isProforma))
                    {
                        Process.Start(PDF_FileName);
                        if (CustomerOrdersReportView.SendByEMail(customerOrder, PDF_FileName, out ErrMsg))
                        {
                            if (CustomerOrdersReportView.UpdateCustomerOrderFlag(customerOrder, CustomerOrderFlag.SendByEMail, true, out ErrMsg))
                            {
                                DataList[DataList.IndexOf(customerOrder)].SendByEMail_CustomerOrder = true;
                                sbInfoExecution.AppendFormat("Creat correctament l'email de la comanda de client '{0}'.\r\n", customerOrder.CustomerOrder_Id);
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
            foreach (CustomerOrdersView customerOrder in ListItems.SelectedItems)
            {
                if (DeliveryNotesReportView.CheckAndContinueIfExistReport(customerOrder, out string PDF_FileName, out string ErrMsg))
                {
                    if (DeliveryNotesReportView.CreateReport(customerOrder, PDF_FileName, out ErrMsg))
                    {
                        Process.Start(PDF_FileName);
                        if (DeliveryNotesReportView.SendByEMail(customerOrder, PDF_FileName, out ErrMsg))
                        {
                            if (DeliveryNotesReportView.UpdateDeliveryNoteFlag(customerOrder, DeliveryNoteFlag.SendByEMail, true, out ErrMsg))
                            {
                                DataList[DataList.IndexOf(customerOrder)].SendByEMail = true;
                                sbInfoExecution.AppendFormat("Creat correctament l'email  de l'albarà '{0}'.\r\n", customerOrder.DeliveryNote_Id);
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
                string Question = string.Format("Està segur que vol crear factures per les comandes de client seleccionades ?");
                if (MsgManager.ShowQuestion(Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        bool ExecuteCommand = true;
                        bool CustomerOrderWithBill = false;
                        LogViewModel.Instance.WriteLog("Create Bill - ActualizeCustomerOrdersFromDb().");
                        ActualizeCustomerOrdersFromDb();
                        SortedDictionary<int, DataForBill> SourceCustomerOrders = new SortedDictionary<int, DataForBill>();
                        foreach (CustomerOrdersView customerOrder in ListItems.SelectedItems)
                        {
                            if (!GlobalViewModel.Instance.HispaniaViewModel.LockRegister(customerOrder, out string ErrMsg))
                            {
                                MsgManager.ShowMessage(ErrMsg);
                                ExecuteCommand = false;
                                break;
                            }
                            if (customerOrder.HasBill)
                            {
                                CustomerOrderWithBill = true;
                                ExecuteCommand = false;
                                break;
                            }
                            if (customerOrder.DeliveryNote_Year != DateTime.Now.Year)
                            {
                                MsgManager.ShowMessage(
                                   string.Format("Error, al crear la Factura de l'Albarà '{0}'.\r\n" +
                                                 "Detalls: no es pot crear una factura d'un Albarà que no pertany a l'any en curs.", 
                                                 customerOrder.DeliveryNote_Id_Str));
                                LogViewModel.Instance.WriteLog("Create Bill - DeliveryNote with Year different of now year.");
                                ExecuteCommand = false;
                                break;
                            }
                            CustomersView Customer = customerOrder.Customer;
                            DateTime DeliveryNoteDate = customerOrder.DeliveryNote_Date;
                            if (!SourceCustomerOrders.ContainsKey(customerOrder.Customer.Customer_Id))
                            {
                                List<CustomerOrdersView> ListCustomerOrders = new List<CustomerOrdersView> { customerOrder };
                                SourceCustomerOrders.Add(Customer.Customer_Id, new DataForBill(DeliveryNoteDate, Customer, ListCustomerOrders));
                            }
                            else
                            {
                                if (DeliveryNoteDate > SourceCustomerOrders[Customer.Customer_Id].Bill_Date)
                                {
                                    SourceCustomerOrders[Customer.Customer_Id].Bill_Date = DeliveryNoteDate;
                                }
                                SourceCustomerOrders[Customer.Customer_Id].Movements.Add(customerOrder);
                            }
                        }
                        LogViewModel.Instance.WriteLog("Create Bill - SourceCustomerOrders : {0}.", SourceCustomerOrders.Count);
                        if (ExecuteCommand)
                        {
                            LogViewModel.Instance.WriteLog("Create Bill - CreateBillsFromCustomerOrders(SourceCustomerOrders).");
                            GlobalViewModel.Instance.HispaniaViewModel.CreateBillsFromCustomerOrders(SourceCustomerOrders);
                            DataChangedManagerActive = false;
                            if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                            DataList = GlobalViewModel.Instance.HispaniaViewModel.CustomerOrders;
                            DataChangedManagerActive = true;
                            ListItems.SelectedItem = null;
                            ListItems.UpdateLayout();
                            LogViewModel.Instance.WriteLog("Create Bill - ListItems -> Layout Updated.");
                        }
                        if (CustomerOrderWithBill)
                        {
                            LogViewModel.Instance.WriteLog("Create Bill - FilterDataListObjects().");
                            FilterDataListObjects();
                        }
                        foreach (DataForBill CustomerOrders in SourceCustomerOrders.Values)
                        {
                            foreach (CustomerOrdersView customerOrder in CustomerOrders.Movements)
                            {
                                if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(customerOrder, out string ErrMsg))
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

        #region Create Delivery Note for Customer Order

        private void BtnCreateDeliveryNote_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItem != null)
            {
                try
                {
                    ActualizeCustomerOrderFromDb();
                    CustomerOrdersView CustomerOrder = (CustomerOrdersView)ListItems.SelectedItem;
                    if (!CustomerOrder.HasDeliveryNote)
                    {
                        string Question = string.Format("Està segur que vol crear un albarà per la comanda de client '{0}' ?", CustomerOrder.CustomerOrder_Id);
                        if (MsgManager.ShowQuestion(Question) == MessageBoxResult.Yes)
                        {
                            string ErrMsg;
                            try
                            {
                                if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(ListItems.SelectedItem, out ErrMsg))
                                {
                                    if (GlobalViewModel.Instance.HispaniaViewModel.CreateDeliveryNoteForCustomerOrder(CustomerOrder, out ErrMsg))
                                    {
                                        GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(ListItems.SelectedItem, out ErrMsg);
                                        DataChangedManagerActive = false;
                                        if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                                        DataList.Remove(CustomerOrder);
                                        DataChangedManagerActive = true;
                                        ListItems.UpdateLayout();
                                        MsgManager.ShowMessage(string.Format("Informació, s'ha creat l'albarà '{0}' per la comanda de client '{1}'.",
                                                                             CustomerOrder.DeliveryNote_Id, CustomerOrder.CustomerOrder_Id), MsgType.Information);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                ErrMsg = string.Format("Error, al crear l'albarà per la Comanda de Client '{0}'.\r\nDetalls: {1}",
                                                        CustomerOrder.CustomerOrder_Id, MsgManager.ExcepMsg(ex));
                            }
                            if (!string.IsNullOrEmpty(ErrMsg))
                            {
                                MsgManager.ShowMessage(ErrMsg);
                                GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(CustomerOrder, out ErrMsg);
                            }
                        }
                        else MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                    }
                    else
                    {
                        MsgManager.ShowMessage(string.Format("Ja s'ha creat un albarà per la comanda de client '{0}' ?", CustomerOrder.CustomerOrder_Id),
                                                MsgType.Warning);
                        FilterDataListObjects();
                    }
                }
                catch (Exception ex)
                {
                    CustomerOrdersView CustomerOrder = (CustomerOrdersView)ListItems.SelectedItem;
                    MsgManager.ShowMessage(
                       string.Format("Error, al crear l'Albarà a partir de la Comanda de Client '{0}'.\r\nDetalls: {1}",
                                     CustomerOrder.CustomerOrder_Id, MsgManager.ExcepMsg(ex)));
                }
            }
        }

        #endregion

        #region Split Customer Order for create Delivery Note

        private void BtnSplitCustomerOrder_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItem != null)
            {
                try
                {
                    ActualizeCustomerOrderFromDb();
                    CustomerOrdersView CustomerOrder = (CustomerOrdersView)ListItems.SelectedItem;
                    string Question = string.Format("Està segur que vol preparar la comanda de client '{0}' per poder crear un Albarà ?", CustomerOrder.CustomerOrder_Id);
                    if (MsgManager.ShowQuestion(Question) == MessageBoxResult.Yes)
                    {
                        string ErrMsg;
                        try
                        {
                            if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(ListItems.SelectedItem, out ErrMsg))
                            {
                                if (GlobalViewModel.Instance.HispaniaViewModel.SplitCustomerOrder(CustomerOrder, out CustomerOrdersView NewCustomerOrder, out ErrMsg))
                                {
                                    GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(ListItems.SelectedItem, out ErrMsg);
                                    DataChangedManagerActive = false;
                                    if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                                    DataList.Remove(CustomerOrder);
                                    DataList.Add(GlobalViewModel.Instance.HispaniaViewModel.GetCustomerOrderFromDb(CustomerOrder));
                                    DataList.Add(GlobalViewModel.Instance.HispaniaViewModel.GetCustomerOrderFromDb(NewCustomerOrder));
                                    DataChangedManagerActive = true;
                                    ListItems.SelectedItem = NewCustomerOrder;
                                    ListItems.UpdateLayout();
                                    if (NewCustomerOrder == null)
                                    {
                                        MsgManager.ShowMessage(
                                            string.Format("Informació, la comanda de client '{0}' ja estava preparada per crear un Albarà.",
                                                          CustomerOrder.CustomerOrder_Id), MsgType.Information);
                                    }
                                    else
                                    {
                                        MsgManager.ShowMessage(
                                            string.Format("Informació, la comanda de client '{0}' s'ha preparat per crear un Albarà.\r\n" +
                                                          "S'ha creat la comanda de client '{1}' amb les línies no conformes de la comanda inicial.",
                                                          CustomerOrder.CustomerOrder_Id, NewCustomerOrder.CustomerOrder_Id), MsgType.Information);
                                    }
                                }
                                else
                                {
                                    ErrMsg = string.Format("Error, al preparar la Comanda de Client '{0}' per crear l'Albarà.\r\nDetalls: {1}",
                                                            CustomerOrder.CustomerOrder_Id, ErrMsg);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            ErrMsg = string.Format("Error, al preparar la Comanda de Client '{0}' per crear l'Albarà.\r\nDetalls: {1}",
                                                    CustomerOrder.CustomerOrder_Id, MsgManager.ExcepMsg(ex));
                        }
                        if (!string.IsNullOrEmpty(ErrMsg))
                        {
                            MsgManager.ShowMessage(ErrMsg);
                            GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(CustomerOrder, out ErrMsg);
                        }
                    }
                    else MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                }
                catch (Exception ex)
                {
                    CustomerOrdersView CustomerOrder = (CustomerOrdersView)ListItems.SelectedItem;
                    MsgManager.ShowMessage(
                       string.Format("Error, al separar la Comanda de Client '{0}'.\r\nDetalls: {1}", 
                                     CustomerOrder.CustomerOrder_Id,
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
                ActualizeCustomerOrdersRefreshFromDb();
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

        private void Customers_Closed(object sender, EventArgs e)
        {
            if (CustomerOrderDataControl.CtrlOperation == Operation.Edit)
            {
                if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(CustomerOrderDataControl.CustomerOrder, out string ErrMsg))
                {
                    MsgManager.ShowMessage(ErrMsg);
                }
            }
        }

        #endregion

        #endregion
                
        #region Database Operations
		        
        private void ActualizeCustomerOrdersRefreshFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                bool IsDataRefreshed = false;
                if (Print == false && PrintCustomerOrder == true && m_CanShowDeliveryNotes == false)
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.CustomerOrdersWindow);
                    IsDataRefreshed = true;
                }
                else if (Print == false && PrintCustomerOrder == false && m_CanShowDeliveryNotes == true)
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.DeliveryNotesWindow);
                    IsDataRefreshed = true;
                }
                else if (Print == true && PrintCustomerOrder == false && m_CanShowDeliveryNotes == true)
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.DeliveryNotesPrintWindow);
                    IsDataRefreshed = true;
                }
                if (IsDataRefreshed)
                {
                    Customers = GlobalViewModel.Instance.HispaniaViewModel.CustomersActiveDict;
                    SendTypes = GlobalViewModel.Instance.HispaniaViewModel.SendTypesDict;
                    EffectTypes = GlobalViewModel.Instance.HispaniaViewModel.EffectTypesDict;
                    Agents = GlobalViewModel.Instance.HispaniaViewModel.AgentsDict;
                    Parameters = GlobalViewModel.Instance.HispaniaViewModel.Parameters;
                    DataList = GlobalViewModel.Instance.HispaniaViewModel.CustomerOrders;
                }
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        private void ActualizeCustomerOrdersFromDb()
        {
            if (ListItems.SelectionMode == SelectionMode.Single) ActualizeCustomerOrderFromDb();
            else ActualizeMultipleCustomerOrdersFromDb();
        }

        private void ActualizeMultipleCustomerOrdersFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                //List<CustomerOrdersView> ItemsInDb = new List<CustomerOrdersView>(ListItems.SelectedItems.Count);
                foreach (CustomerOrdersView customerOrder in new ArrayList(ListItems.SelectedItems))
                {
                    CustomerOrdersView customerInDb = GlobalViewModel.Instance.HispaniaViewModel.GetCustomerOrderFromDb(customerOrder);
                    //ItemsInDb.Add(customerInDb);
                    int Index = DataList.IndexOf(customerOrder);
                    ListItems.SelectedItems.Remove(customerOrder);
                    DataList.Remove(customerOrder);
                    DataList.Insert(Index, customerInDb);
                    ListItems.SelectedItems.Add(customerInDb);
                }
                ListItems.UpdateLayout();
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        private void ActualizeCustomerOrderFromDb()
        {
            //  Update selected item if there are one.
                if (ListItems.SelectedItem != null)
                {
                    //  Deactivate managers
                        DataChangedManagerActive = false;
                    //  Actualize Item Information From DataBase
                        CustomerOrdersView SelectedItem = (CustomerOrdersView)ListItems.SelectedItem;
                        CustomerOrdersView ItemInDb = GlobalViewModel.Instance.HispaniaViewModel.GetCustomerOrderFromDb(SelectedItem);
                        int Index = ListItems.SelectedIndex;
                        ListItems.UnselectAll();
                        DataList.Remove(SelectedItem);
                        DataList.Insert(Index, ItemInDb);
                        ListItems.SelectedItem = ItemInDb;
                        CustomerOrderDataControl.CustomerOrder = ItemInDb;
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
            bool NotViewingCustomer = Customer is null;
            rdCustomerInfo.Height = NotViewingCustomer ? HideComponent : ViewCustomerPannel;
            rdSearchPannel.Height = (DataList.Count > 0) ? ViewSearchPannel : HideComponent;
            cdAdd.Width = (!Print && !IsEditing && !m_CanShowDeliveryNotes && NotViewingCustomer && !CanChangeDate) ? ViewAddButtonPannel : HideComponent;
            bool HasItemSelected = !(ListItems.SelectedItem is null);
            CustomerOrdersView CustomerOrder = HasItemSelected ? (CustomerOrdersView)ListItems.SelectedItem : null;
            bool PrintControlVisibility = ((Print || PrintCustomerOrder) && HasItemSelected && !CanChangeDate);
            cdPrint.Width = PrintControlVisibility ? ViewPrintButtonPannel : HideComponent;
            cdSendByEMail.Width = PrintControlVisibility ? ViewSendByEMailButtonPannel : HideComponent;
            bool CanCreateBill = Print && HasItemSelected && !CustomerOrder.HasBill && !CanChangeDate;
            cdCreateBill.Width =  (CanCreateBill) ? ViewCreateBillButtonPannel : HideComponent;
            bool CanEditOrDelete = !Print && HasItemSelected && !IsEditing && !CanChangeDate;
            cdEdit.Width = CanEditOrDelete ? ViewEditButtonPannel : HideComponent;
            cdDelete.Width = CanEditOrDelete ? ViewEditButtonPannel : HideComponent;
            bool CanSplitCustomerOrder = CanEditOrDelete && !CustomerOrder.HasDeliveryNote && !CanChangeDate;
            cdSplitCustomerOrder.Width = CanSplitCustomerOrder ? ViewSplitCustomerOrderButtonPannel : HideComponent;
            cdView.Width = (HasItemSelected && !CanChangeDate) ? ViewViewButtonPannel : HideComponent;
            bool CanCreateDeliveryNote = !Print && HasItemSelected && !CustomerOrder.HasDeliveryNote && !m_CanShowDeliveryNotes && !CanChangeDate;
            cdCreateDeliveryNote.Width = (CanCreateDeliveryNote) ? ViewCreateDeliveryNoteButtonPannel : HideComponent;
            string SelectedFilterField = (string)cbFieldItemToSearch.SelectedValue;
            bool IsDateFilter = SelectedFilterField == "Date_Str" || SelectedFilterField == "DeliveryNote_Date_Str";
            cdTextFilter.Width = IsDateFilter ? HideComponent : ViewTextFilterPannel;
            cdDateFilter.Width = IsDateFilter ? ViewTextFilterPannel : HideComponent;
            cdChangeDate.Width = (CanChangeDate) ? ViewChangeDateButtonPannel : HideComponent;
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
