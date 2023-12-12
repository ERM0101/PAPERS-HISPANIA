#region Libraries for the class

using System;
using System.Collections.Generic;

#endregion

namespace HispaniaCommon.ViewModel
{
    public class DataForProviderBill
    {
        public DataForProviderBill(DateTime Bill_Date_In, ProvidersView Provider_In, List<ProviderOrdersView> Movements_In)
        {
            Bill_Date = Bill_Date_In;
            Provider = Provider_In;
            Movements = Movements_In;
        }

        public DateTime Bill_Date { get; set; }

        public ProvidersView Provider { get; set; }

        public List<ProviderOrdersView> Movements { get; set; }
    }


}
