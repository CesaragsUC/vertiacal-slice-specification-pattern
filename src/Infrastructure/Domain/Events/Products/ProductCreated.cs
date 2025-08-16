using Application.Features.Categories.Dtos;

namespace Application.Domain.Events.Products; 
public sealed record ProductCreated(Guid Id,string Name,decimal Price,bool isActive, Guid CategoryId, CategoryDto Category);
