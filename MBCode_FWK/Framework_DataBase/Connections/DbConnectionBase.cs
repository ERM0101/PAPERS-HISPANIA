#region Librerias usadas por la clase

using MBCode.Framework.Managers.DataBase;
using MBCode.Framework.Managers.Messages;
using System;
using System.Collections;

#endregion

namespace MBCode.Framework.DataBase
{
    public class DbConnectionBase
    {
        #region Atributos

        private Hashtable m_htParametros;
        
        // ELM 31/03/2010: Se han convertido estas dos variables en estáticas. 
        // Necesario para que funcionen los ProxyDAL tal y como están construidos ahora.
        // solo existirá un dbkapp y un guidApllication para todos los que usen esta instancia del Framework

        private static Guid? m_guidApplication;

        protected static DbKeyApp dbkaApp;

        private string sDataConnectionInfo;

        #endregion

        #region Propiedades

        public string DataConnectionInfo
        {
            get 
            {
                return (sDataConnectionInfo);
            }
        }
        #endregion

        #region Métodos

        #region Abrir Acceso a Base de Datos

        /// <summary>
        /// Método encargado de Obtener el identificador de una conexión activa y libre.
        /// </summary>
        /// <param name="sMensaje">mensaje de error, si se produce uno,</param>
        /// <param name="oParams">Parámetros con los que se debe crear la conexión.</param>
        /// <returns>Mirar definición del enumerado</returns>
        public ResultOpBD AbrirAccesoBBDD(out string sMensaje, params object[] oParams)
        {
            sMensaje = string.Empty;
            try
            {
                ResultOpBD eResult = ResultOpBD.Correct;
                if ((eResult = ConnectionManager.Initialize(null, out sMensaje)) == ResultOpBD.Correct)
                {
                    if (m_guidApplication == null) m_guidApplication = Guid.NewGuid();
                    eResult = ConnectionManager.GetKeyAppBD(m_guidApplication, int.Parse(m_htParametros["NumeroMaximoConexionesConcurrentes"].ToString()), out dbkaApp, out sMensaje, oParams);
                    if (eResult == ResultOpBD.Correct)
                    {
                        // Improve: operaciones posteriores a la obtención de la clave de aplicación.
                    }
                }
                return (eResult);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (ResultOpBD.Error);
            }
        }

        #endregion

        #region Cerrar Acceso a la Base de Datos

        /// <summary>
        /// Método encargado de cerrar las conexiones abiertas por el servicio web en el que se ejecuta.
        /// </summary>
        /// <param name="sMensaje">mensaje de error, si se produce uno,</param>
        /// <returns>Mirar definición del enumerado</returns>
        public ResultOpBD CerrarAccesoBBDD(out string sMensaje)
        {
            sMensaje = string.Empty;
            try
            {
                return (ConnectionManager.Finalize(out sMensaje));

            }
            catch (Exception ex)
            {
                sMensaje = ex.Message;
                return (ResultOpBD.InternalError);
            }
        }

        #endregion

        #region Cargar parámetros del fichero de configuración

