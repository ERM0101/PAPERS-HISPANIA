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
            IEnumerable<ExcelColumnInfo> columns_infos = LoadColumnInfosByType( typeof( TRow ) );

            columns_infos = columns_infos.OrderBy( i => i.Position );

            DataTable result = BuildDataTable<TRow>( _This, columns_infos );

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
        private static DataTable BuildDataTable<TRow>( IEnumerable<TRow> dataStrean, IEnumerable<ExcelColumnInfo> columnsInfo )
        {
            DataTable result = new DataTable();

            // Generate columns
            foreach(ExcelColumnInfo column_info in columnsInfo)
            {
                Type column_yupe = column_info.PropertyInfo.PropertyType;
                DataColumn column = new DataColumn( column_info.Name, column_yupe );
                result.Columns.Add( column );
            }

            // Export data
            foreach(TRow row_data in dataStrean)
            {
                DataRow new_row = result.NewRow();
                foreach(ExcelColumnInfo column_info in columnsInfo)
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
        private static IEnumerable<ExcelColumnInfo> LoadColumnInfosByType( Type type )
        {
            List<ExcelColumnInfo> result = new List<ExcelColumnInfo>();

            PropertyInfo[] properties = type.GetProperties();
            
            IEnumerable<PropertyInfo> excel_properties = properties.Where( p => p.GetCustomAttribute<ExcelColumnAttribute>() != null );

            foreach( PropertyInfo excel_prop in excel_properties)
            {
                ExcelColumnAttribute attribute = excel_prop.GetCustomAttribute<ExcelColumnAttribute>();

                ExcelColumnInfo column_info = new ExcelColumnInfo( attribute.Name, attribute.Position, excel_prop );
                result.Add( column_info );
            }

            return result;
        }

        #endregion

        #region classes

        /// <summary>
        /// 
        /// </summary>
        private class ExcelColumnInfo
        {
            /// <summary>
            /// 
            /// </summary>
            public string Name
            {
                get;
            } 

            /// <summary>
            /// 
            /// </summary>
            public int Position
            {
               get;
            }

            /// <summary>
            /// 
            /// </summary>
            public PropertyInfo PropertyInfo
            {
                get;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="name"></param>
            /// <param name="position"></param>
            /// <param name="propertyInfo"></param>
            public ExcelColumnInfo( string name, int position, PropertyInfo propertyInfo )
            {            
                this.Name = name;
                this.Position = position;
                this.PropertyInfo = propertyInfo;
            }
        }
        #endregion
    }
}
