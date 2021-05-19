#region Librerias usadas por la ventana

using HispaniaCommon.ViewModel;
using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.Theme;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Data;

#endregion

namespace HispaniaCommon.ViewClientWPF.Windows
{
    /// <summary>
    /// Interaction logic for Parameters.xaml
    /// </summary>
    public partial class StockTakingsList : Window
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
        /// Store Goods 
        /// </summary>
        private Dictionary<string, GoodsView> m_Goods;

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
        public StockTakingsList(ApplicationType AppType)
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

        #endregion

        #region Managers

        /// <summary>
        /// Method that define the managers needed for the user operations in the Window
        /// </summary>
        private void LoadManagers()
        {
            //  By default the manager for the Customer Data changes is active.
                DataChangedManagerActive = true;
            //  Foreign Tables Managers
                cbGood_Code_From.SelectionChanged += CbGood_Code_From_SelectionChanged;
            //  Button managers
                btnPrint.Click += BtnPrint_Click;
                btnExit.Click += BtnExit_Click;
        }

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
                string Good_Code_From = cbGood_Code_From.SelectedItem is null ?
                                        null :
                                        ((KeyValuePair<string, GoodsView>)cbGood_Code_From.SelectedItem).Value.Good_Code;
                string Good_Code_Until = cbGood_Code_From.SelectedItem is null ?
                                         null :
                                         ((KeyValuePair<string, GoodsView>)cbGood_Code_Until.SelectedItem).Value.Good_Code;
                if ((Good_Code_From != null) && (Good_Code_Until is null))
                {
                    Good_Code_Until = Good_Code_From;
                }
                GlobalViewModel.Instance.HispaniaViewModel.RefreshStockTakings(Good_Code_From, Good_Code_Until);
                StockTakingsReportView.CreateReport(GlobalViewModel.Instance.HispaniaViewModel.StockTakingsDict);
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al generar l'informe.\r\nDetalls: {0}", MsgManager.ExcepMsg(ex)));
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
        /// Manage the change of the Data in the combobox of CP.
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

        #endregion

        #endregion
    }
}
