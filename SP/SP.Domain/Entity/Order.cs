using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Domain.Entity
{
    public class Order : Base
    {
        public int UserId { get; set; }
        public int EmployeeId { get; set; }
        public string Status { get; set; }
        public decimal TotalPrice { get; set; }
        public User User { get; set; }
        public Employee Employee { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
       

    }
}
