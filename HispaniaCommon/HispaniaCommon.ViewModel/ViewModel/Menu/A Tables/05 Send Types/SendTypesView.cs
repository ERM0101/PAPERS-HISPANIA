#region Librerias usadas por la clase

using System;
using System.Collections.Generic;
using HispaniaCompData = HispaniaComptabilitat.Data;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Camps del Tipus
    /// </summary>
    public enum SendTypesAttributes
    {
        Code,
        Description,
        None,
    }

    /// <summary>
    /// Class that Store the information of a definition of Effect.
    /// </summary>
    public class SendTypesView : IMenuView
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
                        { "Codi", "Code" },
                        { "Descripció", "Description" }
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
                return GlobalViewModel.GetStringFromIntIdValue(SendType_Id);
            }
        }

        #endregion

        #region Main Fields

        public int SendType_Id { get; set; }

        public string Code { get; set; }

        public int CodeForSort
        {
            get
            {
                return GlobalViewModel.GetIntFromIntFromStringValue(Code);
            }
        }

        public string Description { get; set; }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public SendTypesView()
        {
            SendType_Id = -1;
            Code = string.Empty;
            Description = string.Empty;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal SendTypesView(HispaniaCompData.SendType sendType)
        {
            SendType_Id = sendType.SendType_Id;
            Code = sendType.Code;
            Description = sendType.Description;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public SendTypesView(SendTypesView sendType)
        {
            SendType_Id = sendType.SendType_Id;
            Code = sendType.Code;
            Description = sendType.Description;
        }

        #endregion

        #region GetSendType

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.SendType GetSendType()
        {
            HispaniaCompData.SendType sendType = new HispaniaCompData.SendType()
            {
                SendType_Id = SendType_Id,
                Code = Code,
                Description = Description
            };
            return sendType;
        }

        #endregion

        #region Validate

        /// <summary>
        /// Validate the data contained in the instance of the class.
        /// </summary>
        public void Validate(out SendTypesAttributes ErrorField)
        {
            ErrorField = SendTypesAttributes.None;
            if (!GlobalViewModel.IsSendType(Code, out string ErrMsg))
            {
                ErrorField = SendTypesAttributes.Code;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsDescription(Description))
            {
                ErrorField = SendTypesAttributes.Description;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(SendTypesView Data)
        {
            Code = Data.Code;
            Description = Data.Description;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(SendTypesView Data, SendTypesAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case SendTypesAttributes.Code:
                     Code = Data.Code;
                     break;
                case SendTypesAttributes.Description:
                     Description = Data.Description;
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
                SendTypesView SendType = obj as SendTypesView;
                if ((Object)SendType == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (SendType_Id == SendType.SendType_Id) &&
                       (Code == SendType.Code) && 
                       (Description == SendType.Description);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="SendType">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(SendTypesView SendType)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)SendType == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (SendType_Id == SendType.SendType_Id) &&
                       (Code == SendType.Code) && 
                       (Description == SendType.Description);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="SendType_1">Primera instáncia a comparar.</param>
        /// <param name="SendType_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(SendTypesView SendType_1, SendTypesView SendType_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(SendType_1, SendType_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)SendType_1 == null) || ((object)SendType_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (SendType_1.SendType_Id == SendType_2.SendType_Id) &&
                       (SendType_1.Code == SendType_2.Code) && 
                       (SendType_1.Description == SendType_2.Description);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="SendType_1">Primera instáncia a comparar.</param>
        /// <param name="SendType_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(SendTypesView SendType_1, SendTypesView SendType_2)
        {
            return !(SendType_1 == SendType_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(SendType_Id, Code, Description).GetHashCode());
        }

        #endregion
    }
}
