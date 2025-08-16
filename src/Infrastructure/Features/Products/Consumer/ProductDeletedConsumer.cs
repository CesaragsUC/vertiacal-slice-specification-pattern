using Application.Domain.Events.Products;
using Application.Features.Products.Dtos;
using MassTransit;
using MongoDB.Driver;

namespace YourApp.Api.Features.Products.Projections;

public sealed class ProductDeletedConsumer : IConsumer<ProductDeleted>
{
    private readonly IMongoCollection<ProductReadModel> _col;
    public ProductDeletedConsumer(IMongoDatabase db)
        => _col = db.GetCollection<ProductReadModel>("products_read");

    public async Task Consume(ConsumeContext<ProductDeleted> context)
    {
        /*Mongo: procure um documento que bata com o filtro(x => x.Id == e.Id).
        Se existir ? faça um DELETE.
        Se não existir ? ignore.*/
        var e = context.Message;
        await _col.DeleteOneAsync(x => x.Id == e.Id, context.CancellationToken);
    }
}
