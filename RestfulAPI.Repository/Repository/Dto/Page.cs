using System;
using System.Collections.Generic;
using System.Text;

namespace RestfulAPI.Repository.Repository.Dto
{
    public class Page<T>
    {
        /// <summary>
        /// 當前頁
        /// </summary>
        public int PageIndex { get; set; }
        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPages { get; set; }
        /// <summary>
        /// 集合總數
        /// </summary>
        public int TotalRows { get; set; }
        /// <summary>
        /// 每頁項數
        /// </summary>
        public int PageSize { get; set; }
        /// <summary>
        /// 集合
        /// </summary>
        public IList<T> LsList { get; set; }
    }
}