using HispaniaCommon.DataAccess;
using System;
using System.Collections.Generic;
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

        public void RefreshRanges(string Good_Code_From, string Good_Code_Until, string Bill_Id_From, 
                                  string Bill_Id_Until, decimal YearQuery)
        {
            try
            {
                RangesInDb = HispaniaDataAccess.Instance.ReadRanges(Good_Code_From, Good_Code_Until, Bill_Id_From, Bill_Id_Until, YearQuery);
                _RangesInDictionary = new Dictionary<string, List<RangesView>>();
                foreach (HispaniaCompData.Range Range in RangesInDb)
                {
                    RangesView NewRangesView = new RangesView(Range);
                    if (!_RangesInDictionary.ContainsKey(NewRangesView.Familia))
                    {
                        _RangesInDictionary.Add(NewRangesView.Familia, new List<RangesView>());
                    }
                    _RangesInDictionary[NewRangesView.Familia].Add(NewRangesView);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Dictionary<string, List<RangesView>> _RangesInDictionary = null;

        public Dictionary<string, List<RangesView>> RangesDict
        {
            get
            {
                return _RangesInDictionary;
            }
        }

        public string GetKeyRangeView(RangesView RangesView)
        {
            return GetKeyRangeView(RangesView.Good_Code);
        }

        private string GetKeyRangeView(HispaniaCompData.Range Range)
        {
            return GetKeyRangeView(Range.Good_Code);
        }

        private string GetKeyRangeView(string GoodCode)
        {
            return string.Format("{0}", GoodCode);
        }
        
        #endregion

        #region DataBase [CRUD]

        private List<HispaniaCompData.Range> ReadRangesInDb(string Good_Code_From, string Good_Code_Until,
                                                            string Bill_Id_From, string Bill_Id_Until, decimal YearQuery)
        {
            return (HispaniaDataAccess.Instance.ReadRanges(Good_Code_From, Good_Code_Until, Bill_Id_From, Bill_Id_Until, YearQuery));
        }

        private List<HispaniaCompData.Range> _RangesInDb;

        private List<HispaniaCompData.Range> RangesInDb
        {
            get
            {
                return (this._RangesInDb);
            }
            set
            {
                this._RangesInDb = value;
            }
        }

        #endregion
    }
}
