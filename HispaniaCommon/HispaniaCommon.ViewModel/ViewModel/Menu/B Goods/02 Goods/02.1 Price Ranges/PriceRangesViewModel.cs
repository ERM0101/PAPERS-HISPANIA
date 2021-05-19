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

        public void CreatePriceRange(PriceRangesView PriceRangesView)
        {
            HispaniaCompData.PriceRange PriceRangeToCreate = PriceRangesView.GetPriceRange();
            CreatePriceRangeInDb(PriceRangeToCreate);
            PriceRangesView.PriceRange_Id = PriceRangeToCreate.PriceRange_Id;
            RefreshPriceRanges();
        }

        public void RefreshPriceRanges()
        {
            try
            {
                PriceRangesInDb = HispaniaDataAccess.Instance.ReadPriceRanges();
                _PriceRanges = new ObservableCollection<PriceRangesView>();
                _PriceRangesInDictionary = new Dictionary<string, PriceRangesView>();
                foreach (HispaniaCompData.PriceRange PriceRange in PriceRangesInDb)
                {
                    PriceRangesView NewPriceRangesView = new PriceRangesView(PriceRange);
                    _PriceRanges.Add(NewPriceRangesView);
                    _PriceRangesInDictionary.Add(GetKeyPriceRangeView(PriceRange), NewPriceRangesView);
                }
            }
            catch (Exception ex)
            {
                _PriceRanges = null;
                throw ex;
            }
        }

        public ObservableCollection<PriceRangesView> GetPriceRanges(int Good_Id)
        {
            try
            {
                ObservableCollection<PriceRangesView> PriceRangesFiltered = new ObservableCollection<PriceRangesView>();
                foreach (HispaniaCompData.PriceRange PriceRange in HispaniaDataAccess.Instance.ReadPriceRanges(Good_Id))
                {
                    PriceRangesView NewPriceRangesView = new PriceRangesView(PriceRange);
                    PriceRangesFiltered.Add(NewPriceRangesView);
                }
                return PriceRangesFiltered;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ObservableCollection<PriceRangesView> _PriceRanges = null;

        public ObservableCollection<PriceRangesView> PriceRanges
        {
            get
            {
                return _PriceRanges;
            }
        }

        private Dictionary<string, PriceRangesView> _PriceRangesInDictionary = null;

        public Dictionary<string, PriceRangesView> PriceRangesDict
        {
            get
            {
                return _PriceRangesInDictionary;
            }
        }

        public string GetKeyPriceRangeView(PriceRangesView PriceRangesView)
        {
            return GetKeyPriceRangeView(PriceRangesView.PriceRange_Id);
        }

        private string GetKeyPriceRangeView(HispaniaCompData.PriceRange PriceRange)
        {
            return GetKeyPriceRangeView(PriceRange.PriceRange_Id);
        }

        private string GetKeyPriceRangeView(int PriceRange_Id)
        {
            return string.Format("{0}", PriceRange_Id);
        }

        public void UpdatePriceRange(PriceRangesView PriceRangeView)
        {
            UpdatePriceRangeInDb(PriceRangeView.GetPriceRange());
            RefreshPriceRanges();
        }
        public void DeletePriceRange(PriceRangesView PriceRangeView)
        {
            DeletePriceRangeInDb(PriceRangeView.GetPriceRange());
            RefreshPriceRanges();
        }

        public PriceRangesView GetPriceRangeFromDb(PriceRangesView priceRangesView)
        {
            return new PriceRangesView(GetPriceRangeInDb(priceRangesView.PriceRange_Id));
        }

        public HispaniaCompData.PriceRange GetPriceRange(int PriceRange_Id)
        {
            return GetPriceRangeInDb(PriceRange_Id);
        }

        #endregion

        #region DataBase [CRUD]

        private void CreatePriceRangeInDb(HispaniaCompData.PriceRange PriceRange)
        {
            HispaniaDataAccess.Instance.CreatePriceRange(PriceRange);
        }

        private List<HispaniaCompData.PriceRange> _PriceRangesInDb;

        private List<HispaniaCompData.PriceRange> PriceRangesInDb
        {
            get
            {
                return (this._PriceRangesInDb);
            }
            set
            {
                this._PriceRangesInDb = value;
            }
        }

        private void UpdatePriceRangeInDb(HispaniaCompData.PriceRange PriceRange)
        {
            HispaniaDataAccess.Instance.UpdatePriceRange(PriceRange);
        }

        private void DeletePriceRangeInDb(HispaniaCompData.PriceRange PriceRange)
        {
            HispaniaDataAccess.Instance.DeletePriceRange(PriceRange);
        }

        private HispaniaCompData.PriceRange GetPriceRangeInDb(int PriceRange_Id)
        {
            return HispaniaDataAccess.Instance.GetPriceRange(PriceRange_Id);
        }

        #endregion
    }
}
