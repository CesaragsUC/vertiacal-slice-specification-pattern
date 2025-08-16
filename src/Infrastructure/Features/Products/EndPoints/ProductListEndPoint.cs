
using Application;
using Application.Features.Products.Dtos;
using Application.Specifications.Product;
using Cortex.Mediator;
using Cortex.Mediator.Queries;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace YourApp.Api.Features.Products.ListByCategory;

public class ProductListController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("/api/products")]
    public async Task<IActionResult> Get([FromQuery] GetProductsQuery query)
    {
        var result = await mediator.SendQueryAsync<GetProductsQuery, ErrorOr<IEnumerable<ProductDto>>>(query);

        return result.Match(
            products => Ok(products),
            errors => Problem(errors)
        );
    }
}



public class ListProductsQueryHandler : IQueryHandler<GetProductsQuery, ErrorOr<IEnumerable<ProductDto>>>
{
    private readonly IMongoCollection<ProductReadModel> _col;
    public ListProductsQueryHandler(IMongoDatabase db)
        => _col = db.GetCollection<ProductReadModel>("products_read");

    public async Task<ErrorOr<IEnumerable<ProductDto>>> Handle(GetProductsQuery req, CancellationToken ct = default)
    {
        var spec = new ProductsByCategoryReadSpec(req.Category!);

        var find = _col.Find(spec.Filter);
        if (spec.Sort is not null) find = find.Sort(spec.Sort);
        if (spec.Skip.HasValue) find = find.Skip(spec.Skip.Value);
        if (spec.Take.HasValue) find = find.Limit(spec.Take.Value);

        return await find.Project(x => new ProductDto(x.Id, x.Name, x.Category, x.CategoryId, x.Price))
                         .ToListAsync(ct);
    }
}

public sealed record ProductDto(Guid Id, string Name, CategoryReadModel? Category, Guid? CategoryId, decimal Price);

public sealed record GetProductsQuery(string? Category, int Page = 1, int Size = 20) : IQuery<ErrorOr<IEnumerable<ProductDto>>>;