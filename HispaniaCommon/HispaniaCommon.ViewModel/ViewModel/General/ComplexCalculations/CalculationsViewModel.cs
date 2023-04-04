#region Librerias usadas por la clase

using HispaniaCommon.DataAccess;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HispaniaCompData = HispaniaComptabilitat.Data;

#endregion

namespace HispaniaCommon.ViewModel
{
    public enum TypeOfUpdateAVGPC
    {
        AddUnitsEntry,

        AddUnitsDeparture,

        RemoveUnitsEntry,

        RemoveUnitsDeparture,
    }

    /// <summary>
    /// Last update Data: 19/09/2016
    /// Descriptión: class that implements Common ViewModel of the Applications.
    /// </summary>
    public partial class MaintenanceForViewModel
    {
        #region IBAN

        /// <summary>
        /// Calcula l'IBAN Espanyol corresponent a les dades de conta passades com a paràmetre.
        /// </summary>
        /// <param name="CCC">Dades de la conta sobre la que es vol calcular l'IBAN.</param>
        /// <returns>IBAN Espanyol relacionat amb la conta passada com a paràmetre</returns>
        public string CalculateSpanishIBAN(string CCC)
        {
            //  Validem les dades necessaries per poder calcular l'IBAN.
                if (string.IsNullOrEmpty(CCC)) throw new ArgumentException("Error, Conta Corrent sense dades.");
                if (CCC.Length != 20) throw new ArgumentException("Error, la Conta Corrent no te 20 caràcters.");
            //  Calculem l'IBAN
            //  Afegim el codi de Pais, en aquest cas el d'Espanya "ES" : E = "14" i S = "28" més el 00.
                string BaseForCalcul = CCC + "142800";
                string Result = string.Empty;
                for (int i = 0; i < 25; i += 5)
                {
                    Result = (int.Parse(Result + BaseForCalcul.Substring(i, (i!=20) ? 5 : 6)) % 97).ToString();
                }
                return string.Format("ES{0:00}{1}", 98 - decimal.Parse(Result), CCC);
        }

        /// <summary>
        /// Valida l'IBAN Espanyol passat com a paràmetre.
        /// </summary>
        /// <param name="IBAN">IBAN a validar.</param>
        /// <returns>true, si l'IBAN és correcte, false, si l'IBAN és incorrecte.</returns>
        public bool ValidateSpanishIBAN(string IBAN)
        {
            //  Validem les dades necessaries per poder calcular l'IBAN.
                if (string.IsNullOrEmpty(IBAN)) throw new ArgumentException("Error, IBAN sense dades.");
                if (IBAN.Length != 24) throw new ArgumentException("Error, l'IBAN no te 24 caràcters.");
                if (IBAN[0] != 'E') throw new ArgumentException("Error, l'IBAN té un codi de pais incorrecte.");
                if (IBAN[1] != 'S') throw new ArgumentException("Error, l'IBAN té un codi de pais incorrecte.");
            //  Validem l'IBAN
            //  Calculem la cadena bàsica per la validació, "ES" : E = "14" i S = "28" més els dos caracters de pais 
            //  restants posats al final.
                if ((decimal.Parse((IBAN.Substring(4, IBAN.Length - 4) + "1428" + IBAN.Substring(2, 2))) % 97) != 1)
                {
                    throw new Exception("Error, algun dels digits de l'IBAN, pais o conta, son incorrectes.");
                }
                return (true);
        }

        #endregion

        #region Calculate Accounting Values

        public void CalculateAmountInfo(CustomersView Customer,
                                        List<CustomerOrdersView> CustomerOrders,
                                        out ObservableCollection<CustomerOrderMovementsView> Movements,
                                        out decimal GrossAmount,
                                        out decimal EarlyPayementDiscountAmount,
                                        out decimal TaxableBaseAmount,
                                        out decimal IVAAmount,
                                        out decimal SurchargeAmount,
                                        out decimal TotalAmount)
        {
            Movements = new ObservableCollection<CustomerOrderMovementsView>();
            if (!(CustomerOrders is null) && (CustomerOrders.Count > 0))
            {
                foreach (CustomerOrdersView customerOrder in CustomerOrders)
                {
                    foreach (CustomerOrderMovementsView Movement in GetCustomerOrderMovements(customerOrder.CustomerOrder_Id))
                    {
                        Movements.Add(Movement);
                    }
                }
                decimal EarlyPaymentDiscountPrecent = Customer.BillingData_EarlyPaymentDiscount;
                decimal IVAPercent = (Customer.BillingData_IVAType is null) ? 0 : Customer.BillingData_IVAType.IVAPercent;
                decimal SurchargePercent = (Customer.BillingData_IVAType is null) ? 0 : Customer.BillingData_IVAType.SurchargePercent;
                CalculateAmountInfo(Movements, EarlyPaymentDiscountPrecent, IVAPercent, SurchargePercent,
                                    out GrossAmount, out EarlyPayementDiscountAmount, out TaxableBaseAmount,
                                    out IVAAmount, out SurchargeAmount, out TotalAmount);
            }
            else
            {
                GrossAmount= 0;
                EarlyPayementDiscountAmount = 0;
                TaxableBaseAmount = 0;
                IVAAmount= 0;
                SurchargeAmount = 0;
                TotalAmount = 0;
            }
        }

