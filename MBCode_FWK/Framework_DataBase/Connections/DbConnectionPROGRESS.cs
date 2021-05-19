#region Librerias usadas por esta clase

using MBCode.Framework.Managers.Messages;
using System;              

#endregion

namespace MBCode.Framework.DataBase
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou
    /// Fecha última modificación: 01/03/2012
    /// Descripción: clase que define las operaciones de Gestión de Bases de Datos sobre PROGRESS. 
    /// </summary>
    internal partial class DbConnectionPROGRESS : IDbConnection
    {
        #region Métodos

        #region Creador de datos de las claves

        /// <summary>
        /// Método de la clase encargado de la inicialización de una conexión SQL Server. Los Parámetros
        /// esperados son los siguientes:
        /// 
        ///       [0]. Tipo de Base de Datos :- PROGRESS
        ///       
        ///       [1]. DSN                   :- Nombre del Origen de Datos de Sistema que apunta a la  Base  
        ///                                     de Datos Progress (ODBC).
        /// 
        ///       [2]. InitialCatalog        :- Nombre de la Base de Datos con la que se quiere realizar la
        ///                                     conexión.
        /// 
        ///       [3]. User                  :- Nombre de usuario con el que se accede a la instáncia de la 
        ///                                     Base de Datos.
        /// 
        ///       [4]. Password              :- Contraseña con la que se accede a la instáncia de  la  Base
        ///                                     de Datos.
        ///         
        /// </summary>
        /// <param name="guidApplication">Identificador de la aplicación que quiere crear la conexión.</param>
        /// <param name="iNumMaxConnections">Número máximo de conexiones a la Base de Datos</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <param name="optParams">Parámetros con los que se debe realizar la inicialización</param>
        /// <returns>no nulo, operación realizada correctamente, null, operación fallida</returns>
        internal static DbDataConnectionPROGRESSEventArgs CreateDataKey(Guid? guidApplication, int iNumMaxConnections,
                                                                        out string sMensaje, params object[] optParams)
        {
            sMensaje = string.Empty;
            try
            {
                //  Declaración de variables
                    DbDataConnectionPROGRESSEventArgs oDataConnectionEA;
                //  Validación de parámetros.
                    if (guidApplication == null)
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_DbConnection(DataBase)_000");
                        return (null);
                    }
                //  Creación e inicialización de los datos a devolver.
                    oDataConnectionEA = new DbDataConnectionPROGRESSEventArgs(iNumMaxConnections);
                    oDataConnectionEA.Identifier = null;
                    oDataConnectionEA.ApplicationID = guidApplication;
                    oDataConnectionEA.DataBaseType = DBType.PROGRESS;
                //  Inicializa la conexión en función del tipo de seguridad que implementa.
                    Initialize_PROGRESS(ref oDataConnectionEA, out sMensaje, optParams);
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
        /// Método de la clase encargado de la inicialización de una conexión PROGRESS. 
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación para la que se desea crear la nueva conexión.</param>
        /// <param name="guidConnection">Identificador de la conexión a crear.</param>
        /// <param name="eaDCPROGRESS">Parámetros con los que se ha de crear la nueva conexión</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>no nulo, operación realizada correctamente, null, operación fallida</returns>
        internal static DbDataConnectionPROGRESS CreateNewConnection(DbKeyApp dbkaApplication, 
                                                                     Guid? guidConnection,
                                                                     DbDataConnectionPROGRESSEventArgs eaDCPROGRESS,
                                                                     out string sMensaje)
        {
            sMensaje = string.Empty;
            try
            {
                //  Variables
                    DbDataConnectionPROGRESS oDbDataConnection;

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
                    eaDCPROGRESS.Identifier = guidConnection;
                    oDbDataConnection = new DbDataConnectionPROGRESS(dbkaApplication, eaDCPROGRESS, out bResult, out sMensaje);
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
        /// Método encargado de la inicialización de una conexión PROGRESS. Los Parámetros esperados para este 
        /// tipo de conexión son los siguientes:  
        /// 
        ///       [0]. Tipo de Base de Datos :- PROGRESS
        ///       
        ///       [1]. DSN                   :- Nombre del Origen de Datos de Sistema que apunta a la  Base  
        ///                                     de Datos Progress (ODBC).
        /// 
        ///       [2]. InitialCatalog        :- Nombre de la Base de Datos con la que se quiere realizar la
        ///                                     conexión.
        /// 
        ///       [3]. User                  :- Nombre de usuario con el que se accede a la instáncia de la 
        ///                                     Base de Datos.
        /// 
        ///       [4]. Password              :- Contraseña con la que se accede a la instáncia de  la  Base
        ///                                     de Datos.
        ///         
        /// </summary>
        /// <param name="oDataConnectionEA">Clase en la que se almacenan los datos con los que se crea la conexión.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <param name="optParams">Parámetros con los que se debe realizar la inicialización</param>
        /// <returns>true, operación realizada correctamente, false, operación fallida</returns>
        private static bool Initialize_PROGRESS(ref DbDataConnectionPROGRESSEventArgs oDataConnectionEA, out string sMensaje, params object[] optParams)
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
                    oDataConnectionEA.DSN = optParams[1].ToString();
                    oDataConnectionEA.InitialCatalog = optParams[2].ToString();
                    oDataConnectionEA.User = optParams[3].ToString();
                    oDataConnectionEA.Password = optParams[4].ToString();
                    oDataConnectionEA.ConnectionString =
                         string.Format("DSN={0};DB={1};UID={2};PWD={3};",
                                       oDataConnectionEA.DSN, oDataConnectionEA.InitialCatalog, 
                                       oDataConnectionEA.User, oDataConnectionEA.Password);
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

