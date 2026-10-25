namespace MajesticDesktop.Models
{
    public class BranchStockItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalValue { get; set; }
    }
}