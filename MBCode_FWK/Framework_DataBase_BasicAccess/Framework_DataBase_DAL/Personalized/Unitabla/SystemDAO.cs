#region Librerias usadas por la clase

using MBCode.Framework.DataBase;
using MBCode.Framework.Managers.DataBase;
using MBCode.Framework.Managers.Messages;
using System;
using System.Data;     

#endregion

namespace MBCode.Framework.DataBase.DAL
{
    /// <summary>
    /// Autor: Generador de Código Automático.
    /// Fecha Última Modificación: 06/03/2012
    /// Descripción: Clase que contiene los métodos de acceso a datos de la tabla Tcc de la Base de Datos.
    /// </summary>
    public static partial class SystemDAO
    {
        #region Métodos

        /// <sumary>
        /// Testea la conexión con la Base de Datos. 
        /// </summary> 
        /// <param name="dbkaApp">Clave que identifica al Cliente que quiere realizar la operación con la Base de Datos.</param>
       /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, operación correcta, false, error.</returns>
        /// </sumary>
        public static bool TestConnection(DbKeyApp dbkaApp, out string sMensaje)
        {
            try
            {
                DBType eDBType; 

                if (ConnectionManager.GetTypeConnection(dbkaApp, out eDBType, out sMensaje) != ResultOpBD.Correct) return (false);
                string sSQL;
                switch (eDBType)
                {
                    case DBType.PROGRESS:
                         sSQL = "SELECT tbl " +
                                "FROM sysprogress.systables " +
                                "WHERE owner ='pub' and tbltype = 't' " +
                                "ORDER BY tbl";
                         break;
                     case DBType.SQLSERVER:
                          sSQL = "SELECT name As tbl " +
                                 "FROM sys.tables union select name from sys.views " +
                                 "ORDER BY name";
                          break;
                     case DBType.ORACLE:
                          sMensaje = MsgManager.LiteralMsg("Error, opción aún no implementada.");
                          return (false);
                     default:
                          sMensaje = MsgManager.LiteralMsg("Error, tipo de Base de Datos no reconocida.");
                          return (false);
                }                
                DataSet ds;
                ConnectionManager.GetDataSetFromSql(dbkaApp, sSQL, false, out ds, out sMensaje);
                return (ds != null);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion
    }
}


