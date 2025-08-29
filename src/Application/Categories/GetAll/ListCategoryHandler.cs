using Application.Abstractions.Data;
using Application.Categories.Responses;
using Cortex.Mediator.Queries;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Application.Categories.GetAll;


public sealed class ListCategoryHandler : IQueryHandler<GetCategoryListQuery, ErrorOr<List<CategoryResponse>>>
{
    private readonly IApplicationDbContext _db;
    public ListCategoryHandler(IApplicationDbContext db) => _db = db;

    public async Task<ErrorOr<List<CategoryResponse>>> Handle(GetCategoryListQuery req, CancellationToken ct)
    {
        var categories = await _db.Categories
            .Select(c => new CategoryResponse(c.Id, c.Name, c.IsActive))
            .AsNoTracking()
            .ToListAsync(ct);
        return categories;
    }
}
