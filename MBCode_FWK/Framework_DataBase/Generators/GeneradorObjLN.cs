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
    /// Fecha última modificación: 01/03/2012
    /// Descripción: clase encargada de la generación del fichero que define la estructura de una tabla en memória 
    ///              y las operaciones que se pueden aplicar a la misma.
    /// </summary>
    public static class GeneradorObjLN
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
        /// Almacena la región de los constructores de la clase a crear.
        /// </summary>
        private static StringBuilder sbBuilder = null;

        /// <summary>
        /// Almacena el cuerpo de la clase a crear.
        /// </summary>
        private static StringBuilder sbBody = null;

        #endregion

        #region Métodos

        #region Regiones

        #region Librerias

        /// <summary>
        /// Obtiene la region de librerias
        /// </summary>
        /// <param name="sUsing">Using a añadir correspondiente al Servicio Web del que se obtiene la clase de Datos.</param>
        /// <param name="sTableName">Nombre de la Tabla de la Base de Datos a la que proporciona acceso.</param>
        /// <param name="sbFileValue">Texto asociado al fichero.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool GetLibrariesRegion(string sUsing, string sTableName,
                                               ref StringBuilder sbFileValue, out string sMensaje)
        {
            try
            {
                if (sbLibraries == null)
                {
                    sbLibraries = new StringBuilder();
                    sbLibraries.AppendLine("#region Librerias usadas por la clase");
                    sbLibraries.AppendLine();
                    sbLibraries.AppendLine("using System;                                // Libreria necesaria para usar la clase Exception.");
                    sbLibraries.AppendLine("using System.Xml.Serialization;              // Libreria necesaria para usar el atributo XmlInclude.");
                    sbLibraries.AppendLine("using Framework.DataBase;                    // Libreria necesaria para usar la clase CADataTable.");
                    sbLibraries.AppendLine("using Framework.DataBase.Utils;              // Libreria necesaria para usar la clase BDUtils.");
                    sbLibraries.AppendLine("using <TAG_USING>;   // Libreria necesaria para usar la clase <TAG_TABLE_NAME>Data.");
                    sbLibraries.AppendLine();
                    sbLibraries.AppendLine("#endregion");
                }
                sMensaje = string.Empty;
                sbFileValue.AppendLine(Regex.Replace(Regex.Replace(sbLibraries.ToString(), "<TAG_USING>", sUsing),
                                                     "<TAG_TABLE_NAME>", sTableName));
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

        #region Constructores

        /// <summary>
        /// Obtiene la region de los Constructores
        /// </summary>
        /// <param name="dtSchema">Esquema de la Tabla de la Base de Datos a la que proporciona acceso.</param>
        /// <param name="sTableName">Nombre de la tabla traducido a un valor correcto en C#.</param>
        /// <param name="sbFileValue">Texto asociado al fichero.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool GetBuilderRegion(DataTable dtSchema, string sTableName, ref StringBuilder sbFileValue, out string sMensaje)
        {
            try
            {
                if (sbBuilder == null)
                {
                    sbBuilder = new StringBuilder();
                    sbBuilder.AppendLine("        #region Constructores");
                    sbBuilder.AppendLine();
                    sbBuilder.AppendLine(@"        /// <sumary>");
                    sbBuilder.AppendLine(@"        /// Constructor por defecto de la clase.");
                    sbBuilder.AppendLine(@"        /// </sumary>");
                    sbBuilder.AppendLine("        public <TAG_TABLE_NAME>() : base()");
                    sbBuilder.AppendLine("        {");
                    sbBuilder.AppendLine("            try");
                    sbBuilder.AppendLine("            {");
                    sbBuilder.AppendLine("                o<TAG_TABLE_NAME>Data = new <TAG_TABLE_NAME>Data();");
                    sbBuilder.Append("                <TAG_INIT_VALUES_FIELDS>");
                    sbBuilder.AppendLine("                this.EstadoInstancia = InstanceState.Empty;");
                    sbBuilder.AppendLine("            }");
                    sbBuilder.AppendLine("            catch (Exception)");
                    sbBuilder.AppendLine("            {");
                    sbBuilder.AppendLine("                this.EstadoInstancia = InstanceState.Empty;");
                    sbBuilder.AppendLine("            }");
                    sbBuilder.AppendLine("        }");
                    sbBuilder.AppendLine();
                    sbBuilder.Append("        #endregion");
                }
                sMensaje = string.Empty;
                bool bReadInfoFields = true;
                StringBuilder sbInitFieldsValue = new StringBuilder(string.Empty);
                foreach (DataColumn dcSchema in dtSchema.Columns)
                {
                    if (!GetInitValue(sTableName, dcSchema.DataType, dcSchema.ColumnName, ref sbInitFieldsValue, out sMensaje))
                    {
                        bReadInfoFields = false;
                        break;
                    }
                }
                if (!bReadInfoFields) return (false);
                sbFileValue.Append(
                    Regex.Replace(
                          Regex.Replace(sbBuilder.ToString(), "<TAG_TABLE_NAME>", sTableName),
                          "                <TAG_INIT_VALUES_FIELDS>", sbInitFieldsValue.ToString()));
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
        /// <param name="sbFileValue">Texto asociado al fichero.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool GetBodyRegion(string sNameSpace, string sTableName, string sBuilderValue,
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
                    sbBody.AppendLine(@"    /// Descripción: Clase que contiene la representación en clase de un registro de la tabla <TAG_TABLE_NAME>.");
                    sbBody.AppendLine(@"    /// </summary>");
                    sbBody.AppendLine("    [Serializable()]");
                    sbBody.AppendLine("    [XmlInclude(typeof(Framework.DataBase.CADataTable))]");
                    sbBody.AppendLine("    public sealed partial class <TAG_TABLE_NAME> : Framework.DataBase.CADataTable");
                    sbBody.AppendLine("    {");
                    sbBody.AppendLine("        #region Atributos");
                    sbBody.AppendLine();
                    sbBody.AppendLine(@"        /// <summary> ");
                    sbBody.AppendLine(@"        /// Almacena los datos de la instáncia. ");
                    sbBody.AppendLine(@"        /// </summary> ");
                    sbBody.AppendLine("        private <TAG_TABLE_NAME>Data o<TAG_TABLE_NAME>Data = null;");
                    sbBody.AppendLine();
                    sbBody.AppendLine("        #endregion");
                    sbBody.AppendLine();
                    sbBody.AppendLine("        #region Propiedades");
                    sbBody.AppendLine();
                    sbBody.AppendLine(@"        /// <summary> ");
                    sbBody.AppendLine(@"        /// Obtiene/Establece los datos de la instáncia. ");
                    sbBody.AppendLine(@"        /// </summary> ");
                    sbBody.AppendLine("        public <TAG_TABLE_NAME>Data Data");
                    sbBody.AppendLine("        {");
                    sbBody.AppendLine("             get");
                    sbBody.AppendLine("             {");
                    sbBody.AppendLine("                 return (o<TAG_TABLE_NAME>Data);");
                    sbBody.AppendLine("             }");
                    sbBody.AppendLine("             set");
                    sbBody.AppendLine("             {");
                    sbBody.AppendLine("                 o<TAG_TABLE_NAME>Data = value;");
                    sbBody.AppendLine("             }");
                    sbBody.AppendLine("        }");
                    sbBody.AppendLine();
                    sbBody.AppendLine("        #endregion");
                    sbBody.AppendLine();
                    sbBody.AppendLine("        <TAG_CREATORS>");
                    sbBody.AppendLine("    }");
                    sbBody.AppendLine("}");
                }
                sMensaje = string.Empty;
                sbFileValue.AppendLine(Regex.Replace(
                                             Regex.Replace(
                                                   Regex.Replace(sbBody.ToString(), "<TAG_NAME_SPACE>", sNameSpace),
                                                                 "<TAG_TABLE_NAME>", sTableName),
                                                   "        <TAG_CREATORS>", sBuilderValue));
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
        public static bool CrearFichero(string sFileName, string sNameSpace, string sUsing, DataTable dtSchema, out string sMensaje)
        {
            try
            {
                string sTableName;
                StringBuilder sbFileValue = new StringBuilder(string.Empty);
                StringBuilder sbBuilderValue = new StringBuilder(string.Empty);
                if (!BDUtils.TableNameCSharp(dtSchema.TableName, out sTableName, out sMensaje)) return (false);
                if (!GetLibrariesRegion(sUsing, sTableName, ref sbFileValue, out sMensaje)) return (false);
                if (!GetCompilationDirectivesRegion(ref sbFileValue, out sMensaje)) return (false);
                if (!GetBuilderRegion(dtSchema, sTableName, ref sbBuilderValue, out sMensaje)) return (false);
                if (!GetBodyRegion(sNameSpace, sTableName, sbBuilderValue.ToString(), ref sbFileValue, out sMensaje)) return (false);
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
        /// <param name="sTableName">Nombre de la tabla de la Base de Datos a la que pertenece la columna.</param>
        /// <param name="tTypeColumnIn">Tipo de la columna de la Tabla en la Base de Datos</param>
        /// <param name="sNameColumnIn">Nombre del campo de la Base de Datos</param>
        /// <param name="sbInitValue">Nombre del campo</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, si operación correcta, false, si error.</returns>
        private static bool GetInitValue(string sTableName, Type tTypeColumnIn, string sNameColumnIn, 
                                         ref StringBuilder sbInitValue, out string sMensaje)
        {
            try
            {
                //  Variables.
                    string sInitValue;
                    string sFieldNameCSharp;

                //  Test de que el nombre del campo de Base de Datos sea válido.
                    if (!BDUtils.FieldNameCSharp(sNameColumnIn, out sFieldNameCSharp, out sMensaje)) return (false);
                //  Obtención de la cadena de carácteres que representa al valor inicial del tipo indicado.
                    sMensaje = string.Empty;
                    switch (tTypeColumnIn.ToString().ToLower())
                    {
                        case "system.byte[]":
                            sInitValue = "BYTEARRAY_DB_INIT_VALUE";
                            break;
                        case "system.byte":
                             sInitValue = "BYTE_DB_INIT_VALUE";
                             break;
                        case "system.sbyte":
                             sInitValue = "SBYTE_DB_INIT_VALUE";
                             break;
                        case "system.int16":
                             sInitValue = "SHORT_DB_INIT_VALUE";
                             break;
                        case "system.int32":
                             sInitValue = "INT32_DB_INIT_VALUE";
                             break;
                        case "system.int64":
                             sInitValue = "LONG_DB_INIT_VALUE";
                             break;
                        case "system.uint16":
                             sInitValue = "USHORT_DB_INIT_VALUE";
                             break;
                        case "system.uint32":
                             sInitValue = "UINT32_DB_INIT_VALUE";
                             break;
                        case "system.uint64":
                             sInitValue = "ULONG_DB_INIT_VALUE";
                             break;
                        case "system.float":
                        case "system.single":
                             sInitValue = "SINGLE_DB_INIT_VALUE";
                             break;
                        case "system.double":
                             sInitValue = "DOUBLE_DB_INIT_VALUE";
                             break;
                        case "system.decimal":
                             sInitValue = "DECIMAL_DB_INIT_VALUE";
                             break;
                        case "system.boolean":
                             sInitValue = "BOOLEAN_DB_INIT_VALUE";
                             break;
                        case "system.char":
                             sInitValue = "CHAR_DB_INIT_VALUE";
                             break;
                        case "system.string":
                             sInitValue = "STRING_DB_INIT_VALUE";
                             break;
                        case "system.intptr":
                             sInitValue = "INTPTR_DB_INIT_VALUE";
                             break;
                        case "system.uintptr":
                             sInitValue = "UINTPTR_DB_INIT_VALUE";
                             break;
                        case "system.object":
                             sInitValue = "OBJECT_DB_INIT_VALUE";
                             break;
                        case "system.datetime":
                             sInitValue = "DATETIME_DB_INIT_VALUE";
                             break;
                        case "system.guid":
                             sInitValue = "GUID_DB_INIT_VALUE";
                             break;
                        default:
                             sMensaje = MsgManager.ErrorMsg("MSG_GeneradorObjLN_000", tTypeColumnIn.ToString().ToLower());
                             return (false);
                    }
                //  Creación del mensaje a mostrar
                    sbInitValue.AppendLine(
                            Regex.Replace(
                                  string.Format("                o<TAG_TABLE_NAME>Data.{0} = BDUtils.{1};",
                                                sFieldNameCSharp, sInitValue),
                                  "<TAG_TABLE_NAME>", sTableName));
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


