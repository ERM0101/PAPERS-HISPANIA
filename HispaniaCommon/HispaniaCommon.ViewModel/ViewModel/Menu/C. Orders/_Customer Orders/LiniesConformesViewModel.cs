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

        public bool? LiniesConformes(int CustomerOrder_Id)
        {
            return (LiniesConformesInDb(CustomerOrder_Id));
        }

        #endregion

        #region DataBase [CRUD]

        private bool? LiniesConformesInDb(int CustomerOrder_Id)
        {
            return (HispaniaDataAccess.Instance.LiniesConformes(CustomerOrder_Id));
        }

        #endregion
    }
}
