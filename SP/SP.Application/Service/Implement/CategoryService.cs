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
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateCategory(Category category)
        { 
            await _unitOfWork.CategoryRepository.AddAsync(category);
            await _unitOfWork.SaveChangeAsync();

        }

        public async Task DeleteCategory(int id)
        {
            
            var result = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (result != null)
            {
               await _unitOfWork.CategoryRepository.DeleteAsync(result);
               await _unitOfWork.SaveChangeAsync();
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            
            return await _unitOfWork.CategoryRepository.GetAllAsync();
        }

        public async Task<Category> GetCategoryById(int id)
        {
               
            return await _unitOfWork.CategoryRepository.GetByIdAsync(id);
        }

        public async Task UpdateCategory(Category category)
        {          
            var result = await _unitOfWork.CategoryRepository.GetByIdAsync(category.Id);
            if (result != null)
            {
                await _unitOfWork.CategoryRepository.UpdateAsync(category);
               await _unitOfWork.SaveChangeAsync();
            }
        }
    }
}
