using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class StockIn
    {
        public int Id { get; set; }
        public int? FkStockId { get; set; }
        public int? ItemId { get; set; }
        public int? Quantity { get; set; }
        public DateTime? StockInDate { get; set; }

        public virtual Stock FkStock { get; set; }
        public virtual Products Item { get; set; }
    }
}
