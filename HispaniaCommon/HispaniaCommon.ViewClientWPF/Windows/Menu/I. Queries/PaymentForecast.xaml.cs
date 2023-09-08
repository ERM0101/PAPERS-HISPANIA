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
    /// <summary>
    /// Interaction logic for PaymentForecast.xaml
    /// </summary>
    public partial class PaymentForecast:Window
    {
        public PaymentForecast()
        {   
            InitializeComponent();

            QueryPaymentForecastViewModel model = new QueryPaymentForecastViewModel();

            model.SetWaitSystem( OnStartWait, OnStopWait );

            this.DataContext = model;

            model.Refresh();
            
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
