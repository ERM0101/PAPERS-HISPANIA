#region Librerias usadas por esta clase

#endregion

namespace MBCode.Framework.DataBase
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou
    /// Fecha última modificación: 04/05/2008
    /// Descripción: clase que define los enumerados usados por las clases de Bases de Datos de la aplicación. 
    /// </summary>
    #region Enumerados de la Parte de Base de Datos del Framework

    #region Tipo Base de Datos

    /// <summary>
    /// Enumerado que almacena los posibles tipos de Base de Datos con los que trabajará la aplicación.
    /// </summary>
    public enum DBType
    {
        /// <summary>
        /// Indica que la Base de Datos con la que se quiere trabajar es de tipo ORACLE.
        /// </summary>
        ORACLE,

        /// <summary>
        /// Indica que la Base de Datos con la que se quiere trabajar corresponde a una versión de SQL Server
        /// anterior a la 2005.
        /// </summary>
        SQLSERVER,

        ///// <summary>
        ///// Indica que la Base de Datos con la que se quiere trabajar corresponde a SQL Server 2005.
        ///// </summary>
        //SQLSERVER2005,

        ///// <summary>
        ///// Indica que la Base de Datos con la que se quiere trabajar corresponde a la Edición Compacta de SQL 
        ///// Server 2005.
        ///// </summary>
        //SQLCOMPACT2005,

        /// <summary>
        /// Indica que la Base de Datos con la que se quiere trabajar es de tipo PROGRESS y que se
        /// accede a ella mediante un ODBC.
        /// </summary>
        PROGRESS,

        /// <summary>
        /// Indica que no se ha definido el tipo de Base de Datos con la que se quiere trabajar.
        /// </summary>
        UNDEFINED,
    }

    #endregion

    #region Tipo de Autenticación de Seguridad para SQL Server

    /// <summary>
    /// Enumerado que expone los posibles tipos de seguridad para una conexión SQL Server.
    /// </summary>
    public enum AuthenticationType
    {
        /// <summary>
        /// Indica que SQL Server utiliza el usuario y la contraseña de la cuenta con la que el
        /// usuario de la Base de Datos ha iniciado su sesión en Windows.
        /// </summary>
        WINDOWS_AUTHENTICATE,

        /// <summary>
        /// Indica que SQL Server utiliza un usuario y una contraseña propios de SQL Server. 
        /// </summary>
        SQL_SERVER_AUTHENTICATE,

        /// <summary>
        /// Indica que no se ha definido un tipo de autenticación. Esto tiene sentido cuando se
        /// esta trabajando con Bases de Datos que no son SQL Server.
        /// </summary>
        UNDEFINED,
    }

    #endregion

    #region Resultado de la realización de una operación

    /// <summary>
    /// Enumerado que define los posibles resultados de las operaciones con las conexiones de la Base de Datos.
    /// </summary>
    public enum ResultOpBD
    {
        /// <summary>
        /// Indica que la operación se ha realizado de manera correcta.
        /// </summary>
        Correct,

        /// <summary>
        /// Indica que los errores producidos pueden pasarse por alto.
        /// </summary>
        Warning,

        /// <summary>
        /// Indica que se ha producido un error en los parámetros de entrada de la operación.
        /// </summary>
        DataInputError,

        /// <summary>
        /// Error interno de la ejecución de la operación que no se puede reseolver desde el método invocador.
        /// </summary>
        InternalError,

        /// <summary>
        /// Indica que se ha producido un error en el momento de buscar una conexión.
        /// </summary>
        Error,

        /// <summary>
        /// Indica que no ha sido inicializado.
        /// </summary>
        UNDEFINED,
    }

    #endregion

    #region Resultado de pedir o devolver una conexión.

    /// <summary>
    /// Define los posibles resultados del proceso de petición de una nueva conexión.
    /// </summary>
    internal enum ResultExceededConnections
    {
        /// <summary>
        /// Indica que ya se han dado todas las conexiones que se podian dar.
        /// </summary>
        MaxiumNumberOfConnectionsExceeded,

        /// <summary>
        /// Indica que la operación ha finalizado de manera correcta.
        /// </summary>
        NotExceeded,

        /// <summary>
        /// Indica que se ha producido un error interno durante el proceso de gestión de la petición.
        /// </summary>
        InternalError,

        /// <summary>
        /// Indica que se ha producido un error durante el proceso de gestión de la petición.
        /// </summary>
        Error,
    }

    #endregion

    #region Resultado de asignar una conexión.

    /// <summary>
    /// Define los posibles resultados del proceso de petición de una nueva conexión.
    /// </summary>
    internal enum ResultConnectionAsgined
    {
        /// <summary>
        /// Indica que la conexión libre se ha asignado a un elemento que estaba esperando.
        /// </summary>
        QueueConnectionAsigned,

        /// <summary>
        /// Indica que la cola de consultas en espera estaba vacía.
        /// </summary>
        QueueEmpty,

        /// <summary>
        /// Indica que se ha producido un error interno durante el proceso de asignación.
        /// </summary>
        InternalError,

        /// <summary>
        /// Indica que se ha producido un error durante el proceso de asignación.
        /// </summary>
        Error,
    }

    #endregion

    #endregion
}
