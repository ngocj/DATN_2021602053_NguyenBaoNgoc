using SP.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SP.Infrastructure.Repositories.Implement.OrderDetailRepository;

namespace SP.Application.Service.Interface
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetail>> GetAllOrderDetails();
        Task<OrderDetail> GetOrderDetailById(Guid orderId, int productVariantId);
        Task CreateOrderDetail(OrderDetail orderDetail);
        Task UpdateOrderDetail(OrderDetail orderDetail);
        Task DeleteOrderDetail(Guid orderId, int productVariantId);

        // Tổng doanh thu toàn hệ thống
        Task<decimal> GetTotalRevenueAsync();

        // Tổng doanh thu theo khoảng thời gian
        Task<decimal> GetTotalRevenueAsync(DateTime from, DateTime to);

        // Thống kê doanh thu theo tháng
        Task<IEnumerable<(int Year, int Month, decimal Total)>> GetMonthlyRevenueAsync();

        // Số đơn hàng đã hoàn thành
        Task<int> GetCompletedOrderCountAsync();

        // Số sản phẩm đã bán
        Task<int> GetTotalProductSoldAsync();

        // Thống kê top sản phẩm bán chạy nhất
        Task<IEnumerable<TopSellingVariant>> GetTopSellingVariantsAsync(int top = 5);

        // Thống kê tổng số sản phẩm đã giao
        Task<int> GetTotalProductDeliveredAsync();

        // Thống kê tổng số sản phẩm đã hủy
        Task<int> GetTotalProductCanceledAsync();

        // Thống kê tổng số sản phẩm đang giao
        Task<int> GetTotalProductShippingAsync();

        // Thống kê tổng số sản phẩm đang chờ
        Task<int> GetTotalProductPendingAsync();




    }
}
