using Microsoft.EntityFrameworkCore;

namespace Application.Specifications;

public static class SpecificationEvaluator
{
    public static IQueryable<T> Apply<T>(this IQueryable<T> input, ISpecification<T> spec) where T : class
    {
        var query = input;

        if (spec.Criteria is not null)
            query = query.Where(spec.Criteria);

        foreach (var include in spec.Includes)
            query = query.Include(include);

        if (spec.OrderBy is not null)
            query = query.OrderBy(spec.OrderBy);

        if (spec.OrderByDescending is not null)
            query = query.OrderByDescending(spec.OrderByDescending);

        if (spec.Skip.HasValue)
            query = query.Skip(spec.Skip.Value);

        if (spec.Take.HasValue)
            query = query.Take(spec.Take.Value);

        if (spec.AsNoTracking)
            query = query.AsNoTracking();

        return query;
    }
}