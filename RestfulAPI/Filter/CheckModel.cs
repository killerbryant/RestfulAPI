using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.Logging;
using RestfulAPI.Models;

namespace RestfulAPI.Filter
{
    /// <summary>
    /// 驗證實體物件是否合法
    /// </summary>
    public class CheckModel : ActionFilterAttribute
    {
        private readonly ILogger<CheckModel> _logger;

        public CheckModel(ILogger<CheckModel> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Action 調用前執行
        /// </summary>
        /// <param name="actionContext"></param>
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            // 參數資訊
            string parametersInfo = JsonConvert.SerializeObject(actionContext.ActionArguments, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });

            // 運行中的 Controller & Action 資訊
            string controllerName = actionContext.Controller.GetType().Name;
            string actionName = actionContext.ActionDescriptor.DisplayName;

            // 訊息內容
            string message = string.Format(
                "{0}.{1}() => {2}",
                controllerName,
                actionName,
                string.IsNullOrEmpty(parametersInfo) ? "(void)" : parametersInfo
            );

            _logger.LogDebug(message);

            if (!actionContext.ModelState.IsValid)
            {
                //初始化返回結果
                var result = new ResultModel { IsSuccess = false, ReturnCode = 400 };
                foreach (var item in actionContext.ModelState.Values)
                {
                    foreach (var error in item.Errors)
                    {
                        result.ErrorMessage += error.ErrorMessage + "|";
                    }
                }
                actionContext.Result = new JsonResult(result);
            }
        }
        /// <summary>
        /// Action 方法調用後，Result 方法調用前執行
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}