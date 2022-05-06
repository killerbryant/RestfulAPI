using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RestfulAPI.Model.Models.Dto;
using RestfulAPI.Models;
using RestfulAPI.Service.Interface;
using RestfulAPI.Utils;
using System;
using System.Collections.Generic;

namespace RestfulAPI.Controllers
{
    /// <summary>
    /// 使用者模組
    /// </summary>
    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _uberService;

        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 構造函數
        /// </summary>
        /// <param name="userRepository"></param>
        public UserController(IUserService uberService, IMemoryCache memoryCache)
        {
            _uberService = uberService;
            _memoryCache = memoryCache;
        }
        /// <summary>
        /// 創建用戶
        /// </summary>
        /// <returns></returns>
        [Route("createUser")]
        [HttpPost]
        public UserDto CreateUser([FromBody] UserDto user)
        {
            user.UserId = Guid.NewGuid().ToString("N");
            user.CreateTime = DateTime.Now;

            _uberService.AddUser(user);
            return user;
        }
        /// <summary>
        /// 查詢用戶
        /// </summary>
        /// <returns></returns>
        [Route("getUser")]
        [HttpGet]
        public ResultModel GetUser()
        {
            string cacheKey = "cacheUsers";

            var bol = _memoryCache.TryGetValue(cacheKey, out List<UserDto> users);
            //判斷緩存是否存在
            if (!bol)
            {
                List<UserDto> userList = _uberService.GetAllUser();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                _memoryCache.Set("cacheUsers", userList, cacheEntryOptions);

                _memoryCache.TryGetValue(cacheKey, out users);
            }

            return ResultModel.Ok(users);
        }

        [Route("getOneUser")]
        [HttpGet]
        public ResultModel GetOneUser(string id)
        {
            var data = _uberService.GetOneUser(id);
            return ResultModel.Ok(data);
        }

        [Route("getToken")]
        [HttpGet]
        public ActionResult<string> GetToken(string name, string pwd)
        {
            //這裡就是使用者登陸以後，通過資料庫去調取資料，分配許可權的操作
            //這裡直接寫死了

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(pwd))
            {
                return new JsonResult(new
                {
                    Status = false,
                    message = "用戶名或密碼不能為空"
                });
            }

            TokenModelJWT tokenModel = new TokenModelJWT
            {
                Uid = 1,
                Role = name
            };

            string jwtStr = JwtHelper.IssueJWT(tokenModel);
            bool suc = true;
            return Ok(new
            {
                success = suc,
                token = jwtStr
            });
        }

    }
}