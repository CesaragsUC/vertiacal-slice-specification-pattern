using Application.Extensions;
using Application.Features.Categories.Dtos;
using Application.Infrastructure.Persistence.Contexts.ApplicationDbContext;
using Cortex.Mediator;
using Cortex.Mediator.Queries;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories.EndPoints;

public class GetCategoryByNameController(IMediator mediator) : ApiControllerBase
{

    [HttpGet("/api/categories/by-name/{name}")]
    public async Task<IActionResult> Get([FromRoute] string name)
    {
        var result = await mediator.SendQueryAsync<GetCategoryByNameRequest, ErrorOr<CategoryDto>>(new GetCategoryByNameRequest(name));
        return result.Match(Ok, Problem);
 
    }
}

public sealed class GetCategoryByNameHandler : IQueryHandler<GetCategoryByNameRequest, ErrorOr<CategoryDto>>
{
    private readonly AppDbContext _db;
    public GetCategoryByNameHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<CategoryDto>> Handle(GetCategoryByNameRequest req, CancellationToken ct)
    {
        var entity = await _db.Categories.FirstOrDefaultAsync(x => x.Name.Equals(req.Name));
        if (entity is null)
        {
            return Error.NotFound(
                code: "Category.NotFound",
                description: $"Category with Name '{req.Name}' was not found");
        }
        return entity.ToDto();
    }
}

public sealed record GetCategoryByNameRequest(string Name) : IQuery<ErrorOr<CategoryDto>>;

