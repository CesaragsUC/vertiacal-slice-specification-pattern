using Application;
using Application.Domain.Events.Products;
using Application.Infrastructure.Persistence.Contexts.ApplicationDbContext;
using Cortex.Mediator;
using Cortex.Mediator.Commands;
using ErrorOr;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace YourApp.Api.Features.Products.Create;

public class DeleteProductController(IMediator mediator) : ApiControllerBase
{

    [HttpDelete("/api/products")]
    public async Task<IActionResult> Delete(DeleteProductCommand command, CancellationToken ct)
    {
        var result = await mediator.SendCommandAsync<DeleteProductCommand, ErrorOr<Guid>>(command, ct);

        return result.Match(id => Ok(id), Problem);
    }

}

public sealed record DeleteProductCommand(Guid Id) : ICommand<ErrorOr<Guid>>;

public class DeleteProductHandler : ICommandHandler<DeleteProductCommand, ErrorOr<Guid>>
{
    private readonly AppDbContext _db;
    private readonly IPublishEndpoint _publish;

    public DeleteProductHandler(AppDbContext db, IPublishEndpoint publish)
    {
        _db = db;
        _publish = publish;
    }

    public async Task<ErrorOr<Guid>> Handle(DeleteProductCommand req, CancellationToken ct)
    {
        var entity = await _db.Products.FirstOrDefaultAsync(x => x.Id == req.Id, ct);
        if (entity is null)
            return Error.NotFound(description: $"Product with Id {req.Id} not found");

        await _publish.Publish(new ProductDeleted(entity.Id), ct);

        _db.Products.Remove(entity);

        await _db.SaveChangesAsync(ct);

        return entity.Id;
    }
}

