using Application.Product.Delete;
using ErrorOr;
using TestProject.Common;

namespace TestProject.Integration;

[Collection(nameof(ProductIntegrationTestCollection))]
public class DeleteProductHandlerTest : ProductBaseIntegrationTest
{

    public DeleteProductHandlerTest(ProductIntegrationTestFixture fixture) : base(fixture)
    {

    }

    [Fact(DisplayName = "Test 01 - should return a product by id with success")]
    [Trait("DeleteProductHandler", "DeleteProductHandlerTest")]
    public async Task Test1()
    {
        //TODO: use test container to retrieve a valid product id from the postgres db testecontainer

        var queryCommand = new DeleteProductCommand(Guid.NewGuid());

        var result = await Sender.SendCommandAsync<DeleteProductCommand, ErrorOr<Guid>>(queryCommand);

        // Act
        AssertSuccess(result); 
        AssertValidGuid(result.Value);
    }


    [Fact(DisplayName = "Test 02 - should fail to return a product by unexistent id")]
    [Trait("DeleteProductHandler", "DeleteProductHandlerTest")]
    public async Task Test2()
    {
        var queryCommand = new DeleteProductCommand(Guid.NewGuid());

        var result = await Sender.SendCommandAsync<DeleteProductCommand, ErrorOr<Guid>>(queryCommand);

        // Act
        AssertError(result);
        AssertErrorOfType(result,ErrorType.NotFound); 
    }
}
