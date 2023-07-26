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
using HispaniaComptabilitat.Data;

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

        private ObservableCollection<CustomerOrdersView> _CustomerOrders = null;

        public ObservableCollection<CustomerOrdersView> CustomerOrders
        {
            get
            {
                return _CustomerOrders;
            }
        }

        private Dictionary<string, CustomerOrdersView> _CustomerOrdersInDictionary = null;

        public Dictionary<string, CustomerOrdersView> CustomerOrdersDict
        {
            get
            {
                return _CustomerOrdersInDictionary;
            }
        }

        public string GetKeyCustomerOrderView(CustomerOrdersView CustomerOrdersView)
        {
            return GetKeyCustomerOrderView(CustomerOrdersView.CustomerOrder_Id);
        }

        private string GetKeyCustomerOrderView(HispaniaCompData.CustomerOrder CustomerOrder)
        {
            return GetKeyCustomerOrderView(CustomerOrder.CustomerOrder_Id);
        }

        private string GetKeyCustomerOrderView(int CoustomerOrder_Id)
        {
            return string.Format("{0}", CoustomerOrder_Id);
        }

        public CustomerOrdersView GetCustomerOrderFromDb(CustomerOrdersView customerOrdersView)
        {
            return new CustomerOrdersView(GetCustomerOrderInDb(customerOrdersView.CustomerOrder_Id));
        }

        public HispaniaCompData.CustomerOrder GetCustomerOrder(int CustomerOrder_Id)
        {
            return GetCustomerOrderInDb(CustomerOrder_Id);
        }

        public ObservableCollection<CustomerOrdersView> GetCustomerOrdersFromBill(int Bill_Id, decimal Bill_Year)
        {
            try
            {
                ObservableCollection<CustomerOrdersView> CustomerOrdersFromBill = new ObservableCollection<CustomerOrdersView>();
                foreach (HispaniaCompData.CustomerOrder customerOrder in GetCustomerOrdersFromBillInDb(Bill_Id, Bill_Year))
                {
                    CustomerOrdersFromBill.Add(new CustomerOrdersView(customerOrder));
                }
                return CustomerOrdersFromBill;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private Dictionary<DataBaseOp, List<HispaniaCompData.CustomerOrderMovement>> GetCustomerOrderMovements(Guid DataManagementId)
        {
            Dictionary<DataBaseOp, List<HispaniaCompData.CustomerOrderMovement>> movements = new Dictionary<DataBaseOp, List<HispaniaCompData.CustomerOrderMovement>>
            {
                { DataBaseOp.CREATE, new List<HispaniaCompData.CustomerOrderMovement>() },
                { DataBaseOp.UPDATE, new List<HispaniaCompData.CustomerOrderMovement>() },
                { DataBaseOp.DELETE, new List<HispaniaCompData.CustomerOrderMovement>() }
            };
            foreach (KeyValuePair<object, DataBaseOp> movement in GlobalViewModel.Instance.HispaniaViewModel.GetItemsInDataManaged(DataManagementId))
            {
                if (movement.Value != DataBaseOp.READ)
                {
                    movements[movement.Value].Add(((CustomerOrderMovementsView)movement.Key).GetCustomerOrderMovement());
                }
            }
            return movements;
        }

        #endregion

        #region CreateCustomerOrder

        public void CreateCustomerOrder(CustomerOrdersView customerOrdersView, Guid DataManagementId)
        {
            //  Generate the Customer Order Database object to do the operation
                HispaniaCompData.CustomerOrder customerOrderToCreate = customerOrdersView.GetCustomerOrder();
                customerOrderToCreate.Date = DateTime.Now;
            //  Generate the List of Customer 
                Dictionary<DataBaseOp, List<HispaniaCompData.CustomerOrderMovement>> movements = GetCustomerOrderMovements(DataManagementId);
            //  Create new Customer Order
                CreateCustomerOrderInDb(customerOrderToCreate, movements);
            //  Collect the Id of the new Customer Order
                customerOrdersView.CustomerOrder_Id = customerOrderToCreate.CustomerOrder_Id;
        }

        #endregion

        #region UpdateCustomerOrder

        public void UpdateCustomerOrder(CustomerOrdersView customerOrderView)
        {
            //  Generate the Customer Order Database object to do the operation
                HispaniaCompData.CustomerOrder customerOrderToUpdate = customerOrderView.GetCustomerOrder();
            //  Update Customer Order
                UpdateCustomerOrderInDb(customerOrderToUpdate);
        }

        #endregion

        #region DeleteCustomerOrder

        public void DeleteCustomerOrder(CustomerOrdersView CustomerOrderView)
        {

            //  Delete the Customer Order in the Database.
                DeleteCustomerOrderInDb(CustomerOrderView.GetCustomerOrder());
        }

        #endregion

        #region RefreshCustomerOrders

        public void RefreshCustomerOrders(bool HistoricData = false)
        {
            try
            {
                CustomerOrdersInDb = HispaniaDataAccess.Instance.ReadCustomerOrders(HistoricData);
                _CustomerOrders = new ObservableCollection<CustomerOrdersView>();
                _CustomerOrdersInDictionary = new Dictionary<string, CustomerOrdersView>();
                foreach (HispaniaCompData.CustomerOrder CustomerOrder in CustomerOrdersInDb)
                {
                    CustomerOrdersView NewCustomerOrdersView = new CustomerOrdersView(CustomerOrder);
                    _CustomerOrders.Add(NewCustomerOrdersView);
                    _CustomerOrdersInDictionary.Add(GetKeyCustomerOrderView(CustomerOrder), NewCustomerOrdersView);
                }
            }
            catch (Exception ex)
            {
                _CustomerOrders = null;
                throw ex;
            }
        }

        #endregion

        #region CreateDeliveryNoteForCustomerOrder

        public bool CreateDeliveryNoteForCustomerOrder(CustomerOrdersView customerOrder, out string ErrMsg)
        {
            try
            {
                //  Find the movements for the Customer Order.
                    ObservableCollection<CustomerOrderMovementsView> Movements = GetCustomerOrderMovements(customerOrder.CustomerOrder_Id);
                //  Validate movements of Customer Order
                    if (Movements.Count == 0)
                    {
                        ErrMsg = "Error, no es pot crear un Albarà d'una Comanda de Client sense línies de comanda.";
                    }
                    else
                    {
                        bool According = true;
                        foreach (CustomerOrderMovementsView movement in Movements)
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
                            //  Generate the Customer Order Database object to do the operation
                                HispaniaCompData.CustomerOrder customerOrderToUpdate = customerOrder.GetCustomerOrder();
                            //  Generate the Delivery Note Database object to do the operation
                                HispaniaCompData.DeliveryNote deliveryNoteToCreate = (new DeliveryNotesView()).GetDeliveryNote();
                            //  Create the new Delivery Note and add a reference of it at the Customer Order indicated.
                                CreateDeliveryNoteForCustomerOrderInDb(customerOrderToUpdate, deliveryNoteToCreate);
                            //  Collect the Delivery Note information of the Customer Order
                                customerOrder.DeliveryNote_Id = GlobalViewModel.GetIntFromIntIdValue(customerOrderToUpdate.DeliveryNote_Id);
                                customerOrder.DeliveryNote_Year = GlobalViewModel.GetDecimalYearValue(customerOrderToUpdate.DeliveryNote_Year);
                                customerOrder.DeliveryNote_Date = GlobalViewModel.GetDateTimeValue(customerOrderToUpdate.DeliveryNote_Date);
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

        #region CreateBillForCustomer

        public void CreateBillsFromCustomerOrders(SortedDictionary<int, DataForBill> SourceCustomerOrders)
        {
            foreach (DataForBill CustomerOrderInfo in SourceCustomerOrders.Values)
            {
                LogViewModel.Instance.WriteLog("Create Bill - CreateBillForCustomer({0}, {1}, {2})",
                                               CustomerOrderInfo.Bill_Date, CustomerOrderInfo.Customer.Customer_Id, CustomerOrderInfo.Movements.Count);
                CreateBillForCustomer( CustomerOrderInfo.Bill_Date, CustomerOrderInfo.Customer,
                                       CustomerOrderInfo.Movements );
            }
        }

        private void CreateBillForCustomer( DateTime Bill_Date, CustomersView Customer, 
                                            List<CustomerOrdersView> SourceCustomerOrders )
        {
            //  Data Validation
                LogViewModel.Instance.WriteLog("Create Bill - ValidateBillForCustomer({0}, {1})", Customer.Customer_Id, SourceCustomerOrders.Count);
                if (!ValidateBillForCustomer(Customer, SourceCustomerOrders)) return;
            //  Steps to Create bill information and save in the Database.
            //  Step 1 : Calculate amount of the new bill.
            //           This amount will be used for update Customer Risk, Month of Year Customer Sales and  for calculate Bill Receipts.
                LogViewModel.Instance.WriteLog("Create Bill - Step 1 - CalculateAmountInfo({0}, {1}, out ...)",  Customer.Customer_Id, SourceCustomerOrders.Count);
                CalculateAmountInfo(Customer, SourceCustomerOrders, out ObservableCollection<CustomerOrderMovementsView> Movements,
                                    out decimal GrossAmount, out decimal EarlyPayementDiscountAmount, out decimal TaxableBaseAmount,
                                    out decimal IVAAmount, out decimal SurchargeAmount, out decimal TotalAmount);
                foreach (CustomerOrderMovementsView Movement in Movements)
                {
                    if (!Movement.According)
                    {
                        MsgManager.ShowMessage(
                           string.Format("Error, el moviment '{0}' de l'Albarà '{1}' no está confirmat, no es pot generar una factura amb moviments no confirmats.",
                                         Movement.CustomerOrderMovement_Id, Movement.DeliveryNote_Id_Str));
                        return;
                    }
                }
            //  Step 2 : Calculate receipts from the new bill
                LogViewModel.Instance.WriteLog("Create Bill - Step 2 - CalculateReceipts({0}, {1}, {2})", Customer.Customer_Id, TotalAmount, Bill_Date);
                List<HispaniaCompData.Receipt> ReceiptsList = CalculateReceipts(Customer, TotalAmount, Bill_Date);
            //  Step 3 : Calculate amount of Goods
                LogViewModel.Instance.WriteLog("Create Bill - Step 3 - CalculateGoodsAmountValue({0})", Movements.Count);
                Dictionary<int, Pair> GoodsAmountValue = CalculateGoodsAmountValue(Movements);
            //  Step 4 : Create Bill Info from Customer.
                HispaniaCompData.Bill Bill = (new BillsView(Customer, SourceCustomerOrders[0] )).GetBill();
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

                #region //TODO: In the past it was like this - now the data from the CustomerOrder
                
                //if(SourceCustomerOrders[0].EffectType.EffectType_Id == 6)
                //{
                //    Bill.DataBank_EffectType_Id = SourceCustomerOrders[0].EffectType.EffectType_Id;
                //    Bill.DataBank_Bank = SourceCustomerOrders[0].DataBank_Bank;
                //    Bill.DataBank_BankAddress = SourceCustomerOrders[0].DataBank_BankAddress;
                //    Bill.DataBank_IBAN_CountryCode = SourceCustomerOrders[0].DataBank_IBAN_CountryCode;
                //    Bill.DataBank_IBAN_BankCode = SourceCustomerOrders[0].DataBank_IBAN_BankCode;
                //    Bill.DataBank_IBAN_OfficeCode = SourceCustomerOrders[0].DataBank_IBAN_OfficeCode;
                //    Bill.DataBank_IBAN_CheckDigits = SourceCustomerOrders[0].DataBank_IBAN_CheckDigits;
                //    Bill.DataBank_IBAN_AccountNumber = SourceCustomerOrders[0].DataBank_IBAN_AccountNumber;
                //}
                
                #endregion

                LogViewModel.Instance.WriteLog("Create Bill - Step 4.2 - EffectType_Id = {0}, Bill.DataBank_Bank = {1}, Bill.DataBank_BankAddress = {2}, IBAN = {3}", 
                                               Bill.DataBank_EffectType_Id, Bill.DataBank_Bank, Bill.DataBank_BankAddress,
                                               string.Format("{0} {1} {2} {3} {4}", Bill.DataBank_IBAN_CountryCode, Bill.DataBank_IBAN_BankCode,
                                                             Bill.DataBank_IBAN_OfficeCode, Bill.DataBank_IBAN_CheckDigits, Bill.DataBank_IBAN_AccountNumber));
            //  Step 5 : Create List of Customer Orders to add at the Bill
                List<HispaniaCompData.CustomerOrder> CustomerOrdersList = new List<HispaniaCompData.CustomerOrder>(SourceCustomerOrders.Count);
                foreach (CustomerOrdersView customerOrder in SourceCustomerOrders)
                {
                    CustomerOrdersList.Add(customerOrder.GetCustomerOrder());
                }
                LogViewModel.Instance.WriteLog("Create Bill - Step 5 - List of Customer Orders to add at the Bill -> {0}", CustomerOrdersList.Count);
            //  Step 6 : Create List of Customer Orders Movements to add at the Bill
                List<HispaniaCompData.CustomerOrderMovement> MovementsList = new List<HispaniaCompData.CustomerOrderMovement>(Movements.Count);
                foreach (CustomerOrderMovementsView Movement in Movements)
                {
                    MovementsList.Add(Movement.GetCustomerOrderMovement());
                }
                LogViewModel.Instance.WriteLog("Create Bill - Step 6 - List of Customer Orders Movements to add at the Bill -> {0}", MovementsList.Count);
            //  Step 7 : Create List of Histo Customer to add at the Historic table
                int Historic_Id = -1;
                List<HispaniaCompData.HistoCustomer> HistoricMovementsList = new List<HispaniaCompData.HistoCustomer>(Movements.Count);
                foreach (CustomerOrderMovementsView Movement in Movements)
                {
                    HistoCustomersView HistoCustomerView = new HistoCustomersView(Movement)
                    {
                        HistoCustomer_Id = Historic_Id,
                        Good_Description = Movement.Description
                    };
                    Historic_Id--;
                    HistoricMovementsList.Add(HistoCustomerView.GetHistoCustomer());
                }
                LogViewModel.Instance.WriteLog("Create Bill - Step 7 - List of Histo Customer to add at the Historic table -> {0}", HistoricMovementsList.Count);
            //  Step 8 : Use all information to create the new Bill.
                LogViewModel.Instance.WriteLog("Create Bill - Step 8 - CreateBillForCustomerOrdersInDb(Bill, Customer.GetCustomer(), CustomerOrdersList, MovementsList, " +
                                                                                                      "HistoricMovementsList, ReceiptsList, GoodsAmountValue, TotalAmount)");
                CreateBillForCustomerOrdersInDb(Bill, Customer.GetCustomer(), CustomerOrdersList, MovementsList, HistoricMovementsList, ReceiptsList, GoodsAmountValue, 
                                                TotalAmount);
            //  Step 9 : Refresh the list of Customer Orders.
                LogViewModel.Instance.WriteLog("Create Bill - Step 9 - RefreshCustomerOrders()");
                RefreshCustomerOrders();
        }

        private bool ValidateBillForCustomer(CustomersView Customer, List<CustomerOrdersView> SourceCustomerOrders)
        {
            decimal IVAPercent = SourceCustomerOrders[0].IVAPercent;
            decimal SurchagePercent = SourceCustomerOrders[0].SurchargePercent;
            string Address = SourceCustomerOrders[0].Address.ToUpper();
            bool IVAPercentAreEquals = true;
            bool SurchargePercentAreEquals = true;
            bool AddressAreEquals = true;
            foreach (CustomerOrdersView CustomerOrder in SourceCustomerOrders)
            {
                if (!CustomerOrder.According)
                {
                    MsgManager.ShowMessage(
                        string.Format("Error, impossible crear una factura pel client '{0}', " +
                                      "ja que no hi confirmació de l'entrega del material per l'albarà '{1}'.",
                                      CustomerOrder.Customer.Customer_Id,
                                      CustomerOrder.DeliveryNote_Id));
                    return false;
                }
                if (CustomerOrder.IVAPercent != IVAPercent)
                {
                    IVAPercentAreEquals = false;
                    break;
                }
                if (CustomerOrder.SurchargePercent != SurchagePercent)
                {
                    SurchargePercentAreEquals = false;
                    break;
                }
                if (CustomerOrder.Address.ToUpper() != Address)
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
                                      SourceCustomerOrders[0].Customer_Id_Str,
                                      Customer.BillingData_IVAType.IVAPercent);
                if (MsgManager.ShowQuestion(QuestionMsg) != MessageBoxResult.Yes)
                {
                    MsgManager.ShowMessage(
                        string.Format("Avís, creació de la factura pels albarans del client '{0}' cancel·lada per l'usuari.",
                                      SourceCustomerOrders[0].Customer_Id_Str), MsgType.Information);
                    return false;
                }
            }
            if (!SurchargePercentAreEquals)
            {
                QuestionMsg = 
                        string.Format("No tots els albarans seleccionats del client '{0}' tenen el mateix " +
                                      "Tipus de Recàrrec. Si es segueix endavant la factura es crearà fent " + 
                                      "servir el Tipus de Recàrrec definit pel client '{1}'. Vol continuar ?",
                                      SourceCustomerOrders[0].Customer_Id_Str,
                                      Customer.BillingData_IVAType.SurchargePercent);
                if (MsgManager.ShowQuestion(QuestionMsg) != MessageBoxResult.Yes)
                {
                    MsgManager.ShowMessage(
                        string.Format("Avís, creació de la factura pels albarans del client '{0}' cancel·lada per l'usuari.",
                                      SourceCustomerOrders[0].Customer_Id_Str), MsgType.Information);
                    return false;
                }
            }
            if (!AddressAreEquals)
            {
                QuestionMsg =
                        string.Format("No tots els albarans seleccionats del client '{0}' tenen la mateixa " +
                                      "Adreça d'Enviament. Vol continuar ?",
                                      SourceCustomerOrders[0].Customer_Id_Str);
                if (MsgManager.ShowQuestion(QuestionMsg) != MessageBoxResult.Yes)
                {
                    MsgManager.ShowMessage(
                        string.Format("Avís, creació de la factura pels albarans del client '{0}' cancel·lada per l'usuari.",
                                      SourceCustomerOrders[0].Customer_Id_Str), MsgType.Information);
                    return false;
                }
            }
            return true;
        }

        #endregion

        #region RemoveCustomerOrdersFromBill

        public void RemoveCustomerOrdersFromBill(BillsView Bill, CustomersView Customer, List<CustomerOrdersView> SourceCustomerOrders)
        {
            //  Steps to Create bill information and save in the Database.
            //  Step 1 : Calculate amount of the updated bill.
            //           This amount will be used for update Customer Risk, Month of Year Customer Sales and  for calculate Bill Receipts.
                CalculateAmountInfo(Customer, SourceCustomerOrders, out ObservableCollection<CustomerOrderMovementsView> Movements,
                                    out decimal GrossAmount, out decimal EarlyPayementDiscountAmount, out decimal TaxableBaseAmount,
                                    out decimal IVAAmount, out decimal SurchargeAmount, out decimal TotalAmount);
            //  Step 2 : Calculate new receipts from the bill
                CalculateAmountInfo(Customer, Bill, out decimal TotalActualAmount);
                DateTime? Bill_Date = null;
                foreach (CustomerOrdersView customerOrder in GetCustomerOrdersFromBill(Bill.Bill_Id, Bill.Year))
                {
                    if (!SourceCustomerOrders.Contains(customerOrder))
                    {
                        if ((Bill_Date is null) || (customerOrder.Date > Bill_Date))
                        {
                            Bill_Date = customerOrder.Date;
                        }
                    }
                }
                Bill.Date = (DateTime)Bill_Date;
                List<HispaniaCompData.Receipt> ReceiptsList = CalculateReceipts(Customer, TotalActualAmount - TotalAmount, Bill.Date);
            //  Step 3 : Calculate acumulate of Goods
                Dictionary<int, Pair> GoodsAmountValue = CalculateGoodsAmountValue(Movements);
            //  Step 4 : Create List of Customer Orders to remove from the Bill
                List<HispaniaCompData.CustomerOrder> CustomerOrdersList = new List<HispaniaCompData.CustomerOrder>(SourceCustomerOrders.Count);
                foreach (CustomerOrdersView customerOrder in SourceCustomerOrders)
                {
                    CustomerOrdersList.Add(customerOrder.GetCustomerOrder());
                }
            //  Step 5 : Create List of Customer Orders Movements to remove from the Bill
                List<HispaniaCompData.CustomerOrderMovement> MovementsList = new List<HispaniaCompData.CustomerOrderMovement>(Movements.Count);
                foreach (CustomerOrderMovementsView Movement in Movements)
                {
                    MovementsList.Add(Movement.GetCustomerOrderMovement());
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
                RemoveCustomerOrdersFromBillInDb(BillToSave, Customer.GetCustomer(), CustomerOrdersList, MovementsList, 
                                                 ReceiptsList, GoodsAmountValue, TotalAmount);
            //  Step 7 : Refresh the list of Customer Orders.
                RefreshCustomerOrders();
        }
        
        #endregion

        #region AddCustomerOrdersFromBill

        public void AddCustomerOrdersFromBill(BillsView Bill, CustomersView Customer, List<CustomerOrdersView> SourceCustomerOrders)
        {
            //  Steps to Create bill information and save in the Database.
            //  Step 1 : Calculate amount of the updated bill.
            //           This amount will be used for update Customer Risk, Month of Year Customer Sales and  for calculate Bill Receipts.
                CalculateAmountInfo(Customer, SourceCustomerOrders, out ObservableCollection<CustomerOrderMovementsView> Movements,
                                    out decimal GrossAmount, out decimal EarlyPayementDiscountAmount, out decimal TaxableBaseAmount,
                                    out decimal IVAAmount, out decimal SurchargeAmount, out decimal TotalAmount);
            //  Step 2 : Calculate new receipts from the bill
                CalculateAmountInfo(Customer, Bill, out decimal TotalActualAmount);
                foreach (CustomerOrdersView customerOrder in SourceCustomerOrders)
                {
                    if (customerOrder.DeliveryNote_Date > Bill.Date) Bill.Date = customerOrder.DeliveryNote_Date;
                }
                List<HispaniaCompData.Receipt> ReceiptsList = CalculateReceipts(Customer, TotalActualAmount + TotalAmount, Bill.Date);
            //  Step 3 : Calculate acumulate of Goods
                Dictionary<int, Pair> GoodsAmountValue = CalculateGoodsAmountValue(Movements);
            //  Step 4 : Create List of Customer Orders to Add from the Bill
                List<HispaniaCompData.CustomerOrder> CustomerOrdersList = new List<HispaniaCompData.CustomerOrder>(SourceCustomerOrders.Count);
                foreach (CustomerOrdersView customerOrder in SourceCustomerOrders)
                {
                    CustomerOrdersList.Add(customerOrder.GetCustomerOrder());
                }
            //  Step 5 : Create List of Customer Orders Movements to Add from the Bill
                List<HispaniaCompData.CustomerOrderMovement> MovementsList = new List<HispaniaCompData.CustomerOrderMovement>(Movements.Count);
                foreach (CustomerOrderMovementsView Movement in Movements)
                {
                    MovementsList.Add(Movement.GetCustomerOrderMovement());
                }
            //  Step 6 : Create List of Histo Customer to add at the Historic table
                List<HispaniaCompData.HistoCustomer> HistoricMovementsList = new List<HispaniaCompData.HistoCustomer>(Movements.Count);
                foreach (CustomerOrderMovementsView Movement in Movements)
                {
                    if ((!Movement.Historic) && (Movement.According))
                    {
                        HistoricMovementsList.Add((new HistoCustomersView(Movement)).GetHistoCustomer());
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
                AddCustomerOrdersFromBillInDb(BillToSave, Customer.GetCustomer(), CustomerOrdersList, 
                                              MovementsList, HistoricMovementsList, ReceiptsList, GoodsAmountValue,
                                              TotalAmount);
            //  Step 8 : Refresh the list of Customer Orders.
                RefreshCustomerOrders();
        }

        #endregion

        #region UpdateCustomerOrder

        public void UpdateCustomerOrder(CustomerOrdersView customerOrderView, DateTime newDeliveryNoteDate)
        {
            //  Generate the Customer Order Database object to do the operation
                HispaniaCompData.CustomerOrder customerOrderToUpdate = customerOrderView.GetCustomerOrder();
            //  Update Customer Order
                UpdateCustomerOrderInDb(customerOrderToUpdate, newDeliveryNoteDate);
        }
        
        public void UpdateCustomerOrder(CustomerOrdersView customerOrderView, Guid DataManagementId)
        {
            //  Generate the Customer Order Database object to do the operation
                HispaniaCompData.CustomerOrder customerOrderToUpdate = customerOrderView.GetCustomerOrder();
            //  Validate movements values
                Dictionary<DataBaseOp, List<HispaniaCompData.CustomerOrderMovement>> Movements = GetCustomerOrderMovements(DataManagementId);
                if (customerOrderView.HasDeliveryNote)
                {
                    foreach (HispaniaCompData.CustomerOrderMovement Movement in Movements[DataBaseOp.CREATE])
                    {
                        if (Movement.According == false)
                        {
                            throw new Exception(string.Format("Error, al guardar l'Albarà '{0}' conté línies no Conformades.", 
                                                              customerOrderView.CustomerOrder_Id));
                        }
                    }
                    foreach (HispaniaCompData.CustomerOrderMovement Movement in Movements[DataBaseOp.UPDATE])
                    {
                        if (Movement.According == false)
                        {
                            throw new Exception(string.Format("Error, al guardar l'Albarà '{0}' conté línies no Conformades.", 
                                                              customerOrderView.CustomerOrder_Id));
                        }
                    }
                }
            //  Update Customer Order
                UpdateCustomerOrderInDb(customerOrderToUpdate, Movements);
        }

        #endregion

        #region SplitCustomerOrder

        public bool SplitCustomerOrder(CustomerOrdersView CustomerOrder, out CustomerOrdersView CustomerOrderCreated, out string ErrMsg)
        {
            //  Generate the Customer Order Database object to do the operation
                HispaniaCompData.CustomerOrder customerOrderToUpdate = CustomerOrder.GetCustomerOrder();
            //  Get the movements of the Customer Order Movement
                bool According = false;
                CustomerOrderCreated = null;
                List<HispaniaCompData.CustomerOrderMovement> movements_non_according = new List<HispaniaCompData.CustomerOrderMovement>();
                ObservableCollection<CustomerOrderMovementsView> movements_non_according_For_Amount = new ObservableCollection<CustomerOrderMovementsView>();
                foreach (CustomerOrderMovementsView movement in GlobalViewModel.Instance.HispaniaViewModel.GetCustomerOrderMovements(CustomerOrder.CustomerOrder_Id))
                {
                    if (movement.According) According = true;
                    else
                    {
                        movements_non_according.Add(movement.GetCustomerOrderMovement());
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
                                                                               CustomerOrder.BillingData_EarlyPaymentDiscount,
                                                                               CustomerOrder.IVAPercent,
                                                                               CustomerOrder.SurchargePercent,
                                                                               out decimal GrossAmount,
                                                                               out decimal EarlyPayementDiscountAmount,
                                                                               out decimal TaxableBaseAmount,
                                                                               out decimal IVAAmount,
                                                                               out decimal SurchargeAmount,
                                                                               out decimal TotalAmount);
                customerOrderToUpdate.TotalAmount -= TotalAmount;
            //  Update Customer Order
                HispaniaCompData.CustomerOrder NewCustomerOrder = CustomerOrder.GetCustomerOrder();
                NewCustomerOrder.CustomerOrder_Id = -1;
                NewCustomerOrder.TotalAmount = TotalAmount;
                if (SplitCustomerOrderInDb(customerOrderToUpdate, NewCustomerOrder, movements_non_according, out ErrMsg))
                {
                    //  Get the New CustomerOrder information.
                        CustomerOrderCreated = new CustomerOrdersView(NewCustomerOrder);
                    ////  Refresh the list of Customer Orders.
                    //    RefreshCustomerOrders();
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

        #region GetCustomerOrdersFilteredByCustormerId

        public ObservableCollection<CustomerOrdersView> GetCustomerOrdersFilteredByCustormerId(int Customer_Id)
        {
            ObservableCollection<CustomerOrdersView> CustomerOrdersFilteredByCustormerId = new ObservableCollection<CustomerOrdersView>();
            foreach (HispaniaCompData.CustomerOrder customerOrderDb in GetCustomerOrdersFilteredByCustormerIdInDb(Customer_Id))
            {
                CustomerOrdersFilteredByCustormerId.Add(new CustomerOrdersView(customerOrderDb));
            }
            return CustomerOrdersFilteredByCustormerId;
        }

        #endregion

        #region UpdateMarkedFlagCustomerOrder

        public void UpdateMarkedFlagCustomerOrder(int Bill_From_Id, int Bill_Until_Id, DateTime DateToMark, decimal YearQuery)
        {
            UpdateMarkedFlagCustomerOrderInDb(Bill_From_Id, Bill_Until_Id, DateToMark, YearQuery);
        }

        #endregion

        #endregion

        #region DataBase [CRUD]

        private void CreateCustomerOrderInDb(HispaniaCompData.CustomerOrder customerOrder,
                                             Dictionary<DataBaseOp, List<HispaniaCompData.CustomerOrderMovement>> movements)
        {
            HispaniaDataAccess.Instance.CreateCustomerOrder(customerOrder, movements);
        }

        private List<HispaniaCompData.CustomerOrder> _CustomerOrdersInDb;

        private List<HispaniaCompData.CustomerOrder> CustomerOrdersInDb
        {
            get
            {
                return (this._CustomerOrdersInDb);
            }
            set
            {
                this._CustomerOrdersInDb = value;
            }
        }

        private void UpdateCustomerOrderInDb(HispaniaCompData.CustomerOrder customerOrder)
        {
            HispaniaDataAccess.Instance.UpdateCustomerOrder(customerOrder);
        }

        private void UpdateCustomerOrderInDb(HispaniaCompData.CustomerOrder customerOrder, DateTime newDeliveryNoteDate)
        {
            HispaniaDataAccess.Instance.UpdateCustomerOrder(customerOrder, newDeliveryNoteDate);
        }

        private void UpdateCustomerOrderInDb(HispaniaCompData.CustomerOrder customerOrder,
                                             Dictionary<DataBaseOp, List<HispaniaCompData.CustomerOrderMovement>> movements)
        {
            HispaniaDataAccess.Instance.UpdateCustomerOrder(customerOrder, movements);
        }

        private void DeleteCustomerOrderInDb(HispaniaCompData.CustomerOrder customerOrder)
        {
            HispaniaDataAccess.Instance.DeleteCustomerOrder(customerOrder);
        }

        private void CreateDeliveryNoteForCustomerOrderInDb(HispaniaCompData.CustomerOrder customerOrder,
                                                            HispaniaCompData.DeliveryNote deliveryNote)
        {
            HispaniaDataAccess.Instance.CreateDeliveryNoteForCustomerOrder(customerOrder, deliveryNote);
        }

        private void CreateBillForCustomerOrdersInDb(HispaniaCompData.Bill Bill, HispaniaCompData.Customer Customer,
                                                     List<HispaniaCompData.CustomerOrder> CustomerOrdersList,
                                                     List<HispaniaCompData.CustomerOrderMovement> MovementsList,
                                                     List<HispaniaCompData.HistoCustomer> HistoricMovementsList,
                                                     List<HispaniaCompData.Receipt> ReceiptsList,
                                                     Dictionary<int, Pair> GoodsAmountValue,
                                                     decimal TotalAmount)
        {
            HispaniaDataAccess.Instance.CreateBillForCustomerOrders(Bill, Customer, CustomerOrdersList, MovementsList, 
                                                                    HistoricMovementsList, ReceiptsList, GoodsAmountValue,
                                                                    TotalAmount);
        }        

        private void RemoveCustomerOrdersFromBillInDb(HispaniaCompData.Bill Bill, HispaniaCompData.Customer Customer,
                                                      List<HispaniaCompData.CustomerOrder> CustomerOrdersList,
                                                      List<HispaniaCompData.CustomerOrderMovement> MovementsList,
                                                      List<HispaniaCompData.Receipt> ReceiptsList,
                                                      Dictionary<int, Pair> GoodsAmountValue,
                                                      decimal TotalAmount)
        {
            HispaniaDataAccess.Instance.RemoveCustomerOrdersFromBill(Bill, Customer, CustomerOrdersList, MovementsList,
                                                                     ReceiptsList, GoodsAmountValue, TotalAmount);
        }

        private void AddCustomerOrdersFromBillInDb(HispaniaCompData.Bill Bill, HispaniaCompData.Customer Customer,
                                                   List<HispaniaCompData.CustomerOrder> CustomerOrdersList,
                                                   List<HispaniaCompData.CustomerOrderMovement> MovementsList,
                                                   List<HispaniaCompData.HistoCustomer> HistoricMovementsList,
                                                   List<HispaniaCompData.Receipt> ReceiptsList,
                                                   Dictionary<int, Pair> GoodsAmountValue,
                                                   decimal TotalAmount)
        {
            HispaniaDataAccess.Instance.AddCustomerOrdersFromBill(Bill, Customer, CustomerOrdersList, MovementsList, 
                                                                  HistoricMovementsList, ReceiptsList, GoodsAmountValue, 
                                                                  TotalAmount);
        }
        private HispaniaCompData.CustomerOrder GetCustomerOrderInDb(int CustomerOrder_Id)
        {
            return HispaniaDataAccess.Instance.GetCustomerOrder(CustomerOrder_Id);
        }

        private List<HispaniaCompData.CustomerOrder> GetCustomerOrdersFromBillInDb(int Bill_Id, decimal Bill_Year)
        {
            return HispaniaDataAccess.Instance.GetCustomerOrdersFromBill(Bill_Id, Bill_Year);
        }

        private bool SplitCustomerOrderInDb(HispaniaCompData.CustomerOrder CustomerOrder,
                                            HispaniaCompData.CustomerOrder NewCustomerOrder,
                                            List<HispaniaCompData.CustomerOrderMovement> MovementsNonAccording,
                                            out string ErrMsg)
        {
            return HispaniaDataAccess.Instance.SplitCustomerOrder(CustomerOrder, NewCustomerOrder, MovementsNonAccording, out ErrMsg);
        }

        private List<HispaniaCompData.CustomerOrder> GetCustomerOrdersFilteredByCustormerIdInDb(int Customer_Id)
        {
            return HispaniaDataAccess.Instance.GetCustomerOrdersFilteredByCustormerId(Customer_Id);
        }

        private void UpdateMarkedFlagCustomerOrderInDb(int Bill_From_Id, int Bill_Until_Id, DateTime DateToMark, decimal YearQuery)
        {
            HispaniaDataAccess.Instance.UpdateMarkedFlagCustomerOrder(Bill_From_Id, Bill_Until_Id, DateToMark, YearQuery);
        }

        #endregion
    }
}
