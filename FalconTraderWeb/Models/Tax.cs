using System;
using System.Collections.Generic;

namespace FalconTraderWeb.Models
{
    public partial class Tax
    {
        public Tax()
        {
            PurchaseInvoiceDetail = new HashSet<PurchaseInvoiceDetail>();
        }

        public decimal Id { get; set; }
        public string TaxDescp { get; set; }
        public decimal? Percentage { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<PurchaseInvoiceDetail> PurchaseInvoiceDetail { get; set; }
    }
}
