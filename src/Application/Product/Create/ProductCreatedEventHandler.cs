using Application.Domain.Events.Products;
using Cortex.Mediator.Notifications;
using Microsoft.Extensions.Logging;

namespace Application.Product.Create;


public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
{
    private readonly ILogger<ProductCreatedEventHandler> _logger;
    public ProductCreatedEventHandler(ILogger<ProductCreatedEventHandler> loger) => _logger = loger;


    public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogWarning($"Product {notification.Name} created at {DateTime.UtcNow}");

        await Task.CompletedTask;
    }
}
