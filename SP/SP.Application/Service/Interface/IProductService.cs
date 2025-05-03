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
        Task<IEnumerable<Product>> GetAllProductsBySubCategoryId(int subCategoryId);
        Task<IEnumerable<Product>> GetAllProductsByBrandId(int brandId);
<<<<<<< HEAD
        Task<IEnumerable<Product>> GetAllProductsByLastest();
        Task<IEnumerable<Product>> GetAllProductsByPriceDescending();
        Task<IEnumerable<Product>> GetAllProductsByPriceAscending();
        Task<IEnumerable<Product>> GetAllProductsByBestSelling();
        Task<IEnumerable<Product>> GetAllProductsByAZ();
        Task<IEnumerable<Product>> GetAllProductsByZA();
        Task<IEnumerable<Product>> GetAllProductsByOlder();
=======
>>>>>>> b7b1e4a6197011c53f2ec89c21cfc36d5a04e76a
    }
}
