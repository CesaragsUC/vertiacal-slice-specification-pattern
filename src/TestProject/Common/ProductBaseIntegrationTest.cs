using Cortex.Mediator;
using Domain.Categories;
using Domain.Products;
using ErrorOr;
using Infrastructure.Databases.ApplicationDbContext;
using Microsoft.Extensions.DependencyInjection;


namespace TestProject.Common;

public abstract class ProductBaseIntegrationTest : ErrorOrAssertions, IDisposable
{
    private readonly IServiceScope _scope;
    protected readonly IMediator Sender;
    protected readonly AppDbContext DbContext;
    protected readonly ProductIntegrationTestFixture Fixture;


    // Direct access to shared data
    protected Guid ExistingCategoryId => Fixture.ExistingCategoryId;
    protected Guid ExistingProductId => Fixture.ExistingProductId;
    protected string ExistingProductName => Fixture.ExistingProductName;
    protected Product? ProductCreated => Fixture.ProductCreated;
    protected Category? CategoryCreated => Fixture.CategoryCreated;

    protected ProductBaseIntegrationTest(ProductIntegrationTestFixture fixture)
    {
        Fixture = fixture;

        _scope = fixture.Factory.Services.CreateScope();

        Sender = _scope.ServiceProvider.GetRequiredService<IMediator>();

        DbContext = _scope.ServiceProvider
            .GetRequiredService<AppDbContext>();
    }

    public void Dispose()
    {
        _scope?.Dispose();
        DbContext?.Dispose();
    }
}
