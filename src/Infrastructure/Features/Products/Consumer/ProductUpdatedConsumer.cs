using Application.Domain.Events.Products;
using Application.Extensions;
using Application.Features.Categories.Dtos;
using Application.Features.Products.Dtos;
using MassTransit;
using MongoDB.Driver;

namespace YourApp.Api.Features.Products.Projections;

public sealed class ProductUpdatedConsumer : IConsumer<ProductUpdated>
{
    private readonly IMongoCollection<ProductReadModel> _col;
    public ProductUpdatedConsumer(IMongoDatabase db)
        => _col = db.GetCollection<ProductReadModel>("products_read");

    public async Task Consume(ConsumeContext<ProductUpdated> context)
    {
        var e = context.Message;

        var sets = new List<UpdateDefinition<ProductReadModel>>();
        var unsets = new List<UpdateDefinition<ProductReadModel>>();

        sets.Add(Builders<ProductReadModel>.Update.Set(x => x.Name, e.Name));
        sets.Add(Builders<ProductReadModel>.Update.Set(x => x.Price, e.Price));
        sets.Add(Builders<ProductReadModel>.Update.Set(x => x.IsActive, e.IsActive));
        sets.Add(Builders<ProductReadModel>.Update.Set(x => x.Category, e.Category.ToReadModel()));
        sets.Add(Builders<ProductReadModel>.Update.Set(x => x.CategoryId, e.CategoryId));

        if (sets.Count == 0 && unsets.Count == 0)
            return; // nada para atualizar

        var update = Builders<ProductReadModel>.Update.Combine(sets.Concat(unsets));

        await _col.UpdateOneAsync(
            x => x.Id == e.Id,
            update,
            new UpdateOptions { IsUpsert = false }, // true se quiser criar quando não existir
            context.CancellationToken
        );
    }
}
