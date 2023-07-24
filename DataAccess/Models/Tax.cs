using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Tax
    {
        public decimal Id { get; set; }
        public string TaxDescp { get; set; }
        public decimal? Percentage { get; set; }
        public int? Status { get; set; }
    }
}
