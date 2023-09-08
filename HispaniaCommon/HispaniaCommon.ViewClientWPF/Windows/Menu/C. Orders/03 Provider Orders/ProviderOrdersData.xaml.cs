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
using System.Diagnostics;
using HispaniaCommon.ViewModel.ViewModel.Paymets;
using HispaniaCommon.ViewModel.ViewModel;

#endregion

namespace HispaniaCommon.ViewClientWPF.UserControls
{
    /// <summary>
    /// Interaction logic for ProvidersData.xaml
    /// </summary>
    public partial class ProviderOrdersData : UserControl
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
        public delegate void dlgAccept(ProviderOrdersView NewOrEditedGood, Guid DataManagementId);

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
        private ObservableCollection<ProviderOrderMovementsView> m_DataList = new ObservableCollection<ProviderOrderMovementsView>();

        /// <summary>
        /// Store the Data Management associated at Provider Order Movements of this Provider Order
        /// </summary>
        private Guid m_DataManagementId = Guid.Empty;

        /// <summary>
        /// Store the Current Id for the new Provider Order Movements.
        /// </summary>
        private int m_CurrentIdForNewProviderOrderMovement = 0;

        /// <summary>
        /// Store the Good data to manage.
        /// </summary>
        private ProviderOrdersView m_ProviderOrder = null;

        /// <summary>
        /// Store the Parameters View
        /// </summary>
        private ParametersView m_Parameters;

        /// <summary>
        /// Store the Goods
        /// </summary>
        private Dictionary<string, GoodsView> m_Goods;

        /// <summary>
        /// Store the Providers
        /// </summary>
        public Dictionary<string, ProvidersView> m_Providers;

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
        /// Window instance of HistoProviders.
        /// </summary>
        private HistoProviders HistoProvidersWindow = null;

        /// <summary>
        /// Window instance of ProviderOrderMovementsData.
        /// </summary>
        private ProviderOrderMovementsData ProviderOrderMovementsDataWindow = null;

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
        /// Get or Set the Provider Order data to manage.
        /// </summary>
        public ProviderOrdersView ProviderOrder
        {
            get
            {
                return (m_ProviderOrder);
            }
            set
            {
                if (value != null)
                {
                    AreDataChanged = false;
                    m_ProviderOrder = value;
                    EditedProviderOrder = new ProviderOrdersView(m_ProviderOrder);
                    LoadDataInControls(m_ProviderOrder, true, 1);
                }
                else throw new ArgumentNullException("Error, no s'han trobat les dades de la Comanda de Proveidor a carregar."); 
            }
        }

