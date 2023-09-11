#region Librerias usadas por la clase

using HispaniaCommon.DataAccess;
using HispaniaComptabilitat.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using HispaniaCompData = HispaniaComptabilitat.Data;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Camps del Tipus
    /// </summary>
    public enum ProviderOrdersAttributes
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
        Provider,
        PostalCode,
        SendType,
        EffectType,
        BillingData_Agent,
        None,
    }

    /// <summary>
    /// Class that Store the information of a ProviderOrder.
    /// </summary>
    public class ProviderOrdersView : IMenuView
    {
        #region Fields for Filter

        /// <summary>
        /// Store the list of fields that compose the ProviderOrder class
        /// </summary>
        private static Dictionary<string, string> m_Fields = null;

        /// <summary>
        /// Get the list of fields that compose the ProviderOrder class
        /// </summary>
        public static Dictionary<string, string> Fields
        {
            get
            {
                if (m_Fields == null)
                {
                    m_Fields = new Dictionary<string, string>
                    {
                        { "Numero de Comanda", "ProviderOrder_Id" },
                        { "Data de Comanda", "Date_Str" },
                        { "Nam de Client","NameClientAssoc"},
                        { "Numero de Client", "Provider_Id_Str" },
                        { "Client", "Provider_Alias" },
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
        /// Store the list of fields that compose the ProviderOrder class
        /// </summary>
        private static Dictionary<string, string> m_DeliveryNoteFields = null;

        /// <summary>
        /// Get the list of fields that compose the ProviderOrder class
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
                        { "Nam de Client","NameClientAssoc"},
                        { "Numero de Client", "Provider_Id_Str" },
                        { "Client", "Provider_Alias" },
                        { "Numero de Comanda", "ProviderOrder_Id" },
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
                return GlobalViewModel.GetStringFromIntIdValue(ProviderOrder_Id);
            }
        }

        #endregion

        #region Main Fields

        public int ProviderOrder_Id { get; set; }
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

        /// <summary>
        /// 
        /// </summary>
        public string NameClientAssoc
        {
            get;
            set; 
        }

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
                return (According == true) ? "Material lliurat" : 
                            ( (PrevisioLliurament==true) ? "previsio: " + PrevisioLliurament_Str 
                            : "Lliurament pendent");
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

        public bool PrevisioLliurament { get; set; }
        public string PrevisioLliurament_Str
        {
            get
            {
                return (PrevisioLliurament == true) ? PrevisioLliuramentData.ToShortDateString() : "";
            }
        }

        public DateTime PrevisioLliuramentData { get; set; }

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
        public bool Print_ProviderOrder { get; set; }
        public string Print_ProviderOrder_Str
        {
            get
            {
                return (Print_ProviderOrder == true) ? "Imprés" : "No Imprés";
            }
        }
        public bool SendByEMail_ProviderOrder { get; set; }
        public string SendByEMail_ProviderOrder_Str
        {
            get
            {
                return (SendByEMail_ProviderOrder == true) ? "Generat" : "No Generat";
            }
        }
        public bool Historic { get; set; }
        public bool Select_Bill { get; set; }
        public bool Expiration { get; set; }
        public bool Daily { get; set; }

        #endregion

        #region ForeignKey Properties

        #region Provider

        private int _Provider_Id { get; set; }

        private ProvidersView _Provider;

        public ProvidersView Provider
        {
            get
            {
                if ((_Provider == null) && (_Provider_Id != GlobalViewModel.IntIdInitValue))
                {
                    _Provider = new ProvidersView(GlobalViewModel.Instance.HispaniaViewModel.GetProvider((int)_Provider_Id));
                }
                return (_Provider);
            }
            set
            {
                if (value != null)
                {
                    _Provider = new ProvidersView(value);
                    if (_Provider == null) _Provider_Id = GlobalViewModel.IntIdInitValue;
                    else _Provider_Id = _Provider.Provider_Id;
                }
                else
                {
                    _Provider = null;
                    _Provider_Id = GlobalViewModel.IntIdInitValue;
                }
            }
        }

        public string Provider_Id_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromIntIdValue(_Provider_Id);
            }
        }

        public string Provider_Alias
        {
            get
            {
                return (Provider is null) ? string.Empty : Provider.Alias;
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
                return GlobalViewModel.Instance.HispaniaViewModel.ProviderOrderMovementsComments(ProviderOrder_Id);
            }
        }

        public string AccordingMovements_Str
        {
            get
            {
                bool? LiniesConformes = GlobalViewModel.Instance.HispaniaViewModel.LiniesProveidorConformes(ProviderOrder_Id);
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
                bool? LiniesConformes = GlobalViewModel.Instance.HispaniaViewModel.LiniesProveidorConformes(ProviderOrder_Id);
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
        public ProviderOrdersView()
        {
            ProviderOrder_Id = GlobalViewModel.IntIdInitValue;
            _Provider_Id = GlobalViewModel.IntIdInitValue;
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
            Print_ProviderOrder = false;
            SendByEMail_ProviderOrder = false;
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
        internal ProviderOrdersView(HispaniaCompData.ProviderOrder providerOrder)
        {
            ProviderOrder_Id = providerOrder.ProviderOrder_Id;
            _Provider_Id = providerOrder.Provider_Id;
            Date = GlobalViewModel.GetDateTimeValue(providerOrder.Date);
            Bill_Id = GlobalViewModel.GetIntFromIntIdValue(providerOrder.Bill_Id);
            Bill_Year = GlobalViewModel.GetDecimalYearValue(providerOrder.Bill_Year);
            Bill_Date = GlobalViewModel.GetDateTimeValue(providerOrder.Bill_Date);
            DeliveryNote_Id = GlobalViewModel.GetIntFromIntIdValue(providerOrder.DeliveryNote_Id);
            DeliveryNote_Year = GlobalViewModel.GetDecimalYearValue(providerOrder.DeliveryNote_Year);
            DeliveryNote_Date = GlobalViewModel.GetDateTimeValue(providerOrder.DeliveryNote_Date);
            Address = providerOrder.Address;
            TimeTable = providerOrder.TimeTable;
            DataBank_NumEffect = providerOrder.NumEffect;
            DataBank_ExpirationDays = providerOrder.DataBank_ExpirationDays;
            DataBank_ExpirationInterval = providerOrder.DataBank_ExpirationInterval;
            DataBank_Payday_1 = providerOrder.DataBank_Paydays1;
            DataBank_Payday_2 = providerOrder.DataBank_Paydays2;
            DataBank_Payday_3 = providerOrder.DataBank_Paydays3;
            DataBank_Bank = providerOrder.DataBank_Bank;
            DataBank_BankAddress = providerOrder.DataBank_BankAddress;
            DataBank_IBAN_CountryCode = providerOrder.DataBank_IBAN_CountryCode;
            DataBank_IBAN_BankCode = providerOrder.DataBank_IBAN_BankCode;
            DataBank_IBAN_OfficeCode = providerOrder.DataBank_IBAN_OfficeCode;
            DataBank_IBAN_CheckDigits = providerOrder.DataBank_IBAN_CheckDigits;
            DataBank_IBAN_AccountNumber = providerOrder.DataBank_IBAN_AccountNumber;
            BillingData_EarlyPaymentDiscount = GlobalViewModel.GetDecimalValue(providerOrder.BillingData_EarlyPaymentDiscount);
            Remarks = providerOrder.Remarks;
            Daily_Dates = GlobalViewModel.GetDateTimeValue(providerOrder.Daily_Dates);
            According = providerOrder.According;
            Valued = providerOrder.Valued;
            Transfer = providerOrder.Transfer;
            Print = providerOrder.Print;
            SendByEMail = providerOrder.SendByEMail;
            Print_ProviderOrder = providerOrder.Print_ProviderOrder;
            SendByEMail_ProviderOrder = providerOrder.SendByEmail_ProviderOrder;
            Historic = providerOrder.Historic;
            Select_Bill = providerOrder.Select_Bill;
            Expiration = providerOrder.Expiration;
            Daily = providerOrder.Daily;
            TotalAmount = providerOrder.TotalAmount;
            _PostalCode_Id = GlobalViewModel.GetIntFromIntIdValue(providerOrder.PostalCode_Id);
            _SendType_Id = GlobalViewModel.GetIntFromIntIdValue(providerOrder.SendType_Id);
            _EffectType_Id = GlobalViewModel.GetIntFromIntIdValue(providerOrder.EffectType_Id);
            IVAPercent = GlobalViewModel.GetDecimalValue(providerOrder.IVAPercent);
            SurchargePercent = GlobalViewModel.GetDecimalValue(providerOrder.SurchargePercent);
            _Bill_Serie_Id = GlobalViewModel.GetIntFromIntIdValue(providerOrder.Bill_Serie_Id);
            _BillingData_Agent_Id = GlobalViewModel.GetIntFromIntIdValue(providerOrder.BillingData_Agent_Id);
            PrevisioLliurament = providerOrder.PrevisioLliurament.HasValue ? providerOrder.PrevisioLliurament.Value : false ;
            PrevisioLliuramentData = providerOrder.PrevisioLliuramentData.HasValue ? providerOrder.PrevisioLliuramentData.Value : DateTime.MinValue;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public ProviderOrdersView(ProviderOrdersView providerOrder)
        {
            ProviderOrder_Id = providerOrder.ProviderOrder_Id;
            _Provider_Id = providerOrder._Provider_Id;
            Date = providerOrder.Date;
            Bill_Id = providerOrder.Bill_Id;
            Bill_Year = providerOrder.Bill_Year;
            Bill_Date = providerOrder.Bill_Date;
            DeliveryNote_Id = providerOrder.DeliveryNote_Id;
            DeliveryNote_Year = providerOrder.DeliveryNote_Year;
            DeliveryNote_Date = providerOrder.DeliveryNote_Date;
            Address = providerOrder.Address;
            TimeTable = providerOrder.TimeTable;
            DataBank_NumEffect = providerOrder.DataBank_NumEffect;
            DataBank_ExpirationDays = providerOrder.DataBank_ExpirationDays;
            DataBank_ExpirationInterval = providerOrder.DataBank_ExpirationInterval;
            DataBank_Payday_1 = providerOrder.DataBank_Payday_1;
            DataBank_Payday_2 = providerOrder.DataBank_Payday_2;
            DataBank_Payday_3 = providerOrder.DataBank_Payday_3;
            DataBank_Bank = providerOrder.DataBank_Bank;
            DataBank_BankAddress = providerOrder.DataBank_BankAddress;
            DataBank_IBAN_CountryCode = providerOrder.DataBank_IBAN_CountryCode;
            DataBank_IBAN_BankCode = providerOrder.DataBank_IBAN_BankCode;
            DataBank_IBAN_OfficeCode = providerOrder.DataBank_IBAN_OfficeCode;
            DataBank_IBAN_CheckDigits = providerOrder.DataBank_IBAN_CheckDigits;
            DataBank_IBAN_AccountNumber = providerOrder.DataBank_IBAN_AccountNumber;
            BillingData_EarlyPaymentDiscount = providerOrder.BillingData_EarlyPaymentDiscount;
            Remarks = providerOrder.Remarks;
            Daily_Dates = providerOrder.Daily_Dates;
            According = providerOrder.According;
            Valued = providerOrder.Valued;
            Transfer = providerOrder.Transfer;
            Print = providerOrder.Print;
            SendByEMail = providerOrder.SendByEMail;
            Print_ProviderOrder = providerOrder.Print_ProviderOrder;
            SendByEMail_ProviderOrder = providerOrder.SendByEMail_ProviderOrder;
            Historic = providerOrder.Historic;
            Select_Bill = providerOrder.Select_Bill;
            Expiration = providerOrder.Expiration;
            Daily = providerOrder.Daily;
            TotalAmount = providerOrder.TotalAmount;
            PostalCode = providerOrder.PostalCode;
            SendType = providerOrder.SendType;
            EffectType = providerOrder.EffectType;
            IVAPercent = providerOrder.IVAPercent;
            SurchargePercent = providerOrder.SurchargePercent;
            Bill_Serie = providerOrder.Bill_Serie;            
            PrevisioLliurament = providerOrder.PrevisioLliurament;
            PrevisioLliuramentData = providerOrder.PrevisioLliuramentData;
            NameClientAssoc = providerOrder.NameClientAssoc;
        }

        #endregion

        #region GetProviderOrder

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.ProviderOrder GetProviderOrder()
        {
            HispaniaCompData.ProviderOrder ProviderOrder = new HispaniaCompData.ProviderOrder()
            {
                ProviderOrder_Id = ProviderOrder_Id,
                Provider_Id = _Provider_Id,
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
                Print_ProviderOrder = Print_ProviderOrder,
                SendByEmail_ProviderOrder = SendByEMail_ProviderOrder,
                Historic = Historic,
                Select_Bill = Select_Bill,
                Expiration = Expiration,
                Daily = Daily,
                TotalAmount = TotalAmount,
                PostalCode_Id = _PostalCode_Id,
                SendType_Id = _SendType_Id,
                EffectType_Id = _EffectType_Id,
                IVAPercent = IVAPercent,
                SurchargePercent = SurchargePercent,
                PrevisioLliurament = PrevisioLliurament,
                PrevisioLliuramentData = PrevisioLliuramentData,
            };
            return (ProviderOrder);
        }

        #endregion

        #region Validate

        /// <summary>
        /// Validate the data contained in the instance of the class.
        /// </summary>
        public void Validate(out ProviderOrdersAttributes ErrorField)
        {
            ErrorField = ProviderOrdersAttributes.None;
            if (!GlobalViewModel.IsEmptyOrAddress(Address))
            {
                ErrorField = ProviderOrdersAttributes.Address;
                throw new FormatException("Error, format incorrecte de l'ADREÇA DEL CLIENT");
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(DataBank_NumEffect, "NUMERO D''EFECTE", out string ErrMsg))
            {
                ErrorField = ProviderOrdersAttributes.NumEffect;
                throw new FormatException(ErrMsg);
            }
            #region Bank Data (Bancarios)
            if (!GlobalViewModel.IsEmptyOrShortDecimal(DataBank_ExpirationDays, "DIES D''EXPIRACIÓ", out ErrMsg))
            {
                ErrorField = ProviderOrdersAttributes.DataBank_ExpirationDays;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(DataBank_ExpirationInterval, "INTERVAL DE VENCIMENT", out ErrMsg))
            {
                ErrorField = ProviderOrdersAttributes.DataBank_ExpirationInterval;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(DataBank_Payday_1, "PRIMER DIA DE PAGAMENT", out ErrMsg))
            {
                ErrorField = ProviderOrdersAttributes.DataBank_Payday_1;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(DataBank_Payday_2, "SEGON DIA DE PAGAMENT", out ErrMsg))
            {
                ErrorField = ProviderOrdersAttributes.DataBank_Payday_2;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(DataBank_Payday_3, "TERCER DIA DE PAGAMENT", out ErrMsg))
            {
                ErrorField = ProviderOrdersAttributes.DataBank_Payday_3;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrName(DataBank_Bank))
            {
                ErrorField = ProviderOrdersAttributes.DataBank_Bank;
                throw new FormatException("Error, format incorrecte del NOM DEL BANC");
            }
            if (!GlobalViewModel.IsEmptyOrAddress(DataBank_BankAddress))
            {
                ErrorField = ProviderOrdersAttributes.DataBank_BankAddress;
                throw new FormatException("Error, format incorrecte de l'ADREÇA DEL BANC");
            }
            string CCC = string.Format("{0}{1}{2}{3}", DataBank_IBAN_BankCode, DataBank_IBAN_OfficeCode,
                                                       DataBank_IBAN_CheckDigits, DataBank_IBAN_AccountNumber);
            if (!GlobalViewModel.IsEmptyOrIBAN_CountryCode(DataBank_IBAN_CountryCode, CCC, out ErrMsg))
            {
                ErrorField = ProviderOrdersAttributes.DataBank_IBAN_CountryCode;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrIBAN_BankCode(DataBank_IBAN_BankCode))
            {
                ErrorField = ProviderOrdersAttributes.DataBank_IBAN_BankCode;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrIBAN_BankCodeError);
            }
            if (!GlobalViewModel.IsEmptyOrIBAN_OfficeCode(DataBank_IBAN_OfficeCode))
            {
                ErrorField = ProviderOrdersAttributes.DataBank_IBAN_OfficeCode;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrIBAN_OfficeCodeError);
            }
            if (!GlobalViewModel.IsEmptyOrIBAN_CheckDigits(DataBank_IBAN_CheckDigits))
            {
                ErrorField = ProviderOrdersAttributes.DataBank_IBAN_CheckDigits;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrIBAN_CheckDigitsError);
            }
            if (!GlobalViewModel.IsEmptyOrIBAN_AccountNumber(DataBank_IBAN_AccountNumber))
            {
                ErrorField = ProviderOrdersAttributes.DataBank_IBAN_AccountNumber;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrIBAN_AccountNumberError);
            }
            #endregion
            #region Billing Data (Facturación)
            if (!GlobalViewModel.IsEmptyOrPercent(BillingData_EarlyPaymentDiscount, "DESCOMPTE PAGAMENT IMMEDIAT", out ErrMsg))
            {
                ErrorField = ProviderOrdersAttributes.BillingData_EarlyPaymentDiscount;
                throw new FormatException(ErrMsg);
            }
            #endregion
            #region Diversos

            if (!GlobalViewModel.IsEmptyOrComment(Remarks))
            {
                ErrorField = ProviderOrdersAttributes.Remarks;
                throw new FormatException(GlobalViewModel.ValidationCommentError);
            }
            #endregion
            if (Provider == null)
            {
                ErrorField = ProviderOrdersAttributes.Provider;
                throw new FormatException("Error, manca seleccionar el Client.");
            }
            if (PostalCode == null)
            {
                ErrorField = ProviderOrdersAttributes.PostalCode;
                throw new FormatException("Error, manca seleccionar el Codi Postal.");
            }
            if (SendType == null)
            {
                ErrorField = ProviderOrdersAttributes.SendType;
                throw new FormatException("Error, manca seleccionar el Tipus d'Enviament.");
            }
            if (EffectType == null)
            {
                ErrorField = ProviderOrdersAttributes.EffectType;
                throw new FormatException("Error, manca seleccionar el Tipus d'Efecte.");
            }            
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(ProviderOrdersView Data)
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
            Print_ProviderOrder = Data.Print_ProviderOrder;
            SendByEMail_ProviderOrder = Data.SendByEMail_ProviderOrder;
            Historic = Data.Historic;
            Select_Bill = Data.Select_Bill;
            Expiration = Data.Expiration;
            Daily = Data.Daily;
            Provider = Data.Provider;
            PostalCode = Data.PostalCode;
            SendType = Data.SendType;
            EffectType = Data.EffectType;
            BillingData_Agent = Data.BillingData_Agent;
            PrevisioLliurament = Data.PrevisioLliurament;
            PrevisioLliuramentData = Data.PrevisioLliuramentData;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(ProviderOrdersView Data, ProviderOrdersAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case ProviderOrdersAttributes.DataBank_ExpirationDays:
                     DataBank_ExpirationDays = Data.DataBank_ExpirationDays;
                     break;
                case ProviderOrdersAttributes.DataBank_ExpirationInterval:
                     DataBank_ExpirationInterval = Data.DataBank_ExpirationInterval;
                     break;
                case ProviderOrdersAttributes.DataBank_Payday_1:
                     DataBank_Payday_1 = Data.DataBank_Payday_1;
                     break;
                case ProviderOrdersAttributes.DataBank_Payday_2:
                     DataBank_Payday_2 = Data.DataBank_Payday_2;
                     break;
                case ProviderOrdersAttributes.DataBank_Payday_3:
                     DataBank_Payday_3 = Data.DataBank_Payday_3;
                     break;
                case ProviderOrdersAttributes.DataBank_Bank:
                     DataBank_Bank = Data.DataBank_Bank;
                     break;
                case ProviderOrdersAttributes.DataBank_BankAddress:
                     DataBank_BankAddress = Data.DataBank_BankAddress;
                     break;
                case ProviderOrdersAttributes.DataBank_IBAN_CountryCode:
                     DataBank_IBAN_CountryCode = Data.DataBank_IBAN_CountryCode;
                     break;
                case ProviderOrdersAttributes.DataBank_IBAN_BankCode:
                     DataBank_IBAN_BankCode = Data.DataBank_IBAN_BankCode;
                     break;
                case ProviderOrdersAttributes.DataBank_IBAN_OfficeCode:
                     DataBank_IBAN_OfficeCode = Data.DataBank_IBAN_OfficeCode;
                     break;
                case ProviderOrdersAttributes.DataBank_IBAN_CheckDigits:
                     DataBank_IBAN_CheckDigits = Data.DataBank_IBAN_CheckDigits;
                     break;
                case ProviderOrdersAttributes.DataBank_IBAN_AccountNumber:
                     DataBank_IBAN_AccountNumber = Data.DataBank_IBAN_AccountNumber;
                     break;
                case ProviderOrdersAttributes.BillingData_EarlyPaymentDiscount:
                     BillingData_EarlyPaymentDiscount = Data.BillingData_EarlyPaymentDiscount;
                     break;
                case ProviderOrdersAttributes.Remarks:
                     Remarks = Data.Remarks;
                     break;
                case ProviderOrdersAttributes.Daily_Dates:
                     Daily_Dates = Data.Daily_Dates;
                     break;
                case ProviderOrdersAttributes.According:
                     According = Data.According;
                     break;
                case ProviderOrdersAttributes.Valued:
                     Valued = Data.Valued;
                     break;
                case ProviderOrdersAttributes.Transfer:
                     Transfer = Data.Transfer;
                     break;
                case ProviderOrdersAttributes.Historic:
                     Historic = Data.Historic;
                     break;
                case ProviderOrdersAttributes.Select_Bill:
                     Select_Bill = Data.Select_Bill;
                     break;
                case ProviderOrdersAttributes.Expiration:
                     Expiration = Data.Expiration;
                     break;
                case ProviderOrdersAttributes.Daily:
                     Daily = Data.Daily;
                     break;
                case ProviderOrdersAttributes.Provider:
                     Provider = Data.Provider;
                     break;
                case ProviderOrdersAttributes.PostalCode:
                     PostalCode = Data.PostalCode;
                     break;
                case ProviderOrdersAttributes.SendType:
                     SendType = Data.SendType;
                     break;
                case ProviderOrdersAttributes.EffectType:
                     EffectType = Data.EffectType;
                     break;
                case ProviderOrdersAttributes.BillingData_Agent:
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
                ProviderOrdersView providerOrder = obj as ProviderOrdersView;
                if ((Object)providerOrder == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (ProviderOrder_Id == providerOrder.ProviderOrder_Id) && 
                       (_Provider_Id == providerOrder._Provider_Id) && (Date == providerOrder.Date) &&
                       (Bill_Id == providerOrder.Bill_Id) && (Bill_Year == providerOrder.Bill_Year) &&
                       (_Bill_Serie_Id == providerOrder._Bill_Serie_Id) && (Bill_Date == providerOrder.Bill_Date) &&
                       (DeliveryNote_Id == providerOrder.DeliveryNote_Id) && 
                       (DeliveryNote_Year == providerOrder.DeliveryNote_Year) &&
                       (DeliveryNote_Date == providerOrder.DeliveryNote_Date) && 
                       (_PostalCode_Id == providerOrder._PostalCode_Id) &&
                       (_EffectType_Id == providerOrder._EffectType_Id) && 
                       (_SendType_Id == providerOrder._SendType_Id) &&
                       (IVAPercent == providerOrder.IVAPercent) &&
                       (SurchargePercent == providerOrder.SurchargePercent) &&
                       (Address == providerOrder.Address) &&
                       (TimeTable == providerOrder.TimeTable) &&
                       (_BillingData_Agent_Id == providerOrder._BillingData_Agent_Id) && 
                       (DataBank_NumEffect == providerOrder.DataBank_NumEffect) &&
                       (DataBank_ExpirationDays == providerOrder.DataBank_ExpirationDays) &&
                       (DataBank_ExpirationInterval == providerOrder.DataBank_ExpirationInterval) &&
                       (DataBank_Payday_1 == providerOrder.DataBank_Payday_1) &&
                       (DataBank_Payday_2 == providerOrder.DataBank_Payday_2) &&
                       (DataBank_Payday_3 == providerOrder.DataBank_Payday_3) &&
                       (DataBank_Bank == providerOrder.DataBank_Bank) &&
                       (DataBank_BankAddress == providerOrder.DataBank_BankAddress) &&
                       (DataBank_IBAN_CountryCode == providerOrder.DataBank_IBAN_CountryCode) &&
                       (DataBank_IBAN_BankCode == providerOrder.DataBank_IBAN_BankCode) &&
                       (DataBank_IBAN_OfficeCode == providerOrder.DataBank_IBAN_OfficeCode) &&
                       (DataBank_IBAN_CheckDigits == providerOrder.DataBank_IBAN_CheckDigits) &&
                       (DataBank_IBAN_AccountNumber == providerOrder.DataBank_IBAN_AccountNumber) &&
                       (BillingData_EarlyPaymentDiscount == providerOrder.BillingData_EarlyPaymentDiscount) &&
                       (Remarks == providerOrder.Remarks) &&
                       (Daily_Dates == providerOrder.Daily_Dates) &&
                       (According == providerOrder.According) &&
                       (Valued == providerOrder.Valued) &&
                       (Transfer == providerOrder.Transfer) &&
                       (Print == providerOrder.Print) &&
                       (SendByEMail == providerOrder.SendByEMail) &&
                       (Print_ProviderOrder == providerOrder.Print_ProviderOrder) &&
                       (SendByEMail_ProviderOrder == providerOrder.SendByEMail_ProviderOrder) &&                      
                       (Historic == providerOrder.Historic) &&
                       (Select_Bill == providerOrder.Select_Bill) &&
                       (Expiration == providerOrder.Expiration) &&
                       (TotalAmount == providerOrder.TotalAmount) &&
                       (Daily == providerOrder.Daily) &&
                       (PrevisioLliurament == providerOrder.PrevisioLliurament) &&
                       (PrevisioLliuramentData == providerOrder.PrevisioLliuramentData);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(ProviderOrdersView providerOrder)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)providerOrder == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (ProviderOrder_Id == providerOrder.ProviderOrder_Id) &&
                       (_Provider_Id == providerOrder._Provider_Id) && (Date == providerOrder.Date) &&
                       (Bill_Id == providerOrder.Bill_Id) && (Bill_Year == providerOrder.Bill_Year) &&
                       (_Bill_Serie_Id == providerOrder._Bill_Serie_Id) && (Bill_Date == providerOrder.Bill_Date) &&
                       (DeliveryNote_Id == providerOrder.DeliveryNote_Id) && 
                       (DeliveryNote_Year == providerOrder.DeliveryNote_Year) &&
                       (DeliveryNote_Date == providerOrder.DeliveryNote_Date) && 
                       (_PostalCode_Id == providerOrder._PostalCode_Id) &&
                       (_EffectType_Id == providerOrder._EffectType_Id) && 
                       (_SendType_Id == providerOrder._SendType_Id) &&
                       (IVAPercent == providerOrder.IVAPercent) &&
                       (SurchargePercent == providerOrder.SurchargePercent) &&
                       (_BillingData_Agent_Id == providerOrder._BillingData_Agent_Id) &&
                       (Address == providerOrder.Address) &&
                       (TimeTable == providerOrder.TimeTable) &&
                       (DataBank_NumEffect == providerOrder.DataBank_NumEffect) &&
                       (DataBank_ExpirationDays == providerOrder.DataBank_ExpirationDays) &&
                       (DataBank_ExpirationInterval == providerOrder.DataBank_ExpirationInterval) &&
                       (DataBank_Payday_1 == providerOrder.DataBank_Payday_1) &&
                       (DataBank_Payday_2 == providerOrder.DataBank_Payday_2) &&
                       (DataBank_Payday_3 == providerOrder.DataBank_Payday_3) &&
                       (DataBank_Bank == providerOrder.DataBank_Bank) &&
                       (DataBank_BankAddress == providerOrder.DataBank_BankAddress) &&
                       (DataBank_IBAN_CountryCode == providerOrder.DataBank_IBAN_CountryCode) &&
                       (DataBank_IBAN_BankCode == providerOrder.DataBank_IBAN_BankCode) &&
                       (DataBank_IBAN_OfficeCode == providerOrder.DataBank_IBAN_OfficeCode) &&
                       (DataBank_IBAN_CheckDigits == providerOrder.DataBank_IBAN_CheckDigits) &&
                       (DataBank_IBAN_AccountNumber == providerOrder.DataBank_IBAN_AccountNumber) &&
                       (BillingData_EarlyPaymentDiscount == providerOrder.BillingData_EarlyPaymentDiscount) &&
                       (Remarks == providerOrder.Remarks) &&
                       (Daily_Dates == providerOrder.Daily_Dates) &&
                       (According == providerOrder.According) &&
                       (Valued == providerOrder.Valued) &&
                       (Transfer == providerOrder.Transfer) &&
                       (Print == providerOrder.Print) &&
                       (SendByEMail == providerOrder.SendByEMail) &&
                       (TotalAmount == providerOrder.TotalAmount) &&
                       (Print_ProviderOrder == providerOrder.Print_ProviderOrder) &&
                       (SendByEMail_ProviderOrder == providerOrder.SendByEMail_ProviderOrder) &&
                       (Historic == providerOrder.Historic) &&
                       (Select_Bill == providerOrder.Select_Bill) &&
                       (Expiration == providerOrder.Expiration) &&
                       (Daily == providerOrder.Daily) &&
                       (PrevisioLliurament == providerOrder.PrevisioLliurament) &&
                       (PrevisioLliuramentData == providerOrder.PrevisioLliuramentData);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="providerOrder_1">Primera instáncia a comparar.</param>
        /// <param name="providerOrder_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(ProviderOrdersView providerOrder_1, ProviderOrdersView providerOrder_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(providerOrder_1, providerOrder_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)providerOrder_1 == null) || ((object)providerOrder_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (providerOrder_1.ProviderOrder_Id == providerOrder_2.ProviderOrder_Id) && 
                       (providerOrder_1._Provider_Id == providerOrder_2._Provider_Id) &&
                       (providerOrder_1.Date == providerOrder_2.Date) &&
                       (providerOrder_1.Bill_Id == providerOrder_2.Bill_Id) && 
                       (providerOrder_1.Bill_Year == providerOrder_2.Bill_Year) &&
                       (providerOrder_1._Bill_Serie_Id == providerOrder_2._Bill_Serie_Id) && 
                       (providerOrder_1.Bill_Date == providerOrder_2.Bill_Date) &&
                       (providerOrder_1.DeliveryNote_Id == providerOrder_2.DeliveryNote_Id) && 
                       (providerOrder_1.DeliveryNote_Year == providerOrder_2.DeliveryNote_Year) &&
                       (providerOrder_1.DeliveryNote_Date == providerOrder_2.DeliveryNote_Date) && 
                       (providerOrder_1._PostalCode_Id == providerOrder_2._PostalCode_Id) &&
                       (providerOrder_1._EffectType_Id == providerOrder_2._EffectType_Id) && 
                       (providerOrder_1._SendType_Id == providerOrder_2._SendType_Id) &&
                       (providerOrder_1.IVAPercent == providerOrder_2.IVAPercent) &&
                       (providerOrder_1.SurchargePercent == providerOrder_2.SurchargePercent) &&
                       (providerOrder_1._BillingData_Agent_Id == providerOrder_2._BillingData_Agent_Id) &&
                       (providerOrder_1.Address == providerOrder_2.Address) &&
                       (providerOrder_1.TimeTable == providerOrder_2.TimeTable) &&
                       (providerOrder_1.DataBank_NumEffect == providerOrder_2.DataBank_NumEffect) &&
                       (providerOrder_1.DataBank_ExpirationDays == providerOrder_2.DataBank_ExpirationDays) &&
                       (providerOrder_1.DataBank_ExpirationInterval == providerOrder_2.DataBank_ExpirationInterval) &&
                       (providerOrder_1.DataBank_Payday_1 == providerOrder_2.DataBank_Payday_1) &&
                       (providerOrder_1.DataBank_Payday_2 == providerOrder_2.DataBank_Payday_2) &&
                       (providerOrder_1.DataBank_Payday_3 == providerOrder_2.DataBank_Payday_3) &&
                       (providerOrder_1.DataBank_Bank == providerOrder_2.DataBank_Bank) &&
                       (providerOrder_1.DataBank_IBAN_CountryCode == providerOrder_2.DataBank_IBAN_CountryCode) &&
                       (providerOrder_1.DataBank_BankAddress == providerOrder_2.DataBank_BankAddress) &&
                       (providerOrder_1.DataBank_IBAN_BankCode == providerOrder_2.DataBank_IBAN_BankCode) &&
                       (providerOrder_1.DataBank_IBAN_OfficeCode == providerOrder_2.DataBank_IBAN_OfficeCode) &&
                       (providerOrder_1.DataBank_IBAN_CheckDigits == providerOrder_2.DataBank_IBAN_CheckDigits) &&
                       (providerOrder_1.DataBank_IBAN_AccountNumber == providerOrder_2.DataBank_IBAN_AccountNumber) &&
                       (providerOrder_1.BillingData_EarlyPaymentDiscount == providerOrder_2.BillingData_EarlyPaymentDiscount) &&
                       (providerOrder_1.Remarks == providerOrder_2.Remarks) &&
                       (providerOrder_1.Daily_Dates == providerOrder_2.Daily_Dates) &&
                       (providerOrder_1.According == providerOrder_2.According) &&
                       (providerOrder_1.Valued == providerOrder_2.Valued) &&
                       (providerOrder_1.Transfer == providerOrder_2.Transfer) &&
                       (providerOrder_1.Print == providerOrder_2.Print) &&
                       (providerOrder_1.SendByEMail == providerOrder_2.SendByEMail) &&
                       (providerOrder_1.Print_ProviderOrder == providerOrder_2.Print_ProviderOrder) &&
                       (providerOrder_1.SendByEMail_ProviderOrder == providerOrder_2.SendByEMail_ProviderOrder) &&
                       (providerOrder_1.TotalAmount == providerOrder_2.TotalAmount) &&
                       (providerOrder_1.Historic == providerOrder_2.Historic) &&
                       (providerOrder_1.Select_Bill == providerOrder_2.Select_Bill) &&
                       (providerOrder_1.Expiration == providerOrder_2.Expiration) &&
                       (providerOrder_1.Daily == providerOrder_2.Daily)&&
                       (providerOrder_1.PrevisioLliurament == providerOrder_2.PrevisioLliurament) &&
                       (providerOrder_1.PrevisioLliuramentData == providerOrder_2.PrevisioLliuramentData);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="provider_1">Primera instáncia a comparar.</param>
        /// <param name="provider_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(ProviderOrdersView provider_1, ProviderOrdersView provider_2)
        {
            return !(provider_1 == provider_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(ProviderOrder_Id, _Provider_Id).GetHashCode());
        }

        #endregion

        #region Public Functions

        public string GetStringIBANFromProviderOrder()
        {
            return string.Format("{0} {1} {2} {3} {4}",
                                 DataBank_IBAN_CountryCode,
                                 DataBank_IBAN_BankCode,
                                 DataBank_IBAN_OfficeCode,
                                 DataBank_IBAN_CheckDigits,
                                 DataBank_IBAN_AccountNumber);
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="providerOrderId"></param>
        /// <returns></returns>
        public static ProviderOrdersView CreateCopy( int providerOrderId )
        {
            ProviderOrder provider_order = HispaniaDataAccess.Instance.CreateCopyProviderOrderById( providerOrderId );

            ProviderOrdersView result = new ProviderOrdersView( provider_order );            

            return result;
        }
    }
}
