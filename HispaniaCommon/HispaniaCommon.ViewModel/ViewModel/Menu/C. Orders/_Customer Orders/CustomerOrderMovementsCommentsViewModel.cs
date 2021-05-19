#region Librerias usadas por la clas

using HispaniaCommon.DataAccess;
using System.Collections.Generic;
using System.Text;
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

        public string CustomerOrderMovementsComments(int CustomerOrder_Id)
        {
            StringBuilder sbComment = new StringBuilder(string.Empty);
            foreach (HispaniaCompData.CustomerOrderMovementsComment comment in CustomerOrderMovementsCommentsInDb(CustomerOrder_Id))
            {
                sbComment.AppendLine(comment.Comentari.Trim());
            }
            if (sbComment.Length > 0) return sbComment.ToString().Substring(0, sbComment.Length - 2);
            else return sbComment.ToString() == string.Empty ? null : sbComment.ToString();
        }

        #endregion

        #region DataBase [CRUD]

        private List<HispaniaCompData.CustomerOrderMovementsComment> CustomerOrderMovementsCommentsInDb(int CustomerOrder_Id)
        {
            return (HispaniaDataAccess.Instance.CustomerOrderMovementsComments(CustomerOrder_Id));
        }

        #endregion
    }
}
