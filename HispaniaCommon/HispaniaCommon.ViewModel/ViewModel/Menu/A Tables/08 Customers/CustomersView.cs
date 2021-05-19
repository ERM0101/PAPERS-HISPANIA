#region Librerias usadas por la clase

using System;
using System.Collections.Generic;
using HispaniaCompData = HispaniaComptabilitat.Data;
using System.Text;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Camps del Tipus
    /// </summary>
    public enum CustomersAttributes
    {
        Customer_Alias,
        Company_Name,
        Company_Address,
        Company_ContactPerson,
        Company_Phone_1,
        Company_Phone_2,
        Company_Fax,
        Company_MobilePhone,
        Company_EMail,
        Company_TimeTable,
        Company_NumProv,
        Company_Cif,
        DataBank_Bank,
        DataBank_BankAddress,
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
        BillingData_BillingType,
        BillingData_EarlyPaymentDiscount,
        BillingData_RiskGranted,
        BillingData_CurrentRisk,
        BillingData_Unpaid,
        BillingData_Duplicate,
        BillingData_NumUnpaid,
        Several_Remarks,
        Company_PostalCode,
        DataBank_Effect,
        BillingData_Agent,
        BillingData_IVAType,
        BillingData_SendType,
        None,
    }

    /// <summary>
    /// Class that Store the information of a Customer.
    /// </summary>
    public class CustomersView : IMenuView
    {
        #region Fields for Filter

        /// <summary>
        /// Store the list of fields that compose the Customer class
        /// </summary>
        private static Dictionary<string, string> m_Fields = null;

        /// <summary>
        /// Get the list of fields that compose the Customer class
        /// </summary>
        public static Dictionary<string, string> Fields
        {
            get
            {
                if (m_Fields == null)
                {
                    m_Fields = new Dictionary<string, string>
                    {
                        { "Numero de Client", "Customer_Id" },
                        { "Alias", "Customer_Alias" },
                        { "Empresa", "Company_Name" },
                        { "Adreça", "Company_Address" },
                        { "CP", "Company_PostalCode_Str" },
                        { "Població", "Company_City_Str" },
                        { "Telèfon 1", "Company_Phone_1" },
                        { "Telèfon 2", "Company_Phone_2" },
                        { "Fax", "Company_Fax" },
                        { "Mòbil", "Company_MobilePhone" },
                        { "E-mail", "Company_EMail" },
                        { "Persona de Contacte", "Company_ContactPerson" },
                        { "Horari", "Company_TimeTable" },
                        { "Num. Proveidor", "Company_NumProv" }
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
                return GlobalViewModel.GetStringFromIntIdValue(Customer_Id);
            }
        }

        #endregion

        #region Main Fields

        public int Customer_Id { get; set; }
        public string Customer_Alias { get; set; }
        public bool Canceled { get; set; }

        #endregion

        #region Company

        public string Company_Name { get; set; }
        public string Company_Address { get; set; }
        public string Company_Phone_1 { get; set; }
        public string Company_Phone_2 { get; set; }
        public string Company_Fax { get; set; }
        public string Company_MobilePhone { get; set; }
        public string Company_EMail { get; set; }
        public string Company_ContactPerson { get; set; }
        public string Company_TimeTable { get; set; }
        public string Company_NumProv { get; set; }
        public string Company_Cif { get; set; }

        #endregion

        #region Bank Data (Bancarios)

        public decimal DataBank_NumEffect { get; set; }
        public decimal DataBank_FirstExpirationData { get; set; }
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
        public string BillingData_BillingType { get; set; }
        public decimal BillingData_Duplicate { get; set; }
        public decimal BillingData_EarlyPaymentDiscount { get; set; }
        public bool BillingData_Valued { get; set; }
        public decimal BillingData_RiskGranted { get; set; }
        public decimal BillingData_CurrentRisk { get; set; }
        public decimal BillingData_Unpaid { get; set; }
        public decimal BillingData_NumUnpaid { get; set; }
        public DateTime BillingData_Register { get; set; }

        #endregion

        #region Diversos

        public string Several_Remarks { get; set; }
        public decimal SeveralData_Acum_1 { get; set; }
        public decimal SeveralData_Acum_2 { get; set; }
        public decimal SeveralData_Acum_3 { get; set; }
        public decimal SeveralData_Acum_4 { get; set; }
        public decimal SeveralData_Acum_5 { get; set; }
        public decimal SeveralData_Acum_6 { get; set; }
        public decimal SeveralData_Acum_7 { get; set; }
        public decimal SeveralData_Acum_8 { get; set; }
        public decimal SeveralData_Acum_9 { get; set; }
        public decimal SeveralData_Acum_10 { get; set; }
        public decimal SeveralData_Acum_11 { get; set; }
        public decimal SeveralData_Acum_12 { get; set; }

        #endregion

        #region Consultations Data (Consultas)

        public bool QueryData_Active { get; set; }
        public bool QueryData_Print { get; set; }

        #endregion

        #region ForeignKey Properties

        #region Postal Codes

        private int? _Company_PostalCode_Id { get; set;  }

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
                return (Company_PostalCode == null) ? string.Empty : Company_PostalCode.Postal_Code;
            }
        }

        public string Company_City_Str
        {
            get
            {
                return (Company_PostalCode == null) ? string.Empty : Company_PostalCode.City;
            }
        }

        #endregion

        #region EffectTypes

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

        #endregion

        #region Agents

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

        #region IVATypes

        private int? _BillingData_IVAType_Id;

        private IVATypesView _BillingData_IVAType;

        public IVATypesView BillingData_IVAType
        {
            get
            {
                if ((_BillingData_IVAType == null) && (_BillingData_IVAType_Id != GlobalViewModel.IntIdInitValue) && 
                    (_BillingData_IVAType_Id != null))
                {
                    _BillingData_IVAType = new IVATypesView(GlobalViewModel.Instance.HispaniaViewModel.GetIVAType((int)_BillingData_IVAType_Id));
                }
                return (_BillingData_IVAType);
            }
            set
            {
                if (value != null)
                {
                    _BillingData_IVAType = new IVATypesView(value);
                    if (_BillingData_IVAType == null) _BillingData_IVAType_Id = GlobalViewModel.IntIdInitValue;
                    else _BillingData_IVAType_Id = _BillingData_IVAType.IVAType_Id;
                }
                else
                {
                    _BillingData_IVAType = null;
                    _BillingData_IVAType_Id = GlobalViewModel.IntIdInitValue;
                }
            }
        }

        #endregion

        #region SendTypes

        private int? _BillingData_SendType_Id;

        private SendTypesView _BillingData_SendType;

        public SendTypesView BillingData_SendType
        {
            get
            {
                if ((_BillingData_SendType == null) && (_BillingData_SendType_Id != GlobalViewModel.IntIdInitValue) && 
                    (_BillingData_SendType_Id != null))
                {
                    _BillingData_SendType = new SendTypesView(GlobalViewModel.Instance.HispaniaViewModel.GetSendType((int)_BillingData_SendType_Id));
                }
                return (_BillingData_SendType);
            }
            set
            {
                if (value != null)
                {
                    _BillingData_SendType = new SendTypesView(value);
                    if (_BillingData_SendType == null) _BillingData_SendType_Id = GlobalViewModel.IntIdInitValue;
                    else _BillingData_SendType_Id = _BillingData_SendType.SendType_Id;
                }
                else
                {
                    _BillingData_SendType = null;
                    _BillingData_SendType_Id = GlobalViewModel.IntIdInitValue;
                }
            }
        }

        #endregion

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public CustomersView()
        {
            Customer_Id = GlobalViewModel.IntIdInitValue;
            Customer_Alias = string.Empty;
            Company_Name = string.Empty;
            Company_Address = string.Empty;
            Company_Phone_1 = string.Empty;
            Company_Phone_2 = string.Empty;
            Company_Fax = string.Empty;
            Company_MobilePhone = string.Empty;
            Company_EMail = string.Empty;
            Company_ContactPerson = string.Empty;
            Company_TimeTable = string.Empty;
            Company_NumProv = string.Empty;
            DataBank_NumEffect = GlobalViewModel.DecimalInitValue;
            DataBank_FirstExpirationData = GlobalViewModel.DecimalInitValue;
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
            Company_Cif= string.Empty;
            BillingData_BillingType= string.Empty;
            BillingData_Duplicate = GlobalViewModel.DecimalInitValue;
            BillingData_EarlyPaymentDiscount = GlobalViewModel.DecimalInitValue;
            BillingData_Valued= false;
            BillingData_RiskGranted= GlobalViewModel.DecimalInitValue;
            BillingData_CurrentRisk= GlobalViewModel.DecimalInitValue;
            BillingData_Unpaid= GlobalViewModel.DecimalInitValue;
            BillingData_NumUnpaid= GlobalViewModel.DecimalInitValue;
            BillingData_Register= DateTime.Now;
            Several_Remarks= string.Empty;
            SeveralData_Acum_1= GlobalViewModel.DecimalInitValue;
            SeveralData_Acum_2= GlobalViewModel.DecimalInitValue;
            SeveralData_Acum_3= GlobalViewModel.DecimalInitValue;
            SeveralData_Acum_4= GlobalViewModel.DecimalInitValue;
            SeveralData_Acum_5= GlobalViewModel.DecimalInitValue;
            SeveralData_Acum_6= GlobalViewModel.DecimalInitValue;
            SeveralData_Acum_7= GlobalViewModel.DecimalInitValue;
            SeveralData_Acum_8= GlobalViewModel.DecimalInitValue;
            SeveralData_Acum_9= GlobalViewModel.DecimalInitValue;
            SeveralData_Acum_10= GlobalViewModel.DecimalInitValue;
            SeveralData_Acum_11= GlobalViewModel.DecimalInitValue;
            SeveralData_Acum_12= GlobalViewModel.DecimalInitValue;
            QueryData_Active= false;
            QueryData_Print= false;
            Canceled = false;
            Company_PostalCode = null;
            DataBank_EffectType = null;
            BillingData_Agent = null;
            BillingData_IVAType = null;
            BillingData_SendType = null;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal CustomersView(HispaniaCompData.Customer customer)
        {
            Customer_Id = customer.Customer_Id;
            Customer_Alias = customer.Customer_Alias;
            Company_Name = customer.Company_Name;
            Company_Address = customer.Company_Address;
            Company_Phone_1 = customer.Company_Phone_1;
            Company_Phone_2 = customer.Company_Phone_2;
            Company_Fax = customer.Company_Fax;
            Company_MobilePhone = customer.Company_MobilePhone;
            Company_EMail = customer.Company_EMail;
            Company_ContactPerson = customer.Company_ContactPerson;
            Company_TimeTable = customer.Company_TimeTable;
            Company_NumProv = customer.Company_NumProv;
            DataBank_NumEffect = customer.DataBank_NumEffect;
            DataBank_FirstExpirationData = customer.DataBank_FirstExpirationData;
            DataBank_ExpirationInterval = customer.DataBank_ExpirationInterval;
            DataBank_Payday_1 = customer.DataBank_Payday_1;
            DataBank_Payday_2 = customer.DataBank_Payday_2;
            DataBank_Payday_3 = customer.DataBank_Payday_3;
            DataBank_Bank = customer.DataBank_Bank;
            DataBank_BankAddress = customer.DataBank_BankAddress;
            DataBank_IBAN_CountryCode = customer.DataBank_IBAN_CountryCode;
            DataBank_IBAN_BankCode = customer.DataBank_IBAN_BankCode;
            DataBank_IBAN_OfficeCode = customer.DataBank_IBAN_OfficeCode;
            DataBank_IBAN_CheckDigits = customer.DataBank_IBAN_CheckDigits;
            DataBank_IBAN_AccountNumber = customer.DataBank_IBAN_AccountNumber;
            Company_Cif = customer.Company_Cif;
            BillingData_BillingType = customer.BillingData_BillingType;
            BillingData_Duplicate = GlobalViewModel.GetDecimalValue(customer.BillingData_Duplicate);
            BillingData_EarlyPaymentDiscount = GlobalViewModel.GetDecimalValue(customer.BillingData_EarlyPaymentDiscount); 
            BillingData_Valued = customer.BillingData_Valued;
            BillingData_RiskGranted = GlobalViewModel.GetDecimalValue(customer.BillingData_RiskGranted);
            BillingData_CurrentRisk = GlobalViewModel.GetDecimalValue(customer.BillingData_CurrentRisk);
            BillingData_Unpaid = GlobalViewModel.GetDecimalValue(customer.BillingData_Unpaid);
            BillingData_NumUnpaid = GlobalViewModel.GetDecimalValue(customer.BillingData_NumUnpaid);  
            BillingData_Register = GlobalViewModel.GetDateTimeValue(customer.BillingData_Register);
            Several_Remarks = customer.Several_Remarks;
            SeveralData_Acum_1 = GlobalViewModel.GetDecimalValue(customer.SeveralData_Acum_1);
            SeveralData_Acum_2 = GlobalViewModel.GetDecimalValue(customer.SeveralData_Acum_2);
            SeveralData_Acum_3 = GlobalViewModel.GetDecimalValue(customer.SeveralData_Acum_3);
            SeveralData_Acum_4 = GlobalViewModel.GetDecimalValue(customer.SeveralData_Acum_4);
            SeveralData_Acum_5 = GlobalViewModel.GetDecimalValue(customer.SeveralData_Acum_5);
            SeveralData_Acum_6 = GlobalViewModel.GetDecimalValue(customer.SeveralData_Acum_6);
            SeveralData_Acum_7 = GlobalViewModel.GetDecimalValue(customer.SeveralData_Acum_7);
            SeveralData_Acum_8 = GlobalViewModel.GetDecimalValue(customer.SeveralData_Acum_8);
            SeveralData_Acum_9 = GlobalViewModel.GetDecimalValue(customer.SeveralData_Acum_9);
            SeveralData_Acum_10 = GlobalViewModel.GetDecimalValue(customer.SeveralData_Acum_10);
            SeveralData_Acum_11 = GlobalViewModel.GetDecimalValue(customer.SeveralData_Acum_11);
            SeveralData_Acum_12 = GlobalViewModel.GetDecimalValue(customer.SeveralData_Acum_12);
            QueryData_Active = customer.QueryData_Active;
            QueryData_Print = customer.QueryData_Print;
            Canceled = customer.Canceled;
            _Company_PostalCode_Id = customer.Company_PostalCode_Id;
            _DataBank_EffectType_Id = customer.DataBank_EffectType_Id;
            _BillingData_Agent_Id = customer.BillingData_Agent_Id;
            _BillingData_IVAType_Id = customer.BillingData_IVAType_Id;
            _BillingData_SendType_Id = customer.BillingData_SendType_Id;
        }


        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public CustomersView(CustomersView customer)
        {
            Customer_Id = customer.Customer_Id;
            Customer_Alias = customer.Customer_Alias;
            Company_Name = customer.Company_Name;
            Company_Address = customer.Company_Address;
            Company_Phone_1 = customer.Company_Phone_1;
            Company_Phone_2 = customer.Company_Phone_2;
            Company_Fax = customer.Company_Fax;
            Company_MobilePhone = customer.Company_MobilePhone;
            Company_EMail = customer.Company_EMail;
            Company_ContactPerson = customer.Company_ContactPerson;
            Company_TimeTable = customer.Company_TimeTable;
            Company_NumProv = customer.Company_NumProv;
            DataBank_NumEffect = customer.DataBank_NumEffect;
            DataBank_FirstExpirationData = customer.DataBank_FirstExpirationData;
            DataBank_ExpirationInterval = customer.DataBank_ExpirationInterval;
            DataBank_Payday_1 = customer.DataBank_Payday_1;
            DataBank_Payday_2 = customer.DataBank_Payday_2;
            DataBank_Payday_3 = customer.DataBank_Payday_3;
            DataBank_Bank = customer.DataBank_Bank;
            DataBank_BankAddress = customer.DataBank_BankAddress;
            DataBank_IBAN_CountryCode = customer.DataBank_IBAN_CountryCode;
            DataBank_IBAN_BankCode = customer.DataBank_IBAN_BankCode;
            DataBank_IBAN_OfficeCode = customer.DataBank_IBAN_OfficeCode;
            DataBank_IBAN_CheckDigits = customer.DataBank_IBAN_CheckDigits;
            DataBank_IBAN_AccountNumber = customer.DataBank_IBAN_AccountNumber;
            Company_Cif = customer.Company_Cif;
            BillingData_BillingType = customer.BillingData_BillingType;
            BillingData_Duplicate = customer.BillingData_Duplicate;
            BillingData_EarlyPaymentDiscount = customer.BillingData_EarlyPaymentDiscount;
            BillingData_Valued = customer.BillingData_Valued;
            BillingData_RiskGranted = customer.BillingData_RiskGranted;
            BillingData_CurrentRisk = customer.BillingData_CurrentRisk;
            BillingData_Unpaid = customer.BillingData_Unpaid;
            BillingData_NumUnpaid = customer.BillingData_NumUnpaid;
            BillingData_Register = customer.BillingData_Register;
            Several_Remarks = customer.Several_Remarks;
            SeveralData_Acum_1 = customer.SeveralData_Acum_1;
            SeveralData_Acum_2 = customer.SeveralData_Acum_2;
            SeveralData_Acum_3 = customer.SeveralData_Acum_3;
            SeveralData_Acum_4 = customer.SeveralData_Acum_4;
            SeveralData_Acum_5 = customer.SeveralData_Acum_5;
            SeveralData_Acum_6 = customer.SeveralData_Acum_6;
            SeveralData_Acum_7 = customer.SeveralData_Acum_7;
            SeveralData_Acum_8 = customer.SeveralData_Acum_8;
            SeveralData_Acum_9 = customer.SeveralData_Acum_9;
            SeveralData_Acum_10 = customer.SeveralData_Acum_10;
            SeveralData_Acum_11 = customer.SeveralData_Acum_11;
            SeveralData_Acum_12 = customer.SeveralData_Acum_12;
            QueryData_Active = customer.QueryData_Active;
            QueryData_Print = customer.QueryData_Print;
            Canceled = customer.Canceled;
            Company_PostalCode = customer.Company_PostalCode;
            DataBank_EffectType = customer.DataBank_EffectType;
            BillingData_Agent = customer.BillingData_Agent;
            BillingData_IVAType = customer.BillingData_IVAType;
            BillingData_SendType = customer.BillingData_SendType;
        }

        #endregion

        #region GetCustomer

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.Customer GetCustomer()
        {
            HispaniaCompData.Customer customer = new HispaniaCompData.Customer()
            {
                Customer_Id = Customer_Id,
                Customer_Alias = Customer_Alias,
                Company_Name = Company_Name,
                Company_Address = Company_Address,
                Company_Phone_1 = Company_Phone_1,
                Company_Phone_2 = Company_Phone_2,
                Company_Fax = Company_Fax,
                Company_MobilePhone = Company_MobilePhone,
                Company_EMail = Company_EMail,
                Company_ContactPerson = Company_ContactPerson,
                Company_TimeTable = Company_TimeTable,
                Company_NumProv = Company_NumProv,
                DataBank_NumEffect = DataBank_NumEffect,
                DataBank_FirstExpirationData = DataBank_FirstExpirationData,
                DataBank_ExpirationInterval = DataBank_ExpirationInterval,
                DataBank_Payday_1 = DataBank_Payday_1,
                DataBank_Payday_2 = DataBank_Payday_2,
                DataBank_Payday_3 = DataBank_Payday_3,
                DataBank_Bank = DataBank_Bank,
                DataBank_BankAddress = DataBank_BankAddress,
                DataBank_IBAN_CountryCode = DataBank_IBAN_CountryCode,
                DataBank_IBAN_BankCode = DataBank_IBAN_BankCode,
                DataBank_IBAN_OfficeCode = DataBank_IBAN_OfficeCode,
                DataBank_IBAN_CheckDigits = DataBank_IBAN_CheckDigits,
                DataBank_IBAN_AccountNumber = DataBank_IBAN_AccountNumber,
                Company_Cif = Company_Cif,
                BillingData_BillingType = BillingData_BillingType,
                BillingData_Duplicate = BillingData_Duplicate,
                BillingData_EarlyPaymentDiscount = BillingData_EarlyPaymentDiscount,
                BillingData_Valued = BillingData_Valued,
                BillingData_RiskGranted = BillingData_RiskGranted,
                BillingData_CurrentRisk = BillingData_CurrentRisk,
                BillingData_Unpaid = BillingData_Unpaid,
                BillingData_NumUnpaid = BillingData_NumUnpaid,
                BillingData_Register = BillingData_Register,
                Several_Remarks = Several_Remarks,
                SeveralData_Acum_1 = SeveralData_Acum_1,
                SeveralData_Acum_2 = SeveralData_Acum_2,
                SeveralData_Acum_3 = SeveralData_Acum_3,
                SeveralData_Acum_4 = SeveralData_Acum_4,
                SeveralData_Acum_5 = SeveralData_Acum_5,
                SeveralData_Acum_6 = SeveralData_Acum_6,
                SeveralData_Acum_7 = SeveralData_Acum_7,
                SeveralData_Acum_8 = SeveralData_Acum_8,
                SeveralData_Acum_9 = SeveralData_Acum_9,
                SeveralData_Acum_10 = SeveralData_Acum_10,
                SeveralData_Acum_11 = SeveralData_Acum_11,
                SeveralData_Acum_12 = SeveralData_Acum_12,
                QueryData_Active = QueryData_Active,
                QueryData_Print = QueryData_Print,
                Canceled = Canceled,
                Company_PostalCode_Id = _Company_PostalCode_Id,
                DataBank_EffectType_Id = _DataBank_EffectType_Id,
                BillingData_Agent_Id = _BillingData_Agent_Id,
                BillingData_IVAType_Id = _BillingData_IVAType_Id,
                BillingData_SendType_Id = _BillingData_SendType_Id
            };
            return (customer);
        }

        #endregion

        #region Validate

        /// <summary>
        /// Validate the data contained in the instance of the class.
        /// </summary>
        public void Validate(out CustomersAttributes ErrorField)
        {
            ErrorField = CustomersAttributes.None;
            if (!GlobalViewModel.IsComment(Customer_Alias))
            {
                ErrorField = CustomersAttributes.Customer_Alias;
                throw new FormatException("Error, valor no definit o format incorrecte de l'ALIAS");
            }
            #region Company Fields
            if (!GlobalViewModel.IsComment(Company_Name))
            {
                ErrorField = CustomersAttributes.Company_Name;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
            if (!GlobalViewModel.IsEmptyOrAddress(Company_Address))
            {
                ErrorField = CustomersAttributes.Company_Address;
                throw new FormatException("Error, format incorrecte de l'ADREÇA DEL CLIENT");
            }
            if (!GlobalViewModel.IsEmptyOrName(Company_ContactPerson))
            {
                ErrorField = CustomersAttributes.Company_ContactPerson;
                throw new FormatException("Error, format incorrecte de la PERSONA DE CONTACTE");
            }
            if (!GlobalViewModel.IsEmptyOrPhoneNumber(Company_Phone_1))
            {
                ErrorField = CustomersAttributes.Company_Phone_1;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrPhoneNumberError);
            }
            if (!GlobalViewModel.IsEmptyOrPhoneNumber(Company_Phone_2))
            {
                ErrorField = CustomersAttributes.Company_Phone_2;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrPhoneNumberError);
            }
            if (!GlobalViewModel.IsEmptyOrPhoneNumber(Company_Fax))
            {
                ErrorField = CustomersAttributes.Company_Fax;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrPhoneNumberError);
            }
            if (!GlobalViewModel.IsEmptyOrMobilePhoneNumber(Company_MobilePhone))
            {
                ErrorField = CustomersAttributes.Company_MobilePhone;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrMobilePhoneNumberError);
            }
            if (!GlobalViewModel.IsEmptyOrEmail(Company_EMail))
            {
                ErrorField = CustomersAttributes.Company_EMail;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrEmailError);
            }
            if (!GlobalViewModel.IsEmptyOrTimeTable(Company_TimeTable))
            {
                ErrorField = CustomersAttributes.Company_TimeTable;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrTimeTableError);
            }
            //if (!GlobalViewModel.IsEmptyOrUint(Company_NumProv, "NUMERO DE PROVEÏDOR", out string ErrMsg))
            //{
            //    ErrorField = CustomersAttributes.Company_NumProv;
            //    throw new FormatException(ErrMsg);
            //}
            if (!GlobalViewModel.IsEmptyOrCIF(Company_Cif))
            {
                ErrorField = CustomersAttributes.Company_Cif;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrCIFError);
            }
            #endregion
            #region DataBank Fields
            if (!GlobalViewModel.IsEmptyOrComment(DataBank_Bank))
            {
                ErrorField = CustomersAttributes.DataBank_Bank;
                throw new FormatException("Error, format incorrecte del NOM DEL BANC");
            }
            if (!GlobalViewModel.IsEmptyOrAddress(DataBank_BankAddress))
            {
                ErrorField = CustomersAttributes.DataBank_BankAddress;
                throw new FormatException("Error, format incorrecte de l'ADREÇA DEL BANC");
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(DataBank_NumEffect, "NUMERO D''EFECTE", out string ErrMsg))
            {
                ErrorField = CustomersAttributes.DataBank_NumEffect;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(DataBank_FirstExpirationData, "DIES DE PRIMER VENCIMENT", out ErrMsg))
            {
                ErrorField = CustomersAttributes.DataBank_FirstExpirationData;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(DataBank_ExpirationInterval, "INTERVAL DE VENCIMENT", out ErrMsg))
            {
                ErrorField = CustomersAttributes.DataBank_ExpirationInterval;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(DataBank_Payday_1, "PRIMER DIA DE PAGAMENT", out ErrMsg))
            {
                ErrorField = CustomersAttributes.DataBank_Payday_1;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(DataBank_Payday_2, "SEGON DIA DE PAGAMENT", out ErrMsg))
            {
                ErrorField = CustomersAttributes.DataBank_Payday_2;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(DataBank_Payday_3, "TERCER DIA DE PAGAMENT", out ErrMsg))
            {
                ErrorField = CustomersAttributes.DataBank_Payday_3;
                throw new FormatException(ErrMsg);
            }
            string CCC = string.Format("{0}{1}{2}{3}", DataBank_IBAN_BankCode, DataBank_IBAN_OfficeCode,
                                                       DataBank_IBAN_CheckDigits, DataBank_IBAN_AccountNumber);
            if (!GlobalViewModel.IsEmptyOrIBAN_CountryCode(DataBank_IBAN_CountryCode, CCC, out ErrMsg))
            {
                ErrorField = CustomersAttributes.DataBank_IBAN_CountryCode;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrIBAN_BankCode(DataBank_IBAN_BankCode))
            {
                ErrorField = CustomersAttributes.DataBank_IBAN_BankCode;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrIBAN_BankCodeError);
            }
            if (!GlobalViewModel.IsEmptyOrIBAN_OfficeCode(DataBank_IBAN_OfficeCode))
            {
                ErrorField = CustomersAttributes.DataBank_IBAN_OfficeCode;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrIBAN_OfficeCodeError);
            }
            if (!GlobalViewModel.IsEmptyOrIBAN_CheckDigits(DataBank_IBAN_CheckDigits))
            {
                ErrorField = CustomersAttributes.DataBank_IBAN_CheckDigits;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrIBAN_CheckDigitsError);
            }
            if (!GlobalViewModel.IsEmptyOrIBAN_AccountNumber(DataBank_IBAN_AccountNumber))
            {
                ErrorField = CustomersAttributes.DataBank_IBAN_AccountNumber;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrIBAN_AccountNumberError);
            }
            #endregion
            #region BillingData Fields
            if (!GlobalViewModel.IsEmptyOrShortDecimal(BillingData_BillingType, "TIPUS DE FACTURACIÓ", out ErrMsg))
            {
                ErrorField = CustomersAttributes.BillingData_BillingType;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrPercent(BillingData_EarlyPaymentDiscount, "DESCOMPTE PAGAMENT IMMEDIAT", out ErrMsg))
            {
                ErrorField = CustomersAttributes.BillingData_EarlyPaymentDiscount;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(BillingData_RiskGranted, "RISC CONCEDIT", out ErrMsg))
            {
                ErrorField = CustomersAttributes.BillingData_RiskGranted;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(BillingData_CurrentRisk, "RISC ACTUAL", out ErrMsg))
            {
                ErrorField = CustomersAttributes.BillingData_CurrentRisk;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(BillingData_Unpaid, "IMPORT IMPAGATS", out ErrMsg))
            {
                ErrorField = CustomersAttributes.BillingData_Unpaid;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(BillingData_Duplicate, "FACTURES DUPLICADES", out ErrMsg))
            {
                ErrorField = CustomersAttributes.BillingData_Duplicate;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(BillingData_NumUnpaid, "FACTURES IMPAGADES", out ErrMsg))
            {
                ErrorField = CustomersAttributes.BillingData_NumUnpaid;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrComment(Several_Remarks))
            {
                ErrorField = CustomersAttributes.Several_Remarks;
                throw new FormatException(GlobalViewModel.ValidationCommentError);
            }
            #endregion
            if (Company_PostalCode == null)
            {
                ErrorField = CustomersAttributes.Company_PostalCode;
                throw new FormatException("Error, manca seleccionar el Codi Postal.");
            }
            if (DataBank_EffectType == null)
            {
                ErrorField = CustomersAttributes.DataBank_Effect;
                throw new FormatException("Error, manca seleccionar el Tipus d'Efecte.");
            }
            if (BillingData_Agent == null)
            {
                ErrorField = CustomersAttributes.BillingData_Agent;
                throw new FormatException("Error, manca seleccionar l'Agent.");
            }
            if (BillingData_IVAType == null)
            {
                ErrorField = CustomersAttributes.BillingData_IVAType;
                throw new FormatException("Error, manca seleccionar el Tipus d'IVA.");
            }
            if (BillingData_SendType == null)
            {
                ErrorField = CustomersAttributes.BillingData_SendType;
                throw new FormatException("Error, manca seleccionar el Tipus d'Enviament.");
            } 
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(CustomersView Data)
        {
            Customer_Alias = Data.Customer_Alias;
            Company_Name = Data.Company_Name;
            Company_Address = Data.Company_Address;
            Company_ContactPerson = Data.Company_ContactPerson;
            Company_Phone_1 = Data.Company_Phone_1;
            Company_Phone_2 = Data.Company_Phone_2;
            Company_Fax = Data.Company_Fax;
            Company_MobilePhone = Data.Company_MobilePhone;
            Company_EMail = Data.Company_EMail;
            Company_TimeTable = Data.Company_TimeTable;
            //Company_NumProv = Data.Company_NumProv;
            Company_Cif = Data.Company_Cif;
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
            BillingData_BillingType = Data.BillingData_BillingType;
            BillingData_EarlyPaymentDiscount = Data.BillingData_EarlyPaymentDiscount;
            BillingData_RiskGranted = Data.BillingData_RiskGranted;
            BillingData_CurrentRisk = Data.BillingData_CurrentRisk;
            BillingData_Unpaid = Data.BillingData_Unpaid;
            BillingData_Duplicate = Data.BillingData_Duplicate;
            BillingData_NumUnpaid = Data.BillingData_NumUnpaid;
            Several_Remarks = Data.Several_Remarks;
            Company_PostalCode = Data.Company_PostalCode;
            DataBank_EffectType = Data.DataBank_EffectType;
            BillingData_Agent = Data.BillingData_Agent;
            BillingData_IVAType = Data.BillingData_IVAType;
            BillingData_SendType = Data.BillingData_SendType;
            Canceled = Data.Canceled;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(CustomersView Data, CustomersAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case CustomersAttributes.Customer_Alias:
                     Customer_Alias = Data.Customer_Alias;
                     break;
                case CustomersAttributes.Company_Name:
                     Company_Name = Data.Company_Name;
                     break;
                case CustomersAttributes.Company_Address:
                     Company_Address = Data.Company_Address;
                     break;
                case CustomersAttributes.Company_ContactPerson:
                     Company_ContactPerson = Data.Company_ContactPerson;
                     break;
                case CustomersAttributes.Company_Phone_1:
                     Company_Phone_1 = Data.Company_Phone_1;
                     break;
                case CustomersAttributes.Company_Phone_2:
                     Company_Phone_2 = Data.Company_Phone_2;
                     break;
                case CustomersAttributes.Company_Fax:
                     Company_Fax = Data.Company_Fax;
                     break;
                case CustomersAttributes.Company_MobilePhone:
                     Company_MobilePhone = Data.Company_MobilePhone;
                     break;
                case CustomersAttributes.Company_EMail:
                     Company_EMail = Data.Company_EMail;
                     break;
                case CustomersAttributes.Company_TimeTable:
                     Company_TimeTable = Data.Company_TimeTable;
                     break;
                //case CustomersAttributes.Company_NumProv:
                //     Company_NumProv = Data.Company_NumProv;
                //     break;
                case CustomersAttributes.Company_Cif:
                     Company_Cif = Data.Company_Cif;
                     break;
                case CustomersAttributes.DataBank_Bank:
                     DataBank_Bank = Data.DataBank_Bank;
                     break;
                case CustomersAttributes.DataBank_BankAddress:
                     DataBank_BankAddress = Data.DataBank_BankAddress;
                     break;
                case CustomersAttributes.DataBank_NumEffect:
                     DataBank_NumEffect = Data.DataBank_NumEffect;
                     break;
                case CustomersAttributes.DataBank_FirstExpirationData:
                     DataBank_FirstExpirationData = Data.DataBank_FirstExpirationData;
                     break;
                case CustomersAttributes.DataBank_ExpirationInterval:
                     DataBank_ExpirationInterval = Data.DataBank_ExpirationInterval;
                     break;
                case CustomersAttributes.DataBank_Payday_1:
                     DataBank_Payday_1 = Data.DataBank_Payday_1;
                     break;
                case CustomersAttributes.DataBank_Payday_2:
                     DataBank_Payday_2 = Data.DataBank_Payday_2;
                     break;
                case CustomersAttributes.DataBank_Payday_3:
                     DataBank_Payday_3 = Data.DataBank_Payday_3;
                     break;
                case CustomersAttributes.DataBank_IBAN_CountryCode:
                     DataBank_IBAN_CountryCode = Data.DataBank_IBAN_CountryCode;
                     break;
                case CustomersAttributes.DataBank_IBAN_BankCode:
                     DataBank_IBAN_BankCode = Data.DataBank_IBAN_BankCode;
                     break;
                case CustomersAttributes.DataBank_IBAN_OfficeCode:
                     DataBank_IBAN_OfficeCode = Data.DataBank_IBAN_OfficeCode;
                     break;
                case CustomersAttributes.DataBank_IBAN_CheckDigits:
                     DataBank_IBAN_CheckDigits = Data.DataBank_IBAN_CheckDigits;
                     break;
                case CustomersAttributes.DataBank_IBAN_AccountNumber:
                     DataBank_IBAN_AccountNumber = Data.DataBank_IBAN_AccountNumber;
                     break;
                case CustomersAttributes.BillingData_BillingType:
                     BillingData_BillingType = Data.BillingData_BillingType;
                     break;
                case CustomersAttributes.BillingData_EarlyPaymentDiscount:
                     BillingData_EarlyPaymentDiscount = Data.BillingData_EarlyPaymentDiscount;
                     break;
                case CustomersAttributes.BillingData_RiskGranted:
                     BillingData_RiskGranted = Data.BillingData_RiskGranted;
                     break;
                case CustomersAttributes.BillingData_CurrentRisk:
                     BillingData_CurrentRisk = Data.BillingData_CurrentRisk;
                     break;
                case CustomersAttributes.BillingData_Unpaid:
                     BillingData_Unpaid = Data.BillingData_Unpaid;
                     break;
                case CustomersAttributes.BillingData_Duplicate:
                     BillingData_Duplicate = Data.BillingData_Duplicate;
                     break;
                case CustomersAttributes.BillingData_NumUnpaid:
                     BillingData_NumUnpaid = Data.BillingData_NumUnpaid;
                     break;
                case CustomersAttributes.Several_Remarks:
                     Several_Remarks = Data.Several_Remarks;
                     break;
                case CustomersAttributes.Company_PostalCode:
                     Company_PostalCode = Data.Company_PostalCode;
                     break;
                case CustomersAttributes.DataBank_Effect:
                     DataBank_EffectType = Data.DataBank_EffectType;
                     break;
                case CustomersAttributes.BillingData_Agent:
                     BillingData_Agent = Data.BillingData_Agent;
                     break;
                case CustomersAttributes.BillingData_IVAType:
                     BillingData_IVAType = Data.BillingData_IVAType;
                     break;
                case CustomersAttributes.BillingData_SendType:
                     BillingData_SendType = Data.BillingData_SendType;
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
                CustomersView customer = obj as CustomersView;
                if ((Object)customer == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Customer_Id == customer.Customer_Id) && (Customer_Alias == customer.Customer_Alias) &&
                       (Company_Name == customer.Company_Name) && (Company_Address == customer.Company_Address) &&
                       (Company_Phone_1 == customer.Company_Phone_1) && (Company_Phone_2 == customer.Company_Phone_2) &&
                       (Company_Fax == customer.Company_Fax) && (Company_MobilePhone == customer.Company_MobilePhone) &&
                       (Company_EMail == customer.Company_EMail) && (Company_ContactPerson == customer.Company_ContactPerson) &&
                       (Company_TimeTable == customer.Company_TimeTable) && (Company_NumProv == customer.Company_NumProv) &&
                       (DataBank_NumEffect == customer.DataBank_NumEffect) &&
                       (DataBank_FirstExpirationData == customer.DataBank_FirstExpirationData) &&
                       (DataBank_ExpirationInterval == customer.DataBank_ExpirationInterval) &&
                       (DataBank_Payday_1 == customer.DataBank_Payday_1) &&
                       (DataBank_Payday_2 == customer.DataBank_Payday_2) &&
                       (DataBank_Payday_3 == customer.DataBank_Payday_3) &&
                       (DataBank_Bank == customer.DataBank_Bank) &&
                       (DataBank_BankAddress == customer.DataBank_BankAddress) &&
                       (DataBank_IBAN_CountryCode == customer.DataBank_IBAN_CountryCode) &&
                       (DataBank_IBAN_BankCode == customer.DataBank_IBAN_BankCode) &&
                       (DataBank_IBAN_OfficeCode == customer.DataBank_IBAN_OfficeCode) &&
                       (DataBank_IBAN_CheckDigits == customer.DataBank_IBAN_CheckDigits) &&
                       (DataBank_IBAN_AccountNumber == customer.DataBank_IBAN_AccountNumber) &&
                       (Company_Cif == customer.Company_Cif) &&
                       (BillingData_BillingType == customer.BillingData_BillingType) &&
                       (BillingData_Duplicate == customer.BillingData_Duplicate) &&
                       (BillingData_EarlyPaymentDiscount == customer.BillingData_EarlyPaymentDiscount) &&
                       (BillingData_Valued == customer.BillingData_Valued) &&
                       (BillingData_RiskGranted == customer.BillingData_RiskGranted) &&
                       (BillingData_CurrentRisk == customer.BillingData_CurrentRisk) &&
                       (BillingData_Unpaid == customer.BillingData_Unpaid) &&
                       (BillingData_NumUnpaid == customer.BillingData_NumUnpaid) &&
                       (BillingData_Register.Date == customer.BillingData_Register.Date) &&
                       (Several_Remarks == customer.Several_Remarks) &&
                       (SeveralData_Acum_1 == customer.SeveralData_Acum_1) &&
                       (SeveralData_Acum_2 == customer.SeveralData_Acum_2) &&
                       (SeveralData_Acum_3 == customer.SeveralData_Acum_3) &&
                       (SeveralData_Acum_4 == customer.SeveralData_Acum_4) &&
                       (SeveralData_Acum_5 == customer.SeveralData_Acum_5) &&
                       (SeveralData_Acum_6 == customer.SeveralData_Acum_6) &&
                       (SeveralData_Acum_7 == customer.SeveralData_Acum_7) &&
                       (SeveralData_Acum_8 == customer.SeveralData_Acum_8) &&
                       (SeveralData_Acum_9 == customer.SeveralData_Acum_9) &&
                       (SeveralData_Acum_10 == customer.SeveralData_Acum_10) &&
                       (SeveralData_Acum_11 == customer.SeveralData_Acum_11) &&
                       (SeveralData_Acum_12 == customer.SeveralData_Acum_12) &&
                       (QueryData_Active == customer.QueryData_Active) &&
                       (QueryData_Print == customer.QueryData_Print) &&
                       (Canceled == customer.Canceled) &&
                       (_Company_PostalCode_Id == customer._Company_PostalCode_Id) &&
                       (_DataBank_EffectType_Id == customer._DataBank_EffectType_Id) &&
                       (_BillingData_Agent_Id == customer._BillingData_Agent_Id) &&
                       (_BillingData_IVAType_Id == customer._BillingData_IVAType_Id) &&
                       (_BillingData_SendType_Id == customer._BillingData_SendType_Id);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(CustomersView customer)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)customer == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Customer_Id == customer.Customer_Id) && (Customer_Alias == customer.Customer_Alias) &&
                       (Company_Name == customer.Company_Name) && (Company_Address == customer.Company_Address) &&
                       (Company_Phone_1 == customer.Company_Phone_1) && (Company_Phone_2 == customer.Company_Phone_2) &&
                       (Company_Fax == customer.Company_Fax) && (Company_MobilePhone == customer.Company_MobilePhone) &&
                       (Company_EMail == customer.Company_EMail) && (Company_ContactPerson == customer.Company_ContactPerson) &&
                       (Company_TimeTable == customer.Company_TimeTable) && (Company_NumProv == customer.Company_NumProv) &&
                       (DataBank_NumEffect == customer.DataBank_NumEffect) &&
                       (DataBank_FirstExpirationData == customer.DataBank_FirstExpirationData) &&
                       (DataBank_ExpirationInterval == customer.DataBank_ExpirationInterval) &&
                       (DataBank_Payday_1 == customer.DataBank_Payday_1) &&
                       (DataBank_Payday_2 == customer.DataBank_Payday_2) &&
                       (DataBank_Payday_3 == customer.DataBank_Payday_3) &&
                       (DataBank_Bank == customer.DataBank_Bank) &&
                       (DataBank_BankAddress == customer.DataBank_BankAddress) &&
                       (DataBank_IBAN_CountryCode == customer.DataBank_IBAN_CountryCode) &&
                       (DataBank_IBAN_BankCode == customer.DataBank_IBAN_BankCode) &&
                       (DataBank_IBAN_OfficeCode == customer.DataBank_IBAN_OfficeCode) &&
                       (DataBank_IBAN_CheckDigits == customer.DataBank_IBAN_CheckDigits) &&
                       (DataBank_IBAN_AccountNumber == customer.DataBank_IBAN_AccountNumber) &&
                       (Company_Cif == customer.Company_Cif) &&
                       (BillingData_BillingType == customer.BillingData_BillingType) &&
                       (BillingData_Duplicate == customer.BillingData_Duplicate) &&
                       (BillingData_EarlyPaymentDiscount == customer.BillingData_EarlyPaymentDiscount) &&
                       (BillingData_Valued == customer.BillingData_Valued) &&
                       (BillingData_RiskGranted == customer.BillingData_RiskGranted) &&
                       (BillingData_CurrentRisk == customer.BillingData_CurrentRisk) &&
                       (BillingData_Unpaid == customer.BillingData_Unpaid) &&
                       (BillingData_NumUnpaid == customer.BillingData_NumUnpaid) &&
                       (BillingData_Register.Date == customer.BillingData_Register.Date) &&
                       (Several_Remarks == customer.Several_Remarks) &&
                       (SeveralData_Acum_1 == customer.SeveralData_Acum_1) &&
                       (SeveralData_Acum_2 == customer.SeveralData_Acum_2) &&
                       (SeveralData_Acum_3 == customer.SeveralData_Acum_3) &&
                       (SeveralData_Acum_4 == customer.SeveralData_Acum_4) &&
                       (SeveralData_Acum_5 == customer.SeveralData_Acum_5) &&
                       (SeveralData_Acum_6 == customer.SeveralData_Acum_6) &&
                       (SeveralData_Acum_7 == customer.SeveralData_Acum_7) &&
                       (SeveralData_Acum_8 == customer.SeveralData_Acum_8) &&
                       (SeveralData_Acum_9 == customer.SeveralData_Acum_9) &&
                       (SeveralData_Acum_10 == customer.SeveralData_Acum_10) &&
                       (SeveralData_Acum_11 == customer.SeveralData_Acum_11) &&
                       (SeveralData_Acum_12 == customer.SeveralData_Acum_12) &&
                       (QueryData_Active == customer.QueryData_Active) &&
                       (QueryData_Print == customer.QueryData_Print) &&
                       (Canceled == customer.Canceled) &&
                       (_Company_PostalCode_Id == customer._Company_PostalCode_Id) &&
                       (_DataBank_EffectType_Id == customer._DataBank_EffectType_Id) &&
                       (_BillingData_Agent_Id == customer._BillingData_Agent_Id) &&
                       (_BillingData_IVAType_Id == customer._BillingData_IVAType_Id) &&
                       (_BillingData_SendType_Id == customer._BillingData_SendType_Id);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="customer_1">Primera instáncia a comparar.</param>
        /// <param name="customer_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(CustomersView customer_1, CustomersView customer_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(customer_1, customer_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)customer_1 == null) || ((object)customer_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (customer_1.Customer_Id == customer_2.Customer_Id) && (customer_1.Customer_Alias == customer_2.Customer_Alias) &&
                       (customer_1.Company_Name == customer_2.Company_Name) && (customer_1.Company_Address == customer_2.Company_Address) &&
                       (customer_1.Company_Phone_1 == customer_2.Company_Phone_1) && (customer_1.Company_Phone_2 == customer_2.Company_Phone_2) &&
                       (customer_1.Company_Fax == customer_2.Company_Fax) && (customer_1.Company_MobilePhone == customer_2.Company_MobilePhone) &&
                       (customer_1.Company_EMail == customer_2.Company_EMail) && (customer_1.Company_ContactPerson == customer_2.Company_ContactPerson) &&
                       (customer_1.Company_TimeTable == customer_2.Company_TimeTable) && (customer_1.Company_NumProv == customer_2.Company_NumProv) &&
                       (customer_1.DataBank_NumEffect == customer_2.DataBank_NumEffect) &&
                       (customer_1.DataBank_FirstExpirationData == customer_2.DataBank_FirstExpirationData) &&
                       (customer_1.DataBank_ExpirationInterval == customer_2.DataBank_ExpirationInterval) &&
                       (customer_1.DataBank_Payday_1 == customer_2.DataBank_Payday_1) &&
                       (customer_1.DataBank_Payday_2 == customer_2.DataBank_Payday_2) &&
                       (customer_1.DataBank_Payday_3 == customer_2.DataBank_Payday_3) &&
                       (customer_1.DataBank_Bank == customer_2.DataBank_Bank) &&
                       (customer_1.DataBank_IBAN_CountryCode == customer_2.DataBank_IBAN_CountryCode) &&
                       (customer_1.DataBank_BankAddress == customer_2.DataBank_BankAddress) &&
                       (customer_1.DataBank_IBAN_BankCode == customer_2.DataBank_IBAN_BankCode) &&
                       (customer_1.DataBank_IBAN_OfficeCode == customer_2.DataBank_IBAN_OfficeCode) &&
                       (customer_1.DataBank_IBAN_CheckDigits == customer_2.DataBank_IBAN_CheckDigits) &&
                       (customer_1.DataBank_IBAN_AccountNumber == customer_2.DataBank_IBAN_AccountNumber) &&
                       (customer_1.Company_Cif == customer_2.Company_Cif) &&
                       (customer_1.BillingData_BillingType == customer_2.BillingData_BillingType) &&
                       (customer_1.BillingData_Duplicate == customer_2.BillingData_Duplicate) &&
                       (customer_1.BillingData_EarlyPaymentDiscount == customer_2.BillingData_EarlyPaymentDiscount) &&
                       (customer_1.BillingData_Valued == customer_2.BillingData_Valued) &&
                       (customer_1.BillingData_RiskGranted == customer_2.BillingData_RiskGranted) &&
                       (customer_1.BillingData_CurrentRisk == customer_2.BillingData_CurrentRisk) &&
                       (customer_1.BillingData_Unpaid == customer_2.BillingData_Unpaid) &&
                       (customer_1.BillingData_NumUnpaid == customer_2.BillingData_NumUnpaid) &&
                       (customer_1.BillingData_Register.Date == customer_2.BillingData_Register.Date) &&
                       (customer_1.Several_Remarks == customer_2.Several_Remarks) &&
                       (customer_1.SeveralData_Acum_1 == customer_2.SeveralData_Acum_1) &&
                       (customer_1.SeveralData_Acum_2 == customer_2.SeveralData_Acum_2) &&
                       (customer_1.SeveralData_Acum_3 == customer_2.SeveralData_Acum_3) &&
                       (customer_1.SeveralData_Acum_4 == customer_2.SeveralData_Acum_4) &&
                       (customer_1.SeveralData_Acum_5 == customer_2.SeveralData_Acum_5) &&
                       (customer_1.SeveralData_Acum_6 == customer_2.SeveralData_Acum_6) &&
                       (customer_1.SeveralData_Acum_7 == customer_2.SeveralData_Acum_7) &&
                       (customer_1.SeveralData_Acum_8 == customer_2.SeveralData_Acum_8) &&
                       (customer_1.SeveralData_Acum_9 == customer_2.SeveralData_Acum_9) &&
                       (customer_1.SeveralData_Acum_10 == customer_2.SeveralData_Acum_10) &&
                       (customer_1.SeveralData_Acum_11 == customer_2.SeveralData_Acum_11) &&
                       (customer_1.SeveralData_Acum_12 == customer_2.SeveralData_Acum_12) &&
                       (customer_1.QueryData_Active == customer_2.QueryData_Active) &&
                       (customer_1.QueryData_Print == customer_2.QueryData_Print) &&
                       (customer_1.Canceled == customer_2.Canceled) &&
                       (customer_1._Company_PostalCode_Id == customer_2._Company_PostalCode_Id) &&
                       (customer_1._DataBank_EffectType_Id == customer_2._DataBank_EffectType_Id) &&
                       (customer_1._BillingData_Agent_Id == customer_2._BillingData_Agent_Id) &&
                       (customer_1._BillingData_IVAType_Id == customer_2._BillingData_IVAType_Id) &&
                       (customer_1._BillingData_SendType_Id == customer_2._BillingData_SendType_Id);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="customer_1">Primera instáncia a comparar.</param>
        /// <param name="customer_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(CustomersView customer_1, CustomersView customer_2)
        {
            return !(customer_1 == customer_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(Customer_Id, Customer_Alias).GetHashCode());
        }

        #endregion

        #region Override Methods

        public override string ToString()
        {
            return (string.Empty);

            // Activate if its needed
            //try
            //{
            //    StringBuilder sbInfo = new StringBuilder(); 
            //    sbInfo.AppendFormat("Id: {0}\r\nAlias: {1}\r\nName: {2}\r\nCIF: {3}\r\n", Customer_Id, Customer_Alias, Company_Name, Company_Cif);
            //    if (!(Company_PostalCode is null)) sbInfo.AppendFormat("Còdi postal: {0}\r\n", GlobalViewModel.Instance.HispaniaViewModel.GetKeyPostalCodeView(Company_PostalCode));
            //    sbInfo.AppendFormat("Num Efect: {0}\r\nFirst Expiration Data: {1}\r\nExpiration Interval: {2}\r\n",
            //                        DataBank_NumEffect, DataBank_FirstExpirationData, DataBank_ExpirationInterval);
            //    sbInfo.AppendFormat("Dia Pagament (1-2-3): {0}-{1}-{2}\r\nBanc: {3}\r\nAdreça del Banc: {4}\r\nIBAN: {5}-{6}-{7}-{8}-{9}\r\n",
            //                         DataBank_Payday_1, DataBank_Payday_2, DataBank_Payday_3, DataBank_Bank, DataBank_BankAddress, DataBank_IBAN_CountryCode, 
            //                         DataBank_IBAN_BankCode, DataBank_IBAN_OfficeCode, DataBank_IBAN_CheckDigits, DataBank_IBAN_AccountNumber);
            //    sbInfo.AppendFormat("DTPI: {0}\r\nRisc concedit: {1}\r\nRisc actual: {2}\r\nImpagats: {3}\r\nNúmero impagats: {4}\r\n",
            //                        BillingData_EarlyPaymentDiscount, BillingData_RiskGranted, BillingData_CurrentRisk, BillingData_Unpaid, BillingData_NumUnpaid);
            //    sbInfo.AppendFormat("Acumulats: Gener {0} Febrer {1} Març {2}\r\n           Abril {3} Maig {4} Juny {5}\r\n" +
            //                        "           Juliol {6} Agost {7} Setembre {8}\r\n          Octubre {9} Novembre {10} Desembre {11}\r\n",
            //                        SeveralData_Acum_1, SeveralData_Acum_2, SeveralData_Acum_3, SeveralData_Acum_4, SeveralData_Acum_5, SeveralData_Acum_6,
            //                        SeveralData_Acum_7, SeveralData_Acum_8, SeveralData_Acum_9, SeveralData_Acum_10, SeveralData_Acum_11, SeveralData_Acum_12);
            //    if (!(DataBank_EffectType is null)) sbInfo.AppendFormat("Tipus d'efecte: {0}\r\n", DataBank_EffectType.Description);
            //    if (!(BillingData_IVAType is null)) sbInfo.AppendFormat("Tipus d'IVA i Recàrrec: {0}\r\n", GlobalViewModel.Instance.HispaniaViewModel.GetKeyIVATypeView(BillingData_IVAType));
            //    if (!(BillingData_SendType is null)) sbInfo.AppendFormat("Tipus d'enviament: {0}\r\n", BillingData_SendType.Description);
            //    if (!(BillingData_Agent is null) && !(BillingData_Agent.Name is null)) sbInfo.AppendFormat("Agent Name: {0}\r\n", BillingData_Agent.Name);
            //    return (sbInfo.ToString());
            //}
            //catch (Exception)
            //{
            //    return (string.Empty);
            //}
        }

        #endregion
    }
}
