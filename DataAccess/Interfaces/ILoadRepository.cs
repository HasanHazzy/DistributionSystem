using DataAccess.Generic;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface ILoadRepository
    {
        Task<Result> GetAllLoads(DateTime filterdate);
        Task<Result> GetAllPurchases(DateTime fromDate, DateTime ToDate);
        Task<Result> SaveLoadInvoice(LoadInvoice _Purchase);
        Task<Result> GetBill(int InvoiceNo);
        Task<Result> GetDropDownData();
        Task<Result> GetLoadDetails(DateTime? datefrom, DateTime? dateend);
        Task<Result> GetLoadByNo(int? LoadNo);
        Task<Result> UpdateLoadInvoice(LoadInvoice UpdatedLoadInvoice);



    }
}
