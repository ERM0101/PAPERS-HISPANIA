#region Librerias usadas por la clase

using HispaniaCommon.DataAccess;
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

        public List<HispaniaCompData.DeliveryNoteLines_Result> ReadInfoForDeliveryNoteLines()
        {
            return ReadDeliveryNoteLinesInDb();
        }
        
        #endregion

        #region DataBase [CRUD]

        private List<HispaniaCompData.DeliveryNoteLines_Result> ReadDeliveryNoteLinesInDb()
        {
            return (HispaniaDataAccess.Instance.ReadDeliveryNoteLines());
        }

        private List<HispaniaCompData.DeliveryNoteLines_Result> _DeliveryNoteLinesInDb;

        private List<HispaniaCompData.DeliveryNoteLines_Result> DeliveryNoteLinesInDb
        {
            get
            {
                return (this._DeliveryNoteLinesInDb);
            }
            set
            {
                this._DeliveryNoteLinesInDb = value;
            }
        }

        #endregion
    }
}
