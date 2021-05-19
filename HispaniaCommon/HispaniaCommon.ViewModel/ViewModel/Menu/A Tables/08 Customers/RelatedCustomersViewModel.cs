#region Librerias usadas por la clase

using HispaniaCommon.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HispaniaCompData = HispaniaComptabilitat.Data;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Last update Data: 19/09/2016
    /// Descriptión: class that implements Common ViewModel of the Applications.
    /// </summary>
    public partial class MaintenanceForViewModel
    {
        #region ViewModel

        public void CreateRelatedCustomer(RelatedCustomersView relatedCustomersView)
        {
            HispaniaCompData.RelatedCustomer relatedCustomerToCreate = relatedCustomersView.GetRelatedCustomer();
            CreateRelatedCustomerInDb(relatedCustomerToCreate);
        }

        public void RefreshRelatedCustomers(int Customer_Id)
        {
            try
            {
                RelatedCustomersInDb = HispaniaDataAccess.Instance.ReadRelatedCustomers(Customer_Id);
                _RelatedCustomers = new ObservableCollection<RelatedCustomersView>();
                _RelatedCustomersInDictionary = new Dictionary<string, RelatedCustomersView>();
                foreach (HispaniaCompData.RelatedCustomer relatedCustomer in RelatedCustomersInDb)
                {
                    RelatedCustomersView NewRelatedCustomersView = new RelatedCustomersView(relatedCustomer);
                    _RelatedCustomers.Add(NewRelatedCustomersView);
                    _RelatedCustomersInDictionary.Add(GetKeyRelatedCustomerView(relatedCustomer), NewRelatedCustomersView);
                }
            }
            catch (Exception ex)
            {
                _RelatedCustomers = null;
                throw ex;
            }
        }

        private ObservableCollection<RelatedCustomersView> _RelatedCustomers = null;

        public ObservableCollection<RelatedCustomersView> RelatedCustomers
        {
            get
            {
                return _RelatedCustomers;
            }
        }

        private Dictionary<string, RelatedCustomersView> _RelatedCustomersInDictionary = null;

        public Dictionary<string, RelatedCustomersView> RelatedCustomersDict
        {
            get
            {
                return _RelatedCustomersInDictionary;
            }
        }

        public string GetKeyRelatedCustomerView(RelatedCustomersView relatedCustomersView)
        {
            return GetKeyRelatedCustomerView(relatedCustomersView.Customer_Id, relatedCustomersView.Customer_Canceled_Id);
        }

        private string GetKeyRelatedCustomerView(HispaniaCompData.RelatedCustomer relatedCustomer)
        {
            return GetKeyRelatedCustomerView(relatedCustomer.Customer_Id, relatedCustomer.Customer_Canceled_Id);
        }

        private string GetKeyRelatedCustomerView(int Customer_Id, int Customer_Canceled_Id)
        {
            return string.Format("{0}-{1}", Customer_Id, Customer_Canceled_Id);
        }

        public void UpdateRelatedCustomer(RelatedCustomersView RelatedCustomersView)
        {
            UpdateRelatedCustomerInDb(RelatedCustomersView.GetRelatedCustomer());
        }
        public void DeleteRelatedCustomer(RelatedCustomersView RelatedCustomersView)
        {
            DeleteRelatedCustomerInDb(RelatedCustomersView.GetRelatedCustomer());
        }

        public RelatedCustomersView GetRelatedCustomerFromDb(RelatedCustomersView relatedCustomersView)
        {
            return new RelatedCustomersView(GetRelatedCustomerInDb(relatedCustomersView.Customer_Id, relatedCustomersView.Customer_Canceled_Id));
        }

        public HispaniaCompData.RelatedCustomer GetRelatedCustomer(int Customer_Id, int Customer_Canceled_Id)
        {
            return GetRelatedCustomerInDb(Customer_Id, Customer_Canceled_Id);
        }

        #endregion

        #region DataBase [CRUD]

        private void CreateRelatedCustomerInDb(HispaniaCompData.RelatedCustomer customer)
        {
            HispaniaDataAccess.Instance.CreateRelatedCustomer(customer);
        }

        private List<HispaniaCompData.RelatedCustomer> _RelatedCustomersInDb;

        private List<HispaniaCompData.RelatedCustomer> RelatedCustomersInDb
        {
            get
            {
                return (this._RelatedCustomersInDb);
            }
            set
            {
                this._RelatedCustomersInDb = value;
            }
        }

        private void UpdateRelatedCustomerInDb(HispaniaCompData.RelatedCustomer customer)
        {
            HispaniaDataAccess.Instance.UpdateRelatedCustomer(customer);
        }

        private void DeleteRelatedCustomerInDb(HispaniaCompData.RelatedCustomer customer)
        {
            HispaniaDataAccess.Instance.DeleteRelatedCustomer(customer);
        }

        private HispaniaCompData.RelatedCustomer GetRelatedCustomerInDb(int Customers_Id, int Customer_Canceled_Id)
        {
            return HispaniaDataAccess.Instance.GetRelatedCustomer(Customers_Id, Customer_Canceled_Id);
        }
        
        #endregion
    }
}
