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
    /// Interaction logic for CustomersData.xaml
    /// </summary>
    public partial class AgentsData : UserControl
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
        private GridLength ViewAcceptButton = new GridLength(120.0);

        /// <summary>
        /// Show the middle column.
        /// </summary>
        private GridLength ViewMiddleColumn = new GridLength(2.0, GridUnitType.Star);

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
        /// Agent is correct.
        /// </summary>
        /// <param name="NewOrEditedAgent">New or Edited Customer.</param>
        public delegate void dlgAccept(AgentsView NewOrEditedAgent);

        /// <summary>
        /// Delegate that defines the firm of event produced when the Button Cancel is pressed.
        /// </summary>
        public delegate void dlgCancel();

        #endregion

        #region Events

        /// <summary>
        /// Event produced when the Button Accept is pressed and the data of the Agent is correct.
        /// </summary>
        public event dlgAccept EvAccept;

        /// <summary>
        /// Event produced when the Button Cancel is pressed.
        /// </summary>
        public event dlgCancel EvCancel;

        #endregion

        #region Attributes

        /// <summary>
        /// Store the Agent data to manage.
        /// </summary>
        private AgentsView m_Agent = null;

        /// <summary>
        /// Store the Cities and CP 
        /// </summary>
        public Dictionary<string, PostalCodesView> m_PostalCodes;

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
        /// Get or Set the Agent data to manage.
        /// </summary>
        public AgentsView Agent
        {
            get
            {
                return (m_Agent);
            }
            set
            {
                if (value != null)
                {
                    AreDataChanged = false;
                    m_Agent = value;
                    EditedAgent = new AgentsView(m_Agent);
                    LoadDataInControls(m_Agent, true);
                }
                else throw new ArgumentNullException("Error, no s'han trobat les dades del Representant a carregar."); 
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
                    cbAgentPostalCode.ItemsSource = m_PostalCodes;
                    cbAgentPostalCode.DisplayMemberPath = "Key";
                    cbAgentPostalCode.SelectedValuePath = "Value";
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
                         if (Agent == null) throw new InvalidOperationException("Error, impossible visualitzar un Representant sense dades.");
                         tbCancel.Text = "Tornar";
                         break;
                    case Operation.Add:
                         Agent = new AgentsView();
                         tbCancel.Text = "Cancel·lar";
                         break;
                    case Operation.Edit:
                         if (Agent == null) throw new InvalidOperationException("Error, impossible editar un Representant sense dades.");
                         tbCancel.Text = "Cancel·lar";
                         break;
                }
                foreach (Control control in EditableControls)
                {
                    if (control is TextBox) ((TextBox)control).IsReadOnly = (m_CtrlOperation == Operation.Show);
                    else if (control is RichTextBox) ((RichTextBox)control).IsReadOnly = (m_CtrlOperation == Operation.Show);
                    else control.IsEnabled = (m_CtrlOperation != Operation.Show);
                }
            }
        }

        /// <summary>
        /// Get or Set the Edited Agent information.
        /// </summary>
        private AgentsView EditedAgent
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
        /// Builder by default of this control.
        /// </summary>
        public AgentsData()
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
            NonEditableControls = new List<Control>(); 
        }

        /// <summary>
        /// Initialize the list of Editable Controls.
        /// </summary>
        private void InitEditableControls()
        {
            EditableControls = new List<Control>
            {
                lblAgentName,
                tbAgentName,
                lblAgentDNIorCIF,
                tbAgentDNIorCIF,
                lblCanceled,
                chkbCanceled,
                lblAgentAddress,
                tbAgentAddress,
                lblAgentPostalCode,
                cbAgentPostalCode,
                lblAgentPhone,
                tbAgentPhone,
                lblAgentFax,
                tbAgentFax,
                lblAgentMobilePhone,
                tbAgentMobilePhone,
                lblAgentEmail,
                tbAgentEmail,
                lblAgentComment,
                tbAgentComment
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
        /// <param name="agentsView">Data Container.</param>
        /// <param name="ThrowException">true, if want throw an exception if not found a component</param>
        private void LoadDataInControls(AgentsView agentsView, bool ThrowException = false)
        {
            DataChangedManagerActive = false;
            tbAgentName.Text = agentsView.Name;
            tbAgentDNIorCIF.Text = agentsView.DNIorCIF;
            chkbCanceled.IsChecked = agentsView.Canceled;
            tbAgentAddress.Text = agentsView.Address;
            tbAgentPhone.Text = agentsView.Phone;
            tbAgentFax.Text = agentsView.Fax;
            tbAgentMobilePhone.Text = agentsView.MobilePhone;
            tbAgentEmail.Text = agentsView.EMail;
            tbAgentComment.Text = agentsView.Comment;
            LoadExternalTablesInfo(agentsView, ThrowException);
            DataChangedManagerActive = true;
        }

        /// <summary>
        /// Load Data from External Tables.
        /// </summary>
        /// <param name="agentsView">Data Container.</param>
        /// <param name="ThrowException">true, if want throw an exception if not found a component</param>
        private void LoadExternalTablesInfo(AgentsView agentsView, bool ThrowException = false)
        {
            if ((PostalCodes != null) && (agentsView.PostalCode != null))
            {
                Dictionary<string, PostalCodesView> Items = (Dictionary<string, PostalCodesView>)cbAgentPostalCode.ItemsSource;
                string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeyPostalCodeView(agentsView.PostalCode);                
                if (Items.ContainsKey(Key)) cbAgentPostalCode.SelectedValue = PostalCodes[Key];
                else
                {
                    if (ThrowException)
                    {
                        throw new Exception(string.Format("No s'ha trobat el codi postal '{0}'.", PostalCodes[Key].Postal_Code_Info));
                    }
                }
            }
            else cbAgentPostalCode.SelectedIndex = -1;
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
                tbAgentName.PreviewTextInput += TBPreviewTextInput;
                tbAgentName.TextChanged += TBDataChanged;
                tbAgentDNIorCIF.PreviewTextInput += TBPreviewTextInput;
                tbAgentDNIorCIF.TextChanged += TBDataChanged;
                tbAgentAddress.PreviewTextInput += TBPreviewTextInput;
                tbAgentAddress.TextChanged += TBDataChanged;
                tbAgentPhone.PreviewTextInput += TBPreviewTextInput;
                tbAgentPhone.TextChanged += TBDataChanged;
                tbAgentFax.PreviewTextInput += TBPreviewTextInput;
                tbAgentFax.TextChanged += TBDataChanged;
                tbAgentMobilePhone.PreviewTextInput += TBPreviewTextInput;
                tbAgentMobilePhone.TextChanged += TBDataChanged;
                tbAgentEmail.PreviewTextInput += TBPreviewTextInput;
                tbAgentEmail.TextChanged += TBDataChanged;
                tbAgentComment.PreviewTextInput += TBPreviewTextInput;
                tbAgentComment.TextChanged += TBDataChanged;
            //  CheckBox
                chkbCanceled.Checked += ChkbCanceled_Checked;
                chkbCanceled.Unchecked += ChkbCanceled_Unchecked;
            //  ComboBox
                cbAgentPostalCode.SelectionChanged += CbAgentPostalCode_SelectionChanged;
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

        /// <summary>
        /// Checking the Input Char in function of the data type that has associated
        /// </summary>
        private void TBPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (sender == tbAgentDNIorCIF) e.Handled = ! GlobalViewModel.IsValidCIFChar(e.Text);
            else if (sender == tbAgentName) e.Handled = ! GlobalViewModel.IsValidNameChar(e.Text);
            else if (sender == tbAgentAddress) e.Handled = ! GlobalViewModel.IsValidAddressChar(e.Text);
            else if ((sender == tbAgentPhone) || (sender == tbAgentFax)) e.Handled = ! GlobalViewModel.IsValidPhoneNumberChar(e.Text);
            else if (sender == tbAgentMobilePhone) e.Handled = ! GlobalViewModel.IsValidMobilePhoneNumberChar(e.Text);
            else if (sender == tbAgentEmail) e.Handled = ! GlobalViewModel.IsValidEmailChar(e.Text);
            else if (sender == tbAgentComment) e.Handled = ! GlobalViewModel.IsValidCommentChar(e.Text);
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
                    if (sender == tbAgentDNIorCIF) EditedAgent.DNIorCIF = value;
                    else if (sender == tbAgentName) EditedAgent.Name = value;
                    else if (sender == tbAgentAddress) EditedAgent.Address = value;
                    else if (sender == tbAgentPhone) EditedAgent.Phone = value;
                    else if (sender == tbAgentFax) EditedAgent.Fax = value;
                    else if (sender == tbAgentMobilePhone) EditedAgent.MobilePhone = value;
                    else if (sender == tbAgentEmail) EditedAgent.EMail = value;
                    else if (sender == tbAgentComment) EditedAgent.Comment = value;
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedAgent);
                }
                AreDataChanged = (EditedAgent != Agent);
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
            AgentsAttributes ErrorField = AgentsAttributes.None;
            try
            {
                if ((CtrlOperation == Operation.Add) || (CtrlOperation == Operation.Edit))
                {
                    EditedAgent.Validate(out ErrorField);
                    EvAccept?.Invoke(new AgentsView(EditedAgent));
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

        #region CheckBox

        private void ChkbCanceled_Unchecked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                EditedAgent.Canceled = false;
                AreDataChanged = (EditedAgent != Agent);
                DataChangedManagerActive = true;
            }
        }

        private void ChkbCanceled_Checked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                EditedAgent.Canceled = true;
                AreDataChanged = (EditedAgent != Agent);
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region ComboBox

        /// <summary>
        /// Manage the change of the Data in the combobox of CP.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CbAgentPostalCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                if (cbAgentPostalCode.SelectedItem != null)
                {
                    PostalCodesView PostalCodeSelected = ((PostalCodesView)cbAgentPostalCode.SelectedValue);
                    EditedAgent.PostalCode = PostalCodeSelected;
                    AreDataChanged = (EditedAgent != Agent);
                }
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
            EditedAgent.RestoreSourceValues(Agent);
            LoadDataInControls(EditedAgent);
            AreDataChanged = (EditedAgent != Agent);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(AgentsAttributes ErrorField)
        {
            EditedAgent.RestoreSourceValue(Agent, ErrorField);
            LoadDataInControls(EditedAgent);
            AreDataChanged = (EditedAgent != Agent);
        }

        #endregion
    }
}
