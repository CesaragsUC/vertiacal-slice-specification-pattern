namespace Application.Domain.Events.Products;

public sealed record ProductUpdatedEvent(
    Guid Id,
    string Name,
    decimal Price,
    bool IsActive,
    Guid CategoryId,
    string Category);

