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
    public class ProductController : Controller
    {
        private readonly IProductRepository Repo;

        public ProductController()
        {
            Repo = new ProductRepository();

        }
        public async Task<IActionResult> Product()
        {
            //var Result_ = await Repo.GetProducts();
            //if (Result_.Status == ResultStatus.Success)
            //{
            //    ViewBag.Products = Result_.Data;

            //}
            return View();
        }

        [HttpPost]
        public async Task<Result> GetProducts()
        {
            var Result = await Repo.GetProducts();
            return Result;
        }

        [HttpPost]
        public async Task<Result> SaveProduct([FromBody]Products Product)
        {
            
            var Result = await Repo.InsertProduct(Product);
            return Result;
        }

        [HttpPost]
        public async Task<Result> UpdateProduct([FromBody]Products Product)
        {
            var Result = await Repo.UpdateProduct(Product);
            return Result;

        }

        [HttpPost]
        public async Task<Result> DeleteProduct([FromBody]Products Product)
        {
            var Result = await Repo.DeleteProduct(Product.Itemid);
            return Result;

        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Product/Edit/5

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }
    }
}