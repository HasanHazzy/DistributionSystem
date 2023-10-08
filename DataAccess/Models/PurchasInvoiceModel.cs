using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class PurchaseInvoiceModel
    {
        public int Purchaseinvoiceid { get; set; }
        public double? Invoicetotal { get; set; }
        public DateTime? Date { get; set; }
        public string Phone { get; set; }
        public double? TotalDiscount { get; set; }
        public double? TotalTax { get; set; }
        public int? Status { get; set; }
        public string CokeInvoice { get; set; }
        public DateTime? DeliveryDate { get; set; }

        public List<PurchaseInvoiceDetailModel> PurchaseInvoiceDetail { get; set; }
    }

    public class PurchaseInvoiceDetailModel
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
        public int? Status { get; set; }
    }

}
