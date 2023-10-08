using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class StockReturn
    {
        public int ReturnId { get; set; }
        public int? FkStockId { get; set; }
        public int? FkLoadInvoiceId { get; set; }
        public int? FkItemId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Note { get; set; }
        public int? Status { get; set; }

        public virtual Products FkItem { get; set; }
        public virtual LoadInvoice FkLoadInvoice { get; set; }
        public virtual Stock FkStock { get; set; }
    }
}
