using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using RestfulAPI.Model;
using System;
using System.Collections;
using System.Data;

namespace RestfulAPI.Repository.Repository
{
    /// <summary>
    /// 工作單元
    /// </summary>
    public class DapperUnitOfWork : IDapperUnitOfWork
    {
        private bool _disposed;
        private IDbTransaction _trans = null;
        /// <summary>
        /// 事務
        /// </summary>
        public IDbTransaction DbTransaction { get { return _trans; } }

        private IDbConnection _connection;
        /// <summary>
        /// 資料連接
        /// </summary>
        public IDbConnection DbConnection { get { return _connection; } }

        public DapperUnitOfWork(IDbConnection connection)
        {
            _connection = connection;//SqlConnection
            _connection.Open();
            _trans = _connection.BeginTransaction();
        }

        public IDapperRepository<T> GetRepository<T>() where T : class, IEntity => new DapperRepository<T>(_trans);

        /// <summary>
        /// 開啟事務
        /// </summary>
        public void BeginTransaction()
        {
            _trans = _connection.BeginTransaction();
        }
        /// <summary>
        /// 完成事務
        /// </summary>
        public void Commit() => _trans?.Commit();
        /// <summary>
        /// 回滾事務
        /// </summary>
        public void Rollback() => _trans?.Rollback();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DapperUnitOfWork() => Dispose(false);

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _trans?.Dispose();
                _connection?.Dispose();
            }

            _trans = null;
            _connection = null;
            _disposed = true;
        }
    }
}