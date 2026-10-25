using System;
using System.Collections.Generic;

namespace MajesticDesktop.Models
{
    public class Request
    {
        public int Id { get; set; }
        public int LocationId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; } // دي اللي كانت ناقصة وعاملة الإيرور

        // تجهيز الـ Details كليستة عشان متضربش Error
        public List<RequestDetail> Details { get; set; } = new List<RequestDetail>();
    }
}