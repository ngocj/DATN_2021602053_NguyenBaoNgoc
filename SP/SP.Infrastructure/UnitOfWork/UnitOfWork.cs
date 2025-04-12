using SP.Infrastructure.Context;
using SP.Infrastructure.Repositories.Implement;
using SP.Infrastructure.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace SP.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SPContext _SPContext;

        public UnitOfWork(SPContext sPContext)
        {
            _SPContext = sPContext;
            RoleRepository = new RoleRepository(_SPContext);
        }

        public IRoleRepository RoleRepository { get; }

        public Task<int> SaveChangeAsync()
        {
           return _SPContext.SaveChangesAsync();
        }
        public void Dispose()
        {
            _SPContext.Dispose();
        }
    }
}
