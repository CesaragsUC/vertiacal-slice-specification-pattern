using Application.Abstractions.Data;
using Cortex.Mediator.Commands;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace YourApp.Api.EndPoints.Categories;

public sealed class UpdateCategoryHandler : ICommandHandler<UpdateCategoryCommand, ErrorOr<Guid>>
{
    private readonly IApplicationDbContext _db;
    public UpdateCategoryHandler(IApplicationDbContext db) => _db = db;
    public async Task<ErrorOr<Guid>> Handle(UpdateCategoryCommand req, CancellationToken ct)
    {

        var category = await _db.Categories.FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (category is null)
        {
            return Error.NotFound("Category not found.");
        }
        category.Update(req.Name, req.isActive);
        await _db.SaveChangesAsync(ct);
        return category.Id;
    }
}

public sealed record UpdateCategoryCommand(Guid Id, string Name, bool isActive) : ICommand<ErrorOr<Guid>>;