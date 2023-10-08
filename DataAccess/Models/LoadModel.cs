using System;

namespace DataAccess.Models
{
    public class LoadModel
    {
        public string DeliveryMan { get; set; }
        public string RouteName { get; set; }
        public int? LoadInvoiceNo { get; set; }
        public string ProductName { get; set; }
        public int? Quantity { get; set; }
        public int? Return { get; set; }
        public string VehicleNo { get; set; }

        public string BookerName { get; set; }
        public DateTime? Date { get; set; }

        public DateTime? LoadDate { get; set; }
    }
}