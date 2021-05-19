#region Librerias usadas por la clase

using MBCode.Framework.Managers.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

#endregion

namespace MBCode.Framework.DataBase
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha última modificación: 27/02/2012.
    /// Descripción: interfície que define los elementos basicos de los que debe disponer la clase que almacena
    ///              los datos de una conexión.
    /// </summary>
    internal abstract class CADbDataConnectionEventArgs : IDbDataConnectionEventArgs
    {
        #region Constantes

        /// <summary>
        /// Tiempo de espera hasta que se indica que el mutex ha fallado.
        /// </summary>
        private const int WAIT_TIME_MUTEX = 5000;

        #endregion

        #region Atributos

        #region  Identificador de la conexión

        /// <summary>
        /// Almacena el identificador de la conexión.
        /// </summary>
        private Guid? guidIdentifier;

        #endregion

        #region Identificador de la Aplicación

        /// <summary>
        /// Almacena el identificador de la aplicación que ha iniciado la Conexión.
        /// </summary>
        protected Guid? guidApplicationID;

        #endregion

        #region Tipo de Base de Datos

        /// <summary>
        /// Almacena el tipo de Base de Datos a usar.
        /// </summary>
        private DBType eDBType;

        #endregion

        #region Datos de la Conexión

        /// <summary>
        /// Alamcena la cadena de conexión asociada.
        /// </summary>
        private string sConnectionString;

        #endregion

        #region Datos Relacionados con el número de conexiones y la gestión de las mismas

        /// <summary>
        /// Almacena el número máximo de conexiones en uso o libres que acepta la aplicación durante su ejecución.
        /// </summary>
        private int iNumMaxConnections;

        /// <summary>
        /// Almacena el número de conexiones en uso o libres que acepta la aplicación durante su ejecución.
        /// </summary>
        private int iNumConnections;

        /// <summary>
        /// Cola que contiene las operaciones de Base de Datos bloqueadas por falta de conexiones con que 
        /// realizarlas.
        /// </summary>
        private static Queue<DbControllExecution> qConnectionsPending = null;

        /// <summary>
        /// Tabla de Hash que contiene las conexiones a usar para ejecutar las consultas pendientes.
        /// </summary>
        private static Hashtable htConnections = null;

        #endregion

        #endregion

        #region Propiedades

        #region  Identificador de la conexión

        /// <summary>
        /// Obtiene/Establece el identificador de la conexión.
        /// </summary>
        public Guid? Identifier
        {
            get
            {
                return (guidIdentifier);
            }
            set
            {
                this.guidIdentifier = value;
            }
        }

        #endregion

        #region Identificador de la Aplicación

        /// <summary>
        /// Obtiene/Establece el identificador de la aplicación que ha iniciado la Conexión.
        /// </summary>
        public Guid? ApplicationID
        {
            get
            {
                return (guidApplicationID);
            }
            set
            {
                this.guidApplicationID = value;
            }
        }

        #endregion

        #region Tipo de Base de Datos

        /// <summary>
        /// Obtiene/Establece el tipo de Base de Datos a usar.
        /// </summary>
        public DBType DataBaseType
        {
            get
            {
                return (eDBType);
            }
            set
            {
                eDBType = value;
            }
        }

        #endregion

        #region Datos de la Conexión

        /// <summary>
        /// Obtiene/Establece la cadena de conexión asociada.
        /// </summary>
        public string ConnectionString
        {
            get
            {
                return (this.sConnectionString);
            }
            set
            {
                this.sConnectionString = value;
            }
        }

        #endregion

        #region Datos Relacionados con el número de conexiones

        /// <summary>
        /// Obtiene  el número máximo de conexiones en uso o libres que acepta la aplicación durante  su 
        /// ejecución.
        /// </summary>
        internal int NumMaxConnections
        {
            get
            {
                return (iNumMaxConnections);
            }
        }

        #endregion

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor por defecto de la clase.
        /// </summary>
        /// <param name="iNumMaxConnections">Número máximo de conexiones.</param>
        /// <param name="eDBType">Tipo de Base de Datos.</param>
        public CADbDataConnectionEventArgs(int iNumMaxConnections, DBType eDBType)
        {
            this.guidIdentifier = null;
            this.guidApplicationID = null;
            this.sConnectionString = string.Empty;
            this.iNumMaxConnections = iNumMaxConnections;
            this.eDBType = eDBType;
            this.iNumConnections = 0;
            if (htConnections == null) htConnections = new Hashtable();
            if (qConnectionsPending == null) qConnectionsPending = new Queue<DbControllExecution>();
        }

        #endregion

        #region Métodos

        #region Valida que no se haya excedido el número de conexiones asignadas a la clave

        /// <summary>
        /// Define la firma del método que se encargará de gestionar la demanda de una nueva conexión.
        /// </summary>
        /// <param name="dbkaApplication">Clave a tratar.</param>
        /// <param name="oDbDataConnection">Datos de la conexión con la que realizar la operación.</param>
        /// <param name="mtxOp">Controlador de Exclusión Mútua para realizar esta operación.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>Mirar el enumerado</returns>
        public ResultExceededConnections IsExceededMaxinumNumberOfConnections(DbKeyApp dbkaApplication, 
                                                                              out CADbDataConnection oDbDataConnection,
                                                                              ref Mutex mtxOp, out string sMensaje)
        {
            sMensaje = string.Empty;
            oDbDataConnection = null;
            bool bMutexActivated = false;
            try
            {
                //  Variables.
                    DbControllExecution oControllExecution;

                //  Valida que el número de máximo de conexiones asignadas a esta clave no haya sido superado.
                    bMutexActivated = mtxOp.WaitOne(); //(WAIT_TIME_MUTEX, false);
                    if (this.iNumConnections < this.iNumMaxConnections)
                    {
                        this.iNumConnections++;
                        mtxOp.ReleaseMutex();
                        return (ResultExceededConnections.NotExceeded);
                    }
                    else
                    {
                        qConnectionsPending.Enqueue(oControllExecution = new DbControllExecution(dbkaApplication, Guid.NewGuid()));
                        mtxOp.ReleaseMutex();
                        oControllExecution.Esperar();
                        bMutexActivated = mtxOp.WaitOne(); //(WAIT_TIME_MUTEX, false);
                        oDbDataConnection = (CADbDataConnection)htConnections[oControllExecution.Connection];
                        htConnections.Remove(oControllExecution.Connection);
                        mtxOp.ReleaseMutex();
                        return (ResultExceededConnections.MaxiumNumberOfConnectionsExceeded);
                    }
            }
            catch (Exception ex)
            {
                if (bMutexActivated) mtxOp.ReleaseMutex();
                sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnectionEventArgs_001", ex);
                return (ResultExceededConnections.Error);
            }
        }

        #endregion

        #region Ejecuta una conexión bloqueada si hay alguna
    
        /// <summary>
        /// Define la firma del método que se encargará de gestionar la demanda de una nueva conexión.
        /// </summary>
        /// <param name="dbkaApplication">Clave a tratar</param>
        /// <param name="oDbDataConnection">Conexión a manipular.</param>
        /// <param name="mtxOp">Semáforo de exclusión mútua con el que se asegura la independencia de las operaciones.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>Mirar el enumerado</returns>
        public ResultConnectionAsgined AsignConnection(DbKeyApp dbkaApplication, 
                                                       CADbDataConnection oDbDataConnection,
                                                       ref Mutex mtxOp, out string sMensaje)
        {
            sMensaje = string.Empty;
            bool bMutexActivated = false;
            try
            {
                //  Variables.
                    DbControllExecution oControllExecution;

                //  Si es el caso asigna la conexión libre a la consulta que está esperando.
                    bMutexActivated = mtxOp.WaitOne(); //(WAIT_TIME_MUTEX, false);
                    if (qConnectionsPending.Count > 0)
                    {
                        oControllExecution = qConnectionsPending.Dequeue();
                        htConnections.Add(oControllExecution.Connection, oDbDataConnection);
                        oControllExecution.Continuar();
                        return (ResultConnectionAsgined.QueueConnectionAsigned);
                    }
                    else
                    {
                        this.iNumConnections--;
                        return (ResultConnectionAsgined.QueueEmpty);
                    }
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnectionEventArgs_001", ex);
                return (ResultConnectionAsgined.Error);
            }
            finally
            {
                if (bMutexActivated) mtxOp.ReleaseMutex();
            }
        }

        #endregion

        #region Ejecuta una conexión bloqueada si hay alguna

        /// <summary>
        /// Define la firma del método que se encargará de vaciar la cola de conexiones.
        /// </summary>
        /// <param name="dbkaApplication">Clave a tratar</param>
        /// <param name="mtxOp">Semáforo de exclusión mútua con el que se asegura la independencia de las operaciones.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>Mirar el enumerado</returns>
        public ResultOpBD ClearPendingConnections(DbKeyApp dbkaApplication, ref Mutex mtxOp, out string sMensaje)
        {
            sMensaje = string.Empty;
            bool bMutexActivated = false;
            try
            {
                //  Inicia el controlador de exclusión mútua.
                    bMutexActivated = mtxOp.WaitOne(); //(WAIT_TIME_MUTEX, false);
                //  Limpia la cola de conexiones.
                    this.iNumConnections -= qConnectionsPending.Count;
                    qConnectionsPending.Clear();
                //  Retorna operación correcta.
                    return (ResultOpBD.Correct);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ErrorMsg("MSG_CADbDataConnectionEventArgs_001", ex);
                return (ResultOpBD.Error);
            }
            finally
            {
                if (bMutexActivated) mtxOp.ReleaseMutex();
            }
        }

        #endregion

        #endregion
    }
}
