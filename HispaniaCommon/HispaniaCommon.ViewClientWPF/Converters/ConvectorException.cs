using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HispaniaCommon.ViewClientWPF.Converters
{
    public class ConvectorException: 
                Exception
    {

        /// <summary>
        /// Object for conversion
        /// </summary>
        public object SourceObject
        {
            get;
        }

        /// <summary>
        /// Destination type for property "SourceObject"
        /// </summary>
        public Type DestinationType
        {
            get;
        }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <param name="destinationType"></param>
        public ConvectorException( object sourceObject = null, Type destinationType = null ) :
                base( "Error conversion in converter." )
        {
            this.SourceObject = sourceObject;
            this.DestinationType = destinationType;
        }
    }
}
