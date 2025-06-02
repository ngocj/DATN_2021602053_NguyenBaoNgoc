using Microsoft.EntityFrameworkCore;
using SP.Domain.Entity;
using SP.Infrastructure.Context;
using SP.Infrastructure.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Infrastructure.Repositories.Implement
{
    public class EmployeeRepository : GenericRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(SPContext context) : base(context)
        {
        }
        public override async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _SPContext.Set<Employee>()
                .Include(e => e.Role)
                .ToListAsync();
        }
        public override async Task<Employee> GetByIdAsync(Guid id)
        {
            return await _SPContext.Set<Employee>()
                .Include(e => e.Role)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public Task<IEnumerable<string>> GetCustomerNamesHandledByAsync(int employeeId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetHandledOrderCountAsync(int employeeId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<(int Year, int Month, decimal Total)>> GetMonthlyRevenueByEmployeeAsync(int employeeId)
        {
            throw new NotImplementedException();
        }

        public Task<decimal> GetRevenueByEmployeeAsync(int employeeId)
        {
            throw new NotImplementedException();
        }
    }
    
}
