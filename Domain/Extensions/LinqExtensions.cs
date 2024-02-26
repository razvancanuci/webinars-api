using Domain.Interfaces.Requests;

namespace Domain.Extensions;

public static class LinqExtensions
{
    public static IQueryable<T> PageBy<T>(this IQueryable<T> query, IPaginatedRequest request)
    {
        return query.Skip((request.Page - 1) * request.ItemsPerPage)
            .Take(request.ItemsPerPage);
    }
}