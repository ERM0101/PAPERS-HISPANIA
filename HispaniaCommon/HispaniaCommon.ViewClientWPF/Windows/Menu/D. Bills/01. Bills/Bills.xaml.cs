#region Librerias usadas por la ventana

using HispaniaCommon.ViewClientWPF.UserControls;
using HispaniaCommon.ViewModel;
using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.Theme;
using MBCode.Framework.Managers.Trace;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

#endregion

namespace HispaniaCommon.ViewClientWPF.Windows
{
    public enum SelectionResult
    {
        Accept,
        Cancel,
    }

    /// <summary>
    /// Interaction logic for CustomerOrders.xaml
    /// </summary>
    public partial class Bills : Window
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
        /// Show the Add Delivery Note button.
        /// </summary>
        private GridLength ViewAddDeliveryNoteButtonPannel = new GridLength(120.0);

        /// <summary>
        /// Show the Remove Delivery Note button.
        /// </summary>
        private GridLength ViewRemoveDeliveryNoteButtonPannel = new GridLength(110.0);

        /// <summary>
        /// Show the Edit Bill button.
        /// </summary>
        private GridLength ViewEditButtonPannel = new GridLength(90.0);

        /// <summary>
        /// Show the Select Bill button.
        /// </summary>
        private GridLength ViewSelectButtonPannel = new GridLength(100.0);

        /// <summary>
        /// Show the Select Cancel Bill button.
        /// </summary>
        private GridLength ViewSelectCancelButtonPannel = new GridLength(90.0);

        /// <summary>
        /// Show the Show Bill button.
        /// </summary>
        private GridLength ViewShowButtonPannel = new GridLength(120.0);

        /// <summary>
        /// Show the Print Bill button.
        /// </summary>
        private GridLength ViewPrintBillButtonPannel = new GridLength(130.0);

        /// <summary>
        /// Show the Send by EMail button.
        /// </summary>
        private GridLength ViewSendByEMailButtonPannel = new GridLength(180.0);

        /// <summary>
        /// Show the Print Receipt button.
        /// </summary>
        private GridLength ViewPrintReceiptButtonPannel = new GridLength(130.0);

        /// <summary>
        /// Show the Create Report button.
        /// </summary>
        private GridLength ViewCreateReportButtonPannel = new GridLength(140.0);

        /// <summary>
        /// Show the Item Pannel.
        /// </summary>
        private GridLength ViewItemPannel = new GridLength(600.0);

        /// <summary>
        /// Show the Serach Pannel.
        /// </summary>
        private GridLength ViewSearchPannel = new GridLength(30.0);

        /// <summary>
        /// Show the Operation Pannel.
        /// </summary>
        private GridLength ViewOperationPannel = new GridLength(30.0);

        /// <summary>
        /// Hide the ItemPannel.
        /// </summary>
        private GridLength HideComponent = new GridLength(0.0);

        #endregion

        #region Attributes

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private ObservableCollection<BillsView> m_DataList = new ObservableCollection<BillsView>();

        /// <summary>
        /// Store the value that indicates if the Window allows impression or not.
        /// </summary>
        private bool m_Print = false;

        /// <summary>
        /// Store the value that indicates if the Window opened for selection or not.
        /// </summary>
        private bool m_Selection = false;

        /// <summary>
        /// Window to manage the Selection of Customer Orders of a Bill for Remove or the Customer Orders non 
        /// assigned at a Bill for add to a Bill.
        /// </summary>
        private CustomerOrdersSelection AddCustomerOrdersWindow;

        /// <summary>
        /// Window to manage the Selection of Customer Orders of a Bill for Remove or the Customer Orders non 
        /// assigned at a Bill for add to a Bill.
        /// </summary>
        private CustomerOrdersSelection RemoveCustomerOrdersWindow;

        /// <summary>
        /// Window to manage the Selection of Receipts of a Bill for Print this items.
        /// </summary>
        private ReceiptsSelection PrintReceiptsWindow;

