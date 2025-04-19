using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SP.Application.Dto.ProvinceDto;
using SP.Application.Service.Interface;

namespace SP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProvinceService _provinceService;

        public AddressController(IMapper mapper, IProvinceService provinceService)
        {
            _mapper = mapper;
            _provinceService = provinceService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllProvinces()
        {
            var provinces = await _provinceService.GetAllProvinces();
            var provinceDto = _mapper.Map<IEnumerable<ProvinceViewDto>>(provinces);
            return Ok(provinceDto);
        }
        [HttpGet("id")]
        public async Task<IActionResult> GetProvinceById(int id)
        {
            var province = await _provinceService.GetAllProvinceById(id);
            var provinceDto = _mapper.Map<ProvinceViewDto>(province);
            if (province == null)
            {
                return NotFound();
            }
            return Ok(provinceDto);
        }

    }
}
