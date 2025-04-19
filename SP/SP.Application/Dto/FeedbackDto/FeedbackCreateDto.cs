using SP.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Application.Dto.FeedbackDto
{
    public class FeedbackCreateDto
    {
        public int OrderId { get; set; }
        public int ProductVariantId { get; set; }
        public int UserId { get; set; }
        public string? Comment { get; set; }
        public int Rating { get; set; }

    }
}
