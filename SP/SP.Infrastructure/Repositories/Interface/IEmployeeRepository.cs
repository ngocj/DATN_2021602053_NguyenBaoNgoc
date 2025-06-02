using SP.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SP.Infrastructure.Repositories.Interface
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        // Tổng số đơn hàng mà nhân viên đã xử lý
        Task<int> GetHandledOrderCountAsync(int employeeId);

        // Tổng doanh thu từ các đơn hàng mà nhân viên xử lý
        Task<decimal> GetRevenueByEmployeeAsync(int employeeId);

        // Thống kê doanh thu theo tháng của nhân viên
        Task<IEnumerable<(int Year, int Month, decimal Total)>> GetMonthlyRevenueByEmployeeAsync(int employeeId);

        // Danh sách các khách hàng mà nhân viên đã xử lý đơn
        Task<IEnumerable<string>> GetCustomerNamesHandledByAsync(int employeeId);
    }
}
