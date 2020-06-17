using Dapper.Contrib.Extensions;
using RongKang.IBase;
using RongKang.IRepository;
using RongKang.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using UnitOfWork;

namespace Repository
{
    public class UserRepository : BaseRepository<User1>, IUserRepository
    {
        private IUnitofWork _unitOfWork;

        public UserRepository(IUnitofWork unitOfWork)
        : base(unitOfWork)
        {
            this._unitOfWork = unitOfWork;//在构造函数中初始化类的dal属性
        }

        public async Task<long> Insert11(User1 entity)
        {
            var r = await _unitOfWork.DbConnection.InsertAsync<User1>(entity, _unitOfWork.DbTransaction);
            return r;
        }
    }
}
