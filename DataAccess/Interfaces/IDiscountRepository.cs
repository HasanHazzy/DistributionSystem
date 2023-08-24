using DataAccess.Generic;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IDiscountRepository
    {
        Task<Result> GetDiscount();

        Task<Result> SaveDiscount(TblDiscount _Discount);

        Task<Result> UpdateDiscount(TblDiscount _Discount);
    }
}
