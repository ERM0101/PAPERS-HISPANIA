#region Librerias usadas por la ventana

using HispaniaCommon.ViewClientWPF.Managers;
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
    /// Interaction logic for Providers.xaml
    /// </summary>
    public partial class Providers : Window
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
        private GridLength ViewItemPannel = new GridLength(1.0, GridUnitType.Star);

        /// <summary>
        /// Show the Report button.
        /// </summary>
        private GridLength ViewReportButtonPannel = new GridLength(120.0);

        /// <summary>
        /// Show the Report button.
        /// </summary>
        private GridLength ViewReportButtonPannel2 = new GridLength(180.0);

        /// <summary>
        /// Show the Search Pannel.
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
        private ObservableCollection<ProvidersView> m_DataList = new ObservableCollection<ProvidersView>();

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
        public ObservableCollection<ProvidersView> DataList
        {
            get
            {
                return (m_DataList);
            }
            set
            {
                if (value != null) m_DataList = value;
                else m_DataList = new ObservableCollection<ProvidersView>();
                ListItems.ItemsSource = m_DataList;
                ListItems.DataContext = this;
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).SortDescriptions.Add(new SortDescription("Name", ListSortDirection.Ascending));
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Filter = UserFilter;
                if (m_DataList.Count > 0)
                {
                    cdAdd.Width = Print ? HideComponent : ViewReportButtonPannel;
                    cdReport.Width = Print ? ViewReportButtonPannel : HideComponent;
                    cdComandesCompletes.Width = Print ? ViewReportButtonPannel2 : HideComponent;
                    rdSearchPannel.Height = ViewSearchPannel;
                }
                else
                {
                    cdReport.Width = HideComponent;
                    cdComandesCompletes.Width = HideComponent;
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
                Title = m_Print ? "Impressió de Fitxes de Proveïdors" : "Gestió de Proveïdors";
                ListItems.SelectionMode = m_Print ? SelectionMode.Extended : SelectionMode.Single;
                btnEdit.Visibility = btnDelete.Visibility = btnViewData.Visibility = m_Print ? Visibility.Hidden : Visibility.Visible;
                btnReport.Visibility = m_Print ? Visibility.Visible : Visibility.Hidden;
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
        /// Get or Set the Cities and Postal Codes 
        /// </summary>
        public Dictionary<string, PostalCodesView> PostalCodes
        {
            set
            {
                ProviderDataControl.PostalCodes = value;
            }
        }

        /// <summary>
        /// Get or Set the EffectTypes
        /// </summary>
        public Dictionary<string, EffectTypesView> EffectTypes
        {
            set
            {
                ProviderDataControl.EffectTypes = value;
            }
        }

        /// <summary>
        /// Get or Set the SendTypes
        /// </summary>
        public Dictionary<string, SendTypesView> SendTypes
        {
            set
            {
                ProviderDataControl.SendTypes = value;
            }
        }

        /// <summary>
        /// Get or Set the Agents
        /// </summary>
        public Dictionary<string, AgentsView> Agents
        {
            set
            {
                ProviderDataControl.Agents = value;
            }
        }

        /// <summary>
        /// Get or Set the IVATypes
        /// </summary>
        public Dictionary<string, IVATypesView> IVATypes
        {
            set
            {
                ProviderDataControl.IVATypes = value;
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
        public Providers(ApplicationType AppType)
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
                ProviderDataControl.AppType = AppType;
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
                cbFieldItemToSearch.ItemsSource = ProvidersView.Fields;
                cbFieldItemToSearch.DisplayMemberPath = "Key";
                cbFieldItemToSearch.SelectedValuePath = "Value";
                if (ProvidersView.Fields.Count > 0) cbFieldItemToSearch.SelectedIndex = 0;
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
                ProvidersView ItemData = (ProvidersView)item;
                String ProperyName = (string) cbFieldItemToSearch.SelectedValue;
            //  Apply the filter by selected field value
                if (!String.IsNullOrEmpty(tbItemToSearch.Text))
                {
                    object valueToTest = ItemData.GetType().GetProperty(ProperyName).GetValue(ItemData);
                    if ((valueToTest is null) || 
                        (!(valueToTest.ToString().ToUpper()).Contains(tbItemToSearch.Text.ToUpper())))
                    {
                        return false;
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
                Closed += Providers_Closed;
            //  TextBox
                tbItemToSearch.TextChanged += TbItemToSearch_TextChanged;
            //  Button Search Clients
                btnAdd.Click += BtnAdd_Click;
                btnEdit.Click += BtnEdit_Click;
                btnDelete.Click += BtnDelete_Click;
                btnViewData.Click += BtnViewData_Click;
                btnReport.Click += BtnReport_Click;
                btnReport2.Click += BtnReport2_Click;
                btnRefresh.Click += BtnRefresh_Click;
            //  CheckBox
                chkbCanceled.Checked += ChkbCanceled_Checked;
                chkbCanceled.Unchecked += ChkbCanceled_Unchecked;
            //  Define ListView events to manage.
                ListItems.SelectionChanged += ListItems_SelectionChanged;
            //  Define CustomerDataControl events to manage.            
                ProviderDataControl.EvAccept += ProviderDataControl_evAccept;
                ProviderDataControl.EvCancel += ProviderDataControl_evCancel;
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
                ActualizeProvidersRefreshFromDb();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al refrescar els valors dels Provieïdors.", MsgManager.ExcepMsg(ex)));
            }
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
                    ActualizeProvidersFromDb(out List<ProvidersView> ItemsInDb);
                    ProvidersReportView.CreateReport(ItemsInDb);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, al crear l'informe del Proveïdor.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
                }
            }
        }

        /// <summary>
        /// Manage the creation of the Report of Items in the list.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnReport2_Click(object sender, RoutedEventArgs e)
        {
            MsgManager.ShowMessage("Avís, opció encara pendent de definició.", MsgType.Information);
            //if (ListItems.SelectedItems != null)
            //{
            //    try
            //    {
            //        ActualizeProvidersFromDb(out List<ProvidersView> ItemsInDb);
            //        ProvidersReportView.CreateReport2(ItemsInDb);
            //    }
            //    catch (Exception ex)
            //    {
            //        MsgManager.ShowMessage(string.Format("Error, al crear l'informe de Comandes Completes del Prfoveïdor.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
            //    }
            //}
        }

        #endregion

        #region Editing Provider

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
                    ActualizeProviderFromDb();
                    ProviderDataControl.CtrlOperation = Operation.Show;
                    gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                    ShowItemPannel();
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, a l'intentar veure les dades del Proveïdor seleccionat.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
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
            InitializeProviderDataControlData();
            ProviderDataControl.CtrlOperation = Operation.Add;
            gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "EditableGroupBox");
            ShowItemPannel();
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
                    ActualizeProviderFromDb();
                    if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(ListItems.SelectedItem, out string ErrMsg))
                    {
                        ProviderDataControl.CtrlOperation = Operation.Edit;
                        gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "EditableGroupBox");
                        ShowItemPannel();
                    }
                    else MsgManager.ShowMessage(ErrMsg);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, a l'iniciar l'edició del Proveïdor seleccionat.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
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
                ProvidersView ProvidersToDelete = (ProvidersView)ListItems.SelectedItem;
                if (MsgManager.ShowQuestion("Està segur que vol esborrar el Proveïdor seleccionat ?") == MessageBoxResult.Yes)
                {
                    string ErrMsg;
                    try
                    {
                        if (GlobalViewModel.Instance.HispaniaViewModel.LockRegister(ProvidersToDelete, out ErrMsg))
                        {
                            GlobalViewModel.Instance.HispaniaViewModel.DeleteProvider(DataList[DataList.IndexOf(ProvidersToDelete)]);
                            GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(ProvidersToDelete, out ErrMsg);
                            DataChangedManagerActive = false;
                            if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                            DataList.Remove(ProvidersToDelete);
                            DataChangedManagerActive = true;
                            ListItems.UpdateLayout();
                            HideItemPannel();
                        }
                    }
                    catch (Exception ex)
                    {
                        ErrMsg = string.Format("Error, al esborrar esborrar el Proveïdor seleccionat.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex));
                    }
                    if (!string.IsNullOrEmpty(ErrMsg)) MsgManager.ShowMessage(ErrMsg);
                }
                else MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
            }
        }

        /// <summary>
        /// Manage the event produced when the operation in that was doing in the CustomerDataControl was Accepted.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void ProviderDataControl_evAccept(ProvidersView NewOrEditedProvider, List<RelatedProvidersView> NewOrEditedRelatedProviders)
        {
            try
            {
                ProvidersView NewProvider = new ProvidersView(NewOrEditedProvider);
                switch (ProviderDataControl.CtrlOperation)
                {
                    case Operation.Add:
                         ValidateProvidersInList(NewProvider);
                         GlobalViewModel.Instance.HispaniaViewModel.CreateProvider(NewProvider);
                         DataChangedManagerActive = false;
                         if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                         DataList.Add(NewProvider);
                         DataChangedManagerActive = true;
                         ListItems.SelectedItem = NewProvider;
                         ListItems.UpdateLayout();
                         gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                         HideItemPannel();
                         break;
                    case Operation.Edit:
                         ValidateProvidersInList(NewProvider);
                         GlobalViewModel.Instance.HispaniaViewModel.UpdateProvider(NewProvider, NewOrEditedRelatedProviders);
                         if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(ProviderDataControl.Provider, out string ErrMsg))
                         {
                             MsgManager.ShowMessage(ErrMsg);
                         }
                         DataChangedManagerActive = false;
                         ProvidersView SourceProvider = (ProvidersView)ListItems.SelectedItem;
                         if (ListItems.SelectedItem != null) ListItems.UnselectAll();
                         DataList.Remove(SourceProvider);
                         DataList.Add(GlobalViewModel.Instance.HispaniaViewModel.GetProviderFromDb(NewProvider));
                         DataChangedManagerActive = true;
                         ListItems.SelectedItem = NewProvider;
                         ListItems.UpdateLayout();
                         gbEditOrCreateItem.SetResourceReference(Control.StyleProperty, "NonEditableGroupBox");
                         HideItemPannel();
                         break;
                }
            }
            catch (Exception ex)
            {
                string OperationInfo = ". Operació no reconeguda.";
                switch (ProviderDataControl.CtrlOperation)
                {
                    case Operation.Add:
                         OperationInfo = " que s'intenta afegir.";
                         break;
                    case Operation.Edit:
                         OperationInfo = " que s'està editant.";
                         break;
                }
                MsgManager.ShowMessage(string.Format("Error, al guardar les dades del Proveïdor{0}\r\nDetalls: {1}", OperationInfo, MsgManager.ExcepMsg(ex)));
            }
        }

        /// <summary>
        /// Manage the event produced when the operation in that was doing in the CustomerDataControl was Cancelled.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void ProviderDataControl_evCancel()
        {
            switch (ProviderDataControl.CtrlOperation)
            {
                case Operation.Add:
                     MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                     break;
                case Operation.Edit:
                     MsgManager.ShowMessage("Operació cancel·lada per l'usuari.", MsgType.Information);
                     if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(ProviderDataControl.Provider, out string ErrMsg))
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

        #region List View

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

        /// <summary>
        private void Providers_Closed(object sender, EventArgs e)
        {
            if (ProviderDataControl.CtrlOperation == Operation.Edit)
            {
                if (!GlobalViewModel.Instance.HispaniaViewModel.UnlockRegister(ProviderDataControl.Provider, out string ErrMsg))
                {
                    MsgManager.ShowMessage(ErrMsg);
                }
            }
        }

        #endregion

        #endregion

        #region Validation

        /// <summary>
        /// Method verifies that the New Item or the Edited Item not exist yet.
        /// </summary>
        /// <param name="NewProvider">Item to validate.</param>
        private void ValidateProvidersInList(ProvidersView NewProvider)
        {
            switch (ProviderDataControl.CtrlOperation)
            {
                case Operation.Add:
                     foreach (ProvidersView provider in DataList)
                     {
                         if (provider.Provider_Number == NewProvider.Provider_Number)
                         {
                             string MsgError = string.Format("El Proveïdor '{0}' ja està definit.", provider.Provider_Number);
                             throw new Exception(Manager_IU.GetText("ErrorProviders_01", MsgError));
                         }
                     }
                     break;
                case Operation.Edit:
                     foreach (ProvidersView provider in DataList)
                     {
                         if ((provider.Provider_Number == NewProvider.Provider_Number) && (provider.Provider_Id != NewProvider.Provider_Id))
                         {
                             string MsgError = string.Format("El Proveïdor '{0}' ja està definit.", provider.Provider_Number);
                             throw new Exception(Manager_IU.GetText("ErrorProviders_01", MsgError));
                         }
                     }
                     break;
            }
        }

        #endregion

        #region Database Operations


        private void InitializeProviderDataControlData()
        {
            //  Deactivate managers
            DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
            ProviderDataControl.DataListDefinedProviders = DataList;
            ProviderDataControl.DataListRelatedProviders = new ObservableCollection<ProvidersView>();
            //  Activate managers
            DataChangedManagerActive = true;
        }

        private void ActualizeProvidersRefreshFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.ProvidersWindow);
                PostalCodes = GlobalViewModel.Instance.HispaniaViewModel.PostalCodesDict;
                EffectTypes = GlobalViewModel.Instance.HispaniaViewModel.EffectTypesDict;
                SendTypes = GlobalViewModel.Instance.HispaniaViewModel.SendTypesDict;
                Agents = GlobalViewModel.Instance.HispaniaViewModel.AgentsDict;
                IVATypes = GlobalViewModel.Instance.HispaniaViewModel.IVATypesDict;
                DataList = GlobalViewModel.Instance.HispaniaViewModel.Providers;
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        private void ActualizeProviderFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                ProvidersView SelectedItem = (ProvidersView)ListItems.SelectedItem;
                ProvidersView ItemInDb = GlobalViewModel.Instance.HispaniaViewModel.GetProviderFromDb(SelectedItem);
                int Index = ListItems.SelectedIndex;
                ListItems.UnselectAll();
                DataList.Remove(SelectedItem);
                DataList.Insert(Index, ItemInDb);
                ListItems.SelectedItem = ItemInDb;
                ProviderDataControl.Provider = ItemInDb;

                ProviderDataControl.DataListDefinedProviders = DataList;
                GlobalViewModel.Instance.HispaniaViewModel.RefreshRelatedProviders(ItemInDb.Provider_Id);
                ObservableCollection<ProvidersView> RelatedProviders = new ObservableCollection<ProvidersView>();
                foreach (RelatedProvidersView relatedProvider in GlobalViewModel.Instance.HispaniaViewModel.RelatedProviders)
                {
                    ProvidersView provider = GlobalViewModel.Instance.HispaniaViewModel.GetProviderFromDb(relatedProvider.Provider_Canceled_Id);
                    ProviderDataControl.DataListDefinedProviders.Remove(provider);
                    RelatedProviders.Add(provider);
                }
                ProviderDataControl.DataListDefinedProviders = RelatedProviders;

                ListItems.UpdateLayout();
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        private void ActualizeProvidersFromDb(out List<ProvidersView> ItemsInDb)
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                ItemsInDb = new List<ProvidersView>(ListItems.SelectedItems.Count);
                foreach (ProvidersView provider in new ArrayList(ListItems.SelectedItems))
                {
                    ProvidersView providerInDb = GlobalViewModel.Instance.HispaniaViewModel.GetProviderFromDb(provider);
                    ItemsInDb.Add(providerInDb);
                    int Index = DataList.IndexOf(provider);
                    ListItems.SelectedItems.Remove(provider);
                    DataList.Remove(provider);
                    DataList.Insert(Index, providerInDb);
                    ListItems.SelectedItems.Add(providerInDb);
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
            if (m_DataList.Count > 0)
            {
                cdAdd.Width = Print ? HideComponent : ViewReportButtonPannel;
                cdReport.Width = Print ? ViewReportButtonPannel : HideComponent;
                cdComandesCompletes.Width = Print ? ViewReportButtonPannel2 : HideComponent;
                bool IsEditionButtonVisibles = !Print && ListItems.SelectedItem != null;
                btnEdit.Visibility = btnDelete.Visibility = btnViewData.Visibility = IsEditionButtonVisibles ? Visibility.Visible : Visibility.Hidden;
                rdSearchPannel.Height = ViewSearchPannel;
            }
            else
            {
                cdReport.Width = HideComponent;
                cdComandesCompletes.Width = HideComponent;
                rdSearchPannel.Height = HideComponent;
                btnEdit.Visibility = btnDelete.Visibility = btnViewData.Visibility = Visibility.Hidden;
            }
            GridList.IsEnabled = true;
        }

        /// <summary>
        /// Method that actualize the Button Bar look
        /// </summary>
        private void ActualizeButtonBar()
        {
            if (!Print)
            {
                btnEdit.Visibility = btnDelete.Visibility = btnViewData.Visibility = ListItems.SelectedItem is null ? Visibility.Hidden : Visibility.Visible;
            }
        }

        #endregion
    }
}
