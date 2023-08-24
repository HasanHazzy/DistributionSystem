using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Route
    {
        public Route()
        {
            LoadInvoice = new HashSet<LoadInvoice>();
        }

        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public string RouteDescp { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<LoadInvoice> LoadInvoice { get; set; }
    }
}
