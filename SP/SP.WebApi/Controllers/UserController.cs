using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SP.Application.Dto.UserDto;
using SP.Application.Service.Interface;
using SP.Domain.Entity;

namespace SP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UserController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            var userDto = _mapper.Map<IEnumerable<UserViewDto>>(users);
            return Ok(userDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            var userDto = _mapper.Map<UserViewDto>(user);
            return Ok(userDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserCreateDto userCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = _mapper.Map<User>(userCreateDto);
                await _userService.CreateUser(user); 
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
        public async Task<IActionResult> UpdateUser([FromBody] UserViewDto userViewDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _userService.GetUserById(userViewDto.Id);
            if (user == null)
            {
                return NotFound();
            }
            var updatedUser = _mapper.Map<User>(userViewDto);
            await _userService.UpdateUser(updatedUser);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            await _userService.DeleteUser(id);
            return NoContent();
        }


    }
}
