using System;

namespace MajesticDesktop.Models
{
    public class ShiftClosing
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime ClosingDate { get; set; }
        public decimal ExpectedAmount { get; set; }
        public decimal ActualAmount { get; set; }
        public decimal Difference { get; set; }
        public string Notes { get; set; }
    }
}