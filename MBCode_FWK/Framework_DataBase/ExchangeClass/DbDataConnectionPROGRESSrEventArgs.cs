#region Librerias usadas por la clase

using System.Data.Odbc;      

#endregion

namespace MBCode.Framework.DataBase
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou
    /// Fecha última modificación: 03/11/2008
    /// Descripción: clase de intercambio de datos que define los parámetros que se necesitan para almacenar 
    ///              los datos necesarios de una conexión.
    /// </summary>
    internal class DbDataConnectionPROGRESSEventArgs : CADbDataConnectionEventArgs
    {
        #region Atributos

        #region Datos Básicos

        /// <summary>
        /// Almacena el nombre del Origen de Datos de Sistema que apunta a la Base de Datos Progress (ODBC).
        /// </summary>
        private string sDSN;

        /// <summary>
        /// Almacena el nombre de la Base de Datos a la que se debe conectar.
        /// </summary>
        private string sInitialCatalog;

        #endregion
        
        #region DatosConexion

        /// <summary>
        /// Command definido al realizar la inicialización y usado por todo el programa.
        /// </summary>
        private OdbcCommand oOdbcCommand;

        /// <summary>
        /// Conexión definida al realizar la inicialización usada por todo el programa.
        /// </summary>
        private OdbcConnection oOdbcConnection;

        #endregion

        #region Seguridad

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
        /// Obtiene/Establece el nombre del Origen de Datos de Sistema que apunta a la Base de Datos Progress (ODBC).
        /// </summary>
        public string DSN
        {
            get
            {
                return (this.sDSN);
            }
            set
            {
                this.sDSN = value;
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

        #endregion

        #region DatosConexion

        /// <summary>
        /// Obtiene/Establece el Command definido al realizar la inicialización y usado por todo el programa.
        /// </summary>
        public OdbcCommand Command
        {
            get
            {
                return (this.oOdbcCommand);
            }
            set
            {
                this.oOdbcCommand = value;
            }
        }

        /// <summary>
        /// Obtiene/Establece al conexión definida al realizar la inicialización usada por todo el programa.
        /// </summary>
        public OdbcConnection Connection
        {
            get
            {
                return (this.oOdbcConnection);
            }
            set
            {
                this.oOdbcConnection = value;
            }
        }

        #endregion

        #region Seguridad

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
        public DbDataConnectionPROGRESSEventArgs(int iNumMaxConnections) : base (iNumMaxConnections, DBType.PROGRESS)
        {
            this.oOdbcCommand = null;
            this.oOdbcConnection = null;
            this.sUser = string.Empty;
            this.sPassword = string.Empty;
            this.sDSN = string.Empty;
            this.sInitialCatalog = string.Empty;
        }

        #endregion
    }
}

