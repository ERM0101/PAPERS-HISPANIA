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

        public void RefreshCustomers()
        {
            try
            {
                CustomersInDb = HispaniaDataAccess.Instance.ReadCustomers();
                _Customers = new ObservableCollection<CustomersView>();
                _CustomersInDictionary = new Dictionary<string, CustomersView>();
                _CustomersActiveInDictionary = new Dictionary<string, CustomersView>();
                foreach (HispaniaCompData.Customer customer in CustomersInDb)
                {
                    CustomersView NewCustomersView = new CustomersView(customer);
                    _Customers.Add(NewCustomersView);
                    _CustomersInDictionary.Add(GetKeyCustomerView(customer), NewCustomersView);
                    if (!NewCustomersView.Canceled) _CustomersActiveInDictionary.Add(GetKeyCustomerView(customer), NewCustomersView);
                }
            }
            catch (Exception ex)
            {
                _Customers = null;
                throw ex;
            }
        }

        private ObservableCollection<CustomersView> _Customers = null;

        public ObservableCollection<CustomersView> Customers
        {
            get
            {
                return _Customers;
            }
        }

        private Dictionary<string, CustomersView> _CustomersActiveInDictionary = null;

        public Dictionary<string, CustomersView> CustomersActiveDict
        {
            get
            {
                return _CustomersActiveInDictionary;
            }
        }

        private Dictionary<string, CustomersView> _CustomersInDictionary = null;

        public Dictionary<string, CustomersView> CustomersDict
        {
            get
            {
                return _CustomersInDictionary;
            }
        }

        public string GetKeyCustomerView(CustomersView customersView)
        {
            return GetKeyCustomerView(customersView.Customer_Id, customersView.Customer_Alias, customersView.Company_Name);
        }

        private string GetKeyCustomerView(HispaniaCompData.Customer customer)
        {
            return GetKeyCustomerView(customer.Customer_Id, customer.Customer_Alias, customer.Company_Name);
        }

        private string GetKeyCustomerView(int Customer_Id, string Customer_Alias, string Company_Name)
        {
            return string.Format("{0:000000} | {1} {2}", Customer_Id, Customer_Alias, Company_Name);
        }

        public void CreateCustomer(CustomersView customersView, List<RelatedCustomersView> NewOrEditedRelatedCustomers)
        {
            HispaniaCompData.Customer customerToCreate = customersView.GetCustomer();
            CreateCustomerInDb(customerToCreate, GetRelatedCustomersForDb(NewOrEditedRelatedCustomers));
            customersView.Customer_Id = customerToCreate.Customer_Id;
        }

        public void UpdateCustomer(CustomersView CustomersView, List<RelatedCustomersView> NewOrEditedRelatedCustomers)
        {
            UpdateCustomerInDb(CustomersView.GetCustomer(), GetRelatedCustomersForDb(NewOrEditedRelatedCustomers));
        }

        private List<HispaniaCompData.RelatedCustomer> GetRelatedCustomersForDb(List<RelatedCustomersView> RelatedCustomers)
        {
            List<HispaniaCompData.RelatedCustomer> RelatedCustomersForDb = new List<HispaniaCompData.RelatedCustomer>();
            foreach (RelatedCustomersView relatedCutomer in RelatedCustomers)
            {
                RelatedCustomersForDb.Add(relatedCutomer.GetRelatedCustomer());
            }
            return RelatedCustomersForDb;
        }

        public void DeleteCustomer(CustomersView CustomersView)
        {
            DeleteCustomerInDb(CustomersView.GetCustomer());
        }

        public CustomersView GetCustomerFromDb(CustomersView customersView)
        {
            return new CustomersView(GetCustomerInDb(customersView.Customer_Id));
        }

        public CustomersView GetCustomerFromDb(int Customer_Id)
        {
            return new CustomersView(GetCustomerInDb(Customer_Id));
        }

        public HispaniaCompData.Customer GetCustomer(int Customers_Id)
        {
            return GetCustomerInDb(Customers_Id);
        }

        #endregion

        #region DataBase [CRUD]

        private void CreateCustomerInDb(HispaniaCompData.Customer customer, List<HispaniaCompData.RelatedCustomer> relatedCustomers)
        {
            HispaniaDataAccess.Instance.CreateCustomer(customer, relatedCustomers);
        }

        private List<HispaniaCompData.Customer> _CustomersInDb;

        private List<HispaniaCompData.Customer> CustomersInDb
        {
            get
            {
                return (this._CustomersInDb);
            }
            set
            {
                this._CustomersInDb = value;
            }
        }

        private void UpdateCustomerInDb(HispaniaCompData.Customer customer, List<HispaniaCompData.RelatedCustomer> relatedCustomers)
        {
            HispaniaDataAccess.Instance.UpdateCustomer(customer, relatedCustomers);
        }

        private void DeleteCustomerInDb(HispaniaCompData.Customer customer)
        {
            HispaniaDataAccess.Instance.DeleteCustomer(customer);
        }

        private HispaniaCompData.Customer GetCustomerInDb(int Customers_Id)
        {
            return HispaniaDataAccess.Instance.GetCustomer(Customers_Id);
        }
        
        #endregion
    }
}
