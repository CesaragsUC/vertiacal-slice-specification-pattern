using Application.Abstractions.Data;
using Application.Domain.Events.Products;
using Cortex.Mediator;
using Cortex.Mediator.Commands;
using ErrorOr;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using ProductEntity = Domain.Products.Product;


public class CreateProductHandler : ICommandHandler<CreateProductCommand, ErrorOr<Guid>>
{
    private readonly IApplicationDbContext _db;
    private readonly IPublishEndpoint _publish;
    private readonly IMediator _mediator;

    public CreateProductHandler(
        IApplicationDbContext db,
        IPublishEndpoint publish,
        IMediator mediator)
    {
        _db = db;
        _publish = publish;
        _mediator = mediator;
    }

    public async Task<ErrorOr<Guid>> Handle(CreateProductCommand req, CancellationToken ct)
    {
        var category = await _db.Categories.FirstOrDefaultAsync(x => x.Id == req.CategoryId, ct);

        if (category is null)
            return Error.NotFound(description: $"Category with Id {req.CategoryId} not found");

        if (IsNameAlreadyExists(req.Name))
            return Error.Conflict(description: $"Product with name {req.Name} already exists");

        var entity = new ProductEntity(req.Name, req.Price, req.isActive, req.CategoryId, category);
        await _db.Products.AddAsync(entity, ct);

        await MessageBus(entity, category.Name);
        await SendNotification(entity, category.Name);

        await _db.SaveChangesAsync(ct);

        return entity.Id;
    }

    private async Task MessageBus(ProductEntity entity, string category)
    {
        await _publish.Publish(new ProductCreatedEvent(entity.Id,
        entity.Name,
        entity.Price,
        entity.IsActive,
        entity.CategoryId,
        category));
    }

    private async Task SendNotification(ProductEntity entity, string category)
    {
        //Notification Event Cortex
        await _mediator.PublishAsync(new ProductCreatedEvent(
            entity.Id,
            entity.Name,
            entity.Price,
            entity.IsActive,
            entity.CategoryId,
            category));
    }

    private bool IsNameAlreadyExists(string name) 
        => _db.Products.Any(e => EF.Functions.ILike(e.Name, name));
}
