#region Librerias usadas por la clase

using HispaniaCommon.ViewClientWPF.Managers;
using HispaniaCommon.ViewClientWPF.Windows;
using HispaniaCommon.ViewModel;
using MBCode.Framework.Managers.Messages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

#endregion

namespace HispaniaCommon.ViewClientWPF.UserControls
{
    /// <summary>
    /// Lógica de interacción para WarehouseMovementsAddData.xaml
    /// </summary>
    public partial class WarehouseMovementsAddData : UserControl
    {
        #region Enums

        public enum ActionState
        {
            InitialState,

            Adding,

            Editing,

            Saved,

            SavedInEdition,
        }

        #endregion

        #region Events and Delegates

        /// <summary>
        /// Delegate that define the sign of the event that is throwed when the button add movement is pressed.
        /// </summary>
        public delegate void dlgEvAddWarehouseMovement(WarehouseMovementsAddData sender);

        /// <summary>
        /// Delegate that define the sign of the event that is throwed when the button remove movement is pressed.
        /// </summary>
        public delegate void dlgEvDeleteWarehouseMovement(WarehouseMovementsAddData sender, RowDefinition ParentControl, GoodsView good);

        /// <summary>
        /// Delegate that define the sign of the event that is throwed when the data of the movement has changed.
        /// </summary>
        public delegate void dlgEvUpdateInfoWarehouseMovement(WarehouseMovementsView movement, GoodsView good);

        /// <summary>
        /// Event that is throwed when the button add movement is pressed.
        /// </summary>
        public event dlgEvAddWarehouseMovement EvAddWarehouseMovement;

        /// <summary>
        /// Event that is throwed when the button delete movement is pressed.
        /// </summary>
        public event dlgEvDeleteWarehouseMovement EvDeleteWarehouseMovement;

        /// <summary>
        /// Event that is throwed when the data of the movement has changed.
        /// </summary>
        public event dlgEvUpdateInfoWarehouseMovement EvUpdateInfoWarehouseMovement;

        #endregion

        #region Attributes

        /// <summary>
        /// Define the good pattern for make good dictionary keys.
        /// </summary>
        private string KeyGoodPattern = "{0} - {1}";

        /// <summary>
        /// Store a value that indicate if Add Movement Button is yet pressed.
        /// </summary>
        private bool m_AddNewMovementPressed = false;

        /// <summary>
        /// Store the nowadys State of the windows actions.
        /// </summary>
        private ActionState m_State = ActionState.InitialState;
        
        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private Dictionary<string, GoodsView> m_DataGoods = new Dictionary<string, GoodsView>();

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private Dictionary<string, ProvidersView> m_DataProviders = new Dictionary<string, ProvidersView>();

        /// <summary>
        /// Store the movement to show in control.
        /// </summary>
        private WarehouseMovementsView m_Movement = null;

        /// <summary>
        /// Get or Set if the manager of the data change for the Good has active.
        /// </summary>
        private Dictionary<int, string> m_WarehouseMovementType = new Dictionary<int, string>()
        {
            { 1, "ENTRADA" },
            { 5, "SORTIDA" }
        };

        /// <summary>
        /// Stotre if the data of the Good has changed.
        /// </summary>
        private bool m_AreDataChanged;

        #endregion

        #region Properties

        public RowDefinition ParentControl
        {
            get;
            set;
        }


        /// <summary>
        /// Get or Set the data to show in List of Items.
        /// </summary>
        public Dictionary<string, GoodsView> DataGoods
        {
            get
            {
                return (m_DataGoods);
            }
            set
            {
                m_DataGoods = new Dictionary<string, GoodsView>();
                if (value != null) 
                {
                    foreach (GoodsView good in value.Values)
                    {
                        m_DataGoods.Add(string.Format(KeyGoodPattern, good.Good_Code, good.Good_Description), good);
                    }
                }
                cbGood.ItemsSource = m_DataGoods;
                cbGood.DisplayMemberPath = "Key";
                cbGood.SelectedValuePath = "Value";
                CollectionViewSource.GetDefaultView(cbGood.ItemsSource).SortDescriptions.Add(new SortDescription("Key", ListSortDirection.Ascending));
                CollectionViewSource.GetDefaultView(cbGood.ItemsSource).Filter = GoodsFilter;
            }
        }
        
