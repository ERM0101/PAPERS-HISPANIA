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
    public enum CustomerOrdersAttributes
    {
        Address,
        NumEffect,
        DataBank_Bank,
        DataBank_BankAddress,
        DataBank_ExpirationDays,
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
        Remarks,
        Daily_Dates,
        According,
        Valued,
        Transfer,
        Historic,
        Select_Bill,
        Expiration,
        Daily,
        Customer,
        PostalCode,
        SendType,
        EffectType,
        BillingData_Agent,
        None,
    }

    /// <summary>
    /// Class that Store the information of a CustomerOrder.
    /// </summary>
    public class CustomerOrdersView : IMenuView
    {
        #region Fields for Filter

        /// <summary>
        /// Store the list of fields that compose the CustomerOrder class
        /// </summary>
        private static Dictionary<string, string> m_Fields = null;

        /// <summary>
        /// Get the list of fields that compose the CustomerOrder class
        /// </summary>
        public static Dictionary<string, string> Fields
        {
            get
            {
                if (m_Fields == null)
                {
                    m_Fields = new Dictionary<string, string>
                    {
                        { "Numero de Comanda", "CustomerOrder_Id" },
                        { "Data de Comanda", "Date_Str" },
                        { "Numero de Client", "Customer_Id_Str" },
                        { "Client", "Customer_Alias" },
                        { "Adreça d'Enviament", "Address" },
                        { "Codi Postal / Població", "PostalCode_Str" },
                        { "Sistema de Lliurament", "SendType_Str" },
                        { "Comentari de Comanda", "Remarks" },
                    };
                }
                return (m_Fields);
            }
        }

        /// <summary>
        /// Store the list of fields that compose the CustomerOrder class
        /// </summary>
        private static Dictionary<string, string> m_DeliveryNoteFields = null;

        /// <summary>
        /// Get the list of fields that compose the CustomerOrder class
        /// </summary>
        public static Dictionary<string, string> DeliveryNoteFields
        {
            get
            {
                if (m_DeliveryNoteFields == null)
                {
                    m_DeliveryNoteFields = new Dictionary<string, string>
                    {
                        { "Numero d'Albarà", "DeliveryNote_Id_Str" },
                        { "Data de l'Albarà", "DeliveryNote_Date_Str" },
                        { "Numero de Client", "Customer_Id_Str" },
                        { "Client", "Customer_Alias" },
                        { "Numero de Comanda", "CustomerOrder_Id" },
                        { "Data de Comanda", "Date_Str" },
                        { "Estat de Lliurament", "According_Str" },
                        { "Adreça d'Enviament", "Address" },
                        { "Codi Postal / Població", "PostalCode_Str" },
                        { "Sistema de Lliurament", "SendType_Str" },
                    };
                }
                return (m_DeliveryNoteFields);
            }
        }

        #endregion

        #region Properties

        #region IMenuView Interface implementation

        public string GetKey
        {
            get
            {
                return GlobalViewModel.GetStringFromIntIdValue(CustomerOrder_Id);
            }
        }

        #endregion

        #region Main Fields

        public int CustomerOrder_Id { get; set; }
        public DateTime Date { get; set; }
        public string Date_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDateTimeValue(Date);
            }
        }
        public DateTime Bill_Date { get; set; }
        public string Bill_Date_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDateTimeValue(Bill_Date);
            }
        }
        public DateTime DeliveryNote_Date { get; set; }
        public string DeliveryNote_Date_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDateTimeValue(DeliveryNote_Date);
            }
        }
        public string Address { get; set; }
        public string TimeTable { get; set; }
        public decimal IVAPercent { get; set; }
        public decimal SurchargePercent { get; set; }

        #endregion

        #region Bank Data (Bancarios)

        public decimal DataBank_NumEffect { get; set; }
        public decimal DataBank_ExpirationDays { get; set; }
        public decimal DataBank_ExpirationInterval { get; set; }
        public decimal DataBank_Payday_1 { get; set; }
        public decimal DataBank_Payday_2 { get; set; }
        public decimal DataBank_Payday_3 { get; set; }
        public string DataBank_Bank { get; set; }
        public string DataBank_BankAddress { get; set; }
        public string DataBank_IBAN_CountryCode { get; set; }
        public string DataBank_IBAN_BankCode { get; set; }
        public string DataBank_IBAN_OfficeCode { get; set; }
        public string DataBank_IBAN_CheckDigits { get; set; }
        public string DataBank_IBAN_AccountNumber { get; set; }

        #endregion

        #region Billing Data (Facturación)

        public decimal BillingData_EarlyPaymentDiscount { get; set; }

        #endregion

        #region Diversos

        public decimal TotalAmount { get; set; }
        public string TotalAmount_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(TotalAmount, DecimalType.Currency, true);
            }
        }
        public string Remarks { get; set; }
        public DateTime Daily_Dates { get; set; }
        public bool According { get; set; }
        public string According_Str
        {
            get
            {
                return (According == true)? "Material lliurat" : "Lliurament pendent";
            }
        }
        public bool Valued { get; set; }
        public string Valued_Str
        {
            get
            {
                return (Valued == true) ? "Valorat" : "No Valorat";
            }
        }
        public bool Transfer { get; set; }
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
        public bool Print_CustomerOrder { get; set; }
        public string Print_CustomerOrder_Str
        {
            get
            {
                return (Print_CustomerOrder == true) ? "Imprés" : "No Imprés";
            }
        }
        public bool SendByEMail_CustomerOrder { get; set; }
        public string SendByEMail_CustomerOrder_Str
        {
            get
            {
                return (SendByEMail_CustomerOrder == true) ? "Generat" : "No Generat";
            }
        }
        public bool Historic { get; set; }
        public bool Select_Bill { get; set; }
        public bool Expiration { get; set; }
        public bool Daily { get; set; }

        #endregion

        #region ForeignKey Properties

        #region Customer

        private int _Customer_Id { get; set; }

        private CustomersView _Customer;

        public CustomersView Customer
        {
            get
            {
                if ((_Customer == null) && (_Customer_Id != GlobalViewModel.IntIdInitValue))
                {
                    _Customer = new CustomersView(GlobalViewModel.Instance.HispaniaViewModel.GetCustomer((int)_Customer_Id));
                }
                return (_Customer);
            }
            set
            {
                if (value != null)
                {
                    _Customer = new CustomersView(value);
                    if (_Customer == null) _Customer_Id = GlobalViewModel.IntIdInitValue;
                    else _Customer_Id = _Customer.Customer_Id;
                }
                else
                {
                    _Customer = null;
                    _Customer_Id = GlobalViewModel.IntIdInitValue;
                }
            }
        }

        public string Customer_Id_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromIntIdValue(_Customer_Id);
            }
        }

        public string Customer_Alias
        {
            get
            {
                return (Customer is null) ? string.Empty : Customer.Customer_Alias;
            }
        }

        #endregion

        #region  Bill

        public int Bill_Id
        {
            get;
            private set;
        }
        public bool HasBill
        {
            get
            {
                return (Bill_Id != GlobalViewModel.IntIdInitValue);
            }
        }

        public string Bill_Id_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromIntIdValue(Bill_Id);
            }
        }
        public decimal Bill_Year
        {
            get;
            private set;
        }
        public string Bill_Year_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromYearValue(Bill_Year);
            }
        }
        private int _Bill_Serie_Id { get; set; }

        private BillingSeriesView _Bill_Serie;

        public BillingSeriesView Bill_Serie
        {
            get
            {
                if ((_Bill_Serie == null) && (_Bill_Serie_Id != GlobalViewModel.IntIdInitValue))
                {
                    _Bill_Serie = new BillingSeriesView(GlobalViewModel.Instance.HispaniaViewModel.GetBillingSerie((int)_Bill_Serie_Id));
                }
                return (_Bill_Serie);
            }
            set
            {
                if (value != null)
                {
                    _Bill_Serie = new BillingSeriesView(value);
                    if (_Bill_Serie == null) _Bill_Serie_Id = GlobalViewModel.IntIdInitValue;
                    else _Bill_Serie_Id = _Bill_Serie.Serie_Id;
                }
                else
                {
                    _Bill_Serie = null;
                    _Bill_Serie_Id = GlobalViewModel.IntIdInitValue;
                }
            }
        }

        public string Bill_Serie_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromBillingSerieValue(Bill_Serie);
            }
        }

        private BillsView _Bill;

        public BillsView Bill
        {
            get
            {
                if (_Bill == null)
                {
                    HispaniaCompData.Bill bill = GlobalViewModel.Instance.HispaniaViewModel.GetBill(Bill_Id, Bill_Year);
                    if (!(bill is null)) _Bill = new BillsView(bill);
                }
                return (_Bill);
            }
        }

        #endregion

        #region  DeliveryNote

        public int DeliveryNote_Id
        {
            get;
            set;
        }
        public bool HasDeliveryNote
        {
            get
            {
                return (DeliveryNote_Id != GlobalViewModel.IntIdInitValue) &&
                       (DeliveryNote_Year != GlobalViewModel.YearInitValue) &&
                       (DeliveryNote_Date != GlobalViewModel.DateTimeInitValue);

            }
        }
        public string DeliveryNote_Id_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromIntIdValue(DeliveryNote_Id);
            }
        }
        public decimal DeliveryNote_Year
        {
            get;
            set;
        }
        public string DeliveryNote_Year_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromYearValue(DeliveryNote_Year);
            }
        }

        #endregion

        #region Postal Code

        private int? _PostalCode_Id { get; set; }

        private PostalCodesView _PostalCode;

        public PostalCodesView PostalCode
        {
            get
            {
                if ((_PostalCode == null) && (_PostalCode_Id != GlobalViewModel.IntIdInitValue) && 
                    (_PostalCode_Id != null))
                {
                    _PostalCode = new PostalCodesView(GlobalViewModel.Instance.HispaniaViewModel.GetPostalCode((int)_PostalCode_Id));
                }
                return (_PostalCode);
            }
            set
            {
                if (value != null)
                {
                    _PostalCode = new PostalCodesView(value);
                    if (_PostalCode == null) _PostalCode_Id = GlobalViewModel.IntIdInitValue;
                    else _PostalCode_Id = _PostalCode.PostalCode_Id;
                }
                else
                {
                    _PostalCode = null;
                    _PostalCode_Id = GlobalViewModel.IntIdInitValue;
                }
            }
        }

        public string PostalCode_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromPostalCode(PostalCode);
            }
        }

        #endregion

        #region Send Type

        private int? _SendType_Id;

        private SendTypesView _SendType;

        public SendTypesView SendType
        {
            get
            {
                if ((_SendType == null) && (_SendType_Id != GlobalViewModel.IntIdInitValue) && 
                    (_SendType_Id != null))
                {
                    _SendType = new SendTypesView(GlobalViewModel.Instance.HispaniaViewModel.GetSendType((int)_SendType_Id));
                }
                return (_SendType);
            }
            set
            {
                if (value != null)
                {
                    _SendType = new SendTypesView(value);
                    if (_SendType == null) _SendType_Id = GlobalViewModel.IntIdInitValue;
                    else _SendType_Id = _SendType.SendType_Id;
                }
                else
                {
                    _SendType = null;
                    _SendType_Id = GlobalViewModel.IntIdInitValue;
                }
            }
        }

        public string SendType_Str
        {
            get
            {
                return (SendType is null) ? "No Informat" : SendType.Description;
            }
        }

        #endregion

        #region Effect Type

        private int? _EffectType_Id;

        public EffectTypesView _EffectType;

        public EffectTypesView EffectType
        {
            get
            {
                if ((_EffectType == null) && (_EffectType_Id != GlobalViewModel.IntIdInitValue) && 
                    (_EffectType_Id != null))
                {
                    _EffectType = new EffectTypesView(GlobalViewModel.Instance.HispaniaViewModel.GetEffectType((int)_EffectType_Id));
                }
                return (_EffectType);
            }
            set
            {
                if (value != null)
                {
                    _EffectType = new EffectTypesView(value);
                    if (_EffectType == null) _EffectType_Id = GlobalViewModel.IntIdInitValue;
                    else _EffectType_Id = _EffectType.EffectType_Id;
                }
                else
                {
                    _EffectType = null;
                    _EffectType_Id = GlobalViewModel.IntIdInitValue;
                }
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

        #endregion

        #region Calculated Properties

        private string CommentTemp_Str { get; set; }

        public string Comments_Str
        {
            get
            {
                if (String.IsNullOrEmpty(CommentsToolTip_Str))
                {
                    return "pack://application:,,,/HispaniaCommon.ViewClientWPF;component/recursos/Imagenes/NoComment.png";
                }
                else
                {
                    return "pack://application:,,,/HispaniaCommon.ViewClientWPF;component/recursos/Imagenes/Comment.png";
                }
            }
        }

        public string CommentsToolTip_Str
        {
            get
            {
                return GlobalViewModel.Instance.HispaniaViewModel.CustomerOrderMovementsComments(CustomerOrder_Id);
            }
        }

        public string AccordingMovements_Str
        {
            get
            {
                bool? LiniesConformes = GlobalViewModel.Instance.HispaniaViewModel.LiniesConformes(CustomerOrder_Id);
                if (LiniesConformes == true)
                {
                    return "pack://application:,,,/HispaniaCommon.ViewClientWPF;component/recursos/Imagenes/AcceptNonText.png";
                }
                else if (LiniesConformes == false)
                {
                    return "pack://application:,,,/HispaniaCommon.ViewClientWPF;component/recursos/Imagenes/UnAccordingNonText.png";
                }
                else
                {
                    return "pack://application:,,,/HispaniaCommon.ViewClientWPF;component/recursos/Imagenes/Error.png";
                }
            }
        }

        public string AccordingMovementsToolTip_Str
        {
            get
            {
                bool? LiniesConformes = GlobalViewModel.Instance.HispaniaViewModel.LiniesConformes(CustomerOrder_Id);
                if (LiniesConformes == true)
                {
                    return "Totes les línies estan conformes.";
                }
                else if (LiniesConformes == false)
                {
                    return "Hi ha línies no conformes.";
                }
                else
                {
                    return "No hi ha línies definides.";
                }
            }
        }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public CustomerOrdersView()
        {
            CustomerOrder_Id = GlobalViewModel.IntIdInitValue;
            _Customer_Id = GlobalViewModel.IntIdInitValue;
            Date = GlobalViewModel.DateTimeInitValue;
            Bill_Id = GlobalViewModel.IntIdInitValue;
            Bill_Year = GlobalViewModel.YearInitValue;
            Bill_Date = GlobalViewModel.DateTimeInitValue;
            DeliveryNote_Id = GlobalViewModel.IntIdInitValue;
            DeliveryNote_Year = GlobalViewModel.YearInitValue;
            DeliveryNote_Date = GlobalViewModel.DateTimeInitValue;
            Address = string.Empty;
            TimeTable = string.Empty;
            DataBank_NumEffect = GlobalViewModel.DecimalInitValue;
            DataBank_ExpirationDays = GlobalViewModel.DecimalInitValue;
            DataBank_ExpirationInterval = GlobalViewModel.DecimalInitValue;
            DataBank_Payday_1 = GlobalViewModel.DecimalInitValue;
            DataBank_Payday_2 = GlobalViewModel.DecimalInitValue;
            DataBank_Payday_3 = GlobalViewModel.DecimalInitValue;
            DataBank_Bank = string.Empty;
            DataBank_BankAddress = string.Empty;
            DataBank_IBAN_CountryCode = string.Empty;
            DataBank_IBAN_BankCode = string.Empty;
            DataBank_IBAN_OfficeCode = string.Empty;
            DataBank_IBAN_CheckDigits = string.Empty;
            DataBank_IBAN_AccountNumber = string.Empty;
            BillingData_EarlyPaymentDiscount = GlobalViewModel.DecimalInitValue;
            Remarks = string.Empty;
            Daily_Dates = GlobalViewModel.DateTimeInitValue;
            According = false;
            Valued = false;
            Transfer = false;
            Print = false;
            SendByEMail = false;
            Print_CustomerOrder = false;
            SendByEMail_CustomerOrder = false;
            TotalAmount = GlobalViewModel.DecimalInitValue;
            Historic = false;
            Select_Bill = false;
            Expiration = false;
            Daily = false;
            PostalCode = null;
            SendType = null;
            EffectType = null;
            IVAPercent = GlobalViewModel.DecimalInitValue;
            SurchargePercent = GlobalViewModel.DecimalInitValue;
            Bill_Serie = null;
            BillingData_Agent = null;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal CustomerOrdersView(HispaniaCompData.CustomerOrder customerOrder)
        {
            CustomerOrder_Id = customerOrder.CustomerOrder_Id;
            _Customer_Id = customerOrder.Customer_Id;
            Date = GlobalViewModel.GetDateTimeValue(customerOrder.Date);
            Bill_Id = GlobalViewModel.GetIntFromIntIdValue(customerOrder.Bill_Id);
            Bill_Year = GlobalViewModel.GetDecimalYearValue(customerOrder.Bill_Year);
            Bill_Date = GlobalViewModel.GetDateTimeValue(customerOrder.Bill_Date);
            DeliveryNote_Id = GlobalViewModel.GetIntFromIntIdValue(customerOrder.DeliveryNote_Id);
            DeliveryNote_Year = GlobalViewModel.GetDecimalYearValue(customerOrder.DeliveryNote_Year);
            DeliveryNote_Date = GlobalViewModel.GetDateTimeValue(customerOrder.DeliveryNote_Date);
            Address = customerOrder.Address;
            TimeTable = customerOrder.TimeTable;
            DataBank_NumEffect = customerOrder.NumEffect;
            DataBank_ExpirationDays = customerOrder.DataBank_ExpirationDays;
            DataBank_ExpirationInterval = customerOrder.DataBank_ExpirationInterval;
            DataBank_Payday_1 = customerOrder.DataBank_Paydays1;
            DataBank_Payday_2 = customerOrder.DataBank_Paydays2;
            DataBank_Payday_3 = customerOrder.DataBank_Paydays3;
            DataBank_Bank = customerOrder.DataBank_Bank;
            DataBank_BankAddress = customerOrder.DataBank_BankAddress;
            DataBank_IBAN_CountryCode = customerOrder.DataBank_IBAN_CountryCode;
            DataBank_IBAN_BankCode = customerOrder.DataBank_IBAN_BankCode;
            DataBank_IBAN_OfficeCode = customerOrder.DataBank_IBAN_OfficeCode;
            DataBank_IBAN_CheckDigits = customerOrder.DataBank_IBAN_CheckDigits;
            DataBank_IBAN_AccountNumber = customerOrder.DataBank_IBAN_AccountNumber;
            BillingData_EarlyPaymentDiscount = GlobalViewModel.GetDecimalValue(customerOrder.BillingData_EarlyPaymentDiscount);
            Remarks = customerOrder.Remarks;
            Daily_Dates = GlobalViewModel.GetDateTimeValue(customerOrder.Daily_Dates);
            According = customerOrder.According;
            Valued = customerOrder.Valued;
            Transfer = customerOrder.Transfer;
            Print = customerOrder.Print;
            SendByEMail = customerOrder.SendByEMail;
            Print_CustomerOrder = customerOrder.Print_CustomerOrder;
            SendByEMail_CustomerOrder = customerOrder.SendByEmail_CustomerOrder;
            Historic = customerOrder.Historic;
            Select_Bill = customerOrder.Select_Bill;
            Expiration = customerOrder.Expiration;
            Daily = customerOrder.Daily;
            TotalAmount = customerOrder.TotalAmount;
            _PostalCode_Id = GlobalViewModel.GetIntFromIntIdValue(customerOrder.PostalCode_Id);
            _SendType_Id = GlobalViewModel.GetIntFromIntIdValue(customerOrder.SendType_Id);
            _EffectType_Id = GlobalViewModel.GetIntFromIntIdValue(customerOrder.EffectType_Id);
            IVAPercent = GlobalViewModel.GetDecimalValue(customerOrder.IVAPercent);
            SurchargePercent = GlobalViewModel.GetDecimalValue(customerOrder.SurchargePercent);
            _Bill_Serie_Id = GlobalViewModel.GetIntFromIntIdValue(customerOrder.Bill_Serie_Id);
            _BillingData_Agent_Id = GlobalViewModel.GetIntFromIntIdValue(customerOrder.BillingData_Agent_Id);
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public CustomerOrdersView(CustomerOrdersView customerOrder)
        {
            CustomerOrder_Id = customerOrder.CustomerOrder_Id;
            _Customer_Id = customerOrder._Customer_Id;
            Date = customerOrder.Date;
            Bill_Id = customerOrder.Bill_Id;
            Bill_Year = customerOrder.Bill_Year;
            Bill_Date = customerOrder.Bill_Date;
            DeliveryNote_Id = customerOrder.DeliveryNote_Id;
            DeliveryNote_Year = customerOrder.DeliveryNote_Year;
            DeliveryNote_Date = customerOrder.DeliveryNote_Date;
            Address = customerOrder.Address;
            TimeTable = customerOrder.TimeTable;
            DataBank_NumEffect = customerOrder.DataBank_NumEffect;
            DataBank_ExpirationDays = customerOrder.DataBank_ExpirationDays;
            DataBank_ExpirationInterval = customerOrder.DataBank_ExpirationInterval;
            DataBank_Payday_1 = customerOrder.DataBank_Payday_1;
            DataBank_Payday_2 = customerOrder.DataBank_Payday_2;
            DataBank_Payday_3 = customerOrder.DataBank_Payday_3;
            DataBank_Bank = customerOrder.DataBank_Bank;
            DataBank_BankAddress = customerOrder.DataBank_BankAddress;
            DataBank_IBAN_CountryCode = customerOrder.DataBank_IBAN_CountryCode;
            DataBank_IBAN_BankCode = customerOrder.DataBank_IBAN_BankCode;
            DataBank_IBAN_OfficeCode = customerOrder.DataBank_IBAN_OfficeCode;
            DataBank_IBAN_CheckDigits = customerOrder.DataBank_IBAN_CheckDigits;
            DataBank_IBAN_AccountNumber = customerOrder.DataBank_IBAN_AccountNumber;
            BillingData_EarlyPaymentDiscount = customerOrder.BillingData_EarlyPaymentDiscount;
            Remarks = customerOrder.Remarks;
            Daily_Dates = customerOrder.Daily_Dates;
            According = customerOrder.According;
            Valued = customerOrder.Valued;
            Transfer = customerOrder.Transfer;
            Print = customerOrder.Print;
            SendByEMail = customerOrder.SendByEMail;
            Print_CustomerOrder = customerOrder.Print_CustomerOrder;
            SendByEMail_CustomerOrder = customerOrder.SendByEMail_CustomerOrder;
            Historic = customerOrder.Historic;
            Select_Bill = customerOrder.Select_Bill;
            Expiration = customerOrder.Expiration;
            Daily = customerOrder.Daily;
            TotalAmount = customerOrder.TotalAmount;
            PostalCode = customerOrder.PostalCode;
            SendType = customerOrder.SendType;
            EffectType = customerOrder.EffectType;
            IVAPercent = customerOrder.IVAPercent;
            SurchargePercent = customerOrder.SurchargePercent;
            Bill_Serie = customerOrder.Bill_Serie;
            BillingData_Agent = customerOrder.BillingData_Agent;
        }

        #endregion

        #region GetCustomerOrder

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.CustomerOrder GetCustomerOrder()
        {
            HispaniaCompData.CustomerOrder CustomerOrder = new HispaniaCompData.CustomerOrder()
            {
                CustomerOrder_Id = CustomerOrder_Id,
                Customer_Id = _Customer_Id,
                Date = GlobalViewModel.GetDateTimeDatabaseValue(Date),
                Bill_Id = GlobalViewModel.GetIntIdDatabaseValue(Bill_Id),
                Bill_Year = GlobalViewModel.GetYearDatabaseValue(Bill_Date, Bill_Year),
                Bill_Serie_Id = GlobalViewModel.GetIntIdDatabaseValue(_Bill_Serie_Id),
                BillingData_Agent_Id = _BillingData_Agent_Id,
                Bill_Date = GlobalViewModel.GetDateTimeDatabaseValue(Bill_Date),
                DeliveryNote_Id = GlobalViewModel.GetIntIdDatabaseValue(DeliveryNote_Id),
                DeliveryNote_Year = GlobalViewModel.GetYearDatabaseValue(DeliveryNote_Date, DeliveryNote_Year),
                DeliveryNote_Date = GlobalViewModel.GetDateTimeDatabaseValue(DeliveryNote_Date),
                Address = Address,
                TimeTable = TimeTable,
                NumEffect = DataBank_NumEffect,
                DataBank_ExpirationDays = DataBank_ExpirationDays,
                DataBank_ExpirationInterval = DataBank_ExpirationInterval,
                DataBank_Paydays1 = DataBank_Payday_1,
                DataBank_Paydays2 = DataBank_Payday_2,
                DataBank_Paydays3 = DataBank_Payday_3,
                DataBank_Bank = DataBank_Bank,
                DataBank_BankAddress = DataBank_BankAddress,
                DataBank_IBAN_CountryCode = DataBank_IBAN_CountryCode,
                DataBank_IBAN_BankCode = DataBank_IBAN_BankCode,
                DataBank_IBAN_OfficeCode = DataBank_IBAN_OfficeCode,
                DataBank_IBAN_CheckDigits = DataBank_IBAN_CheckDigits,
                DataBank_IBAN_AccountNumber = DataBank_IBAN_AccountNumber,
                BillingData_EarlyPaymentDiscount = BillingData_EarlyPaymentDiscount,
                Remarks = Remarks,
                Daily_Dates = GlobalViewModel.GetDateTimeDatabaseValue(Daily_Dates),
                According = According,
                Valued = Valued,
                Transfer = Transfer,                
                Print = Print,
                SendByEMail = SendByEMail,
                Print_CustomerOrder = Print_CustomerOrder,
                SendByEmail_CustomerOrder = SendByEMail_CustomerOrder,
                Historic = Historic,
                Select_Bill = Select_Bill,
                Expiration = Expiration,
                Daily = Daily,
                TotalAmount = TotalAmount,
                PostalCode_Id = _PostalCode_Id,
                SendType_Id = _SendType_Id,
                EffectType_Id = _EffectType_Id,
                IVAPercent = IVAPercent,
                SurchargePercent = SurchargePercent
            };
            return (CustomerOrder);
        }

        #endregion

        #region Validate

        /// <summary>
        /// Validate the data contained in the instance of the class.
        /// </summary>
        public void Validate(out CustomerOrdersAttributes ErrorField)
        {
            ErrorField = CustomerOrdersAttributes.None;
            if (!GlobalViewModel.IsEmptyOrAddress(Address))
            {
                ErrorField = CustomerOrdersAttributes.Address;
                throw new FormatException("Error, format incorrecte de l'ADREÇA DEL CLIENT");
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(DataBank_NumEffect, "NUMERO D''EFECTE", out string ErrMsg))
            {
                ErrorField = CustomerOrdersAttributes.NumEffect;
                throw new FormatException(ErrMsg);
            }
            #region Bank Data (Bancarios)
            if (!GlobalViewModel.IsEmptyOrShortDecimal(DataBank_ExpirationDays, "DIES D''EXPIRACIÓ", out ErrMsg))
            {
                ErrorField = CustomerOrdersAttributes.DataBank_ExpirationDays;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(DataBank_ExpirationInterval, "INTERVAL DE VENCIMENT", out ErrMsg))
            {
                ErrorField = CustomerOrdersAttributes.DataBank_ExpirationInterval;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(DataBank_Payday_1, "PRIMER DIA DE PAGAMENT", out ErrMsg))
            {
                ErrorField = CustomerOrdersAttributes.DataBank_Payday_1;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(DataBank_Payday_2, "SEGON DIA DE PAGAMENT", out ErrMsg))
            {
                ErrorField = CustomerOrdersAttributes.DataBank_Payday_2;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(DataBank_Payday_3, "TERCER DIA DE PAGAMENT", out ErrMsg))
            {
                ErrorField = CustomerOrdersAttributes.DataBank_Payday_3;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrName(DataBank_Bank))
            {
                ErrorField = CustomerOrdersAttributes.DataBank_Bank;
                throw new FormatException("Error, format incorrecte del NOM DEL BANC");
            }
            if (!GlobalViewModel.IsEmptyOrAddress(DataBank_BankAddress))
            {
                ErrorField = CustomerOrdersAttributes.DataBank_BankAddress;
                throw new FormatException("Error, format incorrecte de l'ADREÇA DEL BANC");
            }
            string CCC = string.Format("{0}{1}{2}{3}", DataBank_IBAN_BankCode, DataBank_IBAN_OfficeCode,
                                                       DataBank_IBAN_CheckDigits, DataBank_IBAN_AccountNumber);
            if (!GlobalViewModel.IsEmptyOrIBAN_CountryCode(DataBank_IBAN_CountryCode, CCC, out ErrMsg))
            {
                ErrorField = CustomerOrdersAttributes.DataBank_IBAN_CountryCode;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrIBAN_BankCode(DataBank_IBAN_BankCode))
            {
                ErrorField = CustomerOrdersAttributes.DataBank_IBAN_BankCode;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrIBAN_BankCodeError);
            }
            if (!GlobalViewModel.IsEmptyOrIBAN_OfficeCode(DataBank_IBAN_OfficeCode))
            {
                ErrorField = CustomerOrdersAttributes.DataBank_IBAN_OfficeCode;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrIBAN_OfficeCodeError);
            }
            if (!GlobalViewModel.IsEmptyOrIBAN_CheckDigits(DataBank_IBAN_CheckDigits))
            {
                ErrorField = CustomerOrdersAttributes.DataBank_IBAN_CheckDigits;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrIBAN_CheckDigitsError);
            }
            if (!GlobalViewModel.IsEmptyOrIBAN_AccountNumber(DataBank_IBAN_AccountNumber))
            {
                ErrorField = CustomerOrdersAttributes.DataBank_IBAN_AccountNumber;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrIBAN_AccountNumberError);
            }
            #endregion
            #region Billing Data (Facturación)
            if (!GlobalViewModel.IsEmptyOrPercent(BillingData_EarlyPaymentDiscount, "DESCOMPTE PAGAMENT IMMEDIAT", out ErrMsg))
            {
                ErrorField = CustomerOrdersAttributes.BillingData_EarlyPaymentDiscount;
                throw new FormatException(ErrMsg);
            }
            #endregion
            #region Diversos

            if (!GlobalViewModel.IsEmptyOrComment(Remarks))
            {
                ErrorField = CustomerOrdersAttributes.Remarks;
                throw new FormatException(GlobalViewModel.ValidationCommentError);
            }
            #endregion
            if (Customer == null)
            {
                ErrorField = CustomerOrdersAttributes.Customer;
                throw new FormatException("Error, manca seleccionar el Client.");
            }
            if (PostalCode == null)
            {
                ErrorField = CustomerOrdersAttributes.PostalCode;
                throw new FormatException("Error, manca seleccionar el Codi Postal.");
            }
            if (SendType == null)
            {
                ErrorField = CustomerOrdersAttributes.SendType;
                throw new FormatException("Error, manca seleccionar el Tipus d'Enviament.");
            }
            if (EffectType == null)
            {
                ErrorField = CustomerOrdersAttributes.EffectType;
                throw new FormatException("Error, manca seleccionar el Tipus d'Efecte.");
            }
            if (BillingData_Agent == null)
            {
                ErrorField = CustomerOrdersAttributes.BillingData_Agent;
                throw new FormatException("Error, manca seleccionar l'Agent.");
            }
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(CustomerOrdersView Data)
        {
            DataBank_ExpirationDays = Data.DataBank_ExpirationDays;
            DataBank_ExpirationInterval = Data.DataBank_ExpirationInterval;
            DataBank_Payday_1 = Data.DataBank_Payday_1;
            DataBank_Payday_2 = Data.DataBank_Payday_2;
            DataBank_Payday_3 = Data.DataBank_Payday_3;
            DataBank_Bank = Data.DataBank_Bank;
            DataBank_BankAddress = Data.DataBank_BankAddress;
            DataBank_IBAN_CountryCode = Data.DataBank_IBAN_CountryCode;
            DataBank_IBAN_BankCode = Data.DataBank_IBAN_BankCode;
            DataBank_IBAN_OfficeCode = Data.DataBank_IBAN_OfficeCode;
            DataBank_IBAN_CheckDigits = Data.DataBank_IBAN_CheckDigits;
            DataBank_IBAN_AccountNumber = Data.DataBank_IBAN_AccountNumber;
            BillingData_EarlyPaymentDiscount = Data.BillingData_EarlyPaymentDiscount;
            Remarks = Data.Remarks;
            Daily_Dates = Data.Daily_Dates;
            According = Data.According;
            Valued = Data.Valued;
            Transfer = Data.Transfer;            
            Print = Data.Print;
            SendByEMail = Data.SendByEMail;
            Print_CustomerOrder = Data.Print_CustomerOrder;
            SendByEMail_CustomerOrder = Data.SendByEMail_CustomerOrder;
            Historic = Data.Historic;
            Select_Bill = Data.Select_Bill;
            Expiration = Data.Expiration;
            Daily = Data.Daily;
            Customer = Data.Customer;
            PostalCode = Data.PostalCode;
            SendType = Data.SendType;
            EffectType = Data.EffectType;
            BillingData_Agent = Data.BillingData_Agent;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(CustomerOrdersView Data, CustomerOrdersAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case CustomerOrdersAttributes.DataBank_ExpirationDays:
                     DataBank_ExpirationDays = Data.DataBank_ExpirationDays;
                     break;
                case CustomerOrdersAttributes.DataBank_ExpirationInterval:
                     DataBank_ExpirationInterval = Data.DataBank_ExpirationInterval;
                     break;
                case CustomerOrdersAttributes.DataBank_Payday_1:
                     DataBank_Payday_1 = Data.DataBank_Payday_1;
                     break;
                case CustomerOrdersAttributes.DataBank_Payday_2:
                     DataBank_Payday_2 = Data.DataBank_Payday_2;
                     break;
                case CustomerOrdersAttributes.DataBank_Payday_3:
                     DataBank_Payday_3 = Data.DataBank_Payday_3;
                     break;
                case CustomerOrdersAttributes.DataBank_Bank:
                     DataBank_Bank = Data.DataBank_Bank;
                     break;
                case CustomerOrdersAttributes.DataBank_BankAddress:
                     DataBank_BankAddress = Data.DataBank_BankAddress;
                     break;
                case CustomerOrdersAttributes.DataBank_IBAN_CountryCode:
                     DataBank_IBAN_CountryCode = Data.DataBank_IBAN_CountryCode;
                     break;
                case CustomerOrdersAttributes.DataBank_IBAN_BankCode:
                     DataBank_IBAN_BankCode = Data.DataBank_IBAN_BankCode;
                     break;
                case CustomerOrdersAttributes.DataBank_IBAN_OfficeCode:
                     DataBank_IBAN_OfficeCode = Data.DataBank_IBAN_OfficeCode;
                     break;
                case CustomerOrdersAttributes.DataBank_IBAN_CheckDigits:
                     DataBank_IBAN_CheckDigits = Data.DataBank_IBAN_CheckDigits;
                     break;
                case CustomerOrdersAttributes.DataBank_IBAN_AccountNumber:
                     DataBank_IBAN_AccountNumber = Data.DataBank_IBAN_AccountNumber;
                     break;
                case CustomerOrdersAttributes.BillingData_EarlyPaymentDiscount:
                     BillingData_EarlyPaymentDiscount = Data.BillingData_EarlyPaymentDiscount;
                     break;
                case CustomerOrdersAttributes.Remarks:
                     Remarks = Data.Remarks;
                     break;
                case CustomerOrdersAttributes.Daily_Dates:
                     Daily_Dates = Data.Daily_Dates;
                     break;
                case CustomerOrdersAttributes.According:
                     According = Data.According;
                     break;
                case CustomerOrdersAttributes.Valued:
                     Valued = Data.Valued;
                     break;
                case CustomerOrdersAttributes.Transfer:
                     Transfer = Data.Transfer;
                     break;
                case CustomerOrdersAttributes.Historic:
                     Historic = Data.Historic;
                     break;
                case CustomerOrdersAttributes.Select_Bill:
                     Select_Bill = Data.Select_Bill;
                     break;
                case CustomerOrdersAttributes.Expiration:
                     Expiration = Data.Expiration;
                     break;
                case CustomerOrdersAttributes.Daily:
                     Daily = Data.Daily;
                     break;
                case CustomerOrdersAttributes.Customer:
                     Customer = Data.Customer;
                     break;
                case CustomerOrdersAttributes.PostalCode:
                     PostalCode = Data.PostalCode;
                     break;
                case CustomerOrdersAttributes.SendType:
                     SendType = Data.SendType;
                     break;
                case CustomerOrdersAttributes.EffectType:
                     EffectType = Data.EffectType;
                     break;
                case CustomerOrdersAttributes.BillingData_Agent:
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
                CustomerOrdersView customerOrder = obj as CustomerOrdersView;
                if ((Object)customerOrder == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (CustomerOrder_Id == customerOrder.CustomerOrder_Id) && 
                       (_Customer_Id == customerOrder._Customer_Id) && (Date == customerOrder.Date) &&
                       (Bill_Id == customerOrder.Bill_Id) && (Bill_Year == customerOrder.Bill_Year) &&
                       (_Bill_Serie_Id == customerOrder._Bill_Serie_Id) && (Bill_Date == customerOrder.Bill_Date) &&
                       (DeliveryNote_Id == customerOrder.DeliveryNote_Id) && 
                       (DeliveryNote_Year == customerOrder.DeliveryNote_Year) &&
                       (DeliveryNote_Date == customerOrder.DeliveryNote_Date) && 
                       (_PostalCode_Id == customerOrder._PostalCode_Id) &&
                       (_EffectType_Id == customerOrder._EffectType_Id) && 
                       (_SendType_Id == customerOrder._SendType_Id) &&
                       (IVAPercent == customerOrder.IVAPercent) &&
                       (SurchargePercent == customerOrder.SurchargePercent) &&
                       (Address == customerOrder.Address) &&
                       (TimeTable == customerOrder.TimeTable) &&
                       (_BillingData_Agent_Id == customerOrder._BillingData_Agent_Id) && 
                       (DataBank_NumEffect == customerOrder.DataBank_NumEffect) &&
                       (DataBank_ExpirationDays == customerOrder.DataBank_ExpirationDays) &&
                       (DataBank_ExpirationInterval == customerOrder.DataBank_ExpirationInterval) &&
                       (DataBank_Payday_1 == customerOrder.DataBank_Payday_1) &&
                       (DataBank_Payday_2 == customerOrder.DataBank_Payday_2) &&
                       (DataBank_Payday_3 == customerOrder.DataBank_Payday_3) &&
                       (DataBank_Bank == customerOrder.DataBank_Bank) &&
                       (DataBank_BankAddress == customerOrder.DataBank_BankAddress) &&
                       (DataBank_IBAN_CountryCode == customerOrder.DataBank_IBAN_CountryCode) &&
                       (DataBank_IBAN_BankCode == customerOrder.DataBank_IBAN_BankCode) &&
                       (DataBank_IBAN_OfficeCode == customerOrder.DataBank_IBAN_OfficeCode) &&
                       (DataBank_IBAN_CheckDigits == customerOrder.DataBank_IBAN_CheckDigits) &&
                       (DataBank_IBAN_AccountNumber == customerOrder.DataBank_IBAN_AccountNumber) &&
                       (BillingData_EarlyPaymentDiscount == customerOrder.BillingData_EarlyPaymentDiscount) &&
                       (Remarks == customerOrder.Remarks) &&
                       (Daily_Dates == customerOrder.Daily_Dates) &&
                       (According == customerOrder.According) &&
                       (Valued == customerOrder.Valued) &&
                       (Transfer == customerOrder.Transfer) &&
                       (Print == customerOrder.Print) &&
                       (SendByEMail == customerOrder.SendByEMail) &&
                       (Print_CustomerOrder == customerOrder.Print_CustomerOrder) &&
                       (SendByEMail_CustomerOrder == customerOrder.SendByEMail_CustomerOrder) &&                      
                       (Historic == customerOrder.Historic) &&
                       (Select_Bill == customerOrder.Select_Bill) &&
                       (Expiration == customerOrder.Expiration) &&
                       (TotalAmount == customerOrder.TotalAmount) &&
                       (Daily == customerOrder.Daily);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(CustomerOrdersView customerOrder)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)customerOrder == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (CustomerOrder_Id == customerOrder.CustomerOrder_Id) &&
                       (_Customer_Id == customerOrder._Customer_Id) && (Date == customerOrder.Date) &&
                       (Bill_Id == customerOrder.Bill_Id) && (Bill_Year == customerOrder.Bill_Year) &&
                       (_Bill_Serie_Id == customerOrder._Bill_Serie_Id) && (Bill_Date == customerOrder.Bill_Date) &&
                       (DeliveryNote_Id == customerOrder.DeliveryNote_Id) && 
                       (DeliveryNote_Year == customerOrder.DeliveryNote_Year) &&
                       (DeliveryNote_Date == customerOrder.DeliveryNote_Date) && 
                       (_PostalCode_Id == customerOrder._PostalCode_Id) &&
                       (_EffectType_Id == customerOrder._EffectType_Id) && 
                       (_SendType_Id == customerOrder._SendType_Id) &&
                       (IVAPercent == customerOrder.IVAPercent) &&
                       (SurchargePercent == customerOrder.SurchargePercent) &&
                       (_BillingData_Agent_Id == customerOrder._BillingData_Agent_Id) &&
                       (Address == customerOrder.Address) &&
                       (TimeTable == customerOrder.TimeTable) &&
                       (DataBank_NumEffect == customerOrder.DataBank_NumEffect) &&
                       (DataBank_ExpirationDays == customerOrder.DataBank_ExpirationDays) &&
                       (DataBank_ExpirationInterval == customerOrder.DataBank_ExpirationInterval) &&
                       (DataBank_Payday_1 == customerOrder.DataBank_Payday_1) &&
                       (DataBank_Payday_2 == customerOrder.DataBank_Payday_2) &&
                       (DataBank_Payday_3 == customerOrder.DataBank_Payday_3) &&
                       (DataBank_Bank == customerOrder.DataBank_Bank) &&
                       (DataBank_BankAddress == customerOrder.DataBank_BankAddress) &&
                       (DataBank_IBAN_CountryCode == customerOrder.DataBank_IBAN_CountryCode) &&
                       (DataBank_IBAN_BankCode == customerOrder.DataBank_IBAN_BankCode) &&
                       (DataBank_IBAN_OfficeCode == customerOrder.DataBank_IBAN_OfficeCode) &&
                       (DataBank_IBAN_CheckDigits == customerOrder.DataBank_IBAN_CheckDigits) &&
                       (DataBank_IBAN_AccountNumber == customerOrder.DataBank_IBAN_AccountNumber) &&
                       (BillingData_EarlyPaymentDiscount == customerOrder.BillingData_EarlyPaymentDiscount) &&
                       (Remarks == customerOrder.Remarks) &&
                       (Daily_Dates == customerOrder.Daily_Dates) &&
                       (According == customerOrder.According) &&
                       (Valued == customerOrder.Valued) &&
                       (Transfer == customerOrder.Transfer) &&
                       (Print == customerOrder.Print) &&
                       (SendByEMail == customerOrder.SendByEMail) &&
                       (TotalAmount == customerOrder.TotalAmount) &&
                       (Print_CustomerOrder == customerOrder.Print_CustomerOrder) &&
                       (SendByEMail_CustomerOrder == customerOrder.SendByEMail_CustomerOrder) &&
                       (Historic == customerOrder.Historic) &&
                       (Select_Bill == customerOrder.Select_Bill) &&
                       (Expiration == customerOrder.Expiration) &&
                       (Daily == customerOrder.Daily);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="customerOrder_1">Primera instáncia a comparar.</param>
        /// <param name="customerOrder_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(CustomerOrdersView customerOrder_1, CustomerOrdersView customerOrder_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(customerOrder_1, customerOrder_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)customerOrder_1 == null) || ((object)customerOrder_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (customerOrder_1.CustomerOrder_Id == customerOrder_2.CustomerOrder_Id) && 
                       (customerOrder_1._Customer_Id == customerOrder_2._Customer_Id) &&
                       (customerOrder_1.Date == customerOrder_2.Date) &&
                       (customerOrder_1.Bill_Id == customerOrder_2.Bill_Id) && 
                       (customerOrder_1.Bill_Year == customerOrder_2.Bill_Year) &&
                       (customerOrder_1._Bill_Serie_Id == customerOrder_2._Bill_Serie_Id) && 
                       (customerOrder_1.Bill_Date == customerOrder_2.Bill_Date) &&
                       (customerOrder_1.DeliveryNote_Id == customerOrder_2.DeliveryNote_Id) && 
                       (customerOrder_1.DeliveryNote_Year == customerOrder_2.DeliveryNote_Year) &&
                       (customerOrder_1.DeliveryNote_Date == customerOrder_2.DeliveryNote_Date) && 
                       (customerOrder_1._PostalCode_Id == customerOrder_2._PostalCode_Id) &&
                       (customerOrder_1._EffectType_Id == customerOrder_2._EffectType_Id) && 
                       (customerOrder_1._SendType_Id == customerOrder_2._SendType_Id) &&
                       (customerOrder_1.IVAPercent == customerOrder_2.IVAPercent) &&
                       (customerOrder_1.SurchargePercent == customerOrder_2.SurchargePercent) &&
                       (customerOrder_1._BillingData_Agent_Id == customerOrder_2._BillingData_Agent_Id) &&
                       (customerOrder_1.Address == customerOrder_2.Address) &&
                       (customerOrder_1.TimeTable == customerOrder_2.TimeTable) &&
                       (customerOrder_1.DataBank_NumEffect == customerOrder_2.DataBank_NumEffect) &&
                       (customerOrder_1.DataBank_ExpirationDays == customerOrder_2.DataBank_ExpirationDays) &&
                       (customerOrder_1.DataBank_ExpirationInterval == customerOrder_2.DataBank_ExpirationInterval) &&
                       (customerOrder_1.DataBank_Payday_1 == customerOrder_2.DataBank_Payday_1) &&
                       (customerOrder_1.DataBank_Payday_2 == customerOrder_2.DataBank_Payday_2) &&
                       (customerOrder_1.DataBank_Payday_3 == customerOrder_2.DataBank_Payday_3) &&
                       (customerOrder_1.DataBank_Bank == customerOrder_2.DataBank_Bank) &&
                       (customerOrder_1.DataBank_IBAN_CountryCode == customerOrder_2.DataBank_IBAN_CountryCode) &&
                       (customerOrder_1.DataBank_BankAddress == customerOrder_2.DataBank_BankAddress) &&
                       (customerOrder_1.DataBank_IBAN_BankCode == customerOrder_2.DataBank_IBAN_BankCode) &&
                       (customerOrder_1.DataBank_IBAN_OfficeCode == customerOrder_2.DataBank_IBAN_OfficeCode) &&
                       (customerOrder_1.DataBank_IBAN_CheckDigits == customerOrder_2.DataBank_IBAN_CheckDigits) &&
                       (customerOrder_1.DataBank_IBAN_AccountNumber == customerOrder_2.DataBank_IBAN_AccountNumber) &&
                       (customerOrder_1.BillingData_EarlyPaymentDiscount == customerOrder_2.BillingData_EarlyPaymentDiscount) &&
                       (customerOrder_1.Remarks == customerOrder_2.Remarks) &&
                       (customerOrder_1.Daily_Dates == customerOrder_2.Daily_Dates) &&
                       (customerOrder_1.According == customerOrder_2.According) &&
                       (customerOrder_1.Valued == customerOrder_2.Valued) &&
                       (customerOrder_1.Transfer == customerOrder_2.Transfer) &&
                       (customerOrder_1.Print == customerOrder_2.Print) &&
                       (customerOrder_1.SendByEMail == customerOrder_2.SendByEMail) &&
                       (customerOrder_1.Print_CustomerOrder == customerOrder_2.Print_CustomerOrder) &&
                       (customerOrder_1.SendByEMail_CustomerOrder == customerOrder_2.SendByEMail_CustomerOrder) &&
                       (customerOrder_1.TotalAmount == customerOrder_2.TotalAmount) &&
                       (customerOrder_1.Historic == customerOrder_2.Historic) &&
                       (customerOrder_1.Select_Bill == customerOrder_2.Select_Bill) &&
                       (customerOrder_1.Expiration == customerOrder_2.Expiration) &&
                       (customerOrder_1.Daily == customerOrder_2.Daily);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="customer_1">Primera instáncia a comparar.</param>
        /// <param name="customer_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(CustomerOrdersView customer_1, CustomerOrdersView customer_2)
        {
            return !(customer_1 == customer_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(CustomerOrder_Id, _Customer_Id).GetHashCode());
        }

        #endregion

        #region Public Functions

        public string GetStringIBANFromCustomerOrder()
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
