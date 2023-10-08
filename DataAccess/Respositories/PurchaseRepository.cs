using DataAccess.Generic;
using DataAccess.Interfaces;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                    if (ExistCokeInvoice != null)
                    {

                        return new Result() { Status = ResultStatus.Error, Message = "Coke Invoice Already Exist!" };


                    }

                    DateTime currentDate = DateTime.Now;
                    _Purchase.Date = currentDate;
                    _Purchase.Status = 0;
                    foreach (var detail in _Purchase.PurchaseInvoiceDetail)
                    {
                        detail.Time = currentDate;
                        detail.Status = 0;

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
                            var stockProduct = await context.StockProducts.FirstOrDefaultAsync(sp => sp.FkItemId == itemId && sp.FkStockId == StockId);
                            if (stockProduct != null)
                            {
                                stockProduct.Quantity += quantity;
                                stockProduct.DiscountAmount += invoiceDetail.DiscountAmount;
                                stockProduct.TaxAmount += invoiceDetail.TaxAmount;
                                stockProduct.GrossAmount += (invoiceDetail.Unitcost * invoiceDetail.Quantity);
                                stockProduct.NetAmount += invoiceDetail.Total;
                                stockProduct.DiscountPercentage = invoiceDetail.DiscountPercentage;
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
                                    GrossAmount = (invoiceDetail.Unitcost * invoiceDetail.Quantity),
                                    NetAmount = invoiceDetail.Total,
                                    DiscountPercentage=invoiceDetail.DiscountPercentage,
                                    Status = 0
                                };

                                context.StockProducts.Add(stock_Product);
                            }

                            var stockInEntry = new StockIn
                            {
                                FkStockId = StockId,
                                ItemId = itemId,
                                Quantity = quantity,
                                StockInDate = currentDate,
                                FkPuchaseInvoiceId = _Purchase.Purchaseinvoiceid
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
                var Products = await Product_Repo.GetProducts();
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

        public async Task<Result> UpdatePurchaseInvoice(PurchaseInvoice updatedPurchase)
        {
            try
            {
                using (var context = new FalconTraderContext())
                {
                    var existingPurchase = await context.PurchaseInvoice
                        .Include(pi => pi.PurchaseInvoiceDetail).Where(pid=>pid.Status==0) // Include related details
                        .FirstOrDefaultAsync(pi => pi.Purchaseinvoiceid == updatedPurchase.Purchaseinvoiceid);

                    if (existingPurchase == null)
                    {
                        return new Result() { Status = ResultStatus.Error, Message = "Purchase Invoice not found." };
                    }

                    // Update properties of the existingPurchase entity with values from updatedPurchase
                    existingPurchase.Invoicetotal = updatedPurchase.Invoicetotal;
                    existingPurchase.Date = updatedPurchase.Date;
                    existingPurchase.Phone = updatedPurchase.Phone;
                    existingPurchase.TotalDiscount = updatedPurchase.TotalDiscount;
                    existingPurchase.TotalTax = updatedPurchase.TotalTax;
                    existingPurchase.Status = updatedPurchase.Status;
                    existingPurchase.CokeInvoice = updatedPurchase.CokeInvoice;
                    existingPurchase.DeliveryDate = updatedPurchase.DeliveryDate;
                    existingPurchase.Status = 0;
                    updatedPurchase.Status = 0;
                    // Update PurchaseInvoiceDetail entries
                    foreach (var updatedDetail in existingPurchase.PurchaseInvoiceDetail)
                    {
                        var existingDetail = existingPurchase.PurchaseInvoiceDetail.FirstOrDefault(d => d.PurchaseDetailId == updatedDetail.PurchaseDetailId);
                        if (existingDetail != null)
                        {

                            existingDetail.Status = 1;
                            existingDetail.FkPurchaseInvoiceId = updatedPurchase.Purchaseinvoiceid;
                        }

                        var stockProduct = await context.StockProducts.FirstOrDefaultAsync(sp => sp.FkItemId == existingDetail.Itemid && sp.FkStockId == existingDetail.FkStockId);
                        if (stockProduct != null)
                        {
                            stockProduct.Quantity -= existingDetail.Quantity;
                            stockProduct.DiscountAmount -= existingDetail.DiscountAmount;
                            stockProduct.TaxAmount -= existingDetail.TaxAmount;
                            stockProduct.GrossAmount -= (existingDetail.Unitcost * existingDetail.Quantity);
                            stockProduct.NetAmount -= existingDetail.Total;
                            stockProduct.Status = 0;
                            await context.SaveChangesAsync();
                        }

                    }

                    foreach (var item in updatedPurchase.PurchaseInvoiceDetail)
                    {
                        item.Status = 0;
                        var stockProduct = await context.StockProducts.FirstOrDefaultAsync(sp => sp.FkItemId == item.Itemid && sp.FkStockId == item.FkStockId);
                        if (stockProduct != null)
                        {
                            
                            item.FkPurchaseInvoiceId = updatedPurchase.Purchaseinvoiceid;
                            item.Time = DateTime.Now;
                            stockProduct.Quantity += item.Quantity;
                            stockProduct.DiscountAmount += item.DiscountAmount;
                            stockProduct.TaxAmount += item.TaxAmount;
                            stockProduct.GrossAmount += (item.Unitcost * item.Quantity);
                            stockProduct.NetAmount += item.Total;
                            stockProduct.DiscountPercentage = item.DiscountPercentage;
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            var stock_Product = new StockProducts
                            {
                                FkStockId = item.FkStockId,
                                FkItemId = item.Itemid,
                                Quantity = item.Quantity,
                                TaxAmount = item.TaxAmount,
                                DiscountAmount = item.DiscountAmount,
                                GrossAmount = (item.Unitcost * item.Quantity),
                                NetAmount = item.Total,
                                DiscountPercentage = item.DiscountPercentage,
                                Status = 0
                            };

                             context.StockProducts.Add(stock_Product);
                        }
                        //var stockInEntry = new StockIn
                        //{
                        //    FkStockId = item.FkStockId,
                        //    ItemId = item.FkStockId,
                        //    Quantity = quantity,
                        //    StockInDate = currentDate,
                        //    FkPuchaseInvoiceId = _Purchase.Purchaseinvoiceid
                        //};

                        //context.StockIn.Add(stockInEntry);
                       
                    }
                    existingPurchase.PurchaseInvoiceDetail = updatedPurchase.PurchaseInvoiceDetail;
                    existingPurchase.Status = 0;

                    int result = await context.SaveChangesAsync();

                    if (result > 0)
                    {
                        return new Result() { Status = ResultStatus.Success, Message = "Purchase Invoice updated successfully." };
                    }
                    else
                    {
                        return new Result() { Status = ResultStatus.Error, Message = "Failed to update Purchase Invoice." };
                    }

                }
            }
            catch (Exception ex)
            {
                return new Result() { Status = ResultStatus.Error, Message = ex.Message };
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
                        invoiceDetail.Status = 1;

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

        public async Task<Result> GetPurchaseByInvoiceNo(string InvoiceNo)
        {
            try
            {
                using (var context = new FalconTraderContext())
                {
                    var purchaseInvoice = await context.PurchaseInvoice
                        .Include(pi => pi.PurchaseInvoiceDetail).Where(pid=>pid.Status==0)
                        .FirstOrDefaultAsync(pi => pi.CokeInvoice == InvoiceNo && pi.Status==0);
                    if (purchaseInvoice!=null)
                    {
                        List<PurchaseInvoiceDetailModel> obj = new List<PurchaseInvoiceDetailModel>();
                        PurchaseInvoiceModel Purchase_Model = new PurchaseInvoiceModel();

                        Purchase_Model.CokeInvoice = purchaseInvoice.CokeInvoice;
                        Purchase_Model.DeliveryDate = purchaseInvoice.DeliveryDate;
                        Purchase_Model.Invoicetotal = purchaseInvoice.Invoicetotal;
                        Purchase_Model.TotalDiscount = purchaseInvoice.TotalDiscount;
                        Purchase_Model.TotalTax = purchaseInvoice.TotalTax;
                        Purchase_Model.Date = purchaseInvoice.Date;
                        Purchase_Model.Invoicetotal = purchaseInvoice.Invoicetotal;
                        Purchase_Model.Purchaseinvoiceid = purchaseInvoice.Purchaseinvoiceid;
                        PurchaseInvoiceDetailModel md = new PurchaseInvoiceDetailModel();
                        foreach (var item in purchaseInvoice.PurchaseInvoiceDetail)
                        {
                            PurchaseInvoiceDetailModel detailmd = new PurchaseInvoiceDetailModel();
                            
                            detailmd.FkStockId = item.FkStockId;
                            detailmd.Itemid = item.Itemid;
                            detailmd.Quantity = item.Quantity;
                            detailmd.TaxAmount = item.TaxAmount;
                            detailmd.Unitcost = item.Unitcost;
                            detailmd.DiscountAmount = item.DiscountAmount;
                            detailmd.FkPurchaseInvoiceId = item.FkPurchaseInvoiceId;
                            detailmd.Total = item.Total;
                            detailmd.Time = item.Time;
                            obj.Add(detailmd);

                        }
                        Purchase_Model.PurchaseInvoiceDetail = obj;
                        return new Result() { Status = ResultStatus.Success, Message = "", Data = Purchase_Model };

                    }
                    else
                    {
                        return new Result() { Status = ResultStatus.NotFound, Message = "Invoice Not Found", Data = purchaseInvoice };

                    }


                }
            }
            catch (Exception ex)
            {
                return new Result() { Status = ResultStatus.Error, Message = "Error", Data = ex.Message };

            }
        }
    }
}
