using Application.Abstractions.Data;
using Cortex.Mediator.Commands;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Application.Categories.Delete;

public sealed class DeleteCategoryHandler : ICommandHandler<DeleteCategoryCommand, ErrorOr<Guid>>
{
    private readonly IApplicationDbContext _db;

    public DeleteCategoryHandler(IApplicationDbContext db) => _db = db;

    public async Task<ErrorOr<Guid>> Handle(DeleteCategoryCommand req, CancellationToken ct)
    {
        var category = await _db.Categories.FirstOrDefaultAsync(x => x.Id == req.Id, ct);
        if (category is null)
        {
            return Error.NotFound("Category not found.");
        }
         _db.Categories.Remove(category);
        await _db.SaveChangesAsync(ct);
        return category.Id;
    }
}


public sealed record DeleteCategoryCommand(Guid Id) : ICommand<ErrorOr<Guid>>;