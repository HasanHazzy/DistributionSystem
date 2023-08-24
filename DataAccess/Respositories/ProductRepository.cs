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
   public class ProductRepository : IProductRepository
    {
    

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
                                     where product.Status == 0
                                     select new Products
                                     {
                                         Itemid=product.Itemid,
                                         ProductCode = product.ProductCode,
                                         Itemdescp = product.Itemdescp,
                                         UnitPrice = product.UnitPrice,
                                         Margin=product.Margin
                                         
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

        public async Task<Result> GetStockProducts()
        {
            try
            {
                using (var context = new FalconTraderContext())
                {
                    var Data = await (from product in context.Products
                                      join stock in context.StockProducts on product.Itemid equals stock.FkItemId
                                      where product.Status==0
                                      select new Products
                                      {
                                          Itemid = product.Itemid,
                                          ProductCode = product.ProductCode,
                                          Itemdescp = product.Itemdescp,
                                          UnitPrice = product.UnitPrice,
                                          FK_StockId = stock.FkStockId,
                                          CurrentStock = stock.Quantity,

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

        public async Task<Result> InsertProduct(Products _product)
        {
            try
            {
                using (var context = new FalconTraderContext())
                {
                    var ExistProduct = await context.Products.FirstOrDefaultAsync(sp => sp.ProductCode == _product.ProductCode && sp.Status==0);
                    if (ExistProduct != null)
                    {

                        return new Result() { Status = ResultStatus.Error, Message = "Product is Already Exist on this Code!" };


                    }
                    _product.Status = 0;
                    context.Products.Add(_product);
                    context.Entry(_product).State = EntityState.Added;
                    int result = await context.SaveChangesAsync();

                    if (result > 0)
                        return new Result() { Status = ResultStatus.Success, Message = "Success", Data = "" };
                    else
                        return new Result() { Status = ResultStatus.Error, Message = "Error" };
                }

            }
            catch (Exception ex)
            {
                return new Result() { Status = ResultStatus.Error, Message = ex.Message, Data = "" };

            }
        }

        public async Task<Result> UpdateProduct(Products _product)
        {
            try
            {
                using (var context = new FalconTraderContext())
                {
                    _product.Status = 0;
                     context.Products.Update(_product);
                    int result = await context.SaveChangesAsync();

                    if (result > 0)
                        return new Result() { Status = ResultStatus.Success, Message = "Success", Data = "" };
                    else
                        return new Result() { Status = ResultStatus.Error, Message = "Error" };
                }

            }
            catch (Exception ex)
            {
                return new Result() { Status = ResultStatus.Error, Message = ex.Message, Data = "" };

            }
        }

        public async Task<Result> DeleteProduct(int ItemId)
        {
            try
            {
                using (var context = new FalconTraderContext())
                {
                    var productToUpdate = await context.Products.Where(x => x.Itemid == ItemId).FirstOrDefaultAsync();

                    if (productToUpdate != null)
                    {
                        productToUpdate.Status = 1;
                        context.Entry(productToUpdate).State = EntityState.Modified;
                        int result = await context.SaveChangesAsync();

                        if (result > 0)
                            return new Result() { Status = ResultStatus.Success, Message = "Product Deleted Successfully", Data = "" };
                        else
                            return new Result() { Status = ResultStatus.Error, Message = "Error updating status" };
                    }
                    else
                    {
                        return new Result() { Status = ResultStatus.Error, Message = "Product Not Found" };
                    }
                }
            }
            catch (Exception ex)
            {
                return new Result() { Status = ResultStatus.Error, Message = ex.Message, Data = "" };
            }
        }
    }
}
