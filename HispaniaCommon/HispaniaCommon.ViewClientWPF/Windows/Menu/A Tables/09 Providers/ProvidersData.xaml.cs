#region Libraries used for this control

using HispaniaCommon.ViewModel;
using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.Theme;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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
        /// Show the Accept button.
        /// </summary>
        private GridLength ViewButton = new GridLength(120.0);

        /// <summary>
        /// Show the Comissions Button.
        /// </summary>
        private GridLength ViewComissionsButton = new GridLength(2.0, GridUnitType.Star);

        /// <summary>
        /// Hide the button bar.
        /// </summary>
        private GridLength HideButton = new GridLength(0.0);

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
        public delegate void dlgAccept(ProvidersView NewOrEditedProvider);

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
        /// Store the Agents
        /// </summary>
        public Dictionary<string, AgentsView> m_Agents;

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
        /// Store the list of Editable Controls.
        /// </summary>
        private List<Control> EditableControls = null;

        /// <summary>
        /// Store the list of Non Editable Controls.
        /// </summary>
        private List<Control> NonEditableControls = null;

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
        /// Get or Set the Edited Provider information.
        /// </summary>
        private ProvidersView EditedProvider
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
                         cbMiddlePanel.Width = HideButton;
                         tbCancel.Text = "Tornar";
                         break;
                    case Operation.Add:
                         Provider = new ProvidersView();
                         if (Provider == null) throw new InvalidOperationException("Error, impossible realitzar l'alta d'un Proveïdor sense Identificador.");
                         cbMiddlePanel.Width = ViewButton;
                         tbCancel.Text = "Cancel·lar";
                         break;
                    case Operation.Edit:
                         if (Provider == null) throw new InvalidOperationException("Error, impossible editar un Representant sense dades.");
                         cbMiddlePanel.Width = ViewButton;
                         tbCancel.Text = "Cancel·lar";
                         break;
                }
                foreach (Control control in EditableControls)
                {
                    if (control is TextBox) ((TextBox)control).IsReadOnly = (m_CtrlOperation == Operation.Show);
                    else if (control is RichTextBox) ((RichTextBox)control).IsReadOnly = (m_CtrlOperation == Operation.Show);
                    else if (control is GroupBox)
                    {
                        ((GroupBox)control).SetResourceReference(Control.StyleProperty,
                                                                 ((m_CtrlOperation == Operation.Show) ? "NonEditableGroupBox" : "EditableGroupBox"));
                    }
                    else control.IsEnabled = (m_CtrlOperation != Operation.Show);
                }
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
                if (m_AreDataChanged) cbAcceptButton.Width = ViewButton;
                else cbAcceptButton.Width = HideButton;
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
                    cbProviderPostalCode.ItemsSource = m_PostalCodes;
                    cbProviderPostalCode.DisplayMemberPath = "Key";
                    cbProviderPostalCode.SelectedValuePath = "Value";
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
                InitEditableControls();
                InitNonEditableControls();
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
                tbProviderAgentFax
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
                gbAgent
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
                cbAcceptButton.Width = HideButton;
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
                tbProviderTransferPercent.TextChanged += TBPrecentDataChanged;
                tbProviderTransferPercent.PreviewTextInput += TBPreviewTextInput;
                tbProviderPromptPaymentDiscount.TextChanged += TBPrecentDataChanged;
                tbProviderPromptPaymentDiscount.PreviewTextInput += TBPreviewTextInput;
                tbProviderAdditionalDiscount.TextChanged += TBPrecentDataChanged;
                tbProviderAdditionalDiscount.PreviewTextInput += TBPreviewTextInput;
                tbProviderComment.TextChanged += TBDataChanged;
                tbProviderComment.PreviewTextInput += TBPreviewTextInput;
            //  CheckBox
                chkbCanceled.Checked += ChkbCanceled_Checked;
                chkbCanceled.Unchecked += ChkbCanceled_Unchecked;
            //  ComboBox
                cbProviderPostalCode.SelectionChanged += CbProviderPostalCode_SelectionChanged;
                cbProviderDataAgent.SelectionChanged += CbProviderDataAgent_SelectionChanged;
            //  Buttons
                btnAccept.Click += BtnAccept_Click;
                btnCancel.Click += BtnCancel_Click;
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
            ProvidersAttributes ErrorField = ProvidersAttributes.None;
            try
            {
                if ((CtrlOperation == Operation.Add) || (CtrlOperation == Operation.Edit))
                {
                    EditedProvider.Validate(out ErrorField);
                    EvAccept?.Invoke(new ProvidersView(EditedProvider));
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

        #endregion

        #region CheckBox

        private void ChkbCanceled_Unchecked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                EditedProvider.Canceled = false;
                AreDataChanged = (EditedProvider != Provider);
                DataChangedManagerActive = true;
            }
        }

        private void ChkbCanceled_Checked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                EditedProvider.Canceled = true;
                AreDataChanged = (EditedProvider != Provider);
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        public void RestoreSourceValues()
        {
            EditedProvider.RestoreSourceValues(Provider);
            LoadDataInControls(EditedProvider);
            AreDataChanged = (EditedProvider != Provider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(ProvidersAttributes ErrorField)
        {
            EditedProvider.RestoreSourceValue(Provider, ErrorField);
            LoadDataInControls(EditedProvider);
            AreDataChanged = (EditedProvider != Provider);
        }

        #endregion
    }
}
