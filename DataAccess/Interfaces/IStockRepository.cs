﻿using DataAccess.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    interface IStockRepository
    {
        Task<Result> GetStock();
    }
}