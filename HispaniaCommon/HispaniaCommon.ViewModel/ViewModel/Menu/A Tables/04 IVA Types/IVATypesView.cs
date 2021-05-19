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
    public enum IVATypesAttributes
    {
        Type,
        IVAPercent,
        SurchargePercent,
        None,
    }

    /// <summary>
    /// Class that Store the information of a definition of IVA Type.
    /// </summary>
    public class IVATypesView : IMenuView
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
                        { "Tipus", "Type" },
                        { "Percentatge d'IVA", "IVAPercent_Str" },
                        { "Data d'Inici de Vigència de l'IVA", "IVAInitValidityData_Str" },
                        { "Data de Fi de Vigència de l'IVA", "IVAEndValidityData_Str" },
                        { "Percentatge del Tipus de Recàrrec", "SurchargePercent_Str" },
                        //{ "Data d'Inici de Vigència del Tipus de Recàrrec", "SurchargeInitValidityData_Str" },
                        //{ "Data de Fi de Vigència del Tipus de Recàrrec", "SurchargeEndValidityData_Str" }
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
                return GlobalViewModel.GetStringFromIntIdValue(IVAType_Id);
            }
        }

        #endregion

        #region Main Fields

        public int IVAType_Id { get; set; }

        public string Type { get; set; }

        public decimal IVAPercent { get; set; }

        public string IVAPercent_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(IVAPercent, DecimalType.Percent, true);
            }
        }

        public DateTime IVAInitValidityData { get; set; }

        public string IVAInitValidityData_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDateTimeValue(IVAInitValidityData);
            }
        }

        public DateTime? IVAEndValidityData { get; set; }

        public string IVAEndValidityData_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDateTimeValue(IVAEndValidityData);
            }
        }

        public decimal SurchargePercent { get; set; }

        public string SurchargePercent_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(SurchargePercent, DecimalType.Percent, true);
            }
        }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public IVATypesView()
        {
            IVAType_Id = -1;
            Type = string.Empty;
            IVAPercent = 0;
            DateTime DateNow = DateTime.Now;
            IVAInitValidityData = DateNow;
            IVAEndValidityData = null;
            SurchargePercent = 0;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal IVATypesView(HispaniaCompData.IVAType ivaType)
        {
            IVAType_Id = ivaType.IVAType_Id;
            Type = ivaType.Type;
            IVAPercent = ivaType.IVAPercent;
            IVAInitValidityData = ivaType.IVAInitValidityData;
            IVAEndValidityData = ivaType.IVAEndValidityData;
            SurchargePercent = ivaType.SurchargePercent;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public IVATypesView(IVATypesView ivaType)
        {
            IVAType_Id = ivaType.IVAType_Id;
            Type = ivaType.Type;
            IVAPercent = ivaType.IVAPercent;
            IVAInitValidityData = ivaType.IVAInitValidityData;
            IVAEndValidityData = ivaType.IVAEndValidityData;
            SurchargePercent = ivaType.SurchargePercent;
        }

        #endregion

        #region GetIVAType

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.IVAType GetIVAType()
        {
            HispaniaCompData.IVAType ivaType = new HispaniaCompData.IVAType()
            {
                IVAType_Id = IVAType_Id,
                Type = Type,
                IVAPercent = IVAPercent,
                IVAInitValidityData = IVAInitValidityData,
                IVAEndValidityData = IVAEndValidityData,
                SurchargePercent = SurchargePercent,
            };
            return ivaType;
        }

        #endregion

        #region Validate

        /// <summary>
        /// Validate the data contained in the instance of the class.
        /// </summary>
        public void Validate(out IVATypesAttributes ErrorField)
        {
            ErrorField = IVATypesAttributes.None;
            if (!GlobalViewModel.IsIVAType(Type, out string ErrMsg))
            {
                ErrorField = IVATypesAttributes.Type;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsPercent(IVAPercent, "PERCENTATGE D''IVA", out ErrMsg))
            {
                ErrorField = IVATypesAttributes.IVAPercent;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsPercent(SurchargePercent, "PERCENTATGE DE RECÀRREC", out ErrMsg))
            {
                ErrorField = IVATypesAttributes.SurchargePercent;
                throw new FormatException(ErrMsg);
            }
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(IVATypesView Data)
        {
            Type = Data.Type;
            IVAPercent = Data.IVAPercent;
            SurchargePercent = Data.SurchargePercent;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(IVATypesView Data, IVATypesAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case IVATypesAttributes.Type:
                     Type = Data.Type;
                     break;
                case IVATypesAttributes.IVAPercent:
                     IVAPercent = Data.IVAPercent;
                     break;
                case IVATypesAttributes.SurchargePercent:
                     SurchargePercent = Data.SurchargePercent;
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
                IVATypesView IVAType = obj as IVATypesView;
                if ((Object)IVAType == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (IVAType_Id == IVAType.IVAType_Id) &&
                       (Type == IVAType.Type) &&
                       (IVAPercent == IVAType.IVAPercent) &&
                       (SurchargePercent == IVAType.SurchargePercent) &&
                       (GlobalViewModel.CompareNullDateTimeValues(IVAInitValidityData, IVAType.IVAInitValidityData.Date)) &&
                       (GlobalViewModel.CompareNullDateTimeValues(IVAEndValidityData, IVAType.IVAEndValidityData));
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="IVAType">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(IVATypesView IVAType)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)IVAType == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (IVAType_Id == IVAType.IVAType_Id) &&
                       (Type == IVAType.Type) &&
                       (IVAPercent == IVAType.IVAPercent) &&
                       (SurchargePercent == IVAType.SurchargePercent) &&
                       (GlobalViewModel.CompareNullDateTimeValues(IVAInitValidityData, IVAType.IVAInitValidityData.Date)) &&
                       (GlobalViewModel.CompareNullDateTimeValues(IVAEndValidityData, IVAType.IVAEndValidityData));
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="IVAType_1">Primera instáncia a comparar.</param>
        /// <param name="IVAType_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(IVATypesView IVAType_1, IVATypesView IVAType_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(IVAType_1, IVAType_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)IVAType_1 == null) || ((object)IVAType_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (IVAType_1.IVAType_Id == IVAType_2.IVAType_Id) &&
                       (IVAType_1.Type == IVAType_2.Type) && 
                       (IVAType_1.IVAPercent == IVAType_2.IVAPercent) &&
                       (IVAType_1.SurchargePercent == IVAType_2.SurchargePercent) &&
                       (GlobalViewModel.CompareNullDateTimeValues(IVAType_1.IVAInitValidityData, IVAType_2.IVAInitValidityData.Date)) &&
                       (GlobalViewModel.CompareNullDateTimeValues(IVAType_1.IVAEndValidityData, IVAType_2.IVAEndValidityData));
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="IVAType_1">Primera instáncia a comparar.</param>
        /// <param name="IVAType_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(IVATypesView IVAType_1, IVATypesView IVAType_2)
        {
            return !(IVAType_1 == IVAType_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(IVAType_Id, Type).GetHashCode());
        }

        #endregion
    }
}
