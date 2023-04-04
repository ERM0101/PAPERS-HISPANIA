#region Libraries used for the class

using HispaniaComptabilitat.Data;
using System;
using System.Collections.Generic;
using HispaniaCompData = HispaniaComptabilitat.Data;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Camps del Tipus
    /// </summary>
    public enum ProvidersAttributes
    {
        ProviderNumber,
        Alias,
        Name,
        NIF,
        Address,
        Phone,
        MobilePhone,
        Fax,
        EMail,
        AcountAcounting,
        TransferPercent,
        PromptPaymentDiscount,
        AdditionalDiscount,
        Comment,
        PostalCode,
        Data_Agent,
        BillingData_Agent,
        BillingData_IVAType,
        BillingData_SendType,
        None,
    }

    /// <summary>
    /// Class that Store the information of a Provider.
    /// </summary>
    public class ProvidersView : IMenuView
    {
        #region Fields for Filter

        /// <summary>
        /// Store the list of fields that compose the Provider class
        /// </summary>
        private static Dictionary<string, string> m_Fields = null;

        /// <summary>
        /// Get the list of fields that compose the Provider class
        /// </summary>
        public static Dictionary<string, string> Fields
        {
            get
            {
                if (m_Fields == null)
                {
                    m_Fields = new Dictionary<string, string>
                    {
                        { "Numero de Proveïdor", "Provider_Number" },
                        { "Nom", "Name" },
                        { "Alias", "Alias" },
                        { "DNI/CIF", "NIF" },
                        { "Adreça", "Address" },
                        { "Còdi Postal", "PostalCode_Str" },
                        { "Població", "City" },
                        { "Telèfon", "Phone" },
                        { "Mòbil", "MobilePhone" },
                        { "E-mail", "EMail" },
                        { "Representant", "Agent_Name" },
                        { "Telèfon Representant", "Agent_Phone" }
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
                return GlobalViewModel.GetStringFromIntIdValue(Provider_Id);
            }
        }

        #endregion

        #region Main Fields

        public int Provider_Id { get; set; }

        public int Provider_Number { get; set; }

        public string Alias { get; set; }

        public string Name { get; set; }

        public string NIF { get; set; }

        public string Address { get; set; }

        public string Phone { get; set; }

        public string MobilePhone { get; set; }

        public string Fax { get; set; }

        public string EMail { get; set; }

        public string AcountAcounting { get; set; }

        public decimal TransferPercent { get; set; }

        public decimal PromptPaymentDiscount { get; set; }

        public decimal AdditionalDiscount { get; set; }

        public string Comment { get; set; }

        public bool Canceled { get; set; }

        #endregion

        #region ForeignKey Properties

        #region Postal Codes

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
                return (PostalCode == null) ? string.Empty : PostalCode.Postal_Code;
            }
        }

        public string City
        {
            get
            {
                return (PostalCode == null) ? string.Empty : PostalCode.City;
            }
        }

        #endregion

        #region Agents

        private int? _Data_Agent_Id;

        private AgentsView _Data_Agent;

        public AgentsView Data_Agent
        {
            get
            {
                if ((_Data_Agent == null) && (_Data_Agent_Id != GlobalViewModel.IntIdInitValue) && 
                    (_Data_Agent_Id != null))
                {
                    _Data_Agent = new AgentsView(GlobalViewModel.Instance.HispaniaViewModel.GetAgent((int)_Data_Agent_Id));
                }
                return (_Data_Agent);
            }
            set
            {
                if (value != null)
                {
                    _Data_Agent = new AgentsView(value);
                    if (_Data_Agent == null) _Data_Agent_Id = GlobalViewModel.IntIdInitValue;
                    else _Data_Agent_Id = _Data_Agent.Agent_Id;
                }
                else
                {
                    _Data_Agent = null;
                    _Data_Agent_Id = GlobalViewModel.IntIdInitValue;
                }
            }
        }

        public string Agent_Name
        {
            get
            {
                return ((Data_Agent == null) ? string.Empty : _Data_Agent.Name);
            }
        }

        public string Agent_Phone
        {
            get
            {
                return ((Data_Agent == null) ? string.Empty : _Data_Agent.Phone);
            }
        }

        public string Agent_Fax
        {
            get
            {
                return ((Data_Agent == null) ? string.Empty : _Data_Agent.Fax);
            }
        }

        #endregion

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

        #region Company

        public string Company_Name { get; set; }
        public string Company_Address { get; set; }
        public string Company_Phone_1 { get; set; }
        public string Company_Phone_2 { get; set; }
        public string Company_Fax { get; set; }
        public string Company_MobilePhone { get; set; }
        public string Company_EMail { get; set; }
        public string Company_EMail2 { get; set; }
        public string Company_EMail3 { get; set; }
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

        #region Postal Codes

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

        public string Company_Province_Str
        {
            get
            {
                return (Company_PostalCode == null) ? string.Empty : Company_PostalCode.Province;
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

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public ProvidersView()
        {
            Provider_Id = GlobalViewModel.IntIdInitValue;
            Provider_Number = 0;
            Name = string.Empty;
            Address = string.Empty;
            Phone = string.Empty;
            NIF = string.Empty;
            MobilePhone = string.Empty;
            EMail = string.Empty;
            Alias = string.Empty;
            Fax = string.Empty;
            AcountAcounting = string.Empty;
            TransferPercent = GlobalViewModel.DecimalInitValue;
            PromptPaymentDiscount = GlobalViewModel.DecimalInitValue;
            AdditionalDiscount = GlobalViewModel.DecimalInitValue;
            Comment = string.Empty;
            Canceled = false;
            PostalCode = null;
            Data_Agent = null;
            BillingData_EarlyPaymentDiscount = GlobalViewModel.DecimalInitValue;
            SeveralData_Acum_1 = GlobalViewModel.DecimalInitValue;
            SeveralData_Acum_2 = GlobalViewModel.DecimalInitValue;
            SeveralData_Acum_3 = GlobalViewModel.DecimalInitValue;
            SeveralData_Acum_4 = GlobalViewModel.DecimalInitValue;
            SeveralData_Acum_5 = GlobalViewModel.DecimalInitValue;
            SeveralData_Acum_6 = GlobalViewModel.DecimalInitValue;
            SeveralData_Acum_7 = GlobalViewModel.DecimalInitValue;
            SeveralData_Acum_8 = GlobalViewModel.DecimalInitValue;
            SeveralData_Acum_9 = GlobalViewModel.DecimalInitValue;
            SeveralData_Acum_10 = GlobalViewModel.DecimalInitValue;
            SeveralData_Acum_11 = GlobalViewModel.DecimalInitValue;
            SeveralData_Acum_12 = GlobalViewModel.DecimalInitValue;
            BillingData_IVAType = null;           
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal ProvidersView(HispaniaCompData.Provider provider)
        {
            Provider_Id = provider.Provider_Id;
            Provider_Number = provider.Provider_Number;
            Name = provider.Name;
            Address = provider.Address;
            Phone = provider.Phone_1;
            NIF = provider.NIF;
            MobilePhone = provider.MobilePhone;
            EMail = provider.EMail;
            Alias = provider.Alias;
            Fax = provider.Fax;
            AcountAcounting = provider.AcountAcounting;
            TransferPercent = GlobalViewModel.GetDecimalValue(GlobalViewModel.GetStringFromDecimalValue(provider.TransferPercent, DecimalType.Percent));
            PromptPaymentDiscount = GlobalViewModel.GetDecimalValue(GlobalViewModel.GetStringFromDecimalValue(provider.PromptPaymentDiscount, DecimalType.Percent));
            AdditionalDiscount = GlobalViewModel.GetDecimalValue(GlobalViewModel.GetStringFromDecimalValue(provider.AdditionalDiscount, DecimalType.Percent));
            Comment = provider.Comment;
            Canceled = provider.Canceled;
            _PostalCode_Id = GlobalViewModel.GetIntFromIntIdValue(provider.PostalCode_Id);
            _Data_Agent_Id = GlobalViewModel.GetIntFromIntIdValue(provider.Agent_Id);
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public ProvidersView(ProvidersView provider)
        {
            Provider_Id = provider.Provider_Id;
            Provider_Number = provider.Provider_Number;
            Name = provider.Name;
            Address = provider.Address;
            Phone = provider.Phone;
            NIF = provider.NIF;
            Data_Agent = provider.Data_Agent;
            MobilePhone = provider.MobilePhone;
            EMail = provider.EMail;
            Alias = provider.Alias;
            Fax = provider.Fax;
            AcountAcounting = provider.AcountAcounting;
            TransferPercent = provider.TransferPercent;
            PromptPaymentDiscount = provider.PromptPaymentDiscount;
            AdditionalDiscount = provider.AdditionalDiscount;
            Comment = provider.Comment;
            Canceled = provider.Canceled;
            PostalCode = provider.PostalCode;
            Data_Agent = provider.Data_Agent;
        }

        #endregion

        #region GetProvider

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.Provider GetProvider()
        {
            HispaniaCompData.Provider provider = new HispaniaCompData.Provider()
            {
                Provider_Id = Provider_Id,
                Provider_Number = Provider_Number,
                Name = Name,
                Address = Address,
                Phone_1 = Phone,
                NIF = NIF,
                MobilePhone = MobilePhone,
                EMail = EMail,
                Alias = Alias,
                Fax = Fax,
                AcountAcounting = AcountAcounting,
                TransferPercent = TransferPercent,
                PromptPaymentDiscount = PromptPaymentDiscount,
                AdditionalDiscount = AdditionalDiscount,
                Comment = Comment,
                Canceled = Canceled,
                PostalCode_Id = _PostalCode_Id,
                Agent_Id = _Data_Agent_Id
            };
            return (provider);
        }

        #endregion

        #region Validate

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        /// <param name="sMsgError">Message associated the error detected.</param>
        public void Validate(out ProvidersAttributes ErrorField)
        {
            ErrorField = ProvidersAttributes.None;
            if ((Provider_Number < 0) || (!GlobalViewModel.IsNumeric(Provider_Number.ToString())))
            {
                ErrorField = ProvidersAttributes.ProviderNumber;
                throw new FormatException("Error, valor no definit o format incorrecte del Numero de Proveïdor");
            }
            if (!GlobalViewModel.IsName(Alias))
            {
                ErrorField = ProvidersAttributes.Alias;
                throw new FormatException("Error, valor no definit o format incorrecte de l'Alias");
            }
            if (!GlobalViewModel.IsName(Name))
            {
                ErrorField = ProvidersAttributes.Name;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
            if (!GlobalViewModel.IsEmptyOrAddress(Address))
            {
                ErrorField = ProvidersAttributes.Address;
                throw new FormatException("Error, format incorrecte de l'Adreça del Proveïdor");
            }
            if (!GlobalViewModel.IsEmptyOrPhoneNumber(Phone))
            {
                ErrorField = ProvidersAttributes.Phone;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrPhoneNumberError);
            }
            if (!GlobalViewModel.IsEmptyOrPhoneNumber(Fax))
            {
                ErrorField = ProvidersAttributes.Fax;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrPhoneNumberError);
            }
            if (!GlobalViewModel.IsEmptyOrMobilePhoneNumber(MobilePhone))
            {
                ErrorField = ProvidersAttributes.MobilePhone;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrMobilePhoneNumberError);
            }
            if (!GlobalViewModel.IsEmptyOrEmail(EMail))
            {
                ErrorField = ProvidersAttributes.EMail;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrEmailError);
            }
            if (!GlobalViewModel.IsEmptyOrCIF(NIF))
            {
                ErrorField = ProvidersAttributes.NIF;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrCIFError);
            }
            if (!GlobalViewModel.IsEmptyOrUint(AcountAcounting, "Compte Comptable", out string ErrMsg))
            {
                ErrorField = ProvidersAttributes.AcountAcounting;
                throw new FormatException(ErrMsg);
            }
            if (GlobalViewModel.GetUintValue(AcountAcounting) > 9999)
            {
                ErrorField = ProvidersAttributes.AcountAcounting;
                throw new FormatException("Error, el compte comptable no pot ser mai de més de 4 dígits.");
            }
            if (!GlobalViewModel.IsEmptyOrPercent(TransferPercent, "Percentatge de Transferència", out ErrMsg))
            {
                ErrorField = ProvidersAttributes.TransferPercent;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrPercent(PromptPaymentDiscount, "Descompte Pagament Immediat", out ErrMsg))
            {
                ErrorField = ProvidersAttributes.PromptPaymentDiscount;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrPercent(AdditionalDiscount, "Descompte Addicional", out ErrMsg))
            {
                ErrorField = ProvidersAttributes.AdditionalDiscount;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrComment(Comment))
            {
                ErrorField = ProvidersAttributes.Comment;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
            if (PostalCode == null)
            {
                ErrorField = ProvidersAttributes.PostalCode;
                throw new FormatException("Error, manca seleccionar el Codi Postal.");
            }
            if (Data_Agent == null)
            {
                ErrorField = ProvidersAttributes.Data_Agent;
                throw new FormatException("Error, manca seleccionar l'Agent.");
            }
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(ProvidersView Data)
        {
            Provider_Number = Data.Provider_Number;
            Alias = Data.Alias;
            Name = Data.Name;
            Address = Data.Address;
            Phone = Data.Phone;
            Fax = Data.Fax;
            MobilePhone = Data.MobilePhone;
            EMail = Data.EMail;
            NIF = Data.NIF;
            AcountAcounting = Data.AcountAcounting;
            TransferPercent = Data.TransferPercent;
            AdditionalDiscount = Data.AdditionalDiscount;
            PromptPaymentDiscount = Data.PromptPaymentDiscount;
            Data_Agent = Data.Data_Agent;
            PostalCode = Data.PostalCode;
            Comment = Data.Comment;
            Canceled = Data.Canceled;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(ProvidersView Data, ProvidersAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case ProvidersAttributes.ProviderNumber:
                     Provider_Number = Data.Provider_Number;
                     break;
                case ProvidersAttributes.Alias:
                     Alias = Data.Alias;
                     break;
                case ProvidersAttributes.Name:
                     Name = Data.Name;
                     break;
                case ProvidersAttributes.Address:
                     Address = Data.Address;
                     break;
                case ProvidersAttributes.Phone:
                     Phone = Data.Phone;
                     break;
                case ProvidersAttributes.Fax:
                     Fax = Data.Fax;
                     break;
                case ProvidersAttributes.MobilePhone:
                     MobilePhone = Data.MobilePhone;
                     break;
                case ProvidersAttributes.EMail:
                     EMail = Data.EMail;
                     break;
                case ProvidersAttributes.NIF:
                     NIF = Data.NIF;
                     break;
                case ProvidersAttributes.AcountAcounting:
                     AcountAcounting = Data.AcountAcounting;
                     break;
                case ProvidersAttributes.TransferPercent:
                     TransferPercent = Data.TransferPercent;
                     break;
                case ProvidersAttributes.AdditionalDiscount:
                     AdditionalDiscount = Data.AdditionalDiscount;
                     break;
                case ProvidersAttributes.PromptPaymentDiscount:
                     PromptPaymentDiscount = Data.PromptPaymentDiscount;
                     break;
                case ProvidersAttributes.Data_Agent:
                     Data_Agent = Data.Data_Agent;
                     break;
                case ProvidersAttributes.PostalCode:
                     PostalCode = Data.PostalCode;
                     break;
                case ProvidersAttributes.Comment:
                     Comment = Data.Comment;
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
                ProvidersView provider = obj as ProvidersView;
                if ((Object)provider == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor. 
                return (Provider_Id == provider.Provider_Id) && (Provider_Number == provider.Provider_Number) && (Name == provider.Name) && 
                       (Address == provider.Address) && (_PostalCode_Id == provider._PostalCode_Id) && (Phone == provider.Phone) && (NIF == provider.NIF) &&
                       (MobilePhone == provider.MobilePhone) && (EMail == provider.EMail) && (_Data_Agent_Id == provider._Data_Agent_Id) && 
                       (Alias == provider.Alias) && (Fax == provider.Fax) && (AcountAcounting == provider.AcountAcounting) &&
                       (TransferPercent == provider.TransferPercent) && (PromptPaymentDiscount == provider.PromptPaymentDiscount) &&
                       (AdditionalDiscount == provider.AdditionalDiscount) && (Comment == provider.Comment) && (Canceled == provider.Canceled);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(ProvidersView provider)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)provider == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Provider_Id == provider.Provider_Id) && (Provider_Number == provider.Provider_Number) && (Name == provider.Name) && 
                       (Address == provider.Address) && (_PostalCode_Id == provider._PostalCode_Id) && (Phone == provider.Phone) && (NIF == provider.NIF) &&
                       (MobilePhone == provider.MobilePhone) && (EMail == provider.EMail) && (_Data_Agent_Id == provider._Data_Agent_Id) && 
                       (Alias == provider.Alias) && (Fax == provider.Fax) && (AcountAcounting == provider.AcountAcounting) &&
                       (TransferPercent == provider.TransferPercent) && (PromptPaymentDiscount == provider.PromptPaymentDiscount) &&
                       (AdditionalDiscount == provider.AdditionalDiscount) && (Comment == provider.Comment) && (Canceled == provider.Canceled);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="provider_1">Primera instáncia a comparar.</param>
        /// <param name="provider_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(ProvidersView provider_1, ProvidersView provider_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(provider_1, provider_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)provider_1 == null) || ((object)provider_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (provider_1.Provider_Id == provider_2.Provider_Id) && (provider_1.Provider_Number == provider_2.Provider_Number) &&
                       (provider_1.Name == provider_2.Name) && (provider_1.Address == provider_2.Address) && 
                       (provider_1._PostalCode_Id == provider_2._PostalCode_Id) && 
                       (provider_1.Phone == provider_2.Phone) && (provider_1.NIF == provider_2.NIF) &&
                       (provider_1._Data_Agent_Id == provider_2._Data_Agent_Id) && (provider_1.MobilePhone == provider_2.MobilePhone) &&
                       (provider_1.EMail == provider_2.EMail) && (provider_1.Alias == provider_2.Alias) &&
                       (provider_1.Fax == provider_2.Fax) && (provider_1.AcountAcounting == provider_2.AcountAcounting) &&
                       (provider_1.TransferPercent == provider_2.TransferPercent) && 
                       (provider_1.PromptPaymentDiscount == provider_2.PromptPaymentDiscount) &&
                       (provider_1.AdditionalDiscount == provider_2.AdditionalDiscount) && 
                       (provider_1.Comment == provider_2.Comment) && (provider_1.Canceled == provider_2.Canceled);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="provider_1">Primera instáncia a comparar.</param>
        /// <param name="provider_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(ProvidersView provider_1, ProvidersView provider_2)
        {
            return !(provider_1 == provider_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(Provider_Id, Name).GetHashCode());
        }

        #endregion
    }
}
