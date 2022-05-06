using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using RestfulAPI.Model;
using RestfulAPI.Repository.Repository.Dto;

namespace RestfulAPI.Repository.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly MyDbContext _dbContext;
        private DbSet<T> _entity;

        /// <summary>
        /// 構造函數
        /// </summary>
        /// <param name="dbContext"></param>
        public Repository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private DbSet<T> Entity => _entity ??= _dbContext.Set<T>();
        /// <summary>
        /// 添加實體(單個)
        /// </summary>
        /// <param name="entity">實體物件</param>
        public void Add(T entity)
        {
            Entity.Add(entity);
        }
        /// <summary>
        /// 批量插入實體(多個)
        /// </summary>
        /// <param name="list">實體清單</param>
        public void AddRange(List<T> list)
        {
            Entity.AddRange(list);
        }

        /// <summary>
        /// 刪除實體(單個)
        /// </summary>
        /// <param name="entity"></param>
        public void Remove(T entity)
        {
            Entity.Remove(entity);
        }
        /// <summary>
        /// 批量刪除實體(多個)
        /// </summary>
        /// <param name="list">實體清單</param>
        public void RemoveRange(List<T> list)
        {
            Entity.RemoveRange(list);
        }
        /// <summary>
        /// 編輯用戶
        /// </summary>
        /// <param name="entity">實體物件</param>
        /// <returns></returns>
        public void Update(T entity)
        {

        }
        /// <summary>
        /// 獲取所有
        /// </summary>
        /// <returns></returns>
        public IQueryable<T> GetAll()
        {
            return Entity.AsQueryable().AsNoTracking();
        }
        /// <summary>
        /// 條件查詢
        /// </summary>
        /// <typeparam name="TKey">排序類型</typeparam>
        /// <param name="pageIndex">當前頁</param>
        /// <param name="pageSize">每頁大小</param>
        /// <param name="isAsc">是否昇冪排列</param>
        /// <param name="predicate">條件運算式</param>
        /// <param name="keySelector">排序運算式</param>
        /// <returns></returns>
        public Page<T> SearchFor<TKey>(int pageIndex, int pageSize, Expression<Func<T, bool>> predicate, bool isAsc, Expression<Func<T, TKey>> keySelector)
        {
            if (pageIndex <= 0 || pageSize <= 0) throw new Exception("pageIndex或pageSize不能小於等於0");
            var page = new Page<T> { PageIndex = pageIndex, PageSize = pageSize };
            var skip = (pageIndex - 1) * pageSize;
            var able = Entity.AsQueryable().AsNoTracking();
            if (predicate == null)
            {
                var count = Entity.Count();
                var query = isAsc
                    ? able.OrderBy(keySelector).Skip(skip).Take(pageSize)
                    : able.OrderByDescending(keySelector).Skip(skip).Take(pageSize);
                page.TotalRows = count;
                page.LsList = query.ToList();
                page.TotalPages = page.TotalRows / pageSize;
                if (page.TotalRows % pageSize != 0) page.TotalPages++;
            }
            else
            {
                var queryable = able.Where(predicate);
                var count = queryable.Count();
                var query = isAsc
                    ? queryable.OrderBy(keySelector).Skip(skip).Take(pageSize)
                    : queryable.OrderByDescending(keySelector).Skip(skip).Take(pageSize);
                page.TotalRows = count;
                page.LsList = query.ToList();
                page.TotalPages = page.TotalRows / pageSize;
                if (page.TotalRows % pageSize != 0) page.TotalPages++;
            }
            return page;
        }
        /// <summary>
        /// 獲取實體
        /// </summary>
        /// <param name="id">主鍵id</param>
        /// <returns></returns>
        public T Get(object id)
        {
            return Entity.Find(id);
        }

        /// <summary>
        /// 獲取實體（條件）
        /// </summary>
        /// <param name="predicate">條件運算式</param>
        /// <returns></returns>
        public T GetModel(Expression<Func<T, bool>> predicate)
        {
            return Entity.FirstOrDefault(predicate);
        }
        /// <summary>
        /// 查詢記錄數
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int Count(Expression<Func<T, bool>> predicate)
        {
            return predicate != null ? Entity.Where(predicate).Count() : Entity.Count();
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="anyLambda"></param>
        /// <returns></returns>
        public bool Exist(Expression<Func<T, bool>> anyLambda)
        {
            return Entity.Any(anyLambda);
        }
    }
}