        public void CalculateAmountInfo(ProvidersView Provider,
                                       List<ProviderOrdersView> ProviderOrders,
                                       out ObservableCollection<ProviderOrderMovementsView> Movements,
                                       out decimal GrossAmount,
                                       out decimal EarlyPayementDiscountAmount,
                                       out decimal TaxableBaseAmount,
                                       out decimal IVAAmount,
                                       out decimal SurchargeAmount,
                                       out decimal TotalAmount)
        {
            Movements = new ObservableCollection<ProviderOrderMovementsView>();
            if (!(ProviderOrders is null) && (ProviderOrders.Count > 0))
            {
                foreach (ProviderOrdersView providerOrder in ProviderOrders)
                {
                    foreach (ProviderOrderMovementsView Movement in GetProviderOrderMovements(providerOrder.ProviderOrder_Id))
                    {
                        Movements.Add(Movement);
                    }
                }
                decimal EarlyPaymentDiscountPrecent = Provider.BillingData_EarlyPaymentDiscount;
                decimal IVAPercent = (Provider.BillingData_IVAType is null) ? 0 : Provider.BillingData_IVAType.IVAPercent;
                decimal SurchargePercent = (Provider.BillingData_IVAType is null) ? 0 : Provider.BillingData_IVAType.SurchargePercent;
                CalculateAmountInfo(Movements, EarlyPaymentDiscountPrecent, IVAPercent, SurchargePercent,
                                    out GrossAmount, out EarlyPayementDiscountAmount, out TaxableBaseAmount,
                                    out IVAAmount, out SurchargeAmount, out TotalAmount);
            }
            else
            {
                GrossAmount = 0;
                EarlyPayementDiscountAmount = 0;
                TaxableBaseAmount = 0;
                IVAAmount = 0;
                SurchargeAmount = 0;
                TotalAmount = 0;
            }
        }
              
        public void CalculateAmountInfo(CustomersView Customer, BillsView Bill, out decimal TotalAmount)
        {
            if (!(CustomerOrders is null) && (CustomerOrders.Count > 0))
            {
                ObservableCollection<CustomerOrderMovementsView> Movements = new ObservableCollection<CustomerOrderMovementsView>();
                foreach (CustomerOrdersView customerOrder in Bill.CustomerOrders)
                {
                    foreach (CustomerOrderMovementsView Movement in GetCustomerOrderMovements(customerOrder.CustomerOrder_Id))
                    {
                        Movements.Add(Movement);
                    }
                }
                decimal EarlyPaymentDiscountPrecent = Customer.BillingData_EarlyPaymentDiscount;
                decimal IVAPercent = (Customer.BillingData_IVAType is null) ? 0 : Customer.BillingData_IVAType.IVAPercent;
                decimal SurchargePercent = (Customer.BillingData_IVAType is null) ? 0 : Customer.BillingData_IVAType.SurchargePercent;
                CalculateAmountInfo(Movements, EarlyPaymentDiscountPrecent, IVAPercent, SurchargePercent,
                                    out decimal GrossAmount, out decimal EarlyPayementDiscountAmount, out decimal TaxableBaseAmount,
                                    out decimal IVAAmount, out decimal SurchargeAmount, out TotalAmount);
            }
            else TotalAmount = 0;
        }

        public void CalculateAmountInfo(ProvidersView Provider, BillsView Bill, out decimal TotalAmount)
        {
            if (!(ProviderOrders is null) && (ProviderOrders.Count > 0))
            {
                ObservableCollection<ProviderOrderMovementsView> Movements = new ObservableCollection<ProviderOrderMovementsView>();
                foreach (ProviderOrdersView providerOrder in Bill.ProviderOrders)
                {
                    foreach (ProviderOrderMovementsView Movement in GetProviderOrderMovements(providerOrder.ProviderOrder_Id))
                    {
                        Movements.Add(Movement);
                    }
                }
                decimal EarlyPaymentDiscountPrecent = Provider.BillingData_EarlyPaymentDiscount;
                decimal IVAPercent = (Provider.BillingData_IVAType is null) ? 0 : Provider.BillingData_IVAType.IVAPercent;
                decimal SurchargePercent = (Provider.BillingData_IVAType is null) ? 0 : Provider.BillingData_IVAType.SurchargePercent;
                CalculateAmountInfo(Movements, EarlyPaymentDiscountPrecent, IVAPercent, SurchargePercent,
                                    out decimal GrossAmount, out decimal EarlyPayementDiscountAmount, out decimal TaxableBaseAmount,
                                    out decimal IVAAmount, out decimal SurchargeAmount, out TotalAmount);
            }
            else TotalAmount = 0;
        }

