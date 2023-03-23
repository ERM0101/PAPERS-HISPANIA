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
    public enum CustomerOrderMovementsAttributes
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
        CustomerOrder,
        Good,
        None,
    }

    /// <summary>
    /// Class that Store the information of a CustomerOrderMovement.
    /// </summary>
    public class CustomerOrderMovementsView : IMenuView
    {
        #region Fields for Filter

        /// <summary>
        /// Store the list of fields that compose the CustomerOrderMovement class
        /// </summary>
        private static Dictionary<string, string> m_Fields = null;

        /// <summary>
        /// Get the list of fields that compose the CustomerOrderMovement class
        /// </summary>
        public static Dictionary<string, string> Fields
        {
            get
            {
                if (m_Fields == null)
                {
                    m_Fields = new Dictionary<string, string>
                    {
                        { "Numero de Comanda", "_CustomerOrder_Id" },
                        { "Numero de línia de Comanda", "CustomerOrderMovement_Id_Str" },
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
                return GlobalViewModel.GetStringFromIntIdValue(CustomerOrderMovement_Id);
            }
        }

        #endregion

        #region Main Fields

        public int CustomerOrderMovement_Id { get; set; }
        public string CustomerOrderMovement_Id_Str
        {
            get
            {
                if (CustomerOrderMovement_Id < 0) return "No Guardada";
                else return CustomerOrderMovement_Id.ToString();
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
                return GlobalViewModel.GetStringFromDecimalValue(RetailPrice, DecimalType.Currency, true);
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
        public string Remark { get; set; }
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

        #region CustomerOrder

        public int _CustomerOrder_Id { get; set; }

        private CustomerOrdersView _CustomerOrder;

        public CustomerOrdersView CustomerOrder
        {
            get
            {
                if ((_CustomerOrder == null) && (_CustomerOrder_Id != GlobalViewModel.IntIdInitValue))
                {
                    _CustomerOrder = new CustomerOrdersView(GlobalViewModel.Instance.HispaniaViewModel.GetCustomerOrder((int)_CustomerOrder_Id));
                }
                return (_CustomerOrder);
            }
            set
            {
                if (value != null)
                {
                    _CustomerOrder = new CustomerOrdersView(value);
                    if (_CustomerOrder == null) _CustomerOrder_Id = GlobalViewModel.IntIdInitValue;
                    else _CustomerOrder_Id = _CustomerOrder.CustomerOrder_Id;
                }
                else
                {
                    _CustomerOrder = null;
                    _CustomerOrder_Id = GlobalViewModel.IntIdInitValue;
                }
            }
        }

        public string DeliveryNote_Id_Str
        {
            get
            {
                return CustomerOrder is null ? "No Informat" : CustomerOrder.DeliveryNote_Id_Str;
            }
        }

        #endregion

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
                return GlobalViewModel.GetValueDecimalForCalculations(Unit_Billing * RetailPrice, DecimalType.Currency);
            }
        }

        public string Amount_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Amount, DecimalType.Currency, true);
            }
        }

        public decimal AmountCost
        {
            get
            {
                return GlobalViewModel.GetValueDecimalForCalculations(Unit_Billing * Good.Average_Price_Cost, DecimalType.Currency);
            }
        }

        public decimal CustomerOrderMovement_Id_For_Sort
        {
            get
            {
                return CustomerOrderMovement_Id < 0 ? 10000000 - CustomerOrderMovement_Id : CustomerOrderMovement_Id;
            }
        }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public CustomerOrderMovementsView()
        {
            CustomerOrderMovement_Id = GlobalViewModel.IntIdInitValue;
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
            CustomerOrder = null;
            Good = null;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal CustomerOrderMovementsView(HispaniaCompData.CustomerOrderMovement customerOrderMovement)
        {
            CustomerOrderMovement_Id = customerOrderMovement.CustomerOrderMovement_Id;
            Description = customerOrderMovement.Description;
            Unit_Shipping = GlobalViewModel.GetDecimalValue(customerOrderMovement.Unit_Shipping);
            Unit_Billing = GlobalViewModel.GetDecimalValue(customerOrderMovement.Unit_Billing);
            RetailPrice = GlobalViewModel.GetDecimalValue(customerOrderMovement.RetailPrice);
            Comission = GlobalViewModel.GetDecimalValue(customerOrderMovement.Comission);
            Remark = customerOrderMovement.Remark;
            According = customerOrderMovement.According;
            Comi = customerOrderMovement.Comi;
            Historic = customerOrderMovement.Historic;
            Unit_Shipping_Definition = customerOrderMovement.Unit_Shipping_Definition;
            Unit_Billing_Definition = customerOrderMovement.Unit_Billing_Definition;
            Internal_Remark = customerOrderMovement.Internal_Remark;
            RowOrder = Convert.ToInt32(customerOrderMovement.RowOrder);
            _CustomerOrder_Id = GlobalViewModel.GetIntFromIntIdValue(customerOrderMovement.CustomerOrder_Id);
            _Good_Id = GlobalViewModel.GetIntFromIntIdValue(customerOrderMovement.Good_Id);
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public CustomerOrderMovementsView(CustomerOrderMovementsView customerOrderMovement)
        {
            CustomerOrderMovement_Id = customerOrderMovement.CustomerOrderMovement_Id;
            Description = customerOrderMovement.Description;
            Unit_Shipping = customerOrderMovement.Unit_Shipping;
            Unit_Billing = customerOrderMovement.Unit_Billing;
            RetailPrice = customerOrderMovement.RetailPrice;
            Comission = customerOrderMovement.Comission;
            Remark = customerOrderMovement.Remark;
            According = customerOrderMovement.According;
            Comi = customerOrderMovement.Comi;
            Historic = customerOrderMovement.Historic;
            Unit_Shipping_Definition = customerOrderMovement.Unit_Shipping_Definition;
            Unit_Billing_Definition = customerOrderMovement.Unit_Billing_Definition;
            Internal_Remark = customerOrderMovement.Internal_Remark;
            CustomerOrder = customerOrderMovement.CustomerOrder;
            Good = customerOrderMovement.Good;
        }

        #endregion

        #region GetCustomerOrderMovement

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.CustomerOrderMovement GetCustomerOrderMovement()
        {
            HispaniaCompData.CustomerOrderMovement CustomerOrderMovement = new HispaniaCompData.CustomerOrderMovement()
            {
                CustomerOrderMovement_Id = CustomerOrderMovement_Id,
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
                CustomerOrder_Id = _CustomerOrder_Id,
                Good_Id = _Good_Id,
            };
            return (CustomerOrderMovement);
        }

        #endregion

        #region Validate

        /// <summary>
        /// Validate the data contained in the instance of the class.
        /// </summary>
        public void Validate(out CustomerOrderMovementsAttributes ErrorField)
        {
            ErrorField = CustomerOrderMovementsAttributes.None;
            if (!GlobalViewModel.IsEmptyOrName(Description))
            {
                ErrorField = CustomerOrderMovementsAttributes.Description;
                throw new FormatException("Error, format incorrecte de la Descriptió de l'Article");
            }
            if (!GlobalViewModel.IsEmptyOrUnit(Unit_Shipping, "UNITATS D'EXPEDICIÓ", out string ErrMsg))
            {
                ErrorField = CustomerOrderMovementsAttributes.Unit_Shipping;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrUnit(Unit_Billing, "UNITATS DE FACTURACIÓ", out ErrMsg))
            {
                ErrorField = CustomerOrderMovementsAttributes.Unit_Billing;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrCurrency(RetailPrice, "PREU DE VENTA AL PÚBLIC", out ErrMsg))
            {
                ErrorField = CustomerOrderMovementsAttributes.RetailPrice;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrPercent(Comission, "PERCENTATGE DE COMISSIÓ", out ErrMsg))
            {
                ErrorField = CustomerOrderMovementsAttributes.Comission;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrComment(Remark))
            {
                ErrorField = CustomerOrderMovementsAttributes.Remark;
                throw new FormatException(GlobalViewModel.ValidationCommentError);
            }
            if (!GlobalViewModel.IsEmptyOrComment(Unit_Shipping_Definition))
            {
                ErrorField = CustomerOrderMovementsAttributes.Unit_Shipping_Definition;
                throw new FormatException(GlobalViewModel.ValidationCommentError);
            }
            if (!GlobalViewModel.IsEmptyOrComment(Unit_Billing_Definition))
            {
                ErrorField = CustomerOrderMovementsAttributes.Unit_Billing_Definition;
                throw new FormatException(GlobalViewModel.ValidationCommentError);
            }
            if (CustomerOrder == null)
            {
                ErrorField = CustomerOrderMovementsAttributes.CustomerOrder;
                throw new FormatException("Error, manca seleccionar la comanda associada al moviment.");
            }
            if (Good == null)
            {
                ErrorField = CustomerOrderMovementsAttributes.Good;
                throw new FormatException("Error, manca seleccionar l'Article associat al moviment.");
            }
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(CustomerOrderMovementsView Data)
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
            CustomerOrder = Data.CustomerOrder;
            Good = Data.Good;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(CustomerOrderMovementsView Data, CustomerOrderMovementsAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case CustomerOrderMovementsAttributes.Description:
                     Description = Data.Description;
                     break;
                case CustomerOrderMovementsAttributes.Unit_Shipping:
                     Unit_Shipping = Data.Unit_Shipping;
                     break;
                case CustomerOrderMovementsAttributes.Unit_Billing:
                     Unit_Billing = Data.Unit_Billing;
                     break;
                case CustomerOrderMovementsAttributes.RetailPrice:
                     RetailPrice = Data.RetailPrice;
                     break;
                case CustomerOrderMovementsAttributes.Comission:
                     Comission = Data.Comission;
                     break;
                case CustomerOrderMovementsAttributes.Remark:
                     Remark = Data.Remark;
                     break;
                case CustomerOrderMovementsAttributes.Unit_Shipping_Definition:
                     Unit_Shipping_Definition = Data.Unit_Shipping_Definition;
                     break;
                case CustomerOrderMovementsAttributes.Unit_Billing_Definition:
                     Unit_Billing_Definition = Data.Unit_Billing_Definition;
                     break;
                case CustomerOrderMovementsAttributes.According:
                     According = Data.According;
                     break;
                case CustomerOrderMovementsAttributes.Comi:
                     Comi = Data.Comi;
                     break;
                case CustomerOrderMovementsAttributes.Historic:
                     Historic = Data.Historic;
                     break;
                case CustomerOrderMovementsAttributes.CustomerOrder:
                     CustomerOrder = Data.CustomerOrder;
                     break;
                case CustomerOrderMovementsAttributes.Good:
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
                CustomerOrderMovementsView customerOrderMovement = obj as CustomerOrderMovementsView;
                if ((Object)customerOrderMovement == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (CustomerOrderMovement_Id == customerOrderMovement.CustomerOrderMovement_Id) &&
                       (_CustomerOrder_Id == customerOrderMovement._CustomerOrder_Id) &&
                       (_Good_Id == customerOrderMovement._Good_Id) &&
                       (Description == customerOrderMovement.Description) &&
                       (Unit_Shipping == customerOrderMovement.Unit_Shipping) &&
                       (Unit_Billing == customerOrderMovement.Unit_Billing) &&
                       (RetailPrice == customerOrderMovement.RetailPrice) &&
                       (Comission == customerOrderMovement.Comission) &&
                       (Remark == customerOrderMovement.Remark) &&
                       (According == customerOrderMovement.According) &&
                       (Historic == customerOrderMovement.Historic) &&
                       (Comi == customerOrderMovement.Comi) &&
                       (Internal_Remark == customerOrderMovement.Internal_Remark) &&
                       (Unit_Shipping_Definition == customerOrderMovement.Unit_Shipping_Definition) &&
                       (Unit_Billing_Definition == customerOrderMovement.Unit_Billing_Definition);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(CustomerOrderMovementsView customerOrderMovement)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)customerOrderMovement == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (CustomerOrderMovement_Id == customerOrderMovement.CustomerOrderMovement_Id) &&
                       (_CustomerOrder_Id == customerOrderMovement._CustomerOrder_Id) &&
                       (_Good_Id == customerOrderMovement._Good_Id) &&
                       (Description == customerOrderMovement.Description) &&
                       (Unit_Shipping == customerOrderMovement.Unit_Shipping) &&
                       (Unit_Billing == customerOrderMovement.Unit_Billing) &&
                       (RetailPrice == customerOrderMovement.RetailPrice) &&
                       (Comission == customerOrderMovement.Comission) &&
                       (Remark == customerOrderMovement.Remark) &&
                       (According == customerOrderMovement.According) &&
                       (Historic == customerOrderMovement.Historic) &&
                       (Comi == customerOrderMovement.Comi) &&
                       (Internal_Remark == customerOrderMovement.Internal_Remark) &&
                       (Unit_Shipping_Definition == customerOrderMovement.Unit_Shipping_Definition) &&
                       (Unit_Billing_Definition == customerOrderMovement.Unit_Billing_Definition);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="customerOrderMovement_1">Primera instáncia a comparar.</param>
        /// <param name="customerOrderMovement_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(CustomerOrderMovementsView customerOrderMovement_1, CustomerOrderMovementsView customerOrderMovement_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(customerOrderMovement_1, customerOrderMovement_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)customerOrderMovement_1 == null) || ((object)customerOrderMovement_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (customerOrderMovement_1.CustomerOrderMovement_Id == customerOrderMovement_2.CustomerOrderMovement_Id) &&
                       (customerOrderMovement_1._CustomerOrder_Id == customerOrderMovement_2._CustomerOrder_Id) &&
                       (customerOrderMovement_1._Good_Id == customerOrderMovement_2._Good_Id) &&
                       (customerOrderMovement_1.Description == customerOrderMovement_2.Description) &&
                       (customerOrderMovement_1.Unit_Shipping == customerOrderMovement_2.Unit_Shipping) &&
                       (customerOrderMovement_1.Unit_Billing == customerOrderMovement_2.Unit_Billing) &&
                       (customerOrderMovement_1.RetailPrice == customerOrderMovement_2.RetailPrice) &&
                       (customerOrderMovement_1.Comission == customerOrderMovement_2.Comission) &&
                       (customerOrderMovement_1.Remark == customerOrderMovement_2.Remark) &&
                       (customerOrderMovement_1.According == customerOrderMovement_2.According) &&
                       (customerOrderMovement_1.Historic == customerOrderMovement_2.Historic) &&
                       (customerOrderMovement_1.Comi == customerOrderMovement_2.Comi) &&
                       (customerOrderMovement_1.Internal_Remark == customerOrderMovement_2.Internal_Remark) &&
                       (customerOrderMovement_1.Unit_Shipping_Definition == customerOrderMovement_2.Unit_Shipping_Definition) &&
                       (customerOrderMovement_1.Unit_Billing_Definition == customerOrderMovement_2.Unit_Billing_Definition);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="customer_1">Primera instáncia a comparar.</param>
        /// <param name="customer_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(CustomerOrderMovementsView customer_1, CustomerOrderMovementsView customer_2)
        {
            return !(customer_1 == customer_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(CustomerOrderMovement_Id, _CustomerOrder_Id, _Good_Id).GetHashCode());
        }

        #endregion
    }
}
