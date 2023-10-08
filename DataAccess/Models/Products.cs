using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccess.Models
{
    public partial class Products
    {
        public Products()
        {
            LoadInvoiceDetail = new HashSet<LoadInvoiceDetail>();
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
        public double? Margin { get; set; }
        public int? Status { get; set; }


        [NotMapped]
        public int? CurrentStock { get; set; }

        [NotMapped]
        public int? FK_StockId { get; set; }

        public virtual ICollection<LoadInvoiceDetail> LoadInvoiceDetail { get; set; }
        public virtual ICollection<PurchaseInvoiceDetail> PurchaseInvoiceDetail { get; set; }
        public virtual ICollection<StockIn> StockIn { get; set; }
        public virtual ICollection<StockOut> StockOut { get; set; }
        public virtual ICollection<StockProducts> StockProducts { get; set; }
        public virtual ICollection<StockReturn> StockReturn { get; set; }
    }
}