        public void CalculateAmountInfo(BillsView Bill,
                                        out decimal GrossAmount, out decimal EarlyPayementDiscountAmount, out decimal TaxableBaseAmount,
                                        out decimal IVAAmount, out decimal SurchargeAmount, out decimal TotalAmount)
        {
            if (!(Bill.CustomerOrders is null) && (Bill.CustomerOrders.Count > 0))
            {
                ObservableCollection<CustomerOrderMovementsView> Movements = new ObservableCollection<CustomerOrderMovementsView>();
                foreach (CustomerOrdersView customerOrder in Bill.CustomerOrders)
                {
                    foreach (CustomerOrderMovementsView Movement in GetCustomerOrderMovements(customerOrder.CustomerOrder_Id))
                    {
                        Movements.Add(Movement);
                    }
                }
                CustomersView Customer = Bill.Customer;
                IVATypesView IVAType = Customer.BillingData_IVAType;
                decimal EarlyPaymentDiscountPrecent = Customer.BillingData_EarlyPaymentDiscount;
                decimal IVAPercent = (IVAType is null) ? 0 : IVAType.IVAPercent;
                decimal SurchargePercent = (IVAType is null) ? 0 : IVAType.SurchargePercent;
                CalculateAmountInfo(Movements, EarlyPaymentDiscountPrecent, IVAPercent, SurchargePercent,
                                    out GrossAmount, out EarlyPayementDiscountAmount, out TaxableBaseAmount,
                                    out IVAAmount, out SurchargeAmount, out TotalAmount);
            }
            else if (!(Bill.ProviderOrders is null) && (Bill.ProviderOrders.Count > 0))
            {
                ObservableCollection<ProviderOrderMovementsView> Movements = new ObservableCollection<ProviderOrderMovementsView>();
                foreach (ProviderOrdersView providerOrder in Bill.ProviderOrders)
                {
                    foreach (ProviderOrderMovementsView Movement in GetProviderOrderMovements(providerOrder.ProviderOrder_Id))
                    {
                        Movements.Add(Movement);
                    }
                }
                CustomersView Customer = Bill.Customer;
                IVATypesView IVAType = Customer.BillingData_IVAType;
                decimal EarlyPaymentDiscountPrecent = Customer.BillingData_EarlyPaymentDiscount;
                decimal IVAPercent = (IVAType is null) ? 0 : IVAType.IVAPercent;
                decimal SurchargePercent = (IVAType is null) ? 0 : IVAType.SurchargePercent;
                CalculateAmountInfo(Movements, EarlyPaymentDiscountPrecent, IVAPercent, SurchargePercent,
                                    out GrossAmount, out EarlyPayementDiscountAmount, out TaxableBaseAmount,
                                    out IVAAmount, out SurchargeAmount, out TotalAmount);
            }
            else
            {
                GrossAmount = 0;
                EarlyPayementDiscountAmount = 0;
                TaxableBaseAmount = 0;
                IVAAmount = 0;
                SurchargeAmount = 0;
                TotalAmount = 0;
            }
        }

        public void CalculateAmountInfo(CustomerOrdersView CustomerOrder,
                                        out decimal GrossAmount, out decimal EarlyPayementDiscountAmount, out decimal TaxableBaseAmount,
                                        out decimal IVAAmount, out decimal SurchargeAmount, out decimal TotalAmount)
        {
            if (!(CustomerOrder is null))
            {
                ObservableCollection<CustomerOrderMovementsView> Movements = new ObservableCollection<CustomerOrderMovementsView>();
                foreach (CustomerOrderMovementsView Movement in GetCustomerOrderMovements(CustomerOrder.CustomerOrder_Id))
                {
                    Movements.Add(Movement);
                }
                CustomersView Customer = CustomerOrder.Customer;
                IVATypesView IVAType = Customer.BillingData_IVAType;
                decimal EarlyPaymentDiscountPrecent = Customer.BillingData_EarlyPaymentDiscount;
                decimal IVAPercent = (IVAType is null) ? 0 : IVAType.IVAPercent;
                decimal SurchargePercent = (IVAType is null) ? 0 : IVAType.SurchargePercent;
                CalculateAmountInfo(Movements, EarlyPaymentDiscountPrecent, IVAPercent, SurchargePercent,
                                    out GrossAmount, out EarlyPayementDiscountAmount, out TaxableBaseAmount,
                                    out IVAAmount, out SurchargeAmount, out TotalAmount);
            }
            else
            {
                GrossAmount = 0;
                EarlyPayementDiscountAmount = 0;
                TaxableBaseAmount = 0;
                IVAAmount = 0;
                SurchargeAmount = 0;
                TotalAmount = 0;
            }
        }

