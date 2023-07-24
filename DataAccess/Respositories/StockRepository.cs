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
    public class StockRepository : IStockRepository
    {
        public async Task<Result> GetStock()
        {
            try
            {
                using (var context = new FalconTraderContext())
                {
                    var Data = await(from stock in context.Stock
                                    

                                     select new Stock
                                     {
                                         Id=stock.Id,
                                         Descp = stock.Descp,
                                     
                                     }).ToListAsync();

                    return new Result() { Status = ResultStatus.Success, Message = "Success", Data = Data };

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
