//#define CULTURE_CHANGE_NOTIFICATION

#region Librerias usadas por la clase

using HispaniaCommon.DataAccess;
using HispaniaCommon.ViewClientWPF.Windows;
using HispaniaCommon.ViewModel;
using MBCode.Framework.Managers.Culture;
using MBCode.Framework.Managers.Messages;
using System;
using System.Data.Entity.Core;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

#endregion

namespace HispaniaComptabilitat
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Constants

        private const int EXIT_OK = 0;

        private const int ENTRY_CANCELLED_BY_USER = 0;

        private const int LOAD_ERROR = -1;

        private const int DEFINE_EVENTS_ERROR = -2;

        private const int EXIT_PROCESS_ERROR = -3;

        private const int BEGIN_PROCESS_ERROR = -4;

        private const int MANAGE_UNCONTROLLED_EXCEPTION_ERROR = -5;

        private const int MANAGE_SESSION_ENDING_ERROR = -6;

        private const int INIT_USER_INTERFACE_ERROR = -7;

        #endregion

        #region Attributes

        /// <summary>
        /// Objeto gestiona el multiidioma de la aplicación (ficheros de recursos).
        /// </summary>
        private CultureAssemblyWPF _oCulture = null;

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene / Establece la ventana de Configuración.
        /// </summary>
        private MainWindow HispaniaMainWindow
        {
            get;
            set;
        }

        #endregion

        #region Builders

        /// <summary>
        /// Constructor de la clase en el que se definen las acciones que se llevan a cabo en el momento en que se inicia
        /// la aplicación.
        /// </summary>
        public App()
        {
            try
            {
                //  Load dictionaries of resources.
                    InitializeComponent();
                //  Define event managers.
                    DefineEventMangers();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(FindResource("App_Msg000"), MsgManager.ExcepMsg(ex));
                Shutdown(LOAD_ERROR);
            }
        }

        #endregion

        #region Managers

        /// <summary>
        /// Define los eventos de la aplicación.
        /// </summary>
        private void DefineEventMangers()
        {
            try
            {                
                //  Initialize application Culture object.
                    if (_oCulture == null) _oCulture = new CultureAssemblyWPF(this);
                //  Manager used only for test.
                    CultureManager.evChangeCulture += new CultureManager.dlgChangeCulture(OnChangeCulture);
                //  Define application managers.
                    Startup += new StartupEventHandler(App_Startup);
                    SessionEnding += new SessionEndingCancelEventHandler(App_SessionEnding);
                    DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
                    Exit += new ExitEventHandler(App_Exit);
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(FindResource("App_Msg000"), MsgManager.ExcepMsg(ex));
                Shutdown(DEFINE_EVENTS_ERROR); 
            }
        }

        /// <summary>
        /// Elimina la definición de los eventos de la aplicación.
        /// </summary>
        private void UnDefineEventMangers()
        {
            try
            {
                Startup -= new StartupEventHandler(App_Startup);
                SessionEnding -= new SessionEndingCancelEventHandler(App_SessionEnding);
                DispatcherUnhandledException -= new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
                Exit -= new ExitEventHandler(App_Exit);
            }
            catch (Exception) { }
        }

        #region OnChangeCulture (Manage Culture Change)

        /// <summary>
        /// Manage Culture Change.
        /// </summary>
        private void OnChangeCulture()
        {
            try
            {
                //  Load dictionaries of resources. 
                    #if CULTURE_CHANGE_NOTIFICATION
                        MsgManager.ShowMessage(FindResource("WPFIdioma"), "HispaniaComptabilitat", MsgType.Information);
                    #endif
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(FindResource("App_Msg007"), MsgManager.ExcepMsg(ex));
                Shutdown(LOAD_ERROR);
            }
        }

        #endregion

        #region StartUp (Manage Init Application Execution)

        /// <summary>
        /// Manage Init Application Execution.
        /// </summary>
        /// <param name="sender">Object that throws this event.</param>
        /// <param name="e">Arguments introduced by the user in the execution of the application.</param>
        private void App_Startup(object sender, StartupEventArgs e)
        {
            //  Set mouse cursor to sand clock for indicate that load operation has initiated.
                Mouse.OverrideCursor = Cursors.Wait;
            //  Do the basic initialitations
                try
                {
                    //  Read Values from Config File.
                        TraceLevel TraceLevelCFG = HispaniaComptabilitat.Properties.Settings.Default.TrazaError;
                    //  Establish the Tracelevel for the error messages of the application.
                        MsgManager.LevelTraceLog = LogViewModel.Instance.TraceLevelToUse = LogDataAccess.Instance.TraceLevelToUse = TraceLevelCFG;
                    //  Initialize Data Base access class.
                        GlobalViewModel.Instance.HispaniaViewModel = new MaintenanceForViewModel(ApplicationType.Comptabilitat);
                    //  Defines when Shutdown event was throwed. 
                        ShutdownMode = ShutdownMode.OnExplicitShutdown;
                }
                catch (EntityException eex)
                {
                    //  Set mouse cursor to arrow for indicate that application has been initiated.
                        Mouse.OverrideCursor = Cursors.Arrow;
                    //  Show error message at the user and close application.
                        // MsgManager.ExcepMsg(eex); If you want show more information.
                        MsgManager.ShowMessage(FindResource("App_Msg000"), 
                                               string.Format("Impossible accedir a la Base de Dades, contacti amb l'administrador del sistema.\r\nDetalls: {0}",
                                                             MsgManager.ExcepMsg(eex)));
                        Shutdown(LOAD_ERROR);
                }
                catch (Exception ex)
                {
                    //  Set mouse cursor to arrow for indicate that application has been initiated.
                        Mouse.OverrideCursor = Cursors.Arrow;
                    //  Show error message at the user and close application.
                        MsgManager.ShowMessage(FindResource("App_Msg000"), MsgManager.ExcepMsg(ex));
                        Shutdown(LOAD_ERROR);
                }
            //  Show the Main Window application
                try
                { 
                    //  Create the Main Window of the Application.
                        HispaniaMainWindow = new MainWindow(ApplicationType.Comptabilitat);
                        MainWindow.Closed += MainWindow_Closed;
                        MainWindow.Show();
                    //  Set mouse cursor to arrow for indicate that application has been initiated.
                        Mouse.OverrideCursor = Cursors.Arrow;
                }
                catch (Exception ex)
                {
                    //  Set mouse cursor to arrow for indicate that application has been initiated.
                        Mouse.OverrideCursor = Cursors.Arrow;
                    //  Show error message at the user and close application.
                        MsgManager.ShowMessage(FindResource("App_Msg000"), MsgManager.ExcepMsg(ex));
                        Shutdown(INIT_USER_INTERFACE_ERROR);
                }
        }

        /// <summary>
        /// Método que se invoca cuando se cierra la ventana principal de la aplicación
        /// </summary>
        /// <param name="sender">Object that throw the event.</param>
        /// <param name="e">Parameters with the event has been throwed</param>
        private void MainWindow_Closed(object sender, EventArgs e)
        {
            //  Close application without errors.
                Shutdown(EXIT_OK);
        }

        #endregion

        #region SessionEnding (Manage Windows Session Ending)

        /// <summary>
        /// Manage Windows Session Ending.
        /// </summary>
        private void App_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            try
            {
                //  Ask at the user for the actions that he or she want to do.
                    if (MsgManager.ShowQuestion(FindResource("App_Msg003").ToString()) == MessageBoxResult.Yes)
                    {
                        //  Do the actions needed for close the application.
                            CloseActions();
                        //  Close the aplication with exit code OK.
                            Shutdown(EXIT_OK);
                    }
                    else
                    {
                        MsgManager.ShowMessage(FindResource("App_Msg004"));
                        e.Cancel = true;
                    }
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(FindResource("App_Msg008"), MsgManager.ExcepMsg(ex));
                Shutdown(MANAGE_SESSION_ENDING_ERROR);
            }
        }

        #endregion

        #region DispatcherUnhandledException (Manage the Unhandled exceptions for the application)

        /// <summary>
        /// Manage the Unhandled exceptions for the application.
        /// </summary>
        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                MsgManager.ShowMessage(FindResource("App_Msg001"), MsgManager.ExcepMsg(e.Exception));
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(FindResource("App_Msg001"), MsgManager.ExcepMsg(ex));
                Shutdown(MANAGE_UNCONTROLLED_EXCEPTION_ERROR);
            }
        }

        #endregion

        #region Exit (Manage Finish Application Execution)

        /// <summary>
        /// Manage Finish Application Execution.
        /// </summary>
        private void App_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                //  Do the actions needed for close the application.
                    CloseActions();
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(FindResource("App_Msg002"), MsgManager.ExcepMsg(ex));
                Shutdown(EXIT_PROCESS_ERROR);
            }
        }

        #endregion

        #endregion

        #region Helpers

        /// <summary>
        /// Actions to do when the application it's closed.
        /// </summary>
        private void CloseActions()
        {
            //  Close Main window if it's open.
                if (MainWindow != null) MainWindow.Close();
            //  Remove DataBase Locks for this execution of the application.
                if (GlobalViewModel.Instance.HispaniaViewModel != null)
                {
                    GlobalViewModel.Instance.HispaniaViewModel.ClearLocks();
                }
            //  Remove the defined managers.
                UnDefineEventMangers();
        }

        #endregion
    }
}
