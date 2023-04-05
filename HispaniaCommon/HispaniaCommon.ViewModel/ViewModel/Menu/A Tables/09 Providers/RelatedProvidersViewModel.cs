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

        public void CreateRelatedProvider(RelatedProvidersView relatedProvidersView)
        {
            HispaniaCompData.RelatedProvider relatedProviderToCreate = relatedProvidersView.GetRelatedProvider();
            CreateRelatedProviderInDb(relatedProviderToCreate);
        }

        public void RefreshRelatedProviders(int Provider_Id)
        {
            try
            {
                RelatedProvidersInDb = HispaniaDataAccess.Instance.ReadRelatedProviders(Provider_Id);
                _RelatedProviders = new ObservableCollection<RelatedProvidersView>();
                _RelatedProvidersInDictionary = new Dictionary<string, RelatedProvidersView>();
                foreach (HispaniaCompData.RelatedProvider relatedProvider in RelatedProvidersInDb)
                {
                    RelatedProvidersView NewRelatedProvidersView = new RelatedProvidersView(relatedProvider);
                    _RelatedProviders.Add(NewRelatedProvidersView);
                    _RelatedProvidersInDictionary.Add(GetKeyRelatedProviderView(relatedProvider), NewRelatedProvidersView);
                }
            }
            catch (Exception ex)
            {
                _RelatedProviders = null;
                throw ex;
            }
        }

        private ObservableCollection<RelatedProvidersView> _RelatedProviders = null;

        public ObservableCollection<RelatedProvidersView> RelatedProviders
        {
            get
            {
                return _RelatedProviders;
            }
        }

        private Dictionary<string, RelatedProvidersView> _RelatedProvidersInDictionary = null;

        public Dictionary<string, RelatedProvidersView> RelatedProvidersDict
        {
            get
            {
                return _RelatedProvidersInDictionary;
            }
        }

        public string GetKeyRelatedProviderView(RelatedProvidersView relatedProvidersView)
        {
            return GetKeyRelatedProviderView(relatedProvidersView.Provider_Id, relatedProvidersView.Provider_Canceled_Id);
        }

        private string GetKeyRelatedProviderView(HispaniaCompData.RelatedProvider relatedProvider)
        {
            return GetKeyRelatedProviderView(relatedProvider.Provider_Id, relatedProvider.Provider_Canceled_Id);
        }

        private string GetKeyRelatedProviderView(int Provider_Id, int Provider_Canceled_Id)
        {
            return string.Format("{0}-{1}", Provider_Id, Provider_Canceled_Id);
        }

        public void UpdateRelatedProvider(RelatedProvidersView RelatedProvidersView)
        {
            UpdateRelatedProviderInDb(RelatedProvidersView.GetRelatedProvider());
        }
        public void DeleteRelatedProvider(RelatedProvidersView RelatedProvidersView)
        {
            DeleteRelatedProviderInDb(RelatedProvidersView.GetRelatedProvider());
        }

        public RelatedProvidersView GetRelatedProviderFromDb(RelatedProvidersView relatedProvidersView)
        {
            return new RelatedProvidersView(GetRelatedProviderInDb(relatedProvidersView.Provider_Id, relatedProvidersView.Provider_Canceled_Id));
        }

        public HispaniaCompData.RelatedProvider GetRelatedProvider(int Provider_Id, int Provider_Canceled_Id)
        {
            return GetRelatedProviderInDb(Provider_Id, Provider_Canceled_Id);
        }

        #endregion

        #region DataBase [CRUD]

        private void CreateRelatedProviderInDb(HispaniaCompData.RelatedProvider provider)
        {
            HispaniaDataAccess.Instance.CreateRelatedProvider(provider);
        }

        private List<HispaniaCompData.RelatedProvider> _RelatedProvidersInDb;

        private List<HispaniaCompData.RelatedProvider> RelatedProvidersInDb
        {
            get
            {
                return (this._RelatedProvidersInDb);
            }
            set
            {
                this._RelatedProvidersInDb = value;
            }
        }

        private void UpdateRelatedProviderInDb(HispaniaCompData.RelatedProvider provider)
        {
            HispaniaDataAccess.Instance.UpdateRelatedProvider(provider);
        }

        private void DeleteRelatedProviderInDb(HispaniaCompData.RelatedProvider provider)
        {
            HispaniaDataAccess.Instance.DeleteRelatedProvider(provider);
        }

        private HispaniaCompData.RelatedProvider GetRelatedProviderInDb(int Providers_Id, int Provider_Canceled_Id)
        {
            return HispaniaDataAccess.Instance.GetRelatedProvider(Providers_Id, Provider_Canceled_Id);
        }
        
        #endregion
    }
}
