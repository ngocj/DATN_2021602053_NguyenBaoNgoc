using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Application.Dto.EmployeeDto
{
    public class EmployeeUpdateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public int RoleId { get; set; }
    }
}