        /// <summary>
        /// Get or Set the data to show in List of Items.
        /// </summary>
        public Dictionary<string, ProvidersView> DataProviders
        {
            get
            {
                return (m_DataProviders);
            }
            set
            {
                if (value != null) m_DataProviders = new Dictionary<string, ProvidersView>(value);
                else m_DataProviders = new Dictionary<string, ProvidersView>();
                cbProvider.ItemsSource = m_DataProviders;
                cbProvider.DisplayMemberPath = "Key";
                cbProvider.SelectedValuePath = "Value";
                CollectionViewSource.GetDefaultView(cbProvider.ItemsSource).SortDescriptions.Add(new SortDescription("Key", ListSortDirection.Ascending));
                CollectionViewSource.GetDefaultView(cbProvider.ItemsSource).Filter = ProvidersFilter;
            }
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

        /// <summary>
        /// Get or Set a value that indicate if Add Movement Button is yet pressed.
        /// </summary>
        public bool AddNewMovementPressed
        {
            get
            {
                return m_AddNewMovementPressed;
            }
            set
            {
                m_AddNewMovementPressed = value;
                if (m_AddNewMovementPressed) btnThirdWHMov.Visibility = Visibility.Hidden;
            }
        }

        /// <summary>
        /// Get or Set a value that indicate if Add Movement Button is yet pressed.
        /// </summary>
        public bool ShowPressedButton
        {
            set
            {
                btnThirdWHMov.Visibility = value ? Visibility.Visible : Visibility.Hidden;
            }
        }

        /// <summary>
        /// Get or Set the nowadays State of the windows actions.
        /// </summary>
        public ActionState State
        {
            get
            {
                return m_State;
            }
            private set
            {
                m_State = value;
                UpdateUI();
            }
        }

        private WarehouseMovementsView Movement
        {
            get
            {
                return m_Movement;
            }
            set
            {
                if (value != null)
                {
                    AreDataChanged = false;
                    m_Movement = new WarehouseMovementsView(value);
                    EditedMovement = new WarehouseMovementsView(m_Movement);
                    if (m_Movement.Good != null)
                    {
                        DataChangedManagerActive = false;
                        cbGood.SelectedValue = null;
                        string Key = string.Format(KeyGoodPattern, m_Movement.Good.Good_Code, m_Movement.Good.Good_Description);
                        if (DataGoods.ContainsKey(Key)) DataGoods.Remove(Key);
                        GoodsView good = new GoodsView(m_Movement.Good);
                        DataGoods.Add(Key, good);
                        DataChangedManagerActive = true;
                    }
                    LoadDataInWindow(m_Movement);
                }
                else
                {
                    throw new ArgumentNullException("Error, no s'han trobat les dades del moviment carregar.");
                }

            }
        }

        /// <summary>
        /// Get or Set the Edited Warehouse Movement information.
        /// </summary>
        private WarehouseMovementsView EditedMovement
        {
            get;
            set;
        }

        public bool IsDataSaved
        {
            get
            {
                return ((OperationState == ActionState.Saved) || ((OperationState == ActionState.Adding) && (State == ActionState.InitialState)));
            }
        }

        public int WarehouseMovement_Id
        {
            get
            {
                return EditedMovement.WarehouseMovement_Id;
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
                //  Update property value.
                    m_AreDataChanged = value;
                //  Update Window State
                    State = (m_AreDataChanged) ? OperationState : InitialState;
                //  Do actions associated to the new value.
                    if (m_AreDataChanged)
                    {
                        //  Sent an event to update the data in the parent window.
                            UpdateWarehouseMovementInfoInParentWindow();
                    }
            }
        }

        /// <summary>
        /// Get or Set the nowadays State of the windows actions.
        /// </summary>
        private ActionState OperationState
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set the nowadays State of the windows actions.
        /// </summary>
        private ActionState InitialState
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set if the manager of the data change for the Good has active.
        /// </summary>
        private bool DataChangedManagerActive
        {
            get;
            set;
        }

        #endregion

        #region Builders

        /// <summary>
        /// Default builder of the class.
        /// </summary>
        public WarehouseMovementsAddData()
        {
            InitializeComponent();
            InitializeWindow();
            LoadManagers();
        }

        private void InitializeWindow()
        {
            //  Initialize movement.
                WarehouseMovementsView NewMovement = new WarehouseMovementsView
                {
                    WarehouseMovement_Id = WarehouseMovementsAdd.NonSavedId,
                    Amount_Unit_Billing = 0,
                    Amount_Unit_Shipping = 0,
                    Price = 0,
                    According = true
                };
                Movement = NewMovement;
            //  Initialize Combo
                cbType.ItemsSource = WarehouseMovementType;
                cbType.DisplayMemberPath = "Value";
                cbType.SelectedValuePath = "Key";
            //  Actualize PreviousState value.
                OperationState = ActionState.Adding;
                InitialState = ActionState.InitialState;
        }

        #endregion

        #region Standings

        /// <summary>
        /// Load data from the movement.
        /// </summary>
        /// <param name="movement">Data Container.</param>
        public void LoadDataInWindow(WarehouseMovementsView movement)
        {
            //  Deactivate managers of controls
                DataChangedManagerActive = false;
            //  Initialize Data in controls.
                tbWarehouseMovement_Id.Text = (movement.WarehouseMovement_Id <= 0) ? 
                                              "No guardat" : 
                                              GlobalViewModel.GetStringFromIntIdValue(movement.WarehouseMovement_Id);
                tbDate.Text = GlobalViewModel.GetStringFromDateTimeValue(movement.Date);
                if (movement.Good != null) cbGood.SelectedValue = movement.Good;
                else cbGood.SelectedIndex = -1;
                tbAmountUnitBilling.Text = GlobalViewModel.GetStringFromDecimalValue(movement.Amount_Unit_Billing, DecimalType.Unit);
                tbAmountUnitShipping.Text = GlobalViewModel.GetStringFromDecimalValue(movement.Amount_Unit_Shipping, DecimalType.Unit);
                if ((movement.Type == 1) || (movement.Type == 5))
                {
                    cbType.SelectedValue = movement.Type;
                }
                else cbType.SelectedIndex = -1;
                if (movement.Provider != null)
                {
                    cbProvider.SelectedValue = movement.Provider;
                    cbProvider.ToolTip = movement.Provider.Name;
                }
                else
                {
                    cbProvider.SelectedIndex = -1;
                    cbProvider.ToolTip = string.Empty;
                }
                chkbAccording.IsChecked = movement.According;
                tbPrice.Text = GlobalViewModel.GetStringFromDecimalValue(movement.Price, DecimalType.Currency);
            //  Update UI
                UpdateUI();
            //  Activate managers of controls
                DataChangedManagerActive = true;
        }


        /// <summary>
        /// Method that filter the elements that are showing in the list
        /// </summary>
        /// <param name="item">Item to test</param>
        /// <returns>true, if the item must be loaded, false, if not.</returns>
        private bool GoodsFilter(object item)
        {
            //  Get Acces to the object and the property name To Filter.
                GoodsView ItemData = ((KeyValuePair<string, GoodsView>) item).Value;
            //  Apply the filter by selected field value
                if (!String.IsNullOrEmpty(tbGoodCodeFilter.Text))
                {
                    if ((ItemData.Good_Code is null) || 
                        (!(ItemData.Good_Code.ToUpper()).StartsWith(tbGoodCodeFilter.Text.ToUpper())))
                    {
                        if ((ItemData.Good_Description is null) || 
                            (!(ItemData.Good_Description.ToUpper()).Contains(tbGoodCodeFilter.Text.ToUpper())))
                        {
                            return false;
                        }
                    }
                }
                return true;
        }
        
        /// <summary>
        /// Method that filter the elements that are showing in the list
        /// </summary>
        /// <param name="item">Item to test</param>
        /// <returns>true, if the item must be loaded, false, if not.</returns>
        private bool ProvidersFilter(object item)
        {
            //  Get Acces to the object and the property name To Filter.
                ProvidersView ItemData = ((KeyValuePair<string, ProvidersView>) item).Value;
            //  Apply the filter by selected field value
                if (!String.IsNullOrEmpty(tbGoodCodeFilter.Text))
                {
                    if ((ItemData.Name is null) || (!ItemData.Name.ToUpper().Contains(tbGoodCodeFilter.Text.ToUpper())))
                    {
                        return false;
                    }
                }
                return true;
        }

        #endregion

        #region Managers

        private void LoadManagers()
        {
            //  User Control Events
                MouseDoubleClick += WarehouseMovementsAddData_MouseDoubleClick;
            //  Button
                btnFirstWHMov.Click += BtnFirstWHMov_Click;
                btnSecondWHMov.Click += BtnSecondWHMov_Click;
                btnThirdWHMov.Click += BtnThirdWHMov_Click;
            //  TextBox
                tbGoodCodeFilter.TextChanged += TbGoodCodeFilter_TextChanged;
                tbProviderNameFilter.TextChanged += TbProviderNameFilter_TextChanged;
                tbAmountUnitBilling.PreviewTextInput += TBPreviewTextInput;
                tbAmountUnitBilling.TextChanged += TBUnitDataChanged;
                tbAmountUnitShipping.PreviewTextInput += TBPreviewTextInput;
                tbAmountUnitShipping.TextChanged += TBUnitDataChanged;
                tbPrice.PreviewTextInput += TBPreviewTextInput;
                tbPrice.TextChanged += TBCurrencyDataChanged;
                tbPrice.GotKeyboardFocus += TbPrice_GotKeyboardFocus;
            //  ComboBox
                cbType.SelectionChanged += CbType_SelectionChanged;
                cbGood.SelectionChanged += CbGood_SelectionChanged;
                cbProvider.SelectionChanged += CbProvider_SelectionChanged;
            //  CheckBox
                //chkbAccording.Checked += ChkbAccording_Checked;
                //chkbAccording.Unchecked += ChkbAccording_Unchecked;
        }

        #region User Control

        /// <summary>
        /// Manage the Mouse Double Click event of the User Control. When a user make a Mouse Double Click in this control 
        /// the Good information is updated in the parent window.
        /// </summary>
        /// <param name="sender">Object that throw this event.</param>
        /// <param name="e">Parameters with this event has been sent.</param>
        private void WarehouseMovementsAddData_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //  Notify change of values at parent window.
                UpdateWarehouseMovementInfoInParentWindow();
        }

