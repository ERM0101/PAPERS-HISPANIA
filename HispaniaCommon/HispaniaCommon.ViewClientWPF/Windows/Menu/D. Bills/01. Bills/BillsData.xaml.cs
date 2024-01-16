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
    public partial class BillsData : UserControl
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
        /// Hide the button bar.
        /// </summary>
        private GridLength HideComponent = new GridLength(0.0);

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
        /// <param name="NewOrEditedGood">New or Edited Bill.</param>
        public delegate void dlgAccept(BillsView NewOrEditedBill);

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
        /// Window instance of CustomerOrderMovementsData.
        /// </summary>
        private CustomerOrderMovementsData CustomerOrderMovementsDataWindow = null;

        /// <summary>
        /// Window instance of HistoCustomers.
        /// </summary>
        private HistoCustomers HistoCustomersWindow = null;

        /// <summary>
        /// Store the data to show in the List of movements.
        /// </summary>
        private ObservableCollection<ReceiptsView> m_ReceiptsDataList = new ObservableCollection<ReceiptsView>();

        /// <summary>
        /// Store the data to show in the List of movements.
        /// </summary>
        private ObservableCollection<CustomerOrdersView> m_SendTypesDataList = new ObservableCollection<CustomerOrdersView>();

        /// <summary>
        /// Store the data to show in the List of movements.
        /// </summary>
        private ObservableCollection<CustomerOrdersView> m_AddressStoresDataList = new ObservableCollection<CustomerOrdersView>();

        /// <summary>
        /// Store the data to show in the List of movements.
        /// </summary>
        private ObservableCollection<CustomerOrderMovementsView> m_DataList = new ObservableCollection<CustomerOrderMovementsView>();

        /// <summary>
        /// Store the Agents
        /// </summary>
        public Dictionary<string, AgentsView> m_Agents;

        /// <summary>
        /// Store the Bill data to manage.
        /// </summary>
        private BillsView m_Bill = null;

        /// <summary>
        /// Store the Parameters View
        /// </summary>
        private ParametersView m_Parameters;

        /// <summary>
        /// Store Bill Customer Orders 
        /// </summary>
        private ObservableCollection<CustomerOrdersView> m_CustomerOrders;

        /// <summary>
        /// Store the EffectTypes
        /// </summary>
        public Dictionary<string, EffectTypesView> m_EffectTypes;

        /// <summary>
        /// Store the BillingSeries
        /// </summary>
        public Dictionary<string, BillingSeriesView> m_BillingSeries;

        /// <summary>
        /// Store the Goods
        /// </summary>
        private Dictionary<string, GoodsView> m_Goods;

        /// <summary>
        /// Context Menu to calculate and validate the IBAN.
        /// </summary>
        private ContextMenu ctxmnuIBAN_Initial;

        /// <summary>
        /// Context Menu to Refresh the IBAN Value .
        /// </summary>
        private ContextMenu ctxmnuRefreshIBAN_Initial;

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
        /// Store Bill Customer Orders 
        /// </summary>
        private ObservableCollection<CustomerOrdersView> CustomerOrders
        {
            get
            {
                return m_CustomerOrders;
            }
            set
            {
                if (value != null)
                {
                    m_CustomerOrders = value;
                    ObservableCollection<CustomerOrderMovementsView> Movements = new ObservableCollection<CustomerOrderMovementsView>();
                    foreach (CustomerOrdersView customerOrder in m_CustomerOrders)
                    {
                        foreach(CustomerOrderMovementsView Movement in GlobalViewModel.Instance.HispaniaViewModel.GetCustomerOrderMovements(customerOrder.CustomerOrder_Id))
                        {
                            Movements.Add(Movement);
                        }
                    }
                    
                    DataList = Movements;
                    DataList = new ObservableCollection<CustomerOrderMovementsView>(DataList.OrderBy(x => x.RowOrder));
                }
                else throw new ArgumentNullException("Error, no s'han trobat els albarans de la Factura a carregar.");
            }
        }

        /// <summary>
        /// Get or Set the Customer Order data to manage.
        /// </summary>
        public BillsView Bill
        {
            get
            {
                return (m_Bill);
            }
            set
            {
                if (value != null)
                {
                    AreDataChanged = false;
                    m_Bill = value;
                    EditedBill = new BillsView(m_Bill);
                    CustomerOrders = m_Bill.CustomerOrders;
                    SendTypesDataList = m_Bill.CustomerOrders;
                    AddressStoresDataList = m_Bill.CustomerOrders;
                    ReceiptsDataList = m_Bill.Receipts;
                    LoadDataInControls(m_Bill, true, 1);
                }
                else throw new ArgumentNullException("Error, no s'han trobat les dades de la Factura a carregar.");
            }
        }

        /// <summary>
        /// Get or Set the Goods 
        /// </summary>
        public Dictionary<string, GoodsView> Goods
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
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).SortDescriptions.Add(new SortDescription("CustomerOrder_Id", ListSortDirection.Ascending));
                ActualizeAvalilableUnitInfo();
                ActualizeAmountInfo();
            }
        }

        private ObservableCollection<ReceiptsView> ReceiptsDataList
        {
            get
            {
                return (m_ReceiptsDataList);
            }
            set
            {
                if (!(value is null))
                {
                    m_ReceiptsDataList = value;
                    decimal Amount = 0;
                    foreach (ReceiptsView receipt in m_ReceiptsDataList)
                    {
                        Amount += GlobalViewModel.GetCurrencyValueFromDecimal(receipt.Amount);
                    }
                    tbCumulateExpiration.Text =  string.Format("{0:0.00€}", Amount);
                }
                else
                {
                    m_ReceiptsDataList = new ObservableCollection<ReceiptsView>();
                    tbCumulateExpiration.Text = "NO HI HA REBUTS";
                }
                ReceiptsListItems.ItemsSource = m_ReceiptsDataList;
                ReceiptsListItems.DataContext = this;
                CollectionViewSource.GetDefaultView(ReceiptsListItems.ItemsSource).SortDescriptions.Add(new SortDescription("Expiration_Date", ListSortDirection.Ascending));
                ReceiptsListItems.SelectedIndex = 0;
            }
        }

        private ObservableCollection<CustomerOrdersView> SendTypesDataList
        {
            get
            {
                return (m_SendTypesDataList);
            }
            set
            {
                if (!(value is null)) m_SendTypesDataList = value;
                else m_SendTypesDataList = new ObservableCollection<CustomerOrdersView>();
                SendTypesListItems.ItemsSource = m_SendTypesDataList;
                SendTypesListItems.DataContext = this;
                CollectionViewSource.GetDefaultView(SendTypesListItems.ItemsSource).SortDescriptions.Add(new SortDescription("DeliveryNote_Date", ListSortDirection.Ascending));
            }
        }

        private ObservableCollection<CustomerOrdersView> AddressStoresDataList
        {
            get
            {
                return (m_AddressStoresDataList);
            }
            set
            {
                if (!(value is null)) m_AddressStoresDataList = value;
                else m_AddressStoresDataList = new ObservableCollection<CustomerOrdersView>();
                AddressStoresListItems.ItemsSource = m_AddressStoresDataList;
                AddressStoresListItems.DataContext = this;
                CollectionViewSource.GetDefaultView(AddressStoresListItems.ItemsSource).SortDescriptions.Add(new SortDescription("DeliveryNote_Date", ListSortDirection.Ascending));
            }
        }

        /// <summary>
        /// Get or Set the data to show in List of Items.
        /// </summary>
        private ObservableCollection<CustomerOrderMovementsView> SourceDataList { get; set; }

        /// <summary>
        /// Get or Set the Edited CustomerOrder information.
        /// </summary>
        private BillsView EditedBill
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
                         if (Bill == null) throw new InvalidOperationException("Error, impossible visualitzar una Factura sense dades.");
                         tbDataBankIBANCountryCode.ContextMenu = null;
                         tbCancel.Text = "Tornar";
                         dtpBillDate.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 155, 33, 29));
                         break;
                    case Operation.Add:
                         throw new InvalidOperationException("Error, no es poden crear factures des d'aquesta finestra.");
                    case Operation.Edit:
                         if (Bill == null) throw new InvalidOperationException("Error, impossible editar una Factura sense dades.");
                         dtpBillDate.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 8, 70, 124));
                         tbDataBankIBANCountryCode.ContextMenu = ctxmnuIBAN_Initial;
                         imgGroupIBAN.ContextMenu = ctxmnuRefreshIBAN_Initial;
                         tbCancel.Text = "Cancel·lar";
                         break;
                }
                string properyValue;
                foreach (Control control in EditableControls)
                {
                    if (control is TextBox) ((TextBox)control).IsReadOnly = (m_CtrlOperation == Operation.Show);
                    else if (control is RichTextBox) ((RichTextBox)control).IsReadOnly = (m_CtrlOperation == Operation.Show);
                    else if (control is ComboBox) ((ComboBox)control).IsEnabled = (m_CtrlOperation != Operation.Show);
                    else if (control is RadioButton) ((RadioButton)control).IsEnabled = (m_CtrlOperation != Operation.Show);
                    else if (control is ListView) ((ListView)control).IsEnabled = true;
                    else if (control is Button)
                    {
                        if (control == btnHistoric) btnHistoric.IsEnabled = (m_CtrlOperation == Operation.Edit);
                        else if (control == btnViewData) btnViewData.IsEnabled = true;
                        else if (control == btnRefreshEffectData) btnRefreshEffectData.IsEnabled = cbEffectType.SelectedIndex != -1;
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
                cdSplitPaid.Width = cdLblPaid.Width = cdPaid.Width = HideComponent;
                cdSplitReturned.Width = cdLblReturned.Width = cdReturned.Width = HideComponent;
                cdSplitExpired.Width = cdLblExpired.Width = cdExpired.Width = HideComponent;
                cdSplitPending.Width = cdLblPending.Width = cdPending.Width = HideComponent;
                dtpBillDate.IsEnabled = (m_CtrlOperation == Operation.Edit) && (Bill.Bill_Id > 0);
                lblBillDate.IsEnabled = (m_CtrlOperation == Operation.Edit) && (Bill.Bill_Id > 0);
                tiHeaderData.IsSelected = true;
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
        /// Get or Set if the manager of the data change for the Good has active.
        /// </summary>
        private bool DataChangedManagerActive
        {
            get;
            set;
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
        /// Get or Set the Billing Series 
        /// </summary>
        public Dictionary<string, BillingSeriesView> BillingSeries
        {
            get
            {
                return (m_BillingSeries);
            }
            set
            {
                m_BillingSeries = value;
                if (m_BillingSeries != null)
                {
                    cbBillingSeries.ItemsSource = m_BillingSeries;
                    cbBillingSeries.DisplayMemberPath = "Key";
                    cbBillingSeries.SelectedValuePath = "Value";
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
        public BillsData()
        {
            //  Initialization of controls of the UserControl
                InitializeComponent();
            //  Initialize GUI.
                ctxmnuIBAN_Initial = ctxmnuIBAN;
                ctxmnuRefreshIBAN_Initial = ctxmnuRefreshIBAN;
                InitEditableControls();
                InitNonEditableControls();
                InitOnlyQueryControls();
            //  Load Data in Window components.
                LoadDataInWindowComponents();
            //  Load the managers of the controls of the UserControl.
                LoadManagers();
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
                lblBillId,
                tbBillId,
                lblBillDate,
                tbBillDate,
                lblCustomerId,
                tbCustomerId,
                lblCustomerAlias,
                tbCustomerAlias,
                lblCompanyName,
                tbCompanyName,
                lblCompanyCif,
                tbCompanyCif,
                lblCompanyNumProv,
                tbCompanyNumProv,
                lblCompanyAddress,
                tbCompanyAddress,
                lblCompanyPostalCode,
                tbCompanyPostalCode,
                tbCumulateExpiration,
                lblBillingDataNumUnpaid,
                tbBillingDataNumUnpaid,
                tbShippingUnitAvailable,
                tbBillingUnitAvailable,
                lblBillingDataEarlyPaymentDiscount,
                tbBillingDataEarlyPaymentDiscount,
                lblIVAPercent,
                tbIVAPercent,
                lblSurchargePercent,
                tbSurchargePercent,
                lblGrossAmount,
                tbGrossAmount,
                lblEarlyPaymentDiscountAmount,
                tbEarlyPaymentDiscountAmount,
                lblTaxableBase,
                tbTaxableBase,
                lblIVAAmount,
                tbIVAAmount,
                lblSurchargeAmount,
                tbSurchargeAmount,
                lblTotalAmount,
                tbTotalAmount,
                lblCustomerRemarks,
                tbCustomerRemarks,
                lblNumEffect,
                tbNumEffect,
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
                btnRefreshEffectData
            };
        }

        /// <summary>
        /// Initialize the list of Editable Controls.
        /// </summary>
        private void InitEditableControls()
        {
            EditableControls = new List<Control>
            {
                lblBillingSeries,
                cbBillingSeries,
                dtpBillDate,
                gbReceiptsItemsList,
                ReceiptsListItems,
                gbSendByTypesItemsList,
                SendTypesListItems,
                gbItemsList,
                ListItems,
                gbAddressStoresItemsList,
                AddressStoresListItems,
                lblBillRemarks,
                tbBillRemarks,
                lblDataBank_Bank,
                tbDataBank_Bank,
                lblDataBank_BankAddress,
                tbDataBank_BankAddress,
                lblBilling_DataBank_Bank,
                tbBilling_DataBank_Bank,
                lblBilling_DataBank_BankAddress,
                tbBilling_DataBank_BankAddress,
                lblEffectType,
                cbEffectType,
                gbIBAN,
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
                btnRefreshEffectData,
                lblBillingDataAgent,
                cbBillingDataAgent,
                lblPaid,
                rbPaid,
                lblReturned,
                rbReturned,
                lblExpired,
                rbExpired,
                lblPending,
                rbPending,
                tiHeaderData,
                tiLines,
                tiFootData,
                tiBillingData,
                tiDivers
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
        /// Method that load data in Window components.
        /// </summary>
        private void LoadDataInWindowComponents()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Set valid dates for change DeliveryNote Date
                dtpBillDate.DisplayDateStart = new DateTime(DateTime.Now.Year, 1, 1);
                dtpBillDate.DisplayDateEnd = new DateTime(DateTime.Now.Year, 12, 31);
            //  Deactivate managers
                DataChangedManagerActive = true;
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
        /// Method that loads the Data in the controls of the Window
        /// </summary>
        /// <param name="billsView">Data Container.</param>
        /// <param name="ThrowException">1, if want throw an exception if not found a component</param>
        private void LoadDataInControls(BillsView billsView, bool Actualize = true, int ThrowException = 0)
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Bill Controls
                tbBillId.Text = GlobalViewModel.GetStringFromIntIdValue(billsView.Bill_Id);
                tbBillDate.Text = billsView.Bill_Date_Str;
                tbCustomerId.Text = GlobalViewModel.GetStringFromIntIdValue(billsView.Customer.Customer_Id);
                tbCustomerAlias.Text = billsView.Customer_Alias;
                tbCompanyName.Text = billsView.Company_Name;
                tbCompanyCif.Text = billsView.Company_Cif;
                tbCompanyNumProv.Text = billsView.Company_NumProv;
                tbCompanyAddress.Text = billsView.Company_Address;
                tbCompanyPostalCode.Text = billsView.Company_PostalCode_Str;
                tbBillingDataEarlyPaymentDiscount.Text = GlobalViewModel.GetStringFromDecimalValue(billsView.BillingData_EarlyPaymentDiscount, DecimalType.Percent, true);
                tbIVAPercent.Text = GlobalViewModel.GetStringFromDecimalValue(billsView.IVAPercent, DecimalType.Percent, true);
                tbSurchargePercent.Text = GlobalViewModel.GetStringFromDecimalValue(billsView.SurchargePercent, DecimalType.Percent, true);
                tbBillRemarks.Text = billsView.Remarks;
                tbDataBank_Bank.Text = billsView.DataBank_Bank;
                tbDataBank_BankAddress.Text = billsView.DataBank_BankAddress;
                tbBilling_DataBank_Bank.Text = billsView.DataBank_Bank;
                tbBilling_DataBank_BankAddress.Text = billsView.DataBank_BankAddress;
                tbNumEffect.Text = GlobalViewModel.GetStringFromDecimalValue(billsView.DataBank_NumEffect, DecimalType.WithoutDecimals);
                tbDataBankFirstExpirationData.Text = GlobalViewModel.GetStringFromDecimalValue(billsView.DataBank_FirstExpirationData, DecimalType.WithoutDecimals);
                tbDataBankExpirationInterval.Text = GlobalViewModel.GetStringFromDecimalValue(billsView.DataBank_ExpirationInterval, DecimalType.WithoutDecimals);
                tbDataBankPayday_1.Text = GlobalViewModel.GetStringFromDecimalValue(billsView.DataBank_Payday_1, DecimalType.WithoutDecimals);
                tbDataBankPayday_2.Text = GlobalViewModel.GetStringFromDecimalValue(billsView.DataBank_Payday_2, DecimalType.WithoutDecimals);
                tbDataBankPayday_3.Text = GlobalViewModel.GetStringFromDecimalValue(billsView.DataBank_Payday_3, DecimalType.WithoutDecimals);
                tbDataBankIBANCountryCode.Text = billsView.DataBank_IBAN_CountryCode;
                tbDataBankIBANBankCode.Text = billsView.DataBank_IBAN_BankCode;
                tbDataBankIBANOfficeCode.Text = billsView.DataBank_IBAN_OfficeCode;
                tbDataBankIBANCheckDigits.Text = billsView.DataBank_IBAN_CheckDigits;
                tbDataBankIBANAccountNumber.Text = billsView.DataBank_IBAN_AccountNumber;
                tbBillingDataNumUnpaid.Text = GlobalViewModel.GetStringFromDecimalValue(billsView.BillingData_NumUnpaid, DecimalType.WithoutDecimals);
                tbCustomerRemarks.Text = billsView.Customer_Remarks;
                LoadExternalTablesInfo(billsView, ThrowException);
            //  Activate managers
                DataChangedManagerActive = true;
        }

        /// <summary>
        /// Load Data from External Tables.
        /// </summary>
        /// <param name="billsView">Data Container.</param>
        /// <param name="ThrowException">1, if want throw an exception if not found a component</param>
        private void LoadExternalTablesInfo(BillsView billsView, int ThrowException = 0)
        {
            if ((EffectTypes != null) && (billsView.DataBank_EffectType != null))
            {
                Dictionary<string, EffectTypesView> Items = (Dictionary<string, EffectTypesView>)cbEffectType.ItemsSource;
                string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeyEffectTypeView(billsView.DataBank_EffectType);
                if (Items.ContainsKey(Key))
                {
                    cbEffectType.SelectedValue = EffectTypes[Key];
                }
                else
                {
                    if (ThrowException == 1)
                    {
                        throw new Exception(string.Format("No s'ha trobat el Tipus d'Efecte '{0}-{1}'.", EffectTypes[Key].Code, EffectTypes[Key].Description));
                    }
                }
            }
            else cbEffectType.SelectedIndex = -1;
            if ((BillingSeries != null) && (billsView.BillingSerie != null))
            {
                Dictionary<string, BillingSeriesView> Items = (Dictionary<string, BillingSeriesView>)cbBillingSeries.ItemsSource;
                string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeyBillingSerieView(billsView.BillingSerie);
                if (Items.ContainsKey(Key)) cbBillingSeries.SelectedValue = BillingSeries[Key];
                else
                {
                    if (ThrowException == 1)
                    {
                        throw new Exception(string.Format("No s'ha trobat la Sèrie de Facturació '{0}-{1}'.", BillingSeries[Key].Name, BillingSeries[Key].Alias));
                    }
                }
            }
            else cbBillingSeries.SelectedIndex = -1;
            if ((Agents != null) && (billsView.BillingData_Agent != null))
            {
                Dictionary<string, AgentsView> Items = (Dictionary<string, AgentsView>)cbBillingDataAgent.ItemsSource;
                string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeyAgentView(billsView.BillingData_Agent);
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

        #endregion

        #region Managers

        /// <summary>
        /// Method that define the managers needed for the user operations in the UserControl
        /// </summary>
        private void LoadManagers()
        {
            //  By default the manager for the Customer Order Data changes is active.
                DataChangedManagerActive = true;
            //  Define ListView events to manage.
                ListItems.SelectionChanged += ListItems_SelectionChanged;
                ReceiptsListItems.SelectionChanged += ReceiptsListItems_SelectionChanged;
            //  TextBox
                tbBilling_DataBank_Bank.PreviewTextInput += TBPreviewTextInput;
                tbBilling_DataBank_Bank.TextChanged += TBDataChanged;
                tbBilling_DataBank_BankAddress.PreviewTextInput += TBPreviewTextInput;
                tbBilling_DataBank_BankAddress.TextChanged += TBDataChanged;
                tbBillRemarks.PreviewTextInput += TBPreviewTextInput;
                tbBillRemarks.TextChanged += TBDataChanged;
                tbDataBank_Bank.PreviewTextInput += TBPreviewTextInput;
                tbDataBank_Bank.TextChanged += TBDataChanged;
                tbDataBank_BankAddress.PreviewTextInput += TBPreviewTextInput;
                tbDataBank_BankAddress.TextChanged += TBDataChanged;
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
            //  DatePiker
                dtpBillDate.SelectedDateChanged += DtpBillDate_SelectedDateChanged;
            //  ContextMenuItem
                ctxmnuItemCalculateIBAN.Click += CtxmnuItemCalculateIBAN_Click;
                ctxmnuItemValidateIBAN.Click += CtxmnuItemValidateIBAN_Click;
                ctxmnuItemRefreshIBAN.Click += CtxmnuItemRefreshIBAN_Click;
            //  ComboBox
                cbEffectType.SelectionChanged += CbEffectType_SelectionChanged;
                cbBillingSeries.SelectionChanged += CbBillingSeries_SelectionChanged;
                cbBillingDataAgent.SelectionChanged += CbBillingDataAgent_SelectionChanged;
            //  RadioButtons
                rbPaid.Checked += RbPaid_Checked;
                rbReturned.Checked += RbReturned_Checked;
                rbExpired.Checked += RbExpired_Checked;
                rbPending.Checked += RbPending_Checked;
            //  Buttons
                btnAccept.Click += BtnAccept_Click;
                btnCancel.Click += BtnCancel_Click;
                btnHistoric.Click += BtnHistoric_Click;
                btnViewData.Click += BtnViewData_Click;
                btnRefreshEffectData.Click += BtnRefreshEffectData_Click;
                btnChangeDataVenciment.Click += btnChangeDataVenciment_Click;
                btnSaveDataVenciment.Click += btnSaveDataVenciment_Click;
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
            if (sender == tbBillRemarks)
            {
                e.Handled = ! GlobalViewModel.IsValidCommentChar(e.Text);
            }
            else if ((sender == tbDataBank_Bank) || (sender == tbBilling_DataBank_Bank))
            {
                e.Handled = ! GlobalViewModel.IsValidNameChar(e.Text);
            }
            else if ((sender == tbDataBank_BankAddress) || (sender == tbBilling_DataBank_BankAddress))
            {
                e.Handled = ! GlobalViewModel.IsValidAddressChar(e.Text);
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
                    if (sender == tbBillRemarks) EditedBill.Remarks = value;
                    else if ((sender == tbDataBank_Bank) || (sender == tbBilling_DataBank_Bank))
                    {
                        if (sender == tbDataBank_Bank) tbBilling_DataBank_Bank.Text = value;
                        else tbDataBank_Bank.Text = value;
                        EditedBill.DataBank_Bank = value;
                    }
                    else if ((sender == tbDataBank_BankAddress) || (sender == tbBilling_DataBank_BankAddress))
                    {
                        if (sender == tbDataBank_BankAddress) tbBilling_DataBank_BankAddress.Text = value;
                        else tbDataBank_BankAddress.Text = value;
                        EditedBill.DataBank_BankAddress = value;
                    }
                    else if (sender == tbDataBankIBANCountryCode) EditedBill.DataBank_IBAN_CountryCode = value;
                    else if (sender == tbDataBankIBANBankCode) EditedBill.DataBank_IBAN_BankCode = value;
                    else if (sender == tbDataBankIBANOfficeCode) EditedBill.DataBank_IBAN_OfficeCode = value;
                    else if (sender == tbDataBankIBANCheckDigits) EditedBill.DataBank_IBAN_CheckDigits = value;
                    else if (sender == tbDataBankIBANAccountNumber) EditedBill.DataBank_IBAN_AccountNumber = value;
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(Bill, false);
                }
                AreDataChanged = (EditedBill != Bill);
                DataChangedManagerActive = true;
            }
        }
        
        #endregion

        #region Buttons

        #region Accept

        /// <summary>
        /// Accept the edition or creatin of the Good.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {
            BillsAttributes ErrorField = BillsAttributes.None;
            try
            {
                if ((CtrlOperation == Operation.Add) || (CtrlOperation == Operation.Edit))
                {
                    EditedBill.Validate(out ErrorField);
                    EvAccept?.Invoke(new BillsView(EditedBill));
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

        #region View 
        
        private void BtnViewData_Click(object sender, RoutedEventArgs e)
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
                           string.Format("Error, al presentar la informació de la línia '{0}'.\r\nDetalls: {1}",
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

        #region Historic

        private void BtnHistoric_Click(object sender, RoutedEventArgs e)
        {
            if (HistoCustomersWindow == null)
            {
                try
                {
                    HistoCustomersWindow = new HistoCustomers(AppType, HistoCustomersMode.Historic)
                    {
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.GetHistoCustomers(EditedBill.Customer.Customer_Id),
                        Data = EditedBill.Customer
                    };
                    HistoCustomersWindow.Closed += HistoCustomersWindow_Closed;
                    HistoCustomersWindow.Show();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'obrir la finestra d'històric de moviments del Client '{0}-{1}'.\r\nDetalls: {2}",
                                     EditedBill.Customer.Customer_Id, EditedBill.Customer.Customer_Alias, 
                                     MsgManager.ExcepMsg(ex)));
                }
            }
            else HistoCustomersWindow.Activate();
        }

        private void HistoCustomersWindow_Closed(object sender, EventArgs e)
        {
            HistoCustomersWindow.Closed -= HistoCustomersWindow_Closed;
            HistoCustomersWindow = null;
        }

        #endregion

        #region Refresh Effect Data

        /// <summary>
        /// Manage the refresh of the information associated at the Effect Type selected.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnRefreshEffectData_Click(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                if (cbEffectType.SelectedItem != null)
                {
                    try
                    {
                        DataChangedManagerActive = false;
                        ActualizeEditedBill(EditedBill.DataBank_EffectType);
                        AreDataChanged = (EditedBill != Bill);
                        DataChangedManagerActive = true;
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(
                           string.Format("Error, a l'actualitzar l'IBAN i les dades Bancaries en funció del Tipus d'Efecte seleccionat.\r\nDetalls: {0}",
                                         MsgManager.ExcepMsg(ex)));
                    }
                }
            }
        }

        #endregion

        #endregion

        #region Radio Buttons

        private void RbPaid_Checked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                AreDataChanged = (EditedBill != Bill) || (!ActualizeReceiptStateInUI(ReceiptState.Paid));
                DataChangedManagerActive = true;
            }
        }

        private void RbReturned_Checked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                AreDataChanged = (EditedBill != Bill) || (!ActualizeReceiptStateInUI(ReceiptState.Returned));
                DataChangedManagerActive = true;
            }
        }

        private void RbExpired_Checked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                AreDataChanged = (EditedBill != Bill) || (!ActualizeReceiptStateInUI(ReceiptState.Expired));
                DataChangedManagerActive = true;
            }
        }

        private void RbPending_Checked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                AreDataChanged = (EditedBill != Bill) || (!ActualizeReceiptStateInUI(ReceiptState.Pending));
                DataChangedManagerActive = true;
            }
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
                try
                {
                    ActualizeAvalilableUnitInfo();
                    ActualizeButtonBar();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, al gestionar la selecció d'una línia.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
                DataChangedManagerActive = true;
            }
        }

        private void ReceiptsListItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                try
                {
                    ActualizeReceiptStateInReceiptsList();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, al gestionar la selecció d'un rebut.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
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
                MsgManager.ShowMessage(
                   string.Format("Error, al calcular l'IBAN.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
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
                MsgManager.ShowMessage(
                   string.Format("Error, al validar l'IBAN.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
            }
        }

        /// <summary>
        /// Métod that refrsh the IBAN value iun function of EffectType selected..
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CtxmnuItemRefreshIBAN_Click(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                if (cbEffectType.SelectedItem != null)
                {
                    try
                    {
                        DataChangedManagerActive = false;
                        ActualizeEditedBill(EditedBill.DataBank_EffectType);
                        AreDataChanged = (EditedBill != Bill);
                        DataChangedManagerActive = true;
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(
                           string.Format("Error, a l'actualitzar l'IBAN i les dades Bancaries en funció del Tipus d'Efecte seleccionat.\r\nDetalls: {0}", 
                                         MsgManager.ExcepMsg(ex)));
                    }
                }
            }
        }

        #endregion

        #region DatePicket

        private void DtpBillDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                EditedBill.Date = (DateTime)dtpBillDate.SelectedDate;
                tbBillDate.Text = GlobalViewModel.GetStringFromDateTimeValue(EditedBill.Date);
                AreDataChanged = (EditedBill != Bill);
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region ComboBox

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
                        DataChangedManagerActive = false;
                        EffectTypesView effectTypeSelected = ((EffectTypesView)cbEffectType.SelectedValue);
                        EditedBill.DataBank_EffectType = effectTypeSelected;
                        ActualizeEditedBill(EditedBill.DataBank_EffectType);
                        AreDataChanged = (EditedBill != Bill);
                        DataChangedManagerActive = true;
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(
                           string.Format("Error, a l'actualitzar el Tipus d'Efecte i les dades associades al Tipus d'Efecte seleccionat.\r\nDetalls: {0}", 
                                         MsgManager.ExcepMsg(ex)));
                    }
                }
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Manage the change of the Data in the combobox of Billing Series.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CbBillingSeries_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                if (cbBillingSeries.SelectedItem != null)
                {
                    try
                    {
                        DataChangedManagerActive = false;
                        BillingSeriesView billingSerieSelected = ((BillingSeriesView)cbBillingSeries.SelectedValue);
                        EditedBill.BillingSerie = billingSerieSelected;
                        AreDataChanged = (EditedBill != Bill);
                        DataChangedManagerActive = true;
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(
                           string.Format("Error, a l'actualitzar el Tipus d'Efecte i les dades associades al Tipus d'Efecte seleccionat.\r\nDetalls: {0}",
                                         MsgManager.ExcepMsg(ex)));
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
                    EditedBill.BillingData_Agent = ((AgentsView)cbBillingDataAgent.SelectedValue);
                    AreDataChanged = (EditedBill != Bill);
                }
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Restore all values.
        /// </summary>
        public void RestoreSourceValues()
        {
            try
            {
                EditedBill.RestoreSourceValues(Bill);
                LoadDataInControls(EditedBill, false);
                AreDataChanged = (EditedBill != Bill);
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(
                    string.Format("Error, al restaurar els valors originals de la factura.\r\nDetalls: {0}",
                                   MsgManager.ExcepMsg(ex)));
            }
        }

        /// <summary>
        /// Restore the value of the indicated field.
        /// </summary>
        /// <param name="ErrorField">Field to restore value.</param>
        public void RestoreSourceValue(BillsAttributes ErrorField)
        {
            try
            {
                EditedBill.RestoreSourceValue(Bill, ErrorField);
                LoadDataInControls(EditedBill, false);
                AreDataChanged = (EditedBill != Bill);
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(
                    string.Format("Error, al restaurar els valors originals de la factura.\r\nDetalls: {0}",
                                   MsgManager.ExcepMsg(ex)));
            }
        }

        #endregion

        #region Update IU Methods

        private void ActualizeAmountInfo()
        {
            decimal EarlyPaymentDiscountPrecent = EditedBill.BillingData_EarlyPaymentDiscount;
            GlobalViewModel.Instance.HispaniaViewModel.CalculateAmountInfo(DataList, 
                                                                           EarlyPaymentDiscountPrecent,
                                                                           EditedBill.IVAPercent,
                                                                           EditedBill.SurchargePercent,
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

        private void ActualizeReceiptStateInReceiptsList()
        {
            lblPaid.Visibility = rbPaid.Visibility = 
                           (ReceiptsListItems.SelectedItem is null) ? Visibility.Hidden : Visibility.Visible;
            lblReturned.Visibility = rbReturned.Visibility = 
                           (ReceiptsListItems.SelectedItem is null) ? Visibility.Hidden : Visibility.Visible;
            lblExpired.Visibility = rbExpired.Visibility = 
                           (ReceiptsListItems.SelectedItem is null) ? Visibility.Hidden : Visibility.Visible;
            if (ReceiptsListItems.SelectedItem != null)
            {
                ReceiptsView Receipt = (ReceiptsView)ReceiptsListItems.SelectedItem;
                switch (Receipt.State)
                {
                    case ReceiptState.Paid:
                         rbPaid.IsChecked = true;
                         break;
                    case ReceiptState.Returned:
                         rbReturned.IsChecked = true;
                         break;
                    case ReceiptState.Expired:
                         rbExpired.IsChecked = true;
                         break;
                    case ReceiptState.Pending:
                    default:
                         rbPending.IsChecked = true;
                         break;
                }
            }
        }

        private bool ActualizeReceiptStateInUI(ReceiptState NewState)
        {
            bool ReceiptsEquals = true; // By default No changes
            try
            {
                //  Step 1 : Actualize the EditedBill Receipt State field. 
                    ReceiptsView Receipt = (ReceiptsView)ReceiptsListItems.SelectedItem;
                    for (int i = 0; i < EditedBill.Receipts.Count; i++)
                    {
                        if (EditedBill.Receipts[i].Receipt_Id == Receipt.Receipt_Id)
                        {
                            EditedBill.Receipts[i].State = NewState;
                            break;
                        }
                    }
                //  Step 2 : Determine if has had a value change.
                    if ((EditedBill.Receipts == null) && (Bill.Receipts == null)) ReceiptsEquals = true;
                    else if ((EditedBill.Receipts == null) && (Bill.Receipts != null)) ReceiptsEquals = true;
                    else if ((EditedBill.Receipts != null) && (Bill.Receipts == null)) ReceiptsEquals = true;
                    else
                    {
                        foreach (ReceiptsView receipt in EditedBill.Receipts)
                        {
                            bool ReceiptFoundAndEqual = false;
                            foreach (ReceiptsView billReceipt in Bill.Receipts)
                            {
                                if (receipt.Receipt_Id == billReceipt.Receipt_Id)
                                {
                                    ReceiptFoundAndEqual = (receipt == billReceipt);
                                    break;
                                }
                            }
                            ReceiptsEquals &= ReceiptFoundAndEqual;
                            if (!ReceiptsEquals) break;
                        }
                    }
                //  Step 3 : Actualize Receipt State in the IU.
                    ((ReceiptsView)ReceiptsListItems.SelectedItem).State = NewState;
                    CollectionViewSource.GetDefaultView(ReceiptsListItems.ItemsSource).Refresh();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                LoadDataInControls(Bill, false);
            }
            return ReceiptsEquals;
        }

        private void ActualizeButtonBar()
        {
            btnViewData.Visibility = (ListItems.SelectedItem is null) ? Visibility.Hidden : Visibility.Visible;
        }

        private void ActualizeEditedBill(EffectTypesView EffectType)
        {
            if (EffectType.EffectType_Id == 6) // TRANSFERÈNCIA (Parameters)
            {
                GlobalViewModel.Instance.HispaniaViewModel.RefreshParameters();
                Parameters = GlobalViewModel.Instance.HispaniaViewModel.Parameters;
                EditedBill.DataBank_Bank = Parameters.DataBank_Bank;
                EditedBill.DataBank_BankAddress = Parameters.DataBank_BankAddress;
                EditedBill.DataBank_IBAN_CountryCode = Parameters.DataBank_IBAN_CountryCode;
                EditedBill.DataBank_IBAN_BankCode = Parameters.DataBank_IBAN_BankCode;
                EditedBill.DataBank_IBAN_OfficeCode = Parameters.DataBank_IBAN_OfficeCode;
                EditedBill.DataBank_IBAN_CheckDigits = Parameters.DataBank_IBAN_CheckDigits;
                EditedBill.DataBank_IBAN_AccountNumber = Parameters.DataBank_IBAN_AccountNumber;
            }
            else // ALTRA SISTEMA DE PAGAMENT (Customer)
            {
                EditedBill.DataBank_Bank = EditedBill.Customer.DataBank_Bank;
                EditedBill.DataBank_BankAddress = EditedBill.Customer.DataBank_BankAddress;
                EditedBill.DataBank_IBAN_CountryCode = EditedBill.Customer.DataBank_IBAN_CountryCode;
                EditedBill.DataBank_IBAN_BankCode = EditedBill.Customer.DataBank_IBAN_BankCode;
                EditedBill.DataBank_IBAN_OfficeCode = EditedBill.Customer.DataBank_IBAN_OfficeCode;
                EditedBill.DataBank_IBAN_CheckDigits = EditedBill.Customer.DataBank_IBAN_CheckDigits;
                EditedBill.DataBank_IBAN_AccountNumber = EditedBill.Customer.DataBank_IBAN_AccountNumber;
            }
            tbDataBank_Bank.Text = tbBilling_DataBank_Bank.Text = EditedBill.DataBank_Bank;
            tbDataBank_BankAddress.Text = tbBilling_DataBank_BankAddress.Text = EditedBill.DataBank_BankAddress;
            tbDataBankIBANCountryCode.Text = EditedBill.DataBank_IBAN_CountryCode;
            tbDataBankIBANBankCode.Text = EditedBill.DataBank_IBAN_BankCode;
            tbDataBankIBANOfficeCode.Text = EditedBill.DataBank_IBAN_OfficeCode;
            tbDataBankIBANCheckDigits.Text = EditedBill.DataBank_IBAN_CheckDigits;
            tbDataBankIBANAccountNumber.Text = EditedBill.DataBank_IBAN_AccountNumber;
        }

        #endregion

        #region Change Data Venciment
        private void btnChangeDataVenciment_Click(object sender, RoutedEventArgs e)
        {
            if (dtpExpirationDate.Visibility == Visibility.Visible)
            {
                GestionarVisibilidadDataVenciment(true);
            }
            else
            {
                GestionarVisibilidadDataVenciment(false);
            }
            
        }

        private void btnSaveDataVenciment_Click(object sender, RoutedEventArgs e)
        {            
            if (dtpExpirationDate.SelectedDate.HasValue)
            {               
                ((ReceiptsView)ReceiptsListItems.SelectedItem).Expiration_Date = dtpExpirationDate.SelectedDate.Value; 
                CollectionViewSource.GetDefaultView(ReceiptsListItems.ItemsSource).Refresh();
                GestionarVisibilidadDataVenciment(true);
                ReceiptsView Receipt = (ReceiptsView)ReceiptsListItems.SelectedItem;
                Receipt.Expiration_Date = dtpExpirationDate.SelectedDate.Value;
                GlobalViewModel.Instance.HispaniaViewModel.UpdateReceipt(Receipt);
            }
        }

        private void GestionarVisibilidadDataVenciment(bool guardado)
        {
            if (guardado)
            {
                dtpExpirationDate.Visibility = Visibility.Hidden;
                btnSaveDataVenciment.Visibility = Visibility.Hidden;               
            }else
            {
                dtpExpirationDate.Visibility = Visibility.Visible;
                btnSaveDataVenciment.Visibility = Visibility.Visible;                
            }
            
        }
        #endregion
    }
}
