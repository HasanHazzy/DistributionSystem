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
    class RouteRepository : IRoute
    {
        public async Task<Result> GetRoutes()
        {
            try
            {
                using (var context = new FalconTraderContext())
                {
                    var Result = await(from Route in context.Route

                                       select new Route
                                       {
                                           RouteId = Route.RouteId,
                                           RouteName = Route.RouteName
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
    }
}
