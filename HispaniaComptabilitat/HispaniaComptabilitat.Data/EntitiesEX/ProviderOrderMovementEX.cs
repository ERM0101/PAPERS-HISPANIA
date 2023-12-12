using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HispaniaComptabilitat.Data.EntitiesEX
{
    /// <summary>
    /// 
    /// </summary>
    public static class ProviderOrderMovementEX
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public static ProviderOrderMovement Update( this ProviderOrderMovement _This, ProviderOrderMovement other )
        {

            _This.ProviderOrderMovement_Id = other.ProviderOrderMovement_Id;
            _This.ProviderOrder_Id = other.ProviderOrder_Id;
            _This.Good_Id = other.Good_Id;
            _This.Description = other.Description;
            _This.Unit_Shipping = other.Unit_Shipping;
            _This.Unit_Billing = other.Unit_Billing;
            _This.RetailPrice = other.RetailPrice;
            _This.Comission = other.Comission;
            _This.Remark = other.Remark;
            _This.According = other.According;
            _This.Comi = other.Comi;
            _This.Historic = other.Historic;
            _This.Unit_Shipping_Definition = other.Unit_Shipping_Definition;
            _This.Unit_Billing_Definition = other.Unit_Billing_Definition;
            _This.Internal_Remark = other.Internal_Remark;
            _This.RowOrder = other.RowOrder;
            _This.ClientName = other.ClientName;
    
            return _This;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_This"></param>
        /// <param name="newId"></param>
        /// <returns></returns>
        public static ProviderOrderMovement Clone( this ProviderOrderMovement _This, int? newId = null )
        {

            ProviderOrderMovement result = new ProviderOrderMovement();
            
            result.Update( _This );

            if( newId.HasValue )
            {
                result.ProviderOrderMovement_Id = newId.Value;                
            }
            
            return result;
        }

    }
}
