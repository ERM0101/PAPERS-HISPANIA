#region Librerias usadas por la clase

using HispaniaCommon.DataAccess;
using HispaniaComptabilitat.Data;
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

        public void CreateProvider(ProvidersView ProvidersView)
        {
            HispaniaCompData.Provider ProviderToCreate = ProvidersView.GetProvider();
            CreateProviderInDb(ProviderToCreate);
            ProvidersView.Provider_Id = ProviderToCreate.Provider_Id;
        }

        public void RefreshProviders()
        {
            try
            {
                ProvidersInDb = HispaniaDataAccess.Instance.ReadProviders();
                _Providers = new ObservableCollection<ProvidersView>();
                _ProvidersInDictionary = new Dictionary<string, ProvidersView>();
                _ProvidersActiveInDictionary = new Dictionary<string, ProvidersView>();
                foreach (HispaniaCompData.Provider Provider in ProvidersInDb)
                {
                    ProvidersView NewProvidersView = new ProvidersView(Provider);
                    _Providers.Add(NewProvidersView);
                    _ProvidersInDictionary.Add(GetKeyProviderView(Provider), NewProvidersView);
                    if (!NewProvidersView.Canceled) _ProvidersActiveInDictionary.Add(GetKeyProviderView(Provider), NewProvidersView);
                }
            }
            catch (Exception ex)
            {
                _Providers = null;
                throw ex;
            }
        }

        private ObservableCollection<ProvidersView> _Providers = null;

        public ObservableCollection<ProvidersView> Providers
        {
            get
            {
                return _Providers;
            }
        }

        private Dictionary<string, ProvidersView> _ProvidersActiveInDictionary = null;

        public Dictionary<string, ProvidersView> ProvidersActiveDict
        {
            get
            {
                return _ProvidersActiveInDictionary;
            }
        }

        private Dictionary<string, ProvidersView> _ProvidersInDictionary = null;

        public Dictionary<string, ProvidersView> ProvidersDict
        {
            get
            {
                return _ProvidersInDictionary;
            }
        }

        public string GetKeyProviderView(ProvidersView ProvidersView)
        {
            return GetKeyProviderView(ProvidersView.Provider_Number, ProvidersView.Name);
        }

        private string GetKeyProviderView(HispaniaCompData.Provider Provider)
        {
            return GetKeyProviderView(Provider.Provider_Number, Provider.Name);
        }

        private string GetKeyProviderView(int Provider_Number, string Provider_Name)
        {
            return string.Format("{0:00000} | {1}", Provider_Number, Provider_Name);
        }

        public void UpdateProvider(ProvidersView ProvidersView, List<RelatedProvidersView> NewOrEditedRelatedProviders)
        {
            UpdateProviderInDb(ProvidersView.GetProvider(), GetRelatedProvidersForDb(NewOrEditedRelatedProviders));
        }

        private List<HispaniaCompData.RelatedProvider> GetRelatedProvidersForDb(List<RelatedProvidersView> RelatedProviders)
        {
            List<HispaniaCompData.RelatedProvider> RelatedProvidersForDb = new List<HispaniaCompData.RelatedProvider>();
            foreach (RelatedProvidersView relatedCutomer in RelatedProviders)
            {
                RelatedProvidersForDb.Add(relatedCutomer.GetRelatedProvider());
            }
            return RelatedProvidersForDb;
        }

        public void DeleteProvider(ProvidersView ProvidersView)
        {
            DeleteProviderInDb(ProvidersView.GetProvider());
        }

        public ProvidersView GetProviderFromDb(ProvidersView providersView)
        {
            return new ProvidersView(GetProviderInDb(providersView.Provider_Id));
        }

        public HispaniaCompData.Provider GetProvider(int Providers_Id)
        {
            return GetProviderInDb(Providers_Id);
        }

        #endregion

        #region DataBase [CRUD]

        private void CreateProviderInDb(HispaniaCompData.Provider Provider)
        {
            HispaniaDataAccess.Instance.CreateProvider(Provider);
        }

        private List<HispaniaCompData.Provider> _ProvidersInDb;

        private List<HispaniaCompData.Provider> ProvidersInDb
        {
            get
            {
                return (this._ProvidersInDb);
            }
            set
            {
                this._ProvidersInDb = value;
            }
        }

        private void UpdateProviderInDb(HispaniaCompData.Provider Provider, List<HispaniaCompData.RelatedProvider> relatedProviders)
        {
            HispaniaDataAccess.Instance.UpdateProvider(Provider, relatedProviders);
        }

        private void DeleteProviderInDb(HispaniaCompData.Provider Provider)
        {
            HispaniaDataAccess.Instance.DeleteProvider(Provider);
        }

        private HispaniaCompData.Provider GetProviderInDb(int Providers_Id)
        {
            return HispaniaDataAccess.Instance.GetProvider(Providers_Id);
        }
        
        #endregion
    }
}
