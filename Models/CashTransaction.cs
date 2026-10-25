using System;

namespace MajesticDesktop.Models
{
    public class CashTransaction
    {
        public int Id { get; set; }
        public DateTime TransactionDate { get; set; }
        public string TransactionType { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public string ReferenceNumber { get; set; }
    }
}