using System.Linq.Expressions;
using Domain.Entities;
using Domain.Interfaces.Requests;

namespace Domain.Specifications;

public abstract class Specification<T>
where T: Entity
{
    public Expression<Func<T, bool>>? Criteria { get; set; }
    
    public IPaginatedRequest? Pagination { get; set; }
    
    public Expression<Func<T, object>>? OrderByAscending { get; set; }
    
    public Expression<Func<T,T>>? Select { get; set; }

    public bool AsNoTracking { get; set; }

    protected Specification() { }
    
    protected Specification(Expression<Func<T, bool>> criteria,
        Expression<Func<T, object>> orderByAscending,
        IPaginatedRequest pagination,
        Expression<Func<T,T>> select,
        bool asNoTracking)
    {
        Criteria = criteria;
        OrderByAscending = orderByAscending;
        Pagination = pagination;
        Select = select;
        AsNoTracking = asNoTracking;
    }
}