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
    public class ExcelColumnAttribute:
                 Attribute
    {
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
        public string Name
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="name"></param>
        public ExcelColumnAttribute( int position, string name )
        {
            this.Position = position;
            this.Name = name;
        }
    }
}
