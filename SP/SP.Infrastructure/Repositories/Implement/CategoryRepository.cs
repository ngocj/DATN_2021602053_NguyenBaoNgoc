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
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(SPContext sPContext) : base(sPContext)
        {
        }
        public async override Task<IEnumerable<Category>> GetAllAsync()
        {
            
            return await _SPContext.Set<Category>()
                .Include(c => c.SubCategories)
                .ToListAsync();
        }
    }
}
