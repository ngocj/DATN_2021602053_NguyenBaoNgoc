using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SP.Application.Dto.EmployeeDto;
using SP.Application.Service.Implement;
using SP.Application.Service.Interface;
using SP.Domain.Entity;
using SP.Infrastructure.Context;

namespace SP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IEmployeeService _employeeService;
        private readonly SPContext _context;

        public EmployeeController(IMapper mapper, IEmployeeService employeeService, SPContext context)
        {
            _mapper = mapper;
            _employeeService = employeeService;
            _context = context;
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
                // Kiểm tra email trùng
                if (await _context.Employees.AnyAsync(u => u.Email == employeeCreateDto.Email))
                {
                    return Conflict(new { field = "Email", message = "Email đã tồn tại." });
                }

                // Kiểm tra số điện thoại trùng
                if (await _context.Employees.AnyAsync(u => u.PhoneNumber == employeeCreateDto.PhoneNumber))
                {
                    return Conflict(new { field = "PhoneNumber", message = "Số điện thoại đã tồn tại." });
                }
                var employee = _mapper.Map<Employee>(employeeCreateDto);

                await _employeeService.CreateEmployee(employee);

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Có lỗi xảy ra khi tạo người dùng.", detail = ex.Message });
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
