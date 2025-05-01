using SP.Domain.Entity;
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
    }
}
