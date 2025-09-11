using Domain.Products;
using ErrorOr;
using TestProject.Common;

namespace TestProject.Integration;

[Collection(nameof(ProductIntegrationTestCollection))]
public class CreateProductHandlerTest : ProductBaseIntegrationTest
{

    public CreateProductHandlerTest(ProductIntegrationTestFixture fixture) : base(fixture)
    {

    }

    [Fact(DisplayName = "Test 01 - should create a new product with success")]
    [Trait("CreateProductHandler", "CreateProductHandlerTest")]
    public async Task Test1()
    {

        var command = ProductFactory.CreateCommand(ExistingCategoryId);

        var result = await Sender.SendCommandAsync<CreateProductCommand, ErrorOr<Guid>>(command);


        // Act
        AssertSuccess(result); 
        AssertValidGuid(result.Value);

    }


    [Fact(DisplayName = "Test 02 - should fail to create product with invalid category id")]
    [Trait("CreateProductHandler", "CreateProductHandlerTest")]
    public async Task Test2()
    {
        var command = ProductFactory.CreateCommand(Guid.NewGuid());

        var result = await Sender.SendCommandAsync<CreateProductCommand, ErrorOr<Guid>>(command);


        // Act
        AssertError(result);
        AssertErrorOfType(result,ErrorType.NotFound);
    }

    [Fact(DisplayName = "Test 03 - should fail to try create a product with name already taken")]
    [Trait("CreateProductHandler", "CreateProductHandlerTest")]
    public async Task Test3()
    {
        Product existentProduct = ProductCreated!;

        var command = new CreateProductCommand(existentProduct.Name, 10.0m, true, ExistingCategoryId);

        var result = await Sender.SendCommandAsync<CreateProductCommand, ErrorOr<Guid>>(command);


        // Act
        AssertError(result);
        AssertErrorOfType(result, ErrorType.Conflict);
    }


}
