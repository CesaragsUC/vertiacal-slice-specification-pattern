using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Product.Response;

public sealed class ProductReadResponse
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public decimal Price { get; set; }
    public bool IsActive { get; set; }

    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid? CategoryId { get; set; }
    public string Category { get; set; }

    public ProductReadResponse(Guid id, string name, decimal price, bool isActive, Guid? categoryId, string category)
    {
        Id = id;
        Name = name;
        Price = price;
        IsActive = isActive;
        CategoryId = categoryId;
        Category = category;
    }
}
