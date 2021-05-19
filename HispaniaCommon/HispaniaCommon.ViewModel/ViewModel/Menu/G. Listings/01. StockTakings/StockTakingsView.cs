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
    public enum StockTakingnsAttributes
    {
        GoodCode,
        None
    }

    /// <summary>
    /// Class that Store the information of a StockTaking.
    /// </summary>
    public class StockTakingsView : IMenuView
    {
        #region Fields for Filter

        /// <summary>
        /// Store the list of fields that compose the WarehouseMovement class
        /// </summary>
        private static Dictionary<string, string> m_Fields = null;

        /// <summary>
        /// Get the list of fields that compose the WarehouseMovement class
        /// </summary>
        public static Dictionary<string, string> Fields
        {
            get
            {
                if (m_Fields == null)
                {
                    m_Fields = new Dictionary<string, string>
                    {
                        { "Codi Article", "GoodCode" },
                        { "Descripció", "Good_Description" },
                        { "Família", "Familia" },
                        { "Anulat", "Canceled_Str" },
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
                return GoodCode;
            }
        }

        #endregion

        #region Main Fields

        public string GoodCode { get; set; }
        public string Familia { get; set; }
        public bool Canceled { get; set; }
        public string Canceled_Str
        {
            get
            {
                return Canceled ? "Anulat" : "No anulat";
            }
        }
        public string Good_Description { get; set; }
        public Nullable<int> UnitType { get; set; }
        public Nullable<int> Minimum { get; set; }
        public decimal Price_Cost { get; set; }
        public decimal Average_Price_Cost { get; set; }
        public decimal Shipping_Unit_Stocks { get; set; }
        public decimal Billing_Unit_Stocks { get; set; }
        public decimal Expression_1 { get; set; }
        public decimal Shipping_Unit_Entrance { get; set; }
        public decimal Billing_Unit_Entrance { get; set; }
        public decimal Shipping_Unit_Departure { get; set; }
        public decimal Billing_Unit_Departure { get; set; }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public StockTakingsView()
        {
            GoodCode = null;
            Price_Cost = GlobalViewModel.DecimalInitValue;
            Average_Price_Cost = GlobalViewModel.DecimalInitValue;
            Shipping_Unit_Stocks = GlobalViewModel.DecimalInitValue;
            Billing_Unit_Stocks = GlobalViewModel.DecimalInitValue;
            Expression_1 = GlobalViewModel.DecimalInitValue;
            Shipping_Unit_Entrance = GlobalViewModel.DecimalInitValue;
            Billing_Unit_Entrance = GlobalViewModel.DecimalInitValue;
            Shipping_Unit_Departure = GlobalViewModel.DecimalInitValue;
            Billing_Unit_Departure = GlobalViewModel.DecimalInitValue;
            Familia = null;
            Canceled = false;
            Good_Description = null;
            UnitType = GlobalViewModel.IntIdInitValue;
            Minimum = GlobalViewModel.IntIdInitValue;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal StockTakingsView(HispaniaCompData.StockTaking stockTaking)
        {
            GoodCode = stockTaking.Good_Code;
            Price_Cost = GlobalViewModel.GetDecimalValue(stockTaking.Price_Cost);
            Average_Price_Cost = GlobalViewModel.GetDecimalValue(stockTaking.Average_Price_Cost);
            Shipping_Unit_Stocks = GlobalViewModel.GetDecimalValue(stockTaking.Shipping_Unit_Stocks);
            Billing_Unit_Stocks = GlobalViewModel.GetDecimalValue(stockTaking.Billing_Unit_Stocks);
            Expression_1 = GlobalViewModel.GetDecimalValue(stockTaking.Expression_1);
            Shipping_Unit_Entrance = GlobalViewModel.GetDecimalValue(stockTaking.Shipping_Unit_Entrance);
            Billing_Unit_Entrance = GlobalViewModel.GetDecimalValue(stockTaking.Billing_Unit_Entrance);
            Shipping_Unit_Departure = GlobalViewModel.GetDecimalValue(stockTaking.Shipping_Unit_Departure);
            Billing_Unit_Departure = GlobalViewModel.GetDecimalValue(stockTaking.Billing_Unit_Departure);
            Familia = stockTaking.Familia;
            Canceled = stockTaking.Canceled;
            Good_Description = stockTaking.Good_Description;
            UnitType = stockTaking.UnitType;
            Minimum = stockTaking.Minimum;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public StockTakingsView(StockTakingsView stockTaking)
        {
            GoodCode = stockTaking.GoodCode;
            Price_Cost = stockTaking.Price_Cost;
            Average_Price_Cost = stockTaking.Average_Price_Cost;
            Shipping_Unit_Stocks = stockTaking.Shipping_Unit_Stocks;
            Billing_Unit_Stocks = stockTaking.Billing_Unit_Stocks;
            Expression_1 = stockTaking.Expression_1;
            Shipping_Unit_Entrance = stockTaking.Shipping_Unit_Entrance;
            Billing_Unit_Entrance = stockTaking.Billing_Unit_Entrance;
            Shipping_Unit_Departure = stockTaking.Shipping_Unit_Departure;
            Billing_Unit_Departure = stockTaking.Billing_Unit_Departure;
            Familia = stockTaking.Familia;
            Canceled = stockTaking.Canceled;
            Good_Description = stockTaking.Good_Description;
            UnitType = stockTaking.UnitType;
            Minimum = stockTaking.Minimum;
        }

        #endregion

        #region GetStockTaking

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.StockTaking GetStockTaking()
        {
            HispaniaCompData.StockTaking StockTaking = new HispaniaCompData.StockTaking()
            {
                Good_Code = GoodCode,
                Price_Cost = Price_Cost,
                Average_Price_Cost = Average_Price_Cost,
                Shipping_Unit_Stocks = Shipping_Unit_Stocks,
                Billing_Unit_Stocks = Billing_Unit_Stocks,
                Expression_1 = Expression_1,
                Shipping_Unit_Entrance = Shipping_Unit_Entrance,
                Billing_Unit_Entrance = Billing_Unit_Entrance,
                Shipping_Unit_Departure = Shipping_Unit_Departure,
                Billing_Unit_Departure = Billing_Unit_Departure,
                Familia = Familia,
                Canceled = Canceled,
                Good_Description = Good_Description,
                UnitType = UnitType,
                Minimum = Minimum
            };
            return (StockTaking);
        }

        #endregion

        #region Validate
        
        /// <summary>
        /// Validate the data contained in the instance of the class.
        /// </summary>
        public void Validate(out StockTakingnsAttributes ErrorField)
        {
            ErrorField = StockTakingnsAttributes.None;
            if (!GlobalViewModel.IsName(GoodCode))
            {
                ErrorField = StockTakingnsAttributes.GoodCode;
                throw new FormatException("Error, el codi de l'article no pot estar buit.");
            }
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(StockTakingsView stockTaking)
        {
            GoodCode = stockTaking.GoodCode;
            Price_Cost = stockTaking.Price_Cost;
            Average_Price_Cost = stockTaking.Average_Price_Cost;
            Shipping_Unit_Stocks = stockTaking.Shipping_Unit_Stocks;
            Billing_Unit_Stocks = stockTaking.Billing_Unit_Stocks;
            Expression_1 = stockTaking.Expression_1;
            Shipping_Unit_Entrance = stockTaking.Shipping_Unit_Entrance;
            Billing_Unit_Entrance = stockTaking.Billing_Unit_Entrance;
            Shipping_Unit_Departure = stockTaking.Shipping_Unit_Departure;
            Billing_Unit_Departure = stockTaking.Billing_Unit_Departure;
            Familia = stockTaking.Familia;
            Canceled = stockTaking.Canceled;
            Good_Description = stockTaking.Good_Description;
            UnitType = stockTaking.UnitType;
            Minimum = stockTaking.Minimum;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(StockTakingsView Data, StockTakingnsAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case StockTakingnsAttributes.GoodCode:
                     GoodCode = Data.GoodCode;
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
                StockTakingsView stockTaking = obj as StockTakingsView;
                if ((Object)stockTaking == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (GoodCode == stockTaking.GoodCode) &&
                       (Price_Cost == stockTaking.Price_Cost) &&
                       (Average_Price_Cost == stockTaking.Average_Price_Cost) &&
                       (Shipping_Unit_Stocks == stockTaking.Shipping_Unit_Stocks) &&
                       (Billing_Unit_Stocks == stockTaking.Billing_Unit_Stocks) &&
                       (Expression_1 == stockTaking.Expression_1) &&
                       (Shipping_Unit_Entrance == stockTaking.Shipping_Unit_Entrance) &&
                       (Billing_Unit_Entrance == stockTaking.Billing_Unit_Entrance) &&
                       (Shipping_Unit_Departure == stockTaking.Shipping_Unit_Departure) &&
                       (Billing_Unit_Departure == stockTaking.Billing_Unit_Departure) &&
                       (Familia == stockTaking.Familia) &&
                       (Canceled == stockTaking.Canceled) &&
                       (Good_Description == stockTaking.Good_Description) &&
                       (UnitType == stockTaking.UnitType) &&
                       (Minimum == stockTaking.Minimum);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(StockTakingsView stockTaking)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)stockTaking == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (GoodCode == stockTaking.GoodCode) &&
                       (Price_Cost == stockTaking.Price_Cost) &&
                       (Average_Price_Cost == stockTaking.Average_Price_Cost) &&
                       (Shipping_Unit_Stocks == stockTaking.Shipping_Unit_Stocks) &&
                       (Billing_Unit_Stocks == stockTaking.Billing_Unit_Stocks) &&
                       (Expression_1 == stockTaking.Expression_1) &&
                       (Shipping_Unit_Entrance == stockTaking.Shipping_Unit_Entrance) &&
                       (Billing_Unit_Entrance == stockTaking.Billing_Unit_Entrance) &&
                       (Shipping_Unit_Departure == stockTaking.Shipping_Unit_Departure) &&
                       (Billing_Unit_Departure == stockTaking.Billing_Unit_Departure) &&
                       (Familia == stockTaking.Familia) &&
                       (Canceled == stockTaking.Canceled) &&
                       (Good_Description == stockTaking.Good_Description) &&
                       (UnitType == stockTaking.UnitType) &&
                       (Minimum == stockTaking.Minimum);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="warehouseMovement_1">Primera instáncia a comparar.</param>
        /// <param name="warehouseMovement_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(StockTakingsView stockTaking_1, StockTakingsView stockTaking_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(stockTaking_1, stockTaking_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)stockTaking_1 == null) || ((object)stockTaking_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (stockTaking_1.GoodCode == stockTaking_2.GoodCode) &&
                       (stockTaking_1.Price_Cost == stockTaking_2.Price_Cost) &&
                       (stockTaking_1.Average_Price_Cost == stockTaking_2.Average_Price_Cost) &&
                       (stockTaking_1.Shipping_Unit_Stocks == stockTaking_2.Shipping_Unit_Stocks) &&
                       (stockTaking_1.Billing_Unit_Stocks == stockTaking_2.Billing_Unit_Stocks) &&
                       (stockTaking_1.Expression_1 == stockTaking_2.Expression_1) &&
                       (stockTaking_1.Shipping_Unit_Entrance == stockTaking_2.Shipping_Unit_Entrance) &&
                       (stockTaking_1.Billing_Unit_Entrance == stockTaking_2.Billing_Unit_Entrance) &&
                       (stockTaking_1.Shipping_Unit_Departure == stockTaking_2.Shipping_Unit_Departure) &&
                       (stockTaking_1.Billing_Unit_Departure == stockTaking_2.Billing_Unit_Departure) &&
                       (stockTaking_1.Familia == stockTaking_2.Familia) &&
                       (stockTaking_1.Canceled == stockTaking_2.Canceled) &&
                       (stockTaking_1.Good_Description == stockTaking_2.Good_Description) &&
                       (stockTaking_1.UnitType == stockTaking_2.UnitType) &&
                       (stockTaking_1.Minimum == stockTaking_2.Minimum);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="customer_1">Primera instáncia a comparar.</param>
        /// <param name="customer_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(StockTakingsView stockTaking_1, StockTakingsView stockTaking_2)
        {
            return !(stockTaking_1 == stockTaking_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(GoodCode).GetHashCode());
        }

        #endregion
    }
}
