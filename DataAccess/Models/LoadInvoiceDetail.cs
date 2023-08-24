using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class LoadInvoiceDetail
    {
        public int Id { get; set; }
        public int? FkLoadInvoiceId { get; set; }
        public int? FkItemId { get; set; }
        public int? Quantity { get; set; }
        public double? Unitcost { get; set; }
        public double? Total { get; set; }
        public double? Margin { get; set; }
        public DateTime? Date { get; set; }
        public int? Status { get; set; }
        public int? FkStockId { get; set; }

        public virtual Products FkItem { get; set; }
        public virtual LoadInvoice FkLoadInvoice { get; set; }
        public virtual Stock FkStock { get; set; }
    }
}
