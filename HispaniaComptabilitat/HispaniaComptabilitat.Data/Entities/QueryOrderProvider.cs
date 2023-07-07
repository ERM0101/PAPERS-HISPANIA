using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HispaniaComptabilitat.Data
{
	/// <summary>
	/// 
	/// </summary>
	public class QueryOrderProvider
	{
		/// <summary>
		/// 
		/// </summary>
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
	    public string ProviderAlias
        {
			get;
			set;
		}

		/// <summary>
		/// 
		/// </summary>
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
        public QueryOrderProvider()
        {

        }
    }
}
