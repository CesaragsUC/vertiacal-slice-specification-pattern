using Application.Abstractions.Data;
using Cortex.Mediator.Commands;
using Domain.Categories;
using ErrorOr;

namespace Application.Categories.Create;

public sealed class CreateCategoryHandler : ICommandHandler<CreateCategoryCommand, ErrorOr<Guid>>
{
    private readonly IApplicationDbContext _db;
    public CreateCategoryHandler(IApplicationDbContext db) => _db = db;
    public async Task<ErrorOr<Guid>> Handle(CreateCategoryCommand req, CancellationToken ct)
    {
        var entity = new Category(req.Name);
        await _db.Categories.AddAsync(entity, ct);

        await _db.SaveChangesAsync(ct);
        return entity.Id;
    }
}


public sealed record CreateCategoryCommand(string Name) : ICommand<ErrorOr<Guid>>;