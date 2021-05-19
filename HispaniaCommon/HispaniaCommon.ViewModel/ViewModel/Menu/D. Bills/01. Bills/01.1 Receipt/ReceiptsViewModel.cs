#region Librerias usadas por la clase

using HispaniaCommon.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HispaniaCompData = HispaniaComptabilitat.Data;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Last update Data: 19/09/2016
    /// Descriptión: class that implements Receipt ViewModel of the Applications.
    /// </summary>
    public partial class MaintenanceForViewModel
    {
        #region ViewModel

        public void CreateReceipt(ReceiptsView ReceiptsView)
        {
            HispaniaCompData.Receipt ReceiptToCreate = ReceiptsView.GetReceipt();
            CreateReceiptInDb(ReceiptToCreate);
            ReceiptsView.Receipt_Id = ReceiptToCreate.Receipt_Id;
            RefreshReceipts();
            RefreshBills();
        }
        public void RefreshReceipts() 
        {
            try
            {
                ReceiptsInDb = HispaniaDataAccess.Instance.ReadReceipts();
                _Receipts = new ObservableCollection<ReceiptsView>();
                _ReceiptsInDictionary = new Dictionary<string, ReceiptsView>();
                foreach (HispaniaCompData.Receipt Receipt in ReceiptsInDb)
                {
                    ReceiptsView NewReceiptsView = new ReceiptsView(Receipt);
                    _Receipts.Add(NewReceiptsView);
                    _ReceiptsInDictionary.Add(GetKeyReceiptView(Receipt), NewReceiptsView);
                }
            }
            catch (Exception ex)
            {
                _Receipts = null;
                throw ex;
            }
        }

        private ObservableCollection<ReceiptsView> _Receipts = null;

        public ObservableCollection<ReceiptsView> Receipts
        {
            get
            {
                return _Receipts;
            }
        }

        private Dictionary<string, ReceiptsView> _ReceiptsInDictionary = null;

        public Dictionary<string, ReceiptsView> ReceiptsDict
        {
            get
            {
                return _ReceiptsInDictionary;
            }
        }

        public string GetKeyReceiptView(ReceiptsView ReceiptsView)
        {
            return GetKeyReceiptView(ReceiptsView.Receipt_Id);
        }

        private string GetKeyReceiptView(HispaniaCompData.Receipt Receipt)
        {
            return GetKeyReceiptView(Receipt.Receipt_Id);
        }

        private string GetKeyReceiptView(int Receipt_Id)
        {
            return string.Format("{0}", Receipt_Id);
        }

        public void UpdateReceipt(ReceiptsView ReceiptsView)
        {
            UpdateReceiptInDb(ReceiptsView.GetReceipt());
        }
        public void DeleteReceipt(ReceiptsView ReceiptsView)
        {
            DeleteReceiptInDb(ReceiptsView.GetReceipt());
        }

        public HispaniaCompData.Receipt GetReceipt(int Receipt_Id)
        {
            return GetReceiptInDb(Receipt_Id);
        }

        public ReceiptsView GetReceiptFromDb(int Receipt_Id)
        {
            return Receipt_Id < 0 ? null : new ReceiptsView(GetReceiptInDb(Receipt_Id));
        }

        public ObservableCollection<ReceiptsView> GetReceiptsFromBill(int Bill_Id, decimal Bill_Year)
        {
            try
            {
                ObservableCollection<ReceiptsView> ReceiptsFromBill = new ObservableCollection<ReceiptsView>();
                foreach (HispaniaCompData.Receipt receipt in GetReceiptsFromBillInDb(Bill_Id, Bill_Year))
                {
                    ReceiptsFromBill.Add(new ReceiptsView(receipt));
                }
                return ReceiptsFromBill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region DataBase [CRUD]

        private void CreateReceiptInDb(HispaniaCompData.Receipt Receipt)
        {
            HispaniaDataAccess.Instance.CreateReceipt(Receipt);
        }

        private List<HispaniaCompData.Receipt> _ReceiptsInDb;

        private List<HispaniaCompData.Receipt> ReceiptsInDb
        {
            get
            {
                return (this._ReceiptsInDb);
            }
            set
            {
                this._ReceiptsInDb = value;
            }
        }

        private void UpdateReceiptInDb(HispaniaCompData.Receipt Receipt)
        {
            HispaniaDataAccess.Instance.UpdateReceipt(Receipt);
        }

        private void DeleteReceiptInDb(HispaniaCompData.Receipt Receipt)
        {
            HispaniaDataAccess.Instance.DeleteReceipt(Receipt);
        }

        private HispaniaCompData.Receipt GetReceiptInDb(int Receipts_Id)
        {
            return HispaniaDataAccess.Instance.GetReceipt(Receipts_Id);
        }

        private List<HispaniaCompData.Receipt> GetReceiptsFromBillInDb(int Bill_Id, decimal Bill_Year)
        {
            return HispaniaDataAccess.Instance.GetReceiptsFromBill(Bill_Id, Bill_Year);
        }

        #endregion
    }
}
