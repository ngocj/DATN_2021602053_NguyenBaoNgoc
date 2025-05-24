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
        public async Task<IEnumerable<Product>> GetAllByLastestAsync(decimal? priceFrom, decimal? priceTo, int categoryId, int? subCategoryId, int? brandId)
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

        // arrange product by price descending
        public async Task<IEnumerable<Product>> GetAllByPriceDescendingAsync(decimal? priceFrom, decimal? priceTo, int categoryId, int? subCategoryId, int? brandId)
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
        // arrange product by price ascending
        public async Task<IEnumerable<Product>> GetAllByPriceAscendingAsync(decimal? priceFrom, decimal? priceTo, int categoryId, int? subCategoryId, int? brandId)
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
        // sort product by best selling products
        public async Task<IEnumerable<Product>> GetAllByBestSellingAsync(
        decimal? priceFrom, decimal? priceTo, int categoryId, int? subCategoryId, int? brandId)
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

        // filer product by category and brand and isActives
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
    }

}
