using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RestfulAPI.Models;
using System;
using System.Threading.Tasks;

namespace RestfulAPI.Filter
{
    /// <summary>
    /// 處理全域資訊中介軟體
    /// </summary>
    public class ExceptionMiddleWare
    {
        /// <summary>
        /// 處理HTTP請求的函數。
        /// </summary>
        private readonly RequestDelegate _next;
        /// <summary>
        /// 構造函數
        /// </summary>
        /// <param name="next"></param>
        public ExceptionMiddleWare(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                //拋給下一個中介軟體
                await _next(context);
            }
            catch (Exception ex)
            {
                await WriteExceptionAsync(context, ex);
            }
            finally
            {
                await WriteExceptionAsync(context, null);
            }
        }

        private async Task WriteExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception != null)
            {
                var response = context.Response;
                var message = exception.InnerException == null ? exception.Message : exception.InnerException.Message;
                response.ContentType = "application/json";
                await response.WriteAsync(JsonConvert.SerializeObject(ResultModel.Error(message, 400))).ConfigureAwait(false);
            }
            else
            {
                var code = context.Response.StatusCode;
                switch (code)
                {
                    case 200:
                        return;
                    case 204:
                        return;
                    case 401:
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(ResultModel.Error("token已過期,請重新登錄.", code))).ConfigureAwait(false);
                        break;
                    default:
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(ResultModel.Error("未知錯誤", code))).ConfigureAwait(false);
                        break;
                }
            }
        }
    }
}