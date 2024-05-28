using HotelApi.Domain.Repositories;
using HotelApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelApi.Infrastructure.Repositories
{
    internal class GenericRepository
    {
    }
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private bool disposed = false;
        private readonly DataContext _context;

        public GenericRepository(DataContext context)
        {
            _context = context;
        }

        protected DbSet<T> EntitySet
        {
            get
            {
                return _context.Set<T>();
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await EntitySet.ToListAsync();
        }

        public async Task<T> GetById(int? id)
        {
            return await EntitySet.FindAsync(id);
        }

       
        public async Task<T> Insert(T entity)
        {
            EntitySet.Add(entity);
            await Save();
            return entity;
        }

        public async Task<T> Delete(int id)
        {
            T entity = await EntitySet.FindAsync(id);
            EntitySet.Remove(entity);
            await Save();
            return entity;
        }

        public async Task Update(T entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await Save();
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed && disposing)
            {
                _context.Dispose();
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<T> Find(Expression<Func<T, bool>> expr)
        {
            return await EntitySet.AsNoTracking().FirstOrDefaultAsync(expr);
        }
    }

}