        /// <summary>
        /// Window to manage the Selection of Receipts of a Bill for Send by E-mail this items.
        /// </summary>
        private ReceiptsSelection SendByEMailReceiptsWindow;

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
            get;
            private set;
        }

        /// <summary>
        /// Get or Set the data to show in List of Items.
        /// </summary>
        public ObservableCollection<BillsView> DataList
        {
            get
            {
                return (m_DataList);
            }
            set
            {
                //  Actualize the value
                    if (value != null) m_DataList = value;
                    else m_DataList = new ObservableCollection<BillsView>();
                //  Deactivate managers
                    DataChangedManagerActive = false;
                //  Set up controls state
                    ListItems.ItemsSource = m_DataList;
                    ListItems.DataContext = this;
                    CollectionViewSource.GetDefaultView(ListItems.ItemsSource).SortDescriptions.Add(new SortDescription("Bill_Id", ListSortDirection.Descending));
                    CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Filter = UserFilter;
                    lblCountBills.Content = DataList.Count.ToString();
                    if (m_DataList.Count > 0) ListItems.SelectedIndex = 0;  
                //  Reactivate managers
                    DataChangedManagerActive = true;
                //  Refresh Button Bar
                    RefreshButtonBar();
            }
        }

        /// <summary>
        /// Get or Set the value that indicates if the Window allows impression or not.
        /// </summary>
        public bool Print
        {
            get
            {
                return (m_Print);
            }
            set
            {
                //  Actualize the value
                    m_Print = value;
                //  Deactivate managers
                    DataChangedManagerActive = false;
                //  Set up controls state
                    Title = m_Print ? "Impressió de Factures i Rebuts" : "Gestió de Factures";
                    ListItems.SelectionMode = m_Print && !m_Selection ? SelectionMode.Extended : SelectionMode.Single;
                //  Reactivate managers
                    DataChangedManagerActive = true;
            }
        }

        public bool Selection
        {
            get
            {
                return (m_Selection);
            }
            set
            {
                //  Actualize the value
                    m_Selection = value;
                //  Deactivate managers
                    DataChangedManagerActive = false;
                //  Set up controls state
                    if (m_Selection) Title = "Selecció de Factures";
                    ListItems.SelectionMode = m_Print && !m_Selection ? SelectionMode.Extended : SelectionMode.Single;
                //  Reactivate managers
                    DataChangedManagerActive = true;
            }
        }

        public BillsView SelectedBill { get; set; }

        private bool AreViewingNowYear
        {
            get
            {
                return (cbYear.SelectedValue.ToString() == DateTime.Now.Year.ToString());
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

        #region Foreign Tables Info

        /// <summary>
        /// Get or Set the Parameters
        /// </summary>
        public ParametersView Parameters
        {
            set
            {
                BillDataControl.Parameters = value;
            }
        }

        /// <summary>
        /// Get or Set the Effect Types
        /// </summary>
        public Dictionary<string, EffectTypesView> EffectTypes
        {
            set
            {
                BillDataControl.EffectTypes = value;
            }
        }

        /// <summary>
        /// Get or Set the Billing Series
        /// </summary>
        public Dictionary<string, BillingSeriesView> BillingSeries
        {
            set
            {
                BillDataControl.BillingSeries = value;
            }
        }

        /// <summary>
        /// Get or Set the Agents
        /// </summary>
        public Dictionary<string, AgentsView> Agents
        {
            set
            {
                BillDataControl.Agents = value;
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
        public Bills(ApplicationType AppType)
        {
            InitializeComponent();
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
            //  Apply Theme to window.
                ThemeManager.ActualTheme = AppTheme;
                BillDataControl.AppType = AppType;
            //  Initialize state of Window components.
                rdItemPannel.Height = HideComponent;
                rdSearchPannel.Height = HideComponent;
                gsSplitter.IsEnabled = false;
        }

        /// <summary>
        /// Method that load data in Window components.
        /// </summary>
        private void LoadDataInWindowComponents()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Set Data into the Window.
                cbFieldItemToSearch.ItemsSource = BillsView.Fields;
                cbFieldItemToSearch.DisplayMemberPath = "Key";
                cbFieldItemToSearch.SelectedValuePath = "Value";
                if (CustomerOrdersView.Fields.Count > 0) cbFieldItemToSearch.SelectedIndex = 0;
                cbYear.ItemsSource = GlobalViewModel.Instance.HispaniaViewModel.GetBillYears();
                cbYear.DisplayMemberPath = "Key";
                cbYear.SelectedValuePath = "Value";
                cbYear.SelectedIndex = 0;
                CollectionViewSource.GetDefaultView(cbYear.ItemsSource).SortDescriptions.Add(new SortDescription("Key", ListSortDirection.Descending));
                cbMonth.ItemsSource = RepositoryViewModel.Month;
                cbMonth.DisplayMemberPath = "Value";
                cbMonth.SelectedValuePath = "Key";
                cbMonth.SelectedIndex = 1;
                lblMonth.Visibility = Visibility.Hidden;
                cbMonth.Visibility = Visibility.Hidden;
            //  Activate managers
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
                if (cbFieldItemToSearch.SelectedIndex == -1)  return (true);
            //  Apply the filter by selected field value
                BillsView ItemData = (BillsView)item;
                String ProperyName = (string)cbFieldItemToSearch.SelectedValue;            
                if (!String.IsNullOrEmpty(tbItemToSearch.Text))
                {
                    object valueToTest = ItemData.GetType().GetProperty(ProperyName).GetValue(ItemData);
                    if ((valueToTest is null) || 
                        (!(valueToTest.ToString().ToUpper()).Contains(tbItemToSearch.Text.ToUpper())))
                    {
                        return false;
                    }
                }
                return (true);
        }

        #endregion

        #region Managers

        /// <summary>
        /// Method that define the managers needed for the user operations in the Window
        /// </summary>
        private void LoadManagers()
        {
            //  Window
                this.Closed += Bills_Closed;
            //  TextBox
                tbItemToSearch.TextChanged += TbItemToSearch_TextChanged;
            //  Button Search Clients
                btnEdit.Click += BtnEdit_Click;
                btnViewData.Click += BtnViewData_Click;
                btnPrintBill.Click += BtnPrintBill_Click;
                btnPrintReceipt.Click += BtnPrintReceipt_Click;
                btnSendBillByEmail.Click += BtnSendBillByEMail_Click;
                btnSendReceiptByEmail.Click += BtnSendReceiptByEMail_Click;
                btnAddDeliveryNote.Click += BtnAddDeliveryNote_Click;
                btnRemoveDeliveryNote.Click += BtnRemoveDeliveryNote_Click;
                btnRefresh.Click += BtnRefresh_Click;
                btnSelect.Click += BtnSelect_Click;
                btnSelectCancel.Click += BtnSelectCancel_Click;
            //  ComboBox
                cbYear.SelectionChanged += CbYear_SelectionChanged;
                cbMonth.SelectionChanged += CbMonth_SelectionChanged;
            //  Define ListView events to manage.
                ListItems.SelectionChanged += ListItems_SelectionChanged;
            //  Define BillDataControl events to manage.
                BillDataControl.EvAccept += BillDataControl_evAccept;
                BillDataControl.EvCancel += BillDataControl_evCancel;
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

        /// <summary>
        /// Manage the search of Items in the list.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void TbItemToSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Refresh();
                RefreshButtonBar();
                DataChangedManagerActive = true;
            }
        }

        private void CbYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                if (cbYear.SelectedItem != null)
                {
                    decimal Year = (decimal)cbYear.SelectedValue;
                    try
                    {
                        if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                        bool IsCurrentYear = (Year == DateTime.Now.Year);
                        lblMonth.Visibility = IsCurrentYear ? Visibility.Hidden : Visibility.Visible;
                        cbMonth.Visibility = IsCurrentYear ? Visibility.Hidden : Visibility.Visible;
                        if (IsCurrentYear) GlobalViewModel.Instance.HispaniaViewModel.RefreshBills(Year);
                        else
                        {
                            cbMonth.SelectedIndex = 1;
                            GlobalViewModel.Instance.HispaniaViewModel.RefreshBills(Year, 1);
                        }
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.Bills;
                        RefreshButtonBar();
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(
                           string.Format("Error, al carregar les factures de l'any '{0}'.\r\nDetalls: {1}",
                                         Year, MsgManager.ExcepMsg(ex)));
                    }
                }
                DataChangedManagerActive = true;
            }
        }

        private void CbMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                if ((cbYear.SelectedItem != null) && (cbMonth.SelectedItem != null))
                {
                    decimal Year = (decimal)cbYear.SelectedValue;
                    int Month = (int)cbMonth.SelectedValue;
                    try
                    {
                        if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                        GlobalViewModel.Instance.HispaniaViewModel.RefreshBills(Year, Month);
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.Bills;
                        RefreshButtonBar();
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(
                           string.Format("Error, al carregar les factures del mes '{0}' de l'any '{1}'.\r\nDetalls: {2}",
                                         Month, Year, MsgManager.ExcepMsg(ex)));
                    }
                }
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region Button

        #region Editing Bills

        /// <summary>
        /// Manage the Button Mouse Click in one of the items of the List to show its data in User Control.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnViewData_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItem != null)
            {
                // No cal bloquejar els Albarans i els Rebuts perquè a l'editar l'albarà ja es bloqueja la factura.
                try
                {
                    ActualizeBillFromDb();
                    if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(ListItems.SelectedItem, out string ErrMsg))
                    {
                        BillDataControl.CtrlOperation = Operation.Show;
                        gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                        ShowItemPannel();
                    }
                    else MsgManager.ShowMessage(ErrMsg);
                }
                catch (Exception ex)
                {
                    BillsView bill = (BillsView)ListItems.SelectedItem;
                    MsgManager.ShowMessage(
                       string.Format("Error, al presentar les dades de la factura '{0}'.\r\nDetalls: {1}",
                                     string.Format("'{0}' de l'any '{1}'", bill.Bill_Id, bill.Year), MsgManager.ExcepMsg(ex)));
                }
            }
        }

        /// <summary>
        /// Manage the button for edit an Item of the list.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItem != null)
            {
                try
                {
                    ActualizeBillFromDb();
                    BillsView billsView = (BillsView)ListItems.SelectedItem;
                    if (billsView.CanModifyBill)
                    {
                        // No cal bloquejar els Albarans i els Rebuts perquè a l'editar l'albarà ja es bloqueja la factura.
                        if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(ListItems.SelectedItem, out string ErrMsg))
                        {
                            BillDataControl.Bill = (BillsView)ListItems.SelectedItem;
                            BillDataControl.CtrlOperation = Operation.Edit;
                            gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "EditableGroupBox");
                            ShowItemPannel();
                        }
                        else MsgManager.ShowMessage(ErrMsg);
                    }
                    else MsgManager.ShowMessage("Avís, no es pot modificar la factura degut a que ja s'ha cobrat com a mínim un dels rebuts.", MsgType.Warning);
                }
                catch (Exception ex)
                {
                    BillsView bill = (BillsView)ListItems.SelectedItem;
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'editar la factura '{0}'.\r\nDetalls: {1}",
                                     string.Format("'{0}' de l'any '{1}'", bill.Bill_Id, bill.Year), MsgManager.ExcepMsg(ex)));
                }
            }
        }

        /// <summary>
        /// Manage the event produced when the operation in that was doing in the BillDataControl was Accepted.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BillDataControl_evAccept(BillsView NewOrEditedBill)
        {
            try
            {
                BillsView NewBill = new BillsView(NewOrEditedBill);
                switch (BillDataControl.CtrlOperation)
                {
                    case Operation.Edit:
                         GlobalViewModel.Instance.HispaniaViewModel.UpdateBill(NewBill);
                         if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(BillDataControl.Bill, out string ErrMsg))
                         {
                             MsgManager.ShowMessage(ErrMsg);
                         }
                         DataChangedManagerActive = false;
                         BillsView SourceBill = (BillsView)ListItems.SelectedItem;
                         if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                         DataList.Remove(SourceBill);
                         DataList.Add(GlobalViewModel.Instance.HispaniaViewModel.GetBillFromDb(NewBill));
                         DataChangedManagerActive = true;
                         ListItems.SelectedItem = NewBill;
                         ListItems.UpdateLayout();
                         RefreshButtonBar();
                         gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                         HideItemPannel();
                         break;
                    case Operation.Show:
                         if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(BillDataControl.Bill, out ErrMsg))
                         {
                             MsgManager.ShowMessage(ErrMsg);
                         }
                         break;
                }
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, a l'editar la factura '{0}'.\r\nDetalls: {1}",
                                                     string.Format("'{0}' de l'any '{1}'", NewOrEditedBill.Bill_Id, NewOrEditedBill.Year), 
                                                     MsgManager.ExcepMsg(ex)));
            }
        }

        /// <summary>
        /// Manage the event produced when the operation in that was doing in the BillDataControl was Cancelled.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BillDataControl_evCancel()
        {
            switch (BillDataControl.CtrlOperation)
            {
                case Operation.Edit:
                     MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                     if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(BillDataControl.Bill, out string ErrMsg))
                     {
                         MsgManager.ShowMessage(ErrMsg);
                     }
                     break;
                case Operation.Show:
                     if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(BillDataControl.Bill, out ErrMsg))
                     {
                         MsgManager.ShowMessage(ErrMsg);
                     }
                     break;
                default:
                     break;
            }
            HideItemPannel();
        }

        #endregion

        #region Print Bill Report

        /// <summary>
        /// Manage the creation of the Report of selected Item and Print this Item.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnPrintBill_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItems != null)
            {
                try
                {
                    ActualizeBillsFromDb();
                    StringBuilder sbInfoExecution = new StringBuilder(string.Empty);
                    foreach (BillsView bill in ListItems.SelectedItems)
                    {
                        if (CreateReport(bill, out string PDF_FileName, 
                                         out Dictionary<int, string> Receipts_PDF_Name, out string ErrMsg))
                        {
                            if (ReportView.PrintReport(PDF_FileName, string.Format("Factura '{0}'", bill.Bill_Id), out ErrMsg))
                            {
                                if (BillsReportView.UpdateBillFlag(bill, BillFlag.Print, true, out ErrMsg))
                                {
                                    bill.Print = true;
                                    sbInfoExecution.AppendFormat("Informe de la Factura '{0}' enviat a impressió correctament.\r\n", bill.Bill_Id);
                                    continue;
                                }
                            }
                        }
                        sbInfoExecution.AppendLine(ErrMsg);
                    }
                    MsgManager.ShowMessage(sbInfoExecution.ToString(), MsgType.Information);
                    ListItems.UnselectAll();
                    DataList = GlobalViewModel.Instance.HispaniaViewModel.Bills;
                    ListItems.UpdateLayout();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, a l'imprimir una o més factures.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
        }

        #endregion

        #region Send Bill By Email Report

        /// <summary>
        /// Manage the creation of the Report of Items, if it's needed, and Send by EMail the Report.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnSendBillByEMail_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItems != null)
            {
                try
                {
                    ActualizeBillsFromDb();
                    StringBuilder sbInfoExecution = new StringBuilder(string.Empty);
                    foreach (BillsView bill in ListItems.SelectedItems)
                    {
                        if (CreateReport(bill, out string PDF_FileName,
                                         out Dictionary<int, string> Receipts_PDF_Name, out string ErrMsg))
                        {
                            Process.Start(PDF_FileName);
                            if (BillsReportView.SendByEMail(bill, PDF_FileName, out ErrMsg))
                            {
                                if (BillsReportView.UpdateBillFlag(bill, BillFlag.SendByEMail, true, out ErrMsg))
                                {
                                    bill.SendByEMail = true;
                                    sbInfoExecution.AppendFormat("Creat correctament l'email asociat a l'informe de la factura '{0}'.\r\n", bill.Bill_Id);
                                    continue;
                                }
                            }
                        }
                        sbInfoExecution.AppendLine(ErrMsg);
                    }
                    MsgManager.ShowMessage(sbInfoExecution.ToString(), MsgType.Information);
                    ListItems.UnselectAll();
                    DataList = GlobalViewModel.Instance.HispaniaViewModel.Bills;
                    ListItems.UpdateLayout();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, a l'enviar per e-mail una o més factures.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
        }

        #endregion

        #region Print Receipt Report

        /// <summary>
        /// Manage the creation of the Report of selected Item and Print this Item.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnPrintReceipt_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItems != null)
            {
                try
                {
                    ActualizeBillsFromDb();
                    foreach (BillsView bill in ListItems.SelectedItems)
                    {
                        if (GetLockReceipts(bill))
                        {
                            if (PrintReceiptsWindow is null)
                            {
                                PrintReceiptsWindow = new ReceiptsSelection(AppType)
                                {
                                    DataList = bill.Receipts,
                                    SelectionModeList = SelectionMode.Extended,
                                    Bill = bill,
                                };
                                PrintReceiptsWindow.Closed += PrintReceiptsWindow_Closed;
                                PrintReceiptsWindow.ShowDialog();
                            }
                            else PrintReceiptsWindow.Activate();
                        }
                    }
                    ListItems.UnselectAll();
                    DataList = GlobalViewModel.Instance.HispaniaViewModel.Bills;
                    ListItems.UpdateLayout();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, a l'imprimir els rebuts d'una o més factures.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
        }
        
        private void PrintReceiptsWindow_Closed(object sender, EventArgs e)
        {
            //  Variables used in the validation.
                string ErrMsg;
                StringBuilder sbInfoExecution = new StringBuilder();
                BillsView SelectedBill = (BillsView)ListItems.SelectedItem;
                List<ReceiptsView> ReceiptsSelecteds = PrintReceiptsWindow.ReceiptsSelecteds;
            //  Step 1 : Print selected Receipts of the selected Bill.
                if ((PrintReceiptsWindow.Result == SelectionResult.Accept) && (ReceiptsSelecteds != null) && 
                    (ReceiptsSelecteds.Count > 0))
                {
                    if (CreateReport(SelectedBill, out string PDF_FileName, out Dictionary<int, string> Receipts_PDF_Name, out ErrMsg))
                    {
                        foreach (ReceiptsView SelectedReceipt in ReceiptsSelecteds)
                        {
                            if (BillsReportView.GetFileNameInReceipt(SelectedReceipt, out PDF_FileName, out ErrMsg))
                            {
                                if (!string.IsNullOrEmpty(PDF_FileName))
                                {
                                    if (ReportView.PrintReport(PDF_FileName,
                                                               string.Format("Rebut '{0}' de la Factura '{1}'",
                                                                             SelectedReceipt.Receipt_Id, SelectedReceipt.Bill_Id),
                                                               out ErrMsg))
                                    {
                                        if (BillsReportView.UpdateReceiptFlag(SelectedReceipt, ReceiptFlag.Print, true, out ErrMsg))
                                        {
                                            SelectedReceipt.Print = true;
                                            sbInfoExecution.AppendFormat("Informe del rebut '{0}' de la factura '{1}' enviat a impressió correctament.\r\n",
                                                                         SelectedReceipt.Receipt_Id, SelectedReceipt.Bill_Id);
                                            continue;
                                        }
                                    }
                                }
                                else
                                {
                                    ErrMsg = string.Format("Error, no s'ha trobat el fitxer PDF associat al rebut {0} de la factura número {1}.",
                                                           SelectedReceipt.Receipt_Id, SelectedReceipt.Bill_Id);
                                }
                            }
                            sbInfoExecution.AppendLine(ErrMsg);
                        }
                    }
                    else sbInfoExecution.AppendLine(ErrMsg);
                    MsgManager.ShowMessage(sbInfoExecution.ToString(), MsgType.Information);
                }
            //  Step 2 : Unlock registers.
                foreach (ReceiptsView Receipt in PrintReceiptsWindow.DataList)
                {
                    if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(Receipt, out ErrMsg))
                    {
                        MsgManager.ShowMessage(ErrMsg);
                        break;
                    }
                }
                if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(SelectedBill, out ErrMsg))
                {
                    MsgManager.ShowMessage(ErrMsg);
                }
            //  Step 3 : Remove the close event.
                PrintReceiptsWindow.Closed -= PrintReceiptsWindow_Closed;
                PrintReceiptsWindow = null;
        }

        #endregion

        #region Send Receipt By Email Report

        /// <summary>
        /// Manage the creation of the Report of Items, if it's needed, and Send by EMail the Report.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnSendReceiptByEMail_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItems != null)
            {
                try
                {
                    ActualizeBillsFromDb();
                    foreach (BillsView bill in ListItems.SelectedItems)
                    {
                        if (GetLockReceipts(bill))
                        {
                            if (PrintReceiptsWindow is null)
                            {
                                SendByEMailReceiptsWindow = new ReceiptsSelection(AppType)
                                {
                                    DataList = bill.Receipts,
                                    SelectionModeList = SelectionMode.Extended,
                                    Bill = bill,
                                };
                                SendByEMailReceiptsWindow.Closed += SendByEMailReceiptsWindow_Closed;
                                SendByEMailReceiptsWindow.ShowDialog();
                            }
                            else SendByEMailReceiptsWindow.Activate();
                        }
                    }
                    ListItems.UnselectAll();
                    DataList = GlobalViewModel.Instance.HispaniaViewModel.Bills;
                    ListItems.UpdateLayout();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, a l'enviar per e-mail els rebuts d'una o més factures.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
        }
        
        private void SendByEMailReceiptsWindow_Closed(object sender, EventArgs e)
        {
            //  Variables used in the validation.
                string ErrMsg;
                StringBuilder sbInfoExecution = new StringBuilder();
                BillsView SelectedBill = (BillsView)ListItems.SelectedItem;
                List<ReceiptsView> ReceiptsSelecteds = SendByEMailReceiptsWindow.ReceiptsSelecteds;
            //  Step 1 : Print selected Receipts of the selected Bill.
                if ((SendByEMailReceiptsWindow.Result == SelectionResult.Accept) && (ReceiptsSelecteds != null) &&
                    (ReceiptsSelecteds.Count > 0))
                {
                    if (CreateReport(SelectedBill, out string PDF_FileName, out Dictionary<int, string> Receipts_PDF_Name, out ErrMsg, false))
                    {
                        foreach (ReceiptsView SelectedReceipt in ReceiptsSelecteds)
                        {
                            if (Receipts_PDF_Name.ContainsKey(SelectedReceipt.Receipt_Id))
                            {
                                SelectedReceipt.FileNamePDF = Receipts_PDF_Name[SelectedReceipt.Receipt_Id];
                                Process.Start(SelectedReceipt.FileNamePDF);
                                if (BillsReportView.SendByEMail(SelectedBill, ReceiptsSelecteds, out ErrMsg))
                                {
                                    if (BillsReportView.UpdateReceiptFlag(SelectedReceipt, ReceiptFlag.SendByEMail, true, out ErrMsg))
                                    {
                                        SelectedReceipt.SendByEMail = true;
                                        sbInfoExecution.AppendFormat("Creat correctament l'email asociat a l'informe de la factura '{0}'.\r\n", SelectedBill.Bill_Id);
                                        continue;
                                    }
                                }
                            }
                            else
                            {
                                ErrMsg = string.Format("Error, no s'ha pogut crear el missatge a enviar.\r\nDetalls: " +
                                                       "No s'ha trobat el fitxer associat al rebut '{0}'.", SelectedReceipt.Receipt_Id_Str);
                            }
                            sbInfoExecution.AppendLine(ErrMsg);
                        }
                    }
                    else sbInfoExecution.AppendLine(ErrMsg);
                    MsgManager.ShowMessage(sbInfoExecution.ToString(), MsgType.Information);
                }
            //  Step 2 : Unlock registers.
                foreach (ReceiptsView Receipt in SendByEMailReceiptsWindow.DataList)
                {
                    if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(Receipt, out ErrMsg))
                    {
                        MsgManager.ShowMessage(ErrMsg);
                        break;
                    }
                }
                if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(SelectedBill, out ErrMsg))
                {
                    MsgManager.ShowMessage(ErrMsg);
                }
            //  Step 3 : Remove the close event.
                SendByEMailReceiptsWindow.Closed -= SendByEMailReceiptsWindow_Closed;
                SendByEMailReceiptsWindow = null;
        }

        #endregion

        #region Common function for Print and Send by E-Mail Bill and Receipts

        private bool CreateReport(BillsView bill, out string PDF_FileName, out Dictionary<int, string> Receipts_PDF_Name, out string ErrMsg, 
                                  bool DuplicateReceipt = true)
        {
            bool Res = false;
            Receipts_PDF_Name = new Dictionary<int, string>();
            if (BillsReportView.CheckAndContinueIfExistReport(bill, out PDF_FileName, out ErrMsg))
            {
                if (BillsReportView.CreateReport(bill, PDF_FileName, out Receipts_PDF_Name, out ErrMsg, DuplicateReceipt))
                {
                    Res = BillsReportView.UpdateFileNameInBill(bill, PDF_FileName, Receipts_PDF_Name, out ErrMsg);
                }
            }
            return Res;
        }

        private bool GetLockReceipts(BillsView SelectedBill)
        {
            string ErrMsg;
            bool LockedOk = true;
            ObservableCollection<ReceiptsView> ReceiptsLockeds = new ObservableCollection<ReceiptsView>();
            foreach (ReceiptsView receipt in SelectedBill.Receipts)
            {
                if (!GlobalViewModel.Instance.HispaniaViewModel.LockRegister(receipt, out ErrMsg))
                {
                    MsgManager.ShowMessage(ErrMsg);
                    LockedOk = false;
                    break;

                }
                else ReceiptsLockeds.Add(new ReceiptsView(receipt));
            }
            if ((LockedOk) && (ReceiptsLockeds.Count > 0))
            {
                if (!GlobalViewModel.Instance.HispaniaViewModel.LockRegister(SelectedBill, out ErrMsg))
                {
                    MsgManager.ShowMessage(ErrMsg);
                    LockedOk = false;
                }
            }
            else
            {
                MsgManager.ShowMessage("Avís, no hi ha rebuts per aquesta factura.", MsgType.Warning);
                LockedOk = false;
            }
            if (!LockedOk)
            {
                foreach (ReceiptsView receipt in ReceiptsLockeds)
                {
                    if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(receipt, out ErrMsg))
                    {
                        MsgManager.ShowMessage(ErrMsg);
                        break;
                    }
                }
            }
            return LockedOk;
        }

        #endregion

        #region Add Delivery Note at a Bill

        private void BtnAddDeliveryNote_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItem != null)
            {
                try
                {
                    ActualizeBillFromDb();
                    BillsView SelectedBill = (BillsView)ListItems.SelectedItem;
                    if (SelectedBill.CanModifyBill)
                    {
                        if (GetLockCustomerOrdersForAddDeliveryNote(SelectedBill, out ObservableCollection<CustomerOrdersView> CustomerOrdersLockeds))
                        {
                            if (AddCustomerOrdersWindow is null)
                            {
                                AddCustomerOrdersWindow = new CustomerOrdersSelection(AppType, CustomerOrderSelectionOperation.Add)
                                {
                                    DataList = CustomerOrdersLockeds,
                                    Bill = SelectedBill,
                                };
                                AddCustomerOrdersWindow.Closed += AddCustomerOrdersWindow_Closed;
                                AddCustomerOrdersWindow.ShowDialog();
                            }
                            else AddCustomerOrdersWindow.Activate();
                        }
                    }
                    else MsgManager.ShowMessage("Avís, no es pot modificar la factura degut a que ja s'ha cobrat com a mínim un dels rebuts.", MsgType.Warning);
                }
                catch (Exception ex)
                {
                    BillsView bill = (BillsView)ListItems.SelectedItem;
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'afegir un albarà a la factura '{0}'.\r\nDetalls: {1}",
                                     string.Format("'{0}' de l'any '{1}'", bill.Bill_Id, bill.Year), MsgManager.ExcepMsg(ex)));
                }
            }
        }

        private void AddCustomerOrdersWindow_Closed(object sender, EventArgs e)
        {
            //  Variables used in the validation.
                bool ExecuteCommand = (AddCustomerOrdersWindow.Result == SelectionResult.Accept);
                StringBuilder sbInfo = new StringBuilder();
                BillsView SelectedBill = (BillsView)ListItems.SelectedItem;
            //  Step 1 : Build the Question for give permission at the user.
                if (ExecuteCommand)
                {
                    foreach (CustomerOrdersView customerOrder in AddCustomerOrdersWindow.CustomerOrdersSelecteds)
                    {
                        sbInfo.AppendFormat("{0}, ", customerOrder.DeliveryNote_Id);
                    }
                    if (sbInfo.Length > 0) sbInfo.Remove(sbInfo.Length - 2, 2);
                    else ExecuteCommand = false;
                }
            //  Step 2 : Execute Command if its needed
                if (ExecuteCommand)
                {
                    string Question = string.Format("Està segur que vol afegir els albarans ({0}) seleccionats de la factura '{1}' ?", 
                                                    sbInfo.ToString(), SelectedBill.Bill_Id);
                    if (MsgManager.ShowQuestion(Question) == MessageBoxResult.Yes)
                    {
                        List<CustomerOrdersView> CustomerOrdersList = new List<CustomerOrdersView>(AddCustomerOrdersWindow.CustomerOrdersSelecteds);
                        CustomersView Customer = CustomerOrdersList[0].Customer;
                        GlobalViewModel.Instance.HispaniaViewModel.AddCustomerOrdersFromBill(SelectedBill, Customer, CustomerOrdersList);
                        DataChangedManagerActive = false;
                        if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                        DataList.Remove(SelectedBill);
                        BillsView NewBill = GlobalViewModel.Instance.HispaniaViewModel.GetBillFromDb(SelectedBill);
                        DataList.Add(NewBill);
                        DataChangedManagerActive = true;
                        ListItems.SelectedItem = NewBill;
                        ListItems.UpdateLayout();
                        RefreshButtonBar();
                    }
                    else MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                }
            //  Step 3 : Unlock registers.
                string ErrMsg;
                foreach (CustomerOrdersView CustomerOrder in AddCustomerOrdersWindow.DataList)
                {
                    if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(CustomerOrder, out ErrMsg))
                    {
                        MsgManager.ShowMessage(ErrMsg);
                        ExecuteCommand = false;
                        break;
                    }
                }
                if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(SelectedBill, out ErrMsg))
                {
                    MsgManager.ShowMessage(ErrMsg);
                }
            //  Step 4 : Remove the close event.
                AddCustomerOrdersWindow.Closed -= AddCustomerOrdersWindow_Closed;
                AddCustomerOrdersWindow = null;
        }

        private bool GetLockCustomerOrdersForAddDeliveryNote(BillsView SelectedBill,
                                                             out ObservableCollection<CustomerOrdersView> CustomerOrdersLockeds)
        {
            string ErrMsg;
            bool LockedOk = true;
            int Customer_Id = SelectedBill.Customer.Customer_Id;
            CustomerOrdersLockeds = new ObservableCollection<CustomerOrdersView>();
            foreach (CustomerOrdersView customerOrder in GlobalViewModel.Instance.HispaniaViewModel.GetCustomerOrdersFilteredByCustormerId(Customer_Id))
            {
                if ((!(customerOrder.Customer is null)) && (customerOrder.Customer.Customer_Id == SelectedBill.Customer.Customer_Id) &&
                    (!customerOrder.HasBill) && (customerOrder.HasDeliveryNote))
                {
                    if (!GlobalViewModel.Instance.HispaniaViewModel.LockRegister(customerOrder, out ErrMsg))
                    {
                        MsgManager.ShowMessage(ErrMsg);
                        LockedOk = false;
                        break;

                    }
                    else CustomerOrdersLockeds.Add(new CustomerOrdersView(customerOrder));
                }
            }
            if ((LockedOk) && (CustomerOrdersLockeds.Count > 0))
            {
                if (!GlobalViewModel.Instance.HispaniaViewModel.LockRegister(SelectedBill, out ErrMsg))
                {
                    MsgManager.ShowMessage(ErrMsg);
                    LockedOk = false;
                }
            }
            else
            {
                MsgManager.ShowMessage("Avís, no hi ha albarans d'aquest client pendents d'assignar.", MsgType.Warning);
                LockedOk = false;
            }
            if (!LockedOk)
            {
                foreach (CustomerOrdersView CustomerOrder in CustomerOrdersLockeds)
                {
                    if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(CustomerOrder, out ErrMsg))
                    {
                        MsgManager.ShowMessage(ErrMsg);
                        break;
                    }
                }
            }
            return LockedOk;
        }

        #endregion

        #region Remove Delivery Note at a Bill

        private void BtnRemoveDeliveryNote_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItem != null)
            {
                try
                {
                    ActualizeBillFromDb();
                    BillsView SelectedBill = (BillsView)ListItems.SelectedItem;
                    if (SelectedBill.CanModifyBill)
                    {
                        if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(SelectedBill, out string ErrMsg))
                        {
                            if (RemoveCustomerOrdersWindow is null)
                            {
                                RemoveCustomerOrdersWindow = new CustomerOrdersSelection(AppType, CustomerOrderSelectionOperation.Remove)
                                {
                                    DataList = SelectedBill.CustomerOrders,
                                    Bill = SelectedBill,
                                };
                                RemoveCustomerOrdersWindow.Closed += RemoveCustomerOrdersWindow_Closed;
                                RemoveCustomerOrdersWindow.ShowDialog();
                            }
                            else RemoveCustomerOrdersWindow.Activate();
                        }
                        else MsgManager.ShowMessage(ErrMsg);
                    }
                    else MsgManager.ShowMessage("Avís, no es pot modificar la factura degut a que ja s'ha cobrat com a mínim un dels rebuts.", MsgType.Warning);
                }
                catch (Exception ex)
                {
                    BillsView bill = (BillsView)ListItems.SelectedItem;
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'esborrar un albarà a la factura '{0}'.\r\nDetalls: {1}",
                                     string.Format("'{0}' de l'any '{1}'", bill.Bill_Id, bill.Year), MsgManager.ExcepMsg(ex)));
                }
            }
        }

        private void RemoveCustomerOrdersWindow_Closed(object sender, EventArgs e)
        {
            //  Variables used in the validation.
                bool ExecuteCommand = (RemoveCustomerOrdersWindow.Result == SelectionResult.Accept);
                StringBuilder sbInfo = new StringBuilder();
                BillsView SelectedBill = (BillsView)ListItems.SelectedItem;
            //  Step 1 : Validate that the user cannot try to remove all Customer Orders of the Bill
                if (RemoveCustomerOrdersWindow.CustomerOrdersSelecteds.Count == SelectedBill.CustomerOrders.Count)
                {
                    MsgManager.ShowMessage("Avís, no es poden treure tots els albarans d'una factura. Operació cancel·lada",
                                           MsgType.Warning);
                    ExecuteCommand = false;
                }
            //  Step 2 : Build the Question for give permission at the user.
                if (ExecuteCommand)
                {
                    foreach (CustomerOrdersView customerOrder in RemoveCustomerOrdersWindow.CustomerOrdersSelecteds)
                    {
                        sbInfo.AppendFormat("{0}, ", customerOrder.DeliveryNote_Id);
                    }
                    if (sbInfo.Length > 0) sbInfo.Remove(sbInfo.Length - 2, 2);
                    else ExecuteCommand = false;
                }
            //  Step 3 : Execute Command if its needed
                if (ExecuteCommand)
                {
                    string Question = string.Format("Està segur que vol treure els albarans ({0}) seleccionats de la factura '{1}' ?", 
                                                    sbInfo.ToString(), SelectedBill.Bill_Id);
                    if (MsgManager.ShowQuestion(Question) == MessageBoxResult.Yes)
                    {
                        List<CustomerOrdersView> CustomerOrdersList = new List<CustomerOrdersView>(RemoveCustomerOrdersWindow.CustomerOrdersSelecteds);
                        CustomersView Customer = CustomerOrdersList[0].Customer;
                        GlobalViewModel.Instance.HispaniaViewModel.RemoveCustomerOrdersFromBill(SelectedBill, Customer, CustomerOrdersList);
                        DataChangedManagerActive = false;
                        if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                        DataList.Remove(SelectedBill);
                        BillsView NewBill = GlobalViewModel.Instance.HispaniaViewModel.GetBillFromDb(SelectedBill);
                        DataList.Add(NewBill);
                        DataChangedManagerActive = true;
                        ListItems.SelectedItem = NewBill;
                        ListItems.UpdateLayout();
                        RefreshButtonBar();
                    }
                    else MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                }
            //  Step 4 : Unlock bill register.
                if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(SelectedBill, out string ErrMsg))
                {
                    MsgManager.ShowMessage(ErrMsg);
                }
            //  Step 5 : Remove the close event.
                RemoveCustomerOrdersWindow.Closed -= RemoveCustomerOrdersWindow_Closed;
                RemoveCustomerOrdersWindow = null;
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
                ActualizeBillsRefreshFromDb();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al refrescar els valors de les Factures.", MsgManager.ExcepMsg(ex)));
            }
        }

        #endregion

        #region Select

        /// <summary>
        /// Manage the button for Select.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (Selection)
            {
                SelectedBill = ListItems.SelectedItem is null ? null : (BillsView)ListItems.SelectedItem;
            }
            Close();
        }

        #endregion

        #region Select Cancel

        /// <summary>
        /// Manage the button for Select Cancel.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnSelectCancel_Click(object sender, RoutedEventArgs e)
        {
            MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
            Close();
        }

        #endregion

        #endregion

        #region ListItems

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
                if (!Selection) RefreshButtonBar();
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region Window

        private void Bills_Closed(object sender, EventArgs e)
        {
            if (BillDataControl.CtrlOperation == Operation.Edit)
            {
                if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(BillDataControl.Bill, out string ErrMsg))
                {
                    MsgManager.ShowMessage(ErrMsg);
                }
            }
        }

        #endregion

        #endregion
                
        #region Database Operations
		        
        private void ActualizeBillsRefreshFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.BillsWindow);
                EffectTypes = GlobalViewModel.Instance.HispaniaViewModel.EffectTypesDict;
                BillingSeries = GlobalViewModel.Instance.HispaniaViewModel.BillingSeriesDict;
                Parameters = GlobalViewModel.Instance.HispaniaViewModel.Parameters;
                DataList = GlobalViewModel.Instance.HispaniaViewModel.Bills;
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        private void ActualizeBillFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                BillsView SelectedItem = (BillsView)ListItems.SelectedItem;
                BillsView ItemInDb = GlobalViewModel.Instance.HispaniaViewModel.GetBillFromDb(SelectedItem);
                int Index = ListItems.SelectedIndex;
                ListItems.UnselectAll();
                DataList.Remove(SelectedItem);
                DataList.Insert(Index, ItemInDb);
                ListItems.SelectedItem = ItemInDb;
                BillDataControl.Bill = ItemInDb;
                ListItems.UpdateLayout();
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        private void ActualizeBillsFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                List<BillsView> ItemsInDb = new List<BillsView>(ListItems.SelectedItems.Count);
                foreach (BillsView customerOrder in new ArrayList(ListItems.SelectedItems))
                {
                    BillsView customerInDb = GlobalViewModel.Instance.HispaniaViewModel.GetBillFromDb(customerOrder);
                    ItemsInDb.Add(customerInDb);
                    int Index = DataList.IndexOf(customerOrder);
                    ListItems.SelectedItems.Remove(customerOrder);
                    DataList.Remove(customerOrder);
                    DataList.Insert(Index, customerInDb);
                    ListItems.SelectedItems.Add(customerInDb);
                }
                ListItems.UpdateLayout();
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        #endregion

        #region Update IU

        /// <summary>
        /// Method that show the Item Pannel
        /// </summary>
        private void ShowItemPannel()
        {
            gsSplitter.IsEnabled = true;
            rdItemPannel.Height = ViewItemPannel;
            rdOperationPannel.Height = HideComponent;
            RefreshButtonBar(true);
            GridList.IsEnabled = false;
        }

        /// <summary>
        /// Method that show the Item Pannel
        /// </summary>
        private void HideItemPannel()
        {
            rdItemPannel.Height = HideComponent;
            gsSplitter.IsEnabled = false;
            rdOperationPannel.Height = ViewOperationPannel;
            RefreshButtonBar();
            GridList.IsEnabled = true;
        }

        /// <summary>
        /// Refresh Button Bar with nowadays state of the formulary.
        /// </summary>
        /// <param name="IsEditing"></param>
        private void RefreshButtonBar(bool IsEditing = false)
        {
            rdSearchPannel.Height = DataList.Count > 0 ? ViewSearchPannel : HideComponent;
            if ((Selection) && (SelectedBill != null))
            {
                ListItems.SelectedItem = SelectedBill;
            }
            bool HasItemSelected = !(ListItems.SelectedItem is null);
            cdPrintBill.Width = Print && HasItemSelected && !Selection ? ViewPrintBillButtonPannel : HideComponent;
            cdSendBillByEMail.Width = Print && HasItemSelected && !Selection ? ViewSendByEMailButtonPannel : HideComponent;
            cdPrintReceipt.Width = Print && HasItemSelected && !Selection ? ViewPrintReceiptButtonPannel : HideComponent;
            cdSendReceiptByEMail.Width = Print && HasItemSelected && !Selection ? ViewSendByEMailButtonPannel : HideComponent;
            bool CanEditOrDelete = !Print && HasItemSelected && AreViewingNowYear && !IsEditing;
            cdEdit.Width = CanEditOrDelete && !Selection ? ViewEditButtonPannel : HideComponent;
            cdView.Width = HasItemSelected && !Selection ? ViewShowButtonPannel : HideComponent;
            cdAddDeliveryNote.Width = CanEditOrDelete && !Selection ? ViewAddDeliveryNoteButtonPannel : HideComponent;
            cdRemoveDeliveryNote.Width = CanEditOrDelete && !Selection ? ViewRemoveDeliveryNoteButtonPannel : HideComponent;
            cdSelect.Width = Selection ? ViewSelectButtonPannel : HideComponent;
            cdSelectCancel.Width = Selection ? ViewSelectCancelButtonPannel : HideComponent;
        }

        #endregion
    }
}
