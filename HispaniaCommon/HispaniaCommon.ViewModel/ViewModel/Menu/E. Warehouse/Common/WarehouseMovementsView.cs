using System;
using System.Collections.Generic;
using HispaniaCompData = HispaniaComptabilitat.Data;

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Camps del Tipus
    /// </summary>
    public enum WarehouseMovementsAttributes
    {
        Date,
        Amount_Unit_Shipping,
        Amount_Unit_Billing,
        Type,
        Price,
        Good,
        Provider,
        None
    }

    /// <summary>
    /// Class that Store the information of a WarehouseMovement.
    /// </summary>
    public class WarehouseMovementsView : IMenuView
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
                        { "Numero de Moviment", "WarehouseMovement_Id" },
                        { "Codi Article", "Good_CodArt_Str" },
                        { "Descripció Article", "Good_Description" },
                        { "Número de Proveidor", "Provider_Id_Str" },
                        { "Nom de Proveidor", "Provider_Name" },
                        { "Data", "Date_Str" },
                        { "Quantitat (UE)", "Amount_Unit_Shipping_Str" },
                        { "Unitats d'Expedició", "Unit_Shipping_Definition" },
                        { "Quantitat (UF)", "Amount_Unit_Billing_Str" },
                        { "Unitats de Facturació", "Unit_Billing_Definition" },
                        { "Tipus", "Type_Str" },
                        { "Preu", "Price_Str" },
                        { "Conforme", "According_Str" },
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
                return GlobalViewModel.GetStringFromIntIdValue(WarehouseMovement_Id);
            }
        }

        #endregion

        #region Main Fields

        public int WarehouseMovement_Id { get; set; }
        public string WarehouseMovement_Id_Str
        {
            get
            {
                if (WarehouseMovement_Id < 0) return "No Guardat";
                else return WarehouseMovement_Id.ToString();
            }
        }

        public DateTime Date { get; set; }
        public string Date_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDateTimeValue(Date);
            }
        }
        public decimal Amount_Unit_Shipping { get; set; }
        public string Amount_Unit_Shipping_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Amount_Unit_Shipping, DecimalType.Unit, true);
            }
        }
        public decimal Amount_Unit_Billing { get; set; }
        public string Amount_Unit_Billing_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Amount_Unit_Billing, DecimalType.Unit, true);
            }
        }
        public decimal Price { get; set; }
        public string Price_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Price, DecimalType.Currency, true);
            }
        }
        public int Type { get; set; }
        public string Type_Str
        {
            get
            {
                return (Type == 1) ? "Entrada" : (Type == 5) ? "Sortida" : throw new ArgumentException("Error, tipus no reconegut");
            }
        }
        public bool According { get; set; }
        public string According_Str
        {
            get
            {
                return (According == true) ? "Conforme" : "No Conforme";
            }
        }

        #endregion

        #region ForeignKey Properties

        #region Good

        public int _Good_Id { get; set; }

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
        public string Good_CodArt_Str
        {
            get
            {
                return (Good == null) ? "No Informat" : Good.Good_Code;
            }
        }
        public string Good_Description
        {
            get
            {
                return (Good == null) ? "No Informat" : Good.Good_Description;
            }
        }

        #endregion

        #region Provider

        public int _Provider_Id { get; set; }

        private ProvidersView _Provider;

        public ProvidersView Provider
        {
            get
            {
                if ((_Provider == null) && (_Provider_Id != GlobalViewModel.IntIdInitValue))
                {
                    _Provider = new ProvidersView(GlobalViewModel.Instance.HispaniaViewModel.GetProvider((int)_Provider_Id));
                }
                return (_Provider);
            }
            set
            {
                if (value != null)
                {
                    _Provider = new ProvidersView(value);
                    if (_Provider == null) _Provider_Id = GlobalViewModel.IntIdInitValue;
                    else _Provider_Id = _Provider.Provider_Id;
                }
                else
                {
                    _Provider = null;
                    _Provider_Id = GlobalViewModel.IntIdInitValue;
                }
            }
        }
        public string Provider_Id_Str
        {
            get
            {
                return (Provider == null) ? "No Informat" : Provider.Provider_Id.ToString();
            }
        }
        public string Provider_Name
        {
            get
            {
                return (Provider == null) ? "No Informat" : Provider.Name;
            }
        }

        #endregion

        #region Unit

        public string Unit_Shipping_Definition
        {
            get
            {
                return (Good == null) ? "No Informat" : Good.Good_Unit.Shipping;
            }
        }

        public string Unit_Billing_Definition
        {
            get
            {
                return (Good == null) ? "No Informat" : Good.Good_Unit.Billing;
            }
        }

        #endregion

        #endregion

        #region Query Properties

        public decimal Amount
        {
            get
            {
                return GlobalViewModel.GetValueDecimalForCalculations(Amount_Unit_Billing * Price, DecimalType.Currency);
            }
        }

        public decimal AmountCost
        {
            get
            {
                decimal AmountCostValue;
                if (Good is null) AmountCostValue = 0;
                else
                {
                    AmountCostValue = Good.Billing_Unit_Stocks * Good.Average_Price_Cost;
                }
                return GlobalViewModel.GetValueDecimalForCalculations(AmountCostValue, DecimalType.Currency);
            }
        }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public WarehouseMovementsView()
        {
            WarehouseMovement_Id = GlobalViewModel.IntIdInitValue;
            Date = DateTime.Now;
            Amount_Unit_Shipping = GlobalViewModel.DecimalInitValue;
            Amount_Unit_Billing = GlobalViewModel.DecimalInitValue;
            Type = GlobalViewModel.IntIdInitValue;
            Price = GlobalViewModel.DecimalInitValue;
            According = false;
            Good = null;
            Provider = null;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal WarehouseMovementsView(HispaniaCompData.WarehouseMovement warehouseMovement)
        {
            WarehouseMovement_Id = warehouseMovement.WarehouseMovement_Id;
            Date = GlobalViewModel.GetDateTimeValue(warehouseMovement.Date);
            Amount_Unit_Shipping = GlobalViewModel.GetDecimalValue(warehouseMovement.Amount_Unit_Shipping);
            Amount_Unit_Billing = GlobalViewModel.GetDecimalValue(warehouseMovement.Amount_Unit_Billing);
            Type = GlobalViewModel.GetIntFromIntIdValue(warehouseMovement.Type);
            Price = GlobalViewModel.GetDecimalValue(warehouseMovement.Price);
            According = warehouseMovement.According;
            _Good_Id = GlobalViewModel.GetIntFromIntIdValue(warehouseMovement.Good_Id);
            _Provider_Id = GlobalViewModel.GetIntFromIntIdValue(warehouseMovement.Provider_Id);
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public WarehouseMovementsView(WarehouseMovementsView warehouseMovement)
        {
            WarehouseMovement_Id = warehouseMovement.WarehouseMovement_Id;
            Date = warehouseMovement.Date;
            Amount_Unit_Shipping = warehouseMovement.Amount_Unit_Shipping;
            Amount_Unit_Billing = warehouseMovement.Amount_Unit_Billing;
            Type = warehouseMovement.Type;
            Price = warehouseMovement.Price;
            According = warehouseMovement.According;
            Good = warehouseMovement.Good;
            Provider = warehouseMovement.Provider;
        }

        #endregion

        #region GetWarehouseMovement

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.WarehouseMovement GetWarehouseMovement()
        {
            HispaniaCompData.WarehouseMovement WarehouseMovement = new HispaniaCompData.WarehouseMovement()
            {
                WarehouseMovement_Id = WarehouseMovement_Id,
                Date = Date,
                Amount_Unit_Shipping = Amount_Unit_Shipping,
                Amount_Unit_Billing = Amount_Unit_Billing,
                Type = Type,
                Price = Price,
                According = According,
                Good_Id = _Good_Id,
                Provider_Id = _Provider_Id == -1 ? null : (int?) _Provider_Id,
            };
            return (WarehouseMovement);
        }

        #endregion

        #region Validate
        
        /// <summary>
        /// Validate the data contained in the instance of the class.
        /// </summary>
        public void Validate(out WarehouseMovementsAttributes ErrorField)
        {
            ErrorField = WarehouseMovementsAttributes.None;
            if (!GlobalViewModel.IsDateTime(Date.ToString(), "DATA", out string ErrMsg))
            {
                ErrorField = WarehouseMovementsAttributes.Date;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(Amount_Unit_Shipping, "QUANTITAT D'UNITATS D'EXPEDICIÓ", out ErrMsg))
            {
                ErrorField = WarehouseMovementsAttributes.Amount_Unit_Shipping;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(Amount_Unit_Billing, "QUANTITAT D'UNITATS DE FACTURACIÓ", out ErrMsg))
            {
                ErrorField = WarehouseMovementsAttributes.Amount_Unit_Billing;
                throw new FormatException(ErrMsg);
            }
            if ((Type != 1) && (Type != 5))
            {
                ErrorField = WarehouseMovementsAttributes.Price;
                throw new FormatException("Error, tipus invàlid, només pot ser 1 : Entrada o 5 : Sortida.");
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(Price, "PREU", out ErrMsg))
            {
                ErrorField = WarehouseMovementsAttributes.Price;
                throw new FormatException(ErrMsg);
            }
            if (Good == null)
            {
                ErrorField = WarehouseMovementsAttributes.Good;
                throw new FormatException("Error, manca seleccionar l'Article associat al moviment.");
            }
            //if (Provider == null)
            //{
            //    ErrorField = WarehouseMovementsAttributes.Provider;
            //    throw new FormatException("Error, manca seleccionar el Proveidor associat al moviment.");
            //}
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(WarehouseMovementsView Data)
        {
            Date = Data.Date;
            Amount_Unit_Shipping = Data.Amount_Unit_Shipping;
            Amount_Unit_Billing = Data.Amount_Unit_Billing;
            Type = Data.Type;
            Price = Data.Price;
            According = Data.According;
            Good = Data.Good;
            Provider = Data.Provider;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(WarehouseMovementsView Data, WarehouseMovementsAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case WarehouseMovementsAttributes.Date:
                     Date = Data.Date;
                     break;
                case WarehouseMovementsAttributes.Amount_Unit_Shipping:
                     Amount_Unit_Shipping = Data.Amount_Unit_Shipping;
                     break;
                case WarehouseMovementsAttributes.Amount_Unit_Billing:
                     Amount_Unit_Billing = Data.Amount_Unit_Billing;
                     break;
                case WarehouseMovementsAttributes.Price:
                     Price = Data.Price;
                     break;
                case WarehouseMovementsAttributes.Type:
                     Type = Data.Type;
                     break;
                case WarehouseMovementsAttributes.Good:
                     Good = Data.Good;
                     break;
                case WarehouseMovementsAttributes.Provider:
                     Provider = Data.Provider;
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
                WarehouseMovementsView warehouseMovement = obj as WarehouseMovementsView;
                if ((Object)warehouseMovement == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (WarehouseMovement_Id == warehouseMovement.WarehouseMovement_Id) &&
                       (_Provider_Id == warehouseMovement._Provider_Id) &&
                       (_Good_Id == warehouseMovement._Good_Id) &&
                       (Date == warehouseMovement.Date) &&
                       (Amount_Unit_Shipping == warehouseMovement.Amount_Unit_Shipping) &&
                       (Amount_Unit_Billing == warehouseMovement.Amount_Unit_Billing) &&
                       (Type == warehouseMovement.Type) &&
                       (Price == warehouseMovement.Price) &&
                       (According == warehouseMovement.According);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(WarehouseMovementsView warehouseMovement)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)warehouseMovement == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (WarehouseMovement_Id == warehouseMovement.WarehouseMovement_Id) &&
                       (_Provider_Id == warehouseMovement._Provider_Id) &&
                       (_Good_Id == warehouseMovement._Good_Id) &&
                       (Date == warehouseMovement.Date) &&
                       (Amount_Unit_Shipping == warehouseMovement.Amount_Unit_Shipping) &&
                       (Amount_Unit_Billing == warehouseMovement.Amount_Unit_Billing) &&
                       (Type == warehouseMovement.Type) &&
                       (Price == warehouseMovement.Price) &&
                       (According == warehouseMovement.According);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="warehouseMovement_1">Primera instáncia a comparar.</param>
        /// <param name="warehouseMovement_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(WarehouseMovementsView warehouseMovement_1, WarehouseMovementsView warehouseMovement_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(warehouseMovement_1, warehouseMovement_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)warehouseMovement_1 == null) || ((object)warehouseMovement_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (warehouseMovement_1.WarehouseMovement_Id == warehouseMovement_2.WarehouseMovement_Id) &&
                       (warehouseMovement_1._Provider_Id == warehouseMovement_2._Provider_Id) &&
                       (warehouseMovement_1._Good_Id == warehouseMovement_2._Good_Id) &&
                       (warehouseMovement_1.Date == warehouseMovement_2.Date) &&
                       (warehouseMovement_1.Amount_Unit_Shipping == warehouseMovement_2.Amount_Unit_Shipping) &&
                       (warehouseMovement_1.Amount_Unit_Billing == warehouseMovement_2.Amount_Unit_Billing) &&
                       (warehouseMovement_1.Type == warehouseMovement_2.Type) &&
                       (warehouseMovement_1.Price == warehouseMovement_2.Price) &&
                       (warehouseMovement_1.According == warehouseMovement_2.According);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="customer_1">Primera instáncia a comparar.</param>
        /// <param name="customer_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(WarehouseMovementsView customer_1, WarehouseMovementsView customer_2)
        {
            return !(customer_1 == customer_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(WarehouseMovement_Id).GetHashCode());
        }

        #endregion
    }
}
