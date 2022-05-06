using RestfulAPI.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace RestfulAPI.Repository.Repository
{
    public interface IDapperUnitOfWork : IDisposable
    {
        /// <summary>
        /// 事务
        /// </summary>
        IDbTransaction DbTransaction { get; }
        /// <summary>
        /// 数据连接
        /// </summary>
        IDbConnection DbConnection { get; }

        IDapperRepository<T> GetRepository<T>() where T : class, IEntity;

        /// <summary>
        /// 开启事务
        /// </summary>
        void BeginTransaction();
        /// <summary>
        /// 完成事务
        /// </summary>
        void Commit();
        /// <summary>
        /// 回滚事务
        /// </summary>
        void Rollback();
    }
}
