using SP.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Application.Dto.FeedbackDto
{
    public class FeedbackViewDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductVariantId { get; set; }

        public int UserId { get; set; }

        public string? Comment { get; set; }
        public int Rating { get; set; }

        public User User { get; set; }

        public OrderDetail OrderDetail { get; set; }
    }
}
