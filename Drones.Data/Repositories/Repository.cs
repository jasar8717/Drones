using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Drones.Data.Repositories
{
    public class Repository<TEntity> : Core.Repositories.IRepository<TEntity> where TEntity : class
    {
        protected readonly DbContext Context;

        public Repository(DbContext context)
        {
            this.Context = context;
        }
        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
        {
            await Context.Set<TEntity>().AddAsync(entity, cancellationToken);
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            await Context.Set<TEntity>().AddRangeAsync(entities, cancellationToken);
        }

        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return Context.Set<TEntity>().Where(predicate);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await Context.Set<TEntity>().ToListAsync(cancellationToken);
        }

        public ValueTask<TEntity> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            return Context.Set<TEntity>().FindAsync(id, cancellationToken);
        }

        public void Remove(TEntity entity, CancellationToken cancellationToken)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            Context.Set<TEntity>().RemoveRange(entities);
        }

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken)
        {
            return Context.Set<TEntity>().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public void Update(TEntity entity, CancellationToken cancellationToken)
        {
            Context.Entry<TEntity>(entity).State = EntityState.Modified;
        }

        public void UpdateRange(IEnumerable<TEntity> entities, CancellationToken cancellationToken)
        {
            foreach (var entity in entities)
            {
                Context.Entry<TEntity>(entity).State = EntityState.Modified;
            }

        }

    }
}
