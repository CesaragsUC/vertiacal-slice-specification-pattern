using Cortex.Mediator.Notifications;

namespace Application.Product.Update;

public sealed record ProductUpdatedEventHandler(
    Guid Id,
    string Name,
    decimal Price,
    bool IsActive,
    Guid CategoryId,
    string Category
    ) : INotification;

public class ProductUpdatedNotificationHandler : INotificationHandler<ProductUpdatedEventHandler>
{
    public async Task Handle(ProductUpdatedEventHandler notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Product {notification.Name} updated at {DateTime.UtcNow}");

        await Task.CompletedTask;
    }
}