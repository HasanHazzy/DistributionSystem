using DataAccess.Generic;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace DataAccess.Respositories
{
    class ProductRepository : IProductRepository
    {
        public Task<Result> DeleteProduct(string ProductId)
        {
            throw new NotImplementedException();
        }

        public Task<Result> GetProductById(string ProductId)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> GetProducts()
        {
            try
            {
                using (var context = new FalconTraderContext())
                {
                    var Data = await(from product in context.Products
                                     
                                     select new Products
                                     {
                                         Itemid=product.Itemid,
                                         ProductCode = product.ProductCode,
                                         Itemdescp = product.Itemdescp,
                                         UnitPrice = product.UnitPrice,

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

        public Task<Result> InsertProduct(Products product_)
        {
            throw new NotImplementedException();
        }

        public Task<Result> UpdateProduct(Products product_)
        {
            throw new NotImplementedException();
        }
    }
}
