#region Librerias usada por la clase

using MBCode.Framework.Managers;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;

#endregion

namespace MBCode.Framework.Controls.WPF.Dialogs
{
    #region Enumerados

    /// <summary>
    /// Posibles estados de la ventana 
    /// </summary>
    public enum DialogState
    {
        /// <summary>
        /// Indica que el usuario ha apretado el botón 'Cancelar'. En este caso da por cancelada la operación y cierra la
        /// Ventana de Espera.
        /// </summary>
        CancelPressed,

        /// <summary>
        /// Indica que se ha llegado al número máximo de pasos '100' y que por lo tanto la operación ha finalizado con éxito.
        /// </summary>
        ActionCompleted,

        /// <summary>
        /// Indica que la Ventana de Espera está viva monitorizando una operación.
        /// </summary>
        Alive,
    }

    /// <summary>
    /// Tipos de Mensaje a mostrar.
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// Indica que de presenta la información asociada a un mensaje de Error.
        /// </summary>
        Error,

        /// <summary>
        /// Indica que de presenta la información asociada a un mensaje de Aviso.
        /// </summary>
        Warning,

        /// <summary>
        /// Indica que de presenta la información asociada a un mensaje de Información.
        /// </summary>
        Information,
        
        /// <summary>
        /// Indica que de presenta la información asociada a un mensaje de Aviso Importante.
        /// </summary>
        Notification,

