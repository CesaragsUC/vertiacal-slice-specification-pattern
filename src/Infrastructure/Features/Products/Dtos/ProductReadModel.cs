using Application.Features.Categories.Dtos;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Features.Products.Dtos;

public sealed class ProductReadModel
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public bool IsActive { get; set; }

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid? CategoryId { get; set; }
    public CategoryReadModel? Category { get; set; }

    public ProductReadModel(Guid id, string name, decimal price, bool isActive, Guid? categoryId, CategoryDto? category)
    {
        Id = id;
        Name = name;
        Price = price;
        IsActive = isActive;
        CategoryId = categoryId;
        Category =  new CategoryReadModel
        {
            Id = category?.Id ?? Guid.Empty,
            Name = category?.Name ?? string.Empty,
            IsActive = category?.IsActive ?? false
        };
    }
}


public sealed class CategoryReadModel
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public bool IsActive { get; set; }
    public CategoryReadModel() { }
    public CategoryReadModel(Guid id, string name,bool isActive)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
    }
}