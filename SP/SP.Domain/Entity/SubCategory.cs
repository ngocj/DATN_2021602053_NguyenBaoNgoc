using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Domain.Entity
{
    public class SubCategory : Base
    {
        public string Name { get; set; }
        public Category Category { get; set; }
        public int CategoryId { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
    
    }
}
