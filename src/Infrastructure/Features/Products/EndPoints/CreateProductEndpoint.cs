using Application;
using Application.Domain;
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

public class CreateProductController(IMediator mediator) : ApiControllerBase
{

    [HttpPost("/api/products")]
    public async Task<IActionResult> Create(CreateProductCommand command, CancellationToken ct)
    {
        var result = await mediator.SendCommandAsync<CreateProductCommand, ErrorOr<Guid>>(command, ct);

        return result.Match(id => Ok(id), Problem);
    }

}

public sealed record CreateProductCommand(string Name, decimal Price,bool isActive, Guid CategoryId) : ICommand<ErrorOr<Guid>>;

public class CreateProductHandler : ICommandHandler<CreateProductCommand, ErrorOr<Guid>>
{
    private readonly AppDbContext _db;
    private readonly IPublishEndpoint _publish;

    public CreateProductHandler(AppDbContext db, IPublishEndpoint publish)
    {
        _db = db;
        _publish = publish;
    }

    public async Task<ErrorOr<Guid>> Handle(CreateProductCommand req, CancellationToken ct)
    {
        var category = await _db.Categories.FirstOrDefaultAsync(x => x.Id == req.CategoryId);
        if (category is null)
            return Error.NotFound(description: $"Category with Id {req.CategoryId} not found");

        var entity = new Product(req.Name, req.Price, req.isActive, req.CategoryId, category);
        await _db.Products.AddAsync(entity, ct);

        //Previamanete configurado o Transactional Outbox no EF + Bus Outbox. Só envia para fila se o produto for criado com sucesso.
        await _publish.Publish(new ProductCreated(entity.Id, entity.Name, entity.Price, entity.IsActive, entity.CategoryId, category.ToDto()), ct);

        await _db.SaveChangesAsync(ct);

        return entity.Id;
    }
}

