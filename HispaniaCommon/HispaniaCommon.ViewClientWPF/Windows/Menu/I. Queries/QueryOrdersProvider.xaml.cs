using HispaniaCommon.ViewModel.ViewModel.Queries;
using System;
using System.Collections.Generic;
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

namespace HispaniaCommon.ViewClientWPF.Windows.Menu.I._Queries
{

    //QueryOrdersProviderViewModel


    /// <summary>
    /// Interaction logic for QueryOrdersProvider.xaml
    /// </summary>
    public partial class QueryOrdersProvider:Window
    {
        public QueryOrdersProvider()
        {
            InitializeComponent();

            QueryOrdersProviderViewModel model = new QueryOrdersProviderViewModel();

            model.SetWaitSystem( OnStartWait, OnStopWait );

            this.DataContext = model;

            this.btnExit.Click += BtnExit_Click;
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
        /// <param name="o"></param>
        /// <param name="e"></param>
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
