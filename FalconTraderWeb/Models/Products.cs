using System;
using System.Collections.Generic;

namespace FalconTraderWeb.Models
{
    public partial class Products
    {
        public Products()
        {
            PurchaseInvoiceDetail = new HashSet<PurchaseInvoiceDetail>();
            StockIn = new HashSet<StockIn>();
            StockOut = new HashSet<StockOut>();
            StockProducts = new HashSet<StockProducts>();
            StockReturn = new HashSet<StockReturn>();
        }

        public int Itemid { get; set; }
        public string ProductCode { get; set; }
        public string Itemdescp { get; set; }
        public double? UnitPrice { get; set; }

        public virtual ICollection<PurchaseInvoiceDetail> PurchaseInvoiceDetail { get; set; }
        public virtual ICollection<StockIn> StockIn { get; set; }
        public virtual ICollection<StockOut> StockOut { get; set; }
        public virtual ICollection<StockProducts> StockProducts { get; set; }
        public virtual ICollection<StockReturn> StockReturn { get; set; }
    }
}
