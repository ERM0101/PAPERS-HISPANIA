#region Librerias usadas por la clase

using System;
using System.Collections.Generic;
using System.Linq;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using MBCode.Framework.Managers.Messages;
using MBCode.Framework.OFFICE;
using Microsoft.Office.Interop.Excel;
using System.Globalization;

#endregion

namespace MBCode.Framework.Managers
{
    /// <summary>
    /// Autor: Alejandro Moltó Bou.
    /// Fecha: 24/10/2017.
    /// Descripción: clase que controla l'enviament de missatges Outlook.
    /// </summary>
    public static class ExcelManager
    {
        /// <summary>
        ///  Export DataTable into an excel file with field names in the header line
        ///          - Save excel file without ever making it visible if filepath is given
        ///          - Don't save excel file, just make it visible if no filepath is given
        ///          
        /// https://stackoverflow.com/questions/8207869/how-to-export-datatable-to-excel
        /// https://code.msdn.microsoft.com/office/How-to-Export-DataGridView-62f1f8ff
        /// </summary>
        /// <param name="dtData">Datatable with data to export to Excel.</param>
        /// <param name="FileExcelPath">Filename associated of new Excel.</param>
        public static void ExportToExcel( this System.Data.DataTable dtData, string WorsheetName = "Data", 
                                          string FileExcelPath = null,
                                          IEnumerable<ExcelColumnInfo> columnsInfos = null )
        {
            try
            {
                //  Validate the Input Data
                    if ((dtData is null) || (dtData.Columns.Count == 0))
                    {
                        throw new ArgumentException("Error, al crear l'excel. L'origen de dades està buit o és nul.\r\n");
                    }
                //  Validate the FileName
                    if (string.IsNullOrEmpty(FileExcelPath))
                    {
                        throw new ArgumentException(
                            string.Format("Error, al crear l'excel. El nom del fitxer excel a crear '{0}' és incorrecte.\r\n",
                                          FileExcelPath));
                    }
                //  Load Excel, and create a new workbook.
                    Excel._Application excelApp = new Excel.Application();
                    excelApp.Workbooks.Add();
                //  Single worksheet
                    Excel._Worksheet workSheet = excelApp.ActiveSheet;
                    workSheet.Name = WorsheetName;
                //  Columns : create headings.
                    for (var i = 0; i < dtData.Columns.Count; i++)
                    {
                        workSheet.Cells[1, i + 1] = dtData.Columns[i].ColumnName.ToUpper();
                        ((Excel.Range) workSheet.Cells[1, i + 1]).Interior.Color = ColorTranslator.ToOle(Color.LightGray);
                }
                //  Rows : insert data.
                    for (var row_idx = 0; row_idx < dtData.Rows.Count; row_idx++ )
                    {
                        for (var col_idx = 0; col_idx < dtData.Columns.Count; col_idx++ )
                        {                                
                            DataColumn column = dtData.Columns[ col_idx ];
                                
                            ExcelColumnInfo column_info = columnsInfos.FindByName( column.ColumnName );
                            Excel.Range range = (Excel.Range)workSheet.Cells[ row_idx + 2, col_idx + 1 ];

                            var value = dtData.Rows[ row_idx ][ col_idx ];                            

                            if(column_info != null && !string.IsNullOrEmpty( column_info.NumberFormat ))
                            {
                                //TODO: format datetime values before printing
                                range.NumberFormat = column_info.NumberFormat;
                            } 

                            range.Value = value;
                        }
                    }
                //  Check file path
                    //if (!string.IsNullOrEmpty(FileExcelPath))
                    //{
                        try
                        {
                            workSheet.SaveAs(FileExcelPath);
                            excelApp.Quit();
                        }
                        catch (Exception ex)
                        {
                            throw new ArgumentException(
                                 string.Format("Error, al crear l'excel. El fitxer no s'ha pogut guardar, comprobi el nom.\r\nDetalls: {0}",
                                                MsgManager.ExcepMsg(ex)));
                        }
                    //}
                    //else
                    //{ 
                    //    //  No file path is given
                    //        excelApp.Visible = true;
                    //}
            }
            catch (Exception ex)
            {
                throw new Exception("Error, al crear l'excel.\r\nDetalls: " + MsgManager.ExcepMsg(ex));
            }
        }
    }
}
