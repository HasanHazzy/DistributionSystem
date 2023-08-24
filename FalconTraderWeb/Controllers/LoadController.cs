using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Models;
using DataAccess.Respositories;
using DataAccess.Interfaces;
using DataAccess.Generic;

namespace FalconTraderWeb.Controllers
{
    public class LoadController : Controller
    {
        private readonly ILoadRepository Repo;
        public LoadController()
        {
            Repo = new LoadRepository();

        }
        public IActionResult Load()
        {
            return View();
        }

        public IActionResult LoadDetails()
        {
            return View();
        }
        [HttpPost]
        public async Task<Result> GetDropDownData()
        {
            var DropDownData = await Repo.GetDropDownData();
            return DropDownData;

        }
        [HttpPost]
        public async Task<Result> SaveLoadInvoice([FromBody]LoadInvoice LoadMaster)
        {
            var Result = await Repo.SaveLoadInvoice(LoadMaster);
            return Result;

        }

        [HttpPost]
        public async Task<IActionResult> LoadDetails(DateTime startdate, DateTime enddate)
        {
            var GetLoad = await Repo.GetLoadDetails(startdate, enddate);
            if (GetLoad.Status == ResultStatus.Success)
            {
                return View(GetLoad.Data);
            }
            else
            {
                return View();
            }

        }
    }
}