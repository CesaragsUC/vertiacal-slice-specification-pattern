using Application.Categories.Responses;

namespace Application.Product.Response;

public sealed record ProductResponse(Guid Id, string Name, string Category, Guid? CategoryId, decimal Price);