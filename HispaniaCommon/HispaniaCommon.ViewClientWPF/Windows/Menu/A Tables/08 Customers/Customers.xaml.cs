#region Librerias usadas por la ventana

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
    /// Interaction logic for MainWindowOp1.xaml
    /// </summary>
    public partial class Customers : Window
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
        /// Show the Item Pannel.
        /// </summary>
        private GridLength ViewItemPannel = new GridLength(500.0);

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
        /// Window instance of StoreAddress.
        /// </summary>
        private AddressStores AddressStoresWindow = null;

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private ObservableCollection<CustomersView> m_DataList = new ObservableCollection<CustomersView>();

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
        /// Get or Set the data to show in List of Items.
        /// </summary>
        public ObservableCollection<CustomersView> DataList
        {
            get
            {
                return (m_DataList);
            }
            set
            {
                if (value != null) m_DataList = value;
                else m_DataList = new ObservableCollection<CustomersView>();
                ListItems.ItemsSource = m_DataList;
                ListItems.DataContext = this;
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).SortDescriptions.Add(new SortDescription("Customer_Id", ListSortDirection.Descending));
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Filter = UserFilter;
                if (m_DataList.Count > 0)
                {
                    cdAdd.Width = (Print ? HideComponent : ViewReportButtonPannel);
                    cdReport.Width = (Print ? ViewReportButtonPannel : HideComponent);
                    rdSearchPannel.Height = ViewSearchPannel;
                }
                else
                {
                    cdReport.Width = HideComponent;
                    rdSearchPannel.Height = HideComponent;
                }
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
                Title = m_Print ? "Impressió de Fitxes de Clients" : "Gestió de Clients";
                ListItems.SelectionMode = m_Print ? SelectionMode.Extended : SelectionMode.Single;
                btnEdit.Visibility = btnDelete.Visibility = btnViewData.Visibility = (m_Print ? Visibility.Hidden : Visibility.Visible);
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

        #region Foreign Key

        /// <summary>
        /// Get or Set the Cities and CP 
        /// </summary>
        public Dictionary<string, PostalCodesView> PostalCodes
        {
            private get
            {
                return CustomerDataControl.PostalCodes;
            }
            set
            {
                CustomerDataControl.PostalCodes = value;
            }
        }

        /// <summary>
        /// Get or Set the EffectTypes
        /// </summary>
        public Dictionary<string, EffectTypesView> EffectTypes
        {
            set
            {
                CustomerDataControl.EffectTypes = value;
            }
        }

        /// <summary>
        /// Get or Set the SendTypes
        /// </summary>
        public Dictionary<string, SendTypesView> SendTypes
        {
            set
            {
                CustomerDataControl.SendTypes = value;
            }
        }

        /// <summary>
        /// Get or Set the Agents
        /// </summary>
        public Dictionary<string, AgentsView> Agents
        {
            set
            {
                CustomerDataControl.Agents = value;
            }
        }

        /// <summary>
        /// Get or Set the IVATypes
        /// </summary>
        public Dictionary<string, IVATypesView> IVATypes
        {
            set
            {
                CustomerDataControl.IVATypes = value;
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
        public Customers(ApplicationType AppType)
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
                CustomerDataControl.AppType = AppType;
            //  Initialize state of Window components.
                rdItemPannel.Height = HideComponent;
                rdSearchPannel.Height = HideComponent;
                gsSplitter.IsEnabled = false;
                btnEdit.Visibility = btnDelete.Visibility = btnViewData.Visibility = Visibility.Hidden;
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
                cbFieldItemToSearch.ItemsSource = CustomersView.Fields;
                cbFieldItemToSearch.DisplayMemberPath = "Key";
                cbFieldItemToSearch.SelectedValuePath = "Value";
                if (CustomersView.Fields.Count > 0) cbFieldItemToSearch.SelectedIndex = 1;
            //  Deactivate managers
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
                CustomersView ItemData = (CustomersView)item;
                String ProperyName = (string) cbFieldItemToSearch.SelectedValue;
            //  Apply the filter by selected field value
                if (!String.IsNullOrEmpty(tbItemToSearch.Text))
                {
                    if (ProperyName == "Company_EMail")
                    {
                        object valueToTest = ItemData.GetType().GetProperty(ProperyName).GetValue(ItemData);
                        object valueToTest2 = ItemData.GetType().GetProperty("Company_EMail2").GetValue(ItemData);
                        object valueToTest3 = ItemData.GetType().GetProperty("Company_EMail3").GetValue(ItemData);
                        if (((valueToTest is null) ||
                            (!(valueToTest.ToString().ToUpper()).Contains(tbItemToSearch.Text.ToUpper()))) &&
                            ((valueToTest2 is null) ||
                                (!(valueToTest2.ToString().ToUpper()).Contains(tbItemToSearch.Text.ToUpper()))) &&
                            ((valueToTest3 is null) ||
                                (!(valueToTest3.ToString().ToUpper()).Contains(tbItemToSearch.Text.ToUpper()))))
                        {
                            return false;
                        }
                    }else
                    {
                        object valueToTest = ItemData.GetType().GetProperty(ProperyName).GetValue(ItemData);
                        if ((valueToTest is null) ||
                            (!(valueToTest.ToString().ToUpper()).Contains(tbItemToSearch.Text.ToUpper())))
                        {
                            return false;
                        }
                    }                   
                }
            //  Calculate the Visibility value with properties values.
                return (chkbCanceled.IsChecked == ItemData.Canceled);
        }
        
        #endregion

        #region Managers

        /// <summary>
        /// Method that define the managers needed for the user operations in the Window
        /// </summary>
        private void LoadManagers()
        {
            //  Window
                this.Closed += Customers_Closed;
            //  TextBox
                tbItemToSearch.TextChanged += TbItemToSearch_TextChanged;
            //  CheckBox
                chkbCanceled.Checked += ChkbCanceled_Checked;
                chkbCanceled.Unchecked += ChkbCanceled_Unchecked;
            //  Button Search Clients
                btnAdd.Click += BtnAdd_Click;
                btnEdit.Click += BtnEdit_Click;
                btnDelete.Click += BtnDelete_Click;
                btnViewData.Click += BtnViewData_Click;
                btnReport.Click += BtnReport_Click;
                btnRefresh.Click += BtnRefresh_Click;
                btnAddressStores.Click += BtnAddressStores_Click;
            //  Define ListView events to manage.
                ListItems.SelectionChanged += ListItems_SelectionChanged;   
            //  Define CustomerDataControl events to manage.
                CustomerDataControl.EvAccept += CustomerDataControl_evAccept;
                CustomerDataControl.EvCancel += CustomerDataControl_evCancel;
        }

        #region Filter

        /// <summary>
        /// Manage the search of Items in the list.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void TbItemToSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterDataListObjects();
        }

        private void ChkbCanceled_Unchecked(object sender, RoutedEventArgs e)
        {
            FilterDataListObjects();
        }

        private void ChkbCanceled_Checked(object sender, RoutedEventArgs e)
        {
            FilterDataListObjects();
        }

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
                ActualizeCustomersRefreshFromDb();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al refrescar els valors dels Clients.", MsgManager.ExcepMsg(ex)));
            }
        }

        #endregion

        #region Editing Customer

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
                    ActualizeCustomerFromDb();
                    CustomerDataControl.CtrlOperation = Operation.Show;
                    gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                    ShowItemPannel();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, a l'intentar veure les dades del Client seleccionat.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
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
                InitializeCustomerDataControlData();
                CustomerDataControl.CtrlOperation = Operation.Add;
                gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "EditableGroupBox");
                ShowItemPannel();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
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
                    ActualizeCustomerFromDb();
                    if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(ListItems.SelectedItem, out string ErrMsg))
                    {
                        CustomerDataControl.CtrlOperation = Operation.Edit;
                        gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "EditableGroupBox");
                        ShowItemPannel();
                    }
                    else MsgManager.ShowMessage(ErrMsg);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, a l'iniciar l'edició del Client seleccionat.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
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
                CustomersView CustomersToDelete = (CustomersView)ListItems.SelectedItem;
                string Question = string.Format("Està segur que vol esborrar el Client '{0}' ?", CustomersToDelete.Customer_Alias);
                if (MsgManager.ShowQuestion(Question) == MessageBoxResult.Yes)
                {
                    string ErrMsg;
                    try
                    {
                        if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(CustomersToDelete, out ErrMsg))
                        {
                            GlobalViewModel.Instance.HispaniaViewModel.DeleteCustomer(DataList[DataList.IndexOf(CustomersToDelete)]);
                            GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(CustomersToDelete, out ErrMsg);
                            DataChangedManagerActive = false;
                            if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                            DataList.Remove(CustomersToDelete);
                            DataChangedManagerActive = true;
                            ListItems.UpdateLayout();
                            HideItemPannel();
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrMsg = string.Format("Error, al esborrar esborrar el Client '{0}'.\r\nDetalls: {1}", CustomersToDelete.Customer_Alias, MsgManager.ExcepMsg(ex));
                    }
                    if (!string.IsNullOrEmpty(ErrMsg)) MsgManager.ShowMessage(ErrMsg);
                }
                else MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
            }
        }

        /// <summary>
        /// Manage the event produced when the operation in that was doing in the CustomerDataControl was Accepted.
        /// </summary>
        /// <param name="NewOrEditedCustomer">New data for Customer Edited or Created</param>
        /// <param name="NewOrEditedRelatedCustomers">Related Customers for Customer Edited or Created</param>
        private void CustomerDataControl_evAccept(CustomersView NewOrEditedCustomer, List<RelatedCustomersView> NewOrEditedRelatedCustomers)
        {
            try
            {
                CustomersView NewCustomer = new CustomersView(NewOrEditedCustomer);
                switch (CustomerDataControl.CtrlOperation)
                {
                    case Operation.Add:
                         GlobalViewModel.Instance.HispaniaViewModel.CreateCustomer(NewCustomer, NewOrEditedRelatedCustomers);
                         DataChangedManagerActive = false;
                         if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                         DataList.Add(NewCustomer);
                         DataChangedManagerActive = true;
                         ListItems.SelectedItem = NewCustomer;
                         ListItems.UpdateLayout();
                         gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                         HideItemPannel();
                         break;
                    case Operation.Edit:
                         GlobalViewModel.Instance.HispaniaViewModel.UpdateCustomer(NewCustomer, NewOrEditedRelatedCustomers);
                         if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(CustomerDataControl.Customer, out string ErrMsg))
                         {
                             MsgManager.ShowMessage(ErrMsg);
                         }
                         DataChangedManagerActive = false;
                         CustomersView SourceCustomer = (CustomersView)ListItems.SelectedItem;
                         if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                         DataList.Remove(SourceCustomer);
                         DataList.Add(GlobalViewModel.Instance.HispaniaViewModel.GetCustomerFromDb(NewCustomer));
                         DataChangedManagerActive = true;
                         ListItems.SelectedItem = NewCustomer;
                         ListItems.UpdateLayout();
                         gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                         HideItemPannel();
                         break;
                }
            }
            catch (Exception ex)
            {
                string OperationInfo = ". Operació no reconeguda.";
                switch (CustomerDataControl.CtrlOperation)
                {
                    case Operation.Add:
                         OperationInfo = " que s'intenta afegir.";
                         break;
                    case Operation.Edit:
                         OperationInfo = " que s'està editant.";
                         break;
                }
                MsgManager.ShowMessage(string.Format("Error, al guardar les dades del Client{0}\r\nDetalls: {1}", OperationInfo, MsgManager.ExcepMsg(ex)));
            }
        }

        /// <summary>
        /// Manage the event produced when the operation in that was doing in the CustomerDataControl was Cancelled.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CustomerDataControl_evCancel()
        {
            switch (CustomerDataControl.CtrlOperation)
            {
                case Operation.Add:
                     MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                     break;
                case Operation.Edit:
                     MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                     if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(CustomerDataControl.Customer, out string ErrMsg))
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

        #region Address Stores (Update data)

        /// <summary>
        /// Defines a new Address Store for the Customer.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnAddressStores_Click(object sender, RoutedEventArgs e)
        {
            if (AddressStoresWindow == null)
            {
                try
                {
                    CustomersView Customer = (CustomersView)ListItems.SelectedItem;
                    AddressStoresWindow = new AddressStores(AppType, Operation.Edit)
                    {
                        PostalCodes = PostalCodes,
                        DataList = GlobalViewModel.Instance.HispaniaViewModel.GetAddressStores(Customer.Customer_Id),
                        Data = Customer
                    };
                    AddressStoresWindow.Closed += StoreAddressesWindow_Closed;
                    AddressStoresWindow.ShowDialog();

                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, al carregar la finestra de gestió de Magatzems.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
            else AddressStoresWindow.Activate();
        }

        private void StoreAddressesWindow_Closed(object sender, EventArgs e)
        {
            AddressStoresWindow.Closed -= StoreAddressesWindow_Closed;
            AddressStoresWindow = null;
        }

        #endregion

        #endregion

        #region List Item

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
                ActualizeButtonBar();
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region Window

        private void Customers_Closed(object sender, EventArgs e)
        {
            if (CustomerDataControl.CtrlOperation == Operation.Edit)
            {
                if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(CustomerDataControl.Customer, out string ErrMsg))
                {
                    MsgManager.ShowMessage(ErrMsg);
                }
            }
        }

        #endregion

        #region Report

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
                    ActualizeCustomersFromDb(out List<CustomersView> ItemsInDb);
                    CustomersReportView.CreateReport(ItemsInDb);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(String.Format("Error, al crear l'informe de Clients.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
        }

        #endregion

        #endregion
        
        #region Database Operations
        		        
        private void ActualizeCustomersRefreshFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.CustomersWindow);
                PostalCodes = GlobalViewModel.Instance.HispaniaViewModel.PostalCodesDict;
                EffectTypes = GlobalViewModel.Instance.HispaniaViewModel.EffectTypesDict;
                SendTypes = GlobalViewModel.Instance.HispaniaViewModel.SendTypesDict;
                Agents = GlobalViewModel.Instance.HispaniaViewModel.AgentsDict;
                IVATypes = GlobalViewModel.Instance.HispaniaViewModel.IVATypesDict;
                DataList = GlobalViewModel.Instance.HispaniaViewModel.Customers;
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        private void InitializeCustomerDataControlData()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                CustomerDataControl.DataListDefinedCustomers = DataList;
                CustomerDataControl.DataListRelatedCustomers = new ObservableCollection<CustomersView>();
            //  Activate managers
                DataChangedManagerActive = true;
        }

        private void ActualizeCustomerFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                CustomersView SelectedItem = (CustomersView)ListItems.SelectedItem;
                CustomersView ItemInDb = GlobalViewModel.Instance.HispaniaViewModel.GetCustomerFromDb(SelectedItem);
                int Index = ListItems.SelectedIndex;
                ListItems.UnselectAll();
                DataList.Remove(SelectedItem);
                DataList.Insert(Index, ItemInDb);
                ListItems.SelectedItem = ItemInDb;
                CustomerDataControl.Customer = ItemInDb;
                CustomerDataControl.DataListDefinedCustomers = DataList;
                GlobalViewModel.Instance.HispaniaViewModel.RefreshRelatedCustomers(ItemInDb.Customer_Id);
                ObservableCollection<CustomersView> RelatedCustomers = new ObservableCollection<CustomersView>();
                foreach (RelatedCustomersView relatedCustomer in GlobalViewModel.Instance.HispaniaViewModel.RelatedCustomers)
                {
                    CustomersView customer = GlobalViewModel.Instance.HispaniaViewModel.GetCustomerFromDb(relatedCustomer.Customer_Canceled_Id);
                    CustomerDataControl.DataListDefinedCustomers.Remove(customer);
                    RelatedCustomers.Add(customer);
                }
                CustomerDataControl.DataListRelatedCustomers = RelatedCustomers;
                ListItems.UpdateLayout();
            //  Activate managers
                DataChangedManagerActive = true;
        }

        private void ActualizeCustomersFromDb(out List<CustomersView> ItemsInDb)
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                ItemsInDb = new List<CustomersView>(ListItems.SelectedItems.Count);
                foreach (CustomersView customer in new ArrayList(ListItems.SelectedItems))
                {
                    CustomersView customerInDb = GlobalViewModel.Instance.HispaniaViewModel.GetCustomerFromDb(customer);
                    ItemsInDb.Add(customerInDb);
                    int Index = DataList.IndexOf(customer);
                    ListItems.SelectedItems.Remove(customer);
                    DataList.Remove(customer);
                    DataList.Insert(Index, customerInDb);
                    ListItems.SelectedItems.Add(customerInDb);
                }
                ListItems.UpdateLayout();
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        #endregion

        #region Update UI

        private void FilterDataListObjects()
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Refresh();
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Method that show the Item Pannel
        /// </summary>
        private void ShowItemPannel()
        {
            gsSplitter.IsEnabled = true;
            rdItemPannel.Height = ViewItemPannel;
            rdOperationPannel.Height = HideComponent;
            btnAdd.Visibility = btnEdit.Visibility = btnDelete.Visibility = btnViewData.Visibility = Visibility.Hidden;
            GridList.IsEnabled = false;
        }

        /// <summary>
        /// Method that show the Item Pannel
        /// </summary>
        private void HideItemPannel()
        {
            rdItemPannel.Height = HideComponent;
            rdOperationPannel.Height = ViewOperationPannel;
            gsSplitter.IsEnabled = false;
            btnAdd.Visibility = Visibility.Visible;
            if (DataList.Count > 0)
            {
                cdAdd.Width = (Print ? HideComponent : ViewReportButtonPannel);
                cdReport.Width = (Print ? ViewReportButtonPannel : HideComponent);
                rdSearchPannel.Height = ViewSearchPannel;
                ActualizeButtonBar();
            }
            else
            {
                cdReport.Width = HideComponent;
                rdSearchPannel.Height = HideComponent;
            }
            GridList.IsEnabled = true;
        }

        /// <summary>
        /// Method that actualize the Button Bar look
        /// </summary>
        private void ActualizeButtonBar()
        {
            if (ListItems.SelectedItem != null)
            {
                btnEdit.Visibility = btnDelete.Visibility = (Print ? Visibility.Hidden : Visibility.Visible);
                btnViewData.Visibility = btnAddressStores.Visibility = (Print ? Visibility.Hidden : Visibility.Visible);
            }
            else btnEdit.Visibility = btnDelete.Visibility = btnViewData.Visibility = btnAddressStores.Visibility = Visibility.Hidden;
        }

        #endregion
    }
}
