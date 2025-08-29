using Application.Abstractions.Data;
using Application.Domain.Events.Products;
using Cortex.Mediator;
using Cortex.Mediator.Commands;
using ErrorOr;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Application.Product.Delete;

public sealed record DeleteProductCommand(Guid Id) : ICommand<ErrorOr<Guid>>;

public class DeleteProductHandler : ICommandHandler<DeleteProductCommand, ErrorOr<Guid>>
{
    private readonly IApplicationDbContext _db;
    private readonly IPublishEndpoint _publish;
    private readonly IMediator _mediator;

    public DeleteProductHandler(IApplicationDbContext db, IPublishEndpoint publish, IMediator mediator)
    {
        _db = db;
        _publish = publish;
        _mediator = mediator;
    }

    public async Task<ErrorOr<Guid>> Handle(DeleteProductCommand req, CancellationToken ct)
    {
        var entity = await _db.Products.FirstOrDefaultAsync(x=> x.Id == req.Id, ct);

        if (entity is null)
            return Error.NotFound(description: $"Product with Id {req.Id} not found");

        await _publish.Publish(new ProductDeletedEvent(entity.Id), ct);

        //Notification
        await _mediator.PublishAsync(new ProductDeletedNotification(entity.Id));

        _db.Products.Remove(entity);

        await _db.SaveChangesAsync(ct);

        return entity.Id;
    }
}

