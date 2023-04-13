using System.Linq.Expressions;

namespace Drones.Core.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        ValueTask<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
        Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken);
        Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
        void Update(TEntity entity, CancellationToken cancellationToken);
        void UpdateRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken);
        void Remove(TEntity entity, CancellationToken cancellationToken);
        void RemoveRange(IEnumerable<TEntity> entitiesv, CancellationToken cancellationToken);
    }
}
