#region Librerias usadas por la ventana

using HispaniaCommon.ViewClientWPF.UserControls;
using HispaniaCommon.ViewModel;
using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.Theme;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

#endregion

namespace HispaniaCommon.ViewClientWPF.Windows
{
    /// <summary>
    /// Interaction logic for Parameters.xaml
    /// </summary>
    public partial class ProviderOrderMovementsData : Window
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
        /// Show the button.
        /// </summary>
        private GridLength ViewButton = new GridLength(120.0);

        /// <summary>
        /// Show the control.
        /// </summary>
        private GridLength ViewlblUnitShipping = new GridLength(150.0);

        /// <summary>
        /// Show the control.
        /// </summary>
        private GridLength ViewtbUnitShipping = new GridLength(150.0);

        /// <summary>
        /// Show the control.
        /// </summary>
        private GridLength ViewlblUnitShippingDefinition = new GridLength(90.0);

        /// <summary>
        /// Show the control.
        /// </summary>
        private GridLength ViewtbUnitShippingDefinition = new GridLength(1.0, GridUnitType.Star);

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
        private ProviderOrderMovementsView m_Data= null;

        /// <summary>
        /// Store the Goods
        /// </summary>
        private Dictionary<string, GoodsView> m_Goods;

        /// <summary>
        /// Store the type of Application with the user want open.
        /// </summary>
        private ApplicationType m_AppType;

        /// <summary>
        /// Stotre if the data of the Provider has changed.
        /// </summary>
        private bool m_AreDataChanged;

        /// <summary>
        /// Store the Operation of the UserControl.
        /// </summary>
        private Operation m_CtrlOperation = Operation.Show;

