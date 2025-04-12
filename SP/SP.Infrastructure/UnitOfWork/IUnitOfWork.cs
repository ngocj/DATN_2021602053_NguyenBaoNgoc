using SP.Infrastructure.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IRoleRepository RoleRepository { get; }
        Task<int> SaveChangeAsync();
    }
}
