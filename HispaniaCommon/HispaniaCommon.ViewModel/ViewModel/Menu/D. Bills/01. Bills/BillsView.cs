#region Librerias usadas por la clase

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HispaniaCompData = HispaniaComptabilitat.Data;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Camps del Tipus
    /// </summary>
    public enum BillsAttributes
    {
        Year,
        BillingSerie,
        Date,
        Customer,
        Customer_Alias,
        Company_Name,
        Company_Cif,
        Company_Address,
        Company_PostalCode,
        Company_NumProv,
        DataBank_Bank,
        DataBank_BankAddress,
        DataBank_Effect,
        DataBank_NumEffect,
        DataBank_FirstExpirationData,
        DataBank_ExpirationInterval,
        DataBank_Payday_1,
        DataBank_Payday_2,
        DataBank_Payday_3,
        DataBank_IBAN_CountryCode,
        DataBank_IBAN_BankCode,
        DataBank_IBAN_OfficeCode,
        DataBank_IBAN_CheckDigits,
        DataBank_IBAN_AccountNumber,
        BillingData_EarlyPaymentDiscount,
        BillingData_NumUnpaid,
        BillingData_Agent,
        FileNamePDF,
        Customer_Remarks,
        Remarks,
        None,
    }

    /// <summary>
    /// Class that Store the information of a Bill.
    /// </summary>
    public class BillsView : IMenuView
    {
        #region Fields for Filter

        /// <summary>
        /// Store the list of fields that compose the Bill class
        /// </summary>
        private static Dictionary<string, string> m_Fields = null;

        /// <summary>
        /// Get the list of fields that compose the Bill class
        /// </summary>
        public static Dictionary<string, string> Fields
        {
            get
            {
                if (m_Fields == null)
                {
                    m_Fields = new Dictionary<string, string>
                    {
                        { "Numero de Factura", "Bill_Id" },
                        { "Any", "Year" },
                        { "Serie", "BillingSerie_Str" },
                        { "Data", "Bill_Date_Str" },
                        { "Nº Client", "Customer_Id" },
                        { "Client", "Customer_Alias" },
                        { "Imprès", "Print_Str" },
                        { "Enviat", "SendByEMail_Str" },
                        { "Forma Pagament", "EffectType_Str" },
                    };
                }
                return (m_Fields);
            }
        }

        #endregion

        #region Properties

        #region IMenuView Interface implementation

        public string GetKey
        {
            get
            {
                return string.Format("{0}{1}{2}",
                                     GlobalViewModel.GetStringFromIntIdValue(Bill_Id),
                                     GlobalViewModel.GetStringFromYearValue(Year),
                                     GlobalViewModel.GetStringFromIntIdValue(_BillingSerie_Id));
            }
        }

        #endregion

        #region Main Fields

        public int Bill_Id { get; set; }

        public string Bill_Id_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromIntIdValue(Bill_Id);
            }
        }
        public decimal Year { get; set; }
        public DateTime Date { get; set; }
        public string Bill_Date_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDateTimeValue(Date);
            }
        }
        public string FileNamePDF { get; set; }
        public decimal IVAPercent { get; set; }
        public decimal SurchargePercent { get; set; }

        #endregion

        #region Customer 
        public string Customer_Alias { get; set; }
        public string Company_Name { get; set; }
        public string Company_Cif { get; set; }
        public string Company_Address { get; set; }
        public string Company_NumProv { get; set; }

        #endregion

        #region Bank Data (Bancarios)

        public string DataBank_Bank { get; set; }
        public string DataBank_BankAddress { get; set; }
        public decimal DataBank_NumEffect { get; set; }
        public decimal DataBank_FirstExpirationData { get; set; }
        public decimal DataBank_ExpirationInterval { get; set; }
        public decimal DataBank_Payday_1 { get; set; }
        public decimal DataBank_Payday_2 { get; set; }
        public decimal DataBank_Payday_3 { get; set; }
        public string DataBank_IBAN_CountryCode { get; set; }
        public string DataBank_IBAN_BankCode { get; set; }
        public string DataBank_IBAN_OfficeCode { get; set; }
        public string DataBank_IBAN_CheckDigits { get; set; }
        public string DataBank_IBAN_AccountNumber { get; set; }

        #endregion

        #region Billing Data (Facturación)

        public decimal BillingData_EarlyPaymentDiscount { get; set; }
        public decimal BillingData_NumUnpaid { get; set; }

        #endregion

        #region Diversos

        public string Customer_Remarks { get; set; }
        public string Remarks { get; set; }
        public bool Print { get; set; }
        public string Print_Str
        {
            get
            {
                return (Print == true) ? "Imprés" : "No Imprés";
            }
        }
        public bool SendByEMail { get; set; }
        public string SendByEMail_Str
        {
            get
            {
                return (SendByEMail == true) ? "Generat" : "No Generat";
            }
        }

        #endregion

        #region ForeignKey Properties

        #region Customer

        public int Customer_Id { get; set; }

        private CustomersView _Customer;

        public CustomersView Customer
        {
            get
            {
                if ((_Customer == null) && (Customer_Id != GlobalViewModel.IntIdInitValue))
                {
                    _Customer = new CustomersView(GlobalViewModel.Instance.HispaniaViewModel.GetCustomer((int)Customer_Id));
                }
                return (_Customer);
            }
            set
            {
                if (value != null)
                {
                    _Customer = new CustomersView(value);
                    if (_Customer == null) Customer_Id = GlobalViewModel.IntIdInitValue;
                    else Customer_Id = _Customer.Customer_Id;
                }
                else
                {
                    _Customer = null;
                    Customer_Id = GlobalViewModel.IntIdInitValue;
                }
            }
        }

        public string Customer_Id_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromIntIdValue(Customer_Id);
            }
        }

        #endregion

        #region Billing Series

        private int _BillingSerie_Id { get; set; }

        private BillingSeriesView _BillingSerie;

        public BillingSeriesView BillingSerie
        {
            get
            {
                if ((_BillingSerie == null) && (_BillingSerie_Id != GlobalViewModel.IntIdInitValue))
                {
                    _BillingSerie = new BillingSeriesView(GlobalViewModel.Instance.HispaniaViewModel.GetBillingSerie((int)_BillingSerie_Id));
                }
                return (_BillingSerie);
            }
            set
            {
                if (value != null)
                {
                    _BillingSerie = new BillingSeriesView(value);
                    if (_BillingSerie == null) _BillingSerie_Id = GlobalViewModel.IntIdInitValue;
                    else _BillingSerie_Id = _BillingSerie.Serie_Id;
                }
                else
                {
                    _BillingSerie = null;
                    _BillingSerie_Id = GlobalViewModel.IntIdInitValue;
                }
            }
        }

        public string BillingSerie_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromBillingSerieValue(BillingSerie);
            }
        }

        #endregion

        #region Postal Code

        private int? _Company_PostalCode_Id { get; set; }

        private PostalCodesView _Company_PostalCode;

        public PostalCodesView Company_PostalCode
        {
            get
            {
                if ((_Company_PostalCode == null) && (_Company_PostalCode_Id != GlobalViewModel.IntIdInitValue) &&
                    (_Company_PostalCode_Id != null))
                {
                    _Company_PostalCode = new PostalCodesView(GlobalViewModel.Instance.HispaniaViewModel.GetPostalCode((int)_Company_PostalCode_Id));
                }
                return (_Company_PostalCode);
            }
            set
            {
                if (value != null)
                {
                    _Company_PostalCode = new PostalCodesView(value);
                    if (_Company_PostalCode == null) _Company_PostalCode_Id = GlobalViewModel.IntIdInitValue;
                    else _Company_PostalCode_Id = _Company_PostalCode.PostalCode_Id;
                }
                else
                {
                    _Company_PostalCode = null;
                    _Company_PostalCode_Id = GlobalViewModel.IntIdInitValue;
                }
            }
        }

        public string Company_PostalCode_Str
        {
            get
            {
                return (Company_PostalCode is null) ?
                              string.Empty :
                              string.Format("{0}, {1}", Company_PostalCode.Postal_Code, Company_PostalCode.City);
            }
        }

        #endregion

        #region Effect Type

        private int? _DataBank_EffectType_Id;

        public EffectTypesView _DataBank_EffectType;

        public EffectTypesView DataBank_EffectType
        {
            get
            {
                if ((_DataBank_EffectType == null) && (_DataBank_EffectType_Id != GlobalViewModel.IntIdInitValue) &&
                    (_DataBank_EffectType_Id != null))
                {
                    _DataBank_EffectType = new EffectTypesView(GlobalViewModel.Instance.HispaniaViewModel.GetEffectType((int)_DataBank_EffectType_Id));
                }
                return (_DataBank_EffectType);
            }
            set
            {
                if (value != null)
                {
                    _DataBank_EffectType = new EffectTypesView(value);
                    if (_DataBank_EffectType == null) _DataBank_EffectType_Id = GlobalViewModel.IntIdInitValue;
                    else _DataBank_EffectType_Id = _DataBank_EffectType.EffectType_Id;
                }
                else
                {
                    _DataBank_EffectType = null;
                    _DataBank_EffectType_Id = GlobalViewModel.IntIdInitValue;
                }
            }
        }

        public string EffectType_Str
        {
            get
            {
                return DataBank_EffectType is null ? "No Informat" : DataBank_EffectType.Description;
            }
        }

        #endregion

        #region Agent

        private int? _BillingData_Agent_Id;

        private AgentsView _BillingData_Agent;

        public AgentsView BillingData_Agent
        {
            get
            {
                if ((_BillingData_Agent == null) && (_BillingData_Agent_Id != GlobalViewModel.IntIdInitValue) &&
                    (_BillingData_Agent_Id != null))
                {
                    _BillingData_Agent = new AgentsView(GlobalViewModel.Instance.HispaniaViewModel.GetAgent((int)_BillingData_Agent_Id));
                }
                return (_BillingData_Agent);
            }
            set
            {
                if (value != null)
                {
                    _BillingData_Agent = new AgentsView(value);
                    if (_BillingData_Agent == null) _BillingData_Agent_Id = GlobalViewModel.IntIdInitValue;
                    else _BillingData_Agent_Id = _BillingData_Agent.Agent_Id;
                }
                else
                {
                    _BillingData_Agent = null;
                    _BillingData_Agent_Id = GlobalViewModel.IntIdInitValue;
                }
            }
        }

        #endregion

        #region Receipts

        private ObservableCollection<ReceiptsView> _Receipts;

        public ObservableCollection<ReceiptsView> Receipts
        {
            get
            {
                if (_Receipts == null)
                {
                    _Receipts = GlobalViewModel.Instance.HispaniaViewModel.GetReceiptsFromBill(Bill_Id, Year);
                }
                return (_Receipts);
            }
            set
            {
                _Receipts = value;
            }
        }

        #endregion

        #region CustomerOrders

        private ObservableCollection<CustomerOrdersView> _CustomerOrders;

        public ObservableCollection<CustomerOrdersView> CustomerOrders
        {
            get
            {
                if (_CustomerOrders == null)
                {
                    _CustomerOrders = GlobalViewModel.Instance.HispaniaViewModel.GetCustomerOrdersFromBill(Bill_Id, Year);
                }
                return (_CustomerOrders);
            }
        }

        #endregion

        #endregion

        #region Information

        public bool CanModifyBill
        {
            get
            {
                if (Receipts is null) return (true);
                else
                {
                    bool CanModifyBill = true;
                    foreach (ReceiptsView receipt in Receipts)
                    {
                        CanModifyBill &= (!receipt.Paid);
                    }
                    return (CanModifyBill);
                }
            }
        }

        public decimal AmountReceipts { get; set; }

        public decimal BillAmount { get; set; }

        #endregion

        #region Calculated

        public string ExpirationDate { get; set; }

        public decimal TotalAmount { get; set; }
        public string TotalAmount_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(TotalAmount, DecimalType.Currency, true);
            }
        }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public BillsView()
        {
            Bill_Id = -1;
            Date = DateTime.Now;
            Year = Date.Year;
            _BillingSerie_Id = 1; // Per defecte Sèrie de Facturació A (Factures normals (valor > 0)).
            Customer_Id = GlobalViewModel.IntIdInitValue;
            Customer_Alias = string.Empty;
            Company_Name = string.Empty;
            Company_Cif = string.Empty;
            Company_Address = string.Empty;
            Company_NumProv = string.Empty;
            DataBank_Bank = string.Empty;
            DataBank_BankAddress = string.Empty;
            DataBank_NumEffect = GlobalViewModel.DecimalInitValue;
            DataBank_FirstExpirationData = GlobalViewModel.DecimalInitValue;
            DataBank_ExpirationInterval = GlobalViewModel.DecimalInitValue;
            DataBank_Payday_1 = GlobalViewModel.DecimalInitValue;
            DataBank_Payday_2 = GlobalViewModel.DecimalInitValue;
            DataBank_Payday_3 = GlobalViewModel.DecimalInitValue;
            DataBank_IBAN_CountryCode = string.Empty;
            DataBank_IBAN_BankCode = string.Empty;
            DataBank_IBAN_OfficeCode = string.Empty;
            DataBank_IBAN_CheckDigits = string.Empty;
            DataBank_IBAN_AccountNumber = string.Empty;
            BillingData_EarlyPaymentDiscount = GlobalViewModel.DecimalInitValue;
            BillingData_NumUnpaid = GlobalViewModel.DecimalInitValue;
            Customer_Remarks = string.Empty;
            Remarks = string.Empty;
            Print = false;
            SendByEMail = false;
            FileNamePDF = string.Empty;
            ExpirationDate = string.Empty;
            TotalAmount = GlobalViewModel.DecimalInitValue;
            Receipts = null;
            Company_PostalCode = null;
            DataBank_EffectType = null;
            IVAPercent = GlobalViewModel.DecimalInitValue;
            SurchargePercent = GlobalViewModel.DecimalInitValue;
            BillingData_Agent = null;
            AmountReceipts = GlobalViewModel.IntIdInitValue;
            BillAmount = GlobalViewModel.IntIdInitValue;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal BillsView(HispaniaCompData.Bill bill)
        {
            Bill_Id = bill.Bill_Id;
            Date = GlobalViewModel.GetDateTimeValue(bill.Date);
            Year = bill.Year;
            _BillingSerie_Id = GlobalViewModel.GetIntFromIntIdValue(bill.Serie_Id);
            Customer_Id = GlobalViewModel.GetIntFromIntIdValue(bill.Customer_Id);
            Customer_Alias = bill.Customer_Alias;
            Company_Name = bill.Company_Name;
            Company_Cif = bill.Company_Cif;
            Company_Address = bill.Company_Address;
            Company_NumProv = bill.Company_NumProv;
            DataBank_Bank = bill.DataBank_Bank;
            DataBank_BankAddress = bill.DataBank_BankAddress;
            DataBank_NumEffect = GlobalViewModel.GetDecimalValue(bill.DataBank_NumEffect); 
            DataBank_FirstExpirationData = GlobalViewModel.GetDecimalValue(bill.DataBank_FirstExpirationData);
            DataBank_ExpirationInterval = GlobalViewModel.GetDecimalValue(bill.DataBank_ExpirationInterval);
            DataBank_Payday_1 = GlobalViewModel.GetDecimalValue(bill.DataBank_Payday_1);
            DataBank_Payday_2 = GlobalViewModel.GetDecimalValue(bill.DataBank_Payday_2);
            DataBank_Payday_3 = GlobalViewModel.GetDecimalValue(bill.DataBank_Payday_3);
            DataBank_IBAN_CountryCode = bill.DataBank_IBAN_CountryCode;
            DataBank_IBAN_BankCode = bill.DataBank_IBAN_BankCode;
            DataBank_IBAN_OfficeCode = bill.DataBank_IBAN_OfficeCode;
            DataBank_IBAN_CheckDigits = bill.DataBank_IBAN_CheckDigits;
            DataBank_IBAN_AccountNumber = bill.DataBank_IBAN_AccountNumber;
            BillingData_EarlyPaymentDiscount = GlobalViewModel.GetDecimalValue(bill.BillingData_EarlyPaymentDiscount);
            BillingData_NumUnpaid = GlobalViewModel.GetDecimalValue(bill.BillingData_NumUnpaid);
            Customer_Remarks = bill.Customer_Remarks;
            Remarks = bill.Remarks;
            Print = GlobalViewModel.GetBoolValue(bill.Print);
            SendByEMail = GlobalViewModel.GetBoolValue(bill.SendByEMail);
            FileNamePDF = bill.FileNamePDF;
            ExpirationDate = bill.ExpirationDate;
            TotalAmount = bill.TotalAmount;
            IVAPercent = GlobalViewModel.GetDecimalValue(bill.IVAPercent);
            SurchargePercent = GlobalViewModel.GetDecimalValue(bill.SurchargePercent);
            _Company_PostalCode_Id = GlobalViewModel.GetIntFromIntIdValue(bill.Company_PostalCode_Id);
            _DataBank_EffectType_Id = GlobalViewModel.GetIntFromIntIdValue(bill.DataBank_EffectType_Id);
            _BillingData_Agent_Id = GlobalViewModel.GetIntFromIntIdValue(bill.BillingData_Agent_Id);
            AmountReceipts = GlobalViewModel.IntIdInitValue;
            BillAmount = GlobalViewModel.IntIdInitValue;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public BillsView(BillsView bill)
        {
            Bill_Id = bill.Bill_Id;
            Date = bill.Date;
            Year = bill.Year;
            BillingSerie = bill.BillingSerie;
            Customer_Id = bill.Customer_Id;
            Customer_Alias = bill.Customer_Alias;
            Company_Name = bill.Company_Name;
            Company_Cif = bill.Company_Cif;
            Company_Address = bill.Company_Address;
            Company_NumProv = bill.Company_NumProv;
            DataBank_Bank = bill.DataBank_Bank;
            DataBank_BankAddress = bill.DataBank_BankAddress;
            DataBank_NumEffect = bill.DataBank_NumEffect; 
            DataBank_FirstExpirationData = bill.DataBank_FirstExpirationData;
            DataBank_ExpirationInterval = bill.DataBank_ExpirationInterval;
            DataBank_Payday_1 = bill.DataBank_Payday_1;
            DataBank_Payday_2 = bill.DataBank_Payday_2;
            DataBank_Payday_3 = bill.DataBank_Payday_3;
            DataBank_IBAN_CountryCode = bill.DataBank_IBAN_CountryCode;
            DataBank_IBAN_BankCode = bill.DataBank_IBAN_BankCode;
            DataBank_IBAN_OfficeCode = bill.DataBank_IBAN_OfficeCode;
            DataBank_IBAN_CheckDigits = bill.DataBank_IBAN_CheckDigits;
            DataBank_IBAN_AccountNumber = bill.DataBank_IBAN_AccountNumber;
            BillingData_EarlyPaymentDiscount = bill.BillingData_EarlyPaymentDiscount;
            BillingData_NumUnpaid = bill.BillingData_NumUnpaid;
            Customer_Remarks = bill.Customer_Remarks;
            Remarks = bill.Remarks;
            Print = bill.Print;
            SendByEMail = bill.SendByEMail;
            FileNamePDF = bill.FileNamePDF;
            ExpirationDate = bill.ExpirationDate;
            TotalAmount = bill.TotalAmount;
            ObservableCollection<ReceiptsView> ReceiptsIn = new ObservableCollection<ReceiptsView>();
            foreach (ReceiptsView Receipt in bill.Receipts)
            {
                ReceiptsIn.Add(new ReceiptsView(Receipt));
            }
            Receipts = ReceiptsIn;
            Company_PostalCode = bill.Company_PostalCode;
            DataBank_EffectType = bill.DataBank_EffectType;
            IVAPercent = bill.IVAPercent;
            SurchargePercent = bill.SurchargePercent;
            BillingData_Agent = bill.BillingData_Agent;
            AmountReceipts = bill.AmountReceipts;
            BillAmount = bill.AmountReceipts;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public BillsView(CustomersView customer)
        {
            Bill_Id = -1;         //  Aquest camp tindrà valor al crear l'Item a la Base de Dades
            Date = DateTime.Now;  //  Aquest camp tindrà valor al crear l'Item a la Base de Dades
            Year = Date.Year;     //  Aquest camp tindrà valor al crear l'Item a la Base de Dades
            _BillingSerie_Id = 1; //  Aquest camp tindrà valor al crear l'Item a la Base de Dades Sèrie de Facturació A (Factures normals (valor > 0)).
            Customer_Id = customer.Customer_Id;
            Customer_Alias = customer.Customer_Alias;
            Company_Name = customer.Company_Name;
            Company_Cif = customer.Company_Cif;
            Company_Address = customer.Company_Address;
            Company_NumProv = customer.Company_NumProv;
            DataBank_Bank = customer.DataBank_Bank;
            DataBank_BankAddress = customer.DataBank_BankAddress;
            DataBank_NumEffect = customer.DataBank_NumEffect;
            DataBank_FirstExpirationData = customer.DataBank_FirstExpirationData;
            DataBank_ExpirationInterval = customer.DataBank_ExpirationInterval;
            DataBank_Payday_1 = customer.DataBank_Payday_1;
            DataBank_Payday_2 = customer.DataBank_Payday_2;
            DataBank_Payday_3 = customer.DataBank_Payday_3;
            DataBank_IBAN_CountryCode = customer.DataBank_IBAN_CountryCode;
            DataBank_IBAN_BankCode = customer.DataBank_IBAN_BankCode;
            DataBank_IBAN_OfficeCode = customer.DataBank_IBAN_OfficeCode;
            DataBank_IBAN_CheckDigits = customer.DataBank_IBAN_CheckDigits;
            DataBank_IBAN_AccountNumber = customer.DataBank_IBAN_AccountNumber;
            BillingData_EarlyPaymentDiscount = customer.BillingData_EarlyPaymentDiscount;
            BillingData_NumUnpaid = customer.BillingData_NumUnpaid;
            Customer_Remarks = customer.Several_Remarks;
            Remarks = string.Empty;
            Print = false;
            SendByEMail = false;
            FileNamePDF = string.Empty;
            ExpirationDate = string.Empty;
            TotalAmount = GlobalViewModel.DecimalInitValue;
            Company_PostalCode = customer.Company_PostalCode;
            DataBank_EffectType = customer.DataBank_EffectType;
            IVAPercent = customer.BillingData_IVAType.IVAPercent;
            SurchargePercent = customer.BillingData_IVAType.SurchargePercent;
            BillingData_Agent = customer.BillingData_Agent;
        }

        #endregion

        #region GetBill

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.Bill GetBill()
        {
            HispaniaCompData.Bill Bill = new HispaniaCompData.Bill()
            {
                Bill_Id = Bill_Id,
                Date = Date,
                Year = Year,
                Serie_Id = _BillingSerie_Id,
                Customer_Id = Customer_Id,
                Customer_Alias = Customer_Alias,
                Company_Name = Company_Name,
                Company_Cif = Company_Cif,
                Company_Address = Company_Address,
                Company_NumProv = Company_NumProv,
                DataBank_Bank = DataBank_Bank,
                DataBank_BankAddress = DataBank_BankAddress,
                DataBank_NumEffect = DataBank_NumEffect,
                DataBank_FirstExpirationData = DataBank_FirstExpirationData,
                DataBank_ExpirationInterval = DataBank_ExpirationInterval,
                DataBank_Payday_1 = DataBank_Payday_1,
                DataBank_Payday_2 = DataBank_Payday_2,
                DataBank_Payday_3 = DataBank_Payday_3,
                DataBank_IBAN_CountryCode = DataBank_IBAN_CountryCode,
                DataBank_IBAN_BankCode = DataBank_IBAN_BankCode,
                DataBank_IBAN_OfficeCode = DataBank_IBAN_OfficeCode,
                DataBank_IBAN_CheckDigits = DataBank_IBAN_CheckDigits,
                DataBank_IBAN_AccountNumber = DataBank_IBAN_AccountNumber,
                BillingData_EarlyPaymentDiscount = BillingData_EarlyPaymentDiscount,
                BillingData_NumUnpaid = BillingData_NumUnpaid,
                Customer_Remarks = Customer_Remarks,
                Remarks = Remarks,
                Print = Print,
                SendByEMail = SendByEMail,
                FileNamePDF = FileNamePDF,
                ExpirationDate = ExpirationDate,
                TotalAmount = TotalAmount,
                Company_PostalCode_Id = _Company_PostalCode_Id,
                DataBank_EffectType_Id = _DataBank_EffectType_Id,
                IVAPercent = IVAPercent,
                SurchargePercent = SurchargePercent,
                BillingData_Agent_Id = _BillingData_Agent_Id
            };
            return (Bill);
        }

        #endregion

        #region Validate

        /// <summary>
        /// Validate the data contained in the instance of the class.
        /// </summary>
        public void Validate(out BillsAttributes ErrorField)
        {
            ErrorField = BillsAttributes.None;
            #region Main Fields
            if (!GlobalViewModel.IsYear(Year, "ANY DE LA FACTURA", out string ErrMsg))
            {
                ErrorField = BillsAttributes.Year;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsDateTime(Date, "DATA DE LA FACTURA", out ErrMsg))
            {
                ErrorField = BillsAttributes.Date;
                throw new FormatException(ErrMsg);
            }
            #endregion
            #region Customer
            if (!GlobalViewModel.IsName(Customer_Alias))
            {
                ErrorField = BillsAttributes.Customer_Alias;
                throw new FormatException("Error, valor no definit o format incorrecte de l'ALIAS");
            }
            if (!GlobalViewModel.IsName(Company_Name))
            {
                ErrorField = BillsAttributes.Company_Name;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
            if (!GlobalViewModel.IsEmptyOrCIF(Company_Cif))
            {
                ErrorField = BillsAttributes.Company_Cif;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrCIFError);
            }
            if (!GlobalViewModel.IsEmptyOrAddress(Company_Address))
            {
                ErrorField = BillsAttributes.Company_Address;
                throw new FormatException("Error, format incorrecte de l'ADREÇA DEL CLIENT");
            }
            //if (!GlobalViewModel.IsEmptyOrUint(Company_NumProv, "NUMERO DE PROVEÏDOR", out ErrMsg))
            //{
            //    ErrorField = BillsAttributes.Company_NumProv;
            //    throw new FormatException(ErrMsg);
            //}
            #endregion
            #region Bank Data (Bancaris)
            if (!GlobalViewModel.IsEmptyOrName(DataBank_Bank))
            {
                ErrorField = BillsAttributes.DataBank_Bank;
                throw new FormatException("Error, format incorrecte del NOM DEL BANC");
            }
            if (!GlobalViewModel.IsEmptyOrAddress(DataBank_BankAddress))
            {
                ErrorField = BillsAttributes.DataBank_BankAddress;
                throw new FormatException("Error, format incorrecte de l'ADREÇA DEL BANC");
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(DataBank_NumEffect, "NUMERO D''EFECTE", out ErrMsg))
            {
                ErrorField = BillsAttributes.DataBank_NumEffect;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(DataBank_FirstExpirationData, "DIES DE PRIMER VENCIMENT", out ErrMsg))
            {
                ErrorField = BillsAttributes.DataBank_FirstExpirationData;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(DataBank_ExpirationInterval, "INTERVAL DE VENCIMENT", out ErrMsg))
            {
                ErrorField = BillsAttributes.DataBank_ExpirationInterval;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(DataBank_Payday_1, "PRIMER DIA DE PAGAMENT", out ErrMsg))
            {
                ErrorField = BillsAttributes.DataBank_Payday_1;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(DataBank_Payday_2, "SEGON DIA DE PAGAMENT", out ErrMsg))
            {
                ErrorField = BillsAttributes.DataBank_Payday_2;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(DataBank_Payday_3, "TERCER DIA DE PAGAMENT", out ErrMsg))
            {
                ErrorField = BillsAttributes.DataBank_Payday_3;
                throw new FormatException(ErrMsg);
            }
            string CCC = string.Format("{0}{1}{2}{3}", DataBank_IBAN_BankCode, DataBank_IBAN_OfficeCode,
                                                       DataBank_IBAN_CheckDigits, DataBank_IBAN_AccountNumber);
            if (!GlobalViewModel.IsEmptyOrIBAN_CountryCode(DataBank_IBAN_CountryCode, CCC, out ErrMsg))
            {
                ErrorField = BillsAttributes.DataBank_IBAN_CountryCode;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrIBAN_BankCode(DataBank_IBAN_BankCode))
            {
                ErrorField = BillsAttributes.DataBank_IBAN_BankCode;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrIBAN_BankCodeError);
            }
            if (!GlobalViewModel.IsEmptyOrIBAN_OfficeCode(DataBank_IBAN_OfficeCode))
            {
                ErrorField = BillsAttributes.DataBank_IBAN_OfficeCode;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrIBAN_OfficeCodeError);
            }
            if (!GlobalViewModel.IsEmptyOrIBAN_CheckDigits(DataBank_IBAN_CheckDigits))
            {
                ErrorField = BillsAttributes.DataBank_IBAN_CheckDigits;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrIBAN_CheckDigitsError);
            }
            if (!GlobalViewModel.IsEmptyOrIBAN_AccountNumber(DataBank_IBAN_AccountNumber))
            {
                ErrorField = BillsAttributes.DataBank_IBAN_AccountNumber;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrIBAN_AccountNumberError);
            }
            #endregion
            #region Billing Data (Facturació)
            if (!GlobalViewModel.IsEmptyOrPercent(BillingData_EarlyPaymentDiscount, "DESCOMPTE PAGAMENT IMMEDIAT", out ErrMsg))
            {
                ErrorField = BillsAttributes.BillingData_EarlyPaymentDiscount;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(BillingData_NumUnpaid, "FACTURES IMPAGADES", out ErrMsg))
            {
                ErrorField = BillsAttributes.BillingData_NumUnpaid;
                throw new FormatException(ErrMsg);
            }
            #endregion
            #region Diversos
            if (!GlobalViewModel.IsEmptyOrComment(Remarks))
            {
                ErrorField = BillsAttributes.Remarks;
                throw new FormatException(GlobalViewModel.ValidationCommentError);
            }
            if (!GlobalViewModel.IsEmptyOrComment(Customer_Remarks))
            {
                ErrorField = BillsAttributes.Customer_Remarks;
                throw new FormatException(GlobalViewModel.ValidationCommentError);
            }
            if (!GlobalViewModel.IsEmptyOrName(FileNamePDF))
            {
                ErrorField = BillsAttributes.FileNamePDF;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
            #endregion
            if (Customer == null)
            {
                ErrorField = BillsAttributes.Customer;
                throw new FormatException("Error, manca seleccionar el Client.");
            }
            if (BillingSerie == null)
            {
                ErrorField = BillsAttributes.BillingSerie;
                throw new FormatException("Error, manca seleccionar la Sèrie de Facturació.");
            }
            if (Company_PostalCode == null)
            {
                ErrorField = BillsAttributes.Company_PostalCode;
                throw new FormatException("Error, manca seleccionar el Codi Postal.");
            }
            if (DataBank_EffectType == null)
            {
                ErrorField = BillsAttributes.DataBank_Effect;
                throw new FormatException("Error, manca seleccionar el Tipus d'Efecte.");
            }
            if (BillingData_Agent == null)
            {
                ErrorField = BillsAttributes.BillingData_Agent;
                throw new FormatException("Error, manca seleccionar l'Agent.");
            }
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(BillsView Data)
        {
            Date = Data.Date;
            Year = Data.Year;
            BillingSerie = Data.BillingSerie;
            Customer_Alias = Data.Customer_Alias;
            Company_Name = Data.Company_Name;
            Company_Cif = Data.Company_Cif;
            Company_Address = Data.Company_Address;
            //Company_NumProv = Data.Company_NumProv;
            DataBank_Bank = Data.DataBank_Bank;
            DataBank_BankAddress = Data.DataBank_BankAddress;
            DataBank_NumEffect = Data.DataBank_NumEffect;
            DataBank_FirstExpirationData = Data.DataBank_FirstExpirationData;
            DataBank_ExpirationInterval = Data.DataBank_ExpirationInterval;
            DataBank_Payday_1 = Data.DataBank_Payday_1;
            DataBank_Payday_2 = Data.DataBank_Payday_2;
            DataBank_Payday_3 = Data.DataBank_Payday_3;
            DataBank_IBAN_CountryCode = Data.DataBank_IBAN_CountryCode;
            DataBank_IBAN_BankCode = Data.DataBank_IBAN_BankCode;
            DataBank_IBAN_OfficeCode = Data.DataBank_IBAN_OfficeCode;
            DataBank_IBAN_CheckDigits = Data.DataBank_IBAN_CheckDigits;
            DataBank_IBAN_AccountNumber = Data.DataBank_IBAN_AccountNumber;
            BillingData_EarlyPaymentDiscount = Data.BillingData_EarlyPaymentDiscount;
            BillingData_NumUnpaid = Data.BillingData_NumUnpaid;
            FileNamePDF = Data.FileNamePDF;
            Customer_Remarks = Data.Customer_Remarks;
            Remarks = Data.Remarks;
            Customer = Data.Customer;
            Receipts = Data.Receipts;
            Company_PostalCode = Data.Company_PostalCode;
            DataBank_EffectType = Data.DataBank_EffectType;
            IVAPercent = Data.IVAPercent;
            SurchargePercent = Data.SurchargePercent;
            BillingData_Agent = Data.BillingData_Agent;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(BillsView Data, BillsAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case BillsAttributes.Year:
                     Year = Data.Year;
                     break;
                case BillsAttributes.Date:
                     Date = Data.Date;
                     break;
                case BillsAttributes.BillingSerie:
                     BillingSerie = Data.BillingSerie;
                     break;
                case BillsAttributes.Customer_Alias:
                     Customer_Alias = Data.Customer_Alias;
                     break;
                case BillsAttributes.Company_Name:
                     Company_Name = Data.Company_Name;
                     break;
                case BillsAttributes.Company_Cif:
                     Company_Cif = Data.Company_Cif;
                     break;
                case BillsAttributes.Company_Address:
                     Company_Address = Data.Company_Address;
                     break;
                //case BillsAttributes.Company_NumProv:
                //     Company_NumProv = Data.Company_NumProv;
                //     break;
                case BillsAttributes.DataBank_Bank:
                     DataBank_Bank = Data.DataBank_Bank;
                     break;
                case BillsAttributes.DataBank_BankAddress:
                     DataBank_BankAddress = Data.DataBank_BankAddress;
                     break;
                case BillsAttributes.DataBank_NumEffect:
                     DataBank_NumEffect = Data.DataBank_NumEffect;
                     break;
                case BillsAttributes.DataBank_FirstExpirationData:
                     DataBank_FirstExpirationData = Data.DataBank_FirstExpirationData;
                     break;
                case BillsAttributes.DataBank_ExpirationInterval:
                     DataBank_ExpirationInterval = Data.DataBank_ExpirationInterval;
                     break;
                case BillsAttributes.DataBank_Payday_1:
                     DataBank_Payday_1 = Data.DataBank_Payday_1;
                     break;
                case BillsAttributes.DataBank_Payday_2:
                     DataBank_Payday_2 = Data.DataBank_Payday_2;
                     break;
                case BillsAttributes.DataBank_Payday_3:
                     DataBank_Payday_3 = Data.DataBank_Payday_3;
                     break;
                case BillsAttributes.DataBank_IBAN_CountryCode:
                     DataBank_IBAN_CountryCode = Data.DataBank_IBAN_CountryCode;
                     break;
                case BillsAttributes.DataBank_IBAN_BankCode:
                     DataBank_IBAN_BankCode = Data.DataBank_IBAN_BankCode;
                     break;
                case BillsAttributes.DataBank_IBAN_OfficeCode:
                     DataBank_IBAN_OfficeCode = Data.DataBank_IBAN_OfficeCode;
                     break;
                case BillsAttributes.DataBank_IBAN_CheckDigits:
                     DataBank_IBAN_CheckDigits = Data.DataBank_IBAN_CheckDigits;
                     break;
                case BillsAttributes.DataBank_IBAN_AccountNumber:
                     DataBank_IBAN_AccountNumber = Data.DataBank_IBAN_AccountNumber;
                     break;
                case BillsAttributes.BillingData_EarlyPaymentDiscount:
                     BillingData_EarlyPaymentDiscount = Data.BillingData_EarlyPaymentDiscount;
                     break;
                case BillsAttributes.BillingData_NumUnpaid:
                     BillingData_NumUnpaid = Data.BillingData_NumUnpaid;
                     break;
                case BillsAttributes.FileNamePDF:
                     FileNamePDF = Data.FileNamePDF;
                     break;
                case BillsAttributes.Customer_Remarks:
                     Customer_Remarks = Data.Customer_Remarks;
                     break;
                case BillsAttributes.Remarks:
                     Remarks = Data.Remarks;
                     break;
                case BillsAttributes.Customer:
                     Customer = Data.Customer;
                     break;
                case BillsAttributes.Company_PostalCode:
                     Company_PostalCode = Data.Company_PostalCode;
                     break;
                case BillsAttributes.DataBank_Effect:
                     DataBank_EffectType = Data.DataBank_EffectType;
                     break;
                case BillsAttributes.BillingData_Agent:
                     BillingData_Agent = Data.BillingData_Agent;
                     break;
                default:
                     break;
            }
        }
        
        #endregion

        #region Equal implementation

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="obj">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public override bool Equals(Object obj)
        {
            //  Si el parámetro es nulo ya hemos acabado.
                if (obj == null) return (false);
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                BillsView bill = obj as BillsView;
                if ((Object)bill == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Bill_Id == bill.Bill_Id) && (Year == bill.Year) && (Date.Date == bill.Date.Date) &&
                       (Customer_Id == bill.Customer_Id) && (Customer_Alias == bill.Customer_Alias) &&
                       (Company_Name == bill.Company_Name) && (Company_Cif == bill.Company_Cif) &&
                       (Company_Address == bill.Company_Address) && (Company_NumProv == bill.Company_NumProv) &&
                       (DataBank_Bank == bill.DataBank_Bank) &&
                       (DataBank_BankAddress == bill.DataBank_BankAddress) &&
                       (DataBank_NumEffect == bill.DataBank_NumEffect) &&
                       (DataBank_FirstExpirationData == bill.DataBank_FirstExpirationData) &&
                       (DataBank_ExpirationInterval == bill.DataBank_ExpirationInterval) &&
                       (DataBank_Payday_1 == bill.DataBank_Payday_1) &&
                       (DataBank_Payday_2 == bill.DataBank_Payday_2) &&
                       (DataBank_Payday_3 == bill.DataBank_Payday_3) &&
                       (DataBank_IBAN_CountryCode == bill.DataBank_IBAN_CountryCode) &&
                       (DataBank_IBAN_BankCode == bill.DataBank_IBAN_BankCode) &&
                       (DataBank_IBAN_OfficeCode == bill.DataBank_IBAN_OfficeCode) &&
                       (DataBank_IBAN_CheckDigits == bill.DataBank_IBAN_CheckDigits) &&
                       (DataBank_IBAN_AccountNumber == bill.DataBank_IBAN_AccountNumber) &&
                       (BillingData_EarlyPaymentDiscount == bill.BillingData_EarlyPaymentDiscount) &&
                       (BillingData_NumUnpaid == bill.BillingData_NumUnpaid) &&
                       (Print == bill.Print) && (SendByEMail == bill.SendByEMail) &&
                       (Customer_Remarks == bill.Customer_Remarks) && (Remarks == bill.Remarks) &&
                       (FileNamePDF == bill.FileNamePDF) && 
                       (ExpirationDate == bill.ExpirationDate) &&
                       (TotalAmount == bill.TotalAmount) &&
                       (_BillingSerie_Id == bill._BillingSerie_Id) &&
                       (_Company_PostalCode_Id == bill._Company_PostalCode_Id) &&
                       (_DataBank_EffectType_Id == bill._DataBank_EffectType_Id) &&
                       (_BillingData_Agent_Id == bill._BillingData_Agent_Id) &&
                       (IVAPercent == bill.IVAPercent) &&
                       (SurchargePercent == bill.SurchargePercent);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(BillsView bill)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)bill == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Bill_Id == bill.Bill_Id) && (Year == bill.Year) && (Date.Date == bill.Date.Date) &&
                       (Customer_Id == bill.Customer_Id) && (Customer_Alias == bill.Customer_Alias) &&
                       (Company_Name == bill.Company_Name) && (Company_Cif == bill.Company_Cif) &&
                       (Company_Address == bill.Company_Address) && (Company_NumProv == bill.Company_NumProv) &&
                       (DataBank_Bank == bill.DataBank_Bank) &&
                       (DataBank_BankAddress == bill.DataBank_BankAddress) &&
                       (DataBank_NumEffect == bill.DataBank_NumEffect) &&
                       (DataBank_FirstExpirationData == bill.DataBank_FirstExpirationData) &&
                       (DataBank_ExpirationInterval == bill.DataBank_ExpirationInterval) &&
                       (DataBank_Payday_1 == bill.DataBank_Payday_1) &&
                       (DataBank_Payday_2 == bill.DataBank_Payday_2) &&
                       (DataBank_Payday_3 == bill.DataBank_Payday_3) &&
                       (DataBank_IBAN_CountryCode == bill.DataBank_IBAN_CountryCode) &&
                       (DataBank_IBAN_BankCode == bill.DataBank_IBAN_BankCode) &&
                       (DataBank_IBAN_OfficeCode == bill.DataBank_IBAN_OfficeCode) &&
                       (DataBank_IBAN_CheckDigits == bill.DataBank_IBAN_CheckDigits) &&
                       (DataBank_IBAN_AccountNumber == bill.DataBank_IBAN_AccountNumber) &&
                       (BillingData_EarlyPaymentDiscount == bill.BillingData_EarlyPaymentDiscount) &&
                       (BillingData_NumUnpaid == bill.BillingData_NumUnpaid) &&
                       (Print == bill.Print) && (SendByEMail == bill.SendByEMail) &&
                       (Customer_Remarks == bill.Customer_Remarks) && (Remarks == bill.Remarks) &&
                       (FileNamePDF == bill.FileNamePDF) &&
                       (ExpirationDate == bill.ExpirationDate) &&
                       (TotalAmount == bill.TotalAmount) &&
                       (_BillingSerie_Id == bill._BillingSerie_Id) &&
                       (_Company_PostalCode_Id == bill._Company_PostalCode_Id) &&
                       (_DataBank_EffectType_Id == bill._DataBank_EffectType_Id) &&
                       (_BillingData_Agent_Id == bill._BillingData_Agent_Id) &&
                       (IVAPercent == bill.IVAPercent) &&
                       (SurchargePercent == bill.SurchargePercent);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="bill_1">Primera instáncia a comparar.</param>
        /// <param name="bill_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(BillsView bill_1, BillsView bill_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(bill_1, bill_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)bill_1 == null) || ((object)bill_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (bill_1.Bill_Id == bill_2.Bill_Id) && (bill_1.Year == bill_2.Year) && (bill_1.Date.Date == bill_2.Date.Date) &&
                       (bill_1.Customer_Id == bill_2.Customer_Id) && (bill_1.Customer_Alias == bill_2.Customer_Alias) &&
                       (bill_1.Company_Name == bill_2.Company_Name) && (bill_1.Company_Cif == bill_2.Company_Cif) &&
                       (bill_1.Company_Address == bill_2.Company_Address) && (bill_1.Company_NumProv == bill_2.Company_NumProv) &&
                       (bill_1.DataBank_Bank == bill_2.DataBank_Bank) &&
                       (bill_1.DataBank_BankAddress == bill_2.DataBank_BankAddress) &&
                       (bill_1.DataBank_NumEffect == bill_2.DataBank_NumEffect) &&
                       (bill_1.DataBank_FirstExpirationData == bill_2.DataBank_FirstExpirationData) &&
                       (bill_1.DataBank_ExpirationInterval == bill_2.DataBank_ExpirationInterval) &&
                       (bill_1.DataBank_Payday_1 == bill_2.DataBank_Payday_1) &&
                       (bill_1.DataBank_Payday_2 == bill_2.DataBank_Payday_2) &&
                       (bill_1.DataBank_Payday_3 == bill_2.DataBank_Payday_3) &&
                       (bill_1.DataBank_IBAN_CountryCode == bill_2.DataBank_IBAN_CountryCode) &&
                       (bill_1.DataBank_IBAN_BankCode == bill_2.DataBank_IBAN_BankCode) &&
                       (bill_1.DataBank_IBAN_OfficeCode == bill_2.DataBank_IBAN_OfficeCode) &&
                       (bill_1.DataBank_IBAN_CheckDigits == bill_2.DataBank_IBAN_CheckDigits) &&
                       (bill_1.DataBank_IBAN_AccountNumber == bill_2.DataBank_IBAN_AccountNumber) &&
                       (bill_1.BillingData_EarlyPaymentDiscount == bill_2.BillingData_EarlyPaymentDiscount) &&
                       (bill_1.BillingData_NumUnpaid == bill_2.BillingData_NumUnpaid) &&
                       (bill_1.Print == bill_2.Print) && (bill_1.SendByEMail == bill_2.SendByEMail) &&
                       (bill_1.Customer_Remarks == bill_2.Customer_Remarks) && 
                       (bill_1.Remarks == bill_2.Remarks) &&
                       (bill_1.FileNamePDF == bill_2.FileNamePDF) &&
                       (bill_1.ExpirationDate == bill_2.ExpirationDate) &&
                       (bill_1.TotalAmount == bill_2.TotalAmount) &&
                       (bill_1._BillingSerie_Id == bill_2._BillingSerie_Id) &&
                       (bill_1._Company_PostalCode_Id == bill_2._Company_PostalCode_Id) &&
                       (bill_1._DataBank_EffectType_Id == bill_2._DataBank_EffectType_Id) &&
                       (bill_1._BillingData_Agent_Id == bill_2._BillingData_Agent_Id) &&
                       (bill_1.IVAPercent == bill_2.IVAPercent) &&
                       (bill_1.SurchargePercent == bill_2.SurchargePercent);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="Bill_1">Primera instáncia a comparar.</param>
        /// <param name="Bill_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(BillsView Bill_1, BillsView Bill_2)
        {
            return !(Bill_1 == Bill_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(Bill_Id, Year, BillingSerie).GetHashCode());
        }

        #endregion

        #region Public Functions

        public string GetStringIBANFromBill()
        {
            return string.Format("{0} {1} {2} {3} {4}",
                                 DataBank_IBAN_CountryCode,
                                 DataBank_IBAN_BankCode,
                                 DataBank_IBAN_OfficeCode,
                                 DataBank_IBAN_CheckDigits,
                                 DataBank_IBAN_AccountNumber);
        }

        #endregion
    }
}
