using SP.Domain.Entity;
using SP.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Infrastructure.Repositories.Interface
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<IEnumerable<Product>> GetAllByBrandIdAsync(int brandId);
        Task<IEnumerable<Product>> GetAllByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Product>> GetAllBySubCategoryIdAsync(int subCategoryId);
        Task<IEnumerable<Product>> GetAllByCategoryAndBrandAsync(int? SubcategoryId, int? brandId, bool? isActive);
        Task<IEnumerable<Product>> GetAllByLastestAsync(decimal? priceFrom, decimal? priceTo, int categoryId, int? subCategoryId, int? brandId,string? search);
        Task<IEnumerable<Product>> GetAllByBestSellingAsync(decimal? priceFrom, decimal? priceTo, int categoryId, int? subCategoryId, int? brandId, string? search);
        Task<IEnumerable<Product>> GetAllByPriceAscendingAsync(decimal? priceFrom, decimal? priceTo, int categoryId, int? subCategoryId, int? brandId, string? search);
        Task<IEnumerable<Product>> GetAllByPriceDescendingAsync(decimal? priceFrom, decimal? priceTo, int categoryId, int? subCategoryId, int? brandId, string? search);
        Task<IEnumerable<Product>> GetTop10BestSellingAsync();
        Task<IEnumerable<Product>> GetTop10NewestAsync();
    }
}
