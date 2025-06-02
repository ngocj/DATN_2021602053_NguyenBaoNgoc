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
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(SPContext context) : base(context)
        {

        }

        public override async Task<OrderDetail> GetByCompositeKeyAsync(Guid id1, int id2)
        {
            return await _SPContext.OrderDetails
                .Include(od => od.ProductVariant)
                    .ThenInclude(pv => pv.Product)
                .Include(od => od.ProductVariant)
                    .ThenInclude(pv => pv.Images)
                .Include(od => od.FeedBacks) // Include Feedback
                .FirstOrDefaultAsync(od => od.OrderId == id1 && od.ProductVariantId == id2);
        }

        public override async Task<IEnumerable<OrderDetail>> GetAllAsync()
        {
            return await _SPContext.OrderDetails
                .Include(od => od.ProductVariant)
                    .ThenInclude(pv => pv.Product)
                .Include(od => od.ProductVariant)
                    .ThenInclude(pv => pv.Images)
                .Include(od => od.FeedBacks) // Include Feedback
                .ToListAsync();
        }

        public async Task<decimal> GetTotalRevenueAsync()
        {
            return await _SPContext.OrderDetails
                .SumAsync(od => od.Price * od.Quantity);
        }

        public async Task<decimal> GetTotalRevenueAsync(DateTime from, DateTime to)
        {
            var toInclusive = to.Date.AddDays(1);

            return await _SPContext.OrderDetails
                .Where(od => od.Order.CreatedAt >= from && od.Order.CreatedAt < toInclusive)
                .SumAsync(od => od.Price * od.Quantity);
        }

        public async Task<IEnumerable<(int Year, int Month, decimal Total)>> GetMonthlyRevenueAsync()
        {
            return await _SPContext.OrderDetails
                .Where(od => od.Order.Status == OrderStatus.Delivered)
                .GroupBy(od => new { od.Order.CreatedAt.Year, od.Order.CreatedAt.Month })
                .Select(g => new ValueTuple<int, int, decimal>(
                    g.Key.Year,
                    g.Key.Month,
                    g.Sum(od => od.Price * od.Quantity)
                ))
                .ToListAsync();
        }


        public async Task<int> GetCompletedOrderCountAsync()
        {
            return await _SPContext.Orders
                .CountAsync(o => o.Status == OrderStatus.Delivered);
        }

        public async Task<int> GetTotalProductPendingAsync()
        {
            return await _SPContext.OrderDetails
                .CountAsync(od => od.Order.Status == OrderStatus.Pending);
        }
        public async Task<int> GetTotalProductDeliveredAsync()
        {
            return await _SPContext.OrderDetails
                .CountAsync(od => od.Order.Status == OrderStatus.Delivered);
        }

        public async Task<int> GetTotalProductCanceledAsync()
        {
            return await _SPContext.OrderDetails
                .CountAsync(od => od.Order.Status == OrderStatus.Canceled);
        }

        public async Task<int> GetTotalProductShippingAsync()
        {
            return await _SPContext.OrderDetails
                .CountAsync(od => od.Order.Status == OrderStatus.Shipping);
        }



        public async Task<int> GetTotalProductSoldAsync()
        {
            return await _SPContext.OrderDetails
                .SumAsync(od => od.Quantity);
        }


        public async Task<IEnumerable<TopSellingVariant>> GetTopSellingVariantsAsync(int top = 5)
        {
            if (top <= 0)
                throw new ArgumentException("Top phải lớn hơn 0.");

            return await _SPContext.OrderDetails
                .Include(od => od.ProductVariant) // Bao gồm ProductVariant để lấy Name
                .GroupBy(od => od.ProductVariantId)
                .OrderByDescending(g => g.Sum(od => od.Quantity))
                .Take(top)
                .Select(g => new TopSellingVariant
                {
                    ProductVariantId = g.Key,
                    Quantity = g.Sum(od => od.Quantity),
                    Name = g.First().ProductVariant != null ? g.First().ProductVariant.Product.ProductName : "Không xác định"
                })
                .ToListAsync();
        }

       

        public class TopSellingVariant
        {
            public int ProductVariantId { get; set; }
            public int Quantity { get; set; }
            public string Name { get; set; }
        }
    }

}
