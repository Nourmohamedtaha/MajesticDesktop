using System;
using System.Collections.Generic;

namespace MajesticDesktop.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public int SupplierId { get; set; }
        public int LocationId { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal PaidAmount { get; set; }
        public decimal RemainingAmount { get; set; }
        public List<PurchaseDetail> Details { get; set; } = new List<PurchaseDetail>();
    }
}