using Dapper.Contrib.Extensions;
using Newtonsoft.Json;
using System;

namespace RestfulAPI.Model.Models
{
    /// <summary>
    /// 用戶表
    /// </summary>
    [Table("Users")]
    public class User: IEntity
    {
        /// <summary>
        /// 用戶Id
        /// </summary>
        [ExplicitKey]
        //[Column("userId")]
        //[StringLength(50)]
        [ColumnName(Name = "userId")]
        public string UserId { get; set; }

        /// <summary>
        /// 用戶名
        /// </summary>
        [JsonProperty("Name")]
        //[Column("userName")]
        //[StringLength(100)]
        [ColumnName(Name = "userName")]
        public string UserName { get; set; }

        /// <summary>
        /// 郵箱
        /// </summary>
        [JsonProperty("Email")]
        //[Column("email")]
        //[StringLength(120)]
        [ColumnName(Name = "email")]
        public string Email { get; set; }

        /// <summary>
        /// 創建時間
        /// </summary>
        //[Column("createTime")]
        [ColumnName(Name = "createTime")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 用戶年齡
        /// </summary>
        //[Column("createTime")]
        [ColumnName(Name = "age")]
        public int UserAge { get; set; }

        //[Editable(false)]
        public string NickName => $"{Email[..Email.IndexOf('@')]}";
    }
}
