using Microsoft.EntityFrameworkCore;
using SolucionPeakHours.Interfaces;
using SolucionPeakHours.Models;
using SolucionPeakHours.Persistence;
using System.Linq.Expressions;

namespace SolucionPeakHours.Repositories
{
    public class BaseCrudRepository<TEntity> : IBaseCrudRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly CndDbContext _context;

        public BaseCrudRepository(CndDbContext context)
        {
            _context = context;
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            var response = await _context
                .Set<TEntity>()
                .AddAsync(entity);

            await _context.SaveChangesAsync();

            return response.Entity;
        }

        public async Task<bool> DeleteAsync(TEntity entity)

        {
            _context
                .Set<TEntity>()
                .Remove(entity);

            await _context.SaveChangesAsync();

            return true;
        }

        public virtual async Task<List<TEntity>> GetAll()
        {
            var entities = await _context
                .Set<TEntity>()
                .ToListAsync();

            return entities;
        }

        public virtual async Task<TEntity> GetByIdAsync(int id)
        {
            var entity = await _context
                .Set<TEntity>()
                .FirstOrDefaultAsync(x => x.Id == id);

            return entity!;
        }

        public Task<IQueryable<TEntity>> Query()
        {
            var query = _context
                .Set<TEntity>()
                .AsQueryable();

            return Task.FromResult(query);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            _context.Set<TEntity>().Update(entity);

            await _context.SaveChangesAsync();

            return entity;
        }
    }
}
