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

        public bool? LiniesProveidorConformes(int ProviderOrder_Id)
        {
            return (LiniesProveidorConformesInDb(ProviderOrder_Id));
        }

        #endregion

        #region DataBase [CRUD]

        private bool? LiniesProveidorConformesInDb(int ProviderOrder_Id)
        {
            return (HispaniaDataAccess.Instance.LiniesProveidorConformes(ProviderOrder_Id));
        }

        #endregion
    }
}
