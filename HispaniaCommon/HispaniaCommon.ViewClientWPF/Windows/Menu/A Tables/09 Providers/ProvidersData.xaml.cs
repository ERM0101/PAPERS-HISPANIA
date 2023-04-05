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
    /// Interaction logic for ProvidersData.xaml
    /// </summary>
    public partial class ProvidersData : UserControl
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
        /// Show the Accept button.
        /// </summary>
        private GridLength ViewAcceptButton = new GridLength(120.0);

        /// <summary>
        /// Show the middle column.
        /// </summary>
        private GridLength ViewMiddleColumn = new GridLength(1.0, GridUnitType.Star);

        /// <summary>
        /// Show the Comissions Button.
        /// </summary>
        private GridLength ViewComissionsButton = new GridLength(2.0, GridUnitType.Star);

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
        /// Provider is correct.
        /// </summary>
        /// <param name="NewOrEditedProvider">New or Edited Provider.</param>
        ///  /// <param name="RelatedProviders">Related Providers from the Provider.</param>
        public delegate void dlgAccept(ProvidersView NewOrEditedProvider, List<RelatedProvidersView> RelatedProviders);

        /// <summary>
        /// Delegate that defines the firm of event produced when the Button Cancel is pressed.
        /// </summary>
        public delegate void dlgCancel();

        #endregion

        #region Events

        /// <summary>
        /// Event produced when the Button Accept is pressed and the data of the Provider is correct.
        /// </summary>
        public event dlgAccept EvAccept;

        /// <summary>
        /// Event produced when the Button Cancel is pressed.
        /// </summary>
        public event dlgCancel EvCancel;

        #endregion

        #region Attributes

        /// <summary>
        /// Store the Provider data to manage.
        /// </summary>
        private ProvidersView m_Provider = null;

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
        /// Stotre if the data of the Provider has changed.
        /// </summary>
        private bool m_AreDataChanged;

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private ObservableCollection<ProvidersView> m_DataListDefinedProviders = new ObservableCollection<ProvidersView>();

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private ObservableCollection<ProvidersView> m_DataListRelatedProviders = new ObservableCollection<ProvidersView>();

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
        /// Store the Provider Orders Window Type Active.
        /// </summary>
        private ProviderOrders ProviderOrdersWindow = null;

        /// <summary>
        /// Store the Provider Orders Window Type Active.
        /// </summary>
        private ProviderOrders ProviderOrders2Window = null;

        /// <summary>
        /// Window instance of HistoCumulativeProviders.
        /// </summary>
        private HistoCumulativeProviders HistoCumulativeProvidersWindow = null;

        /// <summary>
        /// Window instance of BadDebt.
        /// </summary>
        private BadDebts BadDebtsWindow = null;

        /// <summary>
        /// Window instance of HistoProviders.
        /// </summary>
        private HistoProviders HistoProvidersWindow = null;

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
        /// Get or Set the Provider data to manage.
        /// </summary>
        public ProvidersView Provider
        {
            get
            {
                return (m_Provider);
            }
            set
            {
                if (value != null)
                {
                    AreDataChanged = false;
                    m_Provider = value;
                    EditedProvider = new ProvidersView(m_Provider);
                    LoadDataInControls(m_Provider, true);
                }
                else throw new ArgumentNullException("Error, no s'han trobat les dades del Proveïdor a carregar."); 
            }
        }

        /// <summary>
        /// Get or Set the data to show in List of Items.
        /// </summary>
        public ObservableCollection<ProvidersView> DataListDefinedProviders
        {
            get
            {
                return (m_DataListDefinedProviders);
            }
            set
            {
                if (value != null) m_DataListDefinedProviders = new ObservableCollection<ProvidersView>(value);
                else m_DataListDefinedProviders = new ObservableCollection<ProvidersView>();
                ListItemsDefinedProviders.ItemsSource = m_DataListDefinedProviders;
                ListItemsDefinedProviders.DataContext = this;
                CollectionViewSource.GetDefaultView(ListItemsDefinedProviders.ItemsSource).SortDescriptions.Add(new SortDescription("Provider_Id", ListSortDirection.Descending));
                CollectionViewSource.GetDefaultView(ListItemsDefinedProviders.ItemsSource).Filter = UserFilterDefinedProviders;
                UpdateRelatedProvidersControls();
            }
        }

        /// <summary>
        /// Get or Set the data to show in List of Items.
        /// </summary>
        public ObservableCollection<ProvidersView> DataListRelatedProviders
        {
            get
            {
                return (m_DataListRelatedProviders);
            }
            set
            {
                if (value != null) m_DataListRelatedProviders = new ObservableCollection<ProvidersView>(value);
                else m_DataListRelatedProviders = new ObservableCollection<ProvidersView>();
                SourceDataListRelatedProviders = new ObservableCollection<ProvidersView>(m_DataListRelatedProviders);
                ListItemsRelatedProviders.ItemsSource = m_DataListRelatedProviders;
                ListItemsRelatedProviders.DataContext = this;
                CollectionViewSource.GetDefaultView(ListItemsRelatedProviders.ItemsSource).SortDescriptions.Add(new SortDescription("Provider_Id", ListSortDirection.Descending));
                CollectionViewSource.GetDefaultView(ListItemsRelatedProviders.ItemsSource).Filter = UserFilterRelatedProviders;
                UpdateRelatedProvidersControls();
            }
        }

        /// <summary>
        /// Get or Set the Edited Provider information.
        /// </summary>
        private ProvidersView EditedProvider
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set the data to show in List of Items.
        /// </summary>
        private ObservableCollection<ProvidersView> SourceDataListRelatedProviders
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
                         if (Provider == null) throw new InvalidOperationException("Error, impossible visualitzar un Proveïdor sense dades.");
                         cbMiddlePanel.Width = HideAcceptButton;
                         tiConsullation.Visibility = Visibility.Visible;
                         gbSeveralDataAcum.Visibility = Visibility.Visible;
                         cbHistoricButton.Width = ViewHistoricButton;
                         tbDataBankIBANCountryCode.ContextMenu = null;
                         tbCancel.Text = "Tornar";
                         break;
                    case Operation.Add:
                        ProvidersView NewProvider = new ProvidersView();
                         //if (Provider == null) throw new InvalidOperationException("Error, impossible realitzar l'alta d'un Proveïdor sense Identificador.");
                         cbMiddlePanel.Width = ViewAcceptButton;
                         NewProvider.Provider_Id = GlobalViewModel.Instance.HispaniaViewModel.GetNextIdentityValueTable(NewProvider);
                         Provider = NewProvider;
                         tiConsullation.Visibility = Visibility.Hidden;
                         cbHistoricButton.Width = HideHistoricButton;
                         gbSeveralDataAcum.Visibility = Visibility.Hidden;
                         tbDataBankIBANCountryCode.ContextMenu = ctxmnuIBAN_Initial;
                         tbCancel.Text = "Cancel·lar";
                         break;
                    case Operation.Edit:
                         if (Provider == null) throw new InvalidOperationException("Error, impossible editar un Representant sense dades.");
                         tiConsullation.Visibility = Visibility.Visible;
                         cbHistoricButton.Width = ViewHistoricButton;
                         gbSeveralDataAcum.Visibility = Visibility.Visible;
                         tbDataBankIBANCountryCode.ContextMenu = ctxmnuIBAN_Initial;
                         cbMiddlePanel.Width = ViewAcceptButton;
                         tbCancel.Text = "Cancel·lar";
                         break;
                }
                string properyValue;
                foreach (Control control in EditableControls)
                {
                    if (control is TextBox) ((TextBox)control).IsReadOnly = (m_CtrlOperation == Operation.Show);
                    else if (control is RichTextBox) ((RichTextBox)control).IsReadOnly = (m_CtrlOperation == Operation.Show);
                    //else if (control is GroupBox)
                    //{
                    //    ((GroupBox)control).SetResourceReference(Control.StyleProperty,
                    //                                             ((m_CtrlOperation == Operation.Show) ? "NonEditableGroupBox" : "EditableGroupBox"));
                    //}
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
        /// Get or Set if the data of the Provider has changed.
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
        /// Get or Set if the manager of the data change for the Provider has active.
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
                    cbProviderPostalCode.ItemsSource = m_PostalCodes;
                    cbProviderPostalCode.DisplayMemberPath = "Key";
                    cbProviderPostalCode.SelectedValuePath = "Value";
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
                    cbProviderDataAgent.ItemsSource = m_Agents;
                    cbProviderDataAgent.DisplayMemberPath = "Key";
                    cbProviderDataAgent.SelectedValuePath = "Value";
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
        public ProvidersData()
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
                lblProviderAgentPhone,
                tbProviderAgentPhone,
                lblProviderAgentFax,
                tbProviderAgentFax,                
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
                lblProviderNumber,
                tbProviderNumber,
                lblProviderName,
                tbProviderName,
                lblProviderAlias,
                tbProviderAlias,
                lblProviderNIF,
                tbProviderNIF,
                lblProviderAddress,
                tbProviderAddress,
                lblProviderPostalCode,
                cbProviderPostalCode,
                lblProviderPhone,
                tbProviderPhone,
                lblProviderMobilePhone,
                tbProviderMobilePhone,
                lblProviderFax,
                tbProviderFax,
                lblProviderEmail,
                tbProviderEmail,
                lblProviderAgent,
                cbProviderDataAgent,
                lblProviderAgentPhone,
                tbProviderAgentPhone,
                lblProviderAgentFax,
                tbProviderAgentFax,
                lblProviderAcountAcounting,
                tbProviderAcountAcounting,
                lblProviderTransferPercent,
                tbProviderTransferPercent,
                lblProviderPromptPaymentDiscount,
                tbProviderPromptPaymentDiscount,
                lblProviderAdditionalDiscount,
                tbProviderAdditionalDiscount,
                lblProviderComment,
                tbProviderComment,
                lblCanceled,
                chkbCanceled,
                gbAgent,
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
                tiRelatedProvider,
                gbItemsListDefinedProviders,
                gbItemsListRelatedProviders,
                lblCanceled,
                chkbCanceled,
                btnRelatedProvider,
                btnUnRelatedProvider,
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
                btnPendentProviderOrders,
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
        /// <param name="providersView">Data Container.</param>
        /// <param name="ThrowException">true, if want throw an exception if not found a component</param>
        private void LoadDataInControls(ProvidersView providersView, bool ThrowException = false)
        {
            DataChangedManagerActive = false;
            tbProviderNumber.Text = GlobalViewModel.GetStringFromIntValue(providersView.Provider_Number);
            chkbCanceled.IsChecked = providersView.Canceled;
            tbProviderName.Text = providersView.Name;
            tbProviderAlias.Text = providersView.Alias;
            tbProviderNIF.Text = providersView.NIF;
            tbProviderAddress.Text = providersView.Address;
            tbProviderPhone.Text = providersView.Phone;
            tbProviderMobilePhone.Text = providersView.MobilePhone;
            tbProviderEmail.Text = providersView.EMail;
            tbProviderFax.Text = providersView.Fax;
            tbProviderAcountAcounting.Text = providersView.AcountAcounting;
            tbProviderTransferPercent.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.TransferPercent, DecimalType.Percent);
            tbProviderPromptPaymentDiscount.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.PromptPaymentDiscount, DecimalType.Percent);
            tbProviderAdditionalDiscount.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.AdditionalDiscount, DecimalType.Percent);
            tbProviderComment.Text = providersView.Comment;
            //  DataBank Controls
            tbDataBankNumEffect.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.DataBank_NumEffect, DecimalType.WithoutDecimals);
            tbDataBankFirstExpirationData.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.DataBank_FirstExpirationData, DecimalType.WithoutDecimals);
            tbDataBankExpirationInterval.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.DataBank_ExpirationInterval, DecimalType.WithoutDecimals);
            tbDataBankPayday_1.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.DataBank_Payday_1, DecimalType.WithoutDecimals);
            tbDataBankPayday_2.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.DataBank_Payday_2, DecimalType.WithoutDecimals);
            tbDataBankPayday_3.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.DataBank_Payday_3, DecimalType.WithoutDecimals);
            tbDataBankBank.Text = providersView.DataBank_Bank;
            tbDataBankBankAddress.Text = providersView.DataBank_BankAddress;
            tbDataBankIBANCountryCode.Text = providersView.DataBank_IBAN_CountryCode;
            tbDataBankIBANBankCode.Text = providersView.DataBank_IBAN_BankCode;
            tbDataBankIBANOfficeCode.Text = providersView.DataBank_IBAN_OfficeCode;
            tbDataBankIBANCheckDigits.Text = providersView.DataBank_IBAN_CheckDigits;
            tbDataBankIBANAccountNumber.Text = providersView.DataBank_IBAN_AccountNumber;
            //  BillingData
            tbCompanyCif.Text = providersView.Company_Cif;
            tbBillingDataBillingType.Text = providersView.BillingData_BillingType;
            tbBillingDataDuplicate.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.BillingData_Duplicate, DecimalType.WithoutDecimals);
            tbBillingDataEarlyPaymentDiscount.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.BillingData_EarlyPaymentDiscount, DecimalType.Percent);
            chkbBillingDataValued.IsChecked = providersView.BillingData_Valued;
            tbBillingDataRiskGranted.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.BillingData_RiskGranted, DecimalType.Currency);
            tbBillingDataCurrentRisk.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.BillingData_CurrentRisk, DecimalType.Currency);
            tbBillingDataUnpaid.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.BillingData_Unpaid, DecimalType.Currency);
            tbBillingDataNumUnpaid.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.BillingData_NumUnpaid, DecimalType.WithoutDecimals);
            dtpBillingDataRegister.SelectedDate = providersView.BillingData_Register;
            if (dtpBillingDataRegister.SelectedDate != null) tbBillingDataRegister.Text = GlobalViewModel.GetLongDateString(providersView.BillingData_Register);
            LoadExternalTablesInfo(providersView, ThrowException);
            //  Several Remarks
            tbSeveralRemarks.Text = providersView.Several_Remarks;
            tbSeveralDataAcum_1.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.SeveralData_Acum_1, DecimalType.Currency);
            tbSeveralDataAcum_2.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.SeveralData_Acum_2, DecimalType.Currency);
            tbSeveralDataAcum_3.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.SeveralData_Acum_3, DecimalType.Currency);
            tbSeveralDataAcum_4.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.SeveralData_Acum_4, DecimalType.Currency);
            tbSeveralDataAcum_5.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.SeveralData_Acum_5, DecimalType.Currency);
            tbSeveralDataAcum_6.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.SeveralData_Acum_6, DecimalType.Currency);
            tbSeveralDataAcum_7.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.SeveralData_Acum_7, DecimalType.Currency);
            tbSeveralDataAcum_8.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.SeveralData_Acum_8, DecimalType.Currency);
            tbSeveralDataAcum_9.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.SeveralData_Acum_9, DecimalType.Currency);
            tbSeveralDataAcum_10.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.SeveralData_Acum_10, DecimalType.Currency);
            tbSeveralDataAcum_11.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.SeveralData_Acum_11, DecimalType.Currency);
            tbSeveralDataAcum_12.Text = GlobalViewModel.GetStringFromDecimalValue(providersView.SeveralData_Acum_12, DecimalType.Currency);

            LoadExternalTablesInfo(providersView, ThrowException);
            DataChangedManagerActive = true;
        }

        /// <summary>
        /// Load Data from External Tables.
        /// </summary>
        /// <param name="providersView">Data Container.</param>
        /// <param name="ThrowException">true, if want throw an exception if not found a component</param>
        private void LoadExternalTablesInfo(ProvidersView providersView, bool ThrowException = false)
        {
            if ((PostalCodes != null) && (providersView.PostalCode != null))
            {
                Dictionary<string, PostalCodesView> Items = (Dictionary<string, PostalCodesView>)cbProviderPostalCode.ItemsSource;
                string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeyPostalCodeView(providersView.PostalCode);
                if (Items.ContainsKey(Key)) cbProviderPostalCode.SelectedValue = PostalCodes[Key];
                else
                {
                    if (ThrowException)
                    {
                        throw new Exception(string.Format("No s'ha trobat el Còdi Postal '{0}'.", PostalCodes[Key].Postal_Code_Info));
                    }
                }
            }
            else cbProviderPostalCode.SelectedIndex = -1;
            if ((EffectTypes != null) && (providersView.DataBank_EffectType != null))
            {
                Dictionary<string, EffectTypesView> Items = (Dictionary<string, EffectTypesView>)cbDataBankEffect.ItemsSource;
                string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeyEffectTypeView(providersView.DataBank_EffectType);
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
            if ((SendTypes != null) && (providersView.BillingData_SendType != null))
            {
                Dictionary<string, SendTypesView> Items = (Dictionary<string, SendTypesView>)cbBillingDataSendByType.ItemsSource;
                string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeySendTypeView(providersView.BillingData_SendType);
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
            if ((Agents != null) && (providersView.Data_Agent != null))
            {
                Dictionary<string, AgentsView> Items = (Dictionary<string, AgentsView>)cbProviderDataAgent.ItemsSource;
                string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeyAgentView(providersView.Data_Agent);
                if (Items.ContainsKey(Key))
                {
                    cbProviderDataAgent.SelectedValue = Agents[Key];
                    tbProviderAgentPhone.Text = Agents[Key].Phone;
                    tbProviderAgentFax.Text = Agents[Key].Fax;
                }
                else
                {
                    if (ThrowException)
                    {
                        throw new Exception(string.Format("No s'ha trobat el Representant '{0}'.", Agents[Key].Name));
                    }
                }
            }
            else cbProviderDataAgent.SelectedIndex = -1;
            if ((IVATypes != null) && (providersView.BillingData_IVAType != null))
            {
                Dictionary<string, IVATypesView> Items = (Dictionary<string, IVATypesView>)cbBillingDataIVAType.ItemsSource;
                string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeyIVATypeView(providersView.BillingData_IVAType);
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
        private bool UserFilterDefinedProviders(object item)
        {
            //  Don't do anything if Provider doesn't have any value.
            if (Provider is null) return true;
            //  Get Acces to the object and the property name To Filter.
            ProvidersView ItemData = (ProvidersView)item;
            //  If the item is the Provider don't show this item.
            if (ItemData.Provider_Id == Provider.Provider_Id) return false;
            //  Calculate the Visibility value with properties values.
            return (DataListRelatedProviders is null) || (!m_DataListRelatedProviders.Contains(ItemData));
        }

        /// <summary>
        /// Method that filter the elements that are showing in the list
        /// </summary>
        /// <param name="item">Item to test</param>
        /// <returns>true, if the item must be loaded, false, if not.</returns>
        private bool UserFilterRelatedProviders(object item)
        {
            //  Get Acces to the object and the property name To Filter.
            ProvidersView ItemData = (ProvidersView)item;
            //  If the item is the Provider don't show this item.
            if (ItemData.Provider_Id == Provider.Provider_Id) return false;
            //  Calculate the Visibility value with properties values.
            return (DataListDefinedProviders is null) || (!m_DataListDefinedProviders.Contains(ItemData));
        }

        #endregion

        #region Managers

        /// <summary>
        /// Method that define the managers needed for the user operations in the UserControl
        /// </summary>
        private void LoadManagers()
        {
            //  By default the manager for the Provider Data changes is active.
            DataChangedManagerActive = true;
            //  TextBox
                tbProviderNumber.TextChanged += TBDataChanged;
                tbProviderNumber.PreviewTextInput += TBPreviewTextInput;
                tbProviderName.TextChanged += TBDataChanged;
                tbProviderName.PreviewTextInput += TBPreviewTextInput;
                tbProviderAlias.TextChanged += TBDataChanged;
                tbProviderAlias.PreviewTextInput += TBPreviewTextInput;
                tbProviderNIF.TextChanged += TBDataChanged;
                tbProviderNIF.PreviewTextInput += TBPreviewTextInput;
                tbProviderAddress.TextChanged += TBDataChanged;
                tbProviderAddress.PreviewTextInput += TBPreviewTextInput;
                tbProviderPhone.TextChanged += TBDataChanged;
                tbProviderPhone.PreviewTextInput += TBPreviewTextInput;
                tbProviderMobilePhone.TextChanged += TBDataChanged;
                tbProviderMobilePhone.PreviewTextInput += TBPreviewTextInput;
                tbProviderEmail.TextChanged += TBDataChanged;
                tbProviderEmail.PreviewTextInput += TBPreviewTextInput;
                tbProviderFax.TextChanged += TBDataChanged;
                tbProviderFax.PreviewTextInput += TBPreviewTextInput;
                tbProviderAcountAcounting.TextChanged += TBDataChanged;
                tbProviderAcountAcounting.PreviewTextInput += TBPreviewTextInput;
                tbProviderTransferPercent.TextChanged += TBPercentDataChanged;
                tbProviderTransferPercent.PreviewTextInput += TBPreviewTextInput;
                tbProviderPromptPaymentDiscount.TextChanged += TBPercentDataChanged;
                tbProviderPromptPaymentDiscount.PreviewTextInput += TBPreviewTextInput;
                tbProviderAdditionalDiscount.TextChanged += TBPercentDataChanged;
                tbProviderAdditionalDiscount.PreviewTextInput += TBPreviewTextInput;
                tbProviderComment.TextChanged += TBDataChanged;
                tbProviderComment.PreviewTextInput += TBPreviewTextInput;
            tbCompanyEmail2.PreviewTextInput += TBPreviewTextInput;
            tbCompanyEmail2.TextChanged += TBDataChanged;
            tbCompanyEmail3.PreviewTextInput += TBPreviewTextInput;
            tbCompanyEmail3.TextChanged += TBDataChanged;
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
                cbProviderPostalCode.SelectionChanged += CbProviderPostalCode_SelectionChanged;
            cbDataBankEffect.SelectionChanged += CbDataBankEffect_SelectionChanged;
            cbBillingDataSendByType.SelectionChanged += CbBillingDataSendByType_SelectionChanged;
            cbProviderDataAgent.SelectionChanged += CbProviderDataAgent_SelectionChanged;
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
            btnPendentProviderOrders.Click += BtnPendentProviderOrders_Click;
            btnPendentDeliveryNotes.Click += BtnPendentDeliveryNotes_Click;
            btnRelatedProvider.Click += BtnRelatedProvider_Click;
            btnUnRelatedProvider.Click += BtnUnRelatedProvider_Click;
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

        /// Checking the Input Char in function of the data type that has associated
        /// </summary>
        private void TBPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if ((sender == tbProviderName) || (sender == tbProviderAlias) || (sender == tbProviderComment))
            {
                e.Handled = ! GlobalViewModel.IsValidNameChar(e.Text);
            }
            else if (sender == tbProviderNumber) e.Handled = !GlobalViewModel.IsValidNumericChar(e.Text);
            else if (sender == tbProviderNIF) e.Handled = !GlobalViewModel.IsValidCIFChar(e.Text);
            else if (sender == tbProviderAddress) e.Handled = ! GlobalViewModel.IsValidAddressChar(e.Text);
            else if ((sender == tbProviderPhone) || (sender == tbProviderFax)) e.Handled = ! GlobalViewModel.IsValidPhoneNumberChar(e.Text);
            else if (sender == tbProviderMobilePhone) e.Handled = ! GlobalViewModel.IsValidMobilePhoneNumberChar(e.Text);
            else if (sender == tbProviderEmail) e.Handled = ! GlobalViewModel.IsValidEmailChar(e.Text);
            else if (sender == tbProviderAcountAcounting) e.Handled = ! GlobalViewModel.IsNumeric(e.Text);
            else if ((sender == tbProviderTransferPercent) || (sender == tbProviderPromptPaymentDiscount) ||
                     (sender == tbProviderAdditionalDiscount))
            {
                e.Handled = ! GlobalViewModel.IsValidPercentChar(e.Text);
            }
            else if ((sender == tbDataBankNumEffect) || (sender == tbDataBankFirstExpirationData) ||
                     (sender == tbDataBankExpirationInterval) || (sender == tbDataBankPayday_1) ||
                     (sender == tbDataBankPayday_2) || (sender == tbDataBankPayday_3) ||
                     (sender == tbBillingDataBillingType) || (sender == tbBillingDataDuplicate))
            {
                e.Handled = !GlobalViewModel.IsValidShortDecimalChar(e.Text);
            }
            //else if (sender == tbBillingDataNumUnpaid) e.Handled = ! GlobalViewModel.IsValidShortDecimalChar(e.Text);
            else if (sender == tbDataBankIBANCountryCode) e.Handled = !GlobalViewModel.IsValidIBAN_CountryCodeChar(e.Text);
            else if (sender == tbDataBankIBANBankCode) e.Handled = !GlobalViewModel.IsValidIBAN_BankCodeChar(e.Text);
            else if (sender == tbDataBankIBANOfficeCode) e.Handled = !GlobalViewModel.IsValidIBAN_OfficeCodeChar(e.Text);
            else if (sender == tbDataBankIBANCheckDigits) e.Handled = !GlobalViewModel.IsValidIBAN_CheckDigitsChar(e.Text);
            else if (sender == tbDataBankIBANAccountNumber) e.Handled = !GlobalViewModel.IsValidIBAN_AccountNumberChar(e.Text);
            //else if ((sender == tbBillingDataRiskGranted) || (sender == tbBillingDataUnpaid) || (sender == tbBillingDataCurrentRisk)) 
            //{
            //    e.Handled = ! GlobalViewModel.IsValidCurrencyChar(e.Text);
            //} 
            else if (sender == tbBillingDataEarlyPaymentDiscount) e.Handled = !GlobalViewModel.IsValidPercentChar(e.Text);
            else if (sender == tbSeveralRemarks) e.Handled = !GlobalViewModel.IsValidCommentChar(e.Text);

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
                    if (sender == tbProviderNumber) EditedProvider.Provider_Number = GlobalViewModel.GetIntValue(value);
                    else if (sender == tbProviderNIF) EditedProvider.NIF = value;
                    else if (sender == tbProviderName) EditedProvider.Name = value;
                    else if (sender == tbProviderAlias) EditedProvider.Alias = value;
                    else if (sender == tbProviderAddress) EditedProvider.Address = value;
                    else if (sender == tbProviderPhone) EditedProvider.Phone = value;
                    else if (sender == tbProviderMobilePhone) EditedProvider.MobilePhone = value;
                    else if (sender == tbProviderEmail) EditedProvider.EMail = value;
                    else if (sender == tbProviderFax) EditedProvider.Fax = value;
                    else if (sender == tbProviderAcountAcounting) EditedProvider.AcountAcounting = value;
                    else if (sender == tbProviderComment) EditedProvider.Comment = value;
                    else if (sender == tbDataBankBank) EditedProvider.DataBank_Bank = value;
                    else if (sender == tbDataBankBankAddress) EditedProvider.DataBank_BankAddress = value;
                    else if (sender == tbDataBankIBANCountryCode) EditedProvider.DataBank_IBAN_CountryCode = value;
                    else if (sender == tbDataBankIBANBankCode) EditedProvider.DataBank_IBAN_BankCode = value;
                    else if (sender == tbDataBankIBANOfficeCode) EditedProvider.DataBank_IBAN_OfficeCode = value;
                    else if (sender == tbDataBankIBANCheckDigits) EditedProvider.DataBank_IBAN_CheckDigits = value;
                    else if (sender == tbDataBankIBANAccountNumber) EditedProvider.DataBank_IBAN_AccountNumber = value;
                    else if (sender == tbBillingDataBillingType) EditedProvider.BillingData_BillingType = value;
                    else if (sender == tbSeveralRemarks) EditedProvider.Several_Remarks = value;

                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedProvider);
                }
                AreDataChanged = EditedProvider != Provider || ValidateChangesInRelatedProviders(); ;
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
                    if (sender == tbDataBankNumEffect) EditedProvider.DataBank_NumEffect = value;
                    else if (sender == tbDataBankFirstExpirationData) EditedProvider.DataBank_FirstExpirationData = value;
                    else if (sender == tbDataBankExpirationInterval) EditedProvider.DataBank_ExpirationInterval = value;
                    else if (sender == tbDataBankPayday_1) EditedProvider.DataBank_Payday_1 = value;
                    else if (sender == tbDataBankPayday_2) EditedProvider.DataBank_Payday_2 = value;
                    else if (sender == tbDataBankPayday_3) EditedProvider.DataBank_Payday_3 = value;
                    else if (sender == tbBillingDataDuplicate) EditedProvider.BillingData_Duplicate = value;
                    //else if (sender == tbBillingDataNumUnpaid) EditedProvider.BillingData_NumUnpaid = value;
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedProvider);
                }
                AreDataChanged = EditedProvider != Provider || ValidateChangesInRelatedProviders();
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
                    if (sender == tbProviderTransferPercent) EditedProvider.TransferPercent = value;
                    else if (sender == tbProviderPromptPaymentDiscount) EditedProvider.PromptPaymentDiscount = value;
                    else if (sender == tbProviderAdditionalDiscount) EditedProvider.AdditionalDiscount = value;
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedProvider);
                }
                AreDataChanged = (EditedProvider != Provider);
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
                    //if (sender == tbBillingDataRiskGranted) EditedProvider.BillingData_RiskGranted = value;
                    //else if (sender == tbBillingDataCurrentRisk) EditedProvider.BillingData_CurrentRisk = value;
                    //else if (sender == tbBillingDataUnpaid) EditedProvider.BillingData_Unpaid = value;
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedProvider);
                }
                AreDataChanged = EditedProvider != Provider || ValidateChangesInRelatedProviders();
                DataChangedManagerActive = true;
            }
        }


        #endregion

        #region Button

        #region Accept

        /// <summary>
        /// Accept the edition or creatin of the Provider.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {
            ProvidersAttributes ErrorField = ProvidersAttributes.None;
            try
            {
                if ((CtrlOperation == Operation.Add) || (CtrlOperation == Operation.Edit))
                {
                    EditedProvider.Validate(out ErrorField);
                    List<RelatedProvidersView> RelatedProviders = new List<RelatedProvidersView>();
                    foreach (ProvidersView relatedProvider in DataListRelatedProviders)
                    {
                        RelatedProviders.Add(new RelatedProvidersView()
                        {
                            Provider_Id = Provider.Provider_Id,
                            Provider_Canceled_Id = relatedProvider.Provider_Id,
                            Remarks = string.Empty
                        });
                    }
                    EvAccept?.Invoke(new ProvidersView(EditedProvider), RelatedProviders);
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
        /// Cancel the edition or creatin of the Provider.
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
        /// Defines a new Agent for the Provider.
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
        /// When the Provider Window is closed we actualize the AgentsWindow attribute value.
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
            if (ProviderOrdersWindow == null)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.DeliveryNotesWindow);
                    ProviderOrdersWindow = new ProviderOrders(AppType, false, false, true)
                    {
                        FilterByProvider = true,
                        Provider = Provider,
                        Providers = GlobalViewModel.Instance.HispaniaViewModel.ProvidersActiveDict,
                        SendTypes = GlobalViewModel.Instance.HispaniaViewModel.SendTypesDict,
                        EffectTypes = GlobalViewModel.Instance.HispaniaViewModel.EffectTypesDict,
                        Agents = GlobalViewModel.Instance.HispaniaViewModel.AgentsDict,
                        Parameters = GlobalViewModel.Instance.HispaniaViewModel.Parameters,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.ProviderOrders
                    };
                    ProviderOrdersWindow.Closed += ProviderOrdersWindow_Closed;
                    ProviderOrdersWindow.Show();

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
            else ProviderOrdersWindow.Activate();
        }

        /// <summary>
        /// When the Provider Window is closed we actualize the ProvidersWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void ProviderOrdersWindow_Closed(object sender, EventArgs e)
        {
            ProviderOrdersWindow.Closed -= ProviderOrdersWindow_Closed;
            ProviderOrdersWindow = null;
        }

        #endregion

        #region Provider Orders (Update data)

        private void BtnPendentProviderOrders_Click(object sender, RoutedEventArgs e)
        {
            if (ProviderOrders2Window == null)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.ProviderOrdersWindow);
                    ProviderOrders2Window = new ProviderOrders(AppType, false, true, false)
                    {
                        FilterByProvider = true,
                        Provider = Provider,
                        Providers = GlobalViewModel.Instance.HispaniaViewModel.ProvidersActiveDict,
                        SendTypes = GlobalViewModel.Instance.HispaniaViewModel.SendTypesDict,
                        EffectTypes = GlobalViewModel.Instance.HispaniaViewModel.EffectTypesDict,
                        Agents = GlobalViewModel.Instance.HispaniaViewModel.AgentsDict,
                        Parameters = GlobalViewModel.Instance.HispaniaViewModel.Parameters,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.ProviderOrders
                    };
                    ProviderOrders2Window.Closed += ProviderOrders2Window_Closed;
                    ProviderOrders2Window.Show();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, al carregar la finestra de gestió de Comandes de Proveidor.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
                finally
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
            else ProviderOrders2Window.Activate();
        }

        /// <summary>
        /// When the Provider Window is closed we actualize the ProvidersWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void ProviderOrders2Window_Closed(object sender, EventArgs e)
        {
            ProviderOrders2Window.Closed -= ProviderOrders2Window_Closed;
            ProviderOrders2Window = null;
        }

        #endregion

        #region HistoricAcum (Query data)

        private void BtnHistoricAcum_Click(object sender, RoutedEventArgs e)
        {
            if (HistoCumulativeProvidersWindow == null)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    HistoCumulativeProvidersWindow = new HistoCumulativeProviders(AppType)
                    {
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.GetHistoCumulativeProviders(EditedProvider.Provider_Id),
                        Data = EditedProvider
                    };
                    HistoCumulativeProvidersWindow.Closed += HistoCumulativeProvidersWindow_Closed;
                    HistoCumulativeProvidersWindow.ShowDialog();

                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, al carregar la finestra de l'Històric Acumulat de Proveidors.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
                finally
                {
                    Mouse.OverrideCursor = Cursors.Arrow;
                }
            }
            else HistoCumulativeProvidersWindow.Activate();
        }

        private void HistoCumulativeProvidersWindow_Closed(object sender, EventArgs e)
        {
            HistoCumulativeProvidersWindow.Closed -= HistoCumulativeProvidersWindow_Closed;
            HistoCumulativeProvidersWindow = null;
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
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.GetBadDebts(EditedProvider.Provider_Id),
                        DataProv = EditedProvider
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
            if (HistoProvidersWindow == null)
            {
                Mouse.OverrideCursor = Cursors.Wait;
                try
                {
                    HistoProvidersWindow = new HistoProviders(AppType, HistoProvidersMode.Historic)
                    {
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.GetHistoProviders(EditedProvider.Provider_Id),
                        Data = EditedProvider
                    };
                    HistoProvidersWindow.Closed += HistoProvidersWindow_Closed;
                    HistoProvidersWindow.Show();

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
            else HistoProvidersWindow.Activate();
        }

        private void HistoProvidersWindow_Closed(object sender, EventArgs e)
        {
            HistoProvidersWindow.Closed -= HistoProvidersWindow_Closed;
            HistoProvidersWindow = null;
        }

        #endregion

        #region Related Provider

        private void BtnRelatedProvider_Click(object sender, RoutedEventArgs e)
        {
            if (ListItemsDefinedProviders.SelectedItems.Count > 0)
            {
                if (DataChangedManagerActive)
                {
                    DataChangedManagerActive = false;
                    ArrayList ProvidersSelected = new ArrayList(ListItemsDefinedProviders.SelectedItems);
                    ListItemsDefinedProviders.SelectedItems.Clear();
                    ListItemsRelatedProviders.SelectedItems.Clear();
                    foreach (ProvidersView provider in ProvidersSelected)
                    {
                        DataListDefinedProviders.Remove(provider);
                        DataListRelatedProviders.Add(provider);
                        ListItemsRelatedProviders.SelectedItems.Add(provider);
                    }
                    ListItemsDefinedProviders.UpdateLayout();
                    ListItemsRelatedProviders.UpdateLayout();
                    UpdateRelatedProvidersControls();
                    AreDataChanged = EditedProvider != Provider || ValidateChangesInRelatedProviders();
                    DataChangedManagerActive = true;
                }
            }
        }

        private void BtnUnRelatedProvider_Click(object sender, RoutedEventArgs e)
        {
            if (ListItemsRelatedProviders.SelectedItems.Count > 0)
            {
                if (DataChangedManagerActive)
                {
                    DataChangedManagerActive = false;
                    ArrayList ProvidersSelected = new ArrayList(ListItemsRelatedProviders.SelectedItems);
                    ListItemsDefinedProviders.SelectedItems.Clear();
                    ListItemsRelatedProviders.SelectedItems.Clear();
                    foreach (ProvidersView provider in ProvidersSelected)
                    {
                        DataListRelatedProviders.Remove(provider);
                        DataListDefinedProviders.Add(provider);
                        ListItemsDefinedProviders.SelectedItems.Add(provider);
                    }
                    ListItemsDefinedProviders.UpdateLayout();
                    ListItemsRelatedProviders.UpdateLayout();
                    UpdateRelatedProvidersControls();
                    AreDataChanged = EditedProvider != Provider || ValidateChangesInRelatedProviders();
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
        private void CbProviderPostalCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbProviderPostalCode.SelectedItem != null)
            {
                if (DataChangedManagerActive)
                {
                    DataChangedManagerActive = false;
                    PostalCodesView PostalCodeSelected = ((PostalCodesView)cbProviderPostalCode.SelectedValue);
                    EditedProvider.PostalCode = PostalCodeSelected;
                    AreDataChanged = (EditedProvider != Provider) || ValidateChangesInRelatedProviders();
                    DataChangedManagerActive = true;
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
                    EditedProvider.DataBank_EffectType = effectTypeSelected;
                    AreDataChanged = EditedProvider != Provider || ValidateChangesInRelatedProviders();
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
                    EditedProvider.BillingData_SendType = sendTypeSelected;
                    AreDataChanged = EditedProvider != Provider || ValidateChangesInRelatedProviders();
                }
            }
        }

        /// <summary>
        /// Manage the change of the Data in the combobox of SendTypes.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CbProviderDataAgent_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbProviderDataAgent.SelectedItem != null)
            {
                if (DataChangedManagerActive)
                {
                    DataChangedManagerActive = false;
                    AgentsView agentSelected = ((AgentsView)cbProviderDataAgent.SelectedValue);
                    EditedProvider.Data_Agent = agentSelected;
                    tbProviderAgentPhone.Text = agentSelected.Phone;
                    tbProviderAgentFax.Text = agentSelected.Fax;
                    AreDataChanged = (EditedProvider != Provider);
                    DataChangedManagerActive = true;
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
                    EditedProvider.BillingData_IVAType = ivaTypeSelected;
                    AreDataChanged = EditedProvider != Provider || ValidateChangesInRelatedProviders();
                }
            }
        }

        #endregion

        #region CheckBox
        private void ChkbBillingDataValued_Unchecked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                EditedProvider.BillingData_Valued = false;
                AreDataChanged = EditedProvider != Provider || ValidateChangesInRelatedProviders();
            }
        }

        private void ChkbBillingDataValued_Checked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                EditedProvider.BillingData_Valued = true;
                AreDataChanged = EditedProvider != Provider || ValidateChangesInRelatedProviders();
            }
        }


        private void ChkbCanceled_Unchecked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                EditedProvider.Canceled = false;
                AreDataChanged = (EditedProvider != Provider) || ValidateChangesInRelatedProviders();
                DataChangedManagerActive = true;
            }
        }

        private void ChkbCanceled_Checked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                EditedProvider.Canceled = true;
                AreDataChanged = (EditedProvider != Provider) || ValidateChangesInRelatedProviders();
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region DatePicker

        private void DtpBillingDataRegister_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                EditedProvider.BillingData_Register = (DateTime)dtpBillingDataRegister.SelectedDate;
                tbBillingDataRegister.Text = GlobalViewModel.GetLongDateString(EditedProvider.BillingData_Register);
                AreDataChanged = EditedProvider != Provider || ValidateChangesInRelatedProviders();
            }
        }

        #endregion

        #region ListItems

        #endregion

        #endregion

        #region Update UI

        private void UpdateRelatedProvidersControls()
        {
            btnRelatedProvider.Visibility = m_DataListDefinedProviders.Count > 0 ? Visibility.Visible : Visibility.Hidden;
            btnUnRelatedProvider.Visibility = m_DataListRelatedProviders.Count > 0 ? Visibility.Visible : Visibility.Hidden;
        }

        private bool ValidateChangesInRelatedProviders()
        {
            if (SourceDataListRelatedProviders.Count != DataListRelatedProviders.Count) return true;
            else
            {
                foreach (ProvidersView provider in SourceDataListRelatedProviders)
                {
                    if (!DataListRelatedProviders.Contains(provider)) return true;
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
            EditedProvider.RestoreSourceValues(Provider);
            LoadDataInControls(EditedProvider);
            AreDataChanged = (EditedProvider != Provider) || ValidateChangesInRelatedProviders();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(ProvidersAttributes ErrorField)
        {
            EditedProvider.RestoreSourceValue(Provider, ErrorField);
            LoadDataInControls(EditedProvider);
            AreDataChanged = (EditedProvider != Provider) || ValidateChangesInRelatedProviders();
        }

        #endregion
    }
}