        public string GetConnectionString()
        {
            string sConnectionString = string.Empty;

            switch (m_htParametros["TipoBD"].ToString().ToUpper())
            {
                case "PROGRESS":
                     // string.Format("DSN={0};DB={1};UID={2};PWD={3};", this.DSN, m_InitialCatalog, m_User, m_Password);
                     break;
                case "SQLSERVER":
                     sConnectionString = 
                            string.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};Integrated Security=False;Connection Timeout = {4};",
                                          m_htParametros["DataSourceSQLServer"], m_htParametros["InitialCatalogSQLServer"],
                                          m_htParametros["UserSQLServer"], m_htParametros["PasswordSQLServer"],
                                          DbConstants.TIMEOUT_CONNECTION_SQLSERVER);
                     break;
                case "ORACLE":
                     break;
                case "UNDEFINED":
                     break;
            }
            return (sConnectionString);
        }

        /// <summary>
        /// Inicializa los parámetros, pasando una HashTable con los parámetros inicializados
        /// </summary>
        /// <param name="htParametros"></param>
        public bool InicializarParametros(Hashtable htParametros, ref string sMensaje)
        {
            m_htParametros = htParametros;

            sDataConnectionInfo = "<Sin Información>";
            switch (m_htParametros["TipoBD"].ToString().ToUpper())
            {
                case "PROGRESS":
                     m_htParametros.Remove("TipoBD");
                     m_htParametros.Add("TipoBD", DBType.PROGRESS);
                     break;
                case "SQLSERVER":
                     m_htParametros.Remove("TipoBD");
                     m_htParametros.Add("TipoBD", DBType.SQLSERVER);
                     sDataConnectionInfo = htParametros["DataSourceSQLServer"] + " : " +
                                           htParametros["InitialCatalogSQLServer"];
                     switch (htParametros["AuthenticationSQLServer"].ToString().ToUpper())
                     {
                         case "WINDOWS_AUTHENTICATE":
                              htParametros.Remove("AuthenticationSQLServer");
                              htParametros.Add("AuthenticationSQLServer", AuthenticationType.WINDOWS_AUTHENTICATE);
                              break;
                         case "SQL_SERVER_AUTHENTICATE":
                              htParametros.Remove("AuthenticationSQLServer");
                              htParametros.Add("AuthenticationSQLServer", AuthenticationType.SQL_SERVER_AUTHENTICATE);
                              break;
                         case "UNDEFINED":
                         default:
                              htParametros.Remove("AuthenticationSQLServer");
                              htParametros.Add("AuthenticationSQLServer", AuthenticationType.UNDEFINED);
                              sMensaje = "Error al leer los parámetros de la dll: IntegraWS.\r\n" +
                                         "Tipo de acceso definido de manera incorrecta en el fichero de configuración.\r\n|";
                              return (false);
                     } 
                     break;
                case "ORACLE":
                     htParametros.Remove("TipoBD");
                     htParametros.Add("TipoBD", DBType.ORACLE);
                     sMensaje = "Error al leer los parámetros de la dll: IntegraWS.\r\n" +
                                "Acceso por el Tipo de Base de Datos ORACLE aún no implementado.\r\n|";
                     return (false);
                case "UNDEFINED":
                default:
                     htParametros.Remove("TipoBD");
                     htParametros.Add("TipoBD", DBType.UNDEFINED);
                     sMensaje = "Error al leer los parámetros de la dll: IntegraWS.\r\n" +
                                "Tipo de acceso a Base de Datos definido de manera incorrecta en el fichero de configuración.\r\n|";
                     return (false);
            }
            return (true);
        }

        #endregion

        #region Abrir Conexión

        /// <summary>
        /// Método encargado de Abrir la conexión si esta está cerrada o no iniciada.
        /// </summary>
        /// <param name="sMensaje">Mensaje de error, siempre que se produzca uno</param>
        /// <returns>true, conexión aierta correctamente, false, si se ha producido un error al abrir la conexión</returns>
        public bool AbrirConexion(out string sMsgError)
        {
            sMsgError = string.Empty;
            try
            {
                if (m_htParametros == null)
                {
                    m_guidApplication = null;
                    dbkaApp = null;
                    sMsgError = "Error en DbConectionBase.AbriConexion(): No se ha inicializado htParameters";
                    return false;
                }
                if (!ConnectionManager.IsInitialized())                
                {
                    object[] oParams;
                    switch (((DBType)m_htParametros["TipoBD"]))
                    {
                        case DBType.PROGRESS:
                             if (!ObtenerParametrosPROGRESS(out oParams, out sMsgError)) return (false);
                             break;
                        case DBType.SQLSERVER:
                             if (!ObtenerParametrosSQLServer(out oParams, out sMsgError)) return (false);
                             break;
                        default:
                             sMsgError = "Tipo de Base de Datos no reconocido.";
                             return (false);
                    }
                    if (AbrirAccesoBBDD(out sMsgError, oParams) != ResultOpBD.Correct)
                    {
                        sMsgError = "Error al obtener la clave de uso de la aplicación." + sMsgError;
                        return (false);
                    }
                }                
                return (true);
            }
            catch (Exception ex)
            {
                sMsgError = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #region PROGRESS

        /// <summary>
        /// Método encargado de crear los parámetros necesários para inicializar una conexión con un tipo de
        /// Base de Datos PROGRESS.
        /// </summary>
        /// <param name="oParams">Parámetros con los que se ha de crear la conexión.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, operación correcta, false, si error</returns>
        private bool ObtenerParametrosPROGRESS(out object[] oParams, out string sMensaje)
        {
            oParams = null;
            sMensaje = string.Empty;
            try
            {
                oParams = new object[] {DBType.PROGRESS, 
                                        m_htParametros["DSNProgress"].ToString(), 
                                        m_htParametros["InitialCatalogProgress"].ToString(),
                                        m_htParametros["UserProgress"].ToString(),
                                        m_htParametros["PasswordProgress"].ToString()};
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region SQL SERVER

        /// <summary>
        /// Método encargado de crear los parámetros necesários para inicializar una conexión con un tipo de
        /// Base de Datos SQL Server 7.0, 2000 y anteriores.
        /// </summary>
        /// <param name="oParams">Parámetros con los que se ha de crear la conexión.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, operación correcta, false, si error</returns>
        private bool ObtenerParametrosSQLServer(out object[] oParams, out string sMensaje)
        {
            oParams = null;
            sMensaje = string.Empty;
            try
            {
                AuthenticationType eAuthenticationType = (AuthenticationType)m_htParametros["AuthenticationSQLServer"];
                switch (eAuthenticationType)
                {
                    case AuthenticationType.SQL_SERVER_AUTHENTICATE:
                        oParams = new object[] {DBType.SQLSERVER, 
                                                 AuthenticationType.SQL_SERVER_AUTHENTICATE,
                                                 m_htParametros["DataSourceSQLServer"].ToString(), 
                                                 m_htParametros["InitialCatalogSQLServer"].ToString(), 
                                                 m_htParametros["UserSQLServer"].ToString(), 
                                                 m_htParametros["PasswordSQLServer"].ToString()};
                        break;
                    case AuthenticationType.WINDOWS_AUTHENTICATE:
                        oParams = new object[] {DBType.SQLSERVER, 
                                                 AuthenticationType.WINDOWS_AUTHENTICATE,
                                                 m_htParametros["DataSourceSQLServer"].ToString(), 
                                                 m_htParametros["InitialCatalogSQLServer"].ToString()};
                        break;
                    default:

                        break;
                }
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #endregion

        #region Cerrar Conexión

        /// <summary>
        /// Método encargado de Cerrar la conexión si esta está abierta.
        /// </summary>
        /// <returns>true, conexión aierta correctamente, false, si se ha producido un error al abrir la conexión</returns>
        private bool CerrarConexion(ref string sMensaje)
        {
            sMensaje = string.Empty;
            try
            {
                if (ConnectionManager.IsInitialized())
                {
                    return (CerrarAccesoBBDD(out sMensaje) == ResultOpBD.Correct);
                }
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #endregion
    }
}
