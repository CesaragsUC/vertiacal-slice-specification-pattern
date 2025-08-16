using Application.Features.Products.Dtos;
using Application.Specifications.MongoDb;
using MongoDB.Driver;
namespace Application.Specifications.Product; 
public sealed class ProductsByCategoryReadSpec:MongoSpecification<ProductReadModel>
{ 
    public ProductsByCategoryReadSpec(string category)
    { 
        if(category is not null)
            Filter = Builders<ProductReadModel>.Filter.Eq(x => x.Category!.Name, category);

        Sort =Builders<ProductReadModel>.Sort.Ascending(x=>x.Name);
        Take=50;
    } 
}