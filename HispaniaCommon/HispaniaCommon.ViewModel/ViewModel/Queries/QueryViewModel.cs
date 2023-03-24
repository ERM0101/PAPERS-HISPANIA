#region Librerias usadas por la clase

using HispaniaCommon.DataAccess;
using MBCode.Framework.Managers;
using MBCode.Framework.Managers.Messages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;

#endregion

namespace HispaniaCommon.ViewModel
{
    #region Query Types

    public enum QueryType
    {
        None,
        Goods,
        Customers,
        CustomerOrders,
        HistoCustomerForData,
        HistoCustomerForDataAndAgent,
        CustomQuery,
        CustomerConformedOrders,
        Customers_Full
    }
    
    #endregion

    public class QueryViewModel
    {
        #region Attributes

        #region Directories and Files Management

        private static string _GetApplicationPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        private static string _GetQueryDirectory = string.Format("{0}\\Consultes\\", _GetApplicationPath);

        #endregion

        #endregion

        #region Properties

        private static string GetQueryDirectory
        {
            get
            {
                if (!Directory.Exists(_GetQueryDirectory)) Directory.CreateDirectory(_GetQueryDirectory);
                return (_GetQueryDirectory);
            }
            set
            {
                if (!Directory.Exists(value)) Directory.CreateDirectory(value);
                _GetQueryDirectory = value;
            }
        }

        #endregion

        #region Singleton Pattern

        /// <summary>
        /// Store the reference at the singleton instance of the class.
        /// </summary>
        private static QueryViewModel instance;

        /// <summary>
        /// Allow the application to access at the reference at the singleton instance of the class.
        /// </summary>
        public static QueryViewModel Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new QueryViewModel();
                }
                return instance;
            }
        }

        /// <summary>
        /// Default builder.
        /// </summary>
        private QueryViewModel()
        {
        }

        #endregion

        #region Public Functions

        public void CreateExcelFromQuery(QueryType queryType, Dictionary<string, object> Params = null)
        {
            switch (queryType)
            {
                case QueryType.Goods:
                case QueryType.Customers:
                case QueryType.Customers_Full:
                case QueryType.CustomerOrders:
                case QueryType.CustomerConformedOrders:
                     CreateExcel(queryType);
                     break;
                case QueryType.HistoCustomerForData:
                case QueryType.HistoCustomerForDataAndAgent:
                     CreateExcel(queryType, Params);
                     break;
                case QueryType.CustomQuery:
                     CreateExcel(queryType, Params);
                     break;                
                case QueryType.None:
                default:
                     throw new ArgumentException("CreateExcelFromQuery: Query no reconocida.");
            }
        }

        public string GetQuerySQL_UI(QueryType queryType, Dictionary<string, object> Params = null)
        {
            return GetQuerySQL(queryType, Params).Replace("FROM", "\r\nFROM").Replace("WHERE", "\r\nWHERE").Replace("ORDER BY", "\r\nORDER BY");
        }

        public DataTable GetDataQuery(QueryType queryType, Dictionary<string, object> Params = null)
        {
            return (HispaniaDataAccess.Instance.GetDataTableFromQuerySQL(GetQuerySQL(queryType, Params)));
        }

        #endregion

        #region Private Functions

        private void CreateExcel(QueryType queryType, Dictionary<string, object> Params = null)
        {
            string QueryTypeValue = queryType.ToString();
            try
            {
                DateTime Now = DateTime.Now;
                string ExcelFileName = string.Format("{0}{1}-{2:0000}_{3:00}_{4:00}-{5:00}_{6:00}.xlsx",
                                                     GetQueryDirectory,
                                                     QueryTypeValue,
                                                     Now.Year, Now.Month, Now.Day, Now.Hour, Now.Minute);
                ExcelManager.ExportToExcel(GetDataQuery(queryType, Params), QueryTypeValue, ExcelFileName);
                Process.Start(ExcelFileName);
            }
            catch (Exception ex)
            {
                MsgManager.ShowMessage(
                    string.Format("Error, al crear l'Excel '{0}'.\r\nDetalls: {1}",
                                  QueryTypeValue,
                                  MsgManager.ExcepMsg(ex)));
            }
        }

        private string GetQuerySQL(QueryType queryType, Dictionary<string, object> Params = null)
        {
            string Query;
            if (queryType != QueryType.CustomQuery)
            {
                Query = HispaniaDataAccess.Instance.GetQueryCustom(queryType.ToString().ToUpper());
                if (string.IsNullOrEmpty(Query))
                {
                    throw new ArgumentException("No s'ha pogut trobar el SQL associat a la consulta.");
                }
                if ((queryType == QueryType.HistoCustomerForData) || (queryType == QueryType.HistoCustomerForDataAndAgent))
                {
                    DateTime DateInit = (DateTime)Params["DateInit"];
                    DateTime DateEnd = (DateTime)Params["DateEnd"];
                    Query = Query.Replace("{DateInit}", $"\'{DateInit.Year:0000}{DateInit.Month:00}{DateInit.Day:00}\'");
                    Query = Query.Replace("{DateEnd}", $"\'{DateEnd.Year:0000}{DateEnd.Month:00}{DateEnd.Day:00}\'");
                }
                if (queryType == QueryType.HistoCustomerForDataAndAgent)
                {
                    List<AgentsView> Agents = (List<AgentsView>)Params["Agents"];
                    if (Agents.Count > 0)
                    {
                        StringBuilder sbAgents = new StringBuilder(" AND (A.Agent_Id IN (");
                        foreach (AgentsView Agent in Agents)
                        {
                            sbAgents.AppendFormat("{0}, ", Agent.Agent_Id);
                        }
                        sbAgents.Remove(sbAgents.Length - 2, 2);
                        sbAgents.Append("))");
                        Query = Query.Replace("{Agents}", sbAgents.ToString());
                    }
                    else
                    {
                        Query = Query.Replace("{Agents}", " AND (0 = 1)");
                    }
                }
            }
            else
            {
                Query = Params["Query"].ToString();
                string Query_Validate = Query.ToUpper();
                if (Query_Validate.Contains("INSERT") || Query_Validate.Contains("UPDATE") || Query_Validate.Contains("DELETE") ||
                    Query_Validate.Contains("DROP") || Query_Validate.Contains("TRUNCATE"))
                {
                    throw new ArgumentException("Error, no es poden dur a terme consultes que modifiquin la Base de Dades amb aquesta opció.\r\n" +
                                                "Només comandes 'SELECT'. Les comandes 'INSERT', 'DELETE', 'UPDATE', 'DROP', 'TRUNCATE'... están prohibides");
                }
            }
            return Query;
        }

        #endregion
    }
}