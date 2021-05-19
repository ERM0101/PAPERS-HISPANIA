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
    public enum EffectTypesAttributes
    {
        Code,
        Description,
        None,
    }

    /// <summary>
    /// Class that Store the information of a definition of Effect.
    /// </summary>
    public class EffectTypesView : IMenuView
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
                return GlobalViewModel.GetStringFromIntIdValue(EffectType_Id);
            }
        }

        #endregion

        #region Main Fields

        public int EffectType_Id { get; set; }

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
        public EffectTypesView()
        {
            EffectType_Id = -1;
            Code = string.Empty;
            Description = string.Empty;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal EffectTypesView(HispaniaCompData.EffectType effectType)
        {
            EffectType_Id = effectType.EffectType_Id;
            Code = effectType.Code;
            Description = effectType.Description;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public EffectTypesView(EffectTypesView effectType)
        {
            EffectType_Id = effectType.EffectType_Id;
            Code = effectType.Code;
            Description = effectType.Description;
        }

        #endregion

        #region GetEffectType

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.EffectType GetEffectType()
        {
            HispaniaCompData.EffectType effectType = new HispaniaCompData.EffectType()
            {
                EffectType_Id = EffectType_Id,
                Code = Code,
                Description = Description
            };
            return effectType;
        }

        #endregion

        #region Validate

        /// <summary>
        /// Validate the data contained in the instance of the class.
        /// </summary>
        public void Validate(out EffectTypesAttributes ErrorField)
        {
            ErrorField = EffectTypesAttributes.None;
            if (!GlobalViewModel.IsEffectType(Code, out string ErrMsg))
            {
                ErrorField = EffectTypesAttributes.Code;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrDescription(Description))
            {
                ErrorField = EffectTypesAttributes.Description;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(EffectTypesView Data)
        {
            Code = Data.Code;
            Description = Data.Description;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(EffectTypesView Data, EffectTypesAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case EffectTypesAttributes.Code:
                     Code = Data.Code;
                     break;
                case EffectTypesAttributes.Description:
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
                EffectTypesView EffectType = obj as EffectTypesView;
                if ((Object)EffectType == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (EffectType_Id == EffectType.EffectType_Id) &&
                       (Code == EffectType.Code) && 
                       (Description == EffectType.Description);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="EffectType">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(EffectTypesView EffectType)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)EffectType == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (EffectType_Id == EffectType.EffectType_Id) &&
                       (Code == EffectType.Code) && 
                       (Description == EffectType.Description);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="EffectType_1">Primera instáncia a comparar.</param>
        /// <param name="EffectType_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(EffectTypesView EffectType_1, EffectTypesView EffectType_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(EffectType_1, EffectType_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)EffectType_1 == null) || ((object)EffectType_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (EffectType_1.EffectType_Id == EffectType_2.EffectType_Id) &&
                       (EffectType_1.Code == EffectType_2.Code) && 
                       (EffectType_1.Description == EffectType_2.Description);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="EffectType_1">Primera instáncia a comparar.</param>
        /// <param name="EffectType_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(EffectTypesView EffectType_1, EffectTypesView EffectType_2)
        {
            return !(EffectType_1 == EffectType_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(EffectType_Id, Code, Description).GetHashCode());
        }

        #endregion
    }
}
