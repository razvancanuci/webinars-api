﻿using System.Linq.Expressions;
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
    
    public async Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> criteria, Func<IQueryable<TEntity>, IQueryable<TEntity>>? additionalQuery = null, bool asNoTracking = false)
    {
        var query = GetQuery(criteria, additionalQuery, asNoTracking);
        
        var result = await query.ToListAsync();
        
        return result;
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

    private IQueryable<TEntity> GetQuery(Expression<Func<TEntity, bool>> criteria, Func<IQueryable<TEntity>, IQueryable<TEntity>>? additionalQuery, bool asNoTracking)
    {
        var initialQuery = DbSet;
        var query = initialQuery.Where(criteria);

        if (additionalQuery is not null)
        {
            query = additionalQuery(query);
        }
        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return query;
    }
}