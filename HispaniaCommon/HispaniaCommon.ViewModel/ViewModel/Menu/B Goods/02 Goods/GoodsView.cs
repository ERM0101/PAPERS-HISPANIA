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
    public enum GoodsAttributes
    {
        Good_Code,
        Good_Description,
        Initial,
        Initial_Fact,
        Cod_Fam,
        Price_Cost,
        Average_Price_Cost,
        Previous_Average_Price_Cost,
        Billing_Unit_Stocks,
        Billing_Unit_Available,
        Shipping_Unit_Stocks,
        Shipping_Unit_Available,
        Billing_Unit_Entrance,
        Billing_Unit_Departure,
        Shipping_Unit_Entrance,
        Shipping_Unit_Departure,
        Cumulative_Sales_Retail_Price_Month,
        Cumulative_Sales_Retail_Price_Year,
        Cumulative_Sales_Cost_Month,
        Cumulative_Sales_Cost_Year,
        Conversion_Factor,
        Average_Billing_Unit,
        Minimum,
        Unit,
        None,
    }

    /// <summary>
    /// Class that Store the information of a Good.
    /// </summary>
    public class GoodsView : IMenuView
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
                        { "Família", "Cod_Fam" },
                        { "Existències Unitats de Facturació", "Billing_Unit_Stocks_Str" },
                        { "Disponibles Unitats de Facturació", "Billing_Unit_Available_Str" },
                        { "Existències Unitats d'Expedició", "Shipping_Unit_Stocks_Str" },
                        { "Disponibles Unitats d'Expedició", "Shipping_Unit_Available_Str" },
                        { "Factor de Conversió", "Conversion_Factor_Str" },
                    };
                }
                return (m_Fields);
            }
        }

        /// <summary>
        /// Get the list of fields that compose the Good class
        /// </summary>
        public static Dictionary<string, string> FieldsGoodRevisions
        {
            get
            {
                if (m_Fields == null)
                {
                    m_Fields = new Dictionary<string, string>
                    {
                        { "Article", "Good_Code" },
                        { "Descripció", "Good_Description" },
                        { "Codi d'Unitat", "UnitCode" },
                        { "Unitats d'Expedició", "Shipping" },
                        { "Unitats de Facturació", "Billing" },
                        { "Factor de Conversió", "Conversion_Factor_Str" },
                        { "Unitat Mitja de Facturació", "Average_Billing_Unit_Str" }
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
        public decimal Initial_Fact { get; set; }
        public string Cod_Fam { get; set; }
        public decimal Price_Cost { get; set; }
        public decimal Average_Price_Cost { get; set; }
        public decimal Previous_Average_Price_Cost { get; set; }
        public decimal Billing_Unit_Stocks { get; set; }
        public string Billing_Unit_Stocks_Str
        {
            get
            {
                return (GlobalViewModel.GetStringFromDecimalValue(Billing_Unit_Stocks, DecimalType.Unit));
            }
        }
        public decimal Billing_Unit_Available { get; set; }
        public string Billing_Unit_Available_Str
        {
            get
            {
                return (GlobalViewModel.GetStringFromDecimalValue(Billing_Unit_Available, DecimalType.Unit));
            }
        }
        public decimal Shipping_Unit_Stocks { get; set; }
        public string Shipping_Unit_Stocks_Str
        {
            get
            {
                return (GlobalViewModel.GetStringFromDecimalValue(Shipping_Unit_Stocks, DecimalType.Unit));
            }
        }
        public decimal Shipping_Unit_Available { get; set; }
        public string Shipping_Unit_Available_Str
        {
            get
            {
                return (GlobalViewModel.GetStringFromDecimalValue(Shipping_Unit_Available, DecimalType.Unit));
            }
        }
        public decimal Billing_Unit_Entrance { get; set; }
        public decimal Billing_Unit_Departure { get; set; }
        public decimal Shipping_Unit_Entrance { get; set; }
        public decimal Shipping_Unit_Departure { get; set; }
        public decimal Cumulative_Sales_Retail_Price_Month { get; set; }
        public decimal Cumulative_Sales_Retail_Price_Year { get; set; }
        public decimal Cumulative_Sales_Cost_Month { get; set; }
        public decimal Cumulative_Sales_Cost_Year { get; set; }
        public decimal Conversion_Factor { get; set; }
        public string Conversion_Factor_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Conversion_Factor, DecimalType.Unit);
            }
        }
        public decimal Average_Billing_Unit { get; set; }
        public string Average_Billing_Unit_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Average_Billing_Unit, DecimalType.Currency);
            }
        }
        public int Minimum { get; set; }
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
        public bool Revised { get; set; }
        public bool Canceled { get; set; }

        #endregion

        #region ForeignKey Properties

        #region Units

        private int? _Good_Unit_Id { get; set; }

        private UnitsView _Good_Unit;

        public UnitsView Good_Unit
        {
            get
            {
                if ((_Good_Unit == null) && (_Good_Unit_Id != GlobalViewModel.IntIdInitValue) && 
                    (_Good_Unit_Id != null))
                {
                    _Good_Unit = new UnitsView(GlobalViewModel.Instance.HispaniaViewModel.GetUnit((int)_Good_Unit_Id));
                }
                return (_Good_Unit);
            }
            set
            {
                if (value != null)
                {
                    _Good_Unit = new UnitsView(value);
                    if (_Good_Unit == null) _Good_Unit_Id = GlobalViewModel.IntIdInitValue;
                    else _Good_Unit_Id = _Good_Unit.Unit_Id;
                }
                else
                {
                    _Good_Unit = null;
                    _Good_Unit_Id = GlobalViewModel.IntIdInitValue;
                }
            }
        }

        public string UnitType
        {
            get
            {
                return Good_Unit is null ? "No Informat" : Good_Unit.Unit_Id.ToString();
            }
        }

        public string UnitCode
        {
            get
            {
                return Good_Unit is null ? "No Informat" : Good_Unit.Code;
            }
        }

        public string UnitShipping
        {
            get
            {
                return Good_Unit is null ? "No Informat" : Good_Unit.Shipping;
            }
        }

        public string UnitBilling
        {
            get
            {
                return Good_Unit is null ? "No Informat" : Good_Unit.Billing;
            }
        }

        #endregion

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public GoodsView()
        {
            Good_Id = -1;
            Good_Code = string.Empty;
            Initial = 0;
            Initial_Fact = 0;
            Good_Description = string.Empty;
            Cod_Fam = string.Empty;
            Price_Cost = 0;
            Average_Price_Cost = 0;
            Previous_Average_Price_Cost = 0;
            Billing_Unit_Stocks = 0;
            Billing_Unit_Available = 0;
            Shipping_Unit_Stocks = 0;
            Shipping_Unit_Available = 0;
            Billing_Unit_Entrance = 0;
            Billing_Unit_Departure = 0;
            Shipping_Unit_Entrance = 0;
            Shipping_Unit_Departure = 0;
            Cumulative_Sales_Retail_Price_Month = 0;
            Cumulative_Sales_Retail_Price_Year = 0;
            Cumulative_Sales_Cost_Month = 0;
            Cumulative_Sales_Cost_Year = 0;
            Conversion_Factor = 0;
            Average_Billing_Unit = 0;
            Minimum = 0;
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
            Revised = false;
            Canceled = false;
            Good_Unit = null;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal GoodsView(HispaniaCompData.Good good)
        {
            Good_Id = good.Good_Id;
            Good_Code = good.Good_Code;
            Initial = GlobalViewModel.GetDecimalValue(good.Initial);
            Initial_Fact = GlobalViewModel.GetDecimalValue(good.Initial_Fact);
            Good_Description = good.Good_Description;
            Cod_Fam = good.Cod_Fam;
            Price_Cost = GlobalViewModel.GetDecimalValue(good.Price_Cost);
            Average_Price_Cost = GlobalViewModel.GetDecimalValue(good.Average_Price_Cost);
            Previous_Average_Price_Cost = GlobalViewModel.GetDecimalValue(good.Previous_Average_Price_Cost);
            Billing_Unit_Stocks = GlobalViewModel.GetDecimalValue(good.Billing_Unit_Stocks);
            Billing_Unit_Available = GlobalViewModel.GetDecimalValue(good.Billing_Unit_Available);
            Shipping_Unit_Stocks = GlobalViewModel.GetDecimalValue(good.Shipping_Unit_Stocks);
            Shipping_Unit_Available = GlobalViewModel.GetDecimalValue(good.Shipping_Unit_Available);
            Billing_Unit_Entrance = GlobalViewModel.GetDecimalValue(good.Billing_Unit_Entrance);
            Billing_Unit_Departure = GlobalViewModel.GetDecimalValue(good.Billing_Unit_Departure);
            Shipping_Unit_Entrance = GlobalViewModel.GetDecimalValue(good.Shipping_Unit_Entrance);
            Shipping_Unit_Departure = GlobalViewModel.GetDecimalValue(good.Shipping_Unit_Departure);
            Cumulative_Sales_Retail_Price_Month = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Retail_Price_Month);
            Cumulative_Sales_Retail_Price_Year = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Retail_Price_Year);
            Cumulative_Sales_Cost_Month = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Cost_Month);
            Cumulative_Sales_Cost_Year = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Cost_Year);
            Conversion_Factor = GlobalViewModel.GetDecimalValue(good.Conversion_Factor);
            Average_Billing_Unit = GlobalViewModel.GetDecimalValue(good.Average_Billing_Unit);
            Minimum = GlobalViewModel.GetIntValue(good.Minimum);
            Cumulative_Sales_Retail_Price_1 = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Retail_Price_1);
            Cumulative_Sales_Retail_Price_2 = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Retail_Price_2);
            Cumulative_Sales_Retail_Price_3 = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Retail_Price_3);
            Cumulative_Sales_Retail_Price_4 = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Retail_Price_4);
            Cumulative_Sales_Retail_Price_5 = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Retail_Price_5);
            Cumulative_Sales_Retail_Price_6 = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Retail_Price_6);
            Cumulative_Sales_Retail_Price_7 = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Retail_Price_7);
            Cumulative_Sales_Retail_Price_8 = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Retail_Price_8);
            Cumulative_Sales_Retail_Price_9 = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Retail_Price_9);
            Cumulative_Sales_Retail_Price_10 = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Retail_Price_10);
            Cumulative_Sales_Retail_Price_11 = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Retail_Price_11); 
            Cumulative_Sales_Retail_Price_12 = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Retail_Price_12); 
            Cumulative_Sales_Cost_1 = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Cost_1);
            Cumulative_Sales_Cost_2 = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Cost_2);
            Cumulative_Sales_Cost_3 = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Cost_3);
            Cumulative_Sales_Cost_4 = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Cost_4);
            Cumulative_Sales_Cost_5 = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Cost_5);
            Cumulative_Sales_Cost_6 = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Cost_6);
            Cumulative_Sales_Cost_7 = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Cost_7);
            Cumulative_Sales_Cost_8 = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Cost_8);
            Cumulative_Sales_Cost_9 = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Cost_9);
            Cumulative_Sales_Cost_10 = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Cost_10); 
            Cumulative_Sales_Cost_11 = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Cost_11);
            Cumulative_Sales_Cost_12 = GlobalViewModel.GetDecimalValue(good.Cumulative_Sales_Cost_12);
            Revised = good.Revised;
            Canceled = good.Canceled;
            _Good_Unit_Id = GlobalViewModel.GetIntValue(good.Unit_Id);
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public GoodsView(GoodsView good)
        {
            Good_Id = good.Good_Id;
            Good_Code = good.Good_Code;
            Initial = good.Initial;
            Initial_Fact = good.Initial_Fact;
            Good_Description = good.Good_Description;
            Cod_Fam = good.Cod_Fam;
            Price_Cost = good.Price_Cost;
            Average_Price_Cost = good.Average_Price_Cost;
            Previous_Average_Price_Cost = good.Previous_Average_Price_Cost;
            Billing_Unit_Stocks = good.Billing_Unit_Stocks;
            Billing_Unit_Available = good.Billing_Unit_Available;
            Shipping_Unit_Stocks = good.Shipping_Unit_Stocks;
            Shipping_Unit_Available = good.Shipping_Unit_Available;
            Billing_Unit_Entrance = good.Billing_Unit_Entrance;
            Billing_Unit_Departure = good.Billing_Unit_Departure;
            Shipping_Unit_Entrance = good.Shipping_Unit_Entrance;
            Shipping_Unit_Departure = good.Shipping_Unit_Departure;
            Cumulative_Sales_Retail_Price_Month = good.Cumulative_Sales_Retail_Price_Month;
            Cumulative_Sales_Retail_Price_Year = good.Cumulative_Sales_Retail_Price_Year;
            Cumulative_Sales_Cost_Month = good.Cumulative_Sales_Cost_Month;
            Cumulative_Sales_Cost_Year = good.Cumulative_Sales_Cost_Year;
            Conversion_Factor = good.Conversion_Factor;
            Average_Billing_Unit = good.Average_Billing_Unit;
            Minimum = good.Minimum;
            Cumulative_Sales_Retail_Price_1 = good.Cumulative_Sales_Retail_Price_1;
            Cumulative_Sales_Retail_Price_2 = good.Cumulative_Sales_Retail_Price_2;
            Cumulative_Sales_Retail_Price_3 = good.Cumulative_Sales_Retail_Price_3;
            Cumulative_Sales_Retail_Price_4 = good.Cumulative_Sales_Retail_Price_4;
            Cumulative_Sales_Retail_Price_5 = good.Cumulative_Sales_Retail_Price_5;
            Cumulative_Sales_Retail_Price_6 = good.Cumulative_Sales_Retail_Price_6;
            Cumulative_Sales_Retail_Price_7 = good.Cumulative_Sales_Retail_Price_7;
            Cumulative_Sales_Retail_Price_8 = good.Cumulative_Sales_Retail_Price_8;
            Cumulative_Sales_Retail_Price_9 = good.Cumulative_Sales_Retail_Price_9;
            Cumulative_Sales_Retail_Price_10 = good.Cumulative_Sales_Retail_Price_10;
            Cumulative_Sales_Retail_Price_11 = good.Cumulative_Sales_Retail_Price_11;
            Cumulative_Sales_Retail_Price_12 = good.Cumulative_Sales_Retail_Price_12;
            Cumulative_Sales_Cost_1 = good.Cumulative_Sales_Cost_1;
            Cumulative_Sales_Cost_2 = good.Cumulative_Sales_Cost_2;
            Cumulative_Sales_Cost_3 = good.Cumulative_Sales_Cost_3;
            Cumulative_Sales_Cost_4 = good.Cumulative_Sales_Cost_4;
            Cumulative_Sales_Cost_5 = good.Cumulative_Sales_Cost_5;
            Cumulative_Sales_Cost_6 = good.Cumulative_Sales_Cost_6;
            Cumulative_Sales_Cost_7 = good.Cumulative_Sales_Cost_7;
            Cumulative_Sales_Cost_8 = good.Cumulative_Sales_Cost_8;
            Cumulative_Sales_Cost_9 = good.Cumulative_Sales_Cost_9;
            Cumulative_Sales_Cost_10 = good.Cumulative_Sales_Cost_10;
            Cumulative_Sales_Cost_11 = good.Cumulative_Sales_Cost_11;
            Cumulative_Sales_Cost_12 = good.Cumulative_Sales_Cost_12;
            Revised = good.Revised;
            Canceled = good.Canceled;
            _Good_Unit_Id = good._Good_Unit_Id;
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
                Initial = Initial,
                Initial_Fact = Initial_Fact,
                Good_Description = Good_Description,
                Cod_Fam = Cod_Fam,
                Price_Cost = Price_Cost,
                Average_Price_Cost = Average_Price_Cost,
                Previous_Average_Price_Cost = Previous_Average_Price_Cost,
                Billing_Unit_Stocks = Billing_Unit_Stocks,
                Billing_Unit_Available = Billing_Unit_Available,
                Shipping_Unit_Stocks = Shipping_Unit_Stocks,
                Shipping_Unit_Available = Shipping_Unit_Available,
                Billing_Unit_Entrance = Billing_Unit_Entrance,
                Billing_Unit_Departure = Billing_Unit_Departure,
                Shipping_Unit_Entrance = Shipping_Unit_Entrance,
                Shipping_Unit_Departure = Shipping_Unit_Departure,
                Cumulative_Sales_Retail_Price_Month = Cumulative_Sales_Retail_Price_Month,
                Cumulative_Sales_Retail_Price_Year = Cumulative_Sales_Retail_Price_Year,
                Cumulative_Sales_Cost_Month = Cumulative_Sales_Cost_Month,
                Cumulative_Sales_Cost_Year = Cumulative_Sales_Cost_Year,
                Conversion_Factor = Conversion_Factor,
                Average_Billing_Unit = Average_Billing_Unit,
                Minimum = Minimum,
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
                Cumulative_Sales_Cost_12 = Cumulative_Sales_Cost_12,
                Revised = Revised,
                Canceled = Canceled,
                Unit_Id = _Good_Unit_Id
            };
            return (good);
        }

        #endregion

        #region Validate

        public void Validate(out GoodsAttributes ErrorField)
        {
            ErrorField = GoodsAttributes.None;
            if (!GlobalViewModel.IsName(Good_Code))
            {
                ErrorField = GoodsAttributes.Good_Code;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
            if (!GlobalViewModel.IsComment(Good_Description))
            {
                ErrorField = GoodsAttributes.Good_Description;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(Initial, "Estoc d'Expedició", out string ErrMsg))
            {
                ErrorField = GoodsAttributes.Initial;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(Initial_Fact, "Estoc de Facturació", out ErrMsg))
            {
                ErrorField = GoodsAttributes.Initial_Fact;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCod_Fam(Cod_Fam))
            {
                ErrorField = GoodsAttributes.Cod_Fam;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(Price_Cost, "Preu de Cost", out ErrMsg))
            {
                ErrorField = GoodsAttributes.Price_Cost;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(Average_Price_Cost, "Preu Mig de Cost", out ErrMsg))
            {
                ErrorField = GoodsAttributes.Average_Price_Cost;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(Previous_Average_Price_Cost, "Preu Mig de Cost Previ", out ErrMsg))
            {
                ErrorField = GoodsAttributes.Previous_Average_Price_Cost;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(Billing_Unit_Stocks, "Existències Unitats de Facturació", out ErrMsg))
            {
                ErrorField = GoodsAttributes.Billing_Unit_Stocks;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(Billing_Unit_Available, "Disponibles Unitats de Facturació", out ErrMsg))
            {
                ErrorField = GoodsAttributes.Billing_Unit_Available;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(Shipping_Unit_Stocks, "Existències Unitats d'Expedició", out ErrMsg))
            {
                ErrorField = GoodsAttributes.Shipping_Unit_Stocks;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(Shipping_Unit_Available, "Disponibles Unitats d'Expedició", out ErrMsg))
            {
                ErrorField = GoodsAttributes.Shipping_Unit_Available;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(Billing_Unit_Entrance, "Entrades Unitats de Facturació", out ErrMsg))
            {
                ErrorField = GoodsAttributes.Billing_Unit_Entrance;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(Billing_Unit_Departure, "Sortides Unitats de Facturació", out ErrMsg))
            {
                ErrorField = GoodsAttributes.Billing_Unit_Departure;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(Shipping_Unit_Entrance, "Entrades Unitats d'Expedició", out ErrMsg))
            {
                ErrorField = GoodsAttributes.Shipping_Unit_Entrance;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(Shipping_Unit_Departure, "Sortides Unitats d'Expedició", out ErrMsg))
            {
                ErrorField = GoodsAttributes.Shipping_Unit_Departure;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(Cumulative_Sales_Retail_Price_Month, "Vendes Acumulades per Mes", out ErrMsg))
            {
                ErrorField = GoodsAttributes.Cumulative_Sales_Retail_Price_Month;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(Cumulative_Sales_Retail_Price_Year, "Vendes Acumulades per Any", out ErrMsg))
            {
                ErrorField = GoodsAttributes.Cumulative_Sales_Retail_Price_Year;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(Cumulative_Sales_Cost_Month, "Costos Acumulats per Mes", out ErrMsg))
            {
                ErrorField = GoodsAttributes.Cumulative_Sales_Cost_Month;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(Cumulative_Sales_Cost_Year, "Costos Acumulats per Any", out ErrMsg))
            {
                ErrorField = GoodsAttributes.Cumulative_Sales_Cost_Year;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(Conversion_Factor, "Factor de Conversió", out ErrMsg))
            {
                ErrorField = GoodsAttributes.Conversion_Factor;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(Average_Billing_Unit, "Mitjana d'Unitats de Facturació", out ErrMsg))
            {
                ErrorField = GoodsAttributes.Average_Billing_Unit;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrInt(GlobalViewModel.GetStringFromIntValue(Minimum), "Mínim", out ErrMsg))
            {
                ErrorField = GoodsAttributes.Minimum;
                throw new FormatException(ErrMsg);
            }
            if (Good_Unit is null)
            {
                ErrorField = GoodsAttributes.Unit;
                throw new FormatException("Error, manca seleccionar la unitat associada a l'Article.");
            }
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(GoodsView Data)
        {
            Good_Code = Data.Good_Code;
            Good_Description = Data.Good_Description;
            Initial = Data.Initial;
            Initial_Fact = Data.Initial_Fact;
            Cod_Fam = Data.Cod_Fam;
            Price_Cost = Data.Price_Cost;
            Average_Price_Cost = Data.Average_Price_Cost;
            Previous_Average_Price_Cost = Data.Previous_Average_Price_Cost;
            Billing_Unit_Stocks = Data.Billing_Unit_Stocks;
            Billing_Unit_Available = Data.Billing_Unit_Available;
            Shipping_Unit_Stocks = Data.Shipping_Unit_Stocks;
            Shipping_Unit_Available = Data.Shipping_Unit_Available;
            Billing_Unit_Entrance = Data.Billing_Unit_Entrance;
            Billing_Unit_Departure = Data.Billing_Unit_Departure;
            Shipping_Unit_Entrance = Data.Shipping_Unit_Entrance;
            Shipping_Unit_Departure = Data.Shipping_Unit_Departure;
            Cumulative_Sales_Retail_Price_Month = Data.Cumulative_Sales_Retail_Price_Month;
            Cumulative_Sales_Retail_Price_Year = Data.Cumulative_Sales_Retail_Price_Year;
            Cumulative_Sales_Cost_Month = Data.Cumulative_Sales_Cost_Month;
            Cumulative_Sales_Cost_Year = Data.Cumulative_Sales_Cost_Year;
            Conversion_Factor = Data.Conversion_Factor;
            Average_Billing_Unit = Data.Average_Billing_Unit;
            Good_Unit = Data.Good_Unit;
            Minimum = Data.Minimum;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(GoodsView Data, GoodsAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case GoodsAttributes.Good_Code:
                     Good_Code = Data.Good_Code;
                     break;
                case GoodsAttributes.Good_Description:
                     Good_Description = Data.Good_Description;
                     break;
                case GoodsAttributes.Initial:
                     Initial = Data.Initial;
                     break;
                case GoodsAttributes.Initial_Fact:
                     Initial_Fact = Data.Initial_Fact;
                     break;
                case GoodsAttributes.Cod_Fam:
                     Cod_Fam = Data.Cod_Fam;
                     break;
                case GoodsAttributes.Price_Cost:
                     Price_Cost = Data.Price_Cost;
                     break;
                case GoodsAttributes.Average_Price_Cost:
                     Average_Price_Cost = Data.Average_Price_Cost;
                     break;
                case GoodsAttributes.Previous_Average_Price_Cost:
                     Previous_Average_Price_Cost = Data.Previous_Average_Price_Cost;
                     break;
                case GoodsAttributes.Billing_Unit_Stocks:
                     Billing_Unit_Stocks = Data.Billing_Unit_Stocks;
                     break;
                case GoodsAttributes.Billing_Unit_Available:
                     Billing_Unit_Available = Data.Billing_Unit_Available;
                     break;
                case GoodsAttributes.Shipping_Unit_Stocks:
                     Shipping_Unit_Stocks = Data.Shipping_Unit_Stocks;
                     break;
                case GoodsAttributes.Shipping_Unit_Available:
                     Shipping_Unit_Available = Data.Shipping_Unit_Available;
                     break;
                case GoodsAttributes.Billing_Unit_Entrance:
                     Billing_Unit_Entrance = Data.Billing_Unit_Entrance;
                     break;
                case GoodsAttributes.Billing_Unit_Departure:
                     Billing_Unit_Departure = Data.Billing_Unit_Departure;
                     break;
                case GoodsAttributes.Shipping_Unit_Entrance:
                     Shipping_Unit_Entrance = Data.Shipping_Unit_Entrance;
                     break;
                case GoodsAttributes.Shipping_Unit_Departure:
                     Shipping_Unit_Departure = Data.Shipping_Unit_Departure;
                     break;
                case GoodsAttributes.Cumulative_Sales_Retail_Price_Month:
                     Cumulative_Sales_Retail_Price_Month = Data.Cumulative_Sales_Retail_Price_Month;
                     break;
                case GoodsAttributes.Cumulative_Sales_Retail_Price_Year:
                     Cumulative_Sales_Retail_Price_Year = Data.Cumulative_Sales_Retail_Price_Year;
                     break;
                case GoodsAttributes.Cumulative_Sales_Cost_Month:
                     Cumulative_Sales_Cost_Month = Data.Cumulative_Sales_Cost_Month;
                     break;
                case GoodsAttributes.Cumulative_Sales_Cost_Year:
                     Cumulative_Sales_Cost_Year = Data.Cumulative_Sales_Cost_Year;
                     break;
                case GoodsAttributes.Conversion_Factor:
                     Conversion_Factor = Data.Conversion_Factor;
                     break;
                case GoodsAttributes.Average_Billing_Unit:
                     Average_Billing_Unit = Data.Average_Billing_Unit;
                     break;
                case GoodsAttributes.Minimum:
                     Minimum = Data.Minimum;
                     break;
                case GoodsAttributes.Unit:
                     Good_Unit = Data.Good_Unit;
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
                GoodsView good = obj as GoodsView;
                if ((Object)good == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return  (Good_Id == good.Good_Id) && (Good_Code == good.Good_Code) && (Initial == good.Initial) &&
                        (Initial_Fact == good.Initial_Fact) && (Good_Description == good.Good_Description) &&
                        (Cod_Fam == good.Cod_Fam) && (Price_Cost == good.Price_Cost) &&
                        (Average_Price_Cost == good.Average_Price_Cost) &&
                        (Previous_Average_Price_Cost == good.Previous_Average_Price_Cost) &&
                        (Billing_Unit_Stocks == good.Billing_Unit_Stocks) &&
                        (Billing_Unit_Available == good.Billing_Unit_Available) &&
                        (Shipping_Unit_Stocks == good.Shipping_Unit_Stocks) &&
                        (Shipping_Unit_Available == good.Shipping_Unit_Available) &&
                        (Billing_Unit_Entrance == good.Billing_Unit_Entrance) &&
                        (Billing_Unit_Departure == good.Billing_Unit_Departure) &&
                        (Shipping_Unit_Entrance == good.Shipping_Unit_Entrance) &&
                        (Shipping_Unit_Departure == good.Shipping_Unit_Departure) &&
                        (Cumulative_Sales_Retail_Price_Month == good.Cumulative_Sales_Retail_Price_Month) &&
                        (Cumulative_Sales_Retail_Price_Year == good.Cumulative_Sales_Retail_Price_Year) &&
                        (Cumulative_Sales_Cost_Month == good.Cumulative_Sales_Cost_Month) &&
                        (Cumulative_Sales_Cost_Year == good.Cumulative_Sales_Cost_Year) &&
                        (Conversion_Factor == good.Conversion_Factor) &&
                        (Average_Billing_Unit == good.Average_Billing_Unit) &&
                        (_Good_Unit_Id == good._Good_Unit_Id) && (Minimum == good.Minimum) &&
                        (Cumulative_Sales_Retail_Price_1 == good.Cumulative_Sales_Retail_Price_1) &&
                        (Cumulative_Sales_Retail_Price_2 == good.Cumulative_Sales_Retail_Price_2) &&
                        (Cumulative_Sales_Retail_Price_3 == good.Cumulative_Sales_Retail_Price_3) &&
                        (Cumulative_Sales_Retail_Price_4 == good.Cumulative_Sales_Retail_Price_4) &&
                        (Cumulative_Sales_Retail_Price_5 == good.Cumulative_Sales_Retail_Price_5) &&
                        (Cumulative_Sales_Retail_Price_6 == good.Cumulative_Sales_Retail_Price_6) &&
                        (Cumulative_Sales_Retail_Price_7 == good.Cumulative_Sales_Retail_Price_7) &&
                        (Cumulative_Sales_Retail_Price_8 == good.Cumulative_Sales_Retail_Price_8) &&
                        (Cumulative_Sales_Retail_Price_9 == good.Cumulative_Sales_Retail_Price_9) &&
                        (Cumulative_Sales_Retail_Price_10 == good.Cumulative_Sales_Retail_Price_10) &&
                        (Cumulative_Sales_Retail_Price_11 == good.Cumulative_Sales_Retail_Price_11) &&
                        (Cumulative_Sales_Retail_Price_12 == good.Cumulative_Sales_Retail_Price_12) &&
                        (Cumulative_Sales_Cost_1 == good.Cumulative_Sales_Cost_1) &&
                        (Cumulative_Sales_Cost_2 == good.Cumulative_Sales_Cost_2) &&
                        (Cumulative_Sales_Cost_3 == good.Cumulative_Sales_Cost_3) &&
                        (Cumulative_Sales_Cost_4 == good.Cumulative_Sales_Cost_4) &&
                        (Cumulative_Sales_Cost_5 == good.Cumulative_Sales_Cost_5) &&
                        (Cumulative_Sales_Cost_6 == good.Cumulative_Sales_Cost_6) &&
                        (Cumulative_Sales_Cost_7 == good.Cumulative_Sales_Cost_7) &&
                        (Cumulative_Sales_Cost_8 == good.Cumulative_Sales_Cost_8) &&
                        (Cumulative_Sales_Cost_9 == good.Cumulative_Sales_Cost_9) &&
                        (Cumulative_Sales_Cost_10 == good.Cumulative_Sales_Cost_10) &&
                        (Cumulative_Sales_Cost_11 == good.Cumulative_Sales_Cost_11) &&
                        (Cumulative_Sales_Cost_12 == good.Cumulative_Sales_Cost_12) &&
                        (Revised == good.Revised) && (Canceled == good.Canceled);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(GoodsView good)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)good == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return  (Good_Id == good.Good_Id) && (Good_Code == good.Good_Code) && (Initial == good.Initial) &&
                        (Initial_Fact == good.Initial_Fact) && (Good_Description == good.Good_Description) &&
                        (Cod_Fam == good.Cod_Fam) && (Price_Cost == good.Price_Cost) &&
                        (Average_Price_Cost == good.Average_Price_Cost) &&
                        (Previous_Average_Price_Cost == good.Previous_Average_Price_Cost) &&
                        (Billing_Unit_Stocks == good.Billing_Unit_Stocks) &&
                        (Billing_Unit_Available == good.Billing_Unit_Available) &&
                        (Shipping_Unit_Stocks == good.Shipping_Unit_Stocks) &&
                        (Shipping_Unit_Available == good.Shipping_Unit_Available) &&
                        (Billing_Unit_Entrance == good.Billing_Unit_Entrance) &&
                        (Billing_Unit_Departure == good.Billing_Unit_Departure) &&
                        (Shipping_Unit_Entrance == good.Shipping_Unit_Entrance) &&
                        (Shipping_Unit_Departure == good.Shipping_Unit_Departure) &&
                        (Cumulative_Sales_Retail_Price_Month == good.Cumulative_Sales_Retail_Price_Month) &&
                        (Cumulative_Sales_Retail_Price_Year == good.Cumulative_Sales_Retail_Price_Year) &&
                        (Cumulative_Sales_Cost_Month == good.Cumulative_Sales_Cost_Month) &&
                        (Cumulative_Sales_Cost_Year == good.Cumulative_Sales_Cost_Year) &&
                        (Conversion_Factor == good.Conversion_Factor) &&
                        (Average_Billing_Unit == good.Average_Billing_Unit) &&
                        (_Good_Unit_Id == good._Good_Unit_Id) && (Minimum == good.Minimum) &&
                        (Cumulative_Sales_Retail_Price_1 == good.Cumulative_Sales_Retail_Price_1) &&
                        (Cumulative_Sales_Retail_Price_2 == good.Cumulative_Sales_Retail_Price_2) &&
                        (Cumulative_Sales_Retail_Price_3 == good.Cumulative_Sales_Retail_Price_3) &&
                        (Cumulative_Sales_Retail_Price_4 == good.Cumulative_Sales_Retail_Price_4) &&
                        (Cumulative_Sales_Retail_Price_5 == good.Cumulative_Sales_Retail_Price_5) &&
                        (Cumulative_Sales_Retail_Price_6 == good.Cumulative_Sales_Retail_Price_6) &&
                        (Cumulative_Sales_Retail_Price_7 == good.Cumulative_Sales_Retail_Price_7) &&
                        (Cumulative_Sales_Retail_Price_8 == good.Cumulative_Sales_Retail_Price_8) &&
                        (Cumulative_Sales_Retail_Price_9 == good.Cumulative_Sales_Retail_Price_9) &&
                        (Cumulative_Sales_Retail_Price_10 == good.Cumulative_Sales_Retail_Price_10) &&
                        (Cumulative_Sales_Retail_Price_11 == good.Cumulative_Sales_Retail_Price_11) &&
                        (Cumulative_Sales_Retail_Price_12 == good.Cumulative_Sales_Retail_Price_12) &&
                        (Cumulative_Sales_Cost_1 == good.Cumulative_Sales_Cost_1) &&
                        (Cumulative_Sales_Cost_2 == good.Cumulative_Sales_Cost_2) &&
                        (Cumulative_Sales_Cost_3 == good.Cumulative_Sales_Cost_3) &&
                        (Cumulative_Sales_Cost_4 == good.Cumulative_Sales_Cost_4) &&
                        (Cumulative_Sales_Cost_5 == good.Cumulative_Sales_Cost_5) &&
                        (Cumulative_Sales_Cost_6 == good.Cumulative_Sales_Cost_6) &&
                        (Cumulative_Sales_Cost_7 == good.Cumulative_Sales_Cost_7) &&
                        (Cumulative_Sales_Cost_8 == good.Cumulative_Sales_Cost_8) &&
                        (Cumulative_Sales_Cost_9 == good.Cumulative_Sales_Cost_9) &&
                        (Cumulative_Sales_Cost_10 == good.Cumulative_Sales_Cost_10) &&
                        (Cumulative_Sales_Cost_11 == good.Cumulative_Sales_Cost_11) &&
                        (Cumulative_Sales_Cost_12 == good.Cumulative_Sales_Cost_12) &&
                        (Revised == good.Revised) && (Canceled == good.Canceled);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="good_1">Primera instáncia a comparar.</param>
        /// <param name="good_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(GoodsView good_1, GoodsView good_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(good_1, good_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)good_1 == null) || ((object)good_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return  (good_1.Good_Id == good_2.Good_Id) && (good_1.Good_Code == good_2.Good_Code) && (good_1.Initial == good_2.Initial) &&
                        (good_1.Initial_Fact == good_2.Initial_Fact) && (good_1.Good_Description == good_2.Good_Description) &&
                        (good_1.Cod_Fam == good_2.Cod_Fam) && (good_1.Price_Cost == good_2.Price_Cost) &&
                        (good_1.Average_Price_Cost == good_2.Average_Price_Cost) &&
                        (good_1.Previous_Average_Price_Cost == good_2.Previous_Average_Price_Cost) &&
                        (good_1.Billing_Unit_Stocks == good_2.Billing_Unit_Stocks) &&
                        (good_1.Billing_Unit_Available == good_2.Billing_Unit_Available) &&
                        (good_1.Shipping_Unit_Stocks == good_2.Shipping_Unit_Stocks) &&
                        (good_1.Shipping_Unit_Available == good_2.Shipping_Unit_Available) &&
                        (good_1.Billing_Unit_Entrance == good_2.Billing_Unit_Entrance) &&
                        (good_1.Billing_Unit_Departure == good_2.Billing_Unit_Departure) &&
                        (good_1.Shipping_Unit_Entrance == good_2.Shipping_Unit_Entrance) &&
                        (good_1.Shipping_Unit_Departure == good_2.Shipping_Unit_Departure) &&
                        (good_1.Cumulative_Sales_Retail_Price_Month == good_2.Cumulative_Sales_Retail_Price_Month) &&
                        (good_1.Cumulative_Sales_Retail_Price_Year == good_2.Cumulative_Sales_Retail_Price_Year) &&
                        (good_1.Cumulative_Sales_Cost_Month == good_2.Cumulative_Sales_Cost_Month) &&
                        (good_1.Cumulative_Sales_Cost_Year == good_2.Cumulative_Sales_Cost_Year) &&
                        (good_1.Conversion_Factor == good_2.Conversion_Factor) &&
                        (good_1.Average_Billing_Unit == good_2.Average_Billing_Unit) &&
                        (good_1._Good_Unit_Id == good_2._Good_Unit_Id) && (good_1.Minimum == good_2.Minimum) &&
                        (good_1.Cumulative_Sales_Retail_Price_1 == good_2.Cumulative_Sales_Retail_Price_1) &&
                        (good_1.Cumulative_Sales_Retail_Price_2 == good_2.Cumulative_Sales_Retail_Price_2) &&
                        (good_1.Cumulative_Sales_Retail_Price_3 == good_2.Cumulative_Sales_Retail_Price_3) &&
                        (good_1.Cumulative_Sales_Retail_Price_4 == good_2.Cumulative_Sales_Retail_Price_4) &&
                        (good_1.Cumulative_Sales_Retail_Price_5 == good_2.Cumulative_Sales_Retail_Price_5) &&
                        (good_1.Cumulative_Sales_Retail_Price_6 == good_2.Cumulative_Sales_Retail_Price_6) &&
                        (good_1.Cumulative_Sales_Retail_Price_7 == good_2.Cumulative_Sales_Retail_Price_7) &&
                        (good_1.Cumulative_Sales_Retail_Price_8 == good_2.Cumulative_Sales_Retail_Price_8) &&
                        (good_1.Cumulative_Sales_Retail_Price_9 == good_2.Cumulative_Sales_Retail_Price_9) &&
                        (good_1.Cumulative_Sales_Retail_Price_10 == good_2.Cumulative_Sales_Retail_Price_10) &&
                        (good_1.Cumulative_Sales_Retail_Price_11 == good_2.Cumulative_Sales_Retail_Price_11) &&
                        (good_1.Cumulative_Sales_Retail_Price_12 == good_2.Cumulative_Sales_Retail_Price_12) &&
                        (good_1.Cumulative_Sales_Cost_1 == good_2.Cumulative_Sales_Cost_1) &&
                        (good_1.Cumulative_Sales_Cost_2 == good_2.Cumulative_Sales_Cost_2) &&
                        (good_1.Cumulative_Sales_Cost_3 == good_2.Cumulative_Sales_Cost_3) &&
                        (good_1.Cumulative_Sales_Cost_4 == good_2.Cumulative_Sales_Cost_4) &&
                        (good_1.Cumulative_Sales_Cost_5 == good_2.Cumulative_Sales_Cost_5) &&
                        (good_1.Cumulative_Sales_Cost_6 == good_2.Cumulative_Sales_Cost_6) &&
                        (good_1.Cumulative_Sales_Cost_7 == good_2.Cumulative_Sales_Cost_7) &&
                        (good_1.Cumulative_Sales_Cost_8 == good_2.Cumulative_Sales_Cost_8) &&
                        (good_1.Cumulative_Sales_Cost_9 == good_2.Cumulative_Sales_Cost_9) &&
                        (good_1.Cumulative_Sales_Cost_10 == good_2.Cumulative_Sales_Cost_10) &&
                        (good_1.Cumulative_Sales_Cost_11 == good_2.Cumulative_Sales_Cost_11) &&
                        (good_1.Cumulative_Sales_Cost_12 == good_2.Cumulative_Sales_Cost_12) &&
                        (good_1.Revised == good_2.Revised) && (good_1.Canceled == good_2.Canceled);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="customer_1">Primera instáncia a comparar.</param>
        /// <param name="customer_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(GoodsView customer_1, GoodsView customer_2)
        {
            return !(customer_1 == customer_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(Good_Id, _Good_Unit_Id).GetHashCode());
        }

        #endregion
    }
}
