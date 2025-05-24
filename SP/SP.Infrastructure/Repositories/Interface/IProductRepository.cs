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
        // Lấy tất cả sản phẩm theo CategoryId (dựa trên các SubCategory)
        Task<IEnumerable<Product>> GetAllByCategoryIdAsync(int categoryId);

        Task<IEnumerable<Product>> GetAllBySubCategoryIdAsync(int subCategoryId);

        Task<IEnumerable<Product>> GetAllByBrandIdAsync(int brandId);
        // arrage product by lastest
        Task<IEnumerable<Product>> GetAllByLastestAsync(decimal? priceFrom, decimal? priceTo, int categoryId, int? subCategoryId, int? brandId);

        // arrange product by price descending
         Task<IEnumerable<Product>> GetAllByPriceDescendingAsync(decimal? priceFrom, decimal? priceTo, int categoryId, int? subCategoryId, int? brandId);

        // arrange product by price ascending
         Task<IEnumerable<Product>> GetAllByPriceAscendingAsync(decimal? priceFrom, decimal? priceTo, int categoryId, int? subCategoryId, int? brandId);

        // sort product by best selling products
         Task<IEnumerable<Product>> GetAllByBestSellingAsync(decimal? priceFrom, decimal? priceTo, int categoryId, int? subCategoryId, int? brandId);

        // filter product by category and brand
        Task<IEnumerable<Product>> GetAllByCategoryAndBrandAsync(int? SubcategoryId, int? brandId, bool? isActive);



        
   

    }
}
