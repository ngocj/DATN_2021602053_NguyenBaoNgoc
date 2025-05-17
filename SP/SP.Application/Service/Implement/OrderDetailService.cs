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

        public async Task<IEnumerable<OrderDetail>> GetAllOrderDetails()
        {
            return await _unitOfWork.OrderDetailRepository.GetAllAsync();
        }

        public async Task<OrderDetail> GetOrderDetailById(Guid orderId, int productVariantId)
        {
            return await _unitOfWork.OrderDetailRepository.GetByCompositeKeyAsync(orderId, productVariantId);
        }

        public async Task CreateOrderDetail(OrderDetail orderDetail)
        {
            await _unitOfWork.OrderDetailRepository.AddAsync(orderDetail);
            await _unitOfWork.SaveChangeAsync();
        }

        public async Task UpdateOrderDetail(OrderDetail orderDetail)
        {
            var result = await _unitOfWork.OrderDetailRepository.GetByCompositeKeyAsync(orderDetail.OrderId, orderDetail.ProductVariantId);
            if (result != null)
            {
                await _unitOfWork.OrderDetailRepository.UpdateAsync(orderDetail);
                await _unitOfWork.SaveChangeAsync();
            }
        }

        public async Task DeleteOrderDetail(Guid orderId, int productVariantId)
        {
            var result = await _unitOfWork.OrderDetailRepository.GetByCompositeKeyAsync(orderId, productVariantId);
            if (result != null)
            {
                await _unitOfWork.OrderDetailRepository.DeleteAsync(result);
                await _unitOfWork.SaveChangeAsync();
            }
        }
    }

}
