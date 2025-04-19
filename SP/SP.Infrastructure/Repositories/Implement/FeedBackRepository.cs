using SP.Domain.Entity;
using SP.Infrastructure.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Infrastructure.Repositories.Implement
{
    public class FeedBackRepository : GenericRepository<FeedBack>,IFeedbackRepository
    {
        public FeedBackRepository(Context.SPContext context) : base(context)
        {
        }
    }
    
}
