using HispaniaCommon.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HispaniaCommon.ViewClientWPF.Converters
{
    public class DateTimeToStringConverter:
                IValueConverter
    {
        /// <summary>
        /// 
        /// </summary>
        public DateTimeToStringConverter()
        {
        }

        #region interface IValueConverter

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            string result = null;

            if(value is DateTime datetime_value)
            {
                result = DateTimeToString( datetime_value );
            }

            if(value is DateTime?)
            {
                DateTime? datetine_nullable_value = (DateTime?) value;

                if(datetine_nullable_value.HasValue)
                {
                    result = DateTimeToString( datetine_nullable_value.Value );
                }
                else
                {
                    result = string.Empty;
                }
            }


            if(result == null)
            {
                throw new ConvectorException( value, typeof( DateTime ) );
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException();
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public string DateTimeToString( DateTime value )
        {
            string result = GlobalViewModel.GetStringFromDateTimeValue( value );

            return result;
        }
    }
}
