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
    /// Descripción: clase encargada de la generación del fichero que define la estructura de una tabla en memória.
    /// </summary>
    public static class GeneradorObjDDLInternal
    {
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
        /// Almacena la región de la Propiedad asociada al nombre de la Tabla a crear.
        /// </summary>
        private static StringBuilder sbTableNameProperty = null;

        /// <summary>
        /// Almacena la región de las Propiedades de la clase a crear.
        /// </summary>
        private static StringBuilder sbProperties = null;

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

        #region Propiedades

        #region Nombre Tabla

        /// <summary>
        /// Obtiene la region de las Propiedades
        /// </summary>
        /// <param name="tTypeField">Tipo del campo para el que se quiere .</param>
        /// <param name="sTableName">Nombre de la tabla en la Base de Datos.</param>
        /// <param name="sbPropertyTable">Variable en la que se almacenan los resultados.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool GetPropertyTableRegion(string sTableName, out StringBuilder sbPropertyTable, out string sMensaje)
        {
            try
            {
                if (sbTableNameProperty == null)
                {
                    sbTableNameProperty = new StringBuilder();
                    sbTableNameProperty.AppendLine(@"        /// <sumary>");
                    sbTableNameProperty.AppendLine(@"        /// Nombre de la Tabla [<TAG_TABLE_NAME>].");
                    sbTableNameProperty.AppendLine(@"        /// </sumary>");
                    sbTableNameProperty.AppendLine("        public string TableName");
                    sbTableNameProperty.AppendLine("        {");
                    sbTableNameProperty.AppendLine("            get { return (\"<TAG_TABLE_NAME>\"); }");
                    sbTableNameProperty.AppendLine("            set { ; }");
                    sbTableNameProperty.AppendLine("        }");
                    sbTableNameProperty.AppendLine();
                }
                sMensaje = string.Empty;
                sbPropertyTable = new StringBuilder(Regex.Replace(sbTableNameProperty.ToString(), "<TAG_TABLE_NAME>", sTableName));
                return (true);
            }
            catch (Exception ex)
            {
                sbPropertyTable = null;
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region Campos

        /// <summary>
        /// Obtiene la region de las Propiedades
        /// </summary>
        /// <param name="tTypeField">Tipo del campo para el que se quiere .</param>
        /// <param name="sFieldName">Nombre de la columna en la Base de Datos.</param>
        /// <param name="sFieldNameCsharp">Nombre de la columna en C#.</param>
        /// <param name="sbPropertiesFields">Variable en la que se almacenan los resultados.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool GetPropertyRegion(Type tTypeField, string sFieldName, string sFieldNameCsharp, 
                                              ref StringBuilder sbPropertiesFields, out string sMensaje)
        {
            try
            {
                if (sbProperties == null)
                {
                    sbProperties = new StringBuilder();
                    sbProperties.AppendLine(@"        /// <sumary>");
                    sbProperties.AppendLine(@"        /// Nombre del campo '<TAG_FIELD_NAME>' de tipo {<TAG_FIELD_TYPE>}.");
                    sbProperties.AppendLine(@"        /// </sumary>");
                    sbProperties.AppendLine("        public string F<TAG_FIELD_NAME_C#>");
                    sbProperties.AppendLine("        {");
                    sbProperties.AppendLine("            get { return (\"<TAG_FIELD_NAME>\"); }");
                    sbProperties.AppendLine("            set { ; }");
                    sbProperties.AppendLine("        }");
                }
                sMensaje = string.Empty;
                sbPropertiesFields.AppendLine(
                    Regex.Replace(
                          Regex.Replace(
                                Regex.Replace(sbProperties.ToString(), "<TAG_FIELD_NAME>", sFieldName),
                                "<TAG_FIELD_NAME_C#>", sFieldNameCsharp),
                          "<TAG_FIELD_TYPE>", tTypeField.ToString()));

                return (true);
            }
            catch (Exception ex)
            {
                sbPropertiesFields = null;
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
                                          ref StringBuilder sbFileValue, out string sMensaje)
        {
            try
            {
                if (sbBody == null)
                {
                    sbBody = new StringBuilder();
                    sbBody.AppendLine("namespace <TAG_NAME_SPACE>");
                    sbBody.AppendLine("{");
                    sbBody.AppendLine(@"    /// <summary>");
                    sbBody.AppendLine(@"    /// Autor: Generador de Código Automático.");
                    sbBody.AppendLine(@"    /// Fecha Última Modificación: " + DateTime.Now.ToShortDateString());
                    sbBody.AppendLine(@"    /// Descripción: Clase que contiene la estructura interna de la tabla <TAG_TABLE_NAME> de la Base de Datos.");
                    sbBody.AppendLine(@"    /// </summary>");
                    sbBody.AppendLine("    [Serializable()]");
                    sbBody.AppendLine("    public sealed class <TAG_TABLE_NAME>Table");
                    sbBody.AppendLine("    {");
                    sbBody.AppendLine("        #region Propiedades");
                    sbBody.AppendLine();
                    sbBody.AppendLine("        #region Nombre de la Tabla");
                    sbBody.AppendLine();
                    sbBody.Append("        <TAG_PROPERTY_TABLE>");
                    sbBody.AppendLine("        #endregion");
                    sbBody.AppendLine();
                    sbBody.AppendLine("        #region Nombres de los Campos");
                    sbBody.AppendLine();
                    sbBody.Append("        <TAG_PROPERTIES_FIELDS>");
                    sbBody.AppendLine("        #endregion");
                    sbBody.AppendLine();
                    sbBody.AppendLine("        #endregion");
                    sbBody.AppendLine("    }");
                    sbBody.AppendLine("}");
                }
                sMensaje = string.Empty;
                StringBuilder sbPropertyTable;
                StringBuilder sbPropertiesFields = new StringBuilder(string.Empty);
                if (!GetPropertyTableRegion(sTableName, out sbPropertyTable, out sMensaje)) return (false);
                bool bReadInfoFields = true;
                foreach (DataColumn dcSchema in dtSchema.Columns)
                {
                    if (!GetPropertyValue(dcSchema.DataType, dcSchema.ColumnName, ref sbPropertiesFields, out sMensaje))
                    {
                        bReadInfoFields = false;
                        break;
                    }
                }
                if (!bReadInfoFields) return (false);
                sbFileValue.AppendLine(
                            Regex.Replace(
                                  Regex.Replace(
                                        Regex.Replace(
                                              Regex.Replace(sbBody.ToString(), "<TAG_NAME_SPACE>", sNameSpace),
                                              "<TAG_TABLE_NAME>", sTableName),
                                        "        <TAG_PROPERTY_TABLE>", sbPropertyTable.ToString()),
                                  "        <TAG_PROPERTIES_FIELDS>", sbPropertiesFields.ToString()));

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
        /// <param name="sUsing">Using que debe tomar el fichero</param>
        /// <param name="dtSchema">Esquema de la tabla de la Base de Datos para la que se creará el fichero.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, si  operación correcta, false, si error.</returns>
        public static bool CrearFichero(string sFileName, string sNameSpace, DataTable dtSchema, out string sMensaje)
        {
            try
            {
                string sTableName;
                StringBuilder sbFileValue = new StringBuilder(string.Empty);
                StringBuilder sbBuilderValue = new StringBuilder(string.Empty);
                if (!BDUtils.TableNameCSharp(dtSchema.TableName, out sTableName, out sMensaje)) return (false);
                if (!GetLibrariesRegion(ref sbFileValue, out sMensaje)) return (false);
                if (!GetCompilationDirectivesRegion(ref sbFileValue, out sMensaje)) return (false);
                if (!GetBodyRegion(sNameSpace, sTableName, sbBuilderValue.ToString(), dtSchema, ref sbFileValue, out sMensaje)) return (false);
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

        #region Traducción de Tipo de Campo y Nombre de Campo de Base de Datos a C#

        /// <summary>
        /// Convierte el tipo del campo que se ha leido de la base de Datos en el tipo correspondiente dentro
        /// del lenguaje de programación C#.
        /// </summary>
        /// <param name="tTypeColumnIn">Tipo de la columna de la Tabla en la Base de Datos</param>
        /// <param name="sNameColumnIn">Nombre del campo de la Base de Datos</param>
        /// <param name="sbFieldNameValue">Nombre del campo</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, si operación correcta, false, si error.</returns>
        private static bool GetPropertyValue(Type tTypeColumnIn, string sNameColumnIn, 
                                             ref StringBuilder sbFieldNameValue, out string sMensaje)
        {
            try
            {
                //  Variables.
                    string sNameColumnCSharp;

                //  Test de que el nombre del campo de Base de Datos sea válido.
                    if (!BDUtils.FieldNameCSharp(sNameColumnIn, out sNameColumnCSharp, out sMensaje)) return (false);
                    return (GetPropertyRegion(tTypeColumnIn, sNameColumnIn, sNameColumnCSharp, ref sbFieldNameValue, out sMensaje));
            }
            catch (Exception ex)
            {
                sbFieldNameValue = null;
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #endregion

        #endregion
    }
}
