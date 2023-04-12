#region Librerias usadas por la clase

using System;
using System.Collections.Generic;
using HispaniaCompData = HispaniaComptabilitat.Data;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Class that Store the information of a HistoCumulativeProvider.
    /// </summary>
    public class HistoCumulativeProvidersView : IMenuView
    {
        #region Fields for Filter

        /// <summary>
        /// Store the list of fields that compose the HistoCumulativeProvider class
        /// </summary>
        private static Dictionary<string, string> m_Fields = null;

        /// <summary>
        /// Get the list of fields that compose the HistoCumulativeProvider class
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

        public int Provider_Id
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
        public HistoCumulativeProvidersView(int provider_Id)
        {
            Histo_Id = GlobalViewModel.IntIdInitValue;
            Provider_Id = provider_Id;
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
        internal HistoCumulativeProvidersView(HispaniaCompData.HistoCumulativeProvider histoCumulativeProvider)
        {
            Histo_Id = histoCumulativeProvider.Histo_Id;
            Provider_Id = histoCumulativeProvider.Provider_Id;
            Data_Year = GlobalViewModel.GetDecimalYearValue(histoCumulativeProvider.Data_Year);
            Cumulative_Sales_1 = GlobalViewModel.GetDecimalValue(histoCumulativeProvider.Cumulative_Sales_1);
            Cumulative_Sales_2 = GlobalViewModel.GetDecimalValue(histoCumulativeProvider.Cumulative_Sales_2);
            Cumulative_Sales_3 = GlobalViewModel.GetDecimalValue(histoCumulativeProvider.Cumulative_Sales_3);
            Cumulative_Sales_4 = GlobalViewModel.GetDecimalValue(histoCumulativeProvider.Cumulative_Sales_4);
            Cumulative_Sales_5 = GlobalViewModel.GetDecimalValue(histoCumulativeProvider.Cumulative_Sales_5);
            Cumulative_Sales_6 = GlobalViewModel.GetDecimalValue(histoCumulativeProvider.Cumulative_Sales_6);
            Cumulative_Sales_7 = GlobalViewModel.GetDecimalValue(histoCumulativeProvider.Cumulative_Sales_7);
            Cumulative_Sales_8 = GlobalViewModel.GetDecimalValue(histoCumulativeProvider.Cumulative_Sales_8);
            Cumulative_Sales_9 = GlobalViewModel.GetDecimalValue(histoCumulativeProvider.Cumulative_Sales_9);
            Cumulative_Sales_10 = GlobalViewModel.GetDecimalValue(histoCumulativeProvider.Cumulative_Sales_10);
            Cumulative_Sales_11 = GlobalViewModel.GetDecimalValue(histoCumulativeProvider.Cumulative_Sales_11);
            Cumulative_Sales_12 = GlobalViewModel.GetDecimalValue(histoCumulativeProvider.Cumulative_Sales_12);
        }


        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public HistoCumulativeProvidersView(HistoCumulativeProvidersView histoCumulativeProvider)
        {
            Histo_Id = histoCumulativeProvider.Histo_Id;
            Provider_Id = histoCumulativeProvider.Provider_Id;
            Data_Year = histoCumulativeProvider.Data_Year;
            Cumulative_Sales_1 = histoCumulativeProvider.Cumulative_Sales_1;
            Cumulative_Sales_2 = histoCumulativeProvider.Cumulative_Sales_2;
            Cumulative_Sales_3 = histoCumulativeProvider.Cumulative_Sales_3;
            Cumulative_Sales_4 = histoCumulativeProvider.Cumulative_Sales_4;
            Cumulative_Sales_5 = histoCumulativeProvider.Cumulative_Sales_5;
            Cumulative_Sales_6 = histoCumulativeProvider.Cumulative_Sales_6;
            Cumulative_Sales_7 = histoCumulativeProvider.Cumulative_Sales_7;
            Cumulative_Sales_8 = histoCumulativeProvider.Cumulative_Sales_8;
            Cumulative_Sales_9 = histoCumulativeProvider.Cumulative_Sales_9;
            Cumulative_Sales_10 = histoCumulativeProvider.Cumulative_Sales_10;
            Cumulative_Sales_11 = histoCumulativeProvider.Cumulative_Sales_11;
            Cumulative_Sales_12 = histoCumulativeProvider.Cumulative_Sales_12;
        }

        #endregion

        #region GetHistoCumulativeProvider

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.HistoCumulativeProvider GetHistoCumulativeProvider()
        {
            HispaniaCompData.HistoCumulativeProvider histoCumulativeProvider = new HispaniaCompData.HistoCumulativeProvider()
            {
                Histo_Id = Histo_Id,
                Provider_Id = Provider_Id,
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
            return (histoCumulativeProvider);
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
                HistoCumulativeProvidersView histoCumulativeProvider = obj as HistoCumulativeProvidersView;
                if ((Object)histoCumulativeProvider == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Histo_Id == histoCumulativeProvider.Histo_Id) && (Provider_Id == histoCumulativeProvider.Provider_Id) &&
                       (Data_Year == histoCumulativeProvider.Data_Year) &&
                       (Cumulative_Sales_1 == histoCumulativeProvider.Cumulative_Sales_1) &&
                       (Cumulative_Sales_2 == histoCumulativeProvider.Cumulative_Sales_2) &&
                       (Cumulative_Sales_3 == histoCumulativeProvider.Cumulative_Sales_3) &&
                       (Cumulative_Sales_4 == histoCumulativeProvider.Cumulative_Sales_4) &&
                       (Cumulative_Sales_5 == histoCumulativeProvider.Cumulative_Sales_5) &&
                       (Cumulative_Sales_6 == histoCumulativeProvider.Cumulative_Sales_6) &&
                       (Cumulative_Sales_7 == histoCumulativeProvider.Cumulative_Sales_7) &&
                       (Cumulative_Sales_8 == histoCumulativeProvider.Cumulative_Sales_8) &&
                       (Cumulative_Sales_9 == histoCumulativeProvider.Cumulative_Sales_9) &&
                       (Cumulative_Sales_10 == histoCumulativeProvider.Cumulative_Sales_10) &&
                       (Cumulative_Sales_11 == histoCumulativeProvider.Cumulative_Sales_11) &&
                       (Cumulative_Sales_12 == histoCumulativeProvider.Cumulative_Sales_12);
}

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(HistoCumulativeProvidersView histoCumulativeProvider)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)histoCumulativeProvider == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Histo_Id == histoCumulativeProvider.Histo_Id) && (Provider_Id == histoCumulativeProvider.Provider_Id) &&
                       (Data_Year == histoCumulativeProvider.Data_Year) &&
                       (Cumulative_Sales_1 == histoCumulativeProvider.Cumulative_Sales_1) &&
                       (Cumulative_Sales_2 == histoCumulativeProvider.Cumulative_Sales_2) &&
                       (Cumulative_Sales_3 == histoCumulativeProvider.Cumulative_Sales_3) &&
                       (Cumulative_Sales_4 == histoCumulativeProvider.Cumulative_Sales_4) &&
                       (Cumulative_Sales_5 == histoCumulativeProvider.Cumulative_Sales_5) &&
                       (Cumulative_Sales_6 == histoCumulativeProvider.Cumulative_Sales_6) &&
                       (Cumulative_Sales_7 == histoCumulativeProvider.Cumulative_Sales_7) &&
                       (Cumulative_Sales_8 == histoCumulativeProvider.Cumulative_Sales_8) &&
                       (Cumulative_Sales_9 == histoCumulativeProvider.Cumulative_Sales_9) &&
                       (Cumulative_Sales_10 == histoCumulativeProvider.Cumulative_Sales_10) &&
                       (Cumulative_Sales_11 == histoCumulativeProvider.Cumulative_Sales_11) &&
                       (Cumulative_Sales_12 == histoCumulativeProvider.Cumulative_Sales_12);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="histoCumulativeProvider_1">Primera instáncia a comparar.</param>
        /// <param name="histoCumulativeProvider_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(HistoCumulativeProvidersView histoCumulativeProvider_1, HistoCumulativeProvidersView histoCumulativeProvider_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(histoCumulativeProvider_1, histoCumulativeProvider_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)histoCumulativeProvider_1 == null) || ((object)histoCumulativeProvider_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (histoCumulativeProvider_1.Histo_Id == histoCumulativeProvider_2.Histo_Id) && 
                       (histoCumulativeProvider_1.Provider_Id == histoCumulativeProvider_2.Provider_Id) &&
                       (histoCumulativeProvider_1.Data_Year == histoCumulativeProvider_2.Data_Year) &&
                       (histoCumulativeProvider_1.Cumulative_Sales_1 == histoCumulativeProvider_2.Cumulative_Sales_1) &&
                       (histoCumulativeProvider_1.Cumulative_Sales_2 == histoCumulativeProvider_2.Cumulative_Sales_2) &&
                       (histoCumulativeProvider_1.Cumulative_Sales_3 == histoCumulativeProvider_2.Cumulative_Sales_3) &&
                       (histoCumulativeProvider_1.Cumulative_Sales_4 == histoCumulativeProvider_2.Cumulative_Sales_4) &&
                       (histoCumulativeProvider_1.Cumulative_Sales_5 == histoCumulativeProvider_2.Cumulative_Sales_5) &&
                       (histoCumulativeProvider_1.Cumulative_Sales_6 == histoCumulativeProvider_2.Cumulative_Sales_6) &&
                       (histoCumulativeProvider_1.Cumulative_Sales_7 == histoCumulativeProvider_2.Cumulative_Sales_7) &&
                       (histoCumulativeProvider_1.Cumulative_Sales_8 == histoCumulativeProvider_2.Cumulative_Sales_8) &&
                       (histoCumulativeProvider_1.Cumulative_Sales_9 == histoCumulativeProvider_2.Cumulative_Sales_9) &&
                       (histoCumulativeProvider_1.Cumulative_Sales_10 == histoCumulativeProvider_2.Cumulative_Sales_10) &&
                       (histoCumulativeProvider_1.Cumulative_Sales_11 == histoCumulativeProvider_2.Cumulative_Sales_11) &&
                       (histoCumulativeProvider_1.Cumulative_Sales_12 == histoCumulativeProvider_2.Cumulative_Sales_12);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="provider_1">Primera instáncia a comparar.</param>
        /// <param name="provider_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(HistoCumulativeProvidersView provider_1, HistoCumulativeProvidersView provider_2)
        {
            return !(provider_1 == provider_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(Histo_Id, Provider_Id).GetHashCode());
        }

        #endregion
    }
}
