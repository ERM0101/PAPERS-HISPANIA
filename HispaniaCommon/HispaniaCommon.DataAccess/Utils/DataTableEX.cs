using MBCode.Framework.OFFICE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HispaniaCommon.DataAccess.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class DataTableEX
    {


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRow"></typeparam>
        /// <param name="_This"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<TRow>( this IEnumerable<TRow> _This )
        {
            IEnumerable<ExcelColumnInfo> columns_infos = typeof( TRow ).LoadColumnInfos();
                        
            DataTable result = ToDataTable<TRow>( _This, columns_infos );

            return result;
        }

        #region utils

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TRow"></typeparam>
        /// <param name="dataStrean"></param>
        /// <param name="columnsInfo"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<TRow>( this IEnumerable<TRow> _This, 
                                                   IEnumerable<ExcelColumnInfo> columnsInfo )
        {
            if(columnsInfo is null)
            {
                throw new ArgumentNullException( nameof( columnsInfo ) );
            }

            IEnumerable<ExcelColumnInfo> columns_info = columnsInfo.OrderBy( i => i.Position );

            DataTable result = new DataTable();

            // Generate columns
            foreach(ExcelColumnInfo column_info in columns_info)
            {
                Type column_type = column_info.DataType();

                DataColumn column = new DataColumn( column_info.Name, column_type );

                result.Columns.Add( column );
            }

            // Export data
            foreach(TRow row_data in _This )
            {
                DataRow new_row = result.NewRow();
                foreach(ExcelColumnInfo column_info in columns_info )
                {
                    object value = column_info.PropertyInfo.GetValue( row_data );
                    new_row[ column_info.Name ] = value;
                }
                result.Rows.Add( new_row );
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static IEnumerable<ExcelColumnInfo> LoadColumnInfos( this Type _This )
        {
            if(_This is null)
            {
                throw new ArgumentNullException( nameof( _This ) );
            }

            List<ExcelColumnInfo> result = new List<ExcelColumnInfo>();

            PropertyInfo[] properties = _This.GetProperties();
            
            IEnumerable<PropertyInfo> excel_properties = properties.Where( p => p.GetCustomAttribute<ExcelColumnAttribute>() != null );

            foreach( PropertyInfo excel_prop in excel_properties)
            {
                ExcelColumnAttribute attribute = excel_prop.GetCustomAttribute<ExcelColumnAttribute>();

                ExcelColumnInfo column_info = new ExcelColumnInfo( attribute.Name, attribute.Position, 
                                                                   attribute.NumberFormat, excel_prop );
                result.Add( column_info );
            }

            return result;
        }

        #endregion
    }
}
