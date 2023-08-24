using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class PurchaseInvoiceDetail
    {
        public int PurchaseDetailId { get; set; }
        public int? FkPurchaseInvoiceId { get; set; }
        public int? Itemid { get; set; }
        public int? Quantity { get; set; }
        public double? Unitcost { get; set; }
        public double? Total { get; set; }
        public DateTime? Time { get; set; }
        public double? DiscountAmount { get; set; }
        public double? TaxAmount { get; set; }
        public int? FkStockId { get; set; }

        public virtual PurchaseInvoice FkPurchaseInvoice { get; set; }
        public virtual Stock FkStock { get; set; }
        public virtual Products Item { get; set; }
    }
}
