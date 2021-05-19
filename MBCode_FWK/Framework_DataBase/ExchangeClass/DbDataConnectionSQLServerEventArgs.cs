#region Librerias usadas por la clase

using System.Data.SqlClient;

#endregion

namespace MBCode.Framework.DataBase
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou
    /// Fecha última modificación: 14/10/2008
    /// Descripción: clase de intercambio de datos que define los parámetros que se necesitan para almacenar 
    ///              los datos necesarios de una conexión.
    /// </summary>
    internal class DbDataConnectionSQLServerEventArgs : CADbDataConnectionEventArgs
    {
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
        /// Obtiene/Establece el nombre del Servidor que contiene la Base de Datos con la que se debe conectar y 
        /// la instáncia de la Base de Datos que se usará.
        /// </summary>
        public string DataSource
        {
            get
            {
                return (this.sDataSource);
            }
            set
            {
                this.sDataSource = value;
            }
        }

        /// <summary>
        /// Obtiene/Establece el nombre de la Base de Datos a la que se debe conectar.
        /// </summary>
        public string InitialCatalog
        {
            get
            {
                return (this.sInitialCatalog);
            }
            set
            {
                this.sInitialCatalog = value;
            }
        }

        /// <summary>
        /// Obtiene/Establece el nombre de la Base de Datos a la que se debe conectar si falla la primera.
        /// </summary>
        public string InitialCatalogMirroring
        {
            get
            {
                return (this.sInitialCatalogMirroring);
            }
            set
            {
                this.sInitialCatalogMirroring = value;
            }
        }

        #endregion

        #region DatosConexion

        /// <summary>
        /// Obtiene/Establece el Command definido al realizar la inicialización y usado por todo el programa.
        /// </summary>
        public SqlCommand Command
        {
            get
            {
                return (this.oSqlCommand);
            }
            set
            {
                this.oSqlCommand = value;
            }
        }

        /// <summary>
        /// Obtiene/Establece al conexión definida al realizar la inicialización usada por todo el programa.
        /// </summary>
        public SqlConnection Connection
        {
            get
            {
                return (this.oSqlConnection);
            }
            set
            {
                this.oSqlConnection = value;
            }
        }

        #endregion

        #region Seguridad

        /// <summary>
        /// Obtiene/Establece el tipo de autentificación que usara
        /// </summary>
        public AuthenticationType TipoAutenticacion
        {
            get
            {
                return (this.eTipoAutenticacion);
            }
            set
            {
                this.eTipoAutenticacion = value;
            }
        }

        /// <summary>
        /// Obtiene/Establece el nombre de usuario con el que la conexión puede acceder a la Base de Datos.
        /// </summary>
        public string User
        {
            get
            {
                return (this.sUser);
            }
            set
            {
                this.sUser = value;
            }
        }

        /// <summary>
        /// Obtiene/Establece la contraseña asociada al nombre de usuario con el que la conexión puede acceder  a 
        /// la Base de Datos.
        /// </summary>
        public string Password
        {
            get
            {
                return (this.sPassword);
            }
            set
            {
                this.sPassword = value;
            }
        }

        #endregion

        #endregion

        #region Constructores

        /// <summary>
        /// Constructor por defecto de la clase
        /// </summary>
        /// <param name="iNumMaxConnections">Número máximo de conexiones a Base de Datos</param>
        public DbDataConnectionSQLServerEventArgs(int iNumMaxConnections) : base(iNumMaxConnections, DBType.SQLSERVER)
        {
            this.oSqlCommand = null;
            this.oSqlConnection = null;
            this.sUser = string.Empty;
            this.sPassword = string.Empty;
            this.sInitialCatalog = string.Empty;
            this.sDataSource = string.Empty;
            this.sInitialCatalogMirroring = string.Empty;
            this.eTipoAutenticacion = AuthenticationType.UNDEFINED;
        }

        #endregion
    }
}

