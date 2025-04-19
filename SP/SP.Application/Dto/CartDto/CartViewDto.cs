using SP.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Application.Dto.CartDto
{
    public class CartViewDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public User User { get; set; }
        public ProductVariant ProductVariant { get; set; }
    }
}
