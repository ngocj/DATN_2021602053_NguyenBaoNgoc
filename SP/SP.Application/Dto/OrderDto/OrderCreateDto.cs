using SP.Application.Dto.OrderDetailDto;
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
        public Guid UserId { get; set; }
        public Guid EmployeeId { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? UserName { get; set; }
        public string? EmployeeName { get; set; }
        public string AddressDetail { get; set; }
        public List<OrderDetailViewDto> OrderDetails { get; set; }
    }
}
