using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SP.Application.Dto.OrderDetailDto;
using SP.Application.Service.Interface;
using SP.Domain.Entity;

namespace SP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDetailController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrderDetailService _orderDetailService;

        public OrderDetailController(IMapper mapper, IOrderDetailService orderDetailService)
        {
            _mapper = mapper;
            _orderDetailService = orderDetailService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllOrderDetails()
        {
            var orderDetails = await _orderDetailService.GetAllOrderDetails();
            var orderDetailDto = _mapper.Map<IEnumerable<OrderDetailViewDto>>(orderDetails);
            return Ok(orderDetailDto);
        }
        [HttpGet("{orderId}/{productVariantId}")]
        public async Task<IActionResult> GetOrderDetailById(Guid orderId, int productVariantId)
        {
            var orderDetail = await _orderDetailService.GetOrderDetailById(orderId, productVariantId);
            if (orderDetail == null)
            {
                return NotFound();
            }
            var orderDetailDto = _mapper.Map<OrderDetailViewDto>(orderDetail);
            return Ok(orderDetailDto);
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrderDetail([FromBody] OrderDetailCreateDto orderDetailCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var orderDetail = _mapper.Map<OrderDetail>(orderDetailCreateDto);
            await _orderDetailService.CreateOrderDetail(orderDetail);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateOrderDetail([FromBody] OrderDetailViewDto orderDetailViewDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var orderDetail = _mapper.Map<OrderDetail>(orderDetailViewDto);
            await _orderDetailService.UpdateOrderDetail(orderDetail);
            return Ok();
        }
        [HttpDelete("{orderId}/{productVariantId}")]
        public async Task<IActionResult> DeleteOrderDetail(Guid orderId, int productVariantId)
        {
            var orderDetail = await _orderDetailService.GetOrderDetailById(orderId, productVariantId);
            if (orderDetail == null)
            {
                return NotFound();
            }
            await _orderDetailService.DeleteOrderDetail(orderId, productVariantId);
            return Ok();
        }


    }
}
