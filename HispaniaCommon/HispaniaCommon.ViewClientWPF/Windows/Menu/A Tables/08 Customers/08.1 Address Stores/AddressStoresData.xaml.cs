#region Librerias usadas por la clase

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
    public partial class AddressStoresData : UserControl
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
        /// Customer is correct.
        /// </summary>
        /// <param name="NewOrEditedAddressStore">New or Edited AddressStore.</param>
        public delegate void dlgAccept(AddressStoresView NewOrEditedAddressStore);

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
        /// Store the AddressStore data to manage.
        /// </summary>
        private AddressStoresView m_AddressStore = null;

        /// <summary>
        /// Store the Cities and CP 
        /// </summary>
        public Dictionary<string, PostalCodesView> m_PostalCodes;

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private CustomersView m_Data = null;

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private ProvidersView m_DataProv = null;

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
        /// Get or Set the AddressStore data to manage.
        /// </summary>
        public AddressStoresView AddressStore
        {
            get
            {
                return (m_AddressStore);
            }
            set
            {
                if (value != null)
                {
                    AreDataChanged = false;
                    m_AddressStore = value;
                    EditedAddressStore = new AddressStoresView(m_AddressStore);
                    LoadDataInControls(m_AddressStore, true);
                }
                else throw new ArgumentNullException("Error, no s'han trobat les dades de l'adreça del magatzem a carregar."); 
            }
        }

        /// <summary>
        /// Get or Set the Edited Customer information.
        /// </summary>
        private AddressStoresView EditedAddressStore
        {
            get;
            set;
        }

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        public CustomersView Data
        {
            get
            {
                return (m_Data);
            }
            set
            {
                if (value != null) m_Data = value;
                else throw new ArgumentNullException("Error, no s'ha trobat l'Article a carregar.");
            }
        }

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        public ProvidersView DataProv
        {
            get
            {
                return (m_DataProv);
            }
            set
            {
                if (value != null) m_DataProv = value;
                else throw new ArgumentNullException("Error, no s'ha trobat l'Article a carregar.");
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
                         if (AddressStore == null) throw new InvalidOperationException("Error, impossible visualitzar una adreça de magatzem sense dades.");
                         tbCancel.Text = "Tornar";
                         break;
                    case Operation.Add:
                        if (DataProv!=null)
                        {
                            AddressStore = new AddressStoresView(DataProv.Provider_Id);
                        }
                        else
                        {
                            AddressStore = new AddressStoresView(Data.Customer_Id);
                        }
                         
                         tbCancel.Text = "Cancel·lar";
                         break;
                    case Operation.Edit:
                         if (AddressStore == null) throw new InvalidOperationException("Error, impossible editar una adreá de magatzem sense dades.");
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
                    cbPostalCode.ItemsSource = m_PostalCodes;
                    cbPostalCode.DisplayMemberPath = "Key";
                    cbPostalCode.SelectedValuePath = "Value";
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
        public AddressStoresData()
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
                lblContactPerson,
                tbContactPerson,
                lblTimetable,
                tbTimetable,
                lblPhone,
                tbPhone,
                lblFAX,
                tbFAX,
                lblStore_Address,
                tbStore_Address,
                lblPostalCode,
                cbPostalCode,
                tbRemarks
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
        /// <param name="storeAddresessView">Data Container.</param>
        /// <param name="ThrowException">true, if want throw an exception if not found a component</param>
        private void LoadDataInControls(AddressStoresView storeAddresessView, bool ThrowException = false)
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Controls
                tbContactPerson.Text = storeAddresessView.ContactPerson;
                tbTimetable.Text = storeAddresessView.Timetable;
                tbPhone.Text = storeAddresessView.Phone;
                tbFAX.Text = storeAddresessView.FAX;
                tbStore_Address.Text = storeAddresessView.Address;
                tbRemarks.Text = storeAddresessView.Remarks;
                LoadExternalTablesInfo(storeAddresessView, ThrowException);
            //  Activate managers
                DataChangedManagerActive = true;
        }

        /// <summary>
        /// Load Data from External Tables.
        /// </summary>
        /// <param name="parametersView">Data Container.</param>
        /// <param name="ThrowException">true, if want throw an exception if not found a component</param>
        private void LoadExternalTablesInfo(AddressStoresView storeAddresessView, bool ThrowException = false)
        {
            if ((PostalCodes != null) && (storeAddresessView.PostalCode != null))
            {

                Dictionary<string, PostalCodesView> Items = (Dictionary<string, PostalCodesView>)cbPostalCode.ItemsSource;
                string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeyPostalCodeView(storeAddresessView.PostalCode);
                if (Items.ContainsKey(Key)) cbPostalCode.SelectedValue = PostalCodes[Key];
                else
                {
                    if (ThrowException)
                    {
                        throw new Exception(string.Format("No s'ha trobat el codi postal '{0}'.", PostalCodes[Key].Postal_Code_Info));
                    }
                }
            }
            else cbPostalCode.SelectedIndex = -1;
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
                tbContactPerson.TextChanged += TBDataChanged;
                tbContactPerson.PreviewTextInput += TBPreviewTextInput;
                tbTimetable.TextChanged += TBDataChanged;
                tbTimetable.PreviewTextInput += TBPreviewTextInput;
                tbPhone.TextChanged += TBDataChanged;
                tbPhone.PreviewTextInput += TBPreviewTextInput;
                tbFAX.TextChanged += TBDataChanged;
                tbFAX.PreviewTextInput += TBPreviewTextInput;
                tbStore_Address.TextChanged += TBDataChanged;
                tbStore_Address.PreviewTextInput += TBPreviewTextInput;
                tbRemarks.TextChanged += TBDataChanged;
                tbRemarks.PreviewTextInput += TBPreviewTextInput;
            //  ComboBox
                cbPostalCode.SelectionChanged += CbPostalCode_SelectionChanged;
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
            if (sender == tbContactPerson)
            {
                e.Handled = ! GlobalViewModel.IsValidNameChar(e.Text);
            }
            else if ((sender == tbTimetable) || (sender == tbRemarks))
            {
                e.Handled = ! GlobalViewModel.IsValidTimeTableChar(e.Text);
            }
            else if ((sender == tbPhone) || (sender == tbFAX))
            {
                e.Handled = ! GlobalViewModel.IsValidPhoneNumberChar(e.Text);
            }
            else if (sender == tbStore_Address) e.Handled = ! GlobalViewModel.IsValidAddressChar(e.Text);
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
                    if (sender == tbContactPerson) EditedAddressStore.ContactPerson = value;
                    else if (sender == tbTimetable) EditedAddressStore.Timetable = value;
                    else if (sender == tbPhone) EditedAddressStore.Phone = value;
                    else if (sender == tbFAX) EditedAddressStore.FAX = value;
                    else if (sender == tbStore_Address) EditedAddressStore.Address = value;
                    else if (sender == tbRemarks) EditedAddressStore.Remarks = value;
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedAddressStore);
                }
                AreDataChanged = (EditedAddressStore != AddressStore);
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
            AddressStoresAttributes ErrorField = AddressStoresAttributes.None;
            try
            {
                if ((CtrlOperation == Operation.Add) || (CtrlOperation == Operation.Edit))
                {
                    EditedAddressStore.Validate(out ErrorField);
                    EvAccept?.Invoke(new AddressStoresView(EditedAddressStore));
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
        private void CbPostalCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                if (cbPostalCode.SelectedItem != null)
                {
                    PostalCodesView postalcodeSelected = ((PostalCodesView)cbPostalCode.SelectedValue);
                    EditedAddressStore.PostalCode = postalcodeSelected;
                    AreDataChanged = (EditedAddressStore != AddressStore);
                }
            }
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValues()
        {
            EditedAddressStore.RestoreSourceValues(AddressStore);
            LoadDataInControls(EditedAddressStore);
            AreDataChanged = (EditedAddressStore != AddressStore);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(AddressStoresAttributes ErrorField)
        {
            EditedAddressStore.RestoreSourceValue(AddressStore, ErrorField);
            LoadDataInControls(EditedAddressStore);
            AreDataChanged = (EditedAddressStore != AddressStore);
        }

        #endregion
    }
}
