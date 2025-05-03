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
        Task<IEnumerable<Product>> GetAllBySubCategoryIdAsync(int subCategoryId);
        Task<IEnumerable<Product>> GetAllByBrandIdAsync(int brandId);
<<<<<<< HEAD
        // arrage product by lastest
         Task<IEnumerable<Product>> GetAllByLastestAsync();

        // arrange product by price descending
         Task<IEnumerable<Product>> GetAllByPriceDescendingAsync();

        // arrange product by price ascending
         Task<IEnumerable<Product>> GetAllByPriceAscendingAsync();

        // sort product by best selling products
         Task<IEnumerable<Product>> GetAllByBestSellingAsync();

        // sort product by a -> z
         Task<IEnumerable<Product>> GetAllByAZAsync();

        // sort product by z -> a
         Task<IEnumerable<Product>> GetAllByZAAsync();

        // sort product by older -> newer
         Task<IEnumerable<Product>> GetAllByOlderAsync();
   

=======
>>>>>>> b7b1e4a6197011c53f2ec89c21cfc36d5a04e76a
    }
}
