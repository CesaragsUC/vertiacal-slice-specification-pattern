using Application.Domain.Events.Products;
using Application.Extensions;
using Application.Features.Products.Dtos;
using MassTransit;
using MongoDB.Driver;

namespace YourApp.Api.Features.Products.Projections;

public sealed class ProductCreatedConsumer : IConsumer<ProductCreated>
{
    private readonly IMongoCollection<ProductReadModel> _col;
    public ProductCreatedConsumer(IMongoDatabase db)
        => _col = db.GetCollection<ProductReadModel>("products_read");

    public async Task Consume(ConsumeContext<ProductCreated> context)
    {
        /*Mongo: procure um documento que bata com o filtro(x => x.Id == e.Id).
        Se existir ? faça um UPDATE aplicando os operadores(.Set(...)).
        Se não existir ? faça um INSERT criando o documento a partir dos mesmos operadores.
        
        Por que não usar InsertOne / InsertOneAsync aqui?
        Porque seu consumidor vive num mundo at-least - once(Outbox + retries).Isso significa que o mesmo evento pode chegar 2 + vezes.
        Se você fizer InsertOne:
        Na 1ª vez: ok, insere.
        Na 2ª vez(replay/ dup): DuplicateKey(11000) no _id ? exceção, mensagem volta para a fila / retry, vira “poison”, etc. */

        var e = context.Message;
        var update = Builders<ProductReadModel>.Update
            .SetOnInsert(x => x.Id, e.Id)
            .Set(x => x.Name, e.Name)
            .Set(x => x.CategoryId, e.CategoryId)
            .Set(x => x.Category, e.Category.ToReadModel())
            .Set(x => x.Price, e.Price)
            .Set(x => x.IsActive, e.isActive);

        await _col.UpdateOneAsync(x => x.Id == e.Id, update, new UpdateOptions { IsUpsert = true }, context.CancellationToken);
    }
}