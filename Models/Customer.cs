namespace MajesticDesktop.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Region { get; set; }  // إضافة المنطقة
        public string Notes { get; set; }   // إضافة الملحوظة
        public decimal Balance { get; set; }
    }
}