using System;
using System.Collections.Generic;

namespace MajesticDesktop.Models
{
    public class IssueStock
    {
        public int FromLocationId { get; set; }
        public int ToLocationId { get; set; }
        public DateTime IssueDate { get; set; }
        public List<IssueStockDetail> Details { get; set; } = new List<IssueStockDetail>();
    }
}