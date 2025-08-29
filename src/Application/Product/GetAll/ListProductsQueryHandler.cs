using Application.Product.Response;
using Application.Product.Specifications;
using Cortex.Mediator.Queries;
using ErrorOr;
using MongoDB.Driver;

namespace Application.Product.GetAll;

public sealed record GetProductsQuery(string? Category, int Page = 1, int Size = 20) : IQuery<ErrorOr<IEnumerable<ProductResponse>>>;

public class ListProductsQueryHandler : IQueryHandler<GetProductsQuery, ErrorOr<IEnumerable<ProductResponse>>>
{
    private readonly IMongoCollection<ProductReadResponse> _col;
    public ListProductsQueryHandler(IMongoDatabase db)
        => _col = db.GetCollection<ProductReadResponse>("products_read");

    public async Task<ErrorOr<IEnumerable<ProductResponse>>> Handle(GetProductsQuery req, CancellationToken ct = default)
    {
        var spec = new ProductsGetlAllReadSpec(req.Category!);

        var find = _col.Find(spec.Filter);
        if (spec.Sort is not null) find = find.Sort(spec.Sort);
        if (spec.Skip.HasValue) find = find.Skip(spec.Skip.Value);
        if (spec.Take.HasValue) find = find.Limit(spec.Take.Value);

        return await find.Project(x => new ProductResponse(x.Id, x.Name, x.Category, x.CategoryId, x.Price))
                         .ToListAsync(ct);
    }
}
