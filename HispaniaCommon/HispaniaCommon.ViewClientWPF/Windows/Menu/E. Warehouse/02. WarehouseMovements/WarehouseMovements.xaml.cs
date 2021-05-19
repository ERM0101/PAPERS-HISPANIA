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
    /// Interaction logic for WarehouseMovements.xaml
    /// </summary>
    public partial class WarehouseMovements : Window
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
        /// Show the Item Pannel.
        /// </summary>
        private GridLength ViewTextFilterPannel = new GridLength(1.0, GridUnitType.Star);

        /// <summary>
        /// Show the Item Pannel.
        /// </summary>
        private GridLength ViewDateFilterPannel = new GridLength(1.0, GridUnitType.Star);

        /// <summary>
        /// Show the Item Pannel.
        /// </summary>
        private GridLength ViewButtonPannel = new GridLength(120.0);

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
        /// Store the data to show in List of Items.
        /// </summary>
        private ObservableCollection<WarehouseMovementsView> m_DataList = new ObservableCollection<WarehouseMovementsView>();

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
        public ObservableCollection<WarehouseMovementsView> DataList
        {
            get
            {
                return (m_DataList);
            }
            set
            {
                //  Actualize the value
                    if (value != null) m_DataList = value;
                    else m_DataList = new ObservableCollection<WarehouseMovementsView>();
                //  Deactivate managers
                    DataChangedManagerActive = false;
                //  Set up controls state
                    ListItems.ItemsSource = m_DataList;
                    ListItems.DataContext = this;
                    CollectionViewSource.GetDefaultView(ListItems.ItemsSource).SortDescriptions.Add(new SortDescription("Date", ListSortDirection.Ascending));
                    CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Filter = UserFilter;
                //  Reactivate managers
                    DataChangedManagerActive = true;
                //  Initialize the filter options
                    InitDateFilter = NormalizeDateInit(dtpInitDateFilter.SelectedDate);
                    tbInitDateFilter.Text = GlobalViewModel.GetLongDateString(InitDateFilter);
                    EndDateFilter = NormalizeDateEnd(dtpEndDateFilter.SelectedDate);
                    tbEndDateFilter.Text = EndDateFilter is null ? "No seleccionda" : GlobalViewModel.GetLongDateString((DateTime)EndDateFilter);
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

        private DateTime InitDateFilter
        {
            get;
            set;
        }

        private DateTime? EndDateFilter
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

        #region Foreign Tables Info

        /// <summary>
        /// Get or Set the Goods
        /// </summary>
        public Dictionary<string, GoodsView>  Goods
        {
            set
            {
                WarehouseMovementDataControl.Goods = value;
            }
        }

        /// <summary>
        /// Get or Set the Providers
        /// </summary>
        public Dictionary<string, ProvidersView> Providers
        {
            set
            {
                WarehouseMovementDataControl.Providers = value;
            }
        }

        #endregion

        #endregion

        #region Buiders

        /// <summary>
        /// Main Builder of the windows.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        public WarehouseMovements(ApplicationType AppType)
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
                WarehouseMovementDataControl.AppType = AppType;
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
                cbFieldItemToSearch.ItemsSource = WarehouseMovementsView.Fields;
                cbFieldItemToSearch.DisplayMemberPath = "Key";
                cbFieldItemToSearch.SelectedValuePath = "Value";
                if (WarehouseMovementsView.Fields.Count > 0) cbFieldItemToSearch.SelectedIndex = 0;
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
                WarehouseMovementsView ItemData = (WarehouseMovementsView)item;
                String ProperyName = (string) cbFieldItemToSearch.SelectedValue;
            //  Apply the filter by selected field value
                if (ProperyName == "Date_Str")
                {
                    DateTime dateToTest = DateTime.Parse(ItemData.GetType().GetProperty("Date").GetValue(ItemData).ToString());
                    return dateToTest >= InitDateFilter && dateToTest <= EndDateFilter;
                }
                else
                {
                    if (!String.IsNullOrEmpty(tbItemToSearch.Text))
                    {
                        object valueToTest = ItemData.GetType().GetProperty(ProperyName).GetValue(ItemData);
                        if ((valueToTest is null) || 
                            (!(valueToTest.ToString().ToUpper()).Contains(tbItemToSearch.Text.ToUpper())))
                        {
                            return false;
                        }
                    }
                    return true;
                }
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
            //  Button Search Clients
                btnEdit.Click += BtnEdit_Click;
                btnDelete.Click += BtnDelete_Click;
                btnViewData.Click += BtnViewData_Click;
                btnRefresh.Click += BtnRefresh_Click;
            //  TextBox
                tbItemToSearch.TextChanged += TbItemToSearch_TextChanged;
            //  Define ComboBox events to manage.
                cbFieldItemToSearch.SelectionChanged += CbFieldItemToSearch_SelectionChanged;
            //  DatePiker
                dtpInitDateFilter.SelectedDateChanged += SelectedDateChanged;
                dtpEndDateFilter.SelectedDateChanged += SelectedDateChanged;
            //  Define ListView events to manage.
                ListItems.SelectionChanged += ListItems_SelectionChanged;   
            //  Define CustomerDataControl events to manage.
                WarehouseMovementDataControl.EvAccept += WarehouseMovementDataControl_evAccept;
                WarehouseMovementDataControl.EvCancel += WarehouseMovementDataControl_evCancel;
        }

        #region Filter

        /// <summary>
        /// Manage the user selection of filter field
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

        /// <summary>
        /// Manage the user selection of filter field
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CbFieldItemToSearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Refresh();
                RefreshButtonBar();
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Manage the user selection of data filter fields
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                try
                {
                    if (sender == dtpInitDateFilter)
                    {
                        DateTime InitValidityPeriod = NormalizeDateInit(dtpInitDateFilter.SelectedDate);
                        DateTime? EndValidityPeriod = NormalizeDateEnd(dtpEndDateFilter.SelectedDate);
                        if ((EndValidityPeriod != null) && (InitValidityPeriod > (DateTime)EndValidityPeriod))
                        {
                            MsgManager.ShowMessage("Error, la data d'inici del període de filtratge no pot ser major que la de final.");
                            dtpInitDateFilter.SelectedDate = InitDateFilter;
                        }
                        else
                        {
                            dtpEndDateFilter.DisplayDate = EndValidityPeriod ?? (DateTime)NormalizeDateEnd(DateTime.Now);
                            dtpEndDateFilter.DisplayDateStart = InitValidityPeriod;
                            InitDateFilter = InitValidityPeriod;
                        }
                        dtpInitDateFilter.DisplayDate = InitDateFilter;
                        tbInitDateFilter.Text = GlobalViewModel.GetLongDateString(InitDateFilter);
                    }
                    else if (sender == dtpEndDateFilter)
                    {
                        dtpEndDateFilter.DisplayDateStart = InitDateFilter;
                        EndDateFilter = NormalizeDateEnd(dtpEndDateFilter.SelectedDate);
                        tbEndDateFilter.Text = EndDateFilter is null ? "No seleccionda" : GlobalViewModel.GetLongDateString((DateTime)EndDateFilter);
                    }
                    CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Refresh();
                    RefreshButtonBar();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'aplicar el filtre per dates.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
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
                ActualizeWarehouseMovementsFromDb();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al refrescar els valors dels Moviments de Magatzem.", MsgManager.ExcepMsg(ex)));
            }
        }

        #endregion

        #region Editing Warehouse Movement

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
                    ActualizeWarehouseMovementFromDb();
                    WarehouseMovementDataControl.CtrlOperation = Operation.Show;
                    gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                    ShowItemPannel();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, al visualitzar les dades del moviment de magatzem.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
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
                    ActualizeWarehouseMovementFromDb();
                    WarehouseMovementsView WarehouseMovementToEdit = (WarehouseMovementsView)ListItems.SelectedItem;
                    if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(WarehouseMovementToEdit, out string ErrMsg))
                    {
                        if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(WarehouseMovementToEdit.Good, out ErrMsg))
                        {
                            WarehouseMovementDataControl.CtrlOperation = Operation.Edit;
                            gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "EditableGroupBox");
                            ShowItemPannel();
                        }
                        else
                        {
                            MsgManager.ShowMessage(ErrMsg);
                            if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(WarehouseMovementToEdit, out ErrMsg))
                            {
                                MsgManager.ShowMessage(ErrMsg);
                            }
                        }
                    }
                    else MsgManager.ShowMessage(ErrMsg);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(
                       string.Format("Error, a l'editar el moviment de magatzem.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
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
                WarehouseMovementsView WarehouseMovementsToDelete = (WarehouseMovementsView)ListItems.SelectedItem;
                string Question = string.Format("Està segur que vol esborrar el moviment de magatezem '{0}' seleccionat ?", 
                                                WarehouseMovementsToDelete.WarehouseMovement_Id);
                if (MsgManager.ShowQuestion(Question) == MessageBoxResult.Yes)
                {
                    if (WarehouseMovementsToDelete.According)
                    {
                        Question = string.Format("El moviment de magatezem '{0}' seleccionat està conforme, segur que el vol esborrar ?",
                                                 WarehouseMovementsToDelete.WarehouseMovement_Id);
                        if (MsgManager.ShowQuestion(Question) != MessageBoxResult.Yes)
                        {
                            MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                            return;
                        }
                    }
                    bool MovementLocked = false;
                    bool GoodLocked = false;
                    try
                    {
                        if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(WarehouseMovementsToDelete, out string ErrMsg))
                        {
                            MovementLocked = true;
                            if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(WarehouseMovementsToDelete.Good, out ErrMsg))
                            {
                                GoodLocked = true;
                                GlobalViewModel.Instance.HispaniaViewModel.DeleteWarehouseMovement(WarehouseMovementsToDelete, false);
                                if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(WarehouseMovementsToDelete, out ErrMsg))
                                {
                                    MsgManager.ShowMessage(ErrMsg);
                                }
                                DataChangedManagerActive = false;
                                if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                                DataList.Remove(WarehouseMovementsToDelete);
                                //DataList = GlobalViewModel.Instance.HispaniaViewModel.WarehouseMovements;
                                //Goods = GlobalViewModel.Instance.HispaniaViewModel.GoodsActiveDict; //.GoodsDict;
                                DataChangedManagerActive = true;
                                ListItems.UpdateLayout();
                            }
                            else MsgManager.ShowMessage(ErrMsg);
                        }
                        else MsgManager.ShowMessage(ErrMsg);
                    }
                    catch (Exception ex)
                    {
                        MsgManager.ShowMessage(
                           string.Format("Error, a l'esborrar el moviment de magatzem '{0}'.\r\nDetalls: {1}",
                                         WarehouseMovementsToDelete.WarehouseMovement_Id, MsgManager.ExcepMsg(ex)));
                    }
                    finally
                    {
                        if (MovementLocked)
                        {
                            if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(WarehouseMovementsToDelete, out string ErrMsg))
                            {
                                MsgManager.ShowMessage(ErrMsg);
                            }
                        }
                        if (GoodLocked)
                        {
                            if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(WarehouseMovementsToDelete.Good, out string ErrMsg))
                            {
                                MsgManager.ShowMessage(ErrMsg);
                            }
                        }
                    }
                }
                else MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
            }
        }

        /// <summary>
        /// Manage the event produced when the operation in that was doing in the CustomerDataControl was Accepted.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void WarehouseMovementDataControl_evAccept(WarehouseMovementsView NewOrEditedWarehouseMovement)
        {
            try
            {
                WarehouseMovementsView NewWarehouseMovement = new WarehouseMovementsView(NewOrEditedWarehouseMovement);
                switch (WarehouseMovementDataControl.CtrlOperation)
                {
                    case Operation.Edit:
                         WarehouseMovementsView NowadaysWarehouseMovement = new WarehouseMovementsView((WarehouseMovementsView)ListItems.SelectedItem);
                         GlobalViewModel.Instance.HispaniaViewModel.UpdateWarehouseMovement(NowadaysWarehouseMovement, NewWarehouseMovement, false);
                         if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(WarehouseMovementDataControl.WarehouseMovement, out string ErrMsg) ||
                             !GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(NowadaysWarehouseMovement.Good, out ErrMsg))
                         {
                             MsgManager.ShowMessage(ErrMsg);
                         }
                         DataChangedManagerActive = false;
                         if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                         DataList = GlobalViewModel.Instance.HispaniaViewModel.WarehouseMovements;
                         Goods = GlobalViewModel.Instance.HispaniaViewModel.GoodsActiveDict; 
                         DataChangedManagerActive = true;
                         ListItems.SelectedItem = NewWarehouseMovement;
                         ListItems.UpdateLayout();
                         gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                         HideItemPannel();
                         break;
                }
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(
                   string.Format("Error, al guardar les dades del moviment de magatzem.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
            }
        }

        /// <summary>
        /// Manage the event produced when the operation in that was doing in the CustomerDataControl was Cancelled.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void WarehouseMovementDataControl_evCancel()
        {
            switch (WarehouseMovementDataControl.CtrlOperation)
            {
                case Operation.Edit:
                     MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                     if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(WarehouseMovementDataControl.WarehouseMovement, out string ErrMsg))
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

        private void Customers_Closed(object sender, EventArgs e)
        {
            if (WarehouseMovementDataControl.CtrlOperation == Operation.Edit)
            {
                if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(WarehouseMovementDataControl.WarehouseMovement, out string ErrMsg))
                {
                    MsgManager.ShowMessage(ErrMsg);
                }
            }
        }

        #endregion

        #endregion
                
        #region Database Operations
		        
        private void ActualizeWarehouseMovementsFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.WarehouseMovementsWindow);
                Goods = GlobalViewModel.Instance.HispaniaViewModel.GoodsActiveDict;
                Providers = GlobalViewModel.Instance.HispaniaViewModel.ProvidersDict;
                DataList = GlobalViewModel.Instance.HispaniaViewModel.WarehouseMovements;
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        private void ActualizeWarehouseMovementFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                WarehouseMovementsView SelectedItem = (WarehouseMovementsView)ListItems.SelectedItem;
                WarehouseMovementsView ItemInDb = GlobalViewModel.Instance.HispaniaViewModel.GetWarehouseMovementFromDb(SelectedItem);
                int Index = ListItems.SelectedIndex;
                ListItems.UnselectAll();
                DataList.Remove(SelectedItem);
                DataList.Insert(Index, ItemInDb);
                ListItems.SelectedItem = ItemInDb;
                WarehouseMovementDataControl.WarehouseMovement = ItemInDb;
                ListItems.UpdateLayout();
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        #endregion

        #region Update UI
        
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
            rdOperationPannel.Height = ViewOperationPannel;
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
            rdSearchPannel.Height = (DataList.Count > 0) ? ViewSearchPannel : HideComponent;
            bool HasItemSelected = !(ListItems.SelectedItem is null);
            bool CanEditOrDelete = HasItemSelected && !IsEditing;
            cdEdit.Width = CanEditOrDelete ? ViewButtonPannel : HideComponent;
            cdDelete.Width = CanEditOrDelete ? ViewButtonPannel : HideComponent;
            cdView.Width = HasItemSelected ? ViewButtonPannel : HideComponent;
            bool IsDateFilter = (string)cbFieldItemToSearch.SelectedValue == "Date_Str";
            cdTextFilter.Width = IsDateFilter ? HideComponent : ViewTextFilterPannel;
            cdDateFilter.Width = IsDateFilter ? ViewTextFilterPannel : HideComponent;
        }

        #endregion

        #region Shared Functions

        private DateTime NormalizeDateInit(DateTime? Date)
        {
            DateTime DateBase = Date is null ? DateTime.Now : (DateTime) Date;
            return new DateTime(DateBase.Year, DateBase.Month, DateBase.Day, 0, 0, 0);

        }

        private DateTime? NormalizeDateEnd(DateTime? Date)
        {
            if (Date is null) return null;
            else
            {
                DateTime DateBase = (DateTime)Date;
                return Date is null ? Date : new DateTime(DateBase.Year, DateBase.Month, DateBase.Day, 23, 59, 59);
            }
        }

        #endregion
    }
}
