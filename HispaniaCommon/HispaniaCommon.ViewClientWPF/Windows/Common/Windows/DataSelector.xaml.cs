#region Librerias usadas por la ventana

using HispaniaCommon.ViewModel;
using MBCode.Framework.Managers.Theme;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#endregion

namespace HispaniaCommon.ViewClientWPF.Windows.Common
{
    /// <summary>
    /// Allow the user to select a Data
    /// </summary>
    public partial class DataSelector : Window
    {
        #region Enums

        /// <summary>
        /// Defines the possible result of the execution of this Window.
        /// </summary>
        public enum WindowResult
        {
            Accept,

            Cancel
        }

        #endregion

        #region Constants

        /// <summary>
        /// Defines the theme for the main Hispania Application.
        /// </summary>
        private ThemeInfo HispaniaApp = new ThemeInfo("HispaniaCommon.ViewClientWPF", "component/Recursos/Themes/", "HispaniaComptabilitat");

        /// <summary>
        /// Defines the theme for the support Hispania Application.
        /// </summary>
        private ThemeInfo HispaniaSupportApp = new ThemeInfo("HispaniaCommon.ViewClientWPF", "component/Recursos/Themes/", "HispaniaHelp");

        #endregion

        #region Attributes

        /// <summary>
        /// Store the data to show.
        /// </summary>
        private DateTime m_Data = DateTime.Now;

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
                ThemeManager.ActualTheme = AppTheme;
            }
        }

        /// <summary>
        /// Store the data to show in List of Items.
        /// </summary>
        public DateTime Data
        {
            get
            {
                return (m_Data);
            }
            set
            {
                m_Data = value;
            }
        }

        /// <summary>
        /// Store the Window Result of the execution of this Window.
        /// </summary>
        public WindowResult Result
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
        public DataSelector(ApplicationType AppType)
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
            //  Define Calendar accepted values, display value and selected value.
                cldrDataSelector.DisplayDateStart = new DateTime(Data.Year, 1, 1);
                cldrDataSelector.DisplayDateEnd = new DateTime(Data.Year + 1, 12, 31);
                cldrDataSelector.DisplayDate = Data;
                cldrDataSelector.SelectedDate = Data;
        }

        #endregion

        #region Managers

        /// <summary>
        /// Method that define the managers needed for the user operations in the Window
        /// </summary>
        private void LoadManagers()
        {
            //  Calendar 
                cldrDataSelector.SelectedDatesChanged += CldrDataSelector_SelectedDatesChanged; ;
            //  Button managers
                btnAccept.Click += BtnAccept_Click;
                btnCancel.Click += BtnCancel_Click;
        }

        #region Calendar

        /// <summary>
        /// DataDisplay has changed in sender Calendar control.
        /// </summary>
        /// <param name="sender">Calendar control that has produced the event.</param>
        /// <param name="e">Parameters associateds to the event.</param>
        private void CldrDataSelector_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Data = (cldrDataSelector.SelectedDate is null)? DateTime.Now : (DateTime) cldrDataSelector.SelectedDate;
        }

        #endregion

        #region Button

        #region Accept

        /// <summary>
        /// Manage the button for accept the edit operation.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {
            Result = WindowResult.Accept;
        }

        #endregion

        #region Cancel

        /// <summary>
        /// Manage the button for cancel the edit operation.
        /// </summary>
        /// <param name="sender">Object that sends the event.</param>
        /// <param name="e">Parameters with the event was sended.</param>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Result = WindowResult.Cancel;
        }

        #endregion

        #endregion

        #endregion
    }
}
