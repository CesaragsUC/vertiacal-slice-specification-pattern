using Bogus;
using Domain.Categories;
using Domain.Products;

namespace TestProject.Common;


public static class ProductFactory
{
    public static List<Product> CreateProductList(int total = 10)
    {
        var products = new List<Product>();
        for (int i = 0; i < total; i++)
        {
            Faker faker = new Faker("pt_BR");

            var caregory = new Category(faker.Commerce.Categories(1)[0]);

            products.Add(new Product(faker.Commerce.ProductName(), faker.Random.Decimal(1, 100), faker.Random.Bool(), Guid.NewGuid(), caregory));
        }

        return products;
    }

    public static Product CreateProduct()
    {
        Faker faker = new Faker("pt_BR");

        var caregory = new Category(faker.Commerce.Categories(1)[0]);

        var product = new Product(faker.Commerce.ProductName(), faker.Random.Decimal(1, 100), faker.Random.Bool(), Guid.NewGuid(), caregory);

        return product;
    }

    public static List<CreateProductCommand> CreateProductCommandList(int total = 10)
    {
        var commands = new List<CreateProductCommand>();
        for (int i = 0; i < total; i++)
        {
            Faker faker = new Faker("pt_BR");
            commands.Add(new CreateProductCommand(faker.Commerce.ProductName(), faker.Random.Decimal(1, 100), faker.Random.Bool(), Guid.NewGuid()));
        }
        return commands;
    }

    public static CreateProductCommand CreateCommand(Guid gategoryId)
    {
        Faker faker = new Faker("pt_BR");
        var commands = new CreateProductCommand(faker.Commerce.ProductName(), faker.Random.Decimal(1, 100), faker.Random.Bool(), gategoryId);
        return commands;
    }
}