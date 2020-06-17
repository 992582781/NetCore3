using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using RongKang.IBase;

namespace RongKang.IRepository
{
    public interface IUserRepository : IBaseRepository<User1>
    {
        Task<long> Insert11(User1 entity);
    }
}
