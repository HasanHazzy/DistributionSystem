using DataAccess.Generic;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{

    public interface IPurchaseRepository
    {
        Task<Result> GetPurchases(DateTime filterdate);
        Task<Result> GetAllPurchases(DateTime fromDate, DateTime ToDate);
        Task<Result> SavePurchaseInvoice(PurchaseInvoice _Purchase);
        Task<Result> GetBill(int InvoiceNo);
        Task<Result> GetDropDownData();

    }

}
