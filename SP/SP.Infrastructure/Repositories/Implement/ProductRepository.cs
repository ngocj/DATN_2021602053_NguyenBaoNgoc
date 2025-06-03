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
        public override async Task<Product> GetByIdAsync(int id)
        {
            return await _SPContext.Set<Product>()
                .Include(p => p.SubCategory)
                    .ThenInclude(sc => sc.Category) // thêm dòng này
                .Include(p => p.Brand)
                .Include(p => p.Discount)
                .Include(p => p.ProductVariants)
                    .ThenInclude(pv => pv.Images)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
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
        public async Task<IEnumerable<Product>> GetAllByBrandIdAsync(int brandId)
        {
            return await _SPContext.Set<Product>()
                    .Include(p => p.Brand)
                    .Where(p => p.BrandId == brandId)
                    .ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetAllByCategoryIdAsync(int categoryId)
        {
            return await _SPContext.Set<Product>()
                .Include(p => p.ProductVariants)
                .ThenInclude(pv => pv.Images)
                .Include(p => p.Brand)
                .Include(p => p.Discount)
                .Include(p => p.SubCategory)
                .Where(p => p.SubCategory.CategoryId == categoryId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetAllByCategoryAndBrandAsync(int? SubcategoryId, int? brandId, bool? isActive)
        {
            var query = _SPContext.Set<Product>()
                    .Include(p => p.SubCategory)
                    .Include(p => p.Brand)
                    .Include(p => p.Discount)
                    .Include(p => p.ProductVariants)
                    .ThenInclude(pv => pv.Images)
                    .AsQueryable();

            if (SubcategoryId.HasValue)
            {
                query = query.Where(p => p.SubCategoryId == SubcategoryId.Value);
            }

            if (brandId.HasValue)
            {
                query = query.Where(p => p.BrandId == brandId.Value);
            }

            if (isActive.HasValue)
            {
                query = query.Where(p => p.IsActive == isActive.Value);
            }

            return await query.ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetAllByLastestAsync(decimal? priceFrom, decimal? priceTo, int categoryId, int? subCategoryId, int? brandId, string? search)
        {
            var query = _SPContext.Set<Product>()
                .Include(p => p.ProductVariants).ThenInclude(pv => pv.Images)
                .Include(p => p.Brand)
                .Include(p => p.Discount)
                .Include(p => p.SubCategory)
                .Where(p => p.SubCategory.CategoryId == categoryId);

            if (subCategoryId.HasValue)
            {
                query = query.Where(p => p.SubCategory.Id == subCategoryId.Value);
            }

            if (brandId.HasValue)
            {
                query = query.Where(p => p.Brand.Id == brandId.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                var loweredSearch = search.ToLower();
                query = query.Where(p => p.ProductName.ToLower().Contains(loweredSearch));
            }

            if (priceFrom.HasValue || priceTo.HasValue)
            {
                query = query.Where(p =>
                    p.ProductVariants.Any(pv =>
                        (!priceFrom.HasValue ||
                         (pv.Price * (1 - (p.Discount != null ? (decimal)p.Discount.Percent : 0) / 100)) >= priceFrom.Value)
                        &&
                        (!priceTo.HasValue ||
                         (pv.Price * (1 - (p.Discount != null ? (decimal)p.Discount.Percent : 0) / 100)) <= priceTo.Value)
                    )
                );
            }
            return await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetAllByPriceDescendingAsync(decimal? priceFrom, decimal? priceTo, int categoryId, int? subCategoryId, int? brandId, string? search)
        {
            var query = _SPContext.Set<Product>()
                .Include(p => p.ProductVariants).ThenInclude(pv => pv.Images)
                .Include(p => p.Brand)
                .Include(p => p.Discount)
                .Include(p => p.SubCategory)
                .Where(p => p.SubCategory.CategoryId == categoryId);

            if (subCategoryId.HasValue)
            {
                query = query.Where(p => p.SubCategory.Id == subCategoryId.Value);
            }

            if (brandId.HasValue)
            {
                query = query.Where(p => p.Brand.Id == brandId.Value);
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                var loweredSearch = search.ToLower();
                query = query.Where(p => p.ProductName.ToLower().Contains(loweredSearch));
            }

            if (priceFrom.HasValue || priceTo.HasValue)
            {
                query = query.Where(p =>
                    p.ProductVariants.Any(pv =>
                        (!priceFrom.HasValue ||
                         (pv.Price * (1 - (p.Discount != null ? (decimal)p.Discount.Percent : 0) / 100)) >= priceFrom.Value)
                        &&
                        (!priceTo.HasValue ||
                         (pv.Price * (1 - (p.Discount != null ? (decimal)p.Discount.Percent : 0) / 100)) <= priceTo.Value)
                    )
                );
            }
            return await query.OrderByDescending(p => p.ProductVariants.FirstOrDefault().Price).ToListAsync();
        }
        public async Task<IEnumerable<Product>> GetAllByPriceAscendingAsync(decimal? priceFrom, decimal? priceTo, int categoryId, int? subCategoryId, int? brandId, string? search)
        {
            var query = _SPContext.Set<Product>()
               .Include(p => p.ProductVariants).ThenInclude(pv => pv.Images)
               .Include(p => p.Brand)
               .Include(p => p.Discount)
               .Include(p => p.SubCategory)
               .Where(p => p.SubCategory.CategoryId == categoryId);

            if (subCategoryId.HasValue)
            {
                query = query.Where(p => p.SubCategory.Id == subCategoryId.Value);
            }

            if (brandId.HasValue)
            {
                query = query.Where(p => p.Brand.Id == brandId.Value);
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                var loweredSearch = search.ToLower();
                query = query.Where(p => p.ProductName.ToLower().Contains(loweredSearch));
            }

            if (priceFrom.HasValue || priceTo.HasValue)
            {
                query = query.Where(p =>
                    p.ProductVariants.Any(pv =>
                        (!priceFrom.HasValue ||
                         (pv.Price * (1 - (p.Discount != null ? (decimal)p.Discount.Percent : 0) / 100)) >= priceFrom.Value)
                        &&
                        (!priceTo.HasValue ||
                         (pv.Price * (1 - (p.Discount != null ? (decimal)p.Discount.Percent : 0) / 100)) <= priceTo.Value)
                    )
                );
            }
            return await query.OrderBy(p => p.ProductVariants.FirstOrDefault().Price).ToListAsync();
           
        }
        public async Task<IEnumerable<Product>> GetAllByBestSellingAsync(decimal? priceFrom, decimal? priceTo, int categoryId, int? subCategoryId, int? brandId, string? search)
        {
            var query = _SPContext.Set<Product>()
                .Include(p => p.ProductVariants)
                    .ThenInclude(pv => pv.Images)
                .Include(p => p.Brand)
                .Include(p => p.Discount)
                .Include(p => p.SubCategory)
                .Where(p => p.SubCategory.CategoryId == categoryId);

            if (subCategoryId.HasValue)
            {
                query = query.Where(p => p.SubCategoryId == subCategoryId.Value);
            }

            if (brandId.HasValue)
            {
                query = query.Where(p => p.BrandId == brandId.Value);
            }
            if (!string.IsNullOrWhiteSpace(search))
            {
                var loweredSearch = search.ToLower();
                query = query.Where(p => p.ProductName.ToLower().Contains(loweredSearch));
            }

            if (priceFrom.HasValue || priceTo.HasValue)
            {
                query = query.Where(p => p.ProductVariants.Any(v =>
                    (!priceFrom.HasValue || v.Price >= priceFrom.Value) &&
                    (!priceTo.HasValue || v.Price <= priceTo.Value)
                ));
            }

            var queryWithSales = query
                .Select(p => new
                {
                    Product = p,
                    TotalSold = _SPContext.OrderDetails
                        .Where(od => od.ProductVariant.ProductId == p.Id)
                        .Sum(od => (int?)od.Quantity) ?? 0
                });

            var result = await queryWithSales
                .OrderByDescending(x => x.TotalSold)
                .Select(x => x.Product)
                .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<Product>> GetTop10BestSellingAsync()
        {
            var result = await _SPContext.OrderDetails
                .GroupBy(od => od.ProductVariant.ProductId)
                .Select(g => new
                {
                    ProductId = g.Key,
                    TotalSold = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.TotalSold)
                .Take(10)
                .Join(_SPContext.Products
                        .Include(p => p.Brand)
                        .Include(p => p.SubCategory)
                        .Include(p => p.Discount)
                        .Include(p => p.ProductVariants)
                            .ThenInclude(pv => pv.Images),
                      g => g.ProductId,
                      p => p.Id,
                      (g, p) => p)
                .ToListAsync();

            return result;
        }
        public async Task<IEnumerable<Product>> GetTop10NewestAsync()
        {
            return await _SPContext.Products
                .Include(p => p.Brand)
                .Include(p => p.SubCategory)
                .Include(p => p.Discount)
                .Include(p => p.ProductVariants)
                    .ThenInclude(pv => pv.Images)
                .OrderByDescending(p => p.CreatedAt)
                .Take(10)
                .ToListAsync();
        }


    }

}
