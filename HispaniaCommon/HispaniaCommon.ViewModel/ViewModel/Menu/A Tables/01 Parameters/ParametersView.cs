#region Librerias usadas por la clase

using HispaniaCompData = HispaniaComptabilitat.Data;
using System;
using System.Collections.Generic;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Camps del Tipus
    /// </summary>
    public enum ParametersAttributes
    { 
         Company_CIF,
         Company_Name,
         Company_Address,
         Company_Phone_1, 
         Company_Phone_2, 
         Company_Fax, 
         Company_MobilePhone,
         Company_EMail,
         Company_DaysVtoRisk,
         Provider_NumProviderOrder,
         Customer_NumOrder, 
         Customer_NumDeliveryNote, 
         Customer_NumBill,
         DataBank_Bank,
         DataBank_BankAddress,
         DataBank_IBAN_CountryCode,
         DataBank_IBAN_BankCode,
         DataBank_IBAN_OfficeCode,
         DataBank_IBAN_CheckDigits,
         DataBank_IBAN_AccountNumber,
         None,
    }

    /// <summary>
    /// Class that Store the information of a Parameters.
    /// </summary>
    public class ParametersView : IMenuView
    {
        #region Fields for Filter

        /// <summary>
        /// Store the list of fields that compose the Parameters class
        /// </summary>
        private static Dictionary<string, string> m_Fields = null;

        /// <summary>
        /// Get the list of fields that compose the Parameters class
        /// </summary>
        public static Dictionary<string, string> Fields
        {
            get
            {
                if (m_Fields == null)
                {
                    m_Fields = new Dictionary<string, string>
                    {
                        { "CIF", "Company_CIF" },
                        { "Empresa", "Company_Name" },
                        { "Adreça", "Company_Address" },
                        { "CP", "Company_PostalCode" },
                        { "Població", "Company_City" },
                        { "Telèfon 1", "Company_Phone_1" },
                        { "Telèfon 2", "Company_Phone_2" },
                        { "Mòbil", "Company_MobilePhone" },
                        { "Fax", "Company_Fax" },
                        { "E-mail", "Company_EMail" },
                        { "Dies Vto. Risc", "Company_DaysVtoRisk" },
                        { "Numero de Comanda a Proveïdor", "Provider_NumProviderOrder" },
                        { "Numero de Comanda", "Customer_NumOrder" },
                        { "Numero d'Albarà", "Customer_NumDeliveryNote" },
                        { "Numero de Factura", "Customer_NumBill" }
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
                return GlobalViewModel.GetStringFromIntIdValue(Parameter_Id);
            }
        }

        #endregion

        #region Main Fields

        public int Parameter_Id { get; set; }

        public string Company_CIF { get; set; }

        public string Company_Name { get; set; }

        public string Company_Address { get; set; }

        public string Company_Phone_1 { get; set; }

        public string Company_Phone_2 { get; set; }

        public string Company_MobilePhone { get; set; }

        public string Company_Fax { get; set; }

        public string Company_EMail { get; set; }

        public decimal Company_DaysVtoRisk { get; set; }

        #endregion

        #region Providers

        public decimal Provider_NumProviderOrder { get; set; }

        #endregion

        #region Customers

        public decimal Customer_NumOrder { get; set; }

        public decimal Customer_NumDeliveryNote { get; set; }

        public decimal Customer_NumBill { get; set; }

        #endregion

        #region Bank Data (Bancarios)

        public string DataBank_Bank { get; set; }
        public string DataBank_BankAddress { get; set; }
        public string DataBank_IBAN_CountryCode { get; set; }
        public string DataBank_IBAN_BankCode { get; set; }
        public string DataBank_IBAN_OfficeCode { get; set; }
        public string DataBank_IBAN_CheckDigits { get; set; }
        public string DataBank_IBAN_AccountNumber { get; set; }

        #endregion

        #region ForeignKey Properties

        #region PostalCode

        private int? Company_PostalCode_Id { get; set; }

        private PostalCodesView _Company_PostalCode;

        public PostalCodesView Company_PostalCode
        {
            get
            {
                if ((_Company_PostalCode == null) && (Company_PostalCode_Id != -1) && (Company_PostalCode_Id != null))
                {
                    _Company_PostalCode = new PostalCodesView(GlobalViewModel.Instance.HispaniaViewModel.GetPostalCode((int)Company_PostalCode_Id));
                }
                return (_Company_PostalCode);
            }
            set
            {
                if (value != null)
                {
                    _Company_PostalCode = new PostalCodesView(value);
                    if (_Company_PostalCode == null) Company_PostalCode_Id = -1;
                    else Company_PostalCode_Id = _Company_PostalCode.PostalCode_Id;
                }
                else _Company_PostalCode = null;
            }
        }

        #endregion

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public ParametersView()
        {
            Parameter_Id = -1;
            Company_CIF = string.Empty;
            Company_Name = string.Empty;
            Company_Address = string.Empty;
            Company_Phone_1 = string.Empty;
            Company_Phone_2 = string.Empty;
            Company_Fax = string.Empty;
            Company_MobilePhone = string.Empty;
            Company_EMail = string.Empty;
            Company_DaysVtoRisk = 0;   
            Provider_NumProviderOrder = 0;
            Customer_NumOrder = 0;
            Customer_NumDeliveryNote = 0;
            Customer_NumBill = 0;
            Company_PostalCode = null;
            DataBank_Bank = string.Empty;
            DataBank_BankAddress = string.Empty;
            DataBank_IBAN_CountryCode = string.Empty;
            DataBank_IBAN_BankCode = string.Empty;
            DataBank_IBAN_OfficeCode = string.Empty;
            DataBank_IBAN_CheckDigits = string.Empty;
            DataBank_IBAN_AccountNumber = string.Empty;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal ParametersView(HispaniaCompData.Parameter Parameter)
        {
            Parameter_Id = Parameter.Parameter_Id;
            Company_CIF = Parameter.Company_CIF;
            Company_Name = Parameter.Company_Name;
            Company_Address = Parameter.Company_Address;
            Company_Phone_1 = Parameter.Company_Phone_1;
            Company_Phone_2 = Parameter.Company_Phone_2;
            Company_Fax = Parameter.Company_Fax;
            Company_MobilePhone = Parameter.Company_MobilePhone;
            Company_EMail = Parameter.Company_EMail;
            Company_DaysVtoRisk = GlobalViewModel.GetDecimalValue(Parameter.Company_DaysVtoRisk);
            Provider_NumProviderOrder = GlobalViewModel.GetDecimalValue(Parameter.Provider_NumProviderOrder);
            Customer_NumOrder = GlobalViewModel.GetDecimalValue(Parameter.Customer_NumOrder);
            Customer_NumDeliveryNote = GlobalViewModel.GetDecimalValue(Parameter.Customer_NumDeliveryNote);
            Customer_NumBill = GlobalViewModel.GetDecimalValue(Parameter.Customer_NumBill);
            Company_PostalCode_Id = Parameter.Company_PostalCode_Id;
            DataBank_Bank = Parameter.DataBank_Bank;
            DataBank_BankAddress = Parameter.DataBank_BankAddress;
            DataBank_IBAN_CountryCode = Parameter.DataBank_IBAN_CountryCode;
            DataBank_IBAN_BankCode = Parameter.DataBank_IBAN_BankCode;
            DataBank_IBAN_OfficeCode = Parameter.DataBank_IBAN_OfficeCode;
            DataBank_IBAN_CheckDigits = Parameter.DataBank_IBAN_CheckDigits;
            DataBank_IBAN_AccountNumber = Parameter.DataBank_IBAN_AccountNumber;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public ParametersView(ParametersView Parameters)
        {
            Parameter_Id = Parameters.Parameter_Id;
            Company_CIF = Parameters.Company_CIF;
            Company_Name = Parameters.Company_Name;
            Company_Address = Parameters.Company_Address;
            Company_Phone_1 = Parameters.Company_Phone_1;
            Company_Phone_2 = Parameters.Company_Phone_2;
            Company_Fax = Parameters.Company_Fax;
            Company_MobilePhone = Parameters.Company_MobilePhone;
            Company_EMail = Parameters.Company_EMail;
            Company_DaysVtoRisk = Parameters.Company_DaysVtoRisk;
            Provider_NumProviderOrder = Parameters.Provider_NumProviderOrder;
            Customer_NumOrder = Parameters.Customer_NumOrder;
            Customer_NumDeliveryNote = Parameters.Customer_NumDeliveryNote;
            Customer_NumBill = Parameters.Customer_NumBill;
            Company_PostalCode = Parameters.Company_PostalCode;
            DataBank_Bank = Parameters.DataBank_Bank;
            DataBank_BankAddress = Parameters.DataBank_BankAddress;
            DataBank_IBAN_CountryCode = Parameters.DataBank_IBAN_CountryCode;
            DataBank_IBAN_BankCode = Parameters.DataBank_IBAN_BankCode;
            DataBank_IBAN_OfficeCode = Parameters.DataBank_IBAN_OfficeCode;
            DataBank_IBAN_CheckDigits = Parameters.DataBank_IBAN_CheckDigits;
            DataBank_IBAN_AccountNumber = Parameters.DataBank_IBAN_AccountNumber;
        }

        #endregion

        #region GetParameter

        internal HispaniaCompData.Parameter GetParameter()
        {
            HispaniaCompData.Parameter Parameter = new HispaniaCompData.Parameter()
            {
                Parameter_Id = Parameter_Id,
                Company_CIF = Company_CIF,
                Company_Name = Company_Name,
                Company_Address = Company_Address,
                Company_Phone_1 = Company_Phone_1,
                Company_Phone_2 = Company_Phone_2,
                Company_Fax = Company_Fax,
                Company_MobilePhone = Company_MobilePhone,
                Company_EMail = Company_EMail,
                Company_DaysVtoRisk = Company_DaysVtoRisk,
                Provider_NumProviderOrder = Provider_NumProviderOrder,
                Customer_NumOrder = Customer_NumOrder,
                Customer_NumDeliveryNote = Customer_NumDeliveryNote,
                Customer_NumBill = Customer_NumBill,
                Company_PostalCode_Id = Company_PostalCode_Id,
                DataBank_Bank = DataBank_Bank,
                DataBank_BankAddress = DataBank_BankAddress,
                DataBank_IBAN_CountryCode = DataBank_IBAN_CountryCode,
                DataBank_IBAN_BankCode = DataBank_IBAN_BankCode,
                DataBank_IBAN_OfficeCode = DataBank_IBAN_OfficeCode,
                DataBank_IBAN_CheckDigits = DataBank_IBAN_CheckDigits,
                DataBank_IBAN_AccountNumber = DataBank_IBAN_AccountNumber,
            };
            return (Parameter);
        }

        #endregion

        #region Validate

        /// <summary>
        /// Validate the data contained in the instance of the class.
        /// </summary>
        public void Validate(out ParametersAttributes ErrorField)
        {
            ErrorField = ParametersAttributes.None;
            if (!GlobalViewModel.IsCIF(Company_CIF))
            { 
                ErrorField = ParametersAttributes.Company_CIF;
                throw new FormatException(GlobalViewModel.ValidationCIFError);
            }
            if (!GlobalViewModel.IsName(Company_Name))
            {
                ErrorField = ParametersAttributes.Company_Name;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
            if (!GlobalViewModel.IsAddress(Company_Address))
            {
                ErrorField = ParametersAttributes.Company_Address;
                throw new FormatException(GlobalViewModel.ValidationAddressError);
            }
            if (!GlobalViewModel.IsEmptyOrPhoneNumber(Company_Phone_1))
            {
                ErrorField = ParametersAttributes.Company_Phone_1;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrPhoneNumberError);
            }
            if (!GlobalViewModel.IsEmptyOrPhoneNumber(Company_Phone_2))
            {
                ErrorField = ParametersAttributes.Company_Phone_2;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrPhoneNumberError);
            }
            if (!GlobalViewModel.IsEmptyOrPhoneNumber(Company_Fax))
            {
                ErrorField = ParametersAttributes.Company_Fax;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrPhoneNumberError);
            }
            if (!GlobalViewModel.IsEmptyOrMobilePhoneNumber(Company_MobilePhone))
            {
                ErrorField = ParametersAttributes.Company_MobilePhone;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrMobilePhoneNumberError);
            }
            if (!GlobalViewModel.IsEmptyOrEmail(Company_EMail))
            {
                ErrorField = ParametersAttributes.Company_EMail;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrEmailError); 
            }
            if (!GlobalViewModel.IsEmptyOrShortDecimal(Company_DaysVtoRisk, "DIES DE RISC DE VENCIMENT", out string msgError))
            {
                ErrorField = ParametersAttributes.Company_DaysVtoRisk;
                throw new FormatException(msgError); 
            }
            if (!GlobalViewModel.IsEmptyOrLongDecimal(Provider_NumProviderOrder, "PROVEÏDORS -> NUMERO COMANDA DE PROVEÏDOR", out msgError))
            {
                ErrorField = ParametersAttributes.Provider_NumProviderOrder;
                throw new FormatException(msgError); 
            }
            if (!GlobalViewModel.IsEmptyOrLongDecimal(Customer_NumOrder, "CLIENTS -> NUMERO COMANDA", out msgError))
            {
                ErrorField = ParametersAttributes.Customer_NumOrder;
                throw new FormatException(msgError);
            }
            if (!GlobalViewModel.IsEmptyOrLongDecimal(Customer_NumDeliveryNote, "CLIENTS -> NUMERO d''ALBARÀ", out msgError))
            {
                ErrorField = ParametersAttributes.Customer_NumDeliveryNote;
                throw new FormatException(msgError);
            }
            if (!GlobalViewModel.IsEmptyOrLongDecimal(Customer_NumBill, "CLIENTS -> NUMERO FACTURA", out msgError))
            {
                ErrorField = ParametersAttributes.Customer_NumBill;
                throw new FormatException(msgError);
            }
            if (!GlobalViewModel.IsEmptyOrName(DataBank_Bank))
            {
                ErrorField = ParametersAttributes.DataBank_Bank;
                throw new FormatException("Error, format incorrecte del NOM DEL BANC");
            }
            if (!GlobalViewModel.IsEmptyOrAddress(DataBank_BankAddress))
            {
                ErrorField = ParametersAttributes.DataBank_BankAddress;
                throw new FormatException("Error, format incorrecte de l'ADREÇA DEL BANC");
            }
            string CCC = string.Format("{0}{1}{2}{3}", DataBank_IBAN_BankCode, DataBank_IBAN_OfficeCode,
                                                       DataBank_IBAN_CheckDigits, DataBank_IBAN_AccountNumber);
            if (!GlobalViewModel.IsEmptyOrIBAN_CountryCode(DataBank_IBAN_CountryCode, CCC, out string ErrMsg))
            {
                ErrorField = ParametersAttributes.DataBank_IBAN_CountryCode;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrIBAN_BankCode(DataBank_IBAN_BankCode))
            {
                ErrorField = ParametersAttributes.DataBank_IBAN_BankCode;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrIBAN_BankCodeError);
            }
            if (!GlobalViewModel.IsEmptyOrIBAN_OfficeCode(DataBank_IBAN_OfficeCode))
            {
                ErrorField = ParametersAttributes.DataBank_IBAN_OfficeCode;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrIBAN_OfficeCodeError);
            }
            if (!GlobalViewModel.IsEmptyOrIBAN_CheckDigits(DataBank_IBAN_CheckDigits))
            {
                ErrorField = ParametersAttributes.DataBank_IBAN_CheckDigits;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrIBAN_CheckDigitsError);
            }
            if (!GlobalViewModel.IsEmptyOrIBAN_AccountNumber(DataBank_IBAN_AccountNumber))
            {
                ErrorField = ParametersAttributes.DataBank_IBAN_AccountNumber;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrIBAN_AccountNumberError);
            }
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(ParametersView Data, ParametersAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case ParametersAttributes.Company_CIF:
                     Company_CIF = Data.Company_CIF;
                     break;
                case ParametersAttributes.Company_Name:
                     Company_Name = Data.Company_Name;
                     break;
                case ParametersAttributes.Company_Address:
                     Company_Address = Data.Company_Address;
                     break;
                case ParametersAttributes.Company_Phone_1:
                     Company_Phone_1 = Data.Company_Phone_1;
                     break;
                case ParametersAttributes.Company_Phone_2:
                     Company_Phone_2 = Data.Company_Phone_2;
                     break;
                case ParametersAttributes.Company_Fax:
                     Company_Fax = Data.Company_Fax;
                     break;
                case ParametersAttributes.Company_MobilePhone:
                     Company_MobilePhone = Data.Company_MobilePhone;
                     break;
                case ParametersAttributes.Company_EMail:
                     Company_EMail = Data.Company_EMail;
                     break;
                case ParametersAttributes.Company_DaysVtoRisk:
                     Company_DaysVtoRisk = Data.Company_DaysVtoRisk;
                     break;
                case ParametersAttributes.Provider_NumProviderOrder:
                     Provider_NumProviderOrder = Data.Provider_NumProviderOrder;
                     break;
                case ParametersAttributes.Customer_NumOrder:
                     Customer_NumOrder = Data.Customer_NumOrder;
                     break;
                case ParametersAttributes.Customer_NumDeliveryNote:
                     Customer_NumDeliveryNote = Data.Customer_NumDeliveryNote;
                     break;
                case ParametersAttributes.Customer_NumBill:
                     Customer_NumBill = Data.Customer_NumBill;
                     break;
                case ParametersAttributes.DataBank_Bank:
                     DataBank_Bank = Data.DataBank_Bank;
                     break;
                case ParametersAttributes.DataBank_BankAddress:
                     DataBank_BankAddress = Data.DataBank_BankAddress;
                     break;
                case ParametersAttributes.DataBank_IBAN_CountryCode:
                     DataBank_IBAN_CountryCode = Data.DataBank_IBAN_CountryCode;
                     break;
                case ParametersAttributes.DataBank_IBAN_BankCode:
                     DataBank_IBAN_BankCode = Data.DataBank_IBAN_BankCode;
                     break;
                case ParametersAttributes.DataBank_IBAN_OfficeCode:
                     DataBank_IBAN_OfficeCode = Data.DataBank_IBAN_OfficeCode;
                     break;
                case ParametersAttributes.DataBank_IBAN_CheckDigits:
                     DataBank_IBAN_CheckDigits = Data.DataBank_IBAN_CheckDigits;
                     break;
                case ParametersAttributes.DataBank_IBAN_AccountNumber:
                     DataBank_IBAN_AccountNumber = Data.DataBank_IBAN_AccountNumber;
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
                ParametersView Parameters = obj as ParametersView;
                if ((Object)Parameters == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Parameter_Id == Parameters.Parameter_Id)  && (Company_CIF == Parameters.Company_CIF)  &&
                       (Company_Name == Parameters.Company_Name) && (Company_Address == Parameters.Company_Address) &&
                       (Company_Phone_1 == Parameters.Company_Phone_1) && (Company_Phone_2 == Parameters.Company_Phone_2) &&
                       (Company_Fax == Parameters.Company_Fax) && (Company_MobilePhone == Parameters.Company_MobilePhone) &&
                       (Company_EMail == Parameters.Company_EMail) && (Company_DaysVtoRisk == Parameters.Company_DaysVtoRisk) &&
                       (Provider_NumProviderOrder == Parameters.Provider_NumProviderOrder) && (Customer_NumOrder == Parameters.Customer_NumOrder) && 
                       (Customer_NumDeliveryNote == Parameters.Customer_NumDeliveryNote) && (Customer_NumBill == Parameters.Customer_NumBill) &&
                       (Company_PostalCode == Parameters.Company_PostalCode) && 
                       (DataBank_Bank == Parameters.DataBank_Bank) &&
                       (DataBank_BankAddress == Parameters.DataBank_BankAddress) &&
                       (DataBank_IBAN_CountryCode == Parameters.DataBank_IBAN_CountryCode) &&
                       (DataBank_IBAN_BankCode == Parameters.DataBank_IBAN_BankCode) &&
                       (DataBank_IBAN_OfficeCode == Parameters.DataBank_IBAN_OfficeCode) &&
                       (DataBank_IBAN_CheckDigits == Parameters.DataBank_IBAN_CheckDigits) &&
                       (DataBank_IBAN_AccountNumber == Parameters.DataBank_IBAN_AccountNumber);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(ParametersView Parameters)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)Parameters == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Parameter_Id == Parameters.Parameter_Id) && (Company_CIF == Parameters.Company_CIF) &&
                       (Company_Name == Parameters.Company_Name) && (Company_Address == Parameters.Company_Address) &&
                       (Company_Phone_1 == Parameters.Company_Phone_1) && (Company_Phone_2 == Parameters.Company_Phone_2) &&
                       (Company_Fax == Parameters.Company_Fax) && (Company_MobilePhone == Parameters.Company_MobilePhone) &&
                       (Company_EMail == Parameters.Company_EMail) && (Company_DaysVtoRisk == Parameters.Company_DaysVtoRisk) &&
                       (Provider_NumProviderOrder == Parameters.Provider_NumProviderOrder) && (Customer_NumOrder == Parameters.Customer_NumOrder) && 
                       (Customer_NumDeliveryNote == Parameters.Customer_NumDeliveryNote) && (Customer_NumBill == Parameters.Customer_NumBill) &&
                       (Company_PostalCode == Parameters.Company_PostalCode) &&
                       (DataBank_Bank == Parameters.DataBank_Bank) &&
                       (DataBank_BankAddress == Parameters.DataBank_BankAddress) &&
                       (DataBank_IBAN_CountryCode == Parameters.DataBank_IBAN_CountryCode) &&
                       (DataBank_IBAN_BankCode == Parameters.DataBank_IBAN_BankCode) &&
                       (DataBank_IBAN_OfficeCode == Parameters.DataBank_IBAN_OfficeCode) &&
                       (DataBank_IBAN_CheckDigits == Parameters.DataBank_IBAN_CheckDigits) &&
                       (DataBank_IBAN_AccountNumber == Parameters.DataBank_IBAN_AccountNumber);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="Parameters_1">Primera instáncia a comparar.</param>
        /// <param name="Parameters_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(ParametersView Parameters_1, ParametersView Parameters_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(Parameters_1, Parameters_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)Parameters_1 == null) || ((object)Parameters_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Parameters_1.Parameter_Id == Parameters_2.Parameter_Id) && 
                       (Parameters_1.Company_CIF == Parameters_2.Company_CIF) &&
                       (Parameters_1.Company_Name == Parameters_2.Company_Name) && 
                       (Parameters_1.Company_Address == Parameters_2.Company_Address) &&
                       (Parameters_1.Company_Phone_1 == Parameters_2.Company_Phone_1) && 
                       (Parameters_1.Company_Phone_2 == Parameters_2.Company_Phone_2) &&
                       (Parameters_1.Company_Fax == Parameters_2.Company_Fax) && 
                       (Parameters_1.Company_MobilePhone == Parameters_2.Company_MobilePhone) &&
                       (Parameters_1.Company_EMail == Parameters_2.Company_EMail) && 
                       (Parameters_1.Company_DaysVtoRisk == Parameters_2.Company_DaysVtoRisk) &&
                       (Parameters_1.Provider_NumProviderOrder == Parameters_2.Provider_NumProviderOrder) && 
                       (Parameters_1.Customer_NumOrder == Parameters_2.Customer_NumOrder) && 
                       (Parameters_1.Customer_NumDeliveryNote == Parameters_2.Customer_NumDeliveryNote) && 
                       (Parameters_1.Customer_NumBill == Parameters_2.Customer_NumBill) &&
                       (Parameters_1.Company_PostalCode == Parameters_2.Company_PostalCode) &&
                       (Parameters_1.DataBank_Bank == Parameters_2.DataBank_Bank) &&
                       (Parameters_1.DataBank_BankAddress == Parameters_2.DataBank_BankAddress) &&
                       (Parameters_1.DataBank_IBAN_CountryCode == Parameters_2.DataBank_IBAN_CountryCode) &&
                       (Parameters_1.DataBank_IBAN_BankCode == Parameters_2.DataBank_IBAN_BankCode) &&
                       (Parameters_1.DataBank_IBAN_OfficeCode == Parameters_2.DataBank_IBAN_OfficeCode) &&
                       (Parameters_1.DataBank_IBAN_CheckDigits == Parameters_2.DataBank_IBAN_CheckDigits) &&
                       (Parameters_1.DataBank_IBAN_AccountNumber == Parameters_2.DataBank_IBAN_AccountNumber);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="Parameters_1">Primera instáncia a comparar.</param>
        /// <param name="Parameters_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(ParametersView Parameters_1, ParametersView Parameters_2)
        {
            return !(Parameters_1 == Parameters_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(Parameter_Id, Company_CIF, Company_Name).GetHashCode());
        }

        #endregion
    }
}
