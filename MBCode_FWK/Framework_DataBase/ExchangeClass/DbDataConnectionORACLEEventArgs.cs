#region Librerias usadas por la clase

using System.Data.OracleClient; 

#endregion

#region Directivas de Compilación

#pragma warning disable 0618   // Desactiva el aviso relacionado con los objetos que están deprecated.

#endregion

namespace MBCode.Framework.DataBase
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou
    /// Fecha última modificación: 14/10/2008
    /// Descripción: clase de intercambio de datos que define los parámetros que se necesitan para almacenar 
    ///              los datos necesarios de una conexión.
    /// </summary>
    internal class DbDataConnectionORACLEEventArgs : CADbDataConnectionEventArgs
    {
        #region Atributos

        #region Datos Básicos

        /// <summary>
        /// Almacena el nombre de la Base de Datos a la que se debe conectar.
        /// </summary>
        private string sDataSource;

        /// <summary>
        /// Almacena el nombre de la Base de Datos a la que se debe conectar en caso de fallo de la primera opción.
        /// </summary>
        private string sDataSourceMirroring;

        #endregion
        
        #region DatosConexion

        /// <summary>
        /// Command definido al realizar la inicialización y usado por todo el programa.
        /// </summary>
        private OracleCommand oOracleCommand;

        /// <summary>
        /// Conexión definida al realizar la inicialización usada por todo el programa.
        /// </summary>
        private OracleConnection oOracleConnection;

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
        /// Obtiene/Establece el nombre de la Base de Datos a la que se debe conectar.
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
        /// Obtiene/Establece el nombre de la Base de Datos a la que se debe conectar si falla la primera.
        /// </summary>
        public string DataSourceMirroring
        {
            get
            {
                return (this.sDataSourceMirroring);
            }
            set
            {
                this.sDataSourceMirroring = value;
            }
        }

        #endregion

        #region DatosConexion

        /// <summary>
        /// Obtiene/Establece el Command definido al realizar la inicialización y usado por todo el programa.
        /// </summary>
        public OracleCommand Command
        {
            get
            {
                return (this.oOracleCommand);
            }
            set
            {
                this.oOracleCommand = value;
            }
        }

        /// <summary>
        /// Obtiene/Establece al conexión definida al realizar la inicialización usada por todo el programa.
        /// </summary>
        public OracleConnection Connection
        {
            get
            {
                return (this.oOracleConnection);
            }
            set
            {
                this.oOracleConnection = value;
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
        public DbDataConnectionORACLEEventArgs(int iNumMaxConnections) : base(iNumMaxConnections, DBType.SQLSERVER)
        {
            this.oOracleCommand = null;
            this.oOracleConnection = null;
            this.sUser = string.Empty;
            this.sPassword = string.Empty;
            this.sDataSource = string.Empty;
            this.sDataSourceMirroring = string.Empty;
            this.eTipoAutenticacion = AuthenticationType.UNDEFINED;
        }

        #endregion
    }
}

