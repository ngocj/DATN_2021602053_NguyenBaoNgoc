using SP.Application.Dto.ProductVariantDto;
using SP.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Application.Dto.ProductDto
{
    public class ProductViewDto
    {
        public int Id { get; set; }
<<<<<<< HEAD
=======
        public int BrandId { get; set; }
        public int SubCategoryId { get; set; }
>>>>>>> b7b1e4a6197011c53f2ec89c21cfc36d5a04e76a
        public string ProductName { get; set; }
        public int? DiscountId { get; set; }
        public double? Rating { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public List<VariantViewDto> ProductVariants { get; set; } = new List<VariantViewDto>();
        public string  SubCategoryName { get; set; }
        public string BrandName { get; set; }
        public int Percent { get; set; }
    }
}
