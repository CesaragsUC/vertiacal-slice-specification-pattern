using Application.Infrastructure.Persistence.Contexts.ApplicationDbContext;
using Cortex.Mediator;
using Cortex.Mediator.Commands;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;

namespace Application.Features.Categories.EndPoints;

public class DeleteCategoryController(IMediator mediator) : ApiControllerBase
{

    [HttpDelete("/api/categories/{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var result = await mediator.SendCommandAsync<DeleteCategoryCommand, ErrorOr<Guid>>(new DeleteCategoryCommand(id));

        return result.Match(
            _ => NoContent(),
            errors => Problem(errors)
        );
    }
}

public sealed record DeleteCategoryCommand(Guid Id) : ICommand<ErrorOr<Guid>>;

public sealed class DeleteCategoryHandler : ICommandHandler<DeleteCategoryCommand, ErrorOr<Guid>>
{
    private readonly AppDbContext _db;

    public DeleteCategoryHandler(AppDbContext db) => _db = db;

    public async Task<ErrorOr<Guid>> Handle(DeleteCategoryCommand req, CancellationToken ct)
    {
        var entity = await _db.Categories.FindAsync(req.Id, ct);
        if (entity is null)
        {
            return Error.NotFound("Category not found.");
        }
         _db.Categories.Remove(entity);
        await _db.SaveChangesAsync(ct);
        return entity.Id;
    }
}
