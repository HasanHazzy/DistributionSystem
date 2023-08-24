using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class PurchaseInvoice
    {
        public PurchaseInvoice()
        {
            PurchaseInvoiceDetail = new HashSet<PurchaseInvoiceDetail>();
            StockIn = new HashSet<StockIn>();
        }

        public int Purchaseinvoiceid { get; set; }
        public double? Invoicetotal { get; set; }
        public DateTime? Date { get; set; }
        public string Phone { get; set; }
        public double? TotalDiscount { get; set; }
        public double? TotalTax { get; set; }
        public int? Status { get; set; }
        public string CokeInvoice { get; set; }
        public DateTime? DeliveryDate { get; set; }

        public virtual ICollection<PurchaseInvoiceDetail> PurchaseInvoiceDetail { get; set; }
        public virtual ICollection<StockIn> StockIn { get; set; }
    }
}
