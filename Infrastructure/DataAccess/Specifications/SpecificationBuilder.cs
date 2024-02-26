using Domain.Entities;
using Domain.Extensions;
using Domain.Specifications;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Specifications;

public class SpecificationBuilder
{
    public static IQueryable<T> Build<T>(IQueryable<T> inputQuery, Specification<T> specification)
    where T:Entity
    {
        var query = inputQuery;

        if (specification.Criteria is not null)
        {
            query = query.Where(specification.Criteria);
        }
        
        if (specification.AsNoTracking)
        {
            query = query.AsNoTracking();
        }

        if (specification.OrderByAscending is not null)
        {
            query = query.OrderBy(specification.OrderByAscending);
        }

        if (specification.Pagination is not null)
        {
            query = query.PageBy(specification.Pagination);
        }
        
        return query;
    }
}