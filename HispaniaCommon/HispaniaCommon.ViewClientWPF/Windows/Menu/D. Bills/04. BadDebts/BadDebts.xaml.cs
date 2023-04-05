#region Librerias usadas por la clase

using HispaniaCommon.ViewClientWPF.UserControls;
using HispaniaCommon.ViewModel;
using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.Theme;
using System;
using System.Collections;
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
    /// Interaction logic for BadDebts.xaml
    /// </summary>
    public partial class BadDebts : Window
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
        /// Show the Report button.
        /// </summary>
        private GridLength ViewReportButtonPannel = new GridLength(120.0);

        /// <summary>
        /// Show the Item Panel.
        /// </summary>
        private GridLength ViewItemPanel = new GridLength(550.0);

        /// <summary>
        /// Show the Customer Panel.
        /// </summary>
        private GridLength ViewCustomerPanel = new GridLength(80.0);

        /// <summary>
        /// Show the Search Panel.
        /// </summary>
        private GridLength ViewSearchPanel = new GridLength(30.0);

        /// <summary>
        /// Show the Operation Panel.
        /// </summary>
        private GridLength ViewOperationPanel = new GridLength(30.0);

        /// <summary>
        /// Show the Operation Panel.
        /// </summary>
        private GridLength ViewAddPanel = new GridLength(130.0);

        /// <summary>
        /// Show the Operation Panel.
        /// </summary>
        private GridLength ViewEditPanel = new GridLength(120.0);

        /// <summary>
        /// Show the Operation Panel.
        /// </summary>
        private GridLength ViewDeletePanel = new GridLength(120.0);

        /// <summary>
        /// Show the Operation Panel.
        /// </summary>
        private GridLength ViewViewPanel = new GridLength(130.0);

        /// <summary>
        /// Show the Operation Panel.
        /// </summary>
        private GridLength ViewReportPanel = new GridLength(120.0);

        /// <summary>
        /// Hide the ItemPanel.
        /// </summary>
        private GridLength HideComponent = new GridLength(0.0);

        #endregion

        #region Attributes

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private CustomersView m_Data = null;

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private ProvidersView m_DataProv = null;

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private ObservableCollection<BadDebtsView> m_DataList = new ObservableCollection<BadDebtsView>();

        /// <summary>
        /// Store the value that indicates if the Window allows impression or not.
        /// </summary>
        private bool m_Print = false;

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
                m_Data = value;
                LoadDataInWindow(m_Data);
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
                m_DataProv = value;
                LoadDataInWindow(m_DataProv);
            }
        }

        /// <summary>
        /// Get or Set the data to show in List of Items.
        /// </summary>
        public ObservableCollection<BadDebtsView> DataList
        {
            get
            {
                return (m_DataList);
            }
            set
            {
                //  Actualize the value
                    if (value != null) m_DataList = value;
                    else m_DataList = new ObservableCollection<BadDebtsView>();
                //  Deactivate managers
                    DataChangedManagerActive = false;
                //  Set up controls state
                    ListItems.ItemsSource = m_DataList;
                    ListItems.DataContext = this;
                    CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Filter = UserFilter;
                    CollectionViewSource.GetDefaultView(ListItems.ItemsSource).SortDescriptions.Add(new SortDescription("Receipt_Id", ListSortDirection.Ascending));
                //  Select first item of the list.
                    if (((CollectionView)CollectionViewSource.GetDefaultView(ListItems.ItemsSource)).Count > 0) ListItems.SelectedIndex = 0;
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
                m_Print = value;
                Title = m_Print ? "Impressió de Fitxes d'Impagats" : "Gestió d'Impagats";
                ListItems.SelectionMode = m_Print ? SelectionMode.Extended : SelectionMode.Single;
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
        public BadDebts(ApplicationType AppType)
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
                BadDebtDataControl.AppType = AppType;
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
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Filter = UserFilter;
                cbFieldItemToSearch.ItemsSource = BadDebtsView.Fields;
                cbFieldItemToSearch.DisplayMemberPath = "Key";
                cbFieldItemToSearch.SelectedValuePath = "Value";
                if (BadDebtsView.Fields.Count > 0) cbFieldItemToSearch.SelectedIndex = 0;
            //  Activate managers
                DataChangedManagerActive = true;
        }

        /// <summary>
        /// Load the data of the Company into the Window
        /// </summary>
        private void LoadDataInWindow(CustomersView customerView)
        {
            if (customerView != null)
            {
                tbCustomerCode.Text = GlobalViewModel.GetStringFromIntIdValue(customerView.Customer_Id);
                tbCustomerDescription.Text = customerView.Company_Name;
            }
            rdCustomerPannel.Height = customerView is null ? HideComponent : ViewCustomerPanel;
        }


        /// <summary>
        /// Load the data of the Company into the Window
        /// </summary>
        private void LoadDataInWindow(ProvidersView providerView)
        {
            if (providerView != null)
            {
                tbCustomerCode.Text = GlobalViewModel.GetStringFromIntIdValue(providerView.Provider_Id);
                tbCustomerDescription.Text = providerView.Name;
            }
            rdCustomerPannel.Height = providerView is null ? HideComponent : ViewCustomerPanel;
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
                BadDebtsView ItemData = (BadDebtsView)item;
                String ProperyName = (string) cbFieldItemToSearch.SelectedValue;
            //  Apply the filter
                if (!String.IsNullOrEmpty(tbBadDebtsToSearch.Text))
                {
                    object valueToTest = ItemData.GetType().GetProperty(ProperyName).GetValue(ItemData);
                    if (valueToTest is null) return (false);
                    else
                    {
                        if (valueToTest is DateTime)
                        {
                            return GlobalViewModel.GetDateForUI((DateTime)valueToTest).Contains(tbBadDebtsToSearch.Text);
                        }
                        else return ((valueToTest.ToString().ToUpper()).Contains(tbBadDebtsToSearch.Text.ToUpper()));
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
                tbBadDebtsToSearch.TextChanged += TbItemToSearch_TextChanged;
            //  Button 
                btnAdd.Click += BtnAdd_Click;
                btnEdit.Click += BtnEdit_Click;
                btnDelete.Click += BtnDelete_Click;
                btnViewData.Click += BtnViewData_Click;
                btnReport.Click += BtnReport_Click;
            //  Define ListView events to manage.
                ListItems.SelectionChanged += ListItems_SelectionChanged;
            //  Define BadDebtDataControl events to manage.
                BadDebtDataControl.EvAccept += BadDebtDataControl_evAccept;
                BadDebtDataControl.EvCancel += BadDebtDataControl_evCancel;
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

        #endregion

        #region Button

        #region Editing Bad Debts

        /// <summary>
        /// Manage the Button Mouse Click in one of the items of the List to show its data in User Control.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnViewData_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItem != null)
            {
                try
                {
                    ActualizeBadDebtFromDb();
                    BadDebtDataControl.CtrlOperation = Operation.Show;
                    gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                    ShowItemPannel();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, a l'intentar veure les dades de l'Impagat seleccionat.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
        }

        /// <summary>
        /// Manage the button for add Items in the list.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BadDebtDataControl.CtrlOperation = Operation.Add;
                gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "EditableGroupBox");
                ShowItemPannel();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al afegir un nou Impagat.\r\nDetalls:{0}", MsgManager.ExcepMsg(ex)));
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
                    ActualizeBadDebtFromDb();
                    if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(ListItems.SelectedItem, out string ErrMsg))
                    {
                        BadDebtDataControl.BadDebtData = (BadDebtsView)ListItems.SelectedItem;
                        BadDebtDataControl.CtrlOperation = Operation.Edit;
                        gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "EditableGroupBox");
                        ShowItemPannel();
                    }
                    else MsgManager.ShowMessage(ErrMsg);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, a l'iniciar l'edició de l'Impagat seleccionat.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
        }

        /// <summary>
        /// Manage the button for edit an Item of the list.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItem != null)
            {
                BadDebtsView BadDebtToDelete = (BadDebtsView)ListItems.SelectedItem;
                string Question = string.Format("Està segur que vol esborrar l'Impagat del Rebut '{0}' de la Factura '{1}' ?", BadDebtToDelete.Receipt_Id, BadDebtToDelete.Bill_Id);
                if (MsgManager.ShowQuestion(Question) == MessageBoxResult.Yes)
                {
                    string ErrMsg;
                    try
                    {
                        if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(BadDebtToDelete, out ErrMsg))
                        {
                            GlobalViewModel.Instance.HispaniaViewModel.DeleteBadDebt(DataList[DataList.IndexOf(BadDebtToDelete)]);
                            GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(BadDebtToDelete, out ErrMsg);
                            DataChangedManagerActive = false;
                            if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                            DataList = GlobalViewModel.Instance.HispaniaViewModel.BadDebts;
                            DataChangedManagerActive = true;
                            ListItems.UpdateLayout();
                            HideItemPannel();
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrMsg = string.Format("Error, a l'esborrar l'Impagat del Rebut '{0}' de la Factura '{1}'.\r\nDetalls: {1}", 
                                               BadDebtToDelete.Receipt_Id, BadDebtToDelete.Bill_Id, MsgManager.ExcepMsg(ex));
                    }
                    if (!string.IsNullOrEmpty(ErrMsg)) MsgManager.ShowMessage(ErrMsg);
                }
                else MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
            }
        }

        /// <summary>
        /// Manage the event produced when the operation in that was doing in the BadDebtDataControl was Accepted.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BadDebtDataControl_evAccept(BadDebtsView NewOrEditedBadDebt, List<BadDebtMovementsView> NewOrEditedBadDebtMovements)
        {
            try
            {
                BadDebtsView OldBadDebt = (BadDebtsView)ListItems.SelectedItem;
                BadDebtsView NewBadDebt = new BadDebtsView(NewOrEditedBadDebt);
                switch (BadDebtDataControl.CtrlOperation)
                {
                    case Operation.Add:
                         GlobalViewModel.Instance.HispaniaViewModel.CreateBadDebt(NewBadDebt, NewOrEditedBadDebtMovements);
                         DataChangedManagerActive = false;
                         if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                         DataList = GlobalViewModel.Instance.HispaniaViewModel.BadDebts;
                         DataChangedManagerActive = true;
                         ListItems.SelectedItem = NewBadDebt;
                         ListItems.UpdateLayout();
                         gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                         HideItemPannel();
                         break;
                    case Operation.Edit:
                         GlobalViewModel.Instance.HispaniaViewModel.UpdateBadDebt(OldBadDebt, NewBadDebt, NewOrEditedBadDebtMovements);
                         if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(BadDebtDataControl.BadDebtData, out string ErrMsg))
                         {
                             MsgManager.ShowMessage(ErrMsg);
                         }
                         DataChangedManagerActive = false;
                         if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                         DataList = GlobalViewModel.Instance.HispaniaViewModel.BadDebts;
                         DataChangedManagerActive = true;
                         ListItems.SelectedItem = NewBadDebt;
                         ListItems.UpdateLayout();
                         gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                         HideItemPannel();
                         break;
                }
            }
            catch (Exception ex)
            {
                string OperationInfo = ". Operació no reconeguda.";
                switch (BadDebtDataControl.CtrlOperation)
                {
                    case Operation.Add:
                         OperationInfo = " que s'intenta afegir.";
                         break;
                    case Operation.Edit:
                         OperationInfo = " que s'està editant.";
                         break;
                }
                MsgManager.ShowMessage(string.Format("Error, al guardar les dades de l'Impagat{0}\r\nDetalls: {1}", OperationInfo, MsgManager.ExcepMsg(ex)));
            }
        }

        /// <summary>
        /// Manage the event produced when the operation in that was doing in the BadDebtDataControl was Cancelled.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BadDebtDataControl_evCancel()
        {
            switch (BadDebtDataControl.CtrlOperation)
            {
                case Operation.Add:
                     MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                     break;
                case Operation.Edit:
                     MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                     if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(BadDebtDataControl.BadDebtData, out string ErrMsg))
                     {
                         MsgManager.ShowMessage(ErrMsg);
                     }
                     break;
                case Operation.Show:
                     break;
                default:
                     break;
            }
            HideItemPannel();
        }

        #endregion

        #region Reports

        /// <summary>
        /// Manage the creation of the Report of Items in the list.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItems != null)
            {
                try
                {
                    ActualizeBadDebtsFromDb(out List<BadDebtsView> ItemsInDb);
                    BadDebtsReportView.CreateReport(ItemsInDb);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, al crear l'informe dels Impagats.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
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
                ActualizeBadDebtsFromDb();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al refrescar els valors dels Impagats.", MsgManager.ExcepMsg(ex)));
            }
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
                RefreshButtonBar();
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region Window

        /// <summary>
        /// Manage the closing of the window.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void Bills_Closed(object sender, EventArgs e)
        {
            if (BadDebtDataControl.CtrlOperation == Operation.Edit)
            {
                if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(BadDebtDataControl.BadDebtData, out string ErrMsg))
                {
                    MsgManager.ShowMessage(ErrMsg);
                }
            }
        }

        #endregion

        #endregion

        #region Database Operations

        private void ActualizeBadDebtsFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                if (Data is null)
                {
                    RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.BadDebtsWindow);
                    DataList = GlobalViewModel.Instance.HispaniaViewModel.BadDebts;
                }
                else
                {
                    DataList = GlobalViewModel.Instance.HispaniaViewModel.GetBadDebts(Data.Customer_Id);
                }
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        private void ActualizeBadDebtFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                BadDebtsView SelectedItem = (BadDebtsView)ListItems.SelectedItem;
                BadDebtsView ItemInDb = GlobalViewModel.Instance.HispaniaViewModel.GetBadDebtFromDb(SelectedItem);
                int Index = ListItems.SelectedIndex;
                ListItems.UnselectAll();
                DataList.Remove(SelectedItem);
                DataList.Insert(Index, ItemInDb);
                ListItems.SelectedItem = ItemInDb;
                BadDebtDataControl.BadDebtData = ItemInDb;
            //  Deactivate managers
                DataChangedManagerActive = true;
        }
        
        private void ActualizeBadDebtsFromDb(out List<BadDebtsView> ItemsInDb)
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                ItemsInDb = new List<BadDebtsView>(ListItems.SelectedItems.Count);
                foreach (BadDebtsView badDebt in new ArrayList(ListItems.SelectedItems))
                {
                    BadDebtsView badDebtInDb = GlobalViewModel.Instance.HispaniaViewModel.GetBadDebtFromDb(badDebt);
                    ItemsInDb.Add(badDebtInDb);
                    int Index = DataList.IndexOf(badDebt);
                    ListItems.SelectedItems.Remove(badDebt);
                    DataList.Remove(badDebt);
                    DataList.Insert(Index, badDebtInDb);
                    ListItems.SelectedItems.Add(badDebtInDb);
                }
                ListItems.UpdateLayout();
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        #endregion

        #region UpdateUI

        /// <summary>
        /// Method that show the Item Pannel
        /// </summary>
        private void ShowItemPannel()
        {
            gsSplitter.IsEnabled = true;
            rdItemPannel.Height = ViewItemPanel;
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
            rdOperationPannel.Height = ViewOperationPanel;
            gsSplitter.IsEnabled = false;
            RefreshButtonBar();
            GridList.IsEnabled = true;
        }

        /// <summary>
        /// Refresh Button Bar with nowadays state of the formulary.
        /// </summary>
        /// <param name="IsEditing"></param>
        private void RefreshButtonBar(bool IsEditing = false)
        {
            rdSearchPannel.Height = m_DataList.Count > 0 ? ViewSearchPanel : HideComponent;
            cdAdd.Width = IsEditing || Print ? HideComponent : ViewAddPanel;
            bool CanEditOrDelete = ListItems.SelectedItem != null && !IsEditing && !Print;
            cdEdit.Width = CanEditOrDelete ? ViewEditPanel : HideComponent;
            cdView.Width = CanEditOrDelete ? ViewViewPanel : HideComponent;
            cdDelete.Width = CanEditOrDelete ? ViewDeletePanel : HideComponent;
            cdReport.Width = Print ? ViewReportPanel : HideComponent;
        }

        #endregion
    }
}
