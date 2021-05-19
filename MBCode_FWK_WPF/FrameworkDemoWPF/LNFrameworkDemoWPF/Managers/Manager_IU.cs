#region Librerias usadas por la clase

using MBCode.Framework.Managers.Culture;
using MBCode.Framework.Managers.Theme;
using MBCode.FrameworkDemoWFP.InterfazUsuario;
using System;
using System.Collections;
using System.Reflection;
using System.Windows;

#endregion

namespace MBCode.FrameworkDemoWFP.LogicaNegocio
{
    #region Enumerados

    /// <summary>
    /// Enumerado que define los posibles tipos de test a realizar.
    /// </summary>
    public enum DemoType
    {
        Undefined          = -1,
        CultureDemo        = 0,
        XMLDemo            = 1,
        MsgManagerDemo     = 2,
        DataBaseDemo       = 3
    }

    #endregion

    /// <summary>
    /// Clase que se encarga de interaccionar con la interfaz de usuario.
    /// </summary>
    public static class Manager_IU
    {
        #region Atributos (CultureItems)

        /// <summary>
        /// Ventana de arranque de la aplicación.
        /// </summary>
        private static Window MainWindow = null;

        /// <summary>
        /// Almacena el nombre del ensamblador de la Dll.
        /// </summary>
        private static string sAssemblyName = Assembly.GetAssembly(typeof(Manager_IU)).GetName().Name;

        /// <summary>
        /// Almacena el nombre del fichero de recursos del ensamblador de la Dll.
        /// </summary>
        private static string sFileResourcesName = Assembly.GetAssembly(typeof(Manager_IU)).GetName().Name;

        /// <summary>
        /// Se define un elemento del tipo de clase de Cultura heredado que permite trabajar con el fichero de recursos.
        /// </summary>
        public static CultureAssemblyWPF MainResources = new CultureAssemblyWPF(sAssemblyName, sFileResourcesName);

        #endregion

        #region Propiedades

        public static WindowDemoDataBase Window_BD
        {
            get
            {
                return ((WindowDemoDataBase)MainWindow);
            }
        }

        public static WindowDemoXML Window_XML
        {
            get
            {
                return ((WindowDemoXML)MainWindow);
            }
        }

        #endregion

        #region Métodos ({Test})

        /// <summary>
        /// Method that loads the window.
        /// </summary>
        /// <param name="eDemoType">Indicate the Windows type to load.</param>
        /// <param name="Params">Additional params neededs for load the Window.</param>
        public static void LoadMainWindow(DemoType eDemoType, Hashtable Params = null)
        {
            switch (eDemoType)
            {
                case DemoType.CultureDemo:
                     MainWindow = new WindowDemoCulture();
                     break;
                case DemoType.XMLDemo:
                     MainWindow = new WindowDemoXML();
                     ((WindowDemoXML) MainWindow).evExecuteCommand += new WindowDemoXML.dlgExecuteCommand(Manager_XML.OnExecuteCommand);
                     break;
                case DemoType.MsgManagerDemo:
                     MainWindow = new WindowDemoMsgManager();
                     break;
                case DemoType.DataBaseDemo:
                     MainWindow = new WindowDemoDataBase();
                    ((WindowDemoDataBase)MainWindow).evTryToConnect += new WindowDemoDataBase.dlgTryToConnect(Manager_BD.OnEventTryToConnect);
                     ((WindowDemoDataBase)MainWindow).evExecuteQuery += new WindowDemoDataBase.dlgExecuteQuery(Manager_BD.OnEventExecuteQuery);
                     break;
                default:
                     throw new ArgumentException(MainResources.FindTextResource("WPFError_000000"));
            }
            MainWindow.Show();
            CultureManager.evChangeCulture += new CultureManager.dlgChangeCulture(OnChangeCulture); //  Solo para Test.
            ThemeManager.evChangeTheme += new ThemeManager.dlgChangeTheme(OnChangeTheme);
        }

        /// <summary>
        /// Método que se ejecuta al detectar un cambio de tema.
        /// </summary>
        private static void OnChangeTheme()
        {
            //MessageBox.Show(string.Format("Cambio de tema detectado : {0}.", ThemeManager.ActualTheme.FileNameTheme));
        }

        /// <summary>
        /// Método que se ejecuta al detectar un cambio de cultura.
        /// </summary>
        private static void OnChangeCulture()
        {
            //MessageBox.Show(string.Format("Cambio de cultura detectada : {0}.", CultureManager.ActualCultureName));
        }

        public static void UnloadMainWindow(DemoType eDemoType)
        {
            switch (eDemoType)
            {
                case DemoType.CultureDemo:
                case DemoType.XMLDemo:
                     if (MainWindow != null) MainWindow.Close();
                     break;
                default:
                     throw new ArgumentException(MainResources.FindTextResource("WPFError_000000"));
            }
        }

        #endregion
    }
}
