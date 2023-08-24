using DataAccess.Generic;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
   public interface ITax
    {
        Task<Result> GetTax();

        Task<Result> SaveTax(Tax _Tax);

        Task<Result> UpdateTax(Tax _Tax);

    }
}
