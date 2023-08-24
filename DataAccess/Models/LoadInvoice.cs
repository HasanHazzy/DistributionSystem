using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class LoadInvoice
    {
        public LoadInvoice()
        {
            LoadInvoiceDetail = new HashSet<LoadInvoiceDetail>();
            StockOut = new HashSet<StockOut>();
            StockReturn = new HashSet<StockReturn>();
        }

        public int Id { get; set; }
        public int? FkRouteId { get; set; }
        public string VehicleName { get; set; }
        public string VehicleNo { get; set; }
        public string DeliveryMan { get; set; }
        public double? InvoiceTotal { get; set; }
        public double? DiscountFoc { get; set; }
        public double? DiscountHth { get; set; }
        public double? DiscountRegular { get; set; }
        public double? Tax { get; set; }
        public DateTime? Date { get; set; }
        public int? Status { get; set; }
        public int? FkTaxId { get; set; }

        public virtual Route FkRoute { get; set; }
        public virtual Tax FkTax { get; set; }
        public virtual ICollection<LoadInvoiceDetail> LoadInvoiceDetail { get; set; }
        public virtual ICollection<StockOut> StockOut { get; set; }
        public virtual ICollection<StockReturn> StockReturn { get; set; }
    }
}
