using DataAccess.Generic;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
   public interface IStockRepository
    {
        Task<Result> GetStock();

        Task<Result> GetDropDownData();

        Task<Result> SaveStockReturn(List<StockReturn> Stock_Return);

        Task<Result> GetStocks(DateTime? startdate,DateTime? enddate);

        Task<Result> GetStocksOut(DateTime? datefrom, DateTime? dateend);

        Task<Result> GetCurrentStock();

        Task<Result> SaveStocks(Stock _Stock);

        Task<Result> UpdateStock(Stock _Stock);

        Task<Result> DeleteStock(Stock _Stock);


    }
}
