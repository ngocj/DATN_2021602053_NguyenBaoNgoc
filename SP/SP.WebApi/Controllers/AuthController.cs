using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SP.Application.Dto.LoginDto;
using SP.Domain.Entity;
using SP.Infrastructure.Context;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SP.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly SPContext _context;
        private readonly IMapper _mapper;
        public AuthController(IMapper mapper, IConfiguration config, SPContext context)
        {
            _mapper = mapper;
            _config = config;
            _context = context;
        }

    

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewDto loginViewDto)
        {
            if (loginViewDto == null || string.IsNullOrEmpty(loginViewDto.Email) || string.IsNullOrEmpty(loginViewDto.Password))
            {
                return BadRequest("Email and password cannot be blank.");
            }

            var user = _context.Users
                .FirstOrDefault(x => x.Email == loginViewDto.Email && x.Password == loginViewDto.Password);

            if (user == null)
            {
                return Unauthorized("Incorrect email or password.");
            }

            var roleName = _context.Roles
                .Where(x => x.Id == user.RoleId)
                .Select(x => x.RoleName)
                .FirstOrDefault();

            // Tạo token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, roleName)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _config["Jwt:Issuer"],
                Audience = _config["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            return Ok(tokenString);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerViewDto)
        {
            if (registerViewDto == null || string.IsNullOrEmpty(registerViewDto.Email) || string.IsNullOrEmpty(registerViewDto.Password))
            {
                return BadRequest("Email and password cannot be blank.");
            }

            var userExists = _context.Users.Any(x => x.Email == registerViewDto.Email);
            if (userExists)
            {
                return Conflict("Email already exists.");
            }
            var phoneExists = _context.Users.Any(x => x.PhoneNumber == registerViewDto.PhoneNumber);
            if (phoneExists)
            {
                return Conflict("Phone number already exists.");
            }
            var user = _mapper.Map<User>(registerViewDto);
            var passwordHasher = new PasswordHasher<User>();
            user.PasswordHash = passwordHasher.HashPassword(user, registerViewDto.Password);
            _context.Users.Add(user);
            
            return Ok("User registered successfully.");
        }
    }
}
