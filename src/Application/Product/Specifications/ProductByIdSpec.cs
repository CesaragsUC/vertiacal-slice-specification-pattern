using Application.Product.Response;
using Application.Specifications.MongoDb;
using MongoDB.Driver;

namespace Application.Product.Specifications;

public class ProductByIdSpec : MongoSpecification<ProductReadResponse>
{
    public ProductByIdSpec(Guid id)
    {

        Filter = Builders<ProductReadResponse>.Filter.Eq(x => x.Id, id);

        Sort = Builders<ProductReadResponse>.Sort.Ascending(x => x.Name);
        Take = 50;
    }
}