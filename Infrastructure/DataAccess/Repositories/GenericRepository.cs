using System.Linq.Expressions;
using DataAccess.Specifications;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class GenericRepository<TEntity> : IRepository<TEntity>
where TEntity : Entity
{
    protected DbSet<TEntity> DbSet { get; set; }
    protected GenericRepository(WebinarContext context)
    {
        DbSet = context.Set<TEntity>();
    }
    
    public async ValueTask<TEntity?> GetByIdAsync(string id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetAsync(Specification<TEntity> specification)
    {
        return await SpecificationBuilder.Build(DbSet, specification).ToListAsync();
    }

    public async Task InsertAsync(TEntity entity)
    {
       await DbSet.AddAsync(entity);
    }

    public void Delete(TEntity entity)
    {
        DbSet.Remove(entity);
    }
}