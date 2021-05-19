#define TEME

#region Librerias usadas por la clase

using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.Theme;
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#endregion

namespace MBCode.Framework.Managers.Culture
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha: 21/02/2012.
    /// Descripción: elemento que se incrusta en cada control o ventana WPF en la que se desee que se aplique el cambio de
    ///              cultura.
    /// </summary>
    public class CultureAddInUserControlWPF : UserControl, IDisposable 
    {
        #region Atributos

        /// <summary>
        /// Almacena el nombre del ensamblado en el que se encuentra la ventana.
        /// </summary>
        private string _assemblyName = null;

        /// <summary>
        /// Ventana en la que se desea aplicar la localización
        /// </summary>
        private UserControl _managedUserControl = null;

        /// <summary>
        /// Almacena el nombre del fichero que contiene los recursos de la aplicación.
        /// </summary>
        private string _sFileNameResources = null;

        /// <summary>
        /// Almacena el directorio donde se encuentra el fichero que contiene los recursos de la aplicación.
        /// </summary>
        private string _sDirectoryResources = null;

        /// <summary>
        /// Almacena un valor que indica si se debe llevar a cabo o no la sincronización del elemento en el que se ha
        /// colocado el AddIn.
        /// </summary>
        private bool _synchronizeCulture = true;

        /// <summary>
        /// Almacena un valor que indica si se debe llevar a cabo o no la sincronización del tema en el elemento en el que se ha
        /// colocado el AddIn.
        /// </summary>
        private bool _synchronizeTheme = true;

        /// <summary>
        /// Almacena una referencia a la cultura actual del formulario.
        /// </summary>
        private ResourceDictionary rdActualCulture = null;

        /// <summary>
        /// Almacena una referencia al tema actual de la ventana.
        /// </summary>
        private ResourceDictionary rdActualTheme = null;

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene/Establece un valor que indica si se debe llevar a cabo o no la sincronización del elemento en el 
        /// que se ha colocado el AddIn.
        /// </summary>
        [DefaultValue(true),
         Category("Extended Properties"),
         Description("Should the UICulture of this form be changed when the ApplicationUICulture")]
        public bool SynchronizeCulture
        {
            get
            {
                return (_synchronizeCulture);
            }
            set
            {
                _synchronizeCulture = value;
            }
        }

        /// <summary>
        /// Obtiene/Establece un valor que indica si se debe llevar a cabo o no la sincronización del tema en el elemento en el 
        /// que se ha colocado el AddIn.
        /// </summary>
        [DefaultValue(true),
         Category("Extended Properties"),
         Description("Define si el tema se debe aplicar o no a este control")]
        public bool SynchronizeTheme
        {
            get
            {
                return (_synchronizeTheme);
            }
            set
            {
                _synchronizeTheme = value;
            }
        }

        /// <summary>
        /// Obtiene/Establece el nombre del fichero que contiene los recursos de la aplicación.
        /// </summary>
        [Browsable(true),
         Category("Extended Properties"),
         Description("Nombre del ensamblado en el que se encuentra la ventana."),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string AssemblyName
        {
            get
            {
                if ((_assemblyName == null) || (_assemblyName.Equals(string.Empty)))
                    _assemblyName = System.Reflection.Assembly.GetAssembly(_managedUserControl.GetType()).GetName().Name;
                return (_assemblyName);
            }
            set
            {
                if (value == null) throw new ArgumentNullException(MsgManager.ErrorMsg("MSG_CultureAddInWPF_000", "null"));
                //if (value.Equals(string.Empty)) throw new ArgumentException(MsgManager.ErrorMsg("MSG_CultureAddInWPF_000", string.Empty));
                _assemblyName = value;
            }
        }

        /// <summary>
        /// Obtiene/Establece el nombre del fichero que contiene los recursos de la aplicación.
        /// </summary>
        [Browsable(true),
         Category("Extended Properties"),
         Description("Nombre común de los ficheros de recursos (normalmente el ensamblado)."),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string FileNameResources
        {
            get
            {
                if ((_sFileNameResources == null) || (_sFileNameResources.Equals(string.Empty)))
                    _sFileNameResources = System.Reflection.Assembly.GetAssembly(_managedUserControl.GetType()).GetName().Name;
                return (_sFileNameResources);
            }
            set
            {
                if (value == null) throw new ArgumentNullException(MsgManager.ErrorMsg("MSG_CultureAddInWPF_001", "null"));
                //if (value.Equals(string.Empty)) throw new ArgumentException(MsgManager.ErrorMsg("MSG_CultureAddInWPF_001", string.Empty));
                _sFileNameResources = value;
            }
        }

        /// <summary>
        /// Obtiene/Establece el nombre del fichero que contiene los recursos de la aplicación.
        /// </summary>
        [Browsable(true),
         Category("Extended Properties"),
         Description("Directorio en el que se encuentran los ficheros de recursos."),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public string DirectroryResources
        {
            get
            {
                if ((_sDirectoryResources == null) || (_sDirectoryResources.Equals(string.Empty)))
                    _sDirectoryResources = @"component/recursos/resources/";
                return (_sDirectoryResources);
            }
            set
            {
                if (value == null) throw new ArgumentNullException(MsgManager.ErrorMsg("MSG_CultureAddInWPF_002", "null"));
                //if (value.Equals(string.Empty)) throw new ArgumentException(MsgManager.ErrorMsg("MSG_CultureAddInWPF_002", string.Empty));
                _sDirectoryResources = value;
            }
        }

        /// <summary>
        /// Obtiene/Establece el control que implementará los procedimientos de localización. Es importante que esté aquí aunque 
        /// no  se  visualize ni sea usada por el usuario, ya que de esta manera el diseñador crea la línea de código que asigna
        /// el formulario al que desea aplicar la localización a este atributo. 
        /// </summary>
        [Browsable(false),
         Category("Extended Properties"),
         Description("Control que implementará los procedimientos de localización."),
         DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public UserControl ManagedUserControl
        {
            get
            {
                if (_managedUserControl == null) _managedUserControl = FindParentUserControl(this);
                return (_managedUserControl);
            }
            set
            {
                if (value == null) throw new ArgumentNullException(MsgManager.ErrorMsg("MSG_CultureAddInWPF_003", "null"));
                _managedUserControl = value;
            }
        }

        /// <summary>
        /// Busca la ventana en la que se ha incrustado el AddIn.
        /// </summary>
        /// <param name="child">Elemento del que se busca el padre.</param>
        /// <returns>'null', si el padre NO es una ventana, Ventana, si el padre es la ventana en la que está el control.</returns>
        private UserControl FindParentUserControl(DependencyObject child)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(child);
            if (parent == null) return (null);
            UserControl parentUserControl = parent as UserControl;
            if (parentUserControl != null) return (parentUserControl);
            else return (FindParentUserControl(parent));
        }

        #endregion

        #region Constructores y Destructores

        /// <summary>
        /// Constructor único para todas las instáncias de la clase.
        /// </summary>
        static CultureAddInUserControlWPF()
        {
            Type tControl = typeof(CultureAddInUserControlWPF);
            DefaultStyleKeyProperty.OverrideMetadata(tControl, new FrameworkPropertyMetadata(tControl));            
        }

        /// <summary>
        /// Constructor por defecto de la clase.
        /// </summary>
        public CultureAddInUserControlWPF()
        {
            CultureManager.evChangeCulture += new CultureManager.dlgChangeCulture(OnChangeCulture);
            ThemeManager.evChangeTheme += new ThemeManager.dlgChangeTheme(OnChangeTheme);
            this.Loaded += new RoutedEventHandler(OnCultureAddInUserConytrolWPFLoaded);
        }

        /// <summary>
        /// Destructor por defecto de la clase.
        /// </summary>
        ~CultureAddInUserControlWPF()
        {
            Dispose(false);
        }

        /// <summary>
        /// Implementación de la clase IDisposable.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Implementación de la clase IDisposable.
        /// </summary>
        protected void Dispose(bool disposed)
        {
            if (!disposed)
            {
                CultureManager.evChangeCulture -= new CultureManager.dlgChangeCulture(OnChangeCulture);
                disposed = true;
            }
        }

        #endregion

        #region Métodos

        #region Cambio de Cultura de los elementos del formulario.

        /// <summary>
        /// Método que se ejecuta al cambiar la cultura.
        /// </summary>
        public void OnChangeCulture()
        {
            if ((ManagedUserControl != null) && (SynchronizeCulture))
            {
                try
                {
                    ResourceDictionary dict = CultureManager.GetResourceDictionary(AssemblyName, DirectroryResources, FileNameResources, CultureManager.ActualCultureName);
                    if (_managedUserControl.Resources.MergedDictionaries.Count > 0)
                    {
                        if (rdActualCulture == null) _managedUserControl.Resources.MergedDictionaries.RemoveAt(0);
                        else _managedUserControl.Resources.MergedDictionaries.Remove(rdActualCulture);
                        _managedUserControl.Resources.MergedDictionaries.Insert(0, rdActualCulture = dict);
                    }
                    else
                    {
                        _managedUserControl.Resources.MergedDictionaries.Add(rdActualCulture = dict);
                        _managedUserControl.InvalidateVisual();
                    }
                }
                catch (Exception) { }
            }
        }

        #endregion

        #region Eventos de Tema

        /// <summary>
        /// Realiza las acciones pertinentes en el momento en que se detecta un cambio de tema. Se actualiza la cultura al mismo
        /// tiempo que el tema.
        /// </summary>
        private void OnChangeTheme()
        {
            if ((ManagedUserControl != null) && (SynchronizeTheme))
            {
                try
                {
                    ResourceDictionary dict = ThemeManager.GetResourceDictionary();
                    ResourceDictionary dictCulture = CultureManager.GetResourceDictionary(AssemblyName, DirectroryResources, FileNameResources, CultureManager.ActualCultureName);
                    if (_managedUserControl.Resources.MergedDictionaries.Count == 0)
                    {
                        _managedUserControl.Resources.MergedDictionaries.Add(dictCulture);
                        _managedUserControl.Resources.MergedDictionaries.Add(rdActualTheme = dict);
                        _managedUserControl.InvalidateVisual();
                    }
                    else if (_managedUserControl.Resources.MergedDictionaries.Count == 1)
                    {
                        if (rdActualTheme == null) _managedUserControl.Resources.MergedDictionaries.Add(rdActualTheme = dict);
                        else
                        {
                            if (_managedUserControl.Resources.MergedDictionaries.Contains(rdActualTheme))
                            {
                                _managedUserControl.Resources.MergedDictionaries.Remove(rdActualTheme);
                                _managedUserControl.Resources.MergedDictionaries.Add(dictCulture);
                            }
                            _managedUserControl.Resources.MergedDictionaries.Add(rdActualTheme = dict);
                        }
                    }
                    else if (_managedUserControl.Resources.MergedDictionaries.Count == 2)
                    {
                        if (rdActualTheme == null)
                        {
                            if (_managedUserControl.Resources.MergedDictionaries.IndexOf(rdActualCulture) == 0)
                            {
                                _managedUserControl.Resources.MergedDictionaries.RemoveAt(1);
                            }
                            else _managedUserControl.Resources.MergedDictionaries.RemoveAt(0);
                        }
                        else _managedUserControl.Resources.MergedDictionaries.Remove(rdActualTheme);
                        _managedUserControl.Resources.MergedDictionaries.Add(rdActualTheme = dict);
                    }
                }
                catch (Exception) { }
            }
        }

        #endregion

        #region Eventos del formulario

        /// <summary>
        /// Al cargar la ventana a la que pertenece aplica la cultura definida en la aplicación.
        /// </summary>
        private void OnCultureAddInUserConytrolWPFLoaded(object sender, RoutedEventArgs e)
        {
            OnChangeCulture();
            OnChangeTheme();
        }

        #endregion

        #endregion
    }
}
