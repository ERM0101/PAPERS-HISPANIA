#region Librerias usadas por la clase

using MBCode.Framework.Managers.Messages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

#endregion

namespace MBCode.Framework.DataBase
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha última modificación: 28/02/2012.
    /// Descripción: clase que almacena la información relativa a una conexión activa con una base de Datos SQL Server.
    /// </summary>
    internal class DbDataConnectionSQLServer : CADbDataConnection
    {
        #region Constantes

        /// <summary>
        /// Define el tiempo, expresado en milisegundos que debe transcurrir hasta que se cierre la conexión por
        /// falta de actividad.
        /// </summary>
        private const int INTERVAL_TIMEOUT = 3000;

        /// <summary>
        /// Define el tiempo, expresado en milisegundos que debe transcurrir hasta que se cierre la transaction
        /// por falta de actividad.
        /// </summary>
        private const int INTERVAL_TRANSACTION_TIMEOUT = 600000;

        #endregion

        #region Eventos

        /// <summary>
        /// Evento que se produce cuando cambia el estado de la conexión.
        /// </summary>
        override public event dlgDbConnectionStateChanged evEstadoConexionCambiado;

        #endregion

        #region Atributos

        #region Datos Básicos

        /// <summary>
        /// Almacena el nombre del Servidor que contiene la Base de Datos con la que se debe conectar y 
        /// la instáncia de la Base de Datos que se usará.
        /// </summary>
        private string sDataSource;

        /// <summary>
        /// Almacena el nombre de la Base de Datos a la que se debe conectar.
        /// </summary>
        private string sInitialCatalog;

        /// <summary>
        /// Almacena el nombre de la Base de Datos a la que se debe conectar en caso de fallo de la primera opción.
        /// </summary>
        private string sInitialCatalogMirroring;

        #endregion

        #region DatosConexion

        /// <summary>
        /// Command definido al realizar la inicialización y usado por todo el programa.
        /// </summary>
        private SqlCommand oSqlCommand;

        /// <summary>
        /// Conexión definida al realizar la inicialización usada por todo el programa.
        /// </summary>
        private SqlConnection oSqlConnection;

        /// <summary>
        /// DataReader con el que se realiza una consulta por DataReaders.
        /// </summary>
        private SqlDataReader oSqlDataReader;

        #endregion

        #region Seguridad

        /// <summary>
        /// Almacena el tipo de autentificación que usara
        /// </summary>
        private AuthenticationType eTipoAutenticacion;

        /// <summary>
        /// Almacena el nombre de usuario con el que la conexión puede acceder a la Base de Datos.
        /// </summary>
        private string sUser;

        /// <summary>
        /// Almacena la contraseña asociada al nombre de usuario con el que la conexión puede acceder a 
        /// la Base de Datos.
        /// </summary>
        private string sPassword;

        #endregion

        #endregion

        #region Propiedades

        #region Datos Básicos

        /// <summary>
        /// Obtiene el nombre del Servidor que contiene la Base de Datos con la que se debe conectar y 
        /// la instáncia de la Base de Datos que se usará.
        /// </summary>
        public string DataSource
        {
            get
            {
                return (this.sDataSource);
            }
        }

        /// <summary>
        /// Obtiene el nombre de la Base de Datos a la que se debe conectar.
        /// </summary>
        public string InitialCatalog
        {
            get
            {
                return (this.sInitialCatalog);
            }
        }

        #endregion

        #region Datos de la Conexión

        /// <summary>
        /// Obtiene el estado de la conexión.
        /// </summary>
        override public ConnectionState? EstadoConexion
        {
            get
            {
                if (this.oSqlConnection != null) return (this.oSqlConnection.State);
                else return (null);
            }
        }

        #endregion

        #region Seguridad

        /// <summary>
        /// Obtiene el tipo de autentificación que usara
        /// </summary>
        public AuthenticationType TipoAutenticacion
        {
            get
            {
                return (this.eTipoAutenticacion);
            }
        }

        /// <summary>
        /// Obtiene el nombre de usuario con el que la conexión puede acceder a la Base de Datos.
        /// </summary>
        public string User
        {
            get
            {
                return (this.sUser);
            }
        }

        /// <summary>
        /// Obtiene la contraseña asociada al nombre de usuario con el que la conexión puede acceder  a 
        /// la Base de Datos.
        /// </summary>
        public string Password
        {
            get
            {
                return (this.sPassword);
            }
        }

        #endregion

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor de la clase definido con los parámetros necesarios para almacenar la información relativa
        /// a la conexión que representa.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación para la que se crea la conexión.</param>
        /// <param name="oParams">Parámetros con los que se debe inicializar la conexión.</param>
        /// <param name="bResult">true, operación de inicialización correcta, false, error.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        public DbDataConnectionSQLServer(DbKeyApp dbkaApplication, DbDataConnectionSQLServerEventArgs oParams, out bool bResult, out string sMensaje)
            : base(dbkaApplication, DBType.SQLSERVER, oParams.Identifier, oParams.ConnectionString,
                   INTERVAL_TIMEOUT, INTERVAL_TRANSACTION_TIMEOUT)
        {
            try
            {
                //  Inicializa los atributos de la clase en función de los valores pasados como parámetro.
                    this.eTipoAutenticacion = oParams.TipoAutenticacion;
                    this.oSqlCommand = oParams.Command;
                    this.oSqlConnection = oParams.Connection;
                    this.oSqlDataReader = null;
                    this.sDataSource = oParams.DataSource;
                    this.sInitialCatalog = oParams.InitialCatalog;
                    this.sInitialCatalogMirroring = oParams.InitialCatalogMirroring;
                    this.sPassword = oParams.Password;
                    this.sUser = oParams.User;
                //  Da por valida la operación.
                    this.eEstado = CADbDataConnection.State.DataCharged;
                    sMensaje = string.Empty;
                    bResult = true;
            }
            catch (Exception ex)
            {
                //  Asigna el mensaje de error e indica que se ha producido uno.
                    sMensaje = MsgManager.ExcepMsg(ex);
                    bResult = false;
            }
        }

        #endregion

        #region Métodos

        #region Relacionados con la Conexión

        #region Inicialización de la conexión

        /// <summary>
        /// Método encargado de inicializar los atributos mediante los que se realiza el acceso a los datos de una
        /// Base de Datos.
        /// </summary>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar la descripción del enumerado</returns>
        override internal ResultOpBD Initialize(out string sMensaje)
        {
            sMensaje = string.Empty;
            try
            {
                //  Comprueba que el estado de la clase sea el necesario.
                    if (eEstado != State.DataCharged)
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_000");
                        return (ResultOpBD.DataInputError);
                    }
                //  Define la conexión y el command a usar por defecto y finaliza, si así se desea.
                    if ((oSqlConnection = GetConnection(out sMensaje)) == null) return (ResultOpBD.InternalError);
                    if (oSqlConnection.State != ConnectionState.Open) oSqlConnection.Open();
                    if ((oSqlCommand = GetCommand(out sMensaje)) == null)
                    {
                        oSqlConnection.Close();
                        oSqlConnection = null;
                        return (ResultOpBD.InternalError);
                    }
                    oSqlCommand.Connection = oSqlConnection;
                    oSqlCommand.CommandTimeout = DbConstants.TIMEOUT_COMMAND_SQL_SERVER;
                    oSqlCommand.Connection.StateChange += new StateChangeEventHandler(DbDataConnectionSQLServer_StateChange);
                    eEstado = State.Opened;
                    return (ResultOpBD.Correct);
            }
            catch (SqlException XcpSQL)
            {
                StringBuilder sbError = new StringBuilder(string.Empty);
                foreach (SqlError se in XcpSQL.Errors)
                {
                    switch (se.Number)
                    {
                        case DbConstants.SERVER_ERROR:
                            sbError = sbError.AppendLine(MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_001"));
                            break;
                        case DbConstants.DATABASE_ERROR:
                            sbError = sbError.AppendLine(MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_002"));
                            break;
                        case DbConstants.DATAACCESS_ERROR:
                            sbError = sbError.AppendLine(MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_003"));
                            break;
                        default:
                            sbError = sbError.AppendLine(MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_004", se.Message));
                            break;
                    }
                }
                sMensaje += sbError.ToString();
                return (ResultOpBD.Error);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (ResultOpBD.Error);
            }
        }

        #endregion

        #region Finalización de la conexión

        /// <summary>
        /// Método que se ejecuta al eliminar una instáncia de está clase.
        /// </summary>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>Mirar la descripción del enumerado.</returns>
        override internal ResultOpBD Finalize(out string sMensaje)
        {
            sMensaje = string.Empty;
            try
            {
                if (oSqlConnection == null) return (ResultOpBD.Correct);
                if ((oSqlCommand != null) && (oSqlCommand.Transaction != null))
                {
                    RollbackTransaction(out sMensaje);
                    sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_005", sMensaje);
                }
                switch (eEstado)
                {
                    case State.DataCharged:
                         oSqlConnection.StateChange -= new StateChangeEventHandler(DbDataConnectionSQLServer_StateChange);
                         if (oSqlConnection.State == ConnectionState.Open) oSqlConnection.Close();
                         if (oSqlConnection != null) oSqlConnection.Dispose();
                         oSqlConnection = null;
                         if (oSqlCommand != null) oSqlCommand.Dispose();
                         oSqlCommand = null;
                         eEstado = State.DataCharged;
                         return (ResultOpBD.Correct);
                    case State.Opened:
                         switch (oSqlConnection.State)
                         {
                             case ConnectionState.Open:
                                  oSqlConnection.StateChange -= new StateChangeEventHandler(DbDataConnectionSQLServer_StateChange);
                                  oSqlConnection.Close();
                                  if (oSqlConnection != null) oSqlConnection.Dispose();
                                  oSqlConnection = null;
                                  if (oSqlCommand != null) oSqlCommand.Dispose();
                                  oSqlCommand = null;
                                  return (ResultOpBD.Correct);
                             case ConnectionState.Broken:
                             case ConnectionState.Closed:
                                  oSqlConnection.StateChange -= new StateChangeEventHandler(DbDataConnectionSQLServer_StateChange);
                                  if (oSqlConnection != null) oSqlConnection.Dispose();
                                  this.oSqlConnection = null;
                                  if (oSqlCommand != null) oSqlCommand.Dispose();
                                  this.oSqlCommand = null;
                                  return (ResultOpBD.Correct);
                             default: // Debe intentar cerrar la conexión otra vez, llamando a este método.
                                  sMensaje += MsgManager.WarningMsg("MSG_CADbDataConnection(DataBase)_008");
                                  return (ResultOpBD.Warning);
                         }
                    case State.Initialized:
                         return (ResultOpBD.Correct);
                    case State.Undefined:
                    default:
                         sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_007");
                         return (ResultOpBD.InternalError);
                }
            }
            catch (SqlException XcpSQL)
            {
                StringBuilder sbError = new StringBuilder(string.Empty);
                foreach (SqlError se in XcpSQL.Errors)
                {
                    switch (se.Number)
                    {
                        case DbConstants.SERVER_ERROR:
                             sbError = sbError.AppendLine(MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_001"));
                             break;
                        case DbConstants.DATABASE_ERROR:
                             sbError = sbError.AppendLine(MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_002"));
                             break;
                        case DbConstants.DATAACCESS_ERROR:
                             sbError = sbError.AppendLine(MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_003"));
                             break;
                        default:
                             sbError = sbError.AppendLine(MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_006", se.Message));
                             break;
                    }
                }
                sMensaje += sbError.ToString();
                return (ResultOpBD.Error);
            }
            catch (Exception ex)
            {
                sMensaje += MsgManager.ExcepMsg(ex);
                return (ResultOpBD.Error);
            }
        }

        #endregion

        #region Eventos relacionados con la conexión

        /// <summary>
        /// Método encargado de propagar el evento de cambio de estado de una conexión.
        /// </summary>
        /// <param name="sender">Objeto que lanza el evento </param>
        /// <param name="e">Parámetros con los que se lanza el evento</param>
        private void DbDataConnectionSQLServer_StateChange(object sender, StateChangeEventArgs e)
        {
            if (this.evEstadoConexionCambiado != null) this.evEstadoConexionCambiado(this.dbkaApplication, (Guid)this.guidIdentifier, e);
        }

        #endregion

        #endregion

        #region Relacionados con la ejecución de Consultas y Procedimientos Almacenados

        #region Ejecución de Consultas

        /// <summary>
        /// Realiza la consulta indicada por la cadena de carácteres pasada cómo parámetro y devuelve 
        /// un DataSet con el resultado.
        /// </summary>
        /// <param name="sql">Consulta a realizar</param>
        /// <param name="bWithSchema">Indica si se deben utilizar o no las restricciones</param>
        /// <param name="oDataSet">Datos obtenidos al realizar la consulta.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar la descripción del enumerado</returns>
        override internal ResultOpBD GetDataSetFromSql(string sql, bool bWithSchema, out DataSet oDataSet,
                                                       out string sMensaje)
        {
            oDataSet = null;
            sMensaje = string.Empty;
            try
            {
                //  Variables.
                    SqlDataAdapter oDataAdapterSql;
                //  Valida los parámetros de entrada.
                    if ((sql == null) || (sql == string.Empty))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_012");
                        return (ResultOpBD.DataInputError);
                    }
                    if ((eEstado != State.Opened) || (oSqlConnection == null) || (oSqlCommand == null) ||
                        (oSqlConnection.State != ConnectionState.Open))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_011");
                        return (ResultOpBD.InternalError);
                    }
                //  Inicia la realización de la consulta.
                    oSqlCommand.CommandText = sql;
                    oSqlCommand.CommandType = CommandType.Text;
                    oSqlCommand.Parameters.Clear();
                    if ((oDataAdapterSql = GetDataAdapter(oSqlCommand, out sMensaje)) != null)
                    {
                        if (bWithSchema) oDataAdapterSql.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                        oDataSet = new DataSet();
                        oDataAdapterSql.Fill(oDataSet);
                        oDataAdapterSql.Dispose();
                        oDataAdapterSql = null;
                        return (ResultOpBD.Correct);
                    }
                    else
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_010", sMensaje);
                        return (ResultOpBD.InternalError);
                    }
            }
            catch (SqlException ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_013", new string[] { ex.ErrorCode.ToString(), 
                                                ex.Number.ToString(), ex.LineNumber.ToString(), "ExecuteStoredProcedure", ex.Server,
                                                ex.TargetSite.Name, ex.Message });
                oDataSet = null;
                return (ResultOpBD.Error);
            }
            catch (Exception ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_009", ex);
                oDataSet = null;
                return (ResultOpBD.Error);
            }
        }

        #endregion

        #region Ejecución de Procedimientos Almacenados

        /// <summary>
        /// Ejecuta el Stored Procedure indicado por la cadena de carácteres pasada cómo parámetro y devuelve 
        /// un DataSet con el resultado.
        /// </summary>
        /// <param name="sNameStored">Nombre del procedimiento a ejecutar.</param>
        /// <param name="oDbParameter">Parámetros con los que se desea ejecutar el procedimiento.</param>
        /// <param name="oDataSet">Datos leídos</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>DataSet, si operación correcta, null en caso de error.</returns>
        override internal ResultOpBD ExecuteStoredProcedure(string sNameStored, DbParameter[] oDbParameter,
                                                            bool bWithSchema, out DataSet oDataSet, out string sMensaje)
        {            
         
            oDataSet = null;
            sMensaje = string.Empty;
            try
            {
                //  Variables.
                    SqlDataAdapter oDataAdapterSql;

                //  Valida los parámetros de entrada.
                    if ((sNameStored == null) || (sNameStored == string.Empty))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_012");
                        return (ResultOpBD.DataInputError);
                    }
                    if ((eEstado != State.Opened) || (oSqlConnection == null) || (oSqlCommand == null) ||
                        (oSqlConnection.State != ConnectionState.Open))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_011");
                        return (ResultOpBD.InternalError);
                    }
                    
                //  Inicia la realización de la consulta.
                    oSqlCommand.CommandText = sNameStored;
                    oSqlCommand.CommandType = CommandType.StoredProcedure;                    
                    oSqlCommand.Parameters.Clear();
                    oSqlCommand.Parameters.AddRange((SqlParameter[])oDbParameter);
                
                    
                    if ((oDataAdapterSql = GetDataAdapter(this.oSqlCommand, out sMensaje)) != null)
                    {
                        if (bWithSchema) oDataAdapterSql.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                        oDataSet = new DataSet();                        
                        oDataAdapterSql.Fill(oDataSet);                        
                        oDataAdapterSql.Dispose();                        
                        oDataAdapterSql = null;
                        return (ResultOpBD.Correct);
                    }
                    else
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_010", sMensaje);
                        return (ResultOpBD.InternalError);
                    }
            }
            catch (SqlException ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_013", new string[] { ex.ErrorCode.ToString(), 
                                                ex.Number.ToString(), ex.LineNumber.ToString(), "ExecuteStoredProcedure", ex.Server,
                                                ex.TargetSite.Name, ex.Message });
                oDataSet = null;
                return (ResultOpBD.Error);
            }
            catch (Exception ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_009", ex);
                oDataSet = null;
                return (ResultOpBD.Error);
            }
        }

        /// <summary>
        /// Ejecuta el Stored Procedure indicado por la cadena de carácteres pasada cómo parámetro y devuelve 
        /// un DataSet con el resultado.
        /// </summary>
        /// <param name="sNameStored">Nombre del procedimiento a ejecutar.</param>
        /// <param name="oDbParameter">Parámetros con los que se desea ejecutar el procedimiento.</param>
        /// <param name="oDataSet">Datos leídos</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>DataSet, si operación correcta, null en caso de error.</returns>
        override internal ResultOpBD ExecuteStoredProcedure(string sNameStored, DbParameter[] oDbParameter,
                                                            bool bWithSchema, out List<object> listRespt, out string sMensaje)
        {
                        
            sMensaje = string.Empty;
            listRespt = new List<object>();
            try
            {

                //  Valida los parámetros de entrada.
                if ((sNameStored == null) || (sNameStored == string.Empty))
                {
                    sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_012");
                    return (ResultOpBD.DataInputError);
                }
                if ((eEstado != State.Opened) || (oSqlConnection == null) || (oSqlCommand == null) ||
                    (oSqlConnection.State != ConnectionState.Open))
                {
                    sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_011");
                    return (ResultOpBD.InternalError);
                }
                listRespt.Clear();
                //  Inicia la realización de la consulta.
                oSqlCommand.CommandText = sNameStored;
                oSqlCommand.CommandType = CommandType.StoredProcedure;
                oSqlCommand.Parameters.Clear();
                oSqlCommand.Parameters.AddRange((SqlParameter[])oDbParameter);

                if (oSqlCommand.Parameters.Contains("@mensaje"))
                {
                    listRespt.Add(oSqlCommand.ExecuteNonQuery());
                    listRespt.Add(oSqlCommand.Parameters["@retval"].Value);
                    listRespt.Add(oSqlCommand.Parameters["@mensaje"].Value);
                }
                else
                {
                    listRespt.Add(oSqlCommand.ExecuteNonQuery());
                    listRespt.Add(oSqlCommand.Parameters["@retval"].Value);
                }
                if ((int)listRespt[0]> 0)
                    return (ResultOpBD.Correct);

                else
                    return ResultOpBD.Error;  
              
            }
            catch (SqlException ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_013", new string[] { ex.ErrorCode.ToString(), 
                                                ex.Number.ToString(), ex.LineNumber.ToString(), "ExecuteStoredProcedure", ex.Server,
                                                ex.TargetSite.Name, ex.Message });
                listRespt[0] = 0;
                return (ResultOpBD.Error);
            }
            catch (Exception ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_009", ex);
                listRespt[0] = 0;
                return (ResultOpBD.Error);
            }
        }
        #endregion

        #region Obtención de un Escalar

        /// <summary>
        /// Realiza la consulta indicada por la cadena de carácteres pasada cómo parámetro y devuelve un valor escalar 
        /// como resultado de la ejecución de la misma.
        /// </summary>
        /// <param name="sql">Consulta a realizar</param>
        /// <param name="iEscalarResult">Resultado de la consulta.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Entero, si operación correcta, null en caso de error.</returns>
        override internal ResultOpBD GetEscalarFromSql(string sql, out int iEscalarResult, out string sMensaje)
        {
            iEscalarResult = -1;
            sMensaje = string.Empty;
            try
            {
                //  Valida los parámetros de entrada.
                    if ((sql == null) || (sql == string.Empty))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_012");
                        return (ResultOpBD.DataInputError);
                    }
                    if ((eEstado != State.Opened) || (oSqlConnection == null) || (oSqlCommand == null) ||
                        (oSqlConnection.State != ConnectionState.Open))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_011");
                        return (ResultOpBD.InternalError);
                    }
                //  Inicia la realización de la consulta.
                    oSqlCommand.CommandText = sql;
                    oSqlCommand.CommandType = CommandType.Text;
                    oSqlCommand.Parameters.Clear();
                    object oResult = oSqlCommand.ExecuteScalar();
                    if (oResult == DBNull.Value)
                    {
                        iEscalarResult = -2;
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_014");
                        return (ResultOpBD.Error);
                    }
                    else
                    {
                        iEscalarResult = Convert.ToInt32(oResult);
                        return (ResultOpBD.Correct);
                    }
            }
            catch (SqlException ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_013", new string[] { ex.ErrorCode.ToString(), 
                                                ex.Number.ToString(), ex.LineNumber.ToString(), "ExecuteStoredProcedure", ex.Server,
                                                ex.TargetSite.Name, ex.Message });
                return (ResultOpBD.Error);
            }
            catch (Exception ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_009", ex);
                return (ResultOpBD.Error);
            }
        }

        /// <summary>
        /// Realiza la consulta indicada por la cadena de carácteres pasada cómo parámetro y devuelve un valor escalar 
        /// como resultado de la ejecución de la misma.
        /// </summary>
        /// <param name="sql">Consulta a realizar</param>
        /// <param name="iEscalarResult">Resultado de la consulta.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Entero, si operación correcta, null en caso de error.</returns>
        override internal ResultOpBD GetEscalarFromWithParameter(Dictionary<string, List<object>> dicSql,out List<int> listEscalarResult, out string sMensaje)
        {
            listEscalarResult = new List<int>();
            sMensaje = string.Empty;
            try
            {
                //  Valida los parámetros de entrada.
                if (dicSql == null)
                {
                    sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_012");
                    return (ResultOpBD.DataInputError);
                }
                if ((eEstado != State.Opened) || (oSqlConnection == null) || (oSqlCommand == null) ||
                    (oSqlConnection.State != ConnectionState.Open))
                {
                    sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_011");
                    return (ResultOpBD.InternalError);
                }
                foreach (KeyValuePair<string, List<object>> kvp in dicSql)
                {
                    oSqlCommand.CommandText = kvp.Key;
                    oSqlCommand.CommandType = CommandType.Text;
                    oSqlCommand.Parameters.Clear();
                    if (kvp.Value != null)
                    {
                        foreach (object o in kvp.Value)
                        {
                            oSqlCommand.Parameters.Add((SqlParameter)o);
                        }
                    }
                    object oResult = oSqlCommand.ExecuteScalar();
                    if (oResult == DBNull.Value)
                        listEscalarResult.Add(-2);                    
                    else                    
                        listEscalarResult.Add(Convert.ToInt32(oResult));                        
                }
                    //  Devuelve el resultado indicado.
                    foreach (int respt in listEscalarResult)
                    {
                        if (respt == -2)
                        {
                            sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_014");
                            break;
                        }
                    }
                    if (string.IsNullOrEmpty(sMensaje))
                        return (ResultOpBD.Correct);
                    else
                        return (ResultOpBD.Error);                
            }
            catch (SqlException ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_013", new string[] { ex.ErrorCode.ToString(), 
                                                ex.Number.ToString(), ex.LineNumber.ToString(), "ExecuteStoredProcedure", ex.Server,
                                                ex.TargetSite.Name, ex.Message });
                return (ResultOpBD.Error);
            }
            catch (Exception ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_009", ex);
                return (ResultOpBD.Error);
            }
        }


        #endregion

        #region Operaciones que modifican el contenido de la Base de Datos

        /// <summary>
        /// Ejecuta la consulta de inserción, borrado o cambio, pasada cómo parámetro.
        /// </summary>
        /// <param name="sql">Consulta a realizar</param>
        /// <param name="iNumRowsAfected">Resultado de la consulta.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Entero, si operación correcta, null en caso de error.</returns>
        override internal ResultOpBD ExecuteCommandFromSql(string sql, out int iNumRowsAfected, out string sMensaje)
        {
            iNumRowsAfected = -1;
            sMensaje = string.Empty;
            try
            {
                //  Valida los parámetros de entrada.
                    if ((sql == null) || (sql == string.Empty))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_012");
                        return (ResultOpBD.DataInputError);
                    }
                    if ((eEstado != State.Opened) || (oSqlConnection == null) || (oSqlCommand == null) ||
                        (oSqlConnection.State != ConnectionState.Open))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_011");
                        return (ResultOpBD.InternalError);
                    }
                //  Inicia la realización de la consulta.
                    oSqlCommand.CommandText = sql;
                    oSqlCommand.CommandType = CommandType.Text;
                    oSqlCommand.Parameters.Clear();
                    iNumRowsAfected = oSqlCommand.ExecuteNonQuery(); // Convert.ToInt32();
                    //  Devuelve el resultado indicado.
                    if (iNumRowsAfected == 0)
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_016");
                        return (ResultOpBD.Error);
                    }
                    else return (ResultOpBD.Correct);
            }
            catch (SqlException ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_013", new string[] { ex.ErrorCode.ToString(), 
                                                ex.Number.ToString(), ex.LineNumber.ToString(), "ExecuteStoredProcedure", ex.Server,
                                                ex.TargetSite.Name, ex.Message });
                return (ResultOpBD.Error);
            }
            catch (Exception ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_009", ex);
                return (ResultOpBD.Error);
            }
        }

        /// <summary>
        /// Ejecuta más de una consulta de inserción, borrado o cambio al mismo tiempo usando la misma conexión.
        /// </summary>
        /// <param name="sql">Consulta a realizar</param>
        /// <param name="iNumRowsAfected">Resultado de la consulta.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Entero, si operación correcta, null en caso de error.</returns>
        override internal ResultOpBD ExecuteCommandFromSqlMARS(List<string> listSql, out List<int> listNumRowsAfected, out string sMensaje)
        {
            listNumRowsAfected = new List<int>();
            listNumRowsAfected.Clear();
            sMensaje = string.Empty;
            try
            {
                //  Valida los parámetros de entrada.
                if ((listSql == null) || (listSql.Count == 0))
                {
                    sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_012");
                    return (ResultOpBD.DataInputError);
                }
                if ((eEstado != State.Opened) || (oSqlConnection == null) || (oSqlCommand == null) ||
                    (oSqlConnection.State != ConnectionState.Open))
                {
                    sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_011");
                    return (ResultOpBD.InternalError);
                }
                //  Inicia la realización de la consulta.
                foreach (string sql in listSql)
                {
                    oSqlCommand.CommandText = sql;
                    oSqlCommand.CommandType = CommandType.Text;
                    oSqlCommand.Parameters.Clear();
                    listNumRowsAfected.Add(oSqlCommand.ExecuteNonQuery());
                }
                return (ResultOpBD.Correct);
            }
            catch (SqlException ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_013", new string[] { ex.ErrorCode.ToString(), 
                                                ex.Number.ToString(), ex.LineNumber.ToString(), "ExecuteStoredProcedure", ex.Server,
                                                ex.TargetSite.Name, ex.Message });
                return (ResultOpBD.Error);
            }
            catch (Exception ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_009", ex);
                return (ResultOpBD.Error);
            }
        }

        /// <summary>
        /// Ejecuta más de una consulta de inserción, borrado o cambio al mismo tiempo usando la misma conexión.
        /// </summary>
        /// <param name="sql">Consulta a realizar</param>
        /// <param name="iNumRowsAfected">Resultado de la consulta.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Entero, si operación correcta, null en caso de error.</returns>
        override internal ResultOpBD ExecuteCommandFromSqlMARSWithParameters(Dictionary<string, List<object>> dicParameter, out List<int> listNumRowsAfected, out string sMensaje)
        {
            listNumRowsAfected = new List<int>();
            listNumRowsAfected.Clear();
            sMensaje = string.Empty;
            try
            {
                //  Valida los parámetros de entrada.
                    if ((dicParameter == null) || (dicParameter.Count == 0))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_012");
                        return (ResultOpBD.DataInputError);
                    }
                    if ((eEstado != State.Opened) || (oSqlConnection == null) || (oSqlCommand == null) ||
                        (oSqlConnection.State != ConnectionState.Open))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_011");
                        return (ResultOpBD.InternalError);
                    }
                //  Inicia la realización de la consulta.
                    foreach (KeyValuePair<string, List<object>> kvp in dicParameter)
                    {
                        oSqlCommand.CommandText = kvp.Key;
                        oSqlCommand.CommandType = CommandType.Text;
                        oSqlCommand.Parameters.Clear();
                        if (kvp.Value != null)
                        {
                            foreach (object o in kvp.Value)
                            {
                                oSqlCommand.Parameters.Add((SqlParameter)o);
                            }
                        }
                        listNumRowsAfected.Add(oSqlCommand.ExecuteNonQuery());
                    }
                    return (ResultOpBD.Correct);
            }
            catch (SqlException ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_013", new string[] { ex.ErrorCode.ToString(), 
                                                ex.Number.ToString(), ex.LineNumber.ToString(), "ExecuteStoredProcedure", ex.Server,
                                                ex.TargetSite.Name, ex.Message });
                return (ResultOpBD.Error);
            }
            catch (Exception ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_009", ex);
                return (ResultOpBD.Error);
            }
        }

        /// <summary>
        /// Ejecuta la consulta de inserción, borrado o cambio, pasada cómo parámetro.
        /// </summary>
        /// <param name="sql">Consulta a realizar</param>
        /// <param name="iNumRowsAfected">Resultado de la consulta.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Entero, si operación correcta, null en caso de error.</returns>
        override internal ResultOpBD ExecuteCommandFromSql(string sql, List<object> oSqlParameter,out int iNumRowsAfected, out string sMensaje)
        {
            iNumRowsAfected = -1;
            sMensaje = string.Empty;
            try
            {
                //  Valida los parámetros de entrada.
                    if ((sql == null) || (sql == string.Empty))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_012");
                        return (ResultOpBD.DataInputError);
                    }
                    if ((eEstado != State.Opened) || (oSqlConnection == null) || (oSqlCommand == null) ||
                        (oSqlConnection.State != ConnectionState.Open))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_011");
                        return (ResultOpBD.InternalError);
                    }        
                //  Inicia la realización de la consulta.
                    oSqlCommand.CommandText = sql;
                    oSqlCommand.CommandType = CommandType.Text;
                    oSqlCommand.Parameters.Clear();
                    foreach (object o in oSqlParameter)
                    {
                        oSqlCommand.Parameters.Add((SqlParameter)o);                
                    }
                    iNumRowsAfected = oSqlCommand.ExecuteNonQuery(); // Convert.ToInt32();
                //  Devuelve el resultado indicado.
                    if (iNumRowsAfected == 0)
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_016");
                        return (ResultOpBD.Error);
                    }
                    else return (ResultOpBD.Correct);
            }
            catch (SqlException ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_013", new string[] { ex.ErrorCode.ToString(), 
                                                ex.Number.ToString(), ex.LineNumber.ToString(), "ExecuteStoredProcedure", ex.Server,
                                                ex.TargetSite.Name, ex.Message });
                return (ResultOpBD.Error);
            }
            catch (Exception ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_009", ex);
                return (ResultOpBD.Error);
            }
        }

        #endregion

        #region Operaciones que modifican el contenido de la Base de Datos

        /// <summary>
        /// Ejecuta la consulta de inserción, borrado o cambio, pasada cómo parámetro.
        /// </summary>
        /// <param name="sql">Consulta a realizar</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Entero, si operación correcta, null en caso de error.</returns>
        override internal ResultOpBD ActualizeDatSetSql(DataSet dsOld, DataSet dsNew, string sSql, out string sMensaje)
        {
            sMensaje = string.Empty;
            
            try
            {
                //  Variables.
                    string[] asQuery;
                    SqlDataAdapter oDataAdapterSql;

                //  Valida los parámetros de entrada.
                    if ((sSql == null) || (sSql == string.Empty))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_012");
                        return (ResultOpBD.DataInputError);
                    }
                    if ((eEstado != State.Opened) || (oSqlConnection == null) || (oSqlCommand == null) ||
                        (oSqlConnection.State != ConnectionState.Open))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_011");
                        return (ResultOpBD.InternalError);
                    }
                //  Inicia la realización de la consulta.
                    asQuery = sSql.Split(';');
                    int i = 0;
                    foreach (DataTable dt in dsNew.Tables)
                    {
                        oSqlCommand.CommandText = asQuery[i].ToString();
                        oSqlCommand.CommandType = CommandType.Text;
                        oSqlCommand.Parameters.Clear();
                        oDataAdapterSql = GetDataAdapter(oSqlCommand, out sMensaje);
                        if ( oDataAdapterSql!=null)
                        {
                            SqlCommandBuilder cb = new SqlCommandBuilder(oDataAdapterSql);
                            oDataAdapterSql.UpdateCommand = cb.GetUpdateCommand();
                            oDataAdapterSql.InsertCommand = cb.GetInsertCommand();
                            oDataAdapterSql.DeleteCommand = cb.GetDeleteCommand();
                            oDataAdapterSql.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                            oDataAdapterSql.Fill(dsOld.Tables[i]);
                            oDataAdapterSql.Update(dt);
                            oDataAdapterSql.Dispose();
                            oDataAdapterSql = null;
                            cb.Dispose();
                        }
                        else
                        {
                            sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_010", sMensaje);
                            return (ResultOpBD.InternalError);
                        }
                         
                        i++;
                    }
                    return (ResultOpBD.Correct);
            }
            catch (SqlException ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_013", new string[] { ex.ErrorCode.ToString(), 
                                                ex.Number.ToString(), ex.LineNumber.ToString(), "ExecuteStoredProcedure", ex.Server,
                                                ex.TargetSite.Name, ex.Message });
                return (ResultOpBD.Error);
            }
            catch (Exception ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_009", ex);
                return (ResultOpBD.Error);
            }
        }

        #endregion

        #endregion

        #region Relacionados con el control de Transacciones

        #region Información sobre niveles de aislamiento

        // Métodos encargados de iniciar una transacción sobre una conexión y un comando determinados.  Los 
        // diferentes tipos de transacción los determina el parámetro IsolationLevel usado en la definición
        // de la transacción (de menos a más restrictiva):
        //   
        //        · Chaos :- Los cambios pendientes de las transacciones  más aisladas  no  se  pueden 
        //                   sobrescribir.  
        // 
        //        · ReadUncommitted :- Especifica que  las  sentencias  pueden leer las filas que  han 
        //                             sido  modificadas  por  otras  transacciones  aúnque  no  hayan 
        //                             realizado un commit.  Se pueden producir lecturas erróneas,  lo 
        //                             que implica que no se emitan bloqueos compartidos y que  no  se 
        //                             cumplan los bloqueos exclusivos.
        //      
        //        · ReadCommitted :- Especifica que las sentencias no pueden leer los datos  que hayan 
        //                           sido  modificados  por  otras  transacciones  si estás aún no han 
        //                           realizado el commit. Este sistema previene las lecturas erróneas. 
        //                           Los datos  pueden  ser  cambiados  por  otras transacciones entre 
        //                           sentencias individuales dentro de la transacción actual, teniendo 
        //                           como resultado lecturas no repetidas o datos fantasma. 
        // 
        //        · RepeatableRead :- Especifica que las sentencias no pueden leer los datos que hayan 
        //                            sido modificados y  no validados  por otras transacciones y  que 
        //                            ninguna otra transacción puede modificar los datos leídos por la  
        //                            transacción actual hasta que la transacción actual se  complete.   
        //                            Esto evita las lecturas no repetibles  pero sigue existiendo  la 
        //                            posibilidad de que se produzcan filas fantasma. 
        // 
        //        · Serializable :- Las sentencias no pueden leer los datos que hayan sido modificados 
        //                          y no validados por otras transacciones. Ninguna transacción pueden 
        //                          modificar los datos leídos por la  transacción actual hasta que la 
        //                          transacción actual acabe.  Otras  transacciones  no  pueden añadir 
        //                          nuevas filas con los valores  claves  que  pertenezcan a alguna de 
        //                          las sentencias  de  la transacción actual hasta que la transacción 
        //                          actual acabe. 
        // 
        //        · Snapshot :- Especifica  que  los  datos  leídos  por  cualquier  sentencia  en una 
        //                      transacción  son  la  versión  coherente  de los datos que existían al 
        //                      inicarse la misma.  La transacción sólo puede reconocer los cambios de
        //                      datos que fueron realizados antes del inicio de la misma.  Los cambios 
        //                      de datos hechos por otras transacciones después de que el comienzo  de 
        //                      la transacción actual no son visibles a las sentencias que se ejecutan 
        //                      en la transacción actual.  El  efecto es como si las sentencias en una 
        //                      transacción consigan una fotografía de los datos tal y como estaban al 
        //                      iniciarse la transacción. Reduce el bloqueo almacenando una versión de 
        //                      los  datos  que  una  aplicación  puede leer mientras  otra  los  está 
        //                      modificando.  Indica  que  de  una  transacción  no  se pueden ver los 
        //                      cambios realizados en otras transacciones, aunque se vuelva a realizar 
        //                      una consulta.   
        // 
        //        · Unspecified :- Se utiliza un nivel de aislamiento distinto al  especificado,  pero 
        //                         no se puede determinar el nivel. 
        //                         
        // Al utilizar OdbcTransaction,  si no establece IsolationLevel o establece IsolationLevel  en 
        // Unspecied,  la  transacción se ejecuta según  el nivel  de  aislamiento predeterminado  del 
        // controlador ODBC subyacente.

        #endregion

        #region Begin Transaction

        /// <summary>
        /// Método encargado de iniciar una transacción sobre la conexión representada por una instáncia de esta
        /// clase.
        /// </summary>
        /// <param name="sMensaje">Mensaje de error si se produce uno.</param>
        /// <returns>Mirar descripción del enumerado.</returns>
        override internal ResultOpBD BeginTransaction(out string sMensaje)
        {
            return (BeginTransaction(IsolationLevel.Serializable, out sMensaje));
        }

        /// <summary>
        /// Método encargado de iniciar una transacción sobre la conexión representada por una instáncia de esta
        /// clase con el nivel de aislamiento indicado por el valor pasado cómo parámetro.
        /// </summary>
        /// <param name="eIsolationLevel">Nivel de Aislamiento con el que se desea abrir la transacción.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar descripción del enumerado.</returns>
        override internal ResultOpBD BeginTransaction(IsolationLevel eIsolationLevel, out string sMensaje)
        {
            sMensaje = string.Empty;
            try
            {
                if ((eEstado != State.Opened) || (oSqlConnection == null) || (oSqlCommand == null) ||
                    (oSqlConnection.State != ConnectionState.Open))
                {
                    sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_011");
                    return (ResultOpBD.InternalError);
                }
                if ((oSqlConnection != null) && (oSqlConnection.State == ConnectionState.Open))
                {
                    oSqlCommand.Transaction = oSqlConnection.BeginTransaction(eIsolationLevel);
                    return (ResultOpBD.Correct);
                }
                else
                {
                    sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_018");
                    return (ResultOpBD.InternalError);
                }
            }
            catch (SqlException ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_013", new string[] { ex.ErrorCode.ToString(), 
                                                ex.Number.ToString(), ex.LineNumber.ToString(), "ExecuteStoredProcedure", ex.Server,
                                                ex.TargetSite.Name, ex.Message });
                return (ResultOpBD.Error);
            }
            catch (Exception ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_017", ex);
                return (ResultOpBD.Error);
            }
        }

        #endregion

        #region Commit

        /// <summary>
        /// Método encargado de finalizar una transacción aceptando los cambios que se hayan producido.
        /// </summary>
        /// <param name="oSqlTransaction">Transacción a finalizar</param>
        /// <param name="oSqlCommand">Comando que se quiere ejecutar de manera transaccional</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar descripción del enumerado.</returns>
        override internal ResultOpBD CommitTransaction(out string sMensaje)
        {
            sMensaje = string.Empty;
            try
            {
                //  Finaliza la transacción indicada de manera correcta.
                    if (oSqlCommand.Transaction != null)
                    {
                        oSqlCommand.Transaction.Commit();

                        if (oSqlCommand.Transaction != null)
                            oSqlCommand.Transaction.Dispose();

                        oSqlCommand.Transaction = null;
                        return (ResultOpBD.Correct);
                    }
                    else
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_020");
                        return (ResultOpBD.DataInputError);
                    }
            }
            catch (SqlException ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_013", new string[] { ex.ErrorCode.ToString(), 
                                                ex.Number.ToString(), ex.LineNumber.ToString(), "ExecuteStoredProcedure", ex.Server,
                                                ex.TargetSite.Name, ex.Message });
                return (ResultOpBD.Error);
            }
            catch (Exception ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_019", ex);
                return (ResultOpBD.Error);
            }
        }

        #endregion

        #region Rollback

        /// <summary>
        /// Método encargado de finalizar una transacción descartando los cambios que se hayan producido.
        /// </summary>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar descripción del enumerado.</returns>
        override internal ResultOpBD RollbackTransaction(out string sMensaje)
        {
            sMensaje = string.Empty;
            try
            {
                //  Cierra la transacción rechazando los cambios que en ella se han producido.
                    if (oSqlCommand.Transaction != null)
                    {
                        oSqlCommand.Transaction.Rollback();
                        if (oSqlCommand.Transaction!=null)
                            oSqlCommand.Transaction.Dispose();
                        oSqlCommand.Transaction = null;
                        return (ResultOpBD.Correct);
                    }
                    else
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_020");
                        return (ResultOpBD.DataInputError);
                    }
            }
            catch (SqlException ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_013", new string[] { ex.ErrorCode.ToString(), 
                                                ex.Number.ToString(), ex.LineNumber.ToString(), "ExecuteStoredProcedure", ex.Server,
                                                ex.TargetSite.Name, ex.Message });
                return (ResultOpBD.Error);
            }
            catch (Exception ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_021", ex);
                return (ResultOpBD.Error);
            }
        }

        #endregion

        #endregion
        
        #region Relacionados con las operaciones con DataReaders

        #region GetDataReader

        /// <summary>
        /// Método que obtiene un DataReader que permite recorrer los registros asociados a la consulta 
        /// pasada como parámetro.
        /// </summary>
        /// <param name="sQuery">Consulta a realizar.</param>
        /// <param name="guidDataReader">Identificador de la operación mediante DataReaders.</param>
        /// <param name="drd">DataReader, que permite recorrer los registros de la consulta si OK, null, si error.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>Mirar la descripción del enumerado.</returns>
        internal override ResultOpBD GetDataReader(string sQuery, out DbDataReader drdData, out string sMensaje)
        {
            sMensaje = string.Empty;
            drdData = null;
            try
            {
                //  Valida los parámetros de entrada.
                    if ((sQuery == null) || (sQuery == string.Empty))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_012");
                        return (ResultOpBD.DataInputError);
                    }
                    if ((eEstado != State.Opened) || (oSqlConnection == null) || (oSqlCommand == null) || 
                        (oSqlConnection.State != ConnectionState.Open))
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_011");
                        return (ResultOpBD.InternalError);
                    }
                //  Inicia la realización de la consulta.
                    oSqlCommand.CommandText = sQuery;
                    oSqlCommand.CommandType = CommandType.Text;
                    oSqlCommand.Parameters.Clear();
                    if ((oSqlDataReader = oSqlCommand.ExecuteReader()) == null)
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_009", sMensaje);
                        return (ResultOpBD.InternalError);
                    }
                    else
                    {
                        drdData = oSqlDataReader;
                        return (ResultOpBD.Correct);
                    }
            }
            catch (SqlException ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_013", new string[] { ex.ErrorCode.ToString(), 
                                                ex.Number.ToString(), ex.LineNumber.ToString(), "ExecuteStoredProcedure", ex.Server,
                                                ex.TargetSite.Name, ex.Message });
                return (ResultOpBD.Error);
            }
            catch (Exception ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_022", ex);
                return (ResultOpBD.Error);
            }
        }

        #endregion

        #region CloseDataReader

        /// <summary>
        /// Método que cierra el DataReader sobre el que se ha realizado la consulta.
        /// </summary>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>Mirar la descripción del enumerado.</returns>
        internal override ResultOpBD CloseDataReader(out string sMensaje)
        {
            sMensaje = string.Empty;
            try
            {
                //  Cierra el DataReader usado indicada de manera correcta.
                    if (oSqlDataReader != null)
                    {
                        if (!oSqlDataReader.IsClosed) oSqlDataReader.Close();
                        oSqlDataReader = null;
                        return (ResultOpBD.Correct);
                    }
                    else
                    {
                        sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_024");
                        return (ResultOpBD.DataInputError);
                    }
            }
            catch (SqlException ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_013", new string[] { ex.ErrorCode.ToString(), 
                                                ex.Number.ToString(), ex.LineNumber.ToString(), "ExecuteStoredProcedure", ex.Server,
                                                ex.TargetSite.Name, ex.Message });
                return (ResultOpBD.Error);
            }
            catch (Exception ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_023", ex);
                return (ResultOpBD.Error);
            }
        }

        #endregion

        #endregion

        #region Auxiliares

        /// <summary>
        /// Método encargado de Obtener una conexión a partir de los parámetros previamente definidos en la 
        /// inicialización de la clase.  Se puede  usar  si  se  sabe que los parámetros con los que se  ha 
        /// definido la clase corresponden a una conexión SQL Server.
        /// </summary>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Conexión, si todo correcto, null, si se ha producido algun error</returns>
        private SqlConnection GetConnection(out string sMensaje)
        {
            sMensaje = string.Empty;
            try
            {
                return (new SqlConnection(sConnectionString));
            }
            catch (SqlException ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_013", new string[] { ex.ErrorCode.ToString(), 
                                                ex.Number.ToString(), ex.LineNumber.ToString(), "ExecuteStoredProcedure", ex.Server,
                                                ex.TargetSite.Name, ex.Message });
                return (null);
            }
            catch (Exception ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_025", ex);
                return (null);
            }
        }

        /// <summary>
        /// Método encargado de Obtener un DataAdapter a partir de los parámetros previamente definidos en 
        /// la inicialización de la clase.  Se puede usar si se  sabe que los parámetros con los que se ha 
        /// definido la clase corresponden a una conexión SQL Server.
        /// </summary>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Command, si todo correcto, null, si se ha producido algun error</returns>
        private SqlCommand GetCommand(out string sMensaje)
        {
            sMensaje = string.Empty;
            try
            {
                return (new SqlCommand());
            }
            catch (SqlException ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_013", new string[] { ex.ErrorCode.ToString(), 
                                                ex.Number.ToString(), ex.LineNumber.ToString(), "ExecuteStoredProcedure", ex.Server,
                                                ex.TargetSite.Name, ex.Message });
                return (null);
            }
            catch (Exception ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_026", ex);
                return (null);
            }
        }

        /// <summary>
        /// Método encargado de Obtener un DataAdapter a partir de los parámetros previamente definidos
        /// en la inicialización de la clase.  Se  puede usar si se sabe que los parámetros con los que
        /// se ha definido la clase corresponden a una conexión SQL Server.
        /// </summary>
        /// <param name="command">Command asociado al DataAdapter a crear</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Conexión, si todo correcto, null, si se ha producido algun error</returns>
        private SqlDataAdapter GetDataAdapter(SqlCommand command, out string sMensaje)
        {
            sMensaje = string.Empty;
            try
            {
                return (new SqlDataAdapter(command));
            }
            catch (SqlException ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_013", new string[] { ex.ErrorCode.ToString(), 
                                                ex.Number.ToString(), ex.LineNumber.ToString(), "ExecuteStoredProcedure", ex.Server,
                                                ex.TargetSite.Name, ex.Message });
                return (null);
            }
            catch (Exception ex)
            {
                sMensaje += MsgManager.ErrorMsg("MSG_CADbDataConnection(DataBase)_027", ex);
                return (null);
            }
        }

        #endregion

        #endregion
    }
}
