using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;

namespace HispaniaCommon.ViewClientWPF.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DataGridEX
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_This"></param>
        /// <returns></returns>
        public static Dictionary<string, string> GetName2Property( this DataGrid _This )
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach( var column_info in _This.Columns )
            {
                //DataGridTextColumn

                DataGridBoundColumn column = (DataGridBoundColumn)column_info;

                if(null != column)
                {
                    string name = (string)column.Header;
                    if( !string.IsNullOrWhiteSpace(name) )
                    {
                        if(null != column.Binding)
                        {
                            Binding binding = (Binding)column.Binding;
                            if( null != binding && null != binding.Path )
                            {
                                string path = binding.Path.Path;
                                if(!string.IsNullOrWhiteSpace( path ))
                                {
                                    result.Add( name, path );
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }

        public static Dictionary<string, string> GetName2Converter( this DataGrid _This )
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            foreach(var column_info in _This.Columns)
            {
                //DataGridTextColumn

                DataGridBoundColumn column = (DataGridBoundColumn)column_info;

                if(null != column)
                {
                    string name = (string)column.Header;
                    if(!string.IsNullOrWhiteSpace( name ))
                    {
                        if(null != column.Binding)
                        {
                            Binding binding = (Binding)column.Binding;

                            if(null != binding && null != binding.Converter )
                            {
                                string path = binding.Path.Path;
                                if(!string.IsNullOrWhiteSpace( path ))
                                {
                                    Type converter_type = binding.Converter.GetType();

                                    string converter = converter_type.FullName;
                                    if(!string.IsNullOrWhiteSpace( converter ))
                                    {
                                        result.Add( path, converter );
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return result;
        }
    }
}
