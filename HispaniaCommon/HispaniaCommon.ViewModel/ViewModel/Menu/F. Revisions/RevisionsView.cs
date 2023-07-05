#region librerias usadas por la clase

using System;
using System.Collections.Generic;
using HispaniaCompData = HispaniaComptabilitat.Data;

#endregion

namespace HispaniaCommon.ViewModel
{
    /// <summary>
    /// Camps del Tipus
    /// </summary>
    public enum RevisionsAttributes
    {
        GoodCode,
        None
    }

    /// <summary>
    /// Class that Store the information of a Revision.
    /// </summary>
    public class RevisionsView : IMenuView
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
                        { "Article", "GoodCode" },
                        { "Descripció", "GoodDescription" },
                        { "Inicial UE", "InitialUE_Str" },
                        { "Entrades UE", "EntryUE_Str" },
                        { "Sortides UE", "OutputUE_Str" },
                        { "Estoc Previst UE", "StockExpectedUE_Str" },
                        { "Estoc Real UE", "StockReadUE_Str" },
                        { "Inicial UF", "InitialUF_Str" },
                        { "Entrades UF", "EntryUF_Str" },
                        { "Sortides UF", "OutputUF_Str" },
                        { "Estoc Previst UF", "StockExpectedUF_Str" },
                        { "Estoc Real UF", "StockReadUF_Str" },
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
                return GoodCode;
            }
        }

        #endregion

        #region Main Fields

        public string GoodCode { get; set; }
        public string GoodDescription { get; set; }
        public decimal InitialUE { get; set; }
        public string InitialUE_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(InitialUE, DecimalType.Unit);
            }
        }
        public decimal EntryUE { get; set; }
        public string EntryUE_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(EntryUE, DecimalType.Unit);
            }
        }
        public decimal OutputUE { get; set; }
        public string OutputUE_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(OutputUE, DecimalType.Unit);
            }
        }
        public decimal StockExpectedUE { get; set; }
        public string StockExpectedUE_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(StockExpectedUE, DecimalType.Unit);
            }
        }
        public decimal StockRealUE { get; set; }
        public string StockRealUE_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(StockRealUE, DecimalType.Unit);
            }
        }
        public decimal InitialUF { get; set; }
        public string InitialUF_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(InitialUF, DecimalType.Unit);
            }
        }
        public decimal EntryUF { get; set; }
        public string EntryUF_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(EntryUF, DecimalType.Unit);
            }
        }
        public decimal OutputUF { get; set; }
        public string OutputUF_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(OutputUF, DecimalType.Unit);
            }
        }
        public decimal StockExpectedUF { get; set; }
        public string StockExpectedUF_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(StockExpectedUF, DecimalType.Unit);
            }
        }
        public decimal StockRealUF { get; set; }
        public string StockRealUF_Str
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue(StockRealUF, DecimalType.Unit);
            }
        }

        #endregion

        #endregion

        #region Builders

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public RevisionsView()
        {
            GoodCode = null;
            GoodDescription = null;
            InitialUE = GlobalViewModel.DecimalInitValue;
            EntryUE = GlobalViewModel.DecimalInitValue;
            OutputUE = GlobalViewModel.DecimalInitValue;
            StockExpectedUE = GlobalViewModel.DecimalInitValue;
            StockRealUE = GlobalViewModel.DecimalInitValue;
            InitialUF = GlobalViewModel.DecimalInitValue;
            EntryUF = GlobalViewModel.DecimalInitValue;
            OutputUF = GlobalViewModel.DecimalInitValue;
            StockExpectedUF = GlobalViewModel.DecimalInitValue;
            StockRealUF = GlobalViewModel.DecimalInitValue;
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal RevisionsView( HispaniaCompData.Revisions_Result revisio )
        {
            GoodCode = revisio.GoodCode;
            GoodDescription = revisio.GoodDescription;
            InitialUE = GlobalViewModel.GetDecimalValue(revisio.InitialUE);
            EntryUE = GlobalViewModel.GetDecimalValue(revisio.EntryUE);
            OutputUE = GlobalViewModel.GetDecimalValue(revisio.OutputUE);
            StockExpectedUE = GlobalViewModel.GetDecimalValue(revisio.StockExpectedUE);
            StockRealUE = GlobalViewModel.GetDecimalValue(revisio.StockRealUE);
            InitialUF = GlobalViewModel.GetDecimalValue(revisio.InitialUF);
            EntryUF = GlobalViewModel.GetDecimalValue(revisio.EntryUF);
            OutputUF = GlobalViewModel.GetDecimalValue(revisio.OutputUF);
            StockExpectedUF = GlobalViewModel.GetDecimalValue(revisio.StockExpectedUF);
            StockRealUF = GlobalViewModel.GetDecimalValue(revisio.StockRealUF);
        }

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        public RevisionsView(RevisionsView revisio)
        {
            GoodCode = revisio.GoodCode;
            GoodDescription = revisio.GoodDescription;
            InitialUE = revisio.InitialUE;
            EntryUE = revisio.EntryUE;
            OutputUE = revisio.OutputUE;
            StockExpectedUE = revisio.StockExpectedUE;
            StockRealUE = revisio.StockRealUE;
            InitialUF = revisio.InitialUF;
            EntryUF = revisio.EntryUF;
            OutputUF = revisio.OutputUF;
            StockExpectedUF = revisio.StockExpectedUF;
            StockRealUF = revisio.StockRealUF;
        }

        #endregion

        #region GetRevisio

        /// <summary>
        /// Builder by default of the class.
        /// </summary>
        internal HispaniaCompData.Revisions_Result GetRevisio()
        {
            HispaniaCompData.Revisions_Result Revisio = new HispaniaCompData.Revisions_Result()
            {
                GoodCode = GoodCode,
                GoodDescription = GoodDescription,
                InitialUE = InitialUE,
                EntryUE = EntryUE,
                OutputUE = OutputUE,
                StockExpectedUE = StockExpectedUE,
                StockRealUE = StockRealUE,
                InitialUF = InitialUF,
                EntryUF = EntryUF,
                OutputUF = OutputUF,
                StockExpectedUF = StockExpectedUF,
                StockRealUF = StockRealUF,
            };
            return (Revisio);
        }

        #endregion

        #region Validate
        
        /// <summary>
        /// Validate the data contained in the instance of the class.
        /// </summary>
        public void Validate(out RevisionsAttributes ErrorField)
        {
            ErrorField = RevisionsAttributes.None;
            if (!GlobalViewModel.IsGoodCode(GoodCode))
            {
                ErrorField = RevisionsAttributes.GoodCode;
                throw new FormatException("Error, el codi de l'article no pot estar buit.");
            }
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        public void RestoreSourceValues(RevisionsView revisio)
        {
            GoodCode = revisio.GoodCode;
            GoodDescription = revisio.GoodDescription;
            InitialUE = revisio.InitialUE;
            EntryUE = revisio.EntryUE;
            OutputUE = revisio.OutputUE;
            StockExpectedUE = revisio.StockExpectedUE;
            StockRealUE = revisio.StockRealUE;
            InitialUF = revisio.InitialUF;
            EntryUF = revisio.EntryUF;
            OutputUF = revisio.OutputUF;
            StockExpectedUF = revisio.StockExpectedUF;
            StockRealUF = revisio.StockRealUF;
        }

        /// <summary>
        /// Restore sorce values for the field indicated.
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="ErrorField"></param>
        public void RestoreSourceValue(RevisionsView Data, RevisionsAttributes ErrorField)
        {
            switch (ErrorField)
            {
                case RevisionsAttributes.GoodCode:
                     GoodCode = Data.GoodCode;
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
                RevisionsView revisio = obj as RevisionsView;
                if ((Object)revisio == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (GoodCode == revisio.GoodCode) &&
                       (GoodDescription == revisio.GoodDescription) &&
                       (InitialUE == revisio.InitialUE) &&
                       (EntryUE == revisio.EntryUE) &&
                       (OutputUE == revisio.OutputUE) &&
                       (StockExpectedUE == revisio.StockExpectedUE) &&
                       (StockRealUE == revisio.StockRealUE) &&
                       (InitialUF == revisio.InitialUF) &&
                       (EntryUF == revisio.EntryUF) &&
                       (OutputUF == revisio.OutputUF) &&
                       (StockExpectedUF == revisio.StockExpectedUF) &&
                       (StockRealUF == revisio.StockRealUF);
        }

        /// <summary>
        /// Sobreescribe el método Equals.
        /// </summary>
        /// <param name="infoAgent">Objeto a comparar con la instáncia actual.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public bool Equals(RevisionsView revisio)
        {
            //  Si el parámetro no es del tipo 'AgentInfo' indicamos error.
                if ((Object)revisio == null) return (false);
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (GoodCode == revisio.GoodCode) &&
                       (GoodDescription == revisio.GoodDescription) &&
                       (InitialUE == revisio.InitialUE) &&
                       (EntryUE == revisio.EntryUE) &&
                       (OutputUE == revisio.OutputUE) &&
                       (StockExpectedUE == revisio.StockExpectedUE) &&
                       (StockRealUE == revisio.StockRealUE) &&
                       (InitialUF == revisio.InitialUF) &&
                       (EntryUF == revisio.EntryUF) &&
                       (OutputUF == revisio.OutputUF) &&
                       (StockExpectedUF == revisio.StockExpectedUF) &&
                       (StockRealUF == revisio.StockRealUF);
        }

        /// <summary>
        /// Sobreescribe el operador de igualdad '=='.
        /// </summary>
        /// <param name="warehouseMovement_1">Primera instáncia a comparar.</param>
        /// <param name="warehouseMovement_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son iguales, false si son diferentes.</returns>
        public static bool operator ==(RevisionsView revisio_1, RevisionsView revisio_2)
        {
            //  Si las dos instáncias valen null o son la misma instáncia retornamos true.
                if (Object.ReferenceEquals(revisio_1, revisio_2)) return (true);
            //  Su una de las instáncias es null y la otra no devolvemos un false.
                if (((object)revisio_1 == null) || ((object)revisio_2 == null)) return (false);
            //  Return true if the fields match:
            //  Realizamos la comparación que determinará si contienen el mismo valor.
                return (revisio_1.GoodCode == revisio_2.GoodCode) &&
                       (revisio_1.GoodDescription == revisio_2.GoodDescription) &&
                       (revisio_1.InitialUE == revisio_2.InitialUE) &&
                       (revisio_1.EntryUE == revisio_2.EntryUE) &&
                       (revisio_1.OutputUE == revisio_2.OutputUE) &&
                       (revisio_1.StockExpectedUE == revisio_2.StockExpectedUE) &&
                       (revisio_1.StockRealUE == revisio_2.StockRealUE) &&
                       (revisio_1.InitialUF == revisio_2.InitialUF) &&
                       (revisio_1.EntryUF == revisio_2.EntryUF) &&
                       (revisio_1.OutputUF == revisio_2.OutputUF) &&
                       (revisio_1.StockExpectedUF == revisio_2.StockExpectedUF) &&
                       (revisio_1.StockRealUF == revisio_2.StockRealUF);
        }

        /// <summary>
        /// Sobreescribe el operador de desigualdad '!='.
        /// </summary>
        /// <param name="customer_1">Primera instáncia a comparar.</param>
        /// <param name="customer_2">Segunda instáncia a comparar.</param>
        /// <returns>true, si son diferentes, false si son iguales.</returns>
        public static bool operator !=(RevisionsView revisio_1, RevisionsView revisio_2)
        {
            return !(revisio_1 == revisio_2);
        }

        /// <summary>
        /// Sobreescribe el método GetHashCode.
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            return (Tuple.Create(GoodCode).GetHashCode());
        }

        #endregion
    }
}
