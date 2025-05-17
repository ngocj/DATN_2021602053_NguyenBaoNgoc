using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SP.Application.Dto.EmployeeDto;
using SP.Application.Dto.UserDto;
using SP.Application.Service.Implement;
using SP.Application.Service.Interface;
using SP.Domain.Entity;

namespace SP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;

        public EmployeeController(IMapper mapper, IEmployeeService employeeService)
        {
            _mapper = mapper;
            _employeeService = employeeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllEmployees()
        {
            var employees = await _employeeService.GetAllEmployees();
            var employeeDto = _mapper.Map<IEnumerable<EmployeeViewDto>>(employees);
            return Ok(employeeDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmployeeById(Guid id)
        {
            var employee = await _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            var employeeDto = _mapper.Map<EmployeeViewDto>(employee);
            return Ok(employeeDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmployee([FromBody] EmployeeCreateDto employeeCreateDto)
        {
          
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var employee = _mapper.Map<Employee>(employeeCreateDto);
                await _employeeService.CreateEmployee(employee);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Email"))
                {
                    return Conflict("Email đã tồn tại.");
                }
                else if (ex.Message.Contains("Phone number"))
                {
                    return StatusCode(403, "Số điện thoại đã tồn tại.");
                }
                return StatusCode(500, "Có lỗi xảy ra khi tạo người dùng.");
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeViewDto employeeViewDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var employee = await _employeeService.GetEmployeeById(employeeViewDto.Id);
            if (employee == null)
            {
                return NotFound();
            }
            var updatedEmployee = _mapper.Map<Employee>(employeeViewDto);
            await _employeeService.UpdateEmployee(updatedEmployee);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var employee = await _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                return NotFound();
            }
            await _employeeService.DeleteEmployee(id);
            return NoContent();
        }
    }
}
