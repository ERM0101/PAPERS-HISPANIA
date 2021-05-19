#region Librerias usadas por la clase

using System; 

#endregion

namespace MBCode.Framework.DataBase
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha última modificación: 27/02/2012.
    /// Descripción: interfície que define los elementos basicos de los que debe disponer la clase que almacena
    ///              los datos de una conexión.
    /// </summary>
    internal interface IDbDataConnection
    {
        #region Propiedades

        /// <summary>
        /// Obtiene el tipo de Base de Datos con el que trabajar.
        /// </summary>
        DBType TipoBaseDatos { get; }

        /// <summary>
        /// Obtiene el identificador del objeto dentro del controlador de conexiones.
        /// </summary>
        Guid? Identifier { get; }

        /// <summary>
        /// Obtiene el identificador de la clave de aplicación a la que pertenece la conexión.
        /// </summary>
        DbKeyApp DbKeyApplication { get; }

        #endregion
    }
}



