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
    public class PurchaseRepository : IPurchaseRepository
    {
        private readonly IProductRepository Product_Repo;
        private readonly IDiscountRepository Discount_Repo;
        private readonly ITax Tax_Repo;
        private readonly IStockRepository Stock_Repo;
        private readonly IRoute Route_Repo;
        public PurchaseRepository()
        {
            Product_Repo = new ProductRepository();
            Discount_Repo = new DiscountRepository();
            Tax_Repo = new TaxRepository();
            Stock_Repo = new StockRepository();
            Route_Repo = new RouteRepository();

        }
       
        public Task<Result> GetAllPurchases(DateTime fromDate, DateTime ToDate)
        {
            throw new NotImplementedException();
        }

        public Task<Result> GetBill(int InvoiceNo)
        {
            throw new NotImplementedException();
        }

        public Task<Result> GetPurchases(DateTime filterdate)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> SavePurchaseInvoice(PurchaseInvoice _Purchase)
        {
            int generatedId = 0;
            try
            {
                using (var context = new FalconTraderContext())
                {
                    _Purchase.Date = DateTime.Now;
                    _Purchase.Status = 0;
                    foreach (var detail in _Purchase.PurchaseInvoiceDetail)
                    {
                        detail.Time = DateTime.Now;
                        
                    }
                    context.PurchaseInvoice.Add(_Purchase);
                    context.Entry(_Purchase).State = EntityState.Added;

                    int result = await context.SaveChangesAsync();

                    if (result > 0)
                    {

                        //Update Stock
                        foreach (var invoiceDetail in _Purchase.PurchaseInvoiceDetail)
                        {
                            var itemId = invoiceDetail.Itemid;
                            var quantity = invoiceDetail.Quantity;
                            var StockId = invoiceDetail.FkStockId;
                            var stockProduct = await context.StockProducts.FirstOrDefaultAsync(sp => sp.FkItemId == itemId);
                            if (stockProduct != null)
                            {
                                stockProduct.Quantity += quantity;
                                await context.SaveChangesAsync();
                            }
                            else
                            {
                                var stock_Product = new StockProducts
                                {
                                    FkStockId = StockId, // Set the appropriate stock ID here
                                    FkItemId = itemId,
                                    Quantity = quantity,
                                    Status = 1 // Set the appropriate status based on your logic
                                };

                                context.StockProducts.Add(stock_Product);
                               await context.SaveChangesAsync();
                            }
                        }

                        return new Result() { Status = ResultStatus.Success, Message = "Success", Data = generatedId };

                    }
                    else
                    {
                        return new Result() { Status = ResultStatus.Error, Message = "Error" };
                    }
                }

            }
            catch (Exception ex)
            {
                return new Result() { Status = ResultStatus.Error, Message = ex.Message, Data = "" };

            }
        }

        public async Task<Result> GetDropDownData()
        {
            try
            {
                DropDownData obj = new DropDownData();
                var Products=await Product_Repo.GetProducts();
                if (Products.Status==ResultStatus.Success)
                {
                    obj.Products = Products.Data;
                }
                var Discount = await Discount_Repo.GetDiscount();

                if (Discount.Status == ResultStatus.Success)
                {
                    obj.Discount = Discount.Data;
                }

                var Tax = await Tax_Repo.GetTax();

                if (Tax.Status == ResultStatus.Success)
                {
                    obj.Tax =Tax.Data;
                }

                var Stock = await Stock_Repo.GetStock();
                if (Stock.Status==ResultStatus.Success)
                {
                    obj.Stock = Stock.Data;

                }
                var Route = await Route_Repo.GetRoutes();
                if (Route.Status == ResultStatus.Success)
                {
                    obj.Routes = Route.Data;

                }

                return new Result() { Status = ResultStatus.Success, Message = "Success", Data = obj };

            }
            catch (Exception ex)
            {
                return new Result() { Status = ResultStatus.Error, Message = ex.Message, Data = "" };

            }
        }

    }
}
