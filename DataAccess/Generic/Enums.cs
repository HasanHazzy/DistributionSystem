using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess.Generic
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
    

}
