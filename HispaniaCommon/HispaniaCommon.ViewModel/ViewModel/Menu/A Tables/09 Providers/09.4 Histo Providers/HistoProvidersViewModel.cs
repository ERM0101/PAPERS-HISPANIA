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

        public void CreateHistoProvider(HistoProvidersView histoProvidersView)
        {
            HispaniaCompData.HistoProvider histoProviderToCreate = histoProvidersView.GetHistoProvider();
            CreateHistoProviderInDb(histoProviderToCreate);
            histoProvidersView.HistoProvider_Id = histoProviderToCreate.HistoProvider_Id;
        }

        public void RefreshHistoProviders()
        {
            try
            {
                HistoProvidersInDb = HispaniaDataAccess.Instance.ReadHistoProviders();
                _HistoProviders = new ObservableCollection<HistoProvidersView>();
                _HistoProvidersInDictionary = new Dictionary<string, HistoProvidersView>();
                foreach (HispaniaCompData.HistoProvider histoProvider in HistoProvidersInDb)
                {
                    HistoProvidersView NewHistoProvidersView = new HistoProvidersView(histoProvider);
                    _HistoProviders.Add(NewHistoProvidersView);
                    _HistoProvidersInDictionary.Add(GetKeyHistoProviderView(histoProvider), NewHistoProvidersView);
                }
            }
            catch (Exception ex)
            {
                _HistoProviders = null;
                throw ex;
            }
        }

        public ObservableCollection<HistoProvidersView> GetHistoProviders(int Provider_Id, bool WithChilds = false)
        {
            try
            {
                ObservableCollection<HistoProvidersView>  HistoProvidersFiltered = new ObservableCollection<HistoProvidersView>();
                foreach (HispaniaCompData.HistoProvider histoProvider in HispaniaDataAccess.Instance.ReadHistoProviders(Provider_Id, WithChilds))
                {
                    HistoProvidersView NewHistoProvidersView = new HistoProvidersView(histoProvider);
                    HistoProvidersFiltered.Add(NewHistoProvidersView);
                }
                return HistoProvidersFiltered;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ObservableCollection<HistoProvidersView> _HistoProviders = null;

        public ObservableCollection<HistoProvidersView> HistoProviders
        {
            get
            {
                return _HistoProviders;
            }
        }

        private Dictionary<string, HistoProvidersView> _HistoProvidersInDictionary = null;

        public Dictionary<string, HistoProvidersView> HistoProvidersDict
        {
            get
            {
                return _HistoProvidersInDictionary;
            }
        }

        public string GetKeyHistoProviderView(HistoProvidersView histoProvidersView)
        {
            return GetKeyHistoProviderView(histoProvidersView.HistoProvider_Id);
        }

        private string GetKeyHistoProviderView(HispaniaCompData.HistoProvider histoProvider)
        {
            return GetKeyHistoProviderView(histoProvider.HistoProvider_Id);
        }

        private string GetKeyHistoProviderView(int Hist_Id)
        {
            return string.Format("{0}", Hist_Id);
        }


        public void UpdateHistoProvider(HistoProvidersView histoProviderView)
        {
            UpdateHistoProviderInDb(histoProviderView.GetHistoProvider());
        }
        public void DeleteHistoProvider(HistoProvidersView histoProviderView)
        {
            DeleteHistoProviderInDb(histoProviderView.GetHistoProvider());
        }

        public HistoProvidersView GetHistoProviderFromDb(HistoProvidersView providersView)
        {
            return new HistoProvidersView(GetHistoProviderInDb(providersView.HistoProvider_Id));
        }

        public HispaniaCompData.HistoProvider GetHistoProvider(int HistoProvider_Id)
        {
            return GetHistoProviderInDb(HistoProvider_Id);
        }

        #endregion

        #region DataBase [CRUD]

        private void CreateHistoProviderInDb(HispaniaCompData.HistoProvider histoProvider)
        {
            HispaniaDataAccess.Instance.CreateHistoProvider(histoProvider);
        }

        private List<HispaniaCompData.HistoProvider> _HistoProvidersInDb;

        private List<HispaniaCompData.HistoProvider> HistoProvidersInDb
        {
            get
            {
                return (this._HistoProvidersInDb);
            }
            set
            {
                this._HistoProvidersInDb = value;
            }
        }

        private void UpdateHistoProviderInDb(HispaniaCompData.HistoProvider histoProvider)
        {
            HispaniaDataAccess.Instance.UpdateHistoProvider(histoProvider);
        }

        private void DeleteHistoProviderInDb(HispaniaCompData.HistoProvider histoProvider)
        {
            HispaniaDataAccess.Instance.DeleteHistoProvider(histoProvider);
        }

        private HispaniaCompData.HistoProvider GetHistoProviderInDb(int HistoProvider_Id)
        {
            return HispaniaDataAccess.Instance.GetHistoProvider(HistoProvider_Id);
        }

        #endregion
    }
}
