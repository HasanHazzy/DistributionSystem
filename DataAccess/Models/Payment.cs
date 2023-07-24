using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Payment
    {
        public string CustomerId { get; set; }
        public double? TotalPurchase { get; set; }
        public double? TotalPaid { get; set; }
        public double? CurrentBalance { get; set; }
        public double? LastPayment { get; set; }
    }
}
