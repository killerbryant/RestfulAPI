using Dapper;
using Dapper.Contrib.Extensions;
using RestfulAPI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace RestfulAPI.Repository.Repository
{
    public class DapperRepository<T> : IDapperRepository<T> where T : class, IEntity
    {
        protected IDbTransaction Transaction { get; }
        protected IDbConnection Connection => Transaction.Connection;


        public DapperRepository(IDbTransaction transaction)
        {
            Transaction = transaction;
        }
        /// <summary>
        /// 查詢
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public IEnumerable<T> Query(string sql, object param = null, CommandType? commandType = null)
        {
            var r = Connection.Query<T>(sql, param: param, transaction: Transaction, commandType: commandType);
            return r;
        }
        /// <summary>
        /// 刪除行資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityToDelete"></param>
        /// <returns></returns>
        public bool Delete(T entity)
        {
            var r = Connection.Delete<T>(entity, Transaction);
            return r;
        }
        /// <summary>
        /// 刪除表所有資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public bool DeleteAll()
        {
            var r = Connection.DeleteAll<T>(Transaction);
            return r;
        }
        /// <summary>
        /// 獲取行資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Get(object id)
        {
            var r = Connection.Get<T>(id, Transaction);
            return r;
        }
        /// <summary>
        /// 獲取表的所有資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetAll()
        {

            var r = Connection.GetAll<T>(Transaction);
            return r;
        }
        /// <summary>
        /// 添加行數據
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public long Add(T entity)
        {
            var r = Connection.Insert<T>(entity, Transaction);
            return r;
        }
        /// <summary>
        /// 更新行資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool Update(T entity)
        {
            var r = Connection.Update<T>(entity, Transaction);
            return r;
        }
        /// <summary>
        /// 分頁方法
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex">當前頁碼</param>
        /// <param name="pageSize">每頁顯示條數</param>
        /// <param name="param">參數</param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        //public PagedResult<T> GetPageList(string sql, int pageIndex, int pageSize, object param = null)
        //{
        //    var pagingResult = _unitOfWork.DbConnection.GetPageList<T>(sql, pageIndex, pageSize, param: param, transaction: _unitOfWork.DbTransaction);
        //    return pagingResult;
        //}
    }
}