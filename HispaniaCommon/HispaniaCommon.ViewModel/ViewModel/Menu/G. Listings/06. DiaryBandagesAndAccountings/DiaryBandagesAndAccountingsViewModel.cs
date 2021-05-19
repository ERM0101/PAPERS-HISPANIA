#region Libraries used in this class

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

        public void RefreshDiaryBandagesAndAccountings(string Bill_Id_From, string Bill_Id_Until, decimal YearQuery)
        {
            try
            {
                DiaryBandagesAndAccountingsInDb = HispaniaDataAccess.Instance.ReadDiaryBandages(Bill_Id_From, Bill_Id_Until, YearQuery);
                _DiaryBandagesAndAccountingsInDictionary = new SortedDictionary<int, DiaryBandagesAndAccountingsView>();
                foreach (HispaniaCompData.DiaryBandage DiaryBandagesAndAccounting in DiaryBandagesAndAccountingsInDb)
                {
                    DiaryBandagesAndAccountingsView NewDiaryBandagesAndAccountingsView = new DiaryBandagesAndAccountingsView(DiaryBandagesAndAccounting);
                    _DiaryBandagesAndAccountingsInDictionary.Add(NewDiaryBandagesAndAccountingsView.Bill_Id, NewDiaryBandagesAndAccountingsView);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private SortedDictionary<int, DiaryBandagesAndAccountingsView> _DiaryBandagesAndAccountingsInDictionary = null;

        public SortedDictionary<int, DiaryBandagesAndAccountingsView> DiaryBandagesAndAccountingsDict
        {
            get
            {
                return _DiaryBandagesAndAccountingsInDictionary;
            }
        }

        public string GetKeyDiaryBandagesAndAccountingView(DiaryBandagesAndAccountingsView DiaryBandagesAndAccountingsView)
        {
            return GetKeyDiaryBandageView(DiaryBandagesAndAccountingsView.Bill_Id);
        }

        private string GetKeyDiaryBandagesAndAccountingView(HispaniaCompData.DiaryBandage DiaryBandage)
        {
            return GetKeyDiaryBandagesAndAccountingView((int)DiaryBandage.Bill_Id);
        }

        private string GetKeyDiaryBandagesAndAccountingView(int Bill_Id)
        {
            return string.Format("{0}", Bill_Id);
        }
        
        #endregion

        #region DataBase [CRUD]

        private List<HispaniaCompData.DiaryBandage> ReadDiaryBandagesAndAccountingsInDb(string Bill_Id_From, string Bill_Id_Until, decimal YearQuery)
        {
            return (HispaniaDataAccess.Instance.ReadDiaryBandages(Bill_Id_From, Bill_Id_Until, YearQuery));
        }

        private List<HispaniaCompData.DiaryBandage> _DiaryBandagesAndAccountingsInDb;

        private List<HispaniaCompData.DiaryBandage> DiaryBandagesAndAccountingsInDb
        {
            get
            {
                return (this._DiaryBandagesAndAccountingsInDb);
            }
            set
            {
                this._DiaryBandagesAndAccountingsInDb = value;
            }
        }

        #endregion
    }
}
