using Application.Metrics;
using Application.Product.Response;
using Application.Product.Specifications;
using Cortex.Mediator.Queries;
using ErrorOr;
using MongoDB.Driver;
using Prometheus;

namespace Application.Product.GetById;

public sealed record ProductByIdQuery(Guid Id) : IQuery<ErrorOr<ProductResponse>>;

public class ProductByIdQueryHandler : IQueryHandler<ProductByIdQuery, ErrorOr<ProductResponse>>
{
    private readonly IMongoCollection<ProductReadResponse> _col;
    public ProductByIdQueryHandler(IMongoDatabase db)
        => _col = db.GetCollection<ProductReadResponse>("products_read");

    public async Task<ErrorOr<ProductResponse>> Handle(ProductByIdQuery req, CancellationToken ct = default)
    {
        using (GlobalMetrics.DbQueryDuration.WithLabels("get_product_by_id_query").NewTimer())
        {
            var spec = new ProductByIdSpec(req.Id!);

            var find = _col.Find(spec.Filter);
            if (spec.Sort is not null) find = find.Sort(spec.Sort);
            if (spec.Skip.HasValue) find = find.Skip(spec.Skip.Value);
            if (spec.Take.HasValue) find = find.Limit(spec.Take.Value);

            var projection = find.Project(x => new ProductResponse(x.Id, x.Name, x.Category, x.CategoryId, x.Price));
            var result = await projection.FirstOrDefaultAsync(ct);

            return result is not null
                ? result
                : Error.NotFound(description: $"Product with Id {req.Id} not found");
        }

    }
}
