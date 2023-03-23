#region Librerias usadas por la clase

using System;
using System.Collections.Generic;
using HispaniaCompData = HispaniaComptabilitat.Data;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Class that Store the information of a HistoCustomer.
    /// </summary>
    public class HistoCustomersView : IMenuView
    {
        #region Fields for Filter

        /// <summary>
        /// Store the list of fields that compose the HistoCustomer class
        /// </summary>
        private static Dictionary<string, string> m_Fields = null;

        /// <summary>
        /// Get the list of fields that compose the HistoCustomer class
        /// </summary>
        public static Dictionary<string, string> Fields
        {
            get
            {
                if (m_Fields == null)
                {
                    m_Fields = new Dictionary<string, string>
                    {
                        { "Numero de Factura", "Bill_Id_Str" },
                        { "Any de la Factura", "Bill_Year_Str" },
                        { "Sèrie de Facturació de la Factura", "Bill_Serie_Str" },
                        { "Data de la Factura", "Bill_Date_Str" },
                        { "Numero d'Albarà", "DeliveryNote_Id_Str" },
                        { "Any de l'Albarà", "DeliveryNote_Year_Str" },
                        { "Data de l'Albarà", "DeliveryNote_Date_Str" },
                        { "Codi d'Article", "Good_Code" },
                        { "Descripció de l'Article", "Good_Description" },
                        { "Unitat d'Expedició", "Shipping_Units_Str" },
                        { "Unitat de Facturació", "Billing_Units_Str" },
                        { "Preu Venta al Public", "Retail_Price_Str" },
                        { "Percentatge Comissió", "Comission_Str" }
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
                return GlobalViewModel.GetStringFromIntIdValue(HistoCustomer_Id);
            }
        }

        #endregion

        #region Main Fields

        public int HistoCustomer_Id { get; set; }
        public DateTime Bill_Date { get; set; }
        public string Bill_Date_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDateTimeValue(Bill_Date);
            }
        }
        public DateTime DeliveryNote_Date { get; set; }
        public string DeliveryNote_Date_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDateTimeValue(DeliveryNote_Date);
            }
        }
        public decimal Shipping_Units { get; set; }
        public string Shipping_Units_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Shipping_Units, DecimalType.Unit);
            }
        }
        public decimal Billing_Units { get; set; }
        public string Billing_Units_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Billing_Units, DecimalType.Unit);
            }
        }
        public decimal Retail_Price { get; set; }
        public string Retail_Price_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Retail_Price, DecimalType.Currency, true);
            }
        }
        public decimal Comission { get; set; }
        public string Comission_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Comission, DecimalType.Percent, true);
            }
        }
        public string Good_Code { get; set; }
        public string Good_Description { get; set; }
        public string Unit_Shipping_Definition { get; set; }
        public string Unit_Billing_Definition { get; set; }
        public decimal Total_Price
        {
            get
            {
                return Retail_Price * Billing_Units;
            }
        }
        public string Total_Price_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Total_Price, DecimalType.Currency, true);
            }
        }

        #endregion

        #region ForeignKey Properties

        #region Customer

        public int Customer_Id
        {
            get;
            private set;
        }

        #endregion

        #region Customer Order Movement

        public int CustomerOrderMovement_Id
        {
            get;
            private set;
        }

        #endregion

        #region  Bill

        public int Bill_Id
        {
            get;
            private set;
        }
        public string Bill_Id_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromIntIdValue(Bill_Id);
            }
        }
        public decimal Bill_Year
        {
            get;
            private set;
        }
        public string Bill_Year_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromYearValue(Bill_Year);
            }
        }
        private int _Bill_Serie_Id { get; set; }

        private BillingSeriesView _Bill_Serie;

        public BillingSeriesView Bill_Serie
        {
            get
            {
                if ((_Bill_Serie == null) && (_Bill_Serie_Id != GlobalViewModel.IntIdInitValue))
                {
                    _Bill_Serie = new BillingSeriesView(GlobalViewModel.Instance.HispaniaViewModel.GetBillingSerie((int)_Bill_Serie_Id));
                }
                return (_Bill_Serie);
            }
            set
            {
                if (value != null)
                {
                    _Bill_Serie = new BillingSeriesView(value);
                    if (_Bill_Serie == null) _Bill_Serie_Id = GlobalViewModel.IntIdInitValue;
                    else _Bill_Serie_Id = _Bill_Serie.Serie_Id;
                }
                else
                {
                    _Bill_Serie = null;
                    _Bill_Serie_Id = GlobalViewModel.IntIdInitValue;
                }
            }
        }

        public string Bill_Serie_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromBillingSerieValue(Bill_Serie);
            }
        }

        #endregion

        #region  DeliveryNote

        public int DeliveryNote_Id
        {
            get;
            private set;
        }
        public string DeliveryNote_Id_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromIntIdValue(DeliveryNote_Id);
            }
        }
        public decimal DeliveryNote_Year
        {
            get;
            private set;
        }
        public string DeliveryNote_Year_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromYearValue(DeliveryNote_Year);
            }
        }

        #endregion

        #region Good

        private int? _Good_Id { get; set; }

        private GoodsView _Good;

        public GoodsView Good
        {
            get
            {
                if ((_Good == null) && (_Good_Id != GlobalViewModel.IntIdInitValue) && (_Good_Id != null))
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
        public string Good_ShippingUnitStocks_Str
        {
            get
            {
                return (Good == null) ? "No Informat" : Good.Shipping_Unit_Stocks.ToString();
            }
        }
        public string Good_BillingUnitStocks_Str
        {
            get
            {
                return (Good == null) ? "No Informat" : Good.Billing_Unit_Stocks.ToString();
            }
        }
        public string Good_ShippingUnitAvailable_Str
        {
            get
            {
                return (Good == null) ? "No Informat" : Good.Shipping_Unit_Available.ToString();
            }
        }
        public string Good_BillingUnitAvailable_Str
        {
            get
            {
                return (Good == null) ? "No Informat" : Good.Billing_Unit_Available.ToString();
            }
        }

        #endregion

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public HistoCustomersView(int customer_Id)
        {
            HistoCustomer_Id = GlobalViewModel.IntIdInitValue;
            Customer_Id = customer_Id;
            Bill_Id = GlobalViewModel.IntIdInitValue;
            Bill_Year = GlobalViewModel.YearInitValue;
            Bill_Date = GlobalViewModel.DateTimeInitValue;
            DeliveryNote_Id = GlobalViewModel.IntIdInitValue;
            DeliveryNote_Year = GlobalViewModel.YearInitValue;
            DeliveryNote_Date = GlobalViewModel.DateTimeInitValue;
            Shipping_Units = GlobalViewModel.YearInitValue;
            Billing_Units = GlobalViewModel.YearInitValue;
            Retail_Price = GlobalViewModel.YearInitValue;
            Comission = GlobalViewModel.YearInitValue;
            CustomerOrderMovement_Id = GlobalViewModel.IntIdInitValue;
            Unit_Shipping_Definition = string.Empty;
            Unit_Billing_Definition = string.Empty;
            Good_Code = null;
            Good_Description = null;
            Bill_Serie = null;
            Good = null;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HistoCustomersView(HispaniaCompData.HistoCustomer histoCustomer)
        {
            HistoCustomer_Id = histoCustomer.HistoCustomer_Id;
            Customer_Id = histoCustomer.Customer_Id;
            Bill_Id = GlobalViewModel.GetIntFromIntIdValue(histoCustomer.Bill_Id);
            Bill_Year = GlobalViewModel.GetDecimalYearValue(histoCustomer.Bill_Year);
            _Bill_Serie_Id = GlobalViewModel.GetIntFromIntIdValue(histoCustomer.Bill_Serie_Id);
            Bill_Date = GlobalViewModel.GetDateTimeValue(histoCustomer.Bill_Date);
            DeliveryNote_Id = GlobalViewModel.GetIntFromIntIdValue(histoCustomer.DeliveryNote_Id);
            DeliveryNote_Year = GlobalViewModel.GetDecimalYearValue(histoCustomer.DeliveryNote_Year);
            DeliveryNote_Date = GlobalViewModel.GetDateTimeValue(histoCustomer.DeliveryNote_Date);
            Shipping_Units = GlobalViewModel.GetDecimalValue(histoCustomer.Shipping_Units);
            Billing_Units = GlobalViewModel.GetDecimalValue(histoCustomer.Billing_Units);
            Retail_Price = GlobalViewModel.GetDecimalValue(histoCustomer.Retail_Price);
            Comission = GlobalViewModel.GetDecimalValue(histoCustomer.Comission);
            Unit_Shipping_Definition = histoCustomer.Unit_Shipping_Definition;
            Unit_Billing_Definition = histoCustomer.Unit_Billing_Definition;
            CustomerOrderMovement_Id = GlobalViewModel.GetIntFromIntIdValue(histoCustomer.CustomerOrderMovement_Id);
            Good_Code = histoCustomer.Good_Code;
            Good_Description = histoCustomer.Good_Description;
            _Good_Id = histoCustomer.Good_Id;
        }
        
        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public HistoCustomersView(CustomerOrderMovementsView CustomerOrderMovement)
        {
            HistoCustomer_Id = GlobalViewModel.IntIdInitValue; // Ja s'omple al crear el registre a la Base de Dades
            Customer_Id = CustomerOrderMovement.CustomerOrder.Customer.Customer_Id;
            Bill_Id = GlobalViewModel.IntIdInitValue;          // Ja s'omple al crear el registre a la Base de Dades
            Bill_Year = GlobalViewModel.YearInitValue;         // Ja s'omple al crear el registre a la Base de Dades
            Bill_Date = GlobalViewModel.DateTimeInitValue;     // Ja s'omple al crear el registre a la Base de Dades
            DeliveryNote_Id = CustomerOrderMovement.CustomerOrder.DeliveryNote_Id;
            DeliveryNote_Year = CustomerOrderMovement.CustomerOrder.DeliveryNote_Year;
            DeliveryNote_Date = CustomerOrderMovement.CustomerOrder.DeliveryNote_Date;
            Shipping_Units = CustomerOrderMovement.Unit_Shipping;
            Billing_Units = CustomerOrderMovement.Unit_Billing;
            Retail_Price = CustomerOrderMovement.RetailPrice;
            Comission = CustomerOrderMovement.Comission;
            Unit_Shipping_Definition = CustomerOrderMovement.Unit_Shipping_Definition;
            Unit_Billing_Definition = CustomerOrderMovement.Unit_Billing_Definition;
            CustomerOrderMovement_Id = CustomerOrderMovement.CustomerOrderMovement_Id;
            _Good_Id = CustomerOrderMovement.Good.Good_Id;
            Good_Code = CustomerOrderMovement.Good.Good_Code;
            Good_Description = String.IsNullOrEmpty(CustomerOrderMovement.Description) ? CustomerOrderMovement.Good.Good_Description : CustomerOrderMovement.Description;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public HistoCustomersView(HistoCustomersView histoCustomer)
        {
            HistoCustomer_Id = histoCustomer.HistoCustomer_Id;
            Customer_Id = histoCustomer.Customer_Id;
            Bill_Id = histoCustomer.Bill_Id;
            Bill_Year = histoCustomer.Bill_Year;
            _Bill_Serie_Id = histoCustomer._Bill_Serie_Id;
            Bill_Date = histoCustomer.Bill_Date;
            DeliveryNote_Id = histoCustomer.DeliveryNote_Id;
            DeliveryNote_Year = histoCustomer.DeliveryNote_Year;
            DeliveryNote_Date = histoCustomer.DeliveryNote_Date;
            Shipping_Units = histoCustomer.Shipping_Units;
            Billing_Units = histoCustomer.Billing_Units;
            Retail_Price = histoCustomer.Retail_Price;
            Comission = histoCustomer.Comission;
            Unit_Shipping_Definition = histoCustomer.Unit_Shipping_Definition;
            Unit_Billing_Definition = histoCustomer.Unit_Billing_Definition;
            CustomerOrderMovement_Id = histoCustomer.CustomerOrderMovement_Id;
            Good_Code = histoCustomer.Good_Code;
            Good_Description = histoCustomer.Good_Description;
            _Good_Id = histoCustomer._Good_Id;
        }

        #endregion

        #region GetHistoCustomer

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.HistoCustomer GetHistoCustomer()
        {
            HispaniaCompData.HistoCustomer histoCustomer = new HispaniaCompData.HistoCustomer()
            {
                HistoCustomer_Id = HistoCustomer_Id,
                Customer_Id = Customer_Id,
                Bill_Id = Bill_Id,
                Bill_Year = Bill_Year,
                Bill_Serie_Id = _Bill_Serie_Id,
                Bill_Date = Bill_Date,
                DeliveryNote_Id = DeliveryNote_Id,
                DeliveryNote_Year = DeliveryNote_Year,
                DeliveryNote_Date = DeliveryNote_Date,
                Shipping_Units = Shipping_Units,
                Billing_Units = Billing_Units,
                Retail_Price = Retail_Price,
                Comission = Comission,
                Unit_Shipping_Definition = Unit_Shipping_Definition,
                Unit_Billing_Definition = Unit_Billing_Definition,
                CustomerOrderMovement_Id = CustomerOrderMovement_Id,
                Good_Code = Good_Code,
                Good_Description = Good_Description,
                Good_Id = _Good_Id
            };
            return (histoCustomer);
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
                HistoCustomersView histoCustomer = obj as HistoCustomersView;
                if ((Object)histoCustomer == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (HistoCustomer_Id == histoCustomer.HistoCustomer_Id) && (Customer_Id == histoCustomer.Customer_Id) &&
                       (Bill_Id == histoCustomer.Bill_Id) && (Bill_Year == histoCustomer.Bill_Year) &&
                       (_Bill_Serie_Id == histoCustomer._Bill_Serie_Id) && (Bill_Date == histoCustomer.Bill_Date) &&
                       (DeliveryNote_Id == histoCustomer.DeliveryNote_Id) && (DeliveryNote_Year == histoCustomer.DeliveryNote_Year) &&
                       (DeliveryNote_Date == histoCustomer.DeliveryNote_Date) && (Shipping_Units == histoCustomer.Shipping_Units) && 
                       (Billing_Units == histoCustomer.Billing_Units) && (Retail_Price == histoCustomer.Retail_Price) &&
                       (Unit_Shipping_Definition == histoCustomer.Unit_Shipping_Definition) &&
                       (Unit_Billing_Definition == histoCustomer.Unit_Billing_Definition) &&
                       (CustomerOrderMovement_Id == histoCustomer.CustomerOrderMovement_Id) &&
                       (Comission == histoCustomer.Comission) && (Good_Code == histoCustomer.Good_Code) &&
                       (Good_Description == histoCustomer.Good_Description) && (_Good_Id == histoCustomer._Good_Id);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(HistoCustomersView histoCustomer)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)histoCustomer == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (HistoCustomer_Id == histoCustomer.HistoCustomer_Id) && (Customer_Id == histoCustomer.Customer_Id) &&
                       (Bill_Id == histoCustomer.Bill_Id) && (Bill_Year == histoCustomer.Bill_Year) &&
                       (_Bill_Serie_Id == histoCustomer._Bill_Serie_Id) && (Bill_Date == histoCustomer.Bill_Date) &&
                       (DeliveryNote_Id == histoCustomer.DeliveryNote_Id) && (DeliveryNote_Year == histoCustomer.DeliveryNote_Year) &&
                       (DeliveryNote_Date == histoCustomer.DeliveryNote_Date) && (Shipping_Units == histoCustomer.Shipping_Units) &&
                       (Billing_Units == histoCustomer.Billing_Units) && (Retail_Price == histoCustomer.Retail_Price) &&
                       (Unit_Shipping_Definition == histoCustomer.Unit_Shipping_Definition) &&
                       (Unit_Billing_Definition == histoCustomer.Unit_Billing_Definition) &&
                       (CustomerOrderMovement_Id == histoCustomer.CustomerOrderMovement_Id) &&
                       (Comission == histoCustomer.Comission) && (Good_Code == histoCustomer.Good_Code) &&
                       (Good_Description == histoCustomer.Good_Description) && (_Good_Id == histoCustomer._Good_Id);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="histoCustomer_1">Primera instáncia a comparar.</param>
        /// <param name="histoCustomer_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(HistoCustomersView histoCustomer_1, HistoCustomersView histoCustomer_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(histoCustomer_1, histoCustomer_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)histoCustomer_1 == null) || ((object)histoCustomer_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (histoCustomer_1.HistoCustomer_Id == histoCustomer_2.HistoCustomer_Id) && 
                       (histoCustomer_1.Customer_Id == histoCustomer_2.Customer_Id) &&
                       (histoCustomer_1.Bill_Id == histoCustomer_2.Bill_Id) && 
                       (histoCustomer_1.Bill_Year == histoCustomer_2.Bill_Year) &&
                       (histoCustomer_1._Bill_Serie_Id == histoCustomer_2._Bill_Serie_Id) && 
                       (histoCustomer_1.Bill_Date == histoCustomer_2.Bill_Date) &&
                       (histoCustomer_1.DeliveryNote_Id == histoCustomer_2.DeliveryNote_Id) && 
                       (histoCustomer_1.DeliveryNote_Year == histoCustomer_2.DeliveryNote_Year) &&
                       (histoCustomer_1.DeliveryNote_Date == histoCustomer_2.DeliveryNote_Date) && 
                       (histoCustomer_1.Shipping_Units == histoCustomer_2.Shipping_Units) &&
                       (histoCustomer_1.Billing_Units == histoCustomer_2.Billing_Units) && 
                       (histoCustomer_1.Retail_Price == histoCustomer_2.Retail_Price) &&
                       (histoCustomer_1.Comission == histoCustomer_2.Comission) &&
                       (histoCustomer_1.Unit_Shipping_Definition == histoCustomer_2.Unit_Shipping_Definition) &&
                       (histoCustomer_1.Unit_Billing_Definition == histoCustomer_2.Unit_Billing_Definition) &&
                       (histoCustomer_1.CustomerOrderMovement_Id == histoCustomer_2.CustomerOrderMovement_Id) &&
                       (histoCustomer_1.Good_Code == histoCustomer_2.Good_Code) &&
                       (histoCustomer_1.Good_Description == histoCustomer_2.Good_Description) &&
                       (histoCustomer_1._Good_Id == histoCustomer_2._Good_Id);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="customer_1">Primera instáncia a comparar.</param>
        /// <param name="customer_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(HistoCustomersView customer_1, HistoCustomersView customer_2)
        {
            return !(customer_1 == customer_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(HistoCustomer_Id, Customer_Id).GetHashCode());
        }

        #endregion
    }
}
