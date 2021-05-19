#region Librerias usadas por la clase

using System.Collections;
using MBCode.Framework.DataBase;
using MBCode.Framework.DataBase.DAL;

#endregion

namespace MBCode.Framework.DataBase.Proxy.DAL
{
    #region Enumerados

    /// <summary>
    /// Define el tipo de conexión que usará el Proxy.
    /// </summary>
    public enum ProxyDALTypeConn
    {
        /// <summary>
        /// Desde cada una de las aplicaciones donde se usa.
        /// </summary>
        Direct = 0,

        /// <summary>
        /// Mediante una comunicación WCF a uno o más servidores que realizará(án) todas las operaciones de Base de Datos.
        /// </summary>
        ByWCF = 1
    }
    
    #endregion

    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha última modificación: 07/03/2012.
    /// Descripción: clase base de los proxy's que realizarán las operaciones de Base de Datos.
    /// </summary>
    public class ProxyBase : DbConnectionBase
    {
        #region Atributos

        /// <summary>
        /// Almacena el tipo de Conexión.
        /// </summary>
        protected ProxyDALTypeConn _ETipoConexionProxyDAL;

        /// <summary>
        /// Almacena el estado de inicialización e la conexión.
        /// </summary>
        protected bool _Inicializado;

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor en el que se indica el tipo de conexión a usar.
        /// </summary>
        /// <param name="eTipoConexionProxyDal">Tipo de conexión a usar.</param>
        protected ProxyBase(ProxyDALTypeConn eTipoConexionProxyDal)
        {
            _ETipoConexionProxyDAL = eTipoConexionProxyDal;
            _Inicializado = false;
        }

        /// <summary>
        /// Constructor por defecto, sin parámetros, asume tipo de conexión directa..
        /// </summary>
        protected ProxyBase()
        {
            _ETipoConexionProxyDAL = ProxyDALTypeConn.Direct;
            _Inicializado = false;

        }

        #endregion

        #region Métodos

        /// <summary>
        /// Inicializa los parámetros de la 
        /// </summary>
        /// <param name="htParametros">Tabla de Hash que contiene los parámetros de la aplicación que ejecuta el Proxy.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno, string.Empty, en caso contrario.</param>
        /// <returns>true, si operación correcta, false, si error.</returns>
        public new bool InicializarParametros(Hashtable htParametros, ref string sMensaje)
        {
            if (htParametros.ContainsKey("TipoConexionProxyDAL"))
            {
                string sTipoConexionProxyDAL= htParametros["TipoConexionProxyDAL"].ToString();

                switch(sTipoConexionProxyDAL.ToUpper())
                {
                    case "DIRECT":
                         _ETipoConexionProxyDAL= ProxyDALTypeConn.Direct;
                         break;
                    case "WCF":
                         _ETipoConexionProxyDAL= ProxyDALTypeConn.ByWCF;
                         break;
                    default:
                         _ETipoConexionProxyDAL= ProxyDALTypeConn.Direct;
                         break;
                }                
            }
            if (!_Inicializado) _Inicializado = base.InicializarParametros(htParametros, ref sMensaje);
            return _Inicializado;
        }

        /// <summary>
        /// Testea el estado del 'ConnectionManager'.
        /// </summary>
        /// <param name="sMensaje">Mensaje de error, si se produce uno, string.Empty, en caso contrario.</param>
        /// <returns>true, si operacíón correcta, false, si error.</returns>
        public bool TestConnection(out string sMensaje)
        {
            if (AbrirConexion(out sMensaje)) return (SystemDAO.TestConnection(dbkaApp, out sMensaje));
            else return (false);
        }

        #endregion
    }
}
