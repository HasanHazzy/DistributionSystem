using System;
using System.Collections.Generic;

namespace FalconTraderWeb.Models
{
    public partial class PurchaseInvoice
    {
        public PurchaseInvoice()
        {
            PurchaseInvoiceDetail = new HashSet<PurchaseInvoiceDetail>();
            StockReturn = new HashSet<StockReturn>();
        }

        public int Purchaseinvoiceid { get; set; }
        public int? RouteId { get; set; }
        public string VehicleName { get; set; }
        public string VehicleNo { get; set; }
        public double? Invoicetotal { get; set; }
        public DateTime? Date { get; set; }
        public string Phone { get; set; }

        public virtual Route Route { get; set; }
        public virtual ICollection<PurchaseInvoiceDetail> PurchaseInvoiceDetail { get; set; }
        public virtual ICollection<StockReturn> StockReturn { get; set; }
    }
}
