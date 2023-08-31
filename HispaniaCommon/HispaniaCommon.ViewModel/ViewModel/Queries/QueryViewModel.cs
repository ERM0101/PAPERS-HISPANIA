#region Librerias usadas por la clase

using HispaniaCommon.DataAccess;
using HispaniaCommon.DataAccess.Utils;
using HispaniaCommon.ViewModel.ViewModel.Queries;
using HispaniaComptabilitat.Data;
using MBCode.Framework.Managers;
using MBCode.Framework.Managers.Messages;
using MBCode.Framework.OFFICE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Controls.Primitives;
using static HispaniaCommon.DataAccess.Utils.DataTableEX;
using static iTextSharp.text.pdf.AcroFields;
using static iTextSharp.text.pdf.events.IndexEvents;

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
        Customers_Full,
        ProviderConformedOrders,
        Providers,
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
                case QueryType.ProviderConformedOrders:
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


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRow"></typeparam>
        /// <param name="streamData"></param>
        /// <param name="sheetName"></param>
        public void GenerateExcelFromStreamData<TRow>( IEnumerable<TRow> streamData, string sheetName )
        {
            try
            {
                DateTime moment = DateTime.Now;
                string excel_filename = string.Format( "{0}{1}-{2:0000}_{3:00}_{4:00}-{5:00}_{6:00}.xlsx",
                                                     GetQueryDirectory,
                                                     sheetName,
                                                     moment.Year, moment.Month, moment.Day,
                                                     moment.Hour, moment.Minute );

                IEnumerable<ExcelColumnInfo> columns_infos = typeof( TRow ).LoadColumnInfos();

                DataTable data_table = streamData.ToDataTable( columns_infos );

                ExcelManager.ExportToExcel( data_table, sheetName, excel_filename, columns_infos );

                Process.Start( excel_filename );

            } catch(Exception ex)
            {
                MsgManager.ShowMessage(
                    string.Format( "Error, al crear l'Excel '{0}'.\r\nDetalls: {1}",
                                  sheetName,
                                  MsgManager.ExcepMsg( ex ) ) );
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerOrder"></param>
        /// <param name="articleName"></param>
        /// <returns></returns>
        private QueryOrderProviderViewModel CreateQueryOrderProviderViewModel( ProviderOrder providerOrder, string articleName )
        {
            QueryOrderProviderViewModel result = new QueryOrderProviderViewModel()
            {
                ProviderOrderId = providerOrder.ProviderOrder_Id,
                Date = providerOrder.Date,
                According = providerOrder.According,
                PrevisioLliurament = (providerOrder.PrevisioLliurament.HasValue ? providerOrder.PrevisioLliurament.Value : false),
                PrevisioLliuramentData = providerOrder.PrevisioLliuramentData,
                ProviderId = providerOrder.Provider_Id,
                ProviderAlias = (providerOrder.Provider == null ? string.Empty : providerOrder.Provider.Alias),
                Address = providerOrder.Address,
                PostalCode = (providerOrder.PostalCode != null ? providerOrder.PostalCode.Postal_Code : string.Empty),
                City = (providerOrder.PostalCode != null ? providerOrder.PostalCode.City : string.Empty),
                SendTypeDescription = (providerOrder.SendType != null ? providerOrder.SendType.Description : string.Empty),
                TotalAmount = providerOrder.TotalAmount,
                Good = articleName
            };

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="articleId"></param>
        /// <param name="providerId"></param>
        /// <returns></returns>
        public IEnumerable<QueryOrderProviderViewModel> GetQueryOrders( DateTime startDate, 
                                                                        DateTime endDate, 
                                                                        int? articleId, 
                                                                        int? providerId )
        {

            IEnumerable<ProviderOrder> raw_data =
                        HispaniaDataAccess.Instance.GetQueryOrders( startDate, endDate, articleId, providerId );
                        
            foreach( ProviderOrder raw_item in raw_data)
            {
                List<QueryOrderProviderViewModel> items = new List<QueryOrderProviderViewModel>();

                IEnumerable<ProviderOrderMovement> movemients = Enumerable.Empty<ProviderOrderMovement>();

                if( null != raw_item.ProviderOrderMovements && raw_item.ProviderOrderMovements.Any() )
                {
                    if(articleId.HasValue)
                    {
                        if(null != raw_item.ProviderOrderMovements)
                            movemients = raw_item.ProviderOrderMovements.Where( i => i.Good_Id == articleId.Value );
                    }
                    else
                    {
                        if(null != raw_item.ProviderOrderMovements)
                        {
                            movemients = raw_item.ProviderOrderMovements.ToArray();
                        }
                    }

                    foreach(ProviderOrderMovement movement in movemients)
                    {
                        QueryOrderProviderViewModel item = CreateQueryOrderProviderViewModel( raw_item, movement.Description );
                        yield return item;
                    }
                }
                else
                {
                    if(articleId.HasValue == false)
                    {
                        QueryOrderProviderViewModel item = CreateQueryOrderProviderViewModel( raw_item, "" );
                        yield return item;
                    }
                }
            }
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Params"></param>
        /// <returns></returns>
        private string BuildArticleProviderParte( Dictionary<string, object> Params = null )
        {
            string result = "";

            if(null != Params && Params.Count != 0)
            {
                int? article_id = null;
                int? provider_id = null;

                if(Params.TryGetValue( "provider_id", out object value ))
                {
                    if(value is int)
                    {
                        provider_id = (int)value;
                    }
                }

                if(Params.TryGetValue( "article_id", out value ))
                {
                    if(value is int)
                    {
                        article_id = (int)value;
                    }
                }

                if(article_id.HasValue && article_id.Value != 0)
                {
                    result = $" AND HP.Good_Id = {article_id.Value}";
                }

                if( provider_id.HasValue && provider_id.Value != 0 )
                {
                    result += $" AND HP.Provider_Id = {provider_id.Value}";
                }
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryType"></param>
        /// <param name="Params"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
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
                if ( ( queryType == QueryType.HistoCustomerForData) || 
                     ( queryType == QueryType.HistoCustomerForDataAndAgent ) ||
                     ( queryType == QueryType.Providers ) )
                {
                    DateTime DateInit = (DateTime)Params["DateInit"];
                    DateTime DateEnd = (DateTime)Params["DateEnd"];
                    Query = Query.Replace("{DateInit}", $"\'{DateInit.Year:0000}{DateInit.Month:00}{DateInit.Day:00}\'");
                    Query = Query.Replace("{DateEnd}", $"\'{DateEnd.Year:0000}{DateEnd.Month:00}{DateEnd.Day:00}\'");
                    if(queryType == QueryType.Providers)
                    {
                        string article_provider_parte = BuildArticleProviderParte( Params );
                        Query = Query.Replace( "{ArticleProvider}", article_provider_parte );
                    }

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