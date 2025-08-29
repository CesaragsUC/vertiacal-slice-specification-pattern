using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Application.Categories.Responses;

public sealed class CategoryReadResponse
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public bool IsActive { get; set; }
    public CategoryReadResponse() { }
    public CategoryReadResponse(Guid id, string name,bool isActive)
    {
        Id = id;
        Name = name;
        IsActive = isActive;
    }
}