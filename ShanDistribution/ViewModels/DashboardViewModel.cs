namespace ShanDistribution.ViewModels
{
    public class DashboardViewModel
    {
        // 5 Real Key Metric Cards
        public int TotalActiveCustomers { get; set; }
        public int TotalInvoicesCount { get; set; }
        public int ActiveProductsCount { get; set; }
        public int TotalActiveSuppliers { get; set; }
        public int TotalBillsCount { get; set; }

        // Real Database Lists
        public List<LowStockProductItem> LowStockProducts { get; set; } = new();
        public List<TopProductItem> TopSellingProducts { get; set; } = new();

        // Customer Analytics Lists
        public List<CustomerPerformanceItem> TopSellingCustomers { get; set; } = new();
        public List<CustomerPerformanceItem> LowSellingCustomers { get; set; } = new();
    }

    public class LowStockProductItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockCount { get; set; }
    }

    public class TopProductItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int TotalUnitsSold { get; set; }
        public decimal TotalRevenue { get; set; }
    }

    public class CustomerPerformanceItem
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public int TotalInvoices { get; set; }
    }
}