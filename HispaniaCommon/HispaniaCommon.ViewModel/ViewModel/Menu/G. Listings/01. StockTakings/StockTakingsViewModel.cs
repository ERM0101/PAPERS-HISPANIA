using HispaniaCommon.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HispaniaCompData = HispaniaComptabilitat.Data;

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Last update Data: 04/01/2018
    /// Descriptión: class that implements Common ViewModel of the Applications.
    /// </summary>
    public partial class MaintenanceForViewModel
    {
        #region ViewModel

        public void RefreshStockTakings(string Good_Code_From, string Good_Code_Until)
        {
            try
            {
                StockTakingsInDb = HispaniaDataAccess.Instance.ReadStockTakings(Good_Code_From, Good_Code_Until);
                _StockTakingsInDictionary = new Dictionary<string, List<StockTakingsView>>();
                foreach (HispaniaCompData.StockTaking StockTaking in StockTakingsInDb)
                {
                    StockTakingsView NewStockTakingsView = new StockTakingsView(StockTaking);
                    if (!_StockTakingsInDictionary.ContainsKey(NewStockTakingsView.Familia))
                    {
                        _StockTakingsInDictionary.Add(NewStockTakingsView.Familia, new List<StockTakingsView>());
                    }
                    _StockTakingsInDictionary[NewStockTakingsView.Familia].Add(NewStockTakingsView);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Dictionary<string, List<StockTakingsView>> _StockTakingsInDictionary = null;

        public Dictionary<string, List<StockTakingsView>> StockTakingsDict
        {
            get
            {
                return _StockTakingsInDictionary;
            }
        }

        public string GetKeyStockTakingView(StockTakingsView StockTakingsView)
        {
            return GetKeyStockTakingView(StockTakingsView.GoodCode);
        }

        private string GetKeyStockTakingView(HispaniaCompData.StockTaking StockTaking)
        {
            return GetKeyStockTakingView(StockTaking.Good_Code);
        }

        private string GetKeyStockTakingView(string GoodCode)
        {
            return string.Format("{0}", GoodCode);
        }
        
        #endregion

        #region DataBase [CRUD]

        private List<HispaniaCompData.StockTaking> ReadStockTakingsInDb(string Good_Code_From, string Good_Code_Until)
        {
            return (HispaniaDataAccess.Instance.ReadStockTakings(Good_Code_From, Good_Code_Until));
        }

        private List<HispaniaCompData.StockTaking> _StockTakingsInDb;

        private List<HispaniaCompData.StockTaking> StockTakingsInDb
        {
            get
            {
                return (this._StockTakingsInDb);
            }
            set
            {
                this._StockTakingsInDb = value;
            }
        }

        #endregion
    }
}