        /// <summary>
        /// Window instance of HistoProviders.
        /// </summary>
        private HistoProviders HistoProvidersWindow = null;

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
        public ProviderOrderMovementsView Data
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
                    m_Data = value;
                    EditedProviderOrderMovement = new ProviderOrderMovementsView(m_Data);
                    if (m_Data.Good != null)
                    {
                        string GoodKey = GlobalViewModel.Instance.HispaniaViewModel.GetKeyGoodView(m_Data.Good);
                        if (!((SortedDictionary<string, GoodsView>)cbGood.ItemsSource).ContainsKey(GoodKey))
                        {
                            ((SortedDictionary<string, GoodsView>)cbGood.ItemsSource).Add(GoodKey, new GoodsView(m_Data.Good));
                        }
                    }
                    LoadDataInControls(m_Data);
                    tbItemToSearch.Focus();
                }
                else throw new ArgumentNullException("Error, no s'han trobat les dades del moviment a carregar.");
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
                         cbCancelButton.Width = HideColumn;
                         gbGeneralData.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                         break;
                    case Operation.Add:
                    case Operation.Edit:
                         AreDataChanged = false;
                         gbGeneralData.SetResourceReference(Control.StyleProperty, "EditableGroupBox");
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
        /// Get or Set the Edited Provider Order Movement information.
        /// </summary>
        public ProviderOrderMovementsView EditedProviderOrderMovement
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set if the data of the Provider has changed.
        /// </summary>
        internal bool AreDataChanged
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
                    cbCancelButton.Width = ViewButton;
                }
                else
                {
                    cbAcceptButton.Width = HideColumn;
                    cbAcceptButtonExt.Width = HideColumn;
                    cbCancelButton.Width = ViewButton;
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

        public bool IsCanceled
        {
            get;
            private set;
        }

        #region Foreign Keys

        /// <summary>
        /// Get or Set the Providers 
        /// </summary>
        public Dictionary<string, GoodsView> Goods
        {
            get
            {
                return (m_Goods);
            }
            set
            {
                if (value is null) m_Goods = new Dictionary<string, GoodsView>();
                else m_Goods = value;
                cbGood.ItemsSource = new SortedDictionary<string, GoodsView>(m_Goods);
                cbGood.DisplayMemberPath = "Key";
                cbGood.SelectedValuePath = "Value";
                CollectionViewSource.GetDefaultView(cbGood.ItemsSource).Filter = UserFilter;
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
        /// Main Builder of the windows.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        public ProviderOrderMovementsData(ApplicationType AppType)
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
            //  Load Data in Window components.
                LoadDataInWindowComponents();
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
                lblGood,
                cbGood,
                lblUnitShipping,
                tbUnitShipping,
                lblUnitBilling,
                tbUnitBilling,
                lblRetailPrice,
                tbRetailPrice,                
                lblRemark,
                tbRemark,
                lblInternalRemark,
                tbInternalRemark,
                lblAccording,
                chkbAccording,
                lblGoodDescription,
                tbGoodDescription,
                cbFieldItemToSearch,
                tbItemToSearch,
                btnAcceptSearch,
                btnHistoric,
                tbNameClientAssoc,
            };
        }

        /// <summary>
        /// Initialize the list of Editable Controls.
        /// </summary>
        private void InitNonEditableControls()
        {
            NonEditableControls = new List<Control>
            {
                lblProviderOrderId,
                tbProviderOrderId,
                lblValue,
                tbValue,
                lblProviderOrderMovementId,
                tbProviderOrderMovementId,
                lblUnitShippingDefinition,
                tbUnitShippingDefinition,
                lblUnitBillingDefinition,
                tbUnitBillingDefinition,
                tbShippingUnitAvailable,
                tbBillingUnitAvailable
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
        /// Method that load data in Window components.
        /// </summary>
        private void LoadDataInWindowComponents()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Set Data into the Window.
                cbFieldItemToSearch.ItemsSource = GoodsView.Fields;
                cbFieldItemToSearch.DisplayMemberPath = "Key";
                cbFieldItemToSearch.SelectedValuePath = "Value";
                if (GoodsView.Fields.Count > 0) cbFieldItemToSearch.SelectedIndex = 0;
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        /// <summary>
        /// Load the data of the Company into the Window
        /// </summary>
        private void LoadDataInControls(ProviderOrderMovementsView movement)
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  ProviderOrder Controls
                tbProviderOrderId.Text = GlobalViewModel.GetStringFromIntIdValue(movement.ProviderOrder.ProviderOrder_Id);
                tbProviderOrderMovementId.Text = movement.ProviderOrderMovement_Id_Str;
                if ((Goods != null) && (movement.Good != null))
                {
                    string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeyGoodView(movement.Good);
                    cbGood.SelectedValue = Goods[Key];
                    tbShippingUnitAvailable.Text = Goods[Key].Shipping_Unit_Available_Str;
                    tbBillingUnitAvailable.Text = Goods[Key].Billing_Unit_Available_Str;
                    UpdateFilterValueField((GoodsView)cbGood.SelectedValue);
                }
                else
                {
                    cbGood.SelectedIndex = -1;
                    UpdateFilterValueField();
                }
                tbGoodDescription.Text = movement.Description;
                tbUnitBilling.Text = GlobalViewModel.GetStringFromDecimalValue(movement.Unit_Billing, DecimalType.Unit);
                tbUnitBillingDefinition.Text = movement.Unit_Billing_Definition;
                tbUnitShipping.Text = GlobalViewModel.GetStringFromDecimalValue(movement.Unit_Shipping, DecimalType.Unit); 
                tbUnitShippingDefinition.Text = movement.Unit_Shipping_Definition;
                UpdateUnitsDataAndFieldsState(movement);
                tbRetailPrice.Text = GlobalViewModel.GetStringFromDecimalValue(movement.RetailPrice, DecimalType.Currency);
                chkbAccording.IsChecked = movement.According;
                tbRemark.Text = movement.Remark;
                tbInternalRemark.Text = movement.Internal_Remark;

                tbNameClientAssoc.Text = movement.ClientName;

            //  Reactivate managers
                DataChangedManagerActive = true;
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
                GoodsView ItemData = ((KeyValuePair<string, GoodsView>)item).Value;
                String ProperyName = (string) cbFieldItemToSearch.SelectedValue;
            //  Apply the filter by selected field value
                if (!String.IsNullOrEmpty(tbItemToSearch.Text))
                {
                    object valueToTest = ItemData.GetType().GetProperty(ProperyName).GetValue(ItemData);
                    if ((valueToTest is null) || 
                        (!(valueToTest.ToString().ToUpper()).StartsWith(tbItemToSearch.Text.ToUpper())))
                    {
                        return false;
                    }
                }
                return true;
        }

        #endregion

        #region Managers

        /// <summary>
        /// Method that define the managers needed for the user operations in the Window
        /// </summary>
        private void LoadManagers()
        {
            //  By default the manager for the Provider Data changes is active.
                DataChangedManagerActive = false;
                cbGood.ItemContainerStyle.Setters.Add(new EventSetter(MouseMoveEvent, new MouseEventHandler(OnMouseMove)));
            //  Additional Data Managers
                tbUnitBilling.PreviewTextInput += TBPreviewTextInput;
                tbUnitBilling.TextChanged += TBUnitDataChanged;
                tbUnitShipping.PreviewTextInput += TBPreviewTextInput;
                tbUnitShipping.TextChanged += TBUnitDataChanged;
                tbRetailPrice.PreviewTextInput += TBPreviewTextInput;
                tbRetailPrice.TextChanged += TBCurrencyDataChanged;                
                tbRemark.PreviewTextInput += TBPreviewTextInput;
                tbRemark.TextChanged += TBDataChanged;
                tbInternalRemark.PreviewTextInput += TBPreviewTextInput;
                tbInternalRemark.TextChanged += TBDataChanged;
                tbGoodDescription.PreviewTextInput += TBPreviewTextInput;
                tbGoodDescription.TextChanged += TBDataChanged;
                tbItemToSearch.TextChanged += TBItemToSearchDataChanged;
                
                tbNameClientAssoc.PreviewTextInput += TBPreviewTextInput;
                tbNameClientAssoc.TextChanged += TBDataChanged;

            //  CheckBox
            chkbAccording.Checked += ChkbAccording_Checked;
                chkbAccording.Unchecked += ChkbAccording_Unchecked;
            //  ComboBox
                cbFieldItemToSearch.SelectionChanged += CbFieldItemToSearch_SelectionChanged;
                cbGood.PreviewKeyUp += CbGood_PreviewKeyUp;
                //cbGood.MouseMove += CbGood_MouseMove;
                cbGood.DropDownClosed += CbGood_DropDownClosed;
            //  Button managers
                btnAccept.Click += BtnAccept_Click;
                btnCancel.Click += BtnCancel_Click;
                btnAcceptSearch.Click += BtnAcceptSearch_Click;
                btnHistoric.Click += BtnHistoric_Click;
            //  Activate managers
                DataChangedManagerActive = true;
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

        /// <summary>
        /// Checking the Input Char in function of the data type that has associated
        /// </summary>
        private void TBPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if ((sender == tbUnitBilling) || (sender == tbUnitShipping))
            {
                e.Handled = ! GlobalViewModel.IsValidUnitChar(e.Text);
            }
            if (sender == tbRetailPrice)
            {
                e.Handled = ! GlobalViewModel.IsValidCurrencyChar(e.Text);
            }            
            else if ((sender == tbRemark) || (sender == tbGoodDescription) || (sender == tbInternalRemark) 
                    || (sender == tbNameClientAssoc) )
            {
                e.Handled = ! GlobalViewModel.IsValidCommentChar(e.Text);
            } 
        }

        /// <summary>
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
                    if (sender == tbRemark) EditedProviderOrderMovement.Remark = value;
                    else if (sender == tbInternalRemark) EditedProviderOrderMovement.Internal_Remark = value;
                    else if (sender == tbGoodDescription) EditedProviderOrderMovement.Description = value;
                    else if(sender == tbNameClientAssoc )
                        EditedProviderOrderMovement.ClientName = value;
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedProviderOrderMovement);
                }
                AreDataChanged = (EditedProviderOrderMovement != Data);
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Manage the change of the Data in the sender object.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void TBUnitDataChanged(object sender, TextChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                TextBox tbInput = (TextBox)sender;
                try
                {
                    GlobalViewModel.NormalizeTextBox(sender, e, DecimalType.Unit);
                    decimal value = GlobalViewModel.GetUIDecimalValue(tbInput.Text);
                    if (sender == tbUnitBilling) EditedProviderOrderMovement.Unit_Billing = value;
                    else if (sender == tbUnitShipping)
                    {
                        EditedProviderOrderMovement.Unit_Shipping = value;
                        if ((EditedProviderOrderMovement.Good != null) &&
                            (EditedProviderOrderMovement.Good.Conversion_Factor != 0))
                        {
                            decimal NewUnitBilling = EditedProviderOrderMovement.Unit_Shipping *
                                                     EditedProviderOrderMovement.Good.Conversion_Factor;
                            EditedProviderOrderMovement.Unit_Billing = NewUnitBilling;
                            tbUnitBilling.Text = GlobalViewModel.GetStringFromDecimalValue(NewUnitBilling, DecimalType.Unit);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedProviderOrderMovement);
                }
                AreDataChanged = (EditedProviderOrderMovement != Data);
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
                    if (sender == tbRetailPrice) EditedProviderOrderMovement.RetailPrice = value;                    
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedProviderOrderMovement);
                }
                AreDataChanged = (EditedProviderOrderMovement != Data);
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region ComboBox

        /// <summary>
        /// Manage the change of the Data in the Field Item Search ComboBox.
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
        /// Manage the Key item movoment in Goods combobox.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CbGood_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                if ((e.Key == Key.Up) || (e.Key == Key.Down))
                {
                    GoodsView good = ((KeyValuePair<string, GoodsView>)((ComboBoxItem)e.OriginalSource).Content).Value;
                    UpdateFilterValueField(good);
                }
                DataChangedManagerActive = true;
            }
        }

        private void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                GoodsView good = ((KeyValuePair<string, GoodsView>)((ComboBoxItem)sender).Content).Value;
                UpdateFilterValueField(good);
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Manage the DropDown Closed focus for Goods combobox.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CbGood_DropDownClosed(object sender, EventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                UpdateFilterValueField((GoodsView)cbGood.SelectedValue);
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region CheckBox

        private void ChkbAccording_Unchecked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                EditedProviderOrderMovement.According = false;
                AreDataChanged = (EditedProviderOrderMovement != Data);
                DataChangedManagerActive = true;
            }
        }

        private void ChkbAccording_Checked(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                EditedProviderOrderMovement.According = true;
                AreDataChanged = (EditedProviderOrderMovement != Data);
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region Buttons

        #region Accept

        /// <summary>
        /// Manage the button for accept the edit operation.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {
            ProviderOrderMovementsAttributes ErrorField = ProviderOrderMovementsAttributes.None;
            try
            {
                EditedProviderOrderMovement.Validate(out ErrorField);
                IsCanceled = false;                
                Close();
            }
            catch (FormatException fex)
            {
                MsgManager.ShowMessage(MsgManager.ExcepMsg(fex));
                EditedProviderOrderMovement.RestoreSourceValue(Data, ErrorField);
                LoadDataInControls(EditedProviderOrderMovement);
                AreDataChanged = (EditedProviderOrderMovement != Data);
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(MsgManager.ExcepMsg(ex), MsgType.Error);
                EditedProviderOrderMovement.RestoreSourceValue(Data, ErrorField);
                LoadDataInControls(EditedProviderOrderMovement);
                AreDataChanged = (EditedProviderOrderMovement != Data);
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
            IsCanceled = true;
            Close();
        }

        #endregion
        
        #region Historic (Update Data)

        private void BtnHistoric_Click(object sender, RoutedEventArgs e)
        {
            if (HistoProvidersWindow == null)
            {
                ProvidersView Provider = EditedProviderOrderMovement.ProviderOrder.Provider;
                HistoProvidersWindow = new HistoProviders(AppType, HistoProvidersMode.ProviderOrderMovementLoad)
                {
                    DataList = GlobalViewModel.Instance.HispaniaViewModel.GetHistoProviders(Provider.Provider_Id, true),
                    Data = Provider
                };
                HistoProvidersWindow.Closed += HistoProvidersWindow_Closed;
                HistoProvidersWindow.Show();
            }
            else HistoProvidersWindow.Activate();
        }

        private void HistoProvidersWindow_Closed(object sender, EventArgs e)
        {
            //  Actualize List of Provider Order Movements from historic of movements.
                foreach (HistoProvidersView histoProvider in HistoProvidersWindow.HistoProviderSelected)
                {
                    EditedProviderOrderMovement = new ProviderOrderMovementsView()
                    {
                        ProviderOrderMovement_Id = Data.ProviderOrderMovement_Id,
                        ProviderOrder = Data.ProviderOrder,
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
                    string GoodKey = GlobalViewModel.Instance.HispaniaViewModel.GetKeyGoodView(histoProvider.Good);
                    if (!((SortedDictionary<string, GoodsView>)cbGood.ItemsSource).ContainsKey(GoodKey))
                    {
                        ((SortedDictionary<string, GoodsView>)cbGood.ItemsSource).Add(GoodKey, new GoodsView(histoProvider.Good));
                    }
                }
                LoadDataInControls(EditedProviderOrderMovement);
                AreDataChanged = (Data != EditedProviderOrderMovement);
            //  Undefine the close event manager.
                HistoProvidersWindow.Closed -= HistoProvidersWindow_Closed;
                HistoProvidersWindow = null;
        }

        #endregion

        #region Accept Good Selection

        private void BtnAcceptSearch_Click(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                if (cbGood.SelectedItem != null)
                {
                    GoodsView goodSelected = ((GoodsView)cbGood.SelectedValue);
                    if ((EditedProviderOrderMovement.Good is null) || (EditedProviderOrderMovement.Good.Good_Id != goodSelected.Good_Id))
                    {
                        EditedProviderOrderMovement.Good = goodSelected;
                        EditedProviderOrderMovement.Description = goodSelected.Good_Description;
                        EditedProviderOrderMovement.Unit_Shipping_Definition = goodSelected.Good_Unit.Shipping;
                        EditedProviderOrderMovement.Unit_Billing_Definition = goodSelected.Good_Unit.Billing;
                        if (EditedProviderOrderMovement.Good.Conversion_Factor != 0)
                        {
                            decimal NewUnitBilling = EditedProviderOrderMovement.Unit_Shipping *
                                                     EditedProviderOrderMovement.Good.Conversion_Factor;
                            EditedProviderOrderMovement.Unit_Billing = NewUnitBilling;
                            tbUnitBilling.Text = GlobalViewModel.GetStringFromDecimalValue(NewUnitBilling, DecimalType.Unit);
                        }
                        ActualizeGoodInformation(goodSelected);
                        //UpdateUnitsDataAndFieldsState(EditedProviderOrderMovement);
                        EditedProviderOrderMovement.Unit_Billing = 0;
                        EditedProviderOrderMovement.Unit_Shipping = 0;
                        EditedProviderOrderMovement.RetailPrice = 0;
                        EditedProviderOrderMovement.Comission = 0;
                        EditedProviderOrderMovement.According = false;
                        EditedProviderOrderMovement.Remark = string.Empty;
                        EditedProviderOrderMovement.Internal_Remark = string.Empty;
                        AreDataChanged = (EditedProviderOrderMovement != Data);
                        LoadDataInControls(EditedProviderOrderMovement);
                        tbGoodDescription.Focus();
                    }
                }
                else MsgManager.ShowMessage("Error, no hi ha cap Artícle seleccionat.");
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #endregion

        #endregion

        #region Update UI

        private void UpdateUnitsDataAndFieldsState(ProviderOrderMovementsView movement)
        {
            bool AreUnitShippingDefined = !String.IsNullOrEmpty(movement.Unit_Shipping_Definition);
            cdlblUnitShipping.Width = AreUnitShippingDefined ? ViewlblUnitShipping : HideColumn;
            cdtbUnitShipping.Width = AreUnitShippingDefined ? ViewtbUnitShipping : HideColumn;
            cdlblUnitShippingDefinition.Width = AreUnitShippingDefined ? ViewlblUnitShippingDefinition : HideColumn;
            cdtbUnitShippingDefinition.Width = AreUnitShippingDefined ? ViewtbUnitShippingDefinition : HideColumn;
        }

        private void FilterDataListObjects()
        {
            CollectionViewSource.GetDefaultView(cbGood.ItemsSource).Refresh();
            if (cbGood.Items.Count > 0)
            {
                cbGood.SelectedIndex = 0;
                UpdateFilterValueField((GoodsView)cbGood.SelectedValue);
            }
        }

        private void UpdateFilterValueField(GoodsView ItemData = null)
        {
            if (ItemData is null)
            {
                tbShippingUnitAvailable.Text = string.Empty;
                tbBillingUnitAvailable.Text = string.Empty;
                tbValue.Text = string.Empty;
            }
            else
            {
                String ProperyName = (string)cbFieldItemToSearch.SelectedValue;
                object valueToTest = ItemData.GetType().GetProperty(ProperyName).GetValue(ItemData);
                tbValue.Text = (valueToTest is null) || (valueToTest.ToString() == string.Empty) ? string.Empty : valueToTest.ToString();
            }
        }

        private void ActualizeGoodInformation(GoodsView good)
        {
            if (good != null)
            {
                tbGoodDescription.Text = good.Good_Description;
                tbUnitBillingDefinition.Text = good.Good_Unit.Billing;
                tbUnitShippingDefinition.Text = good.Good_Unit.Shipping;
                tbShippingUnitAvailable.Text = good.Shipping_Unit_Available_Str;
                tbBillingUnitAvailable.Text = good.Billing_Unit_Available_Str;
            }
        }

        #endregion
    }
}
