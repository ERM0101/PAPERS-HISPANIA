#region Librerias usadas por la clase

using MBCode.Framework.DataBase.Utils;
using MBCode.Framework.Managers.Messages;
using System;
using System.Collections;
using System.Data;
using System.IO;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

#endregion

namespace MBCode.Framework.DataBase
{
    /// <summary>
    /// Autor: Alejandro Molt� Bou
    /// Fecha �ltima modificaci�n: 01/03/2012
    /// Descripci�n: clase encargada de la generaci�n del fichero que define la estructura de una tabla en mem�ria.
    /// </summary>
    public static class GeneradorObjTablesDDLInternal
    {
        #region Atributos

        /// <summary>
        /// Almacena la(as) libreria(as) a crear.
        /// </summary>
        private static StringBuilder sbLibraries = null;

        /// <summary>
        /// Almacena las directivas de compilaci�n a crear.
        /// </summary>
        private static StringBuilder sbCompilationDirectives = null;

        /// <summary>
        /// Almacena el cuerpo de la clase a crear.
        /// </summary>
        private static StringBuilder sbBody = null;

        /// <summary>
        /// Almacena la regi�n de los Atributos de la clase a crear.
        /// </summary>
        private static StringBuilder sbAttributes = null;

        /// <summary>
        /// Almacena la regi�n de las Propiedades de la clase a crear.
        /// </summary>
        private static StringBuilder sbProperties = null;

        #endregion

        #region M�todos

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

        #region Directivas de Compilaci�n

