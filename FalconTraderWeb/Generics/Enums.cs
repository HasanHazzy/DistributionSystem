using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebPOS.Models
{
    public class Enums
    {
        public enum ResultStatus
        {
            Success = 200,
            Error = 500,
            NotFound = 404,
            Warning = 400,
            InProcess = 500,
            AlreadyExist = 409
        };


        public enum AccountType
        {
            Cash=1,
            Credit=2
        };

    }
}
