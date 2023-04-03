#region Librerias usadas por la clase

using HispaniaCommon.DataAccess;
using MBCode.Framework.Managers.Messages;
using System;
using System.Windows;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HispaniaCompData = HispaniaComptabilitat.Data;
using System.Text;

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

        #region Attributes and Properties

        private ObservableCollection<ProviderOrdersView> _ProviderOrders = null;

        public ObservableCollection<ProviderOrdersView> ProviderOrders
        {
            get
            {
                return _ProviderOrders;
            }
        }

        private Dictionary<string, ProviderOrdersView> _ProviderOrdersInDictionary = null;

        public Dictionary<string, ProviderOrdersView> ProviderOrdersDict
        {
            get
            {
                return _ProviderOrdersInDictionary;
            }
        }

        public string GetKeyProviderOrderView(ProviderOrdersView ProviderOrdersView)
        {
            return GetKeyProviderOrderView(ProviderOrdersView.ProviderOrder_Id);
        }

        private string GetKeyProviderOrderView(HispaniaCompData.ProviderOrder ProviderOrder)
        {
            return GetKeyProviderOrderView(ProviderOrder.ProviderOrder_Id);
        }

        private string GetKeyProviderOrderView(int ProviderOrder_Id)
        {
            return string.Format("{0}", ProviderOrder_Id);
        }

        public ProviderOrdersView GetProviderOrderFromDb(ProviderOrdersView providerOrdersView)
        {
            return new ProviderOrdersView(GetProviderOrderInDb(providerOrdersView.ProviderOrder_Id));
        }

        public HispaniaCompData.ProviderOrder GetProviderOrder(int ProviderOrder_Id)
        {
            return GetProviderOrderInDb(ProviderOrder_Id);
        }

        public ObservableCollection<ProviderOrdersView> GetProviderOrdersFromBill(int Bill_Id, decimal Bill_Year)
        {
            try
            {
                ObservableCollection<ProviderOrdersView> ProviderOrdersFromBill = new ObservableCollection<ProviderOrdersView>();
                foreach (HispaniaCompData.ProviderOrder providerOrder in GetProviderOrdersFromBillInDb(Bill_Id, Bill_Year))
                {
                    ProviderOrdersFromBill.Add(new ProviderOrdersView(providerOrder));
                }
                return ProviderOrdersFromBill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Dictionary<DataBaseOp, List<HispaniaCompData.ProviderOrderMovement>> GetProviderOrderMovements(Guid DataManagementId)
        {
            Dictionary<DataBaseOp, List<HispaniaCompData.ProviderOrderMovement>> movements = new Dictionary<DataBaseOp, List<HispaniaCompData.ProviderOrderMovement>>
            {
                { DataBaseOp.CREATE, new List<HispaniaCompData.ProviderOrderMovement>() },
                { DataBaseOp.UPDATE, new List<HispaniaCompData.ProviderOrderMovement>() },
                { DataBaseOp.DELETE, new List<HispaniaCompData.ProviderOrderMovement>() }
            };
            foreach (KeyValuePair<object, DataBaseOp> movement in GlobalViewModel.Instance.HispaniaViewModel.GetItemsInDataManaged(DataManagementId))
            {
                if (movement.Value != DataBaseOp.READ)
                {
                    movements[movement.Value].Add(((ProviderOrderMovementsView)movement.Key).GetProviderOrderMovement());
                }
            }
            return movements;
        }

        #endregion

        #region CreateProviderOrder

        public void CreateProviderOrder(ProviderOrdersView providerOrdersView, Guid DataManagementId)
        {
            //  Generate the Provider Order Database object to do the operation
                HispaniaCompData.ProviderOrder providerOrderToCreate = providerOrdersView.GetProviderOrder();
                providerOrderToCreate.Date = DateTime.Now;
            //  Generate the List of Provider 
                Dictionary<DataBaseOp, List<HispaniaCompData.ProviderOrderMovement>> movements = GetProviderOrderMovements(DataManagementId);
            //  Create new Provider Order
                CreateProviderOrderInDb(providerOrderToCreate, movements);
            //  Collect the Id of the new Provider Order
                providerOrdersView.ProviderOrder_Id = providerOrderToCreate.ProviderOrder_Id;
        }

        #endregion

        #region UpdateProviderOrder

        public void UpdateProviderOrder(ProviderOrdersView providerOrderView)
        {
            //  Generate the Provider Order Database object to do the operation
                HispaniaCompData.ProviderOrder providerOrderToUpdate = providerOrderView.GetProviderOrder();
            //  Update Provider Order
                UpdateProviderOrderInDb(providerOrderToUpdate);
        }

        #endregion

        #region DeleteProviderOrder

        public void DeleteProviderOrder(ProviderOrdersView ProviderOrderView)
        {

            //  Delete the Provider Order in the Database.
                DeleteProviderOrderInDb(ProviderOrderView.GetProviderOrder());
        }

        #endregion

        #region RefreshProviderOrders

        public void RefreshProviderOrders(bool HistoricData = false)
        {
            try
            {
                ProviderOrdersInDb = HispaniaDataAccess.Instance.ReadProviderOrders(HistoricData);
                _ProviderOrders = new ObservableCollection<ProviderOrdersView>();
                _ProviderOrdersInDictionary = new Dictionary<string, ProviderOrdersView>();
                foreach (HispaniaCompData.ProviderOrder ProviderOrder in ProviderOrdersInDb)
                {
                    ProviderOrdersView NewProviderOrdersView = new ProviderOrdersView(ProviderOrder);
                    _ProviderOrders.Add(NewProviderOrdersView);
                    _ProviderOrdersInDictionary.Add(GetKeyProviderOrderView(ProviderOrder), NewProviderOrdersView);
                }
            }
            catch (Exception ex)
            {
                _ProviderOrders = null;
                throw ex;
            }
        }

        #endregion

        #region CreateDeliveryNoteForProviderOrder

        public bool CreateDeliveryNoteForProviderOrder(ProviderOrdersView providerOrder, out string ErrMsg)
        {
            try
            {
                //  Find the movements for the Provider Order.
                    ObservableCollection<ProviderOrderMovementsView> Movements = GetProviderOrderMovements(providerOrder.ProviderOrder_Id);
                //  Validate movements of Provider Order
                    if (Movements.Count == 0)
                    {
                        ErrMsg = "Error, no es pot crear un Albarà d'una Comanda de Client sense línies de comanda.";
                    }
                    else
                    {
                        bool According = true;
                        foreach (ProviderOrderMovementsView movement in Movements)
                        {
                            According &= movement.According;
                            if (!According) break;
                        }
                        if (!According)
                        {
                            ErrMsg = "Error, no es pot crear un Albarà d'una Comanda de Client que conté línies de comanda no conformes.";
                        }
                        else
                        {
                            //  Generate the Provider Order Database object to do the operation
                                HispaniaCompData.ProviderOrder providerOrderToUpdate = providerOrder.GetProviderOrder();
                            //  Generate the Delivery Note Database object to do the operation
                                HispaniaCompData.DeliveryNote deliveryNoteToCreate = (new DeliveryNotesView()).GetDeliveryNote();
                            //  Create the new Delivery Note and add a reference of it at the Provider Order indicated.
                                CreateDeliveryNoteForProviderOrderInDb(providerOrderToUpdate, deliveryNoteToCreate);
                            //  Collect the Delivery Note information of the Provider Order
                                providerOrder.DeliveryNote_Id = GlobalViewModel.GetIntFromIntIdValue(providerOrderToUpdate.DeliveryNote_Id);
                                providerOrder.DeliveryNote_Year = GlobalViewModel.GetDecimalYearValue(providerOrderToUpdate.DeliveryNote_Year);
                                providerOrder.DeliveryNote_Date = GlobalViewModel.GetDateTimeValue(providerOrderToUpdate.DeliveryNote_Date);
                            //  Indicates that there has not been an error.
                                ErrMsg = string.Empty;
                        }
                    }
            }
            catch (Exception ex)
            {
                ErrMsg = MsgManager.ExcepMsg(ex);
            }
            return (string.IsNullOrEmpty(ErrMsg));
        }

        #endregion

        #region CreateBillForProvider

        public void CreateBillsFromProviderOrders(SortedDictionary<int, DataForProviderBill> SourceProviderOrders)
        {
            foreach (DataForProviderBill ProviderOrderInfo in SourceProviderOrders.Values)
            {
                LogViewModel.Instance.WriteLog("Create Bill - CreateBillForProvider({0}, {1}, {2})",
                                               ProviderOrderInfo.Bill_Date, ProviderOrderInfo.Provider.Provider_Id, ProviderOrderInfo.Movements.Count);
                CreateBillForProvider(ProviderOrderInfo.Bill_Date, ProviderOrderInfo.Provider, ProviderOrderInfo.Movements);
            }
        }

        private void CreateBillForProvider(DateTime Bill_Date, ProvidersView Provider, List<ProviderOrdersView> SourceProviderOrders)
        {
            //  Data Validation
                LogViewModel.Instance.WriteLog("Create Bill - ValidateBillForProvider({0}, {1})", Provider.Provider_Id, SourceProviderOrders.Count);
                if (!ValidateBillForProvider(Provider, SourceProviderOrders)) return;
            //  Steps to Create bill information and save in the Database.
            //  Step 1 : Calculate amount of the new bill.
            //           This amount will be used for update Provider Risk, Month of Year Provider Sales and  for calculate Bill Receipts.
                LogViewModel.Instance.WriteLog("Create Bill - Step 1 - CalculateAmountInfo({0}, {1}, out ...)",  Provider.Provider_Id, SourceProviderOrders.Count);
                CalculateAmountInfo(Provider, SourceProviderOrders, out ObservableCollection<ProviderOrderMovementsView> Movements,
                                    out decimal GrossAmount, out decimal EarlyPayementDiscountAmount, out decimal TaxableBaseAmount,
                                    out decimal IVAAmount, out decimal SurchargeAmount, out decimal TotalAmount);
                foreach (ProviderOrderMovementsView Movement in Movements)
                {
                    if (!Movement.According)
                    {
                        MsgManager.ShowMessage(
                           string.Format("Error, el moviment '{0}' de l'Albarà '{1}' no está confirmat, no es pot generar una factura amb moviments no confirmats.",
                                         Movement.ProviderOrderMovement_Id, Movement.DeliveryNote_Id_Str));
                        return;
                    }
                }
            //  Step 2 : Calculate receipts from the new bill
                LogViewModel.Instance.WriteLog("Create Bill - Step 2 - CalculateReceipts({0}, {1}, {2})", Provider.Provider_Id, TotalAmount, Bill_Date);
                List<HispaniaCompData.Receipt> ReceiptsList = CalculateReceipts(Provider, TotalAmount, Bill_Date);
            //  Step 3 : Calculate amount of Goods
                LogViewModel.Instance.WriteLog("Create Bill - Step 3 - CalculateGoodsAmountValue({0})", Movements.Count);
                Dictionary<int, Pair> GoodsAmountValue = CalculateGoodsAmountValue(Movements);
            //  Step 4 : Create Bill Info from Provider.
                HispaniaCompData.Bill Bill = (new BillsView(Provider)).GetBill();
                Bill.TotalAmount = TotalAmount;
                Bill.Date = Bill_Date;
                StringBuilder sbExpirationDateInfo = new StringBuilder(string.Empty);
                foreach (HispaniaCompData.Receipt receipt in ReceiptsList)
                {
                    sbExpirationDateInfo.AppendFormat("{0} - ", GlobalViewModel.GetStringFromDateTimeValue(receipt.Expiration_Date));
                }
                if (sbExpirationDateInfo.Length > 0)
                {
                    sbExpirationDateInfo = sbExpirationDateInfo.Remove(sbExpirationDateInfo.Length - 3, 3);
                }
                Bill.ExpirationDate = sbExpirationDateInfo.ToString();
                LogViewModel.Instance.WriteLog("Create Bill - Step 4.1 - Bill.ExpirationDate = {0}", Bill.ExpirationDate);
                if (SourceProviderOrders[0].EffectType.EffectType_Id == 6)
                {
                    Bill.DataBank_EffectType_Id = SourceProviderOrders[0].EffectType.EffectType_Id;
                    Bill.DataBank_Bank = SourceProviderOrders[0].DataBank_Bank;
                    Bill.DataBank_BankAddress = SourceProviderOrders[0].DataBank_BankAddress;
                    Bill.DataBank_IBAN_CountryCode = SourceProviderOrders[0].DataBank_IBAN_CountryCode;
                    Bill.DataBank_IBAN_BankCode = SourceProviderOrders[0].DataBank_IBAN_BankCode;
                    Bill.DataBank_IBAN_OfficeCode = SourceProviderOrders[0].DataBank_IBAN_OfficeCode;
                    Bill.DataBank_IBAN_CheckDigits = SourceProviderOrders[0].DataBank_IBAN_CheckDigits;
                    Bill.DataBank_IBAN_AccountNumber = SourceProviderOrders[0].DataBank_IBAN_AccountNumber;
                }
                LogViewModel.Instance.WriteLog("Create Bill - Step 4.2 - EffectType_Id = {0}, Bill.DataBank_Bank = {1}, Bill.DataBank_BankAddress = {2}, IBAN = {3}", 
                                               Bill.DataBank_EffectType_Id, Bill.DataBank_Bank, Bill.DataBank_BankAddress,
                                               string.Format("{0} {1} {2} {3} {4}", Bill.DataBank_IBAN_CountryCode, Bill.DataBank_IBAN_BankCode,
                                                             Bill.DataBank_IBAN_OfficeCode, Bill.DataBank_IBAN_CheckDigits, Bill.DataBank_IBAN_AccountNumber));
            //  Step 5 : Create List of Provider Orders to add at the Bill
                List<HispaniaCompData.ProviderOrder> ProviderOrdersList = new List<HispaniaCompData.ProviderOrder>(SourceProviderOrders.Count);
                foreach (ProviderOrdersView providerOrder in SourceProviderOrders)
                {
                    ProviderOrdersList.Add(providerOrder.GetProviderOrder());
                }
                LogViewModel.Instance.WriteLog("Create Bill - Step 5 - List of Provider Orders to add at the Bill -> {0}", ProviderOrdersList.Count);
            //  Step 6 : Create List of Provider Orders Movements to add at the Bill
                List<HispaniaCompData.ProviderOrderMovement> MovementsList = new List<HispaniaCompData.ProviderOrderMovement>(Movements.Count);
                foreach (ProviderOrderMovementsView Movement in Movements)
                {
                    MovementsList.Add(Movement.GetProviderOrderMovement());
                }
                LogViewModel.Instance.WriteLog("Create Bill - Step 6 - List of Provider Orders Movements to add at the Bill -> {0}", MovementsList.Count);
            //  Step 7 : Create List of Histo Provider to add at the Historic table
                int Historic_Id = -1;
                List<HispaniaCompData.HistoProvider> HistoricMovementsList = new List<HispaniaCompData.HistoProvider>(Movements.Count);
                foreach (ProviderOrderMovementsView Movement in Movements)
                {
                    HistoProvidersView HistoProviderView = new HistoProvidersView(Movement)
                    {
                        HistoProvider_Id = Historic_Id,
                        Good_Description = Movement.Description
                    };
                    Historic_Id--;
                    HistoricMovementsList.Add(HistoProviderView.GetHistoProvider());
                }
                LogViewModel.Instance.WriteLog("Create Bill - Step 7 - List of Histo Provider to add at the Historic table -> {0}", HistoricMovementsList.Count);
            //  Step 8 : Use all information to create the new Bill.
                LogViewModel.Instance.WriteLog("Create Bill - Step 8 - CreateBillForProviderOrdersInDb(Bill, Provider.GetProvider(), ProviderOrdersList, MovementsList, " +
                                                                                                      "HistoricMovementsList, ReceiptsList, GoodsAmountValue, TotalAmount)");
                CreateBillForProviderOrdersInDb(Bill, Provider.GetProvider(), ProviderOrdersList, MovementsList, HistoricMovementsList, ReceiptsList, GoodsAmountValue, 
                                                TotalAmount);
            //  Step 9 : Refresh the list of Provider Orders.
                LogViewModel.Instance.WriteLog("Create Bill - Step 9 - RefreshProviderOrders()");
                RefreshProviderOrders();
        }

        private bool ValidateBillForProvider(ProvidersView Provider, List<ProviderOrdersView> SourceProviderOrders)
        {
            decimal IVAPercent = SourceProviderOrders[0].IVAPercent;
            decimal SurchagePercent = SourceProviderOrders[0].SurchargePercent;
            string Address = SourceProviderOrders[0].Address.ToUpper();
            bool IVAPercentAreEquals = true;
            bool SurchargePercentAreEquals = true;
            bool AddressAreEquals = true;
            foreach (ProviderOrdersView ProviderOrder in SourceProviderOrders)
            {
                if (!ProviderOrder.According)
                {
                    MsgManager.ShowMessage(
                        string.Format("Error, impossible crear una factura pel client '{0}', " +
                                      "ja que no hi confirmació de l'entrega del material per l'albarà '{1}'.",
                                      ProviderOrder.Provider.Provider_Id,
                                      ProviderOrder.DeliveryNote_Id));
                    return false;
                }
                if (ProviderOrder.IVAPercent != IVAPercent)
                {
                    IVAPercentAreEquals = false;
                    break;
                }
                if (ProviderOrder.SurchargePercent != SurchagePercent)
                {
                    SurchargePercentAreEquals = false;
                    break;
                }
                if (ProviderOrder.Address.ToUpper() != Address)
                {
                    AddressAreEquals = false;
                    break;
                }
            }
            string QuestionMsg;
            if (!IVAPercentAreEquals)
            {
                QuestionMsg = 
                        string.Format("No tots els albarans seleccionats del client '{0}' tenen el mateix " +
                                      "Tipus d'IVA. Si es segueix endavant la factura es crearà fent servir " +
                                      "el Tipus d'IVA definit pel client '{1}%'. Vol continuar ?",
                                      SourceProviderOrders[0].Provider_Id_Str,
                                      Provider.BillingData_IVAType.IVAPercent);
                if (MsgManager.ShowQuestion(QuestionMsg) != MessageBoxResult.Yes)
                {
                    MsgManager.ShowMessage(
                        string.Format("Avís, creació de la factura pels albarans del client '{0}' cancel·lada per l'usuari.",
                                      SourceProviderOrders[0].Provider_Id_Str), MsgType.Information);
                    return false;
                }
            }
            if (!SurchargePercentAreEquals)
            {
                QuestionMsg = 
                        string.Format("No tots els albarans seleccionats del client '{0}' tenen el mateix " +
                                      "Tipus de Recàrrec. Si es segueix endavant la factura es crearà fent " + 
                                      "servir el Tipus de Recàrrec definit pel client '{1}'. Vol continuar ?",
                                      SourceProviderOrders[0].Provider_Id_Str,
                                      Provider.BillingData_IVAType.SurchargePercent);
                if (MsgManager.ShowQuestion(QuestionMsg) != MessageBoxResult.Yes)
                {
                    MsgManager.ShowMessage(
                        string.Format("Avís, creació de la factura pels albarans del client '{0}' cancel·lada per l'usuari.",
                                      SourceProviderOrders[0].Provider_Id_Str), MsgType.Information);
                    return false;
                }
            }
            if (!AddressAreEquals)
            {
                QuestionMsg =
                        string.Format("No tots els albarans seleccionats del client '{0}' tenen la mateixa " +
                                      "Adreça d'Enviament. Vol continuar ?",
                                      SourceProviderOrders[0].Provider_Id_Str);
                if (MsgManager.ShowQuestion(QuestionMsg) != MessageBoxResult.Yes)
                {
                    MsgManager.ShowMessage(
                        string.Format("Avís, creació de la factura pels albarans del client '{0}' cancel·lada per l'usuari.",
                                      SourceProviderOrders[0].Provider_Id_Str), MsgType.Information);
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region RemoveProviderOrdersFromBill

        public void RemoveProviderOrdersFromBill(BillsView Bill, ProvidersView Provider, List<ProviderOrdersView> SourceProviderOrders)
        {
            //  Steps to Create bill information and save in the Database.
            //  Step 1 : Calculate amount of the updated bill.
            //           This amount will be used for update Provider Risk, Month of Year Provider Sales and  for calculate Bill Receipts.
                CalculateAmountInfo(Provider, SourceProviderOrders, out ObservableCollection<ProviderOrderMovementsView> Movements,
                                    out decimal GrossAmount, out decimal EarlyPayementDiscountAmount, out decimal TaxableBaseAmount,
                                    out decimal IVAAmount, out decimal SurchargeAmount, out decimal TotalAmount);
            //  Step 2 : Calculate new receipts from the bill
                CalculateAmountInfo(Provider, Bill, out decimal TotalActualAmount);
                DateTime? Bill_Date = null;
                foreach (ProviderOrdersView providerOrder in GetProviderOrdersFromBill(Bill.Bill_Id, Bill.Year))
                {
                    if (!SourceProviderOrders.Contains(providerOrder))
                    {
                        if ((Bill_Date is null) || (providerOrder.Date > Bill_Date))
                        {
                            Bill_Date = providerOrder.Date;
                        }
                    }
                }
                Bill.Date = (DateTime)Bill_Date;
                List<HispaniaCompData.Receipt> ReceiptsList = CalculateReceipts(Provider, TotalActualAmount - TotalAmount, Bill.Date);
            //  Step 3 : Calculate acumulate of Goods
                Dictionary<int, Pair> GoodsAmountValue = CalculateGoodsAmountValue(Movements);
            //  Step 4 : Create List of Provider Orders to remove from the Bill
                List<HispaniaCompData.ProviderOrder> ProviderOrdersList = new List<HispaniaCompData.ProviderOrder>(SourceProviderOrders.Count);
                foreach (ProviderOrdersView providerOrder in SourceProviderOrders)
                {
                    ProviderOrdersList.Add(providerOrder.GetProviderOrder());
                }
            //  Step 5 : Create List of Provider Orders Movements to remove from the Bill
                List<HispaniaCompData.ProviderOrderMovement> MovementsList = new List<HispaniaCompData.ProviderOrderMovement>(Movements.Count);
                foreach (ProviderOrderMovementsView Movement in Movements)
                {
                    MovementsList.Add(Movement.GetProviderOrderMovement());
                }
            //  Step 6 : Use all information to create the new Bill.
                HispaniaCompData.Bill BillToSave = Bill.GetBill();
                BillToSave.TotalAmount = TotalActualAmount - TotalAmount;
                StringBuilder sbExpirationDateInfo = new StringBuilder(string.Empty);
                foreach (HispaniaCompData.Receipt receipt in ReceiptsList)
                {
                    sbExpirationDateInfo.AppendFormat("{0} - ", GlobalViewModel.GetStringFromDateTimeValue(receipt.Expiration_Date));
                }
                if (sbExpirationDateInfo.Length > 0)
                {
                    sbExpirationDateInfo = sbExpirationDateInfo.Remove(sbExpirationDateInfo.Length - 3, 3);
                }
                BillToSave.Date = Bill.Date;
                BillToSave.ExpirationDate = sbExpirationDateInfo.ToString();
                RemoveProviderOrdersFromBillInDb(BillToSave, Provider.GetProvider(), ProviderOrdersList, MovementsList, 
                                                 ReceiptsList, GoodsAmountValue, TotalAmount);
            //  Step 7 : Refresh the list of Provider Orders.
                RefreshProviderOrders();
        }
        
        #endregion

        #region AddProviderOrdersFromBill

        public void AddProviderOrdersFromBill(BillsView Bill, ProvidersView Provider, List<ProviderOrdersView> SourceProviderOrders)
        {
            //  Steps to Create bill information and save in the Database.
            //  Step 1 : Calculate amount of the updated bill.
            //           This amount will be used for update Provider Risk, Month of Year Provider Sales and  for calculate Bill Receipts.
                CalculateAmountInfo(Provider, SourceProviderOrders, out ObservableCollection<ProviderOrderMovementsView> Movements,
                                    out decimal GrossAmount, out decimal EarlyPayementDiscountAmount, out decimal TaxableBaseAmount,
                                    out decimal IVAAmount, out decimal SurchargeAmount, out decimal TotalAmount);
            //  Step 2 : Calculate new receipts from the bill
                CalculateAmountInfo(Provider, Bill, out decimal TotalActualAmount);
                foreach (ProviderOrdersView providerOrder in SourceProviderOrders)
                {
                    if (providerOrder.DeliveryNote_Date > Bill.Date) Bill.Date = providerOrder.DeliveryNote_Date;
                }
                List<HispaniaCompData.Receipt> ReceiptsList = CalculateReceipts(Provider, TotalActualAmount + TotalAmount, Bill.Date);
            //  Step 3 : Calculate acumulate of Goods
                Dictionary<int, Pair> GoodsAmountValue = CalculateGoodsAmountValue(Movements);
            //  Step 4 : Create List of Provider Orders to Add from the Bill
                List<HispaniaCompData.ProviderOrder> ProviderOrdersList = new List<HispaniaCompData.ProviderOrder>(SourceProviderOrders.Count);
                foreach (ProviderOrdersView providerOrder in SourceProviderOrders)
                {
                    ProviderOrdersList.Add(providerOrder.GetProviderOrder());
                }
            //  Step 5 : Create List of Provider Orders Movements to Add from the Bill
                List<HispaniaCompData.ProviderOrderMovement> MovementsList = new List<HispaniaCompData.ProviderOrderMovement>(Movements.Count);
                foreach (ProviderOrderMovementsView Movement in Movements)
                {
                    MovementsList.Add(Movement.GetProviderOrderMovement());
                }
            //  Step 6 : Create List of Histo Provider to add at the Historic table
                List<HispaniaCompData.HistoProvider> HistoricMovementsList = new List<HispaniaCompData.HistoProvider>(Movements.Count);
                foreach (ProviderOrderMovementsView Movement in Movements)
                {
                    if ((!Movement.Historic) && (Movement.According))
                    {
                        HistoricMovementsList.Add((new HistoProvidersView(Movement)).GetHistoProvider());
                    }
                }
            //  Step 7 : Use all information to create the new Bill.
                HispaniaCompData.Bill BillToSave = Bill.GetBill();
                BillToSave.TotalAmount = TotalActualAmount + TotalAmount;
                StringBuilder sbExpirationDateInfo = new StringBuilder(string.Empty);
                foreach (HispaniaCompData.Receipt receipt in ReceiptsList)
                {
                    sbExpirationDateInfo.AppendFormat("{0} - ", GlobalViewModel.GetStringFromDateTimeValue(receipt.Expiration_Date));
                }
                if (sbExpirationDateInfo.Length > 0)
                {
                    sbExpirationDateInfo = sbExpirationDateInfo.Remove(sbExpirationDateInfo.Length - 3, 3);
                }
                BillToSave.ExpirationDate = sbExpirationDateInfo.ToString();
                AddProviderOrdersFromBillInDb(BillToSave, Provider.GetProvider(), ProviderOrdersList, 
                                              MovementsList, HistoricMovementsList, ReceiptsList, GoodsAmountValue,
                                              TotalAmount);
            //  Step 8 : Refresh the list of Provider Orders.
                RefreshProviderOrders();
        }

        #endregion

        #region UpdateProviderOrder

        public void UpdateProviderOrder(ProviderOrdersView providerOrderView, DateTime newDeliveryNoteDate)
        {
            //  Generate the Provider Order Database object to do the operation
                HispaniaCompData.ProviderOrder providerOrderToUpdate = providerOrderView.GetProviderOrder();
            //  Update Provider Order
                UpdateProviderOrderInDb(providerOrderToUpdate, newDeliveryNoteDate);
        }
        
        public void UpdateProviderOrder(ProviderOrdersView providerOrderView, Guid DataManagementId)
        {
            //  Generate the Provider Order Database object to do the operation
                HispaniaCompData.ProviderOrder providerOrderToUpdate = providerOrderView.GetProviderOrder();
            //  Validate movements values
                Dictionary<DataBaseOp, List<HispaniaCompData.ProviderOrderMovement>> Movements = GetProviderOrderMovements(DataManagementId);
                if (providerOrderView.HasDeliveryNote)
                {
                    foreach (HispaniaCompData.ProviderOrderMovement Movement in Movements[DataBaseOp.CREATE])
                    {
                        if (Movement.According == false)
                        {
                            throw new Exception(string.Format("Error, al guardar l'Albarà '{0}' conté línies no Conformades.", 
                                                              providerOrderView.ProviderOrder_Id));
                        }
                    }
                    foreach (HispaniaCompData.ProviderOrderMovement Movement in Movements[DataBaseOp.UPDATE])
                    {
                        if (Movement.According == false)
                        {
                            throw new Exception(string.Format("Error, al guardar l'Albarà '{0}' conté línies no Conformades.", 
                                                              providerOrderView.ProviderOrder_Id));
                        }
                    }
                }
            //  Update Provider Order
                UpdateProviderOrderInDb(providerOrderToUpdate, Movements);
        }

        #endregion

        #region SplitProviderOrder

        public bool SplitProviderOrder(ProviderOrdersView ProviderOrder, out ProviderOrdersView ProviderOrderCreated, out string ErrMsg)
        {
            //  Generate the Provider Order Database object to do the operation
                HispaniaCompData.ProviderOrder providerOrderToUpdate = ProviderOrder.GetProviderOrder();
            //  Get the movements of the Provider Order Movement
                bool According = false;
                ProviderOrderCreated = null;
                List<HispaniaCompData.ProviderOrderMovement> movements_non_according = new List<HispaniaCompData.ProviderOrderMovement>();
                ObservableCollection<ProviderOrderMovementsView> movements_non_according_For_Amount = new ObservableCollection<ProviderOrderMovementsView>();
                foreach (ProviderOrderMovementsView movement in GlobalViewModel.Instance.HispaniaViewModel.GetProviderOrderMovements(ProviderOrder.ProviderOrder_Id))
                {
                    if (movement.According) According = true;
                    else
                    {
                        movements_non_according.Add(movement.GetProviderOrderMovement());
                        movements_non_according_For_Amount.Add(movement);
                    }
                }
                if ((!According) && (movements_non_according.Count == 0))
                {
                    ErrMsg = "No es pot preparar la comanda de client per un Albarà ja que no te línies de comanda.";
                    return false;
                }
                else if (!According)
                {
                    ErrMsg = "No hi ha línies conformades en la comanda de client que es vol preparar per un Albarà.";
                    return false;
                }
                else if (movements_non_according.Count == 0)
                {
                    ErrMsg = string.Empty;
                    return true;
                }
            //  Calculate the new total amounts.
                GlobalViewModel.Instance.HispaniaViewModel.CalculateAmountInfo(movements_non_according_For_Amount,
                                                                               ProviderOrder.BillingData_EarlyPaymentDiscount,
                                                                               ProviderOrder.IVAPercent,
                                                                               ProviderOrder.SurchargePercent,
                                                                               out decimal GrossAmount,
                                                                               out decimal EarlyPayementDiscountAmount,
                                                                               out decimal TaxableBaseAmount,
                                                                               out decimal IVAAmount,
                                                                               out decimal SurchargeAmount,
                                                                               out decimal TotalAmount);
                providerOrderToUpdate.TotalAmount -= TotalAmount;
            //  Update Provider Order
                HispaniaCompData.ProviderOrder NewProviderOrder = ProviderOrder.GetProviderOrder();
                NewProviderOrder.ProviderOrder_Id = -1;
                NewProviderOrder.TotalAmount = TotalAmount;
                if (SplitProviderOrderInDb(providerOrderToUpdate, NewProviderOrder, movements_non_according, out ErrMsg))
                {
                    //  Get the New ProviderOrder information.
                        ProviderOrderCreated = new ProviderOrdersView(NewProviderOrder);
                    ////  Refresh the list of Provider Orders.
                    //    RefreshProviderOrders();
                    //  Indica que la operació ha finalitzat correctament.
                        return true;
                }
                else
                {
                    //  Indica que la operació ha finalitzat amb errors.
                        return false;
                }
        }

        #endregion

        #region GetProviderOrdersFilteredByCustormerId

        public ObservableCollection<ProviderOrdersView> GetProviderOrdersFilteredByCustormerId(int Provider_Id)
        {
            ObservableCollection<ProviderOrdersView> ProviderOrdersFilteredByCustormerId = new ObservableCollection<ProviderOrdersView>();
            foreach (HispaniaCompData.ProviderOrder providerOrderDb in GetProviderOrdersFilteredByCustormerIdInDb(Provider_Id))
            {
                ProviderOrdersFilteredByCustormerId.Add(new ProviderOrdersView(providerOrderDb));
            }
            return ProviderOrdersFilteredByCustormerId;
        }

        #endregion

        #region UpdateMarkedFlagProviderOrder

        public void UpdateMarkedFlagProviderOrder(int Bill_From_Id, int Bill_Until_Id, DateTime DateToMark, decimal YearQuery)
        {
            UpdateMarkedFlagProviderOrderInDb(Bill_From_Id, Bill_Until_Id, DateToMark, YearQuery);
        }

        #endregion

        #endregion

        #region DataBase [CRUD]

        private void CreateProviderOrderInDb(HispaniaCompData.ProviderOrder providerOrder,
                                             Dictionary<DataBaseOp, List<HispaniaCompData.ProviderOrderMovement>> movements)
        {
            HispaniaDataAccess.Instance.CreateProviderOrder(providerOrder, movements);
        }

        private List<HispaniaCompData.ProviderOrder> _ProviderOrdersInDb;

        private List<HispaniaCompData.ProviderOrder> ProviderOrdersInDb
        {
            get
            {
                return (this._ProviderOrdersInDb);
            }
            set
            {
                this._ProviderOrdersInDb = value;
            }
        }

        private void UpdateProviderOrderInDb(HispaniaCompData.ProviderOrder providerOrder)
        {
            HispaniaDataAccess.Instance.UpdateProviderOrder(providerOrder);
        }

        private void UpdateProviderOrderInDb(HispaniaCompData.ProviderOrder providerOrder, DateTime newDeliveryNoteDate)
        {
            HispaniaDataAccess.Instance.UpdateProviderOrder(providerOrder, newDeliveryNoteDate);
        }

        private void UpdateProviderOrderInDb(HispaniaCompData.ProviderOrder providerOrder,
                                             Dictionary<DataBaseOp, List<HispaniaCompData.ProviderOrderMovement>> movements)
        {
            HispaniaDataAccess.Instance.UpdateProviderOrder(providerOrder, movements);
        }

        private void DeleteProviderOrderInDb(HispaniaCompData.ProviderOrder providerOrder)
        {
            HispaniaDataAccess.Instance.DeleteProviderOrder(providerOrder);
        }

        private void CreateDeliveryNoteForProviderOrderInDb(HispaniaCompData.ProviderOrder providerOrder,
                                                            HispaniaCompData.DeliveryNote deliveryNote)
        {
            HispaniaDataAccess.Instance.CreateDeliveryNoteForProviderOrder(providerOrder, deliveryNote);
        }

        private void CreateBillForProviderOrdersInDb(HispaniaCompData.Bill Bill, HispaniaCompData.Provider Provider,
                                                     List<HispaniaCompData.ProviderOrder> ProviderOrdersList,
                                                     List<HispaniaCompData.ProviderOrderMovement> MovementsList,
                                                     List<HispaniaCompData.HistoProvider> HistoricMovementsList,
                                                     List<HispaniaCompData.Receipt> ReceiptsList,
                                                     Dictionary<int, Pair> GoodsAmountValue,
                                                     decimal TotalAmount)
        {
            HispaniaDataAccess.Instance.CreateBillForProviderOrders(Bill, Provider, ProviderOrdersList, MovementsList, 
                                                                    HistoricMovementsList, ReceiptsList, GoodsAmountValue,
                                                                    TotalAmount);
        }        

        private void RemoveProviderOrdersFromBillInDb(HispaniaCompData.Bill Bill, HispaniaCompData.Provider Provider,
                                                      List<HispaniaCompData.ProviderOrder> ProviderOrdersList,
                                                      List<HispaniaCompData.ProviderOrderMovement> MovementsList,
                                                      List<HispaniaCompData.Receipt> ReceiptsList,
                                                      Dictionary<int, Pair> GoodsAmountValue,
                                                      decimal TotalAmount)
        {
            HispaniaDataAccess.Instance.RemoveProviderOrdersFromBill(Bill, Provider, ProviderOrdersList, MovementsList,
                                                                     ReceiptsList, GoodsAmountValue, TotalAmount);
        }

        private void AddProviderOrdersFromBillInDb(HispaniaCompData.Bill Bill, HispaniaCompData.Provider Provider,
                                                   List<HispaniaCompData.ProviderOrder> ProviderOrdersList,
                                                   List<HispaniaCompData.ProviderOrderMovement> MovementsList,
                                                   List<HispaniaCompData.HistoProvider> HistoricMovementsList,
                                                   List<HispaniaCompData.Receipt> ReceiptsList,
                                                   Dictionary<int, Pair> GoodsAmountValue,
                                                   decimal TotalAmount)
        {
            HispaniaDataAccess.Instance.AddProviderOrdersFromBill(Bill, Provider, ProviderOrdersList, MovementsList, 
                                                                  HistoricMovementsList, ReceiptsList, GoodsAmountValue, 
                                                                  TotalAmount);
        }
        private HispaniaCompData.ProviderOrder GetProviderOrderInDb(int ProviderOrder_Id)
        {
            return HispaniaDataAccess.Instance.GetProviderOrder(ProviderOrder_Id);
        }

        private List<HispaniaCompData.ProviderOrder> GetProviderOrdersFromBillInDb(int Bill_Id, decimal Bill_Year)
        {
            return HispaniaDataAccess.Instance.GetProviderOrdersFromBill(Bill_Id, Bill_Year);
        }

        private bool SplitProviderOrderInDb(HispaniaCompData.ProviderOrder ProviderOrder,
                                            HispaniaCompData.ProviderOrder NewProviderOrder,
                                            List<HispaniaCompData.ProviderOrderMovement> MovementsNonAccording,
                                            out string ErrMsg)
        {
            return HispaniaDataAccess.Instance.SplitProviderOrder(ProviderOrder, NewProviderOrder, MovementsNonAccording, out ErrMsg);
        }

        private List<HispaniaCompData.ProviderOrder> GetProviderOrdersFilteredByCustormerIdInDb(int Provider_Id)
        {
            return HispaniaDataAccess.Instance.GetProviderOrdersFilteredByCustormerId(Provider_Id);
        }

        private void UpdateMarkedFlagProviderOrderInDb(int Bill_From_Id, int Bill_Until_Id, DateTime DateToMark, decimal YearQuery)
        {
            HispaniaDataAccess.Instance.UpdateMarkedFlagProviderOrder(Bill_From_Id, Bill_Until_Id, DateToMark, YearQuery);
        }

        #endregion
    }
}
