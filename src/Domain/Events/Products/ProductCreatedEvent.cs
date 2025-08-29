using Cortex.Mediator.Notifications;

namespace Application.Domain.Events.Products;

public sealed record ProductCreatedEvent(
    Guid Id,
    string Name,
    decimal Price,
    bool isActive,
    Guid CategoryId,
    string Category) : INotification;