        public void CalculateAmountInfo(ObservableCollection<CustomerOrderMovementsView> Movements, 
                                        decimal EarlyPaymentDiscountPrecent, decimal IVAPercent, decimal SurchargePercent,
                                        out decimal GrossAmount, out decimal EarlyPayementDiscountAmount, out decimal TaxableBaseAmount,
                                        out decimal IVAAmount, out decimal SurchargeAmount, out decimal TotalAmount)
        {
            //  Calculate GrossAmount amount value.
                GrossAmount = 0;
                foreach (CustomerOrderMovementsView movement in Movements)
                {
                    GrossAmount += movement.Amount;
                }
            //  Calculate Early Payement Discount amount value.
                EarlyPayementDiscountAmount = 0;
                if (EarlyPaymentDiscountPrecent > 0)
                {
                    EarlyPayementDiscountAmount = GlobalViewModel.GetValueDecimalForCalculations(GrossAmount * (EarlyPaymentDiscountPrecent / 100), DecimalType.Currency); 
                }
            //  Calculate Taxable Base
                TaxableBaseAmount = GlobalViewModel.GetValueDecimalForCalculations(GrossAmount - EarlyPayementDiscountAmount, DecimalType.Currency);
            //  Calculate IVA amount value.
                IVAAmount = 0;
                if (IVAPercent > 0)
                {
                    //  We use the TaxableBaseAmount to apply the Early Payement Discount
                        IVAAmount = GlobalViewModel.GetValueDecimalForCalculations(TaxableBaseAmount * (IVAPercent / 100), DecimalType.Currency); 
                }
            //  Calculate Surcharge amount value.
                SurchargeAmount = 0;
                if (SurchargePercent > 0)
                {
                    //  We use the TaxableBaseAmount to apply the Early Payement Discount
                        SurchargeAmount = GlobalViewModel.GetValueDecimalForCalculations(TaxableBaseAmount * (SurchargePercent / 100), DecimalType.Currency);
                }
                TotalAmount = GlobalViewModel.GetValueDecimalForCalculations(TaxableBaseAmount + IVAAmount + SurchargeAmount, DecimalType.Currency);
        }

        public void CalculateAmountInfo(ObservableCollection<ProviderOrderMovementsView> Movements,
                                        decimal EarlyPaymentDiscountPrecent, decimal IVAPercent, decimal SurchargePercent,
                                        out decimal GrossAmount, out decimal EarlyPayementDiscountAmount, out decimal TaxableBaseAmount,
                                        out decimal IVAAmount, out decimal SurchargeAmount, out decimal TotalAmount)
        {
            //  Calculate GrossAmount amount value.
            GrossAmount = 0;
            foreach (ProviderOrderMovementsView movement in Movements)
            {
                GrossAmount += movement.Amount;
            }
            //  Calculate Early Payement Discount amount value.
            EarlyPayementDiscountAmount = 0;
            if (EarlyPaymentDiscountPrecent > 0)
            {
                EarlyPayementDiscountAmount = GlobalViewModel.GetValueDecimalForCalculations(GrossAmount * (EarlyPaymentDiscountPrecent / 100), DecimalType.Currency);
            }
            //  Calculate Taxable Base
            TaxableBaseAmount = GlobalViewModel.GetValueDecimalForCalculations(GrossAmount - EarlyPayementDiscountAmount, DecimalType.Currency);
            //  Calculate IVA amount value.
            IVAAmount = 0;
            if (IVAPercent > 0)
            {
                //  We use the TaxableBaseAmount to apply the Early Payement Discount
                IVAAmount = GlobalViewModel.GetValueDecimalForCalculations(TaxableBaseAmount * (IVAPercent / 100), DecimalType.Currency);
            }
            //  Calculate Surcharge amount value.
            SurchargeAmount = 0;
            if (SurchargePercent > 0)
            {
                //  We use the TaxableBaseAmount to apply the Early Payement Discount
                SurchargeAmount = GlobalViewModel.GetValueDecimalForCalculations(TaxableBaseAmount * (SurchargePercent / 100), DecimalType.Currency);
            }
            TotalAmount = GlobalViewModel.GetValueDecimalForCalculations(TaxableBaseAmount + IVAAmount + SurchargeAmount, DecimalType.Currency);
        }

        #endregion

        #region Calculate Receipts

        public void ActualizeReceiptsAmount(ObservableCollection<ReceiptsView> Receipts, decimal BillAmount)
        {
            if ((Receipts is null) || (Receipts.Count < 1))
            {
                throw new Exception("No s'han trobat rebuts a normalitzar.");
            }
            else if (Receipts.Count == 1)
            {
                Receipts[0].Amount = BillAmount;
            }
            else
            {
                decimal ReceiptAmount = GlobalViewModel.GetValueDecimalForCalculations(BillAmount / Receipts.Count, DecimalType.Currency);
                decimal RestAmount = GlobalViewModel.GetValueDecimalForCalculations(BillAmount, DecimalType.Currency);
                for (int i = 0; i < Receipts.Count; i++)
                {
                    Receipts[i].Amount = (i == Receipts.Count - 1) ? RestAmount : ReceiptAmount;
                    RestAmount = RestAmount - ReceiptAmount;
                }
            }
        }

