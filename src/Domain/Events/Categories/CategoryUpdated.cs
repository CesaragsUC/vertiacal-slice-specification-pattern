namespace Application.Domain.Events.Category;

public sealed record CategoryUpdated(Guid Id, string Name, bool IsActive);

