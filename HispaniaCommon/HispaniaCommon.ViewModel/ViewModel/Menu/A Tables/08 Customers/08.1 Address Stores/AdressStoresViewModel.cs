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

        public void CreateAddressStore(AddressStoresView AddressStoresView)
        {
            HispaniaCompData.AddressStore customerToCreate = AddressStoresView.GetAddressStore();
            CreateAddressStoreInDb(customerToCreate);
            AddressStoresView.AddressStore_Id = customerToCreate.AddressStore_Id;
            RefreshAddressStores();
        }

        public void RefreshAddressStores()
        {
            try
            {
                AddressStoresInDb = HispaniaDataAccess.Instance.ReadAddressStores();
                _AddressStores = new ObservableCollection<AddressStoresView>();
                _AddressStoresInDictionary = new Dictionary<string, AddressStoresView>();
                foreach (HispaniaCompData.AddressStore customer in AddressStoresInDb)
                {
                    AddressStoresView NewAddressStoresView = new AddressStoresView(customer);
                    _AddressStores.Add(NewAddressStoresView);
                    _AddressStoresInDictionary.Add(GetKeyAddressStoreView(customer), NewAddressStoresView);
                }
            }
            catch (Exception ex)
            {
                _AddressStores = null;
                throw ex;
            }
        }

        public ObservableCollection<AddressStoresView> GetAddressStores(int Customer_Id)
        {
            try
            {
                ObservableCollection<AddressStoresView> AddressStoresFiltered = new ObservableCollection<AddressStoresView>();
                foreach (HispaniaCompData.AddressStore AddressStore in HispaniaDataAccess.Instance.ReadAddressStores(Customer_Id))
                {
                    AddressStoresView NewAddressStoresView = new AddressStoresView(AddressStore);
                    AddressStoresFiltered.Add(NewAddressStoresView);
                }
                return AddressStoresFiltered;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ObservableCollection<AddressStoresView> _AddressStores = null;

        public ObservableCollection<AddressStoresView> AddressStores
        {
            get
            {
                return _AddressStores;
            }
        }

        private Dictionary<string, AddressStoresView> _AddressStoresInDictionary = null;

        public Dictionary<string, AddressStoresView> AddressStoresDict
        {
            get
            {
                return _AddressStoresInDictionary;
            }
        }

        public string GetKeyAddressStoreView(AddressStoresView AddressStoresView)
        {
            return GetKeyAddressStoreView(AddressStoresView.AddressStore_Id);
        }

        private string GetKeyAddressStoreView(HispaniaCompData.AddressStore AddressStore)
        {
            return GetKeyAddressStoreView(AddressStore.AddressStore_Id);
        }

        private string GetKeyAddressStoreView(int AddressStore_Id)
        {
            return string.Format("{0}", AddressStore_Id);
        }

        public void UpdateAddressStore(AddressStoresView AddressStoresView)
        {
            UpdateAddressStoreInDb(AddressStoresView.GetAddressStore());
            RefreshAddressStores();
        }
        public void DeleteAddressStore(AddressStoresView AddressStoresView)
        {
            DeleteAddressStoreInDb(AddressStoresView.GetAddressStore());
            RefreshAddressStores();
        }

        public AddressStoresView GetAddressStoreFromDb(AddressStoresView addressStoresView)
        {
            return new AddressStoresView(GetAddressStoreInDb(addressStoresView.AddressStore_Id));
        }

        public HispaniaCompData.AddressStore GetAddressStore(int AddressStores_Id)
        {
            return GetAddressStoreInDb(AddressStores_Id);
        }

        #endregion

        #region DataBase [CRUD]

        private void CreateAddressStoreInDb(HispaniaCompData.AddressStore customer)
        {
            HispaniaDataAccess.Instance.CreateAddressStore(customer);
        }

        private List<HispaniaCompData.AddressStore> _AddressStoresInDb;

        private List<HispaniaCompData.AddressStore> AddressStoresInDb
        {
            get
            {
                return (this._AddressStoresInDb);
            }
            set
            {
                this._AddressStoresInDb = value;
            }
        }

        private void UpdateAddressStoreInDb(HispaniaCompData.AddressStore customer)
        {
            HispaniaDataAccess.Instance.UpdateAddressStore(customer);
        }

        private void DeleteAddressStoreInDb(HispaniaCompData.AddressStore customer)
        {
            HispaniaDataAccess.Instance.DeleteAddressStore(customer);
        }

        private HispaniaCompData.AddressStore GetAddressStoreInDb(int AddressStores_Id)
        {
            return HispaniaDataAccess.Instance.GetAddressStore(AddressStores_Id);
        }

        #endregion
    }
}
