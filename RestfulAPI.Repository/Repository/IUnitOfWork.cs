using System;
using System.Collections.Generic;
using System.Text;

namespace RestfulAPI.Repository.Repository
{
    /// <summary>
    /// 介面
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class;
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
    }
}

