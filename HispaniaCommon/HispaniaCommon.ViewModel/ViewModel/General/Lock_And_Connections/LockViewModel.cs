using HispaniaCommon.DataAccess;
using MBCode.Framework.Managers.Messages;
using System;
using System.Collections.Generic;
using HispaniaCompData = HispaniaComptabilitat.Data;

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Last update Data: 19/09/2016
    /// Descriptión: class that implements Common ViewModel of the Applications.
    /// </summary>
    public partial class MaintenanceForViewModel
    {
        #region ViewModel

        internal string Local_MachineName
        {
            get
            {
                return (Environment.MachineName.ToUpper());
            }
        }

        /// <summary>
        /// This function allow the program to control that two users don't edit at same time one of registers of the 
        /// table.
        /// </summary>
        /// <param name="ObjectNameToLock">Object that representents the register to lock</param>
        /// <param name="ErrMsg">Mensaje de error, si se produce uno.</param>
        public bool LockRegister(string ObjectNameToLock, out string ErrMsg)
        {
            return LockRegister(ObjectNameToLock, "None", out ErrMsg);
        }

        /// <summary>
        /// This function allow the program to control that two users don't edit at same time one of registers of the 
        /// table.
        /// </summary>
        /// <param name="RegisterToLock">Object that representents the register to lock</param>
        /// <param name="ErrMsg">Mensaje de error, si se produce uno.</param>
        public bool LockRegister(object RegisterToLock, out string ErrMsg)
        {
            ErrMsg = string.Empty;
            try
            {
                string TableName = RegisterToLock.GetType().Name.Replace("sView", string.Empty);
                string IdRegister = ((IMenuView)RegisterToLock).GetKey;
                return LockRegister(TableName, IdRegister, out ErrMsg);
            }
            catch (Exception ex)
            {
                ErrMsg = MsgManager.ExcepMsg(ex);
            }
            return (ErrMsg == String.Empty);
        }

        /// <summary>
        /// This function allow the program to control that two users don't edit at same time one of registers of the 
        /// table.
        /// </summary>
        /// <param name="RegisterToLock">Object that representents the register to lock</param>
        /// <param name="ErrMsg">Mensaje de error, si se produce uno.</param>
        private bool LockRegister(string TableName, string IdRegister, out string ErrMsg)
        {
            ErrMsg = string.Empty;
            try
            {
                //#if DEBUG
                //MsgManager.ShowMessage(string.Format("Table Name: {0}, Id_Register: {1}", TableName, IdRegister));
                //#endif
                List<HispaniaCompData.Lock> locksInBD = ReadLockIdDb(TableName, IdRegister);
                if (locksInBD.Count == 0)
                {
                    HispaniaCompData.Lock lockToCreate = new HispaniaCompData.Lock()
                    {
                        TableName = TableName,
                        IdRegister = IdRegister,
                        LocalConnection_Id = Local_Connection
                    };
                    CreateLockInDb(lockToCreate);
                }
                else
                {
                    string MachinesLock = string.Empty;
                    foreach (HispaniaCompData.Lock lockInDB in locksInBD)
                    {
                        if (lockInDB.LocalConnection_Id != Local_Connection)
                        {
                            MachinesLock += string.Format("{0},", GetLocalConnection(lockInDB.LocalConnection_Id).MachineName.ToUpper());
                        }
                    }
                    if (!string.IsNullOrEmpty(MachinesLock))
                    {
                        MachinesLock.Remove(MachinesLock.Length - 1, 1);
                        ErrMsg = string.Format("Error, hi ha usuaris editant aquest registre: '{0}'.", MachinesLock);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrMsg = MsgManager.ExcepMsg(ex);
            }
            return (ErrMsg == String.Empty);
        }

        /// <summary>
        /// This function allow the program to leave the control of one of registers of the table.
        /// </summary>
        /// <param name="RegisterToUnlock">Object that representents the register to unlock</param>
        /// <param name="ErrMsg">Mensaje de error, si se produce uno.</param>
        public bool UnlockRegister(string ObjectNameToLock, out string ErrMsg)
        {
            return UnlockRegister(ObjectNameToLock, "None", out ErrMsg);
        }

        /// <summary>
        /// This function allow the program to leave the control of one of registers of the table.
        /// </summary>
        /// <param name="RegisterToUnlock">Object that representents the register to unlock</param>
        /// <param name="ErrMsg">Mensaje de error, si se produce uno.</param>
        public bool UnlockRegister(object RegisterToUnlock, out string ErrMsg)
        {
            ErrMsg = string.Empty;
            try
            {
                string TableName = RegisterToUnlock.GetType().Name.Replace("sView", string.Empty);
                string IdRegister = ((IMenuView)RegisterToUnlock).GetKey;
                return UnlockRegister(TableName, IdRegister, out ErrMsg);
            }
            catch (Exception ex)
            {
                ErrMsg = MsgManager.ExcepMsg(ex);
            }
            return (ErrMsg == String.Empty);
        }

        /// <summary>
        /// This function allow the program to leave the control of one of registers of the table.
        /// </summary>
        /// <param name="RegisterToUnlock">Object that representents the register to unlock</param>
        /// <param name="ErrMsg">Mensaje de error, si se produce uno.</param>
        public bool UnlockRegister(string TableName, string IdRegister, out string ErrMsg)
        {
            ErrMsg = string.Empty;
            try
            {
                //#if DEBUG
                //MsgManager.ShowMessage(string.Format("Table Name: {0}, Id_Register: {1}", TableName, IdRegister));
                //#endif
                List<HispaniaCompData.Lock> locksInBD = ReadLockIdDb(TableName, IdRegister);
                if (locksInBD.Count > 0)
                {
                    foreach (HispaniaCompData.Lock lockInDB in locksInBD)
                    {
                        if (lockInDB.LocalConnection_Id == Local_Connection) DeleteLockInDb(TableName, IdRegister);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrMsg = MsgManager.ExcepMsg(ex);
            }
            return (ErrMsg == String.Empty);
        }

        /// <summary>
        /// This function allow the program to leave all locks realized in the machine.
        /// </summary>
        public void ClearLocks()
        {
            int Local_Connection_To_Unlock = Local_Connection;
            if (Local_Connection_To_Unlock != LOCAL_CONNECTION_UNDEFINED) DeleteLockInDb(Local_Connection_To_Unlock);
        }

        #endregion

        #region DataBase [CRUD]

        private void CreateLockInDb(HispaniaCompData.Lock lockToCreate)
        {
            HispaniaDataAccess.Instance.CreateLock(lockToCreate);
        }

        private List<HispaniaCompData.Lock> ReadLockIdDb(string TableName, string IdRegister)
        {
            return (HispaniaDataAccess.Instance.ReadLock(TableName, IdRegister));
        }

        private void DeleteLockInDb(string TableName, string IdRegister)
        {
            HispaniaDataAccess.Instance.DeleteLock(TableName, IdRegister);
        }

        private void DeleteLockInDb(int LocalConnection_Id)
        {
            HispaniaDataAccess.Instance.DeleteLock(LocalConnection_Id);
        }

        #endregion
    }
}
