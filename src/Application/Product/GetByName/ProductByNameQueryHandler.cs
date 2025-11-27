using Application.Metrics;
using Application.Product.Response;
using Application.Product.Specifications;
using Cortex.Mediator.Queries;
using ErrorOr;
using MongoDB.Driver;
using Prometheus;

namespace Application.Product.GetByName;

public sealed record ProductByNameQuery(string Name, int Page = 1, int Size = 20) : IQuery<ErrorOr<IEnumerable<ProductResponse>>>;

public class ProductByNameQueryHandler : IQueryHandler<ProductByNameQuery, ErrorOr<IEnumerable<ProductResponse>>>
{
    private readonly IMongoCollection<ProductReadResponse> _col;
    public ProductByNameQueryHandler(IMongoDatabase db)
        => _col = db.GetCollection<ProductReadResponse>("products_read");

    public async Task<ErrorOr<IEnumerable<ProductResponse>>> Handle(ProductByNameQuery req, CancellationToken ct = default)
    {
        using (GlobalMetrics.DbQueryDuration.WithLabels("get_product_by_name_query").NewTimer())
        {

            var spec = new ProductByNameSpec(req.Name!);

            var find = _col.Find(spec.Filter);
            if (spec.Sort is not null) find = find.Sort(spec.Sort);
            if (spec.Skip.HasValue) find = find.Skip(spec.Skip.Value);
            if (spec.Take.HasValue) find = find.Limit(spec.Take.Value);

            return await find.Project(x => new ProductResponse(x.Id, x.Name, x.Category, x.CategoryId, x.Price))
                             .ToListAsync(ct);
        }

    }
}
