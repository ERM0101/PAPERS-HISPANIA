using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace HispaniaCommon.ViewModel.ViewModel
{
    /// <summary>
    /// 
    /// </summary>
    public class ViewModelBase:
            INotifyPropertyChanged
    {
        private Action _StartWait = null;
        private Action _StopWait = null;

        /// <summary>
        /// 
        /// </summary>
        public void SetWaitSystem( Action startWait, Action stopWait )
        {
            if((startWait == null && stopWait == null) ||
                (startWait != null || stopWait != null))
            {
                this._StartWait = startWait;
                this._StopWait = stopWait;
            }
            else
            {
                throw new ArgumentNullException();
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        protected ViewModelBase()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="prop"></param>
        protected void OnPropertyChanged( [CallerMemberName] string prop = "" )
        {
            if( this.PropertyChanged != null)
                this.PropertyChanged( this, new PropertyChangedEventArgs( prop ) );
        }


        #region interface INotifyPropertyChanging

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        /// <summary>
        /// 
        /// </summary>
        protected void StartWait()
        {
            if(this._StartWait != null)
            {
                this._StartWait();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected void StopWait()
        {
            if(this._StopWait != null)
            {
                this._StopWait();
            }
        }
    }
}
