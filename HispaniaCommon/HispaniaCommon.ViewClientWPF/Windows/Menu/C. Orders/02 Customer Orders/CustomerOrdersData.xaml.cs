#region Libraries used for this control

using HispaniaCommon.ViewClientWPF.Windows;
using HispaniaCommon.ViewModel;
using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.Theme;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;
using System.ComponentModel;
using System.Linq;

#endregion

namespace HispaniaCommon.ViewClientWPF.UserControls
{
    /// <summary>
    /// Interaction logic for CustomersData.xaml
    /// </summary>
    public partial class CustomerOrdersData : UserControl
    {
        #region Enumerations

        private enum MovementOp
        {
            Add,
            Remove,
        }

        #endregion

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
        /// Show the button bar.
        /// </summary>
        private GridLength ViewHistoricButton = new GridLength(160.0);

        /// <summary>
        /// Hide the button bar.
        /// </summary>
        private GridLength HideHistoricButton = new GridLength(0.0);

        /// <summary>
        /// Show the button bar.
        /// </summary>
        private GridLength ViewAcceptButton = new GridLength(120.0);

        /// <summary>
        /// Show the middle column.
        /// </summary>
        private GridLength ViewMiddleColumn = new GridLength(1.0, GridUnitType.Star);

        /// <summary>
        /// Hide the button bar.
        /// </summary>
        private GridLength HideAcceptButton = new GridLength(0.0);

        /// <summary>
        /// Store normal color.
        /// </summary>
        private Brush brNormalForeColor = new SolidColorBrush(Color.FromArgb(255, 98, 103, 106));

        /// <summary>
        /// Store normal color.
        /// </summary>
        private Brush brNormalBackColor = new SolidColorBrush(Color.FromArgb(255, 198, 203, 206));

        /// <summary>
        /// Store editable control fore color.
        /// </summary>
        private Brush brEditableCtrlForeColor = new SolidColorBrush(Colors.White);

        #endregion

        #region Delegates

        /// <summary>
        /// Delegate that defines the firm of event produced when the Button Accept is pressed and the data of the
        /// Good is correct.
        /// </summary>
        /// <param name="NewOrEditedGood">New or Edited Good.</param>
        /// <param name="DataManagementId">Data Managed identifier.</param>
        public delegate void dlgAccept(CustomerOrdersView NewOrEditedGood, Guid DataManagementId);

        /// <summary>
        /// Delegate that defines the firm of event produced when the Button Cancel is pressed.
        /// </summary>
        public delegate void dlgCancel();

        #endregion

        #region Events

        /// <summary>
        /// Event produced when the Button Accept is pressed and the data of the Good is correct.
        /// </summary>
        public event dlgAccept EvAccept;

        /// <summary>
        /// Event produced when the Button Cancel is pressed.
        /// </summary>
        public event dlgCancel EvCancel;

        #endregion

        #region Attributes

        /// <summary>
        /// Store the Goods Window Type Active.
        /// </summary>
        private Goods GoodsWindow = null;

        /// <summary>
        /// Store the data to show in the List of movements.
        /// </summary>
        private ObservableCollection<CustomerOrderMovementsView> m_DataList = new ObservableCollection<CustomerOrderMovementsView>();

        /// <summary>
        /// Store the Data Management associated at Customer Order Movements of this Customer Order
        /// </summary>
        private Guid m_DataManagementId = Guid.Empty;

        /// <summary>
        /// Store the Current Id for the new Customer Order Movements.
        /// </summary>
        private int m_CurrentIdForNewCustomerOrderMovement = 0;

        /// <summary>
        /// Store the Good data to manage.
        /// </summary>
        private CustomerOrdersView m_CustomerOrder = null;

        /// <summary>
        /// Store the Parameters View
        /// </summary>
        private ParametersView m_Parameters;

        /// <summary>
        /// Store the Goods
        /// </summary>
        private Dictionary<string, GoodsView> m_Goods;

        /// <summary>
        /// Store the Customers
        /// </summary>
        public Dictionary<string, CustomersView> m_Customers;

        /// <summary>
        /// Store the SendTypes
        /// </summary>
        public Dictionary<string, SendTypesView> m_SendTypes;

        /// <summary>
        /// Store the EffectTypes
        /// </summary>
        public Dictionary<string, EffectTypesView> m_EffectTypes;

        /// <summary>
        /// Store the Agents
        /// </summary>
        public Dictionary<string, AgentsView> m_Agents;

        /// <summary>
        /// Window instance of StoreAddress.
        /// </summary>
        private AddressStores AddressStoresWindow = null;

        /// <summary>
        /// Store the Agents Window Type Active.
        /// </summary>
        private Agents AgentsWindow = null;

        /// <summary>
        /// Window instance of HistoCustomers.
        /// </summary>
        private HistoCustomers HistoCustomersWindow = null;

        /// <summary>
        /// Window instance of CustomerOrderMovementsData.
        /// </summary>
        private CustomerOrderMovementsData CustomerOrderMovementsDataWindow = null;

        /// <summary>
        /// Context Menu to calculate and validate the IBAN.
        /// </summary>
        private ContextMenu ctxmnuIBAN_Initial;

        /// <summary>
        /// Store the type of Application with the user want open.
        /// </summary>
        private ApplicationType m_AppType;

        /// <summary>
        /// Store the Operation of the UserControl.
        /// </summary>
        private Operation m_CtrlOperation = Operation.Show;
        
        /// <summary>
        /// Stotre if the data of the Good has changed.
        /// </summary>
        private bool m_AreDataChanged;

        /// <summary>
        /// Store the list of Editable Controls.
        /// </summary>
        private List<Control> EditableControls = null;

        /// <summary>
        /// Store the list of Non Editable Controls.
        /// </summary>
        private List<Control> NonEditableControls = null;

        /// <summary>
        /// Store the list of Editable Controls.
        /// </summary>
        private List<Control> OnlyQueryControls = null;

     
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
            get
            {
                return (m_AppType);
            }
            set
            {
                m_AppType = value;
                ActualizeUserControlComponents();
            }
        }

        /// <summary>
        /// Get or Set the Customer Order data to manage.
        /// </summary>
        public CustomerOrdersView CustomerOrder
        {
            get
            {
                return (m_CustomerOrder);
            }
            set
            {
                if (value != null)
                {
                    AreDataChanged = false;
                    m_CustomerOrder = value;
                    EditedCustomerOrder = new CustomerOrdersView(m_CustomerOrder);
                    LoadDataInControls(m_CustomerOrder, true, 1);
                }
                else throw new ArgumentNullException("Error, no s'han trobat les dades de la Comanda de Client a carregar."); 
            }
        }

