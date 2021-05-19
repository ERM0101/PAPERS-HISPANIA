namespace HispaniaCommon.DataAccess
{
    #region Operations in Data Base

    /// <summary>
    /// Defines the actions that can do with an item in the DataBase
    /// </summary>
    public enum DataBaseOp
    {
        CREATE, // Register that need being created
        READ,   // Register Readed
        UPDATE, // Register that need being updated
        DELETE, // Register that need being deleted
    }

    #endregion

    #region System Tables 

    /// <summary>
    /// Define the list of tables that can execute system database operations.
    /// </summary>
    public enum SystemTables
    {
        Customer,
        CustomerOrder,
        CustomerOrderMovement,
        Bill,
        IssuanceSupplierOrder,
    }

    #endregion

    #region Class to transfer information

    public class Pair
    {
        public decimal Amount { get; set; }
        public decimal AmountCost { get; set; }

        public Pair(decimal AmountIn, decimal AmountCostIn)
        {
            Amount = AmountIn;
            AmountCost = AmountCostIn;
        }
    }

    #endregion
}
