using SP.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Application.Dto.ProductDto
{
    public class ProductCreateDto
    {       
        public int BrandId { get; set; }
        public int SubCategoryId { get; set; }
        public string ProductName { get; set; }
        public int? DiscountId { get; set; }
        public int? Rating { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
/*        public Brand Brand { get; set; }
        public Category Category { get; set; }
        public Discount Discount { get; set; }
        public List<ProductVariant> ProductVariants { get; set; }*/

    }
}
