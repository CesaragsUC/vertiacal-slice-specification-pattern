using Application.Extensions;
using Application.Features.Categories.Dtos;
using Application.Infrastructure.Persistence.Contexts.ApplicationDbContext;
using Cortex.Mediator;
using Cortex.Mediator.Queries;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Categories.EndPoints;

public class GetCategoryByIdController(IMediator mediator) : ApiControllerBase
{
    [HttpGet("/api/categories/{id:guid}")]
    public async Task<IActionResult> Create([FromRoute] Guid id)
    {
        var result = await mediator.SendQueryAsync<GetCategoryByIdRequest, ErrorOr<CategoryDto>>(new GetCategoryByIdRequest(id));
        return result.Match(Ok, Problem);
     
    }
}

public sealed class GetCategoryByIdHandler : IQueryHandler<GetCategoryByIdRequest, ErrorOr<CategoryDto>>
{
    private readonly AppDbContext _db;
    public GetCategoryByIdHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<CategoryDto>> Handle(GetCategoryByIdRequest req, CancellationToken ct)
    {
        var entity =  await _db.Categories.FirstOrDefaultAsync(x=> x.Id == req.Id);
        if (entity is null)
        {
            return Error.NotFound(
                code: "Category.NotFound",
                description: $"Category with Id '{req.Id}' was not found");
        }
        return entity.ToDto();
    }
}

public sealed record GetCategoryByIdRequest(Guid Id) : IQuery<ErrorOr<CategoryDto>>;

