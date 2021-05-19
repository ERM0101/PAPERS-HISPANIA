using System;
using System.Collections.Generic;
using HispaniaCompData = HispaniaComptabilitat.Data;

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Camps del Tipus
    /// </summary>
    public enum SettlementsAttributes
    {
        None,
    }

    /// <summary>
    /// Class that Store the information of a Good.
    /// </summary>
    public class SettlementsView : IMenuView
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
                return Settlement_Id.ToString();
            }
        }

        #endregion

        #region Main Fields

        public int Settlement_Id { get; set; }
        public int Agent_Id { get; set; }
        public string Agent_Name { get; set; }
        public int Bill_Id { get; set; }
        public DateTime Bill_Date { get; set; }
        public int Customer_Id { get; set; }
        public string Company_Name { get; set; }
        public decimal Base { get; set; }
        public decimal ComissionPercent { get; set; }
        public decimal Comission { get; set; }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public SettlementsView()
        {
            Settlement_Id = GlobalViewModel.IntIdInitValue;
            Agent_Id = GlobalViewModel.IntIdInitValue;
            Agent_Name = string.Empty;
            Bill_Id = GlobalViewModel.IntIdInitValue;
            Bill_Date = DateTime.Now;
            Company_Name = string.Empty;
            Customer_Id = GlobalViewModel.IntIdInitValue;
            Base = GlobalViewModel.DecimalInitValue;
            ComissionPercent = GlobalViewModel.DecimalInitValue;
            Comission = GlobalViewModel.DecimalInitValue;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal SettlementsView(HispaniaCompData.Settlement settlement)
        {
            Settlement_Id = GlobalViewModel.GetIntValue(settlement.Settlement_Id);
            Agent_Id = GlobalViewModel.GetIntValue(settlement.Agent_Id);
            Agent_Name = settlement.Agent_Name;
            Bill_Id = GlobalViewModel.GetIntValue(settlement.Bill_Id);
            Bill_Date = GlobalViewModel.GetDateTimeValue(settlement.Bill_Date);
            Company_Name = settlement.Company_Name;
            Customer_Id = GlobalViewModel.GetIntValue(settlement.Customer_Id);
            Base = GlobalViewModel.GetDecimalValue(settlement.Base);
            ComissionPercent = GlobalViewModel.GetDecimalValue(settlement.ComissionPercent);
            Comission = GlobalViewModel.GetDecimalValue(settlement.Comission);
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public SettlementsView(SettlementsView settlement)
        {
            Settlement_Id = settlement.Settlement_Id;
            Agent_Id = settlement.Agent_Id;
            Agent_Name = settlement.Agent_Name;
            Bill_Id = settlement.Bill_Id;
            Bill_Date = settlement.Bill_Date;
            Company_Name = settlement.Company_Name;
            Customer_Id = settlement.Customer_Id;
            Base = settlement.Base;
            ComissionPercent = settlement.ComissionPercent;
            Comission = settlement.Comission;
        }

        #endregion

        #region GetSettlement

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.Settlement GetSettlement()
        {
            HispaniaCompData.Settlement settlement = new HispaniaCompData.Settlement()
            {
                Bill_Id = Bill_Id,
                Bill_Date = Bill_Date,
                Company_Name = Company_Name,
                Customer_Id = Customer_Id,
                Base = Base,
                ComissionPercent = ComissionPercent,
                Comission = Comission,
                Settlement_Id = Settlement_Id,
                Agent_Id = Agent_Id,
                Agent_Name = Agent_Name,
            };
            return (settlement);
        }

        #endregion

        #region Validate

        public void Validate(out SettlementsAttributes ErrorField)
        {
            ErrorField = SettlementsAttributes.None;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(SettlementsView Data)
        {
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(SettlementsView Data, SettlementsAttributes ErrorField)
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
                SettlementsView settlement = obj as SettlementsView;
                if ((Object)settlement == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Bill_Id == settlement.Bill_Id) && 
                       (Bill_Date == settlement.Bill_Date) &&
                       (Company_Name == settlement.Company_Name) &&
                       (Customer_Id == settlement.Customer_Id) &&
                       (Base == settlement.Base) &&
                       (ComissionPercent == settlement.ComissionPercent) &&
                       (Comission == settlement.Comission) &&
                       (Settlement_Id == settlement.Settlement_Id) &&
                       (Agent_Id == settlement.Agent_Id) &&
                       (Agent_Name == settlement.Agent_Name);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(SettlementsView settlement)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)settlement == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (Bill_Id == settlement.Bill_Id) && 
                       (Bill_Date == settlement.Bill_Date) &&
                       (Company_Name == settlement.Company_Name) &&
                       (Customer_Id == settlement.Customer_Id) &&
                       (Base == settlement.Base) &&
                       (ComissionPercent == settlement.ComissionPercent) &&
                       (Comission == settlement.Comission) &&
                       (Settlement_Id == settlement.Settlement_Id) &&
                       (Agent_Id == settlement.Agent_Id) &&
                       (Agent_Name == settlement.Agent_Name);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="settlement_1">Primera instáncia a comparar.</param>
        /// <param name="settlement_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(SettlementsView settlement_1, SettlementsView settlement_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(settlement_1, settlement_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)settlement_1 == null) || ((object)settlement_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (settlement_1.Bill_Id == settlement_2.Bill_Id) &&
                        (settlement_1.Bill_Date == settlement_2.Bill_Date) &&
                        (settlement_1.Company_Name == settlement_2.Company_Name) &&
                        (settlement_1.Customer_Id == settlement_2.Customer_Id) &&
                        (settlement_1.Base == settlement_2.Base) &&
                        (settlement_1.ComissionPercent == settlement_2.ComissionPercent) &&
                        (settlement_1.Comission == settlement_2.Comission) &&
                        (settlement_1.Settlement_Id == settlement_2.Settlement_Id) &&
                        (settlement_1.Agent_Id == settlement_2.Agent_Id) &&
                        (settlement_1.Agent_Name == settlement_2.Agent_Name);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="customer_1">Primera instáncia a comparar.</param>
        /// <param name="customer_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(SettlementsView settlement_1, SettlementsView settlement_2)
        {
            return !(settlement_1 == settlement_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(Settlement_Id).GetHashCode());
        }

        #endregion
    }
}
