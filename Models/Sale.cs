using System;
using System.Collections.Generic;

namespace MajesticDesktop.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public int? CustomerId { get; set; }
        public int LocationId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal Discount { get; set; }
        public decimal NetAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public string PaymentMethod { get; set; }

        // الخاصية الجديدة اللي كانت ناقصة وعملت الخطأ
        public string Notes { get; set; }

        public List<SaleDetail> Details { get; set; }

        public Sale()
        {
            Details = new List<SaleDetail>();
        }
    }
}