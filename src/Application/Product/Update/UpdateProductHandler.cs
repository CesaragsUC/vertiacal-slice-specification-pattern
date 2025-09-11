using Application.Abstractions.Data;
using Application.Domain.Events.Products;
using Application.Product.Specifications;
using Cortex.Mediator;
using Cortex.Mediator.Commands;
using ErrorOr;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace Application.Product.Update;

public sealed record UpdateProductCommand(Guid Id, string Name, decimal Price, bool isActive, Guid CategoryId) : ICommand<ErrorOr<Guid>>;

public class UpdateProductHandler : ICommandHandler<UpdateProductCommand, ErrorOr<Guid>>
{
    private readonly IApplicationDbContext _db;
    private readonly IPublishEndpoint _publish;
    private readonly IMediator _mediator;

    public UpdateProductHandler(IApplicationDbContext db, IPublishEndpoint publish, IMediator mediator)
    {
        _db = db;
        _publish = publish;
        _mediator = mediator;
    }

    public async Task<ErrorOr<Guid>> Handle(UpdateProductCommand req, CancellationToken ct)
    {
        var category = await _db.Categories.FirstOrDefaultAsync(x => x.Id == req.CategoryId);
        if (category is null)
            return Error.NotFound(description: $"Category with Id {req.CategoryId} not found");

        if (IsNameAlreadyExists(req.Name))
            return Error.Conflict(description: $"Product with name {req.Name} already exists");

        var spec = new ProductByIdSpec(req.Id);
        var entity = await _db.Products.FirstOrDefaultAsync(x => x.Id == req.Id, ct);

        if (entity is null)
            return Error.NotFound(description: $"Product with Id {req.Id} not found");

        entity.Update(req.Name, req.Price, req.isActive, req.CategoryId);
        _db.Products.Update(entity);

        await _publish.Publish(new ProductUpdatedEvent(entity.Id, entity.Name, entity.Price, entity.IsActive, entity.CategoryId, category.Name), ct);


        await _db.SaveChangesAsync(ct);

        return entity.Id;
    }
    private bool IsNameAlreadyExists(string name)
    => _db.Products.Any(e => EF.Functions.ILike(e.Name, name));
}

