namespace Application.Specifications;

public static class SpecOps
{
    public static ISpecification<T> And<T>(this ISpecification<T> a, ISpecification<T> b)
        => new CompositeSpec<T>(a, b, CompositeOp.And);

    public static ISpecification<T> Or<T>(this ISpecification<T> a, ISpecification<T> b)
        => new CompositeSpec<T>(a, b, CompositeOp.Or);

    public static ISpecification<T> Not<T>(this ISpecification<T> a)
        => new NotSpec<T>(a);
}

internal enum CompositeOp { And, Or }

internal sealed class CompositeSpec<T> : Specification<T>
{
    public CompositeSpec(ISpecification<T> a, ISpecification<T> b, CompositeOp op)
    {
        if (a.Criteria is null && b.Criteria is null)
        {
            Criteria = null;
        }
        else if (a.Criteria is not null && b.Criteria is not null)
        {
            Criteria = op == CompositeOp.And
                ? a.Criteria.And(b.Criteria)
                : a.Criteria.Or(b.Criteria);
        }
        else
        {
            Criteria = a.Criteria ?? b.Criteria;
        }

        Includes.AddRange(a.Includes);
        Includes.AddRange(b.Includes);
        OrderBy = a.OrderBy ?? b.OrderBy;
        OrderByDescending = a.OrderByDescending ?? b.OrderByDescending;
        Skip = a.Skip ?? b.Skip;
        Take = a.Take ?? b.Take;
        AsNoTracking = a.AsNoTracking && b.AsNoTracking;
    }
}

internal sealed class NotSpec<T> : Specification<T>
{
    public NotSpec(ISpecification<T> inner)
    {
        if (inner.Criteria is not null)
        {
            var param = System.Linq.Expressions.Expression.Parameter(typeof(T), "x");
            var body = System.Linq.Expressions.Expression.Not(
                new ParameterReplacer(inner.Criteria.Parameters[0], param).Visit(inner.Criteria.Body)!);
            Criteria = System.Linq.Expressions.Expression.Lambda<Func<T, bool>>(body, param);
        }

        Includes.AddRange(inner.Includes);
        OrderBy = inner.OrderBy;
        OrderByDescending = inner.OrderByDescending;
        Skip = inner.Skip;
        Take = inner.Take;
        AsNoTracking = inner.AsNoTracking;
    }

    private sealed class ParameterReplacer : System.Linq.Expressions.ExpressionVisitor
    {
        private readonly System.Linq.Expressions.ParameterExpression _source;
        private readonly System.Linq.Expressions.ParameterExpression _target;
        public ParameterReplacer(System.Linq.Expressions.ParameterExpression source, System.Linq.Expressions.ParameterExpression target)
        { _source = source; _target = target; }
        protected override System.Linq.Expressions.Expression VisitParameter(System.Linq.Expressions.ParameterExpression node)
            => node == _source ? _target : base.VisitParameter(node);
    }
}
