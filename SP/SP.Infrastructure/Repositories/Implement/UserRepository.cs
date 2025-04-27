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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(SPContext context) : base(context)
        {

        }
        public override Task AddAsync(User entity)
        {
            // check email exist in the database
            var existingUser = _SPContext.Users.FirstOrDefault(u => u.Email == entity.Email);
            if (existingUser != null)
            {
                throw new Exception("Email already exists");
            }
            // check phone number exist in the database
            var existingPhone = _SPContext.Users.FirstOrDefault(u => u.PhoneNumber == entity.PhoneNumber);
            if (existingPhone != null)
            {
                throw new Exception("Phone number already exists");
            }        

            return base.AddAsync(entity);
        }
    }
  
    
}
