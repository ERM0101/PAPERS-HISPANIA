#region Librerias usadas por la clase

using MBCode.Framework.DataBase.Utils;
using MBCode.Framework.Managers.Messages;
using System;
using System.Collections.Generic;
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
    /// Descripción: clase encargada de la generación del fichero que define la clase en que se almacenan los
    ///              datos correspondientes a un registro de la tabla indicada de la Base de Datos.
    /// </summary>
    public static class GeneradorObjDDL
    {
        #region Enumerados

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
            /// Indica que la clase se crea para almacenar la información de un Stored Procedure.
            /// </summary>
            StoredProcedure,
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
        /// Almacena la región de los Atributos de la clase a crear.
        /// </summary>
        private static StringBuilder sbAttributes = null;

        /// <summary>
        /// Almacena la región de las Propiedades de la clase a crear.
        /// </summary>
        private static StringBuilder sbProperties = null;

        /// <summary>
        /// Almacena la región asociada al método GetFromDataRow.
        /// </summary>
        private static StringBuilder sbGetFromDataRow = null;

        /// <summary>
        /// Almacena la región asociada al método GetInsertValuesRegion.
        /// </summary>
        private static StringBuilder sbGetInsertValuesRegion = null;

        /// <summary>
        /// Almacena la región asociada al método GetUpdateValuesRegion.
        /// </summary>
        private static StringBuilder sbGetUpdateValuesRegion = null;

        /// <summary>
        /// Almacena la región asociada a la propiedad ValueToString.
        /// </summary>
        private static StringBuilder sbValueToString = null;

        #endregion

        #region Métodos

        #region Regiones

        #region Librerias

        /// <summary>
        /// Obtiene la region de librerias
        /// </summary>
        /// <param name="sbFileValue">Texto asociado al fichero.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool GetLibrariesRegion(ref StringBuilder sbFileValue, out string sMensaje)
        {
            try
            {
                if (sbLibraries == null)
                {
                    sbLibraries = new StringBuilder();
                    sbLibraries.AppendLine("#region Librerias usadas por la clase");
                    sbLibraries.AppendLine();
                    sbLibraries.AppendLine("using System;                                // Libreria necesaria para usar la clase Exception.");
                    sbLibraries.AppendLine("using System.Data;                           // Libreria necesaria para usar la clase DataRow.");
                    sbLibraries.AppendLine("using System.Text;                           // Libreria necesaria para usar la clase StringBuilder.");
                    sbLibraries.AppendLine("using System.Xml.Serialization;              // Libreria necesaria para usar el atributo XmlInclude.");
                    sbLibraries.AppendLine("using Framework.DataBase;                    // Libreria necesaria para usar la clase CADataTable.");
                    sbLibraries.AppendLine("using Framework.DataBase.Utils;              // Libreria necesaria para usar la clase BDUtils.");
                    sbLibraries.AppendLine("using Framework.Mensajes.Format;             // Libreria necesaria para usar la clase MsgFormat.");
                    sbLibraries.AppendLine();
                    sbLibraries.AppendLine("#endregion");
                }
                sMensaje = string.Empty;
                sbFileValue.AppendLine(sbLibraries.ToString());
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

        #region Atributos

        /// <summary>
        /// Obtiene la region de los Atributos
        /// </summary>
        /// <param name="tTypeField">Tipo del campo para el que se quiere .</param>
        /// <param name="sFieldName">Nombre de la columna en la Base de Datos.</param>
        /// <param name="sbFileValue">Variable en la que se almacenan los resultados.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool GetAttributeRegion(Type tTypeField, string sFieldName, 
                                               ref StringBuilder sbFileValue, out string sMensaje)
        {
            try
            {
                if (sbAttributes == null)
                {
                    sbAttributes = new StringBuilder();
                    sbAttributes.AppendLine(@"        /// <sumary>");
                    sbAttributes.AppendLine(@"        /// Almacena el valor del campo '<TAG_FIELD_NAME>' de tipo {<TAG_FIELD_TYPE>}.");
                    sbAttributes.AppendLine(@"        /// </sumary>");
                }
                sMensaje = string.Empty;
                sbFileValue.Append(
                          Regex.Replace(
                                Regex.Replace(sbAttributes.ToString(), "<TAG_FIELD_NAME>", sFieldName),
                                "<TAG_FIELD_TYPE>", tTypeField.ToString()));
                if (!GetAttributeValue(tTypeField, sFieldName, ref sbFileValue, out sMensaje)) return (false);
                sbFileValue.AppendLine();
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Propiedades

        #region Campos

        /// <summary>
        /// Obtiene la region de las Propiedades
        /// </summary>
        /// <param name="tTypeField">Tipo del campo para el que se quiere .</param>
        /// <param name="sFieldName">Nombre de la columna en la Base de Datos.</param>
        /// <param name="sbFileValue">Variable en la que se almacenan los resultados.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool GetPropertyRegion(Type tTypeField, string sFieldName, ref StringBuilder sbFileValue,
                                             out string sMensaje)
        {
            try
            {
                if (sbProperties == null)
                {
                    sbProperties = new StringBuilder();
                    sbProperties.AppendLine(@"        /// <sumary>");
                    sbProperties.AppendLine(@"        /// Obtiene/Establece el valor del campo '<TAG_FIELD_NAME>' de tipo {<TAG_FIELD_TYPE>}.");
                    sbProperties.AppendLine(@"        /// </sumary>");
                }
                sMensaje = string.Empty;
                sbFileValue.Append(
                          Regex.Replace(
                                Regex.Replace(sbProperties.ToString(), "<TAG_FIELD_NAME>", sFieldName),
                                "<TAG_FIELD_TYPE>", tTypeField.ToString()));
                if (!GetPropertyValue(tTypeField, sFieldName, ref sbFileValue, out sMensaje)) return (false);
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Consultoras

        /// <summary>
        /// Obtiene la region de las Propiedades
        /// </summary>
        /// <param name="sbValueToStringInOut">Código de la Propiedad ValueToString.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool GetPropertyValueToStringRegion(DataTable dtSchema, ref StringBuilder sbValueToStringInOut, 
                                                           out string sMensaje)
        {
            try
            {
                if (sbValueToString == null)
                {
                    sbValueToString = new StringBuilder();
                    sbValueToString.AppendLine(@"        /// <sumary>");
                    sbValueToString.AppendLine(@"        /// Propiedad que obtiene una cadena de carácteres con la información contenida en la clase.");
                    sbValueToString.AppendLine(@"        /// </sumary>");
                    sbValueToString.AppendLine(@"        public string ValueToString");
                    sbValueToString.AppendLine(@"        {");
                    sbValueToString.AppendLine(@"            get");
                    sbValueToString.AppendLine(@"            {");
                    sbValueToString.AppendLine(@"                if (EstadoInstancia == InstanceState.DataContains)");
                    sbValueToString.AppendLine(@"                {");
                    sbValueToString.AppendLine(@"                    <TAG_TO_STRING>");
                    sbValueToString.AppendLine(@"                }");
                    sbValueToString.AppendLine(@"                else return (EstadoInstancia.ToString());");
                    sbValueToString.AppendLine(@"            }");
                    sbValueToString.AppendLine(@"            set { ; }");
                    sbValueToString.AppendLine(@"        }");
                }
                sMensaje = string.Empty;
                StringBuilder sbReturnValue;
                if (!CreateReturnValue(dtSchema, out sbReturnValue, out sMensaje)) return (false);
                sbValueToStringInOut.AppendLine(Regex.Replace(sbValueToString.ToString(), "<TAG_TO_STRING>", sbReturnValue.ToString()));
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

        #region Métodos

        #region GetFromDataRow

        /// <summary>
        /// Obtiene la region del Método GetFromDataRow
        /// </summary>
        /// <param name="dtSchema">Esquema de la Tabla de la Base de Datos a la que proporciona acceso.</param>
        /// <param name="sTableName">Nombre de la tabla traducido a un valor correcto en C#.</param>
        /// <param name="sbFileValue">Texto asociado al fichero.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool GetFromDataRowRegion(DataTable dtSchema, string sTableName, ref StringBuilder sbFileValue, out string sMensaje)
        {
            try
            {
                if (sbGetFromDataRow == null)
                {
                    sbGetFromDataRow = new StringBuilder();
                    sbGetFromDataRow.AppendLine(@"        /// <sumary>");
                    sbGetFromDataRow.AppendLine(@"        /// Rellena los atributos de la clase a partir de un registro de la Base de Datos.");
                    sbGetFromDataRow.AppendLine(@"        /// </summary> ");
                    sbGetFromDataRow.AppendLine("        /// <param name=\"dr\">Origen de Datos del que se desea obtener la información.</param>");
                    sbGetFromDataRow.AppendLine("        /// <param name=\"sMensaje\">Mensaje de error, si se produce uno.</param>");
                    sbGetFromDataRow.AppendLine(@"        /// <returns>true, operación correcta, false, error.</returns>");
                    sbGetFromDataRow.AppendLine(@"        /// </sumary>");
                    sbGetFromDataRow.AppendLine("        public bool GetFromDataRow(DataRow dr, out string sMensaje)");
                    sbGetFromDataRow.AppendLine("        {");
                    sbGetFromDataRow.AppendLine("            try");
                    sbGetFromDataRow.AppendLine("            {");
                    sbGetFromDataRow.Append("                <TAG_READERS>");
                    sbGetFromDataRow.AppendLine("                this.EstadoInstancia = InstanceState.DataContains;");
                    sbGetFromDataRow.AppendLine("                return (true);");
                    sbGetFromDataRow.AppendLine("            }");
                    sbGetFromDataRow.AppendLine("            catch (Exception ex)");
                    sbGetFromDataRow.AppendLine("            {");
                    sbGetFromDataRow.AppendLine("                sMensaje = MsgFormat.MakeErrorID(ex.Message);");
                    sbGetFromDataRow.AppendLine("                return (false);");
                    sbGetFromDataRow.AppendLine("            }");
                    sbGetFromDataRow.AppendLine("        }");
                    sbGetFromDataRow.AppendLine();
                }
                sMensaje = string.Empty;
                bool bTagReadersFields = true;
                StringBuilder sbTagReaders = new StringBuilder(string.Empty);
                foreach (DataColumn dcSchema in dtSchema.Columns)
                {
                    if (!GetReadFieldValueBBDD(dcSchema.DataType, dcSchema.ColumnName, sTableName, ref sbTagReaders, out sMensaje))
                    {
                        bTagReadersFields = false;
                        break;
                    }
                }
                if (!bTagReadersFields) return (false);
                sbFileValue.Append(Regex.Replace(sbGetFromDataRow.ToString(), "                <TAG_READERS>", sbTagReaders.ToString()));
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region GetInsertValuesRegion

        /// <summary>
        /// Obtiene la region del Método GetInsertValuesRegion
        /// </summary>
        /// <param name="dtSchema">Esquema de la Tabla de la Base de Datos a la que proporciona acceso.</param>
        /// <param name="sTableName">Nombre de la tabla traducido a un valor correcto en C#.</param>
        /// <param name="sbFileValue">Texto asociado al fichero.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool GetInsertValuesRegion(DataTable dtSchema, string sTableName, ref StringBuilder sbFileValue, out string sMensaje)
        {
            try
            {
                if (sbGetInsertValuesRegion == null)
                {
                    sbGetInsertValuesRegion = new StringBuilder();
                    sbGetInsertValuesRegion.AppendLine(@"        /// <sumary>");
                    sbGetInsertValuesRegion.AppendLine(@"        /// Obtiene el valor del campo VALUES de una consulta Insert.");
                    sbGetInsertValuesRegion.AppendLine(@"        /// </summary> ");
                    sbGetInsertValuesRegion.AppendLine("        /// <param name=\"eDBType\">Tipo de Base de Datos.</param>");
                    sbGetInsertValuesRegion.AppendLine("        /// <param name=\"sValuesRegion\">Sentencia creada.</param>");
                    sbGetInsertValuesRegion.AppendLine("        /// <param name=\"sMensaje\">Mensaje de error, si se produce uno.</param>");
                    sbGetInsertValuesRegion.AppendLine(@"        /// <returns>true, operación correcta, false, error.</returns>");
                    sbGetInsertValuesRegion.AppendLine(@"        /// </sumary>");
                    sbGetInsertValuesRegion.AppendLine("        public bool GetInsertValuesRegion(DBType eDBType, out string sValuesRegion, out string sMensaje)");
                    sbGetInsertValuesRegion.AppendLine("        {");
                    sbGetInsertValuesRegion.AppendLine("            sValuesRegion = string.Empty;");
                    sbGetInsertValuesRegion.AppendLine("            try");
                    sbGetInsertValuesRegion.AppendLine("            {");
                    sbGetInsertValuesRegion.AppendLine("                StringBuilder sbValue = new StringBuilder(\"VALUES (\");");
                    sbGetInsertValuesRegion.AppendLine("                string sValueOut = string.Empty;");
                    sbGetInsertValuesRegion.AppendLine("                ");
                    sbGetInsertValuesRegion.Append("                <TAG_READERS>");
                    sbGetInsertValuesRegion.AppendLine("                sValuesRegion = sbValue.ToString();");
                    sbGetInsertValuesRegion.AppendLine("                return (true);");
                    sbGetInsertValuesRegion.AppendLine("            }");
                    sbGetInsertValuesRegion.AppendLine("            catch (Exception ex)");
                    sbGetInsertValuesRegion.AppendLine("            {");
                    sbGetInsertValuesRegion.AppendLine("                sMensaje = MsgFormat.MakeErrorID(ex.Message);");
                    sbGetInsertValuesRegion.AppendLine("                return (false);");
                    sbGetInsertValuesRegion.AppendLine("            }");
                    sbGetInsertValuesRegion.AppendLine("        }");
                    sbGetInsertValuesRegion.AppendLine();
                }
                int iNumColumnAct = 1;
                sMensaje = string.Empty;
                bool bTagReadersFields = true;
                StringBuilder sbTagReaders = new StringBuilder(string.Empty);
                foreach (DataColumn dcSchema in dtSchema.Columns)
                {
                    if (!GetWriteFieldValueBBDD(dcSchema.DataType, dcSchema.ColumnName, sTableName, 
                                                iNumColumnAct == dtSchema.Columns.Count ,ref sbTagReaders, out sMensaje))
                    {
                        bTagReadersFields = false;
                        break;
                    }
                    iNumColumnAct++;
                }
                if (!bTagReadersFields) return (false);
                sbFileValue.Append(Regex.Replace(sbGetInsertValuesRegion.ToString(), "                <TAG_READERS>", sbTagReaders.ToString()));
                return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region GetUpdateValuesRegion

        /// <summary>
        /// Obtiene la region del Método GetUpdateValuesRegion
        /// </summary>
        /// <param name="dtSchema">Esquema de la Tabla de la Base de Datos a la que proporciona acceso.</param>
        /// <param name="sTableName">Nombre de la tabla traducido a un valor correcto en C#.</param>
        /// <param name="sbFileValue">Texto asociado al fichero.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool GetUpdateValuesRegion(DataTable dtSchema, string sTableName, ref StringBuilder sbFileValue, out string sMensaje)
        {
            try
            {
                if (sbGetUpdateValuesRegion == null)
                {
                    sbGetUpdateValuesRegion = new StringBuilder();
                    sbGetUpdateValuesRegion.AppendLine(@"        /// <sumary>");
                    sbGetUpdateValuesRegion.AppendLine(@"        /// Obtiene los valores del campo Set de una consulta Update.");
                    sbGetUpdateValuesRegion.AppendLine(@"        /// </summary> ");
                    sbGetUpdateValuesRegion.AppendLine("        /// <param name=\"eDBType\">Tipo de Base de Datos.</param>");
                    sbGetUpdateValuesRegion.AppendLine("        /// <param name=\"sValuesRegion\">Sentencia creada.</param>");
                    sbGetUpdateValuesRegion.AppendLine("        /// <param name=\"sMensaje\">Mensaje de error, si se produce uno.</param>");
                    sbGetUpdateValuesRegion.AppendLine(@"        /// <returns>true, operación correcta, false, error.</returns>");
                    sbGetUpdateValuesRegion.AppendLine(@"        /// </sumary>");
                    sbGetUpdateValuesRegion.AppendLine("        public bool GetUpdateValuesRegion(DBType eDBType, out string sValuesRegion, out string sMensaje)");
                    sbGetUpdateValuesRegion.AppendLine("        {");
                    sbGetUpdateValuesRegion.AppendLine("            sValuesRegion = string.Empty;");
                    sbGetUpdateValuesRegion.AppendLine("            try");
                    sbGetUpdateValuesRegion.AppendLine("            {");
                    sbGetUpdateValuesRegion.AppendLine("                StringBuilder sbValue = new StringBuilder(\"SET \");");
                    sbGetUpdateValuesRegion.AppendLine("                string sValueOut = string.Empty;");
                    sbGetUpdateValuesRegion.AppendLine("                ");
                    sbGetUpdateValuesRegion.Append("                <TAG_READERS>");
                    sbGetUpdateValuesRegion.AppendLine("                sValuesRegion = sbValue.ToString();");
                    sbGetUpdateValuesRegion.AppendLine("                return (true);");
                    sbGetUpdateValuesRegion.AppendLine("            }");
                    sbGetUpdateValuesRegion.AppendLine("            catch (Exception ex)");
                    sbGetUpdateValuesRegion.AppendLine("            {");
                    sbGetUpdateValuesRegion.AppendLine("                sMensaje = MsgFormat.MakeErrorID(ex.Message);");
                    sbGetUpdateValuesRegion.AppendLine("                return (false);");
                    sbGetUpdateValuesRegion.AppendLine("            }");
                    sbGetUpdateValuesRegion.AppendLine("        }");
                    sbGetUpdateValuesRegion.AppendLine();
                }
                int iNumColumnAct = 1;
                sMensaje = string.Empty;
                bool bTagReadersFields = true;
                StringBuilder sbTagReaders = new StringBuilder(string.Empty);
                List<DataColumn> lstPK = new List<DataColumn>();
                foreach (DataColumn dcPK in dtSchema.PrimaryKey)
                {
                    lstPK.Add(dcPK);
                }
                foreach (DataColumn dcSchema in dtSchema.Columns)
                {
                    if (!lstPK.Contains(dcSchema))
                    {
                        if (!GetWriteFieldValueBBDDForUpdate(dcSchema.DataType, dcSchema.ColumnName, sTableName,
                                                             iNumColumnAct == dtSchema.Columns.Count,
                                                             ref sbTagReaders, out sMensaje))
                        {
                            bTagReadersFields = false;
                            break;
                        }
                    }
                    iNumColumnAct++;
                }
                if (!bTagReadersFields) return (false);
                sbFileValue.Append(Regex.Replace(sbGetUpdateValuesRegion.ToString(), "                <TAG_READERS>", sbTagReaders.ToString()));
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

        #region Cuerpo

        /// <summary>
        /// Obtiene la región asociada al cuerpo de la clase a crear
        /// </summary>
        /// <param name="sNameSpace">NameSpace al que pertenece la clase de Datos.</param>
        /// <param name="sTableName">Nombre de la Tabla de la Base de Datos a la que proporciona acceso.</param>
        /// <param name="sBuilderValue">Valor de la región asociada al constructor de la clase.</param>
        /// <param name="dtSchema">Tabla que contiene el Esquema de la tabla analizada.</param>
        /// <param name="sbFileValue">Texto asociado al fichero.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool GetBodyRegion(string sNameSpace, string sTableName, string sBuilderValue, DataTable dtSchema,
                                          TypeOfElement eTypeOfElement, ref StringBuilder sbFileValue, out string sMensaje)
        {
            sMensaje = string.Empty;
            try
            {
                string sBody;

                if (!CreateFileStructure(eTypeOfElement, ref sbBody, out sMensaje)) return (false);
                bool bReadAttributes = true;
                StringBuilder sbAttributesValue = new StringBuilder(string.Empty);
                foreach (DataColumn dcSchema in dtSchema.Columns)
                {
                    if (!GetAttributeRegion(dcSchema.DataType, dcSchema.ColumnName, ref sbAttributesValue, out sMensaje))
                    {
                        bReadAttributes = false;
                        break;
                    }
                }
                if (!bReadAttributes) return (false);
                bool bReadProperties = true;
                StringBuilder sbPropertiesValue = new StringBuilder(string.Empty);
                foreach (DataColumn dcSchema in dtSchema.Columns)
                {
                    if (!GetPropertyRegion(dcSchema.DataType, dcSchema.ColumnName, ref sbPropertiesValue, out sMensaje))
                    {
                        bReadProperties = false;
                        break;
                    }
                }
                if (!bReadProperties) return (false);
                StringBuilder sbValueToString = new StringBuilder(string.Empty);
                if (!GetPropertyValueToStringRegion(dtSchema, ref sbValueToString, out sMensaje)) return (false);
                sBody = Regex.Replace(
                              Regex.Replace(
                                    Regex.Replace(
                                          Regex.Replace(Regex.Replace(sbBody.ToString(), "<TAG_NAME_SPACE>", sNameSpace),
                                                        "<TAG_TABLE_NAME>", sTableName),
                                          "        <TAG_ATTRIBUTES>", sbAttributesValue.ToString()),
                                    "        <TAG_PROPERTIES>", sbPropertiesValue.ToString()),
                              "        <TAG_VALUE_TO_STRING>", sbValueToString.ToString());
                if (eTypeOfElement == TypeOfElement.Table)
                {
                    StringBuilder sbGetFromDataRow = new StringBuilder(string.Empty);
                    if (!GetFromDataRowRegion(dtSchema, sTableName, ref sbGetFromDataRow, out sMensaje)) return (false);
                    StringBuilder sbGetInsertValuesRegion = new StringBuilder(string.Empty);
                    if (!GetInsertValuesRegion(dtSchema, sTableName, ref sbGetInsertValuesRegion, out sMensaje)) return (false);
                    StringBuilder sbGetUpdateValuesRegion = new StringBuilder(string.Empty);
                    if (!GetUpdateValuesRegion(dtSchema, sTableName, ref sbGetUpdateValuesRegion, out sMensaje)) return (false);
                    sbFileValue.AppendLine(
                            Regex.Replace(
                                  Regex.Replace(
                                        Regex.Replace(sBody,
                                                      "        <TAG_GET_FROM_DATAROW>", sbGetFromDataRow.ToString()),
                                        "        <TAG_GET_INSERT_VALUES_REGION>", sbGetInsertValuesRegion.ToString()),
                                  "        <TAG_GET_UPDATE_VALUES_REGION>", sbGetUpdateValuesRegion.ToString()));
                }
                else sbFileValue.AppendLine(sBody);
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
                sbBody.AppendLine(@"    /// Descripción: clase que contiene la información de un registro de la tabla <TAG_TABLE_NAME> de la Base de Datos.");
                sbBody.AppendLine(@"    /// </summary>");
                sbBody.AppendLine("    [Serializable()]");
                sbBody.AppendLine("    [XmlInclude(typeof(CADataTable))]");
                sbBody.AppendLine("    public sealed partial class <TAG_TABLE_NAME>Data : CADataTable");
                sbBody.AppendLine("    {");
                sbBody.AppendLine("        #region Atributos");
                sbBody.Append("        <TAG_ATTRIBUTES>");
                sbBody.AppendLine("        #endregion");
                sbBody.AppendLine();
                sbBody.AppendLine("        #region Propiedades");
                sbBody.AppendLine();
                sbBody.AppendLine("        #region Campos");
                sbBody.AppendLine();
                sbBody.Append("        <TAG_PROPERTIES>");
                sbBody.AppendLine("        #endregion");
                sbBody.AppendLine();
                sbBody.AppendLine("        #region Consultoras");
                sbBody.AppendLine();
                sbBody.Append("        <TAG_VALUE_TO_STRING>");
                sbBody.AppendLine("        #endregion");
                sbBody.AppendLine();
                sbBody.AppendLine("        #endregion");
                if (eTypeOfElement == TypeOfElement.Table)
                {
                    sbBody.AppendLine();
                    sbBody.AppendLine("        #region Métodos");
                    sbBody.AppendLine();
                    sbBody.AppendLine("        #region GetFromDataRow");
                    sbBody.AppendLine();
                    sbBody.Append("        <TAG_GET_FROM_DATAROW>");
                    sbBody.AppendLine();
                    sbBody.AppendLine("        #endregion");
                    sbBody.AppendLine();
                    sbBody.AppendLine("        #region GetInsertValuesRegion");
                    sbBody.AppendLine();
                    sbBody.Append("        <TAG_GET_INSERT_VALUES_REGION>");
                    sbBody.AppendLine("        #endregion");
                    sbBody.AppendLine();
                    sbBody.AppendLine("        #region GetUpdateValuesRegion");
                    sbBody.AppendLine();
                    sbBody.Append("        <TAG_GET_UPDATE_VALUES_REGION>");
                    sbBody.AppendLine("        #endregion");
                    sbBody.AppendLine();
                    sbBody.AppendLine("        #endregion");
                }
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

        #region Construcción del fichero

        #region Método Principal

        /// <summary>
        /// Método encargado de Construir el fichero deseado.
        /// </summary>
        /// <param name="sFileName">Nombre del fichero a crear.</param>
        /// <param name="sNameSpace">Espacio de Nombres que debe tomar el fichero.</param>
        /// <param name="eTypeOfElement">Tipo de elemento para el que se desea crear el fichero.</param>
        /// <param name="dtSchema">Esquema de la tabla de la Base de Datos para la que se creará el fichero.</param>
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
                if (!GetLibrariesRegion(ref sbFileValue, out sMensaje)) return (false);
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
                        
        #region Creación del código de un atributo

        /// <summary>
        /// Crea el código de un atributo.
        /// </summary>
        /// <param name="tTypeColumnIn">Tipo de la columna de la Tabla en la Base de Datos</param>
        /// <param name="sNameColumnIn">Nombre del campo de la Base de Datos</param>
        /// <param name="eTypeOfElement">Tipo de elemento a crear.</param>
        /// <param name="sbAttributeValue">Nombre del campo</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, si operación correcta, false, si error.</returns>
        private static bool GetAttributeValue(Type tTypeColumnIn, string sNameColumnIn, 
                                              ref StringBuilder sbAttributeValue, out string sMensaje)
        {
            try
            {
                //  Variables.
                    string sInitValue, sTypeCSharp, sPrefixType, sFieldNameCSharp;

                //  Test de que el nombre del campo de Base de Datos sea válido.
                    if (!BDUtils.FieldNameCSharp(sNameColumnIn, out sFieldNameCSharp, out sMensaje)) return (false);
                    if (!BDUtils.DataTypeCSharp(tTypeColumnIn, out sTypeCSharp, out sMensaje)) return (false);
                    if (!BDUtils.PrefixDataTypeCSharp(tTypeColumnIn, out sPrefixType, out sMensaje)) return (false);
                //  Obtención de la cadena de carácteres que representa al valor inicial del tipo indicado.
                    sMensaje = string.Empty;
                    switch (tTypeColumnIn.Name.ToLower())
                    {
                        case "byte[]":
                            sInitValue = "BYTEARRAY_DB_INIT_VALUE";
                            break;
                        case "byte":
                             sInitValue = "BYTE_DB_INIT_VALUE";
                             break;
                        case "sbyte":
                             sInitValue = "SBYTE_DB_INIT_VALUE";
                             break;
                        case "int16":
                             sInitValue = "SHORT_DB_INIT_VALUE";
                             break;
                        case "int32":
                             sInitValue = "INT32_DB_INIT_VALUE";
                             break;
                        case "int64":
                             sInitValue = "LONG_DB_INIT_VALUE";
                             break;
                        case "uint16":
                             sInitValue = "USHORT_DB_INIT_VALUE";
                             break;
                        case "uint32":
                             sInitValue = "UINT32_DB_INIT_VALUE";
                             break;
                        case "uint64":
                             sInitValue = "ULONG_DB_INIT_VALUE";
                             break;
                        case "single":
                             sInitValue = "SINGLE_DB_INIT_VALUE";
                             break;
                        case "double":
                             sInitValue = "DOUBLE_DB_INIT_VALUE";
                             break;
                        case "decimal":
                             sInitValue = "DECIMAL_DB_INIT_VALUE";
                             break;
                        case "boolean":
                             sInitValue = "BOOLEAN_DB_INIT_VALUE";
                             break;
                        case "char":
                             sInitValue = "CHAR_DB_INIT_VALUE";
                             break;
                        case "string":
                             sInitValue = "STRING_DB_INIT_VALUE";
                             break;
                        case "intptr":
                             sInitValue = "INTPTR_DB_INIT_VALUE";
                             break;
                        case "uintptr":
                             sInitValue = "UINTPTR_DB_INIT_VALUE";
                             break;
                        case "object":
                             sInitValue = "OBJECT_DB_INIT_VALUE";
                             break;
                        case "datetime":
                             sInitValue = "DATETIME_DB_INIT_VALUE";
                             break;
                        case "guid":
                             sInitValue = "GUID_DB_INIT_VALUE";
                             break;
                        default:
                             sMensaje = MsgManager.LiteralMsg(string.Format("Error, tipo '{0}' no reconocido", tTypeColumnIn.ToString().ToLower()));
                             return (false);
                    }
                //  Creación del mensaje a mostrar
                    sbAttributeValue.AppendLine(
                                 string.Format("        internal {0} {1}{2} = BDUtils.{3};",
                                               sTypeCSharp, sPrefixType, sFieldNameCSharp, sInitValue));
                    return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Creación del código de una propiedad

        /// <summary>
        /// Crea el código de una propiedad.
        /// </summary>
        /// <param name="tTypeColumnIn">Tipo de la columna de la Tabla en la Base de Datos</param>
        /// <param name="sNameColumnIn">Nombre del campo de la Base de Datos</param>
        /// <param name="sbPropertyValue">Nombre del campo</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, si operación correcta, false, si error.</returns>
        private static bool GetPropertyValue(Type tTypeColumnIn, string sNameColumnIn, ref StringBuilder sbPropertyValue, 
                                             out string sMensaje)
        {
            try
            {
                //  Variables.
                    string sTypeCSharp;
                    string sPrefixType;
                    string sFieldNameCSharp;

                //  Test de que el nombre del campo de Base de Datos sea válido.
                    if (!BDUtils.FieldNameCSharp(sNameColumnIn, out sFieldNameCSharp, out sMensaje)) return (false);
                    if (!BDUtils.DataTypeCSharp(tTypeColumnIn, out sTypeCSharp, out sMensaje)) return (false);
                    if (!BDUtils.PrefixDataTypeCSharp(tTypeColumnIn, out sPrefixType, out sMensaje)) return (false);
                    sMensaje = string.Empty;
                //  Creación del mensaje a mostrar
                    sbPropertyValue.AppendLine(string.Format("        public {0} {1}", sTypeCSharp, sFieldNameCSharp));
                    sbPropertyValue.AppendLine("        {");
                    sbPropertyValue.AppendLine("            get { return (" + sPrefixType + sFieldNameCSharp + "); }");
                    sbPropertyValue.AppendLine("            set { " + sPrefixType + sFieldNameCSharp + " = value; }");
                    sbPropertyValue.AppendLine("        }");
                    sbPropertyValue.AppendLine();
                    return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Creación del código de lectura del valor de un campo de la Base de Datos.

        /// <summary>
        /// Crea el código que lee el valor de un campo de la Base de Datos.
        /// </summary>
        /// <param name="tTypeColumnIn">Tipo de la columna de la Tabla en la Base de Datos</param>
        /// <param name="sNameColumnIn">Nombre del campo de la Base de Datos</param>
        /// <param name="sTableName">Nombre de la tabla a la que pertenece el valor que se quiere leer.</param>
        /// <param name="sbReadValue">Código que lee el valor del campo indicado.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, si operación correcta, false, si error.</returns>
        private static bool GetReadFieldValueBBDD(Type tTypeColumnIn, string sNameColumnIn, string sTableName, 
                                                  ref StringBuilder sbReadValue, out string sMensaje)
        {
            try
            {
                //  Variables.
                    string sPrefixType;
                    string sMethodAnalyze;
                    string sFieldNameCSharp;

                //  Test de que el nombre del campo de Base de Datos sea válido.
                    if (!BDUtils.FieldNameCSharp(sNameColumnIn, out sFieldNameCSharp, out sMensaje)) return (false);
                    if (!BDUtils.PrefixDataTypeCSharp(tTypeColumnIn, out sPrefixType, out sMensaje)) return (false);
                    if (!BDUtils.MethodToAnalyzeTypeBBDD(tTypeColumnIn, out sMethodAnalyze, out sMensaje)) return (false);
                    sMensaje = string.Empty;
                //  Creación del mensaje a mostrar
                    sbReadValue.AppendLine(
                           string.Format("                if (!{0}(dr[Tables.Schema{1}Table.F{2}], out {3}{2}, out sMensaje)) return (false);",
                                         sMethodAnalyze, sTableName, sFieldNameCSharp, sPrefixType));
                    return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Creación del código de escritura del valor de un campo de la Base de Datos.

        #region Para una senténcia Insert

        /// <summary>
        /// Crea el código que lee el valor de un campo de la Base de Datos.
        /// </summary>
        /// <param name="tTypeColumnIn">Tipo de la columna de la Tabla en la Base de Datos</param>
        /// <param name="sNameColumnIn">Nombre del campo de la Base de Datos</param>
        /// <param name="sTableName">Nombre de la tabla a la que pertenece el valor que se quiere leer.</param>
        /// <param name="sbReadValue">Código que lee el valor del campo indicado.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, si operación correcta, false, si error.</returns>
        private static bool GetWriteFieldValueBBDD(Type tTypeColumnIn, string sNameColumnIn, string sTableName, 
                                                   bool bLastItem, ref StringBuilder sbWriteValue, out string sMensaje)
        {
            try
            {
                //  Variables.
                    string sPrefixType;
                    string sMethodAnalyze;
                    string sFieldNameCSharp;

                //  Test de que el nombre del campo de Base de Datos sea válido.
                    if (!BDUtils.FieldNameCSharp(sNameColumnIn, out sFieldNameCSharp, out sMensaje)) return (false);
                    if (!BDUtils.MethodToAnalyzeTypeCSharp(tTypeColumnIn, out sMethodAnalyze, out sMensaje)) return (false);
                    if (!BDUtils.PrefixDataTypeCSharp(tTypeColumnIn, out sPrefixType, out sMensaje)) return (false);
                    sMensaje = string.Empty;
                //  Creación del mensaje a mostrar
                    if (tTypeColumnIn.Name == "DateTime")
                    {
                        sbWriteValue.AppendLine(
                               string.Format("                if (!{0}(eDBType, {1}{2}, out sValueOut, out sMensaje)) return (false);",
                                             sMethodAnalyze, sPrefixType, sFieldNameCSharp));
                    }
                    else
                    {
                        sbWriteValue.AppendLine(
                               string.Format("                if (!{0}({1}{2}, out sValueOut, out sMensaje)) return (false);",
                                             sMethodAnalyze, sPrefixType, sFieldNameCSharp));
                    }
                    sbWriteValue.AppendLine("                sbValue.AppendLine(sValueOut" +
                                                             ((bLastItem) ? " + \")\"" : " + \", \"") + ");");
                    return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Para una senténcia Update

        /// <summary>
        /// Crea el código que lee el valor de un campo de la Base de Datos.
        /// </summary>
        /// <param name="tTypeColumnIn">Tipo de la columna de la Tabla en la Base de Datos</param>
        /// <param name="sNameColumnIn">Nombre del campo de la Base de Datos</param>
        /// <param name="sTableName">Nombre de la tabla a la que pertenece el valor que se quiere leer.</param>
        /// <param name="sbReadValue">Código que lee el valor del campo indicado.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, si operación correcta, false, si error.</returns>
        private static bool GetWriteFieldValueBBDDForUpdate(Type tTypeColumnIn, string sNameColumnIn, 
                                                            string sTableName, bool bLastItem, 
                                                            ref StringBuilder sbWriteValue, out string sMensaje)
        {
            try
            {
                //  Variables.
                    string sPrefixType;
                    string sMethodAnalyze;
                    string sFieldNameCSharp;
                    string sFieldToUpdate;

                //  Test de que el nombre del campo de Base de Datos sea válido.
                    if (!BDUtils.FieldNameCSharp(sNameColumnIn, out sFieldNameCSharp, out sMensaje)) return (false);
                    if (!BDUtils.MethodToAnalyzeTypeCSharp(tTypeColumnIn, out sMethodAnalyze, out sMensaje)) return (false);
                    if (!BDUtils.PrefixDataTypeCSharp(tTypeColumnIn, out sPrefixType, out sMensaje)) return (false);
                    sMensaje = string.Empty;
                //  Creación del mensaje a mostrar
                    if (tTypeColumnIn.Name == "DateTime")
                    {
                        sbWriteValue.AppendLine(
                               string.Format("                if (!{0}(eDBType, {1}{2}, out sValueOut, out sMensaje)) return (false);",
                                             sMethodAnalyze, sPrefixType, sFieldNameCSharp));
                    }
                    else
                    {
                        sbWriteValue.AppendLine(
                               string.Format("                if (!{0}({1}{2}, out sValueOut, out sMensaje)) return (false);",
                                             sMethodAnalyze, sPrefixType, sFieldNameCSharp));
                    }
                    sFieldToUpdate = string.Format("\"{0} =\" + ", sFieldNameCSharp);
                    sbWriteValue.AppendLine("                sbValue.AppendLine(" + sFieldToUpdate + " sValueOut" +
                                                             ((bLastItem) ? " + \" \"" : " + \", \"") + ");");
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

        #region Creación del código que muestra en un string la información de la clase.

        /// <summary>
        /// Crea el valor que escribe los datos de la clase en un string.
        /// </summary>
        /// <param name="dtSchema">Esquema de la tabla a tratar.</param>
        /// <param name="sbReturnValue">Código de retorno de la aplicación.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool CreateReturnValue(DataTable dtSchema, out StringBuilder sbReturnValue, out string sMensaje)
        {
            sbReturnValue = null;
            try
            {
                sMensaje = string.Empty;
                int iNumCars = 23;
                StringBuilder sbFormatString = new StringBuilder("return (string.Format(\"");
                for (int i = 0; i < dtSchema.Columns.Count; i++)
                {
                    if ((i % 9 == 0) && (i != 0))
                    {
                        sbFormatString.AppendLine("{" + i.ToString() + "}\\r\\n\" + ");
                        sbFormatString.Append("                                          \"");
                        iNumCars = 43;
                    }
                    else
                    {
                        sbFormatString.Append("{" + i.ToString() + "}\\r\\n");
                        iNumCars += ("{" + i.ToString() + "}\\r\\n").Length;
                    }
                }
                sbFormatString.Append("\"");
                iNumCars++;
                string sFieldNameCSharp, sPrefixType;
                foreach (DataColumn dcSchema in dtSchema.Columns)
                {
                    if (!BDUtils.FieldNameCSharp(dcSchema.ColumnName, out sFieldNameCSharp, out sMensaje)) return (false);
                    if (!BDUtils.PrefixDataTypeCSharp(dcSchema.DataType, out sPrefixType, out sMensaje)) return (false);
                    if (iNumCars + sPrefixType.Length + sFieldNameCSharp.Length + 2 < 115)
                    {
                        sbFormatString.Append(", " + sPrefixType + sFieldNameCSharp);
                        iNumCars += sPrefixType.Length + sFieldNameCSharp.Length + 2;
                    }
                    else
                    {
                        sbFormatString.AppendLine(", ");
                        sbFormatString.Append("                                          " + sPrefixType + sFieldNameCSharp);
                        iNumCars = sPrefixType.Length + sFieldNameCSharp.Length + 43;
                    }
                }
                sbReturnValue = sbFormatString.Append("));");
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



