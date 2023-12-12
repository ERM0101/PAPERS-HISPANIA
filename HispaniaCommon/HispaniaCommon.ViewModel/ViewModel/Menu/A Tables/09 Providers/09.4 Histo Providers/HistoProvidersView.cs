#region Librerias usadas por la clase

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using HispaniaCompData = HispaniaComptabilitat.Data;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Class that Store the information of a HistoProvider.
    /// </summary>
    public class HistoProvidersView : IMenuView
    {
        #region Fields for Filter

        /// <summary>
        /// Store the list of fields that compose the HistoProvider class
        /// </summary>
        private static Dictionary<string, string> m_Fields = null;

        /// <summary>
        /// Get the list of fields that compose the HistoProvider class
        /// </summary>
        public static Dictionary<string, string> Fields
        {
            get
            {
                if (m_Fields == null)
                {
                    m_Fields = new Dictionary<string, string>
                    {
                        //{ "Numero de Factura", "Bill_Id_Str" },
                        //{ "Any de la Factura", "Bill_Year_Str" },
                        //{ "Sèrie de Facturació de la Factura", "Bill_Serie_Str" },
                        //{ "Data de la Factura", "Bill_Date_Str" },
                        //{ "Numero d'Albarà", "DeliveryNote_Id_Str" },
                        //{ "Any de l'Albarà", "DeliveryNote_Year_Str" },
                        //{ "Data de l'Albarà", "DeliveryNote_Date_Str" },
                        { "Client", "ClientName" },
                        { "Codi d'Article", "Good_Code" },
                        { "Descripció de l'Article", "Good_Description" },
                        { "Unitat d'Expedició", "Shipping_Units_Str" },
                        { "Unitat de Facturació", "Billing_Units_Str" },
                        { "Percentatge Comissió", "Comission_Str" },
                        { "Preu Venta al Public", "Retail_Price_Str" },
                        { "Total", "Total_Price_Str" }
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
                return GlobalViewModel.GetStringFromIntIdValue(HistoProvider_Id);
            }
        }

        #endregion

        #region Main Fields

        public int HistoProvider_Id { get; set; }
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

        #region Provider

        public int Provider_Id
        {
            get;
            private set;
        }

        #endregion

        #region Provider Order
        public int ProviderOrder_Id
        {
            get;
            set;
        }
        #endregion
        #region Provider Order Movement

        public int ProviderOrderMovement_Id
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

        public string ClientName
        {
            get;
            set;
        }

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
        public HistoProvidersView(int provider_Id)
        {
            HistoProvider_Id = GlobalViewModel.IntIdInitValue;
            Provider_Id = provider_Id;
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
            ProviderOrder_Id = GlobalViewModel.IntIdInitValue;
            ProviderOrderMovement_Id = GlobalViewModel.IntIdInitValue;
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
        internal HistoProvidersView(HispaniaCompData.HistoProvider histoProvider)
        {
            HistoProvider_Id = histoProvider.HistoProvider_Id;
            Provider_Id = histoProvider.Provider_Id;
            Bill_Id = GlobalViewModel.GetIntFromIntIdValue(histoProvider.Bill_Id);
            Bill_Year = GlobalViewModel.GetDecimalYearValue(histoProvider.Bill_Year);
            _Bill_Serie_Id = GlobalViewModel.GetIntFromIntIdValue(histoProvider.Bill_Serie_Id);
            Bill_Date = GlobalViewModel.GetDateTimeValue(histoProvider.Bill_Date);
            DeliveryNote_Id = GlobalViewModel.GetIntFromIntIdValue(histoProvider.DeliveryNote_Id);
            DeliveryNote_Year = GlobalViewModel.GetDecimalYearValue(histoProvider.DeliveryNote_Year);
            DeliveryNote_Date = GlobalViewModel.GetDateTimeValue(histoProvider.DeliveryNote_Date);
            Shipping_Units = GlobalViewModel.GetDecimalValue(histoProvider.Shipping_Units);
            Billing_Units = GlobalViewModel.GetDecimalValue(histoProvider.Billing_Units);
            Retail_Price = GlobalViewModel.GetDecimalValue(histoProvider.Retail_Price);
            Comission = GlobalViewModel.GetDecimalValue(histoProvider.Comission);
            Unit_Shipping_Definition = histoProvider.Unit_Shipping_Definition;
            Unit_Billing_Definition = histoProvider.Unit_Billing_Definition;
            ProviderOrder_Id = GlobalViewModel.GetIntFromIntIdValue(histoProvider.ProviderOrder_Id);
            ProviderOrderMovement_Id = GlobalViewModel.GetIntFromIntIdValue(histoProvider.ProviderOrderMovement_Id);
            Good_Code = histoProvider.Good_Code;
            Good_Description = histoProvider.Good_Description;
            _Good_Id = histoProvider.Good_Id;
            this.ClientName = histoProvider.ClientName;
        }
        
        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public HistoProvidersView(ProviderOrderMovementsView ProviderOrderMovement)
        {
            HistoProvider_Id = GlobalViewModel.IntIdInitValue; // Ja s'omple al crear el registre a la Base de Dades
            Provider_Id = ProviderOrderMovement.ProviderOrder.Provider.Provider_Id;
            Bill_Id = GlobalViewModel.IntIdInitValue;          // Ja s'omple al crear el registre a la Base de Dades
            Bill_Year = GlobalViewModel.YearInitValue;         // Ja s'omple al crear el registre a la Base de Dades
            Bill_Date = GlobalViewModel.DateTimeInitValue;     // Ja s'omple al crear el registre a la Base de Dades
            DeliveryNote_Id = ProviderOrderMovement.ProviderOrder.DeliveryNote_Id;
            DeliveryNote_Year = ProviderOrderMovement.ProviderOrder.DeliveryNote_Year;
            DeliveryNote_Date = ProviderOrderMovement.ProviderOrder.DeliveryNote_Date;
            Shipping_Units = ProviderOrderMovement.Unit_Shipping;
            Billing_Units = ProviderOrderMovement.Unit_Billing;
            Retail_Price = ProviderOrderMovement.RetailPrice;
            Comission = ProviderOrderMovement.Comission;
            Unit_Shipping_Definition = ProviderOrderMovement.Unit_Shipping_Definition;
            Unit_Billing_Definition = ProviderOrderMovement.Unit_Billing_Definition;
            ProviderOrder_Id = ProviderOrderMovement._ProviderOrder_Id;
            ProviderOrderMovement_Id = ProviderOrderMovement.ProviderOrderMovement_Id;
            _Good_Id = ProviderOrderMovement.Good.Good_Id;
            Good_Code = ProviderOrderMovement.Good.Good_Code;
            Good_Description = String.IsNullOrEmpty(ProviderOrderMovement.Description) ? ProviderOrderMovement.Good.Good_Description : ProviderOrderMovement.Description;
            this.ClientName = ProviderOrderMovement.ClientName;
            this.ProviderOrder_Id = ProviderOrderMovement._ProviderOrder_Id;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public HistoProvidersView(HistoProvidersView histoProvider)
        {
            HistoProvider_Id = histoProvider.HistoProvider_Id;
            Provider_Id = histoProvider.Provider_Id;
            Bill_Id = histoProvider.Bill_Id;
            Bill_Year = histoProvider.Bill_Year;
            _Bill_Serie_Id = histoProvider._Bill_Serie_Id;
            Bill_Date = histoProvider.Bill_Date;
            DeliveryNote_Id = histoProvider.DeliveryNote_Id;
            DeliveryNote_Year = histoProvider.DeliveryNote_Year;
            DeliveryNote_Date = histoProvider.DeliveryNote_Date;
            Shipping_Units = histoProvider.Shipping_Units;
            Billing_Units = histoProvider.Billing_Units;
            Retail_Price = histoProvider.Retail_Price;
            Comission = histoProvider.Comission;
            Unit_Shipping_Definition = histoProvider.Unit_Shipping_Definition;
            Unit_Billing_Definition = histoProvider.Unit_Billing_Definition;
            ProviderOrder_Id = histoProvider.ProviderOrder_Id;
            ProviderOrderMovement_Id = histoProvider.ProviderOrderMovement_Id;
            Good_Code = histoProvider.Good_Code;
            Good_Description = histoProvider.Good_Description;
            _Good_Id = histoProvider._Good_Id;
        }

        #endregion

        #region GetHistoProvider

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.HistoProvider GetHistoProvider()
        {
            HispaniaCompData.HistoProvider histoProvider = new HispaniaCompData.HistoProvider()
            {
                HistoProvider_Id = HistoProvider_Id,
                Provider_Id = Provider_Id,
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
                ProviderOrder_Id = ProviderOrder_Id,
                ProviderOrderMovement_Id = ProviderOrderMovement_Id,
                Good_Code = Good_Code,
                Good_Description = Good_Description,
                Good_Id = _Good_Id,
                ClientName = ClientName,
            };
            return (histoProvider);
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
                HistoProvidersView histoProvider = obj as HistoProvidersView;
                if ((Object)histoProvider == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (HistoProvider_Id == histoProvider.HistoProvider_Id) && (Provider_Id == histoProvider.Provider_Id) &&
                       (Bill_Id == histoProvider.Bill_Id) && (Bill_Year == histoProvider.Bill_Year) &&
                       (_Bill_Serie_Id == histoProvider._Bill_Serie_Id) && (Bill_Date == histoProvider.Bill_Date) &&
                       (DeliveryNote_Id == histoProvider.DeliveryNote_Id) && (DeliveryNote_Year == histoProvider.DeliveryNote_Year) &&
                       (DeliveryNote_Date == histoProvider.DeliveryNote_Date) && (Shipping_Units == histoProvider.Shipping_Units) && 
                       (Billing_Units == histoProvider.Billing_Units) && (Retail_Price == histoProvider.Retail_Price) &&
                       (Unit_Shipping_Definition == histoProvider.Unit_Shipping_Definition) &&
                       (Unit_Billing_Definition == histoProvider.Unit_Billing_Definition) &&
                       (ProviderOrderMovement_Id == histoProvider.ProviderOrderMovement_Id) &&
                       (Comission == histoProvider.Comission) && (Good_Code == histoProvider.Good_Code) &&
                       (Good_Description == histoProvider.Good_Description) && (_Good_Id == histoProvider._Good_Id);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(HistoProvidersView histoProvider)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)histoProvider == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (HistoProvider_Id == histoProvider.HistoProvider_Id) && (Provider_Id == histoProvider.Provider_Id) &&
                       (Bill_Id == histoProvider.Bill_Id) && (Bill_Year == histoProvider.Bill_Year) &&
                       (_Bill_Serie_Id == histoProvider._Bill_Serie_Id) && (Bill_Date == histoProvider.Bill_Date) &&
                       (DeliveryNote_Id == histoProvider.DeliveryNote_Id) && (DeliveryNote_Year == histoProvider.DeliveryNote_Year) &&
                       (DeliveryNote_Date == histoProvider.DeliveryNote_Date) && (Shipping_Units == histoProvider.Shipping_Units) &&
                       (Billing_Units == histoProvider.Billing_Units) && (Retail_Price == histoProvider.Retail_Price) &&
                       (Unit_Shipping_Definition == histoProvider.Unit_Shipping_Definition) &&
                       (Unit_Billing_Definition == histoProvider.Unit_Billing_Definition) &&
                       (ProviderOrderMovement_Id == histoProvider.ProviderOrderMovement_Id) &&
                       (Comission == histoProvider.Comission) && (Good_Code == histoProvider.Good_Code) &&
                       (Good_Description == histoProvider.Good_Description) && (_Good_Id == histoProvider._Good_Id);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="histoProvider_1">Primera instáncia a comparar.</param>
        /// <param name="histoProvider_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(HistoProvidersView histoProvider_1, HistoProvidersView histoProvider_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(histoProvider_1, histoProvider_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)histoProvider_1 == null) || ((object)histoProvider_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (histoProvider_1.HistoProvider_Id == histoProvider_2.HistoProvider_Id) && 
                       (histoProvider_1.Provider_Id == histoProvider_2.Provider_Id) &&
                       (histoProvider_1.Bill_Id == histoProvider_2.Bill_Id) && 
                       (histoProvider_1.Bill_Year == histoProvider_2.Bill_Year) &&
                       (histoProvider_1._Bill_Serie_Id == histoProvider_2._Bill_Serie_Id) && 
                       (histoProvider_1.Bill_Date == histoProvider_2.Bill_Date) &&
                       (histoProvider_1.DeliveryNote_Id == histoProvider_2.DeliveryNote_Id) && 
                       (histoProvider_1.DeliveryNote_Year == histoProvider_2.DeliveryNote_Year) &&
                       (histoProvider_1.DeliveryNote_Date == histoProvider_2.DeliveryNote_Date) && 
                       (histoProvider_1.Shipping_Units == histoProvider_2.Shipping_Units) &&
                       (histoProvider_1.Billing_Units == histoProvider_2.Billing_Units) && 
                       (histoProvider_1.Retail_Price == histoProvider_2.Retail_Price) &&
                       (histoProvider_1.Comission == histoProvider_2.Comission) &&
                       (histoProvider_1.Unit_Shipping_Definition == histoProvider_2.Unit_Shipping_Definition) &&
                       (histoProvider_1.Unit_Billing_Definition == histoProvider_2.Unit_Billing_Definition) &&
                       (histoProvider_1.ProviderOrderMovement_Id == histoProvider_2.ProviderOrderMovement_Id) &&
                       (histoProvider_1.Good_Code == histoProvider_2.Good_Code) &&
                       (histoProvider_1.Good_Description == histoProvider_2.Good_Description) &&
                       (histoProvider_1._Good_Id == histoProvider_2._Good_Id);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="provider_1">Primera instáncia a comparar.</param>
        /// <param name="provider_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(HistoProvidersView provider_1, HistoProvidersView provider_2)
        {
            return !(provider_1 == provider_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(HistoProvider_Id, Provider_Id).GetHashCode());
        }

        #endregion
    }
}
