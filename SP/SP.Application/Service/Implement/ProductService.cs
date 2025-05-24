using SP.Application.Service.Interface;
using SP.Domain.Entity;
using SP.Infrastructure.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Application.Service.Implement
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task CreateProduct(Product product)
        {
            await _unitOfWork.ProductRepository.AddAsync(product);
            await _unitOfWork.SaveChangeAsync();
        }
        public async Task DeleteProduct(int id)
        {
            var result = await _unitOfWork.ProductRepository.GetByIdAsync(id);
            if (result != null)
            {
                await _unitOfWork.ProductRepository.DeleteAsync(result);
                await _unitOfWork.SaveChangeAsync();
            }           
        }
        public async Task<Product> GetProductById(int id)
        {
            return await _unitOfWork.ProductRepository.GetByIdAsync(id);
        }
        public async Task UpdateProduct(Product product)
        {
            var result = await _unitOfWork.ProductRepository.GetByIdAsync(product.Id);
            if (result != null)
            {
                await _unitOfWork.ProductRepository.UpdateAsync(product);
                await _unitOfWork.SaveChangeAsync();
            }

        }
        
        // get all product
        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _unitOfWork.ProductRepository.GetAllAsync();          
        }     
        public async Task<IEnumerable<Product>> GetAllProductsByBestSelling(decimal? priceFrom, decimal? priceTo, int categoryId, int? subCategoryId, int? brandId)
        { 
            return await _unitOfWork.ProductRepository.GetAllByBestSellingAsync(priceFrom, priceTo, categoryId, subCategoryId, brandId);                  
        }
        public async Task<IEnumerable<Product>> GetAllByLastestAsync(decimal? priceFrom, decimal? priceTo, int categoryId, int? subCategoryId, int? brandId)
        {
            return await _unitOfWork.ProductRepository.GetAllByLastestAsync( priceFrom, priceTo, categoryId, subCategoryId, brandId);        
        }
        public async Task<IEnumerable<Product>> GetAllProductsByPriceAscending(decimal? priceFrom, decimal? priceTo, int categoryId, int? subCategoryId, int? brandId)
        {
            return await _unitOfWork.ProductRepository.GetAllByPriceAscendingAsync(priceFrom, priceTo, categoryId, subCategoryId, brandId);                           
        }
        public async Task<IEnumerable<Product>> GetAllProductsByPriceDescending(decimal? priceFrom, decimal? priceTo, int categoryId, int? subCategoryId, int? brandId)
        {
            return await _unitOfWork.ProductRepository.GetAllByPriceDescendingAsync(priceFrom, priceTo, categoryId, subCategoryId, brandId);                 
        }
        public async Task<IEnumerable<Product>> GetAllProductsByBrandId(int brandId)
        {
            return await _unitOfWork.ProductRepository.GetAllByBrandIdAsync(brandId);
        }
        public async Task<IEnumerable<Product>> GetAllProductsBySubCategoryId(int subCategoryId)
        {
            return await _unitOfWork.ProductRepository.GetAllBySubCategoryIdAsync(subCategoryId);
            
        }
        public async Task<IEnumerable<Product>> GetAllByCategoryAndBrandAsync(int? SubcategoryId, int? brandId, bool? isActive)
        {
            return await _unitOfWork.ProductRepository.GetAllByCategoryAndBrandAsync(SubcategoryId, brandId, isActive);
        }
        public Task<IEnumerable<Product>> GetAllByCategoryIdAsync(int categoryId)
        {
            return _unitOfWork.ProductRepository.GetAllByCategoryIdAsync(categoryId);            
        }      
     
    }
}
