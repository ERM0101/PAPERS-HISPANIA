using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MBCode.Framework.OFFICE
{
    /// <summary>
    /// 
    /// </summary>
    public class ExcelColumnInfo
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
        public string NumberFormat
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
        /// <param name="numberFormat"></param>
        /// <param name="propertyInfo"></param>
        public ExcelColumnInfo( string name, int position, string numberFormat, PropertyInfo propertyInfo )
        {
            this.Name = name;
            this.Position = position;
            this.NumberFormat = numberFormat;
            this.PropertyInfo = propertyInfo;
        }
    }
}
