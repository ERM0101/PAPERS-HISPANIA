#region Libraries used for this control

using HispaniaCommon.ViewClientWPF.Windows;
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
using System.Windows.Input;
using System.Windows.Media;

#endregion

namespace HispaniaCommon.ViewClientWPF.UserControls
{
    /// <summary>
    /// Interaction logic for CustomersData.xaml
    /// </summary>
    public partial class CustomersData : UserControl
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
        /// Hide the button bar.
        /// </summary>
        private GridLength HideComponent = new GridLength(0.0);

        /// <summary>
        /// Store normal color.
        /// </summary>
        private Brush brNormalDatePickerForeColor = new SolidColorBrush(Color.FromArgb(255, 5, 86, 158));

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
        /// Customer is correct.
        /// </summary>
        /// <param name="NewOrEditedCustomer">New or Edited Customer.</param>
        /// <param name="RelatedCustomers">Related Customers from the Customer.</param>
        public delegate void dlgAccept(CustomersView NewOrEditedCustomer, List<RelatedCustomersView> RelatedCustomers);

        /// <summary>
        /// Delegate that defines the firm of event produced when the Button Cancel is pressed.
        /// </summary>
        public delegate void dlgCancel();

        #endregion

        #region Events

        /// <summary>
        /// Event produced when the Button Accept is pressed and the data of the Customer is correct.
        /// </summary>
        public event dlgAccept EvAccept;

        /// <summary>
        /// Event produced when the Button Cancel is pressed.
        /// </summary>
        public event dlgCancel EvCancel;

        #endregion

        #region Attributes

        /// <summary>
        /// Store the Customer data to manage.
        /// </summary>
        private CustomersView m_Customer = null;

        /// <summary>
        /// Store the Cities and CP 
        /// </summary>
        public Dictionary<string, PostalCodesView> m_PostalCodes;

        /// <summary>
        /// Store the EffectTypes
        /// </summary>
        public Dictionary<string, EffectTypesView> m_EffectTypes;

        /// <summary>
        /// Store the SendTypes
        /// </summary>
        public Dictionary<string, SendTypesView> m_SendTypes;

        /// <summary>
        /// Store the Agents
        /// </summary>
        public Dictionary<string, AgentsView> m_Agents;

        /// <summary>
        /// Store the SendByTypes
        /// </summary>
        public Dictionary<string, IVATypesView> m_IVATypes;

        /// <summary>
        /// Store the type of Application with the user want open.
        /// </summary>
        private ApplicationType m_AppType;

        /// <summary>
        /// Store the Operation of the UserControl.
        /// </summary>
        private Operation m_CtrlOperation = Operation.Show;
        
        /// <summary>
        /// Stotre if the data of the Customer has changed.
        /// </summary>
        private bool m_AreDataChanged;

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private ObservableCollection<CustomersView> m_DataListDefinedCustomers = new ObservableCollection<CustomersView>();

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private ObservableCollection<CustomersView> m_DataListRelatedCustomers = new ObservableCollection<CustomersView>();

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

        /// <summary>
        /// Context Menu to calculate and validate the IBAN.
        /// </summary>
        private ContextMenu ctxmnuIBAN_Initial;

        /// <summary>
        /// Store the Customer Orders Window Type Active.
        /// </summary>
        private CustomerOrders CustomerOrdersWindow = null;

        /// <summary>
        /// Store the Customer Orders Window Type Active.
        /// </summary>
        private CustomerOrders CustomerOrders2Window = null;

        /// <summary>
        /// Window instance of HistoCumulativeCustomers.
        /// </summary>
        private HistoCumulativeCustomers HistoCumulativeCustomersWindow = null;

        /// <summary>
        /// Window instance of BadDebt.
        /// </summary>
        private BadDebts BadDebtsWindow = null;

        /// <summary>
        /// Window instance of HistoCustomers.
        /// </summary>
        private HistoCustomers HistoCustomersWindow = null;

        /// <summary>
        /// Store the Agents Window Type Active.
        /// </summary>
        private Agents AgentsWindow = null;

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
        /// Get or Set the Customer data to manage.
        /// </summary>
        public CustomersView Customer
        {
            get
            {
                return (m_Customer);
            }
            set
            {
                if (value != null)
                {
                    AreDataChanged = false;
                    m_Customer = value;
                    EditedCustomer = new CustomersView(m_Customer);
                    LoadDataInControls(m_Customer, true);
                }
                else throw new ArgumentNullException("Error, no s'han trobat les dades del client a carregar."); 
            }
        }

        /// <summary>
        /// Get or Set the data to show in List of Items.
        /// </summary>
        public ObservableCollection<CustomersView> DataListDefinedCustomers
        {
            get
            {
                return (m_DataListDefinedCustomers);
            }
            set
            {
                if (value != null) m_DataListDefinedCustomers = new ObservableCollection<CustomersView>(value);
                else m_DataListDefinedCustomers = new ObservableCollection<CustomersView>();
                ListItemsDefinedCustomers.ItemsSource = m_DataListDefinedCustomers;
                ListItemsDefinedCustomers.DataContext = this;
                CollectionViewSource.GetDefaultView(ListItemsDefinedCustomers.ItemsSource).SortDescriptions.Add(new SortDescription("Customer_Id", ListSortDirection.Descending));
                CollectionViewSource.GetDefaultView(ListItemsDefinedCustomers.ItemsSource).Filter = UserFilterDefinedCustomers;
                UpdateRelatedCustomersControls();
            }
        }

        /// <summary>
        /// Get or Set the data to show in List of Items.
        /// </summary>
        public ObservableCollection<CustomersView> DataListRelatedCustomers
        {
            get
            {
                return (m_DataListRelatedCustomers);
            }
            set
            {
                if (value != null) m_DataListRelatedCustomers = new ObservableCollection<CustomersView>(value);
                else m_DataListRelatedCustomers = new ObservableCollection<CustomersView>();
                SourceDataListRelatedCustomers = new ObservableCollection<CustomersView>(m_DataListRelatedCustomers);
                ListItemsRelatedCustomers.ItemsSource = m_DataListRelatedCustomers;
                ListItemsRelatedCustomers.DataContext = this;
                CollectionViewSource.GetDefaultView(ListItemsRelatedCustomers.ItemsSource).SortDescriptions.Add(new SortDescription("Customer_Id", ListSortDirection.Descending));
                CollectionViewSource.GetDefaultView(ListItemsRelatedCustomers.ItemsSource).Filter = UserFilterRelatedCustomers;
                UpdateRelatedCustomersControls();
            }
        }

