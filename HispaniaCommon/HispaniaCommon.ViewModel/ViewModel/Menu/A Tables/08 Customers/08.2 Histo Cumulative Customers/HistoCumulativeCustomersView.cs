#region Librerias usadas por la clase

using System;
using System.Collections.Generic;
using HispaniaCompData = HispaniaComptabilitat.Data;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Class that Store the information of a HistoCumulativeCustomer.
    /// </summary>
    public class HistoCumulativeCustomersView : IMenuView
    {
        #region Fields for Filter

        /// <summary>
        /// Store the list of fields that compose the HistoCumulativeCustomer class
        /// </summary>
        private static Dictionary<string, string> m_Fields = null;

        /// <summary>
        /// Get the list of fields that compose the HistoCumulativeCustomer class
        /// </summary>
        public static Dictionary<string, string> Fields
        {
            get
            {
                if (m_Fields == null)
                {
                    m_Fields = new Dictionary<string, string>
                    {
                        { "Any", "Data_Year_Str" },
                        { "Gener", "Cumulative_Sales_1_Str" },
                        { "Febrer", "Cumulative_Sales_2_Str" },
                        { "Març", "Cumulative_Sales_3_Str" },
                        { "Abril", "Cumulative_Sales_4_Str" },
                        { "Maig", "Cumulative_Sales_5_Str" },
                        { "Juny", "Cumulative_Sales_6_Str" },
                        { "Juliol", "Cumulative_Sales_7_Str" },
                        { "Agost", "Cumulative_Sales_8_Str" },
                        { "Setembre", "Cumulative_Sales_9_Str" },
                        { "Octubre", "Cumulative_Sales_10_Str" },
                        { "Novembre", "Cumulative_Sales_11_Str" },
                        { "Dessembre", "Cumulative_Sales_12_Str" },
                        { "Total Acumulat", "Cumulative_Sales_Total_Str" }
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
                return GlobalViewModel.GetStringFromIntIdValue(Histo_Id);
            }
        }

        #endregion

        #region Main Fields

        public int Histo_Id { get; set; }
        public decimal Data_Year { get; set; }
        public string Data_Year_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromYearValue(Data_Year);
            }
        }
        public decimal Cumulative_Sales_1 { get; set; }
        public string Cumulative_Sales_1_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Cumulative_Sales_1, DecimalType.Currency, true);
            }
        }
        public decimal Cumulative_Sales_2 { get; set; }
        public string Cumulative_Sales_2_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Cumulative_Sales_2, DecimalType.Currency, true);
            }
        }
        public decimal Cumulative_Sales_3 { get; set; }
        public string Cumulative_Sales_3_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Cumulative_Sales_3, DecimalType.Currency, true);
            }
        }
        public decimal Cumulative_Sales_4 { get; set; }
        public string Cumulative_Sales_4_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Cumulative_Sales_4, DecimalType.Currency, true);
            }
        }
        public decimal Cumulative_Sales_5 { get; set; }
        public string Cumulative_Sales_5_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Cumulative_Sales_5, DecimalType.Currency, true);
            }
        }
        public decimal Cumulative_Sales_6 { get; set; }
        public string Cumulative_Sales_6_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Cumulative_Sales_6, DecimalType.Currency, true);
            }
        }
        public decimal Cumulative_Sales_7 { get; set; }
        public string Cumulative_Sales_7_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Cumulative_Sales_7, DecimalType.Currency, true);
            }
        }
        public decimal Cumulative_Sales_8 { get; set; }
        public string Cumulative_Sales_8_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Cumulative_Sales_8, DecimalType.Currency, true);
            }
        }
        public decimal Cumulative_Sales_9 { get; set; }
        public string Cumulative_Sales_9_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Cumulative_Sales_9, DecimalType.Currency, true);
            }
        }
        public decimal Cumulative_Sales_10 { get; set; }
        public string Cumulative_Sales_10_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Cumulative_Sales_10, DecimalType.Currency, true);
            }
        }
        public decimal Cumulative_Sales_11 { get; set; }
        public string Cumulative_Sales_11_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Cumulative_Sales_11, DecimalType.Currency, true);
            }
        }
        public decimal Cumulative_Sales_12 { get; set; }
        public string Cumulative_Sales_12_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Cumulative_Sales_12, DecimalType.Currency, true);
            }
        }

        public decimal Cumulative_Sales_Total
        {
            get
            {
                return Cumulative_Sales_1 + Cumulative_Sales_2 + Cumulative_Sales_3 + Cumulative_Sales_4 + 
                       Cumulative_Sales_5 + Cumulative_Sales_6 + Cumulative_Sales_7 + Cumulative_Sales_8 + 
                       Cumulative_Sales_9 + Cumulative_Sales_10 + Cumulative_Sales_11 + Cumulative_Sales_12;
            }
        }
        public string Cumulative_Sales_Total_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Cumulative_Sales_Total, DecimalType.Currency, true);
            }
        }

        #endregion

        #region ForeignKey Properties

        public int Customer_Id
        {
            get;
            private set;
        }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public HistoCumulativeCustomersView(int customer_Id)
        {
            Histo_Id = GlobalViewModel.IntIdInitValue;
            Customer_Id = customer_Id;
            Data_Year = GlobalViewModel.YearInitValue;
            Cumulative_Sales_1 = GlobalViewModel.DecimalInitValue;
            Cumulative_Sales_2 = GlobalViewModel.DecimalInitValue;
            Cumulative_Sales_3 = GlobalViewModel.DecimalInitValue;
            Cumulative_Sales_4 = GlobalViewModel.DecimalInitValue;
            Cumulative_Sales_5 = GlobalViewModel.DecimalInitValue;
            Cumulative_Sales_6 = GlobalViewModel.DecimalInitValue;
            Cumulative_Sales_7 = GlobalViewModel.DecimalInitValue;
            Cumulative_Sales_8 = GlobalViewModel.DecimalInitValue;
            Cumulative_Sales_9 = GlobalViewModel.DecimalInitValue;
            Cumulative_Sales_10 = GlobalViewModel.DecimalInitValue;
            Cumulative_Sales_11 = GlobalViewModel.DecimalInitValue;
            Cumulative_Sales_12 = GlobalViewModel.DecimalInitValue;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HistoCumulativeCustomersView(HispaniaCompData.HistoCumulativeCustomer histoCumulativeCustomer)
        {
            Histo_Id = histoCumulativeCustomer.Histo_Id;
            Customer_Id = histoCumulativeCustomer.Customer_Id;
            Data_Year = GlobalViewModel.GetDecimalYearValue(histoCumulativeCustomer.Data_Year);
            Cumulative_Sales_1 = GlobalViewModel.GetDecimalValue(histoCumulativeCustomer.Cumulative_Sales_1);
            Cumulative_Sales_2 = GlobalViewModel.GetDecimalValue(histoCumulativeCustomer.Cumulative_Sales_2);
            Cumulative_Sales_3 = GlobalViewModel.GetDecimalValue(histoCumulativeCustomer.Cumulative_Sales_3);
            Cumulative_Sales_4 = GlobalViewModel.GetDecimalValue(histoCumulativeCustomer.Cumulative_Sales_4);
            Cumulative_Sales_5 = GlobalViewModel.GetDecimalValue(histoCumulativeCustomer.Cumulative_Sales_5);
            Cumulative_Sales_6 = GlobalViewModel.GetDecimalValue(histoCumulativeCustomer.Cumulative_Sales_6);
            Cumulative_Sales_7 = GlobalViewModel.GetDecimalValue(histoCumulativeCustomer.Cumulative_Sales_7);
            Cumulative_Sales_8 = GlobalViewModel.GetDecimalValue(histoCumulativeCustomer.Cumulative_Sales_8);
            Cumulative_Sales_9 = GlobalViewModel.GetDecimalValue(histoCumulativeCustomer.Cumulative_Sales_9);
            Cumulative_Sales_10 = GlobalViewModel.GetDecimalValue(histoCumulativeCustomer.Cumulative_Sales_10);
            Cumulative_Sales_11 = GlobalViewModel.GetDecimalValue(histoCumulativeCustomer.Cumulative_Sales_11);
            Cumulative_Sales_12 = GlobalViewModel.GetDecimalValue(histoCumulativeCustomer.Cumulative_Sales_12);
        }


        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public HistoCumulativeCustomersView(HistoCumulativeCustomersView histoCumulativeCustomer)
        {
            Histo_Id = histoCumulativeCustomer.Histo_Id;
            Customer_Id = histoCumulativeCustomer.Customer_Id;
            Data_Year = histoCumulativeCustomer.Data_Year;
            Cumulative_Sales_1 = histoCumulativeCustomer.Cumulative_Sales_1;
            Cumulative_Sales_2 = histoCumulativeCustomer.Cumulative_Sales_2;
            Cumulative_Sales_3 = histoCumulativeCustomer.Cumulative_Sales_3;
            Cumulative_Sales_4 = histoCumulativeCustomer.Cumulative_Sales_4;
            Cumulative_Sales_5 = histoCumulativeCustomer.Cumulative_Sales_5;
            Cumulative_Sales_6 = histoCumulativeCustomer.Cumulative_Sales_6;
            Cumulative_Sales_7 = histoCumulativeCustomer.Cumulative_Sales_7;
            Cumulative_Sales_8 = histoCumulativeCustomer.Cumulative_Sales_8;
            Cumulative_Sales_9 = histoCumulativeCustomer.Cumulative_Sales_9;
            Cumulative_Sales_10 = histoCumulativeCustomer.Cumulative_Sales_10;
            Cumulative_Sales_11 = histoCumulativeCustomer.Cumulative_Sales_11;
            Cumulative_Sales_12 = histoCumulativeCustomer.Cumulative_Sales_12;
        }

        #endregion

        #region GetHistoCumulativeCustomer

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.HistoCumulativeCustomer GetHistoCumulativeCustomer()
        {
            HispaniaCompData.HistoCumulativeCustomer histoCumulativeCustomer = new HispaniaCompData.HistoCumulativeCustomer()
            {
                Histo_Id = Histo_Id,
                Customer_Id = Customer_Id,
                Cumulative_Sales_1 = Cumulative_Sales_1,
                Cumulative_Sales_2 = Cumulative_Sales_2,
                Cumulative_Sales_3 = Cumulative_Sales_3,
                Cumulative_Sales_4 = Cumulative_Sales_4,
                Cumulative_Sales_5 = Cumulative_Sales_5,
                Cumulative_Sales_6 = Cumulative_Sales_6,
                Cumulative_Sales_7 = Cumulative_Sales_7,
                Cumulative_Sales_8 = Cumulative_Sales_8,
                Cumulative_Sales_9 = Cumulative_Sales_9,
                Cumulative_Sales_10 = Cumulative_Sales_10,
                Cumulative_Sales_11 = Cumulative_Sales_11,
                Cumulative_Sales_12 = Cumulative_Sales_12
            };
            return (histoCumulativeCustomer);
        }

        #endregion

        #region Validate

        /// <summary>
        /// Validate the data contained in the instance of the class.
        //  It's not needed since the user can't edit the data of this entity in the application
        /// </summary>
        public void Validate()
        {
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
                HistoCumulativeCustomersView histoCumulativeCustomer = obj as HistoCumulativeCustomersView;
                if ((Object)histoCumulativeCustomer == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Histo_Id == histoCumulativeCustomer.Histo_Id) && (Customer_Id == histoCumulativeCustomer.Customer_Id) &&
                       (Data_Year == histoCumulativeCustomer.Data_Year) &&
                       (Cumulative_Sales_1 == histoCumulativeCustomer.Cumulative_Sales_1) &&
                       (Cumulative_Sales_2 == histoCumulativeCustomer.Cumulative_Sales_2) &&
                       (Cumulative_Sales_3 == histoCumulativeCustomer.Cumulative_Sales_3) &&
                       (Cumulative_Sales_4 == histoCumulativeCustomer.Cumulative_Sales_4) &&
                       (Cumulative_Sales_5 == histoCumulativeCustomer.Cumulative_Sales_5) &&
                       (Cumulative_Sales_6 == histoCumulativeCustomer.Cumulative_Sales_6) &&
                       (Cumulative_Sales_7 == histoCumulativeCustomer.Cumulative_Sales_7) &&
                       (Cumulative_Sales_8 == histoCumulativeCustomer.Cumulative_Sales_8) &&
                       (Cumulative_Sales_9 == histoCumulativeCustomer.Cumulative_Sales_9) &&
                       (Cumulative_Sales_10 == histoCumulativeCustomer.Cumulative_Sales_10) &&
                       (Cumulative_Sales_11 == histoCumulativeCustomer.Cumulative_Sales_11) &&
                       (Cumulative_Sales_12 == histoCumulativeCustomer.Cumulative_Sales_12);
}

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(HistoCumulativeCustomersView histoCumulativeCustomer)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)histoCumulativeCustomer == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Histo_Id == histoCumulativeCustomer.Histo_Id) && (Customer_Id == histoCumulativeCustomer.Customer_Id) &&
                       (Data_Year == histoCumulativeCustomer.Data_Year) &&
                       (Cumulative_Sales_1 == histoCumulativeCustomer.Cumulative_Sales_1) &&
                       (Cumulative_Sales_2 == histoCumulativeCustomer.Cumulative_Sales_2) &&
                       (Cumulative_Sales_3 == histoCumulativeCustomer.Cumulative_Sales_3) &&
                       (Cumulative_Sales_4 == histoCumulativeCustomer.Cumulative_Sales_4) &&
                       (Cumulative_Sales_5 == histoCumulativeCustomer.Cumulative_Sales_5) &&
                       (Cumulative_Sales_6 == histoCumulativeCustomer.Cumulative_Sales_6) &&
                       (Cumulative_Sales_7 == histoCumulativeCustomer.Cumulative_Sales_7) &&
                       (Cumulative_Sales_8 == histoCumulativeCustomer.Cumulative_Sales_8) &&
                       (Cumulative_Sales_9 == histoCumulativeCustomer.Cumulative_Sales_9) &&
                       (Cumulative_Sales_10 == histoCumulativeCustomer.Cumulative_Sales_10) &&
                       (Cumulative_Sales_11 == histoCumulativeCustomer.Cumulative_Sales_11) &&
                       (Cumulative_Sales_12 == histoCumulativeCustomer.Cumulative_Sales_12);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="histoCumulativeCustomer_1">Primera instáncia a comparar.</param>
        /// <param name="histoCumulativeCustomer_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(HistoCumulativeCustomersView histoCumulativeCustomer_1, HistoCumulativeCustomersView histoCumulativeCustomer_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(histoCumulativeCustomer_1, histoCumulativeCustomer_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)histoCumulativeCustomer_1 == null) || ((object)histoCumulativeCustomer_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (histoCumulativeCustomer_1.Histo_Id == histoCumulativeCustomer_2.Histo_Id) && 
                       (histoCumulativeCustomer_1.Customer_Id == histoCumulativeCustomer_2.Customer_Id) &&
                       (histoCumulativeCustomer_1.Data_Year == histoCumulativeCustomer_2.Data_Year) &&
                       (histoCumulativeCustomer_1.Cumulative_Sales_1 == histoCumulativeCustomer_2.Cumulative_Sales_1) &&
                       (histoCumulativeCustomer_1.Cumulative_Sales_2 == histoCumulativeCustomer_2.Cumulative_Sales_2) &&
                       (histoCumulativeCustomer_1.Cumulative_Sales_3 == histoCumulativeCustomer_2.Cumulative_Sales_3) &&
                       (histoCumulativeCustomer_1.Cumulative_Sales_4 == histoCumulativeCustomer_2.Cumulative_Sales_4) &&
                       (histoCumulativeCustomer_1.Cumulative_Sales_5 == histoCumulativeCustomer_2.Cumulative_Sales_5) &&
                       (histoCumulativeCustomer_1.Cumulative_Sales_6 == histoCumulativeCustomer_2.Cumulative_Sales_6) &&
                       (histoCumulativeCustomer_1.Cumulative_Sales_7 == histoCumulativeCustomer_2.Cumulative_Sales_7) &&
                       (histoCumulativeCustomer_1.Cumulative_Sales_8 == histoCumulativeCustomer_2.Cumulative_Sales_8) &&
                       (histoCumulativeCustomer_1.Cumulative_Sales_9 == histoCumulativeCustomer_2.Cumulative_Sales_9) &&
                       (histoCumulativeCustomer_1.Cumulative_Sales_10 == histoCumulativeCustomer_2.Cumulative_Sales_10) &&
                       (histoCumulativeCustomer_1.Cumulative_Sales_11 == histoCumulativeCustomer_2.Cumulative_Sales_11) &&
                       (histoCumulativeCustomer_1.Cumulative_Sales_12 == histoCumulativeCustomer_2.Cumulative_Sales_12);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="customer_1">Primera instáncia a comparar.</param>
        /// <param name="customer_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(HistoCumulativeCustomersView customer_1, HistoCumulativeCustomersView customer_2)
        {
            return !(customer_1 == customer_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(Histo_Id, Customer_Id).GetHashCode());
        }

        #endregion
    }
}
