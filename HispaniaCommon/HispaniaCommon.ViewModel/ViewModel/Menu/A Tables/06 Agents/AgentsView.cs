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
    public enum AgentsAttributes
    {
        Name,
        DNIorCIF,
        Address,
        Phone,
        Fax,
        MobilePhone,
        EMail,
        Contact_Info, // Set of Agent_Phone, Agent_MobilePhone and Agent_EMail fields
        PostalCode,
        Comment,
        None,
    }

    /// <summary>
    /// Class that Store the information of a Agent.
    /// </summary>
    public class AgentsView : IMenuView
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
                        { "Número", "Agent_Id" },
                        { "Nom", "Name" },
                        { "DNI / CIF", "DNIorCIF" },
                        { "Adreça", "Address" },
                        { "CP", "PostalCode_Str" },
                        { "Telèfon", "Phone" },
                        { "Mòbil", "MobilePhone" },
                        { "Fax", "Fax" },
                        { "E-mail", "EMail" },
                        { "Comentaris", "Comment" }
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
                return GlobalViewModel.GetStringFromIntIdValue(Agent_Id);
            }
        }

        #endregion

        #region Main Fields

        public int Agent_Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string DNIorCIF { get; set; }

        public string Phone { get; set; }

        public string Fax { get; set; }

        public string MobilePhone { get; set; }

        public string EMail { get; set; }

        public string Comment { get; set; }

        public bool Canceled { get; set; }

        #endregion

        #region ForeignKey Properties

        private int? PostalCode_Id { get; set; }

        private PostalCodesView _PostalCode;

        public PostalCodesView PostalCode
        {
            get
            {
                if ((_PostalCode == null) && (PostalCode_Id != -1) && (PostalCode_Id != null))
                {
                    _PostalCode = new PostalCodesView(GlobalViewModel.Instance.HispaniaViewModel.GetPostalCode((int)PostalCode_Id));
                }
                return (_PostalCode);
            }
            set
            {
                if (value != null)
                {
                    _PostalCode = new PostalCodesView(value);
                    if (_PostalCode == null) PostalCode_Id = -1;
                    else PostalCode_Id = _PostalCode.PostalCode_Id;
                }
                else _PostalCode = null;
            }
        }

        public string PostalCode_Str
        {
            get
            {
                if (PostalCode == null)
                {
                    return string.Empty;
                }
                else return PostalCode.Postal_Code;
            }
        }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public AgentsView()
        {
            Agent_Id = -1;
            Name = string.Empty;
            Address = string.Empty;
            Phone = string.Empty;
            Fax = string.Empty;
            DNIorCIF = string.Empty;
            MobilePhone = string.Empty;
            EMail = string.Empty;
            Comment = string.Empty;
            Canceled = false;
            PostalCode = null;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal AgentsView(HispaniaCompData.Agent agent)
        {
            Agent_Id = agent.Agent_Id;
            Name = agent.Name;
            Address = agent.Address;
            Phone = agent.Phone;
            Fax = agent.Fax;
            DNIorCIF = agent.DNIorCIF;
            MobilePhone = agent.MobilePhone;
            EMail = agent.EMail;
            Comment = agent.Comment;
            Canceled = agent.Canceled;
            PostalCode_Id = agent.PostalCode_Id;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public AgentsView(AgentsView agent)
        {
            Agent_Id = agent.Agent_Id;
            Name = agent.Name;
            Address = agent.Address;
            Phone = agent.Phone;
            Fax = agent.Fax;
            DNIorCIF = agent.DNIorCIF;
            MobilePhone = agent.MobilePhone;
            EMail = agent.EMail;
            Comment = agent.Comment;
            Canceled = agent.Canceled;
            PostalCode = agent.PostalCode;
        }

        #endregion

        #region GetAgent

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.Agent GetAgent()
        {
            HispaniaCompData.Agent agent = new HispaniaCompData.Agent()
            {
                Agent_Id = Agent_Id,
                Name = Name,
                Address = Address,
                Phone = Phone,
                Fax = Fax,
                DNIorCIF = DNIorCIF,
                MobilePhone = MobilePhone,
                EMail = EMail,
                Comment = Comment,
                Canceled = Canceled,
                PostalCode_Id = PostalCode_Id
            };
            return (agent);
        }

        #endregion

        #region Validate

        /// <summary>
        /// Validate the data contained in the instance of the class.
        /// </summary>
        public void Validate(out AgentsAttributes ErrorField)
        {
            ErrorField = AgentsAttributes.None;
            #region Main Fields
            if (!GlobalViewModel.IsName(Name))
            {
                ErrorField = AgentsAttributes.Name;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
            if (!GlobalViewModel.IsCIF(DNIorCIF))
            {
                ErrorField = AgentsAttributes.DNIorCIF;
                throw new FormatException(GlobalViewModel.ValidationCIFError); 
            }
            if (!GlobalViewModel.IsAddress(Address))
            {
                ErrorField = AgentsAttributes.Address;
                throw new FormatException(GlobalViewModel.ValidationAddressError);
            }
            if (String.IsNullOrEmpty(Phone) && (String.IsNullOrEmpty(MobilePhone)) && (String.IsNullOrEmpty(EMail)))
            {
                ErrorField = AgentsAttributes.Contact_Info;
                throw new FormatException("Error, manca introduir una dada de contacte: un telèfon, un mòbil o un e-mail.");
            }
            if (!GlobalViewModel.IsEmptyOrPhoneNumber(Phone))
            {
                ErrorField = AgentsAttributes.Phone;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrPhoneNumberError);
            }
            if (!GlobalViewModel.IsEmptyOrPhoneNumber(Fax))
            {
                ErrorField = AgentsAttributes.Fax;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrPhoneNumberError);
            }
            if (!GlobalViewModel.IsEmptyOrMobilePhoneNumber(MobilePhone))
            {
                ErrorField = AgentsAttributes.MobilePhone;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrMobilePhoneNumberError);
            }
            if (!GlobalViewModel.IsEmptyOrEmail(EMail))
            {
                ErrorField = AgentsAttributes.EMail;
                throw new FormatException(GlobalViewModel.ValidationEmptyOrEmailError);
            }
            if (!GlobalViewModel.IsEmptyOrComment(Comment))
            {
                ErrorField = AgentsAttributes.Comment;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
            #endregion
            if (PostalCode == null)
            {
                ErrorField = AgentsAttributes.PostalCode;
                throw new FormatException("Error, manca seleccionar el Codi Postal.");
            }
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(AgentsView Data)
        {
            Name = Data.Name;
            DNIorCIF = Data.DNIorCIF;
            Address = Data.Address;
            Phone = Data.Phone;
            MobilePhone = Data.MobilePhone;
            EMail = Data.EMail;
            Phone = Data.Phone;
            Fax = Data.Fax;
            MobilePhone = Data.MobilePhone;
            EMail = Data.EMail;
            PostalCode = Data.PostalCode;
            Comment = Data.Comment;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(AgentsView Data, AgentsAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case AgentsAttributes.Name:
                     Name = Data.Name;
                     break;
                case AgentsAttributes.DNIorCIF:
                     DNIorCIF = Data.DNIorCIF;
                     break;
                case AgentsAttributes.Address:
                     Address = Data.Address;
                     break;
                case AgentsAttributes.Contact_Info:
                     Phone = Data.Phone;
                     MobilePhone = Data.MobilePhone;
                     EMail = Data.EMail;
                     break;
                case AgentsAttributes.Phone:
                     Phone = Data.Phone;
                     break;
                case AgentsAttributes.Fax:
                     Fax = Data.Fax;
                     break;
                case AgentsAttributes.MobilePhone:
                     MobilePhone = Data.MobilePhone;
                     break;
                case AgentsAttributes.EMail:
                     EMail = Data.EMail;
                     break;
                case AgentsAttributes.Comment:
                     Comment = Data.Comment;
                     break;
                case AgentsAttributes.PostalCode:
                     PostalCode = Data.PostalCode;
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
                AgentsView agent = obj as AgentsView;
                if ((Object)agent == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Agent_Id == agent.Agent_Id) && (Name == agent.Name) && (Address == agent.Address) &&
                       (PostalCode == agent.PostalCode) && (Phone == agent.Phone) && (DNIorCIF == agent.DNIorCIF) && 
                       (EMail == agent.EMail) && (MobilePhone == agent.MobilePhone) && (Comment == agent.Comment) && 
                       (PostalCode == agent.PostalCode) && (Fax == agent.Fax) && (Canceled == agent.Canceled);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(AgentsView agent)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)agent == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Agent_Id == agent.Agent_Id) && (Name == agent.Name) && (Address == agent.Address) &&
                       (PostalCode == agent.PostalCode) && (Phone == agent.Phone) && (DNIorCIF == agent.DNIorCIF) && 
                       (EMail == agent.EMail) && (MobilePhone == agent.MobilePhone) && (Comment == agent.Comment) && 
                       (PostalCode == agent.PostalCode) && (Fax == agent.Fax) && (Canceled == agent.Canceled);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="agent_1">Primera instáncia a comparar.</param>
        /// <param name="agent_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(AgentsView agent_1, AgentsView agent_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(agent_1, agent_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)agent_1 == null) || ((object)agent_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (agent_1.Agent_Id == agent_2.Agent_Id) && (agent_1.Name == agent_2.Name) && 
                       (agent_1.Address == agent_2.Address) && 
                       (agent_1.PostalCode == agent_2.PostalCode) && 
                       (agent_1.Phone == agent_2.Phone) && 
                       (agent_1.DNIorCIF == agent_2.DNIorCIF) &&
                       (agent_1.MobilePhone == agent_2.MobilePhone) &&
                       (agent_1.EMail == agent_2.EMail) && 
                       (agent_1.Comment == agent_2.Comment) && 
                       (agent_1.PostalCode == agent_2.PostalCode) &&
                       (agent_1.Fax == agent_2.Fax) &&
                       (agent_1.Canceled == agent_2.Canceled);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="agent_1">Primera instáncia a comparar.</param>
        /// <param name="agent_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(AgentsView agent_1, AgentsView agent_2)
        {
            return !(agent_1 == agent_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(Agent_Id, Name, DNIorCIF).GetHashCode());
        }

        #endregion
    }
}
