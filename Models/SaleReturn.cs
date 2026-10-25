using System;
using System.Collections.Generic;

namespace MajesticDesktop.Models
{
    public class SaleReturn
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime ReturnDate { get; set; }
        public decimal TotalRefundAmount { get; set; }
        public string Notes { get; set; }
        public int LocationId { get; set; }
        public List<SaleReturnDetail> Details { get; set; } = new List<SaleReturnDetail>();
    }
}