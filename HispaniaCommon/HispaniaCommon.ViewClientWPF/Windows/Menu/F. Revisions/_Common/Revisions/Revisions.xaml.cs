#region Librerias usadas por la ventana

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
    /// Interaction logic for MainWindowOp1.xaml
    /// </summary>
    public partial class Revisions : Window
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
        private ObservableCollection<RevisionsView> m_DataList = new ObservableCollection<RevisionsView>();

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
        /// Get or Set Revision Type for manage the Window
        /// </summary>
        public RevisionsType RevisionType
        {
            get;
            private set;
        }

        /// <summary>
        /// Get or Set the data to show in List of Items.
        /// </summary>
        public ObservableCollection<RevisionsView> DataList
        {
            get
            {
                return (m_DataList);
            }
            set
            {
                if (value != null) m_DataList = value;
                else m_DataList = new ObservableCollection<RevisionsView>();
                ListItems.ItemsSource = m_DataList;
                ListItems.DataContext = this;
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Filter = UserFilter;
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).SortDescriptions.Add(new SortDescription("GoodCode", ListSortDirection.Ascending));
                if (m_DataList.Count > 0) rdSearchPannel.Height = ViewSearchPannel;
                else rdSearchPannel.Height = HideComponent;
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
        /// <param name="RevisionType">Defines the type of revision for the Window</param>
        public Revisions(ApplicationType AppType, RevisionsType RevisionType)
        {
            InitializeComponent();
            Initialize(AppType, RevisionType);
        }

        /// <summary>
        /// Method that initialize the window.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        private void Initialize(ApplicationType AppType, RevisionsType RevisionType)
        {
            //  Actualize the Window.
                ActualizeWindowComponents(AppType, RevisionType);
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
        /// <param name="RevisionType">Defines the type of revision for the Window</param>
        private void ActualizeWindowComponents(ApplicationType AppType, RevisionsType RevisionType)
        {
            //  Actualize properties of this Window.
                this.AppType = AppType;
                this.RevisionType = RevisionType;
                switch (RevisionType)
                {
                    case RevisionsType.MismatchesAvailable:
                         Title += "Estocs Disponibles (desquadres)";
                         btnSolveError.Visibility = Visibility.Visible;
                         break;
                    case RevisionsType.MismatchesStocks:
                         Title += "Estocs Existències (desquadres)";
                         btnSolveError.Visibility = Visibility.Visible;
                         break;
                    case RevisionsType.GeneralAvailable:
                         Title += "Estocs Disponibles";
                         btnSolveError.Visibility = Visibility.Hidden;
                         break;
                    case RevisionsType.GeneralStocks:
                         Title += "Estocs Existències";
                         btnSolveError.Visibility = Visibility.Hidden;
                         break;
                }
            //  Apply Theme to window.
                ThemeManager.ActualTheme = AppTheme;
            //  Initialize state of Window components.
                rdSearchPannel.Height = HideComponent;
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
                cbFieldItemToSearch.ItemsSource = RevisionsView.Fields;
                cbFieldItemToSearch.DisplayMemberPath = "Key";
                cbFieldItemToSearch.SelectedValuePath = "Value";
                if (GoodsView.Fields.Count > 0) cbFieldItemToSearch.SelectedIndex = 0;
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
                RevisionsView ItemData = (RevisionsView)item;
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
                return true;
        }

        #endregion

        #region Managers

        /// <summary>
        /// Method that define the managers needed for the user operations in the Window
        /// </summary>
        private void LoadManagers()
        {
            //  TextBox
                tbItemToSearch.TextChanged += TbItemToSearch_TextChanged;
            //  Button Search Clients
                btnExit.Click += BtnExit_Click;
                btnReport.Click += BtnReport_Click;
                btnSolveError.Click += BtnSolveError_Click;
                btnRefresh.Click += BtnRefresh_Click;
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
                ActualizeRevisionsFromDb();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al refrescar les Revisions.", MsgManager.ExcepMsg(ex)));
            }
        }

        #endregion

        #region Create Report

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
                    List<RevisionsView> NowadaysRevisions = new List<RevisionsView>(DataList);
                    RevisionsReportView.CreateReport(NowadaysRevisions, RevisionType);
                }
                catch (Exception ex)
                {
                    string ErrBase = "Error, al construïr l'informe associat als {0}.\r\nDetalls: {1}";
                    switch (RevisionType)
                    {
                        case RevisionsType.MismatchesAvailable:
                             MsgManager.ShowMessage(string.Format(ErrBase, "Estocs Disponibles (desquadres)", MsgManager.ExcepMsg(ex)));
                             break;
                        case RevisionsType.MismatchesStocks:
                             MsgManager.ShowMessage(string.Format(ErrBase, "Estocs Existències (desquadres)", MsgManager.ExcepMsg(ex)));
                             break;
                        case RevisionsType.GeneralAvailable:
                             MsgManager.ShowMessage(string.Format(ErrBase, "Estocs Disponibles", MsgManager.ExcepMsg(ex)));
                             break;
                        case RevisionsType.GeneralStocks:
                             MsgManager.ShowMessage(string.Format(ErrBase, "Estocs Existències", MsgManager.ExcepMsg(ex)));
                             break;
                    }
                }
            }
        }
        
        #endregion

        #region Solver Error

        /// <summary>
        /// Manage the solution of the mismatche in the Good selected.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnSolveError_Click(object sender, RoutedEventArgs e)
        {
            if (ListItems.SelectedItems != null)
            {
                foreach (RevisionsView Revision in ListItems.SelectedItems)
                {
                    try
                    {
                        GoodsView Good = GlobalViewModel.Instance.HispaniaViewModel.GetGoodFromDb(Revision);
                        switch (RevisionType)
                        {
                            case RevisionsType.MismatchesAvailable:
                                 Good.Shipping_Unit_Available = Revision.StockExpectedUE;
                                 Good.Billing_Unit_Available = Revision.StockExpectedUF;
                                 Good.Shipping_Unit_Entrance = Revision.EntryUE;
                                 Good.Shipping_Unit_Departure = Revision.OutputUE;
                                 Good.Billing_Unit_Entrance = Revision.EntryUF;
                                 Good.Billing_Unit_Departure = Revision.OutputUF;
                                 GlobalViewModel.Instance.HispaniaViewModel.UpdateGood(Good);
                                 break;
                            case RevisionsType.MismatchesStocks:
                                 Good.Shipping_Unit_Stocks = Revision.StockExpectedUE;
                                 Good.Billing_Unit_Stocks = Revision.StockExpectedUF;
                                 Good.Shipping_Unit_Entrance = Revision.EntryUE;
                                 Good.Shipping_Unit_Departure = Revision.OutputUE;
                                 Good.Billing_Unit_Entrance = Revision.EntryUF;
                                 Good.Billing_Unit_Departure = Revision.OutputUF;
                                 GlobalViewModel.Instance.HispaniaViewModel.UpdateGood(Good);
                                 break;
                        }
                    }
                    catch (Exception ex)
                    {
                        string ErrBase = "Error, al solucionar el desquadre de l'Estoc {0} de l'artícle {1}.\r\nDetalls: {2}";
                        switch (RevisionType)
                        {
                            case RevisionsType.MismatchesAvailable:
                                 MsgManager.ShowMessage(string.Format(ErrBase, "Disponible", Revision.GoodCode, MsgManager.ExcepMsg(ex)));
                                 break;
                            case RevisionsType.MismatchesStocks:
                                 MsgManager.ShowMessage(string.Format(ErrBase, "Existència", Revision.GoodCode, MsgManager.ExcepMsg(ex)));
                                 break;
                        }
                    }
                }
                ListItems.SelectedItems.Clear();
                ActualizeRevisionsFromDb();
            }
        }
        
        #endregion

        #region Exit

        /// <summary>
        /// Manage the Button Mouse Click in one of the items of the List to show its data in User Control.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        #endregion

        #endregion

        #region Database Operations

        private void ActualizeRevisionsFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                switch (RevisionType)
                {
                    case RevisionsType.MismatchesAvailable:
                         RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.MismatchesAvailableWindow);
                         DataList = GlobalViewModel.Instance.HispaniaViewModel.Revisions;
                         break;
                    case RevisionsType.MismatchesStocks:
                         RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.MismatchesStocksWindow);
                         DataList = GlobalViewModel.Instance.HispaniaViewModel.Revisions;
                         break;
                    case RevisionsType.GeneralAvailable:
                         RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.GeneralAvailableWindow);
                         DataList = GlobalViewModel.Instance.HispaniaViewModel.Revisions;
                         break;
                    case RevisionsType.GeneralStocks:
                         RefreshDataViewModel.Instance.RefreshData(WindowToRefresh.GeneralStocksWindow);
                         DataList = GlobalViewModel.Instance.HispaniaViewModel.Revisions;
                         break;
                }
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

        #endregion
    }
}