        public List<HispaniaCompData.Receipt> CalculateReceipts(CustomersView Customer, decimal BillAmount, DateTime Bill_Date)
        {
            //  Load the information needed to calculate the receipts
                decimal NumEffects = Customer.DataBank_NumEffect;
                decimal FirstExpirationData = Customer.DataBank_FirstExpirationData;
                decimal ExpirationInterval = Customer.DataBank_ExpirationInterval;
                decimal Payday_1 = Customer.DataBank_Payday_1;
                decimal Payday_2 = Customer.DataBank_Payday_2;
                decimal Payday_3 = Customer.DataBank_Payday_3;
            //  Return the list of receipts calculated
                return CalculateReceipts(Bill_Date, NumEffects, FirstExpirationData, ExpirationInterval, Payday_1, Payday_2, Payday_3, BillAmount);
        }

        public List<HispaniaCompData.Receipt> CalculateReceipts(ProvidersView Provider, decimal BillAmount, DateTime Bill_Date)
        {
            //  Load the information needed to calculate the receipts
            decimal NumEffects = Provider.DataBank_NumEffect;
            decimal FirstExpirationData = Provider.DataBank_FirstExpirationData;
            decimal ExpirationInterval = Provider.DataBank_ExpirationInterval;
            decimal Payday_1 = Provider.DataBank_Payday_1;
            decimal Payday_2 = Provider.DataBank_Payday_2;
            decimal Payday_3 = Provider.DataBank_Payday_3;
            //  Return the list of receipts calculated
            return CalculateReceipts(Bill_Date, NumEffects, FirstExpirationData, ExpirationInterval, Payday_1, Payday_2, Payday_3, BillAmount);
        }

        public List<HispaniaCompData.Receipt> CalculateReceipts(BillsView Bill)
        {
            //  Load the information needed to calculate the receipts
                decimal NumEffects = Bill.DataBank_NumEffect;
                decimal FirstExpirationData = Bill.DataBank_FirstExpirationData;
                decimal ExpirationInterval = Bill.DataBank_ExpirationInterval;
                decimal Payday_1 = Bill.DataBank_Payday_1;
                decimal Payday_2 = Bill.DataBank_Payday_2;
                decimal Payday_3 = Bill.DataBank_Payday_3;
            //  Return the list of receipts calculated
                return CalculateReceipts(Bill.Date, NumEffects, FirstExpirationData, ExpirationInterval, Payday_1, Payday_2, Payday_3, Bill.TotalAmount);
        }

        private List<HispaniaCompData.Receipt> CalculateReceipts(DateTime Bill_Date, 
                                                                 decimal NumEffects,
                                                                 decimal FirstExpirationData, 
                                                                 decimal ExpirationInterval,
                                                                 decimal Payday_1,
                                                                 decimal Payday_2,
                                                                 decimal Payday_3,
                                                                 decimal BillAmount)
        {
            //  Initialize the data structure to return
                List<HispaniaCompData.Receipt> receipts = new List<HispaniaCompData.Receipt>();
            //  Setep 1 : determine the number of receipts that can be created
            if (NumEffects < 1)
            {
                throw new Exception("Per generar els rebuts el número d'efectes del client ha de ser com a mínim 1.");
            }
            else if (NumEffects == 1)
            {
                //  Only can create one Receipt. Fields 'Receipt_Id' and 'Bill_...' are filled in the save of the Receipt
                //  and can't assign value in this point.
                    receipts.Add(new HispaniaCompData.Receipt()
                    {
                        Expiration_Date = CalculateOneReceiptExpirationDate1(Bill_Date, FirstExpirationData, Payday_1, Payday_2, Payday_3),
                        Amount = BillAmount,
                        Paid = false,
                        Returned = false,
                        Expired = false,
                        Print = false,
                        SendByEMail = false
                    });
            }
            else if (NumEffects > 1)
            {
                //  It's needed create NumEffects Receipts. Fields 'Receipt_Id' and 'Bill_...' are filled in the save of the 
                //  Receipt and can't assign value in this point.
                    SortedDictionary<int, DateTime> ReceiptExpirationDates;
                    ReceiptExpirationDates = CalculateReceiptExpirationDate(Bill_Date, NumEffects, FirstExpirationData, ExpirationInterval, Payday_1, Payday_2, Payday_3);
                    decimal ReceiptAmount = GlobalViewModel.GetValueDecimalForCalculations(BillAmount / NumEffects, DecimalType.Currency);
                    decimal RestAmount = GlobalViewModel.GetValueDecimalForCalculations(BillAmount, DecimalType.Currency);
                    for (int i = 1; i <= NumEffects; i++)
                    {
                        receipts.Add(new HispaniaCompData.Receipt()
                        {
                            Expiration_Date = ReceiptExpirationDates[i],
                            Amount = (i == NumEffects) ? RestAmount : ReceiptAmount,
                            Paid = false,
                            Returned = false,
                            Expired = false,
                            Print = false,
                            SendByEMail = false
                        });
                        RestAmount = RestAmount - ReceiptAmount;
                    }
            }
            //  Return calculated Receipts
                return receipts;
        }

