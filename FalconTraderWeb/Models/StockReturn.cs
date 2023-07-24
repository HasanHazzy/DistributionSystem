using System;
using System.Collections.Generic;

namespace FalconTraderWeb.Models
{
    public partial class StockReturn
    {
        public int ReturnId { get; set; }
        public int? FkStockId { get; set; }
        public int? FkPurcharseInvoiceId { get; set; }
        public int? FkItemId { get; set; }
        public int? Quantity { get; set; }
        public int? Status { get; set; }

        public virtual Products FkItem { get; set; }
        public virtual PurchaseInvoice FkPurcharseInvoice { get; set; }
        public virtual Stock FkStock { get; set; }
    }
}
