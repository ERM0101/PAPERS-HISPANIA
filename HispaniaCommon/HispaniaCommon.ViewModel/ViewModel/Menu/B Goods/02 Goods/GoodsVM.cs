using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Management.Instrumentation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HispaniaCommon.ViewModel.ViewModel.Menu.B_Goods._02_Goods
{
    /// <summary>
    /// 
    /// </summary>
    public class GoodsVM:
                INotifyPropertyChanged
    {
        

        #region interface  INotifyPropertyChanged
        
        /// <summary>
        /// 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private bool _Management;
        private ObservableCollection<GoodsView> _ListItems;
        private Dictionary<string, string> _Fields;
        private string _FieldEntry;
        private bool _IsSearchPanelShow;

        /// <summary>
        /// 
        /// </summary>
        public ObservableCollection<GoodsView> ListItems
        {
            get
            {
                return this._ListItems;
            }
            set
            {
                if(null != this._ListItems)
                {
                    this._ListItems.CollectionChanged -= _ListItems_CollectionChanged;
                }

                this._ListItems = value;
                this._ListItems.CollectionChanged += _ListItems_CollectionChanged;
                NotifyPropertyChanged( "ListItems" );
                this.IsSearchPanelShow = (value.Count > 0);
            }
        }

        private bool _Managemend;
        /// <summary>
        /// 
        /// </summary>
        public bool Managemend
        {
            get
            {
                return this._Managemend;
            }

            set
            {
                this._Managemend = value;
                NotifyPropertyChanged( "Managemend" );
                NotifyPropertyChanged( "NewLineaButtonVisibility" );

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool NewLineaButtonVisibility
        {
            get
            {
                bool result = !this._Managemend;

                return result;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsSearchPanelShow
        {
            get
            {
                return this._IsSearchPanelShow;
            }

            set
            {
                bool need_notify = ( this._IsSearchPanelShow != value );

                this._IsSearchPanelShow = value;
                
                if(need_notify)
                {
                    NotifyPropertyChanged( "IsSearchPanelShow" );
                }
            }
        }
                
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> Fields
        {
            get
            {
                return this._Fields;
            }

            set
            {
                this._Fields = value;
                NotifyPropertyChanged( "Fields" );
                this.FieldEntry = null;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        public bool Management
        {
            get
            {
                return this._Management;
            }
            set
            {
                this._Management = value;
                NotifyPropertyChanged( "Management" );
            }        
        }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="listItems"></param>
        /// <param name="management"></param>
        public GoodsVM( ObservableCollection<GoodsView> listItems = null, bool management = false )
        {
            this.Fields = GoodsView.Fields;

            this.ListItems = ( null == listItems ? 
                                new ObservableCollection<GoodsView>() :
                                listItems );

            this.Management = management;
            this.IsSearchPanelShow = ( this.ListItems.Count > 0 );
        }

        /// <summary>
        /// 
        /// </summary>
        public string FieldEntry
        {
            get
            {
                return this._FieldEntry;
            }
            set
            {
                this._FieldEntry = value;
                NotifyPropertyChanged( "FieldEntry" );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        private void NotifyPropertyChanged( string info )
        {
            if( null != PropertyChanged )
            {
                PropertyChanged( this, new PropertyChangedEventArgs( info ) );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void _ListItems_CollectionChanged( object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e )
        {
            NotifyPropertyChanged( "IsSearchPanelHeight" );
        }
    }
}
