using Application.Product.GetById;
using Application.Product.Response;
using ErrorOr;
using TestProject.Common;

namespace TestProject.Integration;

[Collection(nameof(ProductIntegrationTestCollection))]
public class ProductByIdQueryHandlerTest : ProductBaseIntegrationTest
{

    public ProductByIdQueryHandlerTest(ProductIntegrationTestFixture fixture) : base(fixture)
    {

    }

    [Fact(DisplayName = "Test 01 - should return a product by id with success")]
    [Trait("ProductByIdQueryHandler", "ProductByIdQueryHandlerTest")]
    public async Task Test1()
    {
        //TODO: use test container to retrieve a valid product id from the mongo db


        var queryCommand = new ProductByIdQuery(Guid.NewGuid());

        var result = await Sender.SendQueryAsync<ProductByIdQuery, ErrorOr<ProductResponse>>(queryCommand);

        // Act
        AssertSuccess(result); // check if there was no error
        AssertHasValue(result);//  should contains a product

    }


    [Fact(DisplayName = "Test 02 - should fail to return a product by unexistent id")]
    [Trait("ProductByIdQueryHandler", "ProductByIdQueryHandlerTest")]
    public async Task Test2()
    {
        var queryCommand = new ProductByIdQuery(Guid.NewGuid());//  unexistent Id

        var result = await Sender.SendQueryAsync<ProductByIdQuery, ErrorOr<ProductResponse>>(queryCommand);

        // Act
        AssertSuccess(result); // check if there was no error
        AssertNullValue(result);// should  be null

    }


}
