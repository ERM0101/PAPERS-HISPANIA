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
    /// Interaction logic for Margins.xaml
    /// </summary>
    public partial class RangesList : Window
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
        /// Show the button.
        /// </summary>
        private GridLength ViewButton = new GridLength(120.0);

        /// <summary>
        /// Show the button.
        /// </summary>
        private GridLength ViewExtendedColumn = new GridLength(1.0, GridUnitType.Star);

        /// <summary>
        /// Hide the button.
        /// </summary>
        private GridLength HideColumn = new GridLength(0.0);

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

        #region Attributes

        /// <summary>
        /// Store the type of Application with the user want open.
        /// </summary>
        private ApplicationType m_AppType;

        /// <summary>
        /// Store Months 
        /// </summary>
        private Dictionary<string, string> m_Months;

        /// <summary>
        /// Store Goods 
        /// </summary>
        private Dictionary<string, GoodsView> m_Goods;

        /// <summary>
        /// Store Bills 
        /// </summary>
        private Dictionary<string, BillsView> m_Bills;

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
        /// Get or Set the Months
        /// </summary>
        private Dictionary<string, string> Months
        {
            get
            {
                return (m_Months);
            }
            set
            {

                if (value is null) m_Months = new Dictionary<string, string>();
                else m_Months = value;
                DataChangedManagerActive = false;
                cbMonth.ItemsSource = m_Months;
                cbMonth.DisplayMemberPath = "Key";
                cbMonth.SelectedValuePath = "Value";
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Get or Set the Goods 
        /// </summary>
        public Dictionary<string, GoodsView> Goods
        {
            get
            {
                return (m_Goods);
            }
            set
            {

                if (value is null) m_Goods = new Dictionary<string, GoodsView>();
                else m_Goods = value;
                btnPrint.IsEnabled = m_Goods.Count > 0;
                DataChangedManagerActive = false;
                cbGood_Code_From.ItemsSource = new SortedDictionary<string, GoodsView>(m_Goods);
                cbGood_Code_From.DisplayMemberPath = "Key";
                cbGood_Code_From.SelectedValuePath = "Value";
                cbGood_Code_Until.ItemsSource = new SortedDictionary<string, GoodsView>(m_Goods);
                cbGood_Code_Until.DisplayMemberPath = "Key";
                cbGood_Code_Until.SelectedValuePath = "Value";
                CollectionViewSource.GetDefaultView(cbGood_Code_Until.ItemsSource).Filter = UserFilter;
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Get or Set the Bills 
        /// </summary>
        //public Dictionary<string, BillsView> Bills
        public Dictionary<string, BillsView> Bills
        {
            get
            {
                return (m_Bills);
            }
            set
            {
                if (value is null) m_Bills = new Dictionary<string, BillsView>();
                else m_Bills = value;
                btnPrint.IsEnabled = m_Bills.Count > 0;
                DataChangedManagerActive = false;
                cbBill_Id_From.ItemsSource = new SortedDictionary<string, BillsView>(m_Bills);
                cbBill_Id_From.DisplayMemberPath = "Key";
                cbBill_Id_From.SelectedValuePath = "Value";
                CollectionViewSource.GetDefaultView(cbBill_Id_From.ItemsSource).SortDescriptions.Add(new SortDescription("Key", ListSortDirection.Ascending));
                cbBill_Id_Until.ItemsSource = new SortedDictionary<string, BillsView>(m_Bills);
                cbBill_Id_Until.DisplayMemberPath = "Key";
                cbBill_Id_Until.SelectedValuePath = "Value";
                CollectionViewSource.GetDefaultView(cbBill_Id_Until.ItemsSource).Filter = BillUserFilter;
                DataChangedManagerActive = true;
                if (cbBill_Id_From.Items.Count > 0) cbBill_Id_From.SelectedIndex = 0;
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
        public RangesList(ApplicationType AppType)
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
        }

        /// <summary>
        /// Method that actualize the UserControl components.
        /// </summary>
        /// <param name="AppType">Defines the type of Application with the user want open.</param>
        private void ActualizeUserControlComponents()
        {
            //  Apply Theme to UserControl.
                ThemeManager.ActualTheme = AppTheme;
            //  Initialize Month Combo Box
                Months = new Dictionary<string, string>
                {
                    { "Gener", "1" },
                    { "Febrer", "2" },
                    { "Març", "3" },
                    { "Abril", "4" },
                    { "Maig", "5" },
                    { "Juny", "6" },
                    { "Juliol", "7" },
                    { "Agost", "8" },
                    { "Setembre", "9" },
                    { "Octubre", "10" },
                    { "Novembre", "11" },
                    { "Desembre", "12" },
                };
            //  Initialize CheckBox Content (Interannual Modification)
                ckbYearToFilter.Content = String.Format(" VEURE LES DADES DE L'ANY {0}", DateTime.Now.Year - 1);
        }

        /// <summary>
        /// Method that filter the elements that are showing in the list
        /// </summary>
        /// <param name="item">Item to test</param>
        /// <returns>true, if the item must be loaded, false, if not.</returns>
        private bool UserFilter(object item)
        {
            //  Determine if is needed aplicate one filter.
                if (cbGood_Code_From.SelectedIndex == -1) return (false);
            //  Get Good selected in From combo.
                GoodsView Good_Code_From = (GoodsView)cbGood_Code_From.SelectedValue;
            //  Get Acces to the object and the property name To Filter.
                GoodsView ItemData = ((KeyValuePair<string, GoodsView>)item).Value;
            //  Apply the filter by selected field value
                int Compare = GlobalViewModel.CompareStringValues(ItemData.Good_Code, Good_Code_From.Good_Code);
                return (Compare == 1) || (Compare == 0);
        }

        /// <summary>
        /// Method that filter the elements that are showing in the list
        /// </summary>
        /// <param name="item">Item to test</param>
        /// <returns>true, if the item must be loaded, false, if not.</returns>
        private bool BillUserFilter(object item)
        {
            //  Determine if is needed aplicate one filter.
                if (cbBill_Id_From.SelectedIndex == -1) return (false);
            //  Get Good selected in From combo.
                BillsView Bill_Id_From = (BillsView)cbBill_Id_From.SelectedValue;
            //  Get Acces to the object and the property name To Filter.
                BillsView ItemData = ((KeyValuePair<string, BillsView>)item).Value;
            //  Apply the filter by selected field value
                return (ItemData.Bill_Id >= Bill_Id_From.Bill_Id);
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
            //  ComboBox
                cbGood_Code_From.SelectionChanged += CbGood_Code_From_SelectionChanged;
                cbBill_Id_From.SelectionChanged += CbBill_Id_From_SelectionChanged;
            //  CheckBox (Interannual Modification)
                ckbYearToFilter.Checked += CkbYearToFilter_Checked;
            //  Button managers
                btnPrint.Click += BtnPrint_Click;
                btnExit.Click += BtnExit_Click;
        }

        #region CheckBox

        /// <summary>
        /// Manage the action to Check or Uncheck the check button. (Interannual Modification)
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CkbYearToFilter_Checked(object sender, RoutedEventArgs e)
        {
            DataChangedManagerActive = false;
            try
            {
                Decimal? Year = ckbYearToFilter.IsChecked == true ? DateTime.Now.Year - 1 : DateTime.Now.Year;
                GlobalViewModel.Instance.HispaniaViewModel.RefreshBills(Year);
                Bills = GlobalViewModel.Instance.HispaniaViewModel.BillsDict;
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(String.Format("Error, al accedir a les dades de l'any anterior.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
            }
            finally
            {
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #region Button

        #region Print Report

        /// <summary>
        /// Manage the button for print report with selected items.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string Good_Code_From = cbGood_Code_From.SelectedItem is null ? null
                                                                              : ((KeyValuePair<string, GoodsView>)cbGood_Code_From.SelectedItem).Value.Good_Code;
                string Good_Code_Until = cbGood_Code_From.SelectedItem is null ? null
                                                                               : ((KeyValuePair<string, GoodsView>)cbGood_Code_Until.SelectedItem).Value.Good_Code;
                if ((Good_Code_From != null) && (Good_Code_Until is null))
                {
                    Good_Code_Until = Good_Code_From;
                }
                string Bill_Id_From = cbBill_Id_From.SelectedItem is null ? null
                                                                          : ((BillsView)cbBill_Id_From.SelectedValue).Bill_Id.ToString();
                string Bill_Id_Until = cbBill_Id_From.SelectedItem is null ? null
                                                                           : ((BillsView)cbBill_Id_Until.SelectedValue).Bill_Id.ToString();
                if ((Bill_Id_From != null) && (Bill_Id_Until is null))
                {
                    Bill_Id_Until = Bill_Id_From;
                }
                string MonthValue = cbMonth.SelectedItem is null ?
                                    null :
                                    ((KeyValuePair<string, string>)cbMonth.SelectedItem).Key.ToUpper();
                int? MonthNumber = cbMonth.SelectedValue is null ? null : (int?)int.Parse(cbMonth.SelectedValue.ToString());
                decimal YearQuery = ckbYearToFilter.IsChecked == true ? DateTime.Now.Year - 1 : DateTime.Now.Year;
                GlobalViewModel.Instance.HispaniaViewModel.RefreshRanges(Good_Code_From, Good_Code_Until, Bill_Id_From, Bill_Id_Until, YearQuery);
                RangesReportView.CreateReport(GlobalViewModel.Instance.HispaniaViewModel.RangesDict, MonthValue, MonthNumber);
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al generar l'informe.\r\nDetalls: {0}\r\n.", MsgManager.ExcepMsg(ex)));
            }
        }

        #endregion

        #region Exit

        /// <summary>
        /// Manage the button for close the window.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion

        #endregion

        #region ComboBox

        /// <summary>
        /// Manage the change of the Data in the combobox of Good.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CbGood_Code_From_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                CollectionViewSource.GetDefaultView(cbGood_Code_Until.ItemsSource).Refresh();
                if (cbGood_Code_Until.Items.Count > 0) cbGood_Code_Until.SelectedIndex = 0;
                DataChangedManagerActive = true;
            }
        }

        /// <summary>
        /// Manage the change of the Data in the combobox of Bill.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void CbBill_Id_From_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (DataChangedManagerActive)
            {
                DataChangedManagerActive = false;
                CollectionViewSource.GetDefaultView(cbBill_Id_Until.ItemsSource).Refresh();
                if (cbBill_Id_Until.Items.Count > 0) cbBill_Id_Until.SelectedIndex = 0;
                DataChangedManagerActive = true;
            }
        }

        #endregion

        #endregion
    }
}
