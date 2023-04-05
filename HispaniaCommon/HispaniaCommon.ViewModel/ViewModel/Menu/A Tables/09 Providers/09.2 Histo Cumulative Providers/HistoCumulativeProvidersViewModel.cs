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

        public void CreateHistoCumulativeProvider(HistoCumulativeProvidersView histoCumulativeProvidersView)
        {
            HispaniaCompData.HistoCumulativeProvider histoCumulativeProviderToCreate = histoCumulativeProvidersView.GetHistoCumulativeProvider();
            CreateHistoCumulativeProviderInDb(histoCumulativeProviderToCreate);
            histoCumulativeProvidersView.Histo_Id = histoCumulativeProviderToCreate.Histo_Id;
            RefreshHistoCumulativeProviders();
        }

        public void RefreshHistoCumulativeProviders()
        {
            try
            {
                HistoCumulativeProvidersInDb = HispaniaDataAccess.Instance.ReadHistoCumulativeProviders();
                _HistoCumulativeProviders = new ObservableCollection<HistoCumulativeProvidersView>();
                _HistoCumulativeProvidersInDictionary = new Dictionary<string, HistoCumulativeProvidersView>();
                foreach (HispaniaCompData.HistoCumulativeProvider histoCumulativeProvider in HistoCumulativeProvidersInDb)
                {
                    HistoCumulativeProvidersView NewHistoCumulativeProvidersView = new HistoCumulativeProvidersView(histoCumulativeProvider);
                    _HistoCumulativeProviders.Add(NewHistoCumulativeProvidersView);
                    _HistoCumulativeProvidersInDictionary.Add(GetKeyHistoCumulativeProviderView(histoCumulativeProvider), NewHistoCumulativeProvidersView);
                }
            }
            catch (Exception ex)
            {
                _HistoCumulativeProviders = null;
                throw ex;
            }
        }

        public ObservableCollection<HistoCumulativeProvidersView> GetHistoCumulativeProviders(int Provider_Id)
        {
            try
            {
                ObservableCollection<HistoCumulativeProvidersView>  HistoCumulativeProvidersFiltered = new ObservableCollection<HistoCumulativeProvidersView>();
                foreach (HispaniaCompData.HistoCumulativeProvider histoCumulativeProvider in HispaniaDataAccess.Instance.ReadHistoCumulativeProvider(Provider_Id))
                {
                    HistoCumulativeProvidersView NewHistoCumulativeProvidersView = new HistoCumulativeProvidersView(histoCumulativeProvider);
                    HistoCumulativeProvidersFiltered.Add(NewHistoCumulativeProvidersView);
                }
                return HistoCumulativeProvidersFiltered;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ObservableCollection<HistoCumulativeProvidersView> _HistoCumulativeProviders = null;

        public ObservableCollection<HistoCumulativeProvidersView> HistoCumulativeProviders
        {
            get
            {
                return _HistoCumulativeProviders;
            }
        }

        private Dictionary<string, HistoCumulativeProvidersView> _HistoCumulativeProvidersInDictionary = null;

        public Dictionary<string, HistoCumulativeProvidersView> HistoCumulativeProvidersDict
        {
            get
            {
                return _HistoCumulativeProvidersInDictionary;
            }
        }

        public string GetKeyHistoCumulativeProviderView(HistoCumulativeProvidersView histoCumulativeProvidersView)
        {
            return GetKeyHistoCumulativeProviderView(histoCumulativeProvidersView.Histo_Id);
        }

        private string GetKeyHistoCumulativeProviderView(HispaniaCompData.HistoCumulativeProvider histoCumulativeProvider)
        {
            return GetKeyHistoCumulativeProviderView(histoCumulativeProvider.Histo_Id);
        }

        private string GetKeyHistoCumulativeProviderView(int Hist_Id)
        {
            return string.Format("{0}", Hist_Id);
        }


        public void UpdateHistoCumulativeProvider(HistoCumulativeProvidersView histoCumulativeProviderView)
        {
            UpdateHistoCumulativeProviderInDb(histoCumulativeProviderView.GetHistoCumulativeProvider());
            RefreshHistoCumulativeProviders();
        }
        public void DeleteHistoCumulativeProvider(HistoCumulativeProvidersView histoCumulativeProviderView)
        {
            DeleteHistoCumulativeProviderInDb(histoCumulativeProviderView.GetHistoCumulativeProvider());
            RefreshHistoCumulativeProviders();
        }

        public HispaniaCompData.HistoCumulativeProvider GetHistoCumulativeProvider(int HistoCumulativeProvider_Id)
        {
            return GetHistoCumulativeProviderInDb(HistoCumulativeProvider_Id);
        }

        #endregion

        #region DataBase [CRUD]

        private void CreateHistoCumulativeProviderInDb(HispaniaCompData.HistoCumulativeProvider histoCumulativeProvider)
        {
            HispaniaDataAccess.Instance.CreateHistoCumulativeProvider(histoCumulativeProvider);
        }

        private List<HispaniaCompData.HistoCumulativeProvider> _HistoCumulativeProvidersInDb;

        private List<HispaniaCompData.HistoCumulativeProvider> HistoCumulativeProvidersInDb
        {
            get
            {
                return (this._HistoCumulativeProvidersInDb);
            }
            set
            {
                this._HistoCumulativeProvidersInDb = value;
            }
        }

        private void UpdateHistoCumulativeProviderInDb(HispaniaCompData.HistoCumulativeProvider histoCumulativeProvider)
        {
            HispaniaDataAccess.Instance.UpdateHistoCumulativeProvider(histoCumulativeProvider);
        }

        private void DeleteHistoCumulativeProviderInDb(HispaniaCompData.HistoCumulativeProvider histoCumulativeProvider)
        {
            HispaniaDataAccess.Instance.DeleteHistoCumulativeProvider(histoCumulativeProvider);
        }

        private HispaniaCompData.HistoCumulativeProvider GetHistoCumulativeProviderInDb(int HistoCumulativeProvider_Id)
        {
            return HispaniaDataAccess.Instance.GetHistoCumulativeProvider(HistoCumulativeProvider_Id);
        }

        #endregion
    }
}
