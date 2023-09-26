using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace HispaniaCommon.ViewClientWPF.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ICollectionViewEX
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_This"></param>
        /// <param name="filterCriterias"></param>
        /// <param name="propertyNames2Converters"></param>
        public static void SmartFilter( this ICollectionView _This, 
                                   Dictionary<string,string> filterCriterias,
                                   Dictionary<string,string> propertyNames2Converters = null )
        {

            Type item_type = null;
            PropertyInfo[] cache_propinfos = null;
            
            Lazy<Dictionary<string, IValueConverter>> cache_converters = new Lazy<Dictionary<string, IValueConverter>>(
                () => { return new Dictionary<string, IValueConverter>(); }
                );

            if(filterCriterias != null && filterCriterias.Count > 0)
            {
                _This.Filter = ( item ) =>
                {
                    if(null == item_type)
                    {
                        item_type = item.GetType();
                        cache_propinfos = item_type.GetProperties()
                                                   .Where( pi => filterCriterias.ContainsKey( pi.Name ) )
                                                   .ToArray();
                    }

                    bool? result = null;

                    foreach(KeyValuePair<string, string> prop_info in filterCriterias)
                    {
                        PropertyInfo item_prop_info = cache_propinfos.Where( pi => string.Compare( pi.Name, prop_info.Key ) == 0 )
                                                                     .FirstOrDefault();
                        if(null != item_prop_info)
                        {
                            object prop_value = item_prop_info.GetValue( item );

                            string str_prop_value = null;

                            IValueConverter i_converter = null;

                            if(null != propertyNames2Converters)
                            {
                                if(!cache_converters.Value.TryGetValue( prop_info.Key, out i_converter ))
                                {
                                    if( propertyNames2Converters.ContainsKey( prop_info.Key ) )
                                    {

                                        string class_name = propertyNames2Converters[ prop_info.Key ];
                                        Type type_converter = Type.GetType( class_name );
                                        if(null != type_converter)
                                        {
                                            i_converter = (IValueConverter)Activator.CreateInstance( type_converter );
                                            if(null != i_converter)
                                            {
                                                cache_converters.Value.Add( prop_info.Key, i_converter );
                                            }
                                        }
                                    }
                                }
                            }

                            if(i_converter != null)
                            {
                                object obj_value = i_converter.Convert( prop_value, typeof( string ), null, null );
                                str_prop_value = (string)obj_value;
                            }
                            else
                            {
                                str_prop_value = prop_value.ToString();
                            }

                            if(null != str_prop_value &&
                                str_prop_value.ToUpper().Contains( prop_info.Value.ToUpper() ))
                            {
                                if(result.HasValue)
                                {
                                    result = result.Value && true;
                                }
                                else
                                {
                                    result = true;
                                }
                            }
                        }
                    }

                    return (result.HasValue ? result.Value : false);
                };
            }
            else
            {
                _This.Filter = null;
            }
        }
    }
}
