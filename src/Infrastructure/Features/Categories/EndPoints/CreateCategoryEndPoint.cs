using Application.Domain;
using Application.Infrastructure.Persistence.Contexts.ApplicationDbContext;
using Cortex.Mediator;
using Cortex.Mediator.Commands;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.Categories.EndPoints;

public class CreateCategoryController(IMediator mediator) : ApiControllerBase
{
    [HttpPost("/api/categories")]
    public async Task<IActionResult> Create([FromBody] CreateCategoryCommand command, CancellationToken ct)
    {
        var result = await mediator.SendCommandAsync<CreateCategoryCommand, ErrorOr<Guid>>(command,ct);
        return result.Match(
            id => Ok(id),
            errors => Problem(errors)
        );
    }
}

public sealed class CreateCategoryHandler : ICommandHandler<CreateCategoryCommand, ErrorOr<Guid>>
{
    private readonly AppDbContext _db;
    public CreateCategoryHandler(AppDbContext db) => _db = db;
    public async Task<ErrorOr<Guid>> Handle(CreateCategoryCommand req, CancellationToken ct)
    {
        var entity = new Category(req.Name);
        await _db.Categories.AddAsync(entity, ct);

        await _db.SaveChangesAsync(ct);
        return entity.Id;
    }
}

public sealed record CreateCategoryCommand(string Name) : ICommand<ErrorOr<Guid>>;


