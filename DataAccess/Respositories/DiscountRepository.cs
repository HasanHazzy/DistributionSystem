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
                                            Descp = Discount.Descp,
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

        public async Task<Result> SaveDiscount(TblDiscount _Discount)
        {
            try
            {
                using (var context = new FalconTraderContext())
                {
                    _Discount.Status = 0;
                    context.TblDiscount.Add(_Discount);
                    int result = await context.SaveChangesAsync();

                    if (result > 0)
                        return new Result() { Status = ResultStatus.Success, Message = "Success", Data = "" };
                    else
                        return new Result() { Status = ResultStatus.Error, Message = "Error" };
                }
            }

            catch (Exception ex)
            {
                return new Result() { Status = ResultStatus.Error, Message = ex.Message, Data = "" };
            }
        }


        public async Task<Result> UpdateDiscount(TblDiscount _Discount)
        {
            try
            {
                using (var context = new FalconTraderContext())
                {
                    _Discount.Status = 0;
                    context.TblDiscount.Update(_Discount);
                    int result = await context.SaveChangesAsync();

                    if (result > 0)
                        return new Result() { Status = ResultStatus.Success, Message = "Success", Data = "" };
                    else
                        return new Result() { Status = ResultStatus.Error, Message = "Error" };
                }

            }
            catch (Exception ex)
            {
                return new Result() { Status = ResultStatus.Error, Message = ex.Message, Data = "" };

            }
        }
    }
}

    

