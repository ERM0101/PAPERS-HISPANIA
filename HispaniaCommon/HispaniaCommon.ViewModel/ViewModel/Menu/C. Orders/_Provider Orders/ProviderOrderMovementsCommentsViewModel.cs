﻿#region Librerias usadas por la clas

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

        public string ProviderOrderMovementsComments(int ProviderOrder_Id)
        {
            StringBuilder sbComment = new StringBuilder(string.Empty);
            foreach( string comment in ProviderOrderMovementsCommentsInDb(ProviderOrder_Id))
            {
                sbComment.AppendLine( comment.Trim() );
            }
            if (sbComment.Length > 0) return sbComment.ToString().Substring(0, sbComment.Length - 2);
            else return sbComment.ToString() == string.Empty ? null : sbComment.ToString();
        }

        #endregion

        #region DataBase [CRUD]

        private List<string> ProviderOrderMovementsCommentsInDb(int ProviderOrder_Id)
        {
            return (HispaniaDataAccess.Instance.ProviderOrderMovementsComments(ProviderOrder_Id));
        }

        #endregion
    }
}
