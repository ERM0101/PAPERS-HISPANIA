#region Libraries used for this control

using HispaniaCommon.ViewClientWPF.Windows;
using HispaniaCommon.ViewModel;
using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.Theme;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

#endregion

namespace HispaniaCommon.ViewClientWPF.UserControls
{
    /// <summary>
    /// Interaction logic for BadDebts.xaml
    /// </summary>
    public partial class BadDebtsData : UserControl
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
        /// Cities and Postal Code is correct.
        /// </summary>
        /// <param name="NewOrEditedBadDebts">New or Edited Bad Debt.</param>
        /// <param name="NewOrEditedBadDebtMovements">New or Edited Bad Debt Movements.</param>
        public delegate void dlgAccept(BadDebtsView NewOrEditedBadDebt, List<BadDebtMovementsView> NewOrEditedBadDebtMovements);

        /// <summary>
        /// Delegate that defines the firm of event produced when the Button Cancel is pressed.
        /// </summary>
        public delegate void dlgCancel();

        #endregion

        #region Events

        /// <summary>
        /// Event produced when the Button Accept is pressed and the data of the City and Postal Code is correct.
        /// </summary>
        public event dlgAccept EvAccept;

        /// <summary>
        /// Event produced when the Button Cancel is pressed.
        /// </summary>
        public event dlgCancel EvCancel;

        #endregion

        #region Attributes

        /// <summary>
        /// Store the Bad Debt data to manage.
        /// </summary>
        private BadDebtsView m_BadDebtData = null;

        /// <summary>
        /// Store the Bad Debt Payements data associated at the Bad Deb Data selected to manage.
        /// </summary>
        private ObservableCollection<BadDebtMovementsView> m_BadDebtMovementsList = null;

        /// <summary>
        /// Store the reference at the Bill window used to select the Bill.
        /// </summary>
        private Bills BillSelectionWindow = null;

        /// <summary>
        /// Store the reference at the Receipt Selection window used to select the Reception.
        /// </summary>
        private ReceiptsSelection ReceiptsSelectionWindow = null;

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
        /// Get or Set the BadDebt data to manage.
        /// </summary>
        public BadDebtsView BadDebtData
        {
            get
            {
                return (m_BadDebtData);
            }
            set
            {
                if (value != null)
                {
                    AreDataChanged = false;
                    m_BadDebtData = value;
                    m_BadDebtMovementsList = GlobalViewModel.Instance.HispaniaViewModel.GetBadDebtMovements(m_BadDebtData.BadDebt_Id);
                    EditedBadDebts = new BadDebtsView(m_BadDebtData);
                    EditedBadDebtMovementsList = new ObservableCollection<BadDebtMovementsView>(m_BadDebtMovementsList);
                    LoadDataInControls(m_BadDebtData);
                    LoadDataInControls(m_BadDebtMovementsList);
                    RefreshButtons();
                }
                else throw new ArgumentNullException("Error, no s'han trobat les dades de l'Impagat a carregar."); 
            }
        }

