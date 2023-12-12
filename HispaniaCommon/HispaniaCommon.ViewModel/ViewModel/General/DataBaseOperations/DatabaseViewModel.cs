#region Librerias usadas por la clase

using HispaniaCommon.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Last update Data: 19/09/2016
    /// Descriptión: class that implements Common ViewModel of the Applications.
    /// </summary>
    public partial class MaintenanceForViewModel
    {
        #region Attributes

        /// <summary>
        /// Stores the Data Managed information
        /// </summary>
        private Dictionary<Guid, Dictionary<object, DataBaseOp>> m_DataManaged = new Dictionary<Guid, Dictionary<object, DataBaseOp>>();

        #endregion

        #region Properties

        private Dictionary<Guid, Dictionary<object, DataBaseOp>> DataManaged
        {
            get
            {
                return (m_DataManaged);
            }
            set
            {
                if (value != null) m_DataManaged = value;
                else m_DataManaged = new Dictionary<Guid, Dictionary<object, DataBaseOp>>();
            }
        }

        #endregion

        #region Virtual Data Set

        public Guid InitializeDataManaged(ObservableCollection<CustomerOrderMovementsView> InitialCollection)
        {
            Dictionary<object, DataBaseOp> Data = new Dictionary<object, DataBaseOp>();
            if (InitialCollection != null)
            {
                foreach (object item in InitialCollection)
                {
                    Data.Add(item, DataBaseOp.READ);
                }
            }
            Guid New_Data_Id = Guid.NewGuid();
            DataManaged.Add(New_Data_Id, Data);
            return (New_Data_Id);
        }


        public Guid InitializeDataManaged(ObservableCollection<ProviderOrderMovementsView> InitialCollection)
        {
            Dictionary<object, DataBaseOp> Data = new Dictionary<object, DataBaseOp>();
            if (InitialCollection != null)
            {
                foreach (object item in InitialCollection)
                {
                    Data.Add(item, DataBaseOp.READ);
                }
            }
            Guid New_Data_Id = Guid.NewGuid();
            DataManaged.Add(New_Data_Id, Data);
            return (New_Data_Id);
        }

        public void CreateItemInDataManaged(Guid CollectionId, object ItemData)
        {
            if (!DataManaged.ContainsKey(CollectionId))
            {
                throw new ArgumentException("Error, a l'insertar un nou element. Colecció no trobada.\r\n" +
                                            "Detalls: mètode 'CreateItem'.");
            }
            DataManaged[CollectionId].Add(ItemData, DataBaseOp.CREATE);
        }

        public void UpdateItemInDataManaged(Guid CollectionId, object OldItemData, object NewItemData)
        {
            if (!DataManaged.ContainsKey(CollectionId))
            {
                throw new ArgumentException("Error, a l'editar un element. Colecció no trobada.\r\n" +
                                            "Detalls: mètode 'UpdateItemInDataManaged'.");
            }
            if (!DataManaged[CollectionId].ContainsKey(OldItemData))
            {
                throw new ArgumentException("Error, a l'editar un element. Element no trobat.\r\n" +
                                            "Detalls: mètode 'UpdateItemInDataManaged'.");
            }
            switch (DataManaged[CollectionId][OldItemData])
            {
                case DataBaseOp.CREATE:
                     DataManaged[CollectionId].Remove(OldItemData);
                     if (DataManaged[CollectionId].ContainsKey(NewItemData))
                     {
                         DataManaged[CollectionId][NewItemData] = DataBaseOp.READ;
                     }
                     else
                     {
                         DataManaged[CollectionId].Add(NewItemData, DataBaseOp.CREATE);
                     }
                     break;
                case DataBaseOp.READ:
                     DataManaged[CollectionId][OldItemData] = DataBaseOp.DELETE;
                     DataManaged[CollectionId].Add(NewItemData, DataBaseOp.CREATE);
                     break;
                case DataBaseOp.UPDATE: // This case is impossible because this Tag is not used in this class.
                case DataBaseOp.DELETE: // This case is impossible because a deleted item don't recive another action.
                     throw new ArgumentException("Error, a l'editar un element que s'havia esborrat.\r\n" +
                                                 "Detalls: mètode 'UpdateItemInDataManaged'.");
            }
        }

        public void DeleteItemInDataManaged(Guid CollectionId, object ItemData)
        {
            if (!DataManaged.ContainsKey(CollectionId))
            {
                throw new ArgumentException("Error, a l'esborrar un element. Colecció no trobada.\r\n" +
                                            "Detalls: mètode 'UpdateItemInDataManaged'.");
            }
            if (!DataManaged[CollectionId].ContainsKey(ItemData))
            {
                throw new ArgumentException("Error, a l'esborrar un element. Element no trobat.\r\n" +
                                            "Detalls: mètode 'UpdateItemInDataManaged'.");
            }
            switch (DataManaged[CollectionId][ItemData])
            {
                case DataBaseOp.CREATE: // Item is removed because it is not yet created
                     DataManaged[CollectionId].Remove(ItemData);
                     break;
                case DataBaseOp.READ:
                     DataManaged[CollectionId][ItemData] = DataBaseOp.DELETE;
                     break;
                case DataBaseOp.UPDATE: // This case is impossible because this Tag is not used in this class.
                case DataBaseOp.DELETE: // This case is impossible because a deleted item don't recive another action.
                     throw new ArgumentException("Error, a l'esborrat un element que s'havia esborrat.\r\n" +
                                                 "Detalls: mètode 'UpdateItemInDataManaged'.");
            }
        }

        internal Dictionary<object, DataBaseOp> GetItemsInDataManaged(Guid CollectionId)
        {
            if (!DataManaged.ContainsKey(CollectionId))
            {
                throw new ArgumentException("Error, a l'accedir a la colecció d'elements.\r\n" +
                                            "Detalls: mètode 'GetItemsInDataManaged'.");
            }
            return (new Dictionary<object, DataBaseOp>(DataManaged[CollectionId]));
        }

        #endregion

        #region ViewModel

        /// <summary>
        /// Get the next identity field value of the table passed by parameter
        /// </summary>
        /// <param name="Register">Define the type of object which be applied the operation</param>
        /// <returns>The new Identity Number of the table if the operation ends properly</returns>
        public int GetNextIdentityValueTable(object Register)
        {
            SystemTables SystemTable = (SystemTables)Enum.Parse(typeof(SystemTables), Register.GetType().Name.Replace("sView", string.Empty));
            //#if DEBUG
            //MsgManager.ShowMessage(string.Format("Table Name: '{0}'", SystemTable.ToString("g")));
            //#endif
            return (GetNextIdentityValueTableInDb(SystemTable));
        }

        /// <summary>
        /// Reset the identity field value of the table passed by parameter at 1.
        /// </summary>
        /// <param name="Register">Define the type of object which be applied the operation</param>
        /// <returns>1 if the operation ends properly</returns>
        public void ResetIdentityValueTable(object Register)
        {
            SystemTables SystemTable = (SystemTables)Enum.Parse(typeof(SystemTables), Register.GetType().Name.Replace("sView", string.Empty));
            ResetIdentityValueTableInDb(SystemTable);
        }

        /// <summary>
        /// Transfer Estocs to Initial Stocks.
        /// </summary>
        public void TransferInitialStocs()
        {
            TransferInitialStocsInDb();
        }

        #endregion

        #region DataBase

        /// <summary>
        /// Get the next identity field value of the table passed by parameter
        /// </summary>
        /// <param name="SystemTable">Define the table which be applied the operation</param>
        /// <returns>The new Identity Number of the table if the operation ends properly</returns>
        private int GetNextIdentityValueTableInDb(SystemTables SystemTable)
        {
            return (HispaniaDataAccess.Instance.GetNextIdentityValueTable(SystemTable));
        }

        /// <summary>
        /// Reset the identity field value of the table passed by parameter at 1.
        /// </summary>
        /// <param name="TableType">Define the table which be applied the operation</param>
        /// <returns>1 if the operation ends properly</returns>
        private void ResetIdentityValueTableInDb(SystemTables SystemTable)
        {
            HispaniaDataAccess.Instance.ResetIdentityValueTable(SystemTable);
        }

        /// <summary>
        /// Transfer Estocs to Initial Stocks.
        /// </summary>
        private void TransferInitialStocsInDb()
        {
            HispaniaDataAccess.Instance.TransferInitialStocs();
        }

        #endregion
    }
}
