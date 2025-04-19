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
    public class BrandService : IBrandService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BrandService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateBrand(Brand brand)
        {
            await _unitOfWork.BrandRepository.AddAsync(brand);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task DeleteBrand(int id)
        {
            var result = await _unitOfWork.CategoryRepository.GetByIdAsync(id);
            if (result != null)
            {
                await _unitOfWork.CategoryRepository.DeleteAsync(result);
                await _unitOfWork.SaveChangeAsync();
            }
        }

        public async Task<IEnumerable<Brand>> GetAllBrands()
        {
            return await _unitOfWork.BrandRepository.GetAllAsync();
        }
        public async Task<Brand> GetBrandById(int id)
        {
            return await _unitOfWork.BrandRepository.GetByIdAsync(id);
        }

        public async Task UpdateBrand(Brand brand)
        {
            var result = await _unitOfWork.BrandRepository.GetByIdAsync(brand.Id);
            if (result != null)
            {
                await _unitOfWork.BrandRepository.UpdateAsync(brand);
                await _unitOfWork.SaveChangeAsync();
            }
        }
    }
}
