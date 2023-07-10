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
    public static class ProviderOrderEX
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public static ProviderOrder Clone( this ProviderOrder _This )
        {
            ProviderOrder result = new ProviderOrder()
            {
                Provider_Id = _This.Provider_Id,
                Date = DateTime.Now,//_This.Date,
                PostalCode_Id = _This.PostalCode_Id,
                SendType_Id = _This.SendType_Id,
                EffectType_Id = _This.EffectType_Id,
                IVAPercent = _This.IVAPercent,
                SurchargePercent = _This.SurchargePercent,
                //Bill_Id = _This.Bill_Id,
                //Bill_Year = _This.Bill_Year,
                //Bill_Serie_Id = _This.Bill_Serie_Id
                //Bill_Date = _This.Bill_Date,
                DeliveryNote_Id = _This.DeliveryNote_Id,
                DeliveryNote_Year = _This.DeliveryNote_Year,
                DeliveryNote_Date = _This.DeliveryNote_Date,
                Address = _This.Address,
                NumEffect = _This.NumEffect,
                DataBank_ExpirationDays = _This.DataBank_ExpirationDays,
                DataBank_ExpirationInterval = _This.DataBank_ExpirationInterval,
                DataBank_Paydays1 = _This.DataBank_Paydays1,
                DataBank_Paydays2 = _This.DataBank_Paydays2,
                DataBank_Paydays3 = _This.DataBank_Paydays3,
                DataBank_Bank = _This.DataBank_Bank,
                DataBank_BankAddress = _This.DataBank_BankAddress,
                DataBank_IBAN_CountryCode = _This.DataBank_IBAN_CountryCode,
                DataBank_IBAN_BankCode = _This.DataBank_IBAN_BankCode,
                DataBank_IBAN_OfficeCode = _This.DataBank_IBAN_OfficeCode,
                DataBank_IBAN_CheckDigits = _This.DataBank_IBAN_CheckDigits,
                DataBank_IBAN_AccountNumber = _This.DataBank_IBAN_AccountNumber,
                BillingData_EarlyPaymentDiscount = _This.BillingData_EarlyPaymentDiscount,
                BillingData_Agent_Id = _This.BillingData_Agent_Id,
                According = _This.According,
                Valued = _This.Valued,
                Remarks = _This.Remarks,
                Transfer = _This.Transfer,
                //Print = _This.Print,
                //SendByEMail = _This.SendByEMail,
                //Historic = _This.Historic, 
                //Select_Bill = _This.Select_Bill,
                Expiration = _This.Expiration,
                Daily = _This.Daily,
                Daily_Dates = _This.Daily_Dates,
                //Print_ProviderOrder = _This.Print_ProviderOrder,
                //SendByEmail_ProviderOrder = _This.SendByEmail_ProviderOrder;
                TotalAmount = _This.TotalAmount,
                //TimeTable = _This.TimeTable,
                PrevisioLliurament = _This.PrevisioLliurament,
                PrevisioLliuramentData = _This.PrevisioLliuramentData,
                NameClientAssoc = _This.NameClientAssoc
                
            };

            foreach( ProviderOrderMovement movement in _This.ProviderOrderMovements )
            {
                ProviderOrderMovement new_movement = movement.Clone( 0 );

                result.ProviderOrderMovements.Add( new_movement );

            }

            return result;
        }
    }
}
