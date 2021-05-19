#region Libraries used by the class

using HispaniaCommon.DataAccess;
using System;
using System.Collections.Generic;
using HispaniaCompData = HispaniaComptabilitat.Data;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Last update Data: 19/09/2016
    /// Descriptión: class that implements Common ViewModel of the Applications.
    /// </summary>
    public partial class MaintenanceForViewModel
    {
        #region ViewModel

        private const int LOCAL_CONNECTION_UNDEFINED = 0;

        private int Local_Connection
        {
            get
            {
                List<HispaniaCompData.LocalConnection> localConnectionInBD = ReadLocalConnectionInDb(Local_MachineName);
                if (localConnectionInBD.Count == 1) return (localConnectionInBD[0].LocalConnection_Id);
                else return (LOCAL_CONNECTION_UNDEFINED);
            }
        }

        /// <summary>
        /// This function validate if the machine can execute the application.
        /// </summary>
        public void ValidateMachineToExecuteApplication()
        {
            List<HispaniaCompData.LocalConnection> localConnectionInBD = ReadLocalConnectionInDb(Local_MachineName);
            if (localConnectionInBD.Count == 0)
            {
                throw new ApplicationException(
                               string.Format("Error, la màquina, '{0}', no està habilitada per executar l'aplicació.",
                                             Local_MachineName));
            }
            else if (localConnectionInBD.Count > 1)
            {
                throw new ApplicationException(
                               string.Format("Error, la màquina '{0}' te més d'una connexió associada.\r\nContacteu amb l'administrador.",
                                             Local_MachineName));
            }
        }

        public HispaniaCompData.LocalConnection GetLocalConnection(int LocalConnection_Id)
        {
            return (HispaniaDataAccess.Instance.ReadLocalConnection(LocalConnection_Id));
        }

        #endregion

        #region DataBase [CRUD]

        private HispaniaCompData.LocalConnection ReadLocalConnectionInDb(int LocalConnection_Id)
        {
            return (HispaniaDataAccess.Instance.ReadLocalConnection(LocalConnection_Id));
        }

        private List<HispaniaCompData.LocalConnection> ReadLocalConnectionInDb(string MachineName)
        {
            return (HispaniaDataAccess.Instance.ReadLocalConnection(MachineName));
        }

        #endregion
    }
}
