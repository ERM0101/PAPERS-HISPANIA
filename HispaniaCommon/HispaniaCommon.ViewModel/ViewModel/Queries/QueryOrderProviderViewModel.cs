using HispaniaCommon.DataAccess.Utils;
using HispaniaComptabilitat.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HispaniaCommon.ViewModel.ViewModel.Queries
{
    public class QueryOrderProviderViewModel
    {
        /// <summary>
		/// 
		/// </summary>
        [ExcelColumnAttribute(1, "Nº Comanda" )]
		public int ProviderOrderId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>        
        public DateTime Date
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool According
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool PrevisioLliurament
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public DateTime PrevisioLliuramentData
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int ProviderId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [ExcelColumnAttribute( 5, "Proveidor" )]
        public string ProviderAlias
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [ExcelColumnAttribute( 6, "Adreça d'Enviament" )]
        public string Address
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string PostalCode
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string City
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [ExcelColumnAttribute( 8, "Sistema de Lliurament" )]
        public string SendTypeDescription
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public decimal TotalAmount
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int? LiniesConformes
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        [ExcelColumnAttribute( 3, "Estat lliurament" )]
        public string AccordingStr
        {
            get
            {
                return ( According == true ? "Material lliurat" :
                            ( this.PrevisioLliurament == true ? "previsio: " + this.PrevisioLliuramentStr
                            : "Lliurament pendent" ) );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ExcelColumnAttribute( 9, "Import" )]
        public string TotalAmountStr
        {
            get
            {
                return GlobalViewModel.GetStringFromDecimalValue( this.TotalAmount, 
                                                                  DecimalType.Currency, true );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ExcelColumnAttribute( 4, "Nº Proveidor" )]
        public string ProviderIdStr
        {
            get
            {
                return GlobalViewModel.GetStringFromIntIdValue( this.ProviderId );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PrevisioLliuramentStr
        {
            get
            {
                return (PrevisioLliurament == true) ? PrevisioLliuramentData.ToShortDateString() : "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ExcelColumnAttribute( 2, "Data Comanda" )]
        public string DateStr
        {
            get
            {
                return GlobalViewModel.GetStringFromDateTimeValue( Date );
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ExcelColumnAttribute( 7, "Codi Postal / Població" )]
        public string PostalCodeStr
        {
            get
            {
                string result = $"{this.PostalCode} {this.City}";
                return result;
            }
        }

        /// <summary>
        /// CTOR
        /// </summary>
        public QueryOrderProviderViewModel()
        {
        }    

    }
}
