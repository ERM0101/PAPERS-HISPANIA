#region Librerias usadas por la clase

using MBCode.Framework.DataBase.Utils;
using MBCode.Framework.Managers.Messages;
using System;
using System.Data;
using System.IO;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

#endregion

namespace MBCode.Framework.DataBase
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou
    /// Fecha última modificación: 28/12/2012
    /// Descripción: clase encargada de la generación del fichero que crea las consultas SQL básicas para la
    ///              tabla de Base de Datos que se indique.
    /// </summary>
    public static class GeneradorObjDAL
    {
        #region Enumerados

        /// <summary>
        /// Enumerado que define los posibles resultados de la creación de la query asociada al Update.
        /// </summary>
        public enum ResultQueryUpdate
        { 
            /// <summary>
            /// Indica que no existen campos en la Tabla de la Base de Datos para la que se crea la consulta que no 
            /// formen parte de la Clave Primaria
            /// </summary>
            NotExistFieldsNonPK,

            /// <summary>
            /// Indica que se han producido errores al realizar la operación.
            /// </summary>
            Error,

            /// <summary>
            /// Indica que la operación ha finalizado correctamente.
            /// </summary>
            Correct,
        }

        /// <summary>
        /// Define los posibles tipos de elemento para el que se puede generar este fichero.
        /// </summary>
        public enum TypeOfElement
        {
            /// <summary>
            /// Indica que la clase se crea para almacenar la información de una Tabla.
            /// </summary>
            Table,

            /// <summary>
            /// Indica que la clase se crea para alacenar la información transaccional de una Tabla.
            /// </summary>
            TransactionTable,

            /// <summary>
            /// Indica que la clase se crea para almacenar la información de un Stored Procedure.
            /// </summary>
            StoredProcedure,

            /// <summary>
            /// Indica que la clase se crea para alacenar la información transaccional de un Stored Procedure.
            /// </summary>
            TransactionStoredProcedure,
        }

        #endregion

        #region Atributos

        /// <summary>
        /// Almacena la(as) libreria(as) a crear.
        /// </summary>
        private static StringBuilder sbLibraries = null;

        /// <summary>
        /// Almacena las directivas de compilación a crear.
        /// </summary>
        private static StringBuilder sbCompilationDirectives = null;

        /// <summary>
        /// Almacena el cuerpo de la clase a crear.
        /// </summary>
        private static StringBuilder sbBody = null;

        /// <summary>
        /// Almacena la región asociada al método GetDataByID.
        /// </summary>
        private static StringBuilder sbSelectOneQuery = null;

        /// <summary>
        /// Almacena la región asociada al método GetAllData.
        /// </summary>
        private static StringBuilder sbSelectAllQuery = null;

        /// <summary>
        /// Almacena la región asociada al método InsertData.
        /// </summary>
        private static StringBuilder sbInsertQuery = null;

        /// <summary>
        /// Almacena la región asociada al método ExecuteStoredProcedure.
        /// </summary>
        private static StringBuilder sbExecuteStoredProcedure = null;

        /// <summary>
        /// Almacena la región asociada al método UpdateData.
        /// </summary>
        private static StringBuilder sbUpdateQuery = null;

        /// <summary>
        /// Almacena la región asociada al método DeleteData.
        /// </summary>
        private static StringBuilder sbDeleteQuery = null;

        #endregion

        #region Métodos

        #region Regiones

        #region Librerias

        /// <summary>
        /// Obtiene la region de librerias
        /// </summary>
        /// <param name="sTableName">Nombre de la tabla de la Base de Datos con el que se está trabajando.</param>
        /// <param name="eTypeOfElement">Tipo de elemento para el que se desea crear el fichero.</param>
        /// <param name="sbFileValue">Texto asociado al fichero.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool GetLibrariesRegion(string sTableName, TypeOfElement eTypeOfElement, ref StringBuilder sbFileValue, out string sMensaje)
        {
            try
            {
                sbLibraries = new StringBuilder();
                sbLibraries.AppendLine("#region Librerias usadas por la clase");
                sbLibraries.AppendLine();
                sbLibraries.AppendLine("using System;                                // Libreria necesaria para usar la clase Exception.");
                sbLibraries.AppendLine("using System.Data;                           // Libreria necesaria para usar la clase DataRow.");
                sbLibraries.AppendLine("using Framework.DataBase;                    // Libreria necesaria para usar la clase CADataTable.");
                sbLibraries.AppendLine("using Integra.BaseDatos.DDL;                 // Libreria necesaria para usar la clase <TAG_TABLE_NAME>Data.");
                sbLibraries.AppendLine("using Framework.DataBase.Utils;              // Libreria necesaria para usar la clase BDUtils.");
                sbLibraries.AppendLine("using Framework.Mensajes.Format;             // Libreria necesaria para usar la clase MsgFormat.");
                if ((eTypeOfElement == TypeOfElement.StoredProcedure) || (eTypeOfElement == TypeOfElement.TransactionStoredProcedure))
                {
                    sbLibraries.AppendLine("using System.Data.Odbc;                      // Libreria necesaria para usar la clase OdbcException.");
                    sbLibraries.AppendLine("using System.Data.Common;                    // Libreria necesaria para usar la clase DbParameter.");
                    sbLibraries.AppendLine("using System.Data.SqlClient;                 // Libreria necesaria para usar la clase SqlParameter.");
                    sbLibraries.AppendLine("using System.Data.OracleClient;              // Libreria necesaria para usar la clase OracleException.");
                }
                sbLibraries.AppendLine();
                sbLibraries.AppendLine("#endregion");
                sMensaje = string.Empty;
                sbFileValue.AppendLine(Regex.Replace(sbLibraries.ToString(), "<TAG_TABLE_NAME>", sTableName));
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Directivas de Compilación

        /// <summary>
        /// Obtiene la región de directivas de compilación.
        /// </summary>
        /// <param name="sbFileValue">Texto asociado al fichero.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool GetCompilationDirectivesRegion(ref StringBuilder sbFileValue, out string sMensaje)
        {
            try
            {
                if (sbCompilationDirectives == null)
                {
                    sbCompilationDirectives = new StringBuilder();
                    sbCompilationDirectives.AppendLine("#region Directivas de Compilación");
                    sbCompilationDirectives.AppendLine();
                    sbCompilationDirectives.AppendLine("#pragma warning disable 0168   // Desactiva el aviso relacionado con las variables definidas y no usadas.");
                    sbCompilationDirectives.AppendLine("#pragma warning disable 0169   // Desactiva el aviso relacionado con las variables definidas y no usadas.");
                    sbCompilationDirectives.AppendLine("#pragma warning disable 0414   // Desactiva el aviso relacionado con las variables definidas y no usadas.");
                    sbCompilationDirectives.AppendLine();
                    sbCompilationDirectives.AppendLine("#endregion");
                }
                sMensaje = string.Empty;
                sbFileValue.AppendLine(sbCompilationDirectives.ToString());
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region SelectOne Query

        /// <summary>
        /// Obtiene la region del Método GetDataByID
        /// </summary>
        /// <param name="eTypeOfElement">Tipo de Elemento a Crear.</param>
        /// <param name="dtSchema">Esquema de la Tabla de la Base de Datos a la que proporciona acceso.</param>
        /// <param name="sTableName">Nombre de la tabla traducido a un valor correcto en C#.</param>
        /// <param name="sbFileValue">Texto asociado al fichero.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool SelectOneQueryRegion(TypeOfElement eTypeOfElement, DataTable dtSchema, string sTableName, ref StringBuilder sbFileValue, out string sMensaje)
        {
            try
            {
                string sHeaderProcedure, sCallOperation, sInfoAdditional;
                switch (eTypeOfElement)
                {
                    case TypeOfElement.Table:
                         sHeaderProcedure = "        public static <TAG_TABLE_NAME>Data GetDataById(<TAG_PRIMARY_KEY_COLUMNS>DbKeyApp dbkaApp, out string sMensaje)";
                         sCallOperation = "                ConnectionManager.GetDataSetFromSql(dbkaApp, sql, false, out ds, out sMensaje);";
                         sInfoAdditional = string.Empty;
                         break;
                    case TypeOfElement.TransactionTable:
                         sHeaderProcedure = "        public static <TAG_TABLE_NAME>Data GetDataById(<TAG_PRIMARY_KEY_COLUMNS>DbKeyApp dbkaApp, Guid guidTransaction, out string sMensaje)";
                         sCallOperation = "                ConnectionManager.GetDataSetFromSql(dbkaApp, guidTransaction, sql, false, out ds, out sMensaje);";
                         sInfoAdditional = "        /// <param name=\"guidTransaction\">Clave que identifica la Transacción sobre la que se quiere realizar la operación con la Base de Datos.</param>\r\n";
                         break;
                    default:
                         sMensaje = MsgManager.LiteralMsg("Error, tipo de fichero a crear no reconocido.");
                         return (false);
                }
                sbSelectOneQuery = new StringBuilder();
                sbSelectOneQuery.AppendLine(@"        /// <sumary>");
                sbSelectOneQuery.AppendLine(@"        /// Obtiene los datos del registro de la Tabla <TAG_TABLE_NAME> de la Base de Datos asociado al Identificador");
                sbSelectOneQuery.AppendLine(@"        /// pasado cómo parámetro. ");
                sbSelectOneQuery.AppendLine(@"        /// </summary> ");
                sbSelectOneQuery.AppendLine("        /// <param name=\"dbkaApp\">Clave que identifica al Cliente que quiere realizar la operación con la Base de Datos.</param>");
                sbSelectOneQuery.Append(sInfoAdditional);
                sbSelectOneQuery.AppendLine("        /// <param name=\"sMensaje\">Mensaje de error, si se produce uno.</param>");
                sbSelectOneQuery.AppendLine(@"        /// <returns>true, operación correcta, false, error.</returns>");
                sbSelectOneQuery.AppendLine(@"        /// </sumary>");
                sbSelectOneQuery.AppendLine(sHeaderProcedure);
                sbSelectOneQuery.AppendLine("        {");
                sbSelectOneQuery.AppendLine("            try");
                sbSelectOneQuery.AppendLine("            {");
                sbSelectOneQuery.AppendLine("                DBType eDBType; ");
                sbSelectOneQuery.AppendLine("                string sPrefixTable;");
                sbSelectOneQuery.AppendLine();
                sbSelectOneQuery.AppendLine("                if (ConnectionManager.GetTypeConnection(dbkaApp, out eDBType, out sMensaje) != ResultOpBD.Correct) return (null);");
                sbSelectOneQuery.AppendLine("                if ((sPrefixTable = ConnectionManager.ObtenerPrefijoTabla(eDBType, out sMensaje)) == null) return (null);");
                sbSelectOneQuery.Append("                    <TAG_SELECT>");
                sbSelectOneQuery.AppendLine("                DataSet ds;");
                sbSelectOneQuery.AppendLine(sCallOperation);
                sbSelectOneQuery.AppendLine("                if (ds != null)");
                sbSelectOneQuery.AppendLine("                {");
                sbSelectOneQuery.AppendLine("                    if (ds.Tables.Count > 0) ");
                sbSelectOneQuery.AppendLine("                    {");
                sbSelectOneQuery.AppendLine("                        <TAG_TABLE_NAME>Data o<TAG_TABLE_NAME>Data = new <TAG_TABLE_NAME>Data();");
                sbSelectOneQuery.AppendLine("                        if (ds.Tables[0].Rows.Count > 0) o<TAG_TABLE_NAME>Data.GetFromDataRow(ds.Tables[0].Rows[0], out sMensaje);");
                sbSelectOneQuery.AppendLine("                        return (o<TAG_TABLE_NAME>Data);");
                sbSelectOneQuery.AppendLine("                    }");
                sbSelectOneQuery.AppendLine("                    else return (null);");
                sbSelectOneQuery.AppendLine("                }");
                sbSelectOneQuery.AppendLine("                return (null);");
                sbSelectOneQuery.AppendLine("            }");
                sbSelectOneQuery.AppendLine("            catch (Exception ex)");
                sbSelectOneQuery.AppendLine("            {");
                sbSelectOneQuery.AppendLine("                sMensaje = MsgFormat.MakeErrorID(ex.Message);");
                sbSelectOneQuery.AppendLine("                return (null);");
                sbSelectOneQuery.AppendLine("            }");
                sbSelectOneQuery.AppendLine("        }");
                sMensaje = string.Empty;
                if (dtSchema.PrimaryKey.GetLength(0) == 0)
                {
                    sMensaje = MsgManager.LiteralMsg("Error, la tabla para la que se quiere crear la consulta no tiene clave primaria.");
                    return(false);
                }
                bool bTagPrimaryKeys = true;
                StringBuilder sbTagPrimaryKeyColumns = new StringBuilder(string.Empty);
                foreach (DataColumn dcSchema in dtSchema.PrimaryKey)
                {
                    if (!GetPrimaryKeyParamBBDD(dcSchema.DataType, dcSchema.ColumnName, ref sbTagPrimaryKeyColumns, out sMensaje))
                    {
                        bTagPrimaryKeys = false;
                        break;
                    }
                }
                if (!bTagPrimaryKeys) return (false);
                StringBuilder sbSelect;
                if (!GetSelectValue(dtSchema, sTableName, out sbSelect, out sMensaje)) return (false);
                sbFileValue.Append(
                       Regex.Replace(
                             Regex.Replace(
                                   Regex.Replace(sbSelectOneQuery.ToString(), "<TAG_TABLE_NAME>", sTableName),
                                   "<TAG_PRIMARY_KEY_COLUMNS>", sbTagPrimaryKeyColumns.ToString()),
                              "                    <TAG_SELECT>", sbSelect.ToString()));
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region SelectAll Query

        /// <summary>
        /// Obtiene la region del Método GetAllData
        /// </summary>
        /// <param name="eTypeOfElement">Tipo de Elemento a Crear.</param>
        /// <param name="dtSchema">Esquema de la Tabla de la Base de Datos a la que proporciona acceso.</param>
        /// <param name="sTableName">Nombre de la tabla traducido a un valor correcto en C#.</param>
        /// <param name="sbFileValue">Texto asociado al fichero.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool SelectAllQueryRegion(TypeOfElement eTypeOfElement, DataTable dtSchema, string sTableName, ref StringBuilder sbFileValue, out string sMensaje)
        {
            try
            {
                string sHeaderProcedure, sCallOperation, sInfoAdditional;
                switch (eTypeOfElement)
                {
                    case TypeOfElement.Table:
                         sHeaderProcedure = "        public static DataSet GetAllData(DbKeyApp dbkaApp, out string sMensaje)";
                         sCallOperation = "                ConnectionManager.GetDataSetFromSql(dbkaApp, sql, false, out ds, out sMensaje);";
                         sInfoAdditional = string.Empty;
                         break;
                    case TypeOfElement.TransactionTable:
                         sHeaderProcedure = "        public static DataSet GetAllData(DbKeyApp dbkaApp, Guid guidTransaction, out string sMensaje)";
                         sCallOperation = "                ConnectionManager.GetDataSetFromSql(dbkaApp, guidTransaction, sql, false, out ds, out sMensaje);";
                         sInfoAdditional = "        /// <param name=\"guidTransaction\">Clave que identifica la Transacción sobre la que se quiere realizar la operación con la Base de Datos.</param>\r\n";
                         break;
                    default:
                         sMensaje = MsgManager.LiteralMsg("Error, tipo de fichero a crear no reconocido.");
                         return (false);
                }
                sbSelectAllQuery = new StringBuilder();
                sbSelectAllQuery.AppendLine(@"        /// <sumary>");
                sbSelectAllQuery.AppendLine(@"        /// Obtiene los datos de todos los registros de la Tabla <TAG_TABLE_NAME> de la Base de Datos.");
                sbSelectAllQuery.AppendLine(@"        /// </summary> ");
                sbSelectAllQuery.AppendLine("        /// <param name=\"dbkaApp\">Clave que identifica al Cliente que quiere realizar la operación con la Base de Datos.</param>");
                sbSelectAllQuery.Append(sInfoAdditional);
                sbSelectAllQuery.AppendLine("        /// <param name=\"sMensaje\">Mensaje de error, si se produce uno.</param>");
                sbSelectAllQuery.AppendLine(@"        /// <returns>DataSet con los registros de la tabla, si todo correcto, null, si error.</returns>");
                sbSelectAllQuery.AppendLine(@"        /// </sumary>");
                sbSelectAllQuery.AppendLine(sHeaderProcedure);
                sbSelectAllQuery.AppendLine("        {");
                sbSelectAllQuery.AppendLine("            try");
                sbSelectAllQuery.AppendLine("            {");
                sbSelectAllQuery.AppendLine("                DBType eDBType; ");
                sbSelectAllQuery.AppendLine("                string sPrefixTable;");
                sbSelectAllQuery.AppendLine();
                sbSelectAllQuery.AppendLine("                if (ConnectionManager.GetTypeConnection(dbkaApp, out eDBType, out sMensaje) != ResultOpBD.Correct) return (null);");
                sbSelectAllQuery.AppendLine("                if ((sPrefixTable = ConnectionManager.ObtenerPrefijoTabla(eDBType, out sMensaje)) == null) return (null);");
                sbSelectAllQuery.Append("                    <TAG_SELECT>");
                sbSelectAllQuery.AppendLine("                DataSet ds;");
                sbSelectAllQuery.AppendLine(sCallOperation);
                sbSelectAllQuery.AppendLine("                if ((ds == null) || (ds.Tables.Count <= 0)) return (null);");
                sbSelectAllQuery.AppendLine("                else return (ds);");
                sbSelectAllQuery.AppendLine("            }");
                sbSelectAllQuery.AppendLine("            catch (Exception ex)");
                sbSelectAllQuery.AppendLine("            {");
                sbSelectAllQuery.AppendLine("                sMensaje = MsgFormat.MakeErrorID(ex.Message);");
                sbSelectAllQuery.AppendLine("                return (null);");
                sbSelectAllQuery.AppendLine("            }");
                sbSelectAllQuery.AppendLine("        }");
                sMensaje = string.Empty;
                StringBuilder sbSelect;
                if (!GetSelectAllValues(sTableName, out sbSelect, out sMensaje)) return (false);
                sbFileValue.Append(
                       Regex.Replace(
                             Regex.Replace(sbSelectAllQuery.ToString(), "<TAG_TABLE_NAME>", sTableName),
                             "                    <TAG_SELECT>", sbSelect.ToString()));
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Insert Query
                
        /// <summary>
        /// Obtiene la region del Método Insert
        /// </summary>
        /// <param name="eTypeOfElement">Tipo de Elemento a Crear.</param>
        /// <param name="dtSchema">Esquema de la Tabla de la Base de Datos a la que proporciona acceso.</param>
        /// <param name="sTableName">Nombre de la tabla traducido a un valor correcto en C#.</param>
        /// <param name="sbFileValue">Texto asociado al fichero.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool InsertQueryRegion(TypeOfElement eTypeOfElement, DataTable dtSchema, string sTableName, ref StringBuilder sbFileValue, out string sMensaje)
        {
            try
            {
                string sHeaderProcedure, sCallOperation, sInfoAdditional;
                switch (eTypeOfElement)
                {
                    case TypeOfElement.Table:
                         sHeaderProcedure = "        public static int InsertData(<TAG_TABLE_NAME>Data o<TAG_TABLE_NAME>, DbKeyApp dbkaApp, out string sMensaje)";
                         sCallOperation = "                if (ConnectionManager.ExecuteCommandFromSql(dbkaApp, sql, out iNumRowsAfected, out sMensaje) != ResultOpBD.Correct) return (-1);";
                         sInfoAdditional = string.Empty;
                         break;
                    case TypeOfElement.TransactionTable:
                         sHeaderProcedure = "        public static int InsertData(<TAG_TABLE_NAME>Data o<TAG_TABLE_NAME>, DbKeyApp dbkaApp, Guid guidTransaction, out string sMensaje)";
                         sCallOperation = "                if (ConnectionManager.ExecuteCommandFromSql(dbkaApp, guidTransaction, sql, out iNumRowsAfected, out sMensaje) != ResultOpBD.Correct) return (-1);";
                         sInfoAdditional = "        /// <param name=\"guidTransaction\">Clave que identifica la Transacción sobre la que se quiere realizar la operación con la Base de Datos.</param>\r\n";
                         break;
                    default:
                         sMensaje = MsgManager.LiteralMsg("Error, tipo de fichero a crear no reconocido.");
                         return (false);
                }
                sbInsertQuery = new StringBuilder();
                sbInsertQuery.AppendLine(@"        /// <sumary>");
                sbInsertQuery.AppendLine(@"        /// Inserta un registro en la tabla <TAG_TABLE_NAME> de la Base de Datos la información pasada cómo parámetro. ");
                sbInsertQuery.AppendLine(@"        /// </summary> ");
                sbInsertQuery.AppendLine("        /// <param name=\"dbkaApp\">Clave que identifica al Cliente que quiere realizar la operación con la Base de Datos.</param>");
                sbInsertQuery.Append(sInfoAdditional);
                sbInsertQuery.AppendLine("        /// <param name=\"o<TAG_TABLE_NAME>\">Clase de la que provienen los datos.</param> ");
                sbInsertQuery.AppendLine("        /// <param name=\"sMensaje\">Mensaje de error, si se produce uno.</param>");
                sbInsertQuery.AppendLine(@"        /// <returns>Valor > 0, si todo correcto, -1,  sinó.</returns>");
                sbInsertQuery.AppendLine(@"        /// </sumary>");
                sbInsertQuery.AppendLine(sHeaderProcedure);
                sbInsertQuery.AppendLine("        {");
                sbInsertQuery.AppendLine("            try");
                sbInsertQuery.AppendLine("            {");
                sbInsertQuery.AppendLine("                DBType eDBType; ");
                sbInsertQuery.AppendLine("                string sPrefixTable;");
                sbInsertQuery.AppendLine();
                sbInsertQuery.AppendLine("                if (ConnectionManager.GetTypeConnection(dbkaApp, out eDBType, out sMensaje) != ResultOpBD.Correct) return (-1);");
                sbInsertQuery.AppendLine("                if ((sPrefixTable = ConnectionManager.ObtenerPrefijoTabla(eDBType, out sMensaje)) == null) return (-1);");
                sbInsertQuery.Append("                    <TAG_INSERT>");
                sbInsertQuery.AppendLine("                int iNumRowsAfected;");
                sbInsertQuery.AppendLine(sCallOperation);
                sbInsertQuery.AppendLine("                else return (iNumRowsAfected);");
                sbInsertQuery.AppendLine("            }");
                sbInsertQuery.AppendLine("            catch (Exception ex)");
                sbInsertQuery.AppendLine("            {");
                sbInsertQuery.AppendLine("                sMensaje = MsgFormat.MakeErrorID(ex.Message);");
                sbInsertQuery.AppendLine("                return (-1);");
                sbInsertQuery.AppendLine("            }");
                sbInsertQuery.AppendLine("        }");
                sMensaje = string.Empty;
                StringBuilder sbInsert;
                if (!GetQueryInsert(dtSchema, sTableName, out sbInsert, out sMensaje)) return (false);
                sbFileValue.Append(
                       Regex.Replace(
                             Regex.Replace(sbInsertQuery.ToString(), "<TAG_TABLE_NAME>", sTableName),
                             "                    <TAG_INSERT>", sbInsert.ToString()));
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Update Query

        /// <summary>
        /// Obtiene la region del Método Update
        /// </summary>
        /// <param name="eTypeOfElement">Tipo de Elemento a Crear.</param>
        /// <param name="dtSchema">Esquema de la Tabla de la Base de Datos a la que proporciona acceso.</param>
        /// <param name="sTableName">Nombre de la tabla traducido a un valor correcto en C#.</param>
        /// <param name="sbFileValue">Texto asociado al fichero.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool UpdateQueryRegion(TypeOfElement eTypeOfElement, DataTable dtSchema, string sTableName, ref StringBuilder sbFileValue, out string sMensaje)
        {
            try
            {
                string sHeaderProcedure, sCallOperation, sInfoAdditional;
                switch (eTypeOfElement)
                {
                    case TypeOfElement.Table:
                         sHeaderProcedure = "        public static int UpdateData(<TAG_TABLE_NAME>Data o<TAG_TABLE_NAME>, DbKeyApp dbkaApp, out string sMensaje)";
                         sCallOperation = "                if (ConnectionManager.ExecuteCommandFromSql(dbkaApp, sql, out iNumRowsAfected, out sMensaje) != ResultOpBD.Correct) return (-1);";
                         sInfoAdditional = string.Empty;
                         break;
                    case TypeOfElement.TransactionTable:
                         sHeaderProcedure = "        public static int UpdateData(<TAG_TABLE_NAME>Data o<TAG_TABLE_NAME>, DbKeyApp dbkaApp, Guid guidTransaction, out string sMensaje)";
                         sCallOperation = "                if (ConnectionManager.ExecuteCommandFromSql(dbkaApp, guidTransaction, sql, out iNumRowsAfected, out sMensaje) != ResultOpBD.Correct) return (-1);";
                         sInfoAdditional = "        /// <param name=\"guidTransaction\">Clave que identifica la Transacción sobre la que se quiere realizar la operación con la Base de Datos.</param>\r\n";
                         break;
                    default:
                         sMensaje = MsgManager.LiteralMsg("Error, tipo de fichero a crear no reconocido.");
                         return (false);
                }
                sbUpdateQuery = new StringBuilder();
                sbUpdateQuery.AppendLine(@"        /// <sumary>");
                sbUpdateQuery.AppendLine(@"        /// Modifica un registro de la tabla <TAG_TABLE_NAME> de la Base de Datos con la información pasada cómo parámetro. ");
                sbUpdateQuery.AppendLine(@"        /// </summary> ");
                sbUpdateQuery.AppendLine("        /// <param name=\"dbkaApp\">Clave que identifica al Cliente que quiere realizar la operación con la Base de Datos.</param>");
                sbUpdateQuery.Append(sInfoAdditional);
                sbUpdateQuery.AppendLine("        /// <param name=\"o<TAG_TABLE_NAME>\">Clase de la que provienen los datos.</param> ");
                sbUpdateQuery.AppendLine("        /// <param name=\"sMensaje\">Mensaje de error, si se produce uno.</param>");
                sbUpdateQuery.AppendLine(@"        /// <returns>Valor > 0, si todo correcto, -1,  sinó.</returns>");
                sbUpdateQuery.AppendLine(@"        /// </sumary>");
                sbUpdateQuery.AppendLine(sHeaderProcedure);
                sbUpdateQuery.AppendLine("        {");
                sbUpdateQuery.AppendLine("            try");
                sbUpdateQuery.AppendLine("            {");
                sbUpdateQuery.AppendLine("                DBType eDBType; ");
                sbUpdateQuery.AppendLine("                string sSetRegion;");
                sbUpdateQuery.AppendLine("                string sPrefixTable;");
                sbUpdateQuery.AppendLine();
                sbUpdateQuery.AppendLine("                if (ConnectionManager.GetTypeConnection(dbkaApp, out eDBType, out sMensaje) != ResultOpBD.Correct) return (-1);");
                sbUpdateQuery.AppendLine("                if ((sPrefixTable = ConnectionManager.ObtenerPrefijoTabla(eDBType, out sMensaje)) == null) return (-1);");
                sbUpdateQuery.Append("                    <TAG_UPDATE>");
                sbUpdateQuery.AppendLine("                int iNumRowsAfected;");
                sbUpdateQuery.AppendLine(sCallOperation);
                sbUpdateQuery.AppendLine("                else return (iNumRowsAfected);");
                sbUpdateQuery.AppendLine("            }");
                sbUpdateQuery.AppendLine("            catch (Exception ex)");
                sbUpdateQuery.AppendLine("            {");
                sbUpdateQuery.AppendLine("                sMensaje = MsgFormat.MakeErrorID(ex.Message);");
                sbUpdateQuery.AppendLine("                return (-1);");
                sbUpdateQuery.AppendLine("            }");
                sbUpdateQuery.AppendLine("        }");
                sMensaje = string.Empty;
                StringBuilder sbUpdate;
                sbUpdate = new StringBuilder();
                switch (GetQueryUpdate(dtSchema, sTableName, out sbUpdate, out sMensaje))
                {
                    case ResultQueryUpdate.NotExistFieldsNonPK:
                         sbFileValue.AppendLine(@"        // No se crea la consulta ya que no tiene sentido al no existir campos que no.");
                         sbFileValue.AppendLine(@"        // formen parte de la clave primária de la tabla.");
                         return (true);
                    case ResultQueryUpdate.Correct:
                         sbFileValue.Append(
                                Regex.Replace(
                                      Regex.Replace(sbUpdateQuery.ToString(), "<TAG_TABLE_NAME>", sTableName),
                                      "                    <TAG_UPDATE>", sbUpdate.ToString()));
                         return (true);
                    case ResultQueryUpdate.Error:
                    default:
                         return (false);
                }
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Delete Query

        /// <summary>
        /// Obtiene la region del Método DelateData
        /// </summary>
        /// <param name="eTypeOfElement">Tipo de Elemento a Crear.</param>
        /// <param name="dtSchema">Esquema de la Tabla de la Base de Datos a la que proporciona acceso.</param>
        /// <param name="sTableName">Nombre de la tabla traducido a un valor correcto en C#.</param>
        /// <param name="sbFileValue">Texto asociado al fichero.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool DeleteQueryRegion(TypeOfElement eTypeOfElement, DataTable dtSchema, string sTableName, ref StringBuilder sbFileValue, out string sMensaje)
        {
            try
            {
                string sHeaderProcedure, sCallOperation, sInfoAdditional;
                switch (eTypeOfElement)
                {
                    case TypeOfElement.Table:
                         sHeaderProcedure = "        public static int DeleteData(<TAG_TABLE_NAME>Data o<TAG_TABLE_NAME>, DbKeyApp dbkaApp, out string sMensaje)";
                         sCallOperation = "                if (ConnectionManager.ExecuteCommandFromSql(dbkaApp, sql, out iNumRowsAfected, out sMensaje) != ResultOpBD.Correct) return (-1);";
                         sInfoAdditional = string.Empty;
                         break;
                    case TypeOfElement.TransactionTable:
                         sHeaderProcedure = "        public static int DeleteData(<TAG_TABLE_NAME>Data o<TAG_TABLE_NAME>, DbKeyApp dbkaApp, Guid guidTransaction, out string sMensaje)";
                         sCallOperation = "                if (ConnectionManager.ExecuteCommandFromSql(dbkaApp, guidTransaction, sql, out iNumRowsAfected, out sMensaje) != ResultOpBD.Correct) return (-1);";
                         sInfoAdditional = "        /// <param name=\"guidTransaction\">Clave que identifica la Transacción sobre la que se quiere realizar la operación con la Base de Datos.</param>\r\n";
                         break;
                    default:
                         sMensaje = MsgManager.LiteralMsg("Error, tipo de fichero a crear no reconocido.");
                         return (false);
                }
                sbDeleteQuery = new StringBuilder();
                sbDeleteQuery.AppendLine(@"        /// <sumary>");
                sbDeleteQuery.AppendLine(@"        /// Borra el registro de la Tabla <TAG_TABLE_NAME> de la Base de Datos asociado al Identificador");
                sbDeleteQuery.AppendLine(@"        /// pasado cómo parámetro. ");
                sbDeleteQuery.AppendLine(@"        /// </summary> ");
                sbDeleteQuery.AppendLine("        /// <param name=\"dbkaApp\">Clave que identifica al Cliente que quiere realizar la operación con la Base de Datos.</param>");
                sbDeleteQuery.Append(sInfoAdditional);
                sbDeleteQuery.AppendLine("        /// <param name=\"o<TAG_TABLE_NAME>\">Clase de la que provienen los datos.</param> ");
                sbDeleteQuery.AppendLine("        /// <param name=\"sMensaje\">Mensaje de error, si se produce uno.</param>");
                sbDeleteQuery.AppendLine(@"        /// <returns>Valor > 0, si todo correcto, -1,  sinó.</returns>");
                sbDeleteQuery.AppendLine(@"        /// </sumary>");
                sbDeleteQuery.AppendLine(sHeaderProcedure);
                sbDeleteQuery.AppendLine("        {");
                sbDeleteQuery.AppendLine("            try");
                sbDeleteQuery.AppendLine("            {");
                sbDeleteQuery.AppendLine("                DBType eDBType; ");
                sbDeleteQuery.AppendLine("                string sPrefixTable;");
                sbDeleteQuery.AppendLine();
                sbDeleteQuery.AppendLine("                if (ConnectionManager.GetTypeConnection(dbkaApp, out eDBType, out sMensaje) != ResultOpBD.Correct) return (-1);");
                sbDeleteQuery.AppendLine("                if ((sPrefixTable = ConnectionManager.ObtenerPrefijoTabla(eDBType, out sMensaje)) == null) return (-1);");
                sbDeleteQuery.Append("                    <TAG_DELETE>");
                sbDeleteQuery.AppendLine("                int iNumRowsAfected;");
                sbDeleteQuery.AppendLine(sCallOperation);
                sbDeleteQuery.AppendLine("                else return (iNumRowsAfected);");
                sbDeleteQuery.AppendLine("            }");
                sbDeleteQuery.AppendLine("            catch (Exception ex)");
                sbDeleteQuery.AppendLine("            {");
                sbDeleteQuery.AppendLine("                sMensaje = MsgFormat.MakeErrorID(ex.Message);");
                sbDeleteQuery.AppendLine("                return (-1);");
                sbDeleteQuery.AppendLine("            }");
                sbDeleteQuery.AppendLine("        }");
                sMensaje = string.Empty;
                if (dtSchema.PrimaryKey.GetLength(0) == 0)
                {
                    sMensaje = MsgManager.LiteralMsg("Error, la tabla para la que se quiere crear la consulta no tiene clave primaria.");
                    return (false);
                }
                StringBuilder sbDelete;
                if (!GetDeleteValue(dtSchema, sTableName, out sbDelete, out sMensaje)) return (false);
                sbFileValue.Append(
                       Regex.Replace(
                             Regex.Replace(sbDeleteQuery.ToString(), "<TAG_TABLE_NAME>", sTableName),
                             "                    <TAG_DELETE>", sbDelete.ToString()));
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Execute Stored Procedure

        /// <summary>
        /// Obtiene la region del Método ExecuteStoredProcedure
        /// </summary>
        /// <param name="eTypeOfElement">Tipo de Elemento a Crear.</param>
        /// <param name="dtSchema">Esquema de la Tabla de la Base de Datos a la que proporciona acceso.</param>
        /// <param name="sStoredProcedureName">Nombre del Procedimiento Almacenado traducido a un valor correcto en C#.</param>
        /// <param name="sbFileValue">Texto asociado al fichero.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool ExecuteStoredProcedureRegion(TypeOfElement eTypeOfElement, DataTable dtSchema, string sStoredProcedureName, ref StringBuilder sbFileValue, out string sMensaje)
        {
            try
            {
                string sHeaderProcedure, sCallOperation, sInfoAdditional;
                switch (eTypeOfElement)
                {
                    case TypeOfElement.StoredProcedure:
                         sHeaderProcedure = "        public static DataSet ExecuteStoredProcedure(DbKeyApp dbkaApp, <TAG_TABLE_NAME>Data o<TAG_TABLE_NAME>, out string sMensaje)";
                         sCallOperation = "                    ConnectionManager.ExecuteStoredProcedure(dbkaApp, \"<TAG_TABLE_NAME>\", oDbParameter, false, out ds, out sMensaje);";
                         sInfoAdditional = string.Empty;
                         break;
                    case TypeOfElement.TransactionStoredProcedure:
                         sHeaderProcedure = "        public static DataSet ExecuteStoredProcedure(DbKeyApp dbkaApp, Guid guidTransaction, <TAG_TABLE_NAME>Data o<TAG_TABLE_NAME>, out string sMensaje)";
                         sCallOperation = "                    ConnectionManager.ExecuteStoredProcedure(dbkaApp, guidTransaction, \"<TAG_TABLE_NAME>\", oDbParameter, false, out ds, out sMensaje);";
                         sInfoAdditional = "        /// <param name=\"guidTransaction\">Clave que identifica la Transacción sobre la que se quiere realizar la operación con la Base de Datos.</param>\r\n";
                         break;
                    default:
                         sMensaje = MsgManager.LiteralMsg("Error, tipo de fichero a crear no reconocido.");
                         return (false);
                }
                sbExecuteStoredProcedure = new StringBuilder();
                sbExecuteStoredProcedure.AppendLine(@"        /// <sumary>");
                sbExecuteStoredProcedure.AppendLine(@"        /// Método que ejecuta el Procedimiento Almacenado <TAG_TABLE_NAME> de la Base de Datos con la información pasada cómo parámetro. ");
                sbExecuteStoredProcedure.AppendLine(@"        /// </summary> ");
                sbExecuteStoredProcedure.AppendLine("        /// <param name=\"dbkaApp\">Clave asociada al elemento sobre el que se desea ejecutar el procedimiento.</param> ");
                sbExecuteStoredProcedure.AppendLine("        /// <param name=\"o<TAG_TABLE_NAME>\">Clase de la que provienen los datos.</param> ");
                sbExecuteStoredProcedure.AppendLine("        /// <param name=\"sMensaje\">Mensaje de error, si se produce uno.</param>");
                sbExecuteStoredProcedure.AppendLine(@"        /// <returns>Datos de salida del procedimiento, si todo correcto, null,  si error.</returns>");
                sbExecuteStoredProcedure.AppendLine(sHeaderProcedure);
                sbExecuteStoredProcedure.AppendLine("        {");
                sbExecuteStoredProcedure.AppendLine("            sMensaje = string.Empty;");
                sbExecuteStoredProcedure.AppendLine("            try");
                sbExecuteStoredProcedure.AppendLine("            {");
                sbExecuteStoredProcedure.AppendLine("                //  Variables.");
                sbExecuteStoredProcedure.AppendLine("                    DBType eDBType; ");
                sbExecuteStoredProcedure.AppendLine("                    DbParameter[] oDbParameter;");
                sbExecuteStoredProcedure.AppendLine();
                sbExecuteStoredProcedure.AppendLine("                //  Obtiene el tipo de conexión a usar.");
                sbExecuteStoredProcedure.AppendLine("                    if (ConnectionManager.GetTypeConnection(dbkaApp, out eDBType, out sMensaje) != ResultOpBD.Correct) return (null);");
                sbExecuteStoredProcedure.AppendLine("                //  Crea los parámetros con los que se debe ejecutar el Procedimiento Almacenado.");
                sbExecuteStoredProcedure.AppendLine("                    switch (eDBType)");
                sbExecuteStoredProcedure.AppendLine("                    {");
                sbExecuteStoredProcedure.AppendLine("                        case DBType.SQLSERVER: ");
                sbExecuteStoredProcedure.Append("<TAG_SQL_SERVER>");
                sbExecuteStoredProcedure.AppendLine("                             break;");
                sbExecuteStoredProcedure.AppendLine("                        case DBType.PROGRESS:");
                sbExecuteStoredProcedure.AppendLine("                             sMensaje = MsgFormat.MakeErrorID(\"Error, opción aún no implementada.\");");
                sbExecuteStoredProcedure.AppendLine("                             return (null);");
                sbExecuteStoredProcedure.AppendLine("                        case DBType.ORACLE:");
                sbExecuteStoredProcedure.AppendLine("                             sMensaje = MsgFormat.MakeErrorID(\"Error, opción aún no implementada.\");");
                sbExecuteStoredProcedure.AppendLine("                             return (null);");
                sbExecuteStoredProcedure.AppendLine("                        default:");
                sbExecuteStoredProcedure.AppendLine("                             sMensaje = MsgFormat.MakeErrorID(\"Error, tipo de Base de Datos no reconocido.\");");
                sbExecuteStoredProcedure.AppendLine("                             return (null);");
                sbExecuteStoredProcedure.AppendLine("                    }");
                sbExecuteStoredProcedure.AppendLine("                //  Ejecuta el Procedimiento Almacenado.");
                sbExecuteStoredProcedure.AppendLine("                    DataSet ds;");
                sbExecuteStoredProcedure.AppendLine(sCallOperation);
                sbExecuteStoredProcedure.AppendLine("                    return (ds);");
                sbExecuteStoredProcedure.AppendLine("            }");
                sbExecuteStoredProcedure.AppendLine("            catch (OdbcException ex)");
                sbExecuteStoredProcedure.AppendLine("            {");
                sbExecuteStoredProcedure.AppendLine("                sMensaje =");
                sbExecuteStoredProcedure.AppendLine("                     MsgFormat.MakeErrorID(");
                sbExecuteStoredProcedure.AppendLine("                               string.Format(\"Error ({0}) en {1}, {2}\", ex.ErrorCode, ex.Source, ex.Message));");
                sbExecuteStoredProcedure.AppendLine("                return (null);");
                sbExecuteStoredProcedure.AppendLine("            }");
                sbExecuteStoredProcedure.AppendLine("            catch (SqlException ex)");
                sbExecuteStoredProcedure.AppendLine("            {");
                sbExecuteStoredProcedure.AppendLine("                sMensaje =");
                sbExecuteStoredProcedure.AppendLine("                     MsgFormat.MakeErrorID(");
                sbExecuteStoredProcedure.AppendLine("                               string.Format(\"Error ({0}) de tipo {1} en la línea {2} del procedimiento {3} en el Servidor {4}.\",");
                sbExecuteStoredProcedure.AppendLine("                                             ex.ErrorCode, ex.Number, ex.LineNumber, ex.Procedure, ex.Server)) +");
                sbExecuteStoredProcedure.AppendLine("                     MsgFormat.MakeErrorID(");
                sbExecuteStoredProcedure.AppendLine("                               string.Format(\"Método: {0}\\r\\nMensaje: {1}.\", ex.TargetSite.Name, ex.Message));");
                sbExecuteStoredProcedure.AppendLine("                return (null);");
                sbExecuteStoredProcedure.AppendLine("            }");
                sbExecuteStoredProcedure.AppendLine("            catch (OracleException ex)");
                sbExecuteStoredProcedure.AppendLine("            {");
                sbExecuteStoredProcedure.AppendLine("                sMensaje =");
                sbExecuteStoredProcedure.AppendLine("                     MsgFormat.MakeErrorID(");
                sbExecuteStoredProcedure.AppendLine("                               string.Format(\"Error ({0}) en {1}, {2}\", ex.ErrorCode, ex.Source, ex.Message));");
                sbExecuteStoredProcedure.AppendLine("                return (null);");
                sbExecuteStoredProcedure.AppendLine("            }");
                sbExecuteStoredProcedure.AppendLine("            catch (Exception ex)");
                sbExecuteStoredProcedure.AppendLine("            {");
                sbExecuteStoredProcedure.AppendLine("                sMensaje =");
                sbExecuteStoredProcedure.AppendLine("                     MsgFormat.MakeErrorID(");
                sbExecuteStoredProcedure.AppendLine("                               string.Format(\"Error en {0}.{1}, {2}\", ex.Source, ex.TargetSite, ex.Message));");
                sbExecuteStoredProcedure.AppendLine("                return (null);");
                sbExecuteStoredProcedure.AppendLine("            }");
                sbExecuteStoredProcedure.AppendLine("        }");
                sMensaje = string.Empty;
                StringBuilder sbTagSQLServer;
                if (!GetTagSPSQLSERVER(dtSchema, sStoredProcedureName, out sbTagSQLServer, out sMensaje)) return (false);
                sbFileValue.Append(
                       Regex.Replace(
                             Regex.Replace(sbExecuteStoredProcedure.ToString(), "<TAG_SQL_SERVER>", sbTagSQLServer.ToString()),
                             "<TAG_TABLE_NAME>", sStoredProcedureName));
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Cuerpo

        /// <summary>
        /// Obtiene la región asociada al cuerpo de la clase a crear
        /// </summary>
        /// <param name="sNameSpace">NameSpace al que pertenece la clase de Datos.</param>
        /// <param name="sTableName">Nombre de la Tabla de la Base de Datos a la que proporciona acceso.</param>
        /// <param name="sBuilderValue">Valor de la región asociada al constructor de la clase.</param>
        /// <param name="dtSchema">Tabla que contiene el Esquema de la tabla analizada.</param>
        /// <param name="eTypeOfElement">Tipo de Elemento a Crear.</param>
        /// <param name="sbFileValue">Texto asociado al fichero.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool GetBodyRegion(string sNameSpace, string sTableName, string sBuilderValue, 
                                          DataTable dtSchema, TypeOfElement eTypeOfElement,
                                          ref StringBuilder sbFileValue, out string sMensaje)
        {
            sMensaje = string.Empty;
            try
            {
                string sBody;

                if (!CreateFileStructure(eTypeOfElement, ref sbBody, out sMensaje)) return (false);
                sBody = Regex.Replace(
                              Regex.Replace(sbBody.ToString(), "<TAG_NAME_SPACE>", sNameSpace),
                              "<TAG_TABLE_NAME>", sTableName);
                switch (eTypeOfElement)
                {
                    case TypeOfElement.Table:
                    case TypeOfElement.TransactionTable:
                         StringBuilder sbSelectOneQuery = new StringBuilder(string.Empty);
                         if (!SelectOneQueryRegion(eTypeOfElement, dtSchema, sTableName, ref sbSelectOneQuery, out sMensaje)) return (false);
                         StringBuilder sbSelectAllQuery = new StringBuilder(string.Empty);
                         if (!SelectAllQueryRegion(eTypeOfElement, dtSchema, sTableName, ref sbSelectAllQuery, out sMensaje)) return (false);
                         StringBuilder sbInsertQuery = new StringBuilder(string.Empty);
                         if (!InsertQueryRegion(eTypeOfElement, dtSchema, sTableName, ref sbInsertQuery, out sMensaje)) return (false);
                         StringBuilder sbUpdateQuery = new StringBuilder(string.Empty);
                         if (!UpdateQueryRegion(eTypeOfElement, dtSchema, sTableName, ref sbUpdateQuery, out sMensaje)) return (false);
                         StringBuilder sbDeleteQuery = new StringBuilder(string.Empty);
                         if (!DeleteQueryRegion(eTypeOfElement, dtSchema, sTableName, ref sbDeleteQuery, out sMensaje)) return (false);
                         sbFileValue.AppendLine(
                                Regex.Replace(
                                      Regex.Replace(
                                            Regex.Replace(
                                                  Regex.Replace(
                                                        Regex.Replace(sBody,
                                                                      "        <TAG_SELECT_ONE>", sbSelectOneQuery.ToString()),
                                                        "        <TAG_SELECT_ALL>", sbSelectAllQuery.ToString()),
                                                  "        <TAG_INSERT>", sbInsertQuery.ToString()),
                                            "        <TAG_UPDATE>", sbUpdateQuery.ToString()),
                                      "        <TAG_DELETE>", sbDeleteQuery.ToString()));
                         break;
                    case TypeOfElement.StoredProcedure:
                    case TypeOfElement.TransactionStoredProcedure:
                         StringBuilder sbExecuteStoredProcedure = new StringBuilder(string.Empty);
                         if (!ExecuteStoredProcedureRegion(eTypeOfElement, dtSchema, sTableName, ref sbExecuteStoredProcedure, out sMensaje)) return (false);
                         sbFileValue.AppendLine(
                                Regex.Replace(sBody,
                                      "        <TAG_EXECUTE_STORED_PROCEDURE>", sbExecuteStoredProcedure.ToString()));
                         break;
                }
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #region Estructura

        /// <summary>
        /// Método que crea la estructura del fichero a Crear.
        /// </summary>
        /// <param name="eTypeOfElement">Tipo de elemento a crear.</param>
        /// <param name="sbBody">Estructura resultante.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, operación correcta, false, error.</returns>
        private static bool CreateFileStructure(TypeOfElement eTypeOfElement, ref StringBuilder sbBody,
                                                out string sMensaje)
        {
            try
            {
                sbBody = new StringBuilder();
                sbBody.AppendLine("namespace <TAG_NAME_SPACE>");
                sbBody.AppendLine("{");
                sbBody.AppendLine(@"    /// <summary>");
                sbBody.AppendLine(@"    /// Autor: Generador de Código Automático.");
                sbBody.AppendLine(@"    /// Fecha Última Modificación: " + DateTime.Now.ToShortDateString());
                sbBody.AppendLine(@"    /// Descripción: Clase que contiene los métodos de acceso a datos de la tabla <TAG_TABLE_NAME> de la Base de Datos.");
                sbBody.AppendLine(@"    /// </summary>");
                sbBody.AppendLine("    public static partial class <TAG_TABLE_NAME>DAO");
                sbBody.AppendLine("    {");
                sbBody.AppendLine("        #region Métodos");
                sbBody.AppendLine();
                switch (eTypeOfElement)
                {
                    case TypeOfElement.Table:
                    case TypeOfElement.TransactionTable:
                         sbBody.AppendLine("        #region [SELECT]");
                         sbBody.AppendLine();
                         sbBody.Append("        <TAG_SELECT_ONE>");
                         sbBody.AppendLine();
                         sbBody.Append("        <TAG_SELECT_ALL>");
                         sbBody.AppendLine();
                         sbBody.AppendLine("        #endregion");
                         sbBody.AppendLine();
                         sbBody.AppendLine("        #region [INSERT]");
                         sbBody.AppendLine();
                         sbBody.Append("        <TAG_INSERT>");
                         sbBody.AppendLine();
                         sbBody.AppendLine("        #endregion");
                         sbBody.AppendLine();
                         sbBody.AppendLine("        #region [UPDATE]");
                         sbBody.AppendLine();
                         sbBody.Append("        <TAG_UPDATE>");
                         sbBody.AppendLine();
                         sbBody.AppendLine("        #endregion");
                         sbBody.AppendLine();
                         sbBody.AppendLine("        #region [DELETE]");
                         sbBody.AppendLine();
                         sbBody.Append("        <TAG_DELETE>");
                         sbBody.AppendLine();
                         sbBody.AppendLine("        #endregion");
                         break;
                    case TypeOfElement.StoredProcedure:
                    case TypeOfElement.TransactionStoredProcedure:
                         sbBody.AppendLine("        #region [EXECUTE_STORED_PROCEDURE]");
                         sbBody.AppendLine();
                         sbBody.Append("        <TAG_EXECUTE_STORED_PROCEDURE>");
                         sbBody.AppendLine();
                         sbBody.AppendLine("        #endregion");
                         break;
                }
                sbBody.AppendLine();
                sbBody.AppendLine("        #endregion");
                sbBody.AppendLine("    }");
                sbBody.AppendLine("}");
                sMensaje = string.Empty;
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #endregion

        #endregion

        #region Construcción del fichero

        #region Método Principal

        /// <summary>
        /// Método encargado de Construir el fichero deseado.
        /// </summary>
        /// <param name="sFileName">Nombre del fichero a crear.</param>
        /// <param name="sNameSpace">Espacio de Nombres que debe tomar el fichero.</param>
        /// <param name="sUsing">Using que debe tomar el fichero</param>
        /// <param name="dtSchema">Esquema de la tabla de la Base de Datos para la que se creará el fichero.</param>
        /// <param name="eTypeOfElement">Tipo de elemento para el que se desea crear el fichero.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, si  operación correcta, false, si error.</returns>
        public static bool CrearFichero(string sFileName, string sNameSpace, DataTable dtSchema, 
                                        TypeOfElement eTypeOfElement, out string sMensaje)
        {
            try
            {
                string sTableName;
                StringBuilder sbFileValue = new StringBuilder(string.Empty);
                StringBuilder sbBuilderValue = new StringBuilder(string.Empty);
                if (!BDUtils.TableNameCSharp(dtSchema.TableName, out sTableName, out sMensaje)) return (false);
                if (!GetLibrariesRegion(sTableName, eTypeOfElement, ref sbFileValue, out sMensaje)) return (false);
                if (!GetCompilationDirectivesRegion(ref sbFileValue, out sMensaje)) return (false);
                if (!GetBodyRegion(sNameSpace, sTableName, sbBuilderValue.ToString(), dtSchema, eTypeOfElement, ref sbFileValue, out sMensaje)) return (false);
                using (StreamWriter sw = new StreamWriter(sFileName, false))
                {
                    sw.WriteLine(sbFileValue.ToString());
                }
                sMensaje = string.Empty;
                return (true);
            }
            catch (UnauthorizedAccessException)
            {
                sMensaje = MsgManager.LiteralMsg("Acceso denegado.");
                return (false);
            }
            catch (ArgumentException)
            {
                sMensaje = MsgManager.LiteralMsg("El parámetro path está vacío. O bien path contiene el nombre de un dispositivo del sistema (com1, com2, etc.).");
                return (false);
            }
            catch (DirectoryNotFoundException)
            {
                sMensaje = MsgManager.LiteralMsg("La ruta de acceso especificada no es válida como, por ejemplo, una ruta de una unidad no asignada.");
                return (false);
            }
            catch (PathTooLongException)
            {
                sMensaje = MsgManager.LiteralMsg("La ruta de acceso especificada, el nombre de archivo o ambos superan la longitud máxima definida por el sistema. Por ejemplo, en las plataformas basadas en Windows, las rutas de acceso deben ser inferiores a 248 caracteres y los nombres de archivo deben ser inferiores a 260 caracteres.");
                return (false);
            }
            catch (IOException)
            {
                sMensaje = MsgManager.LiteralMsg("Path incluye una sintaxis no correcta o no válida para el nombre de archivo, el nombre de directorio o la etiqueta de volumen.");
                return (false);
            }
            catch (SecurityException)
            {
                sMensaje = MsgManager.LiteralMsg("El llamador no dispone del permiso requerido. ");
                return (false);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Creación del código asociado a los parámetros de las columnas que forman parte de la clave primaria de la tabla

        /// <summary>
        /// Crea el código asociado a los parámetros de las columnas que forman parte de la clave primaria de la tabla.
        /// </summary>
        /// <param name="tTypeColumnIn">Tipo de la columna de la Tabla en la Base de Datos</param>
        /// <param name="sNameColumnIn">Nombre del campo de la Base de Datos</param>
        /// <param name="sTableName">Nombre de la tabla a la que pertenece el valor que se quiere leer.</param>
        /// <param name="sbReadValue">Código que lee el valor del campo indicado.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, si operación correcta, false, si error.</returns>
        private static bool GetPrimaryKeyParamBBDD(Type tTypeColumnIn, string sNameColumnIn, 
                                                   ref StringBuilder sbPrimaryKeyParam, out string sMensaje)
        {
            try
            {
                //  Variables.
                    string sPrefixType;
                    string sTypeNameCSharp;
                    string sFieldNameCSharp;

                //  Test de que el nombre del campo de Base de Datos sea válido.
                    if (!BDUtils.FieldNameCSharp(sNameColumnIn, out sFieldNameCSharp, out sMensaje)) return (false);
                    if (!BDUtils.PrefixDataTypeCSharp(tTypeColumnIn, out sPrefixType, out sMensaje)) return (false);
                    if (!BDUtils.DataTypeCSharp(tTypeColumnIn, out sTypeNameCSharp, out sMensaje)) return (false);
                    sMensaje = string.Empty;
                //  Creación del mensaje a mostrar
                    sbPrimaryKeyParam.Append(string.Format("{0} {1}{2}, ", sTypeNameCSharp, sPrefixType, sFieldNameCSharp));
                    return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Creación del código asociado al Query de la operación GetDataByID

        /// <summary>
        /// Crea el código asociado al Query de la operación GetDataByID.
        /// </summary>
        /// <param name="dtSchema">Tabla de la que se quiere crear la select.</param>
        /// <param name="sTableName">Nombre de la tabla de la que se quiere crear el select.</param>
        /// <param name="sbSelect">Código que lee el valor del campo indicado.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, si operación correcta, false, si error.</returns>
        private static bool GetSelectValue(DataTable dtSchema, string sTableName, out StringBuilder sbSelect, 
                                           out string sMensaje)
        {
            sMensaje = string.Empty;
            sbSelect = new StringBuilder(string.Empty);
            try
            {
                //  Variables.
                    string sQuota;
                    string sPrefixType;
                    string sFieldNameCSharp;
                    string sIndent = "                ";

                //  Creación de los elementos que forman parte de la clave primaria.
                    int iNumKey = 0;
                    StringBuilder sbWhere = new StringBuilder(string.Empty);
                    foreach (DataColumn dcSchema in dtSchema.PrimaryKey)
                    {
                        if (!BDUtils.FieldNameCSharp(dcSchema.ColumnName, out sFieldNameCSharp, out sMensaje)) return (false);
                        if (!BDUtils.PrefixDataTypeCSharp(dcSchema.DataType, out sPrefixType, out sMensaje)) return (false);
                        if (!BDUtils.GetQuotaTypeCSharp(dcSchema.DataType, out sQuota, out sMensaje)) return (false);
                        switch (dcSchema.DataType.Name.ToString().ToLower())
                        { 
                            case "datetime":
                                 sbSelect.AppendLine(
                                          string.Format("{0}string s{1}Temp;", sIndent, sFieldNameCSharp));
                                 sbSelect.AppendLine(
                                          string.Format("{0}if (!BDUtils.GetDate(eDBType, {2}{1}, BDUtils.FormatDateTime.DateAndTime, out s{1}Temp, out sMensaje)) return (null);", 
                                                       sIndent, sFieldNameCSharp, sPrefixType));
                                 sbWhere.Append(
                                      string.Format("\"(\" + Tables.Schema{0}Table.F{1} + \" = {2}\" + s{1}Temp + \"{2})\"",
                                                    sTableName, sFieldNameCSharp, sQuota));
                                 break;
                            case "string":
                                 sbWhere.Append(
                                      string.Format("\"(\" + Tables.Schema{0}Table.F{1} + \" = {3}\" + {2}{1} + \"{3})\"",
                                                    sTableName, sFieldNameCSharp, sPrefixType, sQuota));
                                 break;
                            default:
                                 sbWhere.Append(
                                      string.Format("\"(\" + Tables.Schema{0}Table.F{1} + \" = {3}\" + {2}{1}.ToString() + \"{3})\"",
                                                    sTableName, sFieldNameCSharp, sPrefixType, sQuota));
                                 break;
                        }
                        iNumKey++;
                        if (iNumKey < dtSchema.PrimaryKey.GetLength(0))
                        {
                            sbWhere.AppendLine(" + \" AND \" +");
                            sbWhere.Append(sIndent + "                           ");
                        }
                        else sbWhere.AppendLine(";");
                    }
                //  Creación del mensaje a mostrar
                    sbSelect.AppendLine(sIndent + "string sql = \"SELECT * \" + ");
                    sbSelect.AppendLine(sIndent + "             \"FROM     \" + sPrefixTable + Tables.Schema" + sTableName + "Table.TableName + \" \" +");
                    sbSelect.Append(sIndent + "             \"WHERE    \" + ");
                    sbSelect.Append(sbWhere.ToString());
                    return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Creación del código asociado al Query de la operación GetAllData

        /// <summary>
        /// Crea el código asociado al Query de la operación GetAllData.
        /// </summary>
        /// <param name="sTableName">Nombre de la tabla de la que se quiere crear el select.</param>
        /// <param name="sbSelect">Código que lee el valor del campo indicado.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, si operación correcta, false, si error.</returns>
        private static bool GetSelectAllValues(string sTableName, out StringBuilder sbSelect, out string sMensaje)
        {
            sMensaje = string.Empty;
            sbSelect = new StringBuilder(string.Empty);
            try
            {
                //  Variables.
                    string sIndent = "                ";

                //  Creación del mensaje a mostrar
                    sbSelect.AppendLine(sIndent + "string sql = \"SELECT * \" + ");
                    sbSelect.AppendLine(sIndent + "             \"FROM     \" + sPrefixTable + Tables.Schema" + sTableName + "Table.TableName;");
                    return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Creación del código asociado al Query de la operación InsertData

        /// <summary>
        /// Crea el código asociado al Query de la operación InsertData.
        /// </summary>
        /// <param name="dtSchema">Tabla de la que se quiere crear la select.</param>
        /// <param name="sTableName">Nombre de la tabla de la que se quiere crear el select.</param>
        /// <param name="sbSelect">Código que lee el valor del campo indicado.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, si operación correcta, false, si error.</returns>
        private static bool GetQueryInsert(DataTable dtSchema, string sTableName, out StringBuilder sbSelect, 
                                           out string sMensaje)
        {
            sMensaje = string.Empty;
            sbSelect = new StringBuilder(string.Empty);
            try
            {
                //  Variables.
                    string sQuota;
                    string sPrefixType;
                    string sFieldNameCSharp;
                    string sIndent = "                ";

                //  Creación de los elementos que forman parte de la clave primaria.
                    int iNumColumn = 0;
                    StringBuilder sbFields = new StringBuilder(string.Empty);
                    foreach (DataColumn dcSchema in dtSchema.Columns)
                    {
                        if (!BDUtils.FieldNameCSharp(dcSchema.ColumnName, out sFieldNameCSharp, out sMensaje)) return (false);
                        if (!BDUtils.PrefixDataTypeCSharp(dcSchema.DataType, out sPrefixType, out sMensaje)) return (false);
                        if (!BDUtils.GetQuotaTypeCSharp(dcSchema.DataType, out sQuota, out sMensaje)) return (false);
                        sbFields.Append(string.Format("Tables.Schema{0}Table.F{1}", sTableName, sFieldNameCSharp));
                        iNumColumn++;
                        if (iNumColumn < dtSchema.Columns.Count)
                        {
                            sbFields.AppendLine(" + \", \" +");
                            sbFields.Append(sIndent + "                              ");
                        }
                        else sbFields.AppendLine(" + \") \"; ");
                    }
                    iNumColumn = 0;
                    StringBuilder sbValues = new StringBuilder(string.Empty);
                //  Creación del mensaje a mostrar
                    sbSelect.AppendLine(sIndent + "string sql = \"INSERT INTO \" + sPrefixTable + Tables.Schema" + sTableName + "Table.TableName + \" \" +");
                    sbSelect.Append(sIndent + "                       \"( \" + ");
                    sbSelect.Append(sbFields.ToString());
                    sbSelect.AppendLine(sIndent + "string sValuesRegion;");
                    sbSelect.AppendLine(sIndent + 
                                        string.Format("if (!o{0}.GetInsertValuesRegion(eDBType, out sValuesRegion, out sMensaje)) return (-1);", sTableName));
                    sbSelect.AppendLine(sIndent + "sql = sql + sValuesRegion;");
                    return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Creación del código asociado al Query de la operación Update

        /// <summary>
        /// Crea el código asociado al Query de la operación Update.
        /// </summary>
        /// <param name="dtSchema">Tabla de la que se quiere crear la select.</param>
        /// <param name="sTableName">Nombre de la tabla de la que se quiere crear el select.</param>
        /// <param name="sbUpdate">Código que lee el valor del campo indicado.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>Mirar el enumerado.</returns>
        private static ResultQueryUpdate GetQueryUpdate(DataTable dtSchema, string sTableName, 
                                                        out StringBuilder sbUpdate, out string sMensaje)
        {
            sMensaje = string.Empty;
            sbUpdate = new StringBuilder(string.Empty);
            try
            {
                //  Variables.
                    string sQuota;
                    string sPrefixType;
                    string sFieldNameCSharp;
                    string sIndent = "                ";

                //  Validación.
                    if (dtSchema.PrimaryKey.GetLength(0) == dtSchema.Columns.Count)
                    {
                        sMensaje = MsgManager.LiteralMsg("Aviso Importante, no se crea la consulta Update ya que no tiene sentido.");
                        return (ResultQueryUpdate.NotExistFieldsNonPK);
                    }
                //  Creación de los elementos que forman parte de la clave primaria.
                    StringBuilder sbSet = new StringBuilder(string.Empty);
                    sbUpdate.AppendLine(sIndent +
                                        string.Format("if (!o{0}.GetUpdateValuesRegion(eDBType, out sSetRegion, out sMensaje)) return (-1);", sTableName));
                    sbSet.AppendLine(sIndent + "             sSetRegion + ");
                    int iNumKey = 0;
                    StringBuilder sbWhere = new StringBuilder(string.Empty);
                    foreach (DataColumn dcSchema in dtSchema.PrimaryKey)
                    {
                        if (!BDUtils.FieldNameCSharp(dcSchema.ColumnName, out sFieldNameCSharp, out sMensaje)) return (ResultQueryUpdate.Error);
                        if (!BDUtils.PrefixDataTypeCSharp(dcSchema.DataType, out sPrefixType, out sMensaje)) return (ResultQueryUpdate.Error);
                        if (!BDUtils.GetQuotaTypeCSharp(dcSchema.DataType, out sQuota, out sMensaje)) return (ResultQueryUpdate.Error);
                        switch (dcSchema.DataType.Name.ToString().ToLower())
                        { 
                            case "datetime":
                                 sbUpdate.AppendLine(
                                          string.Format("{0}string s{1}Temp;", sIndent, sFieldNameCSharp));
                                 sbUpdate.AppendLine(
                                          string.Format("{0}if (!BDUtils.GetDate(eDBType, o{2}.{1}, BDUtils.FormatDateTime.DateAndTime, out s{1}Temp, out sMensaje)) return (-1);", 
                                                       sIndent, sFieldNameCSharp, sTableName));
                                 sbWhere.Append(
                                      string.Format("\"(\" + Tables.Schema{0}Table.F{1} + \" = {2}\" + s{1}Temp + \"{2})\"",
                                                    sTableName, sFieldNameCSharp, sQuota));
                                 break;
                            case "string":
                                 sbWhere.Append(
                                      string.Format("\"(\" + Tables.Schema{0}Table.F{1} + \" = {2}\" + o{0}.{1} + \"{2})\"",
                                                    sTableName, sFieldNameCSharp, sQuota));
                                 break;
                            default:
                                 sbWhere.Append(
                                      string.Format("\"(\" + Tables.Schema{0}Table.F{1} + \" = {2}\" + o{0}.{1}.ToString() + \"{2})\"",
                                                    sTableName, sFieldNameCSharp, sQuota));
                                 break;
                        }
                        iNumKey++;
                        if (iNumKey < dtSchema.PrimaryKey.GetLength(0))
                        {
                            sbWhere.AppendLine(" + \" AND \" +");
                            sbWhere.Append(sIndent + "                           ");
                        }
                        else sbWhere.AppendLine(";");
                    }
                //  Creación del mensaje a mostrar
                    sbUpdate.AppendLine(sIndent + "string sql = \"UPDATE   \" + sPrefixTable + Tables.Schema" + sTableName + "Table.TableName + \" \" +");
                    sbUpdate.Append(sbSet.ToString());
                    sbUpdate.Append(sIndent + "             \"WHERE    \" + ");
                    sbUpdate.Append(sbWhere.ToString());
                    return (ResultQueryUpdate.Correct);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (ResultQueryUpdate.Error);
            }
        }

        #endregion

        #region Creación del código asociado al Query de la operación DeleteData

        /// <summary>
        /// Crea el código asociado al Query de la operación DeleteData.
        /// </summary>
        /// <param name="dtSchema">Tabla de la que se quiere crear la consulta.</param>
        /// <param name="sTableName">Nombre de la tabla de la que se quiere crear el select.</param>
        /// <param name="sbDelete">Código que lee el valor del campo indicado.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, si operación correcta, false, si error.</returns>
        private static bool GetDeleteValue(DataTable dtSchema, string sTableName, out StringBuilder sbDelete, 
                                           out string sMensaje)
        {
            sMensaje = string.Empty;
            sbDelete = new StringBuilder(string.Empty);
            try
            {
                //  Variables.
                    string sQuota;
                    string sPrefixType;
                    string sFieldNameCSharp;
                    string sIndent = "                ";

                //  Creación de los elementos que forman parte de la clave primaria.
                    int iNumKey = 0;
                    StringBuilder sbWhere = new StringBuilder(string.Empty);
                    foreach (DataColumn dcSchema in dtSchema.PrimaryKey)
                    {
                        if (!BDUtils.FieldNameCSharp(dcSchema.ColumnName, out sFieldNameCSharp, out sMensaje)) return (false);
                        if (!BDUtils.PrefixDataTypeCSharp(dcSchema.DataType, out sPrefixType, out sMensaje)) return (false);
                        if (!BDUtils.GetQuotaTypeCSharp(dcSchema.DataType, out sQuota, out sMensaje)) return (false);
                        switch (dcSchema.DataType.Name.ToString().ToLower())
                        { 
                            case "datetime":
                                 sbDelete.AppendLine(
                                          string.Format("{0}string s{1}Temp;", sIndent, sFieldNameCSharp));
                                 sbDelete.AppendLine(
                                          string.Format("{0}if (!BDUtils.GetDate(eDBType, o{2}.{1}, BDUtils.FormatDateTime.DateAndTime, out s{1}Temp, out sMensaje)) return (-1);", 
                                                       sIndent, sFieldNameCSharp, sTableName));
                                 sbWhere.Append(
                                      string.Format("\"(\" + Tables.Schema{0}Table.F{1} + \" = {2}\" + s{1}Temp + \"{2})\"",
                                                    sTableName, sFieldNameCSharp, sQuota));
                                 break;
                            case "string":
                                 sbWhere.Append(
                                      string.Format("\"(\" + Tables.Schema{0}Table.F{1} + \" = {2}\" + o{0}.{1} + \"{2})\"",
                                                    sTableName, sFieldNameCSharp, sQuota));
                                 break;
                            default:
                                 sbWhere.Append(
                                      string.Format("\"(\" + Tables.Schema{0}Table.F{1} + \" = {2}\" + o{0}.{1}.ToString() + \"{2})\"",
                                                    sTableName, sFieldNameCSharp, sQuota));
                                 break;
                        }
                        iNumKey++;
                        if (iNumKey < dtSchema.PrimaryKey.GetLength(0))
                        {
                            sbWhere.AppendLine(" + \" AND \" +");
                            sbWhere.Append(sIndent + "                           ");
                        }
                        else sbWhere.AppendLine(";");
                    }
                //  Creación del mensaje a mostrar
                    sbDelete.AppendLine(sIndent + "string sql = \"DELETE   \" + ");
                    sbDelete.AppendLine(sIndent + "             \"FROM     \" + sPrefixTable + Tables.Schema" + sTableName + "Table.TableName + \" \" +");
                    sbDelete.Append(sIndent + "             \"WHERE    \" + ");
                    sbDelete.Append(sbWhere.ToString());
                    return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Creación del código asociado al Query de la operación ExecuteStoredProcedure

        /// <summary>
        /// Crea el código asociado al Query de la operación ExecuteStoredProcedure.
        /// </summary>
        /// <param name="dtSchema">Tabla de la que se quiere crear la select.</param>
        /// <param name="sTableName">Nombre de la tabla de la que se quiere crear el select.</param>
        /// <param name="sbSelect">Código que lee el valor del campo indicado.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, si operación correcta, false, si error.</returns>
        private static bool GetTagSPSQLSERVER(DataTable dtSchema, string sTableName, out StringBuilder sbSelect, 
                                              out string sMensaje)
        {
            sMensaje = string.Empty;
            sbSelect = new StringBuilder(string.Empty);
            try
            {
                //  Variables.
                    string sIndent = "                             ";

                //  Crea el array de parámetros.
                    sbSelect.AppendLine(sIndent + string.Format("oDbParameter = new SqlParameter[{0}];",dtSchema.Columns.Count));

                //  Creación de los elementos que forman parte de la clave primaria.
                    int iNumColumn = 0;
                    foreach (DataColumn dcSchema in dtSchema.Columns)
                    {
                        if (dtSchema.Rows[0][dcSchema.ColumnName] != DBNull.Value)
                        {
                            sbSelect.AppendLine(
                                     sIndent +
                                     string.Format(
                                            "oDbParameter[{0}] = new SqlParameter(\"@{1}\", SqlDbType.NVarChar, {2});",
                                            iNumColumn, dcSchema.ColumnName, 
                                            int.Parse(dtSchema.Rows[0][dcSchema.ColumnName].ToString())));
                            sbSelect.AppendLine(
                                     sIndent +
                                     string.Format(
                                            "((SqlParameter)oDbParameter[{0}]).Value = o<TAG_TABLE_NAME>.{1};",
                                            iNumColumn, dcSchema.ColumnName));
                        }
                        else
                        {
                            sbSelect.AppendLine(
                                     sIndent +
                                     string.Format(
                                            "oDbParameter[{0}] = new SqlParameter(\"@{1}\", o<TAG_TABLE_NAME>.{1});", 
                                            iNumColumn, dcSchema.ColumnName));
                        }
                        iNumColumn++;
                    }
                    return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
