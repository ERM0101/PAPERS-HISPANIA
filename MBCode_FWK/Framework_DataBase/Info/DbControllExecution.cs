#region Librerias usadas por la clase

using System;
using System.Threading;

#endregion

namespace MBCode.Framework.DataBase
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha última modificación: 10/11/2008.
    /// Descripción: Clase que contiene los datos con los que se bloquea la ejecución de un elemento del programa
    ///              a la espera de un evento.
    /// </summary>
    public class DbControllExecution
    {
        #region Atributos

        /// <summary>
        /// Almacena un valor que indica que se ha iniciado el proceso de espera.
        /// </summary>
        private bool bWaitInitied;

        /// <summary>
        /// Almacena el Controlador real de ejecución.
        /// </summary>
        private ManualResetEvent mreDataControll;

        /// <summary>
        /// Almacena la clave a la que pertenece la conexión.
        /// </summary>
        private DbKeyApp dbkaKey;

        /// <summary>
        /// Almacena el identificador de la conexión.
        /// </summary>
        private Guid? guidConnection;

        #endregion

        #region Propiedades

        /// <summary>
        /// Obtiene el Controlador real de ejecución.
        /// </summary>
        public ManualResetEvent MreDataControll
        {
            get
            {
                return (mreDataControll);
            }
        }

        /// <summary>
        /// Obtiene el número de clave al que pertenece la conexión.
        /// </summary>
        public DbKeyApp Key
        {
            get
            {
                return (dbkaKey);
            }
        }

        /// <summary>
        /// Obtiene el número de conexión.
        /// </summary>
        public Guid? Connection
        {
            get
            {
                return (guidConnection);
            }
        }

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor por defecto de la clase.
        /// </summary>
        /// <param name="dbkaKey">Clave a la que pertenece la conexión.</param>
        /// <param name="guidConnection">Identificador de la conexión.</param>
        public DbControllExecution(DbKeyApp dbkaKey, Guid? guidConnection)
        {
            try
            {
                this.dbkaKey = dbkaKey;
                this.guidConnection = guidConnection;
                this.mreDataControll = new ManualResetEvent(this.bWaitInitied = false);
            }
            catch (Exception)
            {
            }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Método encargado de parar la ejecución del programa a la espera de una acción del mismo.
        /// </summary>
        public void Esperar()
        {
            this.bWaitInitied = true;
            this.mreDataControll.WaitOne();
        }

        /// <summary>
        /// Método encargado de continuar con la ejecución bloqueada del programa.
        /// </summary>
        public void Continuar()
        {
            if (this.bWaitInitied) this.mreDataControll.Set();
        }

        #endregion
    }
}
