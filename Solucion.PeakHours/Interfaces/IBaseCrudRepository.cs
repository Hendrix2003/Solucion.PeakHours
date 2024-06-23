

using SolucionPeakHours.Models;

namespace SolucionPeakHours.Interfaces
{
    public interface IBaseCrudRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IQueryable<TEntity>> Query();
        Task<List<TEntity>> GetAll();
        Task<TEntity> GetByIdAsync(int id);
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(TEntity entity);

    }
}
