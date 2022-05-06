using RestfulAPI.Model;
using RestfulAPI.Model.Models;
using RestfulAPI.Repository.Repository.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace RestfulAPI.Repository.Repository
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// 增加實體(單個)
        /// </summary>
        /// <param name="entity">實體物件</param>
        void Add(T entity);

        /// <summary>
        /// 批量增加實體(多個)
        /// </summary>
        /// <param name="list">實體清單</param>
        void AddRange(List<T> list);

        /// <summary>
        /// 刪除實體(單個)
        /// </summary>
        /// <param name="entity"></param>
        void Remove(T entity);

        /// <summary>
        /// 批量刪除實體(多個)
        /// </summary>
        /// <param name="list">實體清單</param>
        void RemoveRange(List<T> list);
        /// <summary>
        /// 更新用戶
        /// </summary>
        /// <param name="entity">實體物件</param>
        /// <returns></returns>
        void Update(T entity);
        /// <summary>
        /// 取得所有用戶
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();
        /// <summary>
        /// 分頁條件查詢
        /// </summary>
        /// <typeparam name="TKey">排序類型</typeparam>
        /// <param name="pageIndex">當前頁</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="predicate">條件運算式</param>
        /// <param name="isAsc">是否昇冪排列</param>
        /// <param name="keySelector">排序運算式</param>
        /// <returns></returns>
        Page<T> SearchFor<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> predicate,
            bool isAsc, Expression<Func<T, TKey>> keySelector);
        /// <summary>
        /// 取得實體（主鍵）
        /// </summary>
        /// <param name="id">主鍵id</param>
        /// <returns></returns>
        T Get(object id);
        /// <summary>
        /// 取得實體（條件）
        /// </summary>
        /// <param name="predicate">條件運算式</param>
        /// <returns></returns>
        T GetModel(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 查詢記錄數
        /// </summary>
        /// <param name="predicate">條件運算式</param>
        /// <returns>記錄數</returns>
        int Count(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="anyLambda">查詢運算式</param>
        /// <returns>布林值</returns>
        bool Exist(Expression<Func<T, bool>> anyLambda);
    }
}