using System;
using System.Collections.Generic;

namespace FalconTraderWeb.Models
{
    public partial class TblDiscount
    {
        public TblDiscount()
        {
            PurchaseInvoiceDetail = new HashSet<PurchaseInvoiceDetail>();
        }

        public int Id { get; set; }
        public string Descp { get; set; }
        public decimal? Percentage { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<PurchaseInvoiceDetail> PurchaseInvoiceDetail { get; set; }
    }
}
