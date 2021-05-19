#region Librerias usadas por la clase

using System.Windows;
using System.Runtime.InteropServices;
using System;
using System.Windows.Interop;
using System.Windows.Media;
using System.Security;
using System.Collections.Generic;
using System.Windows.Input;
using MBCode.Framework.Managers.Messages;
using System.Windows.Media.Imaging;
using MBCode.Framework.Credentials;

#endregion

namespace HispaniaCommon.ViewClientWPF.Windows
{
    /// <summary>
    /// Ventana que permite al usuario introducir las credenciales para acceder a la aplicación.
    /// </summary>
    public partial class CredentialsWindow : Window
    {
        #region API Windows Definitions

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

        #region Attributes

        /// <summary>
        /// Store the Credentials definied in the application that the Windows use to allow access at the user.
        /// </summary>
        private Dictionary<string, Credential> m_AllCredentials = new Dictionary<string, Credential>();

        /// <summary>
        /// Store a Value that determine if the window use Transparency Mode or not.
        /// </summary>
        private bool m_TransparencyActive = true;

        #endregion

        #region Properties

        /// <summary>
        /// Get or Set a Value that determine if the window use Transparency Mode or not.
        /// </summary>
        public bool TransparencyActive
        {
            get
            {
                return m_TransparencyActive;
            }
            set
            {
                m_TransparencyActive = value;
            }
        }

        /// <summary>
        /// Obtiene / Establece la Credencial asociada a los datos introducidos por el Usuario.
        /// </summary>
        public Credential AppCredential
        {
            get;
            private set;
        }

        /// <summary>
        /// Get or Set the Credentials definied in the application that the Windows use to allow access at the user.
        /// </summary>
        public Dictionary<string, Credential> AllCredentials
        {
            get
            {
                return m_AllCredentials;
            }
            set
            {
                if (value is null)
                {
                    throw new NotImplementedException("Error, la llista d'usuaris definit no pot ser nula.");
                } 
                else
                {
                    m_AllCredentials = value;
                }
            }
        }

        #endregion

        #region Builders

        /// <summary>
        /// Constructor por defecto de la Ventana.
        /// </summary>
        public CredentialsWindow()
        {
            InitializeComponent();
            Initialize();
        }

        /// <summary>
        /// Realiza las inicializaciones adicionales de la ventana.
        /// </summary>
        private void Initialize()
        {
            btnExit.MouseUp += new System.Windows.Input.MouseButtonEventHandler(OnExit);
            btnEntry.MouseUp += new System.Windows.Input.MouseButtonEventHandler(OnValidateCredentials);
        }

        #endregion

        #region Window

        /// <summary>
        /// Método que gestiona las acciones a realizar durante la carga de la ventana.
        /// </summary>
        /// <param name="sender">Objeto que lanza el evento.</param>
        /// <param name="e">Parámetros con los que se lanza el evento.</param>
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            OnRender(null);
            tbUser.Focus();
        }

        /// <summary>
        /// Método que gestiona el pintado de la ventana (para el pintado del fondo transparente).
        /// </summary>
        /// <param name="drawingContext">Parámetros con los que se realiza el pintado.</param>
        protected override void OnRender(DrawingContext drawingContext)
        {
            if (TransparencyActive)
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
                        MARGINS margins = new MARGINS
                        {
                            //  Note: that the default desktop Dpi is 96dpi. The  margins are adjusted for the system Dpi.
                            cxLeftWidth = Convert.ToInt32(((int)this.Width) * DesktopDpiX / 96),
                            cxRightWidth = Convert.ToInt32(((int)this.Width) * DesktopDpiX / 96),
                            cyTopHeight = Convert.ToInt32(((int)this.Height) * (DesktopDpiX / 96)),
                            cyBottomHeight = Convert.ToInt32(((int)this.Height) * (DesktopDpiX / 96))
                        };
                        int hr = DwmExtendFrameIntoClientArea(mainWindowSrc.Handle, ref margins);
                        if (hr < 0) { } // DwmExtendFrameIntoClientArea Failed
                        Application.Current.MainWindow.Background = Brushes.Transparent;
                        this.Background = Brushes.Transparent;
                }
                catch (DllNotFoundException) 
                {
                    //  If not Vista, paint background white. 
                        Application.Current.MainWindow.Background = Brushes.White;
                }
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

        #region Controls

        /// <summary>
        /// Método que gestiona las acciones a realizar al cancelar la ventana.
        /// </summary>
        /// <param name="sender">Objeto que lanza el evento.</param>
        /// <param name="e">Parámetros con los que se lanza el evento.</param>
        private void OnExit(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// Método que gestiona las acciones al aceptar los datos introducidos en la ventana.
        /// </summary>
        /// <param name="sender">Objeto que lanza el evento.</param>
        /// <param name="e">Parámetros con los que se lanza el evento.</param>
        private void OnValidateCredentials(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                //  Variables.
                    string sPasswordEncripted, NewUserName;

                //  Determina si la contraseña introducida es valida.
                    bool ValidateComplexity = false;
                    if (!Credential.ValidatePassword(tbPassword.Password, out string ErrMsg, ValidateComplexity))
                    {
                        MsgManager.ShowMessage(ErrMsg);
                        return;
                    }
                //  Puerta trasera para Indra (Determina si se trata de un SuperUsuario de Indra y si es así se logina).
                    if (Credential.IsSuperUser(tbUser.Text, tbPassword.Password, out Credential SuperUserCredential))
                    {
                        AppCredential = SuperUserCredential;
                        DialogResult = true;
                        this.Close();
                        return;
                    }
                //  Busca si existe el usuario dentro del diccionario de Credenciales.
                    NewUserName = tbUser.Text.ToUpper();
                    if (AllCredentials.ContainsKey(NewUserName))
                    {
                        //  Password del Usuario encriptado.
                            sPasswordEncripted = AllCredentials[NewUserName].Password;
                        //  Valida los datos introducidos por el usuario.
                            switch (Encript.ValidarPassword(tbPassword.Password, sPasswordEncripted, out ErrMsg))
                            {
                                case ComparaHash.Correcto:
                                     AppCredential = AllCredentials[NewUserName];
                                     DialogResult = true;
                                     this.Close();
                                     break;
                                case ComparaHash.Incorrecto:
                                     MsgManager.ShowMessage(MsgManager.LiteralMsg("Error, contraseña incorrecta"));
                                     break;
                                case ComparaHash.ErrorCalculo:
                                default:
                                     MsgManager.ShowMessage(ErrMsg);
                                     break;
                            }
                    }
                    else
                    {
                        MsgManager.ShowMessage(
                            MsgManager.LiteralMsg(string.Format("Error, el usuario '{0}' no existe.", tbUser.Text)));
                    }
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(MsgManager.ExcepMsg(ex));
            }
        }

        #endregion
    }
}
