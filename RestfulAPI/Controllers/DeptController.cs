using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using RestfulAPI.Model.Models.Dto;
using RestfulAPI.Models;
using RestfulAPI.Service.Interface;
using System;
using System.Collections.Generic;

namespace RestfulAPI.Controllers
{
    /// <summary>
    /// 部門模組
    /// </summary>
    [Route("api/dept")]
    [ApiController]
    public class DeptController
    {
        private readonly IDeptService _deptService;

        private readonly IMemoryCache _memoryCache;

        /// <summary>
        /// 構造函數
        /// </summary>
        /// <param name="userRepository"></param>
        public DeptController(IDeptService deptService, IMemoryCache memoryCache)
        {
            _deptService = deptService;
            _memoryCache = memoryCache;
        }
        /// <summary>
        /// 創建用戶
        /// </summary>
        /// <returns></returns>
        [Route("createDept")]
        [HttpPost]
        public DeptDto CreateDept([FromBody] DeptDto dept)
        {
            _deptService.AddDept(dept);
            return dept;
        }

        /// <summary>
        /// 查詢部門
        /// </summary>
        /// <returns></returns>
        [Route("getDept")]
        [HttpGet]
        public ResultModel GetUser()
        {
            string cacheKey = "cacheDepts";

            var bol = _memoryCache.TryGetValue(cacheKey, out List<DeptDto> depts);
            //判斷緩存是否存在
            if (!bol)
            {
                List<DeptDto> deptList = _deptService.GetAllDept();
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                _memoryCache.Set("cacheDepts", deptList, cacheEntryOptions);

                _memoryCache.TryGetValue(cacheKey, out depts);
            }

            return ResultModel.Ok(depts);
        }
    }
}
