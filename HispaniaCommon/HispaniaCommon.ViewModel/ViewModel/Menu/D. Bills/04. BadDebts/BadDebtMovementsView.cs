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
    public enum BadDebtMovementsAttributes
    {
        BadDebt_Id,
        Amount,
        Date,
        None,
    }

    /// <summary>
    /// Class that Store the information of a BadDebtMovement.
    /// </summary>
    public class BadDebtMovementsView : IMenuView
    {
        #region Fields for Filter

        /// <summary>
        /// Store the list of fields that compose the BadDebtMovement class
        /// </summary>
        private static Dictionary<string, string> m_Fields = null;

        /// <summary>
        /// Get the list of fields that compose the BadDebtMovement class
        /// </summary>
        public static Dictionary<string, string> Fields
        {
            get
            {
                if (m_Fields == null)
                {
                    m_Fields = new Dictionary<string, string>
                    {
                        { "Data", "Date_Str" },  
                        { "Import", "Amount_Str" }
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
                return GlobalViewModel.GetStringFromIntValue(BadDebtMovement_Id);
            }
        }

        #endregion

        #region Main Fields

        public int BadDebtMovement_Id { get; set; }
        public int BadDebt_Id { get; set; }
        public DateTime Date { get; set; }
        public string Date_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDateTimeValue(Date);
            }
        }
        public decimal Amount { get; set; }
        public string Amount_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Amount, DecimalType.Currency, true);
            }
        }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public BadDebtMovementsView()
        {
            BadDebtMovement_Id = GlobalViewModel.IntIdInitValue;
            BadDebt_Id = GlobalViewModel.IntIdInitValue;
            Date = GlobalViewModel.DateTimeInitValue;
            Amount = GlobalViewModel.DecimalInitValue;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal BadDebtMovementsView(HispaniaCompData.BadDebtMovement BadDebtMovement)
        {
            BadDebtMovement_Id = BadDebtMovement.BadDebtMovement_Id;
            BadDebt_Id = BadDebtMovement.BadDebt_Id;
            Date = BadDebtMovement.Date;
            Amount = BadDebtMovement.Amount;
        }


        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public BadDebtMovementsView(BadDebtMovementsView BadDebtMovement)
        {
            BadDebtMovement_Id = BadDebtMovement.BadDebtMovement_Id;
            BadDebt_Id = BadDebtMovement.BadDebt_Id;
            Date = BadDebtMovement.Date;
            Amount = BadDebtMovement.Amount;
        }

        #endregion

        #region GetBadDebtMovement

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.BadDebtMovement GetBadDebtMovement()
        {
            HispaniaCompData.BadDebtMovement BadDebtMovement = new HispaniaCompData.BadDebtMovement()
            {
                BadDebtMovement_Id = BadDebtMovement_Id,
                BadDebt_Id = BadDebt_Id,
                Date = Date,
                Amount = Amount,
            };
            return (BadDebtMovement);
        }

        #endregion

        #region Validate

        public void Validate(out BadDebtMovementsAttributes ErrorField)
        {
            ErrorField = BadDebtMovementsAttributes.None;
            if (BadDebt_Id == GlobalViewModel.IntIdInitValue)
            {
                ErrorField = BadDebtMovementsAttributes.BadDebt_Id;
                throw new FormatException("Error, manca seleccionar l'identificador de l'impagat.");
            }
            if (Date == GlobalViewModel.DateTimeInitValue)
            {
                ErrorField = BadDebtMovementsAttributes.Date;
                throw new FormatException("Error, manca seleccionar una data pel pagament.");
            }
            if (!GlobalViewModel.IsCurrency(Amount, "Import del pagament", out string ErrMsg))
            {
                ErrorField = BadDebtMovementsAttributes.Amount;
                throw new FormatException(ErrMsg);
            }
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(BadDebtMovementsView Data)
        {
            BadDebt_Id = Data.BadDebt_Id;
            Date = Data.Date;
            Amount = Data.Amount;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(BadDebtMovementsView Data, BadDebtMovementsAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case BadDebtMovementsAttributes.BadDebt_Id:
                     BadDebt_Id = Data.BadDebt_Id;
                     break;
                case BadDebtMovementsAttributes.Date:
                     Date = Data.Date;
                     break;
                case BadDebtMovementsAttributes.Amount:
                     Amount = Data.Amount;
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
                BadDebtMovementsView BadDebtMovement = obj as BadDebtMovementsView;
                if ((Object)BadDebtMovement == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (BadDebtMovement_Id == BadDebtMovement.BadDebtMovement_Id) && (BadDebt_Id == BadDebtMovement.BadDebt_Id) && 
                       (Date == BadDebtMovement.Date) && (Amount == BadDebtMovement.Amount);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(BadDebtMovementsView BadDebtMovement)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)BadDebtMovement == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (BadDebtMovement_Id == BadDebtMovement.BadDebtMovement_Id) && (BadDebt_Id == BadDebtMovement.BadDebt_Id) && 
                       (Date == BadDebtMovement.Date) && (Amount == BadDebtMovement.Amount);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="BadDebtMovement_1">Primera instáncia a comparar.</param>
        /// <param name="BadDebtMovement_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(BadDebtMovementsView BadDebtMovement_1, BadDebtMovementsView BadDebtMovement_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(BadDebtMovement_1, BadDebtMovement_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)BadDebtMovement_1 == null) || ((object)BadDebtMovement_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (BadDebtMovement_1.BadDebtMovement_Id == BadDebtMovement_2.BadDebtMovement_Id) && 
                       (BadDebtMovement_1.BadDebt_Id == BadDebtMovement_2.BadDebt_Id) && 
                       (BadDebtMovement_1.Date == BadDebtMovement_2.Date) &&
                       (BadDebtMovement_1.Amount == BadDebtMovement_2.Amount);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="BadDebtMovement_1">Primera instáncia a comparar.</param>
        /// <param name="BadDebtMovement_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(BadDebtMovementsView BadDebtMovement_1, BadDebtMovementsView BadDebtMovement_2)
        {
            return !(BadDebtMovement_1 == BadDebtMovement_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(BadDebtMovement_Id).GetHashCode());
        }

        #endregion
    }
}
