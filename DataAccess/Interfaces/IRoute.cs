using DataAccess.Generic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Interfaces
{
    public interface IRoute
    {
        Task<Result> GetRoutes();

    }
}
