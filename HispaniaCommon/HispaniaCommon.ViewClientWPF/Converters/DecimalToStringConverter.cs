using HispaniaCommon.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace HispaniaCommon.ViewClientWPF.Converters
{
    public class DecimalToStringConverter:
                    IValueConverter
    {
        /// <summary>
        /// CTOR
        /// </summary>
        public DecimalToStringConverter()
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
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            string result = null;

            if(value is decimal )
            {
                result = DecimalToString( (decimal)value );
            }

            if(value is decimal?)
            {
                decimal? decimal_nullable_value = (decimal?)value;

                if(decimal_nullable_value.HasValue)
                {
                    result = DecimalToString( decimal_nullable_value.Value );
                }
                else
                {
                    result = string.Empty;
                }
            }
            else
            {
                result = string.Empty;
            }

            // error convertion
            if( result == null )
            {
                throw new ConvectorException( value, typeof( string ) );
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
        private string DecimalToString( Decimal value )
        {
            string result = GlobalViewModel.GetStringFromDecimalValue( value,
                                                                       DecimalType.Currency, true );
            return result;
        }
    }
}
