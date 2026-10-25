namespace MajesticDesktop.Models
{
    public class SaleReturnDetail
    {
        public int ProductId { get; set; }
        public int ReturnedQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
    }
}