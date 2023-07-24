using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static WebPOS.Models.Enums;
using static WebPOS.Models.Structs;

namespace WebPOS.Models
{
    public class Helper
    {

        public static Result<T> PostRequest<T>(string requestURI, Dictionary<string, string> ServiceData) where T : class
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestURI)
                    {
                        Content = new FormUrlEncodedContent(ServiceData)
                    };

                    HttpResponseMessage response = client.PostAsync(request.RequestUri, request.Content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var results = response.Content.ReadAsStringAsync().Result;
                        var status = JsonConvert.DeserializeObject<Result<T>>(results);
                        return status;
                    }
                    else
                    {
                        var results = response.Content.ReadAsStringAsync().Result;
                        var status = JsonConvert.DeserializeObject<Result<T>>(results);

                        return status;

                    }
                }

            }
            catch (System.Exception e)
            {
                return new Result<T>() { Status = ResultStatus.Error, Message = e.Message.ToString(), Data = null };
            }
        }

        public static Result PostRequest(string requestURI, Dictionary<string, string> ServiceData)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, requestURI)
                    {
                        Content = new FormUrlEncodedContent(ServiceData)
                    };

                    HttpResponseMessage response = client.PostAsync(request.RequestUri, request.Content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var results = response.Content.ReadAsStringAsync().Result;
                        var status = JsonConvert.DeserializeObject<Result>(results);
                        return status;
                    }
                    else
                    {
                        var results = response.Content.ReadAsStringAsync().Result;
                        var status = JsonConvert.DeserializeObject<Result>(results);

                        return status;

                    }
                }

            }
            catch (System.Exception e)
            {
                return new Result() { Status = ResultStatus.Error, Message = e.Message.ToString(), Data = null };
            }
        }

        public static string GetRequest(string URL)
        {
            try
            {
                HttpWebRequest webreq = (HttpWebRequest)WebRequest.Create(URL);
                webreq.ContentType = "application/json; charset=utf-8";
                webreq.Headers.Clear();
                webreq.Method = "GET";
                Encoding encode = Encoding.GetEncoding("utf-8");
                HttpWebResponse webres = (HttpWebResponse)webreq.GetResponse();
                Stream reader = webres.GetResponseStream();
                StreamReader sreader = new StreamReader(reader, encode, true);
                string result = sreader.ReadToEnd();
                return result;
            }
            catch (System.Exception e)
            {
                return "Contact details already Exist";
            }
        }



        public static async Task<Result> GetRequestResult(string Url)
        {
            //string Token = CustomerPortalIntegration.GetCurrentToken();
            using (var client = new HttpClient())
            {
                // client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CustomerPortalIntegration.GetCurrentToken(tokenType));

                HttpResponseMessage response = await client.GetAsync(Url);
                if (response.IsSuccessStatusCode)
                {
                    var ResponseData = response.Content.ReadAsStringAsync().Result;
                    var ResultData = JsonConvert.DeserializeObject<Result>(ResponseData);
                    return ResultData;
                }
                else
                {
                    return new Result() { Message = "OPPS! SOMETHING WENT WRONG.", Status = ResultStatus.Error };
                }
            }
        }

        public static async Task<Result> PostRequest(dynamic data, string Url)
        {
            string Json = JsonConvert.SerializeObject(data);
            using (var client = new HttpClient())
            {
                var content = new StringContent(Json, Encoding.UTF8, "application/json");
                //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CustomerPortalIntegration.GetCurrentToken(tokenType));
                HttpResponseMessage response = await client.PostAsync(Url, content);
                if (response.IsSuccessStatusCode)
                {
                    var ResponseData = response.Content.ReadAsStringAsync().Result;
                    var ResultData = JsonConvert.DeserializeObject<Result>(ResponseData);
                    return ResultData;
                }
                else
                {
                    return new Result() { Message = "OPPS! SOMETHING WENT WRONG.", Status = ResultStatus.Error };
                }

            }
        }


        public static Result<T> GetRequest<T>(string URL) where T : class
        {
            try
            {
                using (var client = new HttpClient())
                {
                    //var pass = Extensions.Decrypt(LoginInfoTemp.Password);
                    //var suername = LoginInfoTemp.UserName;
                    //var Token = Convert.ToBase64String(Encoding.Default.GetBytes(suername + ":" + pass));
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", CustomerPortalIntegration.GetCurrentToken(tokenType));

                    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, URL);

                    HttpResponseMessage response = client.GetAsync(request.RequestUri).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var results = response.Content.ReadAsStringAsync().Result;
                        var status = JsonConvert.DeserializeObject<Result<T>>(results);
                        return status;
                    }
                    else
                    {
                        var results = response.Content.ReadAsStringAsync().Result;
                        var status = JsonConvert.DeserializeObject<Result<T>>(results);

                        return status;

                    }
                }

            }
            catch (System.Exception e)
            {
                return new Result<T>() { Status = ResultStatus.Error, Message = e.Message.ToString(), Data = null };
            }
        }

        public static SelectList SelectList<T>(IEnumerable<T> items, string valueProperty, string textProperty)
        {
            var selectListItems = items.Select(item => new SelectListItem
            {
                Value = item.GetType().GetProperty(valueProperty)?.GetValue(item)?.ToString(),
                Text = item.GetType().GetProperty(textProperty)?.GetValue(item)?.ToString()
            });

            return new SelectList(selectListItems, "Value", "Text");
        }


    }
}




