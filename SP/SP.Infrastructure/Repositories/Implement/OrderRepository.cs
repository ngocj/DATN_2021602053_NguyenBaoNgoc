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
      

    }

}
