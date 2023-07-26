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

#endregion

namespace HispaniaCommon.ViewClientWPF.UserControls
{
    /// <summary>
    /// Interaction logic for CustomersData.xaml
    /// </summary>
    public partial class MismatchesReceiptsData : UserControl
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
        /// Store the Goods
        /// </summary>
        private Dictionary<string, GoodsView> m_Goods;

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
                    m_Bill = value;
                    EditedBill = new BillsView(m_Bill);
                    CustomerOrders = m_Bill.CustomerOrders;
                    SendTypesDataList = m_Bill.CustomerOrders;
                    AddressStoresDataList = m_Bill.CustomerOrders;
                    ReceiptsDataList = EditedBill.Receipts;
                    LoadDataInControls(m_Bill);
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
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).SortDescriptions.Add(new SortDescription("CustomerOrderMovement_Id", ListSortDirection.Descending));
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
                if ((value is null) || (value.Count == 0))
                {
                    if (m_Bill.DataBank_NumEffect == 0)
                    {
                        if (MsgManager.ShowQuestion("El número d'efectes del client associat a la factura és 0, mentre sigui 0 no es generaran rebuts.\r\n" +
                                                    "Vol canviar el número d'efectes de la factura a 1 i generar un rebut per la mateixa?") 
                                                    == MessageBoxResult.Yes)
                        {
                            m_Bill.DataBank_NumEffect = 1;
                            m_ReceiptsDataList = EditedBill.Receipts = GlobalViewModel.Instance.HispaniaViewModel.GetReceiptsFromBill(m_Bill);
                            decimal Amount = 0;
                            foreach (ReceiptsView receipt in m_ReceiptsDataList)
                            {
                                Amount += GlobalViewModel.GetCurrencyValueFromDecimal(receipt.Amount);
                            }
                            tbCumulateExpiration.Text = string.Format("{0:0.00€}", Amount);
                            AreDataChanged = true;
                        }
                        else throw new Exception("No hi ha rebuts que permetin normalitzar la factura");
                    }
                    else throw new Exception("No hi ha rebuts que permetin normalitzar la factura");
                }
                else 
                {
                    m_ReceiptsDataList = value;
                    GlobalViewModel.Instance.HispaniaViewModel.ActualizeReceiptsAmount(m_ReceiptsDataList, Bill.BillAmount);
                    decimal Amount = 0;
                    foreach (ReceiptsView receipt in m_ReceiptsDataList)
                    {
                        Amount += GlobalViewModel.GetCurrencyValueFromDecimal(receipt.Amount);
                    }
                    tbCumulateExpiration.Text = string.Format("{0:0.00€}", Amount);
                    AreDataChanged = true;
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
                CollectionViewSource.GetDefaultView(SendTypesListItems.ItemsSource).SortDescriptions.Add(new SortDescription("Expiration_Date", ListSortDirection.Ascending));
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
                CollectionViewSource.GetDefaultView(AddressStoresListItems.ItemsSource).SortDescriptions.Add(new SortDescription("CustomerOrder_Id", ListSortDirection.Ascending));
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
                    case Operation.Edit:
                         if (Bill == null) throw new InvalidOperationException("Error, impossible editar una Factura sense dades.");
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
                    else if (control is RadioButton) ((RadioButton)control).IsEnabled = (m_CtrlOperation != Operation.Show);
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
        public MismatchesReceiptsData()
        {
            //  Initialization of controls of the UserControl
                InitializeComponent();
            //  Initialize GUI.
                ctxmnuIBAN_Initial = ctxmnuIBAN;
                InitEditableControls();
                InitNonEditableControls();
                InitOnlyQueryControls();
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
                lblBillSerieId,
                tbBillSerieId,
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
                lblBillingDataAgent,
                tbBillingDataAgent,
                lblBillingDataNumUnpaid,
                tbBillingDataNumUnpaid,
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
                tbBillingDataNumUnpaid,
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
                tbEffectType,
                lblDataBankIBANCountryCode,
                tbDataBankIBANCountryCode,
                lblDataBankIBANBankCode,
                tbDataBankIBANBankCode,
                lblDataBankIBANOfficeCode,
                tbDataBankIBANOfficeCode,
                lblDataBankIBANCheckDigits,
                tbDataBankIBANCheckDigits,
                lblDataBankIBANAccountNumber,
                tbDataBankIBANAccountNumber
            };
        }

        /// <summary>
        /// Initialize the list of Editable Controls.
        /// </summary>
        private void InitEditableControls()
        {
            EditableControls = new List<Control>
            {
                gbReceiptsItemsList,
                ReceiptsListItems
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
        /// Method that loads the Data in the controls of the Window
        /// </summary>
        private void LoadDataInControls(BillsView billsView, bool Actualize = true)
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Bill Controls
                tbBillId.Text = GlobalViewModel.GetStringFromIntIdValue(billsView.Bill_Id);
                tbBillDate.Text = billsView.Bill_Date_Str;
                tbBillSerieId.Text = billsView.BillingSerie_Str;
                tbCustomerId.Text = GlobalViewModel.GetStringFromIntIdValue(billsView.Customer.Customer_Id);
                tbCustomerAlias.Text = billsView.Customer_Alias;
                tbCompanyName.Text = billsView.Company_Name;
                tbCompanyCif.Text = billsView.Company_Cif;
                tbCompanyNumProv.Text = billsView.Company_NumProv;
                tbCompanyAddress.Text = billsView.Company_Address;
                tbCompanyPostalCode.Text = billsView.Company_PostalCode_Str;
                tbBillingDataEarlyPaymentDiscount.Text = GlobalViewModel.GetStringFromDecimalValue(billsView.BillingData_EarlyPaymentDiscount, DecimalType.Percent);
                tbIVAPercent.Text = GlobalViewModel.GetStringFromDecimalValue(billsView.IVAPercent, DecimalType.Percent, true);
                tbSurchargePercent.Text = GlobalViewModel.GetStringFromDecimalValue(billsView.SurchargePercent, DecimalType.Percent, true);
                tbBillRemarks.Text = billsView.Remarks;
                tbDataBank_Bank.Text = billsView.DataBank_Bank;
                tbDataBank_BankAddress.Text = billsView.DataBank_BankAddress;
                tbBilling_DataBank_Bank.Text = billsView.DataBank_Bank;
                tbBilling_DataBank_BankAddress.Text = billsView.DataBank_BankAddress;
                if ((EffectTypes != null) && (billsView.DataBank_EffectType != null))
                {
                    tbEffectType.Text = billsView.DataBank_EffectType.Description;
                }
                else tbEffectType.Text = "No Informat";
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
                tbBillingDataAgent.Text = (billsView.BillingData_Agent is null) ? 
                                           "No Informat" : 
                                           string.Format("({0}) - {1}", billsView.BillingData_Agent.Agent_Id, billsView.BillingData_Agent.Name);
                tbBillingDataNumUnpaid.Text = GlobalViewModel.GetStringFromDecimalValue(billsView.BillingData_NumUnpaid, DecimalType.WithoutDecimals);
                tbCustomerRemarks.Text = billsView.Customer_Remarks;
            //  Activate managers
                DataChangedManagerActive = true;
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
            //  ListView.
                ListItems.SelectionChanged += ListItems_SelectionChanged;
            //  Buttons
                btnAccept.Click += BtnAccept_Click;
                btnCancel.Click += BtnCancel_Click;
                btnHistoric.Click += BtnHistoric_Click;
                btnViewData.Click += BtnViewData_Click;
        }

        #region Buttons

        #region Accept

        /// <summary>
        /// Accept the management of Mismatch Receipt Bill.
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
                MsgManager.ShowMessage(
                   string.Format("Error, a l'intentar solucionar el desquadre de venciment.\r\nDetalls: {0}",
                                 MsgManager.ExcepMsg(ex)));
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
                           string.Format("Error, a l'intentar presentar les dades del moviment '{0}' del desquadre de venciment.\r\nDetalls: {1}",
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
                       string.Format("Error, a l'intentar presentar els històrics del client '{0}' associat al desquadre de venciment.\r\nDetalls: {1}",
                                     EditedBill.Customer.Customer_Alias, MsgManager.ExcepMsg(ex)));
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

        private void ActualizeDataBankData(CustomersView customer)
        {
            tbDataBank_Bank.Text = tbBilling_DataBank_Bank.Text = customer.DataBank_Bank;
            tbDataBank_BankAddress.Text = tbBilling_DataBank_BankAddress.Text = customer.DataBank_BankAddress;
            tbDataBankIBANCountryCode.Text = customer.DataBank_IBAN_CountryCode;
            tbDataBankIBANBankCode.Text = customer.DataBank_IBAN_BankCode;
            tbDataBankIBANOfficeCode.Text = customer.DataBank_IBAN_OfficeCode;
            tbDataBankIBANCheckDigits.Text = customer.DataBank_IBAN_CheckDigits;
            tbDataBankIBANAccountNumber.Text = customer.DataBank_IBAN_AccountNumber;
            ActualizeEditedBill();
        }

        private void ActualizeDataBankData()
        {
            tbDataBank_Bank.Text = tbBilling_DataBank_Bank.Text = Parameters.DataBank_Bank;
            tbDataBank_BankAddress.Text = tbBilling_DataBank_BankAddress.Text = Parameters.DataBank_BankAddress;
            tbDataBankIBANCountryCode.Text = Parameters.DataBank_IBAN_CountryCode;
            tbDataBankIBANBankCode.Text = Parameters.DataBank_IBAN_BankCode;
            tbDataBankIBANOfficeCode.Text = Parameters.DataBank_IBAN_OfficeCode;
            tbDataBankIBANCheckDigits.Text = Parameters.DataBank_IBAN_CheckDigits;
            tbDataBankIBANAccountNumber.Text = Parameters.DataBank_IBAN_AccountNumber;
            ActualizeEditedBill();
        }

        private void ActualizeEditedBill()
        {
            EditedBill.DataBank_Bank = tbDataBank_Bank.Text;
            EditedBill.DataBank_BankAddress = tbDataBank_BankAddress.Text;
            EditedBill.DataBank_IBAN_CountryCode = tbDataBankIBANCountryCode.Text;
            EditedBill.DataBank_IBAN_BankCode = tbDataBankIBANBankCode.Text;
            EditedBill.DataBank_IBAN_OfficeCode = tbDataBankIBANOfficeCode.Text;
            EditedBill.DataBank_IBAN_CheckDigits = tbDataBankIBANCheckDigits.Text;
            EditedBill.DataBank_IBAN_AccountNumber = tbDataBankIBANAccountNumber.Text;
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

        #endregion
    }
}
