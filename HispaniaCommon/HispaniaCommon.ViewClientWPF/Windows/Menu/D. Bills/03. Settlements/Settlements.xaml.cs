#region Librerias usadas por la ventana

using HispaniaCommon.ViewModel;
using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.Theme;
using System;
using System.Collections.Generic;
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
    public partial class Settlements : Window
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
        /// Store Bills 
        /// </summary>
        private Dictionary<string, BillsView> m_Bills;

        /// <summary>
        /// Store Agents 
        /// </summary>
        private Dictionary<string, AgentsView> m_Agents;

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
        /// Get or Set the Agents 
        /// </summary>
        public Dictionary<string, AgentsView> Agents
        {
            get
            {
                return (m_Agents);
            }
            set
            {

                if (value is null) m_Agents = new Dictionary<string, AgentsView>();
                else m_Agents = value;
                DataChangedManagerActive = false;
                cbAgent.ItemsSource = new SortedDictionary<string, AgentsView>(m_Agents);
                cbAgent.DisplayMemberPath = "Key";
                cbAgent.SelectedValuePath = "Value";
                DataChangedManagerActive = true;
                UpdateButtonState();
            }
        }

        /// <summary>
        /// Get or Set the Bills 
        /// </summary>
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
                DataChangedManagerActive = false;
                cbBill_Id_From.ItemsSource = new SortedDictionary<string, BillsView>(m_Bills);
                cbBill_Id_From.DisplayMemberPath = "Key";
                cbBill_Id_From.SelectedValuePath = "Value";
                cbBill_Id_Until.ItemsSource = new SortedDictionary<string, BillsView>(m_Bills);
                cbBill_Id_Until.DisplayMemberPath = "Key";
                cbBill_Id_Until.SelectedValuePath = "Value";
                CollectionViewSource.GetDefaultView(cbBill_Id_Until.ItemsSource).Filter = BillUserFilter;
                DataChangedManagerActive = true;
                UpdateButtonState();
                LoadDataInControls();                
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
        public Settlements(ApplicationType AppType)
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
            //  Initialize CheckBox Content (Interannual Modification)
                ckbYearToFilter.Content = String.Format(" VEURE LES DADES DE L'ANY {0}", DateTime.Now.Year - 1);
        }

        /// <summary>
        /// Method that loads the Data in the controls of the Window
        /// </summary>
        private void LoadDataInControls()
        {
            //  Deactivate managers
                DataChangedManagerActive = false;
            //  Settlement Controls
                //Bill_Id = GlobalViewModel.Instance.HispaniaViewModel.GetLastBillSetteled() { si calgués fer servir aquest identificador Carles diu que no cal  }
                //GetKeyBillView(int Bill_Id, decimal Year) { Seleccionariem l'element m,itjançant la clau, igual millor modificar el SP_LastBillSetteled }
                if (cbBill_Id_From.Items.Count > 0) cbBill_Id_From.SelectedIndex = 0;
                CollectionViewSource.GetDefaultView(cbBill_Id_Until.ItemsSource).Refresh();
            //  Activate managers
                DataChangedManagerActive = true;
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
                cbBill_Id_From.SelectionChanged += CbBill_Id_From_SelectionChanged;
            //  CheckBox (Interannual Modification)
                ckbYearToFilter.Checked += CkbYearToFilter_Checked;
            //  Button managers
                btnPrint.Click += BtnPrint_Click;
                btnExit.Click += BtnExit_Click;
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

        #endregion

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

        #region Buttons

        #region Print Report

        /// <summary>
        /// Manage the button for print report with selected items.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            if (cbBill_Id_From.SelectedValue is null)
            {
                MsgManager.ShowMessage("Error, cal seleccionar la factura d'inici amb la que seleccionar l'informe.");
            }
            else if (cbBill_Id_Until.SelectedValue is null)
            {
                MsgManager.ShowMessage("Error, cal seleccionar la factura de fi amb la que seleccionar l'informe.");
            }
            else
            {
                string Bill_Id_From = ((BillsView)cbBill_Id_From.SelectedValue).Bill_Id.ToString();
                string Bill_Id_Until = ((BillsView)cbBill_Id_Until.SelectedValue).Bill_Id.ToString();
                int? Agent_Id = cbAgent.SelectedValue is null ? null : (int?) ((AgentsView)cbAgent.SelectedValue).Agent_Id;
                decimal YearQuery = ckbYearToFilter.IsChecked == true ? DateTime.Now.Year - 1 : DateTime.Now.Year;
                try
                {
                    GlobalViewModel.Instance.HispaniaViewModel.RefreshSettlements(Agent_Id, Bill_Id_From, Bill_Id_Until, YearQuery);
                    if (GlobalViewModel.Instance.HispaniaViewModel.SettlementsDict.Count > 0)
                    {
                        SettlementsReportView.CreateReport(Bill_Id_From, Bill_Id_Until,
                                                           GlobalViewModel.Instance.HispaniaViewModel.SettlementsDict);
                    }
                    else MsgManager.ShowMessage("Informació, no hi ha liquidacions pels filtres seleccionats.", MsgType.Information);
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(string.Format("Error, al generar l'informe.\r\nDetalls: {0}\r\n.", MsgManager.ExcepMsg(ex)));
                }
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

        #region Update UI

        private void UpdateButtonState()
        {
            btnPrint.IsEnabled = ((Agents != null) && (Agents.Count > 0)) &&
                                 ((Bills != null) && (Bills.Count > 0));
        }

        #endregion
    }
}
