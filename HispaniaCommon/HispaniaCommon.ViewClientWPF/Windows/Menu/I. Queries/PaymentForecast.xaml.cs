using HispaniaCommon.ViewModel.ViewModel.Queries;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using HispaniaCommon.ViewClientWPF.Extensions;

namespace HispaniaCommon.ViewClientWPF.Windows.Menu.I._Queries
{
    /// <summary>
    /// Interaction logic for PaymentForecast.xaml
    /// </summary>
    public partial class PaymentForecast:Window
    {
        /// <summary>
        /// 
        /// </summary>
        public PaymentForecast()
        {   
            InitializeComponent();

            this.btnExit.Click += BtnExit_Click;

            Dictionary<string,string> name2path = this.dgListItems.GetName2Property();
            Dictionary<string, Dictionary<string, string>> name_name2name = new Dictionary<string, Dictionary<string, string>>();
            name_name2name.Add  ("dgListItem", name2path );


            QueryPaymentForecastViewModel model = new QueryPaymentForecastViewModel( name_name2name, this.OnFilter );

            model.SetWaitSystem( OnStartWait, OnStopWait );

            this.DataContext = model;

            model.Refresh();

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnExit_Click( object sender, RoutedEventArgs e )
        {
            Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filterName"></param>
        /// <param name="filterCriterias"></param>
        private void OnFilter(string filterName, Dictionary<string,string> filterCriterias )
        {
            var data = (CollectionViewSource.GetDefaultView( this.dgListItems.ItemsSource ) as ICollectionView);

            if(null != data)
            {
                Dictionary<string, string> name2converters = this.dgListItems.GetName2Converter();
                data.SmartFilter( filterCriterias, name2converters );

            }

        }

        /// <summary>
        /// 
        /// </summary>
        private void OnStartWait()
        {           

            Mouse.OverrideCursor = Cursors.Wait;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="o"></param>
        /// <param name="e"></param>
        private void OnStopWait()
        {
            Mouse.OverrideCursor = null;
        }
    }
}
