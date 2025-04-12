using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Domain.Entity
{
    public class Cart : Base
    {
        public int UserId { get; set; }
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
        public User User { get; set; }
        public ProductVariant ProductVariant { get; set; }

    }
}
