using Dapper;
using Dapper.Contrib.Extensions;
using RongKang.IRepository;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using UnitOfWork;

namespace RongKang.Repository
{
    /// <summary>
    /// 泛型Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private IUnitofWork _unitOfWork;

        public BaseRepository(IUnitofWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// 执行sql返回DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="useWriteConn"></param>
        /// <returns></returns>
        public async Task<DataTable> ExecuteSqlReturnDataTable(string sql, object param = null, CommandType? commandType = null)
        {
            DataTable dt = new DataTable();
            IDataReader idr = await _unitOfWork.DbConnection.ExecuteReaderAsync(sql, param, transaction: _unitOfWork.DbTransaction,
                commandType: commandType);
            dt.Load(idr);
            return await Task.FromResult(dt);
        }

        /// <summary>
        /// 执行存储过程返回DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="useWriteConn"></param>
        /// <returns></returns>
        public async Task<DataTable> ExecuteProcedureReturnDataTable(string sql, object param = null, CommandType? commandType = null)
        {

            DataTable dt = new DataTable();
            IDataReader idr = await _unitOfWork.DbConnection.ExecuteReaderAsync(sql, param, transaction: _unitOfWork.DbTransaction,
                commandType: CommandType.StoredProcedure);
            dt.Load(idr);
            return await Task.FromResult(dt);
        }

        /// <summary>
        /// 执行存储过程返回多个对象
        /// DynamicParameters para = new DynamicParameters();
        /// para.Add("@CurrentStatus", "");
        /// para.Add("@Start", "");
        /// para.Add("@End", "");
        /// para.Add("@IsList", IsList);
        /// //调用存储过程
        ///return DapperHelper.ExecuteProcedureReturnList<T>(ProDuce, para);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="useWriteConn"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public async Task<List<T>> ExecuteProcedureReturnListAsync(string sql, object param = null, CommandType? commandType = null)
        {
            var r = await _unitOfWork.DbConnection.QueryAsync<T>(sql, param: param, transaction: _unitOfWork.DbTransaction, commandType: CommandType.StoredProcedure);
            return r.ToList();

        }

        /// <summary>
        /// 执行sql返回多个对象--异步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public async Task<List<T>> Query(string sql, object param = null, CommandType? commandType = null)
        {
            var r = await _unitOfWork.DbConnection.QueryAsync<T>(sql, param: param, transaction: _unitOfWork.DbTransaction, commandType: commandType);
            return r.ToList();
        }

        /// <summary>
        /// 执行sql返回一个对象--异步
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="param"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public async Task<T> QueryFirstOrDefault(string sql, object param = null, CommandType? commandType = null)
        {
            var r = await _unitOfWork.DbConnection.QueryFirstOrDefaultAsync<T>(sql, param: param, transaction: _unitOfWork.DbTransaction, commandType: commandType);
            return r;
        }

        public async Task<long> Execute(string sql, object param = null, CommandType? commandType = null)
        {
            var r = await _unitOfWork.DbConnection.ExecuteAsync(sql, param: param, transaction: _unitOfWork.DbTransaction, commandType: commandType);
            return r;
        }


        /// <summary>
        /// 删除行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entityToDelete"></param>
        /// <returns></returns>
        public async Task<bool> Delete(T entity)
        {
            var r = await  _unitOfWork.DbConnection.DeleteAsync<T>(entity, _unitOfWork.DbTransaction);
            return r;
        }
        /// <summary>
        /// 删除表所有数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<bool> DeleteAll()
        {
            var r = await _unitOfWork.DbConnection.DeleteAllAsync<T>(_unitOfWork.DbTransaction);
            return r;
        }
        /// <summary>
        /// 获取行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<T> Get(object id)
        {
            var r = await  _unitOfWork.DbConnection.GetAsync<T>(id, _unitOfWork.DbTransaction);
            return r;
        }
        /// <summary>
        /// 获取表的所有数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public async Task<List<T>> GetAll()
        {
            var r = await _unitOfWork.DbConnection.GetAllAsync<T>(_unitOfWork.DbTransaction);
            return r.ToList();
        }
        /// <summary>
        /// 添加行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<long> Insert(T entity)
        {
            var r = await  _unitOfWork.DbConnection.InsertAsync<T>(entity, _unitOfWork.DbTransaction);
            return r;
        }
        /// <summary>
        /// 更新行数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<bool> Update(T entity)
        {
            var r = await  _unitOfWork.DbConnection.UpdateAsync<T>(entity, _unitOfWork.DbTransaction);
            return r;
        }



        /// <summary>
        /// 批量插入功能
        /// </summary>
        public void InsertBatch(IEnumerable<T> entityList)
        {
            var tblName = string.Format("dbo.{0}", typeof(T).Name);
            var tran = (SqlTransaction)_unitOfWork.DbTransaction;

            using (var bulkCopy = new SqlBulkCopy(_unitOfWork.DbConnection as SqlConnection, SqlBulkCopyOptions.TableLock, tran))
            {
                bulkCopy.BatchSize = entityList.Count();
                bulkCopy.DestinationTableName = tblName;
                var table = new DataTable();
                DapperExtensions.Sql.ISqlGenerator sqlGenerator = new DapperExtensions.Sql.SqlGeneratorImpl(new DapperExtensions.DapperExtensionsConfiguration());
                var classMap = sqlGenerator.Configuration.GetMap<T>();
                var props = classMap.Properties.Where(x => x.Ignored == false).ToArray();
                foreach (var propertyInfo in props)
                {
                    bulkCopy.ColumnMappings.Add(propertyInfo.Name, propertyInfo.Name);
                    table.Columns.Add(propertyInfo.Name, Nullable.GetUnderlyingType(propertyInfo.PropertyInfo.PropertyType) ?? propertyInfo.PropertyInfo.PropertyType);
                }
                var values = new object[props.Count()];
                foreach (var itemm in entityList)
                {
                    for (var i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].PropertyInfo.GetValue(itemm, null);
                    }
                    table.Rows.Add(values);
                }
                bulkCopy.WriteToServer(table);
            }
        }


        /// <summary>
        /// 分页方法
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="pageIndex">当前页码</param>
        /// <param name="pageSize">每页显示条数</param>
        /// <param name="param">参数</param>
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
