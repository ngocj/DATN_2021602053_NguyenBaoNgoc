using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Application.Dto.ImageDto
{
    public class ImageCreateDto
    {
        public int ProductVariantId { get; set; }
        public IFormFile formFile { get; set; }
        
    }
}
