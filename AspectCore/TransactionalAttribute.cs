using AspectCore.DynamicProxy;
using System;
using System.Threading.Tasks;
using UnitOfWork;

namespace AspectCore
{
    /// <summary>
    /// 为工作单元提供事务一致性
    /// </summary>
    public class TransactionalAttribute : AbstractInterceptorAttribute
    {
        IUnitofWork _unitOfWork { get; set; }

        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            try
            {
                _unitOfWork = context.ServiceProvider.GetService(typeof(IUnitofWork)) as IUnitofWork;
                _unitOfWork.BeginTransaction();
                await next(context);
                _unitOfWork.Commit();
            }
            catch (Exception)
            {
                _unitOfWork.Rollback();
                throw;
            }
        }
    }
}
