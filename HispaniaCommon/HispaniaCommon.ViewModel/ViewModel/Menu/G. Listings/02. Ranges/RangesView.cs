using System;
using System.Collections.Generic;
using HispaniaCompData = HispaniaComptabilitat.Data;

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Camps del Tipus
    /// </summary>
    public enum RangesAttributes
    {
        Good_Code,
        Good_Description,
        Familia,
        None,
    }

    /// <summary>
    /// Class that Store the information of a Good.
    /// </summary>
    public class RangesView : IMenuView
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
                        { "Codi", "Good_Code" },
                        { "Descripció", "Good_Description" },
                        { "Família", "Familia" },
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
                return Good_Code;
            }
        }

        #endregion

        #region Main Fields

        public string Good_Code { get; set; }
        public string Good_Description { get; set; }
        public string Familia { get; set; }
        public decimal Cumulative_Sales_Retail_Price_1 { get; set; }
        public decimal Cumulative_Sales_Retail_Price_2 { get; set; }
        public decimal Cumulative_Sales_Retail_Price_3 { get; set; }
        public decimal Cumulative_Sales_Retail_Price_4 { get; set; }
        public decimal Cumulative_Sales_Retail_Price_5 { get; set; }
        public decimal Cumulative_Sales_Retail_Price_6 { get; set; }
        public decimal Cumulative_Sales_Retail_Price_7 { get; set; }
        public decimal Cumulative_Sales_Retail_Price_8 { get; set; }
        public decimal Cumulative_Sales_Retail_Price_9 { get; set; }
        public decimal Cumulative_Sales_Retail_Price_10 { get; set; }
        public decimal Cumulative_Sales_Retail_Price_11 { get; set; }
        public decimal Cumulative_Sales_Retail_Price_12 { get; set; }
        public decimal Cumulative_Sales_Cost_1 { get; set; }
        public decimal Cumulative_Sales_Cost_2 { get; set; }
        public decimal Cumulative_Sales_Cost_3 { get; set; }
        public decimal Cumulative_Sales_Cost_4 { get; set; }
        public decimal Cumulative_Sales_Cost_5 { get; set; }
        public decimal Cumulative_Sales_Cost_6 { get; set; }
        public decimal Cumulative_Sales_Cost_7 { get; set; }
        public decimal Cumulative_Sales_Cost_8 { get; set; }
        public decimal Cumulative_Sales_Cost_9 { get; set; }
        public decimal Cumulative_Sales_Cost_10 { get; set; }
        public decimal Cumulative_Sales_Cost_11 { get; set; }
        public decimal Cumulative_Sales_Cost_12 { get; set; }

        //public bool Canceled { get; set; }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public RangesView()
        {
            Good_Code = string.Empty;
            Good_Description = string.Empty;
            Familia = string.Empty;
            Cumulative_Sales_Retail_Price_1 = 0;
            Cumulative_Sales_Retail_Price_2 = 0;
            Cumulative_Sales_Retail_Price_3 = 0;
            Cumulative_Sales_Retail_Price_4 = 0;
            Cumulative_Sales_Retail_Price_5 = 0;
            Cumulative_Sales_Retail_Price_6 = 0;
            Cumulative_Sales_Retail_Price_7 = 0;
            Cumulative_Sales_Retail_Price_8 = 0;
            Cumulative_Sales_Retail_Price_9 = 0;
            Cumulative_Sales_Retail_Price_10 = 0;
            Cumulative_Sales_Retail_Price_11 = 0;
            Cumulative_Sales_Retail_Price_12 = 0;
            Cumulative_Sales_Cost_1 = 0;
            Cumulative_Sales_Cost_2 = 0;
            Cumulative_Sales_Cost_3 = 0;
            Cumulative_Sales_Cost_4 = 0;
            Cumulative_Sales_Cost_5 = 0;
            Cumulative_Sales_Cost_6 = 0;
            Cumulative_Sales_Cost_7 = 0;
            Cumulative_Sales_Cost_8 = 0;
            Cumulative_Sales_Cost_9 = 0;
            Cumulative_Sales_Cost_10 = 0;
            Cumulative_Sales_Cost_11 = 0;
            Cumulative_Sales_Cost_12 = 0;
            //Canceled = false;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal RangesView(HispaniaCompData.Range range)
        {
            Good_Code = range.Good_Code;
            Good_Description = range.Good_Description;
            Familia = range.Familia;
            Cumulative_Sales_Retail_Price_1 = GlobalViewModel.GetDecimalValue(range.Cumulative_Sales_Retail_Price_1);
            Cumulative_Sales_Retail_Price_2 = GlobalViewModel.GetDecimalValue(range.Cumulative_Sales_Retail_Price_2);
            Cumulative_Sales_Retail_Price_3 = GlobalViewModel.GetDecimalValue(range.Cumulative_Sales_Retail_Price_3);
            Cumulative_Sales_Retail_Price_4 = GlobalViewModel.GetDecimalValue(range.Cumulative_Sales_Retail_Price_4);
            Cumulative_Sales_Retail_Price_5 = GlobalViewModel.GetDecimalValue(range.Cumulative_Sales_Retail_Price_5);
            Cumulative_Sales_Retail_Price_6 = GlobalViewModel.GetDecimalValue(range.Cumulative_Sales_Retail_Price_6);
            Cumulative_Sales_Retail_Price_7 = GlobalViewModel.GetDecimalValue(range.Cumulative_Sales_Retail_Price_7);
            Cumulative_Sales_Retail_Price_8 = GlobalViewModel.GetDecimalValue(range.Cumulative_Sales_Retail_Price_8);
            Cumulative_Sales_Retail_Price_9 = GlobalViewModel.GetDecimalValue(range.Cumulative_Sales_Retail_Price_9);
            Cumulative_Sales_Retail_Price_10 = GlobalViewModel.GetDecimalValue(range.Cumulative_Sales_Retail_Price_10);
            Cumulative_Sales_Retail_Price_11 = GlobalViewModel.GetDecimalValue(range.Cumulative_Sales_Retail_Price_11); 
            Cumulative_Sales_Retail_Price_12 = GlobalViewModel.GetDecimalValue(range.Cumulative_Sales_Retail_Price_12); 
            Cumulative_Sales_Cost_1 = GlobalViewModel.GetDecimalValue(range.Cumulative_Sales_Cost_1);
            Cumulative_Sales_Cost_2 = GlobalViewModel.GetDecimalValue(range.Cumulative_Sales_Cost_2);
            Cumulative_Sales_Cost_3 = GlobalViewModel.GetDecimalValue(range.Cumulative_Sales_Cost_3);
            Cumulative_Sales_Cost_4 = GlobalViewModel.GetDecimalValue(range.Cumulative_Sales_Cost_4);
            Cumulative_Sales_Cost_5 = GlobalViewModel.GetDecimalValue(range.Cumulative_Sales_Cost_5);
            Cumulative_Sales_Cost_6 = GlobalViewModel.GetDecimalValue(range.Cumulative_Sales_Cost_6);
            Cumulative_Sales_Cost_7 = GlobalViewModel.GetDecimalValue(range.Cumulative_Sales_Cost_7);
            Cumulative_Sales_Cost_8 = GlobalViewModel.GetDecimalValue(range.Cumulative_Sales_Cost_8);
            Cumulative_Sales_Cost_9 = GlobalViewModel.GetDecimalValue(range.Cumulative_Sales_Cost_9);
            Cumulative_Sales_Cost_10 = GlobalViewModel.GetDecimalValue(range.Cumulative_Sales_Cost_10); 
            Cumulative_Sales_Cost_11 = GlobalViewModel.GetDecimalValue(range.Cumulative_Sales_Cost_11);
            Cumulative_Sales_Cost_12 = GlobalViewModel.GetDecimalValue(range.Cumulative_Sales_Cost_12);
            //Canceled = range.Canceled;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public RangesView(RangesView range)
        {
            Good_Code = range.Good_Code;
            Good_Description = range.Good_Description;
            Familia = range.Familia;
            Cumulative_Sales_Retail_Price_1 = range.Cumulative_Sales_Retail_Price_1;
            Cumulative_Sales_Retail_Price_2 = range.Cumulative_Sales_Retail_Price_2;
            Cumulative_Sales_Retail_Price_3 = range.Cumulative_Sales_Retail_Price_3;
            Cumulative_Sales_Retail_Price_4 = range.Cumulative_Sales_Retail_Price_4;
            Cumulative_Sales_Retail_Price_5 = range.Cumulative_Sales_Retail_Price_5;
            Cumulative_Sales_Retail_Price_6 = range.Cumulative_Sales_Retail_Price_6;
            Cumulative_Sales_Retail_Price_7 = range.Cumulative_Sales_Retail_Price_7;
            Cumulative_Sales_Retail_Price_8 = range.Cumulative_Sales_Retail_Price_8;
            Cumulative_Sales_Retail_Price_9 = range.Cumulative_Sales_Retail_Price_9;
            Cumulative_Sales_Retail_Price_10 = range.Cumulative_Sales_Retail_Price_10;
            Cumulative_Sales_Retail_Price_11 = range.Cumulative_Sales_Retail_Price_11;
            Cumulative_Sales_Retail_Price_12 = range.Cumulative_Sales_Retail_Price_12;
            Cumulative_Sales_Cost_1 = range.Cumulative_Sales_Cost_1;
            Cumulative_Sales_Cost_2 = range.Cumulative_Sales_Cost_2;
            Cumulative_Sales_Cost_3 = range.Cumulative_Sales_Cost_3;
            Cumulative_Sales_Cost_4 = range.Cumulative_Sales_Cost_4;
            Cumulative_Sales_Cost_5 = range.Cumulative_Sales_Cost_5;
            Cumulative_Sales_Cost_6 = range.Cumulative_Sales_Cost_6;
            Cumulative_Sales_Cost_7 = range.Cumulative_Sales_Cost_7;
            Cumulative_Sales_Cost_8 = range.Cumulative_Sales_Cost_8;
            Cumulative_Sales_Cost_9 = range.Cumulative_Sales_Cost_9;
            Cumulative_Sales_Cost_10 = range.Cumulative_Sales_Cost_10;
            Cumulative_Sales_Cost_11 = range.Cumulative_Sales_Cost_11;
            Cumulative_Sales_Cost_12 = range.Cumulative_Sales_Cost_12;
            //Canceled = range.Canceled;
        }

        #endregion

        #region GetGood

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.Range GetRange()
        {
            HispaniaCompData.Range range = new HispaniaCompData.Range()
            {
                Good_Code = Good_Code,
                Good_Description = Good_Description,
                Familia = Familia,
                Cumulative_Sales_Retail_Price_1 = Cumulative_Sales_Retail_Price_1,
                Cumulative_Sales_Retail_Price_2 = Cumulative_Sales_Retail_Price_2,
                Cumulative_Sales_Retail_Price_3 = Cumulative_Sales_Retail_Price_3,
                Cumulative_Sales_Retail_Price_4 = Cumulative_Sales_Retail_Price_4,
                Cumulative_Sales_Retail_Price_5 = Cumulative_Sales_Retail_Price_5,
                Cumulative_Sales_Retail_Price_6 = Cumulative_Sales_Retail_Price_6,
                Cumulative_Sales_Retail_Price_7 = Cumulative_Sales_Retail_Price_7,
                Cumulative_Sales_Retail_Price_8 = Cumulative_Sales_Retail_Price_8,
                Cumulative_Sales_Retail_Price_9 = Cumulative_Sales_Retail_Price_9,
                Cumulative_Sales_Retail_Price_10 = Cumulative_Sales_Retail_Price_10,
                Cumulative_Sales_Retail_Price_11 = Cumulative_Sales_Retail_Price_11,
                Cumulative_Sales_Retail_Price_12 = Cumulative_Sales_Retail_Price_12,
                Cumulative_Sales_Cost_1 = Cumulative_Sales_Cost_1,
                Cumulative_Sales_Cost_2 = Cumulative_Sales_Cost_2,
                Cumulative_Sales_Cost_3 = Cumulative_Sales_Cost_3,
                Cumulative_Sales_Cost_4 = Cumulative_Sales_Cost_4,
                Cumulative_Sales_Cost_5 = Cumulative_Sales_Cost_5,
                Cumulative_Sales_Cost_6 = Cumulative_Sales_Cost_6,
                Cumulative_Sales_Cost_7 = Cumulative_Sales_Cost_7,
                Cumulative_Sales_Cost_8 = Cumulative_Sales_Cost_8,
                Cumulative_Sales_Cost_9 = Cumulative_Sales_Cost_9,
                Cumulative_Sales_Cost_10 = Cumulative_Sales_Cost_10,
                Cumulative_Sales_Cost_11 = Cumulative_Sales_Cost_11,
                Cumulative_Sales_Cost_12 = Cumulative_Sales_Cost_12 //,
                //Canceled = Canceled,
            };
            return (range);
        }

        #endregion

        #region Validate

        public void Validate(out RangesAttributes ErrorField)
        {
            ErrorField = RangesAttributes.None;
            if (!GlobalViewModel.IsName(Good_Code))
            {
                ErrorField = RangesAttributes.Good_Code;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
            if (!GlobalViewModel.IsName(Good_Description))
            {
                ErrorField = RangesAttributes.Good_Description;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
            if (!GlobalViewModel.IsEmptyOrCod_Fam(Familia))
            {
                ErrorField = RangesAttributes.Familia;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(RangesView Data)
        {
            Good_Code = Data.Good_Code;
            Good_Description = Data.Good_Description;
            Familia = Data.Familia;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(RangesView Data, RangesAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case RangesAttributes.Good_Code:
                     Good_Code = Data.Good_Code;
                     break;
                case RangesAttributes.Good_Description:
                     Good_Description = Data.Good_Description;
                     break;
                case RangesAttributes.Familia:
                     Familia = Data.Familia;
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
                RangesView range = obj as RangesView;
                if ((Object)range == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Good_Code == range.Good_Code) && 
                       (Good_Description == range.Good_Description) &&
                       (Familia == range.Familia) &&
                       (Cumulative_Sales_Retail_Price_1 == range.Cumulative_Sales_Retail_Price_1) &&
                       (Cumulative_Sales_Retail_Price_2 == range.Cumulative_Sales_Retail_Price_2) &&
                       (Cumulative_Sales_Retail_Price_3 == range.Cumulative_Sales_Retail_Price_3) &&
                       (Cumulative_Sales_Retail_Price_4 == range.Cumulative_Sales_Retail_Price_4) &&
                       (Cumulative_Sales_Retail_Price_5 == range.Cumulative_Sales_Retail_Price_5) &&
                       (Cumulative_Sales_Retail_Price_6 == range.Cumulative_Sales_Retail_Price_6) &&
                       (Cumulative_Sales_Retail_Price_7 == range.Cumulative_Sales_Retail_Price_7) &&
                       (Cumulative_Sales_Retail_Price_8 == range.Cumulative_Sales_Retail_Price_8) &&
                       (Cumulative_Sales_Retail_Price_9 == range.Cumulative_Sales_Retail_Price_9) &&
                       (Cumulative_Sales_Retail_Price_10 == range.Cumulative_Sales_Retail_Price_10) &&
                       (Cumulative_Sales_Retail_Price_11 == range.Cumulative_Sales_Retail_Price_11) &&
                       (Cumulative_Sales_Retail_Price_12 == range.Cumulative_Sales_Retail_Price_12) &&
                       (Cumulative_Sales_Cost_1 == range.Cumulative_Sales_Cost_1) &&
                       (Cumulative_Sales_Cost_2 == range.Cumulative_Sales_Cost_2) &&
                       (Cumulative_Sales_Cost_3 == range.Cumulative_Sales_Cost_3) &&
                       (Cumulative_Sales_Cost_4 == range.Cumulative_Sales_Cost_4) &&
                       (Cumulative_Sales_Cost_5 == range.Cumulative_Sales_Cost_5) &&
                       (Cumulative_Sales_Cost_6 == range.Cumulative_Sales_Cost_6) &&
                       (Cumulative_Sales_Cost_7 == range.Cumulative_Sales_Cost_7) &&
                       (Cumulative_Sales_Cost_8 == range.Cumulative_Sales_Cost_8) &&
                       (Cumulative_Sales_Cost_9 == range.Cumulative_Sales_Cost_9) &&
                       (Cumulative_Sales_Cost_10 == range.Cumulative_Sales_Cost_10) &&
                       (Cumulative_Sales_Cost_11 == range.Cumulative_Sales_Cost_11) &&
                       (Cumulative_Sales_Cost_12 == range.Cumulative_Sales_Cost_12); // &&
                       // (Canceled == good.Canceled);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(RangesView range)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)range == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Good_Code == range.Good_Code) && 
                       (Good_Description == range.Good_Description) &&
                       (Familia == range.Familia) &&
                       (Cumulative_Sales_Retail_Price_1 == range.Cumulative_Sales_Retail_Price_1) &&
                       (Cumulative_Sales_Retail_Price_2 == range.Cumulative_Sales_Retail_Price_2) &&
                       (Cumulative_Sales_Retail_Price_3 == range.Cumulative_Sales_Retail_Price_3) &&
                       (Cumulative_Sales_Retail_Price_4 == range.Cumulative_Sales_Retail_Price_4) &&
                       (Cumulative_Sales_Retail_Price_5 == range.Cumulative_Sales_Retail_Price_5) &&
                       (Cumulative_Sales_Retail_Price_6 == range.Cumulative_Sales_Retail_Price_6) &&
                       (Cumulative_Sales_Retail_Price_7 == range.Cumulative_Sales_Retail_Price_7) &&
                       (Cumulative_Sales_Retail_Price_8 == range.Cumulative_Sales_Retail_Price_8) &&
                       (Cumulative_Sales_Retail_Price_9 == range.Cumulative_Sales_Retail_Price_9) &&
                       (Cumulative_Sales_Retail_Price_10 == range.Cumulative_Sales_Retail_Price_10) &&
                       (Cumulative_Sales_Retail_Price_11 == range.Cumulative_Sales_Retail_Price_11) &&
                       (Cumulative_Sales_Retail_Price_12 == range.Cumulative_Sales_Retail_Price_12) &&
                       (Cumulative_Sales_Cost_1 == range.Cumulative_Sales_Cost_1) &&
                       (Cumulative_Sales_Cost_2 == range.Cumulative_Sales_Cost_2) &&
                       (Cumulative_Sales_Cost_3 == range.Cumulative_Sales_Cost_3) &&
                       (Cumulative_Sales_Cost_4 == range.Cumulative_Sales_Cost_4) &&
                       (Cumulative_Sales_Cost_5 == range.Cumulative_Sales_Cost_5) &&
                       (Cumulative_Sales_Cost_6 == range.Cumulative_Sales_Cost_6) &&
                       (Cumulative_Sales_Cost_7 == range.Cumulative_Sales_Cost_7) &&
                       (Cumulative_Sales_Cost_8 == range.Cumulative_Sales_Cost_8) &&
                       (Cumulative_Sales_Cost_9 == range.Cumulative_Sales_Cost_9) &&
                       (Cumulative_Sales_Cost_10 == range.Cumulative_Sales_Cost_10) &&
                       (Cumulative_Sales_Cost_11 == range.Cumulative_Sales_Cost_11) &&
                       (Cumulative_Sales_Cost_12 == range.Cumulative_Sales_Cost_12); // &&
                       // (Canceled == good.Canceled);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="range_1">Primera instáncia a comparar.</param>
        /// <param name="range_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(RangesView range_1, RangesView range_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(range_1, range_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)range_1 == null) || ((object)range_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (range_1.Good_Code == range_2.Good_Code) &&
                        (range_1.Good_Description == range_2.Good_Description) &&
                        (range_1.Familia == range_2.Familia) &&
                        (range_1.Cumulative_Sales_Retail_Price_1 == range_2.Cumulative_Sales_Retail_Price_1) &&
                        (range_1.Cumulative_Sales_Retail_Price_2 == range_2.Cumulative_Sales_Retail_Price_2) &&
                        (range_1.Cumulative_Sales_Retail_Price_3 == range_2.Cumulative_Sales_Retail_Price_3) &&
                        (range_1.Cumulative_Sales_Retail_Price_4 == range_2.Cumulative_Sales_Retail_Price_4) &&
                        (range_1.Cumulative_Sales_Retail_Price_5 == range_2.Cumulative_Sales_Retail_Price_5) &&
                        (range_1.Cumulative_Sales_Retail_Price_6 == range_2.Cumulative_Sales_Retail_Price_6) &&
                        (range_1.Cumulative_Sales_Retail_Price_7 == range_2.Cumulative_Sales_Retail_Price_7) &&
                        (range_1.Cumulative_Sales_Retail_Price_8 == range_2.Cumulative_Sales_Retail_Price_8) &&
                        (range_1.Cumulative_Sales_Retail_Price_9 == range_2.Cumulative_Sales_Retail_Price_9) &&
                        (range_1.Cumulative_Sales_Retail_Price_10 == range_2.Cumulative_Sales_Retail_Price_10) &&
                        (range_1.Cumulative_Sales_Retail_Price_11 == range_2.Cumulative_Sales_Retail_Price_11) &&
                        (range_1.Cumulative_Sales_Retail_Price_12 == range_2.Cumulative_Sales_Retail_Price_12) &&
                        (range_1.Cumulative_Sales_Cost_1 == range_2.Cumulative_Sales_Cost_1) &&
                        (range_1.Cumulative_Sales_Cost_2 == range_2.Cumulative_Sales_Cost_2) &&
                        (range_1.Cumulative_Sales_Cost_3 == range_2.Cumulative_Sales_Cost_3) &&
                        (range_1.Cumulative_Sales_Cost_4 == range_2.Cumulative_Sales_Cost_4) &&
                        (range_1.Cumulative_Sales_Cost_5 == range_2.Cumulative_Sales_Cost_5) &&
                        (range_1.Cumulative_Sales_Cost_6 == range_2.Cumulative_Sales_Cost_6) &&
                        (range_1.Cumulative_Sales_Cost_7 == range_2.Cumulative_Sales_Cost_7) &&
                        (range_1.Cumulative_Sales_Cost_8 == range_2.Cumulative_Sales_Cost_8) &&
                        (range_1.Cumulative_Sales_Cost_9 == range_2.Cumulative_Sales_Cost_9) &&
                        (range_1.Cumulative_Sales_Cost_10 == range_2.Cumulative_Sales_Cost_10) &&
                        (range_1.Cumulative_Sales_Cost_11 == range_2.Cumulative_Sales_Cost_11) &&
                        (range_1.Cumulative_Sales_Cost_12 == range_2.Cumulative_Sales_Cost_12); // &&
                        //(range_1.Canceled == range_2.Canceled);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="customer_1">Primera instáncia a comparar.</param>
        /// <param name="customer_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(RangesView range_1, RangesView range_2)
        {
            return !(range_1 == range_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(Good_Code).GetHashCode());
        }

        #endregion
    }
}