        /// <summary>
        /// Obtiene la regi�n de directivas de compilaci�n.
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
                    sbCompilationDirectives.AppendLine("#region Directivas de Compilaci�n");
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
        /// <param name="sTableName">Nombre de la tabla a tratar.</param>
        /// <param name="sbFileValue">Variable en la que se almacenan los resultados.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool GetAttributeRegion(string sTableName, ref StringBuilder sbFileValue, out string sMensaje)
        {
            try
            {
                if (sbAttributes == null)
                {
                    sbAttributes = new StringBuilder();
                    sbAttributes.AppendLine(@"        /// <sumary>");
                    sbAttributes.AppendLine(@"        /// Almacena una refer�ncia al esquema de la tabla <TAG_TABLE_NAME> en la Base de Datos.");
                    sbAttributes.AppendLine(@"        /// </sumary>");
                    sbAttributes.AppendLine(@"        private static <TAG_TABLE_NAME>Table oSchema<TAG_TABLE_NAME>Table = null;");
                }
                sMensaje = string.Empty;
                sbFileValue.Append(Regex.Replace(sbAttributes.ToString(), "<TAG_TABLE_NAME>", sTableName));
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

        /// <summary>
        /// Obtiene la region de las Propiedades
        /// </summary>
        /// <param name="sTableName">Nombre de la tabla a tratar.</param>
        /// <param name="sbFileValue">Variable en la que se almacenan los resultados.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool GetPropertyRegion(string sTableName, ref StringBuilder sbFileValue, out string sMensaje)
        {
            try
            {
                if (sbProperties == null)
                {
                    sbProperties = new StringBuilder();
                    sbProperties.AppendLine(@"        #region <TAG_TABLE_NAME>");
                    sbProperties.AppendLine();
                    sbProperties.AppendLine(@"        /// <sumary>");
                    sbProperties.AppendLine(@"        /// Obtiene una refer�ncia al esquema de la tabla <TAG_TABLE_NAME> en la Base de Datos.");
                    sbProperties.AppendLine(@"        /// </sumary>");
                    sbProperties.AppendLine(@"        public static <TAG_TABLE_NAME>Table Schema<TAG_TABLE_NAME>Table");
                    sbProperties.AppendLine(@"        {");
                    sbProperties.AppendLine(@"            get");
                    sbProperties.AppendLine(@"            {");
                    sbProperties.AppendLine(@"                if (oSchema<TAG_TABLE_NAME>Table == null) oSchema<TAG_TABLE_NAME>Table = new <TAG_TABLE_NAME>Table();");
                    sbProperties.AppendLine(@"                return (oSchema<TAG_TABLE_NAME>Table);");
                    sbProperties.AppendLine(@"            }");
                    sbProperties.AppendLine(@"        }");
                    sbProperties.AppendLine();
                    sbProperties.AppendLine(@"        #endregion");
                }
                sMensaje = string.Empty;
                sbFileValue.AppendLine(Regex.Replace(sbProperties.ToString(), "<TAG_TABLE_NAME>", sTableName));
                sbBody.AppendLine();
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
        /// Obtiene la regi�n asociada al cuerpo de la clase a crear
        /// </summary>
        /// <param name="sNameSpace">NameSpace al que pertenece la clase de Datos.</param>
        /// <param name="sBuilderValue">Valor de la regi�n asociada al constructor de la clase.</param>
        /// <param name="htTables">Tablas a tratar.</param>
        /// <param name="sbFileValue">Texto asociado al fichero.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>false, si error, true, si todo correcto</returns>
        private static bool GetBodyRegion(string sNameSpace, string sBuilderValue, Hashtable htTables, 
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
                    sbBody.AppendLine(@"    /// Autor: Generador de C�digo Autom�tico.");
                    sbBody.AppendLine(@"    /// Fecha �ltima Modificaci�n: " + DateTime.Now.ToShortDateString());
                    sbBody.AppendLine(@"    /// Descripci�n: Clase que contiene la estructura interna de la tabla <TAG_TABLE_NAME> de la Base de Datos.");
                    sbBody.AppendLine(@"    /// </summary>");
                    sbBody.AppendLine("    public static class Tables");
                    sbBody.AppendLine("    {");
                    sbBody.AppendLine("        #region Atributos");
                    sbBody.AppendLine();
                    sbBody.Append("        <TAG_ATTRIBUTES_TABLE>");
                    sbBody.AppendLine();
                    sbBody.AppendLine("        #endregion");
                    sbBody.AppendLine();
                    sbBody.AppendLine("        #region Propiedades");
                    sbBody.AppendLine();
                    sbBody.Append("        <TAG_PROPERTIES_TABLE>");
                    sbBody.AppendLine("        #endregion");
                    sbBody.AppendLine("    }");
                    sbBody.AppendLine("}");
                }
                sMensaje = string.Empty;
                string sTableName;
                StringBuilder sbAttributeTable = new StringBuilder(string.Empty);
                foreach (DataTable dtSchema in htTables.Values)
                {
                    if (!BDUtils.TableNameCSharp(dtSchema.TableName, out sTableName, out sMensaje)) return (false);
                    if (!GetAttributeRegion(sTableName, ref sbAttributeTable, out sMensaje)) return (false);
                }
                StringBuilder sbPropertyTable = new StringBuilder(string.Empty);
                foreach (DataTable dtSchema in htTables.Values)
                {
                    if (!BDUtils.TableNameCSharp(dtSchema.TableName, out sTableName, out sMensaje)) return (false);
                    if (!GetPropertyRegion(sTableName, ref sbPropertyTable, out sMensaje)) return (false);
                }
                sbFileValue.AppendLine(
                            Regex.Replace(
                                  Regex.Replace(
                                        Regex.Replace(sbBody.ToString(), "<TAG_NAME_SPACE>", sNameSpace),
                                  "        <TAG_ATTRIBUTES_TABLE>", sbAttributeTable.ToString()),
                            "        <TAG_PROPERTIES_TABLE>", sbPropertyTable.ToString()));

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

        #region Construcci�n del fichero

        #region M�todo Principal

        /// <summary>
        /// M�todo encargado de Construir el fichero deseado.
        /// </summary>
        /// <param name="sFileName">Nombre del fichero a crear.</param>
        /// <param name="sNameSpace">Espacio de Nombres que debe tomar el fichero.</param>
        /// <param name="sUsing">Using que debe tomar el fichero</param>
        /// <param name="htTables">Tablas a crear.</param>
        /// <param name="sMensaje">Mensaje de error, si se produce uno.</param>
        /// <returns>true, si  operaci�n correcta, false, si error.</returns>
        public static bool CrearFichero(string sFileName, string sNameSpace, Hashtable htTables, out string sMensaje)
        {
            try
            {
                StringBuilder sbFileValue = new StringBuilder(string.Empty);
                StringBuilder sbBuilderValue = new StringBuilder(string.Empty);
                if (!GetLibrariesRegion(ref sbFileValue, out sMensaje)) return (false);
                if (!GetCompilationDirectivesRegion(ref sbFileValue, out sMensaje)) return (false);
                if (!GetBodyRegion(sNameSpace, sbBuilderValue.ToString(), htTables, ref sbFileValue, out sMensaje)) return (false);
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
                sMensaje = MsgManager.LiteralMsg("El par�metro path est� vac�o. O bien path contiene el nombre de un dispositivo del sistema (com1, com2, etc.).");
                return (false);
            }
            catch (DirectoryNotFoundException) 
            {
                sMensaje = MsgManager.LiteralMsg("La ruta de acceso especificada no es v�lida como, por ejemplo, una ruta de una unidad no asignada.");
                return (false);
            }
            catch (PathTooLongException)
            {
                sMensaje = MsgManager.LiteralMsg("La ruta de acceso especificada, el nombre de archivo o ambos superan la longitud m�xima definida por el sistema. Por ejemplo, en las plataformas basadas en Windows, las rutas de acceso deben ser inferiores a 248 caracteres y los nombres de archivo deben ser inferiores a 260 caracteres.");
                return (false);
            }
            catch (IOException)
            {
                sMensaje = MsgManager.LiteralMsg("Path incluye una sintaxis no correcta o no v�lida para el nombre de archivo, el nombre de directorio o la etiqueta de volumen.");
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

        #endregion

        #endregion
    }
}
