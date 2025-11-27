using Application.Abstractions.Data;
using Application.Categories.Responses;
using Cortex.Mediator.Queries;
using ErrorOr;
using Microsoft.EntityFrameworkCore;



namespace Application.Categories.GetByName;

public sealed class GetCategoryByNameHandler : IQueryHandler<CategoryByNameQuery, ErrorOr<CategoryResponse>>
{
    private readonly IApplicationDbContext _db;
    public GetCategoryByNameHandler(IApplicationDbContext db) => _db = db;

    public async Task<ErrorOr<CategoryResponse>> Handle(CategoryByNameQuery req, CancellationToken ct)
    {

        var entity = await _db.Categories.FirstOrDefaultAsync(x=> x.Name!.Equals(req.Name),ct);
        if (entity is null)
        {
            return Error.NotFound(
                code: "Category.NotFound",
                description: $"Category with Name '{req.Name}' was not found");
        }

        return new CategoryResponse(entity.Id, entity.Name!, entity.IsActive);
    }
}
