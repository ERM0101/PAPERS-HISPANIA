#region Librerias usadas por esta clase

using MBCode.Framework.DataBase;
using MBCode.Framework.Managers.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Text;
using System.Threading;

#endregion

namespace MBCode.Framework.Managers.DataBase
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou
    /// Fecha última modificación: 01/03/2012
    /// Descripción: clase que define las operaciones de Gestión de Bases de Datos. 
    /// </summary>
    public static class ConnectionManager
    {
        #region Constantes

        #region Número máximo de Intentos

        /// <summary>
        /// Número de intentos antes de forzar el borrado de una conexión.
        /// </summary>
        private const int NUM_INTENTS_TO_DISCONNECT = 5;

        /// <summary>
        /// Almacena un valor que indica el número de intentos que realiza el programa como máximo para conseguir 
        /// un Guid válido que asociar a la nueva conexión.
        /// </summary>
        private const int NUM_INTENTS_GET_GUID = 5;

        #endregion

        #region TimeOut's

        /// <summary>
        /// Almacena el tiempo, expresado en milisegundos, máximo que se esperará para obtener una nueva clave de 
        /// Base de Datos.
        /// </summary>
        private const int TIMEOUT_GET_KEY = 5000;

        /// <summary>
        /// Almacena el tiempo,  expresado en milisegundos,  máximo que se esperará para obtener una conexión con 
        /// la Base de Datos.
        /// </summary>
        private const int TIMEOUT_GET_CONNECTION = 10000;

        /// <summary>
        /// Almacena el tiempo, expresado en milisegundos,  máximo que se esperará para  validar  los  datos  del
        /// Connection Manager.
        /// </summary>
        private const int TIMEOUT_VALIDATION = 2000;

        /// <summary>
        /// Almacena el tiempo, expresado en milisegundos,  máximo que se esperará para eliminar una conexión con 
        /// la Base de Datos.
        /// </summary>
        private const int TIMEOUT_DISCONNECTION = 2000;

        /// <summary>
        /// Almacena el tiempo, expresado en milisegundos,  máximo que se esperará para reasignar una conexión con
        /// la Base de Datos.
        /// </summary>
        private const int TIMEOUT_ASSIGNATION = 2000;

        /// <summary>
        /// Almacena el tiempo, expresado en milisegundos,  máximo que se esperará para buscar una conexión con 
        /// la Base de Datos.
        /// </summary>
        private const int TIMEOUT_FINDCONNECTION = 5000;

        #endregion

        #region Otros

        /// <summary>
        /// Almacena un valor que indica el tiempo entre dos ejecuciones consecutivas del Controlador de Conexiones
        /// a Borrar.
        /// </summary>
        private const int TIME_BETWEEN_SURVEILL = 1000;

        #endregion

        #endregion

        #region Enumerados

        #region Privados

        /// <summary>
        /// Enumerado que define los posibles resultados de la búsqueda de conexiones en la tabla de hash.
        /// </summary>
        private enum ResultExist
        {
            /// <summary>
            /// Indica que la conexión ya existe y que esta en estado correcto para trabajar.
            /// </summary>
            Exist,

            /// <summary>
            /// Indica que no existe en la tabla una conexión con las características indicadas.
            /// </summary>
            NotExist,

            /// <summary>
            /// Indica que los datos introducidos para la conexión son incorrectos.
            /// </summary>
            DataError,

            /// <summary>
            /// Indica que se ha producido un error en el momento de buscar una conexión.
            /// </summary>
            Error,
        }

        #endregion

        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que define la firma del evento que notifica la finalización de una operación sobre la
        /// aplicación y la conexión indicadas.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación que ha finalizado la operación.</param>
        /// <param name="guidConnection">Identificador de la conexión.</param>
        /// <param name="eResult">Indica el resultado de la operación.</param>
        /// <param name="sMensaje">Mensaje de error, si se ha producido uno.</param>
        public delegate void dlgOpFinished(DbKeyApp dbkaApplication, Guid? guidConnection, ResultOpBD eResult, string sMensaje);

        /// <summary>
        /// Delegado que define la firma del evento que notifica la finalización de una operación sobre la
        /// aplicación y la conexión indicadas.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación que ha finalizado la operación.</param>
        /// <param name="guidConnection">Identificador de la conexión.</param>
        /// <param name="eResult">Indica el resultado de la operación.</param>
        /// <param name="csConnection">Estado de la conexión creada.</param>
        /// <param name="sMensaje">Mensaje de error, si se ha producido uno.</param>
        public delegate void dlgConnectionFinished(DbKeyApp dbkaApplication, Guid? guidConnection, ResultOpBD eResult, ConnectionState? csConnection, string sMensaje);

        /// <summary>
        /// Delegado que define la firma del evento que notifica que ha cambiado el estado de una conexión.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada al elemento que produce el evento</param>
        /// <param name="guidConnection">Identificador de la conexión asociada al elemento que produce el evento.</param>
        /// <param name="e">Parámetros con los que se lanza el evento.</param>
        public delegate void dlgDbConnectionStateChanged(DbKeyApp dbkaApplication, Guid guidConnection, StateChangeEventArgs e);

        #endregion

        #region Eventos

        /// <summary>
        /// Evento que se produce al finalizar el intento de crear una nueva conexión.
        /// </summary>
        public static event dlgConnectionFinished evInicioConexion;

        /// <summary>
        /// Evento que se produce una vez finalizada una conexión a Base de Datos
        /// </summary>
        public static event dlgOpFinished evFinalizacionConexion;

        /// <summary>
        /// Evento que se produce cuando cambia el estado de la conexión.
        /// </summary>
        public static event dlgDbConnectionStateChanged evEstadoConexionCambiado;

        #endregion

        #region Atributos

        #region Tabla de Hash con las claves de aplicación

        /// <summary>
        /// Almacena laa claves asociadas a los clientes conectados.
        /// </summary>
        private static Hashtable htKeys;

        #endregion

        #region Tablas de Hash para almacenar los datos de las conexiones del pooling

        /// <summary>
        /// Almacena las conexiones libres en un momento determinado.
        /// </summary>
        public static Hashtable htFreeConnections = null;

        /// <summary>
        /// Almacena las conexiones en uso en un momento determinado.
        /// </summary>
        public static Hashtable htConnectionsInUse = null;

        /// <summary>
        /// Almacena las transacciones en uso en un momento determinado.
        /// </summary>
        public static Hashtable htTransactionsInUse = null;

        /// <summary>
        /// Almacena las operaciones con DataReaders en uso en un momento determinado.
        /// </summary>
        public static Hashtable htDataReadersInUse = null;

        #endregion

        #region Semáforos de Exclusión Mútua

        /// <summary>
        /// Almacena el Controlador de Exclusión Mútua usado para aislar el inicio de las transacciones.
        /// </summary>
        public static Mutex mtxManageTransactions = null;

        /// <summary>
        /// Almacena el Controlador de Exclusión Mútua usado para aislar el inicio de una operación con un 
        /// DataReader.
        /// </summary>
        public static Mutex mtxManageDataReader = null;

        /// <summary>
        /// Almacena el Controlador de Exclusión Mútua usado para aislar la actualización del estado de las conexiones.
        /// </summary>
        public static Mutex mtxRefreshConnections = null;

        /// <summary>
        /// Almacena el Controlador de Exclusión Mútua usado para aislar administrar la ejecución de las conexiones.
        /// </summary>
        public static Mutex mtxManageQueue = null;

        /// <summary>
        /// Almacena el Controlador de Exclusión Mútua usado para aislar la obtención de una nueva clave de uso.
        /// </summary>
        public static Mutex mtxGetKey = null;

        #endregion

        #region Estado de la Clase

        /// <summary>
        /// Almacena un valor que indica que el Connection Manager se ha iniciado correctamente.
        /// </summary>
        public static bool bInitialized =  false;

        #endregion

        #endregion

        #region Inicializadores y Destructores

        #region Inicializadores

        /// <summary>
        /// Método encargado de realizar las inicializaciones necesarias para el Connection Manager.
        /// </summary>
        /// <param name="sLogFile">Fichero donde se dejan los Log's del Connection Manager</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno</param>
        /// <returns>Mirar el Enumerado</returns>
        public static ResultOpBD Initialize(string sLogFile, out string sMensaje)
        {
            sMensaje = string.Empty;
            try
            {
                if (bInitialized)
                {
                    sMensaje = MsgManager.WarningMsg("MSG_ConnectionManager_020");
                    return (ResultOpBD.Warning);
                }
                bInitialized = false;
                if (htKeys == null) htKeys = new Hashtable();
                if (htFreeConnections == null) htFreeConnections = new Hashtable();
                if (htConnectionsInUse == null) htConnectionsInUse = new Hashtable();
                if (htTransactionsInUse == null) htTransactionsInUse = new Hashtable();
                if (htDataReadersInUse == null) htDataReadersInUse = new Hashtable();
                if (mtxGetKey == null) mtxGetKey = new Mutex();
                if (mtxManageQueue == null) mtxManageQueue = new Mutex();
                if (mtxRefreshConnections == null) mtxRefreshConnections = new Mutex();
                if (mtxManageTransactions == null) mtxManageTransactions = new Mutex();
                if (mtxManageDataReader == null) mtxManageDataReader = new Mutex();
                bInitialized = true;
                return (ResultOpBD.Correct);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (ResultOpBD.Error);
            }
        }

        #endregion

        #region Finalizadores

        /// <summary>
        /// Método encargado de realizar las finalizaciones necesarias para el Connection Manager.
        /// </summary>
        /// <param name="sMensaje">Mensaje de error, si se produce uno</param>
        /// <returns>Mirar el Enumerado</returns>
        public static ResultOpBD Finalize(out string sMensaje)
        {
            sMensaje = string.Empty;
            bool bMutexActivated = false;
            try
            {
                //  Activamos el controlador de exclusión mútua para este método
                    bMutexActivated = mtxRefreshConnections.WaitOne(); //(TIMEOUT_FINDCONNECTION, false);
                //  Si el Connection Manager no está inicializado da error.
                    if (!bInitialized)
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_000");
                        return (ResultOpBD.InternalError);
                    }
                //  Finaliza las conexiones activas.
                    string sMensajeAux;
                    StringBuilder sbMensaje = new StringBuilder(string.Empty);
                    foreach (DbKeyApp dbkaKeyTemp in htKeys.Keys)
                    {
                        if (FinalizeAllConnectionsApp(dbkaKeyTemp, out sMensajeAux) != ResultOpBD.Correct)
                        {
                            sbMensaje = sbMensaje.AppendLine(sMensajeAux);
                        }
                    }
                    sMensaje = sbMensaje.ToString();
                //  Incializa las tablas de Hash.
                    htKeys.Clear();
                    htKeys = null;
                    htFreeConnections.Clear(); ;
                    htFreeConnections = null;
                    htConnectionsInUse.Clear();
                    htConnectionsInUse = null;
                    htTransactionsInUse.Clear();
                    htTransactionsInUse = null;
                    htDataReadersInUse.Clear();
                    htDataReadersInUse = null;
                //  Libera los recursos usados por una conexión de tipo Odbc si había alguna.
                    OdbcConnection.ReleaseObjectPool();
                //  Indica que el Connection Manager no está inicializado.
                    bInitialized = false;
                //  Indica que la operación ha finalizado correctamente.
                    return (((sMensaje != string.Empty) ? ResultOpBD.Warning : ResultOpBD.Correct));
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (ResultOpBD.Error);
            }
            finally
            {
                if (bMutexActivated) mtxRefreshConnections.ReleaseMutex();
            }
        }

        #endregion

        #endregion

        #region Métodos

        #region Relacionados con la ejecución de Consultas y Procedimientos Almacenados

        #region No Transaccionales

        #region Ejecución de Consultas

        /// <summary>
        /// Realiza la consulta indicada por la cadena de carácteres pasada cómo parámetro y devuelve 
        /// un DataSet con el resultado.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación que desea realizar la consulta.</param>
        /// <param name="sSql">Consulta a realizar</param>
        /// <param name="bWithSchema">Indica si se deben utilizar o no las restricciones</param>
        /// <param name="oDataSet">Datos obtenidos al realizar la consulta.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar la descripción del enumerado</returns>
        public static ResultOpBD GetDataSetFromSql(DbKeyApp dbkaApplication, string sSql, bool bWithSchema, 
                                                   out DataSet oDataSet, out string sMensaje)
        {
            oDataSet = null;
            sMensaje = string.Empty;
            Guid guidConnection = Guid.Empty;
            bool bMostrarEstadoConexion = true;
            string sMessageQuery = string.Empty;
            StateChangeEventArgs sceaState = null;
            try
            {
                //  Variables
                    ResultOpBD eResult;
                    CADbDataConnection oDbDataConnection;
                    
                //  Paso 1: Comprobar que el Connection Manager esté inicializado.
                    if ((eResult = ValidateConnectionManager(dbkaApplication, out sMensaje)) != ResultOpBD.Correct) return (eResult);
                //  Paso 2: Obtener una conexión valida de las libres.
                    if ((eResult = GetConnection(dbkaApplication, out oDbDataConnection, out sMensaje)) != ResultOpBD.Correct)
                    {
                        bMostrarEstadoConexion = false;
                        return (eResult);
                    }
                    else guidConnection = (Guid) oDbDataConnection.Identifier;
                //  Paso 3: Realizar la consulta.
                    if (evEstadoConexionCambiado != null)
                        evEstadoConexionCambiado(dbkaApplication, guidConnection,
                                                 new StateChangeEventArgs(ConnectionState.Open, ConnectionState.Executing));
                    switch (oDbDataConnection.TipoBaseDatos)
                    { 
                        case DBType.SQLSERVER:
                             eResult = oDbDataConnection.GetDataSetFromSql(sSql, bWithSchema, out oDataSet, out sMessageQuery);
                             break;
                        case DBType.PROGRESS:
                             eResult = oDbDataConnection.GetDataSetFromSql(sSql, bWithSchema, out oDataSet, out sMessageQuery);
                             break;
                        case DBType.ORACLE:
                             eResult = oDbDataConnection.GetDataSetFromSql(sSql, bWithSchema, out oDataSet, out sMessageQuery);
                             break;
                        default:
                             sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_008");
                             sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                             return (ResultOpBD.InternalError);
                    }
                //  Paso 4: Informa del nuevo estado de la conexión y la reasigna.
                    if (eResult != ResultOpBD.Correct)
                    {
                        if (oDbDataConnection.EstadoConexion == null)
                        {
                            string sMessageFinalize;
                            if (FinalizeConnection(ref oDbDataConnection, out sMessageFinalize, false) != ResultOpBD.Correct)
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Broken);
                                sMensaje = sMessageQuery + sMessageFinalize;
                            }
                            else
                            {
                                sMensaje = sMessageQuery;
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Closed);
                            }
                            return (eResult);
                        }
                        else sMensaje = sMessageQuery;
                    }
                    string sMessageAsignation;
                    if (AssignConnection(dbkaApplication, guidConnection, ref oDbDataConnection, out sceaState,
                                         out sMessageAsignation) != ResultOpBD.Correct)
                    {
                        sMensaje = sMessageQuery + sMessageAsignation;
                    }
                //  Paso 5: notificar el resultado.
                    return (eResult);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_009", new string[] { MsgManager.ExcepMsg(ex), sMessageQuery });
                return (ResultOpBD.Error);
            }
            finally
            {
                if ((evEstadoConexionCambiado != null) && (bMostrarEstadoConexion))
                           evEstadoConexionCambiado(dbkaApplication, (Guid) guidConnection, sceaState);
            }
        }

        #endregion

        #region Ejecución de Procedimientos Almacenados

        /// <summary>
        /// Ejecuta el Stored Procedure indicado por la cadena de carácteres pasada cómo parámetro y devuelve 
        /// un DataSet con el resultado.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación que desea realizar la consulta.</param>
        /// <param name="sNameStored">Nombre del procedimiento a ejecutar.</param>
        /// <param name="oDbParameter">Parámetros con los que se desea ejecutar el procedimiento.</param>
        /// <param name="bWithSchema">Indica si se deben utilizar o no las restricciones</param>
        /// <param name="oDataSet">Datos obtenidos al realizar la consulta.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar la descripción del enumerado</returns>
        public static ResultOpBD ExecuteStoredProcedure(DbKeyApp dbkaApplication, string sNameStored,
                                                        DbParameter[] oDbParameter , bool bWithSchema, 
                                                        out DataSet oDataSet, out string sMensaje)
        {
            oDataSet = null;
            sMensaje = string.Empty;
            Guid guidConnection = Guid.Empty;
            bool bMostrarEstadoConexion = true;
            string sMessageQuery = string.Empty;
            StateChangeEventArgs sceaState = null;
            try
            {
                //  Variables
                    ResultOpBD eResult;
                    CADbDataConnection oDbDataConnection;
                    
                //  Paso 1: Comprobar que el Connection Manager esté inicializado.
                    if ((eResult = ValidateConnectionManager(dbkaApplication, out sMensaje)) != ResultOpBD.Correct) return (eResult);
                //  Paso 2: Obtener una conexión valida de las libres.
                    if ((eResult = GetConnection(dbkaApplication, out oDbDataConnection, out sMensaje)) != ResultOpBD.Correct)
                    {
                        bMostrarEstadoConexion = false;
                        return (eResult);
                    }
                    else guidConnection = (Guid) oDbDataConnection.Identifier;
                //  Paso 3: Realizar la consulta.
                    if (evEstadoConexionCambiado != null)
                        evEstadoConexionCambiado(dbkaApplication, guidConnection,
                                                 new StateChangeEventArgs(ConnectionState.Open, ConnectionState.Executing));
                    switch (oDbDataConnection.TipoBaseDatos)
                    { 
                        case DBType.SQLSERVER:                            
                             eResult = oDbDataConnection.ExecuteStoredProcedure(sNameStored, oDbParameter, bWithSchema, out oDataSet, out sMessageQuery);                            
                             break;
                        case DBType.PROGRESS:
                             sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                             throw new Exception(MsgManager.ErrorMsg("MSG_ConnectionManager_024"));
                        case DBType.ORACLE:
                             eResult = oDbDataConnection.ExecuteStoredProcedure(sNameStored, oDbParameter, bWithSchema, out oDataSet, out sMessageQuery);
                             break;
                        default:
                             sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_008");
                             sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                             return (ResultOpBD.InternalError);
                    }
                //  Paso 4: Informa del nuevo estado de la conexión y la reasigna.
                    if (eResult != ResultOpBD.Correct)
                    {
                        if (oDbDataConnection.EstadoConexion == null)
                        {
                            string sMessageFinalize;
                            if (FinalizeConnection(ref oDbDataConnection, out sMessageFinalize, false) != ResultOpBD.Correct)
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Broken);
                                sMensaje = sMessageQuery + sMessageFinalize;
                            }
                            else
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Closed);
                                sMensaje = sMessageQuery;
                            }
                            return (eResult);
                        }
                        else sMensaje = sMessageQuery;
                    }
                    string sMessageAssignation;
                    if (AssignConnection(dbkaApplication, guidConnection, ref oDbDataConnection, out sceaState,
                                         out sMessageAssignation) != ResultOpBD.Correct)
                    {
                        sMensaje = sMessageQuery + sMessageAssignation;
                    }
                //  Paso 5: notificar el resultado.
                    return (eResult);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_009", new string[] { MsgManager.ExcepMsg(ex), sMessageQuery });
                return (ResultOpBD.Error);
            }
            finally
            {
                if ((evEstadoConexionCambiado != null) && (bMostrarEstadoConexion))
                           evEstadoConexionCambiado(dbkaApplication, (Guid) guidConnection, sceaState);
            }
        }

        /// <summary>
        /// Ejecuta el Stored Procedure indicado por la cadena de carácteres pasada cómo parámetro y devuelve 
        /// un DataSet con el resultado.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación que desea realizar la consulta.</param>
        /// <param name="sNameStored">Nombre del procedimiento a ejecutar.</param>
        /// <param name="oDbParameter">Parámetros con los que se desea ejecutar el procedimiento.</param>
        /// <param name="bWithSchema">Indica si se deben utilizar o no las restricciones</param>
        /// <param name="oDataSet">Datos obtenidos al realizar la consulta.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar la descripción del enumerado</returns>
        public static ResultOpBD ExecuteStoredProcedure(DbKeyApp dbkaApplication, string sNameStored,
                                                        DbParameter[] oDbParameter, bool bWithSchema,
                                                        out List<object> iRowsAffected, out string sMensaje)
        {
            iRowsAffected = new List<object>();
            sMensaje = string.Empty;
            Guid guidConnection = Guid.Empty;
            bool bMostrarEstadoConexion = true;
            string sMessageQuery = string.Empty;
            StateChangeEventArgs sceaState = null;
            try
            {
                //  Variables
                ResultOpBD eResult;
                CADbDataConnection oDbDataConnection;

                //  Paso 1: Comprobar que el Connection Manager esté inicializado.
                if ((eResult = ValidateConnectionManager(dbkaApplication, out sMensaje)) != ResultOpBD.Correct) return (eResult);
                //  Paso 2: Obtener una conexión valida de las libres.
                if ((eResult = GetConnection(dbkaApplication, out oDbDataConnection, out sMensaje)) != ResultOpBD.Correct)
                {
                    bMostrarEstadoConexion = false;
                    return (eResult);
                }
                else guidConnection = (Guid)oDbDataConnection.Identifier;
                //  Paso 3: Realizar la consulta.
                if (evEstadoConexionCambiado != null)
                    evEstadoConexionCambiado(dbkaApplication, guidConnection,
                                             new StateChangeEventArgs(ConnectionState.Open, ConnectionState.Executing));
                switch (oDbDataConnection.TipoBaseDatos)
                {
                    case DBType.SQLSERVER:
                         eResult = oDbDataConnection.ExecuteStoredProcedure(sNameStored, oDbParameter, bWithSchema, out iRowsAffected, out sMessageQuery);
                         break;
                    case DBType.PROGRESS:
                         sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                         throw new Exception(MsgManager.ErrorMsg("MSG_ConnectionManager_024"));
                    case DBType.ORACLE:
                         eResult = oDbDataConnection.ExecuteStoredProcedure(sNameStored, oDbParameter, bWithSchema, out iRowsAffected, out sMessageQuery);
                         break;
                    default:
                         sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_008");
                         sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                         return (ResultOpBD.InternalError);
                }
                //  Paso 4: Informa del nuevo estado de la conexión y la reasigna.
                if (eResult != ResultOpBD.Correct)
                {
                    if (oDbDataConnection.EstadoConexion == null)
                    {
                        string sMessageFinalize;
                        if (FinalizeConnection(ref oDbDataConnection, out sMessageFinalize, false) != ResultOpBD.Correct)
                        {
                            sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Broken);
                            sMensaje = sMessageQuery + sMessageFinalize;
                        }
                        else
                        {
                            sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Closed);
                            sMensaje = sMessageQuery;
                        }
                        return (eResult);
                    }
                    else sMensaje = sMessageQuery;
                }
                string sMessageAssignation;
                if (AssignConnection(dbkaApplication, guidConnection, ref oDbDataConnection, out sceaState,
                                     out sMessageAssignation) != ResultOpBD.Correct)
                {
                    sMensaje = sMessageQuery + sMessageAssignation;
                }
                //  Paso 5: notificar el resultado.
                return (eResult);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_009", new string[] { MsgManager.ExcepMsg(ex), sMessageQuery });
                return (ResultOpBD.Error);
            }
            finally
            {
                if ((evEstadoConexionCambiado != null) && (bMostrarEstadoConexion))
                    evEstadoConexionCambiado(dbkaApplication, (Guid)guidConnection, sceaState);
            }
        }

        #endregion

        #region Obtención de un Escalar

        /// <summary>
        /// Realiza la consulta indicada por la cadena de carácteres pasada cómo parámetro y devuelve un DataSet con el 
        /// resultado.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación que desea realizar la consulta.</param>
        /// <param name="sSql">Consulta a realizar</param>
        /// <param name="iEscalarResult">Resultado de la ejecución de la consulta</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar la descripción del enumerado</returns>
        public static ResultOpBD GetEscalarFromSql(DbKeyApp dbkaApplication, string sSql, out int iEscalarResult, out string sMensaje)
        {
            iEscalarResult = -1;
            sMensaje = string.Empty;
            Guid guidConnection = Guid.Empty;
            bool bMostrarEstadoConexion = true;
            string sMessageQuery = string.Empty;
            StateChangeEventArgs sceaState = null;
            try
            {
                //  Variables
                    ResultOpBD eResult;
                    CADbDataConnection oDbDataConnection;
                    
                //  Paso 1: Comprobar que el Connection Manager esté inicializado.
                    if ((eResult = ValidateConnectionManager(dbkaApplication, out sMensaje)) != ResultOpBD.Correct) return (eResult);
                //  Paso 2: Obtener una conexión valida de las libres.
                    if ((eResult = GetConnection(dbkaApplication, out oDbDataConnection, out sMensaje)) != ResultOpBD.Correct)
                    {
                        bMostrarEstadoConexion = false;
                        return (eResult);
                    }
                    else guidConnection = (Guid) oDbDataConnection.Identifier;
                //  Paso 3: Realizar la consulta.
                    if (evEstadoConexionCambiado != null)
                        evEstadoConexionCambiado(dbkaApplication, guidConnection,
                                                 new StateChangeEventArgs(ConnectionState.Open, ConnectionState.Executing));
                    switch (oDbDataConnection.TipoBaseDatos)
                    { 
                        case DBType.SQLSERVER:
                             eResult = oDbDataConnection.GetEscalarFromSql(sSql, out iEscalarResult, out sMessageQuery);
                             break;
                        case DBType.PROGRESS:
                             eResult = oDbDataConnection.GetEscalarFromSql(sSql, out iEscalarResult, out sMessageQuery);
                             break;
                        case DBType.ORACLE:
                             eResult = oDbDataConnection.GetEscalarFromSql(sSql, out iEscalarResult, out sMessageQuery);
                             break;
                        default:
                             sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_008");
                             sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                             return (ResultOpBD.InternalError);
                    }
                //  Paso 4: Informa del nuevo estado de la conexión y la reasigna.
                    if (eResult != ResultOpBD.Correct)
                    {
                        if (oDbDataConnection.EstadoConexion == null)
                        {
                            string sMessageFinalize;
                            if (FinalizeConnection(ref oDbDataConnection, out sMessageFinalize, false) != ResultOpBD.Correct)
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Broken);
                                sMensaje = sMessageQuery + sMessageFinalize;
                            }
                            else
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Closed);
                                sMensaje = sMessageQuery;
                            }
                            return (eResult);
                        }
                        else sMensaje = sMessageQuery;
                    }
                    string sMessageAssignation;
                    if (AssignConnection(dbkaApplication, guidConnection, ref oDbDataConnection, out sceaState,
                                         out sMessageAssignation) != ResultOpBD.Correct)
                    {
                        sMensaje = sMessageQuery + sMessageAssignation;
                    }
                //  Paso 5: notificar el resultado.
                    return (eResult);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_009", new string[] { MsgManager.ExcepMsg(ex), sMessageQuery });
                return (ResultOpBD.Error);
            }
            finally
            {
                if ((evEstadoConexionCambiado != null) && (bMostrarEstadoConexion))
                           evEstadoConexionCambiado(dbkaApplication, (Guid) guidConnection, sceaState);
            }
        }

        /// <summary>
        /// Realiza, de manera transaccional, la consulta indicada por la cadena de carácteres pasada cómo parámetro y
        /// devuelve un DataSet con el resultado.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación que desea realizar la consulta.</param>
        /// <param name="guidTransaction">Identificador de la transacción sobre la que se desea realizar la operación.</param>
        /// <param name="sSql">Consulta a realizar</param>
        /// <param name="iEscalarResult">Resultado de la ejecución de la consulta</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar la descripción del enumerado</returns>
        public static ResultOpBD GetEscalarFromWithParameter(DbKeyApp dbkaApplication, Guid guidTransaction,Dictionary<string, List<object>> dicSql,
                                                   out List<int> listEscalarResult, out string sMensaje)
        {
            listEscalarResult = new List<int>();
            sMensaje = string.Empty;
            Guid guidConnection = Guid.Empty;
            bool bMostrarEstadoConexion = true;
            string sMessageQuery = string.Empty;
            StateChangeEventArgs sceaState = null;
            try
            {
                //  Variables
                    ResultOpBD eResult;
                    CADbDataConnection oDbDataConnection;

                //  Paso 1: Comprobar que el Connection Manager esté inicializado.
                    if ((eResult = ValidateConnectionManager(dbkaApplication, out sMensaje)) != ResultOpBD.Correct) return (eResult);
                //  Paso 2: Obtener una conexión valida de las libres.
                    if (!htTransactionsInUse.Contains(guidTransaction))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_021");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    guidConnection = (Guid)htTransactionsInUse[guidTransaction];
                    if (!htConnectionsInUse.ContainsKey(guidConnection))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_021");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    oDbDataConnection = (CADbDataConnection)htConnectionsInUse[guidConnection];
                //  Paso 3: Realizar la consulta.
                    if (evEstadoConexionCambiado != null)
                        evEstadoConexionCambiado(dbkaApplication, guidConnection,
                                                 new StateChangeEventArgs(ConnectionState.Open, ConnectionState.Executing));
                    switch (oDbDataConnection.TipoBaseDatos)
                    {
                        case DBType.SQLSERVER:
                             eResult = oDbDataConnection.GetEscalarFromWithParameter(dicSql, out listEscalarResult, out sMessageQuery);
                             break;
                        case DBType.PROGRESS:
                             eResult = oDbDataConnection.GetEscalarFromWithParameter(dicSql, out listEscalarResult, out sMessageQuery);
                             break;
                        case DBType.ORACLE:
                             eResult = oDbDataConnection.GetEscalarFromWithParameter(dicSql, out listEscalarResult, out sMessageQuery);
                             break;
                        default:
                             sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_008");
                             sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                             return (ResultOpBD.InternalError);
                    }
                //  Paso 4: Informa del nuevo estado de la conexión y la reasigna.
                    if (eResult != ResultOpBD.Correct)
                    {
                        if (oDbDataConnection.EstadoConexion == null)
                        {
                            string sMessageFinalize;
                            if (FinalizeConnection(ref oDbDataConnection, out sMessageFinalize, false) != ResultOpBD.Correct)
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Broken);
                                sMensaje = sMessageQuery + sMessageFinalize;
                            }
                            else
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Closed);
                                sMensaje = sMessageQuery;
                            }
                            return (eResult);
                        }
                        else sMensaje = sMessageQuery;
                    }
                //  Paso 5: notificar el resultado.
                    return (eResult);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_009", new string[] { MsgManager.ExcepMsg(ex), sMessageQuery });
                return (ResultOpBD.Error);
            }
            finally
            {
                if ((evEstadoConexionCambiado != null) && (bMostrarEstadoConexion))
                    evEstadoConexionCambiado(dbkaApplication, (Guid)guidConnection, sceaState);
            }
        }

        #endregion

        #region Operaciones que modifican el contenido de la Base de Datos

        /// <summary>
        /// Ejecuta la consulta de inserción, borrado o cambio, pasada cómo parámetro.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación que desea realizar la consulta.</param>
        /// <param name="sSql">Consulta a realizar</param>
        /// <param name="iNumRowsAfected">Resultado de la ejecución de la consulta</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar la descripción del enumerado</returns>
        public static ResultOpBD ExecuteCommandFromSql(DbKeyApp dbkaApplication, string sSql, out int iNumRowsAfected, out string sMensaje)
        {
            iNumRowsAfected = -1;
            sMensaje = string.Empty;
            Guid guidConnection = Guid.Empty;
            bool bMostrarEstadoConexion = true;
            string sMessageQuery = string.Empty;
            StateChangeEventArgs sceaState = null;
            try
            {
                //  Variables
                    ResultOpBD eResult;
                    CADbDataConnection oDbDataConnection;
                    
                //  Paso 1: Comprobar que el Connection Manager esté inicializado.
                    if ((eResult = ValidateConnectionManager(dbkaApplication, out sMensaje)) != ResultOpBD.Correct) return (eResult);
                //  Paso 2: Obtener una conexión valida de las libres.
                    if ((eResult = GetConnection(dbkaApplication, out oDbDataConnection, out sMensaje)) != ResultOpBD.Correct)
                    {
                        bMostrarEstadoConexion = false;
                        return (eResult);
                    }
                    else guidConnection = (Guid) oDbDataConnection.Identifier;
                //  Paso 3: Realizar la consulta.
                    if (evEstadoConexionCambiado != null)
                        evEstadoConexionCambiado(dbkaApplication, guidConnection,
                                                 new StateChangeEventArgs(ConnectionState.Open, ConnectionState.Executing));
                    switch (oDbDataConnection.TipoBaseDatos)
                    { 
                        case DBType.SQLSERVER:
                             eResult = oDbDataConnection.ExecuteCommandFromSql(sSql, out iNumRowsAfected, out sMessageQuery);
                             break;
                        case DBType.PROGRESS:
                             eResult = oDbDataConnection.ExecuteCommandFromSql(sSql, out iNumRowsAfected, out sMessageQuery);
                             break;
                        case DBType.ORACLE:
                             eResult = oDbDataConnection.ExecuteCommandFromSql(sSql, out iNumRowsAfected, out sMessageQuery);
                             break;
                        default:
                             sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_008");
                             sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                             return (ResultOpBD.InternalError);
                    }
                //  Paso 4: Informa del nuevo estado de la conexión y la reasigna.
                    if (eResult != ResultOpBD.Correct)
                    {
                        iNumRowsAfected = -1; 
                        if (oDbDataConnection.EstadoConexion == null) // Si la conexión se ha perdido
                        {
                            string sMessageFinalize;
                            if (FinalizeConnection(ref oDbDataConnection, out sMessageFinalize, false) != ResultOpBD.Correct)
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Broken);
                                sMensaje = sMessageQuery + sMessageFinalize;
                            }
                            else
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Closed);
                                sMensaje = sMessageQuery;
                            }
                            return (eResult);
                        }
                        else sMensaje = sMessageQuery;
                    }
                    string sMessageAssign;
                    if (AssignConnection(dbkaApplication, guidConnection, ref oDbDataConnection, out sceaState,
                                         out sMessageAssign) != ResultOpBD.Correct)
                    {
                        sMensaje = sMessageQuery + sMessageAssign;
                    }
                //  Paso 5: notificar el resultado.
                    return (eResult);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_009", new string[] { MsgManager.ExcepMsg(ex), sMessageQuery });
                return (ResultOpBD.Error);
            }
            finally
            {
                if ((evEstadoConexionCambiado != null) && (bMostrarEstadoConexion))
                           evEstadoConexionCambiado(dbkaApplication, (Guid) guidConnection, sceaState);
            }
        }

        /// <summary>
        /// Ejecuta la consulta de inserción, borrado o cambio, pasada cómo parámetro.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación que desea realizar la consulta.</param>
        /// <param name="sSql">Consulta a realizar</param>
        /// <param name="iNumRowsAfected">Resultado de la ejecución de la consulta</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar la descripción del enumerado</returns>
        public static ResultOpBD ExecuteCommandFromSqlMARS(DbKeyApp dbkaApplication, Guid guidTransaction, List<string> listSql, out List<int> listNumRowsAfected, out string sMensaje)
        {
            listNumRowsAfected = new List<int>();
            listNumRowsAfected.Clear();

            sMensaje = string.Empty;
            Guid guidConnection = Guid.Empty;
            bool bMostrarEstadoConexion = true;
            string sMessageQuery = string.Empty;
            StateChangeEventArgs sceaState = null;
            try
            {
                //  Variables
                    ResultOpBD eResult;
                    CADbDataConnection oDbDataConnection;

                //  Paso 1: Comprobar que el Connection Manager esté inicializado.
                    if ((eResult = ValidateConnectionManager(dbkaApplication, out sMensaje)) != ResultOpBD.Correct) return (eResult);
                //  Paso 2: Obtener una conexión valida de las libres.
                    if (!htTransactionsInUse.Contains(guidTransaction))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_021");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    guidConnection = (Guid)htTransactionsInUse[guidTransaction];
                    if (!htConnectionsInUse.ContainsKey(guidConnection))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_021");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    oDbDataConnection = (CADbDataConnection)htConnectionsInUse[guidConnection];
                //  Paso 3: Realizar la consulta.
                    if (evEstadoConexionCambiado != null)
                        evEstadoConexionCambiado(dbkaApplication, guidConnection,
                                                 new StateChangeEventArgs(ConnectionState.Open, ConnectionState.Executing));
                    switch (oDbDataConnection.TipoBaseDatos)
                    {
                        case DBType.SQLSERVER:
                             eResult = oDbDataConnection.ExecuteCommandFromSqlMARS(listSql, out listNumRowsAfected, out sMessageQuery);
                             break;
                        case DBType.PROGRESS:
                             eResult = oDbDataConnection.ExecuteCommandFromSqlMARS(listSql, out listNumRowsAfected, out sMessageQuery);
                             break;
                        case DBType.ORACLE:
                             eResult = oDbDataConnection.ExecuteCommandFromSqlMARS(listSql, out listNumRowsAfected, out sMessageQuery);
                             break;
                        default:
                             sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_008");
                             sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                             return (ResultOpBD.InternalError);
                    }
                //  Paso 4: Informa del nuevo estado de la conexión y la reasigna.
                    if (eResult != ResultOpBD.Correct)
                    {
                        if (oDbDataConnection.EstadoConexion == null)
                        {
                            string sMessageFinalize;
                            if (FinalizeConnection(ref oDbDataConnection, out sMessageFinalize, false) != ResultOpBD.Correct)
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Broken);
                                sMensaje = sMessageQuery + sMessageFinalize;
                            }
                            else
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Closed);
                                sMensaje = sMessageQuery;
                            }
                            return (eResult);
                        }
                        else sMensaje = sMessageQuery;
                    }
                //  Paso 5: notificar el resultado.
                    return (eResult);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_009", new string[] { MsgManager.ExcepMsg(ex), sMessageQuery });
                return (ResultOpBD.Error);
            }
            finally
            {
                if ((evEstadoConexionCambiado != null) && (bMostrarEstadoConexion))
                    evEstadoConexionCambiado(dbkaApplication, (Guid)guidConnection, sceaState);
            }
        }


        /// <summary>
        /// Ejecuta la consulta de inserción, borrado o cambio, pasada cómo parámetro.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación que desea realizar la consulta.</param>
        /// <param name="sSql">Consulta a realizar</param>
        /// <param name="iNumRowsAfected">Resultado de la ejecución de la consulta</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar la descripción del enumerado</returns>
        public static ResultOpBD ExecuteCommandFromSqlMARSWithParameters(DbKeyApp dbkaApplication, Guid guidTransaction, Dictionary<string, List<object>> dicParameter, out List<int> listNumRowsAfected, out string sMensaje)
        {
            listNumRowsAfected = new List<int>();
            listNumRowsAfected.Clear();

            sMensaje = string.Empty;
            Guid guidConnection = Guid.Empty;
            bool bMostrarEstadoConexion = true;
            string sMessageQuery = string.Empty;
            StateChangeEventArgs sceaState = null;
            try
            {
                //  Variables
                    ResultOpBD eResult;
                    CADbDataConnection oDbDataConnection;

                //  Paso 1: Comprobar que el Connection Manager esté inicializado.
                    if ((eResult = ValidateConnectionManager(dbkaApplication, out sMensaje)) != ResultOpBD.Correct) return (eResult);
                //  Paso 2: Obtener una conexión valida de las libres.
                    if (!htTransactionsInUse.Contains(guidTransaction))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_021");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    guidConnection = (Guid)htTransactionsInUse[guidTransaction];
                    if (!htConnectionsInUse.ContainsKey(guidConnection))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_021");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    oDbDataConnection = (CADbDataConnection)htConnectionsInUse[guidConnection];
                //  Paso 3: Realizar la consulta.
                    if (evEstadoConexionCambiado != null)
                        evEstadoConexionCambiado(dbkaApplication, guidConnection,
                                                 new StateChangeEventArgs(ConnectionState.Open, ConnectionState.Executing));
                    switch (oDbDataConnection.TipoBaseDatos)
                    {
                        case DBType.SQLSERVER:
                             eResult = oDbDataConnection.ExecuteCommandFromSqlMARSWithParameters(dicParameter, out listNumRowsAfected, out sMessageQuery);
                             break;
                        case DBType.PROGRESS:
                             eResult = oDbDataConnection.ExecuteCommandFromSqlMARSWithParameters(dicParameter, out listNumRowsAfected, out sMessageQuery);
                             break;
                        case DBType.ORACLE:
                             eResult = oDbDataConnection.ExecuteCommandFromSqlMARSWithParameters(dicParameter, out listNumRowsAfected, out sMessageQuery);
                             break;
                        default:
                             sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_008");
                             sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                             return (ResultOpBD.InternalError);
                    }
                //  Paso 4: Informa del nuevo estado de la conexión y la reasigna.
                    if (eResult != ResultOpBD.Correct)
                    {
                        if (oDbDataConnection.EstadoConexion == null)
                        {
                            string sMessageFinalize;
                            if (FinalizeConnection(ref oDbDataConnection, out sMessageFinalize, false) != ResultOpBD.Correct)
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Broken);
                                sMensaje = sMessageQuery + sMessageFinalize;
                            }
                            else
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Closed);
                                sMensaje = sMessageQuery;
                            }
                            return (eResult);
                        }
                        else sMensaje = sMessageQuery;
                    }
                //  Paso 5: notificar el resultado.
                    return (eResult);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_009", new string[] { MsgManager.ExcepMsg(ex), sMessageQuery });
                return (ResultOpBD.Error);
            }
            finally
            {
                if ((evEstadoConexionCambiado != null) && (bMostrarEstadoConexion))
                    evEstadoConexionCambiado(dbkaApplication, (Guid)guidConnection, sceaState);
            }
        }


        /// <summary>
        /// Ejecuta la consulta de inserción, borrado o cambio, pasada cómo parámetro.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación que desea realizar la consulta.</param>
        /// <param name="sSql">Consulta a realizar</param>
        /// <param name="iNumRowsAfected">Resultado de la ejecución de la consulta</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar la descripción del enumerado</returns>
        public static ResultOpBD ExecuteCommandFromSqlParameter(DbKeyApp dbkaApplication, Guid guidTransaction, string sSql, List<object> parameter, out int iNumRowsAfected, out string sMensaje)
        {
            iNumRowsAfected = -1;
            sMensaje = string.Empty;
            Guid guidConnection = Guid.Empty;
            bool bMostrarEstadoConexion = true;
            string sMessageQuery = string.Empty;
            StateChangeEventArgs sceaState = null;
            try
            {
                //  Variables
                    ResultOpBD eResult;
                    CADbDataConnection oDbDataConnection;

                //  Paso 1: Comprobar que el Connection Manager esté inicializado.
                    if ((eResult = ValidateConnectionManager(dbkaApplication, out sMensaje)) != ResultOpBD.Correct) return (eResult);
                //  Paso 2: Obtener una conexión valida de las libres.
                    if (!htTransactionsInUse.Contains(guidTransaction))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_021");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    guidConnection = (Guid)htTransactionsInUse[guidTransaction];
                    if (!htConnectionsInUse.ContainsKey(guidConnection))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_021");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    oDbDataConnection = (CADbDataConnection)htConnectionsInUse[guidConnection];
                //  Paso 3: Realizar la consulta.
                    if (evEstadoConexionCambiado != null)
                        evEstadoConexionCambiado(dbkaApplication, guidConnection,
                                                 new StateChangeEventArgs(ConnectionState.Open, ConnectionState.Executing));
                    switch (oDbDataConnection.TipoBaseDatos)
                    {
                        case DBType.SQLSERVER:
                             eResult = oDbDataConnection.ExecuteCommandFromSql(sSql, parameter,out iNumRowsAfected, out sMessageQuery);
                             break;
                        case DBType.PROGRESS:
                             eResult = oDbDataConnection.ExecuteCommandFromSql(sSql, parameter,out iNumRowsAfected, out sMessageQuery);
                             break;
                        case DBType.ORACLE:
                             sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                             throw new Exception(MsgManager.ErrorMsg("MSG_ConnectionManager_025"));
                        default:
                             sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_008");
                             sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                             return (ResultOpBD.InternalError);
                    }
                //  Paso 4: Informa del nuevo estado de la conexión y la reasigna.
                    if (eResult != ResultOpBD.Correct)
                    {
                        if (oDbDataConnection.EstadoConexion == null)
                        {
                            string sMessageFinalize;
                            if (FinalizeConnection(ref oDbDataConnection, out sMessageFinalize, false) != ResultOpBD.Correct)
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Broken);
                                sMensaje = sMessageQuery + sMessageFinalize;
                            }
                            else
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Closed);
                                sMensaje = sMessageQuery;
                            }
                            return (eResult);
                        }
                        else sMensaje = sMessageQuery;
                    }
                //  Paso 5: notificar el resultado.
                    return (eResult);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_009", new string[] { MsgManager.ExcepMsg(ex), sMessageQuery });
                return (ResultOpBD.Error);
            }
            finally
            {
                if ((evEstadoConexionCambiado != null) && (bMostrarEstadoConexion))
                    evEstadoConexionCambiado(dbkaApplication, (Guid)guidConnection, sceaState);
            }
        }

        #endregion

        #region Operaciones que Actualizan el DataSet

        /// <summary>
        /// Ejecuta el cambio de la información de la Base de Datos en función a los valores contenidos en los parámetros.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación que desea realizar la consulta.</param>
        /// <param name="dsOld">DataSet con los datos anteriores.</param>
        /// <param name="dsNew">DataSet con los nuevos datos.</param>
        /// <param name="sSql">Consulta a realizar</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar la descripción del enumerado</returns>
        public static ResultOpBD ActualizeDatSetSql(DbKeyApp dbkaApplication, DataSet dsOld, DataSet dsNew, string sSql, out string sMensaje)
        {
            sMensaje = string.Empty;
            Guid guidConnection = Guid.Empty;
            bool bMostrarEstadoConexion = true;
            string sMessageQuery = string.Empty;
            StateChangeEventArgs sceaState = null;
            try
            {
                //  Variables
                    ResultOpBD eResult;
                    CADbDataConnection oDbDataConnection;

                //  Paso 1: Comprobar que el Connection Manager esté inicializado.
                    if ((eResult = ValidateConnectionManager(dbkaApplication, out sMensaje)) != ResultOpBD.Correct) return (eResult);
                //  Paso 2: Obtener una conexión valida de las libres.
                    if ((eResult = GetConnection(dbkaApplication, out oDbDataConnection, out sMensaje)) != ResultOpBD.Correct)
                    {
                        bMostrarEstadoConexion = false;
                        return (eResult);
                    }
                    else guidConnection = (Guid)oDbDataConnection.Identifier;
                //  Paso 3: Realizar la consulta.
                    if (evEstadoConexionCambiado != null)
                        evEstadoConexionCambiado(dbkaApplication, guidConnection,
                                                 new StateChangeEventArgs(ConnectionState.Open, ConnectionState.Executing));
                    switch (oDbDataConnection.TipoBaseDatos)
                    {
                        case DBType.SQLSERVER:
                             eResult = oDbDataConnection.ActualizeDatSetSql(dsOld, dsNew, sSql, out sMessageQuery);
                             break;
                        case DBType.PROGRESS:
                             sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                             throw new Exception(MsgManager.ErrorMsg("MSG_ConnectionManager_024"));
                        case DBType.ORACLE:
                             eResult = oDbDataConnection.ActualizeDatSetSql(dsOld, dsNew, sSql, out sMessageQuery);
                             break;
                        default:
                             sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_008");
                             sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                             return (ResultOpBD.InternalError);
                    }
                //  Paso 4: Informa del nuevo estado de la conexión y la reasigna.
                    if (eResult != ResultOpBD.Correct)
                    {
                        if (oDbDataConnection.EstadoConexion == null) // Si la conexión se ha perdido
                        {
                            string sMessageFinalize;
                            if (FinalizeConnection(ref oDbDataConnection, out sMessageFinalize, false) != ResultOpBD.Correct)
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Broken);
                                sMensaje = sMessageQuery + sMessageFinalize;
                            }
                            else
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Closed);
                                sMensaje = sMessageQuery;
                            }
                            return (eResult);
                        }
                        else sMensaje = sMessageQuery;
                    }
                    string sMessageAssign;
                    if (AssignConnection(dbkaApplication, guidConnection, ref oDbDataConnection, out sceaState,
                                         out sMessageAssign) != ResultOpBD.Correct)
                    {
                        sMensaje = sMessageQuery + sMessageAssign;
                    }
                //  Paso 5: notificar el resultado.
                    return (eResult);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_009", new string[] { MsgManager.ExcepMsg(ex), sMessageQuery });
                return (ResultOpBD.Error);
            }
            finally
            {
                if ((evEstadoConexionCambiado != null) && (bMostrarEstadoConexion))
                    evEstadoConexionCambiado(dbkaApplication, (Guid)guidConnection, sceaState);
            }
        }

        /// <summary>
        /// Ejecuta, de manera transaccional la consulta de inserción, borrado o cambio, pasada cómo parámetro.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación que desea realizar la consulta.</param>
        /// <param name="guidTransaction">Identificador de la transacción sobre la que se desea realizar la operación.</param>
        /// <param name="sSql">Consulta a realizar</param>
        /// <param name="iNumRowsAfected">Resultado de la ejecución de la consulta</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar la descripción del enumerado</returns>
        public static ResultOpBD ActualizeDatSetSql(DbKeyApp dbkaApplication,  Guid guidTransaction,DataSet dsOld, DataSet dsNew, string sSql, out string sMensaje)
        {            
            sMensaje = string.Empty;
            Guid guidConnection = Guid.Empty;
            bool bMostrarEstadoConexion = true;
            string sMessageQuery = string.Empty;
            StateChangeEventArgs sceaState = null;
            try
            {
                //  Variables
                    ResultOpBD eResult;
                    CADbDataConnection oDbDataConnection;

                //  Paso 1: Comprobar que el Connection Manager esté inicializado.
                    if ((eResult = ValidateConnectionManager(dbkaApplication, out sMensaje)) != ResultOpBD.Correct) return (eResult);
                //  Paso 2: Obtener una conexión valida de las libres.
                    if (!htTransactionsInUse.Contains(guidTransaction))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_021");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    guidConnection = (Guid)htTransactionsInUse[guidTransaction];
                    if (!htConnectionsInUse.ContainsKey(guidConnection))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_021");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    oDbDataConnection = (CADbDataConnection)htConnectionsInUse[guidConnection];
                //  Paso 3: Realizar la consulta.
                    if (evEstadoConexionCambiado != null)
                        evEstadoConexionCambiado(dbkaApplication, guidConnection,
                                                 new StateChangeEventArgs(ConnectionState.Open, ConnectionState.Executing));
                     switch (oDbDataConnection.TipoBaseDatos)
                     {
                           case DBType.SQLSERVER:
                                eResult = oDbDataConnection.ActualizeDatSetSql(dsOld, dsNew, sSql, out sMessageQuery);
                                break;
                           case DBType.PROGRESS:
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                                throw new Exception(MsgManager.ErrorMsg("MSG_ConnectionManager_024"));
                           case DBType.ORACLE:
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                                throw new Exception(MsgManager.ErrorMsg("MSG_ConnectionManager_025"));
                           default:
                                eResult = oDbDataConnection.ActualizeDatSetSql(dsOld, dsNew, sSql, out sMessageQuery);
                                break;
                    }             
                //  Paso 4: Informa del nuevo estado de la conexión y la reasigna.
                    if (eResult != ResultOpBD.Correct)
                    {
                        if (oDbDataConnection.EstadoConexion == null)
                        {
                            string sMessageFinalize;
                            if (FinalizeConnection(ref oDbDataConnection, out sMessageFinalize, false) != ResultOpBD.Correct)
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Broken);
                                sMensaje = sMessageQuery + sMessageFinalize;
                            }
                            else
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Closed);
                                sMensaje = sMessageQuery;
                            }
                            return (eResult);
                        }
                        else sMensaje = sMessageQuery;
                    }
                //  Paso 5: notificar el resultado.
                    return (eResult);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_009", new string[] { MsgManager.ExcepMsg(ex), sMessageQuery });
                return (ResultOpBD.Error);
            }
            finally
            {
                if ((evEstadoConexionCambiado != null) && (bMostrarEstadoConexion))
                    evEstadoConexionCambiado(dbkaApplication, (Guid)guidConnection, sceaState);
            }
        }

        #endregion

        #endregion

        #region Transaccionales

        #region Ejecución de Consultas

        /// <summary>
        /// Realiza la consulta indicada por la cadena de carácteres pasada cómo parámetro y devuelve 
        /// un DataSet con el resultado.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación que desea realizar la consulta.</param>
        /// <param name="guidTransaction">Identificador de la transacción sobre la que se desea realizar la operación.</param>
        /// <param name="sSql">Consulta a realizar</param>
        /// <param name="bWithSchema">Indica si se deben utilizar o no las restricciones</param>
        /// <param name="oDataSet">Datos obtenidos al realizar la consulta.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar la descripción del enumerado</returns>
        public static ResultOpBD GetDataSetFromSql(DbKeyApp dbkaApplication, Guid guidTransaction, string sSql,
                                                   bool bWithSchema, out DataSet oDataSet, out string sMensaje)
        {
            oDataSet = null;
            sMensaje = string.Empty;
            Guid guidConnection = Guid.Empty;
            bool bMostrarEstadoConexion = true;
            string sMessageQuery = string.Empty;
            StateChangeEventArgs sceaState = null;
            try
            {
                //  Variables
                    ResultOpBD eResult;
                    CADbDataConnection oDbDataConnection;
                    
                //  Paso 1: Comprobar que el Connection Manager esté inicializado.
                    if ((eResult = ValidateConnectionManager(dbkaApplication, out sMensaje)) != ResultOpBD.Correct) return (eResult);
                //  Paso 2: Obtener una conexión valida de las libres.
                    if (!htTransactionsInUse.Contains(guidTransaction))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_021");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    guidConnection = (Guid)htTransactionsInUse[guidTransaction];
                    if (!htConnectionsInUse.ContainsKey(guidConnection))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_021");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    oDbDataConnection = (CADbDataConnection)htConnectionsInUse[guidConnection];
                //  Paso 3: Realizar la consulta.
                    if (evEstadoConexionCambiado != null)
                        evEstadoConexionCambiado(dbkaApplication, guidConnection,
                                                 new StateChangeEventArgs(ConnectionState.Open, ConnectionState.Executing));
                    switch (oDbDataConnection.TipoBaseDatos)
                    { 
                        case DBType.SQLSERVER:
                             eResult = oDbDataConnection.GetDataSetFromSql(sSql, bWithSchema, out oDataSet, out sMessageQuery);
                             break;
                        case DBType.PROGRESS:
                             eResult = oDbDataConnection.GetDataSetFromSql(sSql, bWithSchema, out oDataSet, out sMessageQuery);
                             break;
                        case DBType.ORACLE:
                             eResult = oDbDataConnection.GetDataSetFromSql(sSql, bWithSchema, out oDataSet, out sMessageQuery);
                             break;
                        default:
                             sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_008");
                             sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                             return (ResultOpBD.InternalError);
                    }
                //  Paso 4: Informa del nuevo estado de la conexión y la reasigna.
                    if (eResult != ResultOpBD.Correct)
                    {
                        if (oDbDataConnection.EstadoConexion == null)
                        {
                            string sMessageFinalize;
                            if (FinalizeConnection(ref oDbDataConnection, out sMessageFinalize, false) != ResultOpBD.Correct)
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Broken);
                                sMensaje = sMessageQuery + sMessageFinalize;
                            }
                            else
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Closed);
                                sMensaje = sMessageQuery;
                            }
                            return (eResult);
                        }
                        else sMensaje = sMessageQuery;
                    }
                //  Paso 5: notificar el resultado.
                    return (eResult);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_009", new string[] { MsgManager.ExcepMsg(ex), sMessageQuery });
                return (ResultOpBD.Error);
            }
            finally
            {
                if ((evEstadoConexionCambiado != null) && (bMostrarEstadoConexion))
                           evEstadoConexionCambiado(dbkaApplication, guidConnection, sceaState);
            }
        }

        #endregion

        #region Ejecución de Procedimientos Almacenados

        /// <summary>
        /// Ejecuta, en una transacción, el Stored Procedure indicado por la cadena de carácteres pasada cómo 
        /// parámetro y devuelve un DataSet con el resultado.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación que desea realizar la consulta.</param>
        /// <param name="guidTransaction">Identificador de la transacción sobre la que se desea realizar la operación.</param>
        /// <param name="sNameStored">Nombre del procedimiento a ejecutar.</param>
        /// <param name="oDbParameter">Parámetros con los que se desea ejecutar el procedimiento.</param>
        /// <param name="bWithSchema">Indica si se deben utilizar o no las restricciones</param>
        /// <param name="oDataSet">Datos obtenidos al realizar la consulta.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar la descripción del enumerado</returns>
        public static ResultOpBD ExecuteStoredProcedure(DbKeyApp dbkaApplication, Guid guidTransaction, string sNameStored, 
                                                        DbParameter[] oDbParameter, bool bWithSchema, out DataSet oDataSet, 
                                                        out string sMensaje)
        {
            oDataSet = null;
            sMensaje = string.Empty;
            Guid guidConnection = Guid.Empty;
            bool bMostrarEstadoConexion = true;
            string sMessageQuery = string.Empty;
            StateChangeEventArgs sceaState = null;
            try
            {
                //  Variables
                    ResultOpBD eResult;
                    CADbDataConnection oDbDataConnection;
                    
                //  Paso 1: Comprobar que el Connection Manager esté inicializado.
                    if ((eResult = ValidateConnectionManager(dbkaApplication, out sMensaje)) != ResultOpBD.Correct) return (eResult);
                //  Paso 2: Obtener una conexión valida de las libres.
                    if (!htTransactionsInUse.Contains(guidTransaction))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_021");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    guidConnection = (Guid)htTransactionsInUse[guidTransaction];
                    if (!htConnectionsInUse.ContainsKey(guidConnection))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_021");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    oDbDataConnection = (CADbDataConnection)htConnectionsInUse[guidConnection];
                //  Paso 3: Realizar la consulta.
                    if (evEstadoConexionCambiado != null)
                        evEstadoConexionCambiado(dbkaApplication, guidConnection,
                                                 new StateChangeEventArgs(ConnectionState.Open, ConnectionState.Executing));
                    switch (oDbDataConnection.TipoBaseDatos)
                    { 
                        case DBType.SQLSERVER:                            
                             eResult = oDbDataConnection.ExecuteStoredProcedure(sNameStored, oDbParameter, bWithSchema, out oDataSet, out sMessageQuery);                             
                             break;
                        case DBType.PROGRESS:
                             sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                             throw new Exception(MsgManager.ErrorMsg("MSG_ConnectionManager_024"));
                        case DBType.ORACLE:
                             eResult = oDbDataConnection.ExecuteStoredProcedure(sNameStored, oDbParameter, bWithSchema, out oDataSet, out sMessageQuery);
                             break;
                        default:
                             sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_008");
                             sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                             return (ResultOpBD.InternalError);
                    }
                //  Paso 4: Informa del nuevo estado de la conexión y la reasigna.
                    if (eResult != ResultOpBD.Correct)
                    {
                        if (oDbDataConnection.EstadoConexion == null)
                        {
                            string sMessageFinalize;
                            if (FinalizeConnection(ref oDbDataConnection, out sMessageFinalize, false) != ResultOpBD.Correct)
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Broken);
                                sMensaje = sMessageQuery + sMessageFinalize;
                            }
                            else
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Closed);
                                sMensaje = sMessageQuery;
                            }
                            return (eResult);
                        }
                        else sMensaje = sMessageQuery;
                    }
                //  Paso 5: notificar el resultado.
                    return (eResult);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_009", new string[] { MsgManager.ExcepMsg(ex), sMessageQuery });
                return (ResultOpBD.Error);
            }
            finally
            {
                if ((evEstadoConexionCambiado != null) && (bMostrarEstadoConexion))
                           evEstadoConexionCambiado(dbkaApplication, (Guid) guidConnection, sceaState);
            }
        }

        public static ResultOpBD ExecuteStoredProcedure(DbKeyApp dbkaApplication, Guid guidTransaction, string sNameStored, 
                                                        DbParameter[] oDbParameter, bool bWithSchema, 
                                                        out List<object> iRowsAffected, out string sMensaje)
        {
            iRowsAffected = new List<object>();
            sMensaje = string.Empty;
            Guid guidConnection = Guid.Empty;
            bool bMostrarEstadoConexion = true;
            string sMessageQuery = string.Empty;
            StateChangeEventArgs sceaState = null;
            try
            {
                //  Variables
                    ResultOpBD eResult;
                    CADbDataConnection oDbDataConnection;

                //  Paso 1: Comprobar que el Connection Manager esté inicializado.
                    if ((eResult = ValidateConnectionManager(dbkaApplication, out sMensaje)) != ResultOpBD.Correct) return (eResult);
                //  Paso 2: Obtener una conexión valida de las libres.
                    if (!htTransactionsInUse.Contains(guidTransaction))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_021");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    guidConnection = (Guid)htTransactionsInUse[guidTransaction];
                    if (!htConnectionsInUse.ContainsKey(guidConnection))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_021");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    oDbDataConnection = (CADbDataConnection)htConnectionsInUse[guidConnection];
                //  Paso 3: Realizar la consulta.
                    if (evEstadoConexionCambiado != null)
                        evEstadoConexionCambiado(dbkaApplication, guidConnection,
                                                 new StateChangeEventArgs(ConnectionState.Open, ConnectionState.Executing));
                    switch (oDbDataConnection.TipoBaseDatos)
                    {
                        case DBType.SQLSERVER:
                             eResult = oDbDataConnection.ExecuteStoredProcedure(sNameStored, oDbParameter, bWithSchema, out  iRowsAffected, out sMessageQuery);
                             break;
                        case DBType.PROGRESS:
                             sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                             throw new Exception(MsgManager.ErrorMsg("MSG_ConnectionManager_024"));
                        case DBType.ORACLE:
                             eResult = oDbDataConnection.ExecuteStoredProcedure(sNameStored, oDbParameter, bWithSchema, out  iRowsAffected, out sMessageQuery);
                             break;
                        default:
                             sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_008");
                             sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                             return (ResultOpBD.InternalError);
                    }
                //  Paso 4: Informa del nuevo estado de la conexión y la reasigna.
                    if (eResult != ResultOpBD.Correct)
                    {
                        if (oDbDataConnection.EstadoConexion == null)
                        {
                            string sMessageFinalize;
                            if (FinalizeConnection(ref oDbDataConnection, out sMessageFinalize, false) != ResultOpBD.Correct)
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Broken);
                                sMensaje = sMessageQuery + sMessageFinalize;
                            }
                            else
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Closed);
                                sMensaje = sMessageQuery;
                            }
                            return (eResult);
                        }
                        else sMensaje = sMessageQuery;
                    }
                //  Paso 5: notificar el resultado.
                    return (eResult);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_009", new string[] { MsgManager.ExcepMsg(ex), sMessageQuery });
                return (ResultOpBD.Error);
            }
            finally
            {
                if ((evEstadoConexionCambiado != null) && (bMostrarEstadoConexion))
                    evEstadoConexionCambiado(dbkaApplication, (Guid)guidConnection, sceaState);
            }
        }

        #endregion

        #region Obtención de un Escalar

        /// <summary>
        /// Realiza, de manera transaccional, la consulta indicada por la cadena de carácteres pasada cómo parámetro y
        /// devuelve un DataSet con el resultado.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación que desea realizar la consulta.</param>
        /// <param name="guidTransaction">Identificador de la transacción sobre la que se desea realizar la operación.</param>
        /// <param name="sSql">Consulta a realizar</param>
        /// <param name="iEscalarResult">Resultado de la ejecución de la consulta</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar la descripción del enumerado</returns>
        public static ResultOpBD GetEscalarFromSql(DbKeyApp dbkaApplication, Guid guidTransaction, string sSql, 
                                                   out int iEscalarResult, out string sMensaje)
        {
            iEscalarResult = -1;
            sMensaje = string.Empty;
            Guid guidConnection = Guid.Empty;
            bool bMostrarEstadoConexion = true;
            string sMessageQuery = string.Empty;
            StateChangeEventArgs sceaState = null;
            try
            {
                //  Variables
                    ResultOpBD eResult;
                    CADbDataConnection oDbDataConnection;
                    
                //  Paso 1: Comprobar que el Connection Manager esté inicializado.
                    if ((eResult = ValidateConnectionManager(dbkaApplication, out sMensaje)) != ResultOpBD.Correct) return (eResult);
                //  Paso 2: Obtener una conexión valida de las libres.
                    if (!htTransactionsInUse.Contains(guidTransaction))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_021");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    guidConnection = (Guid)htTransactionsInUse[guidTransaction];
                    if (!htConnectionsInUse.ContainsKey(guidConnection))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_021");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    oDbDataConnection = (CADbDataConnection)htConnectionsInUse[guidConnection];
                //  Paso 3: Realizar la consulta.
                    if (evEstadoConexionCambiado != null)
                        evEstadoConexionCambiado(dbkaApplication, guidConnection,
                                                 new StateChangeEventArgs(ConnectionState.Open, ConnectionState.Executing));
                    switch (oDbDataConnection.TipoBaseDatos)
                    { 
                        case DBType.SQLSERVER:
                             eResult = oDbDataConnection.GetEscalarFromSql(sSql, out iEscalarResult, out sMessageQuery);
                             break;
                        case DBType.PROGRESS:
                             eResult = oDbDataConnection.GetEscalarFromSql(sSql, out iEscalarResult, out sMessageQuery);
                             break;
                        case DBType.ORACLE:
                             eResult = oDbDataConnection.GetEscalarFromSql(sSql, out iEscalarResult, out sMessageQuery);
                             break;
                        default:
                             sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_008");
                             sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                             return (ResultOpBD.InternalError);
                    }
                //  Paso 4: Informa del nuevo estado de la conexión y la reasigna.
                    if (eResult != ResultOpBD.Correct)
                    {
                        if (oDbDataConnection.EstadoConexion == null)
                        {
                            string sMessageFinalize;
                            if (FinalizeConnection(ref oDbDataConnection, out sMessageFinalize, false) != ResultOpBD.Correct)
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Broken);
                                sMensaje = sMessageQuery + sMessageFinalize;
                            }
                            else
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Closed);
                                sMensaje = sMessageQuery;
                            }
                            return (eResult);
                        }
                        else sMensaje = sMessageQuery;
                    }
                //  Paso 5: notificar el resultado.
                    return (eResult);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_009", new string[] { MsgManager.ExcepMsg(ex), sMessageQuery });
                return (ResultOpBD.Error);
            }
            finally
            {
                if ((evEstadoConexionCambiado != null) && (bMostrarEstadoConexion))
                           evEstadoConexionCambiado(dbkaApplication, (Guid) guidConnection, sceaState);
            }
        }   

        #endregion

        #region Operaciones que modifican el contenido de la Base de Datos

        /// <summary>
        /// Ejecuta, de manera transaccional la consulta de inserción, borrado o cambio, pasada cómo parámetro.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación que desea realizar la consulta.</param>
        /// <param name="guidTransaction">Identificador de la transacción sobre la que se desea realizar la operación.</param>
        /// <param name="sSql">Consulta a realizar</param>
        /// <param name="iNumRowsAfected">Resultado de la ejecución de la consulta</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar la descripción del enumerado</returns>
        public static ResultOpBD ExecuteCommandFromSql(DbKeyApp dbkaApplication, Guid guidTransaction, string sSql, 
                                                       out int iNumRowsAfected, out string sMensaje)
        {
            iNumRowsAfected = -1;
            sMensaje = string.Empty;
            Guid guidConnection = Guid.Empty;
            bool bMostrarEstadoConexion = true;
            string sMessageQuery = string.Empty;
            StateChangeEventArgs sceaState = null;
            try
            {
                //  Variables
                    ResultOpBD eResult;
                    CADbDataConnection oDbDataConnection;
                    
                //  Paso 1: Comprobar que el Connection Manager esté inicializado.
                    if ((eResult = ValidateConnectionManager(dbkaApplication, out sMensaje)) != ResultOpBD.Correct) return (eResult);
                //  Paso 2: Obtener una conexión valida de las libres.
                    if (!htTransactionsInUse.Contains(guidTransaction))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_021");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    guidConnection = (Guid)htTransactionsInUse[guidTransaction];
                    if (!htConnectionsInUse.ContainsKey(guidConnection))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_021");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    oDbDataConnection = (CADbDataConnection)htConnectionsInUse[guidConnection];
                //  Paso 3: Realizar la consulta.
                    if (evEstadoConexionCambiado != null)
                        evEstadoConexionCambiado(dbkaApplication, guidConnection,
                                                 new StateChangeEventArgs(ConnectionState.Open, ConnectionState.Executing));
                    switch (oDbDataConnection.TipoBaseDatos)
                    { 
                        case DBType.SQLSERVER:
                             eResult = oDbDataConnection.ExecuteCommandFromSql(sSql, out iNumRowsAfected, out sMessageQuery);
                             break;
                        case DBType.PROGRESS:
                             eResult = oDbDataConnection.ExecuteCommandFromSql(sSql, out iNumRowsAfected, out sMessageQuery);
                             break;
                        case DBType.ORACLE:
                             eResult = oDbDataConnection.ExecuteCommandFromSql(sSql, out iNumRowsAfected, out sMessageQuery);
                             break;
                        default:
                             sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_008");
                             sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                             return (ResultOpBD.InternalError);
                    }
                //  Paso 4: Informa del nuevo estado de la conexión y la reasigna.
                    if (eResult != ResultOpBD.Correct)
                    {
                        if (oDbDataConnection.EstadoConexion == null)
                        {
                            string sMessageFinalize;
                            if (FinalizeConnection(ref oDbDataConnection, out sMessageFinalize, false) != ResultOpBD.Correct)
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Broken);
                                sMensaje = sMessageQuery + sMessageFinalize;
                            }
                            else
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Closed);
                                sMensaje = sMessageQuery;
                            }
                            return (eResult);
                        }
                        else sMensaje = sMessageQuery;
                    }
                //  Paso 5: notificar el resultado.
                    return (eResult);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_009", new string[] { MsgManager.ExcepMsg(ex), sMessageQuery });
                return (ResultOpBD.Error);
            }
            finally
            {
                if ((evEstadoConexionCambiado != null) && (bMostrarEstadoConexion))
                           evEstadoConexionCambiado(dbkaApplication, (Guid) guidConnection, sceaState);
            }
        }

        #endregion

        #region Relacionadas con el control de Transacciones

        #region Begin Transaction

        /// <summary>
        /// Método encargado de Iniciar una Transacción.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación que desea realizar la consulta.</param>
        /// <param name="guidTransaction">Identificador de la transacción a realizar.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar la descripción del enumerado</returns>
        public static ResultOpBD BeginTransaction(DbKeyApp dbkaApplication, IsolationLevel oIsolationLevel, 
                                                  out Guid guidTransaction, out string sMensaje)
        {
            sMensaje = string.Empty;
            guidTransaction = Guid.Empty;
            bool bMutexActivated = false;
            Guid guidConnection = Guid.Empty;
            bool bMostrarEstadoConexion = true;
            StateChangeEventArgs sceaState = null;
            string sMessageTransaction = string.Empty;
            try
            {
                //  Variables
                    ResultOpBD eResult;
                    CADbDataConnection oDbDataConnection;

                //  Paso 0: Activamos el controlador de exclusión mútua para este método
                    bMutexActivated = mtxManageTransactions.WaitOne(); //(TIMEOUT_FINDCONNECTION, false);
                //  Paso 1: Comprobar que el Connection Manager esté inicializado.
                    if ((eResult = ValidateConnectionManager(dbkaApplication, out sMensaje)) != ResultOpBD.Correct)
                        return (eResult);
                //  Paso 2: Obtener una conexión valida de las libres.
                    if ((eResult = GetConnection(dbkaApplication, out oDbDataConnection, out sMensaje)) != ResultOpBD.Correct)
                    {
                        bMostrarEstadoConexion = false;
                        return (eResult);
                    }
                    else guidConnection = (Guid)oDbDataConnection.Identifier;
                //  Paso 3: Crear el Identificador de la transacción.
                    while ((guidTransaction == Guid.Empty) || (htTransactionsInUse.Contains(guidTransaction)))
                    {
                        guidTransaction = Guid.NewGuid();
                    }
                //  Paso 4: Inicia la Transacción.
                    if (evEstadoConexionCambiado != null)
                        evEstadoConexionCambiado(dbkaApplication, guidConnection,
                                                 new StateChangeEventArgs(ConnectionState.Open, ConnectionState.Executing));
                    switch (oDbDataConnection.TipoBaseDatos)
                    { 
                        case DBType.SQLSERVER:
                             eResult = oDbDataConnection.BeginTransaction(oIsolationLevel, out sMessageTransaction);
                             break;
                        case DBType.PROGRESS:
                             eResult = oDbDataConnection.BeginTransaction(oIsolationLevel, out sMessageTransaction);
                             break;
                        case DBType.ORACLE:
                             eResult = oDbDataConnection.BeginTransaction(oIsolationLevel, out sMessageTransaction);
                             break;
                        default:
                             sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_008");
                             sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                             return (ResultOpBD.InternalError);
                    }
                //  Paso 5: Informa del nuevo estado de la conexión y la finaliza si es erróneo.
                    if (eResult != ResultOpBD.Correct)
                    {
                        if (oDbDataConnection.EstadoConexion == null)
                        {
                            string sMessageFinalize;
                            if (FinalizeConnection(ref oDbDataConnection, out sMessageFinalize, false) != ResultOpBD.Correct)
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Broken);
                                sMensaje = sMessageTransaction + sMessageFinalize;
                            }
                            else
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Closed);
                                sMensaje = sMessageTransaction;
                            }
                            return (eResult);
                        }
                        else sMensaje = sMessageTransaction;
                    }
                    else
                    {
                        htTransactionsInUse.Add(guidTransaction, guidConnection);
                        oDbDataConnection.TransactionId = guidTransaction;
                        oDbDataConnection.StartTransactionTimeOut();
                    }
                //  Paso 6: notificar el resultado.
                    return (eResult);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_009", new string[] { MsgManager.ExcepMsg(ex), sMessageTransaction });
                return (ResultOpBD.Error);
            }
            finally
            {
                if (bMutexActivated) mtxManageTransactions.ReleaseMutex();
                if ((evEstadoConexionCambiado != null) && (bMostrarEstadoConexion))
                           evEstadoConexionCambiado(dbkaApplication, guidConnection, sceaState);
            }
        }

        /// <summary>
        /// Método encargado de Iniciar una Transacción.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación que desea realizar la consulta.</param>
        /// <param name="guidTransaction">Identificador de la transacción a realizar.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar la descripción del enumerado</returns>
        public static ResultOpBD BeginTransaction(DbKeyApp dbkaApplication, out Guid guidTransaction, out string sMensaje)
        {
            sMensaje = string.Empty;
            guidTransaction = Guid.Empty;
            bool bMutexActivated = false;
            Guid guidConnection = Guid.Empty;
            bool bMostrarEstadoConexion = true;
            StateChangeEventArgs sceaState = null;
            string sMessageTransaction = string.Empty;
            try
            {
                //  Variables
                    ResultOpBD eResult;
                    CADbDataConnection oDbDataConnection;

                //  Paso 0: Activamos el controlador de exclusión mútua para este método
                    bMutexActivated = mtxManageTransactions.WaitOne(); //(TIMEOUT_FINDCONNECTION, false);
                //  Paso 1: Comprobar que el Connection Manager esté inicializado.
                    if ((eResult = ValidateConnectionManager(dbkaApplication, out sMensaje)) != ResultOpBD.Correct)
                        return (eResult);
                //  Paso 2: Obtener una conexión valida de las libres.
                    if ((eResult = GetConnection(dbkaApplication, out oDbDataConnection, out sMensaje)) != ResultOpBD.Correct)
                    {
                        bMostrarEstadoConexion = false;
                        return (eResult);
                    }
                    else guidConnection = (Guid)oDbDataConnection.Identifier;
                //  Paso 3: Crear el Identificador de la transacción.
                    while ((guidTransaction == Guid.Empty) || (htTransactionsInUse.Contains(guidTransaction)))
                    {
                        guidTransaction = Guid.NewGuid();
                    }
                //  Paso 4: Inicia la Transacción.
                    if (evEstadoConexionCambiado != null)
                        evEstadoConexionCambiado(dbkaApplication, guidConnection,
                                                 new StateChangeEventArgs(ConnectionState.Open, ConnectionState.Executing));
                    switch (oDbDataConnection.TipoBaseDatos)
                    { 
                        case DBType.SQLSERVER:
                             eResult = oDbDataConnection.BeginTransaction(out sMessageTransaction);
                             break;
                        case DBType.PROGRESS:
                             eResult = oDbDataConnection.BeginTransaction(out sMessageTransaction);
                             break;
                        case DBType.ORACLE:
                             eResult = oDbDataConnection.BeginTransaction(out sMessageTransaction);
                             break;
                        default:
                             sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_008");
                             sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                             return (ResultOpBD.InternalError);
                    }
                //  Paso 5: Informa del nuevo estado de la conexión y la finaliza si es erróneo.
                    if (eResult != ResultOpBD.Correct)
                    {
                        if (oDbDataConnection.EstadoConexion == null)
                        {
                            string sMessageFinalize;
                            if (FinalizeConnection(ref oDbDataConnection, out sMessageFinalize, false) != ResultOpBD.Correct)
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Broken);
                                sMensaje = sMessageTransaction + sMessageFinalize;
                            }
                            else
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Closed);
                                sMensaje = sMessageTransaction;
                            }
                            return (eResult);
                        }
                        else sMensaje = sMessageTransaction;
                    }
                    else
                    {
                        htTransactionsInUse.Add(guidTransaction, guidConnection);
                        oDbDataConnection.TransactionId = guidTransaction;
                        oDbDataConnection.StartTransactionTimeOut();
                    }
                //  Paso 6: notificar el resultado.
                    return (eResult);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_009", new string[] { MsgManager.ExcepMsg(ex), sMessageTransaction });
                return (ResultOpBD.Error);
            }
            finally
            {
                if (bMutexActivated) mtxManageTransactions.ReleaseMutex();
                if ((evEstadoConexionCambiado != null) && (bMostrarEstadoConexion))
                           evEstadoConexionCambiado(dbkaApplication, guidConnection, sceaState);
            }
        }

        #endregion

        #region Commit

        /// <summary>
        /// Método encargado de Iniciar una Transacción.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación que desea realizar la consulta.</param>
        /// <param name="guidTransaction">Identificador de la conexión de la transacción a aceptar.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar la descripción del enumerado</returns>
        public static ResultOpBD Commit(DbKeyApp dbkaApplication, Guid guidTransaction, out string sMensaje)
        {
            sMensaje = string.Empty;
            Guid guidConnection = Guid.NewGuid();
            StateChangeEventArgs sceaState = null;
            string sMessageTransaction = string.Empty;
            bool bMutexActivated = false, bMostrarEstadoConexion = true;
            try
            {
                //  Variables
                    ResultOpBD eResult;
                    CADbDataConnection oDbDataConnection;

                //  Paso 0: Activamos el controlador de exclusión mútua para este método
                    bMutexActivated = mtxManageTransactions.WaitOne(); //(TIMEOUT_FINDCONNECTION, false);
                //  Paso 1: Comprobar que el Connection Manager esté inicializado.
                    if ((eResult = ValidateConnectionManager(dbkaApplication, out sMensaje)) != ResultOpBD.Correct) return (eResult);
                //  Paso 2: Comprobar que el identificador de conexión pasado cómo parámetro corresponda a una 
                //          conexión real.
                    if (!htTransactionsInUse.Contains(guidTransaction))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_021");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    guidConnection = (Guid)htTransactionsInUse[guidTransaction];
                    if (!htConnectionsInUse.ContainsKey(guidConnection))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_021");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    oDbDataConnection = (CADbDataConnection)htConnectionsInUse[guidConnection];
                //  Paso 3: Finaliza la Transacción Aceptando los Cambios Realizados.
                    switch (oDbDataConnection.TipoBaseDatos)
                    { 
                        case DBType.SQLSERVER:
                            eResult = oDbDataConnection.CommitTransaction(out sMessageTransaction);
                             break;
                        case DBType.PROGRESS:
                             eResult = oDbDataConnection.CommitTransaction(out sMessageTransaction);
                             break;
                        case DBType.ORACLE:
                             eResult = oDbDataConnection.CommitTransaction(out sMessageTransaction);
                             break;
                        default:
                             sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_008");
                             sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                             return (ResultOpBD.InternalError);
                    }
                //  Paso 4: Informa del nuevo estado de la conexión y la reasigna.
                    if (eResult != ResultOpBD.Correct)
                    {
                        if (oDbDataConnection.EstadoConexion == null)
                        {
                            string sMessageFinalize;
                            if (FinalizeConnection(ref oDbDataConnection, out sMessageFinalize, false) != ResultOpBD.Correct)
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Broken);
                                sMensaje = sMessageTransaction + sMessageFinalize;
                            }
                            else
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Closed);
                                sMensaje = sMessageTransaction;
                            }
                            return (eResult);
                        }
                        else sMensaje = sMessageTransaction;
                    }
                    else
                    {
                        oDbDataConnection.StopTransactionTimeOut();
                        htTransactionsInUse.Remove(guidTransaction);
                        oDbDataConnection.TransactionId = Guid.Empty;
                    }
                    string sMessageAssign;
                    if (AssignConnection(dbkaApplication, guidConnection, ref oDbDataConnection, out sceaState,
                                         out sMessageAssign) != ResultOpBD.Correct)
                    {
                        sMensaje = sMessageTransaction + sMessageAssign;
                    }
                //  Paso 5: notificar el resultado.
                    return (eResult);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_009", new string[] { MsgManager.ExcepMsg(ex), sMessageTransaction });
                return (ResultOpBD.Error);
            }
            finally
            {
                if (bMutexActivated) mtxManageTransactions.ReleaseMutex();
                if ((evEstadoConexionCambiado != null) && (bMostrarEstadoConexion))
                           evEstadoConexionCambiado(dbkaApplication, guidConnection, sceaState);
            }
        }

        #endregion

        #region Rollback

        /// <summary>
        /// Método encargado de Iniciar una Transacción.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación que desea realizar la consulta.</param>
        /// <param name="guidTransaction">Identificador de la conexión de la transacción a aceptar.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar la descripción del enumerado</returns>
        public static ResultOpBD Rollback(DbKeyApp dbkaApplication, Guid guidTransaction, out string sMensaje)
        {
            sMensaje = string.Empty;
            Guid guidConnection = Guid.NewGuid();
            StateChangeEventArgs sceaState = null;
            string sMessageTransaction = string.Empty;
            bool bMutexActivated = false, bMostrarEstadoConexion = true;
            try
            {
                //  Variables
                    ResultOpBD eResult;
                    CADbDataConnection oDbDataConnection;

                //  Paso 0: Activamos el controlador de exclusión mútua para este método
                    bMutexActivated = mtxManageTransactions.WaitOne(); //(TIMEOUT_FINDCONNECTION, false);
                //  Paso 1: Comprobar que el Connection Manager esté inicializado.
                    if ((eResult = ValidateConnectionManager(dbkaApplication, out sMensaje)) != ResultOpBD.Correct) return (eResult);
                //  Paso 2: Comprobar que el identificador de conexión pasado cómo parámetro corresponda a una 
                //          conexión real.
                    if (!htTransactionsInUse.Contains(guidTransaction))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_021");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    guidConnection = (Guid)htTransactionsInUse[guidTransaction];
                    if (!htConnectionsInUse.ContainsKey(guidConnection))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_021");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    oDbDataConnection = (CADbDataConnection)htConnectionsInUse[guidConnection];
                //  Paso 3: Finaliza la Transacción Aceptando los Cambios Realizados.
                    switch (oDbDataConnection.TipoBaseDatos)
                    { 
                        case DBType.SQLSERVER:
                             eResult = oDbDataConnection.RollbackTransaction(out sMessageTransaction);
                             break;
                        case DBType.PROGRESS:
                             eResult = oDbDataConnection.RollbackTransaction(out sMessageTransaction);
                             break;
                        case DBType.ORACLE:
                             eResult = oDbDataConnection.RollbackTransaction(out sMessageTransaction);
                             break;
                        default:
                             sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_008");
                             sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                             return (ResultOpBD.InternalError);
                    }
                //  Paso 4: Informa del nuevo estado de la conexión y la reasigna.
                    if (eResult != ResultOpBD.Correct)
                    {
                        if (oDbDataConnection.EstadoConexion == null)
                        {
                            string sMessageFinalize;
                            if (FinalizeConnection(ref oDbDataConnection, out sMessageFinalize, false) != ResultOpBD.Correct)
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Broken);
                                sMensaje = sMessageTransaction + sMessageFinalize;
                            }
                            else
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Closed);
                                sMensaje = sMessageTransaction;
                            }
                            return (eResult);
                        }
                        else sMensaje = sMessageTransaction;
                    }
                    else
                    {
                        oDbDataConnection.StopTransactionTimeOut();
                        htTransactionsInUse.Remove(guidTransaction);
                        oDbDataConnection.TransactionId = Guid.Empty;
                    }
                    string sMessageAssign;
                    if (AssignConnection(dbkaApplication, guidConnection, ref oDbDataConnection, out sceaState,
                                         out sMessageAssign) != ResultOpBD.Correct)
                    {
                        sMensaje = sMessageTransaction + sMessageAssign;
                    }
                //  Paso 5: notificar el resultado.
                    return (eResult);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_009", new string[] { MsgManager.ExcepMsg(ex), sMessageTransaction });
                return (ResultOpBD.Error);
            }
            finally
            {
                if (bMutexActivated) mtxManageTransactions.ReleaseMutex();
                if ((evEstadoConexionCambiado != null) && (bMostrarEstadoConexion))
                           evEstadoConexionCambiado(dbkaApplication, guidConnection, sceaState);
            }
        }

        #endregion

        #endregion

        #endregion

        #region Operaciones con DataReaders

        #region GetDataReader
        
        /// <summary>
        /// Método encargado de Iniciar una operación con un DataReader.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación que desea realizar la consulta.</param>
        /// <param name="guidDataReader">Identificador de la transacción a realizar.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar la descripción del enumerado</returns>
        public static ResultOpBD GetDataReader(DbKeyApp dbkaApplication, string sSql, int iSecondsInterval, 
                                               out DbDataReader drdData, out Guid guidDataReader, out string sMensaje)
        {
            drdData = null;
            sMensaje = string.Empty;
            guidDataReader = Guid.Empty;
            bool bMutexActivated = false;
            Guid guidConnection = Guid.Empty;
            bool bMostrarEstadoConexion = true;
            StateChangeEventArgs sceaState = null;
            string sMessageDataReader = string.Empty;
            try
            {
                //  Variables
                    ResultOpBD eResult;
                    CADbDataConnection oDbDataConnection;

                //  Paso 0: Activamos el controlador de exclusión mútua para este método
                    bMutexActivated = mtxManageDataReader.WaitOne(); //(TIMEOUT_FINDCONNECTION, false);
                //  Paso 1: Comprobar que el Connection Manager esté inicializado.
                    if ((eResult = ValidateConnectionManager(dbkaApplication, out sMensaje)) != ResultOpBD.Correct)
                        return (eResult);
                //  Paso 2: Obtener una conexión valida de las libres.
                    if ((eResult = GetConnection(dbkaApplication, out oDbDataConnection, out sMensaje)) != ResultOpBD.Correct)
                    {
                        bMostrarEstadoConexion = false;
                        return (eResult);
                    }
                    else guidConnection = (Guid)oDbDataConnection.Identifier;
                //  Paso 3: Crear el Identificador de la transacción.
                    while ((guidDataReader == Guid.Empty) || (htDataReadersInUse.Contains(guidDataReader)))
                    {
                        guidDataReader = Guid.NewGuid();
                    }
                //  Paso 4: Inicia la Transacción.
                    if (evEstadoConexionCambiado != null)
                        evEstadoConexionCambiado(dbkaApplication, guidConnection,
                                                 new StateChangeEventArgs(ConnectionState.Open, ConnectionState.Executing));
                    switch (oDbDataConnection.TipoBaseDatos)
                    { 
                        case DBType.SQLSERVER:
                             eResult = oDbDataConnection.GetDataReader(sSql, out drdData, out sMensaje);
                             break;
                        case DBType.PROGRESS:
                             eResult = oDbDataConnection.GetDataReader(sSql, out drdData, out sMensaje);
                             break;
                        case DBType.ORACLE:
                             eResult = oDbDataConnection.GetDataReader(sSql, out drdData, out sMensaje);
                             break;
                        default:
                             sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_008");
                             sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                             return (ResultOpBD.InternalError);
                    }
                //  Paso 5: Informa del nuevo estado de la conexión y la finaliza si es erróneo.
                    if (eResult != ResultOpBD.Correct)
                    {
                        if (oDbDataConnection.EstadoConexion == null)
                        {
                            string sMessageFinalize;
                            if (FinalizeConnection(ref oDbDataConnection, out sMessageFinalize, false) != ResultOpBD.Correct)
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Broken);
                                sMensaje = sMessageDataReader + sMessageFinalize;
                            }
                            else
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Closed);
                                sMensaje = sMessageDataReader;
                            }
                            return (eResult);
                        }
                        else sMensaje = sMessageDataReader;
                    }
                    else
                    {
                        htDataReadersInUse.Add(guidDataReader, guidConnection);
                        oDbDataConnection.DataReaderId = guidDataReader;
                        oDbDataConnection.StartDataReaderTimeOut(iSecondsInterval);
                    }
                //  Paso 6: notificar el resultado.
                    return (eResult);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_009", new string[] { MsgManager.ExcepMsg(ex), sMessageDataReader });
                return (ResultOpBD.Error);
            }
            finally
            {
                if (bMutexActivated) mtxManageDataReader.ReleaseMutex();
                if ((evEstadoConexionCambiado != null) && (bMostrarEstadoConexion))
                           evEstadoConexionCambiado(dbkaApplication, guidConnection, sceaState);
            }
        }

        #endregion

        #region CloseDataReader

        /// <summary>
        /// Método encargado de Finalizar una operación realizada mediante un DataReader.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación que desea realizar la consulta.</param>
        /// <param name="guidDataReader">Identificador de la conexión de la opearción a finalizar.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar la descripción del enumerado</returns>
        public static ResultOpBD CloseDataReader(DbKeyApp dbkaApplication, Guid guidDataReader, 
                                                 out string sMensaje)
        {
            sMensaje = string.Empty;
            Guid guidConnection = Guid.NewGuid();
            StateChangeEventArgs sceaState = null;
            string sMessageDataReader = string.Empty;
            bool bMutexActivated = false, bMostrarEstadoConexion = true;
            try
            {
                //  Variables
                    ResultOpBD eResult;
                    CADbDataConnection oDbDataConnection;

                //  Paso 0: Activamos el controlador de exclusión mútua para este método
                    bMutexActivated = mtxManageDataReader.WaitOne(); //(TIMEOUT_FINDCONNECTION, false);
                //  Paso 1: Comprobar que el Connection Manager esté inicializado.
                    if ((eResult = ValidateConnectionManager(dbkaApplication, out sMensaje)) != ResultOpBD.Correct) return (eResult);
                //  Paso 2: Comprobar que el identificador de conexión pasado cómo parámetro corresponda a una 
                //          conexión real.
                    if (!htDataReadersInUse.Contains(guidDataReader))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_023");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    guidConnection = (Guid)htDataReadersInUse[guidDataReader];
                    if (!htConnectionsInUse.ContainsKey(guidConnection))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_021");
                        bMostrarEstadoConexion = false;
                        return (ResultOpBD.DataInputError);
                    }
                    oDbDataConnection = (CADbDataConnection)htConnectionsInUse[guidConnection];
                //  Paso 3: Finaliza la Transacción Aceptando los Cambios Realizados.
                    switch (oDbDataConnection.TipoBaseDatos)
                    { 
                        case DBType.SQLSERVER:
                             eResult = oDbDataConnection.CloseDataReader(out sMessageDataReader);
                             break;
                        case DBType.PROGRESS:
                             eResult = oDbDataConnection.CloseDataReader(out sMessageDataReader);
                             break;
                        case DBType.ORACLE:
                             eResult = oDbDataConnection.CloseDataReader(out sMessageDataReader);
                             break;
                        default:
                             sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_008");
                             sceaState = new StateChangeEventArgs(ConnectionState.Executing, (ConnectionState)oDbDataConnection.EstadoConexion);
                             return (ResultOpBD.InternalError);
                    }
                //  Paso 4: Informa del nuevo estado de la conexión y la reasigna.
                    if (eResult != ResultOpBD.Correct)
                    {
                        if (oDbDataConnection.EstadoConexion == null)
                        {
                            string sMessageFinalize;
                            if (FinalizeConnection(ref oDbDataConnection, out sMessageFinalize, false) != ResultOpBD.Correct)
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Broken);
                                sMensaje = sMessageDataReader + sMessageFinalize;
                            }
                            else
                            {
                                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Closed);
                                sMensaje = sMessageDataReader;
                            }
                            return (eResult);
                        }
                        else sMensaje = sMessageDataReader;
                    }
                    else
                    {
                        oDbDataConnection.StopTransactionTimeOut();
                        htDataReadersInUse.Remove(guidDataReader);
                        oDbDataConnection.DataReaderId = Guid.Empty;
                    }
                    string sMessageAssign;
                    if (AssignConnection(dbkaApplication, guidConnection, ref oDbDataConnection, out sceaState,
                                         out sMessageAssign) != ResultOpBD.Correct)
                    {
                        sMensaje = sMessageDataReader + sMessageAssign;
                    }
                //  Paso 5: notificar el resultado.
                    return (eResult);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_009", new string[] { MsgManager.ExcepMsg(ex), sMessageDataReader });
                return (ResultOpBD.Error);
            }
            finally
            {
                if (bMutexActivated) mtxManageDataReader.ReleaseMutex();
                if ((evEstadoConexionCambiado != null) && (bMostrarEstadoConexion))
                           evEstadoConexionCambiado(dbkaApplication, guidConnection, sceaState);
            }
        }

        #endregion

        #endregion

        #endregion

        #region Obtención de una clave de uso

        /// <summary>
        /// Método encargado de obtener la identificación con la que el programa que la reciba podrá
        /// interactuar con la clase de control de los intercambios de información con  la  Base  de 
        /// Datos.
        /// 
        ///         params[0] :- contiene el tipo de Base de Datos con la que se desea interactuar.
        /// 
        /// </summary>
        /// <param name="guidApplicationID">Identificador asociado a la aplicación que pide la conexión.</param>
        /// <param name="iNumMaxConnections">Número máximo de conexiones con las que puede trabajar esta clave</param>
        /// <param name="dbkaApplication">Clave que permitirá a la aplicación interactuar con la Base de Datos.</param>
        /// <param name="sMensaje">mensaje de error, si se produce uno.</param>
        /// <param name="optParams">Parámetros con los que se crea la conexión.</param>
        /// <returns>Mirar el enumerado.</returns>
        public static ResultOpBD GetKeyAppBD(Guid? guidApplicationID, int iNumMaxConnections, out DbKeyApp dbkaApplication, 
                                             out string sMensaje, params object[] optParams)
        {
            dbkaApplication = null;
            sMensaje = string.Empty;
            bool bMutexActivated = false;
            try
            {
                //  Variables.
                    DBType eDBType;
                    ResultOpBD eResult;
                    IDbDataConnectionEventArgs eaDC = null;

                //  Valida los datos de entrada.
                    if ((eResult = ValidateDataKeyAppBD(guidApplicationID, out eDBType, out sMensaje, optParams)) != 
                         ResultOpBD.Correct) return (eResult);
                //  Inicia la parte de código que se debe realizar en Exclusión Mútua.
                    bMutexActivated = mtxGetKey.WaitOne(); //(TIMEOUT_GET_KEY, false);
                //  Una vez validados los parámetros de entrada se procede a crear la nueva clave.
                    if ((eResult = ObtenerClave(guidApplicationID, out dbkaApplication, out sMensaje)) != 
                         ResultOpBD.Correct) return (eResult);
                //  Crea la información asociada a la clave con los datos pasados como parámetro.
                    switch (eDBType) // Solo pueden ser valores válidos, ya que nos hemos encargado de ello.
                    {
                        case DBType.SQLSERVER:
                             eaDC = DbConnectionSQLServer.CreateDataKey(guidApplicationID, iNumMaxConnections, out sMensaje, optParams);
                             if (eaDC == null) return (ResultOpBD.InternalError);
                             break;
                        case DBType.PROGRESS:
                             eaDC = DbConnectionPROGRESS.CreateDataKey(guidApplicationID, iNumMaxConnections, out sMensaje, optParams);
                             if (eaDC == null) return (ResultOpBD.InternalError);
                             break;
                        case DBType.ORACLE:
                             eaDC = DbConnectionORACLE.CreateDataKey(guidApplicationID, iNumMaxConnections, out sMensaje, optParams);
                             if (eaDC == null) return (ResultOpBD.InternalError);
                             break;
                    }                
                //  Se almacena la clave y los datos a los que pertenece.
                    htKeys.Add(dbkaApplication, eaDC);
                //  Indica que la operación ha finalizado de manera correcta.
                    return (ResultOpBD.Correct);
            }
            catch (AbandonedMutexException  ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_ConnectionManager_001", ex);
                return (ResultOpBD.InternalError);
            }
            catch (Exception ex)
            {
                sMensaje += MsgManager.ExcepMsg(ex);
                return (ResultOpBD.Error);
            }
            finally
            {
                //  Libera el semáforo que ha permitido ejecutar el código en exclusión mútua.
                    if (bMutexActivated) mtxGetKey.ReleaseMutex();
            }
        }

        #region Valida los parámetros de entrada

        /// <summary>
        /// Método encargado de validar los parámetros con los que se pretende crear la nueva clave.
        /// </summary>
        /// <param name="guidApplicationID">Identificador asociado a la aplicación que pide la conexión.</param>
        /// <param name="eDBType">Tipo de Base de Datos introducido.</param>
        /// <param name="sMensaje">mensaje de error, si se produce uno.</param>
        /// <param name="optParams">Parámetros con los que se crea la conexión.</param>
        /// <returns>Mirar el enumerado.</returns>
        private static ResultOpBD ValidateDataKeyAppBD(Guid? guidApplicationID, out DBType eDBType, out string sMensaje, params object[] optParams)
        {
            sMensaje = string.Empty;
            eDBType = DBType.UNDEFINED;
            try
            {
                //  Valida los datos de entrada.
                    if (!bInitialized)
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_000");
                        return (ResultOpBD.InternalError);
                    }
                    if (guidApplicationID == null)
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_002");
                        return (ResultOpBD.DataInputError);
                    }
                    if ((optParams == null) || (optParams.Length < 1))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_003");
                        return (ResultOpBD.DataInputError);
                    }
                    if ((eDBType = (DBType)optParams[0]) == DBType.UNDEFINED)
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_004");
                        return (ResultOpBD.DataInputError);
                    }
                    return (ResultOpBD.Correct);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_005", ex);
                return (ResultOpBD.InternalError);
            }
        }

        #endregion

        #region Obtiene una clave de aplicación

        /// <summary>
        /// Método encargado de crear una nueva clave devolviendola.
        /// </summary>
        /// <param name="guidApplicationID">Identificador de la aplicación para la que se desea crear la clave.</param>
        /// <param name="dbkaApplication">Clave a crear.</param>
        /// <param name="sMensaje">mensaje de error, si se produce uno.</param>
        /// <returns>Mirar enumerado</returns>
        private static ResultOpBD ObtenerClave(Guid? guidApplicationID, out DbKeyApp dbkaApplication, out string sMensaje)
        {
            dbkaApplication = null;
            sMensaje = string.Empty;
            try
            {
                //  Valida los datos de entrada
                    if (guidApplicationID == null)
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_006");
                        return (ResultOpBD.DataInputError);
                    }
                //  Se crea la clave.
                    bool bResult;
                    int iNumIntents = 0;
                    dbkaApplication = new DbKeyApp(guidApplicationID, out bResult, out sMensaje);
                    while ((htKeys.ContainsKey(dbkaApplication)) && (!bResult) && (iNumIntents < NUM_INTENTS_GET_GUID))
                    {
                        dbkaApplication = new DbKeyApp(guidApplicationID, out bResult, out sMensaje);
                        iNumIntents++;
                    }
                    if (!bResult) return (ResultOpBD.InternalError);
                    if (iNumIntents == NUM_INTENTS_GET_GUID)
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_007");
                        return (ResultOpBD.InternalError);
                    }
                    return (ResultOpBD.Correct);
            }
            catch (Exception ex)
            {
                sMensaje += MsgManager.ExcepMsg(ex);
                return (ResultOpBD.Error);
            }
        }

        #endregion

        #endregion

        #region Relacionados con la Gestión de las Conexiones

        #region Finaliza todas las conexiones de una aplicación

        /// <summary>
        /// Método encargado de finalizar todas las conexiones de una aplicación.  
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada al cliente para el que se realiza la operación</param>
        /// <param name="sMensaje">mensaje de error, si se produce uno.</param>
        /// <returns>Mirar el enumerado.</returns>
        public static ResultOpBD FinalizeAllConnectionsApp(DbKeyApp dbkaApplication, out string sMensaje)
        {
            sMensaje = string.Empty;
            bool bMutexActivated = false;
            try
            {
                //  Variables.
                    ResultOpBD eResult;
                    string sMessageClear, sMessageFinalizeInUse, sMessageFinalizeFree;

                //  Inicializa las variables de retorno.
                    sMessageFinalizeFree = string.Empty;
                    sMessageFinalizeInUse = string.Empty;
                //  Comprobar que el Connection Manager esté inicializado.
                    if ((eResult = ValidateConnectionManager(dbkaApplication, out sMensaje)) != ResultOpBD.Correct)
                          return (eResult);
                //  Activamos el controlador de exclusión mútua para este método
                    bMutexActivated = mtxRefreshConnections.WaitOne(); //TIMEOUT_FINDCONNECTION, false);
                //  Limpiar la cola de conexiones pendientes.
                    CADbDataConnectionEventArgs eadc = (CADbDataConnectionEventArgs)htKeys[dbkaApplication];
                    if ((eadc.ClearPendingConnections(dbkaApplication, ref mtxManageQueue, out sMessageClear) != ResultOpBD.Correct) ||
                        (FinalizeHashtableConnectionsApp(ref htConnectionsInUse, dbkaApplication, out sMessageFinalizeInUse) != ResultOpBD.Correct) ||
                        (FinalizeHashtableConnectionsApp(ref htFreeConnections, dbkaApplication, out sMessageFinalizeFree) != ResultOpBD.Correct))
                    {
                        sMensaje = sMessageClear + sMessageFinalizeInUse + sMessageFinalizeFree;
                        return (ResultOpBD.Warning);
                    }
                    else return (ResultOpBD.Correct);
            }
            catch (AbandonedMutexException ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_ConnectionManager_010", ex);
                return (ResultOpBD.InternalError);
            }
            catch (Exception ex)
            {
                sMensaje += MsgManager.ExcepMsg(ex);
                return (ResultOpBD.Error);
            }
            finally
            {
                if (bMutexActivated) mtxRefreshConnections.ReleaseMutex();
            }
        }

        #region Limpia de elementos de la clave indicada una Tabla de Hash 

        /// <summary>
        /// Método encargado de finalizar todas las conexiones de una aplicación.  
        /// </summary>
        /// <param name="htTemp">Tabla de Hash a manipular.</param>
        /// <param name="dbkaApp">Clave asociada al cliente para el que se realiza la operación</param>
        /// <param name="sMensaje">mensaje de error, si se produce uno.</param>
        /// <returns>Mirar el enumerado.</returns>
        public static ResultOpBD FinalizeHashtableConnectionsApp(ref Hashtable htTemp, DbKeyApp dbkaApp, out string sMensaje)
        {
            sMensaje = string.Empty;
            try
            {
                //  Variables.
                    string sMensajeAux;
                    ResultOpBD eResult;
                    Guid? guidConnectionTemp = null;

                //  Elimina las conexiones en uso para la clave seleccionada.
                    List<Guid> lstGuid = new List<Guid>();
                    StringBuilder sbResult = new StringBuilder(string.Empty);
                    foreach (CADbDataConnection oDbDataConnection in htTemp.Values)
                    {
                        if (oDbDataConnection.DbKeyApplication == dbkaApp)
                        {
                            oDbDataConnection.StopTimeOut();
                            guidConnectionTemp = oDbDataConnection.Identifier;
                            if ((eResult = oDbDataConnection.Finalize(out sMensajeAux)) == ResultOpBD.Correct)
                            {
                                lstGuid.Add((Guid) guidConnectionTemp);
                                if (evFinalizacionConexion != null) evFinalizacionConexion(dbkaApp, guidConnectionTemp, eResult, sMensajeAux);
                            }
                            else sbResult = sbResult.AppendLine(sMensajeAux);
                        }
                    }
                    try
                    {
                        foreach (Guid guidConnection in lstGuid)
                        {
                            htTemp.Remove(guidConnection);
                        }
                    }
                    catch (Exception) { }
                //  Actualiza los mensajes de error que se hayan producido.
                    if (sbResult.ToString() != string.Empty)
                    {
                        sMensaje = MsgManager.ErrorMsg(sbResult.ToString());
                        return (ResultOpBD.Error);
                    }
                    else return (ResultOpBD.Correct);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (ResultOpBD.Error);
            }
        }

        #endregion

        #endregion

        #region Gestionar el Cambio de Estado de la Conexión

        /// <summary>
        /// Método encargado de propagar el cambio de estado de una de las conexiones activas.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada al elemento que produce el evento</param>
        /// <param name="guidConnection">Identificador de la conexión asociada al elemento que produce el evento.</param>
        /// <param name="e">Parámetros con los que se lanza el evento.</param>
        private static void TratarCambioEstadoConexion(DbKeyApp dbkaApplication, Guid guidConnection, StateChangeEventArgs e)
        {
            if (evEstadoConexionCambiado != null) evEstadoConexionCambiado(dbkaApplication, guidConnection, e);
        }

        #endregion

        #region Gestionar el TimeOut de Conexiones

        /// <summary>
        /// Método encargado de gestionar el TimeOut de una de las conexiones creadas.
        /// </summary>
        /// <param name="sender">Objeto que genera el evento.</param>
        private static void TratarTimeOut(CADbDataConnection sender)
        {
            string sMensaje = string.Empty;
            try
            {
                //  No se hace ningún tratamiento excepcional debido a que la gestión del mutex  ya  se  lleva a
                //  cabo en el método FinalizeConnection. Por la misma razón no se envia en este punto el evento
                //  asociado a la finalización de una conexión.
                    FinalizeConnection(ref sender, out sMensaje, true);
            }
            catch (Exception ex)
            {
                //  En este caso se notifica el error mediante un evento de finalización de conexión en el que se
                //  muestra que se ha producido un error.
                    if (evFinalizacionConexion != null) 
                          evFinalizacionConexion(sender.DbKeyApplication, sender.Identifier, ResultOpBD.Error, MsgManager.ExcepMsg(ex));
            }
        }

        #endregion

        #region Gestionar el TimeOut de una Transacción

        /// <summary>
        /// Método encargado de gestionar el TimeOut de una de las conexiones creadas y asociadas a una 
        /// transacción.
        /// </summary>
        /// <param name="sender">Objeto que genera el evento.</param>
        private static void TratarTransactionTimeOut(CADbDataConnection sender)
        {
            string sMensaje = string.Empty;
            try
            {
                //  No se hace ningún tratamiento excepcional debido a que la gestión del mutex  ya  se  lleva a
                //  cabo en el método FinalizeConnection. Por la misma razón no se envia en este punto el evento
                //  asociado a la finalización de una conexión.
                    FinalizeConnection(ref sender, out sMensaje, true);
            }
            catch (Exception ex)
            {
                //  En este caso se notifica el error mediante un evento de finalización de conexión en el que se
                //  muestra que se ha producido un error.
                    if (evFinalizacionConexion != null)
                        evFinalizacionConexion(sender.DbKeyApplication, sender.Identifier, ResultOpBD.Error, MsgManager.ExcepMsg(ex));
            }
        }

        #endregion

        #region Gestionar el TimeOut de una Operación con DataReader

        /// <summary>
        /// Método encargado de gestionar el TimeOut de una de las conexiones creadas y asociadas a una 
        /// operación mediante DataReaders.
        /// </summary>
        /// <param name="sender">Objeto que genera el evento.</param>
        private static void TratarDataReaderTimeOut(CADbDataConnection sender)
        {
            string sMensaje = string.Empty;
            try
            {
                //  No se hace ningún tratamiento excepcional debido a que la gestión del mutex  ya  se  lleva a
                //  cabo en el método FinalizeConnection. Por la misma razón no se envia en este punto el evento
                //  asociado a la finalización de una conexión.
                FinalizeConnection(ref sender, out sMensaje, true);
            }
            catch (Exception ex)
            {
                //  En este caso se notifica el error mediante un evento de finalización de conexión en el que se
                //  muestra que se ha producido un error.
                if (evFinalizacionConexion != null)
                    evFinalizacionConexion(sender.DbKeyApplication, sender.Identifier, ResultOpBD.Error, MsgManager.ExcepMsg(ex));
            }
        }

        #endregion

        #endregion

        #region Operaciones Consultoras sobre el Estado de la clase y de las Conexiones

        #region Obtiene un booleano que indica si la clase esta inicializada o no

        /// <summary>
        /// Método encargado de indicar si la clase esta inicializada o no.
        /// </summary>
        /// <returns>true, si la clase está inicializada, false, sinó.</returns>
        public static bool IsInitialized()
        {
            return (bInitialized);
        }

        #endregion

        #region Obtiene el tipo de una de las conexiones

        /// <summary>
        /// Método encargado de obtener el tipo de la conexión asociada a los datos pasados como parámetro.  
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la conexión a consultar.</param>
        /// <param name="eTypeConnection">Tipo de la conexión.</param>
        /// <param name="sMensaje">mensaje de error, si se produce uno.</param>
        /// <returns>Mirar el enumerado.</returns>
        public static ResultOpBD GetTypeConnection(DbKeyApp dbkaApplication, out DBType eTypeConnection, out string sMensaje)
        {
            eTypeConnection = DBType.UNDEFINED;
            sMensaje = string.Empty;
            try
            {
                ResultOpBD eResult;
                if ((eResult = ValidateConnectionManager(dbkaApplication, out sMensaje)) != ResultOpBD.Correct) return (eResult);
                eTypeConnection = ((IDbDataConnectionEventArgs)htKeys[dbkaApplication]).DataBaseType;
                return (ResultOpBD.Correct);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (ResultOpBD.Error);
            }
        }

        #endregion

        #region Obtiene la Cadena de Conexión de una de las conexiones

        /// <summary>
        /// Método encargado de obtener la cadena de conexión de la conexión asociada a los datos pasados como 
        /// parámetro.  
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la conexión a consultar.</param>
        /// <param name="sStringConnection">Cadena de Conexión de la conexión.</param>
        /// <param name="sMensaje">mensaje de error, si se produce uno.</param>
        /// <returns>Mirar el enumerado.</returns>
        public static ResultOpBD GetStringConnection(DbKeyApp dbkaApplication, out string sStringConnection, out string sMensaje)
        {
            sStringConnection = null;
            sMensaje = string.Empty;
            try
            {
                ResultOpBD eResult;
                if ((eResult = ValidateConnectionManager(dbkaApplication, out sMensaje)) != ResultOpBD.Correct) return (eResult);
                if ((htKeys == null) || (dbkaApplication == null) || (!htKeys.ContainsKey(dbkaApplication)))
                {
                    sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_011");
                    return (ResultOpBD.DataInputError);
                }
                sStringConnection = ((IDbDataConnectionEventArgs)htKeys[dbkaApplication]).ConnectionString;
                return (ResultOpBD.Correct);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (ResultOpBD.Error);
            }
        }

        #endregion

        #endregion

        #region Obtención del Prefijo de una tabla en función del tipo de Base de Datos.

        /// <summary>
        /// Formatea el prefijo del nombre de la tabla para que se pueda usar en las consultas de la Base de Datos.
        /// </summary>
        /// <param name="eDBType">Tipo para el que se quiere obtener la información</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>null, si error, valor del campo si todo correcto</returns>
        public static string ObtenerPrefijoTabla(DBType eDBType, out string sMensaje)
        {
            sMensaje = string.Empty;
            try
            {
                switch (eDBType)
                {
                    case DBType.PROGRESS:
                         return ("pub.");
                    case DBType.SQLSERVER:
                         return (string.Empty);
                    case DBType.ORACLE:
                         return (string.Empty);
                    case DBType.UNDEFINED:
                    default:
                         return (null);
                }
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        #endregion

        #region Test de conexión a Base de datos 
        
        public static ResultOpBD TestConnection(DbKeyApp dbkaApplication, out string sMensaje)
        {
            int iNumMaxConnections= 1;
            ResultOpBD eResult= ResultOpBD.Correct;
            bool bResult = false;

            DbDataConnectionSQLServerEventArgs eventArgs = new DbDataConnectionSQLServerEventArgs(iNumMaxConnections);
            DbDataConnectionSQLServer oDbDataConnection = new DbDataConnectionSQLServer(dbkaApplication, eventArgs, 
                                                                                        out bResult, out sMensaje);
            if (bResult == false) return ResultOpBD.Error;
            eResult = oDbDataConnection.Initialize(out sMensaje);
            if (eResult != ResultOpBD.Correct) return eResult;
            eResult=  oDbDataConnection.Finalize(out sMensaje);
            return eResult;
        }

        #endregion 

        #region Auxiliares

        #region Obtiene una conexión que cumple con las condiciones requeridas

        /// <summary>
        /// Método encargado de obtener una conexión para realizar una consulta. La conexión puede ser nueva
        /// o una de las conexiones existentes que este desocupada.  
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada al cliente para el que se realiza la operación</param>
        /// <param name="oDbDataConnection">Datos de la conexión con la que se desea trabajar.</param>
        /// <param name="sMensaje">mensaje de error, si se produce uno.</param>
        /// <returns>Mirar el enumerado.</returns>
        private static ResultOpBD GetConnection(DbKeyApp dbkaApplication, out CADbDataConnection oDbDataConnection, out string sMensaje)
        {
            bool bSendEvent = true;
            sMensaje = string.Empty;
            oDbDataConnection = null;
            Guid? guidConnection = null;
            bool bMutexActivated = false;
            ConnectionState? csOutput = null;
            ResultOpBD eResult = ResultOpBD.Error;
            try
            {
                //  En primer lugar comprueba que no se haya sobrepasado el número máximo de conexiones para esta clave. 
                    IDbDataConnectionEventArgs eaDC = (IDbDataConnectionEventArgs)htKeys[dbkaApplication];
                    switch (eaDC.IsExceededMaxinumNumberOfConnections(dbkaApplication, out oDbDataConnection, ref mtxManageQueue, out sMensaje))
                    {
                        case ResultExceededConnections.MaxiumNumberOfConnectionsExceeded: // Usa la que queda libre.
                             oDbDataConnection.StopTimeOut();
                             bSendEvent = false;
                             return (eResult = ResultOpBD.Correct);
                        case ResultExceededConnections.NotExceeded: // Sigue: busca una de las activas o crea una nueva.
                             break;
                        case ResultExceededConnections.InternalError:
                        case ResultExceededConnections.Error:
                        default:  // Si es imposible determinar si se ha superado el límite máximo se da un error.
                             bSendEvent = false;
                             return (ResultOpBD.Error);
                    }
                //  Activa el Objeto de Exclusión Mútua.
                    bMutexActivated = mtxRefreshConnections.WaitOne(); //(TIMEOUT_GET_CONNECTION, false);
                //  En segundo lugar se procede a buscar en la tabla de Hash una conexión valida y a marcarla como una
                //  conexión en uso.
                    string sMessageFree;
                    if (FindFreeConnectionForGetConnection(dbkaApplication, out oDbDataConnection, out sMessageFree) == ResultExist.Exist)
                    {
                        sMensaje += sMessageFree;
                        oDbDataConnection.StopTimeOut();
                        bSendEvent = false;
                        return (eResult = ResultOpBD.Correct);
                    }
                //  Obtiene un identificador válido.
                    string sMessageGetGuidConn;
                    if ((eResult = ObtenerIdentificadorConexion(out guidConnection, out sMessageGetGuidConn)) !=
                         ResultOpBD.Correct)
                    {
                        sMensaje += sMessageGetGuidConn;
                        return (eResult);
                    }
                //  Gestiona la petición de una conexión en función de su tipo.
                    string sMessageCreate, sMessageInitialize;
                    switch (eaDC.DataBaseType)
                    {
                        case DBType.SQLSERVER:                               
                             // Crea una nueva conexión del tipo SQL Server.
                                DbDataConnectionSQLServer oDbDataSQLServerConnection;
                                if ((oDbDataSQLServerConnection =
                                       DbConnectionSQLServer.CreateNewConnection
                                              (dbkaApplication, guidConnection, (DbDataConnectionSQLServerEventArgs)eaDC,
                                               out sMessageCreate)) == null)
                                {
                                    sMensaje += sMessageCreate;
                                    return (eResult = ResultOpBD.Error);
                                }
                             // Inicializa la conexión creada.
                                if ((eResult = oDbDataSQLServerConnection.Initialize(out sMessageInitialize)) !=
                                     ResultOpBD.Correct)
                                {
                                    sMensaje += sMessageInitialize;
                                    return (eResult);
                                }
                                oDbDataSQLServerConnection.evEstadoConexionCambiado += new CADbDataConnection.dlgDbConnectionStateChanged(TratarCambioEstadoConexion);
                                oDbDataSQLServerConnection.evTimeOut += new CADbDataConnection.dlgTimeOut(TratarTimeOut);
                                oDbDataSQLServerConnection.evTransactionTimeOut += new CADbDataConnection.dlgTimeOut(TratarTransactionTimeOut);
                                oDbDataSQLServerConnection.evDataReaderTimeOut += new CADbDataConnection.dlgTimeOut(TratarDataReaderTimeOut);
                                csOutput = oDbDataSQLServerConnection.EstadoConexion;
                             // Inserta la nueva conexión en la tabla de Hash de conexiones.
                                htConnectionsInUse.Add(oDbDataSQLServerConnection.Identifier, oDbDataSQLServerConnection);
                                (oDbDataConnection = oDbDataSQLServerConnection).StopTimeOut();
                             // Devolver operación correcta.
                                return (eResult = ResultOpBD.Correct);
                        case DBType.PROGRESS:
                             // Crea una nueva conexión del tipo PROGRESS
                                DbDataConnectionPROGRESS oDbDataPROGRESSConnection;
                                if ((oDbDataPROGRESSConnection =
                                       DbConnectionPROGRESS.CreateNewConnection
                                              (dbkaApplication, guidConnection, (DbDataConnectionPROGRESSEventArgs)eaDC,
                                               out sMessageCreate)) == null)
                                {
                                    sMensaje += sMessageCreate;
                                    return (eResult = ResultOpBD.Error);
                                }
                             // Inicializa la conexión creada.
                                if ((eResult = oDbDataPROGRESSConnection.Initialize(out sMessageInitialize)) !=
                                     ResultOpBD.Correct)
                                {
                                    sMensaje += sMessageInitialize;
                                    return (eResult);
                                }
                                oDbDataPROGRESSConnection.evEstadoConexionCambiado += new CADbDataConnection.dlgDbConnectionStateChanged(TratarCambioEstadoConexion);
                                oDbDataPROGRESSConnection.evTimeOut += new CADbDataConnection.dlgTimeOut(TratarTimeOut);
                                oDbDataPROGRESSConnection.evTransactionTimeOut += new CADbDataConnection.dlgTimeOut(TratarTransactionTimeOut);
                                oDbDataPROGRESSConnection.evDataReaderTimeOut += new CADbDataConnection.dlgTimeOut(TratarDataReaderTimeOut);
                                csOutput = oDbDataPROGRESSConnection.EstadoConexion;
                             // Inserta la nueva conexión en la tabla de Hash de conexiones.
                                htConnectionsInUse.Add(oDbDataPROGRESSConnection.Identifier, oDbDataPROGRESSConnection);
                                (oDbDataConnection = oDbDataPROGRESSConnection).StopTimeOut();
                             // Devolver operación correcta.
                                return (eResult = ResultOpBD.Correct);
                        case DBType.ORACLE:
                             // Crea una nueva conexión del tipo ORACLE.
                                DbDataConnectionORACLE oDbDataORACLEConnection;
                                if ((oDbDataORACLEConnection =
                                       DbConnectionORACLE.CreateNewConnection
                                              (dbkaApplication, guidConnection, (DbDataConnectionORACLEEventArgs)eaDC,
                                               out sMessageCreate)) == null)
                                {
                                    sMensaje += sMessageCreate;
                                    return (eResult = ResultOpBD.Error);
                                }
                             // Inicializa la conexión creada.
                                if ((eResult = oDbDataORACLEConnection.Initialize(out sMessageInitialize)) != ResultOpBD.Correct)
                                {
                                    sMensaje += sMessageInitialize;
                                    return (eResult);
                                }
                                oDbDataORACLEConnection.evEstadoConexionCambiado += new CADbDataConnection.dlgDbConnectionStateChanged(TratarCambioEstadoConexion);
                                oDbDataORACLEConnection.evTimeOut += new CADbDataConnection.dlgTimeOut(TratarTimeOut);
                                oDbDataORACLEConnection.evTransactionTimeOut += new CADbDataConnection.dlgTimeOut(TratarTransactionTimeOut);
                                oDbDataORACLEConnection.evDataReaderTimeOut += new CADbDataConnection.dlgTimeOut(TratarDataReaderTimeOut);
                                csOutput = oDbDataORACLEConnection.EstadoConexion;
                             // Inserta la nueva conexión en la tabla de Hash de conexiones.
                                htConnectionsInUse.Add(oDbDataORACLEConnection.Identifier, oDbDataORACLEConnection);
                                (oDbDataConnection = oDbDataORACLEConnection).StopTimeOut();
                             // Devolver operación correcta.
                                return (eResult = ResultOpBD.Correct);
                        case DBType.UNDEFINED:
                        default:
                                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_012");
                                return (eResult = ResultOpBD.DataInputError);
                    }
            }
            catch (AbandonedMutexException ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_010", ex);
                return (ResultOpBD.InternalError);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (eResult = ResultOpBD.Error);
            }
            finally
            {
                if (bMutexActivated) mtxRefreshConnections.ReleaseMutex();
                if ((evInicioConexion != null) && (bSendEvent)) evInicioConexion(dbkaApplication, guidConnection, eResult, csOutput, sMensaje);
            }
        }

        #region Obtener Identificadores de Conexión

        /// <summary>
        /// Método que se encarga de obtener un identificador válido para el nuevo documento.
        /// </summary>
        /// <param name="guidConnection">Identificador de la nueva conexión.</param>
        /// <param name="sMensaje">Mensaje de error, si se ha producido uno.</param>
        /// <returns>Mirar la descripción del enumerado.</returns>
        private static ResultOpBD ObtenerIdentificadorConexion(out Guid? guidConnection, out string sMensaje)
        {
            guidConnection = null;
            sMensaje = string.Empty;
            try
            {
                int iNumIntentos = 0;
                do
                {
                    guidConnection = Guid.NewGuid();
                    iNumIntentos++;
                } while ((htFreeConnections.ContainsKey(guidConnection)) && (htConnectionsInUse.ContainsKey(guidConnection)) &&
                         (iNumIntentos < NUM_INTENTS_GET_GUID));
                if (iNumIntentos == NUM_INTENTS_GET_GUID)
                {
                    sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_013");
                    return (ResultOpBD.InternalError);
                }
                else return (ResultOpBD.Correct);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (ResultOpBD.Error);
            }
        }

        #endregion

        #region Buscar Una Conexión Libre

        /// <summary>
        /// Método que procede a buscar en la tabla de Hash una conexión valida y a marcarla como una conexión en uso.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada al cliente para el que se realiza la operación</param>
        /// <param name="oDbDataConnection">Datos de la conexión encontrada.</param>
        /// <param name="sMensaje">mensaje de error, si se produce uno.</param>
        /// <returns>Mirar el enumerado.</returns>
        private static ResultExist FindFreeConnectionForGetConnection(DbKeyApp dbkaApplication, out CADbDataConnection oDbDataConnection, out string sMensaje)
        {            
            sMensaje = string.Empty;
            oDbDataConnection = null;
            try
            {
                //  Primero Obtiene los datos necesarios para realizar la deteccíón de conexiones libres.
                    IDbDataConnectionEventArgs eaDC = (IDbDataConnectionEventArgs)htKeys[dbkaApplication];
                    foreach (CADbDataConnection oDbDataConnectionTemp in htFreeConnections.Values)
                    {
                        if (oDbDataConnectionTemp.TipoBaseDatos == eaDC.DataBaseType)
                        {
                            switch (eaDC.DataBaseType)
                            {
                                case DBType.SQLSERVER:
                                     if ((oDbDataConnectionTemp.ConnectionString == eaDC.ConnectionString) &&
                                         (oDbDataConnectionTemp.EstadoConexion != null) &&
                                         (oDbDataConnectionTemp.EstadoConexion == ConnectionState.Open) &&
                                         (oDbDataConnectionTemp.DbKeyApplication == dbkaApplication))
                                     {
                                         htFreeConnections.Remove(oDbDataConnectionTemp.Identifier);
                                         htConnectionsInUse.Add(oDbDataConnectionTemp.Identifier, oDbDataConnection = oDbDataConnectionTemp);
                                         return (ResultExist.Exist);
                                     }
                                     break;
                                case DBType.PROGRESS:
                                     if ((oDbDataConnectionTemp.ConnectionString == eaDC.ConnectionString) &&
                                         (oDbDataConnectionTemp.EstadoConexion != null) &&
                                         (oDbDataConnectionTemp.EstadoConexion == ConnectionState.Open) &&
                                         (oDbDataConnectionTemp.DbKeyApplication == dbkaApplication))
                                     {
                                         htFreeConnections.Remove(oDbDataConnectionTemp.Identifier);
                                         htConnectionsInUse.Add(oDbDataConnectionTemp.Identifier, oDbDataConnection = oDbDataConnectionTemp);
                                         return (ResultExist.Exist);
                                     }
                                     break;
                                case DBType.ORACLE:
                                     if ((oDbDataConnectionTemp.ConnectionString == eaDC.ConnectionString) &&
                                         (oDbDataConnectionTemp.EstadoConexion != null) &&
                                         (oDbDataConnectionTemp.EstadoConexion == ConnectionState.Open) &&
                                         (oDbDataConnectionTemp.DbKeyApplication == dbkaApplication))
                                     {
                                         htFreeConnections.Remove(oDbDataConnectionTemp.Identifier);
                                         htConnectionsInUse.Add(oDbDataConnectionTemp.Identifier, oDbDataConnection = oDbDataConnectionTemp);
                                         return (ResultExist.Exist);
                                     }
                                     break;
                            }
                        }
                    }
                    return (ResultExist.NotExist);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (ResultExist.Error);
            }
        }

        #endregion

        #endregion

        #region Reasignar Conexión

        /// <summary>
        /// Método encargado de reasignar una conexión que queda libre en caso que haya consultas pendientes de
        /// ser realizadas.  
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada al cliente al que pertenece la conexióna reasignar.</param>
        /// <param name="guidConnection">Identificador de la conexión a manipular.</param>
        /// <param name="oDbDataConnection">Datos de la conexión con la que se desea trabajar.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>Mirar el enumerado.</returns>
        private static ResultOpBD AssignConnection(DbKeyApp dbkaApplication, Guid? guidConnection, 
                                                   ref CADbDataConnection oDbDataConnection, 
                                                   out StateChangeEventArgs sceaState, out string sMensaje)
        {
            sMensaje = string.Empty;
            bool bMutexActivated = false;
            ResultOpBD eResult = ResultOpBD.Correct;
            try
            {
                //  Elimina la conexión de las conexiones en uso. En caso de error lo ignora.
                    bMutexActivated = mtxRefreshConnections.WaitOne(); //(TIMEOUT_ASSIGNATION, false);
                    if (htConnectionsInUse.ContainsKey(guidConnection))
                    {
                        IDbDataConnectionEventArgs eaDC = (IDbDataConnectionEventArgs)htKeys[dbkaApplication];
                        switch (eaDC.AsignConnection(dbkaApplication, oDbDataConnection, ref mtxManageQueue, out sMensaje))
                        {
                            case ResultConnectionAsgined.QueueEmpty:
                                 htConnectionsInUse.Remove(guidConnection);
                                 htFreeConnections.Add(guidConnection, oDbDataConnection);
                                 oDbDataConnection.StartTimeOut();
                                 sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Open);
                                 break;
                            case ResultConnectionAsgined.QueueConnectionAsigned:
                                 sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Fetching);
                                 break;
                            case ResultConnectionAsgined.InternalError:
                            case ResultConnectionAsgined.Error:
                            default:
                                 htConnectionsInUse.Remove(guidConnection);
                                 string sMessageFinalize;
                                 if (FinalizeConnection(ref oDbDataConnection, out sMessageFinalize, false) == ResultOpBD.Correct)
                                 {
                                     sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Closed);
                                     return (ResultOpBD.Correct);
                                 }
                                 else
                                 {
                                     sMensaje += sMessageFinalize;
                                     sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Broken);
                                     return (ResultOpBD.InternalError);
                                 }
                        }
                    }
                    else sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Open);
                //  Devuelve el resultado de llevar a cabo la operación deseada.
                    return (eResult);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_014", ex);
                sceaState = new StateChangeEventArgs(ConnectionState.Executing, ConnectionState.Open);
                return (eResult = ResultOpBD.Error);
            }
            finally
            {
                if (bMutexActivated) mtxRefreshConnections.ReleaseMutex();
            }
        }

        #endregion

        #region Finalizar Conexión

        /// <summary>
        /// Método encargado de finalizar una conexión que se sabe que existe o existía en el momento de invocar a
        /// esta operación.  
        /// </summary>
        /// <param name="oDbDataConnection">Conexión que se quiere finalizar.</param>
        /// <param name="sMensaje">mensaje de error, si se produce uno.</param>
        /// <param name="bActivateMutex">true, se ha de activar el controlador de exclusión mútua, false, no</param>
        /// <returns>Mirar el enumerado.</returns>
        private static ResultOpBD FinalizeConnection(ref CADbDataConnection oDbDataConnection, out string sMensaje, bool bActiveMutex)
        {
            DbKeyApp dbkaApp = null;
            sMensaje = string.Empty;
            Guid? guidConnection = null;
            bool bMutexActivated = false;
            ResultOpBD eResult = ResultOpBD.Correct;
            try
            {
                if (bActiveMutex) bMutexActivated = mtxRefreshConnections.WaitOne(); //(TIMEOUT_DISCONNECTION, false);
                oDbDataConnection.StopTimeOut();
                oDbDataConnection.StopTransactionTimeOut();
                dbkaApp = oDbDataConnection.DbKeyApplication;
                guidConnection = oDbDataConnection.Identifier;
                if (htTransactionsInUse.ContainsKey(oDbDataConnection.TransactionId)) htTransactionsInUse.Remove(oDbDataConnection.TransactionId);
                if (htConnectionsInUse.ContainsKey(oDbDataConnection.Identifier)) htConnectionsInUse.Remove(oDbDataConnection.Identifier);
                if (htFreeConnections.ContainsKey(oDbDataConnection.Identifier)) htFreeConnections.Remove(oDbDataConnection.Identifier);
                return (eResult = oDbDataConnection.Finalize(out sMensaje));
            }
            catch (AbandonedMutexException ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_015", ex);
                return (eResult = ResultOpBD.InternalError);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_016", ex);
                return (eResult = ResultOpBD.Error);
            }
            finally
            {
                if ((bMutexActivated) && (bActiveMutex)) mtxRefreshConnections.ReleaseMutex();
                if (evFinalizacionConexion != null) evFinalizacionConexion(dbkaApp, guidConnection, eResult, sMensaje);
            }
        }

        #endregion

        #region Validación del Estado de la Clase

        /// <summary>
        /// Método encargado de realizar las validaciones basicas sobre el estado del Connection Manager.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la operación a realizar.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>Mirar Enumerado.</returns>
        private static ResultOpBD ValidateConnectionManager(DbKeyApp dbkaApplication, out string sMensaje)
        {
            sMensaje = string.Empty;
            bool bMutexActivated = false;
            try
            {
                //  Activa el controlador de exclusión mútua.
                    bMutexActivated = mtxRefreshConnections.WaitOne(); //(TIMEOUT_VALIDATION, false);
                //  Valida que el Connection Manager esté inicializado.
                    if (!bInitialized)
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_017");
                        return (ResultOpBD.InternalError);
                    }
                //  Comrprueba que la clave indicada exista en el sistema.
                    if ((htKeys == null) || (dbkaApplication == null) || (dbkaApplication.ApplicationID == null) ||
                        (dbkaApplication.ApplicationID == null) || (!htKeys.ContainsKey(dbkaApplication)))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_018");
                        return (ResultOpBD.DataInputError);
                    }
                //  Finalmente indica que todo está correcto.
                    return (ResultOpBD.Correct);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_ConnectionManager_019", ex);
                return (ResultOpBD.Error);
            }
            finally
            {
                if (bMutexActivated) mtxRefreshConnections.ReleaseMutex();
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
