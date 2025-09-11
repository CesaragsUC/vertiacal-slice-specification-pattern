namespace TestProject.Common;


[CollectionDefinition(nameof(ProductIntegrationTestCollection))]
public sealed class ProductIntegrationTestCollection : ICollectionFixture<ProductIntegrationTestFixture>
{
}
