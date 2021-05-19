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

        public void CreateGood(GoodsView goodsView)
        {
            HispaniaCompData.Good goodToCreate = goodsView.GetGood();
            CreateGoodInDb(goodToCreate);
            goodsView.Good_Id = goodToCreate.Good_Id;
        }

        public void RefreshGoods()
        {
            try
            {
                GoodsInDb = HispaniaDataAccess.Instance.ReadGoods();
                _Goods = new ObservableCollection<GoodsView>();
                _GoodsInDictionary = new Dictionary<string, GoodsView>();
                _GoodsActiveInDictionary = new Dictionary<string, GoodsView>();
                foreach (HispaniaCompData.Good good in GoodsInDb)
                {
                    GoodsView NewGoodsView = new GoodsView(good);
                    _Goods.Add(NewGoodsView);
                    _GoodsInDictionary.Add(GetKeyGoodView(good), NewGoodsView);
                    if (!NewGoodsView.Canceled) _GoodsActiveInDictionary.Add(GetKeyGoodView(good), NewGoodsView);
                }
            }
            catch (Exception ex)
            {
                _Goods = null;
                throw ex;
            }
        }

        private ObservableCollection<GoodsView> _Goods = null;

        public ObservableCollection<GoodsView> Goods
        {
            get
            {
                return _Goods;
            }
        }

        private Dictionary<string, GoodsView> _GoodsActiveInDictionary = null;

        public Dictionary<string, GoodsView> GoodsActiveDict
        {
            get
            {
                return _GoodsActiveInDictionary;
            }
        }

        private Dictionary<string, GoodsView> _GoodsInDictionary = null;

        public Dictionary<string, GoodsView> GoodsDict
        {
            get
            {
                return _GoodsInDictionary;
            }
        }

        public string GetKeyGoodView(GoodsView goodsView)
        {
            return GetKeyGoodView(goodsView.Good_Code);
        }

        private string GetKeyGoodView(HispaniaCompData.Good good)
        {
            return GetKeyGoodView(good.Good_Code);
        }

        private string GetKeyGoodView(string Good_Code)
        {
            return string.Format("{0}", Good_Code);
        }

        public void UpdateGood(GoodsView goodView)
        {
            UpdateGoodInDb(goodView.GetGood());
        }

        public void UpdateGoodAcums(GoodsView goodView)
        {
            UpdateGoodAcumsInDb(goodView.GetGood());
            RefreshGoods();
        }

        public void DeleteGood(GoodsView goodView)
        {
            DeleteGoodInDb(goodView.GetGood());
        }

        public GoodsView GetGoodFromDb(GoodsView goodsView)
        {
            return new GoodsView(GetGoodInDb(goodsView.Good_Id));
        }

        public GoodsView GetGoodFromDb(RevisionsView revisionView)
        {
            return new GoodsView(GetGoodInDb(revisionView.GoodCode));
        }

        public HispaniaCompData.Good GetGood(int Good_Id)
        {
            return GetGoodInDb(Good_Id);
        }

        public ObservableCollection<GoodsView> GetGoods()
        {
            ObservableCollection<GoodsView> Goods_Readed = new ObservableCollection<GoodsView>();
            foreach (HispaniaCompData.Good good in HispaniaDataAccess.Instance.ReadGoods())
            {
                Goods_Readed.Add(new GoodsView(good));
            }
            return Goods_Readed;
        }

        #endregion

        #region DataBase [CRUD]

        private void CreateGoodInDb(HispaniaCompData.Good good)
        {
            HispaniaDataAccess.Instance.CreateGood(good);
        }

        private List<HispaniaCompData.Good> _GoodsInDb;

        private List<HispaniaCompData.Good> GoodsInDb
        {
            get
            {
                return (this._GoodsInDb);
            }
            set
            {
                this._GoodsInDb = value;
            }
        }

        private void UpdateGoodInDb(HispaniaCompData.Good good)
        {
            HispaniaDataAccess.Instance.UpdateGood(good);
        }

        private void UpdateGoodAcumsInDb(HispaniaCompData.Good good)
        {
            HispaniaDataAccess.Instance.UpdateGoodAcums(good);
        }

        private void DeleteGoodInDb(HispaniaCompData.Good good)
        {
            HispaniaDataAccess.Instance.DeleteGood(good);
        }

        private HispaniaCompData.Good GetGoodInDb(int Good_Id)
        {
            return HispaniaDataAccess.Instance.GetGood(Good_Id);
        }

        private HispaniaCompData.Good GetGoodInDb(string Good_Code)
        {
            return HispaniaDataAccess.Instance.GetGood(Good_Code);
        }

        #endregion
    }
}
