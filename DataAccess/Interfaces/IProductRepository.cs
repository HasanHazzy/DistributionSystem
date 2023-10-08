using DataAccess.Generic;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IProductRepository
    {
        Task<Result> GetProducts();
        Task<Result> GetStockProducts();
        Task<Result> GetProductById(string ProductId);
        Task<Result> InsertProduct(Products product_);
        Task<Result> UpdateProduct(Products product_);
        Task<Result> DeleteProduct(int ItemId);



    }
}
