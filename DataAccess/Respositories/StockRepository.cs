using DataAccess.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataAccess.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DataAccess.Models;

namespace DataAccess.Respositories
{
    public class StockRepository : IStockRepository
    {
        private readonly IProductRepository Product_Repo;
        public StockRepository()
        {
            Product_Repo = new ProductRepository();

        }

        public async Task<Result> GetStock()
        {
            try
            {
                using (var context = new FalconTraderContext())
                {
                    var Data = await(from stock in context.Stock
                                    where stock.Status==0
                                     select new Stock
                                     {
                                         Id=stock.Id,
                                         Descp = stock.Descp,
                                         Symbol=stock.Symbol
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

        public async Task<Result> SaveStockReturn(StockReturn Stock_Return)
        {
            try
            {

                using (var _dbContext = new FalconTraderContext())
                {
                    var stock =  _dbContext.Stock.FirstOrDefault(s => s.Id == Stock_Return.FkStockId);
                    var LoadInvoice = _dbContext.LoadInvoice
                        .Include(li => li.LoadInvoiceDetail)
                        .FirstOrDefault(pi => pi.Id == Stock_Return.FkLoadInvoiceId);
                    var item =  _dbContext.Products.FirstOrDefault(p => p.Itemid == Stock_Return.FkItemId);

                    var StockProducts = _dbContext.StockProducts.FirstOrDefault(p => p.FkItemId == Stock_Return.FkItemId);


                    if (stock ==null || LoadInvoice ==null || StockProducts == null)
                    {
                        return new Result() { Status = ResultStatus.Error, Message = "Invalid stock, purchase invoice, or item.", Data = "" };

                    }


                    // Update PurchaseInvoice details (if necessary
                    var invoiceDetail = LoadInvoice.LoadInvoiceDetail.FirstOrDefault(d => d.FkItemId == Stock_Return.FkItemId);
                    if (invoiceDetail != null)
                    {
                        invoiceDetail.Quantity -= Stock_Return.Quantity;
                        StockProducts.Quantity += Stock_Return.Quantity;
                        invoiceDetail.Total = (item.UnitPrice) * (invoiceDetail.Quantity);
                      //  _dbContext.SaveChanges();


                        
                    }
                    else
                    {
                        return new Result() { Status = ResultStatus.Error, Message = "Incorrect Item Id", Data = "" };

                    }


                    // Create a new StockReturn entity
                    var stockReturn = new StockReturn
                    {
                        FkStockId = Stock_Return.FkStockId,
                        FkLoadInvoiceId = Stock_Return.FkLoadInvoiceId,
                        FkItemId = Stock_Return.FkItemId,
                        Quantity = Stock_Return.Quantity,
                        Status = 0
                    };

                    var InvoiceTotal = LoadInvoice.LoadInvoiceDetail.Sum(x => x.Total);
                    var TaxPercentage = _dbContext.Tax.Where(x => x.Id == LoadInvoice.FkTaxId).Select(x => x.Percentage).FirstOrDefault();
                    var TaxAmount = CalculateTax(InvoiceTotal, Convert.ToDouble(TaxPercentage));

                    if (InvoiceTotal>0)
                    {
                        InvoiceTotal -= TaxAmount;
                        InvoiceTotal -= LoadInvoice.DiscountHth;
                        InvoiceTotal -= LoadInvoice.DiscountFoc;
                        InvoiceTotal -= LoadInvoice.DiscountRegular;
                        LoadInvoice.Tax = TaxAmount;
                        LoadInvoice.InvoiceTotal = InvoiceTotal;

                    }
                    else
                    {
                        LoadInvoice.Tax = 0;
                        LoadInvoice.InvoiceTotal = 0;
                        LoadInvoice.DiscountHth = 0;
                        LoadInvoice.DiscountFoc = 0;
                        LoadInvoice.DiscountRegular = 0;
                    }
              
                    _dbContext.StockReturn.Add(stockReturn);
                    _dbContext.LoadInvoice.Update(LoadInvoice);
                    _dbContext.StockProducts.Update(StockProducts);
                    _dbContext.SaveChanges();

                    return new Result() { Status = ResultStatus.Success, Message = "Success", Data = "" };


                }
            }
            catch (Exception ex)
            {

                return new Result() { Status = ResultStatus.Error, Message = "Error ", Data = ex.Message };

            }


            // Check if the corresponding Stock, PurchaseInvoice, and Item exist in the database
        
        }

        public double? CalculateTax(double? total, double? TaxPercentage)
        {
            var TaxAmount = total * (TaxPercentage / 100);
            return TaxAmount;
        }

        public double? CalculateDiscount(double? total, double? DiscountPercentage)
        {
            var DiscountAmount = total * (DiscountPercentage / 100);
            return DiscountAmount;
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

                var Stock = await GetStock();
                if (Stock.Status == ResultStatus.Success)
                {
                    obj.Stock = Stock.Data;

                }
             

                return new Result() { Status = ResultStatus.Success, Message = "Success", Data = obj };

            }
            catch (Exception ex)
            {
                return new Result() { Status = ResultStatus.Error, Message = ex.Message, Data = "" };

            }
        }

        public async Task<Result> GetStocks(DateTime? startdate,DateTime? enddate)
        {
            try
            {
                using (var context = new FalconTraderContext())
                {
                    var Result = await (//from stockIn in context.StockIn
                                        from PurchaseInvoice in context.PurchaseInvoice
                                        join PurchaseInvoiceDetail in context.PurchaseInvoiceDetail on PurchaseInvoice.Purchaseinvoiceid equals PurchaseInvoiceDetail.FkPurchaseInvoiceId
                                        join product in context.Products on PurchaseInvoiceDetail.Itemid equals product.Itemid
                                        join Stocks in context.Stock on PurchaseInvoiceDetail.FkStockId equals Stocks.Id
         
                                        where PurchaseInvoice.Date>=startdate && PurchaseInvoice.Date<=enddate && product.Status==0 && PurchaseInvoice.Status==0
                                        select new StockInModel
                                        {
                                            CokeInvoice=PurchaseInvoice.CokeInvoice,
                                            PurchaseInvoiceNo=PurchaseInvoice.Purchaseinvoiceid,
                                            StockName = Stocks.Descp,
                                            ProductName = product.Itemdescp,
                                            Quantity= PurchaseInvoiceDetail.Quantity,
                                            Total=PurchaseInvoiceDetail.Total,
                                            StockIn_Date = PurchaseInvoiceDetail.Time,
                                            DiscountAmount=PurchaseInvoiceDetail.DiscountAmount,
                                            TaxAmount = PurchaseInvoiceDetail.TaxAmount,
                                            DeliveryDate=PurchaseInvoice.DeliveryDate


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

        public async Task<Result> GetStocksOut(DateTime? datefrom,DateTime?dateend)
        {
            try
            {
                using (var context = new FalconTraderContext())
                {
                    var Result = await (from StockOut in context.StockOut
                                        join product in context.Products on StockOut.ItemId equals product.Itemid
                                        join Stocks in context.Stock on StockOut.FkStockId equals Stocks.Id
                                        join LoadInvoice in context.LoadInvoice on StockOut.FkLoadInvoiceId equals LoadInvoice.Id
                                        join LoadInvoiceDetail in context.LoadInvoiceDetail on LoadInvoice.Id equals LoadInvoiceDetail.FkLoadInvoiceId
                                        join StockReturn in context.StockReturn on LoadInvoice.Id equals StockReturn.FkLoadInvoiceId into stockReturnGroup
                                        from stockReturn in stockReturnGroup.DefaultIfEmpty()
                                        where StockOut.StockOutDate >= datefrom && StockOut.StockOutDate <= dateend
                                        select new StockOutModel
                                        {
                                            LoadInvoiceNo = LoadInvoice.Id,
                                            StockName = Stocks.Descp,
                                            ProductName = product.Itemdescp,
                                            Quantity = StockOut.OutQuantity,
                                            LoadInvoiceDetailTotal = LoadInvoiceDetail.Total,
                                            DiscountRegular = LoadInvoice.DiscountRegular,
                                            DiscountFOCAmount = LoadInvoice.DiscountFoc,
                                            DiscountHTHAmount = LoadInvoice.DiscountHth,
                                            TaxAmount = LoadInvoice.Tax,
                                            StockOut_Date = StockOut.StockOutDate,
                                            LoadInvoiceTotal=LoadInvoice.InvoiceTotal,
                                            Return= stockReturn.Quantity,
                                            Margin=LoadInvoiceDetail.Margin
                                          
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

        public async Task<Result> GetCurrentStock()
        {
            try
            {
                using (var context = new FalconTraderContext())
                {

                    var result = await  (from sp in context.StockProducts
                                        from p in context.Products
                                        from s in context.Stock
                                        where p.Itemid == sp.FkItemId && sp.FkStockId == s.Id

                                         select new StockQuantity
                                        {
                                            StockName = s.Descp,
                                            ProductCode = p.ProductCode,
                                            ProductName = p.Itemdescp,
                                            unitPrice=p.UnitPrice,
                                            Quantity = sp.Quantity,
                                            GrossAmount = sp.GrossAmount,
                                            TotalTax = sp.TaxAmount,
                                            NetAmount = sp.NetAmount
                                        }).ToListAsync();

                    return new Result() { Status = ResultStatus.Success, Message = "Success", Data = result };
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

        public async Task<Result> SaveStocks(Stock _Stock)
        {
            try
            {
                using (var context = new FalconTraderContext())
                {
                    _Stock.Status = 0;
                    context.Stock.Add(_Stock);
                    context.Entry(_Stock).State = EntityState.Added;
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

        public async Task<Result> UpdateStock(Stock _Stock)
        {
            try
            {
                using (var context = new FalconTraderContext())
                {
                    _Stock.Status = 0;
                    context.Stock.Update(_Stock);
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

        public async Task<Result> DeleteStock(Stock _Stock)
        {
            try
            {
                using (var context = new FalconTraderContext())
                {
                    var StockToUpdate = await context.Stock.Where(x => x.Id == _Stock.Id).FirstOrDefaultAsync();

                    if (StockToUpdate != null)
                    {
                        StockToUpdate.Status = 1;
                        context.Entry(StockToUpdate).State = EntityState.Modified;
                        int result = await context.SaveChangesAsync();

                        if (result > 0)
                            return new Result() { Status = ResultStatus.Success, Message = "WareHouse Deleted Successfully", Data = "" };
                        else
                            return new Result() { Status = ResultStatus.Error, Message = "Error updating status" };
                    }
                    else
                    {
                        return new Result() { Status = ResultStatus.Error, Message = "WareHouse Not Found" };
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


