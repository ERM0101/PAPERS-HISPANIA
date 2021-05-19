#region Librerias usadas por la clase

using MBCode.Framework.Managers.DataTypes;
using MBCode.Framework.Managers.Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

#endregion

namespace MBCode.Framework.DataBase.Utils
{
    #region Enumerados

    /// <summary>
    /// Define los posibles formatos en los que se pueden almacenar los datos de una tabla.
    /// </summary>
    public enum FormatoSerializacion
    { 
        /// <summary>
        /// Indica que el contenido se guarda en formato XML.
        /// </summary>
        Xml,

        /// <summary>
        /// Indica que el contenido se guarda en formato Binario.
        /// </summary>
        Binario,

        /// <summary>
        /// Indica que no se ha definido ning�n formato.
        /// </summary>
        NO_DEF,
    }

    #endregion

    /// <summary>
    /// Desarrollado por: Alex Molt� Bou  
    /// Fecha �ltima Modificaci�n: Alex Molt� Bou - 25/09/2007
    /// Descripci�n: clase que proporciona un conjunto de m�todos relacionados con el uso de los datos de 
    ///              una tabla  de la  Bases  de  Datos  almacenados  en  mem�ria  mediante  un  DataSet. 
    ///              Las funcionalidades que esta clase proporciona  son las siguientes:
    /// 
    ///   - TABLAS: 
    /// 
    ///            1. Creaci�n de Tablas.
    ///            2. Inserci�n de un registro en una Tabla.
    ///            3. Obtener una lista con los Nombres de los Campos de una Tabla.
    ///            4. Obtener una lista con los Tipos de los Campos de una Tabla.
    ///            5. Obtener una lista con los Campos que forman parte de la clave Primaria de una Tabla.
    ///            6. Obtener el contenido de una Tabla.
    ///            7. Serializar la tabla.
    ///            8. Deserializar la Tabla. (EN PROCESO DE IMPLEMENTACI�N)
    ///            9. Obtener el Esquema Xml de la Estructura de la Tabla.  (NO IMPLEMENTADO)
    ///           10. Borrado de una Tabla.
    ///           11. Copia de tablas.
    ///           12. Ordenaci�n de columnas de una tabla.
    ///           13. Visualizaci�n Gr�fica del Contenido de una Tabla.
    ///          [14. M�todos de uso interno de la Clase]
    /// 
    /// </summary>
    public static class DataTableBD
    {
        #region 1. Creaci�n de Tablas

