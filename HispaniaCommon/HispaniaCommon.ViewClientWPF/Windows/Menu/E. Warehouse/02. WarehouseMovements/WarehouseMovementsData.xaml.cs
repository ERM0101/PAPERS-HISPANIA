#region Libraries used for this control

using HispaniaCommon.ViewModel;
using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.Theme;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

#endregion

namespace HispaniaCommon.ViewClientWPF.UserControls
{
    /// <summary>
    /// Interaction logic for WarehouseMovementsData.xaml
    /// </summary>
    public partial class WarehouseMovementsData : UserControl
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
        /// Good is correct.
        /// </summary>
        /// <param name="NewOrEditedWarehouseMovement">New or Edited Good.</param>
        public delegate void dlgAccept(WarehouseMovementsView NewOrEditedWarehouseMovement);

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
        /// Store the Good data to manage.
        /// </summary>
        private WarehouseMovementsView m_WarehouseMovements = null;

        /// <summary>
        /// Get or Set if the manager of the data change for the Good has active.
        /// </summary>
        private Dictionary<int, string> m_WarehouseMovementType = new Dictionary<int, string>()
        {
            { 1, "ENTRADES" },
            { 5, "SORTIDES" }
        };

        /// <summary>
        /// Store the Goods
        /// </summary>
        public Dictionary<string, GoodsView> m_Goods;

        /// <summary>
        /// Store the Goods
        /// </summary>
        public Dictionary<string, ProvidersView> m_Providers;

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
        /// Get or Set the Good data to manage.
        /// </summary>
        public WarehouseMovementsView WarehouseMovement
        {
            get
            {
                return (m_WarehouseMovements);
            }
            set
            {
                if (value != null)
                {
                    AreDataChanged = false;
                    m_WarehouseMovements = value;
                    EditedWarehouseMovement = new WarehouseMovementsView(m_WarehouseMovements);
                    if (m_WarehouseMovements.Good != null)
                    {
                        string GoodKey = GlobalViewModel.Instance.HispaniaViewModel.GetKeyGoodView(m_WarehouseMovements.Good);
                        if (!((SortedDictionary<string, GoodsView>)cbGoodCode.ItemsSource).ContainsKey(GoodKey))
                        {
                            ((SortedDictionary<string, GoodsView>)cbGoodCode.ItemsSource).Add(GoodKey, new GoodsView(m_WarehouseMovements.Good));
                        }
                    }
                    ResetFilters();
                    LoadDataInControls(m_WarehouseMovements, true, 1);
                }
                else throw new ArgumentNullException("Error, no s'han trobat les dades del moviment carregar."); 
            }
        }

        /// <summary>
        /// Get or Set the Edited Warehouse Movement information.
        /// </summary>
        private WarehouseMovementsView EditedWarehouseMovement
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set the Selected Good information.
        /// </summary>
        private GoodsView SelectedGood
        {
            get;
            set;
        }

        private decimal Selected_Amount_Unit_Billing
        {
            get;
            set;
        }

        private decimal Selected_Amount_Unit_Shipping
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
                         if (WarehouseMovement == null) throw new InvalidOperationException("Error, impossible visualitzar un Moviment de Magatzem sense dades.");
                         tbCancel.Text = "Tornar";
                         break;
                    case Operation.Add:
                         WarehouseMovement = new WarehouseMovementsView();
                         tbCancel.Text = "Cancel·lar";
                         break;
                    case Operation.Edit:
                         if (WarehouseMovement == null) throw new InvalidOperationException("Error, impossible editar un Moviment de Magatzem sense dades.");
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
                tbItemToSearch.Focus();
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
        /// Get or Set if the manager of the data change for the WarehouseMovement has active.
        /// </summary>
        private bool DataChangedManagerActive
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set if the manager of the data change for the Good has active.
        /// </summary>
        private Dictionary<int, string> WarehouseMovementType
        {
            get
            {
                return m_WarehouseMovementType;
            }
        }

        #region Foreign Keys

