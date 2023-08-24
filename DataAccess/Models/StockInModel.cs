using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class StockInModel
    {
        public int PurchaseInvoiceNo { get; set; }

        public string CokeInvoice { get; set; }
        public string StockName { get; set; }
        public string ProductName { get; set; }
        public int? Quantity { get; set; }

        public double? DiscountAmount { get; set; }

        public double? TaxAmount { get; set; }
        public double? Total { get; set; }
        public DateTime? StockIn_Date { get; set; }

        public DateTime? DeliveryDate { get; set; }
    }

    public class StockOutModel
    {
        public int LoadInvoiceNo { get; set; }
        public string StockName { get; set; }
        public string ProductName { get; set; }
        public int? Quantity { get; set; }

        public double? DiscountFOCAmount { get; set; }
        public double? DiscountHTHAmount { get; set; }

        public double? DiscountRegular { get; set; }

        public double? TaxAmount { get; set; }
        public double? LoadInvoiceDetailTotal { get; set; }

        public double? LoadInvoiceTotal { get; set; }

        public double? Margin { get; set; }

        public int? Return { get; set; }
        public DateTime? StockOut_Date { get; set; }
    }

    public class StockQuantity
    {
        public string StockName { get; set; }

        public string ProductCode { get; set; }

        public string ProductName { get; set; }

        public double? unitPrice { get; set; }

        public int? Quantity { get; set; }

        public double? GrossAmount { get; set; }

        public double? NetAmount { get; set; }

        public double? TotalTax { get; set; }

        public double? TotalDiscount { get; set; }



    }
}
