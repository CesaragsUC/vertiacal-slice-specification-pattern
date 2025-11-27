using Application.Categories.Create;
using Cortex.Mediator;
using Domain.Categories;
using Domain.Products;
using ErrorOr;
using Infrastructure.Databases.ApplicationDbContext;
using Microsoft.Extensions.DependencyInjection;

namespace TestProject.Common;


public class ProductIntegrationTestFixture : IAsyncLifetime
{
    private ApiFactory? _factory;
    private readonly Dictionary<string, object> _sharedData = new();

    public ApiFactory Factory => _factory!;
    public Guid ExistingCategoryId { get; private set; }
    public Guid ExistingProductId { get; private set; }
    public string? ExistingProductName { get; private set; }

    public Product? ProductCreated { get; private set; }
    public Category? CategoryCreated { get; private set; }

    public async Task InitializeAsync()
    {
        _factory = new ApiFactory();
        await _factory.InitializeAsync();

        // Initialize shared test data
        await InitializeTestData();
    }

    private async Task InitializeTestData()
    {
        using var scope = _factory?.Services.CreateScope();
        var sender = scope?.ServiceProvider.GetRequiredService<IMediator>();
        var dbContext = scope?.ServiceProvider.GetRequiredService<AppDbContext>();

        // Create a category that all tests can use
        var categoryCommand = new CreateCategoryCommand("Electronics");
        var categoryResult = await sender!.SendCommandAsync<CreateCategoryCommand, ErrorOr<Guid>>(categoryCommand);
        if (!categoryResult.IsError)
        {
            ExistingCategoryId = categoryResult.Value;
        }

        // Create a product that some tests might need
        var productCommand = new CreateProductCommand("Playstation 5", 99.99m, true, ExistingCategoryId);
        var productResult = await sender.SendCommandAsync<CreateProductCommand, ErrorOr<Guid>>(productCommand);
        if (!productResult.IsError)
        {
            ExistingProductId = productResult.Value;
        }

        var productCommand2 = new CreateProductCommand("Nintendo", 41.99m, true, ExistingCategoryId);
        var productResult2 = await sender.SendCommandAsync<CreateProductCommand, ErrorOr<Guid>>(productCommand2);
        if (!productResult2.IsError)
        {
            ExistingProductId = productResult2.Value;
        }

        CategoryCreated = dbContext?.Categories?.Find(ExistingCategoryId)!;

        ProductCreated =  dbContext?.Products?.Find(ExistingProductId)!;
        ExistingProductName = ProductCreated?.Name!;

        // add more initialization here
        await SeedAdditionalData(sender, dbContext!);
    }

    private async Task SeedAdditionalData(IMediator sender, AppDbContext dbContext)
    {
        // Add any additional seeding logic here
        // For example, create users, roles, etc.
    }

    public void SetSharedData(string key, object value)
    {
        _sharedData[key] = value;
    }

    public T GetSharedData<T>(string key)
    {
        if (_sharedData.TryGetValue(key, out var value))
            return (T)value;
        return default(T)!;
    }

    public async Task DisposeAsync()
    {
        if (_factory != null)
        {
            await _factory.DisposeAsync();
        }
    }
}