        /// <summary>
        /// Get or Set the Edited Customer information.
        /// </summary>
        private CustomersView EditedCustomer
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set the data to show in List of Items.
        /// </summary>
        private ObservableCollection<CustomersView> SourceDataListRelatedCustomers
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
                         if (Customer == null) throw new InvalidOperationException("Error, impossible visualitzar un Client sense dades.");
                         tiConsullation.Visibility = Visibility.Visible;
                         gbSeveralDataAcum.Visibility = Visibility.Visible;
                         cbHistoricButton.Width = ViewHistoricButton;
                         tbDataBankIBANCountryCode.ContextMenu = null;
                         tbCancel.Text = "Tornar";
                         break;
                    case Operation.Add:
                         CustomersView NewCustomer = new CustomersView();
                         NewCustomer.Customer_Id = GlobalViewModel.Instance.HispaniaViewModel.GetNextIdentityValueTable(NewCustomer);
                         Customer = NewCustomer;
                         tiConsullation.Visibility = Visibility.Hidden;
                         cbHistoricButton.Width = HideHistoricButton;
                         gbSeveralDataAcum.Visibility = Visibility.Hidden;
                         tbDataBankIBANCountryCode.ContextMenu = ctxmnuIBAN_Initial;
                         tbCancel.Text = "Cancel·lar";
                         break;
                    case Operation.Edit:
                         if (Customer == null) throw new InvalidOperationException("Error, impossible editar un Client sense dades.");
                         tiConsullation.Visibility = Visibility.Visible;
                         cbHistoricButton.Width = ViewHistoricButton;
                         gbSeveralDataAcum.Visibility = Visibility.Visible;
                         tbDataBankIBANCountryCode.ContextMenu = ctxmnuIBAN_Initial;
                         tbCancel.Text = "Cancel·lar";
                         break;
                }
                string properyValue;
                foreach (Control control in EditableControls)
                {
                    if (control is TextBox) ((TextBox)control).IsReadOnly = (m_CtrlOperation == Operation.Show);
                    else if (control is RichTextBox) ((RichTextBox)control).IsReadOnly = (m_CtrlOperation == Operation.Show);
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
                    else if (control is DatePicker)
                    {
                        control.BorderBrush = ((m_CtrlOperation == Operation.Show) ? BrAppColor : brNormalDatePickerForeColor);
                        control.IsEnabled = (m_CtrlOperation != Operation.Show);
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
                cbLblGuarantedRisk.Width = cbGuarantedRisk.Width = HideComponent;
                cbLblCurrentRisk.Width = cbCurrentRisk.Width = HideComponent;
                tiGeneral.IsSelected = true;
            }
        }

        /// <summary>
        /// Get or Set if the data of the Customer has changed.
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
                    cbMiddleColumn_1.Width = ViewMiddleColumn;
                    cbMiddleColumn_2.Width = ViewMiddleColumn;
                }
                else
                {
                    cbAcceptButton.Width = HideAcceptButton;
                    cbMiddleColumn_1.Width = HideAcceptButton;
                    cbMiddleColumn_2.Width = HideAcceptButton;
                }
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

        #region Foreign Keys

        /// <summary>
        /// Get or Set the Cities and CP 
        /// </summary>
        public Dictionary<string, PostalCodesView> PostalCodes
        {
            get
            {
                return (m_PostalCodes);
            }
            set
            {
                m_PostalCodes = value;
                if (m_PostalCodes != null)
                {
                    cbCompanyPostalCode.ItemsSource = m_PostalCodes;
                    cbCompanyPostalCode.DisplayMemberPath = "Key";
                    cbCompanyPostalCode.SelectedValuePath = "Value";
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
                    cbDataBankEffect.ItemsSource = m_EffectTypes;
                    cbDataBankEffect.DisplayMemberPath = "Key";
                    cbDataBankEffect.SelectedValuePath = "Value";
                }
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
                    cbBillingDataSendByType.ItemsSource = m_SendTypes;
                    cbBillingDataSendByType.DisplayMemberPath = "Key";
                    cbBillingDataSendByType.SelectedValuePath = "Value";
                }
            }
        }

        /// <summary>
        /// Get or Set the IVATypes 
        /// </summary>
        public Dictionary<string, IVATypesView> IVATypes
        {
            get
            {
                return (m_IVATypes);
            }
            set
            {
                m_IVATypes = value;
                if (m_IVATypes != null)
                {
                    cbBillingDataIVAType.ItemsSource = m_IVATypes;
                    cbBillingDataIVAType.DisplayMemberPath = "Key";
                    cbBillingDataIVAType.SelectedValuePath = "Value";
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
        public CustomersData()
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
                lblCustomerId,
                tbCustomerId,
                tbSeveralDataAcum_1,
                tbSeveralDataAcum_2,
                tbSeveralDataAcum_3,
                tbSeveralDataAcum_4,
                tbSeveralDataAcum_5,
                tbSeveralDataAcum_6,
                tbSeveralDataAcum_7,
                tbSeveralDataAcum_8,
                tbSeveralDataAcum_9,
                tbSeveralDataAcum_10,
                tbSeveralDataAcum_11,
                tbSeveralDataAcum_12,
                tbBillingDataRegister,
                lblBillingDataUnpaid,
                tbBillingDataUnpaid,
                lblBillingDataNumUnpaid,
                tbBillingDataNumUnpaid,
                lblBillingDataCurrentRisk,
                tbBillingDataCurrentRisk,
                lblBillingDataRiskGranted,
                tbBillingDataRiskGranted,
            };
        }

        /// <summary>
        /// Initialize the list of Editable Controls.
        /// </summary>
        private void InitEditableControls()
        {
            EditableControls = new List<Control>
            {
                lblCustomerAlias,
                tbCustomerAlias,
                lblCompanyName,
                tbCompanyName,
                lblCompanyAddress,
                tbCompanyAddress,
                lblCompanyPostalCode,
                cbCompanyPostalCode,
                lblCompanyPhone1,
                tbCompanyPhone1,
                lblCompanyPhone2,
                tbCompanyPhone2,
                lblCompanyMobilePhone,
                tbCompanyMobilePhone,
                lblCompanyFax,
                tbCompanyFax,
                lblCompanyEmail,
                tbCompanyEmail,
                lblCompanyContactPerson,
                tbCompanyContactPerson,
                lblCompanyTimeTable,
                tbCompanyTimeTable,
                lblCompanyNumProv,
                tbCompanyNumProv,
                lblDataBankEffect,
                cbDataBankEffect,
                lblDataBankNumEffect,
                tbDataBankNumEffect,
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
                lblDataBankBank,
                tbDataBankBank,
                lblDataBankBankAddress,
                tbDataBankBankAddress,
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
                lblCompanyCif,
                tbCompanyCif,
                lblBillingDataSendByType,
                cbBillingDataSendByType,
                lblBillingDataBillingType,
                tbBillingDataBillingType,
                lblBillingDataDuplicate,
                tbBillingDataDuplicate,
                lblBillingDataEarlyPaymentDiscount,
                tbBillingDataEarlyPaymentDiscount,
                lblBillingDataValued,
                chkbBillingDataValued,
                lblBillingDataAgent,
                cbBillingDataAgent,
                lblBillingDataIVAType,
                cbBillingDataIVAType,
                lblBillingDataRegister,
                dtpBillingDataRegister,
                tbSeveralRemarks,
                btnAddAgent,
                gbIBAN,
                tiGeneral,
                tiBankData,
                tiBillingData,
                tiDivers,
                tiConsullation,
                tiRelatedCustomer,
                gbItemsListDefinedCustomers,
                gbItemsListRelatedCustomers,
                lblCanceled,
                chkbCanceled,
                btnRelatedCustomer,
                btnUnRelatedCustomer,
            };
        }

        /// <summary>
        /// Initialize the list of NonEditable Controls.
        /// </summary>
        private void InitOnlyQueryControls()
        {
            OnlyQueryControls = new List<Control>
            {
                btnHistoric,
                btnHistoricAcum,
                btnPendentCustomerOrders,
                btnPendentDeliveryNotes,
                btnBadDebt,
                gbQueries
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
        /// <param name="parametersView">Data Container.</param>
        /// <param name="ThrowException">true, if want throw an exception if not found a component</param>
        private void LoadDataInControls(CustomersView customersView, bool ThrowException = false)
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Customer Controls
                tbCustomerId.Text = GlobalViewModel.GetStringFromIntIdValue(customersView.Customer_Id);
                tbCustomerAlias.Text = customersView.Customer_Alias;
                chkbCanceled.IsChecked = customersView.Canceled;
            //  Company Controls
                tbCompanyName.Text = customersView.Company_Name;
                tbCompanyAddress.Text = customersView.Company_Address;
                tbCompanyPhone1.Text = customersView.Company_Phone_1;
                tbCompanyPhone2.Text = customersView.Company_Phone_2;
                tbCompanyFax.Text = customersView.Company_Fax;
                tbCompanyMobilePhone.Text = customersView.Company_MobilePhone;
                tbCompanyEmail.Text = customersView.Company_EMail;
                tbCompanyContactPerson.Text = customersView.Company_ContactPerson;
                tbCompanyTimeTable.Text = customersView.Company_TimeTable;
                tbCompanyNumProv.Text = customersView.Company_NumProv;
            //  DataBank Controls
                tbDataBankNumEffect.Text = GlobalViewModel.GetStringFromDecimalValue(customersView.DataBank_NumEffect, DecimalType.WithoutDecimals);
                tbDataBankFirstExpirationData.Text = GlobalViewModel.GetStringFromDecimalValue(customersView.DataBank_FirstExpirationData, DecimalType.WithoutDecimals);
                tbDataBankExpirationInterval.Text = GlobalViewModel.GetStringFromDecimalValue(customersView.DataBank_ExpirationInterval, DecimalType.WithoutDecimals);
                tbDataBankPayday_1.Text = GlobalViewModel.GetStringFromDecimalValue(customersView.DataBank_Payday_1, DecimalType.WithoutDecimals);
                tbDataBankPayday_2.Text = GlobalViewModel.GetStringFromDecimalValue(customersView.DataBank_Payday_2, DecimalType.WithoutDecimals);
                tbDataBankPayday_3.Text = GlobalViewModel.GetStringFromDecimalValue(customersView.DataBank_Payday_3, DecimalType.WithoutDecimals);
                tbDataBankBank.Text = customersView.DataBank_Bank;
                tbDataBankBankAddress.Text = customersView.DataBank_BankAddress;
                tbDataBankIBANCountryCode.Text = customersView.DataBank_IBAN_CountryCode;
                tbDataBankIBANBankCode.Text = customersView.DataBank_IBAN_BankCode;
                tbDataBankIBANOfficeCode.Text = customersView.DataBank_IBAN_OfficeCode;
                tbDataBankIBANCheckDigits.Text = customersView.DataBank_IBAN_CheckDigits;
                tbDataBankIBANAccountNumber.Text = customersView.DataBank_IBAN_AccountNumber;
            //  BillingData
                tbCompanyCif.Text = customersView.Company_Cif;
                tbBillingDataBillingType.Text = customersView.BillingData_BillingType;
                tbBillingDataDuplicate.Text = GlobalViewModel.GetStringFromDecimalValue(customersView.BillingData_Duplicate, DecimalType.WithoutDecimals);
                tbBillingDataEarlyPaymentDiscount.Text = GlobalViewModel.GetStringFromDecimalValue(customersView.BillingData_EarlyPaymentDiscount, DecimalType.Percent);
                chkbBillingDataValued.IsChecked = customersView.BillingData_Valued;
                tbBillingDataRiskGranted.Text = GlobalViewModel.GetStringFromDecimalValue(customersView.BillingData_RiskGranted, DecimalType.Currency);
                tbBillingDataCurrentRisk.Text = GlobalViewModel.GetStringFromDecimalValue(customersView.BillingData_CurrentRisk, DecimalType.Currency);
                tbBillingDataUnpaid.Text = GlobalViewModel.GetStringFromDecimalValue(customersView.BillingData_Unpaid, DecimalType.Currency);
                tbBillingDataNumUnpaid.Text = GlobalViewModel.GetStringFromDecimalValue(customersView.BillingData_NumUnpaid, DecimalType.WithoutDecimals);
                dtpBillingDataRegister.SelectedDate = customersView.BillingData_Register;
                if (dtpBillingDataRegister.SelectedDate != null) tbBillingDataRegister.Text = GlobalViewModel.GetLongDateString(customersView.BillingData_Register);
                LoadExternalTablesInfo(customersView, ThrowException);
            //  Several Remarks
                tbSeveralRemarks.Text = customersView.Several_Remarks;
                tbSeveralDataAcum_1.Text = GlobalViewModel.GetStringFromDecimalValue(customersView.SeveralData_Acum_1, DecimalType.Currency);
                tbSeveralDataAcum_2.Text = GlobalViewModel.GetStringFromDecimalValue(customersView.SeveralData_Acum_2, DecimalType.Currency);
                tbSeveralDataAcum_3.Text = GlobalViewModel.GetStringFromDecimalValue(customersView.SeveralData_Acum_3, DecimalType.Currency);
                tbSeveralDataAcum_4.Text = GlobalViewModel.GetStringFromDecimalValue(customersView.SeveralData_Acum_4, DecimalType.Currency);
                tbSeveralDataAcum_5.Text = GlobalViewModel.GetStringFromDecimalValue(customersView.SeveralData_Acum_5, DecimalType.Currency);
                tbSeveralDataAcum_6.Text = GlobalViewModel.GetStringFromDecimalValue(customersView.SeveralData_Acum_6, DecimalType.Currency);
                tbSeveralDataAcum_7.Text = GlobalViewModel.GetStringFromDecimalValue(customersView.SeveralData_Acum_7, DecimalType.Currency);
                tbSeveralDataAcum_8.Text = GlobalViewModel.GetStringFromDecimalValue(customersView.SeveralData_Acum_8, DecimalType.Currency);
                tbSeveralDataAcum_9.Text = GlobalViewModel.GetStringFromDecimalValue(customersView.SeveralData_Acum_9, DecimalType.Currency);
                tbSeveralDataAcum_10.Text = GlobalViewModel.GetStringFromDecimalValue(customersView.SeveralData_Acum_10, DecimalType.Currency);
                tbSeveralDataAcum_11.Text = GlobalViewModel.GetStringFromDecimalValue(customersView.SeveralData_Acum_11, DecimalType.Currency);
                tbSeveralDataAcum_12.Text = GlobalViewModel.GetStringFromDecimalValue(customersView.SeveralData_Acum_12, DecimalType.Currency);
            //  Activate managers
                DataChangedManagerActive = true;
        }

        /// <summary>
        /// Load Data from External Tables.
        /// </summary>
        /// <param name="customersView">Data Container.</param>
        /// <param name="ThrowException">true, if want throw an exception if not found a component</param>
        private void LoadExternalTablesInfo(CustomersView customersView, bool ThrowException = false)
        {
            if ((PostalCodes != null) && (customersView.Company_PostalCode != null))
            {
                Dictionary<string, PostalCodesView> Items = (Dictionary<string, PostalCodesView>)cbCompanyPostalCode.ItemsSource;
                string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeyPostalCodeView(customersView.Company_PostalCode);
                if (Items.ContainsKey(Key)) cbCompanyPostalCode.SelectedValue = PostalCodes[Key];
                else
                {
                    if (ThrowException)
                    {
                        throw new Exception(string.Format("No s'ha trobat el Còdi Postal '{0}'.", PostalCodes[Key].Postal_Code_Info));
                    }
                }
            }
            else cbCompanyPostalCode.SelectedIndex = -1;
            if ((EffectTypes != null) && (customersView.DataBank_EffectType != null))
            {
                Dictionary<string, EffectTypesView> Items = (Dictionary<string, EffectTypesView>)cbDataBankEffect.ItemsSource;
                string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeyEffectTypeView(customersView.DataBank_EffectType);
                if (Items.ContainsKey(Key)) cbDataBankEffect.SelectedValue = EffectTypes[Key];
                else
                {
                    if (ThrowException)
                    {
                        throw new Exception(string.Format("No s'ha trobat el Tipus d'Efecte '{0}-{1}'.", EffectTypes[Key].Code, EffectTypes[Key].Description));
                    }
                }                
            }
            else cbDataBankEffect.SelectedIndex = -1;
            if ((SendTypes != null) && (customersView.BillingData_SendType != null))
            {
                Dictionary<string, SendTypesView> Items = (Dictionary<string, SendTypesView>)cbBillingDataSendByType.ItemsSource;
                string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeySendTypeView(customersView.BillingData_SendType);                
                if (Items.ContainsKey(Key)) cbBillingDataSendByType.SelectedValue = SendTypes[Key];
                else
                {
                    if (ThrowException)
                    {
                        throw new Exception(string.Format("No s'ha trobat el Tipus d'Enviament '{0}-{1}'.", SendTypes[Key].Code, SendTypes[Key].Description));
                    }
                }
            }
            else cbBillingDataSendByType.SelectedIndex = -1;
            if ((Agents != null) && (customersView.BillingData_Agent != null))
            {
                Dictionary<string, AgentsView> Items = (Dictionary<string, AgentsView>)cbBillingDataAgent.ItemsSource;
                string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeyAgentView(customersView.BillingData_Agent);                
                if (Items.ContainsKey(Key)) cbBillingDataAgent.SelectedValue = Agents[Key];
                else
                {
                    if (ThrowException)
                    {
                        throw new Exception(string.Format("No s'ha trobat el Representant '{0}'.", Agents[Key].Name));
                    }
                }
            }
            else cbBillingDataAgent.SelectedIndex = -1;
            if ((IVATypes != null) && (customersView.BillingData_IVAType != null))
            {
                Dictionary<string, IVATypesView> Items = (Dictionary<string, IVATypesView>)cbBillingDataIVAType.ItemsSource;
                string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeyIVATypeView(customersView.BillingData_IVAType);
                if (Items.ContainsKey(Key)) cbBillingDataIVAType.SelectedValue = IVATypes[Key];
                else
                {
                    if (ThrowException)
                    {
                        throw new Exception(
                                     string.Format("No s'ha trobat el Tipus d'IVA '{0}-{1}-{2}'.", 
                                                   IVATypes[Key].Type, IVATypes[Key].IVAPercent_Str, IVATypes[Key].SurchargePercent_Str));
                    }
                }                
            }
            else cbBillingDataIVAType.SelectedIndex = -1;
        }
        
        /// <summary>
        /// Method that filter the elements that are showing in the list
        /// </summary>
        /// <param name="item">Item to test</param>
        /// <returns>true, if the item must be loaded, false, if not.</returns>
        private bool UserFilterDefinedCustomers(object item)
        {
            //  Don't do anything if Customer doesn't have any value.
                if (Customer is null) return true;
            //  Get Acces to the object and the property name To Filter.
                CustomersView ItemData = (CustomersView)item;
            //  If the item is the Customer don't show this item.
                if (ItemData.Customer_Id == Customer.Customer_Id) return false;
            //  Calculate the Visibility value with properties values.
                return (DataListRelatedCustomers is null) || (!m_DataListRelatedCustomers.Contains(ItemData));
        }
        
        /// <summary>
        /// Method that filter the elements that are showing in the list
        /// </summary>
        /// <param name="item">Item to test</param>
        /// <returns>true, if the item must be loaded, false, if not.</returns>
        private bool UserFilterRelatedCustomers(object item)
        {
            //  Get Acces to the object and the property name To Filter.
                CustomersView ItemData = (CustomersView)item;
            //  If the item is the Customer don't show this item.
                if (ItemData.Customer_Id == Customer.Customer_Id) return false;
            //  Calculate the Visibility value with properties values.
                return (DataListDefinedCustomers is null) || (!m_DataListDefinedCustomers.Contains(ItemData));
        }

        #endregion

        #region Managers

        /// <summary>
        /// Method that define the managers needed for the user operations in the UserControl
        /// </summary>
        private void LoadManagers()
        {
            //  By default the manager for the Customer Data changes is active.
                DataChangedManagerActive = true;
            //  TextBox
                tbCustomerAlias.PreviewTextInput += TBPreviewTextInput;
                tbCustomerAlias.TextChanged += TBDataChanged;
                tbCompanyName.PreviewTextInput += TBPreviewTextInput;
                tbCompanyName.TextChanged += TBDataChanged;
                tbCompanyAddress.PreviewTextInput += TBPreviewTextInput;
                tbCompanyAddress.TextChanged += TBDataChanged;
                tbCompanyPhone1.PreviewTextInput += TBPreviewTextInput;
                tbCompanyPhone1.TextChanged += TBDataChanged;
                tbCompanyPhone2.PreviewTextInput += TBPreviewTextInput;
                tbCompanyPhone2.TextChanged += TBDataChanged;
                tbCompanyMobilePhone.PreviewTextInput += TBPreviewTextInput;
                tbCompanyMobilePhone.TextChanged += TBDataChanged;
                tbCompanyFax.PreviewTextInput += TBPreviewTextInput;
                tbCompanyFax.TextChanged += TBDataChanged;
                tbCompanyEmail.PreviewTextInput += TBPreviewTextInput;
                tbCompanyEmail.TextChanged += TBDataChanged;
                tbCompanyContactPerson.PreviewTextInput += TBPreviewTextInput;
                tbCompanyContactPerson.TextChanged += TBDataChanged;
                tbCompanyTimeTable.PreviewTextInput += TBPreviewTextInput;
                tbCompanyTimeTable.TextChanged += TBDataChanged;
                tbCompanyNumProv.PreviewTextInput += TBPreviewTextInput;
                tbCompanyNumProv.TextChanged += TBDataChanged;
                tbDataBankNumEffect.PreviewTextInput += TBPreviewTextInput;
                tbDataBankNumEffect.TextChanged += TBWithoutDecimalsDataChanged;
                tbDataBankFirstExpirationData.PreviewTextInput += TBPreviewTextInput;
                tbDataBankFirstExpirationData.TextChanged += TBWithoutDecimalsDataChanged;
                tbDataBankExpirationInterval.PreviewTextInput += TBPreviewTextInput;
                tbDataBankExpirationInterval.TextChanged += TBWithoutDecimalsDataChanged;
                tbDataBankPayday_1.PreviewTextInput += TBPreviewTextInput;
                tbDataBankPayday_1.TextChanged += TBWithoutDecimalsDataChanged;
                tbDataBankPayday_2.PreviewTextInput += TBPreviewTextInput;
                tbDataBankPayday_2.TextChanged += TBWithoutDecimalsDataChanged;
                tbDataBankPayday_3.PreviewTextInput += TBPreviewTextInput;
                tbDataBankPayday_3.TextChanged += TBWithoutDecimalsDataChanged;
                tbDataBankBank.PreviewTextInput += TBPreviewTextInput;
                tbDataBankBank.TextChanged += TBDataChanged;
                tbDataBankBankAddress.PreviewTextInput += TBPreviewTextInput;
                tbDataBankBankAddress.TextChanged += TBDataChanged;
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
                tbCompanyCif.PreviewTextInput += TBPreviewTextInput;
                tbCompanyCif.TextChanged += TBDataChanged;
                tbBillingDataBillingType.PreviewTextInput += TBPreviewTextInput;
                tbBillingDataBillingType.TextChanged += TBDataChanged;
                tbBillingDataDuplicate.PreviewTextInput += TBPreviewTextInput;
                tbBillingDataDuplicate.TextChanged += TBWithoutDecimalsDataChanged;
                tbBillingDataEarlyPaymentDiscount.PreviewTextInput += TBPreviewTextInput;
                tbBillingDataEarlyPaymentDiscount.TextChanged += TBPercentDataChanged;
                //tbBillingDataRiskGranted.PreviewTextInput += TBPreviewTextInput;
                //tbBillingDataRiskGranted.TextChanged += TBCurrencyDataChanged;
                //tbBillingDataCurrentRisk.PreviewTextInput += TBPreviewTextInput;
                //tbBillingDataCurrentRisk.TextChanged += TBCurrencyDataChanged;
                //tbBillingDataUnpaid.PreviewTextInput += TBPreviewTextInput;
                //tbBillingDataUnpaid.TextChanged += TBCurrencyDataChanged;
                //tbBillingDataNumUnpaid.PreviewTextInput += TBPreviewTextInput;
                //tbBillingDataNumUnpaid.TextChanged += TBDataChanged;
                tbSeveralRemarks.PreviewTextInput += TBPreviewTextInput;
                tbSeveralRemarks.TextChanged += TBDataChanged;
            //  DatePiker
                dtpBillingDataRegister.SelectedDateChanged += DtpBillingDataRegister_SelectedDateChanged;
            //  CheckBox
                chkbBillingDataValued.Checked += ChkbBillingDataValued_Checked;
                chkbBillingDataValued.Unchecked += ChkbBillingDataValued_Unchecked;
                chkbCanceled.Checked += ChkbCanceled_Checked;
                chkbCanceled.Unchecked += ChkbCanceled_Unchecked;
            //  ComboBox
                cbCompanyPostalCode.SelectionChanged += CbCompanyPostalCode_SelectionChanged;
                cbDataBankEffect.SelectionChanged += CbDataBankEffect_SelectionChanged;
                cbBillingDataSendByType.SelectionChanged += CbBillingDataSendByType_SelectionChanged;
                cbBillingDataAgent.SelectionChanged += CbBillingDataAgent_SelectionChanged;
                cbBillingDataIVAType.SelectionChanged += CbBillingDataIVAType_SelectionChanged;
            //  ContextMenuItem
                ctxmnuItemCalculateIBAN.Click += CtxmnuItemCalculateIBAN_Click;
                ctxmnuItemValidateIBAN.Click += CtxmnuItemValidateIBAN_Click;
            //  Buttons
                btnAccept.Click += BtnAccept_Click;
                btnCancel.Click += BtnCancel_Click;
                btnAddAgent.Click += BtnAddAgent_Click;
                btnHistoricAcum.Click += BtnHistoricAcum_Click;
                btnHistoric.Click += BtnHistoric_Click;
                btnBadDebt.Click += BtnBadDebt_Click;
                btnPendentCustomerOrders.Click += BtnPendentCustomerOrders_Click;
                btnPendentDeliveryNotes.Click += BtnPendentDeliveryNotes_Click;
                btnRelatedCustomer.Click += BtnRelatedCustomer_Click;
                btnUnRelatedCustomer.Click += BtnUnRelatedCustomer_Click;
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
            if ((sender == tbCustomerAlias) || (sender == tbCompanyName) || (sender == tbCompanyContactPerson) ||
                (sender == tbDataBankBank))
            {
                e.Handled = ! GlobalViewModel.IsValidCommentChar(e.Text);
            }
            else if ((sender == tbCompanyAddress) || (sender == tbDataBankBankAddress))
            {
                e.Handled = ! GlobalViewModel.IsValidAddressChar(e.Text);
            }
            else if ((sender == tbCompanyPhone1) || (sender == tbCompanyPhone2) || (sender == tbCompanyFax))
            {
                e.Handled = ! GlobalViewModel.IsValidPhoneNumberChar(e.Text);
            }
            else if (sender == tbCompanyMobilePhone) e.Handled = ! GlobalViewModel.IsValidMobilePhoneNumberChar(e.Text);
            else if (sender == tbCompanyEmail) e.Handled = ! GlobalViewModel.IsValidEmailChar(e.Text);
            else if (sender == tbCompanyTimeTable) e.Handled = ! GlobalViewModel.IsValidTimeTableChar(e.Text);
            else if (sender == tbCompanyNumProv) e.Handled = ! GlobalViewModel.IsValidUintChar(e.Text);
            else if (sender == tbCompanyCif) e.Handled = ! GlobalViewModel.IsValidCIFChar(e.Text);
            else if ((sender == tbDataBankNumEffect) || (sender == tbDataBankFirstExpirationData) ||
                     (sender == tbDataBankExpirationInterval) || (sender == tbDataBankPayday_1) ||
                     (sender == tbDataBankPayday_2) || (sender == tbDataBankPayday_3) ||
                     (sender == tbBillingDataBillingType) || (sender == tbBillingDataDuplicate))
            {
                e.Handled = ! GlobalViewModel.IsValidShortDecimalChar(e.Text);
            }
            //else if (sender == tbBillingDataNumUnpaid) e.Handled = ! GlobalViewModel.IsValidShortDecimalChar(e.Text);
            else if (sender == tbDataBankIBANCountryCode) e.Handled = ! GlobalViewModel.IsValidIBAN_CountryCodeChar(e.Text);
            else if (sender == tbDataBankIBANBankCode) e.Handled = ! GlobalViewModel.IsValidIBAN_BankCodeChar(e.Text);
            else if (sender == tbDataBankIBANOfficeCode) e.Handled = ! GlobalViewModel.IsValidIBAN_OfficeCodeChar(e.Text);
            else if (sender == tbDataBankIBANCheckDigits) e.Handled = ! GlobalViewModel.IsValidIBAN_CheckDigitsChar(e.Text);
            else if (sender == tbDataBankIBANAccountNumber) e.Handled = ! GlobalViewModel.IsValidIBAN_AccountNumberChar(e.Text);
            //else if ((sender == tbBillingDataRiskGranted) || (sender == tbBillingDataUnpaid) || (sender == tbBillingDataCurrentRisk)) 
            //{
            //    e.Handled = ! GlobalViewModel.IsValidCurrencyChar(e.Text);
            //} 
            else if (sender == tbBillingDataEarlyPaymentDiscount) e.Handled = ! GlobalViewModel.IsValidPercentChar(e.Text);
            else if (sender == tbSeveralRemarks) e.Handled = ! GlobalViewModel.IsValidCommentChar(e.Text);
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
                    if (sender == tbCustomerAlias) EditedCustomer.Customer_Alias = value;
                    else if (sender == tbCompanyName) EditedCustomer.Company_Name = value;
                    else if (sender == tbCompanyAddress) EditedCustomer.Company_Address = value;
                    else if (sender == tbCompanyContactPerson) EditedCustomer.Company_ContactPerson = value;
                    else if (sender == tbCompanyPhone1) EditedCustomer.Company_Phone_1 = value;
                    else if (sender == tbCompanyPhone2) EditedCustomer.Company_Phone_2 = value;
                    else if (sender == tbCompanyFax) EditedCustomer.Company_Fax = value;
                    else if (sender == tbCompanyMobilePhone) EditedCustomer.Company_MobilePhone = value;
                    else if (sender == tbCompanyEmail) EditedCustomer.Company_EMail = value;
                    else if (sender == tbCompanyTimeTable) EditedCustomer.Company_TimeTable = value;
                    else if (sender == tbCompanyNumProv) EditedCustomer.Company_NumProv = value;
                    else if (sender == tbCompanyCif) EditedCustomer.Company_Cif = value;
                    else if (sender == tbDataBankBank) EditedCustomer.DataBank_Bank = value;
                    else if (sender == tbDataBankBankAddress) EditedCustomer.DataBank_BankAddress = value;
                    else if (sender == tbDataBankIBANCountryCode) EditedCustomer.DataBank_IBAN_CountryCode = value;
                    else if (sender == tbDataBankIBANBankCode) EditedCustomer.DataBank_IBAN_BankCode = value;
                    else if (sender == tbDataBankIBANOfficeCode) EditedCustomer.DataBank_IBAN_OfficeCode = value;
                    else if (sender == tbDataBankIBANCheckDigits) EditedCustomer.DataBank_IBAN_CheckDigits = value;
                    else if (sender == tbDataBankIBANAccountNumber) EditedCustomer.DataBank_IBAN_AccountNumber = value;
                    else if (sender == tbBillingDataBillingType) EditedCustomer.BillingData_BillingType = value;
                    else if (sender == tbSeveralRemarks) EditedCustomer.Several_Remarks = value;
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedCustomer);
                }
                AreDataChanged = EditedCustomer != Customer || ValidateChangesInRelatedCustomers();
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Manage the change of the Data in the sender object.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void TBWithoutDecimalsDataChanged(object sender, TextChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                TextBox tbInput = (TextBox)sender;
                try
                {
                    GlobalViewModel.NormalizeTextBox(sender, e, DecimalType.WithoutDecimals);
                    decimal value = GlobalViewModel.GetUIDecimalValue(tbInput.Text);
                    if (sender == tbDataBankNumEffect) EditedCustomer.DataBank_NumEffect = value;
                    else if (sender == tbDataBankFirstExpirationData) EditedCustomer.DataBank_FirstExpirationData = value;
                    else if (sender == tbDataBankExpirationInterval) EditedCustomer.DataBank_ExpirationInterval = value;
                    else if (sender == tbDataBankPayday_1) EditedCustomer.DataBank_Payday_1 = value;
                    else if (sender == tbDataBankPayday_2) EditedCustomer.DataBank_Payday_2 = value;
                    else if (sender == tbDataBankPayday_3) EditedCustomer.DataBank_Payday_3 = value;
                    else if (sender == tbBillingDataDuplicate) EditedCustomer.BillingData_Duplicate = value;
                    //else if (sender == tbBillingDataNumUnpaid) EditedCustomer.BillingData_NumUnpaid = value;
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedCustomer);
                }
                AreDataChanged = EditedCustomer != Customer || ValidateChangesInRelatedCustomers();
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Manage the change of the Data in the sender object.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void TBPercentDataChanged(object sender, TextChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                TextBox tbInput = (TextBox)sender;
                try
                {
                    GlobalViewModel.NormalizeTextBox(sender, e, DecimalType.Percent);
                    decimal value = GlobalViewModel.GetUIDecimalValue(tbInput.Text);
                    if (sender == tbBillingDataEarlyPaymentDiscount) EditedCustomer.BillingData_EarlyPaymentDiscount = value;
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedCustomer);
                }
                AreDataChanged = EditedCustomer != Customer || ValidateChangesInRelatedCustomers();
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Manage the change of the Data in the sender object.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void TBCurrencyDataChanged(object sender, TextChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                TextBox tbInput = (TextBox)sender;
                try
                {
                    GlobalViewModel.NormalizeTextBox(sender, e, DecimalType.Currency);
                    decimal value = GlobalViewModel.GetUIDecimalValue(tbInput.Text);
                    //if (sender == tbBillingDataRiskGranted) EditedCustomer.BillingData_RiskGranted = value;
                    //else if (sender == tbBillingDataCurrentRisk) EditedCustomer.BillingData_CurrentRisk = value;
                    //else if (sender == tbBillingDataUnpaid) EditedCustomer.BillingData_Unpaid = value;
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedCustomer);
                }
                AreDataChanged = EditedCustomer != Customer || ValidateChangesInRelatedCustomers();
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region Button

        #region Accept

        /// <summary>
        /// Accept the edition or creatin of the Customer.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {
            CustomersAttributes ErrorField = CustomersAttributes.None;
            try
            {
                if ((CtrlOperation == Operation.Add) || (CtrlOperation == Operation.Edit))
                {
                    EditedCustomer.Validate(out ErrorField);
                    List<RelatedCustomersView> RelatedCustomers = new List<RelatedCustomersView>();
                    foreach (CustomersView relatedCustomer in DataListRelatedCustomers)
                    {
                        RelatedCustomers.Add(new RelatedCustomersView()
                                             {
                                                 Customer_Id = Customer.Customer_Id,
                                                 Customer_Canceled_Id = relatedCustomer.Customer_Id,
                                                 Remarks = string.Empty
                                             });
                    }
                    EvAccept?.Invoke(new CustomersView(EditedCustomer), RelatedCustomers);
                }
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al validar les dades introduïdes.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                RestoreSourceValue(ErrorField);
            }
        }

        #endregion

        #region Cancel

        /// <summary>
        /// Cancel the edition or creatin of the Customer.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //  Send the event that indicates at the observer that the operation is cancelled.
                EvCancel?.Invoke();
        }

        #endregion

        #region Add Agent (Update data)

        /// <summary>
        /// Defines a new Agent for the Customer.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnAddAgent_Click(object sender, RoutedEventArgs e)
        {
            if (AgentsWindow == null)
            {
                try
                {
                    AgentsWindow = new Agents(AppType);
                    GlobalViewModel.Instance.HispaniaViewModel.RefreshPostalCodes();
                    AgentsWindow.PostalCodes = GlobalViewModel.Instance.HispaniaViewModel.PostalCodesDict;
                    GlobalViewModel.Instance.HispaniaViewModel.RefreshAgents();
                    AgentsWindow.DataList = GlobalViewModel.Instance.HispaniaViewModel.Agents;
                    AgentsWindow.Closed += AgentsWindow_Closed;
                    AgentsWindow.Show();

                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, al carregar la finestra de gestió de Representants.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
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
            GlobalViewModel.Instance.HispaniaViewModel.RefreshAgents();
            Agents = GlobalViewModel.Instance.HispaniaViewModel.AgentsDict;
            AgentsWindow.Closed -= AgentsWindow_Closed;
            AgentsWindow = null;
        }

        #endregion

        #region Delivery Notes (Update data)

        private void BtnPendentDeliveryNotes_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerOrdersWindow == null)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.DeliveryNotesWindow);
                    CustomerOrdersWindow = new CustomerOrders(AppType, false, false, true)
                    {
                        FilterByCustomer = true,
                        Customer = Customer,
                        Customers = GlobalViewModel.Instance.HispaniaViewModel.CustomersActiveDict,
                        SendTypes = GlobalViewModel.Instance.HispaniaViewModel.SendTypesDict,
                        EffectTypes = GlobalViewModel.Instance.HispaniaViewModel.EffectTypesDict,
                        Agents = GlobalViewModel.Instance.HispaniaViewModel.AgentsDict,
                        Parameters = GlobalViewModel.Instance.HispaniaViewModel.Parameters,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.CustomerOrders
                    };
                    CustomerOrdersWindow.Closed += CustomerOrdersWindow_Closed;
                    CustomerOrdersWindow.Show();

                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, al carregar la finestra de gestió d'Albarans.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
                finally
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
            else CustomerOrdersWindow.Activate();
        }

        /// <summary>
        /// When the Customer Window is closed we actualize the CustomersWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void CustomerOrdersWindow_Closed(object sender, EventArgs e)
        {
            CustomerOrdersWindow.Closed -= CustomerOrdersWindow_Closed;
            CustomerOrdersWindow = null;
        }

        #endregion

        #region Customer Orders (Update data)

        private void BtnPendentCustomerOrders_Click(object sender, RoutedEventArgs e)
        {
            if (CustomerOrders2Window == null)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.CustomerOrdersWindow);
                    CustomerOrders2Window = new CustomerOrders(AppType, false, true, false)
                    {
                        FilterByCustomer = true,
                        Customer = Customer,
                        Customers = GlobalViewModel.Instance.HispaniaViewModel.CustomersActiveDict,
                        SendTypes = GlobalViewModel.Instance.HispaniaViewModel.SendTypesDict,
                        EffectTypes = GlobalViewModel.Instance.HispaniaViewModel.EffectTypesDict,
                        Agents = GlobalViewModel.Instance.HispaniaViewModel.AgentsDict,
                        Parameters = GlobalViewModel.Instance.HispaniaViewModel.Parameters,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.CustomerOrders
                    };
                    CustomerOrders2Window.Closed += CustomerOrders2Window_Closed;
                    CustomerOrders2Window.Show();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, al carregar la finestra de gestió de Comandes de Client.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
                finally
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
            else CustomerOrders2Window.Activate();
        }

        /// <summary>
        /// When the Customer Window is closed we actualize the CustomersWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void CustomerOrders2Window_Closed(object sender, EventArgs e)
        {
            CustomerOrders2Window.Closed -= CustomerOrders2Window_Closed;
            CustomerOrders2Window = null;
        }

        #endregion

        #region HistoricAcum (Query data)

        private void BtnHistoricAcum_Click(object sender, RoutedEventArgs e)
        {
            if (HistoCumulativeCustomersWindow == null)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    HistoCumulativeCustomersWindow = new HistoCumulativeCustomers(AppType)
                    {
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.GetHistoCumulativeCustomers(EditedCustomer.Customer_Id),
                        Data = EditedCustomer
                    };
                    HistoCumulativeCustomersWindow.Closed += HistoCumulativeCustomersWindow_Closed;
                    HistoCumulativeCustomersWindow.ShowDialog();

                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, al carregar la finestra de l'Històric Acumulat de Clients.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
                finally
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
            else HistoCumulativeCustomersWindow.Activate();
        }

        private void HistoCumulativeCustomersWindow_Closed(object sender, EventArgs e)
        {
            HistoCumulativeCustomersWindow.Closed -= HistoCumulativeCustomersWindow_Closed;
            HistoCumulativeCustomersWindow = null;
        }

        #endregion

        #region Data Debt (Query data)

        private void BtnBadDebt_Click(object sender, RoutedEventArgs e)
        {
            if (BadDebtsWindow == null)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    BadDebtsWindow = new BadDebts(AppType)
                    {
                        Print = false,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.GetBadDebts(EditedCustomer.Customer_Id),
                        Data = EditedCustomer
                    };
                    BadDebtsWindow.Closed += BadDebtsWindow_Closed;
                    BadDebtsWindow.ShowDialog();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, al carregar la finestra de gestió d'Impagats.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
                finally
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
            else BadDebtsWindow.Activate();
        }

        private void BadDebtsWindow_Closed(object sender, EventArgs e)
        {
            BadDebtsWindow.Closed -= BadDebtsWindow_Closed;
            BadDebtsWindow = null;
        }

        #endregion

        #region Historic (Query data)

        private void BtnHistoric_Click(object sender, RoutedEventArgs e)
        {
            if (HistoCustomersWindow == null)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    HistoCustomersWindow = new HistoCustomers(AppType, HistoCustomersMode.Historic)
                    {
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.GetHistoCustomers(EditedCustomer.Customer_Id),
                        Data = EditedCustomer
                    };
                    HistoCustomersWindow.Closed += HistoCustomersWindow_Closed;
                    HistoCustomersWindow.Show();

                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, al carregar la finestra d'Històric.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
                finally
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
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

        #region Related Customers

        private void BtnRelatedCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (ListItemsDefinedCustomers.SelectedItems.Count > 0)
            {
                if (DataChangedManagerActive)
                {
                    DataChangedManagerActive = false;
                    ArrayList CustomersSelected = new ArrayList(ListItemsDefinedCustomers.SelectedItems);
                    ListItemsDefinedCustomers.SelectedItems.Clear();
                    ListItemsRelatedCustomers.SelectedItems.Clear();
                    foreach (CustomersView customer in CustomersSelected)
                    {
                        DataListDefinedCustomers.Remove(customer);
                        DataListRelatedCustomers.Add(customer);
                        ListItemsRelatedCustomers.SelectedItems.Add(customer);
                    }
                    ListItemsDefinedCustomers.UpdateLayout();
                    ListItemsRelatedCustomers.UpdateLayout();
                    UpdateRelatedCustomersControls();
                    AreDataChanged = EditedCustomer != Customer || ValidateChangesInRelatedCustomers();
                    DataChangedManagerActive = true;
                }
            }
        }

        private void BtnUnRelatedCustomer_Click(object sender, RoutedEventArgs e)
        {
            if (ListItemsRelatedCustomers.SelectedItems.Count > 0)
            {
                if (DataChangedManagerActive)
                {
                    DataChangedManagerActive = false;
                    ArrayList CustomersSelected = new ArrayList(ListItemsRelatedCustomers.SelectedItems);
                    ListItemsDefinedCustomers.SelectedItems.Clear();
                    ListItemsRelatedCustomers.SelectedItems.Clear();
                    foreach (CustomersView customer in CustomersSelected)
                    {
                        DataListRelatedCustomers.Remove(customer);
                        DataListDefinedCustomers.Add(customer);
                        ListItemsDefinedCustomers.SelectedItems.Add(customer);
                    }
                    ListItemsDefinedCustomers.UpdateLayout();
                    ListItemsRelatedCustomers.UpdateLayout();
                    UpdateRelatedCustomersControls();
                    AreDataChanged = EditedCustomer != Customer || ValidateChangesInRelatedCustomers();
                    DataChangedManagerActive = true;
                }
            }
        }

        #endregion

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

        #region ComboBox

        /// <summary>
        /// Manage the change of the Data in the combobox of CP.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CbCompanyPostalCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                if (cbCompanyPostalCode.SelectedItem != null)
                {
                    PostalCodesView citiesCPSelected = ((PostalCodesView)cbCompanyPostalCode.SelectedValue);
                    EditedCustomer.Company_PostalCode = citiesCPSelected;
                    AreDataChanged = EditedCustomer != Customer || ValidateChangesInRelatedCustomers();
                }
            }
        }

        /// <summary>
        /// Manage the change of the Data in the combobox of EffectTypes.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CbDataBankEffect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                if (cbDataBankEffect.SelectedItem != null)
                {
                    EffectTypesView effectTypeSelected = ((EffectTypesView)cbDataBankEffect.SelectedValue);
                    EditedCustomer.DataBank_EffectType = effectTypeSelected;
                    AreDataChanged = EditedCustomer != Customer || ValidateChangesInRelatedCustomers();
                }
            }
        }

        /// <summary>
        /// Manage the change of the Data in the combobox of SendTypes.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CbBillingDataSendByType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                if (cbBillingDataSendByType.SelectedItem != null)
                {
                    SendTypesView sendTypeSelected = ((SendTypesView)cbBillingDataSendByType.SelectedValue);
                    EditedCustomer.BillingData_SendType = sendTypeSelected;
                    AreDataChanged = EditedCustomer != Customer || ValidateChangesInRelatedCustomers();
                }
            }
        }

        /// <summary>
        /// Manage the change of the Data in the combobox of SendTypes.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CbBillingDataAgent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                if (cbBillingDataAgent.SelectedItem != null)
                {
                    AgentsView agentSelected = ((AgentsView)cbBillingDataAgent.SelectedValue);
                    EditedCustomer.BillingData_Agent = agentSelected;
                    AreDataChanged = EditedCustomer != Customer || ValidateChangesInRelatedCustomers();
                }
            }
        }

        /// <summary>
        /// Manage the change of the Data in the combobox of SendTypes.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CbBillingDataIVAType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                if (cbBillingDataIVAType.SelectedItem != null)
                {
                    IVATypesView ivaTypeSelected = ((IVATypesView)cbBillingDataIVAType.SelectedValue);
                    EditedCustomer.BillingData_IVAType = ivaTypeSelected;
                    AreDataChanged = EditedCustomer != Customer || ValidateChangesInRelatedCustomers();
                }
            }
        }

        #endregion

        #region CheckBox

        private void ChkbBillingDataValued_Unchecked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                EditedCustomer.BillingData_Valued = false;
                AreDataChanged = EditedCustomer != Customer || ValidateChangesInRelatedCustomers();
            }
        }

        private void ChkbBillingDataValued_Checked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                EditedCustomer.BillingData_Valued = true;
                AreDataChanged = EditedCustomer != Customer || ValidateChangesInRelatedCustomers();
            }
        }

        private void ChkbCanceled_Unchecked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                EditedCustomer.Canceled = false;
                AreDataChanged = EditedCustomer != Customer || ValidateChangesInRelatedCustomers();
                DataChangedManagerActive = true;
            }
        }

        private void ChkbCanceled_Checked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                EditedCustomer.Canceled = true;
                AreDataChanged = EditedCustomer != Customer || ValidateChangesInRelatedCustomers();
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region DatePicket

        private void DtpBillingDataRegister_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                EditedCustomer.BillingData_Register = (DateTime) dtpBillingDataRegister.SelectedDate;
                tbBillingDataRegister.Text = GlobalViewModel.GetLongDateString(EditedCustomer.BillingData_Register);
                AreDataChanged = EditedCustomer != Customer || ValidateChangesInRelatedCustomers();
            }
        }

        #endregion

        #region ListItems

        #endregion

        #endregion

        #region Update UI

        private void UpdateRelatedCustomersControls()
        {
            btnRelatedCustomer.Visibility = m_DataListDefinedCustomers.Count > 0 ? Visibility.Visible : Visibility.Hidden;
            btnUnRelatedCustomer.Visibility = m_DataListRelatedCustomers.Count > 0 ? Visibility.Visible : Visibility.Hidden;
        }

        private bool ValidateChangesInRelatedCustomers()
        {
            if (SourceDataListRelatedCustomers.Count != DataListRelatedCustomers.Count) return true;
            else
            {
                foreach (CustomersView customer in SourceDataListRelatedCustomers)
                {
                    if (!DataListRelatedCustomers.Contains(customer)) return true;
                }
                return false;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        public void RestoreSourceValues()
        {
            EditedCustomer.RestoreSourceValues(Customer);
            LoadDataInControls(EditedCustomer);
            AreDataChanged = EditedCustomer != Customer || ValidateChangesInRelatedCustomers();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(CustomersAttributes ErrorField)
        {
            EditedCustomer.RestoreSourceValue(Customer, ErrorField);
            LoadDataInControls(EditedCustomer);
            AreDataChanged = EditedCustomer != Customer || ValidateChangesInRelatedCustomers();
        }

        #endregion
    }
}
