using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Models
{
    public class DropDownData
    {
        public List<Products> Products { get; set; }
        public List<Route> Routes { get; set; }
        public List<Stock> Stock { get; set; }
        public List<Tax> Tax { get; set; }
        public List<TblDiscount> Discount { get; set; }
    }
}
