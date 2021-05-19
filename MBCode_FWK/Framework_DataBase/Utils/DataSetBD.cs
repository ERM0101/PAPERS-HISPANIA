#region Librerias usadas por la clase

using MBCode.Framework.Managers.Messages;
using System;
using System.Data;   

#endregion

namespace MBCode.Framework.DataBase.Utils
{
    /// <summary>
    /// Desarrollado por: Alex Moltó Bou  
    /// Fecha última Modificación: Alex Moltó Bou - 05/03/2012
    /// Descripción: clase que proporciona un conjunto de métodos relacionados con el uso de los DataSet en C#. Las 
    ///              funcionalidades que esta clase proporciona son las siguientes:
    /// 
    ///   - DATASETS:
    /// 
    ///            1. Creación de DataSets.
    ///            2. Insertar Tablas en un DataSet.
    ///            3. Compobar la existencia de una Tabla en un DataSet.
    ///            4. Recuperar una Tabla de un DataSet por su Nombre.
    ///            5. Insertar una fila en una de las Tablas de un DataSet.
    ///            6. Definir claves secundarias o foráneas entre dos tablas de un DataSet.
    ///            7. Visualización Gráfica del Contenido de un DataSet.
    ///            8. Métodos de uso interno de la clase
    /// 
    /// </summary>
    public static class DataSetBD
    {
        #region 1. Creación de DataSets

