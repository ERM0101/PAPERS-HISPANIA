#region Librerias usadas por la ventana

using HispaniaCommon.ViewModel;
using MBCode.Framework.Managers;
using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.Theme;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
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
    public partial class GoodInputsOutputs : Window
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
        private GoodsView m_Data = null;

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        private ObservableCollection<InputsOutputsView> m_DataList = new ObservableCollection<InputsOutputsView>();

        #region Directories and Files Management

        private static string _GetApplicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private static string _GetQueryDirectory = string.Format("{0}\\EntradesSortides\\", _GetApplicationPath);

        #endregion

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
        public GoodsView Data
        {
            get
            {
                return (m_Data);
            }
            set
            {
                if (value != null)
                {
                    m_Data = value;
                    LoadDataInWindow(m_Data);
                }
                else throw new ArgumentNullException("Error, no s'ha trobat l'Article a carregar.");
            }
        }

        /// <summary>
        /// Get or Set the data to show in List of Items.
        /// </summary>
        public ObservableCollection<InputsOutputsView> DataList
        {
            get
            {
                return (m_DataList);
            }
            set
            {
                if (value != null) m_DataList = value;
                else m_DataList = new ObservableCollection<InputsOutputsView>();
                ListItems.ItemsSource = m_DataList;
                ListItems.DataContext = this;
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Filter = UserFilter;
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).SortDescriptions.Add(new SortDescription("IO_Date", ListSortDirection.Descending));
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

        #region Directories and Files Management

        private static string GetQueryDirectory
        {
            get
            {
                if (!Directory.Exists(_GetQueryDirectory)) Directory.CreateDirectory(_GetQueryDirectory);
                return (_GetQueryDirectory);
            }
            set
            {
                if (!Directory.Exists(value)) Directory.CreateDirectory(value);
                _GetQueryDirectory = value;
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
        public GoodInputsOutputs(ApplicationType AppType)
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
                cbFieldItemToSearch.ItemsSource = InputsOutputsView.Fields;
                cbFieldItemToSearch.DisplayMemberPath = "Key";
                cbFieldItemToSearch.SelectedValuePath = "Value";
                if (GoodsView.Fields.Count > 0) cbFieldItemToSearch.SelectedIndex = 0;
                UpdateUI();
            //  Deactivate managers
                DataChangedManagerActive = true;
        }

        /// <summary>
        /// Load the data of the Company into the Window
        /// </summary>
        private void LoadDataInWindow(GoodsView goodView)
        {
            if (goodView == null) return;
            tbGoodCode.Text = goodView.Good_Code;
            tbGoodDescription.Text = goodView.Good_Description;
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
                InputsOutputsView ItemData = (InputsOutputsView)item;
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
                object valueTypeToTest = ItemData.GetType().GetProperty("IO_Type").GetValue(ItemData);
                if (valueTypeToTest is null)
                {
                    return (false);
                }
                string Type = valueTypeToTest.ToString().ToUpper();
            //  Calculate the Visibility value with properties values.
                return ((chkbEntry.IsChecked == true) && (chkbOutput.IsChecked == true)) ||
                       ((chkbEntry.IsChecked == true) && (Type.Contains("ENTRADA"))) ||
                       ((chkbOutput.IsChecked == true) && (Type.Contains("SORTIDA")));
        }

        #endregion

        #region Managers

        /// <summary>
        /// Method that define the managers needed for the user operations in the Window
        /// </summary>
        private void LoadManagers()
        {
            //  By default the manager for the Customer Data changes is active.
                DataChangedManagerActive = true;
            //  TextBox
                tbItemToSearch.TextChanged += TbItemToSearch_TextChanged;
            //  CheckBox
                chkbEntry.Checked += ChkbEntry_Checked;
                chkbEntry.Unchecked += ChkbEntry_Unchecked;
                chkbOutput.Checked += ChkbOutput_Checked;
                chkbOutput.Unchecked += ChkbOutput_Unchecked;
            //  Buttons
                btnCreateExcel.Click += BtnCreateExcel_Click;
                btnExit.Click += BtnExit_Click;
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

        private void ChkbEntry_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateUI();
            FilterDataListObjects();
        }

        private void ChkbEntry_Checked(object sender, RoutedEventArgs e)
        {
            UpdateUI();
            FilterDataListObjects();
        }

        private void ChkbOutput_Unchecked(object sender, RoutedEventArgs e)
        {
            UpdateUI();
            FilterDataListObjects();
        }

        private void ChkbOutput_Checked(object sender, RoutedEventArgs e)
        {
            UpdateUI();
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
                ActualizeGoodsInputsOutputsFromDb();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al refrescar els valors de les Entrades/sortides.", MsgManager.ExcepMsg(ex)));
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
            Close();
        }

        #endregion

        #region Create Excel

        /// <summary>
        /// Manage the Button Mouse Click in the button associated at the creatrion of Excel with items data list.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnCreateExcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime Now = DateTime.Now;
                string ExcelFileName = string.Format("{0}{1}-{2:0000}_{3:00}_{4:00}-{5:00}_{6:00}.xlsx",
                                                     GetQueryDirectory,
                                                     "Entrades_Sortides",
                                                     Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute);
                DataTable Table = new DataTable();
                Table.Columns.Add("DATA", typeof(DateTime));
                Table.Columns.Add("TIPUS", typeof(string));
                Table.Columns.Add("SITUACIÓ", typeof(string));
                Table.Columns.Add("UNITATS_EXPEDICIÓ", typeof(decimal));
                Table.Columns.Add("UNITATS_FACTURACIÓ", typeof(decimal));
                Table.Columns.Add("PREU", typeof(string));
                Table.Columns.Add("NUMERO_ALBARÀ", typeof(string));
                Table.Columns.Add("ANY_ALBARÀ", typeof(string));
                Table.Columns.Add("NUMERO_FACTURA", typeof(string));
                Table.Columns.Add("ANY_FACTURA", typeof(string));
                Table.Columns.Add("SÈRIE_DE_FACTURACIÓ", typeof(string));
                Table.Columns.Add("NUMERO_CLIENT", typeof(string));
                Table.Columns.Add("CLIENT", typeof(string));
                Table.Columns.Add( "NUMERO_PROVEDOR", typeof( string ) );
                Table.Columns.Add( "PROVEDOR", typeof( string ) );
                foreach (InputsOutputsView inputOutput in CollectionViewSource.GetDefaultView(ListItems.ItemsSource))
                {
                    DataRow Row = Table.NewRow();
                    Row["DATA"] = inputOutput.IO_Date;
                    Row["TIPUS"] = inputOutput.IO_Type;
                    Row["SITUACIÓ"] = inputOutput.IO_State_Str;
                    Row["UNITATS_EXPEDICIÓ"] = inputOutput.Amount_Shipping;
                    Row["UNITATS_FACTURACIÓ"] = inputOutput.Amount_Billing;
                    Row["PREU"] = inputOutput.Price_Str;
                    Row["NUMERO_ALBARÀ"] = inputOutput.DeliveryNote_Id_Str;
                    Row["ANY_ALBARÀ"] = inputOutput.DeliveryNote_Year_Str;
                    Row["NUMERO_FACTURA"] = inputOutput.Bill_Id_Str;
                    Row["ANY_FACTURA"] = inputOutput.Bill_Year_Str;
                    Row["SÈRIE_DE_FACTURACIÓ"] = inputOutput.Bill_Serie_Str;
                    Row["NUMERO_CLIENT"] = inputOutput.Customer_Id_Str;
                    Row["CLIENT"] = inputOutput.Customer_Alias;
                    Row["NUMERO_PROVEDOR"] = inputOutput.Provider_Id_Str;
                    Row[ "PROVEDOR" ] = inputOutput.Provider;

                    Table.Rows.Add(Row);
                }
                ExcelManager.ExportToExcel(Table, "ENTRADES_SORTIDES", ExcelFileName);
                Process.Start(ExcelFileName);
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(
                    string.Format("Error, al crear l'Excel de les Entrades i les Sortides.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
            }
        }

        #endregion

        #endregion

        #endregion

        #region Database Operations

        private void ActualizeGoodsInputsOutputsFromDb()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Actualize Item Information From DataBase
                GlobalViewModel.Instance.HispaniaViewModel.RefreshInputsOutputs(Data.Good_Id);
                DataList = GlobalViewModel.Instance.HispaniaViewModel.InputsOutputs;
            //  Deactivate managers
                DataChangedManagerActive = true;
        }


        #endregion

        #region Update UI

        private void UpdateUI()
        {
            if ((chkbEntry.IsChecked == true) && (chkbOutput.IsChecked == true)) 
            {
                Title = "Moviments (Entrades/Sortides)";
                ListItems.View = (GridView) FindResource("GridViewForOutputs");
                lblAcumAmountBilling.Visibility = tbAcumAmountBilling.Visibility = Visibility.Collapsed;
                lblAcumAmountShipping.Visibility = tbAcumAmountShipping.Visibility = Visibility.Collapsed;
            }
            else if ((chkbEntry.IsChecked == true) && (chkbOutput.IsChecked == false))
            {
                Title = "Entrades";
                ListItems.View = (GridView) FindResource("GridViewForEntries");
                lblAcumAmountBilling.Visibility = tbAcumAmountBilling.Visibility = Visibility.Visible;
                lblAcumAmountShipping.Visibility = tbAcumAmountShipping.Visibility = Visibility.Visible;
            }
            else if ((chkbEntry.IsChecked == false) && (chkbOutput.IsChecked == true))
            {
                Title = "Sortides";
                ListItems.View = (GridView) FindResource("GridViewForOutputs");
                lblAcumAmountBilling.Visibility = tbAcumAmountBilling.Visibility = Visibility.Visible;
                lblAcumAmountShipping.Visibility = tbAcumAmountShipping.Visibility = Visibility.Visible;
            }
            else
            {
                Title = "Moviments (Entrades/Sortides)";
                ListItems.View = (GridView) FindResource("GridViewForEntries");
                lblAcumAmountBilling.Visibility = tbAcumAmountBilling.Visibility = Visibility.Collapsed;
                lblAcumAmountShipping.Visibility = tbAcumAmountShipping.Visibility = Visibility.Collapsed;
            }
        }

        private void FilterDataListObjects()
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                CollectionViewSource.GetDefaultView(ListItems.ItemsSource).Refresh();
                bool UpdateAcumulates = ((chkbEntry.IsChecked == true) && (chkbOutput.IsChecked == false)) ||
                                        ((chkbEntry.IsChecked == false) && (chkbOutput.IsChecked == true));
                if (UpdateAcumulates)
                {
                    decimal AmountBilling = 0;
                    decimal AmountShipping = 0;
                    foreach (InputsOutputsView InputOutput in (CollectionView)CollectionViewSource.GetDefaultView(ListItems.ItemsSource))
                    {
                        AmountShipping += InputOutput.Amount_Shipping;
                        AmountBilling += InputOutput.Amount_Billing;
                    }
                    tbAcumAmountShipping.Text = AmountShipping.ToString();
                    tbAcumAmountBilling.Text = AmountBilling.ToString();
                }
                DataChangedManagerActive = true;
            }
        }

        #endregion
    }
}
