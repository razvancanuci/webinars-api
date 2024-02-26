using System.Linq.Expressions;
using Domain.Entities;
using Domain.Specifications;

namespace Domain.Interfaces;

public interface IRepository<TEntity>
where TEntity: Entity
{
    Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> criteria, Func<IQueryable<TEntity>, IQueryable<TEntity>>? additionalQuery = null, bool asNoTracking = false);
    Task<IEnumerable<TEntity>> GetAsync(Specification<TEntity> specification);
    Task InsertAsync(TEntity entity);
    void Delete(TEntity entity);
    ValueTask<TEntity?> GetByIdAsync(string id);
}