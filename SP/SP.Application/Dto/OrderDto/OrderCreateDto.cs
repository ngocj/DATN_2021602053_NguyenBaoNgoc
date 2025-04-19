using SP.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Application.Dto.OrderDto
{
    public class OrderCreateDto
    {
        public int UserId { get; set; }
        public int EmployeeId { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
