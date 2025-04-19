using SP.Domain.Entity;
using SP.Infrastructure.Context;
using SP.Infrastructure.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Infrastructure.Repositories.Implement
{
    public class CartRepository : GenericRepository<Cart>, ICartRepository
    {
        public CartRepository(SPContext context) : base(context)
        {
        }
    }
    
}
