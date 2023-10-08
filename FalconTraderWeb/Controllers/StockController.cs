using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Generic;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Respositories;
using Microsoft.AspNetCore.Mvc;

namespace FalconTraderWeb.Controllers
{
    public class StockController : Controller
    {

        private readonly IStockRepository Repo;
        public StockController()
        {
            Repo = new StockRepository();

        }
        public IActionResult StockReturn()
        {
            return View();
        }

        public IActionResult StockIn()
        {
            return View();
        }

        public async Task<IActionResult> Stocks()
        {
            var GetStocks = await Repo.GetStock();
            if (GetStocks.Status == ResultStatus.Success)
            {
                ViewBag.Stocks = GetStocks.Data;
                return View();
            }
            else
            {
                return View();
            }

        }


        public async Task<IActionResult> StockQuantity()
        {
            var GetStockIn = await Repo.GetCurrentStock();
            if (GetStockIn.Status==ResultStatus.Success)
            {
                return View(GetStockIn.Data);
            }
            else
            {
                return View();
            }
           
        }

        [HttpPost]
        public async Task<IActionResult> StockIn(DateTime startdate, DateTime enddate)
        {
            var GetStockIn = await Repo.GetStocks(startdate,enddate);
            if (GetStockIn.Status == ResultStatus.Success)
            {
                return View(GetStockIn.Data);
            }
            else
            {
                return View();
            }

        }

        public IActionResult StockOut()
        {
              return View();
            
        }

        [HttpPost]
        public async Task<IActionResult> StockOut(DateTime startdate,DateTime enddate)
        {
            var GetStockOut = await Repo.GetStocksOut(startdate, enddate);
            if (GetStockOut.Status == ResultStatus.Success)
            {
                return View(GetStockOut.Data);
            }
            else
            {
                return View();
            }

        }
        [HttpPost]
        public async Task<Result> StockGetDropDownData()
        {
            var DropDownData = await Repo.GetDropDownData();
            return DropDownData;

        }

        [HttpPost]
        public async Task<Result> SaveStockReturn([FromBody] List<StockReturn> _StockReturn)
        {
            var Result = await Repo.SaveStockReturn(_StockReturn);
            return Result;

        }

        [HttpPost]
        public async Task<Result> SaveStocks([FromBody] Stock _Stock)
        {
            var Result = await Repo.SaveStocks(_Stock);
            return Result;

        }

        [HttpPost]
        public async Task<Result> UpdateStock([FromBody] Stock _Stock)
        {
            var Result = await Repo.UpdateStock(_Stock);
            return Result;

        }

        [HttpPost]
        public async Task<Result> DeleteStock([FromBody] Stock _Stock)
        {
            var Result = await Repo.DeleteStock(_Stock);
            return Result;

        }
    }
}