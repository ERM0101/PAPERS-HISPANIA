#region Libraries used by the class

using HispaniaCommon.DataAccess;
using System;
using System.Collections.Generic;
using HispaniaCompData = HispaniaComptabilitat.Data;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Last update Data: 04/01/2018
    /// Descriptión: class that implements Common ViewModel of the Applications.
    /// </summary>
    public partial class MaintenanceForViewModel
    {
        #region ViewModel

        public void RefreshSettlements(int? Agent_Id, string Bill_Id_From, string Bill_Id_Until, decimal YearQuery)
        {
            try
            {
                Dictionary<int, List<SettlementsView>> BillItem;
                SettlementsInDb = HispaniaDataAccess.Instance.ReadSettlements(Agent_Id, Bill_Id_From, Bill_Id_Until, YearQuery);
                _SettlementsInDictionary = new SortedDictionary<int, Dictionary<int, List<SettlementsView>>>();
                foreach (HispaniaCompData.Settlement Settlement in SettlementsInDb)
                {
                    SettlementsView NewSettlementsView = new SettlementsView(Settlement);
                    int Agent_Key = NewSettlementsView.Agent_Id;
                    int Bill_Key = NewSettlementsView.Bill_Id;
                    if (!_SettlementsInDictionary.ContainsKey(Agent_Key))
                    {
                        BillItem = new Dictionary<int, List<SettlementsView>>
                        {
                            { Bill_Key, new List<SettlementsView>() }
                        };
                        _SettlementsInDictionary.Add(NewSettlementsView.Agent_Id, BillItem);
                    }
                    if (!_SettlementsInDictionary[Agent_Key].ContainsKey(Bill_Key))
                    {
                        _SettlementsInDictionary[Agent_Key].Add(Bill_Key, new List<SettlementsView>());
                    }
                    _SettlementsInDictionary[Agent_Key][Bill_Key].Add(NewSettlementsView);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private SortedDictionary<int, Dictionary<int, List<SettlementsView>>> _SettlementsInDictionary = null;

        public SortedDictionary<int, Dictionary<int, List<SettlementsView>>> SettlementsDict
        {
            get
            {
                return _SettlementsInDictionary;
            }
        }

        public string GetKeySettlementView(SettlementsView SettlementsView)
        {
            return GetKeySettlementView(SettlementsView.Settlement_Id);
        }

        private string GetKeySettlementView(HispaniaCompData.Settlement Settlement)
        {
            return GetKeySettlementView((int)Settlement.Settlement_Id);
        }

        private string GetKeySettlementView(int Settlement_Id)
        {
            return string.Format("{0}", Settlement_Id);
        }
        
        #endregion

        #region DataBase [CRUD]

        private List<HispaniaCompData.Settlement> ReadSettlementsInDb(int? Agent_Id, string Bill_Id_From, string Bill_Id_Until, decimal YearQuery)
        {
            return (HispaniaDataAccess.Instance.ReadSettlements(Agent_Id, Bill_Id_From, Bill_Id_Until, YearQuery));
        }

        private List<HispaniaCompData.Settlement> _SettlementsInDb;

        private List<HispaniaCompData.Settlement> SettlementsInDb
        {
            get
            {
                return (this._SettlementsInDb);
            }
            set
            {
                this._SettlementsInDb = value;
            }
        }

        #endregion
    }
}

