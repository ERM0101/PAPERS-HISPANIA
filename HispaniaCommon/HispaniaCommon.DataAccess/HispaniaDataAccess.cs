﻿#region Librerias usadas por la clase

using HispaniaComptabilitat.Data;
using MBCode.Framework.Managers.Messages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.ServiceModel;
using HispaniaCompData = HispaniaComptabilitat.Data;
using System.Configuration;
using HispaniaComptabilitat.Data.EntitiesEX;
using System.ServiceModel.Dispatcher;

#endregion

namespace HispaniaCommon.DataAccess
{
    public class HispaniaDataAccess
    {
        #region Attributes

        /// <summary>
        /// Store the Connections 
        /// </summary>
        private Dictionary<HispaniaComptabilitat.Data.Entities, DbContextTransaction> Connections =
                                   new Dictionary<HispaniaComptabilitat.Data.Entities, DbContextTransaction>();

        #endregion

        #region Singleton Pattern

        /// <summary>
        /// Store the reference at the singleton instance of the class.
        /// </summary>
        internal static HispaniaDataAccess instance;

        /// <summary>
        /// Allow the application to access at the reference at the singleton instance of the class.
        /// </summary>
        public static HispaniaDataAccess Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HispaniaDataAccess();
                }
                return instance;
            }
        }

        /// <summary>
        /// Default builder.
        /// </summary>
        private HispaniaDataAccess()
        {
        }

        #endregion

        #region System

        #region Fill Datatable from Query value

        public DataTable GetDataTableFromQuerySQL(string QuerySQL)
        {
            string MsgErr = string.Format("Error, al obtindre les dades de la consulta '{0}'.\r\nDetalls: {1}\r\n{2}",
                                          QuerySQL, "{0}", "Intentiu de nou, i si el problema persisteix consulti l'administrador");
            DataTable OutputData = null;
            if (string.IsNullOrEmpty(QuerySQL))
            {
                throw new Exception(string.Format(MsgErr, "La consulta no pot ser nula ni buida."));
            }
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HispaniaComptabilitatConnectionString"].ConnectionString))
                    //using (SqlConnection conn = new SqlConnection(db.Database.Connection.ConnectionString + ";Password=Phispania2"))
                    //using (SqlConnection conn = new SqlConnection(db.Database.Connection.ConnectionString))
                    {
                        SqlCommand cmd = new SqlCommand(QuerySQL, conn);
                        conn.Open();
                        try
                        {
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            OutputData = new DataTable();
                            da.Fill(OutputData);
                            conn.Close();
                            da.Dispose();
                        }
                        catch (Exception ex)
                        {
                            conn.Close();
                            throw new Exception(string.Format(MsgErr, MsgManager.ExcepMsg(ex)));
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                throw new Exception(string.Format(MsgErr, MsgManager.ExcepMsg(ex)));
            }
            return OutputData;
        }

        #endregion

        #region Manage Identity Values

        [OperationContract]
        public int GetNextIdentityValueTable(SystemTables SystemTable)
        {
            string TableName = SystemTable.ToString("g");
            string MsgErr = string.Format("Error al calcular el següent valor del camp identitat de la taula '{0}'.\r\nDetalls: {1}\r\n{2}",
                                          TableName, "{0}", "Intentiu de nou, i si el problema persisteix consulti l'administrador");
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    string query;
                    bool IsEmpty = false;
                    switch (SystemTable)
                    {
                        case SystemTables.Customer:
                            IsEmpty = (db.Customers.Count() == 0);
                            break;
                        case SystemTables.CustomerOrder:
                            IsEmpty = (db.CustomerOrders.Count() == 0);
                            break;
                        case SystemTables.CustomerOrderMovement:
                            IsEmpty = (db.CustomerOrderMovements.Count() == 0);
                            break;
                        case SystemTables.Bill:
                            IsEmpty = (db.Bills.Count() == 0);
                            break;
                        case SystemTables.Provider:
                            IsEmpty = (db.Providers.Count() == 0);
                            break;
                        case SystemTables.ProviderOrder:
                            IsEmpty = (db.ProviderOrders.Count() == 0);
                            break;
                        case SystemTables.ProviderOrderMovement:
                            IsEmpty = (db.CustomerOrderMovements.Count() == 0);
                            break;
                        
                        //case SystemTables.IssuanceSupplierOrder: // ToDo: Activate when IssuanceSupplierOrders will be implemented.
                        //     IsEmpty = (db.IssuanceSupplierOrders.Count() == 0);
                        //     break;
                        default:
                            throw new Exception(string.Format(MsgErr, "Taula no reconeguda."));
                    }
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HispaniaComptabilitatConnectionString"].ConnectionString))
                    //using (SqlConnection conn = new SqlConnection(db.Database.Connection.ConnectionString + ";Password=Phispania2"))
                    //using (SqlConnection conn = new SqlConnection(db.Database.Connection.ConnectionString))
                    {
                        conn.Open();
                        try
                        {
                            if (IsEmpty) query = string.Format("SELECT IDENT_CURRENT('{0}')", TableName);
                            else query = string.Format("SELECT IDENT_CURRENT('{0}') + IDENT_INCR('{0}')", TableName);
                            using (SqlCommand command = new SqlCommand(query, conn))
                            {
                                return (Convert.ToInt32(command.ExecuteScalar()));
                            }
                        }
                        catch (Exception ex)
                        {
                            conn.Close();
                            throw new Exception(string.Format(MsgErr, MsgManager.ExcepMsg(ex)));
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                throw new Exception(string.Format(MsgErr, MsgManager.ExcepMsg(ex)));
            }
        }

        [OperationContract]
        public void ResetIdentityValueTable(SystemTables SystemTable)
        {
            string TableName = SystemTable.ToString("g");
            string MsgErr = string.Format("Error al resetejar el valor del camp identitat de la taula '{0}'.\r\nDetalls: {1}\r\n{2}",
                                          TableName, "{0}", "Intentiu de nou, i si el problema persisteix consulti l'administrador");
            switch (SystemTable)
            {
                case SystemTables.Bill:
                    break;
                default:
                    throw new Exception(string.Format(MsgErr, "Taula no reconeguda."));
            }
            bool FinishedWithErrors = false;
            string ExcepErrMsg = string.Empty;
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Determina que cal fer el reseteig de la taula.
                            bool IsResetNeeded = SystemTable == SystemTables.Bill;
                            //  Si es necesari resetja el Id de la taula.
                            if (IsResetNeeded)
                            {
                                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HispaniaComptabilitatConnectionString"].ConnectionString))
                                //using (SqlConnection conn = new SqlConnection(db.Database.Connection.ConnectionString + ";Password=Phispania2"))
                                //using (SqlConnection conn = new SqlConnection(db.Database.Connection.ConnectionString))
                                {
                                    conn.Open();
                                    try
                                    {
                                        string query = string.Format("DBCC CHECKIDENT([{0}], RESEED, 0);", TableName);
                                        using (SqlCommand command = new SqlCommand(query, conn))
                                        {
                                            Convert.ToInt32(command.ExecuteNonQuery());
                                        }
                                        conn.Close();
                                    }
                                    catch (Exception ex)
                                    {
                                        conn.Close();
                                        throw new Exception(string.Format(MsgErr, MsgManager.ExcepMsg(ex)));
                                    }
                                }
                            }
                            //  Si la taula és la de Factures actualitza el valor del camp Num_Factura a paràmetres generals.
                            if (SystemTable == SystemTables.Bill)
                            {
                                HispaniaCompData.Parameter ParameterToModify = db.Parameters.ToList().First<HispaniaCompData.Parameter>();
                                ParameterToModify.Customer_NumBill = 1;
                                db.Entry(ParameterToModify).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            FinishedWithErrors = true;
                            ExcepErrMsg = MsgManager.ExcepMsg(ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FinishedWithErrors = true;
                ExcepErrMsg = MsgManager.ExcepMsg(ex);
            }
            if (FinishedWithErrors) throw new Exception(string.Format(MsgErr, ExcepErrMsg));
        }

        #endregion

        #region Manage Transactions

        [OperationContract]
        public bool BeginTransaction(out HispaniaComptabilitat.Data.Entities db, out string ErrMsg)
        {
            try
            {
                db = new HispaniaComptabilitat.Data.Entities();
                Connections.Add(db, db.Database.BeginTransaction());
                ErrMsg = string.Empty;
            }
            catch (Exception ex)
            {
                db = null;
                ErrMsg = MsgManager.ExcepMsg(ex);
            }
            return (string.IsNullOrEmpty(ErrMsg));
        }

        public bool CommitTransaction( HispaniaComptabilitat.Data.Entities connection, out string ErrMsg)
        {
            try
            {
                Connections[connection].Commit();
                ErrMsg = string.Empty;
            }
            catch (Exception ex)
            {
                ErrMsg = MsgManager.ExcepMsg(ex);
            }
            return (string.IsNullOrEmpty(ErrMsg));
        }

        public bool RollbackTransaction( HispaniaComptabilitat.Data.Entities connection, out string ErrMsg)
        {
            try
            {
                Connections[connection].Rollback();
                ErrMsg = string.Empty;
            }
            catch (Exception ex)
            {
                ErrMsg = MsgManager.ExcepMsg(ex);
            }
            return (string.IsNullOrEmpty(ErrMsg));
        }

        #endregion

        #endregion

        #region Tables

        #region LocalConnections [CRUD]

        [OperationContract]
        public HispaniaCompData.LocalConnection ReadLocalConnection(int LocalConnection_Id)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.LocalConnections.Find(LocalConnection_Id);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.LocalConnection> ReadLocalConnection(string MachineName)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.LocalConnections.Where(p => (p.MachineName.ToUpper() == MachineName.ToUpper())).ToList();
            }
        }

        #endregion

        #region Locks [CRUD]

        [OperationContract]
        public void CreateLock(HispaniaCompData.Lock newLock)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.Lock LockToSave = db.Locks.Add(newLock);
                    db.SaveChanges();
                    newLock.TableName = LockToSave.TableName;
                    newLock.IdRegister = LockToSave.IdRegister;
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot bloquejar el registre '{0}' de la taula '{1}'\r\n{2}.",
                                                newLock.IdRegister, newLock.TableName,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.Lock> ReadLock(string TableName, string IdRegister)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities() )
            {
                return db.Locks.Where(p => ((p.TableName == TableName) && (p.IdRegister == IdRegister))).ToList();
            }
        }

        [OperationContract]
        public void DeleteLock(string TableName, string IdRegister)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.Lock LockToDelete = db.Locks.Find(new object[] { TableName, IdRegister });
                    db.Locks.Remove(LockToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar el bloquejar pel registre '{0}' de la taula '{1}'\r\n{2}.",
                                                IdRegister, TableName,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        //[ValidateAntiForgeryToken]
        public void DeleteLock(int LocalConnection_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    foreach (HispaniaCompData.Lock lockToDelete in db.Locks.Where(p => p.LocalConnection_Id == LocalConnection_Id).ToList())
                    {
                        db.Locks.Remove(lockToDelete);
                        db.SaveChanges();
                    }
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es poden esborrar els bloquejos de la màquina amb identificador '{0}'\r\n{1}.",
                                                LocalConnection_Id, "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region QueryCustom [CRUD]

        [OperationContract]
        public string GetQueryCustom(string QueryCustom_Key)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    List<HispaniaCompData.QueryCustom> queryCustom = db.QueryCustoms.Where(c => c.Key == QueryCustom_Key).ToList();
                    return (queryCustom[0].Query);
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar la consulta {0}.\r\n{1}.", QueryCustom_Key,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region PostalCodes [CRUD]

        [OperationContract]
        public void CreatePostalCode(HispaniaCompData.PostalCode postalCode)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.PostalCode postalCodeToSave = db.PostalCodes.Add(postalCode);
                    db.SaveChanges();
                    postalCode.PostalCode_Id = postalCodeToSave.PostalCode_Id;
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar el nou Codi Postal {0}.\r\n{1}.", postalCode.Postal_Code,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.PostalCode> ReadPostalCodes()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.PostalCodes.ToList();
            }
        }

        [OperationContract]
        public void UpdatePostalCode(HispaniaCompData.PostalCode postalCode)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(postalCode).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden guardar els canvis fets al Codi Postal.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeletePostalCode(HispaniaCompData.PostalCode postalCode)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.PostalCode postalCodeToDelete = db.PostalCodes.Find(postalCode.PostalCode_Id);
                    db.PostalCodes.Remove(postalCodeToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar el Codi Postal {0}.\r\n{1}.", postalCode.Postal_Code,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.PostalCode GetPostalCode(int PostalCode_PostalCode_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.PostalCodes.Find(PostalCode_PostalCode_Id));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar el Codi Postal {0}.\r\n{1}.", PostalCode_PostalCode_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region Parameters [RU]

        [OperationContract]
        public HispaniaCompData.Parameter ReadParameters()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.Parameters.ToList().First<HispaniaCompData.Parameter>();
            }
        }

        [OperationContract]
        public void UpdateParameters(HispaniaCompData.Parameter parameter)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(parameter).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es poden guardar els paràmetres {0}.\r\n{1}.", parameter.Parameter_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region BillingSeries [CRUD]

        [OperationContract]
        public void CreateBillingSerie(HispaniaCompData.BillingSerie billingSerie)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.BillingSerie billingSerieToSave = db.BillingSeries.Add(billingSerie);
                    db.SaveChanges();
                    billingSerie.Serie_Id = billingSerieToSave.Serie_Id;
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar la nova Serie de Facturació {0}.\r\n{1}.", billingSerie.Name,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.BillingSerie> ReadBillingSeries()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.BillingSeries.ToList();
            }
        }

        [OperationContract]
        public void UpdateBillingSerie(HispaniaCompData.BillingSerie billingSerie)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(billingSerie).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden guardar els canvis fets a la Serie de Facturació.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeleteBillingSerie(HispaniaCompData.BillingSerie billingSerie)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.BillingSerie billingSerieToDelete = db.BillingSeries.Find(billingSerie.Serie_Id);
                    db.BillingSeries.Remove(billingSerieToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar la Serie de Facturació {0}.\r\n{1}.", billingSerie.Name,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.BillingSerie GetBillingSerie(int BillingSerie_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.BillingSeries.Find(BillingSerie_Id));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar la Sèrie de Facturació amb identificador '{0}'.\r\n{1}.", BillingSerie_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region IVATypes [CRUD]

        [OperationContract]
        public void CreateIVAType(HispaniaCompData.IVAType ivaType)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.IVAType IVATypeToSave = db.IVATypes.Add(ivaType);
                    db.SaveChanges();
                    ivaType.IVAType_Id = IVATypeToSave.IVAType_Id;
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar el nou Tipus d'IVA ({0} => IVA {1}% / Recàrrec {2}%.\r\n{3}.",
                                                ivaType.Type, ivaType.IVAPercent, ivaType.SurchargePercent,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.IVAType> ReadIVATypes()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.IVATypes.ToList();
            }
        }

        [OperationContract]
        public void UpdateIVAType(HispaniaCompData.IVAType ivaType)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(ivaType).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden guardar els canvis fets al Tipus d'IVA.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeleteIVAType(HispaniaCompData.IVAType ivaType)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.IVAType ivaTypeToDelete = db.IVATypes.Find(ivaType.IVAType_Id);
                    db.IVATypes.Remove(ivaTypeToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar Tipus d'IVA ({0} => IVA {1}% / Recàrrec {2}%.\r\n{3}.",
                                                ivaType.Type, ivaType.IVAPercent, ivaType.SurchargePercent,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.IVAType GetIVAType(int IVAType_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.IVATypes.Find(IVAType_Id));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar el Tipus d'IVA {0}.\r\n{1}.", IVAType_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region SendTypes [CRUD]

        [OperationContract]
        public void CreateSendType(HispaniaCompData.SendType sendType)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.SendType SendTypeToSave = db.SendTypes.Add(sendType);
                    db.SaveChanges();
                    sendType.SendType_Id = SendTypeToSave.SendType_Id;
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar el nou Tipus d'Envimanet {0}\r\n{1}.", sendType.Code,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.SendType> ReadSendTypes()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.SendTypes.ToList();
            }
        }

        [OperationContract]
        public void UpdateSendType(HispaniaCompData.SendType sendType)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(sendType).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden guardar els canvis fets al Tipus d'Enviament.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeleteSendType(HispaniaCompData.SendType sendType)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.SendType SendTypeToDelete = db.SendTypes.Find(sendType.SendType_Id);
                    db.SendTypes.Remove(SendTypeToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar el Tipus d'Enviament {0}\r\n{1}.", sendType.Code,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.SendType GetSendType(int SendType_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.SendTypes.Find(SendType_Id));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar el Tipus d'Enviament {0}.\r\n{1}.", SendType_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region Agents [CRUD]

        [OperationContract]
        public void CreateAgent(HispaniaCompData.Agent agent)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.Agent AgentToSave = db.Agents.Add(agent);
                    db.SaveChanges();
                    agent.Agent_Id = AgentToSave.Agent_Id;
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar el nou Agent '{0}'\r\n{1}.", agent.Name,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.Agent> ReadAgents()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.Agents.ToList();
            }
        }

        [OperationContract]
        public void UpdateAgent(HispaniaCompData.Agent agent)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(agent).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden guardar els canvis fets a l'Agent.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeleteAgent(HispaniaCompData.Agent agent)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.Agent AgentToDelete = db.Agents.Find(agent.Agent_Id);
                    db.Agents.Remove(AgentToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar e l'Agent '{0}'\r\n{1}.", agent.Name,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.Agent GetAgent(int Agent_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.Agents.Find(Agent_Id));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar l'Agent amb identificador '{0}'.\r\n{1}.", Agent_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region EffectTypes [CRUD]

        [OperationContract]
        public void CreateEffectType(HispaniaCompData.EffectType EffectType)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.EffectType EffectTypeToSave = db.EffectTypes.Add(EffectType);
                    db.SaveChanges();
                    EffectType.EffectType_Id = EffectTypeToSave.EffectType_Id;
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar el nou Tipus d'Efecte {0}\r\n{1}.", EffectType.Code,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.EffectType> ReadEffectTypes()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.EffectTypes.ToList();
            }
        }

        [OperationContract]
        public void UpdateEffectType(HispaniaCompData.EffectType EffectType)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(EffectType).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden guardar els canvis fets al Tipus d'Efecte.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeleteEffectType(HispaniaCompData.EffectType EffectType)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.EffectType EffectTypeToDelete = db.EffectTypes.Find(EffectType.EffectType_Id);
                    db.EffectTypes.Remove(EffectTypeToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar el Tipus d'Efecte {0}\r\n{1}.", EffectType.Code,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.EffectType GetEffectType(int EffectType_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.EffectTypes.Find(EffectType_Id));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar el Tipus d'Efecte {0}.\r\n{1}.", EffectType_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region Customers [CRUD]

        [OperationContract]
        public List<HispaniaCompData.Customer> ReadCustomers()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.Customers.ToList();
            }
        }

        [OperationContract]
        public void CreateCustomer(HispaniaCompData.Customer customer, List<HispaniaCompData.RelatedCustomer> relatedCustomers)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Save Customer information in Database
                            HispaniaCompData.Customer CustomerToSave = db.Customers.Add(customer);
                            db.SaveChanges();
                            customer.Customer_Id = CustomerToSave.Customer_Id;
                            //  Save Related Customers information in Database
                            db.RelatedCustomers.AddRange(relatedCustomers);
                            db.SaveChanges();
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            throw new Exception("No es poden guardar els canvis fets al Client.\r\nIntentiu de nou, i si el problema persisteix consulti l'administrador", ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar el nou Client '{0}'\r\n{1}.", customer.Company_Name,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void UpdateCustomer(HispaniaCompData.Customer customer, List<HispaniaCompData.RelatedCustomer> relatedCustomers)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Save Customer information in Database
                            db.Entry(customer).State = EntityState.Modified;
                            db.SaveChanges();
                            //  Remove Old Related Customers
                            db.RelatedCustomers.RemoveRange(db.RelatedCustomers.Where(d => (d.Customer_Id == customer.Customer_Id)).ToList());
                            db.SaveChanges();
                            //  Save Related Customers information in Database
                            db.RelatedCustomers.AddRange(relatedCustomers);
                            db.SaveChanges();
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            throw new Exception("No es poden guardar els canvis fets al Client.\r\nIntentiu de nou, i si el problema persisteix consulti l'administrador", ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden guardar els canvis fets al Client.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeleteCustomer(HispaniaCompData.Customer customer)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Remove Old Related Customers
                            db.RelatedCustomers.RemoveRange(db.RelatedCustomers.Where(d => (d.Customer_Id == customer.Customer_Id)).ToList());
                            db.SaveChanges();
                            //  Delete Customer information in Database
                            HispaniaCompData.Customer CustomerToDelete = db.Customers.Find(customer.Customer_Id);
                            db.Customers.Remove(CustomerToDelete);
                            db.SaveChanges();
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            throw new Exception("No es poden guardar els canvis fets al Client.\r\nIntentiu de nou, i si el problema persisteix consulti l'administrador", ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar el Client '{0}'\r\n{1}.", customer.Company_Name,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.Customer GetCustomer(int Customer_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.Customers.Find(Customer_Id));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar el Client amb identificador '{0}'.\r\n{1}.", Customer_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.Customer> GetCustomersForAgentReport(int Agent_Id)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.Customers.Where(p => ((p.BillingData_Agent_Id == Agent_Id) && (p.QueryData_Active != true))).ToList();
            }
        }

        #endregion

        #region AddressStores [CRUD]

        [OperationContract]
        public void CreateAddressStore(HispaniaCompData.AddressStore AddressStore)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.AddressStore AddressStoreToSave = db.AddressStores.Add(AddressStore);
                    db.SaveChanges();
                    AddressStore.AddressStore_Id = AddressStoreToSave.AddressStore_Id;
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar la nova adreça de magatzem '{0}' del client '{1}'\r\n{2}.",
                                                AddressStore.AddressStore_Id, AddressStore.Customer_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.AddressStore> ReadAddressStores()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.AddressStores.ToList();
            }
        }

        [OperationContract]
        public List<HispaniaCompData.AddressStore> ReadAddressStores(int Customer_Id)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.AddressStores.Where(p => p.Customer_Id == Customer_Id).ToList();
            }
        }

        [OperationContract]
        public void UpdateAddressStore(HispaniaCompData.AddressStore AddressStore)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(AddressStore).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden guardar els canvis fets a l'adreça de magatzem.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeleteAddressStore(HispaniaCompData.AddressStore AddressStore)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.AddressStore AddressStoreToDelete = db.AddressStores.Find(AddressStore.AddressStore_Id);
                    db.AddressStores.Remove(AddressStoreToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar l'adreça de magatzem '{0}' del client '{1}'\r\n{2}.",
                                                AddressStore.AddressStore_Id, AddressStore.Customer_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.AddressStore GetAddressStore(int AddressStore_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.AddressStores.Find(AddressStore_Id));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar l'adreça de magatzem amb identificador '{0}'.\r\n{1}.", AddressStore_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region HistoCumulativeCustomer [CRUD]

        [OperationContract]
        public void CreateHistoCumulativeCustomer(HispaniaCompData.HistoCumulativeCustomer histoCumulativeCustomer)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.HistoCumulativeCustomer HistoCumulativeCustomerToSave = db.HistoCumulativeCustomers.Add(histoCumulativeCustomer);
                    db.SaveChanges();
                    histoCumulativeCustomer.Histo_Id = HistoCumulativeCustomerToSave.Histo_Id;
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar el nou històric d'acumulats de client '{0}' del client '{1}'\r\n{2}.",
                                                histoCumulativeCustomer.Histo_Id, histoCumulativeCustomer.Customer_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.HistoCumulativeCustomer> ReadHistoCumulativeCustomers()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.HistoCumulativeCustomers.ToList();
            }
        }

        [OperationContract]
        public List<HispaniaCompData.HistoCumulativeCustomer> ReadHistoCumulativeCustomer(int Customer_Id)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.HistoCumulativeCustomers.Where(p => p.Customer_Id == Customer_Id).ToList();
            }
        }

        [OperationContract]
        public void UpdateHistoCumulativeCustomer(HispaniaCompData.HistoCumulativeCustomer histoCumulativeCustomer)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(histoCumulativeCustomer).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden guardar els canvis fets a l'històric d'acumulats de client.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeleteHistoCumulativeCustomer(HispaniaCompData.HistoCumulativeCustomer histoCumulativeCustomer)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.HistoCumulativeCustomer HistoCumulativeCustomerToDelete = db.HistoCumulativeCustomers.Find(histoCumulativeCustomer.Histo_Id);
                    db.HistoCumulativeCustomers.Remove(HistoCumulativeCustomerToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar l'historic d'acumulats de client '{0}' del client '{1}'\r\n{2}.",
                                                histoCumulativeCustomer.Histo_Id, histoCumulativeCustomer.Customer_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.HistoCumulativeCustomer GetHistoCumulativeCustomer(int HistoCumulativeCustomer_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.HistoCumulativeCustomers.Find(HistoCumulativeCustomer_Id));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar l'històric d'acumulats de client amb identificador '{0}'.\r\n{1}.", HistoCumulativeCustomer_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region Bills [CRUD]

        [OperationContract]
        public void CreateBill(HispaniaCompData.Bill Bill)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.Bill BillToSave = db.Bills.Add(Bill);
                    db.SaveChanges();
                    Bill.Bill_Id = BillToSave.Bill_Id;
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar la nova Factura '{0}'\r\n{1}.", Bill.Bill_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        /// <summary>
        /// Get the Bills from the Database
        /// </summary>
        /// <param name="Year">If Year value is null we use the current year bills</param>
        /// <param name="Month">Month only take importance if Year is not null and in this case cannot be null</param>
        [OperationContract]
        public List<HispaniaCompData.Bill> ReadBills(decimal? Year = null, decimal? Month = null)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                if (Year is null)
                {
                    return (db.Bills.Where(c => c.Year == DateTime.Now.Year).ToList());
                }
                else
                {
                    if ((Month is null) || (Month == 0))
                    {
                        return (db.Bills.Where(c => c.Year == Year).ToList());
                    }
                    else
                    {
                        return (db.Bills.Where(c => c.Year == Year && c.Date.Value.Month == Month).ToList());
                    }
                }
            }
        }

        /// <summary>
        /// Get the Bills from the Database
        /// </summary>
        /// <param name="Year">If Year value is null we use the current year bills</param>
        /// <param name="Month">Month only take importance if Year is not null and in this case cannot be null</param>
        [OperationContract]
        public List<HispaniaCompData.Bill> ReadBills(out List<HispaniaCompData.CustomerOrderMovement> Movements,
                                                     decimal? Year = null, decimal? Month = null)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                Movements = new List<HispaniaCompData.CustomerOrderMovement>();
                try
                {
                    List<int> CustomerOrdersId;
                    if (Year is null)
                    {
                        CustomerOrdersId = db.CustomerOrders.Where(c => (c.Bill_Year != null && c.Bill_Year == DateTime.Now.Year)).Select(d => d.CustomerOrder_Id).ToList();
                        Movements = db.CustomerOrderMovements.Where(m => (m.CustomerOrder_Id != null && CustomerOrdersId.Contains((int)m.CustomerOrder_Id))).ToList();
                        return (db.Bills.Where(c => c.Year == DateTime.Now.Year).ToList());
                    }
                    else
                    {
                        if ((Month is null) || (Month == 0))
                        {
                            CustomerOrdersId = db.CustomerOrders.Where(c => (c.Bill_Year != null && c.Bill_Year == Year)).Select(d => d.CustomerOrder_Id).ToList();
                            Movements = db.CustomerOrderMovements.Where(m => (m.CustomerOrder_Id != null && CustomerOrdersId.Contains((int)m.CustomerOrder_Id))).ToList();
                            return (db.Bills.Where(c => c.Year == Year).ToList());
                        }
                        else
                        {
                            CustomerOrdersId = db.CustomerOrders.Where(c => (c.Bill_Year != null && c.Bill_Year == Year && c.Date.Value.Month == Month)).Select(d => d.CustomerOrder_Id).ToList();
                            Movements = db.CustomerOrderMovements.Where(m => (m.CustomerOrder_Id != null && CustomerOrdersId.Contains((int)m.CustomerOrder_Id))).ToList();
                            return (db.Bills.Where(c => c.Year == Year && c.Date.Value.Month == Month).ToList());
                        }
                    }
                }
                catch (Exception ex)
                {
                    MsgManager.ShowMessage(ex.Message);
                    return new List<HispaniaCompData.Bill>();
                }
            }
        }

        [OperationContract]
        public void UpdateBill(HispaniaCompData.Bill Bill, List<HispaniaCompData.Receipt> Receipts)
        {
            string ErrMsg;
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Save Bill information in Database
                            db.Entry(Bill).State = EntityState.Modified;
                            db.SaveChanges();
                            //  Save Receipts information in Database
                            foreach (HispaniaCompData.Receipt Receipt in Receipts)
                            {
                                if (GetReceipt(Receipt.Receipt_Id) is null) db.Receipts.Add(Receipt);
                                else db.Entry(Receipt).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            ErrMsg = "No es poden guardar els canvis fets a la factura.\r\n" +
                                     "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                            throw new Exception(ErrMsg, ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                ErrMsg = "No es poden guardar els canvis fets a la Factura.\r\n" +
                         "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(ErrMsg, ex);
            }
        }

        [OperationContract]
        public void DeleteBill(HispaniaCompData.Bill Bill)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.Bill BillToDelete = db.Bills.Find(Bill.Bill_Id);
                    db.Bills.Remove(BillToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar la factura '{0}'\r\n{1}.", Bill.Bill_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.Bill GetBill(int Bill_Id, decimal Bill_Year)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.Bills.Find(new object[] { Bill_Id, Bill_Year }));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar la factura amb identificador '{0}'.\r\n{1}.", Bill_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<decimal> GetBillYears(bool HistoricData = false)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    if (HistoricData)
                    {
                        return (db.Bills.Select(c => c.Year).Distinct().ToList());
                    }
                    else
                    {
                        return (db.Bills.Select(c => c.Year).Where(t => t == DateTime.Now.Year).Distinct().ToList());
                    }
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden obtindre els anys de les factures.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public int GetLastBill(decimal YearQuery)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return db.CustomerOrders.Where(c => ((c.Daily == true) && (c.Bill_Year == YearQuery))).Select(d => (int)d.Bill_Id).DefaultIfEmpty(0).Max();
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es pot obtindre la darrera de les factures.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region Receipts [CRUD]

        [OperationContract]
        public void CreateReceipt(HispaniaCompData.Receipt Receipt)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.Receipt ReceiptToSave = db.Receipts.Add(Receipt);
                    db.SaveChanges();
                    Receipt.Receipt_Id = ReceiptToSave.Receipt_Id;
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar el nou rebut de la factura '{0}'\r\n{1}.", Receipt.Bill_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.Receipt> ReadReceipts(bool HistoricData = false)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                if (HistoricData)
                {
                    return db.Receipts.ToList();
                }
                else
                {
                    return (db.Receipts.Where(c => c.Bill_Year == DateTime.Now.Year).ToList());
                }
            }
        }

        [OperationContract]
        public void UpdateReceipt(HispaniaCompData.Receipt Receipt)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(Receipt).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden guardar els canvis fets al Rebut.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeleteReceipt(HispaniaCompData.Receipt Receipt)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.Receipt ReceiptToDelete = db.Receipts.Find(Receipt.Receipt_Id);
                    db.Receipts.Remove(ReceiptToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar el rebut '{0}'\r\n{1}.", Receipt.Receipt_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.Receipt GetReceipt(int Receipt_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.Receipts.Find(Receipt_Id));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar el Rebut amb identificador '{0}'.\r\n{1}.", Receipt_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.Receipt> GetReceiptsFromBill(int Bill_Id, decimal Bill_Year)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.Receipts.Where(c => c.Bill_Id == Bill_Id && c.Bill_Year == Bill_Year).ToList());
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar els rebuts de la factura amb identificador '{0}'.\r\n{1}.", Bill_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region Units [CRUD]

        [OperationContract]
        public void CreateUnit(HispaniaCompData.Unit unit)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.Unit UnitToSave = db.Units.Add(unit);
                    db.SaveChanges();
                    unit.Unit_Id = UnitToSave.Unit_Id;
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar el nou tipus d'unitat '{0}'\r\n{1}.", unit.Unit_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.Unit> ReadUnits()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.Units.ToList();
            }
        }

        [OperationContract]
        public void UpdateUnit(HispaniaCompData.Unit unit)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(unit).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden guardar els canvis fets a la unitat.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeleteUnit(HispaniaCompData.Unit unit)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.Unit UnitToDelete = db.Units.Find(unit.Unit_Id);
                    db.Units.Remove(UnitToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar la unitat '{0}'\r\n{1}.", unit.Unit_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.Unit GetUnit(int Unit_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.Units.Find(Unit_Id));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar la unitat amb identificador '{0}'.\r\n{1}.", Unit_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region Goods [CRUD]

        [OperationContract]
        public void CreateGood(HispaniaCompData.Good good)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.Good GoodToSave = db.Goods.Add(good);
                    db.SaveChanges();
                    good.Good_Id = GoodToSave.Good_Id;
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar el nou tipus d'article '{0}'\r\n{1}.", good.Good_Code,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.Good> ReadGoods()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.Goods.ToList();
            }
        }


        [OperationContract]
        public HispaniaCompData.Good GetGood(int Good_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.Goods.Find(Good_Id));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar l'article amb identificador '{0}'.\r\n{1}.", Good_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.Good GetGood(string Good_Code)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    List<HispaniaCompData.Good> Info = db.Goods.Where(g => g.Good_Code == Good_Code).ToList();
                    if (Info.Count > 0) return (Info[0]);
                    else throw new Exception("Value not found");
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar l'article amb identificador '{0}'.\r\n{1}.", Good_Code,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void UpdateGood(HispaniaCompData.Good good)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(good).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden guardar els canvis fets a l'article.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void UpdateGoodStocks(HispaniaCompData.Good good)
        {
            try
            {
                HispaniaCompData.Good goodToUpdate = GetGood(good.Good_Id);
                goodToUpdate.Initial = good.Initial;
                goodToUpdate.Initial_Fact = good.Initial_Fact;
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(goodToUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden guardar els canvis fets a l'article.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void UpdateGoodAcums(HispaniaCompData.Good good)
        {
            try
            {
                HispaniaCompData.Good goodToUpdate = GetGood(good.Good_Id);
                goodToUpdate.Cumulative_Sales_Retail_Price_1 = good.Cumulative_Sales_Retail_Price_1;
                goodToUpdate.Cumulative_Sales_Retail_Price_2 = good.Cumulative_Sales_Retail_Price_2;
                goodToUpdate.Cumulative_Sales_Retail_Price_3 = good.Cumulative_Sales_Retail_Price_3;
                goodToUpdate.Cumulative_Sales_Retail_Price_4 = good.Cumulative_Sales_Retail_Price_4;
                goodToUpdate.Cumulative_Sales_Retail_Price_5 = good.Cumulative_Sales_Retail_Price_5;
                goodToUpdate.Cumulative_Sales_Retail_Price_6 = good.Cumulative_Sales_Retail_Price_6;
                goodToUpdate.Cumulative_Sales_Retail_Price_7 = good.Cumulative_Sales_Retail_Price_7;
                goodToUpdate.Cumulative_Sales_Retail_Price_8 = good.Cumulative_Sales_Retail_Price_8;
                goodToUpdate.Cumulative_Sales_Retail_Price_9 = good.Cumulative_Sales_Retail_Price_9;
                goodToUpdate.Cumulative_Sales_Retail_Price_10 = good.Cumulative_Sales_Retail_Price_10;
                goodToUpdate.Cumulative_Sales_Retail_Price_11 = good.Cumulative_Sales_Retail_Price_11;
                goodToUpdate.Cumulative_Sales_Retail_Price_12 = good.Cumulative_Sales_Retail_Price_12;
                goodToUpdate.Cumulative_Sales_Cost_1 = good.Cumulative_Sales_Cost_1;
                goodToUpdate.Cumulative_Sales_Cost_2 = good.Cumulative_Sales_Cost_2;
                goodToUpdate.Cumulative_Sales_Cost_3 = good.Cumulative_Sales_Cost_3;
                goodToUpdate.Cumulative_Sales_Cost_4 = good.Cumulative_Sales_Cost_4;
                goodToUpdate.Cumulative_Sales_Cost_5 = good.Cumulative_Sales_Cost_5;
                goodToUpdate.Cumulative_Sales_Cost_6 = good.Cumulative_Sales_Cost_6;
                goodToUpdate.Cumulative_Sales_Cost_7 = good.Cumulative_Sales_Cost_7;
                goodToUpdate.Cumulative_Sales_Cost_8 = good.Cumulative_Sales_Cost_8;
                goodToUpdate.Cumulative_Sales_Cost_9 = good.Cumulative_Sales_Cost_9;
                goodToUpdate.Cumulative_Sales_Cost_10 = good.Cumulative_Sales_Cost_10;
                goodToUpdate.Cumulative_Sales_Cost_11 = good.Cumulative_Sales_Cost_11;
                goodToUpdate.Cumulative_Sales_Cost_12 = good.Cumulative_Sales_Cost_12;
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(goodToUpdate).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden guardar els canvis fets a l'article.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeleteGood(HispaniaCompData.Good good)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.Good GoodToDelete = db.Goods.Find(good.Good_Id);
                    db.Goods.Remove(GoodToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar l'article '{0}'\r\n{1}.", good.Good_Code,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region PriceRanges [CRUD]

        [OperationContract]
        public void CreatePriceRange(HispaniaCompData.PriceRange PriceRange)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.PriceRange PriceRangeToSave = db.PriceRanges.Add(PriceRange);
                    db.SaveChanges();
                    PriceRange.PriceRange_Id = PriceRangeToSave.PriceRange_Id;
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar el marge de Preus amb identificador {0}.\r\n{1}.",
                                                PriceRange.PriceRange_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.PriceRange> ReadPriceRanges()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.PriceRanges.ToList();
            }
        }

        [OperationContract]
        public List<HispaniaCompData.PriceRange> ReadPriceRanges(int Good_Id)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.PriceRanges.Where(p => p.Good_Id == Good_Id).ToList();
            }
        }

        [OperationContract]
        public void UpdatePriceRange(HispaniaCompData.PriceRange PriceRange)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(PriceRange).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es poden guardar els canvis fets al marge de preus amb identificador {0}.\r\n{1}.",
                                                PriceRange.PriceRange_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeletePriceRange(HispaniaCompData.PriceRange PriceRange)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.PriceRange PriceRangeToDelete = db.PriceRanges.Find(PriceRange.PriceRange_Id);
                    db.PriceRanges.Remove(PriceRangeToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar el marge de preus amb identificador {0}.\r\n{1}.",
                                                PriceRange.PriceRange_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.PriceRange GetPriceRange(int PriceRange_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.PriceRanges.Find(PriceRange_Id));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar el marge de preus amb identificador {0}.\r\n{1}.",
                                                PriceRange_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region HistoGoods [CRUD]

        [OperationContract]
        public void CreateHistoGood(HispaniaCompData.HistoGood histoGood)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.HistoGood HistoGoodToSave = db.HistoGoods.Add(histoGood);
                    db.SaveChanges();
                    histoGood.Histo_Id = HistoGoodToSave.Histo_Id;
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar el nou històric d'article '{0}' de l'article '{1}'\r\n{2}.",
                                                histoGood.Histo_Id, histoGood.Good_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.HistoGood> ReadHistoGoods()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.HistoGoods.ToList();
            }
        }

        [OperationContract]
        public List<HispaniaCompData.HistoGood> ReadHistoGoods(int Good_Id)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.HistoGoods.Where(p => p.Good_Id == Good_Id).ToList();
            }
        }

        [OperationContract]
        public void UpdateHistoGood(HispaniaCompData.HistoGood histoGood)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(histoGood).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden guardar els canvis fets a l'històric de l'article.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeleteHistoGood(HispaniaCompData.HistoGood histoGood)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.HistoGood HistoGoodToDelete = db.HistoGoods.Find(histoGood.Histo_Id);
                    db.HistoGoods.Remove(HistoGoodToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar l'historic de l'article '{0}' de l'article '{1}'\r\n{2}.",
                                                histoGood.Histo_Id, histoGood.Good_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.HistoGood GetHistoGood(int HistoGood_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.HistoGoods.Find(HistoGood_Id));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar l'històric de l'article amb identificador '{0}'.\r\n{1}.", HistoGood_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region Providers [CRUD]

        [OperationContract]
        public void CreateProvider(HispaniaCompData.Provider Provider)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.Provider ProviderToSave = db.Providers.Add(Provider);
                    db.SaveChanges();
                    Provider.Provider_Id = ProviderToSave.Provider_Id;
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar el nou Proveïdor '{0}'\r\n{1}.", Provider.Name,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.Provider> ReadProviders()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.Providers.ToList();
            }
        }

        [OperationContract]
        public void UpdateProvider(HispaniaCompData.Provider provider, List<HispaniaCompData.RelatedProvider> relatedProviders)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Save Provider information in Database
                            db.Entry(provider).State = EntityState.Modified;
                            db.SaveChanges();
                            //  Remove Old Related Providers
                            db.RelatedProviders.RemoveRange(db.RelatedProviders.Where(d => (d.Provider_Id == provider.Provider_Id)).ToList());
                            db.SaveChanges();
                            //  Save Related Providers information in Database
                            db.RelatedProviders.AddRange(relatedProviders);
                            db.SaveChanges();
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            throw new Exception("No es poden guardar els canvis fets al Proveidor.\r\nIntentiu de nou, i si el problema persisteix consulti l'administrador", ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden guardar els canvis fets al Proveidor.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeleteProvider(HispaniaCompData.Provider Provider)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.Provider ProviderToDelete = db.Providers.Find(Provider.Provider_Id);
                    db.Providers.Remove(ProviderToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar el Proveïdor '{0}'\r\n{1}.", Provider.Name,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.Provider GetProvider(int Provider_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.Providers.Find(Provider_Id));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar el Proveïdor amb identificador '{0}'.\r\n{1}.", Provider_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region DeliveryNotes [CRUD]

        [OperationContract]
        public void CreateDeliveryNote(HispaniaCompData.DeliveryNote DeliveryNote)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.DeliveryNote DeliveryNoteToSave = db.DeliveryNotes.Add(DeliveryNote);
                    db.SaveChanges();
                    DeliveryNote.DeliveryNote_Id = DeliveryNoteToSave.DeliveryNote_Id;
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar el nou Albarà '{0}'\r\n{1}.", DeliveryNote.DeliveryNote_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.DeliveryNote> ReadDeliveryNotes()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.DeliveryNotes.ToList();
            }
        }

        [OperationContract]
        public void UpdateDeliveryNote(HispaniaCompData.DeliveryNote DeliveryNote)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(DeliveryNote).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden guardar els canvis fets a l'Albarà.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        private void UpdateDeliveryNote( HispaniaComptabilitat.Data.Entities db, HispaniaCompData.DeliveryNote DeliveryNote)
        {
            db.Entry(DeliveryNote).State = EntityState.Modified;
            db.SaveChanges();
        }

        [OperationContract]
        public void DeleteDeliveryNote(HispaniaCompData.DeliveryNote DeliveryNote)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.DeliveryNote DeliveryNoteToDelete = db.DeliveryNotes.Find(new object[] { DeliveryNote.DeliveryNote_Id, DeliveryNote.Year });
                    db.DeliveryNotes.Remove(DeliveryNoteToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar l'Albarà '{0}'\r\n{1}.", DeliveryNote.DeliveryNote_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.DeliveryNote GetDeliveryNote(int DeliveryNote_Id, decimal DeliveryNotes_Year)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.DeliveryNotes.Find(new object[] { DeliveryNote_Id, DeliveryNotes_Year }));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar l'Albarà amb identificador '<{0},{1}>'.\r\n{2}.", DeliveryNote_Id, DeliveryNotes_Year,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region BadDebts [CRUD]

        [OperationContract]
        public void CreateBadDebt(HispaniaCompData.BadDebt badDebt, List<HispaniaCompData.BadDebtMovement> badDebtMovementsToCreate)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Create Bad Debt and Bad Debt Movements records.
                            CreateBadDebt(db, badDebt, badDebtMovementsToCreate);
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            throw new Exception("No es pot guardar el nou impagat\r\nIntentiu de nou, i si el problema persisteix consulti l'administrador", ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar l'impagat '{0}'\r\n{1}.", badDebt.BadDebt_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        /// <summary>
        /// Operations used to create a new Bad Debt
        /// </summary>
        /// <param name="db">Connection to use.</param>
        /// <param name="badDebt">Bad Debt to create.</param>
        /// <param name="badDebtMovementsToCreate">Bad Debt Movement to create.</param>
        private void CreateBadDebt( HispaniaComptabilitat.Data.Entities db, HispaniaCompData.BadDebt badDebt,
                                   List<HispaniaCompData.BadDebtMovement> badDebtMovementsToCreate, bool Update = false)
        {
            //  Create the register for the new Bad Debt.
            HispaniaCompData.BadDebt BadDebtToSave = db.BadDebts.Add(badDebt);
            db.SaveChanges();
            badDebt.BadDebt_Id = BadDebtToSave.BadDebt_Id;
            //  In this case it's needed to create the Bad Debt Movements (payements) records.
            foreach (HispaniaCompData.BadDebtMovement payement in badDebtMovementsToCreate)
            {
                payement.BadDebt_Id = badDebt.BadDebt_Id;
                db.BadDebtMovements.Add(payement);
                db.SaveChanges();
            }
            //  Update Customer Bad Debt information.
            if (!Update)
            {
                HispaniaCompData.Bill bill = db.Bills.Find(new object[] { badDebt.Bill_Id, badDebt.Bill_Year });
                HispaniaCompData.Customer customer = db.Customers.Find(bill.Customer_Id);
                customer.BillingData_NumUnpaid++;
                customer.BillingData_Unpaid += badDebt.Amount_Pending;
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        [OperationContract]
        public List<HispaniaCompData.BadDebt> ReadBadDebts()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.BadDebts.ToList();
            }
        }

        [OperationContract]
        public List<HispaniaCompData.BadDebt> GetBadDebtsInDb(int Customer_Id)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                List<HispaniaCompData.BadDebt> res;
                res = (from b in db.Bills
                       join bd in db.BadDebts on new { b.Bill_Id, Bill_Year = b.Year } equals new { bd.Bill_Id, bd.Bill_Year }
                       where b.Customer_Id == Customer_Id
                       select bd).ToList();
                return res;
            }
        }

        [OperationContract]
        public void UpdateBadDebt(HispaniaCompData.BadDebt badDebtOld, HispaniaCompData.BadDebt badDebt, List<HispaniaCompData.BadDebtMovement> badDebtMovements)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Delete previous Bad Debt Movement records.
                            DeleteBadDebt(db, badDebt, true);
                            //  Create Bad Debt and Bad Debt Movements records.
                            CreateBadDebt(db, badDebt, badDebtMovements, true);
                            //  Update Customer Information
                            HispaniaCompData.Bill bill = db.Bills.Find(new object[] { badDebt.Bill_Id, badDebt.Bill_Year });
                            HispaniaCompData.Customer customer = db.Customers.Find(bill.Customer_Id);
                            customer.BillingData_Unpaid = customer.BillingData_Unpaid - badDebtOld.Amount_Pending + badDebt.Amount_Pending;
                            db.Entry(customer).State = EntityState.Modified;
                            db.SaveChanges();
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            throw new Exception("No es pot guardar el nou impagat\r\nIntentiu de nou, i si el problema persisteix consulti l'administrador", ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden guardar els canvis fets a l'impagat.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeleteBadDebt(HispaniaCompData.BadDebt badDebt)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Delete Bad Debt and Bad Debt Movements records.
                            DeleteBadDebt(db, badDebt);
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            throw new Exception("No es pot guardar el nou impagat\r\nIntentiu de nou, i si el problema persisteix consulti l'administrador", ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar l'impagat '{0}'\r\n{1}.",
                                                badDebt.BadDebt_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        /// <summary>
        /// Operations used to create a new Bad Debt
        /// </summary>
        /// <param name="db">Connection to use.</param>
        /// <param name="badDebt">Bad Debt to create.</param>
        private void DeleteBadDebt( HispaniaComptabilitat.Data.Entities db, HispaniaCompData.BadDebt badDebt, bool Update = false)
        {
            //  Update Customer Bad Debt information.
            if (!Update)
            {
                HispaniaCompData.Bill bill = db.Bills.Find(new object[] { badDebt.Bill_Id, badDebt.Bill_Year });
                HispaniaCompData.Customer customer = db.Customers.Find(bill.Customer_Id);
                customer.BillingData_NumUnpaid--;
                customer.BillingData_Unpaid -= badDebt.Amount_Pending;
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
            }
            //  In this case it's needed to create the Bad Debt Movements (payements) records.
            int BadDebt_Id_To_Operate = badDebt.BadDebt_Id;
            List<HispaniaCompData.BadDebtMovement> BadDebMovementstToDelete = db.BadDebtMovements.Where(bdm => (bdm.BadDebt_Id == BadDebt_Id_To_Operate)).ToList();
            db.BadDebtMovements.RemoveRange(BadDebMovementstToDelete);
            db.SaveChanges();
            //  Delete the register of the Bad Debt.
            HispaniaCompData.BadDebt BadDebtToDelete = db.BadDebts.Find(BadDebt_Id_To_Operate);
            db.BadDebts.Remove(BadDebtToDelete);
            db.SaveChanges();
        }

        [OperationContract]
        public HispaniaCompData.BadDebt GetBadDebt(int BadDebt_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.BadDebts.Find(BadDebt_Id));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar l'impagat '{0}'.\r\n{1}.", BadDebt_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.BadDebt GetBadDebtFromReceiptInDb(int Receipt_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    List<HispaniaCompData.BadDebt> BadDebtList = db.BadDebts.Where(bd => (bd.Receipt_Id == Receipt_Id)).ToList();
                    return BadDebtList.Count == 0 ? null : BadDebtList[0];
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("Al cercar el Rebut '{0}' associat a l'impagat.\r\n{1}.", Receipt_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region BadDebtMovements [CRUD]

        [OperationContract]
        public void CreateBadDebtMovement(HispaniaCompData.BadDebtMovement badDebtMovement)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.BadDebtMovement BadDebtMovementToSave = db.BadDebtMovements.Add(badDebtMovement);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar el pagament '{0}' de l'impagat '{1}'\r\n{2}.",
                                                badDebtMovement.BadDebtMovement_Id, badDebtMovement.BadDebt_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.BadDebtMovement> ReadBadDebtMovements()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.BadDebtMovements.ToList();
            }
        }

        [OperationContract]
        public void UpdateBadDebtMovement(HispaniaCompData.BadDebtMovement badDebtMovement)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(badDebtMovement).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden guardar els canvis fets al pagament de l'impagat del client.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeleteBadDebtMovement(HispaniaCompData.BadDebtMovement badDebtMovement)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.BadDebtMovement BadDebtMovementToDelete = db.BadDebtMovements.Find(badDebtMovement.BadDebt_Id);
                    db.BadDebtMovements.Remove(BadDebtMovementToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar el pagament '{0}' de l'impagat '{1}'\r\n{2}.",
                                                badDebtMovement.BadDebtMovement_Id, badDebtMovement.BadDebt_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.BadDebtMovement GetBadDebtMovement(int BadDebtMovement_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.BadDebtMovements.Find(BadDebtMovement_Id));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar el pagament '{0}' de l'impagat del client.\r\n{1}.",
                                                BadDebtMovement_Id, "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.BadDebtMovement> GetBadDebtMovementsInDb(int BadDebt_Id)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.BadDebtMovements.Where(bdm => bdm.BadDebt_Id == BadDebt_Id).ToList();
            }
        }

        #endregion

        #region HistoCustomers [CRUD]

        [OperationContract]
        public void CreateHistoCustomer(HispaniaCompData.HistoCustomer histoCustomer)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.HistoCustomer HistoCustomerToSave = db.HistoCustomers.Add(histoCustomer);
                    db.SaveChanges();
                    histoCustomer.HistoCustomer_Id = HistoCustomerToSave.HistoCustomer_Id;
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar el nou històric de client '{0}' del client '{1}'\r\n{2}.",
                                                histoCustomer.HistoCustomer_Id, histoCustomer.Customer_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.HistoCustomer> ReadHistoCustomers()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.HistoCustomers.ToList();
            }
        }

        [OperationContract]
        public List<HispaniaCompData.HistoCustomer> ReadHistoCustomers(int Customer_Id, bool WithChilds)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                if (WithChilds)
                {
                    List<int> RelatedCustomers = new List<int>();
                    RelatedCustomers.Add(Customer_Id);
                    RelatedCustomers.AddRange(db.RelatedCustomers.Where(x => (x.Customer_Id == Customer_Id)).Select(x => (x.Customer_Canceled_Id)).ToList());
                    return db.HistoCustomers.Where(p => RelatedCustomers.Contains(p.Customer_Id)).OrderByDescending(p => p.HistoCustomer_Id)
                                                                                                 .ThenByDescending(p => p.Bill_Date)
                                                                                                 .ThenByDescending(p => p.DeliveryNote_Date)
                                                                                                 .ThenByDescending(p => p.DeliveryNote_Id)
                                                                                                 .ThenBy(p => p.CustomerOrderMovement_Id).ToList();
                }
                else
                {
                    return db.HistoCustomers.Where(p => p.Customer_Id == Customer_Id).OrderByDescending(p=>p.HistoCustomer_Id)
                                                                                     .ThenByDescending(p => p.Bill_Date)
                                                                                     .ThenByDescending(p => p.DeliveryNote_Date)
                                                                                     .ThenByDescending(p => p.DeliveryNote_Id)
                                                                                     .ThenBy(p => p.CustomerOrderMovement_Id).ToList();
                }
            }
        }

        [OperationContract]
        public void UpdateHistoCustomer(HispaniaCompData.HistoCustomer histoCustomer)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(histoCustomer).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden guardar els canvis fets a l'històric de client.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeleteHistoCustomer(HispaniaCompData.HistoCustomer histoCustomer)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.HistoCustomer HistoCustomerToDelete = db.HistoCustomers.Find(histoCustomer.HistoCustomer_Id);
                    db.HistoCustomers.Remove(HistoCustomerToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar l'historic de client '{0}' del client '{1}'\r\n{2}.",
                                                histoCustomer.HistoCustomer_Id, histoCustomer.Customer_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.HistoCustomer GetHistoCustomer(int HistoCustomer_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.HistoCustomers.Find(HistoCustomer_Id));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar l'històric de client amb identificador '{0}'.\r\n{1}.", HistoCustomer_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region CustomerOrders [CRUD]

        #region CreateCustomerOrder

        [OperationContract]
        public void CreateCustomerOrder(HispaniaCompData.CustomerOrder customerOrder,
                                        Dictionary<DataBaseOp, List<HispaniaCompData.CustomerOrderMovement>> movements)
        {
            string ErrMsg;
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Create the register for the new Customer Order
                            HispaniaCompData.CustomerOrder CustomerOrderToSave = db.CustomerOrders.Add(customerOrder);
                            db.SaveChanges();
                            customerOrder.CustomerOrder_Id = CustomerOrderToSave.CustomerOrder_Id;
                            //  In this case it's only possible creation movements (DatabaseOp = CREATE)
                            HispaniaCompData.Good MovementGood;
                            foreach (HispaniaCompData.CustomerOrderMovement movement in movements[DataBaseOp.CREATE])
                            {
                                movement.CustomerOrder_Id = customerOrder.CustomerOrder_Id;
                                db.CustomerOrderMovements.Add(movement);
                                db.SaveChanges();
                                if (movement.According)
                                {
                                    MovementGood = db.Goods.Find(movement.Good_Id);
                                    MovementGood.Billing_Unit_Available -= movement.Unit_Billing;
                                    MovementGood.Shipping_Unit_Available -= movement.Unit_Shipping;
                                    MovementGood.Billing_Unit_Departure += movement.Unit_Billing;
                                    MovementGood.Shipping_Unit_Departure += movement.Unit_Shipping;
                                    db.Entry(MovementGood).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            ErrMsg = "No es pot guardar la nova Comanda de Client\r\n" +
                                     "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                            throw new Exception(ErrMsg, ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                ErrMsg = "No es pot guardar la nova Comanda de Client" +
                         "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(ErrMsg, ex);
            }
        }

        #endregion

        #region ReadCustomerOrders

        [OperationContract]
        public List<HispaniaCompData.CustomerOrder> ReadCustomerOrders(bool HistoricData = false)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                if (HistoricData)
                {
                    return db.CustomerOrders.ToList();
                }
                else
                {
                    return (db.CustomerOrders.Where(c => c.Date.Value.Year >= DateTime.Now.Year - 1).ToList());
                }
            }
        }

        #endregion

        #region SplitCustomerOrder

        [OperationContract]
        public bool SplitCustomerOrder(HispaniaCompData.CustomerOrder CustomerOrder,
                                       HispaniaCompData.CustomerOrder NewCustomerOrder,
                                       List<HispaniaCompData.CustomerOrderMovement> movements_non_acoording,
                                       out string ErrMsg)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Update The source Customer Order data.
                            db.Entry(CustomerOrder).State = EntityState.Modified;
                            db.SaveChanges();
                            //  Remove movements non according from the original CustomerOrder.
                            HispaniaCompData.CustomerOrderMovement movementToDelete;
                            foreach (HispaniaCompData.CustomerOrderMovement movement in movements_non_acoording)
                            {
                                movementToDelete = db.CustomerOrderMovements.Find(movement.CustomerOrderMovement_Id);
                                db.CustomerOrderMovements.Remove(movementToDelete);
                                db.SaveChanges();
                            }
                            //  Create the register for the new Customer Order
                            HispaniaCompData.CustomerOrder CustomerOrderToSave = db.CustomerOrders.Add(NewCustomerOrder);
                            db.SaveChanges();
                            NewCustomerOrder.CustomerOrder_Id = CustomerOrderToSave.CustomerOrder_Id;
                            //  In this case it's only possible creation movements (DatabaseOp = CREATE)
                            foreach (HispaniaCompData.CustomerOrderMovement movement in movements_non_acoording)
                            {
                                movement.CustomerOrder_Id = NewCustomerOrder.CustomerOrder_Id;
                                db.CustomerOrderMovements.Add(movement);
                                db.SaveChanges();
                            }
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                            //  Indicate correct ends of the operation.
                            ErrMsg = string.Empty;
                            return true;
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            ErrMsg = string.Format("No es pot prepar la Comanda de Client '{0}' per  l'Albarà.\r\n{1}.", CustomerOrder.CustomerOrder_Id,
                                                   "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                            throw new Exception(ErrMsg, ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                ErrMsg = string.Format("No es pot prepar la Comanda de Client '{0}' per  l'Albarà.\r\n{1}.", CustomerOrder.CustomerOrder_Id,
                                       "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(ErrMsg, ex);
            }
        }


        #endregion

        #region UpdateCustomerOrder

        [OperationContract]
        public void UpdateCustomerOrder(HispaniaCompData.CustomerOrder customerOrder)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(customerOrder).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string ErrMsg = string.Format("No es poden guardar els canvis a la Comanda de Client '{0}'\r\nDetalls: {1}.", customerOrder.CustomerOrder_Id,
                                              "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(ErrMsg, ex);
            }
        }

        [OperationContract]
        public void UpdateCustomerOrder(HispaniaCompData.CustomerOrder customerOrder, DateTime newDeliveryNoteDate)
        {
            string ErrMsg;
            try
            {
                //  Exit from this function if Customer Order does not have associated a Delivery Note.
                if ((customerOrder.DeliveryNote_Id is null) || (customerOrder.DeliveryNote_Year is null)) // Delivery Note exist.
                {
                    throw new Exception("Error, no hi ha cap Albarà associat a la Comanda de Client número: {0}.");
                }
                //  Manage Customer Order information if Delivery Note is defined.
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Variables
                            int CO_DeliveryNote_Id = (int)customerOrder.DeliveryNote_Id;
                            decimal CO_DeliveryNote_Year = (decimal)customerOrder.DeliveryNote_Year;
                            //  Get Delivery Note for Update
                            HispaniaCompData.DeliveryNote DeliveryNote = db.DeliveryNotes.Find(new object[] { CO_DeliveryNote_Id, CO_DeliveryNote_Year });
                            //  Step 1 : Determine if Date was changed of year
                            if (DeliveryNote.Year == newDeliveryNoteDate.Year)
                            {
                                //  Step 1.1 : Update DeliveryNote Date in Customer Order.
                                customerOrder.DeliveryNote_Date = newDeliveryNoteDate;
                                UpdateCustomerOrder(db, customerOrder);
                                //  Step 1.2 : Update DeliveryNote Date.
                                DeliveryNote.Date = newDeliveryNoteDate;
                                UpdateDeliveryNote(db, DeliveryNote);
                            }
                            else
                            {
                                //  Step 1.1 : Create a new Delivery Note.
                                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HispaniaComptabilitatConnectionString"].ConnectionString))
                                //using (SqlConnection conn = new SqlConnection(db.Database.Connection.ConnectionString + ";Password=Phispania2"))
                                //using (SqlConnection conn = new SqlConnection(db.Database.Connection.ConnectionString))
                                {
                                    conn.Open();
                                    try
                                    {
                                        string Command = string.Format("INSERT INTO [COMPTABILITAT].[dbo].[DeliveryNote] (DeliveryNote_Id, [Year], [Date], FileNamePDF) " +
                                                                        "VALUES ({0}, '{1}', '{2}', '{3}')",
                                                                        CO_DeliveryNote_Id, newDeliveryNoteDate.Year, newDeliveryNoteDate, DeliveryNote.FileNamePDF);
                                        SqlCommand command_0 = new SqlCommand("SET DATEFORMAT dmy", conn);
                                        command_0.ExecuteNonQuery();
                                        SqlCommand command_1 = new SqlCommand("SET IDENTITY_INSERT [COMPTABILITAT].[dbo].[DeliveryNote] ON", conn);
                                        command_1.ExecuteNonQuery();
                                        SqlCommand command_2 = new SqlCommand(Command, conn);
                                        command_2.ExecuteNonQuery();
                                        SqlCommand command_3 = new SqlCommand("SET IDENTITY_INSERT [COMPTABILITAT].[dbo].[DeliveryNote] OFF", conn);
                                        command_3.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        conn.Close();
                                        throw new Exception(MsgManager.ExcepMsg(ex));
                                    }
                                }
                                //  Step 1.2 : Update DeliveryNote Information in Customer Order.
                                customerOrder.DeliveryNote_Id = CO_DeliveryNote_Id;
                                customerOrder.DeliveryNote_Year = newDeliveryNoteDate.Year;
                                customerOrder.DeliveryNote_Date = newDeliveryNoteDate;
                                UpdateCustomerOrder(db, customerOrder);
                                //  Step 1.3 : Remove old delivery Note.
                                db.DeliveryNotes.Remove(DeliveryNote);
                                db.SaveChanges();
                            }
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            ErrMsg = string.Format("Error, no es pot canviar la data de l'Albarà número '{0}'\r\n{1}.", customerOrder.CustomerOrder_Id,
                                                   "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                            throw new Exception(ErrMsg, ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                ErrMsg = string.Format("No es poden guardar els canvis a l'Albarà número '{0}'\r\n{1}.", customerOrder.CustomerOrder_Id,
                                       "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(ErrMsg, ex);
            }
        }

        [OperationContract]
        private void UpdateCustomerOrder( HispaniaComptabilitat.Data.Entities db, HispaniaCompData.CustomerOrder customerOrder)
        {
            db.Entry(customerOrder).State = EntityState.Modified;
            db.SaveChanges();
        }

        [OperationContract]
        public void UpdateCustomerOrder(HispaniaCompData.CustomerOrder customerOrder, Dictionary<DataBaseOp, List<HispaniaCompData.CustomerOrderMovement>> movements)
        {
            string ErrMsg;
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Variables.
                            int? DeliveryNote_Id = customerOrder.DeliveryNote_Id;
                            decimal? DeliveryNote_Year = customerOrder.DeliveryNote_Year;
                            //  Manage Customer Order information if Delivery Note is defined.
                            if ((DeliveryNote_Id != null) && (DeliveryNote_Year != null)) // Delivery Note exist.
                            {
                                //  Get Delivery Note for Update
                                HispaniaCompData.DeliveryNote DeliveryNote = db.DeliveryNotes.Find(new object[] { DeliveryNote_Id, DeliveryNote_Year });
                                //  Step 1 : Determine if Date was changed of year
                                if (((DateTime)DeliveryNote.Date).Year != ((DateTime)customerOrder.DeliveryNote_Date).Year)
                                {
                                    //  Step 1.1 : Create a new Delivery Note.
                                    int NewDeliveryNote_Year = ((DateTime)customerOrder.DeliveryNote_Date).Year;
                                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HispaniaComptabilitatConnectionString"].ConnectionString))
                                    //using (SqlConnection conn = new SqlConnection(db.Database.Connection.ConnectionString + ";Password=Phispania2"))
                                    //using (SqlConnection conn = new SqlConnection(db.Database.Connection.ConnectionString))
                                    {
                                        conn.Open();
                                        try
                                        {
                                            string Command = string.Format("INSERT INTO [COMPTABILITAT].[dbo].[DeliveryNote] " +
                                                                           "(DeliveryNote_Id, [Year], [Date], FileNamePDF) " +
                                                                           "VALUES ({0}, '{1}', '{2}', '{3}')",
                                                                           customerOrder.DeliveryNote_Id, NewDeliveryNote_Year,
                                                                           customerOrder.DeliveryNote_Date, DeliveryNote.FileNamePDF);
                                            SqlCommand command_0 = new SqlCommand("SET DATEFORMAT dmy", conn);
                                            command_0.ExecuteNonQuery();
                                            SqlCommand command_1 = new SqlCommand("SET IDENTITY_INSERT [COMPTABILITAT].[dbo].[DeliveryNote] ON", conn);
                                            command_1.ExecuteNonQuery();
                                            SqlCommand command_2 = new SqlCommand(Command, conn);
                                            command_2.ExecuteNonQuery();
                                            SqlCommand command_3 = new SqlCommand("SET IDENTITY_INSERT [COMPTABILITAT].[dbo].[DeliveryNote] OFF", conn);
                                            command_3.ExecuteNonQuery();
                                        }
                                        catch (Exception ex)
                                        {
                                            conn.Close();
                                            throw new Exception(MsgManager.ExcepMsg(ex));
                                        }
                                    }
                                    //  Step 1.2 : Update Delivery Note Year of Customer Order.
                                    customerOrder.DeliveryNote_Year = NewDeliveryNote_Year;
                                    //  Step 1.3 : Call at the update method.
                                    UpdateCustomerOrder(db, customerOrder, movements);
                                    //  Step 1.4 : Remove old delivery Note.
                                    db.DeliveryNotes.Remove(DeliveryNote);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    //  Step 1.5 : Call at the update method.
                                    UpdateCustomerOrder(db, customerOrder, movements);
                                    //  Step 1.6 : Set new Date to Delivery Note.
                                    DeliveryNote.Date = customerOrder.DeliveryNote_Date;
                                    //  Step 1.7 : Update Delivery Note.
                                    db.Entry(DeliveryNote).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                            else // Delivery Note don't exist.
                            {
                                //  Call at the update method.
                                UpdateCustomerOrder(db, customerOrder, movements);
                            }
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            ErrMsg = string.Format("No es poden guardar els canvis a la Comanda de Client '{0}'\r\n{1}.", customerOrder.CustomerOrder_Id,
                                                   "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                            throw new Exception(ErrMsg, ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                ErrMsg = string.Format("No es poden guardar els canvis a la Comanda de Client '{0}'\r\n{1}.", customerOrder.CustomerOrder_Id,
                                       "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(ErrMsg, ex);
            }
        }

        [OperationContract]
        private void UpdateCustomerOrder( HispaniaComptabilitat.Data.Entities db,
                                         HispaniaCompData.CustomerOrder customerOrder,
                                         Dictionary<DataBaseOp, List<HispaniaCompData.CustomerOrderMovement>> movements)
        {
            //  Create the register for the new Customer Order
            db.Entry(customerOrder).State = EntityState.Modified;
            db.SaveChanges();
            //  Create list of movements to update
            List<int> MovementsToUpdate = new List<int>();
            foreach (HispaniaCompData.CustomerOrderMovement movementDELETE in movements[DataBaseOp.DELETE])
            {
                foreach (HispaniaCompData.CustomerOrderMovement movementCREATE in movements[DataBaseOp.CREATE])
                {
                    if (movementDELETE.CustomerOrderMovement_Id == movementCREATE.CustomerOrderMovement_Id)
                    {
                        MovementsToUpdate.Add(movementCREATE.CustomerOrderMovement_Id);
                    }
                }
            }
            //  First execute DELETE queries
            HispaniaCompData.Good MovementGood;
            HispaniaCompData.CustomerOrderMovement movementToDelete;
            foreach (HispaniaCompData.CustomerOrderMovement movement in movements[DataBaseOp.DELETE])
            {
                if (!MovementsToUpdate.Contains(movement.CustomerOrderMovement_Id))
                {
                    movementToDelete = db.CustomerOrderMovements.Find(movement.CustomerOrderMovement_Id);
                    db.CustomerOrderMovements.Remove(movementToDelete);
                    db.SaveChanges();
                }
                if (movement.According)
                {
                    MovementGood = db.Goods.Find(movement.Good_Id);
                    MovementGood.Billing_Unit_Available += movement.Unit_Billing;
                    MovementGood.Shipping_Unit_Available += movement.Unit_Shipping;
                    MovementGood.Billing_Unit_Departure -= movement.Unit_Billing;
                    MovementGood.Shipping_Unit_Departure -= movement.Unit_Shipping;
                    db.Entry(MovementGood).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            //  Second execute CREATE queries
            //foreach (HispaniaCompData.CustomerOrderMovement movement in movements[DataBaseOp.CREATE].OrderBy(m => m.CustomerOrderMovement_Id))
            foreach (HispaniaCompData.CustomerOrderMovement movement in movements[DataBaseOp.CREATE])
            {
                if (!MovementsToUpdate.Contains(movement.CustomerOrderMovement_Id)) // CREATE
                {
                    db.CustomerOrderMovements.Add(movement);
                    db.SaveChanges();
                }
                else // UPDATE
                {
                    db.Entry(movement).State = EntityState.Modified;
                    db.SaveChanges();
                }
                if (movement.According)
                {
                    MovementGood = db.Goods.Find(movement.Good_Id);
                    MovementGood.Billing_Unit_Available -= movement.Unit_Billing;
                    MovementGood.Shipping_Unit_Available -= movement.Unit_Shipping;
                    MovementGood.Billing_Unit_Departure += movement.Unit_Billing;
                    MovementGood.Shipping_Unit_Departure += movement.Unit_Shipping;
                    db.Entry(MovementGood).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }

        #endregion

        #region DeleteCustomerOrder

        [OperationContract]
        public void DeleteCustomerOrder(HispaniaCompData.CustomerOrder customerOrder)
        {
            string ErrMsg;
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  First execute DELETE queries for movements.
                            HispaniaCompData.Good MovementGood;
                            HispaniaCompData.CustomerOrderMovement movementToDelete;
                            foreach (HispaniaCompData.CustomerOrderMovement movement in ReadCustomerOrderMovements(customerOrder.CustomerOrder_Id))
                            {
                                movementToDelete = db.CustomerOrderMovements.Find(movement.CustomerOrderMovement_Id);
                                db.CustomerOrderMovements.Remove(movementToDelete);
                                if (movement.According)
                                {
                                    MovementGood = db.Goods.Find(movement.Good_Id);
                                    MovementGood.Billing_Unit_Available += movement.Unit_Billing;
                                    MovementGood.Shipping_Unit_Available += movement.Unit_Shipping;
                                    db.Entry(MovementGood).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                            //  Second execute DELETE query for the Customer Order.
                            HispaniaCompData.CustomerOrder CustomerOrderToDelete = db.CustomerOrders.Find(customerOrder.CustomerOrder_Id);
                            db.CustomerOrders.Remove(CustomerOrderToDelete);
                            db.SaveChanges();
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            ErrMsg = string.Format("No es pot esborrar la Comanda de Client '{0}'\r\n{1}.", customerOrder.CustomerOrder_Id,
                                                   "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                            throw new Exception(ErrMsg, ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                ErrMsg = string.Format("No es pot esborrar la Comanda de Client '{0}'\r\n{1}.", customerOrder.CustomerOrder_Id,
                                       "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(ErrMsg, ex);
            }
        }

        #endregion

        #region GetCustomerOrder

        [OperationContract]
        public HispaniaCompData.CustomerOrder GetCustomerOrder(int CustomerOrder_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.CustomerOrders.Find(CustomerOrder_Id));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar la Comanda de Client amb identificador '{0}'.\r\n{1}.", CustomerOrder_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region GetCustomerOrdersFromBill

        [OperationContract]
        public List<HispaniaCompData.CustomerOrder> GetCustomerOrdersFromBill(int Bill_Id, decimal Bill_Year)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.CustomerOrders.Where(c => c.Bill_Id == Bill_Id && c.Bill_Year == Bill_Year).ToList());
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar les Comandes de Client de la factura amb identificador '{0}'.\r\n{1}.", Bill_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region CreateDeliveryNoteForCustomerOrder

        [OperationContract]
        public void CreateDeliveryNoteForCustomerOrder(HispaniaCompData.CustomerOrder customerOrder,
                                                       HispaniaCompData.DeliveryNote deliveryNote)
        {
            string ErrMsg;
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Create a new DeliveryNote
                            HispaniaCompData.DeliveryNote DeliveryNoteToSave = db.DeliveryNotes.Add(deliveryNote);
                            db.SaveChanges();
                            customerOrder.DeliveryNote_Id = DeliveryNoteToSave.DeliveryNote_Id;
                            customerOrder.DeliveryNote_Year = DeliveryNoteToSave.Year;
                            customerOrder.DeliveryNote_Date = DeliveryNoteToSave.Date;
                            //  Create the register for the new Customer Order
                            db.Entry(customerOrder).State = EntityState.Modified;
                            db.SaveChanges();
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            ErrMsg = string.Format("No es pot crear l'albarà de la Comanda de Client '{0}'\r\n{1}.",
                                                   customerOrder.CustomerOrder_Id,
                                                   "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                            throw new Exception(ErrMsg, ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                ErrMsg = string.Format("No es pot crear l'albarà de la Comanda de Client '{0}'\r\n{1}.",
                                        customerOrder.CustomerOrder_Id,
                                       "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(ErrMsg, ex);
            }
        }

        #endregion

        #region CreateBillForCustomerOrders

        [OperationContract]
        public void CreateBillForCustomerOrders(HispaniaCompData.Bill Bill, HispaniaCompData.Customer Customer,
                                                List<HispaniaCompData.CustomerOrder> CustomerOrdersList,
                                                List<HispaniaCompData.CustomerOrderMovement> MovementsList,
                                                List<HispaniaCompData.HistoCustomer> HistoricMovementsList,
                                                List<HispaniaCompData.Receipt> ReceiptsList,
                                                Dictionary<int, Pair> GoodsAmountValue,
                                                decimal TotalAmount)
        {
            string ErrMsg;
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Create a new Bill
                            HispaniaCompData.Bill BillToSave = db.Bills.Add(Bill);
                            db.SaveChanges();
                            Bill.Bill_Id = BillToSave.Bill_Id;
                            LogDataAccess.Instance.WriteLog("Create Bill DA - Create Bill -> {0}.", Bill.Bill_Id);
                            //  Assign Customer Orders selecteds at the new Bill and marks historic flag at true
                            HispaniaCompData.CustomerOrder CustomerOrderToModify;
                            foreach (HispaniaCompData.CustomerOrder customerOrder in CustomerOrdersList)
                            {
                                CustomerOrderToModify = db.CustomerOrders.Find(customerOrder.CustomerOrder_Id);
                                CustomerOrderToModify.Bill_Id = Bill.Bill_Id;
                                CustomerOrderToModify.Bill_Year = Bill.Year;
                                CustomerOrderToModify.Bill_Serie_Id = Bill.Serie_Id;
                                CustomerOrderToModify.Bill_Date = Bill.Date;
                                CustomerOrderToModify.Historic = true;
                                db.Entry(CustomerOrderToModify).State = EntityState.Modified;
                                db.SaveChanges();
                                LogDataAccess.Instance.WriteLog("Create Bill DA - Update Customer Order -> {0}.", customerOrder.CustomerOrder_Id);
                            }
                            //  Marks the historic flag of the movements at true
                            HispaniaCompData.Good MovementGoodToModify;
                            foreach (HispaniaCompData.CustomerOrderMovement Movement in MovementsList)
                            {
                                HispaniaCompData.CustomerOrderMovement MovementToModify = db.CustomerOrderMovements.Find(Movement.CustomerOrderMovement_Id);
                                MovementToModify.Historic = true;
                                db.Entry(MovementToModify).State = EntityState.Modified;
                                db.SaveChanges();
                                LogDataAccess.Instance.WriteLog("Create Bill DA - Movement -> {0}.", Movement.CustomerOrderMovement_Id);
                                MovementGoodToModify = db.Goods.Find(Movement.Good_Id);
                                MovementGoodToModify.Billing_Unit_Stocks -= Movement.Unit_Billing;
                                MovementGoodToModify.Shipping_Unit_Stocks -= Movement.Unit_Shipping;
                                db.Entry(MovementGoodToModify).State = EntityState.Modified;
                                db.SaveChanges();
                                LogDataAccess.Instance.WriteLog("Create Bill DA - MovementGood -> {0}.", MovementGoodToModify.Good_Code);
                            }
                            //  Add one register at Historic table for each movement of every Customer Order of the Bill
                            foreach (HispaniaCompData.HistoCustomer HistoricMovement in HistoricMovementsList)
                            {
                                HistoricMovement.Bill_Id = Bill.Bill_Id;
                                HistoricMovement.Bill_Year = Bill.Year;
                                HistoricMovement.Bill_Serie_Id = Bill.Serie_Id;
                                HistoricMovement.Bill_Date = Bill.Date;
                                db.HistoCustomers.Add(HistoricMovement);
                                db.SaveChanges();
                                LogDataAccess.Instance.WriteLog("Create Bill DA - HistoricMovement -> {0}.", HistoricMovement.HistoCustomer_Id);
                            }
                            //  Add Receipts Registers
                            foreach (HispaniaCompData.Receipt Receipt in ReceiptsList)
                            {
                                Receipt.Bill_Id = Bill.Bill_Id;
                                Receipt.Bill_Year = Bill.Year;
                                Receipt.Bill_Serie_Id = Bill.Serie_Id;
                                db.Receipts.Add(Receipt);
                                db.SaveChanges();
                                LogDataAccess.Instance.WriteLog("Create Bill DA - Receipt -> {0}.", Receipt.Receipt_Id);
                            }
                            //  Update Customer Risk and Sales Aggregate
                            HispaniaCompData.Customer CustomerToModify = db.Customers.Find(Customer.Customer_Id);
                            ActualizeCustomerAggregate(Bill, TotalAmount, ref CustomerToModify);
                            CustomerToModify.BillingData_CurrentRisk += TotalAmount;
                            db.Entry(CustomerToModify).State = EntityState.Modified;
                            db.SaveChanges();
                            LogDataAccess.Instance.WriteLog("Create Bill DA - CustomerToModify -> {0}.", CustomerToModify.Customer_Id);
                            //  Update Goods Amount Value     
                            foreach (KeyValuePair<int, Pair> GoodAmountInfo in GoodsAmountValue)
                            {
                                HispaniaCompData.Good Good = db.Goods.Find(GoodAmountInfo.Key);
                                ActualizeGoodAggregate(Bill, GoodAmountInfo.Value.Amount, GoodAmountInfo.Value.AmountCost, ref Good);
                                db.Entry(Good).State = EntityState.Modified;
                                db.SaveChanges();
                                LogDataAccess.Instance.WriteLog("Create Bill DA - Good -> {0}.", Good.Good_Code);
                            }
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            ErrMsg = "No es poden associar les factures a les comandes de client seleccionades.\r\n" +
                                     "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                            LogDataAccess.Instance.WriteLog("Create Bill DA - Error -> {0}.\r\nDetalls:{1}.", ErrMsg, MsgManager.ExcepMsg(ex));
                            throw new Exception(ErrMsg, ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                ErrMsg = "No es poden associar les factures a les comandes de client seleccionades.\r\n" +
                         "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                LogDataAccess.Instance.WriteLog("Create Bill DA - Error -> {0}.\r\nDetalls:{1}.", ErrMsg, MsgManager.ExcepMsg(ex));
                throw new Exception(ErrMsg, ex);
            }
        }

        #endregion

        #region RemoveCustomerOrdersFromBill

        [OperationContract]
        public void RemoveCustomerOrdersFromBill(HispaniaCompData.Bill Bill, HispaniaCompData.Customer Customer,
                                                 List<HispaniaCompData.CustomerOrder> CustomerOrdersList,
                                                 List<HispaniaCompData.CustomerOrderMovement> MovementsList,
                                                 List<HispaniaCompData.Receipt> ReceiptsList,
                                                 Dictionary<int, Pair> GoodsAmountValue,
                                                 decimal TotalAmount)
        {
            string ErrMsg;
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Do the operations in Database
                            RemoveCustomerOrdersFromBill(db, Bill, Customer, CustomerOrdersList, MovementsList, ReceiptsList,
                                                         GoodsAmountValue, TotalAmount);
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            ErrMsg = "No es poden retirar les comandes de client seleccionades de la factura.\r\n" +
                                     "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                            throw new Exception(ErrMsg, ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                ErrMsg = "No es poden retirar les comandes de client seleccionades de la factura.\r\n" +
                         "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(ErrMsg, ex);
            }
        }

        private void RemoveCustomerOrdersFromBill( HispaniaComptabilitat.Data.Entities db,
                                                   HispaniaCompData.Bill Bill, HispaniaCompData.Customer Customer,
                                                   List<HispaniaCompData.CustomerOrder> CustomerOrdersList,
                                                   List<HispaniaCompData.CustomerOrderMovement> MovementsList,
                                                   List<HispaniaCompData.Receipt> ReceiptsList,
                                                   Dictionary<int, Pair> GoodsAmountValue,
                                                   decimal TotalAmount)
        {
            //  Update the Bill (usually its not needed but I do for safety)
            db.Entry(Bill).State = EntityState.Modified;
            db.SaveChanges();
            //  Unassign Customer Orders selecteds at the new Bill and marks historic flag at false
            foreach (HispaniaCompData.CustomerOrder customerOrder in CustomerOrdersList)
            {
                customerOrder.Bill_Id = null;
                customerOrder.Bill_Year = null;
                customerOrder.Bill_Serie_Id = null; // customerOrder.Bill_Serie_Id = 1; // Serie_Id
                customerOrder.Bill_Date = null;
                customerOrder.Historic = false;
                db.Entry(customerOrder).State = EntityState.Modified;
                db.SaveChanges();
            }
            //  Marks the historic flag of the movements at false
            List<int> MovementsId = new List<int>(MovementsList.Count);
            HispaniaCompData.Good MovementGood;
            foreach (HispaniaCompData.CustomerOrderMovement Movement in MovementsList)
            {
                MovementsId.Add(Movement.CustomerOrderMovement_Id);
                Movement.Historic = false;
                db.Entry(Movement).State = EntityState.Modified;
                db.SaveChanges();
                if (Movement.According)
                {
                    MovementGood = db.Goods.Find(Movement.Good_Id);
                    MovementGood.Billing_Unit_Stocks += Movement.Unit_Billing;
                    MovementGood.Shipping_Unit_Stocks += Movement.Unit_Shipping;
                    db.Entry(MovementGood).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            //  Remove one register at Historic table for each movement of every Customer Order of the Bill
            List<HispaniaCompData.HistoCustomer>
                    HistoCustomersListToRemove =
                            db.HistoCustomers.Where(h => ((h.Bill_Id == Bill.Bill_Id) &&
                                                          (h.Bill_Year == Bill.Year) &&
                                                          (MovementsId.Contains(h.CustomerOrderMovement_Id)))).ToList();
            db.HistoCustomers.RemoveRange(HistoCustomersListToRemove);
            db.SaveChanges();
            //  Remove Old Receipts
            List<HispaniaCompData.Receipt>
                        ReceiptsListToRemove = db.Receipts.Where(c => ((c.Bill_Id == Bill.Bill_Id) &&
                                                                        (c.Bill_Year == Bill.Year))).ToList();
            db.Receipts.RemoveRange(ReceiptsListToRemove);
            db.SaveChanges();
            //  Add New Receipts Registers
            foreach (HispaniaCompData.Receipt Receipt in ReceiptsList)
            {
                Receipt.Bill_Id = Bill.Bill_Id;
                Receipt.Bill_Year = Bill.Year;
                Receipt.Bill_Serie_Id = Bill.Serie_Id;
                db.Receipts.Add(Receipt);
                db.SaveChanges();
            }
            //  Update Customer Risk and Sales Aggregate
            ActualizeCustomerAggregate(Bill, -TotalAmount, ref Customer);
            Customer.BillingData_CurrentRisk -= TotalAmount;
            db.Entry(Customer).State = EntityState.Modified;
            db.SaveChanges();
            //  Update Goods Amount Value     
            foreach (KeyValuePair<int, Pair> GoodAmountInfo in GoodsAmountValue)
            {
                HispaniaCompData.Good Good = db.Goods.Find(GoodAmountInfo.Key);
                ActualizeGoodAggregate(Bill, -GoodAmountInfo.Value.Amount, -GoodAmountInfo.Value.AmountCost, ref Good);
                db.Entry(Good).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        #endregion

        #region AddCustomerOrdersFromBill

        [OperationContract]
        public void AddCustomerOrdersFromBill(HispaniaCompData.Bill Bill, HispaniaCompData.Customer Customer,
                                              List<HispaniaCompData.CustomerOrder> CustomerOrdersList,
                                              List<HispaniaCompData.CustomerOrderMovement> MovementsList,
                                              List<HispaniaCompData.HistoCustomer> HistoricMovementsList,
                                              List<HispaniaCompData.Receipt> ReceiptsList,
                                              Dictionary<int, Pair> GoodsAmountValue,
                                              decimal TotalAmount)
        {
            string ErrMsg;
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Do the operations in Database
                            AddCustomerOrdersFromBill(db, Bill, Customer, CustomerOrdersList, MovementsList, HistoricMovementsList,
                                                      ReceiptsList, GoodsAmountValue, TotalAmount);
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            ErrMsg = "No es poden associar les comandes de client seleccionades a la factura.\r\n" +
                                     "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                            throw new Exception(ErrMsg, ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                ErrMsg = "No es poden associar les comandes de client seleccionades a la factura.\r\n" +
                         "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(ErrMsg, ex);
            }
        }

        private void AddCustomerOrdersFromBill( HispaniaComptabilitat.Data.Entities db,
                                               HispaniaCompData.Bill Bill, HispaniaCompData.Customer Customer,
                                               List<HispaniaCompData.CustomerOrder> CustomerOrdersList,
                                               List<HispaniaCompData.CustomerOrderMovement> MovementsList,
                                               List<HispaniaCompData.HistoCustomer> HistoricMovementsList,
                                               List<HispaniaCompData.Receipt> ReceiptsList,
                                               Dictionary<int, Pair> GoodsAmountValue,
                                               decimal TotalAmount)
        {
            //  Update the Bill (usually its not needed but I do for safety)
            db.Entry(Bill).State = EntityState.Modified;
            db.SaveChanges();
            //  Assign Customer Orders selecteds at the new Bill and marks historic flag at true
            foreach (HispaniaCompData.CustomerOrder customerOrder in CustomerOrdersList)
            {
                customerOrder.Bill_Id = Bill.Bill_Id;
                customerOrder.Bill_Year = Bill.Year;
                customerOrder.Bill_Serie_Id = Bill.Serie_Id;
                customerOrder.Bill_Date = Bill.Date;
                customerOrder.Historic = true;
                db.Entry(customerOrder).State = EntityState.Modified;
                db.SaveChanges();
            }
            //  Marks the historic flag of the movements at true
            HispaniaCompData.Good MovementGood;
            foreach (HispaniaCompData.CustomerOrderMovement Movement in MovementsList)
            {
                Movement.Historic = true;
                db.Entry(Movement).State = EntityState.Modified;
                db.SaveChanges();
                if (Movement.According)
                {
                    MovementGood = db.Goods.Find(Movement.Good_Id);
                    MovementGood.Billing_Unit_Stocks -= Movement.Unit_Billing;
                    MovementGood.Shipping_Unit_Stocks -= Movement.Unit_Shipping;
                    db.Entry(MovementGood).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            //  Add one register at Historic table for each movement of every Customer Order of the Bill
            foreach (HispaniaCompData.HistoCustomer HistoricMovement in HistoricMovementsList)
            {
                HistoricMovement.Bill_Id = Bill.Bill_Id;
                HistoricMovement.Bill_Year = Bill.Year;
                HistoricMovement.Bill_Serie_Id = Bill.Serie_Id;
                HistoricMovement.Bill_Date = Bill.Date;
                db.HistoCustomers.Add(HistoricMovement);
                db.SaveChanges();
            }
            //  Remove Old Receipts
            List<HispaniaCompData.Receipt>
                        ReceiptsListToRemove = db.Receipts.Where(c => ((c.Bill_Id == Bill.Bill_Id) &&
                                                                        (c.Bill_Year == Bill.Year))).ToList();
            db.Receipts.RemoveRange(ReceiptsListToRemove);
            db.SaveChanges();
            //  Add Receipts Registers
            foreach (HispaniaCompData.Receipt Receipt in ReceiptsList)
            {
                Receipt.Bill_Id = Bill.Bill_Id;
                Receipt.Bill_Year = Bill.Year;
                Receipt.Bill_Serie_Id = Bill.Serie_Id;
                db.Receipts.Add(Receipt);
                db.SaveChanges();
            }
            //  Update Customer Risk and Sales Aggregate
            ActualizeCustomerAggregate(Bill, TotalAmount, ref Customer);
            Customer.BillingData_CurrentRisk += TotalAmount;
            db.Entry(Customer).State = EntityState.Modified;
            db.SaveChanges();
            //  Update Goods Amount Value     
            foreach (KeyValuePair<int, Pair> GoodAmountInfo in GoodsAmountValue)
            {
                HispaniaCompData.Good Good = db.Goods.Find(GoodAmountInfo.Key);
                ActualizeGoodAggregate(Bill, GoodAmountInfo.Value.Amount, GoodAmountInfo.Value.AmountCost, ref Good);
                db.Entry(Good).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        #endregion

        #region GetCustomerOrdersFilteredByCustormerId

        [OperationContract]
        public List<HispaniaCompData.CustomerOrder> GetCustomerOrdersFilteredByCustormerId(int Customer_Id)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return (db.CustomerOrders.Where(c => c.Customer_Id == Customer_Id &&
                                                     c.Bill_Id != -1 && c.Bill_Year != -1 &&
                                                     c.DeliveryNote_Id != -1 && c.DeliveryNote_Year != -1).ToList());
            }
        }

        #endregion

        #region Common Functions

        private void ActualizeCustomerAggregate(HispaniaCompData.Bill Bill, decimal TotalAmount, ref HispaniaCompData.Customer Customer)
        {
            switch (Bill.Date.Value.Month)
            {
                case 1:
                    Customer.SeveralData_Acum_1 += TotalAmount;
                    break;
                case 2:
                    Customer.SeveralData_Acum_2 += TotalAmount;
                    break;
                case 3:
                    Customer.SeveralData_Acum_3 += TotalAmount;
                    break;
                case 4:
                    Customer.SeveralData_Acum_4 += TotalAmount;
                    break;
                case 5:
                    Customer.SeveralData_Acum_5 += TotalAmount;
                    break;
                case 6:
                    Customer.SeveralData_Acum_6 += TotalAmount;
                    break;
                case 7:
                    Customer.SeveralData_Acum_7 += TotalAmount;
                    break;
                case 8:
                    Customer.SeveralData_Acum_8 += TotalAmount;
                    break;
                case 9:
                    Customer.SeveralData_Acum_9 += TotalAmount;
                    break;
                case 10:
                    Customer.SeveralData_Acum_10 += TotalAmount;
                    break;
                case 11:
                    Customer.SeveralData_Acum_11 += TotalAmount;
                    break;
                case 12:
                    Customer.SeveralData_Acum_12 += TotalAmount;
                    break;
            }
        }


        private void ActualizeGoodAggregate(HispaniaCompData.Bill Bill, decimal Amount, decimal AmountCost, ref HispaniaCompData.Good Good)
        {
            switch (Bill.Date.Value.Month)
            {
                case 1:
                    Good.Cumulative_Sales_Cost_1 += AmountCost;
                    Good.Cumulative_Sales_Retail_Price_1 += Amount;
                    break;
                case 2:
                    Good.Cumulative_Sales_Cost_2 += AmountCost;
                    Good.Cumulative_Sales_Retail_Price_2 += Amount;
                    break;
                case 3:
                    Good.Cumulative_Sales_Cost_3 += AmountCost;
                    Good.Cumulative_Sales_Retail_Price_3 += Amount;
                    break;
                case 4:
                    Good.Cumulative_Sales_Cost_4 += AmountCost;
                    Good.Cumulative_Sales_Retail_Price_4 += Amount;
                    break;
                case 5:
                    Good.Cumulative_Sales_Cost_5 += AmountCost;
                    Good.Cumulative_Sales_Retail_Price_5 += Amount;
                    break;
                case 6:
                    Good.Cumulative_Sales_Cost_6 += AmountCost;
                    Good.Cumulative_Sales_Retail_Price_6 += Amount;
                    break;
                case 7:
                    Good.Cumulative_Sales_Cost_7 += AmountCost;
                    Good.Cumulative_Sales_Retail_Price_7 += Amount;
                    break;
                case 8:
                    Good.Cumulative_Sales_Cost_8 += AmountCost;
                    Good.Cumulative_Sales_Retail_Price_8 += Amount;
                    break;
                case 9:
                    Good.Cumulative_Sales_Cost_9 += AmountCost;
                    Good.Cumulative_Sales_Retail_Price_9 += Amount;
                    break;
                case 10:
                    Good.Cumulative_Sales_Cost_10 += AmountCost;
                    Good.Cumulative_Sales_Retail_Price_10 += Amount;
                    break;
                case 11:
                    Good.Cumulative_Sales_Cost_11 += AmountCost;
                    Good.Cumulative_Sales_Retail_Price_11 += Amount;
                    break;
                case 12:
                    Good.Cumulative_Sales_Cost_12 += AmountCost;
                    Good.Cumulative_Sales_Retail_Price_12 += Amount;
                    break;
            }
        }

        #endregion

        #endregion

        #region CustomerOrderMovements [CRUD]

        [OperationContract]
        public void CreateCustomerOrderMovement(HispaniaCompData.CustomerOrderMovement customerOrderMovement)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.CustomerOrderMovement CustomerOrderMovementToSave = db.CustomerOrderMovements.Add(customerOrderMovement);
                    db.SaveChanges();
                    customerOrderMovement.CustomerOrderMovement_Id = CustomerOrderMovementToSave.CustomerOrderMovement_Id;
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar el moviment '{0}'  de la nova Comanda de Client.\r\n{1}.",
                                                customerOrderMovement.CustomerOrderMovement_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.CustomerOrderMovement> ReadCustomerOrderMovements()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.CustomerOrderMovements.ToList();
            }
        }

        [OperationContract]
        public List<HispaniaCompData.CustomerOrderMovement> ReadCustomerOrderMovements(int CustomerOrder_Id)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.CustomerOrderMovements.Where(p => p.CustomerOrder_Id == CustomerOrder_Id).ToList();
            }
        }

        [OperationContract]
        public void UpdateCustomerOrderMovement(HispaniaCompData.CustomerOrderMovement customerOrderMovement)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(customerOrderMovement).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden guardar els canvis fets al moviment de la Comanda de Client.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeleteCustomerOrderMovement(HispaniaCompData.CustomerOrderMovement customerOrderMovement)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.CustomerOrderMovement CustomerOrderMovementToDelete =
                                     db.CustomerOrderMovements.Find(customerOrderMovement.CustomerOrderMovement_Id);
                    db.CustomerOrderMovements.Remove(CustomerOrderMovementToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar el moviment '{0}' de la Comanda de Client.\r\n{1}.",
                                                customerOrderMovement.CustomerOrderMovement_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.CustomerOrderMovement GetCustomerOrderMovement(int CustomerOrderMovement_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.CustomerOrderMovements.Find(CustomerOrderMovement_Id));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar el moviment amb identificador '{0}' de la Comanda de Client.\r\n{1}",
                                                 CustomerOrderMovement_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void UpdateMarkedFlagCustomerOrder(int Bill_From_Id, int Bill_Until_Id, DateTime DateToMark, decimal YearQuery)
        {
            string ErrMsg;
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Save Bill information in Database
                            List<HispaniaCompData.CustomerOrder> CustomerOrdersToMark =
                                db.CustomerOrders.Where(c => ((c.Bill_Id >= Bill_From_Id) && (c.Bill_Id <= Bill_Until_Id) && (c.Bill_Year == YearQuery))).ToList();
                            foreach (HispaniaCompData.CustomerOrder customerOrdersToMark in CustomerOrdersToMark)
                            {
                                customerOrdersToMark.Daily = true;
                                customerOrdersToMark.Daily_Dates = DateToMark;
                                db.Entry(customerOrdersToMark).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            ErrMsg = "No es poden guardar els canvis fets a la comanda de client.\r\n" +
                                     "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                            throw new Exception(ErrMsg, ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                ErrMsg = "No es poden guardar els canvis fets a la Factura.\r\n" +
                         "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(ErrMsg, ex);
            }
        }

        #endregion

        #region HistoProviders [CRUD]

        [OperationContract]
        public void CreateHistoProvider(HispaniaCompData.HistoProvider histoProvider)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.HistoProvider HistoProviderToSave = db.HistoProviders.Add(histoProvider);
                    db.SaveChanges();
                    histoProvider.HistoProvider_Id = HistoProviderToSave.HistoProvider_Id;
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar el nou històric de proveidor '{0}' del Proveidor '{1}'\r\n{2}.",
                                                histoProvider.HistoProvider_Id, histoProvider.Provider_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.HistoProvider> ReadHistoProviders()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.HistoProviders.ToList();
            }
        }

        [OperationContract]
        public List<HispaniaCompData.HistoProvider> ReadHistoProviders(int Provider_Id, bool WithChilds)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                {
                    return db.HistoProviders.Where(p => p.Provider_Id == Provider_Id).OrderByDescending(p => p.Bill_Date)
                                                                                     .ThenByDescending(p => p.ProviderOrderMovement_Id).ToList();
                }
            }
        }

        [OperationContract]
        public void UpdateHistoProvider(HispaniaCompData.HistoProvider histoProvider)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(histoProvider).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden guardar els canvis fets a l'històric de client.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeleteHistoProvider(HispaniaCompData.HistoProvider histoProvider)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.HistoProvider HistoProviderToDelete = db.HistoProviders.Find(histoProvider.HistoProvider_Id);
                    db.HistoProviders.Remove(HistoProviderToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar l'historic de client '{0}' del client '{1}'\r\n{2}.",
                                                histoProvider.HistoProvider_Id, histoProvider.Provider_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.HistoProvider GetHistoProvider(int HistoProvider_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.HistoProviders.Find(HistoProvider_Id));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar l'històric de client amb identificador '{0}'.\r\n{1}.", HistoProvider_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.HistoProvider GetHistoProviderByProviderOrderMovement(int ProviderOrderMovement_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.HistoProviders.Where(x=>x.ProviderOrderMovement_Id == ProviderOrderMovement_Id)).FirstOrDefault();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar l'històric de client amb identificador de linea de pedido '{0}'.\r\n{1}.", ProviderOrderMovement_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeleteHistoProviderMovement(int providerOrderId, int ProviderOrderMovementId)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {  
                    HispaniaCompData.HistoProvider HistoProviderToDelete = (db.HistoProviders.Where(x => x.ProviderOrderMovement_Id == ProviderOrderMovementId)).FirstOrDefault();
                    db.HistoProviders.Remove(HistoProviderToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar l'historic de la comanda '{0}' movimiento '{1}'\r\n{2}.",
                                                providerOrderId, ProviderOrderMovementId,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }
        #endregion

        #region HistoCumulativeProvider [CRUD]

        [OperationContract]
        public void CreateHistoCumulativeProvider(HispaniaCompData.HistoCumulativeProvider histoCumulativeProvider)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.HistoCumulativeProvider HistoCumulativeProviderToSave = db.HistoCumulativeProviders.Add(histoCumulativeProvider);
                    db.SaveChanges();
                    histoCumulativeProvider.Histo_Id = HistoCumulativeProviderToSave.Histo_Id;
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar el nou històric d'acumulats de proveidor '{0}' del proveidor '{1}'\r\n{2}.",
                                                histoCumulativeProvider.Histo_Id, histoCumulativeProvider.Provider_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.HistoCumulativeProvider> ReadHistoCumulativeProviders()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.HistoCumulativeProviders.ToList();
            }
        }

        [OperationContract]
        public List<HispaniaCompData.HistoCumulativeProvider> ReadHistoCumulativeProvider(int Provider_Id)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.HistoCumulativeProviders.Where(p => p.Provider_Id == Provider_Id).ToList();
            }
        }

        [OperationContract]
        public void UpdateHistoCumulativeProvider(HispaniaCompData.HistoCumulativeProvider histoCumulativeProvider)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(histoCumulativeProvider).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden guardar els canvis fets a l'històric d'acumulats de proveidor.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeleteHistoCumulativeProvider(HispaniaCompData.HistoCumulativeProvider histoCumulativeProvider)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.HistoCumulativeProvider HistoCumulativeProviderToDelete = db.HistoCumulativeProviders.Find(histoCumulativeProvider.Histo_Id);
                    db.HistoCumulativeProviders.Remove(HistoCumulativeProviderToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar l'historic d'acumulats de proveidor '{0}' del proveidor '{1}'\r\n{2}.",
                                                histoCumulativeProvider.Histo_Id, histoCumulativeProvider.Provider_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.HistoCumulativeProvider GetHistoCumulativeProvider(int HistoCumulativeProvider_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.HistoCumulativeProviders.Find(HistoCumulativeProvider_Id));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar l'històric d'acumulats de proveidor amb identificador '{0}'.\r\n{1}.", HistoCumulativeProvider_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region ProviderOrders [CRUD]

        #region Create Copy ProviderOrder By Id 

        public ProviderOrder CreateCopyProviderOrderById( int providerOrderId )
        {
            ProviderOrder result = null;

            try
            {
                using(var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using(var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            ProviderOrder order = db.ProviderOrders.Where( i => i.ProviderOrder_Id == providerOrderId )
                                                                    .First();

                            result = order.Clone();

                            db.ProviderOrders.Add( result );
                            
                            db.SaveChanges();

                            transaction.Commit();  
                            
                        } catch(Exception )
                        {
                            result = null;

                            transaction.Rollback();

                            throw;
                        }
                    }
                }

            } catch(Exception ex)
            {
                string message = $"Error craeate copy ProviderOrder by Id ={providerOrderId}";
                throw new Exception( message, ex );
            }

            return result;
        }


        #endregion


        #region CreateProviderOrder

        [OperationContract]
        public void CreateProviderOrder(HispaniaCompData.ProviderOrder providerOrder,
                                        Dictionary<DataBaseOp, List<HispaniaCompData.ProviderOrderMovement>> movements)
        {
            string ErrMsg;
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Create the register for the new Provider Order
                            HispaniaCompData.ProviderOrder ProviderOrderToSave = db.ProviderOrders.Add(providerOrder);
                            db.SaveChanges();
                            providerOrder.ProviderOrder_Id = ProviderOrderToSave.ProviderOrder_Id;
                            //  In this case it's only possible creation movements (DatabaseOp = CREATE)
                            HispaniaCompData.Good MovementGood;
                            foreach (HispaniaCompData.ProviderOrderMovement movement in movements[DataBaseOp.CREATE])
                            {
                                movement.ProviderOrder_Id = providerOrder.ProviderOrder_Id;

                                movement.ProviderOrder = null;

                                db.ProviderOrderMovements.Add(movement);

                                db.SaveChanges();
                                
                                //TODO: tarea 119
                                /*
                                if (movement.According)
                                {
                                    MovementGood = db.Goods.Find(movement.Good_Id);
                                    MovementGood.Billing_Unit_Available -= movement.Unit_Billing;
                                    MovementGood.Shipping_Unit_Available -= movement.Unit_Shipping;
                                    MovementGood.Billing_Unit_Departure += movement.Unit_Billing;
                                    MovementGood.Shipping_Unit_Departure += movement.Unit_Shipping;
                                    db.Entry(MovementGood).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                                */
                            }
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            ErrMsg = "No es pot guardar la nova Comanda de Proveidor\r\n" +
                                     "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                            throw new Exception(ErrMsg, ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                ErrMsg = "No es pot guardar la nova Comanda de Proveidor" +
                         "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(ErrMsg, ex);
            }
        }

        #endregion

        #region ReadProviderOrders

        [OperationContract]
        public List<HispaniaCompData.ProviderOrder> ReadProviderOrders(bool HistoricData = false)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                if (HistoricData)
                {
                    return db.ProviderOrders.ToList();
                }
                else
                {
                    return (db.ProviderOrders.Where(c => c.Date.Value.Year >= DateTime.Now.Year - 1).ToList());
                }
            }
        }

        #endregion

        #region SplitProviderOrder

        [OperationContract]
        public bool SplitProviderOrder(HispaniaCompData.ProviderOrder ProviderOrder,
                                       HispaniaCompData.ProviderOrder NewProviderOrder,
                                       List<HispaniaCompData.ProviderOrderMovement> movements_non_acoording,
                                       out string ErrMsg)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Update The source Provider Order data.
                            db.Entry(ProviderOrder).State = EntityState.Modified;
                            db.SaveChanges();
                            //  Remove movements non according from the original ProviderOrder.
                            HispaniaCompData.ProviderOrderMovement movementToDelete;
                            foreach (HispaniaCompData.ProviderOrderMovement movement in movements_non_acoording)
                            {
                                movementToDelete = db.ProviderOrderMovements.Find(movement.ProviderOrderMovement_Id);
                                db.ProviderOrderMovements.Remove(movementToDelete);
                                db.SaveChanges();
                            }
                            //  Create the register for the new Provider Order
                            HispaniaCompData.ProviderOrder ProviderOrderToSave = db.ProviderOrders.Add(NewProviderOrder);
                            db.SaveChanges();
                            NewProviderOrder.ProviderOrder_Id = ProviderOrderToSave.ProviderOrder_Id;
                            //  In this case it's only possible creation movements (DatabaseOp = CREATE)
                            foreach (HispaniaCompData.ProviderOrderMovement movement in movements_non_acoording)
                            {
                                movement.ProviderOrder_Id = NewProviderOrder.ProviderOrder_Id;
                                db.ProviderOrderMovements.Add(movement);
                                db.SaveChanges();
                            }
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                            //  Indicate correct ends of the operation.
                            ErrMsg = string.Empty;
                            return true;
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            ErrMsg = string.Format("No es pot prepar la Comanda de Proveidor '{0}' per  l'Albarà.\r\n{1}.", ProviderOrder.ProviderOrder_Id,
                                                   "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                            throw new Exception(ErrMsg, ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                ErrMsg = string.Format("No es pot prepar la Comanda de Proveidor '{0}' per  l'Albarà.\r\n{1}.", ProviderOrder.ProviderOrder_Id,
                                       "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(ErrMsg, ex);
            }
        }


        #endregion

        #region UpdateProviderOrder

        [OperationContract]
        public void UpdateProviderOrder(HispaniaCompData.ProviderOrder providerOrder)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(providerOrder).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string ErrMsg = string.Format("No es poden guardar els canvis a la Comanda de Proveidor '{0}'\r\nDetalls: {1}.", providerOrder.ProviderOrder_Id,
                                              "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(ErrMsg, ex);
            }
        }

        [OperationContract]
        public void UpdateProviderOrder(HispaniaCompData.ProviderOrder providerOrder, DateTime newDeliveryNoteDate)
        {
            string ErrMsg;
            try
            {
                //  Exit from this function if Provider Order does not have associated a Delivery Note.
                if ((providerOrder.DeliveryNote_Id is null) || (providerOrder.DeliveryNote_Year is null)) // Delivery Note exist.
                {
                    throw new Exception("Error, no hi ha cap Albarà associat a la Comanda de Proveidor número: {0}.");
                }
                //  Manage Provider Order information if Delivery Note is defined.
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Variables
                            int CO_DeliveryNote_Id = (int)providerOrder.DeliveryNote_Id;
                            decimal CO_DeliveryNote_Year = (decimal)providerOrder.DeliveryNote_Year;
                            //  Get Delivery Note for Update
                            HispaniaCompData.DeliveryNote DeliveryNote = db.DeliveryNotes.Find(new object[] { CO_DeliveryNote_Id, CO_DeliveryNote_Year });
                            //  Step 1 : Determine if Date was changed of year
                            if (DeliveryNote.Year == newDeliveryNoteDate.Year)
                            {
                                //  Step 1.1 : Update DeliveryNote Date in Provider Order.
                                providerOrder.DeliveryNote_Date = newDeliveryNoteDate;
                                UpdateProviderOrder(db, providerOrder);
                                //  Step 1.2 : Update DeliveryNote Date.
                                DeliveryNote.Date = newDeliveryNoteDate;
                                UpdateDeliveryNote(db, DeliveryNote);
                            }
                            else
                            {
                                //  Step 1.1 : Create a new Delivery Note.
                                using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HispaniaComptabilitatConnectionString"].ConnectionString))
                                //using (SqlConnection conn = new SqlConnection(db.Database.Connection.ConnectionString + ";Password=Phispania2"))
                                //using (SqlConnection conn = new SqlConnection(db.Database.Connection.ConnectionString))
                                {
                                    conn.Open();
                                    try
                                    {
                                        string Command = string.Format("INSERT INTO [COMPTABILITAT].[dbo].[DeliveryNote] (DeliveryNote_Id, [Year], [Date], FileNamePDF) " +
                                                                        "VALUES ({0}, '{1}', '{2}', '{3}')",
                                                                        CO_DeliveryNote_Id, newDeliveryNoteDate.Year, newDeliveryNoteDate, DeliveryNote.FileNamePDF);
                                        SqlCommand command_0 = new SqlCommand("SET DATEFORMAT dmy", conn);
                                        command_0.ExecuteNonQuery();
                                        SqlCommand command_1 = new SqlCommand("SET IDENTITY_INSERT [COMPTABILITAT].[dbo].[DeliveryNote] ON", conn);
                                        command_1.ExecuteNonQuery();
                                        SqlCommand command_2 = new SqlCommand(Command, conn);
                                        command_2.ExecuteNonQuery();
                                        SqlCommand command_3 = new SqlCommand("SET IDENTITY_INSERT [COMPTABILITAT].[dbo].[DeliveryNote] OFF", conn);
                                        command_3.ExecuteNonQuery();
                                    }
                                    catch (Exception ex)
                                    {
                                        conn.Close();
                                        throw new Exception(MsgManager.ExcepMsg(ex));
                                    }
                                }
                                //  Step 1.2 : Update DeliveryNote Information in Provider Order.
                                providerOrder.DeliveryNote_Id = CO_DeliveryNote_Id;
                                providerOrder.DeliveryNote_Year = newDeliveryNoteDate.Year;
                                providerOrder.DeliveryNote_Date = newDeliveryNoteDate;
                                UpdateProviderOrder(db, providerOrder);
                                //  Step 1.3 : Remove old delivery Note.
                                db.DeliveryNotes.Remove(DeliveryNote);
                                db.SaveChanges();
                            }
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            ErrMsg = string.Format("Error, no es pot canviar la data de l'Albarà número '{0}'\r\n{1}.", providerOrder.ProviderOrder_Id,
                                                   "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                            throw new Exception(ErrMsg, ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                ErrMsg = string.Format("No es poden guardar els canvis a l'Albarà número '{0}'\r\n{1}.", providerOrder.ProviderOrder_Id,
                                       "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(ErrMsg, ex);
            }
        }

        [OperationContract]
        private void UpdateProviderOrder( HispaniaComptabilitat.Data.Entities db, HispaniaCompData.ProviderOrder providerOrder)
        {
            db.Entry(providerOrder).State = EntityState.Modified;
            db.SaveChanges();
        }

        [OperationContract]
        public void UpdateProviderOrder(HispaniaCompData.ProviderOrder providerOrder, Dictionary<DataBaseOp, List<HispaniaCompData.ProviderOrderMovement>> movements)
        {
            string ErrMsg;
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Variables.
                            int? DeliveryNote_Id = providerOrder.DeliveryNote_Id;
                            decimal? DeliveryNote_Year = providerOrder.DeliveryNote_Year;
                            //  Manage Provider Order information if Delivery Note is defined.
                            if ((DeliveryNote_Id != null) && (DeliveryNote_Year != null)) // Delivery Note exist.
                            {
                                //  Get Delivery Note for Update
                                HispaniaCompData.DeliveryNote DeliveryNote = db.DeliveryNotes.Find(new object[] { DeliveryNote_Id, DeliveryNote_Year });
                                //  Step 1 : Determine if Date was changed of year
                                if (((DateTime)DeliveryNote.Date).Year != ((DateTime)providerOrder.DeliveryNote_Date).Year)
                                {
                                    //  Step 1.1 : Create a new Delivery Note.
                                    int NewDeliveryNote_Year = ((DateTime)providerOrder.DeliveryNote_Date).Year;
                                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HispaniaComptabilitatConnectionString"].ConnectionString))
                                    //using (SqlConnection conn = new SqlConnection(db.Database.Connection.ConnectionString + ";Password=Phispania2"))
                                    //using (SqlConnection conn = new SqlConnection(db.Database.Connection.ConnectionString))
                                    {
                                        conn.Open();
                                        try
                                        {
                                            string Command = string.Format("INSERT INTO [COMPTABILITAT].[dbo].[DeliveryNote] " +
                                                                           "(DeliveryNote_Id, [Year], [Date], FileNamePDF) " +
                                                                           "VALUES ({0}, '{1}', '{2}', '{3}')",
                                                                           providerOrder.DeliveryNote_Id, NewDeliveryNote_Year,
                                                                           providerOrder.DeliveryNote_Date, DeliveryNote.FileNamePDF);
                                            SqlCommand command_0 = new SqlCommand("SET DATEFORMAT dmy", conn);
                                            command_0.ExecuteNonQuery();
                                            SqlCommand command_1 = new SqlCommand("SET IDENTITY_INSERT [COMPTABILITAT].[dbo].[DeliveryNote] ON", conn);
                                            command_1.ExecuteNonQuery();
                                            SqlCommand command_2 = new SqlCommand(Command, conn);
                                            command_2.ExecuteNonQuery();
                                            SqlCommand command_3 = new SqlCommand("SET IDENTITY_INSERT [COMPTABILITAT].[dbo].[DeliveryNote] OFF", conn);
                                            command_3.ExecuteNonQuery();
                                        }
                                        catch (Exception ex)
                                        {
                                            conn.Close();
                                            throw new Exception(MsgManager.ExcepMsg(ex));
                                        }
                                    }
                                    //  Step 1.2 : Update Delivery Note Year of Provider Order.
                                    providerOrder.DeliveryNote_Year = NewDeliveryNote_Year;
                                    //  Step 1.3 : Call at the update method.
                                    UpdateProviderOrder(db, providerOrder, movements);
                                    //  Step 1.4 : Remove old delivery Note.
                                    db.DeliveryNotes.Remove(DeliveryNote);
                                    db.SaveChanges();
                                }
                                else
                                {
                                    //  Step 1.5 : Call at the update method.
                                    UpdateProviderOrder(db, providerOrder, movements);
                                    //  Step 1.6 : Set new Date to Delivery Note.
                                    DeliveryNote.Date = providerOrder.DeliveryNote_Date;
                                    //  Step 1.7 : Update Delivery Note.
                                    db.Entry(DeliveryNote).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                            else // Delivery Note don't exist.
                            {
                                //  Call at the update method.
                                UpdateProviderOrder(db, providerOrder, movements);
                            }
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            ErrMsg = string.Format("No es poden guardar els canvis a la Comanda de Proveidor '{0}'\r\n{1}.", providerOrder.ProviderOrder_Id,
                                                   "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                            throw new Exception(ErrMsg, ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                ErrMsg = string.Format("No es poden guardar els canvis a la Comanda de Proveidor '{0}'\r\n{1}.", providerOrder.ProviderOrder_Id,
                                       "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(ErrMsg, ex);
            }
        }

        [OperationContract]
        private void UpdateProviderOrder( HispaniaComptabilitat.Data.Entities db,
                                         HispaniaCompData.ProviderOrder providerOrder,
                                         Dictionary<DataBaseOp, List<HispaniaCompData.ProviderOrderMovement>> movements)
        {
            //  Create the register for the new Provider Order
            db.Entry(providerOrder).State = EntityState.Modified;
            db.SaveChanges();
            //  Create list of movements to update
            List<int> MovementsToUpdate = new List<int>();
            foreach (HispaniaCompData.ProviderOrderMovement movementDELETE in movements[DataBaseOp.DELETE])
            {
                foreach (HispaniaCompData.ProviderOrderMovement movementCREATE in movements[DataBaseOp.CREATE])
                {
                    if (movementDELETE.ProviderOrderMovement_Id == movementCREATE.ProviderOrderMovement_Id)
                    {
                        MovementsToUpdate.Add(movementCREATE.ProviderOrderMovement_Id);
                    }
                }
            }
            //  First execute DELETE queries
            HispaniaCompData.Good MovementGood;
            HispaniaCompData.ProviderOrderMovement movementToDelete;
            foreach (HispaniaCompData.ProviderOrderMovement movement in movements[DataBaseOp.DELETE])
            {
                if (!MovementsToUpdate.Contains(movement.ProviderOrderMovement_Id))
                {
                    movementToDelete = db.ProviderOrderMovements.Find(movement.ProviderOrderMovement_Id);
                    db.ProviderOrderMovements.Remove(movementToDelete);
                    db.SaveChanges();
                }
                if (movement.According)
                {
                    //TODO: Tarea 119
                    /* MovementGood = db.Goods.Find(movement.Good_Id);
                    MovementGood.Billing_Unit_Available += movement.Unit_Billing;
                    MovementGood.Shipping_Unit_Available += movement.Unit_Shipping;
                    MovementGood.Billing_Unit_Departure -= movement.Unit_Billing;
                    MovementGood.Shipping_Unit_Departure -= movement.Unit_Shipping;
                    db.Entry(MovementGood).State = EntityState.Modified;
                    db.SaveChanges(); */
                }
            }
            //  Second execute CREATE queries
            //foreach (HispaniaCompData.ProviderOrderMovement movement in movements[DataBaseOp.CREATE].OrderBy(m => m.ProviderOrderMovement_Id))
            foreach (HispaniaCompData.ProviderOrderMovement movement in movements[DataBaseOp.CREATE])
            {
                if (!MovementsToUpdate.Contains(movement.ProviderOrderMovement_Id)) // CREATE
                {
                    movement.ProviderOrder = null;
                    db.ProviderOrderMovements.Add(movement);
                    db.SaveChanges();
                }
                else // UPDATE
                {
                    //TODO: Error
                    //db.Entry(movement).State = EntityState.Modified;
                    //TODO: FIX
                    ProviderOrderMovement entity = db.ProviderOrderMovements.Find( movement.ProviderOrderMovement_Id );
                    entity.Update( movement );

                    db.SaveChanges();
                }
                if (movement.According)
                {
                    //TODO: Tarea 119
                    /*
                    MovementGood = db.Goods.Find(movement.Good_Id);
                    MovementGood.Billing_Unit_Available -= movement.Unit_Billing;
                    MovementGood.Shipping_Unit_Available -= movement.Unit_Shipping;
                    MovementGood.Billing_Unit_Departure += movement.Unit_Billing;
                    MovementGood.Shipping_Unit_Departure += movement.Unit_Shipping;
                    db.Entry(MovementGood).State = EntityState.Modified;
                    db.SaveChanges();
                    */
                }
            }
        }

        #endregion

        #region DeleteProviderOrder

        [OperationContract]
        public void DeleteProviderOrder(HispaniaCompData.ProviderOrder providerOrder)
        {
            string ErrMsg;
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  First execute DELETE queries for movements.
                            HispaniaCompData.Good MovementGood;
                            HispaniaCompData.ProviderOrderMovement movementToDelete;
                            foreach (HispaniaCompData.ProviderOrderMovement movement in ReadProviderOrderMovements(providerOrder.ProviderOrder_Id))
                            {
                                movementToDelete = db.ProviderOrderMovements.Find(movement.ProviderOrderMovement_Id);
                                db.ProviderOrderMovements.Remove(movementToDelete);
                                db.Entry(movementToDelete).State = EntityState.Deleted;
                                db.SaveChanges();
                                if (movement.According)
                                {
                                    MovementGood = db.Goods.Find(movement.Good_Id);
                                    MovementGood.Billing_Unit_Available += movement.Unit_Billing;
                                    MovementGood.Shipping_Unit_Available += movement.Unit_Shipping;
                                    db.Entry(MovementGood).State = EntityState.Modified;
                                    db.SaveChanges();
                                }
                            }
                            //  Second execute DELETE query for the Provider Order.
                            HispaniaCompData.ProviderOrder ProviderOrderToDelete = db.ProviderOrders.Find(providerOrder.ProviderOrder_Id);
                            db.ProviderOrders.Remove(ProviderOrderToDelete);
                            db.SaveChanges();
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            ErrMsg = string.Format("No es pot esborrar la Comanda de Proveidor '{0}'\r\n{1}.", providerOrder.ProviderOrder_Id,
                                                   "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                            throw new Exception(ErrMsg, ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                ErrMsg = string.Format("No es pot esborrar la Comanda de Proveidor '{0}'\r\n{1}.", providerOrder.ProviderOrder_Id,
                                       "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(ErrMsg, ex);
            }
        }

        #endregion

        #region GetProviderOrder

        [OperationContract]
        public HispaniaCompData.ProviderOrder GetProviderOrder(int ProviderOrder_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.ProviderOrders.Find(ProviderOrder_Id));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar la Comanda de Proveidor amb identificador '{0}'.\r\n{1}.", ProviderOrder_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region GetProviderOrdersFromBill

        [OperationContract]
        public List<HispaniaCompData.ProviderOrder> GetProviderOrdersFromBill(int Bill_Id, decimal Bill_Year)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.ProviderOrders.Where(c => c.Bill_Id == Bill_Id && c.Bill_Year == Bill_Year).ToList());
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar les Comandes de Proveidor de la factura amb identificador '{0}'.\r\n{1}.", Bill_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region CreateDeliveryNoteForProviderOrder

        [OperationContract]
        public void CreateDeliveryNoteForProviderOrder(HispaniaCompData.ProviderOrder providerOrder,
                                                       HispaniaCompData.DeliveryNote deliveryNote)
        {
            string ErrMsg;
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Create a new DeliveryNote
                            HispaniaCompData.DeliveryNote DeliveryNoteToSave = db.DeliveryNotes.Add(deliveryNote);
                            db.SaveChanges();
                            providerOrder.DeliveryNote_Id = DeliveryNoteToSave.DeliveryNote_Id;
                            providerOrder.DeliveryNote_Year = DeliveryNoteToSave.Year;
                            providerOrder.DeliveryNote_Date = DeliveryNoteToSave.Date;
                            //  Create the register for the new Provider Order
                            db.Entry(providerOrder).State = EntityState.Modified;
                            db.SaveChanges();
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            ErrMsg = string.Format("No es pot crear l'albarà de la Comanda de Client '{0}'\r\n{1}.",
                                                   providerOrder.ProviderOrder_Id,
                                                   "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                            throw new Exception(ErrMsg, ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                ErrMsg = string.Format("No es pot crear l'albarà de la Comanda de Client '{0}'\r\n{1}.",
                                        providerOrder.ProviderOrder_Id,
                                       "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(ErrMsg, ex);
            }
        }

        #endregion

        #region CreateBillForProviderOrders

        [OperationContract]
        public void CreateBillForProviderOrders(HispaniaCompData.Bill Bill, HispaniaCompData.Provider Provider,
                                                List<HispaniaCompData.ProviderOrder> ProviderOrdersList,
                                                List<HispaniaCompData.ProviderOrderMovement> MovementsList,
                                                List<HispaniaCompData.HistoProvider> HistoricMovementsList,
                                                List<HispaniaCompData.Receipt> ReceiptsList,
                                                Dictionary<int, Pair> GoodsAmountValue,
                                                decimal TotalAmount)
        {
            string ErrMsg;
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Create a new Bill
                            HispaniaCompData.Bill BillToSave = db.Bills.Add(Bill);
                            db.SaveChanges();
                            Bill.Bill_Id = BillToSave.Bill_Id;
                            LogDataAccess.Instance.WriteLog("Create Bill DA - Create Bill -> {0}.", Bill.Bill_Id);
                            //  Assign Provider Orders selecteds at the new Bill and marks historic flag at true
                            HispaniaCompData.ProviderOrder ProviderOrderToModify;
                            foreach (HispaniaCompData.ProviderOrder providerOrder in ProviderOrdersList)
                            {
                                ProviderOrderToModify = db.ProviderOrders.Find(providerOrder.ProviderOrder_Id);
                                ProviderOrderToModify.Bill_Id = Bill.Bill_Id;
                                ProviderOrderToModify.Bill_Year = Bill.Year;
                                ProviderOrderToModify.Bill_Serie_Id = Bill.Serie_Id;
                                ProviderOrderToModify.Bill_Date = Bill.Date;
                                ProviderOrderToModify.Historic = true;
                                db.Entry(ProviderOrderToModify).State = EntityState.Modified;
                                db.SaveChanges();
                                LogDataAccess.Instance.WriteLog("Create Bill DA - Update Provider Order -> {0}.", providerOrder.ProviderOrder_Id);
                            }
                            //  Marks the historic flag of the movements at true
                            HispaniaCompData.Good MovementGoodToModify;
                            foreach (HispaniaCompData.ProviderOrderMovement Movement in MovementsList)
                            {
                                HispaniaCompData.ProviderOrderMovement MovementToModify = db.ProviderOrderMovements.Find(Movement.ProviderOrderMovement_Id);
                                MovementToModify.Historic = true;
                                db.Entry(MovementToModify).State = EntityState.Modified;
                                db.SaveChanges();
                                LogDataAccess.Instance.WriteLog("Create Bill DA - Movement -> {0}.", Movement.ProviderOrderMovement_Id);
                                MovementGoodToModify = db.Goods.Find(Movement.Good_Id);
                                MovementGoodToModify.Billing_Unit_Stocks -= Movement.Unit_Billing;
                                MovementGoodToModify.Shipping_Unit_Stocks -= Movement.Unit_Shipping;
                                db.Entry(MovementGoodToModify).State = EntityState.Modified;
                                db.SaveChanges();
                                LogDataAccess.Instance.WriteLog("Create Bill DA - MovementGood -> {0}.", MovementGoodToModify.Good_Code);
                            }
                            //  Add one register at Historic table for each movement of every Provider Order of the Bill
                            foreach (HispaniaCompData.HistoProvider HistoricMovement in HistoricMovementsList)
                            {
                                HistoricMovement.Bill_Id = Bill.Bill_Id;
                                HistoricMovement.Bill_Year = Bill.Year;
                                HistoricMovement.Bill_Serie_Id = Bill.Serie_Id;
                                HistoricMovement.Bill_Date = Bill.Date;
                                db.HistoProviders.Add(HistoricMovement);
                                db.SaveChanges();
                                LogDataAccess.Instance.WriteLog("Create Bill DA - HistoricMovement -> {0}.", HistoricMovement.HistoProvider_Id);
                            }
                            //  Add Receipts Registers
                            foreach (HispaniaCompData.Receipt Receipt in ReceiptsList)
                            {
                                Receipt.Bill_Id = Bill.Bill_Id;
                                Receipt.Bill_Year = Bill.Year;
                                Receipt.Bill_Serie_Id = Bill.Serie_Id;
                                db.Receipts.Add(Receipt);
                                db.SaveChanges();
                                LogDataAccess.Instance.WriteLog("Create Bill DA - Receipt -> {0}.", Receipt.Receipt_Id);
                            }
                            //  Update Provider Risk and Sales Aggregate
                            HispaniaCompData.Provider ProviderToModify = db.Providers.Find(Provider.Provider_Id);
                            ActualizeProviderAggregate(Bill, TotalAmount, ref ProviderToModify);
                            ProviderToModify.BillingData_CurrentRisk += TotalAmount;
                            db.Entry(ProviderToModify).State = EntityState.Modified;
                            db.SaveChanges();
                            LogDataAccess.Instance.WriteLog("Create Bill DA - ProviderToModify -> {0}.", ProviderToModify.Provider_Id);
                            //  Update Goods Amount Value     
                            foreach (KeyValuePair<int, Pair> GoodAmountInfo in GoodsAmountValue)
                            {
                                HispaniaCompData.Good Good = db.Goods.Find(GoodAmountInfo.Key);
                                ActualizeGoodAggregate(Bill, GoodAmountInfo.Value.Amount, GoodAmountInfo.Value.AmountCost, ref Good);
                                db.Entry(Good).State = EntityState.Modified;
                                db.SaveChanges();
                                LogDataAccess.Instance.WriteLog("Create Bill DA - Good -> {0}.", Good.Good_Code);
                            }
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            ErrMsg = "No es poden associar les factures a les comandes de proveidor seleccionades.\r\n" +
                                     "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                            LogDataAccess.Instance.WriteLog("Create Bill DA - Error -> {0}.\r\nDetalls:{1}.", ErrMsg, MsgManager.ExcepMsg(ex));
                            throw new Exception(ErrMsg, ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                ErrMsg = "No es poden associar les factures a les comandes de proveidor seleccionades.\r\n" +
                         "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                LogDataAccess.Instance.WriteLog("Create Bill DA - Error -> {0}.\r\nDetalls:{1}.", ErrMsg, MsgManager.ExcepMsg(ex));
                throw new Exception(ErrMsg, ex);
            }
        }

        [OperationContract]
        public void CreateHistoProvider(List<HispaniaCompData.HistoProvider> HistoricMovementsList)
        {
            string ErrMsg;
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (HispaniaCompData.HistoProvider HistoricMovement in HistoricMovementsList)
                            {                                
                                if (HistoricMovement.HistoProvider_Id<=0)
                                {
                                    db.HistoProviders.Add(HistoricMovement);
                                    db.SaveChanges();
                                    LogDataAccess.Instance.WriteLog("Create HistoricMovement -> {0}.", HistoricMovement.HistoProvider_Id);
                                }                                
                            }
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            ErrMsg = "No es poden crear les historic movements de comandes de proveidor seleccionades.\r\n" +
                                     "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                            LogDataAccess.Instance.WriteLog("Create HistoricMovement Error -> {0}.\r\nDetalls:{1}.", ErrMsg, MsgManager.ExcepMsg(ex));
                            throw new Exception(ErrMsg, ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                ErrMsg = "No es poden crear les historic movements de comandes de proveidor seleccionades.\r\n" +
                         "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                LogDataAccess.Instance.WriteLog("Create HistoricMovement - Error -> {0}.\r\nDetalls:{1}.", ErrMsg, MsgManager.ExcepMsg(ex));
                throw new Exception(ErrMsg, ex);
            }
        }

        #endregion

        #region RemoveProviderOrdersFromBill

        [OperationContract]
        public void RemoveProviderOrdersFromBill(HispaniaCompData.Bill Bill, HispaniaCompData.Provider Provider,
                                                 List<HispaniaCompData.ProviderOrder> ProviderOrdersList,
                                                 List<HispaniaCompData.ProviderOrderMovement> MovementsList,
                                                 List<HispaniaCompData.Receipt> ReceiptsList,
                                                 Dictionary<int, Pair> GoodsAmountValue,
                                                 decimal TotalAmount)
        {
            string ErrMsg;
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Do the operations in Database
                            RemoveProviderOrdersFromBill(db, Bill, Provider, ProviderOrdersList, MovementsList, ReceiptsList,
                                                         GoodsAmountValue, TotalAmount);
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            ErrMsg = "No es poden retirar les comandes de proveidor seleccionades de la factura.\r\n" +
                                     "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                            throw new Exception(ErrMsg, ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                ErrMsg = "No es poden retirar les comandes de proveidor seleccionades de la factura.\r\n" +
                         "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(ErrMsg, ex);
            }
        }

        private void RemoveProviderOrdersFromBill( HispaniaComptabilitat.Data.Entities db,
                                                   HispaniaCompData.Bill Bill, HispaniaCompData.Provider Provider,
                                                   List<HispaniaCompData.ProviderOrder> ProviderOrdersList,
                                                   List<HispaniaCompData.ProviderOrderMovement> MovementsList,
                                                   List<HispaniaCompData.Receipt> ReceiptsList,
                                                   Dictionary<int, Pair> GoodsAmountValue,
                                                   decimal TotalAmount)
        {
            //  Update the Bill (usually its not needed but I do for safety)
            db.Entry(Bill).State = EntityState.Modified;
            db.SaveChanges();
            //  Unassign Provider Orders selecteds at the new Bill and marks historic flag at false
            foreach (HispaniaCompData.ProviderOrder providerOrder in ProviderOrdersList)
            {
                providerOrder.Bill_Id = null;
                providerOrder.Bill_Year = null;
                providerOrder.Bill_Serie_Id = null; // providerOrder.Bill_Serie_Id = 1; // Serie_Id
                providerOrder.Bill_Date = null;
                providerOrder.Historic = false;
                db.Entry(providerOrder).State = EntityState.Modified;
                db.SaveChanges();
            }
            //  Marks the historic flag of the movements at false
            List<int> MovementsId = new List<int>(MovementsList.Count);
            HispaniaCompData.Good MovementGood;
            foreach (HispaniaCompData.ProviderOrderMovement Movement in MovementsList)
            {
                MovementsId.Add(Movement.ProviderOrderMovement_Id);
                Movement.Historic = false;
                db.Entry(Movement).State = EntityState.Modified;
                db.SaveChanges();
                if (Movement.According)
                {
                    MovementGood = db.Goods.Find(Movement.Good_Id);
                    MovementGood.Billing_Unit_Stocks += Movement.Unit_Billing;
                    MovementGood.Shipping_Unit_Stocks += Movement.Unit_Shipping;
                    db.Entry(MovementGood).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            //  Remove one register at Historic table for each movement of every Provider Order of the Bill
            List<HispaniaCompData.HistoProvider>
                    HistoProvidersListToRemove =
                            db.HistoProviders.Where(h => ((h.Bill_Id == Bill.Bill_Id) &&
                                                          (h.Bill_Year == Bill.Year) &&
                                                          (MovementsId.Contains(h.ProviderOrderMovement_Id)))).ToList();
            db.HistoProviders.RemoveRange(HistoProvidersListToRemove);
            db.SaveChanges();
            //  Remove Old Receipts
            List<HispaniaCompData.Receipt>
                        ReceiptsListToRemove = db.Receipts.Where(c => ((c.Bill_Id == Bill.Bill_Id) &&
                                                                        (c.Bill_Year == Bill.Year))).ToList();
            db.Receipts.RemoveRange(ReceiptsListToRemove);
            db.SaveChanges();
            //  Add New Receipts Registers
            foreach (HispaniaCompData.Receipt Receipt in ReceiptsList)
            {
                Receipt.Bill_Id = Bill.Bill_Id;
                Receipt.Bill_Year = Bill.Year;
                Receipt.Bill_Serie_Id = Bill.Serie_Id;
                db.Receipts.Add(Receipt);
                db.SaveChanges();
            }
            //  Update Provider Risk and Sales Aggregate
            ActualizeProviderAggregate(Bill, -TotalAmount, ref Provider);
            Provider.BillingData_CurrentRisk -= TotalAmount;
            db.Entry(Provider).State = EntityState.Modified;
            db.SaveChanges();
            //  Update Goods Amount Value     
            foreach (KeyValuePair<int, Pair> GoodAmountInfo in GoodsAmountValue)
            {
                HispaniaCompData.Good Good = db.Goods.Find(GoodAmountInfo.Key);
                ActualizeGoodAggregate(Bill, -GoodAmountInfo.Value.Amount, -GoodAmountInfo.Value.AmountCost, ref Good);
                db.Entry(Good).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        #endregion

        #region AddProviderOrdersFromBill

        [OperationContract]
        public void AddProviderOrdersFromBill(HispaniaCompData.Bill Bill, HispaniaCompData.Provider Provider,
                                              List<HispaniaCompData.ProviderOrder> ProviderOrdersList,
                                              List<HispaniaCompData.ProviderOrderMovement> MovementsList,
                                              List<HispaniaCompData.HistoProvider> HistoricMovementsList,
                                              List<HispaniaCompData.Receipt> ReceiptsList,
                                              Dictionary<int, Pair> GoodsAmountValue,
                                              decimal TotalAmount)
        {
            string ErrMsg;
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Do the operations in Database
                            AddProviderOrdersFromBill(db, Bill, Provider, ProviderOrdersList, MovementsList, HistoricMovementsList,
                                                      ReceiptsList, GoodsAmountValue, TotalAmount);
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            ErrMsg = "No es poden associar les comandes de proveidor seleccionades a la factura.\r\n" +
                                     "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                            throw new Exception(ErrMsg, ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                ErrMsg = "No es poden associar les comandes de proveidor seleccionades a la factura.\r\n" +
                         "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(ErrMsg, ex);
            }
        }

        private void AddProviderOrdersFromBill( HispaniaComptabilitat.Data.Entities db,
                                               HispaniaCompData.Bill Bill, HispaniaCompData.Provider Provider,
                                               List<HispaniaCompData.ProviderOrder> ProviderOrdersList,
                                               List<HispaniaCompData.ProviderOrderMovement> MovementsList,
                                               List<HispaniaCompData.HistoProvider> HistoricMovementsList,
                                               List<HispaniaCompData.Receipt> ReceiptsList,
                                               Dictionary<int, Pair> GoodsAmountValue,
                                               decimal TotalAmount)
        {
            //  Update the Bill (usually its not needed but I do for safety)
            db.Entry(Bill).State = EntityState.Modified;
            db.SaveChanges();
            //  Assign Provider Orders selecteds at the new Bill and marks historic flag at true
            foreach (HispaniaCompData.ProviderOrder providerOrder in ProviderOrdersList)
            {
                providerOrder.Bill_Id = Bill.Bill_Id;
                providerOrder.Bill_Year = Bill.Year;
                providerOrder.Bill_Serie_Id = Bill.Serie_Id;
                providerOrder.Bill_Date = Bill.Date;
                providerOrder.Historic = true;
                db.Entry(providerOrder).State = EntityState.Modified;
                db.SaveChanges();
            }
            //  Marks the historic flag of the movements at true
            HispaniaCompData.Good MovementGood;
            foreach (HispaniaCompData.ProviderOrderMovement Movement in MovementsList)
            {
                Movement.Historic = true;
                db.Entry(Movement).State = EntityState.Modified;
                db.SaveChanges();
                if (Movement.According)
                {
                    MovementGood = db.Goods.Find(Movement.Good_Id);
                    MovementGood.Billing_Unit_Stocks -= Movement.Unit_Billing;
                    MovementGood.Shipping_Unit_Stocks -= Movement.Unit_Shipping;
                    db.Entry(MovementGood).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            //  Add one register at Historic table for each movement of every Provider Order of the Bill
            foreach (HispaniaCompData.HistoProvider HistoricMovement in HistoricMovementsList)
            {
                HistoricMovement.Bill_Id = Bill.Bill_Id;
                HistoricMovement.Bill_Year = Bill.Year;
                HistoricMovement.Bill_Serie_Id = Bill.Serie_Id;
                HistoricMovement.Bill_Date = Bill.Date;
                db.HistoProviders.Add(HistoricMovement);
                db.SaveChanges();
            }
            //  Remove Old Receipts
            List<HispaniaCompData.Receipt>
                        ReceiptsListToRemove = db.Receipts.Where(c => ((c.Bill_Id == Bill.Bill_Id) &&
                                                                        (c.Bill_Year == Bill.Year))).ToList();
            db.Receipts.RemoveRange(ReceiptsListToRemove);
            db.SaveChanges();
            //  Add Receipts Registers
            foreach (HispaniaCompData.Receipt Receipt in ReceiptsList)
            {
                Receipt.Bill_Id = Bill.Bill_Id;
                Receipt.Bill_Year = Bill.Year;
                Receipt.Bill_Serie_Id = Bill.Serie_Id;
                db.Receipts.Add(Receipt);
                db.SaveChanges();
            }
            //  Update Provider Risk and Sales Aggregate
            ActualizeProviderAggregate(Bill, TotalAmount, ref Provider);
            Provider.BillingData_CurrentRisk += TotalAmount;
            db.Entry(Provider).State = EntityState.Modified;
            db.SaveChanges();
            //  Update Goods Amount Value     
            foreach (KeyValuePair<int, Pair> GoodAmountInfo in GoodsAmountValue)
            {
                HispaniaCompData.Good Good = db.Goods.Find(GoodAmountInfo.Key);
                ActualizeGoodAggregate(Bill, GoodAmountInfo.Value.Amount, GoodAmountInfo.Value.AmountCost, ref Good);
                db.Entry(Good).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        #endregion

        #region GetProviderOrdersFilteredByCustormerId

        [OperationContract]
        public List<HispaniaCompData.ProviderOrder> GetProviderOrdersFilteredByCustormerId(int Provider_Id)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return (db.ProviderOrders.Where(c => c.Provider_Id == Provider_Id &&
                                                     c.Bill_Id != -1 && c.Bill_Year != -1 &&
                                                     c.DeliveryNote_Id != -1 && c.DeliveryNote_Year != -1).ToList());
            }
        }

        #endregion

        #region Common Functions

        private void ActualizeProviderAggregate(HispaniaCompData.Bill Bill, decimal TotalAmount, ref HispaniaCompData.Provider Provider)
        {
            switch (Bill.Date.Value.Month)
            {
                case 1:
                    Provider.SeveralData_Acum_1 += TotalAmount;
                    break;
                case 2:
                    Provider.SeveralData_Acum_2 += TotalAmount;
                    break;
                case 3:
                    Provider.SeveralData_Acum_3 += TotalAmount;
                    break;
                case 4:
                    Provider.SeveralData_Acum_4 += TotalAmount;
                    break;
                case 5:
                    Provider.SeveralData_Acum_5 += TotalAmount;
                    break;
                case 6:
                    Provider.SeveralData_Acum_6 += TotalAmount;
                    break;
                case 7:
                    Provider.SeveralData_Acum_7 += TotalAmount;
                    break;
                case 8:
                    Provider.SeveralData_Acum_8 += TotalAmount;
                    break;
                case 9:
                    Provider.SeveralData_Acum_9 += TotalAmount;
                    break;
                case 10:
                    Provider.SeveralData_Acum_10 += TotalAmount;
                    break;
                case 11:
                    Provider.SeveralData_Acum_11 += TotalAmount;
                    break;
                case 12:
                    Provider.SeveralData_Acum_12 += TotalAmount;
                    break;
            }
        }


        #endregion

        #endregion

        #region ProviderOrderMovements [CRUD]

        [OperationContract]
        public void CreateProviderOrderMovement(HispaniaCompData.ProviderOrderMovement providerOrderMovement)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.ProviderOrderMovement ProviderOrderMovementToSave = db.ProviderOrderMovements.Add(providerOrderMovement);
                    db.SaveChanges();
                    providerOrderMovement.ProviderOrderMovement_Id = ProviderOrderMovementToSave.ProviderOrderMovement_Id;
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar el moviment '{0}'  de la nova Comanda de Proveidor.\r\n{1}.",
                                                providerOrderMovement.ProviderOrderMovement_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.ProviderOrderMovement> ReadProviderOrderMovements()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.ProviderOrderMovements.ToList();
            }
        }

        [OperationContract]
        public List<HispaniaCompData.ProviderOrderMovement> ReadProviderOrderMovements(int ProviderOrder_Id)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                List<HispaniaCompData.ProviderOrderMovement> result = null;
                result =  db.ProviderOrderMovements.Where(p => p.ProviderOrder_Id == ProviderOrder_Id).ToList();
                return result;
            }
        }

        [OperationContract]
        public void UpdateProviderOrderMovement(HispaniaCompData.ProviderOrderMovement providerOrderMovement)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(providerOrderMovement).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden guardar els canvis fets al moviment de la Comanda de Proveidor.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeleteProviderOrderMovement(HispaniaCompData.ProviderOrderMovement providerOrderMovement)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.ProviderOrderMovement ProviderOrderMovementToDelete =
                                     db.ProviderOrderMovements.Find(providerOrderMovement.ProviderOrderMovement_Id);
                    db.ProviderOrderMovements.Remove(ProviderOrderMovementToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar el moviment '{0}' de la Comanda de Proveidor.\r\n{1}.",
                                                providerOrderMovement.ProviderOrderMovement_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.ProviderOrderMovement GetProviderOrderMovement(int ProviderOrderMovement_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.ProviderOrderMovements.Find(ProviderOrderMovement_Id));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar el moviment amb identificador '{0}' de la Comanda de Proveidor.\r\n{1}",
                                                 ProviderOrderMovement_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void UpdateMarkedFlagProviderOrder(int Bill_From_Id, int Bill_Until_Id, DateTime DateToMark, decimal YearQuery)
        {
            string ErrMsg;
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Save Bill information in Database
                            List<HispaniaCompData.ProviderOrder> ProviderOrdersToMark =
                                db.ProviderOrders.Where(c => ((c.Bill_Id >= Bill_From_Id) && (c.Bill_Id <= Bill_Until_Id) && (c.Bill_Year == YearQuery))).ToList();
                            foreach (HispaniaCompData.ProviderOrder providerOrdersToMark in ProviderOrdersToMark)
                            {
                                providerOrdersToMark.Daily = true;
                                providerOrdersToMark.Daily_Dates = DateToMark;
                                db.Entry(providerOrdersToMark).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            ErrMsg = "No es poden guardar els canvis fets a la comanda de proveidor.\r\n" +
                                     "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                            throw new Exception(ErrMsg, ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                ErrMsg = "No es poden guardar els canvis fets a la Factura.\r\n" +
                         "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(ErrMsg, ex);
            }
        }

        #endregion

        #region RelatedProviders [CRUD]

        [OperationContract]
        public void CreateRelatedProvider(HispaniaCompData.RelatedProvider relatedProvider)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.RelatedProviders.Add(relatedProvider);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar la nova relació de Proveidors '{0}-{1}'\r\n{2}.",
                                                relatedProvider.Provider, relatedProvider.Provider_Canceled_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.RelatedProvider> ReadRelatedProviders(int Provider_Id)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.RelatedProviders.Where(d => (d.Provider_Id == Provider_Id)).ToList();
            }
        }

        [OperationContract]
        public void UpdateRelatedProvider(HispaniaCompData.RelatedProvider relatedProvider)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(relatedProvider).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es poden guardar els canvis fets a la relació de Proveidors '{0}-{1}'\r\n{2}.",
                                                relatedProvider.Provider, relatedProvider.Provider_Canceled_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeleteRelatedProvider(HispaniaCompData.RelatedProvider relatedProvider)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.RelatedProvider RelatedProviderToDelete =
                                                        db.RelatedProviders.Find(new object[] { relatedProvider.Provider_Id, relatedProvider.Provider_Canceled_Id });
                    db.RelatedProviders.Remove(RelatedProviderToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar la relació de Proveidors '{0}-{1}'\r\n{2}.",
                                                relatedProvider.Provider, relatedProvider.Provider_Canceled_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.RelatedProvider GetRelatedProvider(int Provider_Id, int Provider_Canceled_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.RelatedProviders.Find(new object[] { Provider_Id, Provider_Canceled_Id }));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar la relació de Proveidors '{0}-{1}'\r\n{2}.", Provider_Id, Provider_Canceled_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region WarehouseMovements [CRUD]

        [OperationContract]
        public void CreateWarehouseMovement(HispaniaCompData.WarehouseMovement warehouseMovement,
                                            HispaniaCompData.Good good = null)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Remove the Warehouse Movement selected
                            HispaniaCompData.WarehouseMovement WarehouseMovementToSave = db.WarehouseMovements.Add(warehouseMovement);
                            db.SaveChanges();
                            warehouseMovement.WarehouseMovement_Id = WarehouseMovementToSave.WarehouseMovement_Id;
                            //  Update good with the new information if is needed
                            if (good != null)
                            {
                                db.Entry(good).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            string ErrMsg = string.Format("No es pot crear el nou moviment de magatzem\r\n{0}.\r\n",
                                                          "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                            throw new Exception(ErrMsg, ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar el moviment de mangatzem '{0}'.\r\n{1}.",
                                                warehouseMovement.WarehouseMovement_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.WarehouseMovement> ReadWarehouseMovements()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.WarehouseMovements.Where(c => c.Date.Value.Year == DateTime.Now.Year).ToList();
            }
        }

        [OperationContract]
        public List<HispaniaCompData.WarehouseMovement> ReadWarehouseMovements(int WarehouseMovement_Id)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.WarehouseMovements.Where(p => p.WarehouseMovement_Id == WarehouseMovement_Id).ToList();
            }
        }

        [OperationContract]
        public void UpdateWarehouseMovement(HispaniaCompData.WarehouseMovement warehouseMovement,
                                            HispaniaCompData.Good goodOld = null,
                                            HispaniaCompData.Good goodUpdated = null)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Remove the Warehouse Movement selected
                            db.Entry(warehouseMovement).State = EntityState.Modified;
                            db.SaveChanges();
                            //  Update good with the new information if is needed
                            if (goodOld != null)
                            {
                                db.Entry(goodOld).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            //  Update good with the new information if is needed
                            if (goodUpdated != null)
                            {
                                db.Entry(goodUpdated).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            string ErrMsg = string.Format("No es pot crear el nou moviment de magatzem\r\n{0}.\r\n",
                                                          "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                            throw new Exception(ErrMsg, ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                string MsgError = "No es poden guardar els canvis fets al moviment de magatzem.\r\n" +
                                  "Intentiu de nou, i si el problema persisteix consulti l'administrador";
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeleteWarehouseMovement(HispaniaCompData.WarehouseMovement warehouseMovement,
                                            HispaniaCompData.Good good = null)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (var dbTransaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            //  Update good with the new information if is needed
                            if (good != null)
                            {
                                db.Entry(good).State = EntityState.Modified;
                                db.SaveChanges();
                            }
                            //  Remove the Warehouse Movement selected
                            HispaniaCompData.WarehouseMovement WarehouseMovementToDelete =
                                             db.WarehouseMovements.Find(warehouseMovement.WarehouseMovement_Id);
                            db.WarehouseMovements.Remove(WarehouseMovementToDelete);
                            db.SaveChanges();
                            //  Accept the operations realised.
                            dbTransaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            dbTransaction.Rollback();
                            string ErrMsg = string.Format("No es pot esborra el moviment de magatzem '{0}'\r\nDetalls: {1}.\r\n",
                                                          warehouseMovement.WarehouseMovement_Id,
                                                          "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                            throw new Exception(ErrMsg, ex);
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar el moviment de magaztem'{0}'.\r\n{1}.",
                                                warehouseMovement.WarehouseMovement_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.WarehouseMovement GetWarehouseMovement(int WarehouseMovement_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.WarehouseMovements.Find(WarehouseMovement_Id));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar el moviment de magatzem amb identificador '{0}'.\r\n{1}.",
                                                 WarehouseMovement_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion

        #region RelatedCustomers [CRUD]

        [OperationContract]
        public void CreateRelatedCustomer(HispaniaCompData.RelatedCustomer relatedCustomer)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.RelatedCustomers.Add(relatedCustomer);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot guardar la nova relació de Clients '{0}-{1}'\r\n{2}.",
                                                relatedCustomer.Customer, relatedCustomer.Customer_Canceled_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public List<HispaniaCompData.RelatedCustomer> ReadRelatedCustomers(int Customer_Id)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.RelatedCustomers.Where(d => (d.Customer_Id == Customer_Id)).ToList();
            }
        }

        [OperationContract]
        public void UpdateRelatedCustomer(HispaniaCompData.RelatedCustomer relatedCustomer)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    db.Entry(relatedCustomer).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es poden guardar els canvis fets a la relació de Clients '{0}-{1}'\r\n{2}.",
                                                relatedCustomer.Customer, relatedCustomer.Customer_Canceled_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public void DeleteRelatedCustomer(HispaniaCompData.RelatedCustomer relatedCustomer)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    HispaniaCompData.RelatedCustomer RelatedCustomerToDelete =
                                                        db.RelatedCustomers.Find(new object[] { relatedCustomer.Customer_Id, relatedCustomer.Customer_Canceled_Id });
                    db.RelatedCustomers.Remove(RelatedCustomerToDelete);
                    db.SaveChanges();
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot esborrar la relació de Clients '{0}-{1}'\r\n{2}.",
                                                relatedCustomer.Customer, relatedCustomer.Customer_Canceled_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        [OperationContract]
        public HispaniaCompData.RelatedCustomer GetRelatedCustomer(int Customer_Id, int Customer_Canceled_Id)
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    return (db.RelatedCustomers.Find(new object[] { Customer_Id, Customer_Canceled_Id }));
                }
            }
            catch (DataException ex)
            {
                string MsgError = string.Format("No es pot trobar la relació de Clients '{0}-{1}'\r\n{2}.", Customer_Id, Customer_Canceled_Id,
                                                "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(MsgError, ex);
            }
        }

        #endregion


        #endregion

        #region Stored Procedures

        #region Revisions [EXECUTE]

        [OperationContract]
        public List<HispaniaCompData.Revisions_Result> ReadRevisions(int FilterOption)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.Revisions(FilterOption).ToList();
            }
        }

        #endregion

        #region DeliveryNoteLines [EXECUTE]

        [OperationContract]
        public List<HispaniaCompData.DeliveryNoteLines_Result> ReadDeliveryNoteLines()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.DeliveryNoteLines().ToList();
            }
        }

        #endregion

        #region InputsOutputs [EXECUTE]

        [OperationContract]
        public List<HispaniaCompData.InputsOutputs_Result> ReadInputOutputs( int Good_Id )
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.InputsOutputs( Good_Id ).ToList();
            }
        }

        #endregion

        #region StockTakings [EXECUTE]

        [OperationContract]
        public List<HispaniaCompData.StockTaking_Result> ReadStockTakings(string Good_Code_From, string Good_Code_Until)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.StockTaking( Good_Code_From, Good_Code_Until ).ToList();
            }
        }

        #endregion

        #region Ranges [EXECUTE]

        [OperationContract]
        public List<HispaniaCompData.Ranges_Result> ReadRanges(string Good_Code_From, string Good_Code_Until,
                                                       string Bill_Id_From, string Bill_Id_Until, decimal YearQuery)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.Ranges(Good_Code_From, Good_Code_Until, Bill_Id_From, Bill_Id_Until, YearQuery).ToList();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public class SPParam
        {
            /// <summary>
            /// 
            /// </summary>
            public string Name
            {
                get;
            }

            /// <summary>
            /// 
            /// </summary>
            public object Value
            {
                get;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="name"></param>
            /// <param name="value"></param>
            /// <exception cref="ArgumentException"></exception>
            public SPParam( string name, object value )
            {
                if(string.IsNullOrWhiteSpace( name ))
                {
                    throw new ArgumentException( $"'{nameof( name )}' cannot be null or whitespace.", nameof( name ) );
                }

                string str = name.Trim();
                if(str[ 0 ] != '@')
                {
                    str = "@" + str;
                }

                this.Name = str;
                this.Value = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public IEnumerable<Tuple<ProviderOrder,DateTime>> GetQueryPaymentForecast( DateTime startDate, DateTime endDate )
        {
            using(var db = new HispaniaComptabilitat.Data.Entities())
            {
                foreach(ProviderOrder order in db.ProviderOrders /*.Where( i=> i.ProviderOrder_Id == 83 )*/ )
                {
                    DateTime? date = ((order.PrevisioLliurament.HasValue && order.PrevisioLliurament.Value)
                                        ? (order.PrevisioLliuramentData.HasValue ? order.PrevisioLliuramentData.Value : (DateTime?)null)
                                        : (order.Date.HasValue ? order.Date.Value : (DateTime?)null));
                    if(date.HasValue)
                    {
                        // Calc date payment
                        DateTime date_payment = date.Value.AddDays( (double)order.DataBank_ExpirationDays );

                        List<decimal> days = BuildDays( order.DataBank_Paydays1,
                                                        order.DataBank_Paydays2,
                                                        order.DataBank_Paydays3 );

                        days = NormalizeDays( date_payment, days );
                        int day = (int)FindDayPayment( days, date_payment.Day );                        
                        date_payment = new DateTime( date_payment.Year, date_payment.Month, day );

                        if(date_payment >= startDate && date_payment <= endDate)
                        {
                            bool? confornes = LiniesProveidorConformes( order.ProviderOrder_Id );
                            if(confornes.HasValue && confornes.Value)
                            {   // conformes
                                yield return new Tuple<ProviderOrder, DateTime>( order, date_payment );
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="days"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        private decimal FindDayPayment( IEnumerable<decimal> days, decimal day )
        {
            decimal result = day;

            if( days.Any() )
            {
                result = days.Where( i => i >= day ).OrderBy( i => i ).FirstOrDefault();
            }
            
            return ( result == 0 ? day : result );
        }

        private List<decimal> NormalizeDays( DateTime date, List<decimal> days )
        {
            int last_day = DateTime.DaysInMonth( date.Year, date.Month );

            List<decimal> result = days.OrderBy( i => i ).ToList();

            // delete days == 0 || days > last_day ( if feb -> delete 30-31 or 29 )
            result = days.Where( d => d != 0 && d < last_day ).ToList();

            // if last element < last_day -> add last_day
            if(result.Count > 0)
            {
                if(result[ result.Count - 1 ] < last_day)
                {
                    result.Add( last_day );
                }
            }           

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="day1"></param>
        /// <param name="day2"></param>
        /// <param name="day3"></param>
        /// <returns></returns>
        private List<decimal> BuildDays( decimal day1, decimal day2, decimal day3 )
        {
            List<decimal> result = new List<decimal>();

            result.Add( day1 );

            if(!result.Contains( day2 ))
            {
                result.Add( day2 );
            }

            if(!result.Contains( day3 ))
            {
                result.Add( day3 );
            }

            return result;
        }

        public IEnumerable<ProviderOrder> GetQueryOrders( DateTime startDate, DateTime endDate, int? articleId, int? providerId )
        {
            using(var db = new HispaniaComptabilitat.Data.Entities())
            {

                var query = db.ProviderOrders.Include( "Provider").Include("PostalCode")
                                             .Include("SendType" ).Include( "ProviderOrderMovements" )
                                             .Where( i => i.Date >= startDate && i.Date <= endDate );

                if(providerId.HasValue)
                {
                    query = query.Where( i => i.Provider_Id == providerId.Value );
                }

                if(articleId.HasValue)
                {
                    query = query.Where( i => i.ProviderOrderMovements.Any( j => j.Good_Id == articleId.Value ) );
                }

                // Sort
                query = query.OrderByDescending( i => i.Date );

                // result;
                return query.ToArray();
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="name"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> ExecuteSP<TEntity>( string name, IEnumerable<SPParam> parameters = null )
        {
            object[] query_parameters = null ;
            string parameters_parte = "";

            if(parameters != null && parameters.Any())
            {
                query_parameters = parameters.Select( i => new SqlParameter( i.Name,
                                                            ( i.Value != null ? i.Value : DBNull.Value  ) ) )
                                                 .ToArray();

                parameters_parte = string.Join( ", ", parameters.Select( i => i.Name ).ToArray() );
            }
            else
            {
                query_parameters = Enumerable.Empty<object>().ToArray();
            }

            string sql_query = $"EXEC {name} {parameters_parte}";
            
            using(var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.Database.SqlQuery<TEntity>( sql_query, query_parameters ).ToArray();
            }
        }

        #endregion

        #region DiaryBandages [EXECUTE]

        [OperationContract]
        public List<HispaniaCompData.DiaryBandages_Result> ReadDiaryBandages(string Bill_Id_From, string Bill_Id_Until, decimal YearQuery)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.DiaryBandages(Bill_Id_From, Bill_Id_Until, YearQuery).ToList();
            }
        }

        #endregion

        #region CustomerSales [EXECUTE]

        [OperationContract]
        public List<HispaniaCompData.CustomerSales_Result> ReadCustomersSales(decimal Upper_Limit_Sales)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.CustomerSales( Upper_Limit_Sales ).ToList();
            }
        }

        #endregion

        #region Settlements [EXECUTE]

        [OperationContract]
        public List<HispaniaCompData.Settlements_Result> ReadSettlements(int? Agent_Id, string Bill_Id_From, string Bill_Id_Until, decimal YearQuery)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                return db.Settlements(Agent_Id, Bill_Id_From, Bill_Id_Until, YearQuery).ToList();
            }
        }

        //TODO: probar

        [OperationContract]
        public int GetLastBillSetteled()
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                int? LastBillSetteled_Id = db.LastBillSetteleds().ToList()[0];
                return ( LastBillSetteled_Id.HasValue ? LastBillSetteled_Id.Value : 0 );
            }
        }

        #endregion

        #region HistoricAcumCustomers [EXECUTE]

        [OperationContract]
        public bool HistoricAcumCustomers(int Year, out string ErrMsg)
        {
            ErrMsg = string.Empty;
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                using (var dbTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //  Return the number of registers afecteds
                        int Result = db.HistoricAcumCustomers( Year );
                        if (Result < 0)
                        {
                            dbTransaction.Rollback();
                            ErrMsg = string.Format("Error, al actualitzar els acumulats de clients.\r\nDetalls:{0}",
                                                   "No s'ha actualitzat cap registre client.");
                        }
                        else dbTransaction.Commit();
                        return (Result >= 0);
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        ErrMsg = string.Format("Error, al actualitzar els acumulats de clients.\r\nDetalls:{0}",
                                               MsgManager.ExcepMsg(ex));
                        return false;
                    }
                }
            }
        }

        #endregion

        #region HistoricAcumGoods [EXECUTE]

        [OperationContract]
        public bool HistoricAcumGoods(int Year, out string ErrMsg)
        {
            ErrMsg = string.Empty;
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                using (var dbTransaction = db.Database.BeginTransaction())
                {
                    try
                    {
                        //  Return the number of registers afecteds
                        int Result = db.HistoricAcumGoods( Year );
                        if (Result < 0)
                        {
                            dbTransaction.Rollback();
                            ErrMsg = string.Format("Error, al actualitzar els acumulats d'articles.\r\nDetalls:{0}",
                                                   "No s'ha actualitzat cap registre client.");
                        }
                        else dbTransaction.Commit();
                        return (Result >= 0);
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();
                        ErrMsg = string.Format("Error, al actualitzar els acumulats d'articles.\r\nDetalls:{0}",
                                               MsgManager.ExcepMsg(ex));
                        return false;
                    }
                }
            }
        }

        #endregion

        #region RemoveWarehouseMovements [EXECUTE]

        [OperationContract]
        public bool RemoveWarehouseMovements(out string ErrMsg)
        {
            ErrMsg = string.Empty;
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                int Result = -1;
                try
                {
                    //  Return the number of registers afecteds
                    Result = db.RemoveWarehouseMovements();
                    if (Result < 0)
                    {
                        ErrMsg = string.Format("Error, a l'esborrar els moviments de magatzem.\r\nDetalls:{0}",
                                               "No s'han esborrat els moviments de magatzem.");
                    }
                }
                catch (Exception ex)
                {
                    ErrMsg = string.Format("Error, a l'esborrar els moviments de magatzem.\r\nDetalls:{0}",
                                            MsgManager.ExcepMsg(ex));
                    Result = -2;
                }
                return (Result >= 0);
            }
        }

        #endregion

        #region LiniesConformes [EXECUTE]

        [OperationContract]
        public bool? LiniesConformes(int CustomerOrder_Id)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                List<HispaniaCompData.LiniesConformes_Result> LiniesConformes = db.LiniesConformes( CustomerOrder_Id ).ToList();
                if (LiniesConformes.Count == 1)
                {
                    return LiniesConformes[0].LiniesConformes == 0 ? false : true;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        #region LiniesProveidorConformes [EXECUTE]

        [OperationContract]
        public bool? LiniesProveidorConformes(int ProviderOrder_Id)
        {
            using (var db = new HispaniaComptabilitat.Data.Entities())
            {
                List<HispaniaCompData.LiniesProveidorConformes_Result> LiniesConformes = db.LiniesProveidorConformes(ProviderOrder_Id).ToList();
                if (LiniesConformes.Count == 1)
                {
                    return LiniesConformes[0].LiniesConformes == 0 ? false : true;
                }
                else
                {
                    return null;
                }
            }
        }

        #endregion

        #region CustomerOrderMovementsComments [EXECUTE]

        [OperationContract]
        public List<string> CustomerOrderMovementsComments(int CustomerOrder_Id)
        {
            using (var db = new HispaniaCompData.Entities())
            {
                return db.CustomerOrderMovementsComments(CustomerOrder_Id).ToList();
            }
        }

        #endregion

        #region ProviderOrderMovementsComments [EXECUTE]

        [OperationContract]
        public List<string> ProviderOrderMovementsComments( int ProviderOrder_Id )
        {
            using( var db = new HispaniaCompData.Entities() )
            {
                return db.ProviderOrderMovementsComments( ProviderOrder_Id ).ToList();
            }
        }

        #endregion

        #endregion

        #region Direct Queries

        [OperationContract]
        public void TransferInitialStocs()
        {
            try
            {
                using (var db = new HispaniaComptabilitat.Data.Entities())
                {
                    using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HispaniaComptabilitatConnectionString"].ConnectionString))
                    //using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["HispaniaComptabilitatEntities"].ConnectionString))  //db.Database.Connection.ConnectionString + ";Password=Phispania2"))
                    //using (SqlConnection conn = new SqlConnection(db.Database.Connection.ConnectionString))
                    {
                        conn.Open();
                        try
                        {
                            SqlCommand command = new SqlCommand("UPDATE [COMPTABILITAT].[dbo].[Good] " +
                                                                "SET Initial = Shipping_Unit_Stocks, Initial_Fact = Billing_Unit_Stocks", conn);
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            conn.Close();
                            throw new Exception(MsgManager.ExcepMsg(ex));
                        }
                    }
                }
            }
            catch (DataException ex)
            {
                string ErrMsg = string.Format("No es poden transferir els estocs inicials.\r\nDetalls: {0}.",
                                              "Intentiu de nou, i si el problema persisteix consulti l'administrador");
                throw new Exception(ErrMsg, ex);
            }
        }

        #endregion
    }
}