        /// <summary>
        /// Indica que de presenta la información asociada a un mensaje normal, sin función aparente.
        /// </summary>
        Normal,
    }

    #endregion

    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha última modificación: 13/08/2013
    /// Descripción: ventana asociada a un 'WaitDialog'.
    /// </summary>
    public partial class WaitDialogWindow : Window, IDisposable
    {
        #region Definiciones API de Windows

        #region Para la Transparencia de la Ventana

        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int cxLeftWidth;      // width of left border that retains its size 
            public int cxRightWidth;     // width of right border that retains its size 
            public int cyTopHeight;      // height of top border that retains its size 
            public int cyBottomHeight;   // height of bottom border that retains its size
        };

        [DllImport("DwmApi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS pMarInset);

        #endregion

        #region Para la ocultación del botón de cierre de la Ventana

        [DllImport("user32.dll")]
        static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        private const int GWL_STYLE = -16;

        private const uint WS_SYSMENU = 0x80000;

        #endregion

        #endregion

        #region Atributos

        /// <summary>
        /// Almacena un valor que determina si el Dialogo se debe mostrar durante un tiempo determinado (tiempo > 0) o no 
        /// (tiempo <= 0). 
        /// 
        ///                tiempo de espera <= 0 :-> No tiene en cuenta el cierre retardado
        ///                tiempo de espera > 0  :-> Cierra la ventana de espera en el tiempo en segundos expresado
        ///                                          por el valor pasado cómo parámetro.
        ///     
        /// </summary>
        private int iTimeToAutomaticClose;

        /// <summary>
        /// Almacena el timer usado para controlar el cierre del WaitDialog cuando se está en modo 'TimeToAutomaticClose'.
        /// </summary>
        private Timer tmrTimeToAutomaticClose;

        /// <summary>
        /// Define el estilo del primer 25% del recorrido de la barra de la ProgressBar.
        /// </summary>
        private Style styleFirstQuarter;

        /// <summary>
        /// Define el estilo del segundo 25% del recorrido de la barra de la ProgressBar.
        /// </summary>
        private Style styleSecondQuarter;

        /// <summary>
        /// Define el estilo del tercer 25% del recorrido de la barra de la ProgressBar.
        /// </summary>
        private Style styleThirdQuarter;

        /// <summary>
        /// Define el estilo del cuarto 25% del recorrido de la barra de la ProgressBar.
        /// </summary>
        private Style styleFourthQuarter;

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene / Establece el Identificador de la Ventana.
        /// </summary>
        public Guid Identifier
        {
            get;
            private set;
        }

        /// <summary>
        /// Obtiene / Establece un valor que indica el estado de la Ventana.
        /// </summary>
        public DialogState State
        {
            get;
            private set;
        }

        /// <summary>
        /// Obtiene / Establece un valor que determina si la ProgressBar muestra valores reales o información sobre el progreso 
        /// continuo y genérico. 
        /// 
        ///     false :-> si ProgressBar muestra los valores reales (valor predeterminado). 
        ///     true  :-> si ProgressBar muestra el progreso genérico. 
        ///     
        /// </summary>
        public bool ContinousMode
        {
            get
            {
                return (pbElapsedTime.IsIndeterminate);
            }
            set
            {
                if (value)
                {
                    if (tmrTimeToAutomaticClose != null) tmrTimeToAutomaticClose.Dispose();
                    iTimeToAutomaticClose = 0;
                }
                pbElapsedTime.IsIndeterminate = value;
            }
        }

        /// <summary>
        /// Obtiene / Establece un valor que indica si el Dialogo se debe mostrar durante un tiempo determinado (tiempo > 0) o no 
        /// (tiempo <= 0). 
        /// 
        ///                tiempo de espera <= 0 :-> No tiene en cuenta el cierre retardado
        ///                tiempo de espera > 0  :-> Cierra la ventana de espera en el tiempo en segundos expresado
        ///                                          por el valor pasado cómo parámetro.
        ///     
        /// </summary>
        public int TimeToAutomaticClose
        {
            get
            {
                return (iTimeToAutomaticClose);
            }
            set
            {
                if (value > 0)
                {
                    if (pbElapsedTime.IsIndeterminate) pbElapsedTime.IsIndeterminate = false;
                    if (tmrTimeToAutomaticClose != null) tmrTimeToAutomaticClose.Dispose();
                    iTimeToAutomaticClose = value;
                    pbElapsedTime.Value = 0;
                    pbElapsedTime.Maximum = value;
                    tmrTimeToAutomaticClose = new Timer(ManageAutomaticClose, null, TimeSpan.FromSeconds(0), TimeSpan.FromSeconds(1));
                }
                else
                {
                    if (tmrTimeToAutomaticClose != null) tmrTimeToAutomaticClose.Dispose();
                    iTimeToAutomaticClose = 0;
                }
            }
        }

        /// <summary>
        /// Método que gestiona el modo automático.
        /// </summary>
        /// <param name="o">Objeto que gestiona el Timer.</param>
        public void ManageAutomaticClose(object o)
        {
            Dispatcher.BeginInvoke(new Action(delegate() { ManageAutomaticClose(); }));
        }

        /// <summary>
        /// Método que gestiona el modo automático.
        /// </summary>
        public void ManageAutomaticClose()
        {
            pbElapsedTime.Style = null;
            if (pbElapsedTime.Value >= iTimeToAutomaticClose)
            {
                tmrTimeToAutomaticClose.Dispose();
                State = DialogState.ActionCompleted;
                Close();
            }
            else
            {
                double percent = pbElapsedTime.Value / iTimeToAutomaticClose;
                if (percent < 0.25) pbElapsedTime.Style = styleFirstQuarter; 
                else if (percent < 0.50) pbElapsedTime.Style = styleSecondQuarter; 
                else if (percent < 0.75) pbElapsedTime.Style = styleThirdQuarter; 
                else pbElapsedTime.Style = styleFourthQuarter; 
                pbElapsedTime.Value++;
            }
        }

        /// <summary>
        /// Obtiene / Establece la orientación que seguirá la animación de avance de la ProgressBar.
        /// </summary>
        public Orientation OrientationAnimation
        {
            get
            {
                return (pbElapsedTime.Orientation);
            }
            set
            {
                pbElapsedTime.Orientation = value;
            }
        }

        /// <summary>
        /// Obtiene / Establece el Texto de la Acción asociada a la posción actual de la ProgressBar.
        /// </summary>
        public string SetActionDescription
        {
            get
            {
                return (tbAction.Text);
            }
            set
            {
                if (value == null) tbAction.Text = string.Empty;
                else if (value.Length < 536) tbAction.Text = value;
                else tbAction.Text = string.Format("{0}...", value.Substring(0, 536));
            }
        }

        /// <summary>
        /// Establece el título de la ventana de espera.
        /// </summary>
        private string WindowTitle
        {
            set
            {
                lblWaitWindowTitle.Content = value;
            }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor por defecto de la clase.
        /// </summary>
        /// <param name="WaitWindowTitle">Título de la Ventana de Espera.</param>
        /// <param name="ContinousMode">Valor que indica si la ProgressBar muestra valores reales o información sobre el progreso continuo y genérico.</param>
        /// <param name="TimeToAutomaticClose">Tiempo de Cierre automático de la Ventana de Espera.</param>
        /// <param name="OrientationAnimation">Define la orientación que seguirá la animación de avance de la ProgressBar.</param>
        /// <param name="InitialAction">Texto asociado a la acción inicial.</param>
        /// <param name="ShowCancelButton">Determina si se debe mostrar o no el botón de Cancelar.</param>
        /// <param name="TypeMessage">Tipo de mensaje asociado a la información que se presenta.</param>
        public WaitDialogWindow(string WaitWindowTitle, bool ContinousMode, int TimeToAutomaticClose, Orientation OrientationAnimation,
                                string InitialAction, bool ShowCancelButton, MessageType TypeMessage) 
        {
            InitializeComponent();
            Initialize(WaitWindowTitle, ContinousMode, TimeToAutomaticClose, OrientationAnimation, InitialAction, ShowCancelButton,
                       TypeMessage);
        }

        /// <summary>
        /// Realiza las inicializaciones adicionales de la ventana. Por defecto la ProgressBar tiene:
        ///
        ///                  · pbElapsedTime.Minimum :->    0.0   // Valor mínimo de la ProgressBar.
        ///                  · pbElapsedTime.Maximum :->  100.0   // Valor máximo de la Progressbar.
        ///                  
        /// </summary>
        /// <param name="WaitWindowTitle">Título de la Ventana de Espera.</param>
        /// <param name="ContinousMode">Valor que indica si la ProgressBar muestra valores reales o información sobre el progreso continuo y genérico.</param>
        /// <param name="TimeToAutomaticClose">Tiempo de Cierre automático de la Ventana de Espera.</param>
        /// <param name="OrientationAnimation">Define la orientación que seguirá la animación de avance de la ProgressBar.</param>
        /// <param name="InitialAction">Texto asociado a la acción inicial.</param>
        /// <param name="ShowCancelButton">Determina si se debe mostrar o no el botón de Cancelar.</param>
        /// <param name="TypeMessage">Tipo de mensaje asociado a la información que se presenta.</param>
        private void Initialize(string WaitWindowTitle, bool ContinousMode, int TimeToAutomaticClose, Orientation OrientationAnimation,
                                string InitialAction, bool ShowCancelButton, MessageType TypeMessage) 
        {
            //  Valida los datos con los que se desea inicializar el Wait Dialog.
                if ((ContinousMode) && (TimeToAutomaticClose > 0))
                {
                    throw new ArgumentException("Error, you can not use at the same time modes 'Continuous' and 'AutomaticTime'.");
                }
            //  Define los Colores Solidos a usar en los estilos.
                SolidColorBrush BackgroundProgressBar = new SolidColorBrush(Color.FromArgb(202, 219, 211, 205));
                SolidColorBrush BorderBrushProgressBar = new SolidColorBrush(Color.FromArgb(130, 135, 144, 255));
            //  Construye el estilo asociado al primer 25% de recorrido de la ProgressBar.
                styleFirstQuarter = new Style(typeof(ProgressBar));
                styleFirstQuarter.Setters.Add(new Setter(ProgressBar.BackgroundProperty, BackgroundProgressBar));
                styleFirstQuarter.Setters.Add(new Setter(ProgressBar.ForegroundProperty, Brushes.Red));
                styleFirstQuarter.Setters.Add(new Setter(ProgressBar.BorderThicknessProperty, new Thickness(1.0)));
                styleFirstQuarter.Setters.Add(new Setter(ProgressBar.BorderBrushProperty, BorderBrushProgressBar));
             //  Construye el estilo asociado al segundo 25% de recorrido de la ProgressBar.
                styleSecondQuarter = new Style(typeof(ProgressBar));
                styleSecondQuarter.Setters.Add(new Setter(ProgressBar.BackgroundProperty, BackgroundProgressBar));
                styleSecondQuarter.Setters.Add(new Setter(ProgressBar.ForegroundProperty, Brushes.Orange));
                styleSecondQuarter.Setters.Add(new Setter(ProgressBar.BorderThicknessProperty, new Thickness(1.0)));
                styleSecondQuarter.Setters.Add(new Setter(ListBoxItem.BorderBrushProperty, BorderBrushProgressBar));
             //  Construye el estilo asociado al tercer 25% de recorrido de la ProgressBar.
                styleThirdQuarter = new Style(typeof(ProgressBar));
                styleThirdQuarter.Setters.Add(new Setter(ProgressBar.BackgroundProperty, BackgroundProgressBar));
                styleThirdQuarter.Setters.Add(new Setter(ProgressBar.ForegroundProperty, Brushes.Yellow));
                styleThirdQuarter.Setters.Add(new Setter(ProgressBar.BorderThicknessProperty, new Thickness(1.0)));
                styleThirdQuarter.Setters.Add(new Setter(ProgressBar.BorderBrushProperty, BorderBrushProgressBar));
            //  Construye el estilo asociado al cuarto 25% de recorrido de la ProgressBar.
                styleFourthQuarter = new Style(typeof(ProgressBar));
                styleFourthQuarter.Setters.Add(new Setter(ProgressBar.BackgroundProperty, BackgroundProgressBar));
                styleFourthQuarter.Setters.Add(new Setter(ProgressBar.ForegroundProperty, Brushes.Green));
                styleFourthQuarter.Setters.Add(new Setter(ProgressBar.BorderThicknessProperty, new Thickness(1.0)));
                styleFourthQuarter.Setters.Add(new Setter(ProgressBar.BorderBrushProperty, BorderBrushProgressBar));
            //  Inicializa los atributos introducidos en el Constructor de la Clase.
                tmrTimeToAutomaticClose = null;
                if (WaitWindowTitle != null) WindowTitle = WaitWindowTitle;
            //  Carga la imagen asociada al tipo de Mensaje.
                Dispatcher.Invoke(new Action(delegate() { LoadIconMessage(TypeMessage); }));
                this.ContinousMode = ContinousMode;
                this.TimeToAutomaticClose = TimeToAutomaticClose;
                this.OrientationAnimation = OrientationAnimation;
                SetActionDescription = (InitialAction == null) ? string.Empty : InitialAction;
                pbElapsedTime.Value = 0;
            //  Da valor a los atributos.
                Identifier = Guid.NewGuid();
            //  Define los eventos que tratará el formulario.
                btnMinimize.Click += new RoutedEventHandler(OnMinimizeConsole);
                imgMinimizeConsole.MouseEnter += new MouseEventHandler(OnMinimizeConsoleActive);
                imgMinimizeConsole.MouseLeave += new MouseEventHandler(OnMinimizeConsoleInactive);
                if (!ShowCancelButton)
                {
                    btnCancel.Visibility = Visibility.Hidden;
                    Row_AdditionalInfoAction.Height = new GridLength(10.0);
                }
                else btnCancel.Click += new RoutedEventHandler(OnCancel);
            //  Actualiza el estado del Diálogo.
                State = DialogState.Alive;
        }

        /// <summary>
        /// Método que carga el icono que indica el tipos de mensaje que se está mostrando en la ventana en función del tipos de
        /// mensaje pasado cómo parámetro.
        /// </summary>
        /// <param name="TypeMessage">Tipo de mensaje a mostrar.</param>
        private void LoadIconMessage(MessageType TypeMessage)
        {
            try
            {
                imgMsgErrorIcon.Visibility = imgMsgInformationIcon.Visibility = imgNormalIcon.Visibility = Visibility.Hidden;
                imgMsgNotificationIcon.Visibility = imgMsgWarningIcon.Visibility = Visibility.Hidden;
                switch (TypeMessage)
                {
                    case MessageType.Error:
                         imgMsgErrorIcon.Visibility = Visibility.Visible;
                         break;
                    case MessageType.Information:
                         imgMsgInformationIcon.Visibility = Visibility.Visible;
                         break;
                    case MessageType.Notification:
                         imgMsgNotificationIcon.Visibility = Visibility.Visible;
                         break;
                    case MessageType.Warning:
                         imgMsgWarningIcon.Visibility = Visibility.Visible;
                         break;
                    case MessageType.Normal:
                    default:
                         imgNormalIcon.Visibility = Visibility.Visible;
                         break;
                }
            }
            catch { }
        }

        #endregion

        #region Destructores
        
        #region Implementación de la Interfaz IDisposable

        /// <summary>
        /// Atributo que almacena un valor que indica si se han liberado o no los recursos de la clase.
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// Método de liberación de recursos.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Libera los recursos de la clase.
        /// </summary>
        /// <param name="disposing">Indica si se deben liberar los recursos o no.</param>
        protected virtual void Dispose(bool disposing)
        {
            //  Comprueba que no se hayan liberado ya los recursos. 
                if(!this.disposed)
                {
                    //  Si Disosing == true se deben liberar los recursos.
                        //if (disposing) ;
                    //  La limpieza de recursos ha finalizado.
                        disposed = true;
                }
        }

        /// <summary>
        /// Destructor de la clase.
        /// </summary>
        ~WaitDialogWindow()
        {
            //  No volver a liberar los recursos de la clase en este punto.
                Dispose(false);
        }

        #endregion

        #endregion

        #region Métodos

        #region Load

        /// <summary>
        /// Método que gestiona las acciones a realizar durante la carga de la ventana.
        /// </summary>
        /// <param name="sender">Objeto que lanza el evento.</param>
        /// <param name="e">Parámetros con los que se lanza el evento.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            OnRender(null);
        }

        #endregion

        #region Control de Pintado de la Ventana

        /// <summary>
        /// Método que gestiona el pintado de la ventana (para el pintado del fondo transparente).
        /// </summary>
        /// <param name="drawingContext">Parámetros con los que se realiza el pintado.</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            try
            {
                //  Obtain the window handle for WPF application
                    IntPtr mainWindowPtr = new WindowInteropHelper(this).Handle;
                    HwndSource mainWindowSrc = HwndSource.FromHwnd(mainWindowPtr);
                    mainWindowSrc.CompositionTarget.BackgroundColor = Color.FromArgb(0, 0, 0, 0);
                //  Get System Dpi
                    System.Drawing.Graphics desktop = System.Drawing.Graphics.FromHwnd(mainWindowPtr);
                    float DesktopDpiX = desktop.DpiX;
                    float DesktopDpiY = desktop.DpiY;
                //  Set Margins
                    MARGINS margins = new MARGINS();
                //  Note: that the default desktop Dpi is 96dpi. The  margins are adjusted for the system Dpi.
                    margins.cxLeftWidth = Convert.ToInt32(((int)this.Width) * DesktopDpiX / 96);
                    margins.cxRightWidth = Convert.ToInt32(((int)this.Width) * DesktopDpiX / 96);
                    margins.cyTopHeight = Convert.ToInt32(((int)this.Height) * (DesktopDpiX / 96));
                    margins.cyBottomHeight = Convert.ToInt32(((int)this.Height) * (DesktopDpiX / 96));
                    int hr = DwmExtendFrameIntoClientArea(mainWindowSrc.Handle, ref margins);
                    if (hr < 0) { } // DwmExtendFrameIntoClientArea Failed
                    //Application.Current.MainWindow.Background = Brushes.Transparent;
                    this.Background = Brushes.Transparent;
            }
            // If not Vista, paint background white. 
            catch (DllNotFoundException)
            {
                Application.Current.MainWindow.Background = Brushes.White;
            }
            base.OnRender(drawingContext);
        }

        /// <summary>
        /// Método que oculta el botón de cierre de la ventana.
        /// </summary>
        /// <param name="e">Argumentos con los que se lanza el método.</param>
        protected override void OnSourceInitialized(EventArgs e)
        {
            IntPtr hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle;
            SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & (0xFFFFFFFF ^ WS_SYSMENU));
            base.OnSourceInitialized(e);
        }

        #endregion

        #region Gestión de la Barra de Tareas de la Consola

        /// <summary>
        /// Método que gestiona el minimizado de la consola.
        /// </summary>
        /// <param name="sender">Objeto que lanza el evento.</param>
        /// <param name="e">Parámetros con los que se lanza el evento.</param>
        private void OnMinimizeConsole(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Método que gestiona el estado activo del minimizado de la consola.
        /// </summary>
        /// <param name="sender">Objeto que lanza el evento.</param>
        /// <param name="e">Parámetros con los que se lanza el evento.</param>
        private void OnMinimizeConsoleActive(object sender, MouseEventArgs e)
        {
            imgMinimizeConsole.Source = Manager_IU.GetBitmapImage("MinimizeConsoleImage");
        }

        /// <summary>
        /// Método que gestiona el estado inactivo del minimizado de la consola.
        /// </summary>
        /// <param name="sender">Objeto que lanza el evento.</param>
        /// <param name="e">Parámetros con los que se lanza el evento.</param>
        private void OnMinimizeConsoleInactive(object sender, MouseEventArgs e)
        {
            imgMinimizeConsole.Source = Manager_IU.GetBitmapImage("MinimizeConsoleNotSelectedImage");
        }

        #endregion

        #region Gestión de los Controles de la Ventana

        /// <summary>
        /// Método que gestiona el cierre de la consola.
        /// </summary>
        /// <param name="sender">Objeto que lanza el evento.</param>
        /// <param name="e">Parámetros con los que se lanza el evento.</param>
        //private void OnCancel(object sender, MouseEventArgs e)
        private void OnCancel(object sender, RoutedEventArgs e)
        {
            State = DialogState.CancelPressed;
            Close();
        }

        #endregion

        #region Expuestos para la interacción de la ventana con el Usuario

        /// <summary>
        /// Método que, si la ProgressBar está en modo 'Por Pasos', añade el número de pasos indicado por la variable pasada
        /// como parámetro a la ProgressBar.
        /// </summary>
        /// <param name="NumSteps">Número de pasos que se desea añadir a la animación de avance de la ProgressBar.</param>
        /// <param name="sMsgError">Mensaje de Error, si se produce uno.</param>
        public bool AddStep(int NumSteps, out string sMsgError)
        {
            //  Determina si la Ventana de Espera tiene la ProgressBar en Modo 'Paso a Paso'.
                if (ContinousMode) 
                {
                    //  Inicializa la variable de retorno que contiene el mensaje de error e indica que hay un error.
                        sMsgError = "Error, impossible add a step in a WaitDialog in mode Continous.";
                        return (false);
                }
                else if (TimeToAutomaticClose > 0)
                {
                    //  Inicializa la variable de retorno que contiene el mensaje de error e indica que hay un error.
                        sMsgError = "Error, impossible add a step in a WaitDialog in mode AutomaticClose.";
                        return (false);
                }
                else 
                {
                    //  Inicializa la variable de retorno que contiene el mensaje de error.
                        sMsgError = null;
                    //  Si no está en modo continuo añade los pasos indicados.
                        pbElapsedTime.Value += NumSteps;
                    ////  Determina si se ha llegado a la cota máxima de pasos y si es así actualiza el estado de la ventana y
                    ////  la cierra.
                    //    if (pbElapsedTime.Value >= pbElapsedTime.Maximum)
                    //    {
                    //        State = DialogState.ActionCompleted;
                    //        Close();
                    //    }
                    //  Indica que no se han producido errores.
                        return (true);
                }
        }

        /// <summary>
        /// Método que, si la ProgressBar está en modo 'Continuo', cierra la Ventana de Espera.
        /// </summary>
        /// <param name="sMsgError">Mensaje de Error, si se produce uno.</param>
        public bool CloseDialog(out string sMsgError)
        {
            ////  Determina si la Ventana de Espera tiene la ProgressBar en Modo 'Continuo'.
            //    if (!ContinousMode) 
            //    {
            //        sMsgError = "Error, impossible close the WaitDialog in mode Step by Step.";
            //        return (false);
            //    }
            //    else
            //    {
                    //  Inicializa la variable de retorno que contiene el mensaje de error.
                        sMsgError = null;
                    //  Actualiza el estado de la ventana y la cierra.
                        State = DialogState.ActionCompleted;
                        Close();
                    //  Indica que no se han producido errores.
                        return (true);
                //}
        }

        #endregion

        #endregion
    }
}
