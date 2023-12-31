﻿using System;
using System.Collections.Generic;

namespace DataAccess.Models
{
    public partial class StockOut
    {
        public int Id { get; set; }
        public int? FkStockId { get; set; }
        public int? ItemId { get; set; }
        public int? OutQuantity { get; set; }
        public DateTime? StockOutDate { get; set; }
        public int? FkLoadInvoiceId { get; set; }

        public virtual LoadInvoice FkLoadInvoice { get; set; }
        public virtual Stock FkStock { get; set; }
        public virtual Products Item { get; set; }
    }
}
