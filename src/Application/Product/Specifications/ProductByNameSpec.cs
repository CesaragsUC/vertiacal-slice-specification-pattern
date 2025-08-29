using Application.Product.Response;
using Application.Specifications.MongoDb;
using MongoDB.Driver;

namespace Application.Product.Specifications;

public class ProductByNameSpec : MongoSpecification<ProductReadResponse>
{
    public ProductByNameSpec(string name)
    {

        Filter = Builders<ProductReadResponse>.Filter.Eq(x => x.Name, name);

        Sort = Builders<ProductReadResponse>.Sort.Ascending(x => x.Name);
        Take = 50;
    }
}