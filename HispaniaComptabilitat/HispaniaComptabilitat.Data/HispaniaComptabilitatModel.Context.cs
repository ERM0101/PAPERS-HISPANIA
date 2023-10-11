﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HispaniaComptabilitat.Data
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AddressStore> AddressStores { get; set; }
        public virtual DbSet<Agent> Agents { get; set; }
        public virtual DbSet<BadDebt> BadDebts { get; set; }
        public virtual DbSet<BadDebtMovement> BadDebtMovements { get; set; }
        public virtual DbSet<Bill> Bills { get; set; }
        public virtual DbSet<BillingSerie> BillingSeries { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerOrder> CustomerOrders { get; set; }
        public virtual DbSet<CustomerOrderMovement> CustomerOrderMovements { get; set; }
        public virtual DbSet<DeliveryNote> DeliveryNotes { get; set; }
        public virtual DbSet<EffectType> EffectTypes { get; set; }
        public virtual DbSet<Good> Goods { get; set; }
        public virtual DbSet<HistoCumulativeCustomer> HistoCumulativeCustomers { get; set; }
        public virtual DbSet<HistoCumulativeProvider> HistoCumulativeProviders { get; set; }
        public virtual DbSet<HistoCustomer> HistoCustomers { get; set; }
        public virtual DbSet<HistoGood> HistoGoods { get; set; }
        public virtual DbSet<HistoProvider> HistoProviders { get; set; }
        public virtual DbSet<IVAType> IVATypes { get; set; }
        public virtual DbSet<LocalConnection> LocalConnections { get; set; }
        public virtual DbSet<Lock> Locks { get; set; }
        public virtual DbSet<Parameter> Parameters { get; set; }
        public virtual DbSet<PostalCode> PostalCodes { get; set; }
        public virtual DbSet<PriceRange> PriceRanges { get; set; }
        public virtual DbSet<Provider> Providers { get; set; }
        public virtual DbSet<ProviderOrder> ProviderOrders { get; set; }
        public virtual DbSet<ProviderOrderMovement> ProviderOrderMovements { get; set; }
        public virtual DbSet<QueryCustom> QueryCustoms { get; set; }
        public virtual DbSet<Receipt> Receipts { get; set; }
        public virtual DbSet<RelatedCustomer> RelatedCustomers { get; set; }
        public virtual DbSet<RelatedProvider> RelatedProviders { get; set; }
        public virtual DbSet<SendType> SendTypes { get; set; }
        public virtual DbSet<Unit> Units { get; set; }
        public virtual DbSet<WarehouseMovement> WarehouseMovements { get; set; }
        public virtual DbSet<TEMP_DIARY_BANDAGE> TEMP_DIARY_BANDAGE { get; set; }
    
        public virtual ObjectResult<string> CustomerOrderMovementsComments(Nullable<int> customerOrder_Id)
        {
            var customerOrder_IdParameter = customerOrder_Id.HasValue ?
                new ObjectParameter("CustomerOrder_Id", customerOrder_Id) :
                new ObjectParameter("CustomerOrder_Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("CustomerOrderMovementsComments", customerOrder_IdParameter);
        }
    
        public virtual ObjectResult<CustomerSales_Result> CustomerSales(Nullable<decimal> upper_Limit_Sales)
        {
            var upper_Limit_SalesParameter = upper_Limit_Sales.HasValue ?
                new ObjectParameter("Upper_Limit_Sales", upper_Limit_Sales) :
                new ObjectParameter("Upper_Limit_Sales", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<CustomerSales_Result>("CustomerSales", upper_Limit_SalesParameter);
        }
    
        public virtual ObjectResult<DeliveryNoteLines_Result> DeliveryNoteLines()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<DeliveryNoteLines_Result>("DeliveryNoteLines");
        }
    
        public virtual ObjectResult<DiaryBandages_Result> DiaryBandages(string bill_Id_From, string bill_Id_Until, Nullable<decimal> year_Query)
        {
            var bill_Id_FromParameter = bill_Id_From != null ?
                new ObjectParameter("Bill_Id_From", bill_Id_From) :
                new ObjectParameter("Bill_Id_From", typeof(string));
    
            var bill_Id_UntilParameter = bill_Id_Until != null ?
                new ObjectParameter("Bill_Id_Until", bill_Id_Until) :
                new ObjectParameter("Bill_Id_Until", typeof(string));
    
            var year_QueryParameter = year_Query.HasValue ?
                new ObjectParameter("Year_Query", year_Query) :
                new ObjectParameter("Year_Query", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<DiaryBandages_Result>("DiaryBandages", bill_Id_FromParameter, bill_Id_UntilParameter, year_QueryParameter);
        }
    
        public virtual int HistoricAcumCustomers(Nullable<int> data_Year)
        {
            var data_YearParameter = data_Year.HasValue ?
                new ObjectParameter("Data_Year", data_Year) :
                new ObjectParameter("Data_Year", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("HistoricAcumCustomers", data_YearParameter);
        }
    
        public virtual int HistoricAcumGoods(Nullable<int> data_Year)
        {
            var data_YearParameter = data_Year.HasValue ?
                new ObjectParameter("Data_Year", data_Year) :
                new ObjectParameter("Data_Year", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("HistoricAcumGoods", data_YearParameter);
        }
    
        public virtual ObjectResult<InputsOutputs_Result> InputsOutputs(Nullable<int> good_Id)
        {
            var good_IdParameter = good_Id.HasValue ?
                new ObjectParameter("Good_Id", good_Id) :
                new ObjectParameter("Good_Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<InputsOutputs_Result>("InputsOutputs", good_IdParameter);
        }
    
        public virtual ObjectResult<Nullable<int>> LastBillSetteleds()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Nullable<int>>("LastBillSetteleds");
        }
    
        public virtual ObjectResult<LiniesConformes_Result> LiniesConformes(Nullable<int> customerOrder_Id)
        {
            var customerOrder_IdParameter = customerOrder_Id.HasValue ?
                new ObjectParameter("CustomerOrder_Id", customerOrder_Id) :
                new ObjectParameter("CustomerOrder_Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LiniesConformes_Result>("LiniesConformes", customerOrder_IdParameter);
        }
    
        public virtual ObjectResult<LiniesProveidorConformes_Result> LiniesProveidorConformes(Nullable<int> providerOrder_Id)
        {
            var providerOrder_IdParameter = providerOrder_Id.HasValue ?
                new ObjectParameter("ProviderOrder_Id", providerOrder_Id) :
                new ObjectParameter("ProviderOrder_Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<LiniesProveidorConformes_Result>("LiniesProveidorConformes", providerOrder_IdParameter);
        }
    
        public virtual ObjectResult<string> ProviderOrderMovementsComments(Nullable<int> providerOrder_Id)
        {
            var providerOrder_IdParameter = providerOrder_Id.HasValue ?
                new ObjectParameter("ProviderOrder_Id", providerOrder_Id) :
                new ObjectParameter("ProviderOrder_Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<string>("ProviderOrderMovementsComments", providerOrder_IdParameter);
        }
    
        public virtual ObjectResult<QueryOrdersProvider_Result> QueryOrdersProvider(Nullable<System.DateTime> dateInit, Nullable<System.DateTime> dateEnd, Nullable<int> provider_Id, Nullable<int> good_Id)
        {
            var dateInitParameter = dateInit.HasValue ?
                new ObjectParameter("DateInit", dateInit) :
                new ObjectParameter("DateInit", typeof(System.DateTime));
    
            var dateEndParameter = dateEnd.HasValue ?
                new ObjectParameter("DateEnd", dateEnd) :
                new ObjectParameter("DateEnd", typeof(System.DateTime));
    
            var provider_IdParameter = provider_Id.HasValue ?
                new ObjectParameter("Provider_Id", provider_Id) :
                new ObjectParameter("Provider_Id", typeof(int));
    
            var good_IdParameter = good_Id.HasValue ?
                new ObjectParameter("Good_Id", good_Id) :
                new ObjectParameter("Good_Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<QueryOrdersProvider_Result>("QueryOrdersProvider", dateInitParameter, dateEndParameter, provider_IdParameter, good_IdParameter);
        }
    
        public virtual ObjectResult<Ranges_Result> Ranges(string good_Id_From, string good_Id_Until, string bill_Id_From, string bill_Id_Until, Nullable<decimal> year_Query)
        {
            var good_Id_FromParameter = good_Id_From != null ?
                new ObjectParameter("Good_Id_From", good_Id_From) :
                new ObjectParameter("Good_Id_From", typeof(string));
    
            var good_Id_UntilParameter = good_Id_Until != null ?
                new ObjectParameter("Good_Id_Until", good_Id_Until) :
                new ObjectParameter("Good_Id_Until", typeof(string));
    
            var bill_Id_FromParameter = bill_Id_From != null ?
                new ObjectParameter("Bill_Id_From", bill_Id_From) :
                new ObjectParameter("Bill_Id_From", typeof(string));
    
            var bill_Id_UntilParameter = bill_Id_Until != null ?
                new ObjectParameter("Bill_Id_Until", bill_Id_Until) :
                new ObjectParameter("Bill_Id_Until", typeof(string));
    
            var year_QueryParameter = year_Query.HasValue ?
                new ObjectParameter("Year_Query", year_Query) :
                new ObjectParameter("Year_Query", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Ranges_Result>("Ranges", good_Id_FromParameter, good_Id_UntilParameter, bill_Id_FromParameter, bill_Id_UntilParameter, year_QueryParameter);
        }
    
        public virtual int RemoveWarehouseMovements()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("RemoveWarehouseMovements");
        }
    
        public virtual ObjectResult<Revisions_Result> Revisions(Nullable<int> operation_Type)
        {
            var operation_TypeParameter = operation_Type.HasValue ?
                new ObjectParameter("Operation_Type", operation_Type) :
                new ObjectParameter("Operation_Type", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Revisions_Result>("Revisions", operation_TypeParameter);
        }
    
        public virtual ObjectResult<Settlements_Result> Settlements(Nullable<int> agent_Id, string bill_Id_From, string bill_Id_Until, Nullable<decimal> year_Query)
        {
            var agent_IdParameter = agent_Id.HasValue ?
                new ObjectParameter("Agent_Id", agent_Id) :
                new ObjectParameter("Agent_Id", typeof(int));
    
            var bill_Id_FromParameter = bill_Id_From != null ?
                new ObjectParameter("Bill_Id_From", bill_Id_From) :
                new ObjectParameter("Bill_Id_From", typeof(string));
    
            var bill_Id_UntilParameter = bill_Id_Until != null ?
                new ObjectParameter("Bill_Id_Until", bill_Id_Until) :
                new ObjectParameter("Bill_Id_Until", typeof(string));
    
            var year_QueryParameter = year_Query.HasValue ?
                new ObjectParameter("Year_Query", year_Query) :
                new ObjectParameter("Year_Query", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Settlements_Result>("Settlements", agent_IdParameter, bill_Id_FromParameter, bill_Id_UntilParameter, year_QueryParameter);
        }
    
        public virtual ObjectResult<StockTaking_Result> StockTaking(string good_Id_From, string good_Id_Until)
        {
            var good_Id_FromParameter = good_Id_From != null ?
                new ObjectParameter("Good_Id_From", good_Id_From) :
                new ObjectParameter("Good_Id_From", typeof(string));
    
            var good_Id_UntilParameter = good_Id_Until != null ?
                new ObjectParameter("Good_Id_Until", good_Id_Until) :
                new ObjectParameter("Good_Id_Until", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<StockTaking_Result>("StockTaking", good_Id_FromParameter, good_Id_UntilParameter);
        }
    }
}
