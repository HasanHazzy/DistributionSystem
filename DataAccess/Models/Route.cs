using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class Route
    {
        public int RouteId { get; set; }
        public string RouteName { get; set; }
        public string RouteDescp { get; set; }
        public int? Status { get; set; }
    }
}
