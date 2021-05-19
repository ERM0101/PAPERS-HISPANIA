#region Librerias usadas por la clase

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Timers;

#endregion

#region Directivas de compilación

#pragma warning disable 0168   // Desactiva el aviso relacionado con las variables definidas y no usadas.
#pragma warning disable 0169   // Desactiva el aviso relacionado con las variables definidas y no usadas.
#pragma warning disable 0414   // Desactiva el aviso relacionado con las variables definidas y no usadas.

#endregion

namespace MBCode.Framework.DataBase
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou
    /// Fecha última modificación: 27/02/2012.
    /// Descripción: Interfície que define las características básicas que debe tener un item de conexión a un
    ///              determinado tupo de Base de Datos. 
    /// </summary>
    internal abstract class CADbDataConnection : IDbDataConnection
    {
        #region Constantes

        /// <summary>
        /// Almacena el mínimo tiempo que puede transcurrir entre dos procesos de encuesta del TimeOut.
        /// </summary>
        private const int MINIUM_TIME_CLOCK = 100;

        #endregion

        #region Enumerados

        /// <summary>
        /// Enumerado que define los posibles estados en los que puede estar la conexión.
        /// </summary>
        public enum State
        {
            /// <summary>
            /// Indica que la conexión no ha sido inicializada bien sea porque se ha proucido un error al
            /// cargar los datos bien sea porque nunca se ha inicializado la clase.
            /// </summary>
            Undefined,

            /// <summary>
            /// Indica que la conexión ha sido inicializada y abierta.
            /// </summary>
            Initialized,

            /// <summary>
            /// Indica que el proceso de carga de los datos de la conexión se ha realizado con éxito.
            /// </summary>
            DataCharged,

            /// <summary>
            /// Indica que la conexión esta activa.
            /// </summary>
            Opened,
        }

        #endregion

        #region Delegados

        /// <summary>
        /// Delegado que define la firma del evento que notifica que se ha producido un cambio de estado en la 
        /// conexión.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada al elemento que produce el evento</param>
        /// <param name="guidConnection">Identificador de la conexión asociada al elemento que produce el evento.</param>
        /// <param name="e">Parámetros con los que se lanza el evento.</param>
        public delegate void dlgDbConnectionStateChanged(DbKeyApp dbkaApplication, Guid guidConnection, StateChangeEventArgs e);

        /// <summary>
        /// Delegado que define la firma del evento que notifica que se ha producido un TimeOut en la conexión.
        /// </summary>
        /// <param name="sender"></param>
        public delegate void dlgTimeOut(CADbDataConnection sender);

        #endregion

        #region Eventos

        /// <summary>
        /// Evento que se produce cuando cambia el estado de la conexión.
        /// </summary>
        public abstract event dlgDbConnectionStateChanged evEstadoConexionCambiado;

        /// <summary>
        /// Evento que se produce en el momento en que se cumple el TimeOut de la conexión.
        /// </summary>
        public event dlgTimeOut evTimeOut;

        /// <summary>
        /// Evento que se produce en el momento en que se cumple el TimeOut de la transacción.
        /// </summary>
        public event dlgTimeOut evTransactionTimeOut;

        /// <summary>
        /// Evento que se produce en el momento en que se cumple el TimeOut de la operación con DataReaders.
        /// </summary>
        public event dlgTimeOut evDataReaderTimeOut;

        #endregion

        #region Atributos

        #region Identificador de la conexión

        /// <summary>
        /// Almacena el identificador del objeto dentro del controlador de conexiones.
        /// </summary>
        protected Guid? guidIdentifier;

        #endregion

        #region Identificador de la transacción

        /// <summary>
        /// Almacena el identificador de la transacción dentro de la tabla de hash de transacciones.
        /// </summary>
        private Guid guidTransactionId;

        #endregion

        #region Identificador de la operación con DataReaders

        /// <summary>
        /// Almacena el identificador de la operación con DataReaders a realizar dentro de la tabla de hash 
        /// de operaciones con DataReaders.
        /// </summary>
        private Guid guidDataReaderId;

        #endregion

        #region Clave asociada a la Aplicación

        /// <summary>
        /// Almacena la clave asociada a la aplicación que es propietaria de la conexión.
        /// </summary>
        protected DbKeyApp dbkaApplication;

        #endregion

        #region Propias de la Conexión

        /// <summary>
        /// Almacena el tipo de Base de Datos con el que trabajar.
        /// </summary>
        protected DBType eTipoBD;

        /// <summary>
        /// Almacena la cadena de conexión con la que se realiza la conexión a la Base de Datos.
        /// </summary>
        protected string sConnectionString;

        #endregion

        #region Atributos de control de la clase

        /// <summary>
        /// Almacena un valor que indica el estado en el que se encuentra la conexión.
        /// </summary>
        protected State eEstado;

        #endregion

        #region Control del Timeout

        #region Connection TimeOut

        /// <summary>
        /// Almacena el tiempo máximo, expresado en milisegundos, que se esperará antes de que se considere que la 
        /// conexión ya no es útil. El tiempo ha de ser superior a 100ms.
        /// </summary>
        private TimeSpan tsTimeOut;

        /// <summary>
        /// Almacena el tiempo, expresado en milisegundos, transcurrido desde el último uso de la conexión.
        /// </summary>
        private TimeSpan tsElapsedTime;

        /// <summary>
        /// Timer que controla el timeout de la conexión.
        /// </summary>
        private Timer tmrTimeout;

        #endregion

        #region Transaction TimeOut

        /// <summary>
        /// Almacena el tiempo máximo, expresado en milisegundos, que se esperará antes de que se considere que la 
        /// transacción ya no es útil. El tiempo ha de ser superior a 100ms.
        /// </summary>
        private TimeSpan tsTransactionTimeOut;

        /// <summary>
        /// Almacena el tiempo, expresado en milisegundos, transcurrido desde el inicio de la transacción.
        /// </summary>
        private TimeSpan tsTransactionElapsedTime;

        /// <summary>
        /// Timer que controla el timeout de la transacción.
        /// </summary>
        private Timer tmrTransactionTimeout;

        #endregion

        #region DataReader Timeout

        /// <summary>
        /// Almacena el tiempo máximo, expresado en milisegundos, que se esperará antes de que se considere que la 
        /// transacción ya no es útil. El tiempo ha de ser superior a 100ms.
        /// </summary>
        private TimeSpan tsDataReaderTimeOut;

        /// <summary>
        /// Almacena el tiempo, expresado en milisegundos, transcurrido desde el inicio de la transacción.
        /// </summary>
        private TimeSpan tsDataReaderElapsedTime;

        /// <summary>
        /// Timer que controla el timeout de una operación con DataReaders.
        /// </summary>
        private Timer tmrDataReaderTimeout;

        #endregion

        #region Comunes

        /// <summary>
        /// Almacena el valor constante correspondiente al incremento de tiempo a realizar al calculo del tiempo 
        /// transcurrido desde el último uso de la conexión.
        /// </summary>
        private TimeSpan tsIncrement = new TimeSpan(0, 0, 0, 0, MINIUM_TIME_CLOCK);

        /// <summary>
        /// Almacena el valor constante usado para inicializar el contador.
        /// </summary>
        private TimeSpan tsZero = new TimeSpan(0);

        #endregion

        #endregion

        #endregion

        #region Propiedades

        #region Identificador de la conexión

        /// <summary>
        /// Obtiene el identificador del objeto dentro del controlador de conexiones.
        /// </summary>
        public Guid? Identifier
        {
            get
            {
                return (guidIdentifier);
            }
        }

        #endregion

        #region Identificador de Transacción

        /// <summary>
        /// Obtiene/Establece el identificador de la transacción dentro de la tabla de hash de transacciones.
        /// </summary>
        public Guid TransactionId
        {
            get 
            { 
                return (guidTransactionId); 
            }
            set 
            { 
                guidTransactionId = value; 
            }
        }

        #endregion

        #region Identificador de la operación con DataReader

        /// <summary>
        /// Obtiene/Establece el identificador de la operación con DataReaders iniciada dentro de la tabla de 
        /// hash de operaciones con DataReaders.
        /// </summary>
        public Guid DataReaderId
        {
            get
            {
                return (guidDataReaderId);
            }
            set
            {
                guidDataReaderId = value;
            }
        }

        #endregion

        #region Clave asociada a la aplicación

        /// <summary>
        /// Obtiene  la clave asociada a la aplicación que es propietaria de la conexión.
        /// </summary>
        public DbKeyApp DbKeyApplication
        {
            get
            {
                return (this.dbkaApplication);
            }
        }

        #endregion

        #region Propias de la Conexión

        /// <summary>
        /// Obtiene el tipo de Base de Datos con el que trabajar.
        /// </summary>
        public DBType TipoBaseDatos
        {
            get
            {
                return (this.eTipoBD);
            }
        }

        /// <summary>
        /// Obtiene la cadena de conexión con la que se realiza la conexión a la Base de Datos.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return (this.sConnectionString);
            }
        }

        #endregion

        #region Atributos de control de la clase

        /// <summary>
        /// Obtiene/Establece un valor que indica el estado en el que se encuentra la conexión.
        /// </summary>
        public State Estado
        {
            get
            {
                return eEstado;
            }
        }

        #endregion

        #region Consultoras

        /// <summary>
        /// Obtiene el estado de la conexión. 
        /// </summary>
        public abstract ConnectionState? EstadoConexion { get; }

        #endregion

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor por defecto de la clase.
        /// </summary>
        /// <param name="dbkaApplication">Clave asociada a la aplicación para la que se crea la conexión.</param>
        /// <param name="eTipoBD">Tipo de Base de Datos a inicializar.</param>
        /// <param name="guidIdentifier">Identificador de la conexión.</param>
        /// <param name="sConnectionString">Cadena de conexión asociada a la clave pasada como parámetro.</param>
        /// <param name="iTimeoutConnection">Tiempo, expresado en milisegundos, que ha de transcurrir hasta que salte el timeout de la conexión.</param>
        /// <param name="iTimeoutTransaction">Tiempo, expresado en milisegundos, que ha de transcurrir hasta que salte el timeout de la transacción.</param>
        public CADbDataConnection(DbKeyApp dbkaApplication, DBType eTipoBD, Guid? guidIdentifier, string sConnectionString, int iTimeoutConnection, int iTimeoutTransaction)
        {
            this.eTipoBD = eTipoBD;
            this.guidTransactionId = Guid.Empty;
            this.guidDataReaderId = Guid.Empty;
            this.guidIdentifier = guidIdentifier;
            this.dbkaApplication = dbkaApplication;
            this.sConnectionString = sConnectionString;
            this.eEstado = State.Undefined;
            this.tsTimeOut = new TimeSpan(0, 0, 0, 0, iTimeoutConnection);
            this.tsTransactionTimeOut = new TimeSpan(0, 0, 0, 0, iTimeoutTransaction);
            this.tsElapsedTime = this.tsZero;
            this.tsTransactionElapsedTime = this.tsZero;
            this.tmrTimeout = new Timer();
            this.tmrTimeout.Interval = MINIUM_TIME_CLOCK;
            this.tmrTimeout.Elapsed += new ElapsedEventHandler(TimeOutController);
            this.tmrTimeout.Enabled = true;
            this.tmrTransactionTimeout = new Timer();
            this.tmrTransactionTimeout.Interval = MINIUM_TIME_CLOCK;
            this.tmrTransactionTimeout.Elapsed += new ElapsedEventHandler(TransactionTimeoutController);
            this.tmrTransactionTimeout.Enabled = false;
            this.tmrDataReaderTimeout = new Timer();
            this.tmrDataReaderTimeout.Interval = MINIUM_TIME_CLOCK;
            this.tmrDataReaderTimeout.Elapsed += new ElapsedEventHandler(DataReaderTimeoutController);
            this.tmrDataReaderTimeout.Enabled = false;
        }

        #endregion

        #region Métodos

        #region Declarados

        #region Relacionados con la Conexión

        /// <summary>
        /// Método encargado de inicializar la conexión representada por la clase.
        /// </summary>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>Mirar descripción del enumerado.</returns>
        internal abstract ResultOpBD Initialize(out string sMensaje);

        /// <summary>
        /// Método que se encarga de finalizar la conexión representada por la clase.
        /// </summary>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, si operación correcta, false, si error.</returns>
        internal abstract ResultOpBD Finalize(out string sMensaje);

        #endregion

        #region Relacionados con la ejecución de Consultas y Procedimientos Almacenados

        #region Ejecución de Consultas

        /// <summary>
        /// Realiza la consulta indicada por la cadena de carácteres pasada cómo parámetro y devuelve 
        /// un DataSet con el resultado.
        /// </summary>
        /// <param name="sql">Consulta a realizar</param>
        /// <param name="bWithSchema">Indica si se deben utilizar o no las restricciones</param>
        /// <param name="oDataSet">Datos leídos</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>DataSet, si operación correcta, null en caso de error.</returns>
        internal abstract ResultOpBD GetDataSetFromSql(string sql, bool bWithSchema, out DataSet oDataSet, out string sMensaje);

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
        internal abstract ResultOpBD ExecuteStoredProcedure(string sNameStored, DbParameter[] oDbParameter, 
                                                            bool bWithSchema, out DataSet oDataSet, out string sMensaje);

        /// <summary>
        /// Ejecuta el Stored Procedure indicado por la cadena de carácteres pasada cómo parámetro y devuelve 
        /// un DataSet con el resultado.
        /// </summary>
        /// <param name="sNameStored">Nombre del procedimiento a ejecutar.</param>
        /// <param name="oDbParameter">Parámetros con los que se desea ejecutar el procedimiento.</param>
        /// <param name="oDataSet">Datos leídos</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>DataSet, si operación correcta, null en caso de error.</returns>
        internal abstract ResultOpBD ExecuteStoredProcedure(string sNameStored, DbParameter[] oDbParameter,
                                                            bool bWithSchema, out List<object> iRowsAffected, out string sMensaje);

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
        internal abstract ResultOpBD GetEscalarFromSql(string sql, out int iEscalarResult, out string sMensaje);

        /// <summary>
        /// Realiza la consulta indicada por la cadena de carácteres pasada cómo parámetro y devuelve un valor escalar 
        /// como resultado de la ejecución de la misma.
        /// </summary>
        /// <param name="sql">Consulta a realizar</param>
        /// <param name="iEscalarResult">Resultado de la consulta.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Entero, si operación correcta, null en caso de error.</returns>
        internal abstract ResultOpBD GetEscalarFromWithParameter(Dictionary<string, List<object>> dicSql,out List<int> listEscalarResult, out string sMensaje);

        #endregion

        #region Realización de una actualización de Datos de la Base de Datos mediante DataSets

        /// <summary>
        /// Método de realización de una actualización de Datos de la Base de Datos mediante DataSets.
        /// </summary>
        /// <param name="dsOld">DataSet con los Datos originales.</param>
        /// <param name="dsNew"></param>
        /// <param name="sSql"></param>
        /// <param name="sMessageQuery"></param>
        /// <returns></returns>
        internal abstract ResultOpBD ActualizeDatSetSql(DataSet dsOld, DataSet dsNew, string sSql, out string sMessageQuery);

        #endregion

        #region Operaciones que modifican el contenido de la Base de Datos

        /// <summary>
        /// Ejecuta la consulta de inserción, borrado o cambio, pasada cómo parámetro.
        /// </summary>
        /// <param name="sql">Consulta a realizar</param>
        /// <param name="iNumRowsAfected">Resultado de la consulta.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Entero, si operación correcta, null en caso de error.</returns>
        internal abstract ResultOpBD ExecuteCommandFromSql(string sql, out int iNumRowsAfected, out string sMensaje);

        /// <summary>
        /// Ejecuta la consulta de inserción, borrado o cambio, pasada cómo parámetro.
        /// </summary>
        /// <param name="sql">Consulta a realizar</param>
        /// <param name="iNumRowsAfected">Resultado de la consulta.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Entero, si operación correcta, null en caso de error.</returns>
        internal abstract ResultOpBD ExecuteCommandFromSql(string sql,List<object> parameter, out int iNumRowsAfected, out string sMensaje);

        /// <summary>
        /// Ejecuta más de una consulta de inserción, borrado o cambio al mismo tiempo usando la misma conexión.
        /// </summary>
        /// <param name="sql">Consulta a realizar</param>
        /// <param name="iNumRowsAfected">Resultado de la consulta.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Entero, si operación correcta, null en caso de error.</returns>
        internal abstract ResultOpBD ExecuteCommandFromSqlMARS(List<string> listSql, out List<int> listNumRowsAfected, out string sMensaje);

        /// <summary>
        /// Ejecuta más de una consulta de inserción, borrado o cambio al mismo tiempo usando la misma conexión.
        /// </summary>
        /// <param name="sql">Consulta a realizar</param>
        /// <param name="iNumRowsAfected">Resultado de la consulta.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Entero, si operación correcta, null en caso de error.</returns>
        internal abstract ResultOpBD ExecuteCommandFromSqlMARSWithParameters(Dictionary<string,List<object>> dicParameter,out List<int> listNumRowsAfected, out string sMensaje);
       

        #endregion

        #endregion

        #region Relacionados con el control de Transacciones

        #region Begin Transaction

        /// <summary>
        /// Método encargado de iniciar una transacción sobre la conexión representada por una instáncia de esta
        /// clase.
        /// </summary>
        /// <param name="sMensaje">Mensaje de error si se produce uno.</param>
        /// <returns>Mirar descripción del enumerado.</returns>
        internal abstract ResultOpBD BeginTransaction(out string sMensaje);

        /// <summary>
        /// Método encargado de iniciar una transacción sobre la conexión representada por una instáncia de esta
        /// clase con el nivel de aislamiento indicado por el valor pasado cómo parámetro.
        /// </summary>
        /// <param name="eIsolationLevel">Nivel de Aislamiento con el que se desea abrir la transacción.</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar descripción del enumerado.</returns>
        internal abstract ResultOpBD BeginTransaction(IsolationLevel eIsolationLevel, out string sMensaje);

        #endregion

        #region Commit

        /// <summary>
        /// Método encargado de finalizar una transacción aceptando los cambios que se hayan producido.
        /// </summary>
        /// <param name="oSqlTransaction">Transacción a finalizar</param>
        /// <param name="oSqlCommand">Comando que se quiere ejecutar de manera transaccional</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar descripción del enumerado.</returns>
        internal abstract ResultOpBD CommitTransaction(out string sMensaje);

        #endregion

        #region Rollback

        /// <summary>
        /// Método encargado de finalizar una transacción descartando los cambios que se hayan producido.
        /// </summary>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar descripción del enumerado.</returns>
        internal abstract ResultOpBD RollbackTransaction(out string sMensaje);

        #endregion

        #endregion

        #region Relacionados con el control de DataReaders

        #region GetDataReader

        /// <summary>
        /// Método encargado de obtener un DataReader asociado a la consulta pasada como parámetro.
        /// </summary>
        /// <param name="sMensaje">Mensaje de error si se produce uno.</param>
        /// <returns>Mirar descripción del enumerado.</returns>
        internal abstract ResultOpBD GetDataReader(string sQuery, out DbDataReader drdData, out string sMensaje);

        #endregion

        #region CloseDataReader

        /// <summary>
        /// Método encargado de finalizar una operación mediante un DataReader.
        /// </summary>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Mirar descripción del enumerado.</returns>
        internal abstract ResultOpBD CloseDataReader(out string sMensaje);

        #endregion

        #endregion

        #endregion

        #region Implementados

        #region TimeOut de Conexiones

        /// <summary>
        /// Método encargado de controlar que no se produzca un TimeOut de la conexión.
        /// </summary>
        /// <param name="sender">Objeto que lanza el evento.</param>
        /// <param name="e">Parámetros con los que se lanza el evento.</param>
        private void TimeOutController(object sender, ElapsedEventArgs e)
        {
            this.tsElapsedTime = this.tsElapsedTime.Add(this.tsIncrement);
            if (this.tsElapsedTime >= this.tsTimeOut)
            {
                this.tmrTimeout.Enabled = false;
                if (evTimeOut != null) evTimeOut(this);
            }
        }

        /// <summary>
        /// Método que permite a las clases que heredan de la actual detener el Reloj asociado al TiemOut.
        /// </summary>
        public void StopTimeOut()
        {
            this.tmrTimeout.Enabled = false;
        }

        /// <summary>
        /// Método que permite que las clases que heredan de la actual reinicien el Reloj que controla el TimeOut de la
        /// conexión.
        /// </summary>
        public void StartTimeOut()
        {
            this.tsElapsedTime = this.tsZero;
            this.tmrTimeout.Enabled = true;
        }

        #endregion

        #region TimeOut de Transacciones

        /// <summary>
        /// Método encargado de controlar que no se produzca un TimeOut de la transacción.
        /// </summary>
        /// <param name="sender">Objeto que lanza el evento.</param>
        /// <param name="e">Parámetros con los que se lanza el evento.</param>
        private void TransactionTimeoutController(object sender, ElapsedEventArgs e)
        {
            this.tsTransactionElapsedTime = this.tsTransactionElapsedTime.Add(this.tsIncrement);
            if (this.tsTransactionElapsedTime >= this.tsTransactionTimeOut)
            {
                this.tmrTransactionTimeout.Enabled = false;
                if (evTransactionTimeOut != null) evTransactionTimeOut(this);
            }
        }

        /// <summary>
        /// Método que permite a las clases que heredan de la actual detener el Reloj asociado al TiemOut.
        /// </summary>
        public void StopTransactionTimeOut()
        {
            this.tmrTransactionTimeout.Enabled = false;
        }

        /// <summary>
        /// Método que permite que las clases que heredan de la actual reinicien el Reloj que controla el TimeOut de la
        /// conexión.
        /// </summary>
        public void StartTransactionTimeOut()
        {
            this.tsTransactionElapsedTime = this.tsZero;
            this.tmrTransactionTimeout.Enabled = true;
        }

        #endregion

        #region TimeOut de Operaciones con DataReaders

        /// <summary>
        /// Método encargado de controlar que no se produzca un TimeOut de la transacción.
        /// </summary>
        /// <param name="sender">Objeto que lanza el evento.</param>
        /// <param name="e">Parámetros con los que se lanza el evento.</param>
        private void DataReaderTimeoutController(object sender, ElapsedEventArgs e)
        {
            this.tsDataReaderElapsedTime = this.tsDataReaderElapsedTime.Add(this.tsIncrement);
            if (this.tsDataReaderElapsedTime >= this.tsDataReaderTimeOut)
            {
                this.tmrDataReaderTimeout.Enabled = false;
                if (evDataReaderTimeOut != null) evDataReaderTimeOut(this);
            }
        }

        /// <summary>
        /// Método que permite a las clases que heredan de la actual detener el Reloj asociado al TiemOut.
        /// </summary>
        public void StopDataReaderTimeOut()
        {
            this.tmrDataReaderTimeout.Enabled = false;
        }

        /// <summary>
        /// Método que permite que las clases que heredan de la actual reinicien el Reloj que controla el TimeOut de la
        /// conexión.
        /// </summary>
        public void StartDataReaderTimeOut(int iSecondsInterval)
        {
            this.tsDataReaderElapsedTime = this.tsZero;
            this.tmrDataReaderTimeout.Interval = iSecondsInterval * 1000;
            this.tmrDataReaderTimeout.Enabled = true;
        }

        #endregion

        #endregion

        #endregion
    }
}




