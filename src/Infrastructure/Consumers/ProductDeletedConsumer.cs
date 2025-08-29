using Application.Domain.Events.Products;
using Application.Product.Response;
using MassTransit;
using MongoDB.Driver;

namespace Infrastructure.Consumers;

public sealed class ProductDeletedConsumer : IConsumer<ProductDeletedEvent>
{
    private readonly IMongoCollection<ProductReadResponse> _col;
    public ProductDeletedConsumer(IMongoDatabase db)
        => _col = db.GetCollection<ProductReadResponse>("products_read");

    public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
    {
        /*Mongo: procure um documento que bata com o filtro(x => x.Id == e.Id).
        Se existir ? faça um DELETE.
        Se não existir ? ignore.*/
        var e = context.Message;
        await _col.DeleteOneAsync(x => x.Id == e.Id, context.CancellationToken);
    }
}
