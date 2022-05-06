using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulAPI.Models
{
    /// <summary>
    /// 回應實體類
    /// </summary>
    public class ResultModel
    {
        [JsonProperty("Code")]
        /// <summary>
        /// 狀態碼
        /// </summary>
        public int ReturnCode { get; set; }

        /// <summary>
        /// 內容
        /// </summary>
        public object Data { get; set; }

        /// <summary>
        /// 錯誤資訊
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// 是否成功
        /// </summary>
        public bool IsSuccess { get; set; }

        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data">返回資料</param>
        /// <returns></returns>
        public static ResultModel Ok(object data)
        {
            return new ResultModel { Data = data, ErrorMessage = null, IsSuccess = true, ReturnCode = 200 };
        }
        /// <summary>
        /// 失敗
        /// </summary>
        /// <param name="str">錯誤資訊</param>
        /// <param name="code">狀態碼</param>
        /// <returns></returns>
        public static ResultModel Error(string str, int code)
        {
            return new ResultModel { Data = null, ErrorMessage = str, IsSuccess = false, ReturnCode = code };
        }
    }
}