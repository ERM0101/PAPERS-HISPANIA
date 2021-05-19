using HispaniaCommon.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HispaniaCompData = HispaniaComptabilitat.Data;

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Last update Data: 19/09/2016
    /// Descriptión: class that implements Common ViewModel of the Applications.
    /// </summary>
    public partial class MaintenanceForViewModel
    {
        #region ViewModel

        public void CreateHistoCumulativeCustomer(HistoCumulativeCustomersView histoCumulativeCustomersView)
        {
            HispaniaCompData.HistoCumulativeCustomer histoCumulativeCustomerToCreate = histoCumulativeCustomersView.GetHistoCumulativeCustomer();
            CreateHistoCumulativeCustomerInDb(histoCumulativeCustomerToCreate);
            histoCumulativeCustomersView.Histo_Id = histoCumulativeCustomerToCreate.Histo_Id;
            RefreshHistoCumulativeCustomers();
        }

        public void RefreshHistoCumulativeCustomers()
        {
            try
            {
                HistoCumulativeCustomersInDb = HispaniaDataAccess.Instance.ReadHistoCumulativeCustomers();
                _HistoCumulativeCustomers = new ObservableCollection<HistoCumulativeCustomersView>();
                _HistoCumulativeCustomersInDictionary = new Dictionary<string, HistoCumulativeCustomersView>();
                foreach (HispaniaCompData.HistoCumulativeCustomer histoCumulativeCustomer in HistoCumulativeCustomersInDb)
                {
                    HistoCumulativeCustomersView NewHistoCumulativeCustomersView = new HistoCumulativeCustomersView(histoCumulativeCustomer);
                    _HistoCumulativeCustomers.Add(NewHistoCumulativeCustomersView);
                    _HistoCumulativeCustomersInDictionary.Add(GetKeyHistoCumulativeCustomerView(histoCumulativeCustomer), NewHistoCumulativeCustomersView);
                }
            }
            catch (Exception ex)
            {
                _HistoCumulativeCustomers = null;
                throw ex;
            }
        }

        public ObservableCollection<HistoCumulativeCustomersView> GetHistoCumulativeCustomers(int Customer_Id)
        {
            try
            {
                ObservableCollection<HistoCumulativeCustomersView>  HistoCumulativeCustomersFiltered = new ObservableCollection<HistoCumulativeCustomersView>();
                foreach (HispaniaCompData.HistoCumulativeCustomer histoCumulativeCustomer in HispaniaDataAccess.Instance.ReadHistoCumulativeCustomer(Customer_Id))
                {
                    HistoCumulativeCustomersView NewHistoCumulativeCustomersView = new HistoCumulativeCustomersView(histoCumulativeCustomer);
                    HistoCumulativeCustomersFiltered.Add(NewHistoCumulativeCustomersView);
                }
                return HistoCumulativeCustomersFiltered;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ObservableCollection<HistoCumulativeCustomersView> _HistoCumulativeCustomers = null;

        public ObservableCollection<HistoCumulativeCustomersView> HistoCumulativeCustomers
        {
            get
            {
                return _HistoCumulativeCustomers;
            }
        }

        private Dictionary<string, HistoCumulativeCustomersView> _HistoCumulativeCustomersInDictionary = null;

        public Dictionary<string, HistoCumulativeCustomersView> HistoCumulativeCustomersDict
        {
            get
            {
                return _HistoCumulativeCustomersInDictionary;
            }
        }

        public string GetKeyHistoCumulativeCustomerView(HistoCumulativeCustomersView histoCumulativeCustomersView)
        {
            return GetKeyHistoCumulativeCustomerView(histoCumulativeCustomersView.Histo_Id);
        }

        private string GetKeyHistoCumulativeCustomerView(HispaniaCompData.HistoCumulativeCustomer histoCumulativeCustomer)
        {
            return GetKeyHistoCumulativeCustomerView(histoCumulativeCustomer.Histo_Id);
        }

        private string GetKeyHistoCumulativeCustomerView(int Hist_Id)
        {
            return string.Format("{0}", Hist_Id);
        }


        public void UpdateHistoCumulativeCustomer(HistoCumulativeCustomersView histoCumulativeCustomerView)
        {
            UpdateHistoCumulativeCustomerInDb(histoCumulativeCustomerView.GetHistoCumulativeCustomer());
            RefreshHistoCumulativeCustomers();
        }
        public void DeleteHistoCumulativeCustomer(HistoCumulativeCustomersView histoCumulativeCustomerView)
        {
            DeleteHistoCumulativeCustomerInDb(histoCumulativeCustomerView.GetHistoCumulativeCustomer());
            RefreshHistoCumulativeCustomers();
        }

        public HispaniaCompData.HistoCumulativeCustomer GetHistoCumulativeCustomer(int HistoCumulativeCustomer_Id)
        {
            return GetHistoCumulativeCustomerInDb(HistoCumulativeCustomer_Id);
        }

        #endregion

        #region DataBase [CRUD]

        private void CreateHistoCumulativeCustomerInDb(HispaniaCompData.HistoCumulativeCustomer histoCumulativeCustomer)
        {
            HispaniaDataAccess.Instance.CreateHistoCumulativeCustomer(histoCumulativeCustomer);
        }

        private List<HispaniaCompData.HistoCumulativeCustomer> _HistoCumulativeCustomersInDb;

        private List<HispaniaCompData.HistoCumulativeCustomer> HistoCumulativeCustomersInDb
        {
            get
            {
                return (this._HistoCumulativeCustomersInDb);
            }
            set
            {
                this._HistoCumulativeCustomersInDb = value;
            }
        }

        private void UpdateHistoCumulativeCustomerInDb(HispaniaCompData.HistoCumulativeCustomer histoCumulativeCustomer)
        {
            HispaniaDataAccess.Instance.UpdateHistoCumulativeCustomer(histoCumulativeCustomer);
        }

        private void DeleteHistoCumulativeCustomerInDb(HispaniaCompData.HistoCumulativeCustomer histoCumulativeCustomer)
        {
            HispaniaDataAccess.Instance.DeleteHistoCumulativeCustomer(histoCumulativeCustomer);
        }

        private HispaniaCompData.HistoCumulativeCustomer GetHistoCumulativeCustomerInDb(int HistoCumulativeCustomer_Id)
        {
            return HispaniaDataAccess.Instance.GetHistoCumulativeCustomer(HistoCumulativeCustomer_Id);
        }

        #endregion
    }
}