        /// <summary>
        /// Get or Set Goods
        /// </summary>
        public Dictionary<string, GoodsView> Goods
        {
            get
            {
                return (m_Goods);
            }
            set
            {
                m_Goods = value;
                if (m_Goods != null)
                {
                    cbGoodCode.ItemsSource = new SortedDictionary<string, GoodsView>(m_Goods);
                    cbGoodCode.DisplayMemberPath = "Key";
                    cbGoodCode.SelectedValuePath = "Value";
                    CollectionViewSource.GetDefaultView(cbGoodCode.ItemsSource).Filter = UserFilter;
                }
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
                m_Providers = value;
                if (m_Providers != null)
                {
                    cbProvider.ItemsSource = new SortedDictionary<string, ProvidersView>(m_Providers);
                    cbProvider.DisplayMemberPath = "Key";
                    cbProvider.SelectedValuePath = "Value";
                    CollectionViewSource.GetDefaultView(cbProvider.ItemsSource).Filter = UserFilterProvider;
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
        public WarehouseMovementsData()
        {
            //  Initialization of controls of the UserControl
                InitializeComponent();
            //  Initialize GUI.
                InitEditableControls();
                InitNonEditableControls();
                InitOnlyQueryControls();
            //  Initialize Controls
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
                lblWarehouseMovement_Id,
                tbWarehouseMovement_Id,
                lblDate,
                tbDate,
                tbGoodDescription,
                tbUnitBillingDefinition,
                tbUnitShippingDefinition,
                tbAmount,
                tbPriceCost,
                tbAveragePriceCost,
                lblAccording,
                chkbAccording,
                tbValue,
                lblGoodCode,
                cbGoodCode,
                lblAmountUnitBilling,
                tbAmountUnitBilling,
                lblAmountUnitShipping,
                tbAmountUnitShipping,
                tbBillingUnitStocks,
                tbShippingUnitStocks,
                tbBillingUnitAvailable,
                tbShippingUnitAvailable,
                tbBillingUnitEntrance,
                tbShippingUnitEntrance,
                tbUnitBillingDefinition,
                tbUnitShippingDefinition,
                cbFieldItemToSearch,
                tbItemToSearch,
                btnAcceptSearch,
                lblType,
                cbType,
                gbGoods
            };
        }

        /// <summary>
        /// Initialize the list of Editable Controls.
        /// </summary>
        private void InitEditableControls()
        {
            EditableControls = new List<Control>
            {
                lblPrice,
                tbPrice,
                lblProvider,
                cbProvider,
                cbFieldItemToSearchProvider,
                tbItemToSearchProvider,
                btnAcceptSearchProvider,
                gbProviders
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
                cbType.ItemsSource = WarehouseMovementType;
                cbType.DisplayMemberPath = "Value";
                cbType.SelectedValuePath = "Key";
                cbFieldItemToSearch.ItemsSource = GoodsView.Fields;
                cbFieldItemToSearch.DisplayMemberPath = "Key";
                cbFieldItemToSearch.SelectedValuePath = "Value";
                if (GoodsView.Fields.Count > 0) cbFieldItemToSearch.SelectedIndex = 0;
                cbFieldItemToSearchProvider.ItemsSource = ProvidersView.Fields;
                cbFieldItemToSearchProvider.DisplayMemberPath = "Key";
                cbFieldItemToSearchProvider.SelectedValuePath = "Value";
                if (ProvidersView.Fields.Count > 0) cbFieldItemToSearchProvider.SelectedIndex = 0;
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        /// <summary>
        /// Method that loads the Data in the controls of the Window
        /// </summary>
        /// <param name="warehouseMovement">Data Container.</param>
        /// <param name="UpdateGoodSelected">Indicate if its needed update the Good selected.</param>
        /// <param name="ThrowException">true, if want throw an exception if not found a component</param>
        private void LoadDataInControls(WarehouseMovementsView warehouseMovement, bool UpdateGoodSelected = false, int ThrowException = 0)
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Warehouse Movement Controls
                tbWarehouseMovement_Id.Text = warehouseMovement.WarehouseMovement_Id_Str;
                tbDate.Text = GlobalViewModel.GetStringFromDateTimeValue(warehouseMovement.Date);
                LoadExternalTablesInfo(warehouseMovement, ThrowException);
                if (UpdateGoodSelected)
                {
                    if (cbGoodCode.SelectedIndex == -1) SelectedGood = null;
                    else
                    {
                        SelectedGood = new GoodsView((GoodsView)cbGoodCode.SelectedValue);
                        Selected_Amount_Unit_Billing = EditedWarehouseMovement.Amount_Unit_Billing;
                        Selected_Amount_Unit_Shipping = EditedWarehouseMovement.Amount_Unit_Shipping;
                        ActualizeGoodDataInfo(warehouseMovement);
                    }
                }
                tbAmountUnitBilling.Text = GlobalViewModel.GetStringFromDecimalValue(warehouseMovement.Amount_Unit_Billing, DecimalType.Unit);
                tbAmountUnitShipping.Text = GlobalViewModel.GetStringFromDecimalValue(warehouseMovement.Amount_Unit_Shipping, DecimalType.Unit);
                chkbAccording.IsChecked = warehouseMovement.According;
                tbPrice.Text = GlobalViewModel.GetStringFromDecimalValue(warehouseMovement.Price, DecimalType.Currency);
                tbAmount.Text = GlobalViewModel.GetStringFromDecimalValue(warehouseMovement.Amount, DecimalType.Currency);
                tbValue.Text = GlobalViewModel.GetStringFromDecimalValue(warehouseMovement.AmountCost, DecimalType.Currency);
            //  Activate managers
                DataChangedManagerActive = true;
        }

        /// <summary>
        /// Load Data from External Tables.
        /// </summary>
        /// <param name="warehouseMovement">Data Container.</param>
        /// <param name="ThrowException">true, if want throw an exception if not found a component</param>
        private void LoadExternalTablesInfo(WarehouseMovementsView warehouseMovement, int ThrowException = 0)
        {
            if ((cbGoodCode.ItemsSource is null) || (warehouseMovement.Good is null)) cbGoodCode.SelectedIndex = -1;
            else
            {
                SortedDictionary<string, GoodsView> Items = (SortedDictionary<string, GoodsView>)cbGoodCode.ItemsSource;
                string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeyGoodView(warehouseMovement.Good);
                if (Items.ContainsKey(Key)) cbGoodCode.SelectedValue = Items[Key];
                else
                {
                    if (ThrowException == 1)
                    {
                        throw new Exception(string.Format("No s'ha trobat l'Article '{0}-{1}'.", Items[Key].Good_Code, Items[Key].Good_Description));
                    }
                }
            }
            if (warehouseMovement.Provider != null)
            {
                SortedDictionary<string, ProvidersView> Items = (SortedDictionary<string, ProvidersView>)cbProvider.ItemsSource;
                string Key = GlobalViewModel.Instance.HispaniaViewModel.GetKeyProviderView(warehouseMovement.Provider);
                if (Items.ContainsKey(Key))
                {
                    cbProvider.SelectedValue = Providers[Key];
                    cbProvider.ToolTip = Providers[Key].Name;
                }
                else
                {
                    if (ThrowException == 1)
                    {
                        throw new Exception(string.Format("No s'ha trobat el Proveïdor '{0}-{1}'.", Providers[Key].Name));
                    }
                }
            }
            else cbProvider.SelectedIndex = -1;
            if ((warehouseMovement.Type == 1) || (warehouseMovement.Type == 5))
            {
                cbType.SelectedValue = warehouseMovement.Type;
            }
            else cbType.SelectedIndex = -1;
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
        
        /// <summary>
        /// Method that filter the elements that are showing in the list
        /// </summary>
        /// <param name="item">Item to test</param>
        /// <returns>true, if the item must be loaded, false, if not.</returns>
        private bool UserFilterProvider(object item)
        {
            //  Determine if is needed aplicate one filter.
                if (cbFieldItemToSearchProvider.SelectedIndex == -1) return (true);
            //  Get Acces to the object and the property name To Filter.
                ProvidersView ItemData = ((KeyValuePair<string, ProvidersView>)item).Value;
                String ProperyName = (string) cbFieldItemToSearchProvider.SelectedValue;
            //  Apply the filter by selected field value
                if (!String.IsNullOrEmpty(tbItemToSearchProvider.Text))
                {
                    object valueToTest = ItemData.GetType().GetProperty(ProperyName).GetValue(ItemData);
                    if ((valueToTest is null) || 
                        (!(valueToTest.ToString().ToUpper()).Contains(tbItemToSearchProvider.Text.ToUpper())))
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
            //  By default the manager for the Good Data changes is active.
                DataChangedManagerActive = true;
            //  TextBox
                tbAmountUnitBilling.PreviewTextInput += TBPreviewTextInput;
                tbAmountUnitBilling.TextChanged += TBUnitDataChanged;
                tbAmountUnitShipping.PreviewTextInput += TBPreviewTextInput;
                tbAmountUnitShipping.TextChanged += TBUnitDataChanged;
                tbPrice.PreviewTextInput += TBPreviewTextInput;
                tbPrice.TextChanged += TBCurrencyDataChanged;
                tbPrice.GotKeyboardFocus += TbPrice_GotKeyboardFocus;
                //tbItemToSearch.TextChanged += TBItemToSearchDataChanged;
                tbItemToSearchProvider.TextChanged += TBItemToSearchProviderDataChanged;
            //  CheckBox
                //chkbAccording.Checked += ChkbAccording_Checked;
                //chkbAccording.Unchecked += ChkbAccording_Unchecked;
            //  ComboBox
                //cbFieldItemToSearch.SelectionChanged += CbFieldItemToSearch_SelectionChanged;
                cbFieldItemToSearchProvider.SelectionChanged += CbFieldItemToSearchProvider_SelectionChanged;
                cbType.SelectionChanged += CbType_SelectionChanged;
                cbGoodCode.SelectionChanged += CbGoodCode_SelectionChanged;
            //  Buttons
                btnAccept.Click += BtnAccept_Click;
                btnCancel.Click += BtnCancel_Click;
                //btnAcceptSearch.Click += BtnAcceptSearch_Click;
                btnAcceptSearchProvider.Click += BtnAcceptSearchProvider_Click;
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
            if ((sender == tbAmountUnitBilling) || (sender == tbAmountUnitShipping)) 
            {
                e.Handled = ! GlobalViewModel.IsValidUnitChar(e.Text);
            }
            else if (sender == tbPrice)
            {
                e.Handled = !GlobalViewModel.IsValidCurrencyChar(e.Text);
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
                    if (sender == tbAmountUnitBilling)
                    {
                        EditedWarehouseMovement.Amount_Unit_Billing = value;
                        UpdateGoodInfo();
                    }
                    else if (sender == tbAmountUnitShipping)
                    {
                        EditedWarehouseMovement.Amount_Unit_Shipping = value;
                        UpdateGoodInfo();
                    }
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedWarehouseMovement);
                }
                AreDataChanged = (EditedWarehouseMovement != WarehouseMovement);
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
                    if (sender == tbPrice)
                    {
                        EditedWarehouseMovement.Price = value;
                        UpdateGoodInfo();
                    }
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                    LoadDataInControls(EditedWarehouseMovement);
                }
                AreDataChanged = (EditedWarehouseMovement != WarehouseMovement);
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Manage the event that's produced when the TextBox Price Got Keyboard Focus
        /// </summary>
        /// <param name="sender">Object that send the event.</param>
        /// <param name="e">Parameters with the event was sent</param>
        private void TbPrice_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                //  Deactivate the management of this method
                    DataChangedManagerActive = false;
                //  Refresh Edited Data
                    string QMsg = "Vol actualitzar el valor del camp preu en funció de l'entrada o deixar el del moviment de magatzem ?";
                    if (MsgManager.ShowQuestion(QMsg) == MessageBoxResult.Yes)
                    {
                        try
                        {
                            //  If the user hasn't selected previously a Good and a Movement Type we can't do anything.
                                if ((EditedWarehouseMovement.Good != null) && (cbType.SelectedValue != null))
                                {
                                    if ((int)cbType.SelectedValue == 1) // Entry
                                    {
                                        EditedWarehouseMovement.Price = EditedWarehouseMovement.Good.Price_Cost;
                                        tbPrice.Text = GlobalViewModel.GetStringFromDecimalValue(EditedWarehouseMovement.Price, DecimalType.Currency);
                                        UpdateGoodInfo();
                                    }
                                    else if ((int)cbType.SelectedValue == 5) // Exit
                                    {
                                        EditedWarehouseMovement.Price = EditedWarehouseMovement.Good.Average_Price_Cost;
                                        tbPrice.Text = GlobalViewModel.GetStringFromDecimalValue(EditedWarehouseMovement.Price, DecimalType.Currency);
                                        UpdateGoodInfo();
                                    }
                                    else
                                    {
                                        MsgManager.ShowMessage("Avís, no s'ha carregat cap valor en el camp Preu ja que ho s'ha seleccionat el tipus de moviment",
                                                               MsgType.Warning);
                                    }
                                }
                        }
                        catch (Exception ex)
                        {
                            MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                            LoadDataInControls(EditedWarehouseMovement);
                        }
                    }
                //  Determine if are changes with the original data.
                    AreDataChanged = (EditedWarehouseMovement != WarehouseMovement);
                //  Activate the management of this method
                    DataChangedManagerActive = true;
            }
        }

        ///// <summary>
        ///// Filter Goods.
        ///// </summary>
        ///// <param name="sender">Object that sends the event.</param>
        ///// <param name="e">Parameters with the event was sended.</param>
        //private void TBItemToSearchDataChanged(object sender, TextChangedEventArgs e)
        //{
        //    if (DataChangedManagerActive)
        //    {
        //        DataChangedManagerActive = false;
        //        FilterDataListObjects();
        //        DataChangedManagerActive = true;
        //    }
        //}

        /// <summary>
        /// Filter providers.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void TBItemToSearchProviderDataChanged(object sender, TextChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                FilterDataListObjectsProvider();
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
            WarehouseMovementsAttributes ErrorField = WarehouseMovementsAttributes.None;
            try
            {
                if ((CtrlOperation == Operation.Add) || (CtrlOperation == Operation.Edit))
                {
                    EditedWarehouseMovement.Validate(out ErrorField);
                    EvAccept?.Invoke(new WarehouseMovementsView(EditedWarehouseMovement));
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

        #region Search Good // Disabled

        //private void BtnAcceptSearch_Click(object sender, RoutedEventArgs e)
        //{
        //    if (DataChangedManagerActive)
        //    {
        //        //  Deactivate the management of this method
        //            DataChangedManagerActive = false;
        //            if (cbGoodCode.SelectedItem != null)
        //            {
        //                //  Update Good information.
        //                    try
        //                    {
        //                        GoodsView good = (GoodsView)cbGoodCode.SelectedValue;
        //                        EditedWarehouseMovement.Good = good;
        //                        if (good.Good_Code.ToUpper() != SelectedGood.Good_Code.ToUpper())
        //                        {
        //                            SelectedGood = new GoodsView(good);
        //                            Selected_Amount_Unit_Billing = 0;
        //                            Selected_Amount_Unit_Shipping = 0;
        //                            if ((EditedWarehouseMovement.Good != null) && (cbType.SelectedValue != null))
        //                            {
        //                                if ((int)cbType.SelectedValue == 1) // Entry
        //                                {
        //                                    EditedWarehouseMovement.Price = EditedWarehouseMovement.Good.Price_Cost;
        //                                    tbPrice.Text = GlobalViewModel.GetStringFromDecimalValue(EditedWarehouseMovement.Price, DecimalType.Currency);
        //                                }
        //                                else if ((int)cbType.SelectedValue == 5) // Exit
        //                                {
        //                                    EditedWarehouseMovement.Price = EditedWarehouseMovement.Good.Average_Price_Cost;
        //                                    tbPrice.Text = GlobalViewModel.GetStringFromDecimalValue(EditedWarehouseMovement.Price, DecimalType.Currency);
        //                                }
        //                            }
        //                            UpdateGoodInfo();
        //                        }
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        MsgManager.ShowMessage(
        //                           string.Format("Error, a l'actualizar les dades de l'artícle.\r\n.Detalls: {0}", 
        //                                         MsgManager.ExcepMsg(ex)));
        //                        RestoreSourceValue(WarehouseMovementsAttributes.Good);
        //                    }
        //                    AreDataChanged = (EditedWarehouseMovement != WarehouseMovement);
        //            }
        //        //  Activate the management of this method
        //            DataChangedManagerActive = true;
        //    }
        //}

        #endregion

        #region Search Provider

        private void BtnAcceptSearchProvider_Click(object sender, RoutedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                if (cbProvider.SelectedItem != null)
                {
                    EditedWarehouseMovement.Provider = (ProvidersView)cbProvider.SelectedValue;
                    AreDataChanged = (EditedWarehouseMovement != WarehouseMovement);
                }
                else MsgManager.ShowMessage("Error, no hi ha cap Proveïdor seleccionat.");
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #endregion

        #region ComboBox

        /// <summary>
        /// Manage the change of the Data in the combobox of Units.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                if (cbType.SelectedItem != null)
                {
                    int TypeSelected = ((int)cbType.SelectedValue);
                    EditedWarehouseMovement.Type = TypeSelected;
                    AreDataChanged = (EditedWarehouseMovement != WarehouseMovement);
                }
                DataChangedManagerActive = true;
            }
        }

        ///// <summary>
        ///// Manage the change of the Data in the combobox of Goods.
        ///// </summary>
        ///// <param name="sender">Object that sends the event.</param>
        ///// <param name="e">Parameters with the event was sended.</param>
        //private void CbFieldItemToSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (DataChangedManagerActive)
        //    {
        //        DataChangedManagerActive = false;
        //        FilterDataListObjects();
        //        DataChangedManagerActive = true;
        //    }
        //}

        /// <summary>
        /// Manage the change of the Data in the combobox of Goods.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CbFieldItemToSearchProvider_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                FilterDataListObjectsProvider();
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Manage the good code selection.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CbGoodCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                if (cbGoodCode.SelectedValue != null)
                {
                    tbGoodDescription.Text = ((GoodsView)cbGoodCode.SelectedValue).Good_Description;
                }
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region CheckBox

        //private void ChkbAccording_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    if (DataChangedManagerActive)
        //    {
        //        DataChangedManagerActive = false;
        //        EditedWarehouseMovement.According = false;
        //        AreDataChanged = (EditedWarehouseMovement != WarehouseMovement);
        //        DataChangedManagerActive = true;
        //    }
        //}

        //private void ChkbAccording_Checked(object sender, RoutedEventArgs e)
        //{
        //    if (DataChangedManagerActive)
        //    {
        //        DataChangedManagerActive = false;
        //        EditedWarehouseMovement.According = true;
        //        AreDataChanged = (EditedWarehouseMovement != WarehouseMovement);
        //        DataChangedManagerActive = true;
        //    }
        //}

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Restore all values.
        /// </summary>
        public void RestoreSourceValues()
        {
            EditedWarehouseMovement.RestoreSourceValues(WarehouseMovement);
            LoadDataInControls(EditedWarehouseMovement, true);
            AreDataChanged = (EditedWarehouseMovement != WarehouseMovement);
        }

        /// <summary>
        /// Restore the value of the indicated field.
        /// </summary>
        /// <param name="ErrorField">Field to restore value.</param>
        public void RestoreSourceValue(WarehouseMovementsAttributes ErrorField)
        {
            EditedWarehouseMovement.RestoreSourceValue(WarehouseMovement, ErrorField);
            LoadDataInControls(EditedWarehouseMovement, ErrorField == WarehouseMovementsAttributes.Good);
            AreDataChanged = (EditedWarehouseMovement != WarehouseMovement);
        }

        #endregion

        #region Update UI

        private void ResetFilters()
        {
            DataChangedManagerActive = false;
            //tbItemToSearch.Text = string.Empty;
            //FilterDataListObjects();
            tbItemToSearchProvider.Text = string.Empty;
            FilterDataListObjectsProvider();
            DataChangedManagerActive = true;
        }

        //private void FilterDataListObjects()
        //{
        //    CollectionViewSource.GetDefaultView(cbGoodCode.ItemsSource).Refresh();
        //    if (cbGoodCode.Items.Count > 0)
        //    {
        //        cbGoodCode.SelectedIndex = 0;
        //        tbGoodDescription.Text = ((GoodsView)cbGoodCode.SelectedValue).Good_Description;
        //    }
        //    else tbGoodDescription.Text = "No hi ha articles que compleixin amb el filtre seleccionat.";
        //    btnAcceptSearch.IsEnabled = cbGoodCode.Items.Count > 0;
        //}

        private void FilterDataListObjectsProvider()
        {
            CollectionViewSource.GetDefaultView(cbProvider.ItemsSource).Refresh();
            if (cbProvider.Items.Count > 0) cbProvider.SelectedIndex = 0;
        }

        private void UpdateGoodInfo()
        {
            GlobalViewModel.Instance.HispaniaViewModel.UpdateGoodInformation(EditedWarehouseMovement.Good,
                                                                             Selected_Amount_Unit_Billing,
                                                                             Selected_Amount_Unit_Shipping,
                                                                             EditedWarehouseMovement.Amount_Unit_Billing,
                                                                             EditedWarehouseMovement.Amount_Unit_Shipping,
                                                                             EditedWarehouseMovement.Price,
                                                                             EditedWarehouseMovement.Type,
                                                                             out GoodsView Updated_SelectedGood);
            SelectedGood = Updated_SelectedGood;
            ActualizeGoodDataInfo(EditedWarehouseMovement);
            UpdateAmounts();
        }

        private void ActualizeGoodDataInfo(WarehouseMovementsView warehouseMovement)
        {
            tbGoodDescription.ToolTip = tbGoodDescription.Text = SelectedGood.Good_Description;
            tbUnitBillingDefinition.Text = warehouseMovement.Unit_Billing_Definition;
            tbUnitBillingDefinition.ToolTip = tbUnitBillingDefinition.Text;
            tbUnitShippingDefinition.Text = warehouseMovement.Unit_Shipping_Definition;
            tbUnitShippingDefinition.ToolTip = tbUnitShippingDefinition.Text;
            tbPriceCost.Text = GlobalViewModel.GetStringFromDecimalValue(SelectedGood.Price_Cost, DecimalType.Currency, true);
            tbAveragePriceCost.Text = GlobalViewModel.GetStringFromDecimalValue(SelectedGood.Average_Price_Cost, DecimalType.Currency, true);
            tbBillingUnitStocks.Text = GlobalViewModel.GetStringFromDecimalValue(SelectedGood.Billing_Unit_Stocks, DecimalType.Unit, true);
            tbShippingUnitStocks.Text = GlobalViewModel.GetStringFromDecimalValue(SelectedGood.Shipping_Unit_Stocks, DecimalType.Unit, true);
            tbBillingUnitAvailable.Text = GlobalViewModel.GetStringFromDecimalValue(SelectedGood.Billing_Unit_Available, DecimalType.Unit, true);
            tbShippingUnitAvailable.Text = GlobalViewModel.GetStringFromDecimalValue(SelectedGood.Shipping_Unit_Available, DecimalType.Unit, true);
            tbBillingUnitEntrance.Text = GlobalViewModel.GetStringFromDecimalValue(SelectedGood.Billing_Unit_Entrance, DecimalType.Unit, true);
            tbShippingUnitEntrance.Text = GlobalViewModel.GetStringFromDecimalValue(SelectedGood.Shipping_Unit_Entrance, DecimalType.Unit, true);
            tbBillingUnitDepartures.Text = GlobalViewModel.GetStringFromDecimalValue(SelectedGood.Billing_Unit_Departure, DecimalType.Unit, true);
            tbShippingUnitDepartures.Text = GlobalViewModel.GetStringFromDecimalValue(SelectedGood.Shipping_Unit_Departure, DecimalType.Unit, true);
        }

        private void UpdateAmounts()
        {
            tbAmount.Text = GlobalViewModel.GetStringFromDecimalValue(EditedWarehouseMovement.Amount, DecimalType.Currency, true);
            tbValue.Text = GlobalViewModel.GetStringFromDecimalValue(SelectedGood.Average_Price_Cost * SelectedGood.Billing_Unit_Stocks, DecimalType.Currency, true);
        }

        #endregion
    }
}
