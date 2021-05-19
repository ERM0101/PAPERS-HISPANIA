#region Librerias usadas por la clase

using MBCode.Framework.Managers.Messages;
using System;

#endregion

namespace MBCode.Framework.Managers.Theme
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha: 23/02/2012.
    /// Descripción: clase que almacena la información de un Tema.
    /// </summary>
    public class ThemeInfo
    {
        #region Constantes

        /// <summary>
        /// Nombre del ensamblador del Framework.
        /// </summary>
        private const string sAssembleNameFRWK = @"MBCode.Framework";

        /// <summary>
        /// Directorio donde se encuentra el fichero de recursos del Framework.
        /// </summary>
        private const string sPathFileResourcesFWK = @"component/Recursos/Themes/";

        #endregion

        #region Atributos
        
        /// <summary>
        /// Almacena el nombre del ensamblador que contiene el Tema.
        /// </summary>
        private string _sAssemblyName = null;

        /// <summary>
        /// Almacena el directorio de  donde se encuentra el fichero que contiene el Tema.
        /// </summary>
        private string _sPathFileTheme = null;

        /// <summary>
        /// Almacena el nombre del fichero en que se almacena el tema.
        /// </summary>
        private string _sFileNameTheme = null;
       
        /// <summary>
        /// Almacena el tema predefinido asociado al elemento.
        /// </summary>
        private Themes _eTheme = Themes.Undefined;

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene el nombre del ensamblador que contiene el Tema.
        /// </summary>
        public string AssemblyName
        {
            get { return _sAssemblyName; }
        }

        /// <summary>
        /// Obtiene el directorio de  donde se encuentra el fichero que contiene el Tema.
        /// </summary>
        public string PathFileTheme
        {
            get { return _sPathFileTheme; }
        }

        /// <summary>
        /// Obtiene el nombre del fichero en que se almacena el tema.
        /// </summary>
        public string FileNameTheme
        {
            get { return (_sFileNameTheme); }
        }

        /// <summary>
        /// Obtiene el tema predefinido asociado al elemento.
        /// </summary>
        public Themes Theme
        {
            get { return (_eTheme); }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Crea un nuevo elemento de información de Tema.
        /// </summary>
        /// <param name="eTheme">Tema predefinido en el Framework que se desea aplicar.</param>
        public ThemeInfo(Themes eTheme)
        {
            if (eTheme == Themes.Undefined) throw new ArgumentException(MsgManager.ErrorMsg("MSG_ThemeInfo_000"));
            _sAssemblyName = sAssembleNameFRWK;
            _sPathFileTheme = sPathFileResourcesFWK;
            _sFileNameTheme = eTheme.ToString();
            _eTheme = eTheme;
        }

        /// <summary>
        /// Crea un nuevo elemento de información de Tema.
        /// </summary>
        /// <param name="sAssembleName">Nombre del ensamblador que contiene el Tema.</param>
        /// <param name="sPathFileTheme">Directorio de  donde se encuentra el fichero que contiene el Tema.</param>
        /// <param name="sFileNameTheme">Nombre del fichero en que se almacena el tema.</param>
        public ThemeInfo(string sAssemblyName, string sPathFileTheme, string sFileNameTheme)
        {
            if (String.IsNullOrEmpty(sAssemblyName)) throw new ArgumentNullException(MsgManager.ErrorMsg("MSG_ThemeInfo_001", "null or empty"));
            if (String.IsNullOrEmpty(sPathFileTheme)) throw new ArgumentNullException(MsgManager.ErrorMsg("MSG_ThemeInfo_002", "null or empty"));
            if (String.IsNullOrEmpty(sFileNameTheme)) throw new ArgumentNullException(MsgManager.ErrorMsg("MSG_ThemeInfo_003", "null or empty"));
            _sAssemblyName = sAssemblyName;
            _sPathFileTheme = sPathFileTheme;
            _sFileNameTheme = sFileNameTheme;
            _eTheme = Themes.AppTheme;
        }

        #endregion

        #region Métodos

        #region Redefinición de los métodos de igualdad

        /// <summary>
        /// Override 'Equals' method.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>true, if both items are equals, false, otherwise.</returns>
        public override bool Equals(Object obj)
        {
            //  If parameter is null.
                if (obj == null) return (false);
            //  If parameter cannot be cast to Point return false.
                ThemeInfo p = obj as ThemeInfo;
                if ((Object)p == null) return (false);
            //  Return true if the fields match:
                return (((_eTheme == Themes.Undefined) && ((_sAssemblyName == p._sAssemblyName) &&
                         (_sFileNameTheme == p._sFileNameTheme) && (_sPathFileTheme == p._sPathFileTheme))) ||
                        ((_eTheme != Themes.Undefined) && (_eTheme == p._eTheme)));
        }

        /// <summary>
        /// Override 'Equals' method.
        /// </summary>
        /// <param name="p">Object to compare.</param>
        /// <returns>true, if both items are equals, false, otherwise.</returns>
        public bool Equals(ThemeInfo p)
        {
            //  If parameter is null return false:
                if ((object)p == null) return (false);
            //  Return true if the fields match:
                return (((_eTheme == Themes.Undefined) && ((_sAssemblyName == p._sAssemblyName) &&
                         (_sFileNameTheme == p._sFileNameTheme) && (_sPathFileTheme == p._sPathFileTheme))) ||
                        ((_eTheme != Themes.Undefined) && (_eTheme == p._eTheme)));
        }

        /// <summary>
        /// Override the operation '=='.
        /// </summary>
        /// <param name="a">First object to compare.</param>
        /// <param name="b">Second object to compare.</param>
        /// <returns>true, if both items are equals, false, otherwise.</returns>
        public static bool operator ==(ThemeInfo a, ThemeInfo b)
        {
            //  If both are null, or both are same instance, return true.
                if (Object.ReferenceEquals(a, b)) return (true);
            //  If one is null, but not both, return false.
                if (((object)a == null) || ((object)b == null)) return (false);
            //  Return true if the fields match:
                if (a._eTheme != b._eTheme) return (false);
                if ((a._eTheme == Themes.Undefined) || (a._eTheme == Themes.AppTheme))
                {
                    return ((a._sAssemblyName == b._sAssemblyName) && (a._sFileNameTheme == b._sFileNameTheme) && 
                            (a._sPathFileTheme == b._sPathFileTheme));
                }
                else return (a._eTheme == b._eTheme);
        }

        /// <summary>
        /// Override the operation '!='.
        /// </summary>
        /// <param name="a">First object to compare.</param>
        /// <param name="b">Second object to compare.</param>
        /// <returns>true, if both items NOT are equals, false, otherwise.</returns>
        public static bool operator !=(ThemeInfo a, ThemeInfo b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Método que obtiene el Hash.
        /// </summary>
        /// <returns>Código de Hash</returns>
        public override int GetHashCode()
        {
            return (base.GetHashCode());
        }

        #endregion

        #endregion
    }
}
