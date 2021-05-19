#region Librerias usadas por la clase

#endregion

namespace MBCode.Framework.DataBase.Proxy.DAL
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha última modificación: 07/03/2012.
    /// Descripción: contiene las operaciones con Base de Datos básicas para los Plugins.
    /// </summary>
    public class CommonProxy : ProxyBase
    {
        #region Atributos

        /// <summary>
        /// Almacena una instáncia del Proxy.
        /// </summary>
        private static CommonProxy _Instance;

        #endregion

        #region Propiedades

        /// <summary>
        /// Representa una instancia estática de la clase SeguridadProxy. Engloba un pool de conexiones
        /// </summary>
        public static CommonProxy Instance
        {
            get 
            { 
                return (_Instance); 
            }
        }

        /// <summary>
        /// Indica si esta instancia de CommonProxy ha sido inicializada (se ha llamado al metodo Inicializa()).
        /// </summary>
        public bool Inicializado
        {
            get { return _Inicializado; }
        }

        #endregion 

        #region Constructores

        /// <summary>
        /// Constructor estático de la clase que una instancia estática del Proxy que compartiran todos los clientes.
        /// </summary>
        static CommonProxy()
        {
            _Instance = new CommonProxy();            
        }

        /// <summary>
        /// Constructor en el que se indica el tipo de conexión que usará el Proxy.
        /// </summary>
        /// <param name="eTipoConexionProxyDAL">Direct, para conexión directa, ByWCF, a traves de un servidor WCF.</param>
        public CommonProxy(ProxyDALTypeConn eTipoConexionProxyDAL) : base(eTipoConexionProxyDAL)
        {
            // Poner aquí inicializaciones especificas de CommonProxy. Por el momento no hay
        }

        /// <summary>
        /// Constructor por defecto de la clase.
        /// </summary>
        public CommonProxy()
        {
        }

        #endregion 

        #region Consultas

        //#region Tabla TEntidad

        ///// <summary>
        ///// Inserta una Entidad en la Base de Datos.
        ///// </summary>
        ///// <param name="oTentidad">Datos de la entidad que se desea insertar.</param>
        ///// <param name="sMensaje">Mensaje de error, si se produce uno, string.Empty, en caso contrario.</param>
        ///// <returns>número de registros insertados, si operación correcta, -1, si error.</returns>
        //public int InsertEntity(TentidadData oTentidad, out string sMensaje)
        //{
        //    if (AbrirConexion(out sMensaje)) return (TentidadDAO.InsertData(oTentidad, dbkaApp, out sMensaje));
        //    else return (-1);
        //}

        //#endregion

        #endregion
    }
}
