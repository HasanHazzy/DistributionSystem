﻿using DataAccess.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    interface ITax
    {
        Task<Result> GetTax();

    }
}
