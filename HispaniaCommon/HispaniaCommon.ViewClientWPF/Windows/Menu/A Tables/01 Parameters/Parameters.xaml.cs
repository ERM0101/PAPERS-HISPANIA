#region Librerias usadas por la ventana

using HispaniaCommon.ViewClientWPF.UserControls;
using HispaniaCommon.ViewModel;
using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.Theme;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#endregion

namespace HispaniaCommon.ViewClientWPF.Windows
{
    /// <summary>
    /// Interaction logic for Parameters.xaml
    /// </summary>
    public partial class Parameters : Window
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
        /// Hide the button.
        /// </summary>
        private GridLength HideComponent = new GridLength(0.0);

        /// <summary>
        /// Show the button.
        /// </summary>
        private GridLength ViewButton = new GridLength(120.0);

        /// <summary>
        /// Show the button.
        /// </summary>
        private GridLength ViewExtendedColumn = new GridLength(1.0, GridUnitType.Star);

        /// <summary>
        /// Hide the button.
        /// </summary>
        private GridLength HideColumn = new GridLength(0.0);

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

        #region Attributes

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private ParametersView m_Data= null;

        /// <summary>
        /// Store the Cities and CP 
        /// </summary>
        public Dictionary<string, PostalCodesView> m_PostalCodes;

        /// <summary>
        /// Store the type of Application with the user want open.
        /// </summary>
        private ApplicationType m_AppType;

        /// <summary>
        /// Stotre if the data of the Customer has changed.
        /// </summary>
        private bool m_AreDataChanged;

        /// <summary>
        /// Store the Operation of the UserControl.
        /// </summary>
        private Operation m_CtrlOperation = Operation.Show;

        #region GUI

        /// <summary>
        /// Store the background color for the search text box.
        /// </summary>
        private Brush m_AppColor = null;

        /// <summary>
        /// Store the list of Editable Controls.
        /// </summary>
        private List<Control> EditableControls = null;

