using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Domain.Entity
{
    public enum OrderStatus
    {
        Pending,      // Chờ xác nhận
        Confirmed,    // Đã xác nhận
        Shipping,     // Đang giao
        Delivered,    // Đã giao
        Canceled      // Đã huỷ
    }
    public class Order : Base
    {
        public int UserId { get; set; }
        public int EmployeeId { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
        public User User { get; set; }
        public Employee Employee { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
       

    }
}