        /// <summary>
        /// Crea un DataSet sin nombre y sin tablas.
        /// </summary>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Si todo correcto se devuelve el DataSet creado, sinó, si error, null</returns>
        public static DataSet Crear(ref string sMensaje)
        {
            try
            {
                return (new DataSet());
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        /// <summary>
        /// Crea un DataSet con el nombre pasado como parámetro.
        /// </summary>
        /// <param name="sNombreDataSet">Nombre que ha de tener el DataSet</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Si todo correcto se devuelve el DataSet creado segun los parámetros sinó, si error, null</returns>
        public static DataSet Crear(string sNombreDataSet, ref string sMensaje)
        {
            try
            {
                if (sNombreDataSet == null) return (Crear(ref sMensaje));
                else return (new DataSet(sNombreDataSet));
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        /// <summary>
        /// Crea un DataSet con la estructura pasada como parámetro.
        /// </summary>
        /// <param name="aTablas">Array de Tablas que ha de contener el nuevo DataSet</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Si todo correcto se devuelve el DataSet creado segun los parámetros sinó, si error, null</returns>
        public static DataSet Crear(DataTable[] aTablas, ref string sMensaje)
        {
            DataSet dsDataSetResult = null;
            int i = 0;
            try
            {
                dsDataSetResult = Crear(ref sMensaje);
                if (aTablas != null)
                {
                    for (i = 0; i < aTablas.Length; i++)
                    {
                        dsDataSetResult.Tables.Add(aTablas[i]);
                    }
                }
                return (dsDataSetResult);
            }
            catch (DuplicateNameException)
            {
                sMensaje = MsgManager.LiteralMsg("I try to add a two tables with the same name in the DataSet.");
                return (null);
            }
            catch (ArgumentNullException)
            {
                sMensaje = MsgManager.LiteralMsg("I try to add a nil table in the DataSet.");
                return (null);
            }
            catch (ArgumentException)
            {
                sMensaje = MsgManager.LiteralMsg("I try to add a table that already exists in the DataSet.");
                return (null);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        /// <summary>
        /// Crea un DataSet con la estructura pasada como parámetro.
        /// </summary>
        /// <param name="sNombreDataSet">Nombre que ha de tener el DataSet</param>
        /// <param name="aTablas">Tablas a insertar en el DataSet resultante</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>Si todo correcto se devuelve el DataSet creado segun los parámetros sinó, si error, null</returns>
        public static DataSet Crear(string sNombreDataSet, DataTable[] aTablas, ref string sMensaje)
        {
            DataSet dsDataSetResult = null;
            try
            {
                if ((dsDataSetResult = Crear(aTablas, ref sMensaje)) != null)
                {
                    if (sNombreDataSet != null) dsDataSetResult.DataSetName = sNombreDataSet;
                    return (dsDataSetResult);
                }
                else return (null);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        #endregion

        #region 2. Inserción de Tablas en un DataSet

        /// <summary>
        /// Insertar un nueva tabla en un DataSet.
        /// </summary>
        /// <param name="dsDataSetDatos">DataSet al que se quiere añadir la tabla</param>
        /// <param name="dtTablaDatos">Tabla sobre la que se quiere realizar la inserción de los datos</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>true, si inserción correcta, false sinó</returns>
        public static bool InsertarTabla(ref DataSet dsDataSetDatos, DataTable dtTablaDatos, ref string sMensaje)
        {
            try
            {
                if (ValidarParametros(dsDataSetDatos, ref sMensaje))
                {
                    if (dtTablaDatos != null) dsDataSetDatos.Tables.Add(dtTablaDatos);
                    return (true);
                }
                else return (false);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        /// <summary>
        /// Insertar un conjunto de Tablas en un DataSet.
        /// </summary>
        /// <param name="dsDataSetDatos">DataSet al que se quieren añadir las tablas</param>
        /// <param name="aTablas">Tablas a Insertar</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>true, si inserción correcta, false sinó</returns>
        public static bool InsertarTabla(ref DataSet dsDataSetDatos, DataTable[] aTablas, ref string sMensaje)
        {
            try
            {
                if (ValidarParametros(dsDataSetDatos, ref sMensaje))
                {
                    foreach (DataTable dtTabla in aTablas)
                    {
                        dsDataSetDatos.Tables.Add(dtTabla);
                    }
                    return (true);
                }
                else return (false);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region 3. Comprobar la existencia de una tabla en un DataSet

        /// <summary>
        /// Comprueba si el DataSet pasado como parámetro contiene la tabla de nombre la
        /// cadena de carácteres pasada como parámetro también.
        /// </summary>
        /// <param name="dsDataSetDatos">DataSet a examinar</param>
        /// <param name="sNombreTabla">Nombre de la Tabla a Buscar</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>true, si existe una tabla de nombre 'sNombreTabla' en el DataSet, false sinó</returns>
        public static bool ContieneTabla(DataSet dsDataSetDatos, string sNombreTabla, ref string sMensaje)   
        {
            try
            {
                if ((dsDataSetDatos == null) || (sNombreTabla == null)) return (false);
                return (dsDataSetDatos.Tables.Contains(sNombreTabla));
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        /// <summary>
        /// Comprueba si el DataSet pasado como parámetro contiene la tabla pasada como parámetro.
        /// </summary>
        /// <param name="dsDataSetDatos">DataSet a examinar</param>
        /// <param name="dtTablaDatos">Tabla a Buscar</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>true, si existe la tabla 'dtTablaDatos' en el DataSet, false sinó</returns>
        public static bool ContieneTabla(DataSet dsDataSetDatos, DataTable dtTablaDatos, ref string sMensaje)   
        {
            try
            {
                if (dtTablaDatos == null) return (false);
                else return (ContieneTabla(dsDataSetDatos, dtTablaDatos.TableName, ref sMensaje));
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region 4. Comprobar la existencia de una tabla en un DataSet

        /// <summary>
        /// Comprueba si el DataSet pasado como parámetro contiene la tabla de nombre la
        /// cadena de carácteres pasada como parámetro también.
        /// </summary>
        /// <param name="dsDataSetDatos">DataSet a examinar</param>
        /// <param name="sNombreTabla">Nombre de la Tabla a Buscar</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>La Tabla, si existe una tabla de nombre 'sNombreTabla' en el DataSet, null sinó</returns>
        public static DataTable ObtenerTabla(DataSet dsDataSetDatos, string sNombreTabla, ref string sMensaje)
        {
            try
            {
                if ((dsDataSetDatos == null) || (sNombreTabla == null)) return (null);
                if (!dsDataSetDatos.Tables.Contains(sNombreTabla)) return (null);
                else return (dsDataSetDatos.Tables[dsDataSetDatos.Tables.IndexOf(sNombreTabla)]);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (null);
            }
        }

        #endregion

        #region 5. Insertar una fila en una de las Tablas de un DataSet

        /// <summary>
        /// Insertar un nuevo registro en la tabla, con los datos pasados como parámetro.
        /// </summary>
        /// <param name="dsInput">DataSet que contiene la Tabla sobre la que se quiere realizar la inserción de los datos</param>
        /// <param name="sTableName">Nombre de la tabla sobre la que se quiere realizar la inserción de los datos</param>
        /// <param name="Datos">Lista con los datos del registro a insertar</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>true, si inserción correcta, false sinó</returns>
        public static bool InsertarFilaTabla(ref DataSet dsInput, string sTableName, Object[] Datos, ref string sMensaje)
        {
            try
            {
                if (ValidarParametros(dsInput, ref sMensaje))
                {
                    int i = 0;
                    DataRow RowInsert = dsInput.Tables[sTableName].NewRow();
                    foreach (DataColumn Col in dsInput.Tables[sTableName].Columns)
                    {
                        RowInsert[Col.ColumnName] = Datos[i];
                        i++;
                    }
                    dsInput.Tables[sTableName].Rows.Add(RowInsert);
                    return (true);
                }
                else return (false);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region 6. Definir claves secundarias o foráneas entre dos tablas de un DataSet

        /// <summary>
        /// Método encargado de crear una Clave Secundaria o Foránea entre dos tablas de un DataSet
        /// </summary>
        /// <param name="dsInput">DataSet a manipular</param>
        /// <param name="sForeignKeyConstraintName">Nombre de la nueva restricción</param>
        /// <param name="sTableNameBase">Nombre de la Tabla que contiene las claves foráneas</param>
        /// <param name="sTableNameLinked">Nombre de la Tabla origen de los datos que actuan de clave foránea</param>
        /// <param name="sColumnNameBase">Nombre de la columna de la tabla con las claves foráneas</param>
        /// <param name="sColumnNameLinked">Nombre de la columna en la tabla origen de los datos</param>
        /// <param name="sMensaje">Mensaje de error, si este se ha producido</param>
        /// <returns>true, si definición correcta, false sinó</returns>
        public static bool CreateConstraint(ref DataSet dsInput,
                                            string sForeignKeyConstraintName,
                                            string sTableNameBase, string sTableNameLinked,
                                            string sColumnNameBase, string sColumnNameLinked,
                                            ref string sMensaje)
        {
            try
            {
                if (ValidarParametros(dsInput, ref sMensaje))
                {
                    //  Obtiene una referencia a las columnas relacionadas.
                        DataColumn oColumnTableBase = dsInput.Tables[sTableNameBase].Columns[sColumnNameBase];
                        DataColumn oColumnTableLinked = dsInput.Tables[sTableNameLinked].Columns[sColumnNameLinked];
                    //  Crea el objeto que contendrá la definición de la clase secundaria.
                        ForeignKeyConstraint oForeignKeyConstraint = new ForeignKeyConstraint(sForeignKeyConstraintName, oColumnTableBase, oColumnTableLinked);
                    //  Establece los valores de las filas relacionadas en DBNull.
                        oForeignKeyConstraint.DeleteRule = Rule.SetNull;
                    //  Elimina o Actualiza las filas modificadas.
                        oForeignKeyConstraint.UpdateRule = Rule.Cascade;
                    //  Los cambios se propagan en cascada a traves de la relación.
                        oForeignKeyConstraint.AcceptRejectRule = AcceptRejectRule.Cascade;
                    //  Añade la Clave secundaria.
                        dsInput.Tables[sTableNameLinked].Constraints.Add(oForeignKeyConstraint);
                        dsInput.EnforceConstraints = true;
                        return (true);
                }
                else return (false);
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
                return (false);
            }
        }

        #endregion

        #region 7. Visualización Gráfica del contenido de un DataSet

        /// <summary>
        /// Muestra el contenido del DataSet especificado
        /// </summary>
        /// <param name="dsDataSetDatos">DataSet del que se quiere ver el contenido</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        public static void VistaGrafica(DataSet dsDataSetDatos, ref string sMensaje)    
        {
            try
            {
                if (ValidarParametros(dsDataSetDatos, ref sMensaje))
                {
                    frmMostrarDatosDataSet frmDatos = new frmMostrarDatosDataSet();
                    string sLeyenda = "Data and Structure of DataSet: " + dsDataSetDatos.DataSetName;
                    if (dsDataSetDatos.Tables.Count != 0) frmDatos.LeyendaFormulario = sLeyenda;
                    else frmDatos.LeyendaFormulario = sLeyenda + " - < DataSet Empty >";
                    frmDatos.DataSetOrigenDatos = dsDataSetDatos;
                    frmDatos.ShowDialog();
                    frmDatos.Dispose();
                }
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
            }
        }

        /// <summary>
        /// Muestra el contenido del DataSet especificado con la Leyenda especificada
        /// </summary>
        /// <param name="dsDataSetDatos">DataSet del que se quiere ver el contenido</param>
        /// <param name="sLeyenda">Leyenda a mostrar en el formulario que muestra los datos</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        public static void VistaGrafica(DataSet dsDataSetDatos, string sLeyenda, ref string sMensaje)    
        {
            try
            {
                if (ValidarParametros(dsDataSetDatos, ref sMensaje))
                {
                    frmMostrarDatosDataSet frmDatos = new frmMostrarDatosDataSet();
                    if (dsDataSetDatos.Tables.Count != 0) frmDatos.LeyendaFormulario = sLeyenda;
                    else frmDatos.LeyendaFormulario = sLeyenda + " - < DataSet Empty >";
                    frmDatos.DataSetOrigenDatos = dsDataSetDatos;
                    frmDatos.ShowDialog();
                    frmDatos.Dispose();
                }
            }
            catch (Exception ex)
            {
                sMensaje = MsgManager.ExcepMsg(ex);
            }
        }

        #endregion

        #region 8. Métodos de uso interno de la clase

        /// <summary>
        /// Realiza todas las comprobaciones necesarias para las operaciones con DataSets
        /// que no sean la de Creación.
        /// </summary>
        /// <param name="dsDataSetDatos">DataSet a testear</param>
        /// <param name="sMensaje">Mensaje de error si se produce.</param>
        /// <returns>true, si datos correctos, false sinó</returns>
        private static bool ValidarParametros(DataSet dsDataSetDatos, ref string sMensaje)
        {
            try
            {
                if (dsDataSetDatos == null)
                {
                    sMensaje = MsgManager.LiteralMsg("Table Not Initialized");
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
    }
}

