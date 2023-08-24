using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DataAccess.Models;
using DataAccess.Respositories;
using DataAccess.Interfaces;
using DataAccess.Generic;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FalconTraderWeb.Controllers
{
   

    public class PurchaseController : Controller
    {
        private readonly IPurchaseRepository Repo;
        public PurchaseController()
        {
            Repo = new PurchaseRepository();

        }
        // GET: /<controller>/
        public IActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public ActionResult Purchase()
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
        public async Task<Result> SavePurchaseInvoice([FromBody] PurchaseInvoice _Purchase)
        {
            var Result = await Repo.SavePurchaseInvoice(_Purchase);
            return Result;

        }

        [HttpPost]
        public async Task<Result> DeletePurchaseInvoice([FromBody] PurchaseInvoice _Purchase)
        {
            var Result = await Repo.DeletePurchaseInvoice(_Purchase.Purchaseinvoiceid);
            return Result;

        }
    }
}
