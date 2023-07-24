using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class TblDiscount
    {
        public int Id { get; set; }
        public string Descp { get; set; }
        public decimal? Percentage { get; set; }
        public int? Status { get; set; }
    }
}