        private DateTime CalculateOneReceiptExpirationDate1(DateTime Bill_Date, decimal FirstExpirationData,
                                                            decimal Payday_1, decimal Payday_2, decimal Payday_3)
        {
            //  Calculated the estimated Expiration Date
                DateTime Expiration_Date = Bill_Date.AddDays((double)FirstExpirationData);
            //  Actualize calculation variables
                int Expiration_Year = Expiration_Date.Year;
                int Expiration_Month = Expiration_Date.Month;
                int Expiration_Day = Expiration_Date.Day;
            //  Init proces of calculation
                if ((Payday_1 == 0) && (Payday_2 == 0) && (Payday_3 == 0)) return Expiration_Date;
                else if (Expiration_Day <= Payday_1)
                {
                    Expiration_Day = (Payday_1 == 0) ? 1 : (int)Payday_1;
                    if ((Expiration_Month == 2) && (Expiration_Day > DateTime.DaysInMonth(Bill_Date.Year, 2)))
                    {
                        Expiration_Day = DateTime.DaysInMonth(Bill_Date.Year, 2);
                    }
                }
                else if ((Expiration_Day > Payday_1) && (Expiration_Day <= Payday_2))
                {
                    Expiration_Day = (int)Payday_2;
                    if ((Expiration_Month == 2) && (Expiration_Day > DateTime.DaysInMonth(Bill_Date.Year, 2)))
                    {
                        Expiration_Day = DateTime.DaysInMonth(Bill_Date.Year, 2);
                    }
                }
                else if ((Expiration_Day > Payday_2) && (Expiration_Day <= Payday_3))
                {
                    Expiration_Day = (int)Payday_3;
                    if ((Expiration_Month == 2) && (Expiration_Day > DateTime.DaysInMonth(Bill_Date.Year, 2)))
                    {
                        Expiration_Day = DateTime.DaysInMonth(Bill_Date.Year, 2);
                    }
                }
                else
                {
                    Expiration_Day = (Payday_1 == 0)? 1 : (int)Payday_1;
                    Expiration_Month++;
                    if (Expiration_Month > 12)
                    {
                        Expiration_Month = 1;
                        Expiration_Year++;
                    }
                    if ((Expiration_Month == 2) && (Expiration_Day > DateTime.DaysInMonth(Bill_Date.Year, 2)))
                    {
                        Expiration_Day = DateTime.DaysInMonth(Bill_Date.Year, 2);
                    }
                }
            //  Return the data calculated
                return (new DateTime(Expiration_Year, Expiration_Month, Expiration_Day));
        }
        
        private DateTime CalculateOneReceiptExpirationDate(DateTime Bill_Date, decimal FirstExpirationData,
                                                           decimal Payday_1, decimal Payday_2, decimal Payday_3)
        {
            //  Calculated the estimated Expiration Date
                DateTime Expiration_Date = Bill_Date.AddDays((double)FirstExpirationData);
            //  Actualize calculation variables
                int Expiration_Year = Expiration_Date.Year;
                int Expiration_Month = Expiration_Date.Month;
                int Expiration_Day = Expiration_Date.Day;
            //  Init proces of calculation
                if ((Payday_1 == 0) && (Payday_2 == 0) && (Payday_3 == 0)) return Expiration_Date;
                else if (Expiration_Day <= Payday_1)
                {
                    Expiration_Day = (Payday_1 == 0) ? 1 : (int)Payday_1;
                    if ((Expiration_Month == 2) && (Expiration_Day > DateTime.DaysInMonth(Bill_Date.Year, 2)))
                    {
                        Expiration_Day = DateTime.DaysInMonth(Bill_Date.Year, 2);
                    }
                }
                else if ((Expiration_Day > Payday_1) && (Expiration_Day <= Payday_2))
                {
                    Expiration_Day = (int)Payday_2;
                    if ((Expiration_Month == 2) && (Expiration_Day > DateTime.DaysInMonth(Bill_Date.Year, 2)))
                    {
                        Expiration_Day = DateTime.DaysInMonth(Bill_Date.Year, 2);
                    }
                }
                else if ((Expiration_Day > Payday_2) && (Expiration_Day <= Payday_3))
                {
                    Expiration_Day = (int)Payday_3;
                    if ((Expiration_Month == 2) && (Expiration_Day > DateTime.DaysInMonth(Bill_Date.Year, 2)))
                    {
                        Expiration_Day = DateTime.DaysInMonth(Bill_Date.Year, 2);
                    }
                }
                else
                {
                    Expiration_Day = (Payday_1 == 0)? 1 : (int)Payday_1;
                    Expiration_Month++;
                    if (Expiration_Month > 12)
                    {
                        Expiration_Month = 1;
                        Expiration_Year++;
                    }
                    if ((Expiration_Month == 2) && (Expiration_Day > DateTime.DaysInMonth(Bill_Date.Year, 2)))
                    {
                        Expiration_Day = DateTime.DaysInMonth(Bill_Date.Year, 2);
                    }
                }
            //  Return the data calculated
                return (new DateTime(Expiration_Year, Expiration_Month, Expiration_Day));
        }

