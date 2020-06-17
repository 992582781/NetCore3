using Microsoft.Extensions.Configuration;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace UnitOfWork
{
    /// <summary>
    /// 工作单元
    /// </summary>
    public class UnitofWork : IUnitofWork
    {
        private bool _disposed;
        private IDbTransaction _trans = null;
        /// <summary>
        /// 事务
        /// </summary>
        public IDbTransaction DbTransaction { get { return _trans; } }

        private IDbConnection _connection;
        /// <summary>
        /// 数据连接
        /// </summary>
        public IDbConnection DbConnection { get { return _connection; } }

        public UnitofWork(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlConnection");
            //_connection = new MySqlConnection(connectionString);//MySqlConnector
            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        public void BeginTransaction()
        {
            _trans = _connection.BeginTransaction();
        }
        /// <summary>
        /// 完成事务
        /// </summary>
        public void Commit() => _trans?.Commit();
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback() => _trans?.Rollback();

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~UnitofWork() => Dispose(false);

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
