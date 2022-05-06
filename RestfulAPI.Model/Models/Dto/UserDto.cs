using Newtonsoft.Json;
using System;

namespace RestfulAPI.Model.Models.Dto
{
    public class UserDto: BaseEntity
    {
        /// <summary>
        /// 用戶Id
        /// </summary>
        [JsonProperty("ID")]
        [System.Text.Json.Serialization.JsonIgnore]
        public string UserId { get; set; }
        /// <summary>
        /// 用戶名
        /// </summary>
        [JsonProperty("Name")]
        [System.Text.Json.Serialization.JsonPropertyName("Name")]
        public string UserName { get; set; }
        /// <summary>
        /// 郵箱
        /// </summary>
        [JsonProperty("Email")]
        [System.Text.Json.Serialization.JsonPropertyName("Email")]
        public string Email { get; set; }

        /// <summary>
        /// 用戶年齡
        /// </summary>
        [JsonProperty("age")]
        public int UserAge { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        [JsonProperty("CreateDate")]
        [System.Text.Json.Serialization.JsonIgnore]
        public DateTime CreateTime { get; set; }

       
    }
}
