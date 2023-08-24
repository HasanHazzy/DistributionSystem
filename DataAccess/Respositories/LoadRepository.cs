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
    public class LoadRepository : ILoadRepository
    {
        private readonly IProductRepository Product_Repo;
        private readonly IDiscountRepository Discount_Repo;
        private readonly ITax Tax_Repo;
        private readonly IStockRepository Stock_Repo;
        private readonly IRoute Route_Repo;
        public LoadRepository()
        {
            Product_Repo = new ProductRepository();
            Discount_Repo = new DiscountRepository();
            Tax_Repo = new TaxRepository();
            Stock_Repo = new StockRepository();
            Route_Repo = new RouteRepository();

        }

        public Task<Result> GetAllLoads(DateTime filterdate)
        {
            throw new NotImplementedException();
        }

        public Task<Result> GetAllPurchases(DateTime fromDate, DateTime ToDate)
        {
            throw new NotImplementedException();
        }

        public Task<Result> GetBill(int InvoiceNo)
        {
            throw new NotImplementedException();
        }

        public async Task<Result> GetDropDownData()
        {
            try
            {
                DropDownData obj = new DropDownData();
                var Products = await Product_Repo.GetStockProducts();
                if (Products.Status == ResultStatus.Success)
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
                    obj.Tax = Tax.Data;
                }

                var Stock = await Stock_Repo.GetStock();
                if (Stock.Status == ResultStatus.Success)
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

        public Task<Result> GetPurchases(DateTime filterdate)
        {
            throw new NotImplementedException();
        } 

        public async Task<Result> SaveLoadInvoice(LoadInvoice _Load)
        {
            int generatedId = 0;
            try
            {
                using (var context = new FalconTraderContext())
                {
                    DateTime currentDate = DateTime.Now;
                    _Load.Date = currentDate;
                    _Load.Status = 0;
                    foreach (var detail in _Load.LoadInvoiceDetail)
                    {
                        detail.Date = currentDate;


                    }
                    context.LoadInvoice.Add(_Load);
                    context.Entry(_Load).State = EntityState.Added;

                    int result = await context.SaveChangesAsync();

                    if (result > 0)
                    {

                        //Update Stock
                        foreach (var invoiceDetail in _Load.LoadInvoiceDetail)
                        {
                            var itemId = invoiceDetail.FkItemId;
                            var quantity = invoiceDetail.Quantity;
                            var StockId = invoiceDetail.FkStockId;
                            var stockProduct = await context.StockProducts.FirstOrDefaultAsync(sp => sp.FkItemId == itemId && sp.FkStockId == StockId);
                            if (stockProduct != null)
                            {
                                double? grossAmount = (invoiceDetail.Unitcost * invoiceDetail.Quantity);
                              
                                stockProduct.GrossAmount -= grossAmount;
                                stockProduct.NetAmount -= invoiceDetail.Total;
                                stockProduct.Quantity -= quantity;
                                stockProduct.TaxAmount -= calculateTax(0.10, grossAmount);
                                await context.SaveChangesAsync();
                            }
                            invoiceDetail.Status = 0;

                            var stockOutEntry = new StockOut
                            {
                                FkStockId = StockId,
                                ItemId = itemId,
                                OutQuantity= quantity,
                                StockOutDate = currentDate,
                                FkLoadInvoiceId=_Load.Id
                                
                            };

                            context.StockOut.Add(stockOutEntry);

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

        public async Task<Result> GetLoadDetails(DateTime? datefrom, DateTime? dateend)
        {
            try
            {
                using (var context = new FalconTraderContext())
                {
                    var Result = await (//from StockOut in context.StockOut
                                        from LoadInvoice in context.LoadInvoice
                                        join LoadInvoiceDetail in context.LoadInvoiceDetail on LoadInvoice.Id equals LoadInvoiceDetail.FkLoadInvoiceId
                                        join product in context.Products on LoadInvoiceDetail.FkItemId equals product.Itemid
                                        join Route in context.Route on LoadInvoice.FkRouteId equals Route.RouteId
                                        join StockReturn in context.StockReturn on LoadInvoice.Id equals StockReturn.FkLoadInvoiceId into stockReturnGroup
                                        from stockReturn in stockReturnGroup.DefaultIfEmpty()
                                        where LoadInvoice.Date >= datefrom && LoadInvoice.Date <= dateend
                                        select new LoadModel
                                        {
                                            DeliveryMan=LoadInvoice.DeliveryMan,
                                            RouteName=Route.RouteName,
                                            LoadInvoiceNo = LoadInvoice.Id,
                                            ProductName = product.Itemdescp,
                                            Quantity=LoadInvoiceDetail.Quantity,
                                            Return=stockReturn.Quantity,
                                            VehicleNo=LoadInvoice.VehicleName,
                                            Date=LoadInvoice.Date


                                        }).ToListAsync();

                    return new Result() { Status = ResultStatus.Success, Message = "Success", Data = Result };
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


        public double? calculateTax(double taxpercentage, double? total)
        {
            return total * (taxpercentage / 100);

        }


    }
}
