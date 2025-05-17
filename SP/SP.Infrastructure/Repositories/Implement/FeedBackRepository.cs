using Microsoft.EntityFrameworkCore;
using SP.Domain.Entity;
using SP.Infrastructure.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Infrastructure.Repositories.Implement
{
    public class FeedBackRepository : GenericRepository<FeedBack>,IFeedbackRepository
    {
        public FeedBackRepository(Context.SPContext context) : base(context)
        {
        }
        public override async Task<IEnumerable<FeedBack>> GetAllAsync()
        {
            return await _SPContext.Set<FeedBack>()
                         .Include(fb => fb.User)
                         .Include(fb => fb.OrderDetail)
                         .ThenInclude(od => od.ProductVariant)
                         .ThenInclude(pv => pv.Product)
                         .ToListAsync();
        }
        public override async Task<FeedBack> GetByIdAsync(int id)
        {
            return await _SPContext.Set<FeedBack>()
                         .Include(fb => fb.User)
                         .Include(fb => fb.OrderDetail)
                         .ThenInclude(od => od.ProductVariant)
                         .ThenInclude(pv => pv.Product)
                         .FirstOrDefaultAsync(fb => fb.Id == id);
        }

    }
    
}
