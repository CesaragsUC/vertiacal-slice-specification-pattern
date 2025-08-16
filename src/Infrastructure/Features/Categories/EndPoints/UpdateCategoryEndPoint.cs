using Application.Infrastructure.Persistence.Contexts.ApplicationDbContext;
using Cortex.Mediator;
using Cortex.Mediator.Commands;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.Categories.EndPoints;

public class UpdateCategoryController(IMediator mediator) : ApiControllerBase
{
    [HttpPut("/api/categories")]
    public async Task<IActionResult> Update([FromBody] UpdateCategoryCommand command)
    {
        var result = await mediator.SendCommandAsync<UpdateCategoryCommand, ErrorOr<Guid>>(command);
        return result.Match(
            _ => NoContent(),
            errors => Problem(errors)
        );
    }
}

public sealed class UpdateCategoryHandler : ICommandHandler<UpdateCategoryCommand, ErrorOr<Guid>>
{
    private readonly AppDbContext _db;
    public UpdateCategoryHandler(AppDbContext db) => _db = db;
    public async Task<ErrorOr<Guid>> Handle(UpdateCategoryCommand req, CancellationToken ct)
    {
        var entity = await _db.Categories.FindAsync(req.Id, ct);
        if (entity is null)
        {
            return Error.NotFound("Category not found.");
        }
        entity.Update(req.Name, req.isActive);
        await _db.SaveChangesAsync(ct);
        return entity.Id;
    }
}

public sealed record UpdateCategoryCommand(Guid Id, string Name, bool isActive) : ICommand<ErrorOr<Guid>>;
