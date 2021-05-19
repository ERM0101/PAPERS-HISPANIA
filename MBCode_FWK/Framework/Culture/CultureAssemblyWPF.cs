#region Librerias usadas por la clase

using MBCode.Framework.Managers.Logs;
using MBCode.Framework.Managers.Messages;
using System;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

#endregion

namespace MBCode.Framework.Managers.Culture
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha: 21/02/2012.
    /// Descripción: elemento que se incrusta en cada control o ventana WPF en la que se desee que se aplique el cambio de
    ///              cultura.
    /// </summary>
    public class CultureAssemblyWPF : IDisposable
    {
        #region Atributos

        /// <summary>
        /// Almacena el nombre del ensamblado en el que se encuentra la ventana.
        /// </summary>
        private string _assemblyName = null;

        /// <summary>
        /// Almacena una referencia al 'objeto' en el que se crea el objeto cultura.
        /// </summary>
        private object _objectParent = null;

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
        /// Almacena una referencia al diccionario actual.
        /// </summary>
        private ResourceDictionary _rdActualDictionary = null;

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene/Establece un valor que indica si se debe llevar a cabo o no la sincronización del elemento en el 
        /// que se ha colocado el AddIn.
        /// </summary>
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
        /// Obtiene/Establece el nombre del fichero que contiene los recursos de la aplicación.
        /// </summary>
        public string AssemblyName
        {
            get
            {
                if ((_assemblyName == null) || (_assemblyName.Equals(string.Empty)))
                    _assemblyName = Assembly.GetAssembly(_objectParent.GetType()).GetName().Name;
                return (_assemblyName);
            }
            set
            {
                if (value == null) throw new ArgumentNullException(MsgManager.ErrorMsg("MSG_CultureAssemblyWPF_000", "null"));
                if (value.Equals(string.Empty)) throw new ArgumentException(MsgManager.ErrorMsg("MSG_CultureAssemblyWPF_000", string.Empty));
                _assemblyName = value;
            }
        }

        /// <summary>
        /// Obtiene/Establece el nombre del fichero que contiene los recursos de la aplicación.
        /// </summary>
        public string FileNameResources
        {
            get
            {
                if ((_sFileNameResources == null) || (_sFileNameResources.Equals(string.Empty)))
                    _sFileNameResources = Assembly.GetAssembly(_objectParent.GetType()).GetName().Name;
                return (_sFileNameResources);
            }
            set
            {
                if (value == null) throw new ArgumentNullException(MsgManager.ErrorMsg("MSG_CultureAssemblyWPF_001", "null"));
                if (value.Equals(string.Empty)) throw new ArgumentException(MsgManager.ErrorMsg("MSG_CultureAssemblyWPF_001", string.Empty));
                _sFileNameResources = value;
            }
        }

        /// <summary>
        /// Obtiene/Establece el nombre del fichero que contiene los recursos de la aplicación.
        /// </summary>
        public string DirectoryResources
        {
            get
            {
                if ((_sDirectoryResources == null) || (_sDirectoryResources.Equals(string.Empty))) 
                    _sDirectoryResources = @"component/recursos/resources/";
                return (_sDirectoryResources);
            }
            set
            {
                if (value == null) throw new ArgumentNullException(MsgManager.ErrorMsg("MSG_CultureAssemblyWPF_002", "null"));
                if (value.Equals(string.Empty)) throw new ArgumentException(MsgManager.ErrorMsg("MSG_CultureAssemblyWPF_002", string.Empty));
                _sDirectoryResources = value;
            }
        }

        /// <summary>
        /// Obtiene una referencia al 'objeto' en el que se crea el objeto cultura.
        /// </summary>
        public object ObjectParent
        {
            get
            {
                return (_objectParent);
            }
        }


        /// <summary>
        /// Almacena una referencia al diccionario actual.
        /// </summary>
        public ResourceDictionary Resources
        {
            get
            {
                if (_rdActualDictionary == null) LoadCulture();
                return (_rdActualDictionary);
            }
        }

        #endregion

        #region Constructores y Destructores

        /// <summary>
        /// Constructor necesario para que se pueda definir en una Dll mediante el AssemblyName.
        /// </summary>
        public CultureAssemblyWPF(string sAssemblyName, string sFileName)
        {
            AssemblyName = sAssemblyName;
            FileNameResources = sFileName;
            _objectParent = sAssemblyName;
            try
            {
                CultureManager.evChangeCulture += new CultureManager.dlgChangeCulture(OnChangeCulture);
            }
            catch { }
            OnChangeCulture();
        }

        /// <summary>
        /// Constructor por defecto de la clase.
        /// </summary>
        public CultureAssemblyWPF(object objectParent)
        {
            _objectParent = objectParent;
            try
            {
                CultureManager.evChangeCulture += new CultureManager.dlgChangeCulture(OnChangeCulture);
            }
            catch { } OnChangeCulture();
        }

        /// <summary>
        /// Destructor por defecto de la clase.
        /// </summary>
        ~CultureAssemblyWPF()
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
                try
                {
                    CultureManager.evChangeCulture -= new CultureManager.dlgChangeCulture(OnChangeCulture);
                }
                catch { }
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
            if ((ObjectParent != null) && (SynchronizeCulture))
            {
                LoadCulture();
                if (_objectParent is Application) ((Application)_objectParent).Resources = _rdActualDictionary;
            }
        }

        /// <summary>
        /// Método encargado de cargar el fichero de recursos correcto.
        /// </summary>
        private void LoadCulture()
        {
            try
            {
                _rdActualDictionary = 
                    CultureManager.GetResourceDictionary(AssemblyName, DirectoryResources, FileNameResources, CultureManager.ActualCultureName);
            }
            catch (Exception ex) 
            {
                Manager_Log.WriteLogFRW(string.Format("Error, in LoadCulture() method: {0}\r\n.", MsgManager.ExcepMsg(ex)));
            }
        }

        #endregion

        #region Búsqueda de contenido en el fichero de recursos definido.

        /// <summary>
        /// Obtiene el recurso asociado al valor de la clave pasada como parámetro.
        /// </summary>
        /// <param name="_objectKey">Clave del objeto dentro del fichero de recursos.</param>
        /// <returns>objeto del fichero de recursos.</returns>
        public object FindResource(object _objectKey)
        {
            return (Resources[_objectKey]);
        }

        /// <summary>
        /// Obtiene el Image asociado al valor de la clave pasada como parámetro.
        /// </summary>
        /// <param name="_objectKey">Clave del objeto dentro del fichero de recursos.</param>
        /// <returns>objeto del fichero de recursos.</returns>
        public Image FindImageResource(object _objectKey)
        {
            try
            {
                Image image = new Image();
                image.Source = FindBitmapImageResource("Accept");
                return (image);
            }
            catch (Exception)
            {
                return (null);
            }
        }

        /// <summary>
        /// Obtiene el BitmapImage asociado al valor de la clave pasada como parámetro.
        /// </summary>
        /// <param name="_objectKey">Clave del objeto dentro del fichero de recursos.</param>
        /// <returns>objeto del fichero de recursos.</returns>
        public BitmapImage FindBitmapImageResource(object _objectKey)
        {
            try
            {
                return (new BitmapImage(new Uri(FindResource(_objectKey).ToString())));
            }
            catch (Exception)
            {
                return (null);
            }
        }

        /// <summary>
        /// Obtiene el texto asociado al valor de la clave pasada como parámetro.
        /// </summary>
        /// <param name="_objectKey">Clave del objeto dentro del fichero de recursos.</param>
        /// <returns>objeto del fichero de recursos.</returns>
        public string FindTextResource(object _objectKey)
        {
            try
            {
                return (FindResource(_objectKey).ToString());
            }
            catch (Exception)
            {
                return (string.Format("Error Resource en key [{0}]", ((_objectKey == null)? "null" : _objectKey.ToString())));
            }
        }

        /// <summary>
        /// Obtiene el texto asociado al valor de la clave pasada como parámetro.
        /// </summary>
        /// <param name="_objectKey">Clave del objeto dentro del fichero de recursos.</param>
        /// <returns>objeto del fichero de recursos.</returns>
        public string FindTextResource(object _objectKey, object[] oParams)
        {
            try
            {
                return (string.Format(FindResource(_objectKey).ToString(), oParams));
            }
            catch (Exception)
            {
                return (string.Format("Error Resource en key [{0}]" , ((_objectKey == null)? "null" : _objectKey.ToString())));
            }
        }

        #endregion

        #endregion
    }
}
