using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace RestfulAPI.Interceptor
{
    public class LoggingInterceptor : IInterceptor
    {
        private readonly ILogger<LoggingInterceptor> _logger;

        public LoggingInterceptor(ILogger<LoggingInterceptor> logger)
        {
            _logger = logger;
        }

        public void Intercept(IInvocation invocation)
        {
            var methodName = invocation.Method.Name;
            try
            {
                invocation.Proceed();
                LogExecuteInfo(invocation, JsonConvert.SerializeObject(invocation.ReturnValue));
            }
            catch (Exception e)
            {
                LogExecuteError(e, invocation);
                throw;
            }
        }

        #region helpMethod
        /// <summary>
        /// 獲取攔截方法資訊（類名、方法名、參數）
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        private string GetMethodInfo(IInvocation invocation)
        {
            //方法類名
            string className = invocation.Method.DeclaringType.Name;
            //方法名
            string methodName = invocation.Method.Name;
            //參數
            string args = string.Join(", ", invocation.Arguments.Select(a => (a ?? "").ToString()).ToArray());


            if (string.IsNullOrWhiteSpace(args))
            {
                return $"{className}.{methodName}";
            }
            else
            {
                return $"{className}.{methodName}: {args}";
            }
        }
        private void LogExecuteInfo(IInvocation invocation, string result)
        {
            _logger.LogDebug("方法{0}，返回值{1}", GetMethodInfo(invocation), result);
        }
        private void LogExecuteError(Exception ex, IInvocation invocation)
        {
            _logger.LogError(ex, "執行{0}時發生錯誤！", GetMethodInfo(invocation));
        }
        #endregion

    }
}