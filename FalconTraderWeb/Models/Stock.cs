using System;
using System.Collections.Generic;

namespace FalconTraderWeb.Models
{
    public partial class Stock
    {
        public Stock()
        {
            StockIn = new HashSet<StockIn>();
            StockOut = new HashSet<StockOut>();
            StockProducts = new HashSet<StockProducts>();
            StockReturn = new HashSet<StockReturn>();
        }

        public int Id { get; set; }
        public string Descp { get; set; }
        public string Symbol { get; set; }

        public virtual ICollection<StockIn> StockIn { get; set; }
        public virtual ICollection<StockOut> StockOut { get; set; }
        public virtual ICollection<StockProducts> StockProducts { get; set; }
        public virtual ICollection<StockReturn> StockReturn { get; set; }
    }
}
