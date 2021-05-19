#region Libraries for the class

using System;
using System.Collections.Generic;

#endregion

namespace HispaniaCommon.ViewModel
{
    public class DataForBill
    {
        public DataForBill(DateTime Bill_Date_In, CustomersView Customer_In, List<CustomerOrdersView> Movements_In)
        {
            Bill_Date = Bill_Date_In;
            Customer = Customer_In;
            Movements = Movements_In;
        }

        public DateTime Bill_Date { get; set; }

        public CustomersView Customer { get; set; }

        public List<CustomerOrdersView> Movements { get; set; }
    }


}
