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
    internal partial class DbConnectionORACLE : IDbConnection
    {
        #region Métodos

        #region Creador de datos de las claves

        /// <summary>
        /// Método de la clase encargado de la inicialización de una conexión SQL Server. Los Parámetros
        /// esperados son los siguientes:
        /// 
        ///       [0]. Tipo de Base de Datos :- ORACLE
        ///       
        ///       [1]. Tipo de Autenticación :- UnDef (no definida).
        /// 
        ///       [2]. DataSource            :- Servidor donde se encuentra la Base de Datos y nombre
        ///                                     de la instáncia de esta que se quiere utilizar.
        /// 
        ///       [3]. User                  :- Nombre de usuario con el que se accede a la instáncia
        ///                                     de la Base de Datos.
        /// 
        ///       [4]. Password              :- Contraseña con la que se accede a la instáncia de la
        ///                                     Base de Datos.
        ///         
        /// </summary>
        /// <param name="guidApplication">Identificador de la aplicación que quiere crear la conexión.</param>
        /// <param name="iNumMaxConnections">Número máximo de conexiones a la Base de Datos</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <param name="optParams">Parámetros con los que se debe realizar la inicialización</param>
        /// <returns>no nulo, operación realizada correctamente, null, operación fallida</returns>
        internal static DbDataConnectionORACLEEventArgs CreateDataKey(Guid? guidApplication, int iNumMaxConnections, 
                                                                      out string sMensaje, params object[] optParams)
        {
            sMensaje = string.Empty;
            try
            {
                //  Declaración de variables
                    DbDataConnectionORACLEEventArgs oDataConnectionEA;
                //  Validación de parámetros.
                    if (guidApplication == null)
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_DbConnection(DataBase)_000");
                        return (null);
                    }
                //  Creación e inicialización de los datos a devolver.
                    oDataConnectionEA = new DbDataConnectionORACLEEventArgs(iNumMaxConnections);
                    oDataConnectionEA.Identifier = null;
                    oDataConnectionEA.ApplicationID = guidApplication;
                    oDataConnectionEA.DataBaseType = DBType.ORACLE;
                //  Inicializa la conexión en función del tipo de seguridad que implementa.
                    switch (oDataConnectionEA.TipoAutenticacion = (AuthenticationType)optParams[1])
                    {
                        case AuthenticationType.UNDEFINED:
                             if (!Initialize_ORACLE(ref oDataConnectionEA, out sMensaje, optParams)) return (null);
                             break;
                        case AuthenticationType.WINDOWS_AUTHENTICATE:
                        case AuthenticationType.SQL_SERVER_AUTHENTICATE:
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
        internal static DbDataConnectionORACLE CreateNewConnection(DbKeyApp dbkaApplication, Guid? guidConnection,
                                                                   DbDataConnectionORACLEEventArgs eaDCORACLE,
                                                                   out string sMensaje)
        {
            sMensaje = string.Empty;
            try
            {
                //  Variables
                    DbDataConnectionORACLE oDbDataConnection;

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
                    eaDCORACLE.Identifier = guidConnection;
                    oDbDataConnection = new DbDataConnectionORACLE(dbkaApplication, eaDCORACLE, out bResult, out sMensaje);
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

        /// <summary>
        /// Método encargado de la inicialización de una conexión SQL Server que utiliza la Seguridad propia de SQL Server
        /// para acceder a la Base de Datos. Los Parámetros esperados para este tipo de conexión son los siguientes:  
        /// 
        ///       [0]. Tipo de Base de Datos :- ORACLE
        ///       
        ///       [1]. Tipo de Autenticación :- UnDef
        /// 
        ///       [2]. DataSource            :- Nombre de la Base de Datos.
        /// 
        ///       [3]. User                  :- Nombre de usuario con el que se accede a la instáncia
        ///                                     de la Base de Datos.
        /// 
        ///       [4]. Password              :- Contraseña con la que se accede a la instáncia de la
        ///                                     Base de Datos.
        ///
        /// </summary>
        /// <param name="oDataConnectionEA">Clase en la que se almacenan los datos con los que se crea la conexión.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <param name="optParams">Parámetros con los que se debe realizar la inicialización</param>
        /// <returns>true, operación realizada correctamente, false, operación fallida</returns>
        private static bool Initialize_ORACLE(ref DbDataConnectionORACLEEventArgs oDataConnectionEA, out string sMensaje, 
                                               params object[] optParams)
        {
            sMensaje = string.Empty;
            try
            {
                //  Comprueba que tenga un mínimo de parámetros
                    if (optParams.Length != 5)
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_DbConnection(DataBase)_003");
                        return (false);
                    }
                //  Inicializa la Conexión.
                    oDataConnectionEA.DataSource = optParams[2].ToString();
                    oDataConnectionEA.User = optParams[3].ToString();
                    oDataConnectionEA.Password = optParams[4].ToString();
                    oDataConnectionEA.ConnectionString =
                         string.Format("Data Source={0};User Id={1};Password={2};",
                                       oDataConnectionEA.DataSource, oDataConnectionEA.User, oDataConnectionEA.Password);
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
    }
}

