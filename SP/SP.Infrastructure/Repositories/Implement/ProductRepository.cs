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
        // get product by id
        public override async Task<Product> GetByIdAsync(int id)
        {
            // include productVariant and image
            return await _SPContext.Set<Product>()
                    .Include(c => c.SubCategory)
                    .Include(c => c.Brand)
                    .Include(p => p.Discount)
                    .Include(p => p.ProductVariants)
                    .ThenInclude(pv => pv.Images)
                    .FirstOrDefaultAsync(p => p.Id == id);
        }
        // get all product
        public override async Task<IEnumerable<Product>> GetAllAsync()
        {
           // include productVariant and image
            return await _SPContext.Set<Product>()
                    .Include(c => c.SubCategory)
                    .Include(c => c.Brand)
                    .Include(p => p.Discount)
                    .Include(p => p.ProductVariants)
                    .ThenInclude(pv => pv.Images)
                    .ToListAsync();
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
        // arrage product by lastest
        public async Task<IEnumerable<Product>> GetAllByLastestAsync()
        {
            return await _SPContext.Set<Product>()
                    .Include(p => p.ProductVariants)
                    .ThenInclude(pv => pv.Images)
                    .OrderByDescending(p => p.CreatedAt)
                    .ToListAsync();
        }
        // arrange product by price descending
        public async Task<IEnumerable<Product>> GetAllByPriceDescendingAsync()
        {
            return await _SPContext.Set<Product>()
                    .Include(p => p.ProductVariants)
                    .ThenInclude(pv => pv.Images)
                    .OrderByDescending(p => p.ProductVariants.FirstOrDefault().Price)
                    .ToListAsync();
        }
        // arrange product by price ascending
        public async Task<IEnumerable<Product>> GetAllByPriceAscendingAsync()
        {
            return await _SPContext.Set<Product>()
                    .Include(p => p.ProductVariants)
                    .ThenInclude(pv => pv.Images)
                    .OrderBy(p => p.ProductVariants.FirstOrDefault().Price)
                    .ToListAsync();
        }
        // sort product by best selling products
        public async Task<IEnumerable<Product>> GetAllByBestSellingAsync()
        {
            return await _SPContext.Set<Product>()
                    .Include(p => p.ProductVariants)
                    .ThenInclude(pv => pv.Images)
                    .OrderByDescending(p => p.ProductVariants.FirstOrDefault().Quantity)
                    .ToListAsync();
        }
        // sort product by a -> z
        public async Task<IEnumerable<Product>> GetAllByAZAsync()
        {
            return await _SPContext.Set<Product>()
                    .Include(p => p.ProductVariants)
                    .ThenInclude(pv => pv.Images)
                    .OrderBy(p => p.ProductName)
                    .ToListAsync();
        }
        // sort product by z -> a
        public async Task<IEnumerable<Product>> GetAllByZAAsync()
        {
            return await _SPContext.Set<Product>()
                    .Include(p => p.ProductVariants)
                    .ThenInclude(pv => pv.Images)
                    .OrderByDescending(p => p.ProductName)
                    .ToListAsync();
        }
        // sort product by older -> newer
        public async Task<IEnumerable<Product>> GetAllByOlderAsync()
        {
            return await _SPContext.Set<Product>()
                    .Include(p => p.ProductVariants)
                    .ThenInclude(pv => pv.Images)
                    .OrderBy(p => p.CreatedAt)
                    .ToListAsync();
        }
    }

}
