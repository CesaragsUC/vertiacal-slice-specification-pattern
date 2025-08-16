using Application;
using Application.Domain.Events.Products;
using Application.Extensions;
using Application.Infrastructure.Persistence.Contexts.ApplicationDbContext;
using Cortex.Mediator;
using Cortex.Mediator.Commands;
using ErrorOr;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace YourApp.Api.Features.Products.Create;

public class UpdateProductController(IMediator mediator) : ApiControllerBase
{

    [HttpPut("/api/products")]
    public async Task<IActionResult> Update(UpdateProductCommand command)
    {
        var result = await mediator.SendCommandAsync<UpdateProductCommand, ErrorOr<Guid>>(command);

        return result.Match(id => Ok(id), Problem);
    }

}

public sealed record UpdateProductCommand(Guid Id, string Name,  decimal Price,bool isActive, Guid CategoryId) : ICommand<ErrorOr<Guid>>;

public class UpdateProductHandler : ICommandHandler<UpdateProductCommand, ErrorOr<Guid>>
{
    private readonly AppDbContext _db;
    private readonly IPublishEndpoint _publish;

    public UpdateProductHandler(AppDbContext db, IPublishEndpoint publish)
    {
        _db = db;
        _publish = publish;
    }

    public async Task<ErrorOr<Guid>> Handle(UpdateProductCommand req, CancellationToken ct)
    {
        var category = await _db.Categories.FirstOrDefaultAsync(x => x.Id == req.CategoryId);
        if (category is null)
            return Error.NotFound(description: $"Category with Id {req.CategoryId} not found");

        var entity = await _db.Products.FirstOrDefaultAsync(x => x.Id == req.Id, ct);
        if (entity is null)
            return Error.NotFound(description: $"Product with Id {req.Id} not found");

        entity.Update(req.Name, req.Price, req.isActive, req.CategoryId);
        _db.Products.Update(entity);

        await _publish.Publish(new ProductUpdated(entity.Id, entity.Name, entity.Price, entity.IsActive, entity.CategoryId, category.ToDto()), ct);

        await _db.SaveChangesAsync(ct);

        return entity.Id;
    }
}

