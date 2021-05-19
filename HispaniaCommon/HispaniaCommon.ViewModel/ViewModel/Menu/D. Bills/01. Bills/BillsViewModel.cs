#region Librerias usadas por la clase

using HispaniaCommon.DataAccess;
using MBCode.Framework.Managers.Messages;
using MBCode.Framework.Managers.Trace;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HispaniaCompData = HispaniaComptabilitat.Data;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Last update Data: 19/09/2016
    /// Descriptión: class that implements Bill ViewModel of the Applications.
    /// </summary>
    public partial class MaintenanceForViewModel
    {
        #region ViewModel

        public void CreateBill(BillsView BillsView)
        {
            HispaniaCompData.Bill BillToCreate = BillsView.GetBill();
            CreateBillInDb(BillToCreate);
            BillsView.Bill_Id = BillToCreate.Bill_Id;
            RefreshBills();
        }

        /// <summary>
        /// Get the Bills from the Database
        /// </summary>
        /// <param name="Year">If Year value is null we use the current year bills</param>
        /// <param name="Month">Month only take importance if Year is not null</param>
        public void RefreshBills(decimal? Year = null, decimal? Month = null, bool MismatchesReceipts = false) 
        {
            try
            {
                Dictionary<int, List<CustomerOrderMovementsView>> MovementsDict = new Dictionary<int, List<CustomerOrderMovementsView>>();
                if (MismatchesReceipts)
                {
                    BillsInDb = HispaniaDataAccess.Instance.ReadBills(out List<HispaniaCompData.CustomerOrderMovement> Movements, Year, Month);
                    foreach (HispaniaCompData.CustomerOrderMovement Movement in Movements)
                    {
                        int Movement_Id = (int)Movement.CustomerOrder_Id;
                        if (!MovementsDict.ContainsKey(Movement_Id))
                        {
                            MovementsDict.Add(Movement_Id, new List<CustomerOrderMovementsView>());
                        }
                        MovementsDict[Movement_Id].Add(new CustomerOrderMovementsView(Movement));
                    }
                }
                else
                {
                    BillsInDb = HispaniaDataAccess.Instance.ReadBills(Year, Month);
                }
                _Bills = new ObservableCollection<BillsView>();
                _BillsInDictionary = new Dictionary<string, BillsView>();
                foreach (HispaniaCompData.Bill Bill in BillsInDb)
                {
                    BillsView NewBillsView = new BillsView(Bill);
                    if (MismatchesReceipts)
                    {
                        NewBillsView.AmountReceipts = CalculateAmountReceipts(NewBillsView.Receipts);
                        NewBillsView.BillAmount = NewBillsView.TotalAmount; 
                    }
                    _Bills.Add(NewBillsView);
                    _BillsInDictionary.Add(GetKeyBillView(Bill), NewBillsView);
                }
            }
            catch (Exception ex)
            {
                _Bills = null;
                throw ex;
            }
        }

        private decimal CalculateAmountReceipts(ObservableCollection<ReceiptsView> Receipts)
        {
            decimal Amount = 0;
            if (!(Receipts is null))
            {
                foreach (ReceiptsView receipt in Receipts)
                {
                    Amount += GlobalViewModel.GetCurrencyValueFromDecimal(receipt.Amount);
                }
            }
            return (decimal.Parse(string.Format("{0:0.00}", Amount)));
        }

        private ObservableCollection<BillsView> _Bills = null;

        public ObservableCollection<BillsView> Bills
        {
            get
            {
                return _Bills;
            }
        }

        private Dictionary<string, BillsView> _BillsInDictionary = null;

        public Dictionary<string, BillsView> BillsDict
        {
            get
            {
                return _BillsInDictionary;
            }
        }

        public string GetKeyBillView(BillsView BillsView)
        {
            return GetKeyBillView(BillsView.Bill_Id, BillsView.Year);
        }

        private string GetKeyBillView(HispaniaCompData.Bill Bill)
        {
            return GetKeyBillView(Bill.Bill_Id, Bill.Year);
        }

        public string GetKeyBillView(int Bill_Id, decimal Year)
        {
            return string.Format("{0} | {1:00000}", Year, Bill_Id);
        }

        public void UpdateBill(BillsView BillsView, bool MismatchesReceipts = false)
        {
            List<HispaniaCompData.Receipt> Receipts = new List<HispaniaCompData.Receipt>();
            foreach (ReceiptsView ReceiptView in BillsView.Receipts)
            {
                Receipts.Add(ReceiptView.GetReceipt());
            }
            UpdateBillInDb(BillsView.GetBill(), Receipts);
        }

        public void DeleteBill(BillsView BillsView)
        {
            DeleteBillInDb(BillsView.GetBill());
            RefreshBills();
        }

        public BillsView GetBillFromDb(int Bill_Id, decimal Bill_Year)
        {
            if ((Bill_Id < 0) || (Bill_Year == GlobalViewModel.YearInitValue))
            {
                return null;
            }
            else
            {
                return new BillsView(GetBillInDb(Bill_Id, Bill_Year));
            }
        }

        public BillsView GetBillFromDb(BillsView billsView, bool MismatchesReceipts = false)
        {
            BillsView BillInDb = new BillsView(GetBillInDb(billsView.Bill_Id, billsView.Year));
            if (MismatchesReceipts)
            {
                BillInDb.AmountReceipts = CalculateAmountReceipts(BillInDb.Receipts);
                GlobalViewModel.Instance.HispaniaViewModel.CalculateAmountInfo(BillInDb,
                                                                               out decimal GrossAmount,
                                                                               out decimal EarlyPayementDiscountAmount,
                                                                               out decimal TaxableBaseAmount,
                                                                               out decimal IVAAmount,
                                                                               out decimal SurchargeAmount,
                                                                               out decimal TotalAmount);
                BillInDb.BillAmount = TotalAmount;
            }
            return BillInDb;
        }

        public HispaniaCompData.Bill GetBill(int Bills_Id, decimal Bill_Year)
        {
            return GetBillInDb(Bills_Id, Bill_Year);
        }

        public ReverseSortedDictionary<string, decimal> GetBillYears(bool HistoricData = true)
        {
            int Nowadays_Year = DateTime.Now.Year;
            ReverseSortedDictionary<string, decimal> years = new ReverseSortedDictionary<string, decimal>()
            {
                { Nowadays_Year.ToString(), Nowadays_Year }
            };
            if (HistoricData)
            {
                foreach (decimal year in GetBillYearsInDb(HistoricData))
                {
                    if (((int)year) != Nowadays_Year) years.Add(year.ToString(), year);
                }
            }
            return years;
        }

        public int GetLastBill(decimal YearQuery)
        {
            return GetLastBillInDb(YearQuery);
        }

        public int GetLastBillSetteled()
        {
            return GetLastBillSetteledInDb();
        }

        public ObservableCollection<ReceiptsView> GetReceiptsFromBill(BillsView Bill)
        {
            ObservableCollection<ReceiptsView> Receipts = new ObservableCollection<ReceiptsView>();
            foreach (HispaniaCompData.Receipt Receipt in CalculateReceipts(Bill))
            {
                ReceiptsView ReceiptView = new ReceiptsView(Receipt)
                {
                    Receipt_Id = GlobalViewModel.IntIdInitValue,
                    Bill_Id = Bill.Bill_Id,
                    Bill_Year = Bill.Year,
                    Bill_Serie_Id = Bill.BillingSerie.Serie_Id
                };
                Receipts.Add(ReceiptView);
            }
            return Receipts;
        }

        #endregion

        #region DataBase [CRUD]

        private void CreateBillInDb(HispaniaCompData.Bill Bill)
        {
            HispaniaDataAccess.Instance.CreateBill(Bill);
        }

        private List<HispaniaCompData.Bill> _BillsInDb;

        private List<HispaniaCompData.Bill> BillsInDb
        {
            get
            {
                return (this._BillsInDb);
            }
            set
            {
                this._BillsInDb = value;
            }
        }

        private void UpdateBillInDb(HispaniaCompData.Bill Bill, List<HispaniaCompData.Receipt> Receipts)
        {
            HispaniaDataAccess.Instance.UpdateBill(Bill, Receipts);
        }

        private void DeleteBillInDb(HispaniaCompData.Bill Bill)
        {
            HispaniaDataAccess.Instance.DeleteBill(Bill);
        }

        private HispaniaCompData.Bill GetBillInDb(int Bills_Id, decimal Bill_Year)
        {
            return HispaniaDataAccess.Instance.GetBill(Bills_Id, Bill_Year);
        }
        private List<decimal> GetBillYearsInDb(bool HistoricData = false)
        {
            return HispaniaDataAccess.Instance.GetBillYears(HistoricData);
        }
        private int GetLastBillSetteledInDb()
        {
            return HispaniaDataAccess.Instance.GetLastBillSetteled();
        }
        private int GetLastBillInDb(decimal YearQuery)
        {
            return HispaniaDataAccess.Instance.GetLastBill(YearQuery);
        }

        #endregion
    }
}
