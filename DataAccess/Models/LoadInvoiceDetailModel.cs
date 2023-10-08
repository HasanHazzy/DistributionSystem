using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class LoadInvoiceDetailModel
    {
        public int Id { get; set; }

        public string ProductName { get; set; }

        public string ProductCode { get; set; }
        public int? FkLoadInvoiceId { get; set; }
        public int? FkItemId { get; set; }
        public int? Quantity { get; set; }
        public double? Unitcost { get; set; }
        public double? Total { get; set; }
        public double? Margin { get; set; }
        public DateTime? Date { get; set; }
        public int? Status { get; set; }
        public int? FkStockId { get; set; }
    }
}
