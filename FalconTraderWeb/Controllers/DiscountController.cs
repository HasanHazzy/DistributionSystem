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
    public class DiscountController : Controller
    {
        private readonly IDiscountRepository Repo;

        public DiscountController()
        {
            Repo = new DiscountRepository();

        }


        public async Task<IActionResult> Discount()
        {
            var Result_ = await Repo.GetDiscount();
            if (Result_.Status == ResultStatus.Success)
            {
                ViewBag.Discount = Result_.Data;

            }
            return View();
        }

        [HttpPost]
        public async Task<Result> SaveDiscount([FromBody]TblDiscount _Discount)
        {
            var Result = await Repo.SaveDiscount(_Discount);
            return Result;
        }

        [HttpPost]
        public async Task<Result> UpdateDiscount([FromBody]TblDiscount _Discount)
        {
            var Result = await Repo.UpdateDiscount(_Discount);
            return Result;

        }
    }
}