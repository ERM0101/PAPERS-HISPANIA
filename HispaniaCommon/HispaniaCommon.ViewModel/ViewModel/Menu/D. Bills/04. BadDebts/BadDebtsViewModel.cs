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

        public void CreateBadDebt(BadDebtsView badDebtsView, List<BadDebtMovementsView> NewOrEditedBadDebtMovements)
        {
            if (GetBadDebtFromReceiptInDb(badDebtsView.Receipt_Id) is null)
            {
                HispaniaCompData.BadDebt badDebtToCreate = badDebtsView.GetBadDebt();
                List<HispaniaCompData.BadDebtMovement> badDebtMovementsToCreate = new List<HispaniaCompData.BadDebtMovement>();
                foreach (BadDebtMovementsView payement in NewOrEditedBadDebtMovements)
                {
                    badDebtMovementsToCreate.Add(payement.GetBadDebtMovement());
                }
                CreateBadDebtInDb(badDebtToCreate, badDebtMovementsToCreate);
                badDebtsView.BadDebt_Id = badDebtToCreate.BadDebt_Id;
                RefreshBadDebts();
            }
            else
            {
                throw new Exception(string.Format("Error, ja hi ha un impagat associat pel rebut seleccionat '{0}'.", badDebtsView.Receipt_Id));
            }
        }

        public void RefreshBadDebts()
        {
            try
            {
                BadDebtsInDb = HispaniaDataAccess.Instance.ReadBadDebts();
                _BadDebts = new ObservableCollection<BadDebtsView>();
                _BadDebtsInDictionary = new Dictionary<string, BadDebtsView>();
                foreach (HispaniaCompData.BadDebt badDebt in BadDebtsInDb)
                {
                    BadDebtsView NewBadDebtsView = new BadDebtsView(badDebt);
                    _BadDebts.Add(NewBadDebtsView);
                    _BadDebtsInDictionary.Add(GetKeyBadDebtView(badDebt), NewBadDebtsView);
                }
            }
            catch (Exception ex)
            {
                _BadDebts = null;
                throw ex;
            }
        }

        public ObservableCollection<BadDebtsView> GetBadDebts(int Customer_Id)
        {
            try
            {
                ObservableCollection<BadDebtsView> BadDebtsFiltered = new ObservableCollection<BadDebtsView>();
                foreach (HispaniaCompData.BadDebt badDebt in HispaniaDataAccess.Instance.GetBadDebtsInDb(Customer_Id))
                {
                    BadDebtsView NewBadDebtsView = new BadDebtsView(badDebt);
                    BadDebtsFiltered.Add(NewBadDebtsView);
                }
                return BadDebtsFiltered;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ObservableCollection<BadDebtsView> _BadDebts = null;

        public ObservableCollection<BadDebtsView> BadDebts
        {
            get
            {
                return _BadDebts;
            }
        }

        private Dictionary<string, BadDebtsView> _BadDebtsInDictionary = null;

        public Dictionary<string, BadDebtsView> BadDebtsDict
        {
            get
            {
                return _BadDebtsInDictionary;
            }
        }

        public string GetKeyBadDebtView(BadDebtsView badDebtsView)
        {
            return GetKeyBadDebtView(badDebtsView.BadDebt_Id);
        }

        private string GetKeyBadDebtView(HispaniaCompData.BadDebt badDebt)
        {
            return GetKeyBadDebtView(badDebt.BadDebt_Id);
        }

        private string GetKeyBadDebtView(int BadDebt_Id)
        {
            return string.Format("{0}", BadDebt_Id);
        }


        public void UpdateBadDebt(BadDebtsView badDebtViewOld, BadDebtsView badDebtView, List<BadDebtMovementsView> badDebtMovementsList)
        {
            List<HispaniaCompData.BadDebtMovement> badDebtMovements = new List<HispaniaCompData.BadDebtMovement>();
            foreach (BadDebtMovementsView payement in badDebtMovementsList)
            {
                badDebtMovements.Add(payement.GetBadDebtMovement());
            }
            UpdateBadDebtInDb(badDebtViewOld.GetBadDebt(), badDebtView.GetBadDebt(), badDebtMovements);
            RefreshBadDebts();
        }
        public void DeleteBadDebt(BadDebtsView badDebtView)
        {
            DeleteBadDebtInDb(badDebtView.GetBadDebt());
            RefreshBadDebts();
        }

        public HispaniaCompData.BadDebt GetBadDebt(int BadDebt_Id)
        {
            return GetBadDebtInDb(BadDebt_Id);
        }

        public BadDebtsView GetBadDebtFromDb(BadDebtsView BadDebtsView)
        {
            return new BadDebtsView(GetBadDebtInDb(BadDebtsView.BadDebt_Id));
        }

        #endregion

        #region DataBase [CRUD]

        private void CreateBadDebtInDb(HispaniaCompData.BadDebt badDebt, List<HispaniaCompData.BadDebtMovement> badDebtMovementsToCreate)
        {
            HispaniaDataAccess.Instance.CreateBadDebt(badDebt, badDebtMovementsToCreate);
        }

        private List<HispaniaCompData.BadDebt> _BadDebtsInDb;

        private List<HispaniaCompData.BadDebt> BadDebtsInDb
        {
            get
            {
                return (this._BadDebtsInDb);
            }
            set
            {
                this._BadDebtsInDb = value;
            }
        }

        private void UpdateBadDebtInDb(HispaniaCompData.BadDebt badDebtOld, HispaniaCompData.BadDebt badDebt, List<HispaniaCompData.BadDebtMovement> badDebtMovements)
        {
            HispaniaDataAccess.Instance.UpdateBadDebt(badDebtOld, badDebt, badDebtMovements);
        }

        private void DeleteBadDebtInDb(HispaniaCompData.BadDebt badDebt)
        {
            HispaniaDataAccess.Instance.DeleteBadDebt(badDebt);
        }

        private HispaniaCompData.BadDebt GetBadDebtInDb(int BadDebt_Id)
        {
            return HispaniaDataAccess.Instance.GetBadDebt(BadDebt_Id);
        }

        private HispaniaCompData.BadDebt GetBadDebtFromReceiptInDb(int Receipt_Id)
        {
            return HispaniaDataAccess.Instance.GetBadDebtFromReceiptInDb(Receipt_Id);
        }

        #endregion
    }
}
