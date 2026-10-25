namespace MajesticDesktop.Models
{
    public class Stock
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; }

        public int LocationId { get; set; }

        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}