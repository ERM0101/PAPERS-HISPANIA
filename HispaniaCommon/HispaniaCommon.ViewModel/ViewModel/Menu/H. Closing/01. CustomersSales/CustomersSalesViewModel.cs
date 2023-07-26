using HispaniaCommon.DataAccess;
using System;
using System.Collections.Generic;
using HispaniaCompData = HispaniaComptabilitat.Data;

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Last update Data: 04/01/2018
    /// Descriptión: class that implements Common ViewModel of the Applications.
    /// </summary>
    public partial class MaintenanceForViewModel
    {
        #region ViewModel

        public void RefreshCustomersSales(decimal Upper_Limit_Sales)
        {
            try
            {
                CustomersSalesInDb = HispaniaDataAccess.Instance.ReadCustomersSales(Upper_Limit_Sales);
                _CustomersSalesInDictionary = new SortedDictionary<int, CustomersSalesView>();
                foreach (HispaniaCompData.CustomerSales_Result CustomerSale in CustomersSalesInDb)
                {
                    CustomersSalesView NewCustomersSalesView = new CustomersSalesView(CustomerSale);
                    _CustomersSalesInDictionary.Add(NewCustomersSalesView.Customer_Id, NewCustomersSalesView);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private SortedDictionary<int, CustomersSalesView> _CustomersSalesInDictionary = null;

        public SortedDictionary<int, CustomersSalesView> CustomersSalesDict
        {
            get
            {
                return _CustomersSalesInDictionary;
            }
        }

        public string GetKeyCustomersSaleView(CustomersSalesView CustomersSalesView)
        {
            return GetKeyCustomersSaleView(CustomersSalesView.Customer_Id);
        }

        private string GetKeyCustomersSaleView( HispaniaCompData.CustomerSales_Result CustomerSale )
        {
            return GetKeyCustomersSaleView( CustomerSale.Customer_Id );
        }

        private string GetKeyCustomersSaleView( int Customer_Id )
        {
            return string.Format( "{0}", Customer_Id );
        }
        
        #endregion

        #region DataBase [CRUD]

        private List<HispaniaCompData.CustomerSales_Result> ReadCustomersSalesInDb(decimal Upper_Limit_Sales)
        {
            return (HispaniaDataAccess.Instance.ReadCustomersSales(Upper_Limit_Sales));
        }

        private List<HispaniaCompData.CustomerSales_Result> _CustomersSalesInDb;

        private List<HispaniaCompData.CustomerSales_Result> CustomersSalesInDb
        {
            get
            {
                return (this._CustomersSalesInDb);
            }
            set
            {
                this._CustomersSalesInDb = value;
            }
        }

        #endregion
    }
}
