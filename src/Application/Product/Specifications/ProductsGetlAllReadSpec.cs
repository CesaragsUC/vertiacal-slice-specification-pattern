using Application.Product.Response;
using Application.Specifications.MongoDb;
using MongoDB.Driver;

namespace Application.Product.Specifications;

public class ProductsGetlAllReadSpec : MongoSpecification<ProductReadResponse>
{
    public ProductsGetlAllReadSpec(string category)
    {
        if (category is not null)
            Filter = Builders<ProductReadResponse>.Filter.Eq(x => x.Category, category);

        Sort = Builders<ProductReadResponse>.Sort.Ascending(x => x.Name);
        Take = 50;
    }
}