        /// <summary>
        /// Store the list of Non Editable Controls.
        /// </summary>
        private List<Control> NonEditableControls = null;

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
        /// Store the data to show in List of Items.
        /// </summary>
        public ParametersView Data
        {
            get
            {
                return (m_Data);
            }
            set
            {
                if (value != null)
                {
                    AreDataChanged = false;
                    m_Data = new ParametersView(value);
                    LoadDataInControls(m_Data, true);
                }
                else throw new ArgumentNullException("Error, no s'han trobat els paràmetres a carregar.");
            }
        }

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
                    DataChangedManagerActive = false;
                    cbCompanyPostalCode.ItemsSource = m_PostalCodes;
                    cbCompanyPostalCode.DisplayMemberPath = "Key";
                    cbCompanyPostalCode.SelectedValuePath = "Value";
                    DataChangedManagerActive = true;
                }
            }
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
                         cbAcceptButton.Width = HideColumn;
                         cbAcceptButtonExt.Width = HideColumn;
                         cbEditButton.Width = ViewButton;
                         cbCancelButtonExt.Width = HideColumn;
                         cbCancelButton.Width = HideColumn;
                         btnRefresh.IsEnabled = true;
                         gbGeneralData.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                         break;
                    case Operation.Edit:
                         AreDataChanged = false;
                         gbGeneralData.SetResourceReference(Control.StyleProperty, "EditableGroupBox");
                         btnRefresh.IsEnabled = false;
                         break;
                }
                foreach (Control control in EditableControls)
                {
                    if (control is TextBox) ((TextBox)control).IsReadOnly = (m_CtrlOperation == Operation.Show);
                    else if (control is RichTextBox) ((RichTextBox)control).IsReadOnly = (m_CtrlOperation == Operation.Show);
                    else control.IsEnabled = (m_CtrlOperation != Operation.Show);
                }
                cdLblCompanyDaysVtoRisk.Width = cdCompanyDaysVtoRisk.Width = HideComponent;
            }
        }

        /// <summary>
        /// Get or Set the Edited Company information.
        /// </summary>
        private ParametersView EditedParameters
        {
            get;
            set;
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
                    cbAcceptButton.Width = ViewButton;
                    cbAcceptButtonExt.Width = ViewExtendedColumn;
                    cbEditButton.Width = HideColumn;
                    cbCancelButtonExt.Width = ViewExtendedColumn;
                    cbCancelButton.Width = ViewButton;
                }
                else
                {
                    cbAcceptButton.Width = HideColumn;
                    cbAcceptButtonExt.Width = HideColumn;
                    cbEditButton.Width = HideColumn;
                    cbCancelButtonExt.Width = HideColumn;
                    cbCancelButton.Width = ViewButton;
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
        public Parameters(ApplicationType AppType)
        {
            InitializeComponent();
            InitNonEditableControls();
            InitEditableControls();
            Initialize(AppType);
        }

        /// <summary>
        /// Method that initialize the window.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        private void Initialize(ApplicationType AppType)
        {
            //  Actualize the Window.
                ActualizeWindowComponents(AppType);
            //  Load the managers of the controls of the Window.
                LoadManagers();
        }

        #endregion

        #region Standings

        /// <summary>
        /// Method that actualize the Window components.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        private void ActualizeWindowComponents(ApplicationType AppType)
        {
            //  Actualize properties of this Window.
                this.AppType = AppType;
            //  Actualize state of Window controls
                CtrlOperation = Operation.Show;       
        }

        /// <summary>
        /// Initialize the list of Editable Controls.
        /// </summary>
        private void InitEditableControls()
        {
            EditableControls = new List<Control>
            {
                lblCompanyCIF,
                tbCompanyCIF,
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
                lblCompanyDaysVtoRisk,
                tbCompanyDaysVtoRisk,
                lblDataBank_Bank,
                tbDataBank_Bank,
                lblDataBank_BankAddress,
                tbDataBank_BankAddress,
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
            };
        }

        /// <summary>
        /// Initialize the list of Editable Controls.
        /// </summary>
        private void InitNonEditableControls()
        {
            NonEditableControls = new List<Control>
            {
                lblProviderNumProviderOrder,
                tbProviderNumProviderOrder,
                lblCustomerNumOrder,
                tbCustomerNumOrder,
                lblCustomerNumDeliveryNote,
                tbCustomerNumDeliveryNote,
                tbCustomerNumBill,
                lblCustomerNumBill
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
        }

        /// <summary>
        /// Load the data of the Company into the Window
        /// </summary>
        /// <param name="parametersView">Data Container.</param>
        /// <param name="ThrowException">true, if want throw an exception if not found a component</param>
        private void LoadDataInControls(ParametersView parametersView, bool ThrowException = false)
        {
            if (parametersView != null) 
            {
                DataChangedManagerActive = false;
                tbCompanyCIF.Text = parametersView.Company_CIF;
                tbCompanyName.Text = parametersView.Company_Name;
                tbCompanyAddress.Text = parametersView.Company_Address;
                tbCompanyPhone1.Text = parametersView.Company_Phone_1;
                tbCompanyPhone2.Text = parametersView.Company_Phone_2;
                tbCompanyMobilePhone.Text = parametersView.Company_MobilePhone;
                tbCompanyFax.Text = parametersView.Company_Fax;
                tbCompanyEmail.Text = parametersView.Company_EMail;
                tbCompanyDaysVtoRisk.Text = GlobalViewModel.GetStringFromDecimalValue(parametersView.Company_DaysVtoRisk, DecimalType.WithoutDecimals);
                tbProviderNumProviderOrder.Text = GlobalViewModel.GetStringFromDecimalValue(parametersView.Provider_NumProviderOrder, DecimalType.WithoutDecimals);
                tbCustomerNumOrder.Text = GlobalViewModel.GetStringFromDecimalValue(parametersView.Customer_NumOrder, DecimalType.WithoutDecimals);
                tbCustomerNumDeliveryNote.Text = GlobalViewModel.GetStringFromDecimalValue(parametersView.Customer_NumDeliveryNote, DecimalType.WithoutDecimals);
                tbCustomerNumBill.Text = GlobalViewModel.GetStringFromDecimalValue(parametersView.Customer_NumBill, DecimalType.WithoutDecimals);
                tbDataBank_Bank.Text = parametersView.DataBank_Bank;
                tbDataBank_BankAddress.Text = parametersView.DataBank_BankAddress;
                tbDataBankIBANCountryCode.Text = parametersView.DataBank_IBAN_CountryCode;
                tbDataBankIBANBankCode.Text = parametersView.DataBank_IBAN_BankCode;
                tbDataBankIBANOfficeCode.Text = parametersView.DataBank_IBAN_OfficeCode;
                tbDataBankIBANCheckDigits.Text = parametersView.DataBank_IBAN_CheckDigits;
                tbDataBankIBANAccountNumber.Text = parametersView.DataBank_IBAN_AccountNumber;
                LoadExternalTablesInfo(parametersView, ThrowException);
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Load Data from External Tables.
        /// </summary>
        /// <param name="parametersView">Data Container.</param>
        /// <param name="ThrowException">true, if want throw an exception if not found a component</param>
        private void LoadExternalTablesInfo(ParametersView parametersView, bool ThrowException = false)
        {
            if ((PostalCodes != null) && (parametersView.Company_PostalCode != null))
            {

                Dictionary<string, PostalCodesView> Items = (Dictionary<string, PostalCodesView>)cbCompanyPostalCode.ItemsSource;
                string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeyPostalCodeView(parametersView.Company_PostalCode);
                if (Items.ContainsKey(Key)) cbCompanyPostalCode.SelectedValue = PostalCodes[Key];
                else
                {
                    if (ThrowException)
                    {
                        throw new Exception(string.Format("No s'ha trobat el codi postal '{0}'.", PostalCodes[Key].Postal_Code_Info));
                    }
                }
            }
            else cbCompanyPostalCode.SelectedIndex = -1;
        }

        #endregion

        #region Managers

        /// <summary>
        /// Method that define the managers needed for the user operations in the Window
        /// </summary>
        private void LoadManagers()
        {
            //  Window.
                this.Closed += Parameters_Closed;
            //  By default the manager for the Customer Data changes is active.
                DataChangedManagerActive = true;
            //  Basic Company Values (must be not null and not empty).
                tbCompanyCIF.PreviewTextInput += TBPreviewTextInput;
                tbCompanyCIF.TextChanged += TBDataChanged;
                tbCompanyName.PreviewTextInput += TBPreviewTextInput;
                tbCompanyName.TextChanged += TBDataChanged;
                tbCompanyAddress.PreviewTextInput += TBPreviewTextInput;
                tbCompanyAddress.TextChanged += TBDataChanged;
            //  Text Phone Managers 
                tbCompanyPhone1.PreviewTextInput += TBPreviewTextInput;
                tbCompanyPhone1.TextChanged += TBDataChanged;
                tbCompanyPhone2.PreviewTextInput += TBPreviewTextInput;
                tbCompanyPhone2.TextChanged += TBDataChanged;
                tbCompanyFax.PreviewTextInput += TBPreviewTextInput;
                tbCompanyFax.TextChanged += TBDataChanged;
                tbCompanyMobilePhone.PreviewTextInput += TBPreviewTextInput;
                tbCompanyMobilePhone.TextChanged += TBDataChanged;
            //  Additional Data Managers
                tbCompanyEmail.PreviewTextInput += TBPreviewTextInput;
                tbCompanyEmail.TextChanged += TBDataChanged;
                tbCompanyDaysVtoRisk.PreviewTextInput += TBPreviewTextInput;
                tbCompanyDaysVtoRisk.TextChanged += TBDecimalDataChanged;
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
            //  ContextMenuItem
                ctxmnuItemCalculateIBAN.Click += CtxmnuItemCalculateIBAN_Click;
                ctxmnuItemValidateIBAN.Click += CtxmnuItemValidateIBAN_Click;
            //  Foreign Tables Managers
                cbCompanyPostalCode.SelectionChanged += CbCompanyPostalCode_SelectionChanged;
            //  Button managers
                btnAccept.Click += BtnAccept_Click;
                btnCancel.Click += BtnCancel_Click;
                btnEdit.Click += BtnEdit_Click;
                btnRefresh.Click += BtnRefresh_Click;
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
            if (sender == tbCompanyCIF) e.Handled = ! GlobalViewModel.IsValidCIFChar(e.Text);
            else if (sender == tbCompanyName) e.Handled = ! GlobalViewModel.IsValidNameChar(e.Text);
            else if (sender == tbCompanyAddress) e.Handled = ! GlobalViewModel.IsValidAddressChar(e.Text);
            else if ((sender == tbCompanyPhone1) || (sender == tbCompanyPhone2) || (sender == tbCompanyFax))
            {
                e.Handled = ! GlobalViewModel.IsValidPhoneNumberChar(e.Text);
            }
            else if (sender == tbCompanyMobilePhone) e.Handled = ! GlobalViewModel.IsValidMobilePhoneNumberChar(e.Text);
            else if (sender == tbCompanyEmail) e.Handled = ! GlobalViewModel.IsValidEmailChar(e.Text);
            else if (sender == tbCompanyDaysVtoRisk) e.Handled = ! GlobalViewModel.IsValidByteChar(e.Text);
            else if (sender == tbDataBank_Bank) e.Handled = ! GlobalViewModel.IsValidNameChar(e.Text);
            else if (sender == tbDataBank_BankAddress) e.Handled = ! GlobalViewModel.IsValidAddressChar(e.Text);
            else if (sender == tbDataBankIBANCountryCode) e.Handled = ! GlobalViewModel.IsValidIBAN_CountryCodeChar(e.Text);
            else if (sender == tbDataBankIBANBankCode) e.Handled = ! GlobalViewModel.IsValidIBAN_BankCodeChar(e.Text);
            else if (sender == tbDataBankIBANOfficeCode) e.Handled = ! GlobalViewModel.IsValidIBAN_OfficeCodeChar(e.Text);
            else if (sender == tbDataBankIBANCheckDigits) e.Handled = ! GlobalViewModel.IsValidIBAN_CheckDigitsChar(e.Text);
            else if (sender == tbDataBankIBANAccountNumber) e.Handled = ! GlobalViewModel.IsValidIBAN_AccountNumberChar(e.Text);
        }

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
                    if (sender == tbCompanyCIF) EditedParameters.Company_CIF = value;
                    else if (sender == tbCompanyName) EditedParameters.Company_Name = value;
                    else if (sender == tbCompanyAddress) EditedParameters.Company_Address = value;
                    else if (sender == tbCompanyPhone1) EditedParameters.Company_Phone_1 = value;
                    else if (sender == tbCompanyPhone2) EditedParameters.Company_Phone_2 = value;
                    else if (sender == tbCompanyFax) EditedParameters.Company_Fax = value;
                    else if (sender == tbCompanyMobilePhone) EditedParameters.Company_MobilePhone = value;
                    else if (sender == tbCompanyFax) EditedParameters.Company_Fax = value;
                    else if (sender == tbCompanyEmail) EditedParameters.Company_EMail = value;
                    else if (sender == tbDataBank_Bank) EditedParameters.DataBank_Bank = value;
                    else if (sender == tbDataBank_BankAddress) EditedParameters.DataBank_BankAddress = value;
                    else if (sender == tbDataBankIBANCountryCode) EditedParameters.DataBank_IBAN_CountryCode = value;
                    else if (sender == tbDataBankIBANBankCode) EditedParameters.DataBank_IBAN_BankCode = value;
                    else if (sender == tbDataBankIBANOfficeCode) EditedParameters.DataBank_IBAN_OfficeCode = value;
                    else if (sender == tbDataBankIBANCheckDigits) EditedParameters.DataBank_IBAN_CheckDigits = value;
                    else if (sender == tbDataBankIBANAccountNumber) EditedParameters.DataBank_IBAN_AccountNumber = value;
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedParameters);
                }
                AreDataChanged = (EditedParameters != Data);
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Manage the change of the Data in the sender object.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void TBDecimalDataChanged(object sender, TextChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                TextBox tbInput = (TextBox)sender;
                try
                {
                    GlobalViewModel.NormalizeTextBox(sender, e, DecimalType.WithoutDecimals);
                    decimal value = GlobalViewModel.GetUIDecimalValue(tbInput.Text);
                    if (sender == tbCompanyDaysVtoRisk) EditedParameters.Company_DaysVtoRisk = value;
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedParameters);
                }
                AreDataChanged = (EditedParameters != Data);
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region Button

        #region Edit

        /// <summary>
        /// Manage the button for Edit parameters.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ActualizeParametersFromDb();
                if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(Data, out string ErrMsg))
                {
                    EditedParameters = new ParametersView(m_Data);
                    CtrlOperation = Operation.Edit;
                }
                else MsgManager.ShowMessage(ErrMsg);
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, a l'iniciar l'edició dels Paràmetres Generals.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
            }
        }

        #endregion

        #region Accept

        /// <summary>
        /// Manage the button for accept the edit operation.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {
            ParametersAttributes ErrorField = ParametersAttributes.None;
            try
            {
                EditedParameters.Validate(out ErrorField);
                GlobalViewModel.Instance.HispaniaViewModel.UpdateParameters(EditedParameters);
                if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(Data, out string ErrMsg)) MsgManager.ShowMessage(ErrMsg);
                Data = new ParametersView(EditedParameters);
                CtrlOperation = Operation.Show;
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al guardar els valors editats dels Paràmetres Generals.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                EditedParameters.RestoreSourceValue(Data, ErrorField);
                LoadDataInControls(EditedParameters);
                AreDataChanged = (EditedParameters != Data);
            }
        }

        #endregion

        #region Cancel

        /// <summary>
        /// Manage the button for cancel the edit operation.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(Data, out string ErrMsg)) MsgManager.ShowMessage(ErrMsg);
                EditedParameters = null;
                LoadDataInControls(Data);
                CtrlOperation = Operation.Show;
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al cancel·lar l'edició dels Paràmetres Generals.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
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
                ActualizeParametersFromDb();
                CtrlOperation = Operation.Show;
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al refrescar els valors dels Paràmetres Generals.", MsgManager.ExcepMsg(ex)));
            }
        }

        #endregion

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
                    PostalCodesView PostalCodeSelected = ((PostalCodesView)cbCompanyPostalCode.SelectedValue);
                    EditedParameters.Company_PostalCode = PostalCodeSelected;
                    AreDataChanged = (EditedParameters != Data);
                }
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

        #region Window

        /// <summary>
        /// Clear the Registers locked for the user when the window is closed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Parameters_Closed(object sender, EventArgs e)
        {
            if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(Data, out string ErrMsg)) MsgManager.ShowMessage(ErrMsg);
        }

        #endregion

        #endregion
                        
        #region Database Operations

        private void ActualizeParametersFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.ParametersWindow);
                PostalCodes = GlobalViewModel.Instance.HispaniaViewModel.PostalCodesDict;
                Data = GlobalViewModel.Instance.HispaniaViewModel.Parameters;
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        #endregion
    }
}
