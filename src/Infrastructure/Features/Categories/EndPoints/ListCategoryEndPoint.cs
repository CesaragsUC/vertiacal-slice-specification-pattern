using Application.Features.Products.Dtos;
using Application.Infrastructure.Persistence.Contexts.ApplicationDbContext;
using Cortex.Mediator;
using Cortex.Mediator.Queries;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Application.Features.Categories.EndPoints;

public class ListCategoryController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("/api/categories/all")]
    public async Task<IActionResult> Get()
    {
        var result = await mediator.SendQueryAsync<ListCategoryRequest, ErrorOr<List<CategoryReadModel>>>(new ListCategoryRequest());
        return result.Match(
            categories => Ok(categories),
            errors => Problem(errors)
        );
    }
}

public sealed class ListCategoryHandler : IQueryHandler<ListCategoryRequest, ErrorOr<List<CategoryReadModel>>>
{
    private readonly AppDbContext _db;
    public ListCategoryHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<List<CategoryReadModel>>> Handle(ListCategoryRequest req, CancellationToken ct)
    {
        var categories = await _db.Categories
            .Select(c => new CategoryReadModel(c.Id, c.Name, c.IsActive))
            .AsNoTracking()
            .ToListAsync(ct);
        return categories;
    }
}

public sealed record ListCategoryRequest : IQuery<ErrorOr<List<CategoryReadModel>>>;


