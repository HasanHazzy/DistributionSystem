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
   public class TaxRepository : ITax
    {
        public async Task<Result> GetTax()
        {
            try
            {
                using (var context = new FalconTraderContext())
                {
                    var Result = await(from Tax in context.Tax
                                    
                                       select new Tax
                                       {
                                           Id=Tax.Id,
                                           TaxDescp=Tax.TaxDescp,
                                           Percentage=Tax.Percentage
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

        public async Task<Result> SaveTax(Tax _Tax)
        {
            try
            {
                using (var context = new FalconTraderContext())
                {
                    context.Tax.Add(_Tax);
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

        public async Task<Result> UpdateTax(Tax _Tax)
        {
            try
            {
                using (var context = new FalconTraderContext())
                {
                    context.Tax.Update(_Tax);
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
