using Cortex.Mediator.Notifications;

namespace Application.Product.Delete;

public sealed record ProductDeletedNotification(Guid Id) : INotification;

public class ProductDeleteNotificationHandler : INotificationHandler<ProductDeletedNotification>
{
    public async Task Handle(ProductDeletedNotification notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Product with Id {notification.Id} deleted at {DateTime.UtcNow}");

        await Task.CompletedTask;
    }
}
