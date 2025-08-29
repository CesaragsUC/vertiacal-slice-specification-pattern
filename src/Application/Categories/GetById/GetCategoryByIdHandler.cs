using Application.Abstractions.Data;
using Application.Categories.Responses;
using Cortex.Mediator.Queries;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Application.Categories.GetById;

public sealed class GetCategoryByIdHandler : IQueryHandler<GetCategoryByIdQuery, ErrorOr<CategoryResponse>>
{
    private readonly IApplicationDbContext _db;
    public GetCategoryByIdHandler(IApplicationDbContext db) => _db = db;

    public async Task<ErrorOr<CategoryResponse>> Handle(GetCategoryByIdQuery req, CancellationToken ct)
    {
        var category = await _db.Categories.FirstOrDefaultAsync(x => x.Id == req.Id, ct);
        if (category is null)
        {
            return Error.NotFound(
                code: "Category.NotFound",
                description: $"Category with Id '{req.Id}' was not found");
        }
        return new CategoryResponse(category.Id, category.Name, category.IsActive);
    }
}