        private SortedDictionary<int, DateTime> CalculateReceiptExpirationDate(DateTime Bill_Date, decimal NumEffects,
                                                                               decimal FirstExpirationData, decimal ExpirationInterval,
                                                                               decimal Payday_1, decimal Payday_2, decimal Payday_3)
        {
            SortedDictionary<int, DateTime> Expiration_Dates = new SortedDictionary<int, DateTime>();
            DateTime FirsDate = CalculateOneReceiptExpirationDate(Bill_Date, FirstExpirationData, Payday_1, Payday_2, Payday_3);
            Expiration_Dates.Add(1, FirsDate);
            for (int i = 2; i <= NumEffects; i++)
            {
                DateTime NextDate = CalculateOneReceiptExpirationDate(Expiration_Dates[i - 1], ExpirationInterval, Payday_1, Payday_2, Payday_3);
                Expiration_Dates.Add(i, NextDate);
            }
            return Expiration_Dates;
        }

        #endregion

        #region Calculate Goods Acum

        public Dictionary<int, Pair> CalculateGoodsAmountValue(ObservableCollection<CustomerOrderMovementsView> Movements)
        {
            Dictionary<int, Pair> GoodsAmountValue = new Dictionary<int, Pair>();
            foreach (CustomerOrderMovementsView Movement in Movements)
            {
                if (!GoodsAmountValue.ContainsKey(Movement.Good.Good_Id))
                {
                    GoodsAmountValue.Add(Movement.Good.Good_Id, new Pair(Movement.Amount, Movement.AmountCost));
                }
                else
                {
                    GoodsAmountValue[Movement.Good.Good_Id].Amount += Movement.Amount;
                    GoodsAmountValue[Movement.Good.Good_Id].AmountCost += Movement.AmountCost;
                }
            }
            return (GoodsAmountValue);
        }

        public Dictionary<int, Pair> CalculateGoodsAmountValue(ObservableCollection<ProviderOrderMovementsView> Movements)
        {
            Dictionary<int, Pair> GoodsAmountValue = new Dictionary<int, Pair>();
            foreach (ProviderOrderMovementsView Movement in Movements)
            {
                if (!GoodsAmountValue.ContainsKey(Movement.Good.Good_Id))
                {
                    GoodsAmountValue.Add(Movement.Good.Good_Id, new Pair(Movement.Amount, Movement.AmountCost));
                }
                else
                {
                    GoodsAmountValue[Movement.Good.Good_Id].Amount += Movement.Amount;
                    GoodsAmountValue[Movement.Good.Good_Id].AmountCost += Movement.AmountCost;
                }
            }
            return (GoodsAmountValue);
        }

        #endregion

        #region Calculate Update of Good Average Price Cost

        public void UpdateGoodUnitInfo(GoodsView good, decimal Price, decimal Amount_Unit_Billing_In, decimal Amount_Unit_Shipping_In, 
                                       TypeOfUpdateAVGPC TypeOfCalculation, out GoodsView Updated_SelectedGood)
        {
            HispaniaCompData.Good Good = good.GetGood();
            UpdateGoodUnitInfo(Good, Price, Amount_Unit_Billing_In, Amount_Unit_Shipping_In, TypeOfCalculation);
            Updated_SelectedGood = new GoodsView(Good);
        }

        public void UpdateGoodUnitInfo(HispaniaCompData.Good good, decimal Price, decimal Amount_Unit_Billing_In, decimal Amount_Unit_Shipping_In, 
                                       TypeOfUpdateAVGPC TypeOfCalculation, bool UpdateCosts = false)
        {
            if (UpdateCosts) UpdateAveragePriceCost(good, Amount_Unit_Billing_In, Price, TypeOfCalculation);
            decimal Amount_Unit_Billing = GlobalViewModel.GetValueDecimalForCalculations(Amount_Unit_Billing_In, DecimalType.Unit);
            decimal Amount_Unit_Shipping = GlobalViewModel.GetValueDecimalForCalculations(Amount_Unit_Shipping_In, DecimalType.Unit); 
            switch (TypeOfCalculation)
            {
                case TypeOfUpdateAVGPC.AddUnitsEntry:
                     if (UpdateCosts) good.Price_Cost = Price;
                     good.Billing_Unit_Stocks += Amount_Unit_Billing;
                     good.Billing_Unit_Available += Amount_Unit_Billing;
                     good.Billing_Unit_Entrance += Amount_Unit_Billing;
                     good.Shipping_Unit_Stocks += Amount_Unit_Shipping;
                     good.Shipping_Unit_Available += Amount_Unit_Shipping;
                     good.Shipping_Unit_Entrance += Amount_Unit_Shipping;
                     break;
                case TypeOfUpdateAVGPC.AddUnitsDeparture:
                     good.Billing_Unit_Stocks -= Amount_Unit_Billing;
                     good.Billing_Unit_Available -= Amount_Unit_Billing;
                     good.Billing_Unit_Departure += Amount_Unit_Billing;
                     good.Shipping_Unit_Stocks -= Amount_Unit_Shipping;
                     good.Shipping_Unit_Available -= Amount_Unit_Shipping;
                     good.Shipping_Unit_Departure += Amount_Unit_Shipping;
                     break;

                case TypeOfUpdateAVGPC.RemoveUnitsEntry:
                     good.Billing_Unit_Stocks -= Amount_Unit_Billing;
                     good.Billing_Unit_Available -= Amount_Unit_Billing;
                     good.Billing_Unit_Entrance -= Amount_Unit_Billing;
                     good.Shipping_Unit_Stocks -= Amount_Unit_Shipping;
                     good.Shipping_Unit_Available -= Amount_Unit_Shipping;
                     good.Shipping_Unit_Entrance -= Amount_Unit_Shipping;
                     break;
                case TypeOfUpdateAVGPC.RemoveUnitsDeparture:
                     good.Billing_Unit_Stocks += Amount_Unit_Billing;
                     good.Billing_Unit_Available += Amount_Unit_Billing;
                     good.Billing_Unit_Departure -= Amount_Unit_Billing;
                     good.Shipping_Unit_Stocks += Amount_Unit_Shipping;
                     good.Shipping_Unit_Available += Amount_Unit_Shipping;
                     good.Shipping_Unit_Departure -= Amount_Unit_Shipping;
                     break;
                default:
                     throw new ArgumentException(string.Format("Error, type of calculation '{0}' for UpdateGoodUnitInfo not recognized", TypeOfCalculation.ToString()));
            }
        }

