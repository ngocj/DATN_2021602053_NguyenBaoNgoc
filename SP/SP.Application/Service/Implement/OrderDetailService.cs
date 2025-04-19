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
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderDetailService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task CreateOrderDetail(OrderDetail orderDetail)
        {
            await _unitOfWork.OrderDetailRepository.AddAsync(orderDetail);
            await _unitOfWork.SaveChangeAsync();

            
        }

        public async Task DeleteOrderDetail(int id)
        {
            var result = await _unitOfWork.OrderDetailRepository.GetByIdAsync(id);
            if (result != null)
            {
                await _unitOfWork.OrderDetailRepository.DeleteAsync(result);
                await _unitOfWork.SaveChangeAsync();
            }
            
        }

        public async Task<IEnumerable<OrderDetail>> GetAllOrderDetails()
        {
            return await  _unitOfWork.OrderDetailRepository.GetAllAsync();
            
        }

        public async Task<OrderDetail> GetOrderDetailById(int id)
        {
            return await _unitOfWork.OrderDetailRepository.GetByIdAsync(id);
            
        }

        public async Task UpdateOrderDetail(OrderDetail orderDetail)
        {
            var result = await _unitOfWork.OrderDetailRepository.GetByIdAsync(orderDetail.OrderId);
            if (result != null)
            {
                await _unitOfWork.OrderDetailRepository.UpdateAsync(orderDetail);
                await _unitOfWork.SaveChangeAsync();
            }
            
        }
    }
}
