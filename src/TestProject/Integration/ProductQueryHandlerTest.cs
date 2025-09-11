using Application.Product.GetAll;
using Application.Product.Response;
using ErrorOr;
using TestProject.Common;

namespace TestProject.Integration;

[Collection(nameof(ProductIntegrationTestCollection))]
public class ListProductsQueryHandlerTest : ProductBaseIntegrationTest
{

    public ListProductsQueryHandlerTest(ProductIntegrationTestFixture fixture) : base(fixture)
    {
        
    }

    [Fact(DisplayName = "Test 01 - should return a product list with success")]
    [Trait("ListProductsQueryHandler", "ListProductsQueryHandlerTest")]
    public async Task Test1()
    {
        var queryCommand = new GetProductsQuery("PC");

       var result =  await Sender.SendQueryAsync<GetProductsQuery, ErrorOr<IEnumerable<ProductResponse>>>(queryCommand);

        // Act
        AssertSuccess(result); // check if there was no error
        AssertNonEmptyCollection(result);//  should be a non empty collection

    }


    [Fact(DisplayName = "Test 02 - should fail to return a product list by cars category")]
    [Trait("ListProductsQueryHandler", "ListProductsQueryHandlerTest")]
    public async Task Test2()
    {
        var queryCommand = new GetProductsQuery("Cars");//  unexistent category

        var result = await Sender.SendQueryAsync<GetProductsQuery, ErrorOr<IEnumerable<ProductResponse>>>(queryCommand);

        // Act
        AssertSuccess(result); // check if there was no error
        AssertEmptyCollection(result);// should be an empty collection

    }


}
