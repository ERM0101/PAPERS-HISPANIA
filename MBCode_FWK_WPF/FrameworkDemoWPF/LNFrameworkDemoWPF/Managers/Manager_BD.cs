#region Librerias usadas por la clase

using MBCode.Framework.DataBase.Proxy.DAL;
using MBCode.Framework.Managers.Messages;
using MBCode.FrameworkDemoWFP.InterfazUsuario;
using System;
using System.Collections;
using System.Collections.Generic;

#endregion

namespace MBCode.FrameworkDemoWFP.LogicaNegocio
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha última modificación: 15/03/2012
    /// Descripción: clase que se encarga de interaccionar con los datos de la Base de Datos.
    /// </summary>
    public static class Manager_BD
    {
        internal static void OnEventTryToConnect(string sTypeBD, string sAuthenticationType, string sServer, string sDataBase, 
                                                 string sUser, string sPassword)
        {
            try
            {
                Hashtable htParams = new Hashtable();
                htParams.Add("TipoBD", sTypeBD);
                htParams.Add("AuthenticationSQLServer", sAuthenticationType);
                htParams.Add("NumeroMaximoConexionesConcurrentes", 20);
                htParams.Add("TipoConexionProxyDAL", "DIRECT");
                htParams.Add("DataSourceSQLServer", sServer);
                htParams.Add("InitialCatalogSQLServer", sDataBase);
                htParams.Add("UserSQLServer", sUser);
                htParams.Add("PasswordSQLServer", sPassword);
                string sMensaje = string.Empty;
                if (CommonProxy.Instance.InicializarParametros(htParams, ref sMensaje))
                {
                    if (CommonProxy.Instance.TestConnection(out sMensaje)) Manager_IU.Window_BD.Connection_Accept();
                    else Manager_IU.Window_BD.Connection_Fail(sMensaje);
                }
                else Manager_IU.Window_BD.Connection_Fail(sMensaje);
            }
            catch (Exception ex)
            {
                Manager_IU.Window_BD.Connection_Fail(MsgManager.ExcepMsg(ex));
            }
        }

        internal static void OnEventExecuteQuery(QueriesGroups eGroupSelected, Enum eQuerySelected, Dictionary<string, object> oParams = null)
        {
            try
            {
                if (oParams == null) oParams = MakeDataForTest(eGroupSelected, eQuerySelected);
                if (oParams == null) return;
                switch (eGroupSelected)
                { 
                    //case QueriesGroups.Entity:
                    //     EntityQueries_BD.Instance.ExecuteQuery((EntityQueries)eQuerySelected, oParams);
                    //     break;
                    default:
                         Manager_IU.Window_BD.QueryExecuted_Failed(MsgManager.ErrorMsg("Error, tipo de grupo no reconocido"));
                         break;
                }
            }
            catch (Exception ex)
            {
                Manager_IU.Window_BD.QueryExecuted_Failed(MsgManager.ExcepMsg(ex));
            }
        }

        internal static Dictionary<string, object> MakeDataForTest(QueriesGroups eGroupSelected, Enum eQuerySelected)
        { 
            try
            {
                switch (eGroupSelected)
                { 
                    //case QueriesGroups.Entity:
                    //     return (EntityQueries_BD_Test.Instance.MakeDataForTest((EntityQueries)eQuerySelected));
                    default:
                         Manager_IU.Window_BD.QueryExecuted_Failed(MsgManager.ErrorMsg("Error, al generar los datos de test"));
                         return (null);
                }
            }
            catch (Exception ex)
            {
                Manager_IU.Window_BD.QueryExecuted_Failed(MsgManager.ExcepMsg(ex));
                return (null);
            }
        }
    }   
}
