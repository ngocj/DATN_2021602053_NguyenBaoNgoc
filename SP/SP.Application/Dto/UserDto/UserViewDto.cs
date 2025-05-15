using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Application.Dto.UserDto
{
    public class UserViewDto
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? PhoneNumber { get; set; }
        public DateOnly? DateOfBirth { get; set; }
        public string? AddressDetail { get; set; }
        public bool? IsActive { get; set; }
        public int RoleId { get; set; }
    }
}
