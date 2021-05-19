#region Librerias usadas por esta clase

#endregion

namespace MBCode.Framework.DataBase
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou
    /// Fecha última modificación: 04/05/2008
    /// Descripción: clase que almacena las constantes asociadas a la gestión de las Bases de Datos. 
    /// </summary>
    public static class DbConstants
    {
        #region Constantes

        #region ERRORES

        /// <summary>
        /// Error del Servidor que almacena la Base de Datos en los datos de la conexión.
        /// </summary>
        public const int SERVER_ERROR = 17;

        /// <summary>
        /// Error de Base de Datos en los datos de la conexión.
        /// </summary>
        public const int DATABASE_ERROR = 4060;

        /// <summary>
        /// Error de Usuario o Contraseña en los datos de la conexión.
        /// </summary>
        public const int DATAACCESS_ERROR = 18456;

        #endregion

        #region TIMEOUT's

        /// <summary>
        /// Almacena el valor que indica el tiempo, en segundos, que se espera a que se realice una conexión.
        /// No se usa,  ya que Microsoft no permite canviar su valor que por defecto es de 15 segundos.  Esta
        /// constante se deja como información del tiempo preestablecido por defecto.
        /// </summary>
        public const int TIMEOUT_CONNECTION_SQLSERVER = 15;

        /// <summary>
        /// Almacena el valor que indica el tiempo, en segundos, que se espera a que se realice una conexión.
        /// No se usa,  ya que Microsoft no permite canviar su valor que por defecto es de 15 segundos.  Esta
        /// constante se deja como información del tiempo preestablecido por defecto.
        /// </summary>
        public const int TIMEOUT_CONNECTION_ORACLE = 15;

        /// <summary>
        /// Almacena el valor que indica el tiempo, en segundos, que se espera a que se realice una conexión.
        /// </summary>
        public const int TIMEOUT_CONNECTION_PROGRESS = 15;

        /// <summary>
        /// Almacena el valor que indica el tiempo, en segundos, que se espera a que se realice un comando.
        /// </summary>
        public const int TIMEOUT_COMMAND_SQL_SERVER = 60;

        /// <summary>
        /// Almacena el valor que indica el tiempo, en segundos, que se espera a que se realice un comando.
        /// </summary>
        public const int TIMEOUT_COMMAND_ORACLE = 60;

        /// <summary>
        /// Almacena el valor que indica el tiempo, en segundos, que se espera a que se realice un comando.
        /// </summary>
        public const int TIMEOUT_COMMAND_SQL_PROGRESS = 300;

        #endregion

        #endregion
    }
}
