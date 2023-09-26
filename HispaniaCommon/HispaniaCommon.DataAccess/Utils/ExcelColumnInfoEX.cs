using MBCode.Framework.OFFICE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HispaniaCommon.DataAccess.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExcelColumnInfoEX
    {
        /// <summary>
        /// Get .NET type
        /// </summary>
        /// <param name="_This"></param>
        /// <returns></returns>
        public static Type DataType( this ExcelColumnInfo _This )
        {
            Type result = _This.PropertyInfo.PropertyType;

            Type nullable_type = Nullable.GetUnderlyingType( result );

            if(null != nullable_type)
            {
                result = nullable_type;
            }

            return result;
        }
    }
}