        #endregion

        #region Button

        #region First Button :- Save for Adding or Editing state and Editing for Saved state

        /// <summary>
        /// Manage two kind of actions depends of the State that is active in the moment which this event is 
        /// throwed:
        ///          1 - Adding or Editing : operation is Save.
        ///          2 - Saved : operation is Edit the Warehouse Movement selected.
        /// </summary>
        /// <param name="sender">Object that send the event.</param>
        /// <param name="e">Parameters with the event was sent</param>
        private void BtnFirstWHMov_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            switch (OperationState)
            {
                case ActionState.Adding:
                     //  Execute the method that save the new Warehouse Movement into the database.
                         SaveWarehouseMovement();
                         break;
                case ActionState.Editing:
                     //  Execute the method that save the new Warehouse Movement into the database.
                         SaveWarehouseMovement();
                         break;
                case ActionState.Saved:
                     //  Put Warehouse Movement in editing mode.
                         EditWarehouseMovement();
                         break;
            }
        }

        #endregion

        #region Second Button :- Cancel for Adding or Editing state and Remove for Saved state

        /// <summary>
        /// Manage two kind of actions depends of the State that is active in the moment which this event is 
        /// throwed:
        ///          1 - Adding or Editing : operation is Cancel.
        ///          2 - Saved : operation is Remove.
        /// </summary>
        /// <param name="sender">Object that send the event.</param>
        /// <param name="e">Parameters with the event was sent</param>
        private void BtnSecondWHMov_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            string QuestionAdd = "Esta segur que vol cancel·lar els canvis realitzats ?";
            string QuestionEdit = "Esta segur que vol cancel·lar els canvis realitzats en el moviment {0} ?";
            string QuestionDelete = "Esta segur que vol esborrar el moviment {0} ?";
            string QuestionDeleteAccording = "El moviment '{0}' està conforme, segur que el vol esborrar ?";
            switch (OperationState)
            {
                case ActionState.Adding:
                     if (MsgManager.ShowQuestion(QuestionAdd) == MessageBoxResult.Yes)
                     {
                        //  Cancel Changes that user has made in the Warehouse Movement.
                            CancelChangesInWarehouseMovement();
                        //  Update Flag Movement.
                            WarehouseMovementsAdd.IsEditingWarehouseMovement = -1;
                    }
                    break;    
                case ActionState.Editing:
                     if (AreDataChanged)
                     {
                        //  Create the Question to ask at the user.
                            QuestionEdit = string.Format(QuestionEdit, EditedMovement.WarehouseMovement_Id);
                        //  Do the question at the user.
                            if (MsgManager.ShowQuestion(QuestionEdit) == MessageBoxResult.Yes)
                            {
                                //  Cancel Changes that user has made in the Warehouse Movement.
                                    CancelChangesInWarehouseMovement();
                                //  Update Flag Movement.
                                    WarehouseMovementsAdd.IsEditingWarehouseMovement = -1;
                            }
                     }
                     else
                     {
                        //  If data has no changed we need cancel the operation without ask anything at the user.
                            CancelEdition();
                        //  Update Flag Movement.
                            WarehouseMovementsAdd.IsEditingWarehouseMovement = -1;
                     }
                     break;
                case ActionState.Saved:
                     if (WarehouseMovementsAdd.IsEditingWarehouseMovement != -1)
                     {
                         MsgManager.ShowMessage(
                            string.Format("Error, a l'intentar esborrar el moviment '{0}'.\r\nDetalls: S'està editant el moviment '{1}'.",
                                          EditedMovement.WarehouseMovement_Id, WarehouseMovementsAdd.IsEditingWarehouseMovement));
                         return;
                     }
                     else WarehouseMovementsAdd.IsEditingWarehouseMovement = EditedMovement.WarehouseMovement_Id;
                     QuestionDelete = string.Format(QuestionDelete, EditedMovement.WarehouseMovement_Id);
                     if (MsgManager.ShowQuestion(QuestionDelete) == MessageBoxResult.Yes)
                     {
                        //  Validate if Movement is according.
                            if (EditedMovement.According)
                            {
                                QuestionDeleteAccording = string.Format(QuestionDeleteAccording, EditedMovement.WarehouseMovement_Id);
                                if (MsgManager.ShowQuestion(QuestionDeleteAccording) != MessageBoxResult.Yes)
                                {
                                    //  Notify the cancellation of the operation at the user.
                                        MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                                    //  Finish the operation.
                                        return;
                                }
                            }
                        //  Execute the method that save the new Warehouse Movement into the database.
                            DeleteWarehouseMovement(EditedMovement);
                     }
                     else
                     {
                        //  Notify the cancellation of the operation at the user.
                            MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                     }
                     WarehouseMovementsAdd.IsEditingWarehouseMovement = -1;
                     break;
            }
        }

        #endregion

        #region Third Button :- Add new Warehouse Movement

        /// <summary>
        /// Manage the creation of a new Warehouse Movement control.
        /// </summary>
        /// <param name="sender">Object that send the event.</param>
        /// <param name="e">Parameters with the event was sent</param>
        private void BtnThirdWHMov_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            try
            {
                //  Throw event that indicate at the parent window that must create a new warehouse movement control.
                    EvAddWarehouseMovement?.Invoke(this);
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(
                   string.Format("Error, al afegir el control per introduir un nou moviment.\r\nDetalls:{0}", MsgManager.ExcepMsg(ex)));
            }
        }

        #endregion

        #endregion

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
        /// <param name="sender">Object that send the event.</param>
        /// <param name="e">Parameters with the event was sent</param>
        private void TBPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if ((sender == tbAmountUnitBilling) || (sender == tbAmountUnitShipping))
            {
                e.Handled = ! GlobalViewModel.IsValidUnitChar(e.Text);
            }
            else if (sender == tbPrice)
            {
                e.Handled = ! GlobalViewModel.IsValidCurrencyChar(e.Text);
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
                //  Deactivate the management of this method
                    DataChangedManagerActive = false;
                //  Refresh Edited Data
                    TextBox tbInput = (TextBox)sender;
                    try
                    {
                        GlobalViewModel.NormalizeTextBox(sender, e, DecimalType.Unit);
                        decimal value = GlobalViewModel.GetUIDecimalValue(tbInput.Text);
                        if (sender == tbAmountUnitBilling) EditedMovement.Amount_Unit_Billing = value;
                        else if (sender == tbAmountUnitShipping) EditedMovement.Amount_Unit_Shipping = value;
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                        LoadDataInWindow(EditedMovement);
                    }
                //  Determine if are changes with the original data.
                    AreDataChanged = (EditedMovement != Movement);
                //  Activate the management of this method
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
                //  Deactivate the management of this method
                    DataChangedManagerActive = false;
                //  Refresh Edited Data
                    TextBox tbInput = (TextBox)sender;
                    try
                    {
                        GlobalViewModel.NormalizeTextBox(sender, e, DecimalType.Currency);
                        decimal value = GlobalViewModel.GetUIDecimalValue(tbInput.Text);
                        if (sender == tbPrice) EditedMovement.Price = value;
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                        LoadDataInWindow(EditedMovement);
                    }
                //  Determine if are changes with the original data.
                    AreDataChanged = (EditedMovement != Movement);
                //  Activate the management of this method
                    DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Manage the event that's produced when the text is changed in a TextBox
        /// </summary>
        /// <param name="sender">Object that send the event.</param>
        /// <param name="e">Parameters with the event was sent</param>
        private void TbGoodCodeFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                //  Deactivate the management of this method
                    DataChangedManagerActive = false;
                //  Refresh Edited Data
                    try
                    {
                        CollectionViewSource.GetDefaultView(cbGood.ItemsSource).Refresh();
                        if (cbGood.Items.Count <= 0)
                        {
                            cbGood.SelectedIndex = -1;
                            EditedMovement.Good = null;
                        }
                        else
                        {
                            cbGood.SelectedIndex = 0;
                            EditedMovement.Good = new GoodsView(((GoodsView)cbGood.SelectedValue));
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                        LoadDataInWindow(EditedMovement);
                    }
                //  Determine if are changes with the original data.
                    AreDataChanged = (EditedMovement != Movement);
                //  Activate the management of this method
                    DataChangedManagerActive = true;
                //  Allow to the control manager to continue of the current operation.
                    e.Handled = ! true;
            }
        }

        /// <summary>
        /// Manage the event that's produced when the text is changed in a TextBox
        /// </summary>
        /// <param name="sender">Object that send the event.</param>
        /// <param name="e">Parameters with the event was sent</param>
        private void TbProviderNameFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                //  Deactivate the management of this method
                    DataChangedManagerActive = false;
                //  Refresh Edited Data
                    try
                    {
                        CollectionViewSource.GetDefaultView(cbProvider.ItemsSource).Refresh();
                        if (cbProvider.Items.Count <= 0)
                        {
                            cbProvider.SelectedIndex = -1;
                            cbProvider.ToolTip = "Proveïdor NO seleccionat.";
                            EditedMovement.Provider = null;
                        }
                        else
                        {
                            cbProvider.SelectedIndex = 0;
                            cbProvider.ToolTip = ((ProvidersView)cbProvider.SelectedValue).Name;
                            EditedMovement.Provider = new ProvidersView(((ProvidersView)cbProvider.SelectedValue));
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                        LoadDataInWindow(EditedMovement);
                    }
                //  Determine if are changes with the original data.
                    AreDataChanged = (EditedMovement != Movement);
                //  Activate the management of this method
                    DataChangedManagerActive = true;
                //  Allow to the control manager to continue of the current operation.
                    e.Handled = ! true;
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
                    try
                    {
                        //  If the user hasn't selected previously a Good and a Movement Type we can't do anything.
                            if ((EditedMovement.Good != null) && (cbType.SelectedValue != null))
                            {
                                if ((int)cbType.SelectedValue == 1) // Entry
                                {
                                    EditedMovement.Price = EditedMovement.Good.Price_Cost;
                                    tbPrice.Text = GlobalViewModel.GetStringFromDecimalValue(EditedMovement.Price, DecimalType.Currency);
                                }
                                else if ((int)cbType.SelectedValue == 5) // Exit
                                {
                                    EditedMovement.Price = EditedMovement.Good.Average_Price_Cost;
                                    tbPrice.Text = GlobalViewModel.GetStringFromDecimalValue(EditedMovement.Price, DecimalType.Currency);
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
                        LoadDataInWindow(EditedMovement);
                    }
                //  Determine if are changes with the original data.
                    AreDataChanged = (EditedMovement != Movement);
                //  Activate the management of this method
                    DataChangedManagerActive = true;
            }
        }

        #endregion

        #region ComboBox

        /// <summary>
        /// Event produced when the combo selection had changed.
        /// </summary>
        /// <param name="sender">Object that send the event.</param>
        /// <param name="e">Parameters with the event was sent</param>
        private void CbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                //  Deactivate the management of this method
                    DataChangedManagerActive = false;
                //  Refresh Edited Data
                    try
                    {
                        if (cbType.SelectedValue is null)
                        {
                            EditedMovement.Type = GlobalViewModel.IntIdInitValue; 
                        }
                        else
                        {
                            EditedMovement.Type = (int) cbType.SelectedValue;
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                        LoadDataInWindow(EditedMovement);
                    }
                //  Determine if are changes with the original data.
                    AreDataChanged = (EditedMovement != Movement);
                //  Activate the management of this method
                    DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Event produced when the combo selection had changed.
        /// </summary>
        /// <param name="sender">Object that send the event.</param>
        /// <param name="e">Parameters with the event was sent</param>
        private void CbGood_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                //  Deactivate the management of this method
                    DataChangedManagerActive = false;
                //  Refresh Edited Data
                    try
                    {
                        if (cbGood.SelectedValue is null) EditedMovement.Good = null;
                        else
                        {
                            EditedMovement.Good = (GoodsView)cbGood.SelectedValue;
                        }
                        UpdateEditableControls(cbGood.SelectedValue != null);
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                        LoadDataInWindow(EditedMovement);
                    }
                //  Determine if are changes with the original data.
                    AreDataChanged = (EditedMovement != Movement);
                //  Activate the management of this method
                    DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Event produced when the combo selection had changed.
        /// </summary>
        /// <param name="sender">Object that send the event.</param>
        /// <param name="e">Parameters with the event was sent</param>
        private void CbProvider_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                //  Deactivate the management of this method
                    DataChangedManagerActive = false;
                //  Refresh Edited Data
                    try
                    {
                        if (cbProvider.SelectedValue is null)
                        {
                            EditedMovement.Provider = null;
                            cbProvider.ToolTip = "Proveïdor NO seleccionat.";
                        }
                        else
                        {
                            EditedMovement.Provider = (ProvidersView)cbProvider.SelectedValue;
                            cbProvider.ToolTip = EditedMovement.Provider.Name;
                        }
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                        LoadDataInWindow(EditedMovement);
                    }
                //  Determine if are changes with the original data.
                    AreDataChanged = (EditedMovement != Movement);
                //  Activate the management of this method
                    DataChangedManagerActive = true;
            }
        }

        #endregion

        #region CheckBox

        ///// <summary>
        ///// Event produced when the checkbox has been checked.
        ///// </summary>
        ///// <param name="sender">Object that send the event.</param>
        ///// <param name="e">Parameters with the event was sent</param>
        //private void ChkbAccording_Checked(object sender, RoutedEventArgs e)
        //{
        //    if (DataChangedManagerActive)
        //    {
        //        //  Deactivate the management of this method
        //            DataChangedManagerActive = false;
        //        //  Refresh Edited Data
        //            EditedMovement.According = (chkbAccording.IsChecked == true) ? true : false; 
        //        //  Determine if are changes with the original data.
        //            AreDataChanged = (EditedMovement != Movement);
        //        //  Activate the management of this method
        //            DataChangedManagerActive = true;
        //    }
        //}

        ///// <summary>
        ///// Event produced when the checkbox has been unchecked.
        ///// </summary>
        ///// <param name="sender">Object that send the event.</param>
        ///// <param name="e">Parameters with the event was sent</param>
        //private void ChkbAccording_Unchecked(object sender, RoutedEventArgs e)
        //{
        //    if (DataChangedManagerActive)
        //    {
        //        //  Deactivate the management of this method
        //            DataChangedManagerActive = false;
        //        //  Refresh Edited Data
        //            EditedMovement.According = (chkbAccording.IsChecked == true) ? true : false; 
        //        //  Determine if are changes with the original data.
        //            AreDataChanged = (EditedMovement != Movement);
        //        //  Activate the management of this method
        //            DataChangedManagerActive = true;
        //    }
        //}

        #endregion

        #endregion

        #region Data Managers

        #region Save Warehouse Movement

        /// <summary>
        /// Manage the event produced when the user would save the data that he/she has introduced.
        /// </summary>
        private void SaveWarehouseMovement()
        {
            WarehouseMovementsAttributes ErrorField = WarehouseMovementsAttributes.None;
            try
            {
                //  Validate the data of the new Warehouse Movement (if validate error then throw an exception).
                    EditedMovement.Validate(out ErrorField);
                //  Save the new Warehouse Movement into the DataBase
                    WarehouseMovementsView NewWarehouseMovement = new WarehouseMovementsView(EditedMovement);
                    switch (OperationState)
                    {
                        case ActionState.Adding:
                             //  Determine if  editing a Warehouse Movement.
                                 if (WarehouseMovementsAdd.IsEditingWarehouseMovement != -1)
                                 {
                                     MsgManager.ShowMessage(
                                        string.Format("Error, a l'intentar crear un moviment.\r\nDetalls: S'està editant el moviment '{0}'.",
                                                      WarehouseMovementsAdd.IsEditingWarehouseMovement));
                                     return;
                                 }
                                 else WarehouseMovementsAdd.IsEditingWarehouseMovement = int.MaxValue;
                             //  Create new Item in DataBase
                                 GlobalViewModel.Instance.HispaniaViewModel.CreateWarehouseMovement(NewWarehouseMovement);
                             //  Update initial movement value.
                                 Movement = GlobalViewModel.Instance.HispaniaViewModel.GetWarehouseMovementFromDb(NewWarehouseMovement);
                             //  Update State values.
                                 OperationState = ActionState.Saved;
                                 InitialState = ActionState.Saved;
                             //  Update controller of data changed.
                                 AreDataChanged = false;
                             //  Update the field associated with the identifier of movement.
                                 tbWarehouseMovement_Id.Text = Movement.WarehouseMovement_Id.ToString();
                             //  Notify change of values at parent window.
                                 UpdateWarehouseMovementInfoInParentWindow();
                             //  Update Is Editing Flag.
                                 WarehouseMovementsAdd.IsEditingWarehouseMovement = -1;
                                 break;
                        case ActionState.Editing:
                             //  Determine if  editing a Warehouse Movement.
                                 if (WarehouseMovementsAdd.IsEditingWarehouseMovement != Movement.WarehouseMovement_Id)
                                 {
                                     MsgManager.ShowMessage(
                                        string.Format("Error, a l'intentar editar el moviment '{0}'.\r\nDetalls: S'està editant el moviment '{1}'.",
                                                      Movement.WarehouseMovement_Id, WarehouseMovementsAdd.IsEditingWarehouseMovement));
                                     return;
                                 }
                             //  Make a copy of current Warehouse Movement.
                                 WarehouseMovementsView CurrentWarehouseMovement = new WarehouseMovementsView(Movement);
                             //  Update Warehouse Movement in the database.
                                 GlobalViewModel.Instance.HispaniaViewModel.UpdateWarehouseMovement(CurrentWarehouseMovement, NewWarehouseMovement, true);
                             //  Unlock information.
                                 UnlockMovement(Movement);
                                 UnlockGood(Movement.Good);
                             //  Update initial movement value.
                                 Movement = GlobalViewModel.Instance.HispaniaViewModel.GetWarehouseMovementFromDb(NewWarehouseMovement);
                             //  Update State values.
                                 OperationState = ActionState.Saved;
                                 InitialState = ActionState.Saved;
                             //  Update controller of data changed.
                                 AreDataChanged = false;
                             //  Update the field associated with the identifier of movement.
                                 tbWarehouseMovement_Id.Text = Movement.WarehouseMovement_Id.ToString();
                             //  Notify change of values at parent window.
                                 UpdateWarehouseMovementInfoInParentWindow();
                             //  Update Is Editing Flag.
                                 WarehouseMovementsAdd.IsEditingWarehouseMovement = -1;
                                 break;
                    }
            }
            catch (Exception ex)
            {
                //  Restore source value if the error affect at one field value.
                    if (ErrorField != WarehouseMovementsAttributes.None)
                    {
                        //  Restore values.
                            EditedMovement.RestoreSourceValue(Movement, ErrorField);
                            LoadDataInWindow(EditedMovement);
                            AreDataChanged = (EditedMovement != Movement);
                    }
                //  Notify to the user the error produced
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex), MsgType.Error);
            }
        }

        #endregion

        #region DeleteWarehouseMovement

        /// <summary>
        /// Manage the event produced when the user would delete one movement that he/she has created.
        /// </summary>
        private void DeleteWarehouseMovement(WarehouseMovementsView MovementToDelete)
        {
            bool MovementLocked = false;
            bool GoodLocked = false;
            try
            {
                if (MovementLocked = LockMovement(MovementToDelete))
                {
                    if (GoodLocked = LockGood(MovementToDelete.Good))
                    {
                        //  Delete movement from the database.
                            GlobalViewModel.Instance.HispaniaViewModel.DeleteWarehouseMovement(MovementToDelete, true);
                        //  Update Good Code information of selected movement.
                            GoodsView good = GlobalViewModel.Instance.HispaniaViewModel.GetGoodFromDb(MovementToDelete.Good);
                        //  Sent an event to remove the data in the parent window.
                            EvDeleteWarehouseMovement?.Invoke(this, ParentControl, good);
                    }
                }
            }
            catch (Exception ex)
            {
                //  Notify at the user the error that is produced.
                    MsgManager.ShowMessage(string.Format("Error, a l'esborrar el moviment '{0}'.\r\nDetalls: {1}",
                                           MovementToDelete.WarehouseMovement_Id, MsgManager.ExcepMsg(ex)));
                //  If its needed Unlock Movement to delete
                    if (MovementLocked) UnlockMovement(MovementToDelete);
                //  If is needed Unlock the Good associated at the Movement to delete.
                    if (GoodLocked) UnlockGood(MovementToDelete.Good);
            }
        }

        #endregion

        #region Edit Warehouse Movement

        /// <summary>
        /// Manage the event produced when the user would edit one movement that he/she has created.
        /// </summary>
        private void EditWarehouseMovement()
        {
            string ErrMsg = "Error, a l'intentar editar el moviment '{0}'.\r\nDetalls: {1}";
            if (WarehouseMovementsAdd.IsEditingWarehouseMovement != -1)
            {
                MsgManager.ShowMessage(
                   string.Format("Error, a l'intentar editar el moviment '{0}'.\r\nDetalls: S'està editant el moviment '{1}'.",
                                 Movement.WarehouseMovement_Id, WarehouseMovementsAdd.IsEditingWarehouseMovement));
                return;
            }
            else WarehouseMovementsAdd.IsEditingWarehouseMovement = Movement.WarehouseMovement_Id;
            try
            {
                //  Actualize Item Information From DataBase
                    Movement = GlobalViewModel.Instance.HispaniaViewModel.GetWarehouseMovementFromDb(Movement);
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format(ErrMsg, Movement.WarehouseMovement_Id, MsgManager.ExcepMsg(ex)));
                return;
            }
            bool MovementLocked = false;
            bool GoodLocked = false;
            try
            {
                //  Lock the components affecteds in the edition
                    if (MovementLocked = LockMovement(Movement))
                    {
                        if (GoodLocked = LockGood(Movement.Good))
                        {
                            //  Update Operation State value.
                                OperationState = ActionState.Editing;
                                InitialState = ActionState.SavedInEdition;
                            //  Update AreDataChanged property.
                                AreDataChanged = false;
                            //  Notify change of values at parent window.
                                UpdateWarehouseMovementInfoInParentWindow();
                        }
                    }
            }
            catch (Exception ex)
            {
                //  Set operation State.
                    OperationState = ActionState.Saved;
                    InitialState = ActionState.Saved;
                //  If its needed Unlock Movement to delete
                    if (MovementLocked) UnlockMovement(Movement);
                //  If is needed Unlock the Good associated at the Movement to delete.
                    if (GoodLocked) UnlockGood(Movement.Good);
                //  Notify at the user the error that is produced.
                    MsgManager.ShowMessage(string.Format(ErrMsg, Movement.WarehouseMovement_Id, MsgManager.ExcepMsg(ex)));
            }
        }

        #endregion

        #region Cancel Changes in Warehouse Movement

        /// <summary>
        /// Actions needed for cancel the changes in the edition of Warehouse Movement.
        /// </summary>
        private void CancelChangesInWarehouseMovement()
        {
            string ErrMsg = string.Empty;
            try
            {
                //  Update EditedMovement information.
                    EditedMovement = new WarehouseMovementsView(Movement);
                //  Load source Data into the Window controls.
                    LoadDataInWindow(EditedMovement);
            }
            catch (Exception ex)
            {
                //  Make a text with the information associated at the error that was produced.
                    ErrMsg = string.Format("Error, al cancel·lar els canvis en el Moviment.\r\nDetalls:{0}", 
                                           MsgManager.ExcepMsg(ex));
            }
            if (String.IsNullOrEmpty(ErrMsg))
            {
                //  Update Operation State value.
                    if (OperationState == ActionState.Editing)
                    {
                        OperationState = ActionState.Saved;
                        InitialState = ActionState.Saved;
                    }
                //  Update AreDataChanged property.
                    AreDataChanged = false;
                //  Sent an event to update the data in the parent window.
                    UpdateWarehouseMovementInfoInParentWindow();
                //  Notify the cancellation of the operation at the user.
                    MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
            }
            else
            {
                //  Show at the user the information associated at the error that was produced.
                    MsgManager.ShowMessage(ErrMsg);
            }
        }

        /// <summary>
        /// Actions needed for cancel the changes in the edition of Warehouse Movement.
        /// </summary>
        private void CancelEdition()
        {
            //  Update Operation State value (by default OperationState is Editing in this case.
                OperationState = ActionState.Saved;
                InitialState = ActionState.Saved;
            //  Update AreDataChanged property.
                AreDataChanged = false;
            //  Notify the cancellation of the operation at the user.
                MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
        }

        #endregion

        #region Lock / Unlock Movements

        private bool LockMovement(WarehouseMovementsView Movement)
        {
            bool Locked;
            if (!(Locked = GlobalViewModel.Instance.HispaniaViewModel.LockRegister(Movement, out string ErrMsg)))
            {
                MsgManager.ShowMessage(ErrMsg);
            }
            return Locked;
        }

        private bool LockGood(GoodsView Good)
        {
            bool Locked;
            if (!(Locked = GlobalViewModel.Instance.HispaniaViewModel.LockRegister(Good, out string ErrMsg)))
            {
                MsgManager.ShowMessage(ErrMsg);
            }
            return Locked;
        }

        private void UnlockMovement(WarehouseMovementsView Movement)
        {
            if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(Movement, out string ErrMsg))
            {
                MsgManager.ShowMessage(ErrMsg);
            }
        }

        private void UnlockGood(GoodsView Good)
        {
            if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(Good, out string ErrMsg))
            {
                MsgManager.ShowMessage(ErrMsg);
            }
        }

        #endregion

        #endregion

        #region Update UI

        private void UpdateWarehouseMovementInfoInParentWindow()
        {
            try
            {
                //  Notify change of values at parent window.
                    EvUpdateInfoWarehouseMovement?.Invoke(EditedMovement, EditedMovement.Good);
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
            }
        }

        private void UpdateUI()
        {
            switch (State)
            {
                case ActionState.InitialState:
                     InitialStateUI();
                     break;
                case ActionState.Editing:
                case ActionState.Adding:
                     EditingUI();
                     break;
                case ActionState.Saved:
                     SavedUI();
                     break;
                case ActionState.SavedInEdition:
                     SavedInEditionUI();
                     break;
                default:
                     throw new InvalidEnumArgumentException("Error, l'estat seleccionat pel control és incorrecte.");
            }
        }

        private void InitialStateUI()
        {
            //  Adapt first button to the new State.
                btnFirstWHMov.Visibility = Visibility.Hidden;
            //  Adapt second button to the new State.
                btnSecondWHMov.Visibility = Visibility.Hidden;
            //  Adapt third button to the new State.
                btnThirdWHMov.Visibility = Visibility.Hidden;
            //  Update the color of the control.
                Background = new SolidColorBrush(Color.FromArgb(255, 255, 232, 232));
                BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 204, 204));
            //  Deactivate edition controls of the Window.
                UpdateActivationControls(true);
                UpdateEditableControls(false);
        }

        private void EditingUI()
        {
            //  Adapt first button to the new State.
                imgbtnFirst.Source = Manager_IU.GetImage("AcceptWarehouseAdd").Source;
                btnFirstWHMov.ToolTip = "Guardar les dades del moviment de magatzem.";
                btnFirstWHMov.Visibility = Visibility.Visible;
            //  Adapt second button to the new State.
                imgbtnSecond.Source = Manager_IU.GetImage("CancelNonText").Source;
                btnSecondWHMov.ToolTip = "Cancel·la la operació actual amb el moviment de magatzem.";
                btnSecondWHMov.Visibility = Visibility.Visible;
            //  Adapt third button to the new State.
                btnThirdWHMov.Visibility = Visibility.Hidden;
            //  Update the color of the control.
                Background = new SolidColorBrush(Color.FromArgb(255, 255, 232, 232));
                BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 204, 204));
            //  Deactivate edition controls of the Window.
                UpdateActivationControls(true);
                UpdateEditableControls(cbGood.SelectedValue != null);
        }

        private void SavedUI()
        {
            //  Adapt first button to the new State.
                imgbtnFirst.Source = Manager_IU.GetImage("Edit").Source;
                btnFirstWHMov.ToolTip = "Edita el moviment de magatzem.";
                btnFirstWHMov.Visibility = Visibility.Visible;
            //  Adapt second button to the new State.
                imgbtnSecond.Source = Manager_IU.GetImage("Trash").Source;
                btnSecondWHMov.ToolTip = "Esborra el moviment de magatzem.";
                btnSecondWHMov.Visibility = Visibility.Visible;
            //  Adapt third button to the new State.
                imgbtnThird.Source = Manager_IU.GetImage("Add").Source;
                btnThirdWHMov.ToolTip = "Afegeix un nou moviment de magatzem.";
                btnThirdWHMov.Visibility = (AddNewMovementPressed) ? Visibility.Hidden : Visibility.Visible;
            //  Update the color of the control.
                Background = new SolidColorBrush(Color.FromArgb(255, 225, 224, 205));
                BorderBrush = new SolidColorBrush(Color.FromArgb(255, 206, 205, 186));
            //  Deactivate edition controls of the Window.
                UpdateActivationControls(false);
                UpdateEditableControls(false);
        }
        
        private void SavedInEditionUI()
        {
            //  Adapt first button to the new State.
                btnFirstWHMov.Visibility = Visibility.Hidden;
            //  Adapt second button to the new State.
                imgbtnSecond.Source = Manager_IU.GetImage("CancelNonText").Source;
                btnSecondWHMov.ToolTip = "Cancel·la la operació actual amb el moviment de magatzem.";
                btnSecondWHMov.Visibility = Visibility.Visible;
            //  Adapt third button to the new State.
                btnThirdWHMov.Visibility = Visibility.Hidden;
            //  Update the color of the control.
                Background = new SolidColorBrush(Color.FromArgb(255, 255, 232, 232));
                BorderBrush = new SolidColorBrush(Color.FromArgb(255, 255, 204, 204));
            //  Deactivate edition controls of the Window.
                UpdateActivationControls(true);
                UpdateEditableControls(cbGood.SelectedValue != null);
        }

        /// <summary>
        /// Update the active state of the edit controls of the Window.
        /// </summary>
        /// <param name="IsActive">Active state to apply</param>
        private void UpdateActivationControls(bool IsActive = true)
        {
            tbGoodCodeFilter.IsEnabled = IsActive;
            cbGood.IsEnabled = IsActive;
        }

        private void UpdateEditableControls(bool IsActive = true)
        { 
            cbType.IsEnabled = IsActive;
            tbAmountUnitBilling.IsEnabled = IsActive;
            tbAmountUnitShipping.IsEnabled = IsActive;
            tbPrice.IsEnabled = IsActive;
            tbProviderNameFilter.IsEnabled = IsActive;
            cbProvider.IsEnabled = IsActive;
            //chkbAccording.IsEnabled = IsActive;
        }

        #endregion
    }
}
