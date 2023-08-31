using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MBCode.Framework.OFFICE
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExcelColumnInfoEX
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_This"></param>
        /// <param name="term"></param>
        /// <param name="ignoredCase"></param>
        /// <param name="trimAll"></param>
        /// <param name="throwIfNotFound"></param>
        /// <returns></returns>
        /// <exception cref="KeyNotFoundException"></exception>
        public static ExcelColumnInfo FindByName( this IEnumerable<ExcelColumnInfo> _This,
                                                  string term,
                                                  bool ignoredCase = false,
                                                  bool trimAll = true,
                                                  bool throwIfNotFound = false )
        {
            ExcelColumnInfo result = null;

            string normalized_term = ( trimAll ? term.Trim() : term );

            if(!string.IsNullOrEmpty( normalized_term ))
            {
                foreach(ExcelColumnInfo column_info in _This)
                {
                    if(string.Compare( column_info.Name, normalized_term, ignoredCase ) == 0)
                    {
                        result = column_info;
                        break;
                    }
                }
            }

            if(result == null && throwIfNotFound)
            {
                throw new KeyNotFoundException( $"ColumnInfo with name '{term}' not found." );
            }

            return result;
        }
    }
}
