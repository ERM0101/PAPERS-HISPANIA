#region Libraries used by the class

using HispaniaCommon.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HispaniaCompData = HispaniaComptabilitat.Data;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Tipus de revisions que ha de cercar a la Base de Dades
    /// </summary>
    public enum RevisionsType
    {
        MismatchesAvailable = 1,
        MismatchesStocks = 2,
        GeneralAvailable = 3,
        GeneralStocks = 4,
    }

    /// <summary>
    /// Last update Data: 04/01/2018
    /// Descriptión: class that implements Common ViewModel of the Applications.
    /// </summary>
    public partial class MaintenanceForViewModel
    {
        #region ViewModel

        public void RefreshRevisions(RevisionsType FilterOption)
        {
            try
            {
                RevisionsInDb = HispaniaDataAccess.Instance.ReadRevisions((int) FilterOption);
                _Revisions = new ObservableCollection<RevisionsView>();
                _RevisionsInDictionary = new Dictionary<string, RevisionsView>();
                foreach (HispaniaCompData.Revisions_Result Revisio in RevisionsInDb)
                {
                    RevisionsView NewRevisionsView = new RevisionsView(Revisio);
                    _Revisions.Add(NewRevisionsView);
                    _RevisionsInDictionary.Add(GetKeyRevisionView(Revisio), NewRevisionsView);
                }
            }
            catch (Exception ex)
            {
                _Revisions = null;
                throw ex;
            }
        }

        private ObservableCollection<RevisionsView> _Revisions = null;

        public ObservableCollection<RevisionsView> Revisions
        {
            get
            {
                return _Revisions;
            }
        }

        private Dictionary<string, RevisionsView> _RevisionsInDictionary = null;

        public Dictionary<string, RevisionsView> RevisionsDict
        {
            get
            {
                return _RevisionsInDictionary;
            }
        }

        public string GetKeyRevisionView(RevisionsView RevisionsView)
        {
            return GetKeyRevisionView(RevisionsView.GoodCode);
        }

        private string GetKeyRevisionView(HispaniaCompData.Revisions_Result Revisio)
        {
            return GetKeyRevisionView( Revisio.GoodCode );
        }

        private string GetKeyRevisionView(string GoodCode)
        {
            return string.Format("{0}", GoodCode);
        }
        
        #endregion

        #region DataBase [CRUD]

        private List<HispaniaCompData.Revisions_Result> ReadRevisionsInDb(int FilterOption)
        {
            return (HispaniaDataAccess.Instance.ReadRevisions(FilterOption));
        }

        private List<HispaniaCompData.Revisions_Result> _RevisionsInDb;

        private List<HispaniaCompData.Revisions_Result> RevisionsInDb
        {
            get
            {
                return (this._RevisionsInDb);
            }
            set
            {
                this._RevisionsInDb = value;
            }
        }

        #endregion
    }
}
