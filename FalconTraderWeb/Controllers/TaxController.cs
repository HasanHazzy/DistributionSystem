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
    public class TaxController : Controller
    {

        private readonly ITax Repo;

        public TaxController()
        {
            Repo = new TaxRepository();

        }

        
        public async Task<IActionResult> Tax()
        {
            var Result_ = await Repo.GetTax();
            if (Result_.Status == ResultStatus.Success)
            {
                ViewBag.Tax = Result_.Data;

            }
            return View();
        }

        [HttpPost]
        public async Task<Result> SaveTax([FromBody]Tax Tax)
        {
            var Result = await Repo.SaveTax(Tax);
            return Result;
        }

        [HttpPost]
        public async Task<Result> UpdateTax([FromBody]Tax tax)
        {
            var Result = await Repo.UpdateTax(tax);
            return Result;

        }

    }
}