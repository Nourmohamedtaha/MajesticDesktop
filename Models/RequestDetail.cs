namespace MajesticDesktop.Models
{
    public class RequestDetail
    {
        public int ProductId { get; set; }
        public int RequestedQuantity { get; set; }
        public int ApprovedQuantity { get; set; } // الكمية المعتمدة من المدير
    }
}