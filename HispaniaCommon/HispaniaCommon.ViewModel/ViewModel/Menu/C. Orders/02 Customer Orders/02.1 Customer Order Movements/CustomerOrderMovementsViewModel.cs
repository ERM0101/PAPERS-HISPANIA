#region Libraries used by the class

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

        public void CreateCustomerOrderMovement(CustomerOrderMovementsView CustomerOrderMovementsView)
        {
            HispaniaCompData.CustomerOrderMovement CustomerOrderMovementToCreate = CustomerOrderMovementsView.GetCustomerOrderMovement();
            CreateCustomerOrderMovementInDb(CustomerOrderMovementToCreate);
            CustomerOrderMovementsView.CustomerOrderMovement_Id = CustomerOrderMovementToCreate.CustomerOrderMovement_Id;
            RefreshCustomerOrderMovements();
        }

        public void RefreshCustomerOrderMovements()
        {
            try
            {
                CustomerOrderMovementsInDb = HispaniaDataAccess.Instance.ReadCustomerOrderMovements();
                _CustomerOrderMovements = new ObservableCollection<CustomerOrderMovementsView>();
                _CustomerOrderMovementsInDictionary = new Dictionary<string, CustomerOrderMovementsView>();
                foreach (HispaniaCompData.CustomerOrderMovement CustomerOrderMovement in CustomerOrderMovementsInDb)
                {
                    CustomerOrderMovementsView NewCustomerOrderMovementsView = new CustomerOrderMovementsView(CustomerOrderMovement);
                    _CustomerOrderMovements.Add(NewCustomerOrderMovementsView);
                    _CustomerOrderMovementsInDictionary.Add(GetKeyCustomerOrderMovementView(CustomerOrderMovement), NewCustomerOrderMovementsView);
                }
            }
            catch (Exception ex)
            {
                _CustomerOrderMovements = null;
                throw ex;
            }
        }

        public ObservableCollection<CustomerOrderMovementsView> GetCustomerOrderMovements(int CustomerOrder_Id)
        {
            try
            {
                ObservableCollection<CustomerOrderMovementsView> CustomerOrderMovementsFiltered = new ObservableCollection<CustomerOrderMovementsView>();
                foreach (HispaniaCompData.CustomerOrderMovement customerOrderMovement in ReadCustomerOrderMovementsInDb(CustomerOrder_Id))
                {
                    CustomerOrderMovementsView NewCustomerOrderMovementsView = new CustomerOrderMovementsView(customerOrderMovement);
                    CustomerOrderMovementsFiltered.Add(NewCustomerOrderMovementsView);
                }
                return CustomerOrderMovementsFiltered;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ObservableCollection<CustomerOrderMovementsView> _CustomerOrderMovements = null;

        public ObservableCollection<CustomerOrderMovementsView> CustomerOrderMovements
        {
            get
            {
                return _CustomerOrderMovements;
            }
        }

        private Dictionary<string, CustomerOrderMovementsView> _CustomerOrderMovementsInDictionary = null;

        public Dictionary<string, CustomerOrderMovementsView> CustomerOrderMovementsDict
        {
            get
            {
                return _CustomerOrderMovementsInDictionary;
            }
        }

        public string GetKeyCustomerOrderMovementView(CustomerOrderMovementsView CustomerOrderMovementsView)
        {
            return GetKeyCustomerOrderMovementView(CustomerOrderMovementsView.CustomerOrderMovement_Id);
        }

        private string GetKeyCustomerOrderMovementView(HispaniaCompData.CustomerOrderMovement CustomerOrderMovement)
        {
            return GetKeyCustomerOrderMovementView(CustomerOrderMovement.CustomerOrderMovement_Id);
        }

        private string GetKeyCustomerOrderMovementView(int CoustomerOrder_Id)
        {
            return string.Format("{0}", CoustomerOrder_Id);
        }


        public void UpdateCustomerOrderMovement(CustomerOrderMovementsView CustomerOrderMovementView)
        {
            UpdateCustomerOrderMovementInDb(CustomerOrderMovementView.GetCustomerOrderMovement());
            RefreshCustomerOrderMovements();
        }
        public void DeleteCustomerOrderMovement(CustomerOrderMovementsView CustomerOrderMovementView)
        {
            DeleteCustomerOrderMovementInDb(CustomerOrderMovementView.GetCustomerOrderMovement());
            RefreshCustomerOrderMovements();
        }

        public HispaniaCompData.CustomerOrderMovement GetCustomerOrderMovement(int CustomerOrderMovement_Id)
        {
            return GetCustomerOrderMovementInDb(CustomerOrderMovement_Id);
        }

        #endregion

        #region DataBase [CRUD]

        private List<HispaniaCompData.CustomerOrderMovement> ReadCustomerOrderMovementsInDb(int CustomerOrder_Id)
        {
            return (HispaniaDataAccess.Instance.ReadCustomerOrderMovements(CustomerOrder_Id));
        }

        private void CreateCustomerOrderMovementInDb(HispaniaCompData.CustomerOrderMovement customerOrderMovement)
        {
            HispaniaDataAccess.Instance.CreateCustomerOrderMovement(customerOrderMovement);
        }

        private List<HispaniaCompData.CustomerOrderMovement> _CustomerOrderMovementsInDb;

        private List<HispaniaCompData.CustomerOrderMovement> CustomerOrderMovementsInDb
        {
            get
            {
                return (this._CustomerOrderMovementsInDb);
            }
            set
            {
                this._CustomerOrderMovementsInDb = value;
            }
        }

        private void UpdateCustomerOrderMovementInDb(HispaniaCompData.CustomerOrderMovement customerOrderMovement)
        {
            HispaniaDataAccess.Instance.UpdateCustomerOrderMovement(customerOrderMovement);
        }

        private void DeleteCustomerOrderMovementInDb(HispaniaCompData.CustomerOrderMovement customerOrderMovement)
        {
            HispaniaDataAccess.Instance.DeleteCustomerOrderMovement(customerOrderMovement);
        }

        private HispaniaCompData.CustomerOrderMovement GetCustomerOrderMovementInDb(int CustomerOrderMovement_Id)
        {
            return HispaniaDataAccess.Instance.GetCustomerOrderMovement(CustomerOrderMovement_Id);
        }

        #endregion
    }
}
