using Microsoft.EntityFrameworkCore;
using RestfulAPI.Model;
using System;
using System.Collections;

namespace RestfulAPI.Repository.Repository
{
    /// <summary>
    /// 實現類
    /// </summary>
    public class UnitOfWork<TDbContext> : IUnitOfWork where TDbContext : DbContext
    {
        /// <summary>
        /// dbContext上下文
        /// </summary>
        private readonly MyDbContext _dbContext;

        /// <summary>
        /// 構造函數
        /// </summary>
        /// <param name="dbContext"></param>
        public UnitOfWork(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IRepository<T> GetRepository<T>() where T : class => new Repository<T>(_dbContext);

        /// <summary>
        /// 保存
        /// </summary>
        public int SaveChanges()
        {
            int code;
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    code = _dbContext.SaveChanges();
                    dbContextTransaction.Commit();
                }
                catch (Exception)
                {
                    dbContextTransaction.Rollback();
                    throw;
                }
            }
            return code;
        }

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _dbContext.Dispose();
                }
            }
            this.disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}