using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;

namespace DataAccess.Respositories
{
    public class DiscountRepository : IDiscountRepository
    {
        public async Task<Result> GetDiscount()
        {
                try
                {
                    using (var context = new FalconTraderContext())
                    {
                        var Result = await (from Discount in context.TblDiscount

                                            select new TblDiscount
                                            {
                                                Id = Discount.Id,
                                                Descp=Discount.Descp,
                                                Percentage = Discount.Percentage,
                                               
                                            }).ToListAsync();

                        return new Result() { Status = ResultStatus.Success, Message = "Success", Data = Result };
                    }
                }
                catch (Exception ex)
                {
                    return new Result()
                    {
                        Data = "Error",
                        Message = ex.Message,
                        Status = ResultStatus.Error
                    };
                }
            
        }
    }
}
