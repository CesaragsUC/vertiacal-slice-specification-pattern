using Application.Abstractions.Data;
using Application.Domain.Events.Products;
using Application.Metrics;
using Cortex.Mediator;
using Cortex.Mediator.Commands;
using Domain.Categories;
using ErrorOr;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Prometheus;
using Serilog;

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
        using (ProductMetrics.ProcessDuration.NewTimer())
        {
            var entity = await _db.Products.FirstOrDefaultAsync(x => x.Id == req.Id, ct);

            if (entity is null)
            {
                ProductMetrics.ValidationErrors.WithLabels("product_deleted", "product_not_found").Inc();
                return Error.NotFound(description: $"Product with Id {req.Id} not found");
            }


            await _publish.Publish(new ProductDeletedEvent(entity.Id), ct);

            //Notification
            await _mediator.PublishAsync(new ProductDeletedNotification(entity.Id));

            _db.Products.Remove(entity);

            await _db.SaveChangesAsync(ct);

            ProductMetrics.ProductsDeleted.WithLabels("product_deleted_successfully").Inc();

            Log.Information("Product deleted with Id: {Id}", entity.Id);

            return entity.Id;

        }

    }
}

