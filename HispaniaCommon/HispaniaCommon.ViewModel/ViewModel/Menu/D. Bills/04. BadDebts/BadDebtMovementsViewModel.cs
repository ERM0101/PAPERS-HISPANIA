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

        public void CreateBadDebtMovement(BadDebtMovementsView BadDebtMovementsView)
        {
            HispaniaCompData.BadDebtMovement BadDebtMovementToCreate = BadDebtMovementsView.GetBadDebtMovement();
            CreateBadDebtMovementInDb(BadDebtMovementToCreate);
            BadDebtMovementsView.BadDebtMovement_Id = BadDebtMovementToCreate.BadDebtMovement_Id;
            RefreshBadDebtMovements();
        }

        public void RefreshBadDebtMovements()
        {
            try
            {
                BadDebtMovementsInDb = HispaniaDataAccess.Instance.ReadBadDebtMovements();
                _BadDebtMovements = new ObservableCollection<BadDebtMovementsView>();
                _BadDebtMovementsInDictionary = new Dictionary<string, BadDebtMovementsView>();
                foreach (HispaniaCompData.BadDebtMovement BadDebtMovement in BadDebtMovementsInDb)
                {
                    BadDebtMovementsView NewBadDebtMovementsView = new BadDebtMovementsView(BadDebtMovement);
                    _BadDebtMovements.Add(NewBadDebtMovementsView);
                    _BadDebtMovementsInDictionary.Add(GetKeyBadDebtMovementView(BadDebtMovement), NewBadDebtMovementsView);
                }
            }
            catch (Exception ex)
            {
                _BadDebtMovements = null;
                throw ex;
            }
        }

        public ObservableCollection<BadDebtMovementsView> GetBadDebtMovements(int BadDebt_Id)
        {
            try
            {
                ObservableCollection<BadDebtMovementsView> BadDebtMovementsFiltered = new ObservableCollection<BadDebtMovementsView>();
                foreach (HispaniaCompData.BadDebtMovement BadDebtMovement in HispaniaDataAccess.Instance.GetBadDebtMovementsInDb(BadDebt_Id))
                {
                    BadDebtMovementsView NewBadDebtMovementsView = new BadDebtMovementsView(BadDebtMovement);
                    BadDebtMovementsFiltered.Add(NewBadDebtMovementsView);
                }
                return BadDebtMovementsFiltered;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ObservableCollection<BadDebtMovementsView> _BadDebtMovements = null;

        public ObservableCollection<BadDebtMovementsView> BadDebtMovements
        {
            get
            {
                return _BadDebtMovements;
            }
        }

        private Dictionary<string, BadDebtMovementsView> _BadDebtMovementsInDictionary = null;

        public Dictionary<string, BadDebtMovementsView> BadDebtMovementsDict
        {
            get
            {
                return _BadDebtMovementsInDictionary;
            }
        }

        public string GetKeyBadDebtMovementView(BadDebtMovementsView BadDebtMovementsView)
        {
            return GetKeyBadDebtMovementView(BadDebtMovementsView.BadDebtMovement_Id);
        }

        private string GetKeyBadDebtMovementView(HispaniaCompData.BadDebtMovement BadDebtMovement)
        {
            return GetKeyBadDebtMovementView(BadDebtMovement.BadDebtMovement_Id);
        }

        private string GetKeyBadDebtMovementView(int BadDebtMovement_Id)
        {
            return string.Format("{0}", BadDebtMovement_Id);
        }

        public void UpdateBadDebtMovement(BadDebtMovementsView BadDebtMovementView)
        {
            UpdateBadDebtMovementInDb(BadDebtMovementView.GetBadDebtMovement());
            RefreshBadDebtMovements();
        }

        public void DeleteBadDebtMovement(BadDebtMovementsView BadDebtMovementView)
        {
            DeleteBadDebtMovementInDb(BadDebtMovementView.GetBadDebtMovement());
            RefreshBadDebtMovements();
        }

        public HispaniaCompData.BadDebtMovement GetBadDebtMovement(int BadDebtMovement_Id)
        {
            return GetBadDebtMovementInDb(BadDebtMovement_Id);
        }

        #endregion

        #region DataBase [CRUD]

        private void CreateBadDebtMovementInDb(HispaniaCompData.BadDebtMovement BadDebtMovement)
        {
            HispaniaDataAccess.Instance.CreateBadDebtMovement(BadDebtMovement);
        }

        private List<HispaniaCompData.BadDebtMovement> _BadDebtMovementsInDb;

        private List<HispaniaCompData.BadDebtMovement> BadDebtMovementsInDb
        {
            get
            {
                return (this._BadDebtMovementsInDb);
            }
            set
            {
                this._BadDebtMovementsInDb = value;
            }
        }

        private void UpdateBadDebtMovementInDb(HispaniaCompData.BadDebtMovement BadDebtMovement)
        {
            HispaniaDataAccess.Instance.UpdateBadDebtMovement(BadDebtMovement);
        }

        private void DeleteBadDebtMovementInDb(HispaniaCompData.BadDebtMovement BadDebtMovement)
        {
            HispaniaDataAccess.Instance.DeleteBadDebtMovement(BadDebtMovement);
        }

        private HispaniaCompData.BadDebtMovement GetBadDebtMovementInDb(int BadDebtMovement_Id)
        {
            return HispaniaDataAccess.Instance.GetBadDebtMovement(BadDebtMovement_Id);
        }

        #endregion
    }
}
