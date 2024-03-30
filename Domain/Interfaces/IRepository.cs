using Domain.Entities;
using Domain.Specifications;

namespace Domain.Interfaces;

public interface IRepository<TEntity>
where TEntity: Entity
{
    Task<IEnumerable<TEntity>> GetAsync(Specification<TEntity> specification, CancellationToken cancellationToken = default);
    Task InsertAsync(TEntity entity);
    void Delete(TEntity entity);
    ValueTask<TEntity?> GetByIdAsync(string id);
    Task<int> CountAsync(CancellationToken cancellationToken = default);
}