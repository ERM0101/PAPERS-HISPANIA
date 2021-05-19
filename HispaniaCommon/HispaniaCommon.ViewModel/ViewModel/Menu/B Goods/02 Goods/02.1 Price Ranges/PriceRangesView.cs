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
    public enum PriceRangesAttributes
    {
        Good_Id,
        Sequence,
        Since,
        Until,
        None,
    }
    /// <summary>
    /// Class that Store the information of a CustomerOrderMovement.
    /// </summary>
    public class PriceRangesView : IMenuView
    {
        #region Fields for Filter

        /// <summary>
        /// Store the list of fields that compose the PriceRange class
        /// </summary>
        private static Dictionary<string, string> m_Fields = null;

        /// <summary>
        /// Get the list of fields that compose the PriceRange class
        /// </summary>
        public static Dictionary<string, string> Fields
        {
            get
            {
                if (m_Fields == null)
                {
                    m_Fields = new Dictionary<string, string>
                    {
                        { "Article", "Good_Code" },
                        { "Descripció", "Good_Description" },
                        { "Ordre", "Sequence" },
                        { "Des de", "Since_Str" },
                        { "Fins a", "Until_Str" }
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
                return GlobalViewModel.GetStringFromIntIdValue(PriceRange_Id);
            }
        }

        #endregion

        #region Main Fields

        public int PriceRange_Id { get; set; }
        public decimal Sequence { get; set; }
        public decimal Since { get; set; }
        public string Since_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Since, DecimalType.Currency);
            }
        }
        public decimal Until { get; set; }
        public string Until_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Until, DecimalType.Currency);
            }
        }

        #endregion

        #region ForeignKey Properties

        #region Goods

        private int _Good_Id { get; set; }

        private GoodsView _Good;

        public GoodsView Good
        {
            get
            {
                if ((_Good == null) && (_Good_Id != GlobalViewModel.IntIdInitValue))
                {
                    _Good = new GoodsView(GlobalViewModel.Instance.HispaniaViewModel.GetGood((int)_Good_Id));
                }
                return (_Good);
            }
            set
            {
                if (value != null)
                {
                    _Good = new GoodsView(value);
                    if (_Good == null) _Good_Id = GlobalViewModel.IntIdInitValue;
                    else _Good_Id = _Good.Good_Id;
                }
                else
                {
                    _Good = null;
                    _Good_Id = GlobalViewModel.IntIdInitValue;
                }
            }
        }

        public string Good_Code
        {
            get
            {
                return (Good is null) ? "Sense Article" : Good.Good_Code;
            }
        }

        public string Good_Description
        {
            get
            {
                return (Good is null) ? "Sense Descripció" : Good.Good_Description;
            }
        }

        #endregion

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public PriceRangesView(int good_Id)
        {
            PriceRange_Id = -1;
            Sequence = 0;
            Since = 0;
            Until = 0;
            _Good_Id = good_Id;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal PriceRangesView(HispaniaCompData.PriceRange PriceRange)
        {
            PriceRange_Id = PriceRange.PriceRange_Id;
            Sequence = GlobalViewModel.GetDecimalValue(PriceRange.Sequence);
            Since = GlobalViewModel.GetDecimalValue(PriceRange.Since);
            Until = GlobalViewModel.GetDecimalValue(PriceRange.Until);
            _Good_Id = PriceRange.Good_Id;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public PriceRangesView(PriceRangesView PriceRange)
        {
            PriceRange_Id = PriceRange.PriceRange_Id;
            Sequence = PriceRange.Sequence;
            Since = PriceRange.Since;
            Until = PriceRange.Until;
            _Good_Id = PriceRange._Good_Id;
        }

        #endregion

        #region GetPriceRange

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.PriceRange GetPriceRange()
        {
            HispaniaCompData.PriceRange PriceRange = new HispaniaCompData.PriceRange()
            {
                PriceRange_Id = PriceRange_Id,
                Sequence = Sequence,
                Since = Since,
                Until = Until,
                Good_Id = _Good_Id
            };
            return (PriceRange);
        }

        #endregion

        #region Validate

        public void Validate(out PriceRangesAttributes ErrorField)
        {
            ErrorField = PriceRangesAttributes.None;
            if (!GlobalViewModel.IsEmptyOrShortDecimal(Sequence, "Ordre", out string ErrMsg))
            {
                ErrorField = PriceRangesAttributes.Sequence;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(Since, "Des de", out ErrMsg))
            {
                ErrorField = PriceRangesAttributes.Since;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(Until, "Fins a", out ErrMsg))
            {
                ErrorField = PriceRangesAttributes.Until;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
            if (Good == null)
            {
                ErrorField = PriceRangesAttributes.Good_Id;
                throw new FormatException("Error, manca seleccionar l'article.");
            }
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(PriceRangesView Data)
        {
            Good = Data.Good;
            Sequence = Data.Sequence;
            Since = Data.Since;
            Until = Data.Until;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(PriceRangesView Data, PriceRangesAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case PriceRangesAttributes.Good_Id:
                     Good = Data.Good;
                     break;
                case PriceRangesAttributes.Sequence:
                     Sequence = Data.Sequence;
                     break;
                case PriceRangesAttributes.Since:
                     Since = Data.Since;
                     break;
                case PriceRangesAttributes.Until:
                     Until = Data.Until;
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
                PriceRangesView PriceRange = obj as PriceRangesView;
                if ((Object)PriceRange == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (PriceRange_Id == PriceRange.PriceRange_Id) && (Sequence == PriceRange.Sequence) &&
                       (Since == PriceRange.Since) && (Until == PriceRange.Until) && (_Good_Id == PriceRange._Good_Id);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(PriceRangesView PriceRange)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)PriceRange == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (PriceRange_Id == PriceRange.PriceRange_Id) && (Sequence == PriceRange.Sequence) &&
                       (Since == PriceRange.Since) && (Until == PriceRange.Until) && (_Good_Id == PriceRange._Good_Id);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="PriceRange_1">Primera instáncia a comparar.</param>
        /// <param name="PriceRange_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(PriceRangesView PriceRange_1, PriceRangesView PriceRange_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(PriceRange_1, PriceRange_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)PriceRange_1 == null) || ((object)PriceRange_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (PriceRange_1.PriceRange_Id == PriceRange_2.PriceRange_Id) && 
                       (PriceRange_1.Sequence == PriceRange_2.Sequence) &&
                       (PriceRange_1.Since == PriceRange_2.Since) && 
                       (PriceRange_1.Until == PriceRange_2.Until) &&
                       (PriceRange_1._Good_Id == PriceRange_2._Good_Id);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="customer_1">Primera instáncia a comparar.</param>
        /// <param name="customer_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(PriceRangesView customer_1, PriceRangesView customer_2)
        {
            return !(customer_1 == customer_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(PriceRange_Id).GetHashCode());
        }

        #endregion
    }
}
