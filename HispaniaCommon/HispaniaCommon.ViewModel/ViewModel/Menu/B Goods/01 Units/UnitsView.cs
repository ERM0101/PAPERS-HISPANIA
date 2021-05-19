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
    public enum UnitsAttributes
    {
        Code,
        Shipping,
        Billing,
        None,
    }

    /// <summary>
    /// Class that Store the information of a City and its Postal Code.
    /// </summary>
    public class UnitsView : IMenuView
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
                        { "Codi d'Unitat", "Code" },
                        { "Expedició", "Shipping" },
                        { "Facturació", "Billing" }
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
                return GlobalViewModel.GetStringFromIntIdValue(Unit_Id);
            }
        }

        #endregion

        #region Main Fields

        public int Unit_Id { get; set; }
        public string Code { get; set; }
        public int Code_Value
        {
            get
            {
                return GlobalViewModel.GetIntValue(Code);
            }
        }
        public string Shipping { get; set; }
        public string Billing { get; set; }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public UnitsView()
        {
            Unit_Id = -1;
            Code = string.Empty;
            Shipping = string.Empty;
            Billing = string.Empty;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal UnitsView(HispaniaCompData.Unit unit)
        {
            Unit_Id = unit.Unit_Id;
            Code = unit.Code;
            Shipping = unit.Shipping;
            Billing = unit.Billing;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public UnitsView(UnitsView unit)
        {
            Unit_Id = unit.Unit_Id;
            Code = unit.Code;
            Shipping = unit.Shipping;
            Billing = unit.Billing;
        }

        #endregion

        #region GetBillingSerie

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.Unit GetUnit()
        {
            HispaniaCompData.Unit unit = new HispaniaCompData.Unit()
            {
                Unit_Id = Unit_Id,
                Code = Code,
                Shipping = Shipping,
                Billing = Billing
            };
            return unit;
        }

        #endregion

        #region Validate

        public void Validate(out UnitsAttributes ErrorField)
        {
            ErrorField = UnitsAttributes.None;
            if (!GlobalViewModel.IsUnitCode(Code, out string ErrMsg))
            {
                ErrorField = UnitsAttributes.Code;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrName(Shipping))
            {
                ErrorField = UnitsAttributes.Shipping;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
            if (!GlobalViewModel.IsEmptyOrName(Billing))
            {
                ErrorField = UnitsAttributes.Billing;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(UnitsView Data)
        {
            Code = Data.Code;
            Shipping = Data.Shipping;
            Billing = Data.Billing;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(UnitsView Data, UnitsAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case UnitsAttributes.Code:
                     Code = Data.Code;
                     break;
                case UnitsAttributes.Shipping:
                     Shipping = Data.Shipping;
                     break;
                case UnitsAttributes.Billing:
                     Billing = Data.Billing;
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
                UnitsView units = obj as UnitsView;
                if ((Object)units == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Unit_Id == units.Unit_Id) && (Code == units.Code) && (Shipping == units.Shipping) &&
                       (Billing == units.Billing);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(UnitsView units)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)units == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Unit_Id == units.Unit_Id) && (Code == units.Code) && (Shipping == units.Shipping) &&
                       (Billing == units.Billing);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="units_1">Primera instáncia a comparar.</param>
        /// <param name="units_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(UnitsView units_1, UnitsView units_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(units_1, units_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)units_1 == null) || ((object)units_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (units_1.Unit_Id == units_2.Unit_Id) && (units_1.Code == units_2.Code) &&
                       (units_1.Shipping == units_2.Shipping) && (units_1.Billing == units_2.Billing);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="units_1">Primera instáncia a comparar.</param>
        /// <param name="units_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(UnitsView units_1, UnitsView units_2)
        {
            return !(units_1 == units_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(Unit_Id, Code).GetHashCode());
        }

        #endregion
    }
}
