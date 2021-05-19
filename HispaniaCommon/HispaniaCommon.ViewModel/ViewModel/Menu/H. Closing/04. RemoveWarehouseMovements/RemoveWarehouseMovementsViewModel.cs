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

        public bool RemoveWarehouseMovements(out string ErrMsg)
        {
           return RemoveWarehouseMovementsInDb(out ErrMsg);
        }
        
        #endregion

        #region DataBase [CRUD]

        private bool RemoveWarehouseMovementsInDb(out string ErrMsg)
        {
            return (HispaniaDataAccess.Instance.RemoveWarehouseMovements(out ErrMsg));
        }

        #endregion
    }
}