        /// <summary>
        ///  - El preu mig s'ha de calcular de la següent manera:
        ///       Si((SA + UC) == 0) PMS = PMA €
        ///       Sinó PMS = (((SA * PMA) + (UC * PC)) / (SA + UC)) €
        ///       Essent: PMS = Preu Mig en Stock(UE)
        ///               SA = Stock Actual
        ///               PMA = Precio Medio Actual
        ///               UC = Unidades de la compra, suma de las unidades de entrada en un mismo día.
        ///               PC =  Precio de la compra, precio medio de las entradas en un mismo día.
        /// </summary>
        /// <param name="good"></param>
        /// <param name="Amount_Unit_Billing"></param>
        /// <param name="Price"></param>
        /// <param name="TypeOfCalculation"></param>
        public void UpdateAveragePriceCost(HispaniaCompData.Good good, decimal Amount_Unit_Billing, decimal Price, TypeOfUpdateAVGPC TypeOfCalculation)
        {
            //  If new Amount Unit Billing is zero the PMS don't change.
                if (Amount_Unit_Billing == 0) return;
            //  Else is needed calculate the new value.
                decimal PMA = GlobalViewModel.GetValueDecimalForCalculations((decimal)good.Average_Price_Cost, DecimalType.Currency); // PMA
                decimal SA = GlobalViewModel.GetValueDecimalForCalculations((decimal)good.Billing_Unit_Stocks, DecimalType.Unit);     // SA
                decimal UC = GlobalViewModel.GetValueDecimalForCalculations((decimal)Amount_Unit_Billing, DecimalType.Unit);          // UC
                decimal PC = GlobalViewModel.GetValueDecimalForCalculations((decimal)Price, DecimalType.Unit);                        // PC
                switch (TypeOfCalculation)
                {
                    case TypeOfUpdateAVGPC.AddUnitsEntry:
                         if (SA + UC == 0) good.Average_Price_Cost = PMA;
                         else
                         { 
                             if (SA < 0)
                             {
                                decimal Average_Price_Cost = ((-SA * PMA) + (UC * PC)) / (-SA + UC);
                                good.Average_Price_Cost = GlobalViewModel.GetValueDecimalForCalculations(Average_Price_Cost, DecimalType.Currency);
                             }
                             else
                             {
                                decimal Average_Price_Cost = ((SA * PMA) + (UC * PC)) / (SA + UC);
                                good.Average_Price_Cost = GlobalViewModel.GetValueDecimalForCalculations(Average_Price_Cost, DecimalType.Currency);
                             }
                         }
                         break;                         
                    case TypeOfUpdateAVGPC.AddUnitsDeparture:
                         break;
                    case TypeOfUpdateAVGPC.RemoveUnitsEntry:
                         if (SA - UC <= 0) good.Average_Price_Cost = PMA;
                         else
                         {
                            decimal Average_Price_Cost = ((SA * PMA) - (UC * PC)) / (SA - UC);
                            good.Average_Price_Cost = GlobalViewModel.GetValueDecimalForCalculations(Average_Price_Cost, DecimalType.Currency);
                         }
                         break;
                    case TypeOfUpdateAVGPC.RemoveUnitsDeparture:
                         break;
                    default:
                         throw new ArgumentException(string.Format("Error, type of calculation '{0}' for UpdateAveragePriceCost not recognized", TypeOfCalculation.ToString()));
                }
        }

        #endregion
    }
}
