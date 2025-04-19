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
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task CreateCart(Cart cart)
        {
            await _unitOfWork.CartRepository.AddAsync(cart);
            await _unitOfWork.SaveChangeAsync();
            
        }

        public async Task DeleteCart(int id)
        {
            var result = await _unitOfWork.CartRepository.GetByIdAsync(id);
            if (result != null)
            {
               await _unitOfWork.CartRepository.DeleteAsync(result);
               await  _unitOfWork.SaveChangeAsync();
            }
    
        }

        public async Task<IEnumerable<Cart>> GetAllCarts()
        {
            return await _unitOfWork.CartRepository.GetAllAsync();        
        }

        public async Task<Cart> GetCartById(int id)
        {
            return await _unitOfWork.CartRepository.GetByIdAsync(id);
        }

        public async Task UpdateCart(Cart cart)
        {
            var result = await _unitOfWork.CartRepository.GetByIdAsync(cart.Id);
            if (result != null)
            {
               await _unitOfWork.CartRepository.UpdateAsync(cart);
               await _unitOfWork.SaveChangeAsync();
            }

           
        }
    }
}
