using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Tax
    {
        public Tax()
        {
            LoadInvoice = new HashSet<LoadInvoice>();
        }

        public int Id { get; set; }
        public string TaxDescp { get; set; }
        public decimal? Percentage { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<LoadInvoice> LoadInvoice { get; set; }
    }
}
