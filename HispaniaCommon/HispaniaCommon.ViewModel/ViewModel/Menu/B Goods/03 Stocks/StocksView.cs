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
    public enum StocksAttributes
    {
        Good_Code,
        Good_Description,
        Initial,
        Initial_Fact,
        None,
    }

    /// <summary>
    /// Class that Store the information of a Good.
    /// </summary>
    public class StocksView : IMenuView
    {
        #region Fields for Filter

        /// <summary>
        /// Store the list of fields that compose the Good class
        /// </summary>
        private static Dictionary<string, string> m_Fields = null;

        /// <summary>
        /// Get the list of fields that compose the Good class
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
                        { "Estoc d'Expedició", "Initial_Str" },
                        { "Estoc de Facturació", "Initial_Fact_Str" }
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
                return GlobalViewModel.GetStringFromIntIdValue(Good_Id);
            }
        }

        #endregion

        #region Main Fields

        public int Good_Id { get; set; }
        public string Good_Code { get; set; }
        public string Good_Description { get; set; }
        public decimal Initial { get; set; }
        public string Initial_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Initial, DecimalType.Unit);
            }
        }
        public decimal Initial_Fact { get; set; }
        public string Initial_Fact_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Initial_Fact, DecimalType.Unit);
            }
        }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public StocksView()
        {
            Good_Id = -1;
            Good_Code = string.Empty;
            Good_Description = string.Empty;
            Initial = 0;
            Initial_Fact = 0;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal StocksView(HispaniaCompData.Good good)
        {
            Good_Id = good.Good_Id;
            Good_Code = good.Good_Code;
            Good_Description = good.Good_Description;
            Initial = GlobalViewModel.GetDecimalValue(good.Initial);
            Initial_Fact = GlobalViewModel.GetDecimalValue(good.Initial_Fact);
        }


        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public StocksView(StocksView good)
        {
            Good_Id = good.Good_Id;
            Good_Code = good.Good_Code;
            Good_Description = good.Good_Description;
            Initial = good.Initial;
            Initial_Fact = good.Initial_Fact;
        }

        #endregion

        #region GetGood

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.Good GetGood()
        {
            HispaniaCompData.Good good = new HispaniaCompData.Good()
            {
                Good_Id = Good_Id,
                Good_Code = Good_Code,
                Good_Description = Good_Description,
                Initial = Initial,
                Initial_Fact = Initial_Fact
            };
            return (good);
        }

        #endregion

        #region Validate

        public void Validate(out StocksAttributes ErrorField)
        {
            ErrorField = StocksAttributes.None;
            if (!GlobalViewModel.IsGoodCode(Good_Code))
            {
                ErrorField = StocksAttributes.Good_Code;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
            if (!GlobalViewModel.IsName(Good_Description))
            {
                ErrorField = StocksAttributes.Good_Description;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
            if (!GlobalViewModel.IsEmptyOrStock(Initial, "Estoc d'Expedició", out string ErrMsg, true))
            {
                ErrorField = StocksAttributes.Initial;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrStock(Initial_Fact, "Estoc de Facturació", out ErrMsg, true))
            {
                ErrorField = StocksAttributes.Initial_Fact;
                throw new FormatException(ErrMsg);
            }
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(StocksView Data)
        {
            Good_Code = Data.Good_Code;
            Good_Description = Data.Good_Description;
            Initial = Data.Initial;
            Initial_Fact = Data.Initial_Fact;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(StocksView Data, StocksAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case StocksAttributes.Good_Code:
                     Good_Code = Data.Good_Code;
                     break;
                case StocksAttributes.Good_Description:
                     Good_Description = Data.Good_Description;
                     break;
                case StocksAttributes.Initial:
                     Initial = Data.Initial;
                     break;
                case StocksAttributes.Initial_Fact:
                     Initial_Fact = Data.Initial_Fact;
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
                StocksView stock = obj as StocksView;
                if ((Object)stock == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return  (Good_Id == stock.Good_Id) && (Good_Code == stock.Good_Code) && (Good_Description == stock.Good_Description) &&
                        (Initial == stock.Initial) && (Initial_Fact == stock.Initial_Fact);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(StocksView stock)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)stock == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Good_Id == stock.Good_Id) && (Good_Code == stock.Good_Code) && (Good_Description == stock.Good_Description) &&
                       (Initial == stock.Initial) && (Initial_Fact == stock.Initial_Fact);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="stock_1">Primera instáncia a comparar.</param>
        /// <param name="stock_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(StocksView stock_1, StocksView stock_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(stock_1, stock_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)stock_1 == null) || ((object)stock_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return  (stock_1.Good_Id == stock_2.Good_Id) &&
                        (stock_1.Good_Code == stock_2.Good_Code) &&
                        (stock_1.Good_Description == stock_2.Good_Description) &&
                        (stock_1.Initial == stock_2.Initial) &&
                        (stock_1.Initial_Fact == stock_2.Initial_Fact);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="stock_1">Primera instáncia a comparar.</param>
        /// <param name="stock_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(StocksView stock_1, StocksView stock_2)
        {
            return !(stock_1 == stock_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(Good_Id, Good_Code).GetHashCode());
        }

        #endregion
    }
}
