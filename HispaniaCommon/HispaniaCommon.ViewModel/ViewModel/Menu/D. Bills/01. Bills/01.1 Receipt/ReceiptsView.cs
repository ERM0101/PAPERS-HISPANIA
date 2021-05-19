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
    public enum ReceiptsAttributes
    {
        Bill_Id,
        Bill_Year,
        Bill_Serie_Id,
        Expiration_Date,
        Amount,
        FileNamePDF,
        None,
    }

    public enum ReceiptState
    {
        Paid,
        Returned,
        Expired,
        Pending,
    }

    /// <summary>
    /// Class that Store the information of a Bill.
    /// </summary>
    public class ReceiptsView : IMenuView
    {
        #region Fields for Filter

        /// <summary>
        /// Store the list of fields that compose the Bill class
        /// </summary>
        private static Dictionary<string, string> m_Fields = null;

        /// <summary>
        /// Get the list of fields that compose the Bill class
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
                        { "Data de Venciment", "Expiration_Date_Str" },
                        { "Estat", "State_Str" },
                        { "Imprès", "Print_Str" },
                        { "Enviat", "SendByEMail_Str" },
                        { "Import", "Amount" },
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
                return string.Format("{0}", GlobalViewModel.GetStringFromIntIdValue(Receipt_Id));
            }
        }

        #endregion

        #region Main Fields
 
        public int Receipt_Id { get; set; }
        public string Receipt_Id_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromIntIdValue(Receipt_Id);
            }
        }
        public int Bill_Id { get; set; }
        public decimal Bill_Year { get; set; }
        public int Bill_Serie_Id { get; set; }
        public DateTime Expiration_Date { get; set; }
        public string Expiration_Date_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDateTimeValue(Expiration_Date);
            }
        }
        public decimal Amount { get; set; }
        public string Amount_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(Amount, DecimalType.Currency, true);
            }
        }
        public bool Paid { get; set; }
        public bool Returned { get; set; }
        public bool Expired { get; set; }
        public string State_Str
        {
            get
            {
                switch (State)
                {
                    case ReceiptState.Paid:
                         return "Pagat";
                    case ReceiptState.Returned:
                         return "Retornat";
                    case ReceiptState.Expired:
                         return "Vençut";
                    case ReceiptState.Pending:
                    default:
                         return "Pendent";
                }
            }
        }

        public ReceiptState m_State;
            
        public ReceiptState State
        {
            get
            {
                return m_State;
            }
            set
            {
                m_State = value;
                switch (m_State)
                {
                    case ReceiptState.Paid:
                         Paid = true;
                         Returned = Expired = false;
                         break;
                    case ReceiptState.Returned:
                         Returned = true;
                         Paid = Expired = false;
                         break;
                    case ReceiptState.Expired:
                         Expired = true;
                         Paid = Returned = false;
                         break;
                    case ReceiptState.Pending:
                    default:
                         Paid = Returned = Expired = false;
                         break;
                }
            }
        }
        public bool Print { get; set; }
        public string Print_Str
        {
            get
            {
                return (Print == true) ? "Imprés" : "No Imprés";
            }
        }
        public bool SendByEMail { get; set; }
        public string SendByEMail_Str
        {
            get
            {
                return (SendByEMail == true) ? "Generat" : "No Generat";
            }
        }

        public string FileNamePDF { get; set; }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public ReceiptsView()
        {
            Receipt_Id = GlobalViewModel.IntIdInitValue;
            Bill_Id = GlobalViewModel.IntIdInitValue;
            Bill_Year = DateTime.Now.Year;
            Bill_Serie_Id = 1; // Per defecte Sèrie de Facturació A (Factures normals (valor > 0)).
            Expiration_Date = DateTime.Now;
            Amount = 0;
            Paid = false;
            Returned = false;
            Expired = false;
            State = ReceiptState.Pending;
            Print = false;
            SendByEMail = false;
            FileNamePDF = string.Empty;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal ReceiptsView(HispaniaCompData.Receipt receipt)
        {
            Receipt_Id = receipt.Receipt_Id;
            Bill_Id = receipt.Bill_Id;
            Bill_Year = receipt.Bill_Year;
            Bill_Serie_Id = receipt.Bill_Serie_Id;
            Expiration_Date = GlobalViewModel.GetDateTimeValue(receipt.Expiration_Date);
            Amount = GlobalViewModel.GetDecimalValue(receipt.Amount);
            Paid = GlobalViewModel.GetBoolValue(receipt.Paid);
            Returned = GlobalViewModel.GetBoolValue(receipt.Returned);
            Expired = GlobalViewModel.GetBoolValue(receipt.Expired);
            if (Paid == true) State = ReceiptState.Paid;
            else if (Returned == true) State = ReceiptState.Returned;
            else if (Expired == true) State = ReceiptState.Expired;
            else State = ReceiptState.Pending;
            Print = GlobalViewModel.GetBoolValue(receipt.Print);
            SendByEMail = GlobalViewModel.GetBoolValue(receipt.SendByEMail);
            FileNamePDF = receipt.FileNamePDF;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public ReceiptsView(ReceiptsView receipt)
        {
            Receipt_Id = receipt.Receipt_Id;
            Bill_Id = receipt.Bill_Id;
            Bill_Year = receipt.Bill_Year;
            Bill_Serie_Id = receipt.Bill_Serie_Id;
            Expiration_Date = GlobalViewModel.GetDateTimeValue(receipt.Expiration_Date);
            Amount = GlobalViewModel.GetDecimalValue(receipt.Amount);
            Paid = GlobalViewModel.GetBoolValue(receipt.Paid);
            Returned = GlobalViewModel.GetBoolValue(receipt.Returned);
            Expired = GlobalViewModel.GetBoolValue(receipt.Expired);
            State = receipt.State;
            Print = GlobalViewModel.GetBoolValue(receipt.Print);
            SendByEMail = GlobalViewModel.GetBoolValue(receipt.SendByEMail);
            FileNamePDF = receipt.FileNamePDF;
        }

        #endregion

        #region GetReceipt

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.Receipt GetReceipt()
        {
            HispaniaCompData.Receipt Receipt = new HispaniaCompData.Receipt()
            {
                Receipt_Id = Receipt_Id,
                Bill_Id = Bill_Id,
                Bill_Year = Bill_Year,
                Bill_Serie_Id = Bill_Serie_Id,
                Expiration_Date = Expiration_Date,
                Amount = Amount,
                Paid = Paid,
                Returned = Returned,
                Expired = Expired,
                Print = Print,
                SendByEMail = SendByEMail,
                FileNamePDF = FileNamePDF
            };
            return (Receipt);
        }

        #endregion

        #region Validate

        /// <summary>
        /// Validate the data contained in the instance of the class.
        /// </summary>
        public void Validate(out ReceiptsAttributes ErrorField)
        {
            ErrorField = ReceiptsAttributes.None;
            if (Bill_Id == GlobalViewModel.IntIdInitValue)
            {
                ErrorField = ReceiptsAttributes.Bill_Id;
                throw new FormatException("Error, manca seleccionar l'identificador de la factura.");
            }
            if (!GlobalViewModel.IsYear(Bill_Year, "Any de la factura", out string ErrMsg))
            {
                ErrorField = ReceiptsAttributes.Bill_Year;
                throw new FormatException(ErrMsg);
            }
            if (Bill_Serie_Id == GlobalViewModel.IntIdInitValue)
            {
                ErrorField = ReceiptsAttributes.Bill_Serie_Id;
                throw new FormatException("Error, manca seleccionar la sèrie de facturació de la factura.");
            }
            if (!GlobalViewModel.IsDateTime(Expiration_Date, "Data de venciment de la factura", out ErrMsg))
            {
                ErrorField = ReceiptsAttributes.Expiration_Date;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsCurrency(Amount, "Import del rebut", out ErrMsg))
            {
                ErrorField = ReceiptsAttributes.Amount;
                throw new FormatException(ErrMsg);
            }
            if (!GlobalViewModel.IsEmptyOrName(FileNamePDF))
            {
                ErrorField = ReceiptsAttributes.FileNamePDF;
                throw new FormatException(GlobalViewModel.ValidationNameError);
            }
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(ReceiptsView Data)
        {
            Bill_Id = Data.Bill_Id;
            Bill_Year = Data.Bill_Year;
            Bill_Serie_Id = Data.Bill_Serie_Id;
            Expiration_Date = Data.Expiration_Date;
            Amount = Data.Amount;
            FileNamePDF = Data.FileNamePDF;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(ReceiptsView Data, ReceiptsAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case ReceiptsAttributes.Bill_Id:
                     Bill_Id = Data.Bill_Id;
                     break;
                case ReceiptsAttributes.Bill_Year:
                     Bill_Year = Data.Bill_Year;
                     break;
                case ReceiptsAttributes.Bill_Serie_Id:
                     Bill_Serie_Id = Data.Bill_Serie_Id;
                     break;
                case ReceiptsAttributes.Expiration_Date:
                     Expiration_Date = Data.Expiration_Date;
                     break;
                case ReceiptsAttributes.Amount:
                     Amount = Data.Amount;
                     break;
                case ReceiptsAttributes.FileNamePDF:
                     FileNamePDF = Data.FileNamePDF;
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
                ReceiptsView receipt = obj as ReceiptsView;
                if ((Object)receipt == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Receipt_Id == receipt.Receipt_Id) && (Bill_Id == receipt.Bill_Id) && (Bill_Year == receipt.Bill_Year) &&
                       (Bill_Serie_Id == receipt.Bill_Serie_Id) && (Expiration_Date == receipt.Expiration_Date) &&
                       (Amount == receipt.Amount) && (Paid == receipt.Paid) && (Returned == receipt.Returned) && 
                       (Expired == receipt.Expired) && (Print == receipt.Print) && (SendByEMail == receipt.SendByEMail) && 
                       (FileNamePDF == receipt.FileNamePDF);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(ReceiptsView receipt)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)receipt == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Receipt_Id == receipt.Receipt_Id) && (Bill_Id == receipt.Bill_Id) && (Bill_Year == receipt.Bill_Year) &&
                       (Bill_Serie_Id == receipt.Bill_Serie_Id) && (Expiration_Date == receipt.Expiration_Date) &&
                       (Amount == receipt.Amount) && (Paid == receipt.Paid) && (Returned == receipt.Returned) && 
                       (Expired == receipt.Expired) && (Print == receipt.Print) && (SendByEMail == receipt.SendByEMail) &&
                       (FileNamePDF == receipt.FileNamePDF);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="receipt_1">Primera instáncia a comparar.</param>
        /// <param name="receipt_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(ReceiptsView receipt_1, ReceiptsView receipt_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(receipt_1, receipt_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)receipt_1 == null) || ((object)receipt_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (receipt_1.Receipt_Id == receipt_2.Receipt_Id) && (receipt_1.Bill_Id == receipt_2.Bill_Id) && 
                       (receipt_1.Bill_Year == receipt_2.Bill_Year) && (receipt_1.Bill_Serie_Id == receipt_2.Bill_Serie_Id) && 
                       (receipt_1.Expiration_Date == receipt_2.Expiration_Date) && (receipt_1.Amount == receipt_2.Amount) && 
                       (receipt_1.Paid == receipt_2.Paid) && (receipt_1.Returned == receipt_2.Returned) && 
                       (receipt_1.Expired == receipt_2.Expired) && (receipt_1.Print == receipt_2.Print) && 
                       (receipt_1.SendByEMail == receipt_2.SendByEMail) &&
                       (receipt_1.FileNamePDF == receipt_2.FileNamePDF);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="Bill_1">Primera instáncia a comparar.</param>
        /// <param name="Bill_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(ReceiptsView receipt_1, ReceiptsView receipt_2)
        {
            return !(receipt_1 == receipt_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(Receipt_Id).GetHashCode());
        }

        #endregion
    }
}
