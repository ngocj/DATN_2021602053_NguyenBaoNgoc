using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;
using SP.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SP.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly SPContext _SPContext;

        public GenericRepository(SPContext sPContext)
        {
            _SPContext = sPContext;
        }

        public async  Task AddAsync(T entity)
        {
           await _SPContext.Set<T>().AddAsync(entity);
        }

        public Task DeleteAsync(T entity)
        {
              _SPContext.Set<T>().Remove(entity);
             return Task.CompletedTask;

        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
           return await _SPContext.Set<T>().ToListAsync();
        }

        public virtual async Task<T> GetByIdAsync(int id)
        {
            return await _SPContext.Set<T>().FindAsync(id);
        }

        public Task UpdateAsync(T entity)
        {
            _SPContext.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }
    }
}
