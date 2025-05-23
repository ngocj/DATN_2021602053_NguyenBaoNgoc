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
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(SPContext context) : base(context)
        {

        }
        public override async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _SPContext.Set<Order>()
                .Include(fb => fb.User)
                .Include(fb => fb.Employee)
                .Include(fb => fb.OrderDetails)
                    .ThenInclude(od => od.ProductVariant)
                    .ThenInclude(pv => pv.Product)
                .ToListAsync();
        }
        public override async Task<Order> GetByIdAsync(Guid id)
        {
            var order = await _SPContext.Set<Order>()
                .Include(fb => fb.User)
                .Include(fb => fb.Employee)
                .Include(fb => fb.OrderDetails)
                    .ThenInclude(od => od.ProductVariant)
                        .ThenInclude(pv => pv.Images)
                .Include(fb => fb.OrderDetails)
                    .ThenInclude(od => od.ProductVariant)
                        .ThenInclude(pv => pv.Product)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order != null)
            {
                // Nếu bạn lưu giá tại thời điểm đặt hàng trong OrderDetail.Price
                order.TotalPrice = order.OrderDetails.Sum(od => od.ProductVariant.Price * od.Quantity);

                // Nếu bạn muốn dùng giá hiện tại trong ProductVariant (ít khuyến khích)
                // order.TotalPrice = order.OrderDetails.Sum(od => od.ProductVariant.Price * od.Quantity);
            }

            return order;
        }

    }

}
