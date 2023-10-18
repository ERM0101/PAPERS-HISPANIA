#region Librerias usadas por la clase

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using HispaniaCompData = HispaniaComptabilitat.Data;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Camps del Tipus
    /// </summary>
    public enum ProviderOrderMovementsAttributes
    {
        Description,
        Unit_Shipping,
        Unit_Billing,
        RetailPrice,
        Comission,
        Remark,
        According,
        Comi,
        Historic,
        Unit_Shipping_Definition,
        Unit_Billing_Definition,
        ProviderOrder,
        Good,
        None,
    }

    /// <summary>
    /// Class that Store the information of a ProviderOrderMovement.
    /// </summary>
    public class ProviderOrderMovementsView : IMenuView
    {
        #region Fields for Filter

        /// <summary>
        /// Store the list of fields that compose the ProviderOrderMovement class
        /// </summary>
        private static Dictionary<string, string> m_Fields = null;

        /// <summary>
        /// Get the list of fields that compose the ProviderOrderMovement class
        /// </summary>
        public static Dictionary<string, string> Fields
        {
            get
            {
                if (m_Fields == null)
                {
                    m_Fields = new Dictionary<string, string>
                    {
                        { "Numero de Comanda", "_ProviderOrder_Id" },
                        { "Numero de línia de Comanda", "ProviderOrderMovement_Id_Str" },
                        { "Codi Article", "Good_CodArt_Str" },
                        { "Descripció Article", "Description" },
                        { "Quantitat (UE)", "Unit_Shipping_Str" },
                        { "Unitats d'Expedició", "Unit_Shipping_Definition" },
                        { "Quantitat (UF)", "Unit_Billing_Str" },
                        { "Unitats de Facturació", "Unit_Billing_Definition" },
                        { "Preu de Venta al Públic", "RetailPrice_Str" },
                        { "Comissió", "Comission_Str" },
                        { "Total", "Amount" },                        
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
                return GlobalViewModel.GetStringFromIntIdValue(ProviderOrderMovement_Id);
            }
        }

        #endregion

        #region Main Fields

        public int ProviderOrderMovement_Id { get; set; }
        public string ProviderOrderMovement_Id_Str
        {
            get
            {
                if (ProviderOrderMovement_Id < 0) return "No Guardada";
                else return ProviderOrderMovement_Id.ToString();
            }
        }
        public string Description { get; set; }
        public decimal Unit_Shipping { get; set; }

        public string Unit_Shipping_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Unit_Shipping, DecimalType.Unit);
            }
        }
        public decimal Unit_Billing { get; set; }

        public string Unit_Billing_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Unit_Billing, DecimalType.Unit);
            }
        }
        public decimal RetailPrice { get; set; }

        public string RetailPrice_Str
        {
            get
            {
                decimal earlyDiscount = 1;
                if (ProviderOrder.BillingData_EarlyPaymentDiscount > 0)
                {
                    earlyDiscount = 1 - (ProviderOrder.BillingData_EarlyPaymentDiscount / 100);
                }
                return GlobalViewModel.GetStringFromDecimalValue(RetailPrice * earlyDiscount, DecimalType.Currency, true);
            }
        }

        public string RetailPriceWithoutDiscount
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(RetailPrice , DecimalType.Currency, true);
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
        public string Remark { 
            get; 
            set; 
        }
        public bool According { get; set; }
        public string According_Str
        {
            get
            {
                return (According == true) ? "Conforme" : "No Conforme";
            }
        }
        public bool Comi { get; set; }
        public bool Historic { get; set; }
        public string Unit_Shipping_Definition { get; set; }
        public string Unit_Billing_Definition { get; set; }
        public string Internal_Remark { get; set; }
        public int RowOrder { get; set; }

        #endregion

        #region ForeignKey Properties

        #region ProviderOrder

        public int _ProviderOrder_Id { get; set; }

        private ProviderOrdersView _ProviderOrder;

        public ProviderOrdersView ProviderOrder
        {
            get
            {
                if ((_ProviderOrder == null) && (_ProviderOrder_Id != GlobalViewModel.IntIdInitValue))
                {
                    _ProviderOrder = new ProviderOrdersView(GlobalViewModel.Instance.HispaniaViewModel.GetProviderOrder((int)_ProviderOrder_Id));
                }
                return (_ProviderOrder);
            }
            set
            {
                if (value != null)
                {
                    _ProviderOrder = new ProviderOrdersView(value);
                    if (_ProviderOrder == null) _ProviderOrder_Id = GlobalViewModel.IntIdInitValue;
                    else _ProviderOrder_Id = _ProviderOrder.ProviderOrder_Id;
                }
                else
                {
                    _ProviderOrder = null;
                    _ProviderOrder_Id = GlobalViewModel.IntIdInitValue;
                }
            }
        }

        public string DeliveryNote_Id_Str
        {
            get
            {
                return ProviderOrder is null ? "No Informat" : ProviderOrder.DeliveryNote_Id_Str;
            }
        }

        #endregion

        /// <summary>
        /// Client name
        /// </summary>
        public string ClientName
        {
            get;
            set;
        }


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
        public string Good_Key
        {
            get
            {
                return GlobalViewModel.Instance.HispaniaViewModel.GetKeyGoodView(Good);
            }
        }

        #endregion

        #endregion

        #region Query Properties

        public decimal Amount
        {
            get
            {                
                return GlobalViewModel.GetValueDecimalForCalculations(Unit_Billing * RetailPrice , DecimalType.Currency);
            }
        }

        public string Amount_Str
        {
            get
            {
                decimal earlyDiscount = 1;
                if (ProviderOrder.BillingData_EarlyPaymentDiscount > 0)
                {
                    earlyDiscount = 1 - (ProviderOrder.BillingData_EarlyPaymentDiscount / 100);
                }
                return GlobalViewModel.GetStringFromDecimalValue(Amount * earlyDiscount, DecimalType.Currency, true);
            }
        }

        public decimal AmountCost
        {
            get
            {
                return GlobalViewModel.GetValueDecimalForCalculations(Unit_Billing * Good.Average_Price_Cost, DecimalType.Currency);
            }
        }

        public decimal ProviderOrderMovement_Id_For_Sort
        {
            get
            {
                return ProviderOrderMovement_Id < 0 ? 10000000 - ProviderOrderMovement_Id : ProviderOrderMovement_Id;
            }
        }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public ProviderOrderMovementsView()
        {
            ProviderOrderMovement_Id = GlobalViewModel.IntIdInitValue;
            Description = string.Empty;
            Unit_Shipping = GlobalViewModel.DecimalInitValue;
            Unit_Billing = GlobalViewModel.DecimalInitValue;
            RetailPrice = GlobalViewModel.DecimalInitValue;
            Comission = GlobalViewModel.DecimalInitValue;
            Remark = string.Empty;
            According = false;
            Comi = false;
            Historic = false;
            Unit_Shipping_Definition = string.Empty;
            Unit_Billing_Definition = string.Empty;
            Internal_Remark = string.Empty;
            ProviderOrder = null;
            Good = null;
            this.ClientName = string.Empty;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal ProviderOrderMovementsView(HispaniaCompData.ProviderOrderMovement providerOrderMovement)
        {
            ProviderOrderMovement_Id = providerOrderMovement.ProviderOrderMovement_Id;
            Description = providerOrderMovement.Description;
            Unit_Shipping = GlobalViewModel.GetDecimalValue(providerOrderMovement.Unit_Shipping);
            Unit_Billing = GlobalViewModel.GetDecimalValue(providerOrderMovement.Unit_Billing);
            RetailPrice = GlobalViewModel.GetDecimalValue(providerOrderMovement.RetailPrice);
            Comission = GlobalViewModel.GetDecimalValue(providerOrderMovement.Comission);
            Remark = providerOrderMovement.Remark;
            According = providerOrderMovement.According;
            Comi = providerOrderMovement.Comi;
            Historic = providerOrderMovement.Historic;
            Unit_Shipping_Definition = providerOrderMovement.Unit_Shipping_Definition;
            Unit_Billing_Definition = providerOrderMovement.Unit_Billing_Definition;
            Internal_Remark = providerOrderMovement.Internal_Remark;
            RowOrder = Convert.ToInt32(providerOrderMovement.RowOrder);
            _ProviderOrder_Id = GlobalViewModel.GetIntFromIntIdValue(providerOrderMovement.ProviderOrder_Id);
            _Good_Id = GlobalViewModel.GetIntFromIntIdValue(providerOrderMovement.Good_Id);
            this.ClientName = providerOrderMovement.ClientName;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public ProviderOrderMovementsView(ProviderOrderMovementsView providerOrderMovement)
        {
            ProviderOrderMovement_Id = providerOrderMovement.ProviderOrderMovement_Id;
            Description = providerOrderMovement.Description;
            Unit_Shipping = providerOrderMovement.Unit_Shipping;
            Unit_Billing = providerOrderMovement.Unit_Billing;
            RetailPrice = providerOrderMovement.RetailPrice;
            Comission = providerOrderMovement.Comission;
            Remark = providerOrderMovement.Remark;
            According = providerOrderMovement.According;
            Comi = providerOrderMovement.Comi;
            Historic = providerOrderMovement.Historic;
            Unit_Shipping_Definition = providerOrderMovement.Unit_Shipping_Definition;
            Unit_Billing_Definition = providerOrderMovement.Unit_Billing_Definition;
            Internal_Remark = providerOrderMovement.Internal_Remark;
            ProviderOrder = providerOrderMovement.ProviderOrder;
            Good = providerOrderMovement.Good;

            this.ClientName = providerOrderMovement.ClientName;
        }

        #endregion

        #region GetProviderOrderMovement

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.ProviderOrderMovement GetProviderOrderMovement()
        {
            HispaniaCompData.ProviderOrderMovement ProviderOrderMovement = new HispaniaCompData.ProviderOrderMovement()
            {
                ProviderOrderMovement_Id = ProviderOrderMovement_Id,
                Description = Description,
                Unit_Shipping = Unit_Shipping,
                Unit_Billing = Unit_Billing,
                RetailPrice = RetailPrice,
                Comission = Comission,
                Remark = Remark,
                According = According,
                Comi = Comi,
                Historic = Historic,
                Unit_Shipping_Definition = Unit_Shipping_Definition,
                Unit_Billing_Definition = Unit_Billing_Definition,
                RowOrder = RowOrder,
                Internal_Remark = Internal_Remark,
                ProviderOrder_Id = _ProviderOrder_Id,
                Good_Id = _Good_Id,
                ProviderOrder = ProviderOrder.GetProviderOrder(),
                ClientName = this.ClientName
            };
            return (ProviderOrderMovement);
        }

        #endregion

        #region Validate

        /// <summary>
        /// Validate the data contained in the instance of the class.
        /// </summary>
        public void Validate(out ProviderOrderMovementsAttributes ErrorField)
        {
            ErrorField = ProviderOrderMovementsAttributes.None;
            if (!GlobalViewModel.IsEmptyOrName(Description))
            {
                ErrorField = ProviderOrderMovementsAttributes.Description;
                throw new FormatException("Error, format incorrecte de la Descriptió de l'Article");
            }
            if (!GlobalViewModel.IsEmptyOrUnit(Unit_Shipping, "UNITATS D'EXPEDICIÓ", out string ErrMsg))
            {
                ErrorField = ProviderOrderMovementsAttributes.Unit_Shipping;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrUnit(Unit_Billing, "UNITATS DE FACTURACIÓ", out ErrMsg))
            {
                ErrorField = ProviderOrderMovementsAttributes.Unit_Billing;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(RetailPrice, "PREU DE VENTA AL PÚBLIC", out ErrMsg))
            {
                ErrorField = ProviderOrderMovementsAttributes.RetailPrice;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrPercent(Comission, "PERCENTATGE DE COMISSIÓ", out ErrMsg))
            {
                ErrorField = ProviderOrderMovementsAttributes.Comission;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrComment(Remark))
            {
                ErrorField = ProviderOrderMovementsAttributes.Remark;
                throw new FormatException(GlobalViewModel.ValidationCommentError);
            }
            if (!GlobalViewModel.IsEmptyOrComment(Unit_Shipping_Definition))
            {
                ErrorField = ProviderOrderMovementsAttributes.Unit_Shipping_Definition;
                throw new FormatException(GlobalViewModel.ValidationCommentError);
            }
            if (!GlobalViewModel.IsEmptyOrComment(Unit_Billing_Definition))
            {
                ErrorField = ProviderOrderMovementsAttributes.Unit_Billing_Definition;
                throw new FormatException(GlobalViewModel.ValidationCommentError);
            }
            if (ProviderOrder == null)
            {
                ErrorField = ProviderOrderMovementsAttributes.ProviderOrder;
                throw new FormatException("Error, manca seleccionar la comanda associada al moviment.");
            }
            if (Good == null)
            {
                ErrorField = ProviderOrderMovementsAttributes.Good;
                throw new FormatException("Error, manca seleccionar l'Article associat al moviment.");
            }
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(ProviderOrderMovementsView Data)
        {
            Description = Data.Description;
            Unit_Shipping = Data.Unit_Shipping;
            Unit_Billing = Data.Unit_Billing;
            RetailPrice = Data.RetailPrice;
            Comission = Data.Comission;
            Remark = Data.Remark;
            According = Data.According;
            Comi = Data.Comi;
            Historic = Data.Historic;
            Unit_Shipping_Definition = Data.Unit_Shipping_Definition;
            Unit_Billing_Definition = Data.Unit_Billing_Definition;
            ProviderOrder = Data.ProviderOrder;
            Good = Data.Good;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(ProviderOrderMovementsView Data, ProviderOrderMovementsAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case ProviderOrderMovementsAttributes.Description:
                     Description = Data.Description;
                     break;
                case ProviderOrderMovementsAttributes.Unit_Shipping:
                     Unit_Shipping = Data.Unit_Shipping;
                     break;
                case ProviderOrderMovementsAttributes.Unit_Billing:
                     Unit_Billing = Data.Unit_Billing;
                     break;
                case ProviderOrderMovementsAttributes.RetailPrice:
                     RetailPrice = Data.RetailPrice;
                     break;
                case ProviderOrderMovementsAttributes.Comission:
                     Comission = Data.Comission;
                     break;
                case ProviderOrderMovementsAttributes.Remark:
                     Remark = Data.Remark;
                     break;
                case ProviderOrderMovementsAttributes.Unit_Shipping_Definition:
                     Unit_Shipping_Definition = Data.Unit_Shipping_Definition;
                     break;
                case ProviderOrderMovementsAttributes.Unit_Billing_Definition:
                     Unit_Billing_Definition = Data.Unit_Billing_Definition;
                     break;
                case ProviderOrderMovementsAttributes.According:
                     According = Data.According;
                     break;
                case ProviderOrderMovementsAttributes.Comi:
                     Comi = Data.Comi;
                     break;
                case ProviderOrderMovementsAttributes.Historic:
                     Historic = Data.Historic;
                     break;
                case ProviderOrderMovementsAttributes.ProviderOrder:
                     ProviderOrder = Data.ProviderOrder;
                     break;
                case ProviderOrderMovementsAttributes.Good:
                     Good = Data.Good;
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
                ProviderOrderMovementsView providerOrderMovement = obj as ProviderOrderMovementsView;
                if ((Object)providerOrderMovement == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (ProviderOrderMovement_Id == providerOrderMovement.ProviderOrderMovement_Id) &&
                       (_ProviderOrder_Id == providerOrderMovement._ProviderOrder_Id) &&
                       (_Good_Id == providerOrderMovement._Good_Id) &&
                       (Description == providerOrderMovement.Description) &&
                       (Unit_Shipping == providerOrderMovement.Unit_Shipping) &&
                       (Unit_Billing == providerOrderMovement.Unit_Billing) &&
                       (RetailPrice == providerOrderMovement.RetailPrice) &&
                       (Comission == providerOrderMovement.Comission) &&
                       (Remark == providerOrderMovement.Remark) &&
                       (According == providerOrderMovement.According) &&
                       (Historic == providerOrderMovement.Historic) &&
                       (Comi == providerOrderMovement.Comi) &&
                       (Internal_Remark == providerOrderMovement.Internal_Remark) &&
                       (Unit_Shipping_Definition == providerOrderMovement.Unit_Shipping_Definition) &&
                       (Unit_Billing_Definition == providerOrderMovement.Unit_Billing_Definition) &&
                       (ClientName == providerOrderMovement.ClientName );
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(ProviderOrderMovementsView providerOrderMovement)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)providerOrderMovement == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (ProviderOrderMovement_Id == providerOrderMovement.ProviderOrderMovement_Id) &&
                       (_ProviderOrder_Id == providerOrderMovement._ProviderOrder_Id) &&
                       (_Good_Id == providerOrderMovement._Good_Id) &&
                       (Description == providerOrderMovement.Description) &&
                       (Unit_Shipping == providerOrderMovement.Unit_Shipping) &&
                       (Unit_Billing == providerOrderMovement.Unit_Billing) &&
                       (RetailPrice == providerOrderMovement.RetailPrice) &&
                       (Comission == providerOrderMovement.Comission) &&
                       (Remark == providerOrderMovement.Remark) &&
                       (According == providerOrderMovement.According) &&
                       (Historic == providerOrderMovement.Historic) &&
                       (Comi == providerOrderMovement.Comi) &&
                       (Internal_Remark == providerOrderMovement.Internal_Remark) &&
                       (Unit_Shipping_Definition == providerOrderMovement.Unit_Shipping_Definition) &&
                       (Unit_Billing_Definition == providerOrderMovement.Unit_Billing_Definition) &&
                       (this.ClientName == providerOrderMovement.ClientName );
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="providerOrderMovement_1">Primera instáncia a comparar.</param>
        /// <param name="providerOrderMovement_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(ProviderOrderMovementsView providerOrderMovement_1, ProviderOrderMovementsView providerOrderMovement_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(providerOrderMovement_1, providerOrderMovement_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)providerOrderMovement_1 == null) || ((object)providerOrderMovement_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (providerOrderMovement_1.ProviderOrderMovement_Id == providerOrderMovement_2.ProviderOrderMovement_Id) &&
                       (providerOrderMovement_1._ProviderOrder_Id == providerOrderMovement_2._ProviderOrder_Id) &&
                       (providerOrderMovement_1._Good_Id == providerOrderMovement_2._Good_Id) &&
                       (providerOrderMovement_1.Description == providerOrderMovement_2.Description) &&
                       (providerOrderMovement_1.Unit_Shipping == providerOrderMovement_2.Unit_Shipping) &&
                       (providerOrderMovement_1.Unit_Billing == providerOrderMovement_2.Unit_Billing) &&
                       (providerOrderMovement_1.RetailPrice == providerOrderMovement_2.RetailPrice) &&
                       (providerOrderMovement_1.Comission == providerOrderMovement_2.Comission) &&
                       (providerOrderMovement_1.Remark == providerOrderMovement_2.Remark) &&
                       (providerOrderMovement_1.According == providerOrderMovement_2.According) &&
                       (providerOrderMovement_1.Historic == providerOrderMovement_2.Historic) &&
                       (providerOrderMovement_1.Comi == providerOrderMovement_2.Comi) &&
                       (providerOrderMovement_1.Internal_Remark == providerOrderMovement_2.Internal_Remark) &&
                       (providerOrderMovement_1.Unit_Shipping_Definition == providerOrderMovement_2.Unit_Shipping_Definition) &&
                       (providerOrderMovement_1.Unit_Billing_Definition == providerOrderMovement_2.Unit_Billing_Definition) &&
                       (providerOrderMovement_1.ClientName == providerOrderMovement_2.ClientName );
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="provider_1">Primera instáncia a comparar.</param>
        /// <param name="provider_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(ProviderOrderMovementsView provider_1, ProviderOrderMovementsView provider_2)
        {
            return !(provider_1 == provider_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(ProviderOrderMovement_Id, _ProviderOrder_Id, _Good_Id).GetHashCode());
        }

        #endregion
    }
}
