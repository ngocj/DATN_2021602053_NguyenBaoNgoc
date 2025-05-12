using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SP.Application.Dto.ProductVariantDto;
using SP.Application.Service.Interface;
using SP.Domain.Entity;

namespace SP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductVariantController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProductVariantService _productVariantService;
        public ProductVariantController(IMapper mapper, IProductVariantService productVariantService)
        {
            _mapper = mapper;
            _productVariantService = productVariantService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProductVariants()
        {
            var productVariants = await _productVariantService.GetAllProductVariants();
            var productVariantDto = _mapper.Map<IEnumerable<VariantViewDto>>(productVariants);
            return Ok(productVariantDto);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductVariantById(int id)
        {
            var productVariant = await _productVariantService.GetProductVariantById(id);
            if (productVariant == null)
            {
                return NotFound();
            }
            var productVariantDto = _mapper.Map<VariantViewDto>(productVariant);
            return Ok(productVariantDto);
        }
        [HttpPost]
        public async Task<IActionResult> CreateProductVariant( VariantCreateDto productVariantCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productVariant = _mapper.Map<ProductVariant>(productVariantCreateDto);
            await _productVariantService.CreateProductVariant(productVariant);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProductVariant( VariantViewDto productVariantViewDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var productVariant = await _productVariantService.GetProductVariantById(productVariantViewDto.Id);
            if (productVariant == null)
            {
                return NotFound();
            }
            var updatedProductVariant = _mapper.Map<ProductVariant>(productVariantViewDto);
            await _productVariantService.UpdateProductVariant(updatedProductVariant);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProductVariant(int id)
        {
            var productVariant = await _productVariantService.GetProductVariantById(id);
            if (productVariant == null)
            {
                return NotFound();
            }
            await _productVariantService.DeleteProductVariant(id);
            return Ok();
        }
        
    }
}
