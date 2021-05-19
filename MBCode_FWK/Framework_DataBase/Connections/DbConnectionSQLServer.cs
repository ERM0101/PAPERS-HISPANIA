#region Librerias usadas por esta clase

using MBCode.Framework.Managers.Messages;
using System;                           

#endregion

namespace MBCode.Framework.DataBase
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou
    /// Fecha última modificación: 01/03/2012
    /// Descripción: clase que define las operaciones de Gestión de Bases de Datos sobre SQL Server. 
    /// </summary>
    internal partial class DbConnectionSQLServer : IDbConnection
    {
        #region Métodos

        #region Creador de datos de las claves

        /// <summary>
        /// Método de la clase encargado de la inicialización de una conexión SQL Server. Los Parámetros
        /// esperados son los siguientes:
        /// 
        ///       [0]. Tipo de Base de Datos :- SQLSERVER
        ///       
        ///       [1]. Tipo de Autenticación :- Integrada | No Integrada
        /// 
        ///       [2]. DataSource            :- Servidor donde se encuentra la Base de Datos y nombre
        ///                                     de la instáncia de esta que se quiere utilizar.
        /// 
        ///       [3]. InitialCatalog        :- Nombre de la Base de Datos.
        /// 
        ///       [4]. User                  :- Nombre de usuario con el que se accede a la instáncia
        ///                                     de la Base de Datos.
        /// 
        ///       [5]. Password              :- Contraseña con la que se accede a la instáncia de la
        ///                                     Base de Datos.
        ///         
        /// </summary>
        /// <param name="guidApplication">Identificador de la aplicación que quiere crear la conexión.</param>
        /// <param name="iNumMaxConnections">Número máximo de conexiones a la Base de Datos</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <param name="optParams">Parámetros con los que se debe realizar la inicialización</param>
        /// <returns>no nulo, operación realizada correctamente, null, operación fallida</returns>
        internal static DbDataConnectionSQLServerEventArgs CreateDataKey(Guid? guidApplication, int iNumMaxConnections, 
                                                                         out string sMensaje, params object[] optParams)
        {
            sMensaje = string.Empty;
            try
            {
                //  Declaración de variables
                    DbDataConnectionSQLServerEventArgs oDataConnectionEA;
                //  Validación de parámetros.
                    if (guidApplication == null)
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_DbConnection(DataBase)_000");
                        return (null);
                    }
                //  Creación e inicialización de los datos a devolver.
                    oDataConnectionEA = new DbDataConnectionSQLServerEventArgs(iNumMaxConnections);
                    oDataConnectionEA.Identifier = null;
                    oDataConnectionEA.ApplicationID = guidApplication;
                    oDataConnectionEA.DataBaseType = DBType.SQLSERVER;
                //  Inicializa la conexión en función del tipo de seguridad que implementa.
                    switch (oDataConnectionEA.TipoAutenticacion = (AuthenticationType)optParams[1])
                    {
                        case AuthenticationType.WINDOWS_AUTHENTICATE:
                             if (!Initialize_WINDOWS_SECURITY(ref oDataConnectionEA, out sMensaje, optParams)) return (null);
                             break;
                        case AuthenticationType.SQL_SERVER_AUTHENTICATE:
                             if (!Initialize_SQLSERVER_SECURITY(ref oDataConnectionEA, out sMensaje, optParams)) return (null);
                             break;
                        case AuthenticationType.UNDEFINED:
                        default:
                             sMensaje = MsgManager.ErrorMsg("MSG_DbConnection(DataBase)_001");
                             return (null);
                    }
                //  Crea la nueva conexión con los datos leídos.
                    return (oDataConnectionEA);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        #endregion

        #region Crear una nueva conexión

        /// <summary>
        /// Método de la clase encargado de la inicialización de una conexión SQL Server. 
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación para la que se desea crear la nueva conexión.</param>
        /// <param name="guidConnection">Identificador de la conexión a crear.</param>
        /// <param name="eaDCSQLServer">Parámetros con los que se ha de crear la nueva conexión</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>no nulo, operación realizada correctamente, null, operación fallida</returns>
        internal static DbDataConnectionSQLServer CreateNewConnection(DbKeyApp dbkaApplication, Guid? guidConnection,
                                                                      DbDataConnectionSQLServerEventArgs eaDCSQLServer, out string sMensaje)
        {
            sMensaje = string.Empty;
            try
            {
                //  Variables
                    DbDataConnectionSQLServer oDbDataConnection;

                //  Valida que el identificador de conexión sea válido.
                    if (guidConnection == null)
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_DbConnection(DataBase)_002");
                        return (null);
                    }
                    if (dbkaApplication == null)
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_DbConnection(DataBase)_000");
                        return (null);
                    }
                //  Crea la nueva conexión con los datos leídos.
                    bool bResult;
                    eaDCSQLServer.Identifier = guidConnection;
                    oDbDataConnection = new DbDataConnectionSQLServer(dbkaApplication, eaDCSQLServer, out bResult, out sMensaje);
                    return ((bResult)? oDbDataConnection : null);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        #endregion

        #region Auxiliares

        #region Construcción de la Cadena de Conexión

        #region Seguridad Intregrada con Windows

        /// <summary>
        /// Método encargado de la inicialización de una conexión SQL Server que utiliza la Seguridad de Windows para
        /// acceder a la Base de Datos. Los Parámetros esperados para este tipo de conexión son los siguientes:
        /// 
        /// 
        ///       [0]. Tipo de Base de Datos :- SQLSERVER
        ///       
        ///       [1]. Tipo de Autenticación :- Integrada | No Integrada
        /// 
        ///       [2]. DataSource     :- Servidor donde se encuentra la Base de Datos y nombre
        ///                              de la instáncia de esta que se quiere utilizar.
        /// 
        ///       [3]. InitialCatalog :- Nombre de la Base de Datos.
        /// 
        /// </summary>
        /// <param name="oDataConnectionEA">Clase en la que se almacenan los datos con los que se crea la conexión.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <param name="optParams">Parámetros con los que se debe realizar la inicialización</param>
        /// <returns>true, operación realizada correctamente, false, operación fallida</returns>
        private static bool Initialize_WINDOWS_SECURITY(ref DbDataConnectionSQLServerEventArgs oDataConnectionEA, out string sMensaje, params object[] optParams)
        {
            sMensaje = string.Empty;
            try
            {
                //  Comprueba que tenga un mínimo de parámetros
                    if (optParams.Length != 4)
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_DbConnection(DataBase)_003");
                        return (false);
                    }
                //  Inicializa la Conexión.
                    oDataConnectionEA.DataSource = optParams[2].ToString();
                    oDataConnectionEA.InitialCatalog = optParams[3].ToString();
                    oDataConnectionEA.User = string.Empty;
                    oDataConnectionEA.Password = string.Empty;
                    oDataConnectionEA.ConnectionString =
                         string.Format("Data Source={0};Initial Catalog={1};Integrated Security=True;Connection Timeout = {2};MultipleActiveResultSets=true;",
                                       oDataConnectionEA.DataSource, oDataConnectionEA.InitialCatalog,
                                       DbConstants.TIMEOUT_CONNECTION_SQLSERVER);
                    return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Seguridad Propia de SQL Server

        /// <summary>
        /// Método encargado de la inicialización de una conexión SQL Server que utiliza la Seguridad propia de SQL Server
        /// para acceder a la Base de Datos. Los Parámetros esperados para este tipo de conexión son los siguientes:  
        /// 
        ///       [0]. Tipo de Base de Datos :- SQLSERVER
        ///       
        ///       [1]. Tipo de Autenticación :- Integrada | No Integrada
        /// 
        ///       [2]. DataSource            :- Servidor donde se encuentra la Base de Datos y nombre
        ///                                     de la instáncia de esta que se quiere utilizar.
        /// 
        ///       [3]. InitialCatalog        :- Nombre de la Base de Datos.
        /// 
        ///       [4]. User                  :- Nombre de usuario con el que se accede a la instáncia
        ///                                     de la Base de Datos.
        /// 
        ///       [5]. Password              :- Contraseña con la que se accede a la instáncia de la
        ///                                     Base de Datos.
        ///
        /// </summary>
        /// <param name="oDataConnectionEA">Clase en la que se almacenan los datos con los que se crea la conexión.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <param name="optParams">Parámetros con los que se debe realizar la inicialización</param>
        /// <returns>true, operación realizada correctamente, false, operación fallida</returns>
        private static bool Initialize_SQLSERVER_SECURITY(ref DbDataConnectionSQLServerEventArgs oDataConnectionEA, out string sMensaje, params object[] optParams)
        {
            sMensaje = string.Empty;
            try
            {
                //  Comprueba que tenga un mínimo de parámetros
                    if (optParams.Length != 6)
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_DbConnection(DataBase)_003");
                        return (false);
                    }
                //  Inicializa la Conexión.
                    oDataConnectionEA.DataSource = optParams[2].ToString();
                    oDataConnectionEA.InitialCatalog = optParams[3].ToString();
                    oDataConnectionEA.User = optParams[4].ToString();
                    oDataConnectionEA.Password = optParams[5].ToString();
                    oDataConnectionEA.ConnectionString =
                         string.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};Integrated Security=False;Connection Timeout = {4};MultipleActiveResultSets=true;",
                                       oDataConnectionEA.DataSource, oDataConnectionEA.InitialCatalog,
                                       oDataConnectionEA.User, oDataConnectionEA.Password, DbConstants.TIMEOUT_CONNECTION_SQLSERVER);
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

        #endregion

        #endregion
    }
}