        /// <summary>
        /// Get or Set the Edited Bad Debt information.
        /// </summary>
        private ObservableCollection<BadDebtMovementsView> BadDebtMovementsList
        {
            get
            {
                return (m_BadDebtMovementsList);
            }
            set
            {
                if (value != null)
                {
                    m_BadDebtMovementsList = value;
                    EditedBadDebtMovementsList = new ObservableCollection<BadDebtMovementsView>(m_BadDebtMovementsList);
                    LoadDataInControls(m_BadDebtMovementsList);
                    RefreshButtons();
                }
                else throw new ArgumentNullException("Error, no s'han trobat les dades dels pagaments de l'Impagat a carregar.");
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
                         if (BadDebtData == null) throw new InvalidOperationException("Error, impossible visualitzar un Impagat sense dades.");
                         tbCancel.Text = "Tornar";
                         break;
                    case Operation.Add:
                         BadDebtData = new BadDebtsView();
                         tbCancel.Text = "Cancel·lar";
                         break;
                    case Operation.Edit:
                         if (BadDebtData == null) throw new InvalidOperationException("Error, impossible editar un Impagat sense dades.");
                         tbCancel.Text = "Cancel·lar";
                         break;
                }
                foreach (Control control in EditableControls)
                {
                    if (control is TextBox) ((TextBox)control).IsReadOnly = (m_CtrlOperation == Operation.Show);
                    else if (control is RichTextBox) ((RichTextBox)control).IsReadOnly = (m_CtrlOperation == Operation.Show);
                    else control.IsEnabled = (m_CtrlOperation != Operation.Show);
                }
                RefreshButtons();
            }
        }

        /// <summary>
        /// Get or Set the Edited Bad Debt information.
        /// </summary>
        private BadDebtsView EditedBadDebts
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set the Edited Bad Debt information.
        /// </summary>
        private ObservableCollection<BadDebtMovementsView> EditedBadDebtMovementsList
        {
            get;
            set;
        }

        /// <summary>
        /// Get or Set Payement temporary property for Payements operations.
        /// </summary>
        private BadDebtMovementsView Payement { get; set; }

        /// <summary>
        /// Get or Set Payement Operation active.
        /// </summary>
        private Operation Op { get; set; }

        /// <summary>
        /// Get or Set if the data of the City and Postal Code has changed.
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
        public BadDebtsData()
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
            NonEditableControls = new List<Control>()
            {
                lblBillId,
                tbBillId,
                lblBillDate,
                tbBillDate,
                lblBillSerieId,
                tbBillSerieId,
                lblCustomerId,
                tbCustomerId,
                lblCustomerAlias,
                tbCustomerAlias,
                lblCompanyName,
                tbCompanyName,
                lblEffectType,
                tbEffectType,
                lblReceiptId,
                tbReciptId,
                lblExpirationDate,
                tbExpirationDate,
                lblAmount,
                tbAmount,
                lblAmountPending,
                tbAmountPending,
                tbPayementDate
            };
        }

        /// <summary>
        /// Initialize the list of Editable Controls.
        /// </summary>
        private void InitEditableControls()
        {
            EditableControls = new List<Control>
            {
                btnSelectBill,
                gbBillInfo,
                btnSelectReceipt,
                gbReceiptInfo,
                lblPayementDate,
                dtpPayementDate,
                lblAmountPayement,
                tbAmountPayement,
                gbBadDebtInfo,
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
        private void LoadDataInControls(BadDebtsView badDebtsView)
        {
            DataChangedManagerActive = false;
            LoadDataFromBill(badDebtsView.Bill);
            LoadDataFromReceipt(badDebtsView.Receipt, false);
            tbAmountPending.Text = badDebtsView.Amount_Pending_Str;
            DataChangedManagerActive = true;
        }

        /// <summary>
        /// Method that loads the Data in the controls of the Window
        /// </summary>
        private void LoadDataInControls(ObservableCollection<BadDebtMovementsView> badDebtMovementsList)
        {
            DataChangedManagerActive = false;
            PayementsListItems.ItemsSource = badDebtMovementsList;
            PayementsListItems.DataContext = this;
            CollectionViewSource.GetDefaultView(PayementsListItems.ItemsSource).SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Ascending));
            PayementsListItems.UpdateLayout();
            DataChangedManagerActive = true;
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
                tbAmountPayement.PreviewTextInput += TBPreviewTextInput;
                tbAmountPayement.TextChanged += TBCurrencyDataChanged;
            //  DatePiker
                dtpPayementDate.SelectedDateChanged += DtpPayementDate_SelectedDateChanged;
            //  Buttons
                btnAccept.Click += BtnAccept_Click;
                btnCancel.Click += BtnCancel_Click;
                btnSelectBill.Click += BtnSelectBill_Click;
                btnSelectReceipt.Click += BtnSelectReceipt_Click;
                btnAddPayement.Click += BtnAddPayement_Click;
                btnEditPayement.Click += BtnEditPayement_Click;
                btnDeletePayement.Click += BtnDeletePayement_Click;
                btnAcceptPayement.Click += BtnAcceptPayement_Click;
                btnCancelPayement.Click += BtnCancelPayement_Click;
        }

        #region Filter

        /// <summary>
        /// Select all text in sender TextBox control.
        /// </summary>
        /// <param name="sender">TextBox control that has produced the event.</param>
        /// <param name="e">Parameters associateds to the event.</param>
        private void TBGotFocus(object sender, RoutedEventArgs e)
        {
            GlobalViewModel.Instance.SelectAllTextInGotFocusEvent(sender, e);
        }

        #endregion

        #region TextBox

        /// <summary>
        /// Checking the Input Char in function of the data type that has associated
        /// </summary>
        private void TBPreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (sender == tbAmountPayement) e.Handled = !GlobalViewModel.IsValidCurrencyChar(e.Text) || e.Text == "-";
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
                    if (sender == tbAmountPayement)
                    {
                        if (EditedBadDebts.Amount_Pending - value < 0)
                        {
                            MsgManager.ShowMessage("Error, el valor del pagament supera l'import pendent de pagar.");
                            tbAmountPayement.Text = GlobalViewModel.GetStringFromDecimalValue(Payement.Amount, DecimalType.Currency);
                        }
                        else
                        {
                            Payement.Amount = value;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
                }
                AreDataChanged = (EditedBadDebts != BadDebtData) || (!AreBadDebtMovementListEquals());
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
            BadDebtsAttributes ErrorField = BadDebtsAttributes.None;
            try
            {
                if ((CtrlOperation == Operation.Add) || (CtrlOperation == Operation.Edit))
                {
                    EditedBadDebts.Validate(out ErrorField);
                    EvAccept?.Invoke(new BadDebtsView(EditedBadDebts), new List<BadDebtMovementsView>(EditedBadDebtMovementsList));
                }
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al validar les dades introduïdes.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                RestoreSourceValues();
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

        #region Selected Bill

        /// <summary>
        /// Manage the selection of a Bill.
        /// </summary>
        /// <param name="sender">Object that has throwed the event.</param>
        /// <param name="e">Parameters with the event has been sent.</param>
        private void BtnSelectBill_Click(object sender, RoutedEventArgs e)
        {
            if (BillSelectionWindow == null)
            {
                try
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.BillsWindow);
                    BillSelectionWindow = new Bills(AppType)
                    {
                        Print = false,
                        Selection = true,
                        SelectedBill = EditedBadDebts.Bill,
                        EffectTypes = GlobalViewModel.Instance.HispaniaViewModel.EffectTypesDict,
                        BillingSeries = GlobalViewModel.Instance.HispaniaViewModel.BillingSeriesDict,
                        Parameters = GlobalViewModel.Instance.HispaniaViewModel.Parameters,
                        Agents = GlobalViewModel.Instance.HispaniaViewModel.AgentsActiveDict,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.Bills
                    };
                    BillSelectionWindow.Closed += BillSelectionWindow_Closed;
                    BillSelectionWindow.ShowDialog();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, a l'obrir la finestra de Selecció de Factures.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else BillSelectionWindow.Activate();
        }

        private void BillSelectionWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                if (BillSelectionWindow.SelectedBill != null)
                {
                    EditedBadDebts.Bill_Id = BillSelectionWindow.SelectedBill.Bill_Id;
                    EditedBadDebts.Bill_Year = BillSelectionWindow.SelectedBill.Year;
                    EditedBadDebts.Bill_Serie_Id = BillSelectionWindow.SelectedBill.BillingSerie.Serie_Id;
                    LoadDataFromBill(EditedBadDebts.Bill);
                    if (EditedBadDebts.Bill != BadDebtData.Bill)
                    {
                        EditedBadDebts.Receipt_Id = GlobalViewModel.IntIdInitValue;
                        EditedBadDebts.Amount_Pending = GlobalViewModel.DecimalInitValue;
                        LoadDataFromReceipt(EditedBadDebts.Receipt);
                        EditedBadDebtMovementsList.Clear();
                        LoadDataInControls(EditedBadDebtMovementsList);
                    }
                    AreDataChanged = (EditedBadDebts != BadDebtData) || (!AreBadDebtMovementListEquals());
                    RefreshButtons();
                }
                BillSelectionWindow.Closed -= BillSelectionWindow_Closed;
                BillSelectionWindow = null;
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, a traspasar la informació de la Factura seleccionada.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                LoadDataFromBill(BadDebtData.Bill);
            }
        }

        #endregion

        #region Select Receipt

        /// <summary>
        /// Manage the creation of the Report of selected Item and Print this Item.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnSelectReceipt_Click(object sender, RoutedEventArgs e)
        {
            if (ReceiptsSelectionWindow is null)
            {
                BillsView bill = EditedBadDebts.Bill;
                if (bill is null)
                {
                    MsgManager.ShowMessage("Avís, no hi ha cap factura seleccionada, s'ha de seleccionar primer una factura.", MsgType.Warning);
                }
                else
                {
                    ReceiptsSelectionWindow = new ReceiptsSelection(AppType)
                    {
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.GetReceiptsFromBill(bill.Bill_Id, bill.Year),
                        SelectionModeList = SelectionMode.Single,
                        Bill = bill,
                    };
                    ReceiptsSelectionWindow.Closed += ReceiptsSelectionWindow_Closed;
                    ReceiptsSelectionWindow.ShowDialog();
                }
            }
            else ReceiptsSelectionWindow.Activate();
        }

        private void ReceiptsSelectionWindow_Closed(object sender, EventArgs e)
        {
            try
            {
                List<ReceiptsView> ReceiptsSelecteds = ReceiptsSelectionWindow.ReceiptsSelecteds;
                if ((ReceiptsSelectionWindow.Result == SelectionResult.Accept) && (ReceiptsSelecteds != null))
                {
                    if (ReceiptsSelecteds.Count == 1)
                    {
                        EditedBadDebts.Receipt_Id = ReceiptsSelecteds[0].Receipt_Id;
                        EditedBadDebts.Amount_Pending = ReceiptsSelecteds[0].Amount;
                        LoadDataFromReceipt(EditedBadDebts.Receipt);
                        if (EditedBadDebts.Receipt != BadDebtData.Receipt)
                        {
                            EditedBadDebtMovementsList.Clear();
                            LoadDataInControls(EditedBadDebtMovementsList);
                        }
                        AreDataChanged = (EditedBadDebts != BadDebtData) || (!AreBadDebtMovementListEquals());
                        RefreshButtons();
                    }
                }
                ReceiptsSelectionWindow.Closed -= ReceiptsSelectionWindow_Closed;
                ReceiptsSelectionWindow = null;
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, a traspasar la informació del Rebut seleccionat.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                LoadDataFromBill(BadDebtData.Bill);
            }
        }

        #endregion

        #region Editing Payements

        /// <summary>
        /// Manage the creation of a new Payement for this item.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnAddPayement_Click(object sender, RoutedEventArgs e)
        {
            Payement = new BadDebtMovementsView()
            {
                BadDebt_Id = EditedBadDebts.BadDebt_Id,
                Date = DateTime.Now
            };
            Op = Operation.Add;
            RefreshButtons(true);
        }

        /// <summary>
        /// Manage the creation of a new Payement for this item.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnEditPayement_Click(object sender, RoutedEventArgs e)
        {
            if (PayementsListItems.SelectedItem != null)
            {
                Payement = new BadDebtMovementsView((BadDebtMovementsView) PayementsListItems.SelectedItem);
                EditedBadDebts.Amount_Pending += Payement.Amount;
                tbAmountPending.Text = EditedBadDebts.Amount_Pending_Str;
                DataChangedManagerActive = false;
                tbAmountPayement.Text = GlobalViewModel.GetStringFromDecimalValue(Payement.Amount, DecimalType.Currency);
                dtpPayementDate.SelectedDate = Payement.Date;
                tbPayementDate.Text = GlobalViewModel.GetStringFromDateTimeValue(Payement.Date);
                DataChangedManagerActive = true;
                Op = Operation.Edit;
                RefreshButtons(true);
            }
        }

        /// <summary>
        /// Manage the button for edit an Item of the list.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnDeletePayement_Click(object sender, RoutedEventArgs e)
        {
            if (PayementsListItems.SelectedItem != null)
            {
                BadDebtMovementsView BadDebtMovementToDelete = (BadDebtMovementsView)PayementsListItems.SelectedItem;
                if (MsgManager.ShowQuestion("Està segur que vol esborrar el pagament seleccionat ?") == MessageBoxResult.Yes)
                {
                    try
                    {
                        EditedBadDebts.Amount_Pending += BadDebtMovementToDelete.Amount;
                        tbAmountPending.Text = EditedBadDebts.Amount_Pending_Str;
                        EditedBadDebtMovementsList.Remove(BadDebtMovementToDelete);
                        LoadDataInControls(EditedBadDebtMovementsList);
                        PayementsListItems.SelectedItem = Payement;
                        ClearPayementControlsData();
                        RefreshButtons();
                        AreDataChanged = (EditedBadDebts != BadDebtData) || (!AreBadDebtMovementListEquals());
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(string.Format("Error, a l'esborrar el pagament seleccionat.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                    }
                }
                else MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
            }
        }

        /// <summary>
        /// Manage the acknoledge of a creation of a new Payement for this item.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnAcceptPayement_Click(object sender, RoutedEventArgs e)
        {
            switch (Op)
            {
                case Operation.Add:
                     EditedBadDebts.Amount_Pending -= Payement.Amount;
                     tbAmountPending.Text = EditedBadDebts.Amount_Pending_Str;
                     EditedBadDebtMovementsList.Add(Payement);
                     break;
                case Operation.Edit:
                     EditedBadDebts.Amount_Pending -= Payement.Amount;
                     tbAmountPending.Text = EditedBadDebts.Amount_Pending_Str;
                     EditedBadDebtMovementsList.Remove((BadDebtMovementsView) PayementsListItems.SelectedItem);
                     EditedBadDebtMovementsList.Add(Payement);
                     break;
            }
            LoadDataInControls(EditedBadDebtMovementsList);
            PayementsListItems.SelectedItem = Payement;
            ClearPayementControlsData();
            RefreshButtons();
            AreDataChanged = (EditedBadDebts != BadDebtData) || (!AreBadDebtMovementListEquals());

        }

        /// <summary>
        /// Manage the acknoledge of a creation of a new Payement for this item.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnCancelPayement_Click(object sender, RoutedEventArgs e)
        {
            MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
            switch (Op)
            {
                case Operation.Edit:
                     EditedBadDebts.Amount_Pending -= Payement.Amount;
                     tbAmountPending.Text = EditedBadDebts.Amount_Pending_Str;
                     break;
                default:
                     break;
            }
            ClearPayementControlsData();
            RefreshButtons();
        }

        #endregion

        #endregion

        #region DatePicket

        private void DtpPayementDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                Payement.Date = (DateTime)dtpPayementDate.SelectedDate;
                tbPayementDate.Text = GlobalViewModel.GetStringFromDateTimeValue(Payement.Date);
                AreDataChanged = (EditedBadDebts != BadDebtData) || (!AreBadDebtMovementListEquals());
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #endregion

        #region UpdateUI Methods

        private void RefreshButtons(bool IsEditing = false)
        {
            bool IsReceiptSelected = EditedBadDebts.Receipt_Id != GlobalViewModel.IntIdInitValue;
            bool IsPayementSelected = PayementsListItems.SelectedItem != null;
            bool IsEditableOperation = CtrlOperation == Operation.Add || CtrlOperation == Operation.Edit;
            btnAddPayement.IsEnabled = IsReceiptSelected && !IsEditing && EditedBadDebts.Amount_Pending > 0 && IsEditableOperation;
            btnEditPayement.IsEnabled = btnDeletePayement.IsEnabled = IsReceiptSelected && IsPayementSelected && !IsEditing && IsEditableOperation;
            btnSelectBill.IsEnabled = btnSelectReceipt.IsEnabled = !IsEditing && IsEditableOperation;
            dtpPayementDate.IsEnabled = tbAmountPayement.IsEnabled = IsEditing ;
            PayementsListItems.IsEnabled = IsReceiptSelected && !IsEditing;
            btnAccept.IsEnabled = btnCancel.IsEnabled = !IsEditing;
            Visibility PayementVisibility = IsEditing ? Visibility.Visible : Visibility.Hidden;
            lblAmountPayement.Visibility = tbAmountPayement.Visibility = lblPayementDate.Visibility = PayementVisibility;
            dtpPayementDate.Visibility = tbPayementDate.Visibility = btnAcceptPayement.Visibility = PayementVisibility;
            btnCancelPayement.Visibility = PayementVisibility;
            Color color = IsEnabled ? Color.FromArgb(255, 8, 70, 124) : Color.FromArgb(255, 155, 33, 29);
            dtpPayementDate.BorderBrush = new SolidColorBrush(color);
        }

        private void ClearPayementControlsData()
        {
            DataChangedManagerActive = false;
            tbAmountPayement.Text = tbPayementDate.Text = string.Empty;
            dtpPayementDate.SelectedDate = null;
            DataChangedManagerActive = true;
        }

        private void LoadDataFromBill(BillsView bill)
        {
            if (bill is null)
            {
                tbBillId.Text = tbBillDate.Text = tbBillSerieId.Text = tbCustomerId.Text = "No Informat";
                tbCustomerAlias.Text = tbCompanyName.Text = tbEffectType.Text = "No Informat";
            }
            else
            {
                tbBillId.Text = bill.Bill_Id_Str;
                tbBillDate.Text = bill.Bill_Date_Str;
                tbBillSerieId.Text = bill.BillingSerie_Str;
                tbCustomerId.Text = bill.Customer_Id_Str;
                tbCustomerAlias.Text = bill.Customer_Alias;
                tbCompanyName.Text = bill.Company_Name;
                tbEffectType.Text = bill.EffectType_Str;
            }
        }

        private void LoadDataFromReceipt(ReceiptsView receipt, bool actualizeAmountPending = true)
        {
            if (receipt is null)
            {
                tbReciptId.Text = tbExpirationDate.Text = tbAmount.Text = "No Informat";
                if (actualizeAmountPending) tbAmountPending.Text = "No Informat";
            }
            else
            {
                tbReciptId.Text = receipt.Receipt_Id_Str;
                tbExpirationDate.Text = receipt.Expiration_Date_Str;
                tbAmount.Text = receipt.Amount_Str;
                if (actualizeAmountPending) tbAmountPending.Text = receipt.Amount_Str;
            }
        }

        private bool AreBadDebtMovementListEquals()
        {
            if (BadDebtMovementsList.Count != EditedBadDebtMovementsList.Count) return false; // the collections are not equal
            foreach (BadDebtMovementsView item in BadDebtMovementsList)
            {
                if (!EditedBadDebtMovementsList.Contains(item)) return false; // the collections are not equal
            }
            foreach (BadDebtMovementsView item in EditedBadDebtMovementsList)
            {
                if (!BadDebtMovementsList.Contains(item)) return false; // the collections are not equal
            }
            return true; // the collections are equal
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValues()
        {
            EditedBadDebts.RestoreSourceValues(BadDebtData);
            EditedBadDebtMovementsList = new ObservableCollection<BadDebtMovementsView>(BadDebtMovementsList);
            LoadDataInControls(EditedBadDebts);
            LoadDataInControls(EditedBadDebtMovementsList);
            RefreshButtons();
            AreDataChanged = (EditedBadDebts != BadDebtData) || (!AreBadDebtMovementListEquals());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(BadDebtsAttributes ErrorField)
        {
            EditedBadDebts.RestoreSourceValue(BadDebtData, ErrorField);
            LoadDataInControls(EditedBadDebts);
            LoadDataInControls(EditedBadDebtMovementsList);
            RefreshButtons();
            AreDataChanged = (EditedBadDebts != BadDebtData) || (!AreBadDebtMovementListEquals());
        }

        #endregion
    }
}
