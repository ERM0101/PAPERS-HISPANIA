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
    public enum InputsOutputsAttributes
    {
        GoodCode,
        None
    }

    /// <summary>
    /// Class that Store the information of an Input Output.
    /// </summary>
    public class InputsOutputsView : IMenuView
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
                        { "Data", "IO_Date_Str" },
                        { "Tipus", "IO_Type" },
                        { "Situació", "IO_State_Str" },
                        { "Unitats d'Expedició", "Amount_Shipping" },
                        { "Unitats de Facturació", "Amount_Billing" },
                        { "Número Albarà", "DeliveryNote_Id_Str" },
                        { "Any Albarà", "DeliveryNote_Year_Str" },
                        { "Número Factura", "Bill_Id_Str" },
                        { "Any Factura", "Bill_Year_Str" },
                        { "Serie Factura", "Bill_Serie_Str" },
                        { "Nº Client", "Customer_Id_Str" },
                        { "Client", "Customer_Alias" },
                        { "Preu", "Price_Str" },
                        { "Provedor", "Provider" }
                    };
                }
                return (m_Fields);
            }
        }

        #endregion

        #region Attributes

        private string HashCode = Guid.NewGuid().ToString();

        #endregion

        #region Properties

        #region IMenuView Interface implementation

        public string GetKey
        {
            get
            {
                return GetHashCode().ToString();
            }
        }

        #endregion

        #region Main Fields

        public string Good_Code { get; set; }
        public DateTime IO_Date { get; set; }
        public string IO_Date_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDateTimeValue(IO_Date);
            }
        }
        public string IO_Type { get; set; }
        public decimal Amount_Shipping { get; set; }
        public decimal Amount_Billing { get; set; }
        public decimal Price { get; set; }
        public string Price_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Price, DecimalType.Currency, true);
            }
        }
        public int DeliveryNote_Id { get; set; }
        public string DeliveryNote_Id_Str
        {
            get
            {
                return DeliveryNote_Id == 0 ? "No Informat" : DeliveryNote_Id.ToString();
            }
        }
        public int DeliveryNote_Year { get; set; }
        public string DeliveryNote_Year_Str
        {
            get
            {
                return DeliveryNote_Year == 0 ? "No Informat" : DeliveryNote_Year.ToString();
            }
        }
        public int Customer_Id { get; set; }
        public string Customer_Id_Str
        {
            get
            {
                return Customer_Id == 0 ? "No Informat" : Customer_Id.ToString();
            }
        }
        public string Customer_Alias
        {
            get
            {
                return Customer_Id == 0 ? "No Informat" 
                                        : GlobalViewModel.Instance.HispaniaViewModel.GetCustomer(Customer_Id).Customer_Alias;
            }
        }
        public int Bill_Id { get; set; }
        public string Bill_Id_Str
        {
            get
            {
                return Bill_Id == 0 ? "No Informat" : Bill_Id.ToString();
            }
        }
        public int Bill_Year { get; set; }
        public string Bill_Year_Str
        {
            get
            {
                return Bill_Year == 0 ? "No Informat" : Bill_Year.ToString();
            }
        }
        public string Bill_Serie { get; set; }
        public string Bill_Serie_Str
        {
            get
            {
                return String.IsNullOrEmpty(Bill_Serie) ? "No Informat" : Bill_Serie.ToString();
            }
        }

        public int IO_State { get; set; }
        public string IO_State_Str
        {
            get
            {
                return IO_State == 1 ? "C" : "A";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string Provider
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int? Provider_Id
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string Provider_Id_Str
        {
            get
            {
                string result = ( Provider_Id.HasValue ? Provider_Id.ToString() : "" );
                return result;

            }
        }
        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public InputsOutputsView()
        {
            Good_Code = null;
            Amount_Shipping = GlobalViewModel.DecimalInitValue;
            Amount_Billing = GlobalViewModel.DecimalInitValue;
            Price = GlobalViewModel.DecimalInitValue;
            IO_Date = GlobalViewModel.DateTimeInitValue;
            IO_Type = null;
            DeliveryNote_Id = GlobalViewModel.IntIdInitValue;
            DeliveryNote_Year = GlobalViewModel.IntIdInitValue;
            Customer_Id = GlobalViewModel.IntIdInitValue;
            Bill_Id = GlobalViewModel.IntIdInitValue;
            Bill_Year = GlobalViewModel.IntIdInitValue;
            Bill_Serie = null;
            IO_State = 0;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        /// <param name="inputOutput"></param>
        internal InputsOutputsView(HispaniaCompData.InputsOutputs_Result inputOutput)
        {
            Good_Code = inputOutput.Good_Code;
            Amount_Shipping = GlobalViewModel.GetDecimalValue(inputOutput.Amount_Shipping);
            Amount_Billing = GlobalViewModel.GetDecimalValue(inputOutput.Amount_Billing);
            Price = GlobalViewModel.GetDecimalValue(inputOutput.Price);
            IO_Date = GlobalViewModel.GetDateTimeValue(inputOutput.IO_Date);
            IO_Type = inputOutput.IO_Type;
            DeliveryNote_Id = GlobalViewModel.GetIntValue(inputOutput.DeliveryNote_Id);
            DeliveryNote_Year = GlobalViewModel.GetIntValue(inputOutput.DeliveryNote_Year);
            Customer_Id = GlobalViewModel.GetIntValue(inputOutput.Customer_Id);
            Bill_Id = GlobalViewModel.GetIntValue(inputOutput.Bill_Id);
            Bill_Year = GlobalViewModel.GetIntValue(inputOutput.Bill_Year);
            Bill_Serie = inputOutput.Bill_Serie;
            IO_State = inputOutput.IO_State;
            Provider = inputOutput.Provider;
            Provider_Id = inputOutput.Provider_Id;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public InputsOutputsView(InputsOutputsView inputOutput)
        {
            Good_Code = inputOutput.Good_Code;
            Amount_Shipping = inputOutput.Amount_Shipping;
            Amount_Billing = inputOutput.Amount_Billing;
            Price = inputOutput.Price;
            IO_Date = inputOutput.IO_Date;
            IO_Type = inputOutput.IO_Type;
            DeliveryNote_Id = inputOutput.DeliveryNote_Id;
            DeliveryNote_Year = inputOutput.DeliveryNote_Year;
            Customer_Id = inputOutput.Customer_Id;
            Bill_Id = inputOutput.Bill_Id;
            Bill_Year = inputOutput.Bill_Year;
            Bill_Serie = inputOutput.Bill_Serie;
            IO_State = inputOutput.IO_State;
            this.Provider = inputOutput.Provider;
            this.Provider_Id = inputOutput.Provider_Id;
        }

        #endregion

        #region GetRevisio

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.InputsOutputs_Result GetInputOutput()
        {
            HispaniaCompData.InputsOutputs_Result InputOutput = new HispaniaCompData.InputsOutputs_Result()
            {
                Good_Code = Good_Code,
                Amount_Shipping = Amount_Shipping,
                Amount_Billing = Amount_Billing,
                Price = Price,
                IO_Date = IO_Date,
                IO_Type = IO_Type,
                DeliveryNote_Id = DeliveryNote_Id,
                DeliveryNote_Year = DeliveryNote_Year,
                Customer_Id = Customer_Id,
                Bill_Id = Bill_Id,
                Bill_Year = Bill_Year,
                Bill_Serie = Bill_Serie,
                IO_State = IO_State,
                Provider = Provider,
                Provider_Id = Provider_Id
            };
            return (InputOutput);
        }

        #endregion

        #region Validate
        
        /// <summary>
        /// Validate the data contained in the instance of the class.
        /// </summary>
        public void Validate(out InputsOutputsAttributes ErrorField)
        {
            ErrorField = InputsOutputsAttributes.None;
            if (!GlobalViewModel.IsName(Good_Code))
            {
                ErrorField = InputsOutputsAttributes.GoodCode;
                throw new FormatException("Error, el codi de l'article no pot estar buit.");
            }
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(InputsOutputsView inputOutput)
        {
            Good_Code = inputOutput.Good_Code;
            Amount_Shipping = inputOutput.Amount_Shipping;
            Amount_Billing = inputOutput.Amount_Billing;
            Price = inputOutput.Price;
            IO_Date = inputOutput.IO_Date;
            IO_Type = inputOutput.IO_Type;
            DeliveryNote_Id = inputOutput.DeliveryNote_Id;
            DeliveryNote_Year = inputOutput.DeliveryNote_Year;
            Customer_Id = inputOutput.Customer_Id;
            Bill_Id = inputOutput.Bill_Id;
            Bill_Year = inputOutput.Bill_Year;
            Bill_Serie = inputOutput.Bill_Serie;
            IO_State = inputOutput.IO_State;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(InputsOutputsView Data, InputsOutputsAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case InputsOutputsAttributes.GoodCode:
                     Good_Code = Data.Good_Code;
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
                InputsOutputsView inputOutput = obj as InputsOutputsView;
                if ((Object)inputOutput == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Good_Code == inputOutput.Good_Code) &&
                       (Amount_Shipping == inputOutput.Amount_Shipping) &&
                       (Amount_Billing == inputOutput.Amount_Billing) &&
                       (Price == inputOutput.Price) &&
                       (IO_Date == inputOutput.IO_Date) &&
                       (IO_Type == inputOutput.IO_Type) &&
                       (DeliveryNote_Id == inputOutput.DeliveryNote_Id) &&
                       (DeliveryNote_Year == inputOutput.DeliveryNote_Year) &&
                       (Customer_Id == inputOutput.Customer_Id) &&
                       (Bill_Id == inputOutput.Bill_Id) &&
                       (Bill_Year == inputOutput.Bill_Year) &&
                       (Bill_Serie == inputOutput.Bill_Serie) && 
                       (IO_State == inputOutput.IO_State);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(InputsOutputsView inputOutput)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)inputOutput == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Good_Code == inputOutput.Good_Code) &&
                       (Amount_Shipping == inputOutput.Amount_Shipping) &&
                       (Amount_Billing == inputOutput.Amount_Billing) &&
                       (Price == inputOutput.Price) &&
                       (IO_Date == inputOutput.IO_Date) &&
                       (IO_Type == inputOutput.IO_Type) &&
                       (DeliveryNote_Id == inputOutput.DeliveryNote_Id) &&
                       (DeliveryNote_Year == inputOutput.DeliveryNote_Year) &&
                       (Customer_Id == inputOutput.Customer_Id) &&
                       (Bill_Id == inputOutput.Bill_Id) &&
                       (Bill_Year == inputOutput.Bill_Year) &&
                       (Bill_Serie == inputOutput.Bill_Serie) && 
                       (IO_State == inputOutput.IO_State);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="inputOutput_1">Primera instáncia a comparar.</param>
        /// <param name="inputOutput_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(InputsOutputsView inputOutput_1, InputsOutputsView inputOutput_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(inputOutput_1, inputOutput_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)inputOutput_1 == null) || ((object)inputOutput_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (inputOutput_1.Good_Code == inputOutput_2.Good_Code) &&
                       (inputOutput_1.Amount_Shipping == inputOutput_2.Amount_Shipping) &&
                       (inputOutput_1.Amount_Billing == inputOutput_2.Amount_Billing) &&
                       (inputOutput_1.Price == inputOutput_2.Price) &&
                       (inputOutput_1.IO_Date == inputOutput_2.IO_Date) &&
                       (inputOutput_1.IO_Type == inputOutput_2.IO_Type) &&
                       (inputOutput_1.DeliveryNote_Id == inputOutput_2.DeliveryNote_Id) &&
                       (inputOutput_1.DeliveryNote_Year == inputOutput_2.DeliveryNote_Year) &&
                       (inputOutput_1.Customer_Id == inputOutput_2.Customer_Id) &&
                       (inputOutput_1.Bill_Id == inputOutput_2.Bill_Id) &&
                       (inputOutput_1.Bill_Year == inputOutput_2.Bill_Year) &&
                       (inputOutput_1.Bill_Serie == inputOutput_2.Bill_Serie) && 
                       (inputOutput_1.IO_State == inputOutput_2.IO_State);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="inputOutput_1">Primera instáncia a comparar.</param>
        /// <param name="inputOutput_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(InputsOutputsView inputOutput_1, InputsOutputsView inputOutput_2)
        {
            return !(inputOutput_1 == inputOutput_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(HashCode).GetHashCode());
        }

        #endregion
    }
}
