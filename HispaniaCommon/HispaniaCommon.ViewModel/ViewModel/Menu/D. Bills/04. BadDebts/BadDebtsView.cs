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
    public enum BadDebtsAttributes
    {
        Bill_Id,
        Bill_Year,
        Bill_Serie_Id,
        Receipt_Id,
        Amount,
        Amount_Pending,
        None,
    }

    /// <summary>
    /// Class that Store the information of a BadDebt.
    /// </summary>
    public class BadDebtsView : IMenuView
    {
        #region Fields for Filter

        /// <summary>
        /// Store the list of fields that compose the BadDebt class
        /// </summary>
        private static Dictionary<string, string> m_Fields = null;

        /// <summary>
        /// Get the list of fields that compose the BadDebt class
        /// </summary>
        public static Dictionary<string, string> Fields
        {
            get
            {
                if (m_Fields == null)
                {
                    m_Fields = new Dictionary<string, string>
                    {
                        { "Numero de Rebut", "Receipt_Id" },
                        { "Numero de la Factura", "Bill_Id_Str" },
                        { "Data de la Factura", "Bill_Date_Str" },
                        { "Data de Venciment del Rebut", "Receipt_Expiration_Date_Str" },
                        { "Import Pendent", "Amount_Pending_Str" },
                        { "Numero de Client", "Customer_Id_Str" },
                        { "Alias del Client", "Customer_Alias_Str" }
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
                return GlobalViewModel.GetStringFromIntValue(BadDebt_Id);
            }
        }

        #endregion

        #region Main Fields

        public int BadDebt_Id { get; set; }
        public int Bill_Id { get; set; }
        public string Bill_Id_Str
        {
            get
            {
                return Bill.Bill_Id_Str;
            }
        }
        public decimal Bill_Year { get; set; }
        public int Bill_Serie_Id { get; set; }
        public BillsView Bill
        {
            get
            {
                return GlobalViewModel.Instance.HispaniaViewModel.GetBillFromDb(Bill_Id, Bill_Year);
            }
        }
        public string Bill_Date_Str
        {
            get
            {
                return Bill.Bill_Date_Str;
            }
        }

        public string Customer_Id_Str
        {
            get
            {
                return Bill.Customer_Id_Str;
            }
        }

        public string Customer_Alias_Str
        {
            get
            {
                return Bill.Customer.Customer_Id == GlobalViewModel.IntIdInitValue ? "No Informat" : Bill.Customer.Customer_Alias;
            }
        }

        public int Receipt_Id { get; set; }
        public ReceiptsView Receipt
        {
            get
            {
                return GlobalViewModel.Instance.HispaniaViewModel.GetReceiptFromDb(Receipt_Id);
            }
        }
        public string Receipt_Expiration_Date_Str
        { 
            get
            {
                return Receipt.Expiration_Date_Str;
            }
        }
        public bool Paid { get; set; }
        public bool Returned { get; set; }
        public bool Expired { get; set; }
        public decimal Amount { get; set; }
        public decimal Amount_Pending { get; set; }
        public string Amount_Pending_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Amount_Pending, DecimalType.Currency, true);
            }
        }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public BadDebtsView()
        {
            BadDebt_Id = GlobalViewModel.IntIdInitValue;
            Bill_Id = GlobalViewModel.IntIdInitValue;
            Bill_Year = GlobalViewModel.YearInitValue;
            Bill_Serie_Id = GlobalViewModel.IntIdInitValue;
            Receipt_Id = GlobalViewModel.IntIdInitValue;
            Paid = GlobalViewModel.BoolInitValue;
            Returned = GlobalViewModel.BoolInitValue;
            Expired = GlobalViewModel.BoolInitValue;
            Amount = GlobalViewModel.DecimalInitValue;
            Amount_Pending = GlobalViewModel.DecimalInitValue;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal BadDebtsView(HispaniaCompData.BadDebt badDebt)
        {
            BadDebt_Id = badDebt.BadDebt_Id;
            Bill_Id = GlobalViewModel.GetIntFromIntIdValue(badDebt.Bill_Id);
            Bill_Year = GlobalViewModel.GetDecimalYearValue(badDebt.Bill_Year);
            Bill_Serie_Id = GlobalViewModel.GetIntFromIntIdValue(badDebt.Bill_Serie_Id);
            Receipt_Id = GlobalViewModel.GetIntFromIntIdValue(badDebt.Receipt_Id);
            Paid = GlobalViewModel.GetBoolNullValue(badDebt.Paid);
            Returned = GlobalViewModel.GetBoolNullValue(badDebt.Returned);
            Expired = GlobalViewModel.GetBoolNullValue(badDebt.Expired);
            Amount = badDebt.Amount;
            Amount_Pending = badDebt.Amount_Pending;
        }


        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public BadDebtsView(BadDebtsView badDebt)
        {
            BadDebt_Id = badDebt.BadDebt_Id;
            Bill_Id = badDebt.Bill_Id;
            Bill_Year = badDebt.Bill_Year;
            Bill_Serie_Id = badDebt.Bill_Serie_Id;
            Receipt_Id = badDebt.Receipt_Id;
            Paid = badDebt.Paid;
            Returned = badDebt.Returned;
            Expired = badDebt.Expired;
            Amount = badDebt.Amount;
            Amount_Pending = badDebt.Amount_Pending;
        }

        #endregion

        #region GetBadDebt

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.BadDebt GetBadDebt()
        {
            HispaniaCompData.BadDebt badDebt = new HispaniaCompData.BadDebt()
            {
                BadDebt_Id = BadDebt_Id,
                Bill_Id = Bill_Id,
                Bill_Year = Bill_Year,
                Bill_Serie_Id = Bill_Serie_Id,
                Receipt_Id = Receipt_Id,
                Paid = Paid,
                Returned = Returned,
                Expired = Expired,
                Amount = Amount,
                Amount_Pending = Amount_Pending,
            };
            return (badDebt);
        }

        #endregion

        #region Validate

        public void Validate(out BadDebtsAttributes ErrorField)
        {
            ErrorField = BadDebtsAttributes.None;
            if (Bill_Id == GlobalViewModel.IntIdInitValue)
            {
                ErrorField = BadDebtsAttributes.Bill_Id;
                throw new FormatException("Error, manca seleccionar el numero de la Factura.");
            }
            if (!GlobalViewModel.IsYear(Bill_Year, "Any de la Factura", out string ErrMsg))
            {
                ErrorField = BadDebtsAttributes.Bill_Year;
                throw new FormatException(ErrMsg);
            }
            if (Bill_Serie_Id == GlobalViewModel.IntIdInitValue)
            {
                ErrorField = BadDebtsAttributes.Bill_Serie_Id;
                throw new FormatException("Error, manca seleccionar la sèrie de facturació de la Factura.");
            }
            if (Receipt_Id == GlobalViewModel.IntIdInitValue)
            {
                ErrorField = BadDebtsAttributes.Receipt_Id;
                throw new FormatException("Error, manca seleccionar el numero del Rebut.");
            }
            if (!GlobalViewModel.IsCurrency(Amount, "Import del Rebut", out ErrMsg))
            {
                ErrorField = BadDebtsAttributes.Amount;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsCurrency(Amount, "Import Pendent del Rebut", out ErrMsg))
            {
                ErrorField = BadDebtsAttributes.Amount_Pending;
                throw new FormatException(ErrMsg);
            }
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(BadDebtsView Data)
        {
            Bill_Id = Data.Bill_Id;
            Bill_Year = Data.Bill_Year;
            Bill_Serie_Id = Data.Bill_Serie_Id;
            Receipt_Id = Data.Receipt_Id;
            Amount = Data.Amount;
            Amount_Pending = Data.Amount_Pending;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(BadDebtsView Data, BadDebtsAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case BadDebtsAttributes.Bill_Id:
                     Bill_Id = Data.Bill_Id;
                     break;
                case BadDebtsAttributes.Bill_Year:
                     Bill_Year = Data.Bill_Year;
                     break;
                case BadDebtsAttributes.Bill_Serie_Id:
                     Bill_Serie_Id = Data.Bill_Serie_Id;
                     break;
                case BadDebtsAttributes.Receipt_Id:
                     Receipt_Id = Data.Receipt_Id;
                     break;
                case BadDebtsAttributes.Amount:
                     Amount = Data.Amount;
                     break;
                case BadDebtsAttributes.Amount_Pending:
                     Amount = Data.Amount;
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
                BadDebtsView badDebt = obj as BadDebtsView;
                if ((Object)badDebt == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (BadDebt_Id == badDebt.BadDebt_Id) && (Bill_Id == badDebt.Bill_Id) && (Bill_Year == badDebt.Bill_Year) &&
                       (Bill_Serie_Id == badDebt.Bill_Serie_Id) && (Receipt_Id == badDebt.Receipt_Id) &&
                       (Paid == badDebt.Paid) && (Returned == badDebt.Returned) && (Expired == badDebt.Expired) && 
                       (Amount == badDebt.Amount) && (Amount_Pending == badDebt.Amount_Pending);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(BadDebtsView badDebt)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)badDebt == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (BadDebt_Id == badDebt.BadDebt_Id) && (Bill_Id == badDebt.Bill_Id) && (Bill_Year == badDebt.Bill_Year) &&
                       (Bill_Serie_Id == badDebt.Bill_Serie_Id) && (Receipt_Id == badDebt.Receipt_Id) &&
                       (Paid == badDebt.Paid) && (Returned == badDebt.Returned) && (Expired == badDebt.Expired) &&
                       (Amount == badDebt.Amount) && (Amount_Pending == badDebt.Amount_Pending);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="badDebt_1">Primera instáncia a comparar.</param>
        /// <param name="badDebt_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(BadDebtsView badDebt_1, BadDebtsView badDebt_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(badDebt_1, badDebt_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)badDebt_1 == null) || ((object)badDebt_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (badDebt_1.BadDebt_Id == badDebt_2.BadDebt_Id) && (badDebt_1.Bill_Id == badDebt_2.Bill_Id) && 
                       (badDebt_1.Bill_Year == badDebt_2.Bill_Year) && (badDebt_1.Bill_Serie_Id == badDebt_2.Bill_Serie_Id) && 
                       (badDebt_1.Receipt_Id == badDebt_2.Receipt_Id) && (badDebt_1.Paid == badDebt_2.Paid) && 
                       (badDebt_1.Returned == badDebt_2.Returned) && (badDebt_1.Expired == badDebt_2.Expired) &&
                       (badDebt_1.Amount == badDebt_2.Amount) && (badDebt_1.Amount_Pending == badDebt_2.Amount_Pending);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="badDebt_1">Primera instáncia a comparar.</param>
        /// <param name="badDebt_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(BadDebtsView badDebt_1, BadDebtsView badDebt_2)
        {
            return !(badDebt_1 == badDebt_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(BadDebt_Id).GetHashCode());
        }

        #endregion
    }
}
