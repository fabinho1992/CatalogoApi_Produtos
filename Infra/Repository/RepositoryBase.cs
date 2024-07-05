using Dominio.Interfaces.Generic;
using Infraestrutura.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CatalogoApi.Repository
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected readonly ApiDbContext _context;

        public RepositoryBase(ApiDbContext context)
        {
            _context = context;
        }

        public async Task<T> Create(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            //await _context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
            //await _context.SaveChangesAsync();
        }

        public async Task<T?> Get(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T> Update(T entity)
        {
            _context.Set<T>().Update(entity);
            //await _context.SaveChangesAsync();
            return entity;
        }
    }
}