        /// <summary>
        /// Crea una tabla sin estructura.
        /// </summary>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>null, si error, la tabla creada sin�</returns>
        public static DataTable Crear(ref string sMensaje)
        {
            try
            {
                DataTable dt = new DataTable();
                return (new DataTable());
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        /// <summary>
        /// Crea una tabla sin estructura.
        /// </summary>
        /// <param name="sNombreTabla">Nombre de la Tabla a crear</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>null, si error, la tabla creada segun los par�metros sin�</returns>
        public static DataTable Crear(string sNombreTabla, ref string sMensaje)
        {
            try
            {
                if (sNombreTabla == null) return (Crear(ref sMensaje));
                else return (new DataTable(sNombreTabla));
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        /// <summary>
        /// Crea una tabla con la estructura pasada como par�metro.
        /// </summary>
        /// <param name="sNombreTabla">Nombre que ha de tener la tabla</param>
        /// <param name="NombreCampos">Array con el nombre de los diferentes campos de la tabla</param>
        /// <param name="TiposCampos">Array con los tipos de los diferentes campos de la tabla</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>null, si error, la tabla creada segun los par�metros sin�</returns>
        public static DataTable Crear(string sNombreTabla, string[] NombreCampos, Type[] TiposCampos, ref string sMensaje)
        {
            DataTable dtTableResult;
            int i = 0;

            if (!ValidarParametros(NombreCampos, TiposCampos, ref sMensaje))
            {
                sMensaje = MsgManager.LiteralMsg("Error, incorrect params.");
                return (null);
            }
            dtTableResult = Crear(sNombreTabla, ref sMensaje);
            try
            {
                for (i = 0; i < NombreCampos.Length; i++)
                {
                    dtTableResult.Columns.Add(NombreCampos[i].ToUpper(), TiposCampos[i]);
                }
                return (dtTableResult);
            }
            catch (DuplicateNameException ex)
            {
                dtTableResult.Columns.Clear();
                dtTableResult.Dispose();
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
            catch (InvalidExpressionException ex)
            {
                dtTableResult.Columns.Clear();
                dtTableResult.Dispose();
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        /// <summary>
        /// Crea una tabla con la estructura pasada como par�metro.
        /// </summary>
        /// <param name="sNombreTabla">Nombre que ha de tener la tabla</param>
        /// <param name="NombreCampos">Array con el nombre de los diferentes campos de la tabla</param>
        /// <param name="TiposCampos">Array con los tipos de los diferentes campos de la tabla</param>
        /// <param name="sPrimaryKey">Array con los campos de la tabla que forman parte de la Primary Key</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>null, si error, la tabla creada segun los par�metros sin�</returns>
        public static DataTable Crear(string sNombreTabla, string[] NombreCampos, Type[] TiposCampos, string[] sPrimaryKey, ref string sMensaje)
        {
            DataTable dtTableResult;
            int i = 0, j = 0;

            if (!ValidarParametros(NombreCampos, TiposCampos, ref sMensaje))
            {
                sMensaje = MsgManager.LiteralMsg("Error, incorrect params.");
                return (null);
            }
            dtTableResult = Crear(sNombreTabla, ref sMensaje);
            try
            {
                for (i = 0; i < NombreCampos.Length; i++)
                {
                    dtTableResult.Columns.Add(NombreCampos[i].ToUpper(), TiposCampos[i]);
                }

                DataColumn[] dcPrimaryKey = new DataColumn[sPrimaryKey.Length];
                for (j = 0; j < sPrimaryKey.Length; j++)
                {
                    dcPrimaryKey[j] = dtTableResult.Columns[sPrimaryKey[j].ToString()];
                }
                dtTableResult.PrimaryKey = dcPrimaryKey;

                return (dtTableResult);
            }
            catch (DuplicateNameException ex)
            {
                dtTableResult.Columns.Clear();
                dtTableResult.Dispose();
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
            catch (InvalidExpressionException ex)
            {
                dtTableResult.Columns.Clear();
                dtTableResult.Dispose();
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        #endregion

        #region 2. Inserci�n de Registros en una Tabla

        /// <summary>
        /// Insertar un nuevo registro en la tabla, con los datos pasados como par�metro.
        /// </summary>
        /// <param name="dtTablaDatos">Tabla sobre la que se quiere realizar la inserci�n de los datos</param>
        /// <param name="Datos">Lista con los datos del registro a insertar</param>
        /// <returns>true, si inserci�n correcta, false sin�</returns>
        public static bool InsertarFila(ref DataTable dtTablaDatos, Object[] Datos, ref string sMensaje)
        {
            try
            {
                DataRow RowInsert;

                if (ValidarParametros(dtTablaDatos, ref sMensaje))
                {
                    int i = 0;
                    RowInsert = dtTablaDatos.NewRow();
                    foreach (DataColumn Col in dtTablaDatos.Columns)
                    {
                        RowInsert[Col.ColumnName] = Datos[i];
                        i++;
                    }
                    dtTablaDatos.Rows.Add(RowInsert);
                    return (true);
                }
                else
                {
                    sMensaje = MsgManager.LiteralMsg("Error, incorrect params.");
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

        #region 3. Obtener una lista con los Nombres de los Campos de una Tabla

        /// <summary>
        /// Devuelve una lista con los nombres de los campos de una tabla.
        /// </summary>
        /// <param name="dtTablaDatos">Tabla a tratar</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Array de strings con los nombres de los campos de la tabla pasada como par�metro o null si error</returns>
        public static string[] ObtenerNombresCampos(DataTable dtTablaDatos, ref string sMensaje)    
        {
            try
            {
                string[] aListaCampos = null;

                if (ValidarParametros(dtTablaDatos, ref sMensaje))
                {
                    aListaCampos = new string[dtTablaDatos.Columns.Count];
                    int i = 0;
                    foreach (DataColumn Col in dtTablaDatos.Columns)
                    {
                        aListaCampos[i] = Col.ColumnName;
                        i++;
                    }
                    return (aListaCampos);
                }
                else
                {
                    sMensaje = MsgManager.LiteralMsg("Error, incorrect params.");
                    return (null);
                }
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        #endregion

        #region 4. Obtener una lista con los Tipos de los Campos de una Tabla

        /// <summary>
        /// Devuelve una lista con los tipos de los campos de una tabla.
        /// <param name="dtTablaDatos">Tabla a tratar</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Devuelve un array con los tipos de los campos de la tabla pasada como par�metro o null si error.</returns>
        public static Type[] ObtenerTiposCampos(DataTable dtTablaDatos, ref string sMensaje)    
        {
            try
            {
                Type[] aTipoCampos = null;

                if (ValidarParametros(dtTablaDatos, ref sMensaje))
                {
                    aTipoCampos = new Type[dtTablaDatos.Columns.Count];
                    int i = 0;
                    foreach (DataColumn Col in dtTablaDatos.Columns)
                    {
                        aTipoCampos[i] = Col.DataType;
                        i++;
                    }
                    return (aTipoCampos);
                }
                else
                {
                    sMensaje = MsgManager.LiteralMsg("Error, incorrect params.");
                    return (null);
                }
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }


        #endregion

        #region 5. Obtener una lista con los Campos que forman parte de la clave Primaria de una Tabla

        /// <summary>
        /// Devuelve una lista con los nombres de los campos que forman parte de la clave primaria de una tabla.
        /// <param name="dtTablaDatos">Tabla a tratar</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Array de strings con los nombres de los campos de la tabla pasada como par�metro o null si error</returns>
        public static bool ObtenerNombresCamposClavePrimaria(DataTable dtTablaDatos, out List<string> aListaCampos, ref string sMensaje)
        {
            aListaCampos = new List<string>();
            try
            {
                if (ValidarParametros(dtTablaDatos, ref sMensaje))
                {
                    if (dtTablaDatos.PrimaryKey != null)
                    {
                        foreach (DataColumn Col in dtTablaDatos.PrimaryKey)
                        {
                            aListaCampos.Add(Col.ColumnName);
                        }
                    }
                    return (true);
                }
                else
                {
                    sMensaje = MsgManager.LiteralMsg("Error, incorrect params.");
                    return (false);
                }
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        /// <summary>
        /// Devuelve una lista con los nombres de los campos que forman parte de la clave primaria de una tabla.
        /// <param name="dtTablaDatos">Tabla a tratar</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Array de strings con los nombres de los campos de la tabla pasada como par�metro o null si error</returns>
        public static string[] ObtenerNombresCamposClavePrimaria(DataTable dtTablaDatos, ref string sMensaje)    
        {
            try
            {
                string[] aListaCampos = null;

                if (ValidarParametros(dtTablaDatos, ref sMensaje))
                {
                    if ((dtTablaDatos.PrimaryKey == null) || (dtTablaDatos.PrimaryKey.Length == 0)) aListaCampos = new string[] { };
                    else
                    {
                        aListaCampos = new string[dtTablaDatos.PrimaryKey.Length];
                        int i = 0;
                        foreach (DataColumn Col in dtTablaDatos.PrimaryKey)
                        {
                            aListaCampos[i] = Col.ColumnName;
                            i++;
                        }
                    }
                    return (aListaCampos);
                }
                else
                {
                    sMensaje = MsgManager.LiteralMsg("Error, incorrect params.");
                    return (null);
                }
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        /// <summary>
        /// Devuelve una lista con los nombres de los campos que forman parte de la clave primaria de una tabla.
        /// Tambi�n devuelve los tipos de las columnas que la forman.
        /// <param name="dtTablaDatos">Tabla a tratar</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Array de strings con los nombres de los campos de la tabla pasada como par�metro o null si error</returns>
        public static string[] ObtenerNombresCamposClavePrimaria(DataTable dtTablaDatos,out Type[] aListaTipos, ref string sMensaje)
        {
            string[] aListaCampos = null;
            aListaTipos = null;
            try
            {
                if (ValidarParametros(dtTablaDatos, ref sMensaje))
                {
                    if ((dtTablaDatos.PrimaryKey == null) || (dtTablaDatos.PrimaryKey.Length == 0)) aListaCampos = new string[] { };
                    else
                    {
                        aListaCampos = new string[dtTablaDatos.PrimaryKey.Length];
                        aListaTipos = new Type[dtTablaDatos.PrimaryKey.Length];
                        int i = 0;
                        foreach (DataColumn Col in dtTablaDatos.PrimaryKey)
                        {
                            aListaCampos[i] = Col.ColumnName;
                            aListaTipos[i] = Col.DataType;
                            i++;
                        }
                    }
                    return (aListaCampos);
                }
                else
                {
                    sMensaje = MsgManager.LiteralMsg("Error, incorrect params.");
                    return (null);
                }
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        #endregion

        #region 6. Obtener el contenido de una Tabla

        /// <summary>
        /// Obtiene un cadena de car�cteres con el contenido de la tabla pasada como
        /// par�metro.
        /// </summary>
        /// <param name="dtTablaDatos">Tabla a inspeccionar</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Cadena de car�cteres con el contenido de la tabla o null si error</returns>
        public static string ObtenerRegistros(DataTable dtTablaDatos, ref string sMensaje)
        {
            try
            {
                StringBuilder sInfo = new StringBuilder(string.Empty);

                if (ValidarParametros(dtTablaDatos, ref sMensaje))
                {
                    for (int i = 0; i < dtTablaDatos.Rows.Count; i++)
                    {
                        for (int j = 0; j < dtTablaDatos.Columns.Count; j++)
                        {
                            sInfo.Append("\t" + dtTablaDatos.Rows[i][j].ToString());
                        }
                        sInfo.Append("\r\n");
                    }
                    return (sInfo.ToString());
                }
                else 
                {
                    sMensaje = MsgManager.LiteralMsg("Error, incorrect params.");
                    return (null);
                }
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        #endregion

        #region 7. Serializar la Tabla

        /// <summary>
        /// Almacena los datos de la tabla pasada como par�metro en un fichero con el formato
        /// y el nombre indicados.
        /// </summary>
        /// <param name="dtTablaDatos">Tabla a serializar</param>
        /// <param name="sPathFichero">Nombre del fichero, sin extensi�n, en el que se quieren almacenar los datos</param>
        /// <param name="oFormato">Formato con el que se escriben los datos en el fichero</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>true si operaci�n correcta, false si error</returns>
        public static bool Serializar(DataTable dtTablaDatos, string sPathFichero, FormatoSerializacion oFormato, ref string sMensaje)
        {
            try
            {
                if (ValidarParametros(dtTablaDatos, ref sMensaje) && (oFormato != FormatoSerializacion.NO_DEF))
                {
                    switch (oFormato)
                    {
                        case FormatoSerializacion.Xml:
                             return (SerializarXml(dtTablaDatos, sPathFichero, ref sMensaje));
                        case FormatoSerializacion.Binario:
                             return (SerializarBinario(dtTablaDatos, sPathFichero, ref sMensaje));
                        default:
                             sMensaje = MsgManager.LiteralMsg("Error, unknown format.");
                             return (false);
                    }
                }
                else
                {
                    sMensaje = MsgManager.LiteralMsg("Error, incorrect params.");
                    return (false);
                }
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #region Formato XML

        /// <summary>
        /// Almacena los datos de la tabla pasada como par�metro en un fichero con formato Xml
        /// y con el nombre indicado.
        /// </summary>
        /// <param name="dtTablaDatos">Tabla a serializar</param>
        /// <param name="sPathFichero">Nombre del fichero en el que se quieren almacenar los datos</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>true si operaci�n correcta, false si error</returns>
        private static bool SerializarXml(DataTable dtTablaDatos, string sPathFichero, ref string sMensaje)    
        {
            sMensaje = "";
            try
            {
                //  Borra el anterior fichero hist�rico.
                    FileInfo fiTemp = new FileInfo(sPathFichero);
                //  Comprueba si el fichero existe y si es as� lo renombra, a�adiendo .old al nombre
                //  de fichero.  Si ya existe un fichero de nombre .old este es borrado y se crea un
                //  nuevo .old.
                    if (fiTemp.Exists)
                    {
                        fiTemp.MoveTo(sPathFichero + " - " + DateTimeManager.FechaAString(DateTime.Now, FormatoFecha.FechaCorta, out sMensaje) + ".old");
                    }
                //  Serializa los Datos en formato XML adjuntando el esquema.
                    dtTablaDatos.WriteXml(sPathFichero, XmlWriteMode.WriteSchema);
                //  Operaci�n Finalizada
                    return (true);
            }
            catch (Exception ex)
            {
                sMensaje += "Ejecuci�n incorrecta del m�todo: SerializarXml \r\n" +
                            ex.Message + "\r\n|";
                return (false);
            }
        }

        #endregion

        #region Formato Binario

        /// <summary>
        /// Almacena los datos de la tabla pasada como par�metro en un fichero con formato Binario
        /// y con el nombre indicado.
        /// </summary>
        /// <param name="dtTablaDatos">Tabla a serializar</param>
        /// <param name="sPathFichero">Nombre del fichero, sin extensi�n, en el que se quieren almacenar los datos</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>true si operaci�n correcta, false si error</returns>
        private static bool SerializarBinario(DataTable dtTablaDatos, string sPathFichero, ref string sMensaje)    
        {
            sMensaje = "";
            try
            {
                //  Borra el anterior fichero hist�rico.
                    FileInfo fiTemp = new FileInfo(sPathFichero + ".dat");
                //  Comprueba si el fichero existe y si es as� lo renombra, a�adiendo .old al nombre
                //  de fichero.  Si ya existe un fichero de nombre .old este es borrado y se crea un
                //  nuevo .old.
                    if (fiTemp.Exists)
                    {
                        FileInfo fiTempOld = new FileInfo(sPathFichero + ".dat.old");
                        if (fiTempOld.Exists) fiTempOld.Delete();
                        FileInfo fiTempXsl = new FileInfo(sPathFichero + "-dat.xsl");
                        if (fiTempXsl.Exists)
                        {
                            FileInfo fiTempXslOld = new FileInfo(sPathFichero + "-dat.xsl.old");
                            if (fiTempXslOld.Exists) fiTempXslOld.Delete();
                            fiTempXsl.MoveTo(sPathFichero + "-dat.xsl.old");
                        }
                        fiTemp.MoveTo(sPathFichero + ".dat.old");
                    }
                //  Crea el nuevo fichero
                    FileStream fsEscritor = new FileStream(sPathFichero + ".dat", FileMode.Create);
                //  Crea el formateador
                    BinaryFormatter bfFormateador = new BinaryFormatter();
                //  Serializa el contenido de la Tabla.
                    SortedList slFilas = new SortedList(dtTablaDatos.Rows.Count);
                    object[] aFila = new object[dtTablaDatos.Columns.Count];
                    long lIndice = 1;
                    foreach (DataRow drTemp in dtTablaDatos.Rows)
                    {
                        for (int i = 0; i < dtTablaDatos.Columns.Count; i++)
                        {
                            aFila[i] = drTemp[i];
                        }
                        slFilas.Add(lIndice++, aFila);
                    }
                    bfFormateador.Serialize(fsEscritor, slFilas);
                //  Crea tambi�n el esquema xml que permitir� en caso necesario recrear la estructura
                //  de la tabla que contiene los datos grabados.
                    dtTablaDatos.WriteXmlSchema(sPathFichero + "-dat.xsl");
                //  Operaci�n Finalizada
                    return (true);
            }
            catch (Exception ex)
            {
                sMensaje += "Ejecuci�n incorrecta del m�todo: SerializarBinario \r\n" +
                            ex.Message + "\r\n|";
                return (false);
            }
        }

        #endregion

        #endregion

        #region 8. Deserializar la Tabla (EN PROCESO DE IMPLEMENTACI�N)

        /// <summary>
        /// Devuelve la tabla pasada como par�metro con los datos contenidos en el fichero con el nombre
        /// indicado.
        /// </summary>
        /// <param name="sPathFichero">Nombre del fichero completo, con extensi�n, que contiene los datos a leer</param>
        /// <param name="oFormato">Formato con el que se leen los datos del fichero</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Tabla donde almacenar los datos, si operaci�n correcta, null, si error</returns>
        public static DataTable Deserializar(string sPathFichero, FormatoSerializacion oFormato, ref string sMensaje)
        {
            try
            {
                switch (oFormato)
                {
                    case FormatoSerializacion.Xml:
                         return (DeserializarXml(sPathFichero, ref sMensaje));
                    //case FormatoSerializacion.Binario:
                    //    return (DeserializarBinario(sPathFichero));
                    default:
                         sMensaje = MsgManager.LiteralMsg("Error, incorrect params.");
                         return (null);
                 }
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        #region Formato XML

        /// <summary>
        /// Almacena los datos de la tabla pasada como par�metro en un fichero con formato Xml
        /// y con el nombre indicado.
        /// </summary>
        /// <param name="sPathFichero">Nombre del fichero completo, con extensi�n, que contiene los datos a leer</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Tabla donde almacenar los datos, si operaci�n correcta, null, si error</returns>
        private static DataTable DeserializarXml(string sPathFichero, ref string sMensaje)
        {
            try
            {
                //  Comprueba que exista el fichero a leer.
                    FileInfo fiTemp = new FileInfo(sPathFichero);
                    if (!fiTemp.Exists) return (null);
                //  Crea la tabla a retornar.
                    DataTable dtResult = new DataTable();
                //  Deserializa los Datos almacenados en formato XML. O sea tanto el esquema de la tabla,
                //  que contiene su estructura, como los datos que contenia en el momento en que se creo
                //  el fichero Xml que ahora se lee.
                    dtResult.ReadXml(sPathFichero);
                //  Asigna
                //  Operaci�n Finalizada
                    return (dtResult);
            }
            catch (Exception ex)
            {
                sMensaje += "Ejecuci�n incorrecta del m�todo: DeserializarXml \r\n" +
                            ex.Message + "\r\n|";
                return (null);
            }
        }

        #endregion

        #endregion

        #region 9. Obtener el Esquema Xml de la Estructura de la Tabla.  (NO IMPLEMENTADO)

        #endregion

        #region 10. Borrado de Tablas

        /// <summary>
        /// Borra una Tabla, previamente creada.
        /// </summary>
        /// <param name="dtTablaDatos">Tabla a eliminar</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>false, si error, true sin�</returns>
        public static bool Borrar(DataTable dtTablaDatos, ref string sMensaje)    
        {
            try
            {
                if (dtTablaDatos != null)
                {
                    dtTablaDatos.Rows.Clear();
                    dtTablaDatos.Dispose();
                    dtTablaDatos = null;
                    return (true);
                }
                else
                {
                    sMensaje = MsgManager.LiteralMsg("Error, incorrect params.");
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
        
        #region 11. Copia de tablas

        /// <summary>
        /// Devuelve una tabla copia.
        /// </summary>
        /// <param name="dtTablaDatos"></param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns></returns>
        public static DataTable Copiar(DataTable dtTablaDatos, ref string sMensaje)
        {
            try
            {
                if (ValidarParametros(dtTablaDatos, ref sMensaje))
                {
                    DataTable dtSalida = dtTablaDatos.Clone();
                    dtSalida.TableName = dtTablaDatos.TableName;
                    foreach (DataRow dr in dtTablaDatos.Rows)
                    {
                        DataTableBD.InsertarFila(ref dtSalida, dr.ItemArray, ref sMensaje);
                    }
                    return (dtSalida);
                }
                else
                {
                    sMensaje = MsgManager.LiteralMsg("Error, incorrect params.");
                    return (null);
                }
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        #endregion
        
        #region 12. Ordenacion de columnas

        /// <summary>
        /// Devuelve una tabla copia con el orden de columnas especificado.
        /// </summary>
        /// <param name="dtTablaDatos"></param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns></returns>
        public static DataTable Ordenar(DataTable dtTablaDatos,int[] IndicesColumnas, ref string sMensaje)    
        {
            try
            {
                if (dtTablaDatos.Columns.Count != IndicesColumnas.Length) 
                {
                    sMensaje = MsgManager.LiteralMsg("Error, the board at the outset has an I number of different fields to that of entrance.");
                    return (null);
                }
                if (ValidarParametros(dtTablaDatos, ref sMensaje))
                {
                    string[] Campos = new string[IndicesColumnas.Length];
                    Campos = ObtenerNombresCampos(dtTablaDatos, ref sMensaje);
                    Type[] Tipos = new Type[IndicesColumnas.Length];
                    Tipos = ObtenerTiposCampos(dtTablaDatos, ref sMensaje);
                    DataTable dtTablaSalida = new DataTable();
                    for (int i = 0; i < IndicesColumnas.Length; i++)
                    {
                        dtTablaSalida.Columns.Add(Campos[IndicesColumnas[i]], (Type)Tipos[IndicesColumnas[i]]);
                    }
                    for (int i = 0; i < dtTablaDatos.Rows.Count; i++)
                    {
                        dtTablaSalida.Rows.Add(dtTablaSalida.NewRow());
                        dtTablaSalida.Rows[i].BeginEdit();
                        foreach (DataColumn dt in dtTablaDatos.Columns)
                        {
                            dtTablaSalida.Rows[i][dt.ColumnName] = dtTablaDatos.Rows[i][dt.ColumnName];
                        }
                        dtTablaSalida.Rows[i].EndEdit();
                    }
                    return (dtTablaSalida);
                }
                else
                {
                    sMensaje = MsgManager.LiteralMsg("Error, incorrect params.");
                    return (null);
                }
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        #endregion
        
        #region 13. Visualizaci�n Gr�fica del contenido de una Tabla

        /// <summary>
        /// Muestra el contenido de la Tabla especificada
        /// </summary>
        /// <param name="dtTablaDatos">Tabla a mostrar</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        public static bool VistaGrafica(DataTable dtTablaDatos, ref string sMensaje)    
        {
            try
            {
                if (ValidarParametros(dtTablaDatos, ref sMensaje))
                {
                    frmMostrarDatosTabla frmDatos = new frmMostrarDatosTabla();
                    frmDatos.TablaOrigenDatos = dtTablaDatos;
                    frmDatos.LeyendaFormulario = "Data and Structure of the Table: " + dtTablaDatos.TableName;
                    frmDatos.ShowDialog();
                    frmDatos.Dispose();
                    return (true);
                }
                else
                {
                    sMensaje = MsgManager.LiteralMsg("Error, incorrect parameters.");
                    return (false);
                }
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        /// <summary>
        /// Muestra el contenido de la Tabla especificada con la Leyenda especificada.
        /// </summary>
        /// <param name="dtTablaDatos">Tabla a mostrar</param>
        /// <param name="sLeyenda">Leyenda a mostrar por parte del formulario que muestra la tabla</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        public static bool VistaGrafica(DataTable dtTablaDatos, string sLeyenda, ref string sMensaje)    
        {
            try
            {
                if (ValidarParametros(dtTablaDatos, ref sMensaje))
                {
                    frmMostrarDatosTabla frmDatos = new frmMostrarDatosTabla();
                    frmDatos.TablaOrigenDatos = dtTablaDatos;
                    frmDatos.LeyendaFormulario = sLeyenda;
                    frmDatos.ShowDialog();
                    frmDatos.Dispose();
                    return (true);
                }
                else
                {
                    sMensaje = MsgManager.LiteralMsg("Error, incorrect parameters.");
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

        #region 14. M�todos de uso interno de la clase

        #region Creaci�n de Tablas

        /// <summary>
        /// Realiza Todas las comprobaciones necesarias para la Operaci�n CrearTabla.
        /// </summary>
        /// <param name="NombreCampos">Array con el nombre de los diferentes campos de la tabla</param>
        /// <param name="TiposCampos">Array con los tipos de los diferentes campos de la tabla</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>true, si datos correctos, false sin�</returns>
        private static bool ValidarParametros(string[] NombreCampos, Type[] TiposCampos, ref string sMensaje)
        {
            try
            {
                if (NombreCampos.Length != TiposCampos.Length) 
                {
                    sMensaje = MsgManager.LiteralMsg("Error, there is not the same number of fields that of types.");
                    return (false);
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

        #region Resto de Operaciones con Tablas

        /// <summary>
        /// Realiza Todas las comprobaciones necesarias para las operaciones con tablas
        /// que no sean la de Creaci�n.
        /// </summary>
        /// <param name="dtTablaDatos">Tabla a testear</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>true, si datos correctos, false sin�</returns>
        private static bool ValidarParametros(DataTable dtTablaDatos, ref string sMensaje)
        {
            try
            {
                if (dtTablaDatos == null) 
                {
                    sMensaje = MsgManager.LiteralMsg("Error, table not intialized.");
                    return (false);
                }
                else return (true);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #endregion
    }
}
