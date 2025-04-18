﻿using SP.Application.Service.Interface;
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

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _unitOfWork.ProductRepository.GetAllAsync();
            
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
    }
}
