using SP.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Application.Dto.OrderDetailDto
{
    public class OrderDetailCreateDto
    {
        public int OrderId { get; set; }
        public int ProductVariantId { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