        /// <summary>
        /// Get or Set the data to show in List of Items.
        /// </summary>
        public ObservableCollection<CustomerOrderMovementsView> DataList
        {
            get
            {
                return (m_DataList);
            }
            set
            {
                if (!(value is null))
                {
                    m_DataList = value;
                    Goods = new Dictionary<string, GoodsView>();
                    foreach (CustomerOrderMovementsView movement in m_DataList)
                    {
                        if (!Goods.ContainsKey(movement.Good_Key))
                        {
                            Goods.Add(movement.Good_Key, movement.Good);
                        }
                    }
                }
                else m_DataList = new ObservableCollection<CustomerOrderMovementsView>();
                SourceDataList = new ObservableCollection<CustomerOrderMovementsView>(m_DataList);
                ListItems.ItemsSource = m_DataList;
                ListItems.DataContext = this;
                //CollectionViewSource.GetDefaultView(ListItems.ItemsSource).SortDescriptions.Add(new SortDescription("CustomerOrderMovement_Id_For_Sort", ListSortDirection.Ascending));
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).SortDescriptions.Add(new SortDescription("CustomerOrderMovement_RowOrder_For_Sort", ListSortDirection.Ascending));
                 ActualizeAvalilableUnitInfo();
                ActualizeAmountInfo(EditedCustomerOrder);
            }
        }

        /// <summary>
        /// Get or Set the Customers 
        /// </summary>
        private Dictionary<string, GoodsView> Goods
        {
            get
            {
                return m_Goods;
            }
            set
            {
                m_Goods = (value is null) ? new Dictionary<string, GoodsView>() : value;
            }
        }

        /// <summary>
        /// Get or Set the data to show in List of Items.
        /// </summary>
        private ObservableCollection<CustomerOrderMovementsView> SourceDataList { get; set; }

        /// <summary>
        /// Get or Set the Edited CustomerOrder information.
        /// </summary>
        private CustomerOrdersView EditedCustomerOrder
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set the Operation of the UserControl.
        /// </summary>
        public Operation CtrlOperation
        {
            get
            {
                return (m_CtrlOperation);
            }
            set
            {
                m_CtrlOperation = value;
                switch (m_CtrlOperation)
                {
                    case Operation.Show :
                         if (CustomerOrder == null) throw new InvalidOperationException("Error, impossible visualitzar una Comanda de Client sense dades.");
                         tbDataBankIBANCountryCode.ContextMenu = null;
                         tbCancel.Text = "Tornar";
                         break;
                    case Operation.Add:
                         CustomerOrdersView NewCustomerOrder = new CustomerOrdersView();
                         NewCustomerOrder.CustomerOrder_Id = GlobalViewModel.Instance.HispaniaViewModel.GetNextIdentityValueTable(NewCustomerOrder);
                         NewCustomerOrder.Date = DateTime.Now;
                         CustomerOrder = NewCustomerOrder;
                         tbDataBankIBANCountryCode.ContextMenu = ctxmnuIBAN_Initial;
                         tbCancel.Text = "Cancel·lar";
                         break;
                    case Operation.Edit:
                         if (CustomerOrder == null) throw new InvalidOperationException("Error, impossible editar una Comanda de Client sense dades.");
                         tbDataBankIBANCountryCode.ContextMenu = ctxmnuIBAN_Initial;
                         tbCancel.Text = "Cancel·lar";
                         break;
                }
                string properyValue;
                foreach (Control control in EditableControls)
                {
                    if (control is TextBox) ((TextBox)control).IsReadOnly = (m_CtrlOperation == Operation.Show);
                    else if (control is RichTextBox) ((RichTextBox)control).IsReadOnly = (m_CtrlOperation == Operation.Show);
                    else if (control is ComboBox) ((ComboBox)control).IsEnabled = (m_CtrlOperation != Operation.Show);
                    else if (control is ListView) ((ListView)control).IsEnabled = true;
                    else if (control is Button)
                    {
                        if (control == btnHistoric) btnHistoric.IsEnabled = (m_CtrlOperation == Operation.Edit);
                        else if (control == btnViewData) btnViewData.IsEnabled = true;
                        else ((Button)control).IsEnabled = (m_CtrlOperation != Operation.Show);
                    }
                    else if (control is GroupBox)
                    {
                        properyValue = ((m_CtrlOperation == Operation.Show) ? "NonEditableGroupBox" : "EditableGroupBox");
                        ((GroupBox)control).SetResourceReference(Control.StyleProperty, properyValue);
                    }
                    else if (control is TabItem)
                    {
                        properyValue = ((m_CtrlOperation == Operation.Show) ? "NonEditableTabItem" : "EditableTabItem");
                        ((TabItem)control).SetResourceReference(Control.StyleProperty, properyValue);
                    }
                    else control.IsEnabled = (m_CtrlOperation != Operation.Show);
                }
                foreach (Control control in OnlyQueryControls)
                {
                    if (control is Button) control.IsEnabled = (m_CtrlOperation == Operation.Show);
                    else if (control is GroupBox)
                    {
                        properyValue = ((m_CtrlOperation == Operation.Show) ? "EditableGroupBox" : "NonEditableGroupBox");
                        ((GroupBox)control).SetResourceReference(Control.StyleProperty, properyValue);
                    }
                }
                dtpDeliveryNoteDate.IsEnabled = (m_CtrlOperation == Operation.Edit) && (CustomerOrder.DeliveryNote_Id > 0);
                lblDeliveryNoteDate.IsEnabled = (m_CtrlOperation == Operation.Edit) && (CustomerOrder.DeliveryNote_Id > 0);
                tiHeaderData.Focus();
                tbItemToSearch.Focus();
                ActualizeButtonBar();
            }
        }

        /// <summary>
        /// Get or Set if the data of the Good has changed.
        /// </summary>
        private bool AreDataChanged
        {
            get
            {
                return (m_AreDataChanged);
            }
            set
            {
                m_AreDataChanged = value;
                if (m_AreDataChanged)
                {
                    cbAcceptButton.Width = ViewAcceptButton;
                    cbMiddleColumn.Width = ViewMiddleColumn;
                }
                else
                {
                    cbAcceptButton.Width = HideAcceptButton;
                    cbMiddleColumn.Width = HideAcceptButton;
                }
            }
        }

        /// <summary>
        /// Gets or Set the Data Management associated at Customer Order Movements of this Customer Order
        /// </summary>
        private Guid DataManagementId
        {
            get
            {
                return m_DataManagementId;
            }
            set
            {
                if (value != null) m_DataManagementId = value;
                else m_DataManagementId = Guid.Empty;
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
        /// Store the Current Id for the new Customer Order Movements.
        /// </summary>
        private int CurrentIdForNewCustomerOrderMovement
        {
            get
            {
                return m_CurrentIdForNewCustomerOrderMovement;
            }
            set
            {
                m_CurrentIdForNewCustomerOrderMovement = value;
            }
        }

        #region Foreign Keys

        /// <summary>
        /// Get or Set Parameters 
        /// </summary>
        public ParametersView Parameters
        {
            get
            {
                return m_Parameters;
            }
            set
            {
                m_Parameters = value;
            }
        }

        /// <summary>
        /// Get or Set Customers 
        /// </summary>
        public Dictionary<string, CustomersView> Customers
        {
            get
            {
                return (m_Customers);
            }
            set
            {
                if (value is null) m_Customers = new Dictionary<string, CustomersView>();
                else m_Customers = value;
                cbCustomer.ItemsSource = new SortedDictionary<string, CustomersView>(m_Customers);
                cbCustomer.DisplayMemberPath = "Key";
                cbCustomer.SelectedValuePath = "Value";
                CollectionViewSource.GetDefaultView(cbCustomer.ItemsSource).Filter = UserFilter;
            }
        }

        /// <summary>
        /// Get or Set the Send Types 
        /// </summary>
        public Dictionary<string, SendTypesView> SendTypes
        {
            get
            {
                return (m_SendTypes);
            }
            set
            {
                m_SendTypes = value;
                if (m_SendTypes != null)
                {
                    cbSendType.ItemsSource = m_SendTypes;
                    cbSendType.DisplayMemberPath = "Key";
                    cbSendType.SelectedValuePath = "Value";
                }
            }
        }

        /// <summary>
        /// Get or Set the Effect Types 
        /// </summary>
        public Dictionary<string, EffectTypesView> EffectTypes
        {
            get
            {
                return (m_EffectTypes);
            }
            set
            {
                m_EffectTypes = value;
                if (m_EffectTypes != null)
                {
                    cbEffectType.ItemsSource = m_EffectTypes;
                    cbEffectType.DisplayMemberPath = "Key";
                    cbEffectType.SelectedValuePath = "Value";
                }
            }
        }

        /// <summary>
        /// Get or Set the Agents 
        /// </summary>
        public Dictionary<string, AgentsView> Agents
        {
            get
            {
                return (m_Agents);
            }
            set
            {
                m_Agents = value;
                if (m_Agents != null)
                {
                    cbBillingDataAgent.ItemsSource = m_Agents;
                    cbBillingDataAgent.DisplayMemberPath = "Key";
                    cbBillingDataAgent.SelectedValuePath = "Value";
                }
            }
        }

        #endregion

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
        /// Builder by default of this control.
        /// </summary>
        public CustomerOrdersData()
        {
            //  Initialization of controls of the UserControl
                InitializeComponent();
            //  Initialize GUI.
                ctxmnuIBAN_Initial = ctxmnuIBAN;
                InitEditableControls();
                InitNonEditableControls();
                InitOnlyQueryControls();
            //  Load Data in Window components.
                LoadDataInWindowComponents();
            //  Load the managers of the controls of the UserControl.
                LoadManagers();
                CheckRowOrder();
        }

        #endregion

        #region Standings

        /// <summary>
        /// Initialize the list of NonEditable Controls.
        /// </summary>
        private void InitNonEditableControls()
        {
            NonEditableControls = new List<Control>
            {
                lblCustomerOrderId,
                tbCustomerOrderId,
                lblDeliveryNoteId,
                tbDeliveryNoteId,
                tbDeliveryNoteDate,
                lblBillId,
                tbBillId,
                lblBillDate,
                tbBillDate,
                lblBillSerieId,
                tbBillSerieId,
                lblCustomerAlias,
                tbCustomerAlias,
                lblCompanyName,
                tbCompanyName,
                lblCompanyCif,
                tbCompanyCif,
                lblCompanyNumProv,
                tbCompanyNumProv,
                lblSendAddress,
                tbSendAddress,
                lblSendPostalCode,
                tbSendPostalCode,
                lblSendTimetable,
                tbSendTimetable,
                lblCustomerRemarks,
                tbCustomerRemarks,
                lblBillingDataNumUnpaid,
                tbBillingDataNumUnpaid,
                tbShippingUnitAvailable,
                tbBillingUnitAvailable,
                lblIVAPercent,
                tbIVAPercent,
                lblSurchargePercent,
                tbSurchargePercent,
                lblBillingDataEarlyPaymentDiscount,
                tbBillingDataEarlyPaymentDiscount,
                tbCustomerId,
                tbCustomerAliasClient,
                tbCompanyNameClient,
                tbCompanyContactPerson,
                tbCompanyEmail,
                tbCompanyAddress,                
                tbCompanyPhone1,
                tbCompanyPhone2,
                tbCompanyMobilePhone,
                tbCompanyFax,
                tbCompanyTimeTable,
                tbCompanyNumProvClient,
                cbCompanyPostalCode
        };
        }

        /// <summary>
        /// Initialize the list of Editable Controls.
        /// </summary>
        private void InitEditableControls()
        {
            EditableControls = new List<Control>
            {
                lblDeliveryNoteDate,
                dtpDeliveryNoteDate,
                lblCustomer,
                cbCustomer,
                lblAccording,
                chkbAccording,
                lblValued,
                chkbValued,
                lblAddressStores,
                cbAddressStores,
                btnAddressStores,
                lblSendType,
                cbSendType,
                lblCustomerOrderRemarks,
                tbCustomerOrderRemarks,
                lblEffectType,
                cbEffectType,
                lblNumEffect,
                tbNumEffect,
                lblDataBank_Bank,
                tbDataBank_Bank,
                lblDataBank_BankAddress,
                tbDataBank_BankAddress,
                lblBilling_DataBank_Bank,
                tbBilling_DataBank_Bank,
                lblBilling_DataBank_BankAddress,
                tbBilling_DataBank_BankAddress,
                lblDataBankFirstExpirationData,
                tbDataBankFirstExpirationData,
                lblDataBankExpirationInterval,
                tbDataBankExpirationInterval,
                lblDataBankPayday_1,
                tbDataBankPayday_1,
                lblDataBankPayday_2,
                tbDataBankPayday_2,
                lblDataBankPayday_3,
                tbDataBankPayday_3,
                lblDataBankIBANCountryCode,
                tbDataBankIBANCountryCode,
                lblDataBankIBANBankCode,
                tbDataBankIBANBankCode,
                lblDataBankIBANOfficeCode,
                tbDataBankIBANOfficeCode,
                lblDataBankIBANCheckDigits,
                tbDataBankIBANCheckDigits,
                lblDataBankIBANAccountNumber,
                tbDataBankIBANAccountNumber,
                lblBillingDataAgent,
                cbBillingDataAgent,
                btnAgents,
                gbItemsList,
                ListItems,
                gbIBAN,
                tiHeaderData,
                tiLines,
                tiFootData,
                tiBillingData,
                tiDivers,
                btnAdd,
                btnHistoric,
                btnEdit,
                btnDelete,
                btnViewData,
                btnAddGood,
                btnAccordingMovement,
                btnUnAccordingMovement,
                cbFieldItemToSearch,
                tbItemToSearch,
                btnAcceptSearch,
                chkPrevisioLliurament,
                dtpPrevisioLliurament,
                btnUp,
                btnDown
            };
        }

        /// <summary>
        /// Initialize the list of NonEditable Controls.
        /// </summary>
        private void InitOnlyQueryControls()
        {
            OnlyQueryControls = new List<Control>
            {
            };
        }

        /// <summary>
        /// Method that actualize the UserControl components.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        private void ActualizeUserControlComponents()
        {
            //  Apply Theme to UserControl.
                ThemeManager.ActualTheme = AppTheme;
            //  Put the UserControl in Initial State.
                cbAcceptButton.Width = HideAcceptButton;            
        }
                
        /// <summary>
        /// Method that load data in Window components.
        /// </summary>
        private void LoadDataInWindowComponents()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Set Data into the Window.
                cbFieldItemToSearch.ItemsSource = CustomersView.Fields;
                cbFieldItemToSearch.DisplayMemberPath = "Key";
                cbFieldItemToSearch.SelectedValuePath = "Value";
                if (CustomersView.Fields.Count > 0) cbFieldItemToSearch.SelectedIndex = 1;
            //  Set valid dates for change DeliveryNote Date
                dtpDeliveryNoteDate.DisplayDateStart = new DateTime(DateTime.Now.Year, 1, 1);
                dtpDeliveryNoteDate.DisplayDateEnd = new DateTime(DateTime.Now.Year, 12, 31);
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        /// <summary>
        /// Method that loads the Data in the controls of the Window
        /// </summary>
        private void LoadDataInControls(CustomerOrdersView CustomerOrder, bool Actualize = true, int ThrowException = 0)
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Main Controls
                tbCustomerOrderId.Text = GlobalViewModel.GetStringFromIntIdValue(CustomerOrder.CustomerOrder_Id);
                tbCustomerOrderDate.Text = CustomerOrder.Date_Str;
                tbDeliveryNoteId.Text = CustomerOrder.DeliveryNote_Id_Str;
                tbDeliveryNoteDate.Text = CustomerOrder.DeliveryNote_Date_Str;
                dtpDeliveryNoteDate.SelectedDate = CustomerOrder.DeliveryNote_Date;
                tbBillId.Text = CustomerOrder.Bill_Id_Str;
                tbBillDate.Text = CustomerOrder.Bill_Date_Str;
                tbBillSerieId.Text = CustomerOrder.Bill_Serie_Str;
                chkbAccording.IsChecked = CustomerOrder.According;
                chkbValued.IsChecked = CustomerOrder.Valued;
                chkPrevisioLliurament.IsChecked = CustomerOrder.PrevisioLliurament;
                dtpPrevisioLliurament.Visibility = Visibility.Hidden;
                if (CustomerOrder.PrevisioLliurament)
                {
                    dtpPrevisioLliurament.Visibility = Visibility.Visible;
                    dtpPrevisioLliurament.SelectedDate = CustomerOrder.PrevisioLliuramentData;
                }
            //  Header Data Tab Controls
                if (CustomerOrder.Customer is null)
                {
                    cbCustomer.SelectedIndex = -1;
                    tbItemToSearch.Text = string.Empty;
                    tbCustomerAlias.Text = string.Empty;
                    tbCompanyName.Text = string.Empty;
                    tbCompanyCif.Text = string.Empty;
                    tbCompanyNumProv.Text = string.Empty;
                    ActualizeCustomerAddressData();
                }
                else
                {
                    string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeyCustomerView(CustomerOrder.Customer);
                    if (!Customers.ContainsKey(Key))
                    {
                        Customers.Add(Key, GlobalViewModel.Instance.HispaniaViewModel.GetCustomerFromDb(CustomerOrder.Customer));
                        ((SortedDictionary<string, CustomersView>) cbCustomer.ItemsSource).Add(Key, GlobalViewModel.Instance.HispaniaViewModel.GetCustomerFromDb(CustomerOrder.Customer));
                    }
                    cbCustomer.SelectedValue = Customers[Key];
                    tbCustomerAlias.Text = CustomerOrder.Customer.Customer_Alias;
                    tbCompanyName.Text = CustomerOrder.Customer.Company_Name;
                    tbCompanyCif.Text = CustomerOrder.Customer.Company_Cif;
                    tbCompanyNumProv.Text = CustomerOrder.Customer.Company_NumProv;
                    //  Dades de Client Tab Controls
                    tbCustomerId.Text = CustomerOrder.Customer.Customer_Id.ToString();
                    tbCustomerAliasClient.Text = CustomerOrder.Customer.Customer_Alias;
                    tbCompanyNameClient.Text = CustomerOrder.Customer.Company_Name;
                    tbCompanyContactPerson.Text = CustomerOrder.Customer.Company_ContactPerson;
                    tbCompanyEmail.Text = CustomerOrder.Customer.Company_EMail;
                    tbCompanyAddress.Text = CustomerOrder.Customer.Company_Address;
                    cbCompanyPostalCode.Text = CustomerOrder.Customer.Company_PostalCode_Str;
                    tbCompanyPhone1.Text = CustomerOrder.Customer.Company_Phone_1;
                    tbCompanyPhone2.Text = CustomerOrder.Customer.Company_Phone_2;
                    tbCompanyMobilePhone.Text = CustomerOrder.Customer.Company_MobilePhone;
                    tbCompanyFax.Text = CustomerOrder.Customer.Company_Fax;
                    tbCompanyTimeTable.Text = CustomerOrder.Customer.Company_TimeTable;
                    tbCompanyNumProvClient.Text= CustomerOrder.Customer.Company_NumProv;

                    ActualizeCustomerAddressData(CustomerOrder.Customer);
                }
                tbIVAPercent.Text = GlobalViewModel.GetStringFromDecimalValue(CustomerOrder.IVAPercent, DecimalType.Percent, true);
                tbSurchargePercent.Text = GlobalViewModel.GetStringFromDecimalValue(CustomerOrder.SurchargePercent, DecimalType.Percent, true);
                ActualizeSendAddresInfo(CustomerOrder);
            //  Line Data Tab Controls
                if (Actualize)
                {
                    DataList = GlobalViewModel.Instance.HispaniaViewModel.GetCustomerOrderMovements(CustomerOrder.CustomerOrder_Id);
                    DataManagementId = GlobalViewModel.Instance.HispaniaViewModel.InitializeDataManaged(DataList);
                }
            //  Foot Data Tab Controls
                tbBillingDataEarlyPaymentDiscount.Text = GlobalViewModel.GetStringFromDecimalValue(CustomerOrder.BillingData_EarlyPaymentDiscount, DecimalType.Percent, true);
                ActualizeAmountInfo(CustomerOrder);
                tbCustomerOrderRemarks.Text = CustomerOrder.Remarks;
            //  Data Bank Tab Controls
                tbNumEffect.Text = GlobalViewModel.GetStringFromDecimalValue(CustomerOrder.DataBank_NumEffect, DecimalType.WithoutDecimals);
                tbDataBankFirstExpirationData.Text = GlobalViewModel.GetStringFromDecimalValue(CustomerOrder.DataBank_ExpirationDays, DecimalType.WithoutDecimals);
                tbDataBankExpirationInterval.Text = GlobalViewModel.GetStringFromDecimalValue(CustomerOrder.DataBank_ExpirationInterval, DecimalType.WithoutDecimals);
                tbDataBankPayday_1.Text = GlobalViewModel.GetStringFromDecimalValue(CustomerOrder.DataBank_Payday_1, DecimalType.WithoutDecimals);
                tbDataBankPayday_2.Text = GlobalViewModel.GetStringFromDecimalValue(CustomerOrder.DataBank_Payday_2, DecimalType.WithoutDecimals);
                tbDataBankPayday_3.Text = GlobalViewModel.GetStringFromDecimalValue(CustomerOrder.DataBank_Payday_3, DecimalType.WithoutDecimals);
                tbDataBank_Bank.Text = tbBilling_DataBank_Bank.Text = CustomerOrder.DataBank_Bank;
                tbDataBank_BankAddress.Text = tbBilling_DataBank_BankAddress.Text = CustomerOrder.DataBank_BankAddress;
                tbDataBankIBANCountryCode.Text = CustomerOrder.DataBank_IBAN_CountryCode;
                tbDataBankIBANBankCode.Text = CustomerOrder.DataBank_IBAN_BankCode;
                tbDataBankIBANOfficeCode.Text = CustomerOrder.DataBank_IBAN_OfficeCode;
                tbDataBankIBANCheckDigits.Text = CustomerOrder.DataBank_IBAN_CheckDigits;
                tbDataBankIBANAccountNumber.Text = CustomerOrder.DataBank_IBAN_AccountNumber;
            //  Various Tab Controls
                if (CustomerOrder.Customer is null)
                {
                    tbCustomerRemarks.Text = String.Empty;
                    tbBillingDataNumUnpaid.Text = string.Empty;
                }
                else
                {
                    tbCustomerRemarks.Text = CustomerOrder.Customer.Several_Remarks;
                    tbBillingDataNumUnpaid.Text = GlobalViewModel.GetStringFromDecimalValue(CustomerOrder.Customer.BillingData_NumUnpaid, DecimalType.WithoutDecimals);                    
                }
                LoadExternalTablesInfo(CustomerOrder, ThrowException);
            //  Update historic button
                btnHistoric.IsEnabled = true;
            //  Activate managers
                DataChangedManagerActive = true;                
        }

        /// <summary>
        /// Load Data from External Tables.
        /// </summary>
        /// <param name="customerOrdersView">Data Container.</param>
        /// <param name="ThrowException">true, if want throw an exception if not found a component</param>
        private void LoadExternalTablesInfo(CustomerOrdersView customerOrder, int ThrowException = 0)
        {
            if ((EffectTypes != null) && (customerOrder.EffectType != null))
            {
                Dictionary<string, EffectTypesView> Items = (Dictionary<string, EffectTypesView>)cbEffectType.ItemsSource;
                string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeyEffectTypeView(customerOrder.EffectType);
                if (Items.ContainsKey(Key)) cbEffectType.SelectedValue = EffectTypes[Key];
                else
                {
                    if (ThrowException == 1)
                    {
                        throw new Exception(string.Format("No s'ha trobat el Tipus d'Efecte '{0}-{1}'.", EffectTypes[Key].Code, EffectTypes[Key].Description));
                    }
                }
            }
            else cbEffectType.SelectedIndex = -1;
            if ((SendTypes != null) && (customerOrder.SendType != null))
            {
                Dictionary<string, SendTypesView> Items = (Dictionary<string, SendTypesView>)cbSendType.ItemsSource;
                string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeySendTypeView(customerOrder.SendType);
                if (Items.ContainsKey(Key)) cbSendType.SelectedValue = SendTypes[Key];
                else
                {
                    if (ThrowException == 1)
                    {
                        throw new Exception(string.Format("No s'ha trobat el Tipus d'Enviament '{0}-{1}'.", SendTypes[Key].Code, SendTypes[Key].Description));
                    }
                }
            }
            else cbSendType.SelectedIndex = -1;
            if ((Agents != null) && (customerOrder.BillingData_Agent != null))
            {
                Dictionary<string, AgentsView> Items = (Dictionary<string, AgentsView>)cbBillingDataAgent.ItemsSource;
                string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeyAgentView(customerOrder.BillingData_Agent);
                if (Items.ContainsKey(Key)) cbBillingDataAgent.SelectedValue = Agents[Key];
                else
                {
                    if (ThrowException == 1)
                    {
                        throw new Exception(string.Format("No s'ha trobat el Representant '{0}'.", Agents[Key].Name));
                    }
                }
            }
            else cbBillingDataAgent.SelectedIndex = -1;
        }

        /// <summary>
        /// Method that filter the elements that are showing in the list
        /// </summary>
        /// <param name="item">Item to test</param>
        /// <returns>true, if the item must be loaded, false, if not.</returns>
        private bool UserFilter(object item)
        {
            //  Determine if is needed aplicate one filter.
                if (cbFieldItemToSearch.SelectedIndex == -1) return (true);
            //  Get Acces to the object and the property name To Filter.
                CustomersView ItemData = ((KeyValuePair<string, CustomersView>)item).Value;
                String ProperyName = (string) cbFieldItemToSearch.SelectedValue;
            //  Apply the filter by selected field value
                if (!String.IsNullOrEmpty(tbItemToSearch.Text))
                {
                    object valueToTest = ItemData.GetType().GetProperty(ProperyName).GetValue(ItemData);
                    if ((valueToTest is null) ||
                        (!(valueToTest.ToString().ToUpper()).Contains(tbItemToSearch.Text.ToUpper())))
                    {
                        return false;
                    }
                }
                return true;
        }

        #endregion

        #region Managers

        /// <summary>
        /// Method that define the managers needed for the user operations in the UserControl
        /// </summary>
        private void LoadManagers()
        {
            //  By default the manager for the Customer Order Data changes is active.
                DataChangedManagerActive = true;
            //  TextBox
                tbBillingDataEarlyPaymentDiscount.PreviewTextInput += TBPreviewTextInput;
                tbBillingDataEarlyPaymentDiscount.TextChanged += TBPrecentDataChanged;
                tbCustomerOrderRemarks.PreviewTextInput += TBPreviewTextInput;
                tbCustomerOrderRemarks.TextChanged += TBDataChanged;
                tbNumEffect.PreviewTextInput += TBPreviewTextInput;
                tbNumEffect.TextChanged += TBDataChanged;
                tbDataBank_Bank.PreviewTextInput += TBPreviewTextInput;
                tbDataBank_Bank.TextChanged += TBDataChanged;
                tbDataBank_BankAddress.PreviewTextInput += TBPreviewTextInput;
                tbDataBank_BankAddress.TextChanged += TBDataChanged;
                tbBilling_DataBank_Bank.PreviewTextInput += TBPreviewTextInput;
                tbBilling_DataBank_Bank.TextChanged += TBDataChanged;
                tbBilling_DataBank_BankAddress.PreviewTextInput += TBPreviewTextInput;
                tbBilling_DataBank_BankAddress.TextChanged += TBDataChanged;
                tbDataBankFirstExpirationData.PreviewTextInput += TBPreviewTextInput;
                tbDataBankFirstExpirationData.TextChanged += TBDataChanged;
                tbDataBankExpirationInterval.PreviewTextInput += TBPreviewTextInput;
                tbDataBankExpirationInterval.TextChanged += TBDataChanged;
                tbDataBankPayday_1.PreviewTextInput += TBPreviewTextInput;
                tbDataBankPayday_1.TextChanged += TBDataChanged;
                tbDataBankPayday_2.PreviewTextInput += TBPreviewTextInput;
                tbDataBankPayday_2.TextChanged += TBDataChanged;
                tbDataBankPayday_3.PreviewTextInput += TBPreviewTextInput;
                tbDataBankPayday_3.TextChanged += TBDataChanged;
                tbDataBankIBANCountryCode.PreviewTextInput += TBPreviewTextInput;
                tbDataBankIBANCountryCode.TextChanged += TBDataChanged;
                tbDataBankIBANBankCode.PreviewTextInput += TBPreviewTextInput;
                tbDataBankIBANBankCode.TextChanged += TBDataChanged;
                tbDataBankIBANOfficeCode.PreviewTextInput += TBPreviewTextInput;
                tbDataBankIBANOfficeCode.TextChanged += TBDataChanged;
                tbDataBankIBANCheckDigits.PreviewTextInput += TBPreviewTextInput;
                tbDataBankIBANCheckDigits.TextChanged += TBDataChanged;
                tbDataBankIBANAccountNumber.PreviewTextInput += TBPreviewTextInput;
                tbDataBankIBANAccountNumber.TextChanged += TBDataChanged;
                tbItemToSearch.TextChanged += TBItemToSearchDataChanged;
            //  DatePiker
                dtpDeliveryNoteDate.SelectedDateChanged += DtpDeliveryNoteDate_SelectedDateChanged;
            dtpPrevisioLliurament.SelectedDateChanged += DtpPrevisioLliurament_SelectedDateChanged;
            //  CheckBox
                chkbAccording.Checked += ChkbAccording_Checked;
                chkbAccording.Unchecked += ChkbAccording_Unchecked;
                chkbValued.Checked += ChkbValued_Checked;
                chkbValued.Unchecked += ChkbValued_Unchecked;
            chkPrevisioLliurament.Checked += ChkPrevisioLliurament_Checked;
            chkPrevisioLliurament.Unchecked += ChkPrevisioLliurament_Unchecked;
            //  ComboBox
            cbAddressStores.SelectionChanged += CbAddressStores_SelectionChanged;
                cbSendType.SelectionChanged += CbSendType_SelectionChanged;
                cbEffectType.SelectionChanged += CbEffectType_SelectionChanged;
                cbBillingDataAgent.SelectionChanged += CbBillingDataAgent_SelectionChanged;
                cbFieldItemToSearch.SelectionChanged += CbFieldItemToSearch_SelectionChanged;
            //  Define ListView events to manage.
                ListItems.SelectionChanged += ListItems_SelectionChanged;   
            //  ContextMenuItem
                ctxmnuItemCalculateIBAN.Click += CtxmnuItemCalculateIBAN_Click;
                ctxmnuItemValidateIBAN.Click += CtxmnuItemValidateIBAN_Click;
            //  Buttons
                btnAddressStores.Click += BtnAddressStores_Click;
                btnHistoric.Click += BtnHistoric_Click;
                btnAdd.Click += BtnAdd_Click;
                btnEdit.Click += BtnEdit_Click;
                btnViewData.Click += BtnViewData_Click;
                btnDelete.Click += BtnDelete_Click;
                btnAccordingMovement.Click += BtnAccordingMovement_Click;
                btnUnAccordingMovement.Click += BtnUnAccordingMovement_Click;
                btnAcceptSearch.Click += BtnAcceptSearch_Click;
                btnAddGood.Click += BtnAddGood_Click;
                btnAgents.Click += BtnAgents_Click;
                btnAccept.Click += BtnAccept_Click;
                btnCancel.Click += BtnCancel_Click;
        }

        private void DtpPrevisioLliurament_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            AreDataChanged = true;
            if (((DatePicker)sender).SelectedDate.HasValue)
            {
                EditedCustomerOrder.PrevisioLliuramentData = ((DatePicker)sender).SelectedDate.Value;
            }
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

        /// <summary>
        /// Checking the Input Char in function of the data type that has associated
        /// </summary>
        private void TBPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (sender == tbCustomerOrderRemarks) e.Handled = ! GlobalViewModel.IsValidCommentChar(e.Text);
            else if (sender == tbBillingDataEarlyPaymentDiscount) e.Handled = ! GlobalViewModel.IsValidPercentChar(e.Text);
            else if ((sender == tbDataBank_Bank) || (sender == tbBilling_DataBank_Bank))
            {
                e.Handled = ! GlobalViewModel.IsValidNameChar(e.Text);
            }
            else if ((sender == tbDataBank_BankAddress) || (sender == tbBilling_DataBank_BankAddress))
            {
                e.Handled = ! GlobalViewModel.IsValidAddressChar(e.Text);
            }
            else if ((sender == tbNumEffect) || (sender == tbDataBankFirstExpirationData) ||
                     (sender == tbDataBankExpirationInterval) || (sender == tbDataBankPayday_1) ||
                     (sender == tbDataBankPayday_2) || (sender == tbDataBankPayday_3))
            {
                e.Handled = ! GlobalViewModel.IsValidShortDecimalChar(e.Text);
            }
            else if (sender == tbDataBankIBANCountryCode) e.Handled = ! GlobalViewModel.IsValidIBAN_CountryCodeChar(e.Text);
            else if (sender == tbDataBankIBANBankCode) e.Handled = ! GlobalViewModel.IsValidIBAN_BankCodeChar(e.Text);
            else if (sender == tbDataBankIBANOfficeCode) e.Handled = ! GlobalViewModel.IsValidIBAN_OfficeCodeChar(e.Text);
            else if (sender == tbDataBankIBANCheckDigits) e.Handled = ! GlobalViewModel.IsValidIBAN_CheckDigitsChar(e.Text);
            else if (sender == tbDataBankIBANAccountNumber) e.Handled = ! GlobalViewModel.IsValidIBAN_AccountNumberChar(e.Text);
        }

        /// <summary>
        /// Manage the change of the Data in the sender object.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void TBDataChanged(object sender, TextChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                string value = ((TextBox)sender).Text;
                try
                {
                    if (sender == tbNumEffect) EditedCustomerOrder.DataBank_NumEffect = GlobalViewModel.GetDecimalValue(value);
                    else if ((sender == tbDataBank_Bank) || (sender == tbBilling_DataBank_Bank))
                    {
                        if (sender == tbDataBank_Bank) tbBilling_DataBank_Bank.Text = value;
                        else tbDataBank_Bank.Text = value;
                        EditedCustomerOrder.DataBank_Bank = value;
                    }
                    else if ((sender == tbDataBank_BankAddress) || (sender == tbBilling_DataBank_BankAddress))
                    {
                        if (sender == tbDataBank_BankAddress) tbBilling_DataBank_BankAddress.Text = value;
                        else tbDataBank_BankAddress.Text = value;
                        EditedCustomerOrder.DataBank_BankAddress = value;
                    }
                    else if (sender == tbCustomerOrderRemarks) EditedCustomerOrder.Remarks = value;
                    else if (sender == tbDataBankFirstExpirationData) EditedCustomerOrder.DataBank_ExpirationDays = GlobalViewModel.GetDecimalValue(value);
                    else if (sender == tbDataBankExpirationInterval) EditedCustomerOrder.DataBank_ExpirationInterval = GlobalViewModel.GetDecimalValue(value);
                    else if (sender == tbDataBankPayday_1) EditedCustomerOrder.DataBank_Payday_1 = GlobalViewModel.GetDecimalValue(value);
                    else if (sender == tbDataBankPayday_2) EditedCustomerOrder.DataBank_Payday_2 = GlobalViewModel.GetDecimalValue(value);
                    else if (sender == tbDataBankPayday_3) EditedCustomerOrder.DataBank_Payday_3 = GlobalViewModel.GetDecimalValue(value);
                    else if (sender == tbDataBankIBANCountryCode) EditedCustomerOrder.DataBank_IBAN_CountryCode = value;
                    else if (sender == tbDataBankIBANBankCode) EditedCustomerOrder.DataBank_IBAN_BankCode = value;
                    else if (sender == tbDataBankIBANOfficeCode) EditedCustomerOrder.DataBank_IBAN_OfficeCode = value;
                    else if (sender == tbDataBankIBANCheckDigits) EditedCustomerOrder.DataBank_IBAN_CheckDigits = value;
                    else if (sender == tbDataBankIBANAccountNumber) EditedCustomerOrder.DataBank_IBAN_AccountNumber = value;
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedCustomerOrder, false);
                }
                AreDataChanged = (EditedCustomerOrder != CustomerOrder);
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Manage the change of the Data in the sender object.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void TBPrecentDataChanged(object sender, TextChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                TextBox tbInput = (TextBox)sender;
                try
                {
                    GlobalViewModel.NormalizeTextBox(sender, e, DecimalType.Percent);
                    decimal value = GlobalViewModel.GetUIDecimalValue(tbInput.Text);
                    if (sender == tbBillingDataEarlyPaymentDiscount) EditedCustomerOrder.BillingData_EarlyPaymentDiscount = value;
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedCustomerOrder, false);
                }
                AreDataChanged = (EditedCustomerOrder != CustomerOrder);
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void TBItemToSearchDataChanged(object sender, TextChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                FilterDataListObjects();
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region Buttons

        #region Accept

        /// <summary>
        /// Accept the edition or creation of the Good.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {
            CustomerOrdersAttributes ErrorField = CustomerOrdersAttributes.None;
            try
            {
                if ((CtrlOperation == Operation.Add) || (CtrlOperation == Operation.Edit))
                {
                    EditedCustomerOrder.Validate(out ErrorField);
                    EvAccept?.Invoke(new CustomerOrdersView(EditedCustomerOrder), DataManagementId);
                }
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(MsgManager.ExcepMsg(ex), MsgType.Error);
                RestoreSourceValue(ErrorField);
            }
        }

        #endregion

        #region Cancel

        /// <summary>
        /// Cancel the edition or creatin of the Good.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //  Send the event that indicates at the observer that the operation is cancelled.
                EvCancel?.Invoke();
        }

        #endregion
        
        #region Accept Customer Selection

        private void BtnAcceptSearch_Click(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                try
                {
                    if (cbCustomer.SelectedItem != null)
                    {
                        CustomersView customerSelected = ((CustomersView)cbCustomer.SelectedValue);
                        bool IsSameCustomer = (EditedCustomerOrder.Customer != null) &&
                                              (EditedCustomerOrder.Customer.Customer_Id == customerSelected.Customer_Id);
                        EditedCustomerOrder.Customer = customerSelected;
                        ActualizeEditedCustomerOrder(customerSelected, customerSelected.DataBank_EffectType);
                        if (!IsSameCustomer)
                        {
                            EditedCustomerOrder.Address = string.Empty;
                            EditedCustomerOrder.TimeTable = string.Empty;
                            EditedCustomerOrder.PostalCode = null;
                        }
                        EditedCustomerOrder.EffectType = customerSelected.DataBank_EffectType;
                        EditedCustomerOrder.BillingData_EarlyPaymentDiscount = customerSelected.BillingData_EarlyPaymentDiscount;
                        if (customerSelected.BillingData_IVAType is null)
                        {
                            EditedCustomerOrder.IVAPercent = 0;
                            EditedCustomerOrder.SurchargePercent = 0;
                        }
                        else
                        {
                            EditedCustomerOrder.IVAPercent = customerSelected.BillingData_IVAType.IVAPercent;
                            EditedCustomerOrder.SurchargePercent = customerSelected.BillingData_IVAType.SurchargePercent;
                        }
                        EditedCustomerOrder.SendType = customerSelected.BillingData_SendType;
                        EditedCustomerOrder.DataBank_NumEffect = customerSelected.DataBank_NumEffect;
                        EditedCustomerOrder.DataBank_ExpirationDays = customerSelected.DataBank_FirstExpirationData;
                        EditedCustomerOrder.DataBank_ExpirationInterval = customerSelected.DataBank_ExpirationInterval;
                        EditedCustomerOrder.DataBank_Payday_1 = customerSelected.DataBank_Payday_1;
                        EditedCustomerOrder.DataBank_Payday_2 = customerSelected.DataBank_Payday_2;
                        EditedCustomerOrder.DataBank_Payday_3 = customerSelected.DataBank_Payday_3;
                        EditedCustomerOrder.Customer.Several_Remarks = customerSelected.Several_Remarks;
                        EditedCustomerOrder.Customer.BillingData_NumUnpaid = customerSelected.BillingData_NumUnpaid;
                        EditedCustomerOrder.BillingData_Agent = customerSelected.BillingData_Agent;
                        EditedCustomerOrder.Valued = customerSelected.BillingData_Valued;
                        AreDataChanged = (EditedCustomerOrder != CustomerOrder);
                        LoadDataInControls(EditedCustomerOrder, false);
                        ActualizeCustomerAddressData(EditedCustomerOrder.Customer);
                        if (cbAddressStores.Items.Count > 0)
                        {
                            if (!IsSameCustomer)
                            {
                                cbAddressStores.SelectedIndex = 0;
                                AddressStoresView addressStore = (AddressStoresView)cbAddressStores.SelectedValue;
                                EditedCustomerOrder.Address = addressStore.Address;
                                EditedCustomerOrder.TimeTable = addressStore.Timetable;
                                EditedCustomerOrder.PostalCode = addressStore.PostalCode;
                                ActualizeSendAddresInfoData(EditedCustomerOrder);
                            }
                            else ActualizeSendAddresInfo(EditedCustomerOrder);
                        }
                        ShowGoodRemark();
                        ActualizeButtonBar();
                        cbSendType.Focus();
                        cbAddressStores.IsDropDownOpen = (EditedCustomerOrder.Customer != null) && (cbAddressStores.Items.Count > 0) && 
                                                         (!IsSameCustomer);
                    }
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, al carregar la informació del client.\r\nDetalls: {0}.", MsgManager.ExcepMsg(ex)));
                }
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region Address Stores (Update data)

        /// <summary>
        /// Defines a new Address Store for the Customer.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnAddressStores_Click(object sender, RoutedEventArgs e)
        {
            if (AddressStoresWindow == null)
            {
                try
                {
                    CustomersView Customer = EditedCustomerOrder.Customer;
                    GlobalViewModel.Instance.HispaniaViewModel.RefreshPostalCodes();
                    AddressStoresWindow = new AddressStores(AppType, Operation.Edit)
                    {

                        PostalCodes = GlobalViewModel.Instance.HispaniaViewModel.PostalCodesDict,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.GetAddressStores(Customer.Customer_Id),
                        Data = Customer
                    };
                    AddressStoresWindow.Closed += StoreAddressesWindow_Closed;
                    AddressStoresWindow.ShowDialog();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, al carregar la finestra d'edició de les adreces de magatzem.\r\nDetalls:{0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else AddressStoresWindow.Activate();
        }

        private void StoreAddressesWindow_Closed(object sender, EventArgs e)
        {
            ActualizeCustomerAddressData(EditedCustomerOrder.Customer);
            AddressStoresWindow.Closed -= StoreAddressesWindow_Closed;
            AddressStoresWindow = null;
        }

        #endregion

        #region Agents (Update data)

        /// <summary>
        /// Defines a new Agent for the CustomerOrder.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnAgents_Click(object sender, RoutedEventArgs e)
        {
            if (AgentsWindow == null)
            {
                try
                {
                    GlobalViewModel.Instance.HispaniaViewModel.RefreshPostalCodes();
                    GlobalViewModel.Instance.HispaniaViewModel.RefreshAgents();
                    AgentsWindow = new Agents(AppType)
                    {
                        PostalCodes = GlobalViewModel.Instance.HispaniaViewModel.PostalCodesDict,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.Agents
                    };
                    AgentsWindow.Closed += AgentsWindow_Closed;
                    AgentsWindow.Show();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, al carregar la finestra d'edició de representants.\r\nDetalls:{0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else AgentsWindow.Activate();
        }

        /// <summary>
        /// When the Customer Window is closed we actualize the AgentsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void AgentsWindow_Closed(object sender, EventArgs e)
        {
            ActualizeCustomerOrderAgentsData();
            AgentsWindow.Closed -= AgentsWindow_Closed;
            AgentsWindow = null;
        }

        #endregion

        #region Add Goods

        /// <summary>
        /// Allow the creation of a new good.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnAddGood_Click(object sender, RoutedEventArgs e)
        {
            if (GoodsWindow == null)
            {
                try
                {
                    GlobalViewModel.Instance.HispaniaViewModel.RefreshUnits();
                    GlobalViewModel.Instance.HispaniaViewModel.RefreshGoods();
                    GoodsWindow = new Goods(AppType)
                    {
                        Units = GlobalViewModel.Instance.HispaniaViewModel.UnitsDict,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.Goods
                    };
                    GoodsWindow.Closed += GoodsWindow_Closed;
                    GoodsWindow.Show();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, al carregar la finestra d'edició d'artícles.\r\nDetalls:{0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else GoodsWindow.Activate();
        }

        /// <summary>
        /// When the Goods Window is closed we actualize the GoodsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void GoodsWindow_Closed(object sender, EventArgs e)
        {
            CheckIfAddingNewArticle();
            ActualizeGoodsData();

            GoodsWindow.Closed -= GoodsWindow_Closed;
            GoodsWindow = null;
        }

        #endregion

        #region Movements (Update Data)

        #region View (View Data)

        private void BtnViewData_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItem != null)
            {
                CustomerOrderMovementsView Movement = (CustomerOrderMovementsView)ListItems.SelectedItem;
                if (CustomerOrderMovementsDataWindow == null)
                {
                    try
                    {
                        CustomerOrderMovementsDataWindow = new CustomerOrderMovementsData(AppType)
                        {
                            CtrlOperation = Operation.Show,
                            Goods = GlobalViewModel.Instance.HispaniaViewModel.GoodsActiveDict,
                            Data = new CustomerOrderMovementsView(Movement)
                        };
                        CustomerOrderMovementsDataWindow.Closed += CustomerOrderMovementsDataWindow_Closed;
                        CustomerOrderMovementsDataWindow.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(
                            string.Format("Error, a l'intentar visualitzar les dades de la línia '{0}'.\r\nDetalls: {1}",
                                            Movement.CustomerOrderMovement_Id, MsgManager.ExcepMsg(ex)));
                    }
                }
                else CustomerOrderMovementsDataWindow.Activate();
            }
        }

        private void CustomerOrderMovementsDataWindow_Closed(object sender, EventArgs e)
        {
            //  Undefine the close event manager.
            CustomerOrderMovementsDataWindow.Closed -= CustomerOrderMovementsDataWindow_Closed;
            CustomerOrderMovementsDataWindow = null;
        }

        #endregion

        #region Add (Update Data)

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerOrderMovementsDataWindow == null)
            {
                try
                {
                    m_CurrentIdForNewCustomerOrderMovement -= 1;
                    CustomerOrderMovementsView newMovement = new CustomerOrderMovementsView()
                    {
                        CustomerOrderMovement_Id = m_CurrentIdForNewCustomerOrderMovement,
                        CustomerOrder = EditedCustomerOrder
                    };
                    CustomerOrderMovementsDataWindow = new CustomerOrderMovementsData(AppType)
                    {
                        CtrlOperation = Operation.Add,
                        Goods = GlobalViewModel.Instance.HispaniaViewModel.GoodsActiveDict,
                        Data = newMovement
                    };
                    CustomerOrderMovementsDataWindow.Closed += CustomerOrderMovementsDataWindow_Add_Closed;
                    CustomerOrderMovementsDataWindow.ShowDialog();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, al carregar la finestra de creació d'una nova línia de comanda.\r\nDetalls:{0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else CustomerOrderMovementsDataWindow.Activate();
        }

        private void CustomerOrderMovementsDataWindow_Add_Closed(object sender, EventArgs e)
        {
            //  Actualize List of Customer Order Movements from historic of movements.
                if ((CustomerOrderMovementsDataWindow.AreDataChanged) && (!CustomerOrderMovementsDataWindow.IsCanceled))
                {
                    try
                    {
                        CustomerOrderMovementsView newMovement = CustomerOrderMovementsDataWindow.EditedCustomerOrderMovement;
                        GlobalViewModel.Instance.HispaniaViewModel.CreateItemInDataManaged(DataManagementId, newMovement);
                        DataList.Add(newMovement);
                        ActualizeGoodInfo(newMovement, MovementOp.Add);
                        ListItems.SelectedItem = newMovement;
                        ActualizeAmountInfo(EditedCustomerOrder);
                        AreDataChanged = AreNotEquals(DataList, SourceDataList);
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(
                           string.Format("Error, a l'actualitzar les dades de la nova línia de comanda.\r\nDetalls:{0}", MsgManager.ExcepMsg(ex)));
                    }
                }
            //  Undefine the close event manager.
                CustomerOrderMovementsDataWindow.Closed -= CustomerOrderMovementsDataWindow_Add_Closed;
                CustomerOrderMovementsDataWindow = null;
        }

        #endregion
                
        #region Update (Update Data)

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItem != null)
            {
                if (CustomerOrderMovementsDataWindow == null)
                {
                    CustomerOrderMovementsView Movement = (CustomerOrderMovementsView)ListItems.SelectedItem;
                    try
                    {
                        CustomerOrderMovementsDataWindow = new CustomerOrderMovementsData(AppType)
                        {
                            CtrlOperation = Operation.Edit,
                            Goods = GlobalViewModel.Instance.HispaniaViewModel.GoodsActiveDict,
                            Data = new CustomerOrderMovementsView(Movement)
                        };
                        CustomerOrderMovementsDataWindow.Closed += CustomerOrderMovementsDataWindow_Edit_Closed;
                        CustomerOrderMovementsDataWindow.ShowDialog();
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(
                           string.Format("Error, al carregar la finestra d'edició de la línia de comanda '{0}'.\r\nDetalls:{1}",
                                         Movement.CustomerOrderMovement_Id, MsgManager.ExcepMsg(ex)));
                    }
                }
                else CustomerOrderMovementsDataWindow.Activate();
            }
        }

        private void CustomerOrderMovementsDataWindow_Edit_Closed(object sender, EventArgs e)
        {
            //  Actualize List of Customer Order Movements from historic of movements.
                if ((CustomerOrderMovementsDataWindow.AreDataChanged) && (!CustomerOrderMovementsDataWindow.IsCanceled))
                {
                    CustomerOrderMovementsView currentMovement = (CustomerOrderMovementsView)ListItems.SelectedItem;
                    try
                    {
                        CustomerOrderMovementsView newMovement = CustomerOrderMovementsDataWindow.EditedCustomerOrderMovement;
                        GlobalViewModel.Instance.HispaniaViewModel.UpdateItemInDataManaged(DataManagementId, currentMovement, newMovement);
                        int Index = ListItems.SelectedIndex;
                        DataChangedManagerActive = false;
                        ListItems.SelectedItem = null;
                        DataChangedManagerActive = true;
                        DataList.Remove(currentMovement);
                        DataList.Insert(Index, newMovement);
                        ActualizeGoodInfo(currentMovement, newMovement);
                        ListItems.SelectedItem = newMovement;
                        ActualizeAmountInfo(EditedCustomerOrder);
                        AreDataChanged = AreNotEquals(DataList, SourceDataList);
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(
                           string.Format("Error, a l'actualitzar les dades de la línia de comanda '{0}'.\r\nDetalls:{1}", 
                                         currentMovement.CustomerOrderMovement_Id, MsgManager.ExcepMsg(ex)));
                    }
                }
            //  Undefine the close event manager.
                CustomerOrderMovementsDataWindow.Closed -= CustomerOrderMovementsDataWindow_Edit_Closed;
                CustomerOrderMovementsDataWindow = null;
        }

        #endregion

        #region Delete Movement (Update Data)

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItem != null)
            {
                CustomerOrderMovementsView Movement = (CustomerOrderMovementsView)ListItems.SelectedItem;
                try
                {
                    string Question = string.Format("Està segur que vol esborrar la línia de Comanda de Client '{0}' ?",
                                                    ((CustomerOrderMovementsView)ListItems.SelectedItem).CustomerOrderMovement_Id);
                    if (MsgManager.ShowQuestion(Question) == MessageBoxResult.Yes)
                    {
                        GlobalViewModel.Instance.HispaniaViewModel.DeleteItemInDataManaged(DataManagementId, Movement);
                        ListItems.SelectedItem = null;
                        DataList.Remove(Movement);
                        ActualizeGoodInfo(Movement, MovementOp.Remove, false);
                        ListItems.SelectedItem = null;
                        ActualizeAmountInfo(EditedCustomerOrder);
                        AreDataChanged = AreNotEquals(DataList, SourceDataList);
                    }
                    else MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'esborrar de la línia de comanda '{0}'.\r\nDetalls:{1}",
                                     Movement.CustomerOrderMovement_Id, MsgManager.ExcepMsg(ex)));
                }
            }
        }

        #endregion

        #region Historic (Update Data)

        private void BtnHistoric_Click(object sender, RoutedEventArgs e)
        {
            if (HistoCustomersWindow == null)
            {
                try
                {
                    HistoCustomersWindow = new HistoCustomers(AppType, HistoCustomersMode.CustomerOrderMovementInput)
                    {
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.GetHistoCustomers(EditedCustomerOrder.Customer.Customer_Id, true),
                        Data = EditedCustomerOrder.Customer
                    };
                    HistoCustomersWindow.Closed += HistoCustomersWindow_Closed;
                    HistoCustomersWindow.ShowDialog();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, al carregar la finestra per afegir linies de comanda des dels històrics de client.\r\nDetalls:{0}", 
                                     MsgManager.ExcepMsg(ex)));
                }
            }
            else HistoCustomersWindow.Activate();
        }

        private void HistoCustomersWindow_Closed(object sender, EventArgs e)
        {
            //  Actualize List of Customer Order Movements from historic of movements.
                foreach (HistoCustomersView histoCustomer in HistoCustomersWindow.HistoCustomerSelected)
                {
                    try
                    {
                        m_CurrentIdForNewCustomerOrderMovement -= 1;
                        CustomerOrderMovementsView newMovement = new CustomerOrderMovementsView()
                        {
                            CustomerOrderMovement_Id = m_CurrentIdForNewCustomerOrderMovement,
                            CustomerOrder = EditedCustomerOrder,
                            Good = histoCustomer.Good,
                            Description = histoCustomer.Good_Description,
                            Unit_Shipping = histoCustomer.Shipping_Units,
                            Unit_Billing = histoCustomer.Billing_Units,
                            RetailPrice = histoCustomer.Retail_Price,
                            Comission = histoCustomer.Comission,
                            Unit_Billing_Definition = histoCustomer.Unit_Billing_Definition,
                            Unit_Shipping_Definition = histoCustomer.Unit_Shipping_Definition,
                            Remark = string.Empty,
                            According = false,
                            Comi = false
                        };
                        DataList.Add(newMovement);
                        if (!Goods.ContainsKey(newMovement.Good_Key))
                        {
                            Goods.Add(newMovement.Good_Key, newMovement.Good);
                        }
                        GlobalViewModel.Instance.HispaniaViewModel.CreateItemInDataManaged(DataManagementId, newMovement);
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(
                           string.Format("Error, a l'afegir una de les linies de comanda seleccionades des dels històrics de client.\r\nDetalls:{0}", 
                                         MsgManager.ExcepMsg(ex)));
                    }
                }
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Refresh();
                ActualizeAmountInfo(EditedCustomerOrder);
                AreDataChanged = AreNotEquals(DataList, SourceDataList);
            //  Undefine the close event manager.
                HistoCustomersWindow.Closed -= HistoCustomersWindow_Closed;
                HistoCustomersWindow = null;
        }

        #endregion

        #region According (Update Data)

        /// <summary>
        /// Set According for the Customer Order Movement selected at true.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnAccordingMovement_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItem != null)
            {
                try
                {
                    ManageAccordingChanged(true);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'actualizar el valor de 'Conformitat' de la línia.\r\nDetalls:{0}", MsgManager.ExcepMsg(ex)));
                }
            }
        }

        /// <summary>
        /// Set According for the Customer Order Movement selected at false.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnUnAccordingMovement_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItem != null)
            {
                try
                {
                    ManageAccordingChanged(false);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'actualizar el valor de 'Conformitat' de la línia.\r\nDetalls:{0}", MsgManager.ExcepMsg(ex)));
                }
            }
        }

        #endregion

        #region Up/Down Movement

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            int Index = ListItems.SelectedIndex;
            CheckRowOrder();
            if (Index > 0)
            {
                DataChangedManagerActive = false;
                CustomerOrderMovementsView currentMovement = (CustomerOrderMovementsView)ListItems.Items[Index];
                CustomerOrderMovementsView previousMovement = (CustomerOrderMovementsView)ListItems.Items[Index - 1];
                var ord = currentMovement.RowOrder;
                currentMovement.RowOrder = previousMovement.RowOrder;
                previousMovement.RowOrder = ord;
                DataList = new ObservableCollection<CustomerOrderMovementsView>(DataList.OrderBy(x => x.RowOrder));
                if (currentMovement.CustomerOrderMovement_Id<=0)
                {
                    GlobalViewModel.Instance.HispaniaViewModel.CreateCustomerOrderMovement(currentMovement);
                }
                else
                {
                    GlobalViewModel.Instance.HispaniaViewModel.UpdateCustomerOrderMovement(currentMovement);
                }

                if (previousMovement.CustomerOrderMovement_Id <= 0)
                {
                    GlobalViewModel.Instance.HispaniaViewModel.CreateCustomerOrderMovement(previousMovement);
                }
                else
                {
                    GlobalViewModel.Instance.HispaniaViewModel.UpdateCustomerOrderMovement(previousMovement);
                }               
            }
        }

      

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            int Index = ListItems.SelectedIndex;
            CheckRowOrder();
            if (Index < ListItems.Items.Count - 1)
            {
                DataChangedManagerActive = false;
                CustomerOrderMovementsView currentMovement = (CustomerOrderMovementsView)ListItems.Items[Index];
                CustomerOrderMovementsView nextMovement = (CustomerOrderMovementsView)ListItems.Items[Index + 1];
                var ord = currentMovement.RowOrder;
                currentMovement.RowOrder = nextMovement.RowOrder;
                nextMovement.RowOrder = ord;
                DataList = new ObservableCollection<CustomerOrderMovementsView>(DataList.OrderBy(x => x.RowOrder));

                if (currentMovement.CustomerOrderMovement_Id <= 0)
                {
                    GlobalViewModel.Instance.HispaniaViewModel.CreateCustomerOrderMovement(currentMovement);
                }
                else
                {
                    GlobalViewModel.Instance.HispaniaViewModel.UpdateCustomerOrderMovement(currentMovement);
                }

                if (nextMovement.CustomerOrderMovement_Id <= 0)
                {
                    GlobalViewModel.Instance.HispaniaViewModel.CreateCustomerOrderMovement(nextMovement);
                }
                else
                {
                    GlobalViewModel.Instance.HispaniaViewModel.UpdateCustomerOrderMovement(nextMovement);
                }
            }
        }
        private void CheckRowOrder()
        {
            if (!ComprobarRowOrder())
            {
                AsignarRowOrder();
            }
        }

        private bool ComprobarRowOrder()
        {
            if (ListItems.Items.Count > 1)
            {
                var listaOrden = new List<int>();
                foreach(CustomerOrderMovementsView item in ListItems.Items)
                {
                    if (listaOrden.Contains(item.RowOrder))
                    {
                        return false;
                    }else
                    {
                        listaOrden.Add(item.RowOrder);
                    }
                }
                return true;                
            }
            else
            {
                return true;
            }
                    
        }

        private void AsignarRowOrder()
        {
            int i = 0;
            foreach (CustomerOrderMovementsView item in ListItems.Items)
            {
                item.RowOrder = i;
                i++;                
            }            
        }

        #endregion

        #endregion

        #endregion

        #region ComboBox

        /// <summary>
        /// Manage the change of the Data in the combobox of Goods.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CbFieldItemToSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                FilterDataListObjects();
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Manage the change of the Data in the combobox of Customers.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CbAddressStores_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                try
                {
                    if (cbAddressStores.SelectedItem is null)
                    {
                        EditedCustomerOrder.Address = string.Empty;
                        EditedCustomerOrder.TimeTable = string.Empty;
                        EditedCustomerOrder.PostalCode = null;
                    }
                    else
                    {
                        AddressStoresView addressStore = (AddressStoresView)cbAddressStores.SelectedValue;
                        EditedCustomerOrder.Address = addressStore.Address;
                        EditedCustomerOrder.TimeTable = addressStore.Timetable;
                        EditedCustomerOrder.PostalCode = addressStore.PostalCode;
                    }
                    ActualizeSendAddresInfo(EditedCustomerOrder);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, al seleccionar l'adreça.\r\nDetalls:{0}", MsgManager.ExcepMsg(ex)));
                }
                AreDataChanged = (EditedCustomerOrder != CustomerOrder);
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Manage the change of the Data in the combobox of SendTypes.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CbSendType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                if (cbSendType.SelectedItem != null)
                {
                    SendTypesView sendTypeSelected = ((SendTypesView)cbSendType.SelectedValue);
                    EditedCustomerOrder.SendType = sendTypeSelected;
                    AreDataChanged = (EditedCustomerOrder != CustomerOrder);
                }
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Manage the change of the Data in the combobox of EffectTypes.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CbEffectType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                if (cbEffectType.SelectedItem != null)
                {
                    try
                    {
                        EffectTypesView effectTypeSelected = ((EffectTypesView)cbEffectType.SelectedValue);
                        ActualizeEditedCustomerOrder(EditedCustomerOrder.Customer, effectTypeSelected);
                        EditedCustomerOrder.EffectType = effectTypeSelected;
                        AreDataChanged = (EditedCustomerOrder != CustomerOrder);
                        tbDataBank_Bank.Text = tbBilling_DataBank_Bank.Text = EditedCustomerOrder.DataBank_Bank;
                        tbDataBank_BankAddress.Text = tbBilling_DataBank_BankAddress.Text = EditedCustomerOrder.DataBank_BankAddress;
                        tbDataBankIBANCountryCode.Text = EditedCustomerOrder.DataBank_IBAN_CountryCode;
                        tbDataBankIBANBankCode.Text = EditedCustomerOrder.DataBank_IBAN_BankCode;
                        tbDataBankIBANOfficeCode.Text = EditedCustomerOrder.DataBank_IBAN_OfficeCode;
                        tbDataBankIBANCheckDigits.Text = EditedCustomerOrder.DataBank_IBAN_CheckDigits;
                        tbDataBankIBANAccountNumber.Text = EditedCustomerOrder.DataBank_IBAN_AccountNumber;
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(string.Format("Error, a l'intentar canviar el tipus d'Efecte.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                    }
                }
                DataChangedManagerActive = true;
            }
        }

        private void CbBillingDataAgent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                if (cbBillingDataAgent.SelectedItem != null)
                {
                    EditedCustomerOrder.BillingData_Agent = ((AgentsView)cbBillingDataAgent.SelectedValue);
                    AreDataChanged = (EditedCustomerOrder != CustomerOrder);
                }
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region CheckBox

        private void ChkbAccording_Unchecked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                EditedCustomerOrder.According = false;
                AreDataChanged = (EditedCustomerOrder != CustomerOrder);
            }
            lblPrevisioLliurament.Visibility = Visibility.Visible;
            chkPrevisioLliurament.Visibility = Visibility.Visible;
        }

        private void ChkbAccording_Checked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                EditedCustomerOrder.According = true;
                AreDataChanged = (EditedCustomerOrder != CustomerOrder);
            }
            lblPrevisioLliurament.Visibility = Visibility.Hidden;
            chkPrevisioLliurament.Visibility = Visibility.Hidden;
        }

        private void ChkbValued_Unchecked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                EditedCustomerOrder.Valued = false;
                AreDataChanged = (EditedCustomerOrder != CustomerOrder);
            }
        }

        private void ChkbValued_Checked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                EditedCustomerOrder.Valued = true;
                AreDataChanged = (EditedCustomerOrder != CustomerOrder);
            }
        }

        private void ChkPrevisioLliurament_Checked(object sender, RoutedEventArgs e)
        {
            this.dtpPrevisioLliurament.Visibility = Visibility.Visible;
            EditedCustomerOrder.PrevisioLliurament = true;
        }

        private void ChkPrevisioLliurament_Unchecked(object sender, RoutedEventArgs e)
        {
            this.dtpPrevisioLliurament.Visibility = Visibility.Hidden;
            EditedCustomerOrder.PrevisioLliurament = false;
        }

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
                ActualizeAvalilableUnitInfo();
                ActualizeButtonBar();
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region DatePicket

        private void DtpDeliveryNoteDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                EditedCustomerOrder.DeliveryNote_Date = (DateTime)dtpDeliveryNoteDate.SelectedDate;
                tbDeliveryNoteDate.Text = GlobalViewModel.GetStringFromDateTimeValue(EditedCustomerOrder.DeliveryNote_Date);
                AreDataChanged = (EditedCustomerOrder != CustomerOrder);
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region ContextMenuItem

        /// <summary>
        /// Métod that calculate the IBAN Country Code of the CCC introduced by the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CtxmnuItemCalculateIBAN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string CCC = string.Format("{0}{1}{2}{3}",
                                            tbDataBankIBANBankCode.Text, tbDataBankIBANOfficeCode.Text,
                                            tbDataBankIBANCheckDigits.Text, tbDataBankIBANAccountNumber.Text);
                string IBAN = GlobalViewModel.Instance.HispaniaViewModel.CalculateSpanishIBAN(CCC);
                MsgManager.ShowMessage("Còdi de pais de l'IBAN calculat correctament.", MsgType.Information);
                tbDataBankIBANCountryCode.Text = IBAN.Substring(0, 4);
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
            }
        }

        /// <summary>
        /// Métod that validate the IBAN introduced by the user.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CtxmnuItemValidateIBAN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string IBAN = string.Format("{0}{1}{2}{3}{4}",
                                            tbDataBankIBANCountryCode.Text, tbDataBankIBANBankCode.Text,
                                            tbDataBankIBANOfficeCode.Text, tbDataBankIBANCheckDigits.Text,
                                            tbDataBankIBANAccountNumber.Text);
                GlobalViewModel.Instance.HispaniaViewModel.ValidateSpanishIBAN(IBAN);
                MsgManager.ShowMessage("Còdi de pais de l'IBAN correcte.", MsgType.Information);
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
            }
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Show Good Remarks at Entry Window or Good selection changed.
        /// </summary>
        public void ShowGoodRemark()
        {
            if ((EditedCustomerOrder != null) && (EditedCustomerOrder.Customer != null))
            {
                MsgManager.ShowMessage(EditedCustomerOrder.Customer.Several_Remarks, MsgType.Information);
                tbItemToSearch.Focus();
            }
        }

        /// <summary>
        /// Restore all values.
        /// </summary>
        public void RestoreSourceValues()
        {
            EditedCustomerOrder.RestoreSourceValues(CustomerOrder);
            LoadDataInControls(EditedCustomerOrder, false);
            AreDataChanged = (EditedCustomerOrder != CustomerOrder);
        }

        /// <summary>
        /// Restore the value of the indicated field.
        /// </summary>
        /// <param name="ErrorField">Field to restore value.</param>
        public void RestoreSourceValue(CustomerOrdersAttributes ErrorField)
        {
            EditedCustomerOrder.RestoreSourceValue(CustomerOrder, ErrorField);
            LoadDataInControls(EditedCustomerOrder, false);
            AreDataChanged = (EditedCustomerOrder != CustomerOrder);
        }

        #endregion

        #region Update IU Methods

        private void FilterDataListObjects()
        {
            CollectionViewSource.GetDefaultView(cbCustomer.ItemsSource).Refresh();
            if (cbCustomer.Items.Count > 0) cbCustomer.SelectedIndex = 0;
        }

        private void ManageAccordingChanged(bool NewAccordingValue)
        {
            CustomerOrderMovementsView currentMovement = (CustomerOrderMovementsView)ListItems.SelectedItem;
            CustomerOrderMovementsView newMovement = new CustomerOrderMovementsView(currentMovement)
            {
                According = NewAccordingValue
            };
            GlobalViewModel.Instance.HispaniaViewModel.UpdateItemInDataManaged(DataManagementId, currentMovement, newMovement);
            int Index = ListItems.SelectedIndex;
            DataChangedManagerActive = false;
            ListItems.SelectedItem = null;
            DataChangedManagerActive = true;
            DataList.Remove(currentMovement);
            DataList.Insert(Index, newMovement);
            ActualizeGoodInfo(currentMovement, newMovement);
            ListItems.SelectedItem = newMovement;
            ActualizeAmountInfo(EditedCustomerOrder);
            AreDataChanged = AreNotEquals(DataList, SourceDataList);
        }

        private void ActualizeGoodsData()
        {
            try
            {
                GlobalViewModel.Instance.HispaniaViewModel.RefreshGoods();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(
                        string.Format("Error, a l'actualitzar la informació dels artícles.\r\nDetalls:{0}",
                                      MsgManager.ExcepMsg(ex)));
            }
        }

        private void ActualizeCustomerOrderAgentsData()
        {
            try
            {
                GlobalViewModel.Instance.HispaniaViewModel.RefreshAgents();
                this.Agents = GlobalViewModel.Instance.HispaniaViewModel.AgentsDict;
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(
                        string.Format("Error, a l'actualitzar la informació dels representants.\r\nDetalls:{0}",
                                      MsgManager.ExcepMsg(ex)));
            }
        }

        private void ActualizeAvalilableUnitInfo()
        {
            if (ListItems.SelectedItem != null)
            {
                CustomerOrderMovementsView movement = (CustomerOrderMovementsView)ListItems.SelectedItem;
                tbShippingUnitAvailable.Text = Goods[movement.Good_Key].Shipping_Unit_Available_Str;
                tbBillingUnitAvailable.Text = Goods[movement.Good_Key].Billing_Unit_Available_Str;
            }
            else
            {
                tbShippingUnitAvailable.Text = "Línia no seleccionda";
                tbBillingUnitAvailable.Text = "Línia no seleccionda";
            }
        }

        private void ActualizeButtonBar()
        {
            if ((ListItems.SelectedItem is null) || (EditedCustomerOrder.Customer is null))
            {
                btnEdit.Visibility = btnDelete.Visibility = btnViewData.Visibility = Visibility.Hidden;
                btnAccordingMovement.Visibility = btnUnAccordingMovement.Visibility = Visibility.Hidden;
                btnUp.Visibility = btnDown.Visibility = Visibility.Hidden;
            }
            else
            {
                Visibility VisualStyle = (m_CtrlOperation != Operation.Show) ? Visibility.Visible : Visibility.Hidden;
                btnEdit.Visibility = btnDelete.Visibility = VisualStyle;
                btnUp.Visibility = btnDown.Visibility = VisualStyle;
                btnAccordingMovement.Visibility = btnUnAccordingMovement.Visibility = VisualStyle;
                if (((CustomerOrderMovementsView)ListItems.SelectedItem).According)
                {
                    btnAccordingMovement.IsEnabled = false;
                    btnUnAccordingMovement.IsEnabled = true;
                }
                else
                {
                    btnAccordingMovement.IsEnabled = true;
                    btnUnAccordingMovement.IsEnabled = false;
                }
                btnViewData.Visibility = Visibility.Visible;
            }
            if ((m_CtrlOperation == Operation.Show) || (EditedCustomerOrder.Customer is null))
            {
                btnAdd.Visibility = btnHistoric.Visibility = Visibility.Hidden;
            }
            else
            {
                btnAdd.Visibility = Visibility.Visible;
                btnHistoric.Visibility = EditedCustomerOrder.Customer != null ? Visibility.Visible : Visibility.Hidden;
            }
            btnAddGood.Visibility = m_CtrlOperation != Operation.Show ? Visibility.Visible : Visibility.Hidden;
            if ((m_CtrlOperation == Operation.Show) || 
                ((m_CtrlOperation != Operation.Show) && (!EditedCustomerOrder.HasDeliveryNote)))
            {
                dtpDeliveryNoteDate.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 155, 33, 29));
            }
            else
            {
                dtpDeliveryNoteDate.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 8, 70, 124));
            }
        }

        private void ActualizeGoodInfo(CustomerOrderMovementsView currentMovement,
                                       CustomerOrderMovementsView newMovement)
        {
            //  Actualitza l'estructura de dades que emmagatzema els canvis realitzats
                string currentGoodKey = currentMovement.Good_Key;
                string newGoodKey = newMovement.Good_Key;
                if ((currentMovement.According) && (!newMovement.According))
                {
                    //  Es treu l'actual i no es comptabilitza el nou moviment.
                        ActualizeGoodInfo(currentMovement, MovementOp.Remove, false);
                }
                else if ((!currentMovement.According) && (newMovement.According))
                {
                    //  Si no estaba comptabilitzat l'anterior no s'ha de treure
                        ActualizeGoodInfo(newMovement, MovementOp.Add, true);
                }
                else if ((currentMovement.According) && (newMovement.According)) 
                {
                    //  Si estaba compatibilitzat s'ha de seguir compatbilitzant
                        ActualizeGoodInfo(currentMovement, MovementOp.Remove, false);
                        ActualizeGoodInfo(newMovement, MovementOp.Add, true);
                }
                // else :- Si no es comptabilitzaba i no es comptabilitza no caldrà fer res.
            //  Si el nou article no existeix el creem per futures comptabilitzacions.
                if (!Goods.ContainsKey(newMovement.Good_Key))
                {
                    Goods.Add(newMovement.Good_Key, newMovement.Good);
                }
        }

        private void ActualizeGoodInfo(CustomerOrderMovementsView Movement, MovementOp Action,
                                        bool NotifyStatus = true)
        {
            if (!Goods.ContainsKey(Movement.Good_Key))
            {
                Goods.Add(Movement.Good_Key, Movement.Good);
            }
            ActualizeBillingAndShippingUnits(Movement.Good_CodArt_Str, Movement.Good_Key, Movement.According, 
                                             Movement.Unit_Billing, Movement.Unit_Shipping, Action,
                                             NotifyStatus);
        }

        private void ActualizeBillingAndShippingUnits(string GoodCode, string GoodKey, bool According,
                                                      decimal Billing_Unit, decimal Shipping_Unit, 
                                                      MovementOp Action, bool NotifyStatus = true)
        {
            switch (Action)
            {
                case MovementOp.Add:
                     if (According)
                     {
                        Goods[GoodKey].Billing_Unit_Available -= Billing_Unit;
                        Goods[GoodKey].Shipping_Unit_Available -= Shipping_Unit;
                     }
                     break;
                case MovementOp.Remove:
                     if (According)
                     {
                        Goods[GoodKey].Billing_Unit_Available += Billing_Unit;
                        Goods[GoodKey].Shipping_Unit_Available += Shipping_Unit;
                     }
                     break;
                default:
                     MsgManager.ShowMessage("Error, operación no reconeguda al actualizar la línia de comanda.");
                     return;
            }
            if (!NotifyStatus) return;
            string BaseErrMsg = "Avis, no hi ha suficients Unitats de {0} de l'article '{1}' per servir la comanda.";
            string ErrMsg = null;
            if (Goods[GoodKey].Billing_Unit_Available < 0)
            {
                ErrMsg = string.Format(BaseErrMsg, "Facturació", GoodCode);
            }
            if (Goods[GoodKey].Shipping_Unit_Available < 0)
            {
                ErrMsg = ((ErrMsg is null) ? "" : ErrMsg + "\r\n") + string.Format(BaseErrMsg, "Expedició", GoodCode);
            }
            if (!(ErrMsg is null)) MsgManager.ShowMessage(ErrMsg, MsgType.Warning);
        }

        private void ActualizeAmountInfo(CustomerOrdersView CustomerOrder)
        {
            decimal EarlyPaymentDiscountPrecent = CustomerOrder.BillingData_EarlyPaymentDiscount;
            GlobalViewModel.Instance.HispaniaViewModel.CalculateAmountInfo(DataList,
                                                                           EarlyPaymentDiscountPrecent,
                                                                           CustomerOrder.IVAPercent,
                                                                           CustomerOrder.SurchargePercent,
                                                                           out decimal GrossAmount,
                                                                           out decimal EarlyPayementDiscountAmount,
                                                                           out decimal TaxableBaseAmount,
                                                                           out decimal IVAAmount,
                                                                           out decimal SurchargeAmount,
                                                                           out decimal TotalAmount);
            tbGrossAmount.Text = GlobalViewModel.GetStringFromDecimalValue(GrossAmount, DecimalType.Currency, true);
            tbEarlyPaymentDiscountAmount.Text = GlobalViewModel.GetStringFromDecimalValue(EarlyPayementDiscountAmount, DecimalType.Currency, true);
            tbTaxableBase.Text = GlobalViewModel.GetStringFromDecimalValue(TaxableBaseAmount, DecimalType.Currency, true);
            tbIVAAmount.Text = GlobalViewModel.GetStringFromDecimalValue(IVAAmount, DecimalType.Currency, true);
            tbSurchargeAmount.Text = GlobalViewModel.GetStringFromDecimalValue(SurchargeAmount, DecimalType.Currency, true);
            tbTotalAmount.Text = GlobalViewModel.GetStringFromDecimalValue(TotalAmount, DecimalType.Currency, true);
            CustomerOrder.TotalAmount = TotalAmount;
        }

        private void ActualizeEditedCustomerOrder(CustomersView Customer, EffectTypesView EffectType)
        {
            if (EffectType.EffectType_Id == 6) // TRANSFERÈNCIA (Parameters)
            {
                GlobalViewModel.Instance.HispaniaViewModel.RefreshParameters();
                Parameters = GlobalViewModel.Instance.HispaniaViewModel.Parameters;
                EditedCustomerOrder.DataBank_Bank = Parameters.DataBank_Bank;
                EditedCustomerOrder.DataBank_BankAddress = Parameters.DataBank_BankAddress;
                EditedCustomerOrder.DataBank_IBAN_CountryCode = Parameters.DataBank_IBAN_CountryCode;
                EditedCustomerOrder.DataBank_IBAN_BankCode = Parameters.DataBank_IBAN_BankCode;
                EditedCustomerOrder.DataBank_IBAN_OfficeCode = Parameters.DataBank_IBAN_OfficeCode;
                EditedCustomerOrder.DataBank_IBAN_CheckDigits = Parameters.DataBank_IBAN_CheckDigits;
                EditedCustomerOrder.DataBank_IBAN_AccountNumber = Parameters.DataBank_IBAN_AccountNumber;
            }
            else // ALTRA SISTEMA DE PAGAMENT (Customer)
            {
                if (!(EditedCustomerOrder.Customer is null))
                {
                    EditedCustomerOrder.DataBank_Bank = Customer.DataBank_Bank;
                    EditedCustomerOrder.DataBank_BankAddress = Customer.DataBank_BankAddress;
                    EditedCustomerOrder.DataBank_IBAN_CountryCode = Customer.DataBank_IBAN_CountryCode;
                    EditedCustomerOrder.DataBank_IBAN_BankCode = Customer.DataBank_IBAN_BankCode;
                    EditedCustomerOrder.DataBank_IBAN_OfficeCode = Customer.DataBank_IBAN_OfficeCode;
                    EditedCustomerOrder.DataBank_IBAN_CheckDigits = Customer.DataBank_IBAN_CheckDigits;
                    EditedCustomerOrder.DataBank_IBAN_AccountNumber = Customer.DataBank_IBAN_AccountNumber;
                }
                else
                {
                    EditedCustomerOrder.DataBank_Bank = string.Empty;
                    EditedCustomerOrder.DataBank_BankAddress = string.Empty;
                    EditedCustomerOrder.DataBank_IBAN_CountryCode = string.Empty;
                    EditedCustomerOrder.DataBank_IBAN_BankCode = string.Empty;
                    EditedCustomerOrder.DataBank_IBAN_OfficeCode = string.Empty;
                    EditedCustomerOrder.DataBank_IBAN_CheckDigits = string.Empty;
                    EditedCustomerOrder.DataBank_IBAN_AccountNumber = string.Empty;
                }
            }
        }

        private void ActualizeCustomerAddressData(CustomersView customer = null)
        {
            if (customer is null)
            {
                cbAddressStores.ItemsSource = null;
                cbAddressStores.SelectedIndex = -1;
            }
            else
            {
                string Key;
                Dictionary<string, AddressStoresView> AddressStoresOfCustomer = new Dictionary<string, AddressStoresView>();
                if (customer.Company_PostalCode == null) Key = $"Adreça Principal del Client | {customer.Company_Address}";
                else
                {
                    string PostalCode = customer.Company_PostalCode.Postal_Code_Info;
                    Key = $"Adreça Principal del Client ->  Adreça: '{customer.Company_Address}'  -  Còdi Postal: '{PostalCode}'";
                }
                AddressStoresView Address = new AddressStoresView(customer.Customer_Id)
                {
                    Address = customer.Company_Address,
                    PostalCode = customer.Company_PostalCode,
                    Timetable = customer.Company_TimeTable
                };
                AddressStoresOfCustomer.Add(Key, Address);
                foreach (AddressStoresView addressStore in GlobalViewModel.Instance.HispaniaViewModel.GetAddressStores(customer.Customer_Id))
                {
                    if (addressStore.PostalCode == null) Key = $"{addressStore.AddressStore_Id} | {addressStore.Address}";
                    else
                    {
                        string PostalCode = addressStore.PostalCode.Postal_Code_Info;
                        Key = $"Id: {addressStore.AddressStore_Id}  ->  Adreça: '{addressStore.Address}'  -  Còdi Postal: '{PostalCode}'";
                    }
                    AddressStoresOfCustomer.Add(Key, addressStore);
                }
                cbAddressStores.ItemsSource = AddressStoresOfCustomer;
                cbAddressStores.DisplayMemberPath = "Key";
                cbAddressStores.SelectedValuePath = "Value";
            }
        }

        private void ActualizeSendAddresInfoData(CustomerOrdersView CustomerOrder)
        {
            string AddressValue = (string.IsNullOrEmpty(CustomerOrder.Address)) ? string.Empty : CustomerOrder.Address;
            tbSendAddress.Text = AddressValue;
            tbSendAddress.ToolTip = AddressValue;
            string PostalCodeValue = (CustomerOrder.PostalCode == null) ? string.Empty : CustomerOrder.PostalCode.Postal_Code_Info;
            tbSendPostalCode.Text = PostalCodeValue;
            tbSendPostalCode.ToolTip = PostalCodeValue;
            string TimeTableValue = (string.IsNullOrEmpty(CustomerOrder.TimeTable)) ? string.Empty : CustomerOrder.TimeTable;
            tbSendTimetable.Text = TimeTableValue;
            tbSendTimetable.ToolTip = TimeTableValue;
        }
        private void ActualizeSendAddresInfo(CustomerOrdersView CustomerOrder)
        {
            ActualizeSendAddresInfoData(CustomerOrder);
            foreach (KeyValuePair<string, AddressStoresView> item in cbAddressStores.Items)
            {
                string AddresStore_Address = (item.Value.Address == null) ? item.Value.Address : item.Value.Address.ToLower();
                string customerOrdersView_Address = (CustomerOrder.Address == null) ? CustomerOrder.Address : CustomerOrder.Address.ToLower();
                if (string.Equals(AddresStore_Address, customerOrdersView_Address))
                {
                    cbAddressStores.SelectedItem = item;
                    break;
                }
            }
        }

        private void CheckIfAddingNewArticle()
        {            
            if ((GoodsWindow.SelectedGoodView!=null))
            {
                try
                {
                    CustomerOrderMovementsView newMovement = new CustomerOrderMovementsView();
                    newMovement.Good = GoodsWindow.SelectedGoodView;
                    newMovement.CustomerOrder = this.CustomerOrder;
                    newMovement.Description = GoodsWindow.SelectedGoodView.Good_Description;
                    newMovement.Unit_Billing_Definition = GoodsWindow.SelectedGoodView.Good_Unit.Billing;
                    newMovement.Unit_Shipping_Definition = GoodsWindow.SelectedGoodView.Good_Unit.Shipping;
                    //newMovement.ShippingUnitAvailable = good.Shipping_Unit_Available_Str;
                    //newMovement.BillingUnitAvailable = good.Billing_Unit_Available_Str;
                    GlobalViewModel.Instance.HispaniaViewModel.CreateItemInDataManaged(DataManagementId, newMovement);
                    DataList.Add(newMovement);
                    ActualizeGoodInfo(newMovement, MovementOp.Add);
                    ListItems.SelectedItem = newMovement;
                    //ActualizeAmountInfo(EditedCustomerOrder);
                    AreDataChanged = AreNotEquals(DataList, SourceDataList);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'actualitzar les dades de la nova línia de comanda.\r\nDetalls:{0}", MsgManager.ExcepMsg(ex)));
                }
            }
            
        }

        #endregion

        #region Helper Functions

        private bool AreNotEquals(ObservableCollection<CustomerOrderMovementsView> Data,
                                  ObservableCollection<CustomerOrderMovementsView> SourceData)
        {
            if ((Data is null) && (SourceData is null))
            {
                return false;
            }
            else if ((Data != null) && (SourceData != null))
            {
                if (Data.Count == SourceData.Count)
                {
                    foreach (CustomerOrderMovementsView Movement in Data)
                    {
                        if ((SourceData.Contains(Movement)) && (Movement == SourceData[SourceData.IndexOf(Movement)]))
                        {
                            continue;
                        }
                        return true;
                    }
                    return false;
                }
            }
            return true;
        }

        #endregion

    }
}
