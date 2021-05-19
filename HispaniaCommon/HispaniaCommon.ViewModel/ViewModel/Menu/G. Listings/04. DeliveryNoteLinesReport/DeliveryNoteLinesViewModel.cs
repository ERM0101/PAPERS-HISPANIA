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

        public List<HispaniaCompData.DeliveryNoteLine> ReadInfoForDeliveryNoteLines()
        {
            return ReadDeliveryNoteLinesInDb();
        }
        
        #endregion

        #region DataBase [CRUD]

        private List<HispaniaCompData.DeliveryNoteLine> ReadDeliveryNoteLinesInDb()
        {
            return (HispaniaDataAccess.Instance.ReadDeliveryNoteLines());
        }

        private List<HispaniaCompData.DeliveryNoteLine> _DeliveryNoteLinesInDb;

        private List<HispaniaCompData.DeliveryNoteLine> DeliveryNoteLinesInDb
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
