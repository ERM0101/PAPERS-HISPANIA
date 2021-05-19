using HispaniaCommon.DataAccess;

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Last update Data: 04/01/2018
    /// Descriptión: class that implements Common ViewModel of the Applications.
    /// </summary>
    public partial class MaintenanceForViewModel
    {
        #region ViewModel

        public bool HistoricAcumCustomers(int Year, out string ErrMsg)
        {
           return HistoricAcumCustomersInDb(Year, out ErrMsg);
        }
        
        #endregion

        #region DataBase [CRUD]

        private bool HistoricAcumCustomersInDb(int Year, out string ErrMsg)
        {
            return (HispaniaDataAccess.Instance.HistoricAcumCustomers(Year, out ErrMsg));
        }

        #endregion
    }
}
