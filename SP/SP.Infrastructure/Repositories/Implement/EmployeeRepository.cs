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

        public async Task<IEnumerable<string>> GetCustomerNamesHandledByAsync(Guid employeeId)
        {
            return await _SPContext.Orders
               .Where(o => o.EmployeeId == employeeId)
               .Select(o => o.User.UserName)
               .Distinct()
               .ToListAsync();
        }

        public async Task<int> GetHandledOrderCountAsync(Guid employeeId)
        {
            return await _SPContext.Orders
                .CountAsync(o => o.EmployeeId == employeeId && o.Status == OrderStatus.Delivered);
        }

        public  async Task<decimal> GetRevenueByEmployeeAsync(Guid employeeId)
        {
            return await _SPContext.Orders
               .Where(o => o.EmployeeId == employeeId)
               .SumAsync(o => o.TotalPrice);
        }
        public class HandledOrderDto
        {
            public Guid OrderId { get; set; }
            public string CustomerName { get; set; }
            public DateTime OrderDate { get; set; }
            public decimal TotalPrice { get; set; }
            public OrderStatus Status { get; set; }
        }

        public async Task<IEnumerable<HandledOrderDto>> GetHandledOrdersByEmployeeAsync(Guid employeeId)
        {
            return await _SPContext.Orders
                .Where(o => o.EmployeeId == employeeId && o.Status == OrderStatus.Delivered)
                .Select(o => new HandledOrderDto
                {
                    OrderId = o.Id,
                    CustomerName = o.User.UserName,
                    OrderDate = o.CreatedAt,
                    TotalPrice = o.TotalPrice,
                    Status = o.Status
                })
                .ToListAsync();
        }





    }

}
