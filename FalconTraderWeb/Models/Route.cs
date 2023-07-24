using System;
using System.Collections.Generic;

namespace FalconTraderWeb.Models
{
    public partial class Route
    {
        public Route()
        {
            PurchaseInvoice = new HashSet<PurchaseInvoice>();
        }

        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public string RouteDescp { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<PurchaseInvoice> PurchaseInvoice { get; set; }
    }
}