        /// <summary>
        /// Get or Set the data to show in List of Items.
        /// </summary>
        public ObservableCollection<ProviderOrderMovementsView> DataList
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
                    foreach (ProviderOrderMovementsView movement in m_DataList)
                    {
                        if (!Goods.ContainsKey(movement.Good_Key))
                        {
                            Goods.Add(movement.Good_Key, movement.Good);
                        }
                    }
                }
                else m_DataList = new ObservableCollection<ProviderOrderMovementsView>();
                SourceDataList = new ObservableCollection<ProviderOrderMovementsView>(m_DataList);
                ListItems.ItemsSource = m_DataList;
                ListItems.DataContext = this;
                //CollectionViewSource.GetDefaultView(ListItems.ItemsSource).SortDescriptions.Add(new SortDescription("ProviderOrderMovement_Id_For_Sort", ListSortDirection.Ascending));
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).SortDescriptions.Add(new SortDescription("ProviderOrderMovement_RowOrder_For_Sort", ListSortDirection.Ascending));
                 ActualizeAvalilableUnitInfo();
                ActualizeAmountInfo(EditedProviderOrder);
            }
        }

        /// <summary>
        /// Get or Set the Providers 
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
        private ObservableCollection<ProviderOrderMovementsView> SourceDataList { get; set; }

        /// <summary>
        /// Get or Set the Edited ProviderOrder information.
        /// </summary>
        private ProviderOrdersView EditedProviderOrder
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
                         if (ProviderOrder == null) throw new InvalidOperationException("Error, impossible visualitzar una Comanda de Client sense dades.");
                         tbDataBankIBANCountryCode.ContextMenu = null;
                         tbCancel.Text = "Tornar";
                         break;
                    case Operation.Add:
                         ProviderOrdersView NewProviderOrder = new ProviderOrdersView();
                         NewProviderOrder.ProviderOrder_Id = GlobalViewModel.Instance.HispaniaViewModel.GetNextIdentityValueTable(NewProviderOrder);
                         NewProviderOrder.Date = DateTime.Now;
                         ProviderOrder = NewProviderOrder;
                         tbDataBankIBANCountryCode.ContextMenu = ctxmnuIBAN_Initial;
                         tbCancel.Text = "Cancel·lar";
                         break;
                    case Operation.Edit:
                         if (ProviderOrder == null) throw new InvalidOperationException("Error, impossible editar una Comanda de Client sense dades.");
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
                dtpDeliveryNoteDate.IsEnabled = (m_CtrlOperation == Operation.Edit) && (ProviderOrder.DeliveryNote_Id > 0);
                lblDeliveryNoteDate.IsEnabled = (m_CtrlOperation == Operation.Edit) && (ProviderOrder.DeliveryNote_Id > 0);
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
                if (m_AreDataChanged && !this._ProcessLoadDataInControls )
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
        /// Get or Set if the according of the movement has changed.
        /// </summary>
        private bool AreAccordingChanged
        {
            get; set;
        }

        /// <summary>
        /// Gets or Set the Data Management associated at Provider Order Movements of this Provider Order
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
        /// Store the Current Id for the new Provider Order Movements.
        /// </summary>
        private int CurrentIdForNewProviderOrderMovement
        {
            get
            {
                return m_CurrentIdForNewProviderOrderMovement;
            }
            set
            {
                m_CurrentIdForNewProviderOrderMovement = value;
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
        /// Get or Set Providers 
        /// </summary>
        public Dictionary<string, ProvidersView> Providers
        {
            get
            {
                return (m_Providers);
            }
            set
            {
                if (value is null) m_Providers = new Dictionary<string, ProvidersView>();
                else m_Providers = value;
                cbProvider.ItemsSource = new SortedDictionary<string, ProvidersView>(m_Providers);
                cbProvider.DisplayMemberPath = "Key";
                cbProvider.SelectedValuePath = "Value";
                CollectionViewSource.GetDefaultView(cbProvider.ItemsSource).Filter = UserFilter;
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
        public ProviderOrdersData()
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
                lblProviderOrderId,
                tbProviderOrderId,
                lblDeliveryNoteId,
                tbDeliveryNoteId,
                tbDeliveryNoteDate,
                lblBillId,
                tbBillId,
                lblBillDate,
                tbBillDate,
                lblBillSerieId,
                tbBillSerieId,
                lblProviderAlias,
                tbProviderAlias,
                lblCompanyName,
                tbCompanyName,
                lblCompanyCif,
                tbCompanyCif,                
                lblSendAddress,
                tbSendAddress,
                lblSendPostalCode,
                tbSendPostalCode,
                lblSendTimetable,
                tbSendTimetable,                
                tbShippingUnitAvailable,
                tbBillingUnitAvailable,
                lblIVAPercent,
                tbIVAPercent,
                lblSurchargePercent,
                tbSurchargePercent,
                lblBillingDataEarlyPaymentDiscount,
                tbBillingDataEarlyPaymentDiscount,
                tbProviderId,
                tbProviderAliasProveidor,
                tbCompanyNameProveidor,
                tbCompanyContactPerson,
                tbCompanyEmail,
                tbCompanyAddress,                
                tbCompanyPhone1,
                tbCompanyPhone2,
                tbCompanyMobilePhone,
                tbCompanyFax,
                tbCompanyTimeTable,
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
                lblProvider,
                cbProvider,
                lblAccording,
                lblPrevisioLliurament,
                chkbAccording,
                lblValued,
                chkbValued,
                lblAddressStores,
                cbAddressStores,
                btnAddressStores,
                lblSendType,
                cbSendType,
                lblProviderOrderRemarks,
                tbProviderOrderRemarks,
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
                gbItemsList,
                ListItems,
                gbIBAN,
                tiHeaderData,
                tiLines,
                tiFootData,
                tiBillingData,
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
                btnDown,
                tbNameClientAssoc,
                lblNameClientAssoc
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
                cbFieldItemToSearch.ItemsSource = ProvidersView.Fields;
                cbFieldItemToSearch.DisplayMemberPath = "Key";
                cbFieldItemToSearch.SelectedValuePath = "Value";
                if (ProvidersView.Fields.Count > 0) cbFieldItemToSearch.SelectedIndex = 1;
            //  Set valid dates for change DeliveryNote Date
                dtpDeliveryNoteDate.DisplayDateStart = new DateTime(DateTime.Now.Year, 1, 1);
                dtpDeliveryNoteDate.DisplayDateEnd = new DateTime(DateTime.Now.Year, 12, 31);
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        private bool _ProcessLoadDataInControls = false;

        /// <summary>
        /// Method that loads the Data in the controls of the Window
        /// </summary>
        private void LoadDataInControls(ProviderOrdersView ProviderOrder, bool Actualize = true, int ThrowException = 0)
        {
            this._ProcessLoadDataInControls = true;

            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Main Controls
                tbProviderOrderId.Text = GlobalViewModel.GetStringFromIntIdValue(ProviderOrder.ProviderOrder_Id);
                tbProviderOrderDate.Text = ProviderOrder.Date_Str;
                tbDeliveryNoteId.Text = ProviderOrder.DeliveryNote_Id_Str;
                tbDeliveryNoteDate.Text = ProviderOrder.DeliveryNote_Date_Str;
                dtpDeliveryNoteDate.SelectedDate = ProviderOrder.DeliveryNote_Date;
                tbBillId.Text = ProviderOrder.Bill_Id_Str;
                tbBillDate.Text = ProviderOrder.Bill_Date_Str;
                tbBillSerieId.Text = ProviderOrder.Bill_Serie_Str;
                chkbAccording.IsChecked = ProviderOrder.According;
                chkbValued.IsChecked = ProviderOrder.Valued;
                chkPrevisioLliurament.IsChecked = ProviderOrder.PrevisioLliurament;
                dtpPrevisioLliurament.Visibility = Visibility.Hidden;
                if (ProviderOrder.PrevisioLliurament)
                {
                    dtpPrevisioLliurament.Visibility = Visibility.Visible;
                    dtpPrevisioLliurament.SelectedDate = ProviderOrder.PrevisioLliuramentData;
                }
            //  Header Data Tab Controls
                if (ProviderOrder.Provider is null)
                {
                    cbProvider.SelectedIndex = -1;
                    tbItemToSearch.Text = string.Empty;
                    tbProviderAlias.Text = string.Empty;
                    tbCompanyName.Text = string.Empty;
                    tbCompanyCif.Text = string.Empty;
                    tbNameClientAssoc.Text = string.Empty;
                    ActualizeProviderAddressData();
                }
                else
                {
                    string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeyProviderView(ProviderOrder.Provider);
                    if (!Providers.ContainsKey(Key))
                    {
                        Providers.Add(Key, GlobalViewModel.Instance.HispaniaViewModel.GetProviderFromDb(ProviderOrder.Provider));
                        ((SortedDictionary<string, ProvidersView>) cbProvider.ItemsSource).Add(Key, GlobalViewModel.Instance.HispaniaViewModel.GetProviderFromDb(ProviderOrder.Provider));
                    }
                    cbProvider.SelectedValue = Providers[Key];
                    tbProviderAlias.Text = ProviderOrder.Provider.Alias;
                    tbCompanyName.Text = ProviderOrder.Provider.Name;
                    tbCompanyCif.Text = ProviderOrder.Provider.NIF;
                    tbNameClientAssoc.Text = ProviderOrder.NameClientAssoc;                
                    //  Dades de Proveidor Tab Controls
                    tbProviderId.Text = ProviderOrder.Provider.Provider_Id.ToString();
                    tbProviderAliasProveidor.Text = ProviderOrder.Provider.Alias;
                    tbCompanyNameProveidor.Text = ProviderOrder.Provider.Name;
                    tbCompanyContactPerson.Text = ProviderOrder.Provider.Agent_Name;
                    tbCompanyEmail.Text = ProviderOrder.Provider.EMail;
                    tbCompanyAddress.Text = ProviderOrder.Provider.Address;
                    cbCompanyPostalCode.Text = ProviderOrder.Provider.PostalCode_Str;
                    tbCompanyPhone1.Text = ProviderOrder.Provider.Phone;
                    //tbCompanyPhone2.Text = ProviderOrder.Provider.Phone_2;
                    tbCompanyMobilePhone.Text = ProviderOrder.Provider.MobilePhone;
                    tbCompanyFax.Text = ProviderOrder.Provider.Fax;
                    //tbCompanyTimeTable.Text = ProviderOrder.Provider.TimeTable;
                    //tbCompanyNumProvClient.Text= ProviderOrder.Provider.Company_NumProv;

                    ActualizeProviderAddressData(ProviderOrder.Provider);
                }
                tbIVAPercent.Text = GlobalViewModel.GetStringFromDecimalValue(ProviderOrder.IVAPercent, DecimalType.Percent, true);
                tbSurchargePercent.Text = GlobalViewModel.GetStringFromDecimalValue(ProviderOrder.SurchargePercent, DecimalType.Percent, true);
                ActualizeSendAddresInfo(ProviderOrder);
            //  Line Data Tab Controls
                if (Actualize)
                {
                    DataList = GlobalViewModel.Instance.HispaniaViewModel.GetProviderOrderMovements(ProviderOrder.ProviderOrder_Id);
                    DataManagementId = GlobalViewModel.Instance.HispaniaViewModel.InitializeDataManaged(DataList);
                }
            //  Foot Data Tab Controls
                tbBillingDataEarlyPaymentDiscount.Text = GlobalViewModel.GetStringFromDecimalValue(ProviderOrder.BillingData_EarlyPaymentDiscount, DecimalType.Percent, true);
                ActualizeAmountInfo(ProviderOrder);
                tbProviderOrderRemarks.Text = ProviderOrder.Remarks;
            //  Data Bank Tab Controls
                tbNumEffect.Text = GlobalViewModel.GetStringFromDecimalValue(ProviderOrder.DataBank_NumEffect, DecimalType.WithoutDecimals);
                tbDataBankFirstExpirationData.Text = GlobalViewModel.GetStringFromDecimalValue(ProviderOrder.DataBank_ExpirationDays, DecimalType.WithoutDecimals);
                tbDataBankExpirationInterval.Text = GlobalViewModel.GetStringFromDecimalValue(ProviderOrder.DataBank_ExpirationInterval, DecimalType.WithoutDecimals);
                tbDataBankPayday_1.Text = GlobalViewModel.GetStringFromDecimalValue(ProviderOrder.DataBank_Payday_1, DecimalType.WithoutDecimals);
                tbDataBankPayday_2.Text = GlobalViewModel.GetStringFromDecimalValue(ProviderOrder.DataBank_Payday_2, DecimalType.WithoutDecimals);
                tbDataBankPayday_3.Text = GlobalViewModel.GetStringFromDecimalValue(ProviderOrder.DataBank_Payday_3, DecimalType.WithoutDecimals);
                tbDataBank_Bank.Text = tbBilling_DataBank_Bank.Text = ProviderOrder.DataBank_Bank;
                tbDataBank_BankAddress.Text = tbBilling_DataBank_BankAddress.Text = ProviderOrder.DataBank_BankAddress;
                tbDataBankIBANCountryCode.Text = ProviderOrder.DataBank_IBAN_CountryCode;
                tbDataBankIBANBankCode.Text = ProviderOrder.DataBank_IBAN_BankCode;
                tbDataBankIBANOfficeCode.Text = ProviderOrder.DataBank_IBAN_OfficeCode;
                tbDataBankIBANCheckDigits.Text = ProviderOrder.DataBank_IBAN_CheckDigits;
                tbDataBankIBANAccountNumber.Text = ProviderOrder.DataBank_IBAN_AccountNumber;            
                LoadExternalTablesInfo(ProviderOrder, ThrowException);
            //  Update historic button
                btnHistoric.IsEnabled = true;
            //  Activate managers
                DataChangedManagerActive = true;

                this._ProcessLoadDataInControls = false;            
        }

        /// <summary>
        /// Load Data from External Tables.
        /// </summary>
        /// <param name="providerOrdersView">Data Container.</param>
        /// <param name="ThrowException">true, if want throw an exception if not found a component</param>
        private void LoadExternalTablesInfo(ProviderOrdersView providerOrder, int ThrowException = 0)
        {
            if ((EffectTypes != null) && (providerOrder.EffectType != null))
            {
                Dictionary<string, EffectTypesView> Items = (Dictionary<string, EffectTypesView>)cbEffectType.ItemsSource;
                string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeyEffectTypeView(providerOrder.EffectType);
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
            if ((SendTypes != null) && (providerOrder.SendType != null))
            {
                Dictionary<string, SendTypesView> Items = (Dictionary<string, SendTypesView>)cbSendType.ItemsSource;
                string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeySendTypeView(providerOrder.SendType);
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
                ProvidersView ItemData = ((KeyValuePair<string, ProvidersView>)item).Value;
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
            //  By default the manager for the Provider Order Data changes is active.
                DataChangedManagerActive = true;
            //  TextBox
                tbNameClientAssoc.TextChanged += TBDataChanged;
                tbNameClientAssoc.PreviewTextInput += TBPreviewTextInput;

                tbBillingDataEarlyPaymentDiscount.PreviewTextInput += TBPreviewTextInput;
                tbBillingDataEarlyPaymentDiscount.TextChanged += TBPrecentDataChanged;
                tbProviderOrderRemarks.PreviewTextInput += TBPreviewTextInput;
                tbProviderOrderRemarks.TextChanged += TBDataChanged;
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
                btnAccept.Click += BtnAccept_Click;
                btnCancel.Click += BtnCancel_Click;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DtpPrevisioLliurament_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            AreDataChanged = true;

            DateTime? date = ((DatePicker)sender).SelectedDate;

            if( date.HasValue)
            {
                EditedProviderOrder.PrevisioLliuramentData = date.Value;
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
            if (sender == tbProviderOrderRemarks) e.Handled = ! GlobalViewModel.IsValidCommentChar(e.Text);
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
                    if(sender == tbNumEffect) EditedProviderOrder.DataBank_NumEffect = GlobalViewModel.GetDecimalValue( value );
                    else if((sender == tbDataBank_Bank) || (sender == tbBilling_DataBank_Bank))
                    {
                        if(sender == tbDataBank_Bank) tbBilling_DataBank_Bank.Text = value;
                        else tbDataBank_Bank.Text = value;
                        EditedProviderOrder.DataBank_Bank = value;
                    }
                    else if((sender == tbDataBank_BankAddress) || (sender == tbBilling_DataBank_BankAddress))
                    {
                        if(sender == tbDataBank_BankAddress) tbBilling_DataBank_BankAddress.Text = value;
                        else tbDataBank_BankAddress.Text = value;
                        EditedProviderOrder.DataBank_BankAddress = value;
                    }
                    else if(sender == tbProviderOrderRemarks) EditedProviderOrder.Remarks = value;
                    else if(sender == tbDataBankFirstExpirationData) EditedProviderOrder.DataBank_ExpirationDays = GlobalViewModel.GetDecimalValue( value );
                    else if(sender == tbDataBankExpirationInterval) EditedProviderOrder.DataBank_ExpirationInterval = GlobalViewModel.GetDecimalValue( value );
                    else if(sender == tbDataBankPayday_1)
                    {
                        EditedProviderOrder.DataBank_Payday_1 = GlobalViewModel.GetDecimalValue( value );                        
                    }
                    else if(sender == tbDataBankPayday_2)
                    {                    
                        EditedProviderOrder.DataBank_Payday_2 = GlobalViewModel.GetDecimalValue(value);
                    }
                    else if (sender == tbDataBankPayday_3)
                    { 
                        EditedProviderOrder.DataBank_Payday_3 = GlobalViewModel.GetDecimalValue(value);
                    }
                    else if (sender == tbDataBankIBANCountryCode) EditedProviderOrder.DataBank_IBAN_CountryCode = value;
                    else if (sender == tbDataBankIBANBankCode) EditedProviderOrder.DataBank_IBAN_BankCode = value;
                    else if (sender == tbDataBankIBANOfficeCode) EditedProviderOrder.DataBank_IBAN_OfficeCode = value;
                    else if (sender == tbDataBankIBANCheckDigits) EditedProviderOrder.DataBank_IBAN_CheckDigits = value;
                    else if (sender == tbDataBankIBANAccountNumber) EditedProviderOrder.DataBank_IBAN_AccountNumber = value;
                    else if(sender == tbNameClientAssoc) EditedProviderOrder.NameClientAssoc = value;
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedProviderOrder, false);
                }
                AreDataChanged = (EditedProviderOrder != ProviderOrder);
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
                    if (sender == tbBillingDataEarlyPaymentDiscount) EditedProviderOrder.BillingData_EarlyPaymentDiscount = value;
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedProviderOrder, false);
                }
                AreDataChanged = (EditedProviderOrder != ProviderOrder);
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
            ProviderOrdersAttributes ErrorField = ProviderOrdersAttributes.None;
            try
            {
                if ((CtrlOperation == Operation.Add) || (CtrlOperation == Operation.Edit) )
                {
                    EditedProviderOrder.Validate(out ErrorField);
                    EvAccept?.Invoke(new ProviderOrdersView(EditedProviderOrder), DataManagementId);
                                     
                    if (AreAccordingChanged)
                    {
                        var providerOrders = new List<ProviderOrdersView>();
                        providerOrders.Add(EditedProviderOrder);
                        GlobalViewModel.Instance.HispaniaViewModel.CreateHistoProviders(EditedProviderOrder.Provider, providerOrders);
                    }
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
        
        #region Accept Provider Selection

        private void BtnAcceptSearch_Click(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                try
                {
                    if (cbProvider.SelectedItem != null)
                    {
                        ProvidersView providerSelected = ((ProvidersView)cbProvider.SelectedValue);
                        bool IsSameProvider = (EditedProviderOrder.Provider != null) &&
                                              (EditedProviderOrder.Provider.Provider_Id == providerSelected.Provider_Id);
                        EditedProviderOrder.Provider = providerSelected;
                        if (providerSelected.DataBank_EffectType != null)
                        {
                            ActualizeEditedProviderOrder(providerSelected, providerSelected.DataBank_EffectType);
                        }                        
                        if (!IsSameProvider)
                        {
                            EditedProviderOrder.Address = string.Empty;
                            EditedProviderOrder.TimeTable = string.Empty;
                            EditedProviderOrder.PostalCode = null;
                        }
                        EditedProviderOrder.EffectType = providerSelected.DataBank_EffectType;
                        EditedProviderOrder.BillingData_EarlyPaymentDiscount = providerSelected.BillingData_EarlyPaymentDiscount;
                        if (providerSelected.BillingData_IVAType is null)
                        {
                            EditedProviderOrder.IVAPercent = 0;
                            EditedProviderOrder.SurchargePercent = 0;
                        }
                        else
                        {
                            EditedProviderOrder.IVAPercent = providerSelected.BillingData_IVAType.IVAPercent;
                            EditedProviderOrder.SurchargePercent = providerSelected.BillingData_IVAType.SurchargePercent;
                        }
                        EditedProviderOrder.SendType = providerSelected.BillingData_SendType;
                        EditedProviderOrder.DataBank_NumEffect = providerSelected.DataBank_NumEffect;
                        EditedProviderOrder.DataBank_ExpirationDays = providerSelected.DataBank_FirstExpirationData;
                        EditedProviderOrder.DataBank_ExpirationInterval = providerSelected.DataBank_ExpirationInterval;
                        EditedProviderOrder.DataBank_Payday_1 = providerSelected.DataBank_Payday_1;
                        EditedProviderOrder.DataBank_Payday_2 = providerSelected.DataBank_Payday_2;
                        EditedProviderOrder.DataBank_Payday_3 = providerSelected.DataBank_Payday_3;
                        EditedProviderOrder.Provider.Several_Remarks = providerSelected.Several_Remarks;
                        EditedProviderOrder.Provider.BillingData_NumUnpaid = providerSelected.BillingData_NumUnpaid;
                        EditedProviderOrder.BillingData_Agent = providerSelected.BillingData_Agent;
                        EditedProviderOrder.Valued = providerSelected.BillingData_Valued;
                        AreDataChanged = (EditedProviderOrder != ProviderOrder);
                        LoadDataInControls(EditedProviderOrder, false);
                        ActualizeProviderAddressData(EditedProviderOrder.Provider);
                        if (cbAddressStores.Items.Count > 0)
                        {
                            if (!IsSameProvider)
                            {
                                cbAddressStores.SelectedIndex = 0;
                                AddressStoresView addressStore = (AddressStoresView)cbAddressStores.SelectedValue;
                                EditedProviderOrder.Address = addressStore.Address;
                                EditedProviderOrder.TimeTable = addressStore.Timetable;
                                EditedProviderOrder.PostalCode = addressStore.PostalCode;
                                ActualizeSendAddresInfoData(EditedProviderOrder);
                            }
                            else ActualizeSendAddresInfo(EditedProviderOrder);
                        }
                        ShowGoodRemark();
                        ActualizeButtonBar();
                        cbSendType.Focus();
                        cbAddressStores.IsDropDownOpen = (EditedProviderOrder.Provider != null) && (cbAddressStores.Items.Count > 0) && 
                                                         (!IsSameProvider);
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
        /// Defines a new Address Store for the Provider.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnAddressStores_Click(object sender, RoutedEventArgs e)
        {
            if (AddressStoresWindow == null)
            {
                try
                {
                    ProvidersView Provider = EditedProviderOrder.Provider;
                    GlobalViewModel.Instance.HispaniaViewModel.RefreshPostalCodes();
                    AddressStoresWindow = new AddressStores(AppType, Operation.Edit)
                    {

                        PostalCodes = GlobalViewModel.Instance.HispaniaViewModel.PostalCodesDict,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.GetAddressStores(Provider.Provider_Id),
                        DataProv = Provider
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
            ActualizeProviderAddressData(EditedProviderOrder.Provider);
            AddressStoresWindow.Closed -= StoreAddressesWindow_Closed;
            AddressStoresWindow = null;
        }

        #endregion

        #region Agents (Update data)

        /// <summary>
        /// Defines a new Agent for the ProviderOrder.
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
        /// When the Provider Window is closed we actualize the AgentsWindow attribute value.
        /// </summary>
        /// <param name="sender">Label that prodece the event</param>
        /// <param name="e">Parameters of the event</param>
        private void AgentsWindow_Closed(object sender, EventArgs e)
        {
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
            if(GoodsWindow == null)
            {
                try
                {
                    GlobalViewModel.Instance.HispaniaViewModel.RefreshUnits();
                    GlobalViewModel.Instance.HispaniaViewModel.RefreshGoods();
                    GoodsWindow = new Goods( AppType )
                    {
                        Units = GlobalViewModel.Instance.HispaniaViewModel.UnitsDict,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.Goods
                    };
                    GoodsWindow.GoodsVM.Managemend = false;
                    GoodsWindow.Closed += GoodsWindow_Closed;
                    GoodsWindow.Show();
                } catch(Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format( "Error, al carregar la finestra d'edició d'artícles.\r\nDetalls:{0}", MsgManager.ExcepMsg( ex ) ) );
                }
            }
            else
            {
                GoodsWindow.GoodsVM.Managemend = false;
                GoodsWindow.Activate();
            }
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
                ProviderOrderMovementsView Movement = (ProviderOrderMovementsView)ListItems.SelectedItem;
                if (ProviderOrderMovementsDataWindow == null)
                {
                    try
                    {
                        ProviderOrderMovementsDataWindow = new ProviderOrderMovementsData(AppType)
                        {
                            CtrlOperation = Operation.Show,
                            Goods = GlobalViewModel.Instance.HispaniaViewModel.GoodsActiveDict,
                            Data = new ProviderOrderMovementsView(Movement)
                        };
                        ProviderOrderMovementsDataWindow.Closed += ProviderOrderMovementsDataWindow_Closed;
                        ProviderOrderMovementsDataWindow.Show();
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(
                            string.Format("Error, a l'intentar visualitzar les dades de la línia '{0}'.\r\nDetalls: {1}",
                                            Movement.ProviderOrderMovement_Id, MsgManager.ExcepMsg(ex)));
                    }
                }
                else ProviderOrderMovementsDataWindow.Activate();
            }
        }

        private void ProviderOrderMovementsDataWindow_Closed(object sender, EventArgs e)
        {
            //  Undefine the close event manager.
            ProviderOrderMovementsDataWindow.Closed -= ProviderOrderMovementsDataWindow_Closed;
            ProviderOrderMovementsDataWindow = null;
        }

        #endregion

        #region Add (Update Data)

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (ProviderOrderMovementsDataWindow == null)
            {
                try
                {
                    m_CurrentIdForNewProviderOrderMovement -= 1;
                    ProviderOrderMovementsView newMovement = new ProviderOrderMovementsView()
                    {
                        ProviderOrderMovement_Id = m_CurrentIdForNewProviderOrderMovement,
                        ProviderOrder = EditedProviderOrder
                    };
                    ProviderOrderMovementsDataWindow = new ProviderOrderMovementsData(AppType)
                    {
                        CtrlOperation = Operation.Add,
                        Goods = GlobalViewModel.Instance.HispaniaViewModel.GoodsActiveDict,
                        Data = newMovement
                    };
                    ProviderOrderMovementsDataWindow.Closed += ProviderOrderMovementsDataWindow_Add_Closed;
                    ProviderOrderMovementsDataWindow.Show();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, al carregar la finestra de creació d'una nova línia de comanda.\r\nDetalls:{0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else ProviderOrderMovementsDataWindow.Activate();
        }

        private void ProviderOrderMovementsDataWindow_Add_Closed(object sender, EventArgs e)
        {
            //  Actualize List of Provider Order Movements from historic of movements.
                if ((ProviderOrderMovementsDataWindow.AreDataChanged) && (!ProviderOrderMovementsDataWindow.IsCanceled))
                {
                    try
                    {
                        ProviderOrderMovementsView newMovement = ProviderOrderMovementsDataWindow.EditedProviderOrderMovement;
                        GlobalViewModel.Instance.HispaniaViewModel.CreateItemInDataManaged(DataManagementId, newMovement);
                        DataList.Add(newMovement);
                        ActualizeGoodInfo(newMovement, MovementOp.Add);
                        ListItems.SelectedItem = newMovement;
                        ActualizeAmountInfo(EditedProviderOrder);
                        AreDataChanged = AreNotEquals(DataList, SourceDataList);
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(
                           string.Format("Error, a l'actualitzar les dades de la nova línia de comanda.\r\nDetalls:{0}", MsgManager.ExcepMsg(ex)));
                    }
                }
            //  Undefine the close event manager.
                ProviderOrderMovementsDataWindow.Closed -= ProviderOrderMovementsDataWindow_Add_Closed;
                ProviderOrderMovementsDataWindow = null;
        }

        #endregion
                
        #region Update (Update Data)

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItem != null)
            {
                if (ProviderOrderMovementsDataWindow == null)
                {
                    ProviderOrderMovementsView Movement = (ProviderOrderMovementsView)ListItems.SelectedItem;
                    try
                    {
                        ProviderOrderMovementsDataWindow = new ProviderOrderMovementsData(AppType)
                        {
                            CtrlOperation = Operation.Edit,
                            Goods = GlobalViewModel.Instance.HispaniaViewModel.GoodsActiveDict,
                            Data = new ProviderOrderMovementsView(Movement)
                        };
                        ProviderOrderMovementsDataWindow.Closed += ProviderOrderMovementsDataWindow_Edit_Closed;
                        ProviderOrderMovementsDataWindow.Show();
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(
                           string.Format("Error, al carregar la finestra d'edició de la línia de comanda '{0}'.\r\nDetalls:{1}",
                                         Movement.ProviderOrderMovement_Id, MsgManager.ExcepMsg(ex)));
                    }
                }
                else ProviderOrderMovementsDataWindow.Activate();
            }
        }

        private void ProviderOrderMovementsDataWindow_Edit_Closed(object sender, EventArgs e)
        {
            //  Actualize List of Provider Order Movements from historic of movements.
                if ((ProviderOrderMovementsDataWindow.AreDataChanged) && (!ProviderOrderMovementsDataWindow.IsCanceled))
                {
                    ProviderOrderMovementsView currentMovement = (ProviderOrderMovementsView)ListItems.SelectedItem;
                    try
                    {
                        ProviderOrderMovementsView newMovement = ProviderOrderMovementsDataWindow.EditedProviderOrderMovement;
                        GlobalViewModel.Instance.HispaniaViewModel.UpdateItemInDataManaged(DataManagementId, currentMovement, newMovement);
                        int Index = ListItems.SelectedIndex;
                        DataChangedManagerActive = false;
                        ListItems.SelectedItem = null;
                        DataChangedManagerActive = true;
                        DataList.Remove(currentMovement);
                        DataList.Insert(Index, newMovement);
                        ActualizeGoodInfo(currentMovement, newMovement);
                        ListItems.SelectedItem = newMovement;
                        ActualizeAmountInfo(EditedProviderOrder);
                        AreDataChanged = AreNotEquals(DataList, SourceDataList);
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(
                           string.Format("Error, a l'actualitzar les dades de la línia de comanda '{0}'.\r\nDetalls:{1}", 
                                         currentMovement.ProviderOrderMovement_Id, MsgManager.ExcepMsg(ex)));
                    }
                }
            //  Undefine the close event manager.
                ProviderOrderMovementsDataWindow.Closed -= ProviderOrderMovementsDataWindow_Edit_Closed;
                ProviderOrderMovementsDataWindow = null;
        }

        #endregion

        #region Delete Movement (Update Data)

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItem != null)
            {
                ProviderOrderMovementsView Movement = (ProviderOrderMovementsView)ListItems.SelectedItem;
                try
                {
                    string Question = string.Format("Està segur que vol esborrar la línia de Comanda de Client '{0}' ?",
                                                    ((ProviderOrderMovementsView)ListItems.SelectedItem).ProviderOrderMovement_Id);
                    if (MsgManager.ShowQuestion(Question) == MessageBoxResult.Yes)
                    {
                        GlobalViewModel.Instance.HispaniaViewModel.DeleteItemInDataManaged(DataManagementId, Movement);
                        ListItems.SelectedItem = null;
                        DataList.Remove(Movement);
                        ActualizeGoodInfo(Movement, MovementOp.Remove, false);
                        ListItems.SelectedItem = null;
                        ActualizeAmountInfo(EditedProviderOrder);
                        AreDataChanged = AreNotEquals(DataList, SourceDataList);
                    }
                    else MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'esborrar de la línia de comanda '{0}'.\r\nDetalls:{1}",
                                     Movement.ProviderOrderMovement_Id, MsgManager.ExcepMsg(ex)));
                }
            }
        }

        #endregion

        #region Historic (Update Data)

        private void BtnHistoric_Click(object sender, RoutedEventArgs e)
        {
            if (HistoProvidersWindow == null)
            {
                try
                {
                    HistoProvidersWindow = new HistoProviders(AppType, HistoProvidersMode.ProviderOrderMovementInput)
                    {
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.GetHistoProviders(EditedProviderOrder.Provider.Provider_Id, true),
                        Data = EditedProviderOrder.Provider
                    };
                    HistoProvidersWindow.Closed += HistoProvidersWindow_Closed;
                    HistoProvidersWindow.Show();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, al carregar la finestra per afegir linies de comanda des dels històrics de client.\r\nDetalls:{0}", 
                                     MsgManager.ExcepMsg(ex)));
                }
            }
            else HistoProvidersWindow.Activate();
        }

        private void HistoProvidersWindow_Closed(object sender, EventArgs e)
        {
            //  Actualize List of Provider Order Movements from historic of movements.
                foreach (HistoProvidersView histoProvider in HistoProvidersWindow.HistoProviderSelected)
                {
                    try
                    {
                        m_CurrentIdForNewProviderOrderMovement -= 1;
                        ProviderOrderMovementsView newMovement = new ProviderOrderMovementsView()
                        {
                            ProviderOrderMovement_Id = m_CurrentIdForNewProviderOrderMovement,
                            ProviderOrder = EditedProviderOrder,
                            Good = histoProvider.Good,
                            Description = histoProvider.Good_Description,
                            Unit_Shipping = histoProvider.Shipping_Units,
                            Unit_Billing = histoProvider.Billing_Units,
                            RetailPrice = histoProvider.Retail_Price,
                            Comission = histoProvider.Comission,
                            Unit_Billing_Definition = histoProvider.Unit_Billing_Definition,
                            Unit_Shipping_Definition = histoProvider.Unit_Shipping_Definition,
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
                ActualizeAmountInfo(EditedProviderOrder);
                AreDataChanged = AreNotEquals(DataList, SourceDataList);
            //  Undefine the close event manager.
                HistoProvidersWindow.Closed -= HistoProvidersWindow_Closed;
                HistoProvidersWindow = null;
        }

        #endregion

        #region According (Update Data)

        /// <summary>
        /// Set According for the Provider Order Movement selected at true.
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
        /// Set According for the Provider Order Movement selected at false.
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
        /// Manage the change of the Data in the combobox of Providers.
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
                        EditedProviderOrder.Address = string.Empty;
                        EditedProviderOrder.TimeTable = string.Empty;
                        EditedProviderOrder.PostalCode = null;
                    }
                    else
                    {
                        AddressStoresView addressStore = (AddressStoresView)cbAddressStores.SelectedValue;
                        EditedProviderOrder.Address = addressStore.Address;
                        EditedProviderOrder.TimeTable = addressStore.Timetable;
                        EditedProviderOrder.PostalCode = addressStore.PostalCode;
                    }
                    ActualizeSendAddresInfo(EditedProviderOrder);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, al seleccionar l'adreça.\r\nDetalls:{0}", MsgManager.ExcepMsg(ex)));
                }
                AreDataChanged = (EditedProviderOrder != ProviderOrder);
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
                    EditedProviderOrder.SendType = sendTypeSelected;
                    AreDataChanged = (EditedProviderOrder != ProviderOrder);
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
                        ActualizeEditedProviderOrder(EditedProviderOrder.Provider, effectTypeSelected);
                        EditedProviderOrder.EffectType = effectTypeSelected;
                        AreDataChanged = (EditedProviderOrder != ProviderOrder);
                        tbDataBank_Bank.Text = tbBilling_DataBank_Bank.Text = EditedProviderOrder.DataBank_Bank;
                        tbDataBank_BankAddress.Text = tbBilling_DataBank_BankAddress.Text = EditedProviderOrder.DataBank_BankAddress;
                        tbDataBankIBANCountryCode.Text = EditedProviderOrder.DataBank_IBAN_CountryCode;
                        tbDataBankIBANBankCode.Text = EditedProviderOrder.DataBank_IBAN_BankCode;
                        tbDataBankIBANOfficeCode.Text = EditedProviderOrder.DataBank_IBAN_OfficeCode;
                        tbDataBankIBANCheckDigits.Text = EditedProviderOrder.DataBank_IBAN_CheckDigits;
                        tbDataBankIBANAccountNumber.Text = EditedProviderOrder.DataBank_IBAN_AccountNumber;
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(string.Format("Error, a l'intentar canviar el tipus d'Efecte.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                    }
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
                EditedProviderOrder.According = false;
                AreDataChanged = (EditedProviderOrder != ProviderOrder);
            }
            lblPrevisioLliurament.Visibility = Visibility.Visible;
            chkPrevisioLliurament.Visibility = Visibility.Visible;
        }

        private void ChkbAccording_Checked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                EditedProviderOrder.According = true;
                AreDataChanged = (EditedProviderOrder != ProviderOrder);
            }
            lblPrevisioLliurament.Visibility = Visibility.Hidden;
            chkPrevisioLliurament.Visibility = Visibility.Hidden;

            //this._PaymentInfoViewModel.Visible = false;
        }

        private void ChkbValued_Unchecked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                EditedProviderOrder.Valued = false;
                AreDataChanged = (EditedProviderOrder != ProviderOrder);
            }
        }

        private void ChkbValued_Checked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                EditedProviderOrder.Valued = true;
                AreDataChanged = (EditedProviderOrder != ProviderOrder);
            }
        }

        private void ChkPrevisioLliurament_Checked(object sender, RoutedEventArgs e)
        {
            this.dtpPrevisioLliurament.Visibility = Visibility.Visible;

            EditedProviderOrder.PrevisioLliurament = true;
            this.AreDataChanged = true;

        }

        private void ChkPrevisioLliurament_Unchecked(object sender, RoutedEventArgs e)
        {
            this.dtpPrevisioLliurament.Visibility = Visibility.Hidden;

            EditedProviderOrder.PrevisioLliurament = false;
            this.AreDataChanged = true;
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
                EditedProviderOrder.DeliveryNote_Date = (DateTime)dtpDeliveryNoteDate.SelectedDate;
                tbDeliveryNoteDate.Text = GlobalViewModel.GetStringFromDateTimeValue(EditedProviderOrder.DeliveryNote_Date);
                AreDataChanged = (EditedProviderOrder != ProviderOrder);
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
            if ((EditedProviderOrder != null) && (EditedProviderOrder.Provider != null))
            {
                MsgManager.ShowMessage(EditedProviderOrder.Provider.Several_Remarks, MsgType.Information);
                tbItemToSearch.Focus();
            }
        }

        /// <summary>
        /// Restore all values.
        /// </summary>
        public void RestoreSourceValues()
        {
            EditedProviderOrder.RestoreSourceValues(ProviderOrder);
            LoadDataInControls(EditedProviderOrder, false);
            AreDataChanged = (EditedProviderOrder != ProviderOrder);
        }

        /// <summary>
        /// Restore the value of the indicated field.
        /// </summary>
        /// <param name="ErrorField">Field to restore value.</param>
        public void RestoreSourceValue(ProviderOrdersAttributes ErrorField)
        {
            EditedProviderOrder.RestoreSourceValue(ProviderOrder, ErrorField);
            LoadDataInControls(EditedProviderOrder, false);
            AreDataChanged = (EditedProviderOrder != ProviderOrder);
        }

        #endregion

        #region Update IU Methods

        private void FilterDataListObjects()
        {
            CollectionViewSource.GetDefaultView(cbProvider.ItemsSource).Refresh();
            if (cbProvider.Items.Count > 0) cbProvider.SelectedIndex = 0;
        }

        private void ManageAccordingChanged(bool NewAccordingValue)
        {
            ProviderOrderMovementsView currentMovement = (ProviderOrderMovementsView)ListItems.SelectedItem;
            ProviderOrderMovementsView newMovement = new ProviderOrderMovementsView(currentMovement)
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
            ActualizeAmountInfo(EditedProviderOrder);
            AreDataChanged = AreNotEquals(DataList, SourceDataList);
            AreAccordingChanged = AreNotEquals(DataList, SourceDataList);
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
                ProviderOrderMovementsView movement = (ProviderOrderMovementsView)ListItems.SelectedItem;
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
            if ((ListItems.SelectedItem is null) || (EditedProviderOrder.Provider is null))
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
                if (((ProviderOrderMovementsView)ListItems.SelectedItem).According)
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
            if ((m_CtrlOperation == Operation.Show) || (EditedProviderOrder.Provider is null))
            {
                btnAdd.Visibility = btnHistoric.Visibility = Visibility.Hidden;
            }
            else
            {
                btnAdd.Visibility = Visibility.Visible;
                btnHistoric.Visibility = EditedProviderOrder.Provider != null ? Visibility.Visible : Visibility.Hidden;
            }
            btnAddGood.Visibility = m_CtrlOperation != Operation.Show ? Visibility.Visible : Visibility.Hidden;
            if ((m_CtrlOperation == Operation.Show) || 
                ((m_CtrlOperation != Operation.Show) && (!EditedProviderOrder.HasDeliveryNote)))
            {
                dtpDeliveryNoteDate.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 155, 33, 29));
            }
            else
            {
                dtpDeliveryNoteDate.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 8, 70, 124));
            }
        }

        private void ActualizeGoodInfo(ProviderOrderMovementsView currentMovement,
                                       ProviderOrderMovementsView newMovement)
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

        private void ActualizeGoodInfo(ProviderOrderMovementsView Movement, MovementOp Action,
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

        private void ActualizeAmountInfo(ProviderOrdersView ProviderOrder)
        {
            decimal EarlyPaymentDiscountPrecent = ProviderOrder.BillingData_EarlyPaymentDiscount;
            GlobalViewModel.Instance.HispaniaViewModel.CalculateAmountInfo(DataList,
                                                                           EarlyPaymentDiscountPrecent,
                                                                           ProviderOrder.IVAPercent,
                                                                           ProviderOrder.SurchargePercent,
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
            ProviderOrder.TotalAmount = TotalAmount;
        }

        private void ActualizeEditedProviderOrder(ProvidersView Provider, EffectTypesView EffectType)
        {
            if (EffectType.EffectType_Id == 6) // TRANSFERÈNCIA (Parameters)
            {
                GlobalViewModel.Instance.HispaniaViewModel.RefreshParameters();
                Parameters = GlobalViewModel.Instance.HispaniaViewModel.Parameters;
                EditedProviderOrder.DataBank_Bank = Parameters.DataBank_Bank;
                EditedProviderOrder.DataBank_BankAddress = Parameters.DataBank_BankAddress;
                EditedProviderOrder.DataBank_IBAN_CountryCode = Parameters.DataBank_IBAN_CountryCode;
                EditedProviderOrder.DataBank_IBAN_BankCode = Parameters.DataBank_IBAN_BankCode;
                EditedProviderOrder.DataBank_IBAN_OfficeCode = Parameters.DataBank_IBAN_OfficeCode;
                EditedProviderOrder.DataBank_IBAN_CheckDigits = Parameters.DataBank_IBAN_CheckDigits;
                EditedProviderOrder.DataBank_IBAN_AccountNumber = Parameters.DataBank_IBAN_AccountNumber;
            }
            else // ALTRA SISTEMA DE PAGAMENT (Provider)
            {
                if (!(EditedProviderOrder.Provider is null))
                {
                    EditedProviderOrder.DataBank_Bank = Provider.DataBank_Bank;
                    EditedProviderOrder.DataBank_BankAddress = Provider.DataBank_BankAddress;
                    EditedProviderOrder.DataBank_IBAN_CountryCode = Provider.DataBank_IBAN_CountryCode;
                    EditedProviderOrder.DataBank_IBAN_BankCode = Provider.DataBank_IBAN_BankCode;
                    EditedProviderOrder.DataBank_IBAN_OfficeCode = Provider.DataBank_IBAN_OfficeCode;
                    EditedProviderOrder.DataBank_IBAN_CheckDigits = Provider.DataBank_IBAN_CheckDigits;
                    EditedProviderOrder.DataBank_IBAN_AccountNumber = Provider.DataBank_IBAN_AccountNumber;
                }
                else
                {
                    EditedProviderOrder.DataBank_Bank = string.Empty;
                    EditedProviderOrder.DataBank_BankAddress = string.Empty;
                    EditedProviderOrder.DataBank_IBAN_CountryCode = string.Empty;
                    EditedProviderOrder.DataBank_IBAN_BankCode = string.Empty;
                    EditedProviderOrder.DataBank_IBAN_OfficeCode = string.Empty;
                    EditedProviderOrder.DataBank_IBAN_CheckDigits = string.Empty;
                    EditedProviderOrder.DataBank_IBAN_AccountNumber = string.Empty;
                }
            }
        }

        private void ActualizeProviderAddressData(ProvidersView provider = null)
        {
            if (provider is null)
            {
                cbAddressStores.ItemsSource = null;
                cbAddressStores.SelectedIndex = -1;
            }
            else
            {
                string Key;
                Dictionary<string, AddressStoresView> AddressStoresOfProvider = new Dictionary<string, AddressStoresView>();
                if (provider.PostalCode == null) Key = $"Adreça Principal del Client | {provider.Address}";
                else
                {
                    string PostalCode = provider.PostalCode.Postal_Code_Info;
                    Key = $"Adreça Principal del Client ->  Adreça: '{provider.Address}'  -  Còdi Postal: '{PostalCode}'";
                }
                AddressStoresView Address = new AddressStoresView(provider.Provider_Id)
                {
                    Address = provider.Address,
                    PostalCode = provider.PostalCode,
                    //Timetable = provider.Company_TimeTable
                };
                AddressStoresOfProvider.Add(Key, Address);
                foreach (AddressStoresView addressStore in GlobalViewModel.Instance.HispaniaViewModel.GetAddressStores(provider.Provider_Id))
                {
                    if (addressStore.PostalCode == null) Key = $"{addressStore.AddressStore_Id} | {addressStore.Address}";
                    else
                    {
                        string PostalCode = addressStore.PostalCode.Postal_Code_Info;
                        Key = $"Id: {addressStore.AddressStore_Id}  ->  Adreça: '{addressStore.Address}'  -  Còdi Postal: '{PostalCode}'";
                    }
                    AddressStoresOfProvider.Add(Key, addressStore);
                }
                cbAddressStores.ItemsSource = AddressStoresOfProvider;
                cbAddressStores.DisplayMemberPath = "Key";
                cbAddressStores.SelectedValuePath = "Value";
            }
        }

        private void ActualizeSendAddresInfoData(ProviderOrdersView ProviderOrder)
        {
            string AddressValue = (string.IsNullOrEmpty(ProviderOrder.Address)) ? string.Empty : ProviderOrder.Address;
            tbSendAddress.Text = AddressValue;
            tbSendAddress.ToolTip = AddressValue;
            string PostalCodeValue = (ProviderOrder.PostalCode == null) ? string.Empty : ProviderOrder.PostalCode.Postal_Code_Info;
            tbSendPostalCode.Text = PostalCodeValue;
            tbSendPostalCode.ToolTip = PostalCodeValue;
            string TimeTableValue = (string.IsNullOrEmpty(ProviderOrder.TimeTable)) ? string.Empty : ProviderOrder.TimeTable;
            tbSendTimetable.Text = TimeTableValue;
            tbSendTimetable.ToolTip = TimeTableValue;
        }
        private void ActualizeSendAddresInfo(ProviderOrdersView ProviderOrder)
        {
            ActualizeSendAddresInfoData(ProviderOrder);
            foreach (KeyValuePair<string, AddressStoresView> item in cbAddressStores.Items)
            {
                string AddresStore_Address = (item.Value.Address == null) ? item.Value.Address : item.Value.Address.ToLower();
                string providerOrdersView_Address = (ProviderOrder.Address == null) ? ProviderOrder.Address : ProviderOrder.Address.ToLower();
                if (string.Equals(AddresStore_Address, providerOrdersView_Address))
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
                    ProviderOrderMovementsView newMovement = new ProviderOrderMovementsView();
                    newMovement.Good = GoodsWindow.SelectedGoodView;
                    newMovement.ProviderOrder = this.ProviderOrder;
                    newMovement.Description = GoodsWindow.SelectedGoodView.Good_Description;
                    newMovement.Unit_Billing_Definition = GoodsWindow.SelectedGoodView.Good_Unit.Billing;
                    newMovement.Unit_Shipping_Definition = GoodsWindow.SelectedGoodView.Good_Unit.Shipping;
                    //newMovement.ShippingUnitAvailable = good.Shipping_Unit_Available_Str;
                    //newMovement.BillingUnitAvailable = good.Billing_Unit_Available_Str;
                    GlobalViewModel.Instance.HispaniaViewModel.CreateItemInDataManaged(DataManagementId, newMovement);
                    DataList.Add(newMovement);
                    ActualizeGoodInfo(newMovement, MovementOp.Add);
                    ListItems.SelectedItem = newMovement;
                    //ActualizeAmountInfo(EditedProviderOrder);
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

        private bool AreNotEquals(ObservableCollection<ProviderOrderMovementsView> Data,
                                  ObservableCollection<ProviderOrderMovementsView> SourceData)
        {
            if ((Data is null) && (SourceData is null))
            {
                return false;
            }
            else if ((Data != null) && (SourceData != null))
            {
                if (Data.Count == SourceData.Count)
                {
                    foreach (ProviderOrderMovementsView Movement in Data)
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


        #region Up/Down Movement

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
                foreach (ProviderOrderMovementsView item in ListItems.Items)
                {
                    if (listaOrden.Contains(item.RowOrder))
                    {
                        return false;
                    }
                    else
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
            foreach (ProviderOrderMovementsView item in ListItems.Items)
            {
                item.RowOrder = i;
                i++;
            }
        }


        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
           
            int Index = ListItems.SelectedIndex;
            CheckRowOrder();
            if (Index>0)
            {
                DataChangedManagerActive = false;
                ProviderOrderMovementsView currentMovement = (ProviderOrderMovementsView)ListItems.Items[Index];
                ProviderOrderMovementsView previousMovement = (ProviderOrderMovementsView)ListItems.Items[Index - 1];
                if ((currentMovement._ProviderOrder_Id <= 0 || previousMovement._ProviderOrder_Id <= 0) || ((currentMovement.ProviderOrder.EffectType == null) || (previousMovement.ProviderOrder.EffectType == null)))
                {
                    MessageBox.Show("Debe guardar la comanda antes de poder ordenar las líneas");
                    return;
                }

                var ord = currentMovement.RowOrder;
                currentMovement.RowOrder = previousMovement.RowOrder;
                previousMovement.RowOrder = ord;
                DataList = new ObservableCollection<ProviderOrderMovementsView>(DataList.OrderBy(x => x.RowOrder));


                if (currentMovement.ProviderOrderMovement_Id <= 0)
                {                    
                    GlobalViewModel.Instance.HispaniaViewModel.CreateProviderOrderMovement(currentMovement);
                }
                else
                {
                    GlobalViewModel.Instance.HispaniaViewModel.UpdateProviderOrderMovement(currentMovement);
                }

                if (previousMovement.ProviderOrderMovement_Id <= 0)
                {
                    GlobalViewModel.Instance.HispaniaViewModel.CreateProviderOrderMovement(previousMovement);
                }
                else
                {
                    GlobalViewModel.Instance.HispaniaViewModel.UpdateProviderOrderMovement(previousMovement);
                }
                AreDataChanged = true;
            }
        }

        private void btnDown_Click(object sender, RoutedEventArgs e)
        {
            int Index = ListItems.SelectedIndex;
            CheckRowOrder();
            if (Index < ListItems.Items.Count-1)
            {
                DataChangedManagerActive = false;
                ProviderOrderMovementsView currentMovement = (ProviderOrderMovementsView)ListItems.Items[Index];
                ProviderOrderMovementsView nextMovement = (ProviderOrderMovementsView)ListItems.Items[Index + 1];
                if ((currentMovement._ProviderOrder_Id <= 0 || nextMovement._ProviderOrder_Id <= 0) || ((currentMovement.ProviderOrder.EffectType == null) ||(nextMovement.ProviderOrder.EffectType==null)))
                {
                    MessageBox.Show("Debe guardar la comanda antes de poder ordenar las líneas");
                    return;
                }

                var ord = currentMovement.RowOrder;
                currentMovement.RowOrder = nextMovement.RowOrder;
                nextMovement.RowOrder = ord;
                DataList = new ObservableCollection<ProviderOrderMovementsView>(DataList.OrderBy(x => x.RowOrder));

                if (nextMovement.ProviderOrderMovement_Id <= 0)
                {
                    GlobalViewModel.Instance.HispaniaViewModel.CreateProviderOrderMovement(nextMovement);
                }
                else
                {
                    GlobalViewModel.Instance.HispaniaViewModel.UpdateProviderOrderMovement(nextMovement);
                }

                if (currentMovement.ProviderOrderMovement_Id <= 0)
                {
                    GlobalViewModel.Instance.HispaniaViewModel.CreateProviderOrderMovement(currentMovement);
                }else
                {
                    GlobalViewModel.Instance.HispaniaViewModel.UpdateProviderOrderMovement(currentMovement);
                }

                AreDataChanged = true;

            }
        }

        #endregion
    }
}
