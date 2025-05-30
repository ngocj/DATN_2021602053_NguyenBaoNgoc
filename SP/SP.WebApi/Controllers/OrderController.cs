using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SP.Application.Dto.OrderDto;
using SP.Application.Service.Interface;
using SP.Domain.Entity;
using SP.Infrastructure.Context;

namespace SP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;
        private readonly SPContext _sPContext;

        public OrderController(IMapper mapper, IOrderService orderService, SPContext sPContext)
        {
            _mapper = mapper;
            _orderService = orderService;
            _sPContext = sPContext;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrders();
            var orderDto = _mapper.Map<IEnumerable<OrderViewDto>>(orders);
            return Ok(orderDto);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(Guid id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            var orderDto = _mapper.Map<OrderViewDto>(order);
            return Ok(orderDto);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderCreateDto orderCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Tạo ID mới nếu chưa có
            if (orderCreateDto.Id == Guid.Empty)
            {
                orderCreateDto.Id = Guid.NewGuid();
            }

            // Tìm user
            var user = await _sPContext.Users.FirstOrDefaultAsync(u => u.Id == orderCreateDto.UserId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            // Cập nhật địa chỉ cho user (có thể mở rộng cập nhật thêm các trường nếu cần)
            user.WardId = orderCreateDto.WardId;
            _sPContext.Users.Update(user);

            // Map từ DTO sang Entity Order
            var order = _mapper.Map<Order>(orderCreateDto);
            await _sPContext.Orders.AddAsync(order);

            // Lưu thay đổi
            await _sPContext.SaveChangesAsync();

            return Ok();
        }


        [HttpPut]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderUpdateDto orderUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = await _orderService.GetOrderById(orderUpdateDto.Id);
            if (order == null)
            {
                return NotFound();
            }

            // Cập nhật các trường cho phép từ DTO sang entity hiện có
            _mapper.Map(orderUpdateDto, order);

            await _orderService.UpdateOrder(order);

            return Ok();
        }


        [HttpDelete("{id}")]    
        public async Task<IActionResult> DeleteOrder(Guid id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            await _orderService.DeleteOrder(id);
            return Ok();
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserOrders(Guid userId)
        {
            var orders = await _orderService.GetOrdersByUserIdAsync(userId);
           
            var orderDtos = _mapper.Map<List<OrderViewDto>>(orders);

            return Ok(orderDtos);
        }
    }
}
