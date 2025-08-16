using Application.Features.Categories.Dtos;

namespace Application.Domain.Events.Products;

public sealed record ProductUpdated(Guid Id, string Name, decimal Price,bool IsActive, Guid CategoryId, CategoryDto Category);

