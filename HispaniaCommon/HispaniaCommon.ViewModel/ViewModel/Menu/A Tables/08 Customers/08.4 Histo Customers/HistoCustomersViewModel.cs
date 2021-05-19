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

        public void CreateHistoCustomer(HistoCustomersView histoCustomersView)
        {
            HispaniaCompData.HistoCustomer histoCustomerToCreate = histoCustomersView.GetHistoCustomer();
            CreateHistoCustomerInDb(histoCustomerToCreate);
            histoCustomersView.HistoCustomer_Id = histoCustomerToCreate.HistoCustomer_Id;
        }

        public void RefreshHistoCustomers()
        {
            try
            {
                HistoCustomersInDb = HispaniaDataAccess.Instance.ReadHistoCustomers();
                _HistoCustomers = new ObservableCollection<HistoCustomersView>();
                _HistoCustomersInDictionary = new Dictionary<string, HistoCustomersView>();
                foreach (HispaniaCompData.HistoCustomer histoCustomer in HistoCustomersInDb)
                {
                    HistoCustomersView NewHistoCustomersView = new HistoCustomersView(histoCustomer);
                    _HistoCustomers.Add(NewHistoCustomersView);
                    _HistoCustomersInDictionary.Add(GetKeyHistoCustomerView(histoCustomer), NewHistoCustomersView);
                }
            }
            catch (Exception ex)
            {
                _HistoCustomers = null;
                throw ex;
            }
        }

        public ObservableCollection<HistoCustomersView> GetHistoCustomers(int Customer_Id, bool WithChilds = false)
        {
            try
            {
                ObservableCollection<HistoCustomersView>  HistoCustomersFiltered = new ObservableCollection<HistoCustomersView>();
                foreach (HispaniaCompData.HistoCustomer histoCustomer in HispaniaDataAccess.Instance.ReadHistoCustomers(Customer_Id, WithChilds))
                {
                    HistoCustomersView NewHistoCustomersView = new HistoCustomersView(histoCustomer);
                    HistoCustomersFiltered.Add(NewHistoCustomersView);
                }
                return HistoCustomersFiltered;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ObservableCollection<HistoCustomersView> _HistoCustomers = null;

        public ObservableCollection<HistoCustomersView> HistoCustomers
        {
            get
            {
                return _HistoCustomers;
            }
        }

        private Dictionary<string, HistoCustomersView> _HistoCustomersInDictionary = null;

        public Dictionary<string, HistoCustomersView> HistoCustomersDict
        {
            get
            {
                return _HistoCustomersInDictionary;
            }
        }

        public string GetKeyHistoCustomerView(HistoCustomersView histoCustomersView)
        {
            return GetKeyHistoCustomerView(histoCustomersView.HistoCustomer_Id);
        }

        private string GetKeyHistoCustomerView(HispaniaCompData.HistoCustomer histoCustomer)
        {
            return GetKeyHistoCustomerView(histoCustomer.HistoCustomer_Id);
        }

        private string GetKeyHistoCustomerView(int Hist_Id)
        {
            return string.Format("{0}", Hist_Id);
        }


        public void UpdateHistoCustomer(HistoCustomersView histoCustomerView)
        {
            UpdateHistoCustomerInDb(histoCustomerView.GetHistoCustomer());
        }
        public void DeleteHistoCustomer(HistoCustomersView histoCustomerView)
        {
            DeleteHistoCustomerInDb(histoCustomerView.GetHistoCustomer());
        }

        public HistoCustomersView GetHistoCustomerFromDb(HistoCustomersView customersView)
        {
            return new HistoCustomersView(GetHistoCustomerInDb(customersView.HistoCustomer_Id));
        }

        public HispaniaCompData.HistoCustomer GetHistoCustomer(int HistoCustomer_Id)
        {
            return GetHistoCustomerInDb(HistoCustomer_Id);
        }

        #endregion

        #region DataBase [CRUD]

        private void CreateHistoCustomerInDb(HispaniaCompData.HistoCustomer histoCustomer)
        {
            HispaniaDataAccess.Instance.CreateHistoCustomer(histoCustomer);
        }

        private List<HispaniaCompData.HistoCustomer> _HistoCustomersInDb;

        private List<HispaniaCompData.HistoCustomer> HistoCustomersInDb
        {
            get
            {
                return (this._HistoCustomersInDb);
            }
            set
            {
                this._HistoCustomersInDb = value;
            }
        }

        private void UpdateHistoCustomerInDb(HispaniaCompData.HistoCustomer histoCustomer)
        {
            HispaniaDataAccess.Instance.UpdateHistoCustomer(histoCustomer);
        }

        private void DeleteHistoCustomerInDb(HispaniaCompData.HistoCustomer histoCustomer)
        {
            HispaniaDataAccess.Instance.DeleteHistoCustomer(histoCustomer);
        }

        private HispaniaCompData.HistoCustomer GetHistoCustomerInDb(int HistoCustomer_Id)
        {
            return HispaniaDataAccess.Instance.GetHistoCustomer(HistoCustomer_Id);
        }

        #endregion
    }
}
