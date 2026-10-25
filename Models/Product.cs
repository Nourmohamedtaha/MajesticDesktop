namespace MajesticDesktop.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public decimal PurchasePrice { get; set; }
        public string ImagePath { get; set; }
        public bool IsActive { get; set; }
        public int AlertLimit { get; set; }
        public decimal CurrentQuantity { get; set; } // اتغيرت لـ decimal لدعم الكسور
        public string Notes { get; set; } // الملاحظات الجديدة
    }
}