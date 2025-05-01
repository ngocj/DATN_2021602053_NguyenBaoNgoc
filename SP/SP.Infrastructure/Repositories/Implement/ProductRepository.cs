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
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(SPContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Product>> GetAllBySubCategoryIdAsync(int subCategoryId)
        {
            return await _SPContext.Set<Product>()
                    .Include(p => p.SubCategory)
                    .Where(p => p.SubCategoryId == subCategoryId)
                    .ToListAsync();
        }
        // get all brand
        public async Task<IEnumerable<Product>> GetAllByBrandIdAsync(int brandId)
        {
            return await _SPContext.Set<Product>()
                    .Include(p => p.Brand)
                    .Where(p => p.BrandId == brandId)
                    .ToListAsync();
        }
    }
   
}
