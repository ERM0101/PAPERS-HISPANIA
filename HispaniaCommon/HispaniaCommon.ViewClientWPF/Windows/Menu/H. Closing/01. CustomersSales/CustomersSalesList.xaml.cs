#region Librerias usadas por la ventana

using HispaniaCommon.ViewModel;
using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.Theme;
using System;
using System.Windows;
using System.Windows.Media;

#endregion

namespace HispaniaCommon.ViewClientWPF.Windows
{
    /// <summary>
    /// Interaction logic for CustomersSales.xaml
    /// </summary>
    public partial class CustomersSalesList : Window
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
        public CustomersSalesList(ApplicationType AppType)
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
            //  Initialize Controls.
                tbReportData.Tag = DateTime.Now;
                tbReportData.Text = GlobalViewModel.GetLongDateString(DateTime.Now);
        }

        #endregion

        #region Managers

        /// <summary>
        /// Method that define the managers needed for the user operations in the Window
        /// </summary>
        private void LoadManagers()
        {
            //  Button managers
                btnStipple.Click += BtnStipple_Click;
                btnSend.Click += BtnSend_Click;
                btnExit.Click += BtnExit_Click;
        }

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

        #region Stipple Report

        /// <summary>
        /// Manage the button for stipple option.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnStipple_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (decimal.TryParse(tbUpper_Limit_Sales.Text, out decimal Upper_Limit_Sales))
                {
                    DateTime ReportData = (DateTime) tbReportData.Tag;
                    GlobalViewModel.Instance.HispaniaViewModel.RefreshCustomersSales(Upper_Limit_Sales);
                    CustomersSalesReportView.CreateReport(CustomersSalesReportType.Stipple, Upper_Limit_Sales, ReportData,
                                                          GlobalViewModel.Instance.HispaniaViewModel.CustomersSalesDict);
                }
                else
                {
                    MsgManager.ShowMessage("Error, al crear l'informe.\r\nDetalls: el marge de venda superior introduït és incorrecte");
                }
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al generar l'informe del puntejat.\r\nDetalls: {0}\r\n.",
                                                     MsgManager.ExcepMsg(ex)));
            }
        }

        #endregion

        #region Send Report

        /// <summary>
        /// Manage the button for send option.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (decimal.TryParse(tbUpper_Limit_Sales.Text, out decimal Upper_Limit_Sales))
                {
                    DateTime ReportData = (DateTime)tbReportData.Tag;
                    GlobalViewModel.Instance.HispaniaViewModel.RefreshCustomersSales(Upper_Limit_Sales);
                    CustomersSalesReportView.CreateReport(CustomersSalesReportType.Send, Upper_Limit_Sales, ReportData,
                                                          GlobalViewModel.Instance.HispaniaViewModel.CustomersSalesDict);
                }
                else
                {
                    MsgManager.ShowMessage("Error, al crear l'informe.\r\nDetalls: el marge de venda superior introduït és incorrecte");
                }
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(string.Format("Error, al generar l'informe del puntejat.\r\nDetalls: {0}\r\n.",
                                                        MsgManager.ExcepMsg(ex)));
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

        #endregion
    }
}
