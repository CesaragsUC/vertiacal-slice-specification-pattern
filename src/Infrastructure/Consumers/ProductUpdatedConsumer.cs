using Application.Domain.Events.Products;
using Application.Product.Response;
using MassTransit;
using MongoDB.Driver;

namespace Infrastructure.Consumers;

public sealed class ProductUpdatedConsumer : IConsumer<ProductUpdatedEvent>
{
    private readonly IMongoCollection<ProductReadResponse> _col;
    public ProductUpdatedConsumer(IMongoDatabase db)
        => _col = db.GetCollection<ProductReadResponse>("products_read");

    public async Task Consume(ConsumeContext<ProductUpdatedEvent> context)
    {
        var e = context.Message;

        var sets = new List<UpdateDefinition<ProductReadResponse>>();
        var unsets = new List<UpdateDefinition<ProductReadResponse>>();

        sets.Add(Builders<ProductReadResponse>.Update.Set(x => x.Name, e.Name));
        sets.Add(Builders<ProductReadResponse>.Update.Set(x => x.Price, e.Price));
        sets.Add(Builders<ProductReadResponse>.Update.Set(x => x.IsActive, e.IsActive));
        sets.Add(Builders<ProductReadResponse>.Update.Set(x => x.Category, e.Category));
        sets.Add(Builders<ProductReadResponse>.Update.Set(x => x.CategoryId, e.CategoryId));

        if (sets.Count == 0 && unsets.Count == 0)
            return; // nada para atualizar

        var update = Builders<ProductReadResponse>.Update.Combine(sets.Concat(unsets));

        await _col.UpdateOneAsync(
            x => x.Id == e.Id,
            update,
            new UpdateOptions { IsUpsert = false }, // true se quiser criar quando não existir
            context.CancellationToken
        );
    }
}
