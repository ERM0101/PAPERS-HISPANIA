#region Librerias usadas por la clase

using MBCode.Framework.Managers.Culture;
using MBCode.Framework.Managers.Theme;
using MBCode.FrameworkDemoWFP.LogicaNegocio;
using System;
using System.Windows;
using System.Windows.Threading;

#endregion

namespace MBCode.Framework.Demo.WPF
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        #region Culture Items

        /// <summary>
        /// Objeto gestiona el multiidioma de la aplicación (ficheros de recursos).
        /// </summary>
        private CultureAssemblyWPF _oCulture = null;

        #endregion

        #region Atributos

        /// <summary>
        /// Almacena el tipo de demo que se desea realizar.
        /// </summary>
        DemoType _eDemoType = DemoType.Undefined;

        ///// <summary>
        ///// Ventana de arranque de la aplicación.
        ///// </summary>
        //private Window MainWindow;

        #endregion
        
        #region Constantes

        private const int LOAD_ERROR = -1;

        private const int DEFINE_EVENTS_ERROR = -2;

        private const int EXIT_PROCESS_ERROR = -3;

        private const int BEGIN_PROCESS_ERROR = -4;

        private const int MANAGE_UNCONTROLLED_EXCEPTION_ERROR = -5;

        private const int MANAGE_SESSION_ENDING_ERROR = -6;

        #endregion

        #region Constructores (Culture Items)

        /// <summary>
        /// Constructor de la clase en el que se definen las acciones que se llevan a cabo en el momento en que se inicia
        /// la aplicación.
        /// </summary>
        public App()
        {
            try
            {
                //  Incializa el objeto de cultura para la aplicación.
                    if (_oCulture == null) _oCulture = new CultureAssemblyWPF(this);
                //  Solo para Test.
                    CultureManager.evChangeCulture += new CultureManager.dlgChangeCulture(OnChangeCulture);
                //  Define los eventos.
                    DefineEventMangers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                Shutdown(LOAD_ERROR);
            }
        }

        /// <summary>
        /// Método que se ejecuta al detectar un cambio de cultura.
        /// </summary>
        private void OnChangeCulture()
        {
            //MessageBox.Show(this.FindResource("WPFIdioma").ToString(), "FrameworkdemoWPF");
        }

        #endregion

        #region Métodos

        #region Relacionados con los Eventos del Objeto Aplication

        #region Definición y Retirada

        /// <summary>
        /// Define los eventos de la aplicación.
        /// </summary>
        private void DefineEventMangers()
        {
            try
            {
                Startup += new StartupEventHandler(App_Startup);
                SessionEnding += new SessionEndingCancelEventHandler(App_SessionEnding);
                DispatcherUnhandledException += new DispatcherUnhandledExceptionEventHandler(App_DispatcherUnhandledException);
                Exit += new ExitEventHandler(App_Exit);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
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

        #endregion

        #region Gestores

        /// <summary>
        /// Gestor del evento asociado a la ejecución del método Run del objeto aplicación.
        /// </summary>
        private void App_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                if (e.Args.Length == 1)
                {
                    try
                    {
                        //  Load the Theme Manager
                            ThemeManager.ActualTheme = new ThemeInfo(@"MBCode.Framework.Controls.WPF", @"component/Recursos/Themes/", "Temp"); //"Hispania_Blau");
                        //  Create and show the Selector Window at the user.
                            SelectorWindow SelWindow = new SelectorWindow();
                            SelWindow.Closed += SelWindow_Closed;
                            SelWindow.Show();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error, al cargar el formulario de demostración, {" + ex.Message + "}.");
                        Environment.Exit(-1);
                    }
                }
                else
                {
                    MessageBox.Show("Error, al cargar el formulario de demostración, número de parámetros incorrecto.");
                    Environment.Exit(BEGIN_PROCESS_ERROR);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                Shutdown(BEGIN_PROCESS_ERROR);
            }
        }

        private void SelWindow_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Gestor del evento que se genera cuando el usuario inicia el cierre de la sesión de windows.
        /// </summary>
        private void App_SessionEnding(object sender, SessionEndingCancelEventArgs e)
        {
            try
            {
                //  Variables 
                    MessageBoxResult mbrResult;

                //  Interroga al usuario sobre sus intenciones.
                    mbrResult = MessageBox.Show("¿ Desea cerrar la aplicación y continuar con el cierre de sesión de Windows ?",
                                                "Advertencia de cierre de la aplicación", MessageBoxButton.YesNo, MessageBoxImage.Question,
                                                MessageBoxResult.No);
                    if (mbrResult != MessageBoxResult.Yes)
                    {
                        MessageBox.Show("Cierre de la aplicación cancelado por el usuario.");
                        e.Cancel = true;
                    }
                    else Manager_IU.LoadMainWindow(_eDemoType); //MainWindow.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                Shutdown(MANAGE_SESSION_ENDING_ERROR);
            }
        }

        /// <summary>
        /// Gestor del evento que se genera al producirse en la aplicación una excepción no controlada.
        /// </summary>
        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            try
            {
                MessageBox.Show(e.Exception.StackTrace);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                Shutdown(MANAGE_UNCONTROLLED_EXCEPTION_ERROR);
            }
        }

        /// <summary>
        /// Gestor del evento que se produce antes de la salida de la aplicación.
        /// </summary>
        private void App_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                //  Elimina las definiciones de eventos realizadas en la inicialización de la aplicación.
                    UnDefineEventMangers();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.StackTrace);
                Environment.ExitCode = EXIT_PROCESS_ERROR;
            }
        }

        #endregion

        #endregion

        #endregion
    }
}

