using SP.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Application.Service.Interface
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task CreateProduct(Product product);
        Task UpdateProduct(Product product);
        Task DeleteProduct(int id);

        // sorting
        Task<IEnumerable<Product>> GetAllProductsBySubCategoryId(int subCategoryId);
        Task<IEnumerable<Product>> GetAllProductsByBrandId(int brandId);
        Task<IEnumerable<Product>> GetAllProductsByLastest();
        Task<IEnumerable<Product>> GetAllProductsByPriceDescending();
        Task<IEnumerable<Product>> GetAllProductsByPriceAscending();
        Task<IEnumerable<Product>> GetAllProductsByBestSelling();
        Task<IEnumerable<Product>> GetAllProductsByAZ();
        Task<IEnumerable<Product>> GetAllProductsByZA();
        Task<IEnumerable<Product>> GetAllProductsByOlder();

        // filter
        Task<IEnumerable<Product>> GetAllByCategoryAndBrandAsync(int? categoryId, int? brandId, bool? isActive);
    }
}
