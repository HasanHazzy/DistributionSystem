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
                            var Product = await context.Products.FirstOrDefaultAsync(p => p.Itemid == itemId);
                            if (stockProduct != null)
                            {
                                
                                double? grossAmount = (invoiceDetail.Unitcost * invoiceDetail.Quantity);
                                double? tax = calculateTax(0.10, grossAmount);
                                double? DiscountPercentage = CalculateDiscountPercentage(stockProduct.GrossAmount, stockProduct.DiscountAmount);
                                double? discountamount = CalculateDiscount(DiscountPercentage, grossAmount);
                                stockProduct.GrossAmount -= grossAmount;
                                stockProduct.Quantity -= quantity;
                                stockProduct.TaxAmount -= calculateTax(0.10, grossAmount);
                                stockProduct.DiscountAmount -= discountamount;
                                stockProduct.NetAmount -= (invoiceDetail.Total + tax) - discountamount;

                                if (stockProduct.TaxAmount<0)
                                {
                                    stockProduct.TaxAmount = 0;

                                }
                                if (stockProduct.DiscountAmount<0)
                                {
                                    stockProduct.DiscountAmount = 0;

                                }

                                if (stockProduct.NetAmount<0)
                                {
                                    stockProduct.NetAmount = 0;

                                }

                                if (stockProduct.GrossAmount < 0)
                                {
                                    stockProduct.GrossAmount = 0;

                                }
                                await context.SaveChangesAsync();
                            }
                            invoiceDetail.Status = 0;

                            var stockOutEntry = new StockOut
                            {
                                FkStockId = StockId,
                                ItemId = itemId,
                                OutQuantity = quantity,
                                StockOutDate = currentDate,
                                FkLoadInvoiceId = _Load.Id

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
        public async Task<Result> UpdateLoadInvoice(LoadInvoice UpdatedLoadInvoice)
        {
            int generatedId = 0;
            try
            {
                using (var context = new FalconTraderContext())
                {

                    var ExistingLoadInvoice = await context.LoadInvoice
                       .Include(pi => pi.LoadInvoiceDetail).Where(pid => pid.Status == 0)
                       .FirstOrDefaultAsync(pi => pi.LoadInvoiceNo == UpdatedLoadInvoice.LoadInvoiceNo);


                    if (ExistingLoadInvoice == null)
                    {
                        return new Result() { Status = ResultStatus.Error, Message = "Load Invoice not found." };
                    }

                    ExistingLoadInvoice.BookerName = UpdatedLoadInvoice.BookerName;
                    ExistingLoadInvoice.DeliveryMan = UpdatedLoadInvoice.DeliveryMan;
                    ExistingLoadInvoice.LoadDate = UpdatedLoadInvoice.LoadDate;
                    ExistingLoadInvoice.InvoiceTotal = UpdatedLoadInvoice.InvoiceTotal;
                    ExistingLoadInvoice.VehicleName = UpdatedLoadInvoice.VehicleName;


                    //Update Stock
                    foreach (var existingDetail in ExistingLoadInvoice.LoadInvoiceDetail)
                    {
                        var itemId = existingDetail.FkItemId;
                        var quantity = existingDetail.Quantity;
                        var StockId = existingDetail.FkStockId;
                        existingDetail.Status = 1;
                        var stockProduct = await context.StockProducts.FirstOrDefaultAsync(sp => sp.FkItemId == itemId && sp.FkStockId == StockId);
                        var Product = await context.Products.FirstOrDefaultAsync(p => p.Itemid == itemId);
                        if (stockProduct != null)
                        {
                            double? grossAmount = (existingDetail.Unitcost * existingDetail.Quantity);
                            double? tax = calculateTax(0.10, grossAmount);
                            double? DiscountPercentage = CalculateDiscountPercentage(stockProduct.GrossAmount, stockProduct.DiscountAmount);
                            double? discountamount = CalculateDiscount(DiscountPercentage, grossAmount);
                            stockProduct.GrossAmount += grossAmount;
                            stockProduct.Quantity += quantity;
                            stockProduct.TaxAmount += calculateTax(0.10, grossAmount);
                            stockProduct.DiscountAmount += discountamount;
                            stockProduct.NetAmount += (existingDetail.Total + tax) - discountamount;

                        }

                    }
                    await context.SaveChangesAsync();


                    foreach (var UpdateDetail in UpdatedLoadInvoice.LoadInvoiceDetail)
                    {
                        var itemId = UpdateDetail.FkItemId;
                        var quantity = UpdateDetail.Quantity;
                        var StockId = UpdateDetail.FkStockId;
                        var stockProduct = await context.StockProducts.FirstOrDefaultAsync(sp => sp.FkItemId == itemId && sp.FkStockId == StockId);
                        var Product = await context.Products.FirstOrDefaultAsync(p => p.Itemid == itemId);
                        if (stockProduct != null)
                        {
                            double? grossAmount = (UpdateDetail.Unitcost * UpdateDetail.Quantity);
                            double? DiscountPercentage = CalculateDiscountPercentage(stockProduct.GrossAmount, stockProduct.DiscountAmount);
                            double? tax = calculateTax(0.10, grossAmount);
                            double? discountamount = CalculateDiscount(DiscountPercentage, grossAmount);
                            stockProduct.GrossAmount -= grossAmount;
                            stockProduct.Quantity -= quantity;
                            stockProduct.TaxAmount -= calculateTax(0.10, grossAmount);
                            stockProduct.DiscountAmount -= discountamount;
                            stockProduct.NetAmount -= (UpdateDetail.Total + tax) - discountamount;
                        }
                        UpdateDetail.Status = 0;
                        UpdateDetail.Date = DateTime.Now;

                        if (stockProduct.TaxAmount < 0)
                        {
                            stockProduct.TaxAmount = 0;

                        }
                        if (stockProduct.DiscountAmount < 0)
                        {
                            stockProduct.DiscountAmount = 0;

                        }

                        if (stockProduct.NetAmount < 0)
                        {
                            stockProduct.NetAmount = 0;

                        }

                        if (stockProduct.GrossAmount < 0)
                        {
                            stockProduct.GrossAmount = 0;

                        }

                    }
                    ExistingLoadInvoice.LoadInvoiceDetail = UpdatedLoadInvoice.LoadInvoiceDetail;
                    await context.SaveChangesAsync();

                    return new Result() { Status = ResultStatus.Success, Message = "Success", Data = generatedId };


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
                                        where LoadInvoice.LoadDate >= datefrom && LoadInvoice.LoadDate <= dateend
                                        select new LoadModel
                                        {
                                            DeliveryMan = LoadInvoice.DeliveryMan,
                                            RouteName = Route.RouteName,
                                            LoadInvoiceNo = LoadInvoice.LoadInvoiceNo,
                                            ProductName = product.Itemdescp,
                                            Quantity = LoadInvoiceDetail.Quantity,
                                            Return = stockReturn.Quantity,
                                            VehicleNo = LoadInvoice.VehicleName,
                                            Date = LoadInvoice.Date,
                                            LoadDate = LoadInvoice.LoadDate

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

        public async Task<Result> GetLoadByNo(int? LoadNo)
        {
            try
            {
                using (var context = new FalconTraderContext())
                {
                    var LoadInvoice = await context.LoadInvoice
                       .Include(pi => pi.LoadInvoiceDetail).Where(pid => pid.Status == 0)
                       .FirstOrDefaultAsync(pi => pi.LoadInvoiceNo == LoadNo);
                    if (LoadInvoice != null)
                    {
                        List<LoadInvoiceDetailModel> obj = new List<LoadInvoiceDetailModel>();
                        LoadInvoiceModel Load_Model = new LoadInvoiceModel();

                        Load_Model.LoadInvoiceNo = LoadInvoice.LoadInvoiceNo;
                        Load_Model.BookerName = LoadInvoice.BookerName;
                        Load_Model.DeliveryMan = LoadInvoice.DeliveryMan;
                        Load_Model.InvoiceTotal = LoadInvoice.InvoiceTotal;
                        Load_Model.VehicleName = LoadInvoice.VehicleName;
                        Load_Model.FkRouteId = LoadInvoice.FkRouteId;
                        Load_Model.FkTaxId = LoadInvoice.FkTaxId;
                        Load_Model.LoadDate = LoadInvoice.LoadDate;
                        LoadInvoiceDetailModel md = new LoadInvoiceDetailModel();
                        foreach (var item in LoadInvoice.LoadInvoiceDetail)
                        {
                            LoadInvoiceDetailModel detailmd = new LoadInvoiceDetailModel();
                            detailmd.FkStockId = item.FkStockId;
                            detailmd.FkItemId = item.FkItemId;
                            detailmd.Quantity = item.Quantity;
                            detailmd.Unitcost = item.Unitcost;
                            detailmd.FkLoadInvoiceId = item.FkLoadInvoiceId;
                            detailmd.Total = item.Total;
                            detailmd.Date = item.Date;
                            obj.Add(detailmd);

                        }
                        Load_Model.LoadInvoiceDetail = obj;
                        return new Result() { Status = ResultStatus.Success, Message = "", Data = Load_Model };

                    }
                    else
                    {
                        return new Result() { Status = ResultStatus.NotFound, Message = "Invoice Not Found", Data = "" };

                    }

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

        public double? CalculateDiscount(double? DiscountPercentage, double? total)
        {
            double? t= total * (DiscountPercentage / 100);
            return Math.Round(Convert.ToDouble(t), 2);

        }

        public double? CalculateDiscountPercentage(double? ProductTotal, double? DiscountAmount)
        {
            double? percentage = DiscountAmount * 100 / ProductTotal;
            return Math.Round(Convert.ToDouble(percentage),2);

        }
    }
}
