using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace HispaniaCommon.ViewClientWPF.Converters
{
    /// <summary>
    /// 
    /// </summary>
    public class BoolToHeightConverter:
                 IMultiValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert( object[] values, Type targetType, object parameter, CultureInfo culture )
        {            
            double result = 0.0;

            if( values.Length == 3 && values[0] is bool && values[1] is string && values[2] is string && 
                double.TryParse( values[1] as string, NumberStyles.Any, CultureInfo.InvariantCulture, out double false_value ) &&
                double.TryParse( values[ 2 ] as string, NumberStyles.Any, CultureInfo.InvariantCulture, out double true_value ) )
            {
                bool value = (bool)values[0];
                result = (value ? true_value : false_value);
            }

            return new GridLength( result );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetTypes"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public object[] ConvertBack( object value, Type[] targetTypes, object parameter, CultureInfo culture )
        {
            throw new NotSupportedException();
        }
    }
}
