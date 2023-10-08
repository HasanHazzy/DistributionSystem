using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class LoadInvoiceModel
    {
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
        public DateTime? LoadDate { get; set; }
        public int? LoadInvoiceNo { get; set; }
        public string BookerName { get; set; }

        public List<LoadInvoiceDetailModel> LoadInvoiceDetail { get; set; }

    }
}
