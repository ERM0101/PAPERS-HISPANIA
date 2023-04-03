#region Libraries used by the class

using HispaniaCommon.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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

        public void CreateProviderOrderMovement(ProviderOrderMovementsView ProviderOrderMovementsView)
        {
            HispaniaCompData.ProviderOrderMovement ProviderOrderMovementToCreate = ProviderOrderMovementsView.GetProviderOrderMovement();
            CreateProviderOrderMovementInDb(ProviderOrderMovementToCreate);
            ProviderOrderMovementsView.ProviderOrderMovement_Id = ProviderOrderMovementToCreate.ProviderOrderMovement_Id;
            RefreshProviderOrderMovements();
        }

        public void RefreshProviderOrderMovements()
        {
            try
            {
                ProviderOrderMovementsInDb = HispaniaDataAccess.Instance.ReadProviderOrderMovements();
                _ProviderOrderMovements = new ObservableCollection<ProviderOrderMovementsView>();
                _ProviderOrderMovementsInDictionary = new Dictionary<string, ProviderOrderMovementsView>();
                foreach (HispaniaCompData.ProviderOrderMovement ProviderOrderMovement in ProviderOrderMovementsInDb)
                {
                    ProviderOrderMovementsView NewProviderOrderMovementsView = new ProviderOrderMovementsView(ProviderOrderMovement);
                    _ProviderOrderMovements.Add(NewProviderOrderMovementsView);
                    _ProviderOrderMovementsInDictionary.Add(GetKeyProviderOrderMovementView(ProviderOrderMovement), NewProviderOrderMovementsView);
                }
            }
            catch (Exception ex)
            {
                _ProviderOrderMovements = null;
                throw ex;
            }
        }

        public ObservableCollection<ProviderOrderMovementsView> GetProviderOrderMovements(int ProviderOrder_Id)
        {
            try
            {
                ObservableCollection<ProviderOrderMovementsView> ProviderOrderMovementsFiltered = new ObservableCollection<ProviderOrderMovementsView>();
                var providersOrderMovementsOrdered = ReadProviderOrderMovementsInDb(ProviderOrder_Id).OrderBy(x => x.RowOrder);
                if (providersOrderMovementsOrdered.Any() && providersOrderMovementsOrdered.FirstOrDefault().RowOrder==null)
                {
                    var i = 0;
                    foreach (var movement in providersOrderMovementsOrdered)
                    {
                        movement.RowOrder = i;
                        i++;
                    }
                   
                }
            
                foreach (HispaniaCompData.ProviderOrderMovement providerOrderMovement in providersOrderMovementsOrdered)
                {
                    ProviderOrderMovementsView NewProviderOrderMovementsView = new ProviderOrderMovementsView(providerOrderMovement);
                    ProviderOrderMovementsFiltered.Add(NewProviderOrderMovementsView);
                }
                return ProviderOrderMovementsFiltered;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ObservableCollection<ProviderOrderMovementsView> _ProviderOrderMovements = null;

        public ObservableCollection<ProviderOrderMovementsView> ProviderOrderMovements
        {
            get
            {
                return _ProviderOrderMovements;
            }
        }

        private Dictionary<string, ProviderOrderMovementsView> _ProviderOrderMovementsInDictionary = null;

        public Dictionary<string, ProviderOrderMovementsView> ProviderOrderMovementsDict
        {
            get
            {
                return _ProviderOrderMovementsInDictionary;
            }
        }

        public string GetKeyProviderOrderMovementView(ProviderOrderMovementsView ProviderOrderMovementsView)
        {
            return GetKeyProviderOrderMovementView(ProviderOrderMovementsView.ProviderOrderMovement_Id);
        }

        private string GetKeyProviderOrderMovementView(HispaniaCompData.ProviderOrderMovement ProviderOrderMovement)
        {
            return GetKeyProviderOrderMovementView(ProviderOrderMovement.ProviderOrderMovement_Id);
        }

        private string GetKeyProviderOrderMovementView(int CoustomerOrder_Id)
        {
            return string.Format("{0}", CoustomerOrder_Id);
        }


        public void UpdateProviderOrderMovement(ProviderOrderMovementsView ProviderOrderMovementView)
        {
            UpdateProviderOrderMovementInDb(ProviderOrderMovementView.GetProviderOrderMovement());
            RefreshProviderOrderMovements();
        }
        public void DeleteProviderOrderMovement(ProviderOrderMovementsView ProviderOrderMovementView)
        {
            DeleteProviderOrderMovementInDb(ProviderOrderMovementView.GetProviderOrderMovement());
            RefreshProviderOrderMovements();
        }

        public HispaniaCompData.ProviderOrderMovement GetProviderOrderMovement(int ProviderOrderMovement_Id)
        {
            return GetProviderOrderMovementInDb(ProviderOrderMovement_Id);
        }

        #endregion

        #region DataBase [CRUD]

        private List<HispaniaCompData.ProviderOrderMovement> ReadProviderOrderMovementsInDb(int ProviderOrder_Id)
        {
            return (HispaniaDataAccess.Instance.ReadProviderOrderMovements(ProviderOrder_Id));
        }

        private void CreateProviderOrderMovementInDb(HispaniaCompData.ProviderOrderMovement providerOrderMovement)
        {
            HispaniaDataAccess.Instance.CreateProviderOrderMovement(providerOrderMovement);
        }

        private List<HispaniaCompData.ProviderOrderMovement> _ProviderOrderMovementsInDb;

        private List<HispaniaCompData.ProviderOrderMovement> ProviderOrderMovementsInDb
        {
            get
            {
                return (this._ProviderOrderMovementsInDb);
            }
            set
            {
                this._ProviderOrderMovementsInDb = value;
            }
        }

        private void UpdateProviderOrderMovementInDb(HispaniaCompData.ProviderOrderMovement providerOrderMovement)
        {
            HispaniaDataAccess.Instance.UpdateProviderOrderMovement(providerOrderMovement);
        }

        private void DeleteProviderOrderMovementInDb(HispaniaCompData.ProviderOrderMovement providerOrderMovement)
        {
            HispaniaDataAccess.Instance.DeleteProviderOrderMovement(providerOrderMovement);
        }

        private HispaniaCompData.ProviderOrderMovement GetProviderOrderMovementInDb(int ProviderOrderMovement_Id)
        {
            return HispaniaDataAccess.Instance.GetProviderOrderMovement(ProviderOrderMovement_Id);
        }

        #endregion
    }
}
