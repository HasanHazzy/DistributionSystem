using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Generic;
using DataAccess.Interfaces;
using DataAccess.Models;
using DataAccess.Respositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FalconTrader_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : ControllerBase
    {
        private readonly IPurchaseRepository Repo;

        public PurchaseController()
        {
            Repo = new PurchaseRepository();

        }

        [Route("SavePurchaseInvoice")]
        [HttpPost]
        public async Task<Result> SaveSalesInvoice([FromBody] PurchaseInvoice _Purchase)
        {
            var Result = await Repo.SavePurchaseInvoice(_Purchase);
            return Result;
        }

        [Route("GetDropDownData")]
        [HttpPost]
        public async Task<Result> GetDropDownData()
        {
            var Result = await Repo.GetDropDownData();
            return Result;
        }
    }
}