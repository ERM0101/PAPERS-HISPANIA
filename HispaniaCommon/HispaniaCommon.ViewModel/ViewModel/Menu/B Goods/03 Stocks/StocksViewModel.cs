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

        public void RefreshStocks()
        {
            try
            {
                StocksInDb = HispaniaDataAccess.Instance.ReadGoods();
                _Stocks = new ObservableCollection<StocksView>();
                _StocksInDictionary = new Dictionary<string, StocksView>();
                foreach (HispaniaCompData.Good good in StocksInDb)
                {
                    StocksView NewStocksView = new StocksView(good);
                    _Stocks.Add(NewStocksView);
                    _StocksInDictionary.Add(GetKeyStockView(good), NewStocksView);
                }

            }
            catch (Exception ex)
            {
                _Stocks = null;
                throw ex;
            }
        }

        private ObservableCollection<StocksView> _Stocks = null;

        public ObservableCollection<StocksView> Stocks
        {
            get
            {
                return _Stocks;
            }
        }

        private Dictionary<string, StocksView> _StocksInDictionary = null;

        public Dictionary<string, StocksView> StocksDict
        {
            get
            {
                return _StocksInDictionary;
            }
        }

        public string GetKeyStockView(StocksView stocksView)
        {
            return GetKeyStockView(stocksView.Good_Code);
        }

        private string GetKeyStockView(HispaniaCompData.Good good)
        {
            return GetKeyStockView(good.Good_Code);
        }

        private string GetKeyStockView(string Good_Code)
        {
            return string.Format("{0}", Good_Code);
        }

        public StocksView GetStockFromDb(StocksView stocksView)
        {
            return new StocksView(GetGoodInDb(stocksView.Good_Id));
        }

        public void UpdateGoodStocks(StocksView stocksView)
        {
            UpdateGoodStocksInDb(stocksView.GetGood());
            RefreshStocks();
        }
        
        #endregion

        #region DataBase [CRUD]

        private List<HispaniaCompData.Good> _StocksInDb;

        private List<HispaniaCompData.Good> StocksInDb
        {
            get
            {
                return (this._StocksInDb);
            }
            set
            {
                this._StocksInDb = value;
            }
        }

        private void UpdateGoodStocksInDb(HispaniaCompData.Good good)
        {
            HispaniaDataAccess.Instance.UpdateGoodStocks(good);
        }

        #endregion
    }
}
