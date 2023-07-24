using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static WebPOS.Models.Enums;

namespace WebPOS.Models
{
    public class Structs
    {
        public struct Result<T> where T : class
        {
            public ResultStatus Status { get; set; }
            public string Message { get; set; }
            public IEnumerable<T> Data { get; set; }
        }

        public struct GenericResult<T> where T : class
        {
            public ResultStatus Status { get; set; }
            public string Message { get; set; }
            public T Data { get; set; }
        }
        public struct ResultSingleObject<T> where T : class
        {
            public ResultStatus Status { get; set; }
            public string Message { get; set; }
            public T Data { get; set; }
        }
        //public struct PagedResult<T> where T : class
        //{
        //    public int sEcho { get; set; }
        //    public int iTotalRecords { get; set; }
        //    public int iTotalDisplayRecords { get; set; }
        //    public IPagedList<T> aaData { get; set; }
        //    public ResultStatus Status { get; set; }
        //    public string Message { get; set; }
        //}

        public struct Result
        {
            public ResultStatus Status { get; set; }
            public string Message { get; set; }
            public dynamic Data { get; set; }
        }

        public struct ExtAPIResult
        {

            public bool isValid { get; set; }
            public string Message { get; set; }
            public string status { get; set; }
            public dynamic Result { get; set; }
        }

    }




}

