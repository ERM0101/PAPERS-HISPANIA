#region Librerias usadas por la clase

using System;
using System.Collections.Generic;
using HispaniaCompData = HispaniaComptabilitat.Data;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Class that Store the information of a CustomerOrderMovement.
    /// </summary>
    public class HistoGoodsView : IMenuView
    {
        #region Fields for Filter

        /// <summary>
        /// Store the list of fields that compose the HistoGood class
        /// </summary>
        private static Dictionary<string, string> m_Fields = null;

        /// <summary>
        /// Get the list of fields that compose the HistoGood class
        /// </summary>
        public static Dictionary<string, string> Fields
        {
            get
            {
                if (m_Fields == null)
                {
                    m_Fields = new Dictionary<string, string>
                    {
                        { "Perióde", "Year" },
                        { "Vendes Gener", "Sales_Acum_1_Str" },
                        { "Vendes Febrer", "Sales_Acum_2_Str" },
                        { "Vendes Març", "Sales_Acum_3_Str" },
                        { "Vendes Abril", "Sales_Acum_4_Str" },
                        { "Vendes Maig", "Sales_Acum_5_Str" },
                        { "Vendes Juny", "Sales_Acum_6_Str" },
                        { "Vendes Juliol", "Sales_Acum_7_Str" },
                        { "Vendes Agost", "Sales_Acum_8_Str" },
                        { "Vendes Setembre", "Sales_Acum_9_Str" },
                        { "Vendes Octubre", "Sales_Acum_10_Str" },
                        { "Vendes Novembre", "Sales_Acum_11_Str" },
                        { "Vendes Desembre", "Sales_Acum_12_Str" },
                        { "Costos Gener", "Costs_Acum_1_Str" },
                        { "Costos Febrer", "Costs_Acum_2_Str" },
                        { "Costos Març", "Costs_Acum_3_Str" },
                        { "Costos Abril", "Costs_Acum_4_Str" },
                        { "Costos Maig", "Costs_Acum_5_Str" },
                        { "Costos Juny", "Costs_Acum_6_Str" },
                        { "Costos Juliol", "Costs_Acum_7_Str" },
                        { "Costos Agost", "Costs_Acum_8_Str" },
                        { "Costos Setembre", "Costs_Acum_9_Str" },
                        { "Costos Obtubre", "Costs_Acum_10_Str" },
                        { "Costos Novembre", "Costs_Acum_11_Str" },
                        { "Costos Desembre", "Costs_Acum_12_Str" }
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
        public decimal Year { get; set; }
        public decimal Sales_Acum_1 { get; set; }
        public string Sales_Acum_1_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Sales_Acum_1, DecimalType.Currency, true);
            }
        }
        public decimal Sales_Acum_2 { get; set; }
        public string Sales_Acum_2_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Sales_Acum_2, DecimalType.Currency, true);
            }
        }
        public decimal Sales_Acum_3 { get; set; }
        public string Sales_Acum_3_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Sales_Acum_3, DecimalType.Currency, true);
            }
        }
        public decimal Sales_Acum_4 { get; set; }
        public string Sales_Acum_4_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Sales_Acum_4, DecimalType.Currency, true);
            }
        }
        public decimal Sales_Acum_5 { get; set; }
        public string Sales_Acum_5_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Sales_Acum_5, DecimalType.Currency, true);
            }
        }
        public decimal Sales_Acum_6 { get; set; }
        public string Sales_Acum_6_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Sales_Acum_6, DecimalType.Currency, true);
            }
        }
        public decimal Sales_Acum_7 { get; set; }
        public string Sales_Acum_7_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Sales_Acum_7, DecimalType.Currency, true);
            }
        }
        public decimal Sales_Acum_8 { get; set; }
        public string Sales_Acum_8_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Sales_Acum_8, DecimalType.Currency, true);
            }
        }
        public decimal Sales_Acum_9 { get; set; }
        public string Sales_Acum_9_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Sales_Acum_9, DecimalType.Currency, true);
            }
        }
        public decimal Sales_Acum_10 { get; set; }
        public string Sales_Acum_10_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Sales_Acum_10, DecimalType.Currency, true);
            }
        }
        public decimal Sales_Acum_11 { get; set; }
        public string Sales_Acum_11_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Sales_Acum_11, DecimalType.Currency, true);
            }
        }
        public decimal Sales_Acum_12 { get; set; }
        public string Sales_Acum_12_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Sales_Acum_12, DecimalType.Currency, true);
            }
        }
        public decimal Costs_Acum_1 { get; set; }
        public string Costs_Acum_1_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Costs_Acum_1, DecimalType.Currency, true);
            }
        }
        public decimal Costs_Acum_2 { get; set; }
        public string Costs_Acum_2_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Costs_Acum_2, DecimalType.Currency, true);
            }
        }
        public decimal Costs_Acum_3 { get; set; }
        public string Costs_Acum_3_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Costs_Acum_3, DecimalType.Currency, true);
            }
        }
        public decimal Costs_Acum_4 { get; set; }
        public string Costs_Acum_4_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Costs_Acum_4, DecimalType.Currency, true);
            }
        }
        public decimal Costs_Acum_5 { get; set; }
        public string Costs_Acum_5_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Costs_Acum_5, DecimalType.Currency, true);
            }
        }
        public decimal Costs_Acum_6 { get; set; }
        public string Costs_Acum_6_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Costs_Acum_6, DecimalType.Currency, true);
            }
        }
        public decimal Costs_Acum_7 { get; set; }
        public string Costs_Acum_7_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Costs_Acum_7, DecimalType.Currency, true);
            }
        }
        public decimal Costs_Acum_8 { get; set; }
        public string Costs_Acum_8_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Costs_Acum_8, DecimalType.Currency, true);
            }
        }
        public decimal Costs_Acum_9 { get; set; }
        public string Costs_Acum_9_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Costs_Acum_9, DecimalType.Currency, true);
            }
        }
        public decimal Costs_Acum_10 { get; set; }
        public string Costs_Acum_10_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Costs_Acum_10, DecimalType.Currency, true);
            }
        }
        public decimal Costs_Acum_11 { get; set; }
        public string Costs_Acum_11_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Costs_Acum_11, DecimalType.Currency, true);
            }
        }
        public decimal Costs_Acum_12 { get; set; }
        public string Costs_Acum_12_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Costs_Acum_12, DecimalType.Currency, true);
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

        #endregion

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public HistoGoodsView(int good_Id)
        {
            Histo_Id = -1;
            Year = -1;
            Sales_Acum_1 = 0;
            Sales_Acum_2 = 0;
            Sales_Acum_3 = 0;
            Sales_Acum_4 = 0;
            Sales_Acum_5 = 0;
            Sales_Acum_6 = 0;
            Sales_Acum_7 = 0;
            Sales_Acum_8 = 0;
            Sales_Acum_9 = 0;
            Sales_Acum_10 = 0;
            Sales_Acum_11 = 0;
            Sales_Acum_12 = 0;
            Costs_Acum_1 = 0;
            Costs_Acum_2 = 0;
            Costs_Acum_3 = 0;
            Costs_Acum_4 = 0;
            Costs_Acum_5 = 0;
            Costs_Acum_6 = 0;
            Costs_Acum_7 = 0;
            Costs_Acum_8 = 0;
            Costs_Acum_9 = 0;
            Costs_Acum_10 = 0;
            Costs_Acum_11 = 0;
            Costs_Acum_12 = 0;
            _Good_Id = good_Id;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HistoGoodsView(HispaniaCompData.HistoGood histoGood)
        {
            Histo_Id = histoGood.Histo_Id;
            Year = GlobalViewModel.GetDecimalValue(histoGood.Year);
            Sales_Acum_1 = GlobalViewModel.GetDecimalValue(histoGood.Sales_Acum_1);
            Sales_Acum_2 = GlobalViewModel.GetDecimalValue(histoGood.Sales_Acum_2);
            Sales_Acum_3 = GlobalViewModel.GetDecimalValue(histoGood.Sales_Acum_3);
            Sales_Acum_4 = GlobalViewModel.GetDecimalValue(histoGood.Sales_Acum_4);
            Sales_Acum_5 = GlobalViewModel.GetDecimalValue(histoGood.Sales_Acum_5);
            Sales_Acum_6 = GlobalViewModel.GetDecimalValue(histoGood.Sales_Acum_6);
            Sales_Acum_7 = GlobalViewModel.GetDecimalValue(histoGood.Sales_Acum_7);
            Sales_Acum_8 = GlobalViewModel.GetDecimalValue(histoGood.Sales_Acum_8);
            Sales_Acum_9 = GlobalViewModel.GetDecimalValue(histoGood.Sales_Acum_9);
            Sales_Acum_10 = GlobalViewModel.GetDecimalValue(histoGood.Sales_Acum_10);
            Sales_Acum_11 = GlobalViewModel.GetDecimalValue(histoGood.Sales_Acum_11);
            Sales_Acum_12 = GlobalViewModel.GetDecimalValue(histoGood.Sales_Acum_12);
            Costs_Acum_1 = GlobalViewModel.GetDecimalValue(histoGood.Costs_Acum_1);
            Costs_Acum_2 = GlobalViewModel.GetDecimalValue(histoGood.Costs_Acum_2);
            Costs_Acum_3 = GlobalViewModel.GetDecimalValue(histoGood.Costs_Acum_3);
            Costs_Acum_4 = GlobalViewModel.GetDecimalValue(histoGood.Costs_Acum_4);
            Costs_Acum_5 = GlobalViewModel.GetDecimalValue(histoGood.Costs_Acum_5);
            Costs_Acum_6 = GlobalViewModel.GetDecimalValue(histoGood.Costs_Acum_6);
            Costs_Acum_7 = GlobalViewModel.GetDecimalValue(histoGood.Costs_Acum_7);
            Costs_Acum_8 = GlobalViewModel.GetDecimalValue(histoGood.Costs_Acum_8);
            Costs_Acum_9 = GlobalViewModel.GetDecimalValue(histoGood.Costs_Acum_9);
            Costs_Acum_10 = GlobalViewModel.GetDecimalValue(histoGood.Costs_Acum_10);
            Costs_Acum_11 = GlobalViewModel.GetDecimalValue(histoGood.Costs_Acum_11);
            Costs_Acum_12 = GlobalViewModel.GetDecimalValue(histoGood.Costs_Acum_12);
            _Good_Id = histoGood.Good_Id;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public HistoGoodsView(HistoGoodsView histoGood)
        {
            Histo_Id = histoGood.Histo_Id;
            Year = histoGood.Year;
            Sales_Acum_1 = histoGood.Sales_Acum_1;
            Sales_Acum_2 = histoGood.Sales_Acum_2;
            Sales_Acum_3 = histoGood.Sales_Acum_3;
            Sales_Acum_4 = histoGood.Sales_Acum_4;
            Sales_Acum_5 = histoGood.Sales_Acum_5;
            Sales_Acum_6 = histoGood.Sales_Acum_6;
            Sales_Acum_7 = histoGood.Sales_Acum_7;
            Sales_Acum_8 = histoGood.Sales_Acum_8;
            Sales_Acum_9 = histoGood.Sales_Acum_9;
            Sales_Acum_10 = histoGood.Sales_Acum_10;
            Sales_Acum_11 = histoGood.Sales_Acum_11;
            Sales_Acum_12 = histoGood.Sales_Acum_12;
            Costs_Acum_1 = histoGood.Costs_Acum_1;
            Costs_Acum_2 = histoGood.Costs_Acum_2;
            Costs_Acum_3 = histoGood.Costs_Acum_3;
            Costs_Acum_4 = histoGood.Costs_Acum_4;
            Costs_Acum_5 = histoGood.Costs_Acum_5;
            Costs_Acum_6 = histoGood.Costs_Acum_6;
            Costs_Acum_7 = histoGood.Costs_Acum_7;
            Costs_Acum_8 = histoGood.Costs_Acum_8;
            Costs_Acum_9 = histoGood.Costs_Acum_9;
            Costs_Acum_10 = histoGood.Costs_Acum_10;
            Costs_Acum_11 = histoGood.Costs_Acum_11;
            Costs_Acum_12 = histoGood.Costs_Acum_12;
            _Good_Id = histoGood._Good_Id;
        }

        #endregion

        #region GetHistoGood

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.HistoGood GetHistoGood()
        {
            HispaniaCompData.HistoGood histoGood = new HispaniaCompData.HistoGood();
            histoGood.Histo_Id = Histo_Id;
            histoGood.Year = Year;
            histoGood.Sales_Acum_1 = Sales_Acum_1;
            histoGood.Sales_Acum_2 = Sales_Acum_2;
            histoGood.Sales_Acum_3 = Sales_Acum_3;
            histoGood.Sales_Acum_4 = Sales_Acum_4;
            histoGood.Sales_Acum_5 = Sales_Acum_5;
            histoGood.Sales_Acum_6 = Sales_Acum_6;
            histoGood.Sales_Acum_7 = Sales_Acum_7;
            histoGood.Sales_Acum_8 = Sales_Acum_8;
            histoGood.Sales_Acum_9 = Sales_Acum_9;
            histoGood.Sales_Acum_10 = Sales_Acum_10;
            histoGood.Sales_Acum_11 = Sales_Acum_11;
            histoGood.Sales_Acum_12 = Sales_Acum_12;
            histoGood.Costs_Acum_1 = Costs_Acum_1;
            histoGood.Costs_Acum_2 = Costs_Acum_2;
            histoGood.Costs_Acum_3 = Costs_Acum_3;
            histoGood.Costs_Acum_4 = Costs_Acum_4;
            histoGood.Costs_Acum_5 = Costs_Acum_5;
            histoGood.Costs_Acum_6 = Costs_Acum_6;
            histoGood.Costs_Acum_7 = Costs_Acum_7;
            histoGood.Costs_Acum_8 = Costs_Acum_8;
            histoGood.Costs_Acum_9 = Costs_Acum_9;
            histoGood.Costs_Acum_10 = Costs_Acum_10;
            histoGood.Costs_Acum_11 = Costs_Acum_11;
            histoGood.Costs_Acum_12 = Costs_Acum_12;
            histoGood.Good_Id = _Good_Id;
            return (histoGood);
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
                HistoGoodsView histoGood = obj as HistoGoodsView;
                if ((Object)histoGood == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Histo_Id == histoGood.Histo_Id) && (Year == histoGood.Year) &&
                       (Sales_Acum_1 == histoGood.Sales_Acum_1) &&
                       (Sales_Acum_2 == histoGood.Sales_Acum_2) &&
                       (Sales_Acum_3 == histoGood.Sales_Acum_3) &&
                       (Sales_Acum_4 == histoGood.Sales_Acum_4) &&
                       (Sales_Acum_5 == histoGood.Sales_Acum_5) &&
                       (Sales_Acum_6 == histoGood.Sales_Acum_6) &&
                       (Sales_Acum_7 == histoGood.Sales_Acum_7) &&
                       (Sales_Acum_8 == histoGood.Sales_Acum_8) &&
                       (Sales_Acum_9 == histoGood.Sales_Acum_9) &&
                       (Sales_Acum_10 == histoGood.Sales_Acum_10) &&
                       (Sales_Acum_11 == histoGood.Sales_Acum_11) &&
                       (Sales_Acum_12 == histoGood.Sales_Acum_12) &&
                       (Costs_Acum_1 == histoGood.Costs_Acum_1) &&
                       (Costs_Acum_2 == histoGood.Costs_Acum_2) &&
                       (Costs_Acum_3 == histoGood.Costs_Acum_3) &&
                       (Costs_Acum_4 == histoGood.Costs_Acum_4) &&
                       (Costs_Acum_5 == histoGood.Costs_Acum_5) &&
                       (Costs_Acum_6 == histoGood.Costs_Acum_6) &&
                       (Costs_Acum_7 == histoGood.Costs_Acum_7) &&
                       (Costs_Acum_8 == histoGood.Costs_Acum_8) &&
                       (Costs_Acum_9 == histoGood.Costs_Acum_9) &&
                       (Costs_Acum_10 == histoGood.Costs_Acum_10) &&
                       (Costs_Acum_11 == histoGood.Costs_Acum_11) &&
                       (Costs_Acum_12 == histoGood.Costs_Acum_12) &&
                       (_Good_Id == histoGood._Good_Id);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(HistoGoodsView histoGood)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)histoGood == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Histo_Id == histoGood.Histo_Id) && (Year == histoGood.Year) &&
                       (Sales_Acum_1 == histoGood.Sales_Acum_1) &&
                       (Sales_Acum_2 == histoGood.Sales_Acum_2) &&
                       (Sales_Acum_3 == histoGood.Sales_Acum_3) &&
                       (Sales_Acum_4 == histoGood.Sales_Acum_4) &&
                       (Sales_Acum_5 == histoGood.Sales_Acum_5) &&
                       (Sales_Acum_6 == histoGood.Sales_Acum_6) &&
                       (Sales_Acum_7 == histoGood.Sales_Acum_7) &&
                       (Sales_Acum_8 == histoGood.Sales_Acum_8) &&
                       (Sales_Acum_9 == histoGood.Sales_Acum_9) &&
                       (Sales_Acum_10 == histoGood.Sales_Acum_10) &&
                       (Sales_Acum_11 == histoGood.Sales_Acum_11) &&
                       (Sales_Acum_12 == histoGood.Sales_Acum_12) &&
                       (Costs_Acum_1 == histoGood.Costs_Acum_1) &&
                       (Costs_Acum_2 == histoGood.Costs_Acum_2) &&
                       (Costs_Acum_3 == histoGood.Costs_Acum_3) &&
                       (Costs_Acum_4 == histoGood.Costs_Acum_4) &&
                       (Costs_Acum_5 == histoGood.Costs_Acum_5) &&
                       (Costs_Acum_6 == histoGood.Costs_Acum_6) &&
                       (Costs_Acum_7 == histoGood.Costs_Acum_7) &&
                       (Costs_Acum_8 == histoGood.Costs_Acum_8) &&
                       (Costs_Acum_9 == histoGood.Costs_Acum_9) &&
                       (Costs_Acum_10 == histoGood.Costs_Acum_10) &&
                       (Costs_Acum_11 == histoGood.Costs_Acum_11) &&
                       (Costs_Acum_12 == histoGood.Costs_Acum_12) &&
                       (_Good_Id == histoGood._Good_Id);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="histoGood_1">Primera instáncia a comparar.</param>
        /// <param name="histoGood_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(HistoGoodsView histoGood_1, HistoGoodsView histoGood_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(histoGood_1, histoGood_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)histoGood_1 == null) || ((object)histoGood_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (histoGood_1.Histo_Id == histoGood_2.Histo_Id) && 
                       (histoGood_1.Year == histoGood_2.Year) &&
                       (histoGood_1.Sales_Acum_1 == histoGood_2.Sales_Acum_1) &&
                       (histoGood_1.Sales_Acum_2 == histoGood_2.Sales_Acum_2) &&
                       (histoGood_1.Sales_Acum_3 == histoGood_2.Sales_Acum_3) &&
                       (histoGood_1.Sales_Acum_4 == histoGood_2.Sales_Acum_4) &&
                       (histoGood_1.Sales_Acum_5 == histoGood_2.Sales_Acum_5) &&
                       (histoGood_1.Sales_Acum_6 == histoGood_2.Sales_Acum_6) &&
                       (histoGood_1.Sales_Acum_7 == histoGood_2.Sales_Acum_7) &&
                       (histoGood_1.Sales_Acum_8 == histoGood_2.Sales_Acum_8) &&
                       (histoGood_1.Sales_Acum_9 == histoGood_2.Sales_Acum_9) &&
                       (histoGood_1.Sales_Acum_10 == histoGood_2.Sales_Acum_10) &&
                       (histoGood_1.Sales_Acum_11 == histoGood_2.Sales_Acum_11) &&
                       (histoGood_1.Sales_Acum_12 == histoGood_2.Sales_Acum_12) &&
                       (histoGood_1.Costs_Acum_1 == histoGood_2.Costs_Acum_1) &&
                       (histoGood_1.Costs_Acum_2 == histoGood_2.Costs_Acum_2) &&
                       (histoGood_1.Costs_Acum_3 == histoGood_2.Costs_Acum_3) &&
                       (histoGood_1.Costs_Acum_4 == histoGood_2.Costs_Acum_4) &&
                       (histoGood_1.Costs_Acum_5 == histoGood_2.Costs_Acum_5) &&
                       (histoGood_1.Costs_Acum_6 == histoGood_2.Costs_Acum_6) &&
                       (histoGood_1.Costs_Acum_7 == histoGood_2.Costs_Acum_7) &&
                       (histoGood_1.Costs_Acum_8 == histoGood_2.Costs_Acum_8) &&
                       (histoGood_1.Costs_Acum_9 == histoGood_2.Costs_Acum_9) &&
                       (histoGood_1.Costs_Acum_10 == histoGood_2.Costs_Acum_10) &&
                       (histoGood_1.Costs_Acum_11 == histoGood_2.Costs_Acum_11) &&
                       (histoGood_1.Costs_Acum_12 == histoGood_2.Costs_Acum_12) &&
                       (histoGood_1._Good_Id == histoGood_2._Good_Id);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="customer_1">Primera instáncia a comparar.</param>
        /// <param name="customer_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(HistoGoodsView customer_1, HistoGoodsView customer_2)
        {
            return !(customer_1 == customer_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(Histo_Id).GetHashCode());
        }

        #endregion
    }
}
