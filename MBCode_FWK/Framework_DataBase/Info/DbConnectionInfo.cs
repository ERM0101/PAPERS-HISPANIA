#region Librerias usadas por la clase

using MBCode.Framework.Managers.Messages;
using System;

#endregion

namespace MBCode.Framework.DataBase
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha: 27/03/2012
    /// Descripción: almacena la información referente a una conexión a Base de Datos.
    /// </summary>
    public class DbConnectionInfo
    {
        #region Variables privadas

        private DBType m_DbType;

        private string m_User;

        private string m_Password;

        private string m_InitialCatalog;

        private string m_ServerName;

        #endregion 

        #region Propiedades

        public DBType DbType
        {
            get { return m_DbType; }
            set { m_DbType = value; }
        }
        public string User
        {
            get { return m_User; }
            set { m_User = value; }
        }

        public string Password
        {
            get { return m_Password; }
            set { m_Password = value; }
        }

        public string InitialCatalog
        {
            get { return m_InitialCatalog; }
            set { m_InitialCatalog = value; }
        }

        public string ServerName
        {
            get { return m_ServerName; }
            set { m_ServerName = value; }
        }

        public string DSN
        {
            get { return m_ServerName; }
            set { m_ServerName = value; }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Obtiene el ConnectionString asociado a la conexión cuyos datos se almacenan aquí.
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString()
        {
            string sConnectionString;
            switch (m_DbType)
            {
                case DBType.PROGRESS:
                     sConnectionString =
                           string.Format("DSN={0};DB={1};UID={2};PWD={3};", this.DSN, m_InitialCatalog, m_User, m_Password);
                     break; 
                case DBType.SQLSERVER:
                     sConnectionString = 
                           string.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};Integrated Security=False;Connection Timeout = {4};",
                                         m_ServerName, m_InitialCatalog, m_User, m_Password, DbConstants.TIMEOUT_CONNECTION_SQLSERVER);
                     break;
                case DBType.ORACLE:
                     throw new Exception(MsgManager.ErrorMsg("MSG_DbConnectionInfo_000"));
                case DBType.UNDEFINED:
                default:
                     throw new Exception(MsgManager.ErrorMsg("MSG_DbConnectionInfo_001"));
            }
            return (sConnectionString);
        }

        #endregion 
    }
}
