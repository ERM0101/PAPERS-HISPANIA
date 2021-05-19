#region Librerias usadas por la clase

using HispaniaCommon.DataAccess;

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

        public bool HistoricAcumGoods(int Year, out string ErrMsg)
        {
           return HistoricAcumGoodsInDb(Year, out ErrMsg);
        }
        
        #endregion

        #region DataBase [CRUD]

        private bool HistoricAcumGoodsInDb(int Year, out string ErrMsg)
        {
            return (HispaniaDataAccess.Instance.HistoricAcumGoods(Year, out ErrMsg));
        }

        #endregion
    }
}
