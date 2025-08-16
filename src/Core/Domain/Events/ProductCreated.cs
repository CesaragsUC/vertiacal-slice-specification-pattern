namespace Core.Domain.Events; 
public sealed record ProductCreated(Guid Id,string Name,string Category,decimal Price);