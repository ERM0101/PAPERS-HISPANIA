#region Librerias usadas por la ventana

using HispaniaCommon.ViewClientWPF.UserControls;
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

namespace HispaniaCommon.ViewClientWPF.Windows
{
    /// <summary>
    /// Interaction logic for CustomerOrders.xaml
    /// </summary>
    public partial class MismatchesReceipts : Window
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
        /// Show the Operation Pannel.
        /// </summary>
        private GridLength ViewSolveMismatchButtonPannel = new GridLength(200.0);

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
                //  Deactivate managers
                    DataChangedManagerActive = false;
                //  Actualize the value
                    if (value != null) m_DataList = value;
                    else m_DataList = new ObservableCollection<BillsView>();
                //  Set up controls state
                    ListItems.ItemsSource = m_DataList;
                    ListItems.DataContext = this;
                    CollectionViewSource.GetDefaultView(ListItems.ItemsSource).SortDescriptions.Add(new SortDescription("Bill_Id", ListSortDirection.Descending));
                    CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Filter = UserFilter;
                    lblCountMismatchesReceipts.Content = ((CollectionView)CollectionViewSource.GetDefaultView(ListItems.ItemsSource)).Count.ToString();
                    ListItems.UnselectAll();
                //  Reactivate managers
                    DataChangedManagerActive = true;
                //  Refresh Button Bar
                    RefreshButtonBar();
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
                MismatchesReceiptsDataControl.Parameters = value;
            }
        }

        /// <summary>
        /// Get or Set the Effect Types
        /// </summary>
        public Dictionary<string, EffectTypesView> EffectTypes
        {
            set
            {
                MismatchesReceiptsDataControl.EffectTypes = value;
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

        #region Buiders

        /// <summary>
        /// Main Builder of the windows.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        public MismatchesReceipts(ApplicationType AppType)
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
                MismatchesReceiptsDataControl.AppType = AppType;
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
                ListItems.ItemsSource = m_DataList;
                DataContext = DataList;
                cbFieldItemToSearch.ItemsSource = BillsView.Fields;
                cbFieldItemToSearch.DisplayMemberPath = "Key";
                cbFieldItemToSearch.SelectedValuePath = "Value";
                if (CustomerOrdersView.Fields.Count > 0) cbFieldItemToSearch.SelectedIndex = 0;
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
            //  Get Acces to the object and the property name To Filter.
                BillsView ItemData = (BillsView)item;
                String ProperyName = (string) cbFieldItemToSearch.SelectedValue;
            //  Apply the filter by selected field value
                object valueBillAmountToTest = ItemData.GetType().GetProperty("BillAmount").GetValue(ItemData);
                object valueAmountReceiptsToTest = ItemData.GetType().GetProperty("AmountReceipts").GetValue(ItemData);
                if ((valueBillAmountToTest is null) || (valueAmountReceiptsToTest is null) ||
                    (((decimal)valueBillAmountToTest) == ((decimal)valueAmountReceiptsToTest)))
                {
                    return (false);
                }
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
                Closed += MismatchesReceipts_Closed;
            //  TextBox
                tbItemToSearch.TextChanged += TbItemToSearch_TextChanged;
            //  Button
                btnSolveMismatch.Click += BtnSolveMismatch_Click;
                btnRefresh.Click += BtnRefresh_Click;
            //  Define ListView events to manage.
                ListItems.SelectionChanged += ListItems_SelectionChanged;
            //  Define BillDataControl events to manage.
                MismatchesReceiptsDataControl.EvAccept += MismatchesReceiptsDataControl_evAccept;
                MismatchesReceiptsDataControl.EvCancel += MismatchesReceiptsDataControl_evCancel;
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
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region Button
									
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
                ActualizeMismatcheReceiptsFromDb();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al refrescar els valors dels Descquadres de Rebuts.", MsgManager.ExcepMsg(ex)));
            }
        }

        #endregion

        #region Editing Mismatched Receipts

        private void BtnSolveMismatch_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItem != null)
            {
                try
                {
                    ActualizeBillFromDb();
                    BillsView billsView = (BillsView)ListItems.SelectedItem;
                    if (billsView.CanModifyBill)
                    {
                        if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(billsView, out string ErrMsg))
                        {
                            MismatchesReceiptsDataControl.CtrlOperation = Operation.Edit;
                            gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "EditableGroupBox");
                            ShowItemPannel();
                        }
                        else MsgManager.ShowMessage(ErrMsg);
                    }
                    else MsgManager.ShowMessage("Avís, no es pot modificar el venciment de la factura degut a que " +
                                                "ja s'ha cobrat com a mínim un dels rebuts.", MsgType.Warning);
                }
                catch (Exception ex)
                {
                    BillsView bill = (BillsView)ListItems.SelectedItem;
                    MsgManager.ShowMessage(
                       string.Format("Error, al solucionar el problema amb el desquadre de veciment de la factura '{0}' de l'any '{1}'.\r\nDetalls: {2}",
                                     bill.Bill_Id, bill.Year, MsgManager.ExcepMsg(ex)));
                }
            }
        }

        /// <summary>
        /// Manage the event produced when the operation in that was doing in the BillDataControl was Accepted.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void MismatchesReceiptsDataControl_evAccept(BillsView NewOrEditedBill)
        {
            try
            {
                BillsView NewBill = new BillsView(NewOrEditedBill);
                BillsView SelectedBill = (BillsView) ListItems.SelectedItem;
                switch (MismatchesReceiptsDataControl.CtrlOperation)
                {
                    case Operation.Edit:
                         GlobalViewModel.Instance.HispaniaViewModel.UpdateBill(NewBill, true);
                         if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(SelectedBill, out string ErrMsg))
                         {
                             MsgManager.ShowMessage(ErrMsg);
                         }
                         rdItemPannel.IsEnabled = false;
                         DataChangedManagerActive = false;
                         ListItems.UnselectAll();
                         DataList.Remove(SelectedBill);
                         DataChangedManagerActive = true;
                         ListItems.SelectedItem = NewBill;
                         ListItems.UpdateLayout();
                         RefreshButtonBar();
                         gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                         HideItemPannel();
                         break;
                }
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(
                   string.Format("Error, al solucionar el problema amb el desquadre de veciment de la factura '{0}' de l'any '{1}'.\r\nDetalls: {2}",
                                 NewOrEditedBill.Bill_Id, NewOrEditedBill.Year, MsgManager.ExcepMsg(ex)));
            }
        }

        /// <summary>
        /// Manage the event produced when the operation in that was doing in the BillDataControl was Cancelled.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void MismatchesReceiptsDataControl_evCancel()
        {
            MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
            if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(MismatchesReceiptsDataControl.Bill,
                                                                           out string ErrMsg))
            {
                MsgManager.ShowMessage(ErrMsg);
            }
            HideItemPannel();
        }

        #endregion

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
                RefreshButtonBar();
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region Window

        private void MismatchesReceipts_Closed(object sender, EventArgs e)
        {
            if (MismatchesReceiptsDataControl.CtrlOperation == Operation.Edit)
            {
                if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(MismatchesReceiptsDataControl.Bill, 
                                                                               out string ErrMsg))
                {
                    MsgManager.ShowMessage(ErrMsg);
                }
            }
        }

        #endregion

        #endregion
                        
        #region Database Operations
		        
        private void ActualizeMismatcheReceiptsFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.MismatchesReceiptsWindow);
                EffectTypes = GlobalViewModel.Instance.HispaniaViewModel.EffectTypesDict;
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
                BillsView ItemInDb = GlobalViewModel.Instance.HispaniaViewModel.GetBillFromDb(SelectedItem, true);
                int Index = ListItems.SelectedIndex;
                ListItems.UnselectAll();
                DataList.Remove(SelectedItem);
                DataList.Insert(Index, ItemInDb);
                ListItems.SelectedItem = ItemInDb;
                MismatchesReceiptsDataControl.Bill = ItemInDb;
                ListItems.UpdateLayout();
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        #endregion

        #region Update

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
            gsSplitter.IsEnabled = false;
            rdItemPannel.Height = HideComponent;
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
            rdSearchPannel.Height = (((CollectionView)CollectionViewSource.GetDefaultView(ListItems.ItemsSource)).Count > 0) ? ViewSearchPannel : HideComponent;
            bool HasItemSelected = !(ListItems.SelectedItem is null);
            cdSolveMismatch.Width = (HasItemSelected) ? ViewSolveMismatchButtonPannel : HideComponent;
        }

        #endregion
    }
}
