#region Librerias usadas por la clase

using System;
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
    internal interface IDbDataConnectionEventArgs
    {
        #region Propiedades

        /// <summary>
        /// Obtiene/Establece el identificador asociado a la conexión de la que se almacenan los datos.
        /// </summary>
        Guid? Identifier { get; set; }

        /// <summary>
        /// Obtiene/Establece el identificador de la aplicación que ha iniciado la Conexión.
        /// </summary>
        Guid? ApplicationID { get; set; }

        /// <summary>
        /// Obtiene/Establece la cadena de conexión asociada.
        /// </summary>
        string ConnectionString { get; set; }

        /// <summary>
        /// Obtiene/Establece el tipo de Base de Datos a usar.
        /// </summary>
        DBType DataBaseType { get; set; }

        #endregion

        #region Métodos

        /// <summary>
        /// Define la firma del método que se encargará de gestionar la demanda de una nueva conexión.
        /// </summary>
        /// <param name="dbkaApplication">Clave a tratar.</param>
        /// <param name="oDbDataConnection">Datos de la conexión con la que realizar la operación.</param>
        /// <param name="mtxOp">Controlador de Exclusión Mútua para realizar esta operación.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>Mirar el enumerado</returns>
        ResultExceededConnections IsExceededMaxinumNumberOfConnections(DbKeyApp dbkaApplication,
                                                                       out CADbDataConnection oDbDataConnection,
                                                                       ref Mutex mtxOp, out string sMensaje);

        /// <summary>
        /// Define la firma del método que se encargará de gestionar la demanda de una nueva conexión.
        /// </summary>
        /// <param name="dbkaApplication">Clave a tratar</param>
        /// <param name="oDbDataConnection">Conexión a manipular.</param>
        /// <param name="mtxOp">Semáforo de exclusión mútua</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>Mirar el enumerado</returns>
        ResultConnectionAsgined AsignConnection(DbKeyApp dbkaApplication, CADbDataConnection oDbDataConnection,
                                                ref Mutex mtxOp, out string sMensaje);

        #endregion
    }
}
