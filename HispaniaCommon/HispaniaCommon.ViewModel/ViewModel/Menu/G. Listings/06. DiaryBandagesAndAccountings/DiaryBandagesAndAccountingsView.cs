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
    public enum DiaryBandagesAndAccountingsAttributes
    {
        None,
    }

    /// <summary>
    /// Class that Store the information of a Good.
    /// </summary>
    public class DiaryBandagesAndAccountingsView : IMenuView
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
                        { "Nº Factura", "Bill_Id" },
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
                return Bill_Id.ToString();
            }
        }

        #endregion

        #region Main Fields

        public int Bill_Id { get; set; }
        public DateTime Bill_Date { get; set; }
        public int Customer_Id { get; set; }
        public string Company_Name { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal EarlyPayementDiscount { get; set; }
        public decimal TaxableBaseAmount { get; set; }
        public decimal IVAAmount { get; set; }
        public decimal SurchargeAmount { get; set; }
        public decimal Total { get; set; }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public DiaryBandagesAndAccountingsView()
        {
            Bill_Id = GlobalViewModel.IntIdInitValue;
            Bill_Date = DateTime.Now;
            Company_Name = string.Empty;
            Customer_Id = GlobalViewModel.IntIdInitValue;
            GrossAmount = GlobalViewModel.DecimalInitValue;
            EarlyPayementDiscount = GlobalViewModel.DecimalInitValue;
            TaxableBaseAmount = GlobalViewModel.DecimalInitValue;
            IVAAmount = GlobalViewModel.DecimalInitValue;
            SurchargeAmount = GlobalViewModel.DecimalInitValue;
            Total = GlobalViewModel.DecimalInitValue;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal DiaryBandagesAndAccountingsView(HispaniaCompData.DiaryBandages_Result diaryBandage )
        {
            Bill_Id = GlobalViewModel.GetIntValue(diaryBandage.Bill_Id);
            Bill_Date = GlobalViewModel.GetDateTimeValue(diaryBandage.Bill_Date);
            Company_Name = diaryBandage.Company_Name;
            Customer_Id = GlobalViewModel.GetIntValue(diaryBandage.Customer_Id);
            GrossAmount = GlobalViewModel.GetDecimalValue(diaryBandage.GrossAmount);
            EarlyPayementDiscount = GlobalViewModel.GetDecimalValue(diaryBandage.EarlyPayementDiscount);
            TaxableBaseAmount = GlobalViewModel.GetDecimalValue(diaryBandage.TaxableBaseAmount);
            IVAAmount = GlobalViewModel.GetDecimalValue(diaryBandage.IVAAmount);
            SurchargeAmount = GlobalViewModel.GetDecimalValue(diaryBandage.SurchargeAmount);
            Total = GlobalViewModel.GetDecimalValue(diaryBandage.Total);
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public DiaryBandagesAndAccountingsView(DiaryBandagesAndAccountingsView diaryBandagesAndAccounting)
        {
            Bill_Id = diaryBandagesAndAccounting.Bill_Id;
            Bill_Date = diaryBandagesAndAccounting.Bill_Date;
            Company_Name = diaryBandagesAndAccounting.Company_Name;
            Customer_Id = diaryBandagesAndAccounting.Customer_Id;
            GrossAmount = diaryBandagesAndAccounting.GrossAmount;
            EarlyPayementDiscount = diaryBandagesAndAccounting.EarlyPayementDiscount;
            TaxableBaseAmount = diaryBandagesAndAccounting.TaxableBaseAmount;
            IVAAmount = diaryBandagesAndAccounting.IVAAmount;
            SurchargeAmount = diaryBandagesAndAccounting.SurchargeAmount;
            Total = diaryBandagesAndAccounting.Total;
        }

        #endregion

        #region GetGood

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.DiaryBandages_Result GetDiaryBandagesAndAccounting()
        {
            HispaniaCompData.DiaryBandages_Result diaryBandage = new HispaniaCompData.DiaryBandages_Result()
            {
                Bill_Id = Bill_Id,
                Bill_Date = Bill_Date,
                Company_Name = Company_Name,
                Customer_Id = Customer_Id,
                GrossAmount = GrossAmount,
                EarlyPayementDiscount = EarlyPayementDiscount,
                TaxableBaseAmount = TaxableBaseAmount,
                IVAAmount = IVAAmount,
                SurchargeAmount = SurchargeAmount,
                Total = Total,
            };
            return (diaryBandage);
        }

        #endregion

        #region Validate

        public void Validate(out DiaryBandagesAndAccountingsAttributes ErrorField)
        {
            ErrorField = DiaryBandagesAndAccountingsAttributes.None;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(DiaryBandagesAndAccountingsView Data)
        {
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(DiaryBandagesAndAccountingsView Data, 
                                       DiaryBandagesAndAccountingsAttributes ErrorField)
        {
            switch (ErrorField)
            {
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
                DiaryBandagesAndAccountingsView diaryBandagesAndAccounting = obj as DiaryBandagesAndAccountingsView;
                if ((Object)diaryBandagesAndAccounting == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Bill_Id == diaryBandagesAndAccounting.Bill_Id) && 
                       (Bill_Date == diaryBandagesAndAccounting.Bill_Date) &&
                       (Company_Name == diaryBandagesAndAccounting.Company_Name) &&
                       (Customer_Id == diaryBandagesAndAccounting.Customer_Id) &&
                       (GrossAmount == diaryBandagesAndAccounting.GrossAmount) &&
                       (EarlyPayementDiscount == diaryBandagesAndAccounting.EarlyPayementDiscount) &&
                       (TaxableBaseAmount == diaryBandagesAndAccounting.TaxableBaseAmount) &&
                       (IVAAmount == diaryBandagesAndAccounting.IVAAmount) &&
                       (SurchargeAmount == diaryBandagesAndAccounting.SurchargeAmount) &&
                       (Total == diaryBandagesAndAccounting.Total);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(DiaryBandagesAndAccountingsView diaryBandagesAndAccounting)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)diaryBandagesAndAccounting == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Bill_Id == diaryBandagesAndAccounting.Bill_Id) && 
                       (Bill_Date == diaryBandagesAndAccounting.Bill_Date) &&
                       (Company_Name == diaryBandagesAndAccounting.Company_Name) &&
                       (Customer_Id == diaryBandagesAndAccounting.Customer_Id) &&
                       (GrossAmount == diaryBandagesAndAccounting.GrossAmount) &&
                       (EarlyPayementDiscount == diaryBandagesAndAccounting.EarlyPayementDiscount) &&
                       (TaxableBaseAmount == diaryBandagesAndAccounting.TaxableBaseAmount) &&
                       (IVAAmount == diaryBandagesAndAccounting.IVAAmount) &&
                       (SurchargeAmount == diaryBandagesAndAccounting.SurchargeAmount) &&
                       (Total == diaryBandagesAndAccounting.Total);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="diaryBandagesAndAccounting_1">Primera instáncia a comparar.</param>
        /// <param name="diaryBandagesAndAccounting_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(DiaryBandagesAndAccountingsView diaryBandagesAndAccounting_1,
                                       DiaryBandagesAndAccountingsView diaryBandagesAndAccounting_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(diaryBandagesAndAccounting_1, diaryBandagesAndAccounting_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)diaryBandagesAndAccounting_1 == null) || ((object)diaryBandagesAndAccounting_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (diaryBandagesAndAccounting_1.Bill_Id == diaryBandagesAndAccounting_2.Bill_Id) &&
                        (diaryBandagesAndAccounting_1.Bill_Date == diaryBandagesAndAccounting_2.Bill_Date) &&
                        (diaryBandagesAndAccounting_1.Company_Name == diaryBandagesAndAccounting_2.Company_Name) &&
                        (diaryBandagesAndAccounting_1.Customer_Id == diaryBandagesAndAccounting_2.Customer_Id) &&
                        (diaryBandagesAndAccounting_1.GrossAmount == diaryBandagesAndAccounting_2.GrossAmount) &&
                        (diaryBandagesAndAccounting_1.EarlyPayementDiscount == diaryBandagesAndAccounting_2.EarlyPayementDiscount) &&
                        (diaryBandagesAndAccounting_1.TaxableBaseAmount == diaryBandagesAndAccounting_2.TaxableBaseAmount) &&
                        (diaryBandagesAndAccounting_1.IVAAmount == diaryBandagesAndAccounting_2.IVAAmount) &&
                        (diaryBandagesAndAccounting_1.SurchargeAmount == diaryBandagesAndAccounting_2.SurchargeAmount) &&
                        (diaryBandagesAndAccounting_1.Total == diaryBandagesAndAccounting_2.Total);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="customer_1">Primera instáncia a comparar.</param>
        /// <param name="customer_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(DiaryBandagesAndAccountingsView diaryBandagesAndAccounting_1,
                                       DiaryBandagesAndAccountingsView diaryBandagesAndAccounting_2)
        {
            return !(diaryBandagesAndAccounting_1 == diaryBandagesAndAccounting_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(Bill_Id).GetHashCode());
        }

        #endregion
    }
}
