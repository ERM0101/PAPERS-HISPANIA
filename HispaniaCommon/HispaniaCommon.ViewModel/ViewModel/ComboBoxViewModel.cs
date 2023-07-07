using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HispaniaCommon.ViewModel.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TValue"></typeparam>
    public class ComboBoxViewModel<TValue>
    {

        /// <summary>
        /// 
        /// </summary>
        public TValue Value
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Text
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="text"></param>
        public ComboBoxViewModel( TValue value, string text )
        {
            this.Value = value;
            this.Text = text;
        }
    }
}
