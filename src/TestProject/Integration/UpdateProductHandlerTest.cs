using Application.Product.Update;
using Domain.Categories;
using Domain.Products;
using ErrorOr;
using Microsoft.EntityFrameworkCore;
using TestProject.Common;

namespace TestProject.Integration;

[Collection(nameof(ProductIntegrationTestCollection))]
public class UpdateProductHandlerTest : ProductBaseIntegrationTest
{
    public UpdateProductHandlerTest(ProductIntegrationTestFixture fixture) : base(fixture)
    {

    }

    [Fact(DisplayName = "Test 01 - should update a product with success")]
    [Trait("UpdateProductHandler", "UpdateProductHandlerTest")]
    public async Task Test1()
    {
        //TODO: use a real product saved in testecontainer postgress

        var product = DbContext.Products.FirstOrDefault();
        var command = new UpdateProductCommand(product.Id, "Xbox Series X", 200m, product.IsActive, product.CategoryId);

        var result = await Sender.SendCommandAsync<UpdateProductCommand, ErrorOr<Guid>>(command);


        // Act
        AssertSuccess(result);
        AssertValidGuid(result.Value);

    }


    [Fact(DisplayName = "Test 02 - should fail to update product with invalid category id")]
    [Trait("UpdateProductHandler", "UpdateProductHandlerTest")]
    public async Task Test2()
    {

        Guid invalidCategoryId = Guid.NewGuid();

        //TODO: use a real product saved in testecontainer postgress
        var product = new Product("Product Name", 10.0m, true, invalidCategoryId, new Category(""));

        var command = new UpdateProductCommand(product.Id, product.Name, product.Price, product.IsActive, invalidCategoryId);

        var result = await Sender.SendCommandAsync<UpdateProductCommand, ErrorOr<Guid>>(command);


        // Act
        AssertError(result);
        AssertErrorOfType(result, ErrorType.NotFound);
    }

    [Fact(DisplayName = "Test 03 - should fail to try update a product with invalid id")]
    [Trait("UpdateProductHandler", "UpdateProductHandlerTest")]
    public async Task Test3()
    {
        var categoryId = Guid.NewGuid(); //TODO: use a real category id from your seeded data saved in testecontainer postgress

        var command = new UpdateProductCommand(Guid.NewGuid(), "Iphone", 10.0m, true, categoryId);

        var result = await Sender.SendCommandAsync<UpdateProductCommand, ErrorOr<Guid>>(command);

        // Act
        AssertError(result);
        AssertErrorOfType(result, ErrorType.NotFound);
    }


    [Fact(DisplayName = "Test 04 - should fail to try update a product with name already taken")]
    [Trait("UpdateProductHandler", "UpdateProductHandlerTest")]
    public async Task Test4()
    {

        //TODO: read a existent product from testecontainer postgress firstOrDefault and use its name
        var existentProduct = ProductCreated!;

        var teste = await DbContext.Products.ToListAsync();

        //TODO: get the a product from testecontainer postgress with different name from existentProduct
        var product = await DbContext.Products.FirstOrDefaultAsync(p => !EF.Functions.ILike(p.Name, existentProduct.Name));

        product.Update(existentProduct.Name, 200.0m, false, product.CategoryId);

        var command = new UpdateProductCommand(product.Id, product.Name, product.Price, product.IsActive, product.CategoryId);

        var result = await Sender.SendCommandAsync<UpdateProductCommand, ErrorOr<Guid>>(command);

        // Act
        AssertError(result);
        AssertErrorOfType(result, ErrorType.Conflict);
    }

}