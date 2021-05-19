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

        public void CreateHistoGood(HistoGoodsView HistoGoodsView)
        {
            HispaniaCompData.HistoGood HistoGoodToCreate = HistoGoodsView.GetHistoGood();
            CreateHistoGoodInDb(HistoGoodToCreate);
            HistoGoodsView.Histo_Id = HistoGoodToCreate.Histo_Id;
            RefreshHistoGoods();
        }

        public void RefreshHistoGoods()
        {
            try
            {
                HistoGoodsInDb = HispaniaDataAccess.Instance.ReadHistoGoods();
                _HistoGoods = new ObservableCollection<HistoGoodsView>();
                _HistoGoodsInDictionary = new Dictionary<string, HistoGoodsView>();
                foreach (HispaniaCompData.HistoGood HistoGood in HistoGoodsInDb)
                {
                    HistoGoodsView NewHistoGoodsView = new HistoGoodsView(HistoGood);
                    _HistoGoods.Add(NewHistoGoodsView);
                    _HistoGoodsInDictionary.Add(GetKeyHistoGoodView(HistoGood), NewHistoGoodsView);
                }
            }
            catch (Exception ex)
            {
                _HistoGoods = null;
                throw ex;
            }
        }

        public ObservableCollection<HistoGoodsView> GetHistoGoods(int Good_Id)
        {
            try
            {
                ObservableCollection<HistoGoodsView> HistoGoodsFiltered = new ObservableCollection<HistoGoodsView>();
                foreach (HispaniaCompData.HistoGood HistoGood in HispaniaDataAccess.Instance.ReadHistoGoods(Good_Id))
                {
                    HistoGoodsView NewHistoGoodsView = new HistoGoodsView(HistoGood);
                    HistoGoodsFiltered.Add(NewHistoGoodsView);
                }
                return HistoGoodsFiltered;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private ObservableCollection<HistoGoodsView> _HistoGoods = null;

        public ObservableCollection<HistoGoodsView> HistoGoods
        {
            get
            {
                return _HistoGoods;
            }
        }

        private Dictionary<string, HistoGoodsView> _HistoGoodsInDictionary = null;

        public Dictionary<string, HistoGoodsView> HistoGoodsDict
        {
            get
            {
                return _HistoGoodsInDictionary;
            }
        }

        public string GetKeyHistoGoodView(HistoGoodsView HistoGoodsView)
        {
            return GetKeyHistoGoodView(HistoGoodsView.Histo_Id);
        }

        private string GetKeyHistoGoodView(HispaniaCompData.HistoGood HistoGood)
        {
            return GetKeyHistoGoodView(HistoGood.Histo_Id);
        }

        private string GetKeyHistoGoodView(int HistoGood_Id)
        {
            return string.Format("{0}", HistoGood_Id);
        }


        public void UpdateHistoGood(HistoGoodsView HistoGoodView)
        {
            UpdateHistoGoodInDb(HistoGoodView.GetHistoGood());
            RefreshHistoGoods();
        }
        public void DeleteHistoGood(HistoGoodsView HistoGoodView)
        {
            DeleteHistoGoodInDb(HistoGoodView.GetHistoGood());
            RefreshHistoGoods();
        }

        public HispaniaCompData.HistoGood GetHistoGood(int HistoGood_Id)
        {
            return GetHistoGoodInDb(HistoGood_Id);
        }

        #endregion

        #region DataBase [CRUD]

        private void CreateHistoGoodInDb(HispaniaCompData.HistoGood HistoGood)
        {
            HispaniaDataAccess.Instance.CreateHistoGood(HistoGood);
        }

        private List<HispaniaCompData.HistoGood> _HistoGoodsInDb;

        private List<HispaniaCompData.HistoGood> HistoGoodsInDb
        {
            get
            {
                return (this._HistoGoodsInDb);
            }
            set
            {
                this._HistoGoodsInDb = value;
            }
        }

        private void UpdateHistoGoodInDb(HispaniaCompData.HistoGood HistoGood)
        {
            HispaniaDataAccess.Instance.UpdateHistoGood(HistoGood);
        }

        private void DeleteHistoGoodInDb(HispaniaCompData.HistoGood HistoGood)
        {
            HispaniaDataAccess.Instance.DeleteHistoGood(HistoGood);
        }

        private HispaniaCompData.HistoGood GetHistoGoodInDb(int HistoGood_Id)
        {
            return HispaniaDataAccess.Instance.GetHistoGood(HistoGood_Id);
        }

        #endregion
    }
}
