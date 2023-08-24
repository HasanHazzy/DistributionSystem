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

                    var ExistCokeInvoice = await context.PurchaseInvoice.FirstOrDefaultAsync(sp => sp.CokeInvoice == _Purchase.CokeInvoice);
                    if (ExistCokeInvoice!=null)
                    {

                        return new Result() { Status = ResultStatus.Error, Message = "Coke Invoice Already Exist!" };


                    }

                    DateTime currentDate = DateTime.Now;
                    _Purchase.Date = currentDate;
                    _Purchase.Status = 0;
                    foreach (var detail in _Purchase.PurchaseInvoiceDetail)
                    {
                        detail.Time = currentDate;


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
                            var stockProduct = await context.StockProducts.FirstOrDefaultAsync(sp => sp.FkItemId == itemId && sp.FkStockId==StockId);
                            if (stockProduct != null)
                            {
                                stockProduct.Quantity += quantity;
                                stockProduct.DiscountAmount += invoiceDetail.DiscountAmount;
                                stockProduct.TaxAmount += invoiceDetail.TaxAmount;
                                stockProduct.GrossAmount += (invoiceDetail.Unitcost * invoiceDetail.Quantity);
                                stockProduct.NetAmount += invoiceDetail.Total;
                                await context.SaveChangesAsync();
                            }
                            else
                            {
                                var stock_Product = new StockProducts
                                {
                                    FkStockId = StockId,
                                    FkItemId = itemId,
                                    Quantity = quantity,
                                    TaxAmount = invoiceDetail.TaxAmount,
                                    DiscountAmount = invoiceDetail.DiscountAmount,
                                    GrossAmount= (invoiceDetail.Unitcost * invoiceDetail.Quantity),
                                    NetAmount=invoiceDetail.Total,
                                    Status = 1 
                                };

                                context.StockProducts.Add(stock_Product);
                            }

                            var stockInEntry = new StockIn
                            {
                                FkStockId = StockId,
                                ItemId = itemId,
                                Quantity = quantity,
                                StockInDate = currentDate,
                                FkPuchaseInvoiceId=_Purchase.Purchaseinvoiceid
                            };

                            context.StockIn.Add(stockInEntry);

                        }
                           await context.SaveChangesAsync();

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


        public async Task<Result> DeletePurchaseInvoice(int purchaseInvoiceId)
        {
            try
            {
                using (var context = new FalconTraderContext())
                {
                    var purchaseInvoice = await context.PurchaseInvoice
                        .Include(pi => pi.PurchaseInvoiceDetail)
                        .FirstOrDefaultAsync(pi => pi.Purchaseinvoiceid == purchaseInvoiceId);

                    if (purchaseInvoice == null)
                    {
                        return new Result() { Status = ResultStatus.Error, Message = "Purchase Invoice not found." };
                    }
                    else
                    {
                        purchaseInvoice.Status = 1;
                    }

                    //context.PurchaseInvoiceDetail.RemoveRange(purchaseInvoice.PurchaseInvoiceDetail);
                    context.PurchaseInvoice.Update(purchaseInvoice);

                    // Update Stock
                    foreach (var invoiceDetail in purchaseInvoice.PurchaseInvoiceDetail)
                    {
                        var itemId = invoiceDetail.Itemid;
                        var quantity = invoiceDetail.Quantity;
                        var stockId = invoiceDetail.FkStockId;
                        var stockProduct = await context.StockProducts.FirstOrDefaultAsync(sp => sp.FkItemId == itemId && sp.FkStockId == stockId);

                        if (stockProduct != null)
                        {
                            stockProduct.Quantity -= quantity;
                        }

                        
                    }

                    int result = await context.SaveChangesAsync();

                    if (result > 0)
                    {
                        return new Result() { Status = ResultStatus.Success, Message = "Purchase Invoice deleted successfully." };
                    }
                    else
                    {
                        return new Result() { Status = ResultStatus.Error, Message = "Error deleting Purchase Invoice." };
                    }
                }
            }
            catch (Exception ex)
            {
                return new Result() { Status = ResultStatus.Error, Message = ex.Message };
            }
        }

       

    }
}
