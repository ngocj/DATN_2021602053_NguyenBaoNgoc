using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SP.Application.Dto.OrderDto;
using SP.Application.Service.Interface;
using SP.Domain.Entity;

namespace SP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrderService _orderService;
        public OrderController(IMapper mapper, IOrderService orderService)
        {
            _mapper = mapper;
            _orderService = orderService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderService.GetAllOrders();
            var orderDto = _mapper.Map<IEnumerable<OrderViewDto>>(orders);
            return Ok(orderDto);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrderById(int id)
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
            var order = _mapper.Map<Order>(orderCreateDto);
            await _orderService.CreateOrder(order);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateOrder([FromBody] OrderViewDto orderViewDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var order = await _orderService.GetOrderById(orderViewDto.Id);
            if (order == null)
            {
                return NotFound();
            }
            var updatedOrder = _mapper.Map<Order>(orderViewDto);
            await _orderService.UpdateOrder(updatedOrder);
            return Ok();
        }
        [HttpDelete("{id}")]    
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }
            await _orderService.DeleteOrder(id);
            return Ok();
        }

    }
}
