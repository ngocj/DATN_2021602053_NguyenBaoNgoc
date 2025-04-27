using System.ComponentModel.DataAnnotations;

namespace SP.Application.Dto.UserDto
{
    public class UserCreateDto
    {
     
        public string UserName { get; set; }


        public string Email { get; set; }


        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 100 ký tự.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$",
            ErrorMessage = "Mật khẩu phải có ít nhất 1 chữ hoa, 1 chữ thường, 1 số và 1 ký tự đặc biệt.")]
        public string Password { get; set; }

        [RegularExpression(@"^\d{10}$", ErrorMessage = "Số điện thoại phải gồm đúng 10 chữ số.")]
        public string? PhoneNumber { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? AddressDetail { get; set; }
    }
}
