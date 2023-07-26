#region Librerias usadas por la clase

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

        public void RefreshDiaryBandages(string Bill_Id_From, string Bill_Id_Until, decimal YearQuery)
        {
            try
            {
                DiaryBandagesInDb = HispaniaDataAccess.Instance.ReadDiaryBandages(Bill_Id_From, Bill_Id_Until, YearQuery);
                _DiaryBandagesInDictionary = new SortedDictionary<int, DiaryBandagesView>();
                foreach (HispaniaCompData.DiaryBandages_Result DiaryBandage in DiaryBandagesInDb)
                {
                    DiaryBandagesView NewDiaryBandagesView = new DiaryBandagesView(DiaryBandage);
                    _DiaryBandagesInDictionary.Add(NewDiaryBandagesView.Bill_Id, NewDiaryBandagesView);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private SortedDictionary<int, DiaryBandagesView> _DiaryBandagesInDictionary = null;

        public SortedDictionary<int, DiaryBandagesView> DiaryBandagesDict
        {
            get
            {
                return _DiaryBandagesInDictionary;
            }
        }

        public string GetKeyDiaryBandageView(DiaryBandagesView DiaryBandagesView)
        {
            return GetKeyDiaryBandageView(DiaryBandagesView.Bill_Id);
        }

        private string GetKeyDiaryBandageView(HispaniaCompData.DiaryBandages_Result DiaryBandage)
        {
            return GetKeyDiaryBandageView((int)DiaryBandage.Bill_Id);
        }

        private string GetKeyDiaryBandageView(int Bill_Id)
        {
            return string.Format("{0}", Bill_Id);
        }
        
        #endregion

        #region DataBase [CRUD]

        private List<HispaniaCompData.DiaryBandages_Result> ReadDiaryBandagesInDb(string Bill_Id_From, string Bill_Id_Until, decimal YearQuery)
        {
            return (HispaniaDataAccess.Instance.ReadDiaryBandages(Bill_Id_From, Bill_Id_Until, YearQuery));
        }

        private List<HispaniaCompData.DiaryBandages_Result> _DiaryBandagesInDb;

        private List<HispaniaCompData.DiaryBandages_Result> DiaryBandagesInDb
        {
            get
            {
                return (this._DiaryBandagesInDb);
            }
            set
            {
                this._DiaryBandagesInDb = value;
            }
        }

        #endregion
    }
}
