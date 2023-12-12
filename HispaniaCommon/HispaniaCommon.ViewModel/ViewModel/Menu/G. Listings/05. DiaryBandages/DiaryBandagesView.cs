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
    public enum DiaryBandagesAttributes
    {
        None,
    }

    /// <summary>
    /// Class that Store the information of a Good.
    /// </summary>
    public class DiaryBandagesView : IMenuView
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
        public DiaryBandagesView()
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
        internal DiaryBandagesView( HispaniaCompData.DiaryBandages_Result diaryBandage )
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
        public DiaryBandagesView(DiaryBandagesView diaryBandage)
        {
            Bill_Id = diaryBandage.Bill_Id;
            Bill_Date = diaryBandage.Bill_Date;
            Company_Name = diaryBandage.Company_Name;
            Customer_Id = diaryBandage.Customer_Id;
            GrossAmount = diaryBandage.GrossAmount;
            EarlyPayementDiscount = diaryBandage.EarlyPayementDiscount;
            TaxableBaseAmount = diaryBandage.TaxableBaseAmount;
            IVAAmount = diaryBandage.IVAAmount;
            SurchargeAmount = diaryBandage.SurchargeAmount;
            Total = diaryBandage.Total;
        }

        #endregion

        #region GetGood

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.DiaryBandages_Result GetDiaryBandage()
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

        public void Validate(out DiaryBandagesAttributes ErrorField)
        {
            ErrorField = DiaryBandagesAttributes.None;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(DiaryBandagesView Data)
        {
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(DiaryBandagesView Data, DiaryBandagesAttributes ErrorField)
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
                DiaryBandagesView diaryBandage = obj as DiaryBandagesView;
                if ((Object)diaryBandage == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Bill_Id == diaryBandage.Bill_Id) && 
                       (Bill_Date == diaryBandage.Bill_Date) &&
                       (Company_Name == diaryBandage.Company_Name) &&
                       (Customer_Id == diaryBandage.Customer_Id) &&
                       (GrossAmount == diaryBandage.GrossAmount) &&
                       (EarlyPayementDiscount == diaryBandage.EarlyPayementDiscount) &&
                       (TaxableBaseAmount == diaryBandage.TaxableBaseAmount) &&
                       (IVAAmount == diaryBandage.IVAAmount) &&
                       (SurchargeAmount == diaryBandage.SurchargeAmount) &&
                       (Total == diaryBandage.Total);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(DiaryBandagesView diaryBandage)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)diaryBandage == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Bill_Id == diaryBandage.Bill_Id) && 
                       (Bill_Date == diaryBandage.Bill_Date) &&
                       (Company_Name == diaryBandage.Company_Name) &&
                       (Customer_Id == diaryBandage.Customer_Id) &&
                       (GrossAmount == diaryBandage.GrossAmount) &&
                       (EarlyPayementDiscount == diaryBandage.EarlyPayementDiscount) &&
                       (TaxableBaseAmount == diaryBandage.TaxableBaseAmount) &&
                       (IVAAmount == diaryBandage.IVAAmount) &&
                       (SurchargeAmount == diaryBandage.SurchargeAmount) &&
                       (Total == diaryBandage.Total);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="diaryBandage_1">Primera instáncia a comparar.</param>
        /// <param name="diaryBandage_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(DiaryBandagesView diaryBandage_1, DiaryBandagesView diaryBandage_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(diaryBandage_1, diaryBandage_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)diaryBandage_1 == null) || ((object)diaryBandage_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (diaryBandage_1.Bill_Id == diaryBandage_2.Bill_Id) &&
                        (diaryBandage_1.Bill_Date == diaryBandage_2.Bill_Date) &&
                        (diaryBandage_1.Company_Name == diaryBandage_2.Company_Name) &&
                        (diaryBandage_1.Customer_Id == diaryBandage_2.Customer_Id) &&
                        (diaryBandage_1.GrossAmount == diaryBandage_2.GrossAmount) &&
                        (diaryBandage_1.EarlyPayementDiscount == diaryBandage_2.EarlyPayementDiscount) &&
                        (diaryBandage_1.TaxableBaseAmount == diaryBandage_2.TaxableBaseAmount) &&
                        (diaryBandage_1.IVAAmount == diaryBandage_2.IVAAmount) &&
                        (diaryBandage_1.SurchargeAmount == diaryBandage_2.SurchargeAmount) &&
                        (diaryBandage_1.Total == diaryBandage_2.Total);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="customer_1">Primera instáncia a comparar.</param>
        /// <param name="customer_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(DiaryBandagesView diaryBandage_1, DiaryBandagesView diaryBandage_2)
        {
            return !(diaryBandage_1 == diaryBandage_2);
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
