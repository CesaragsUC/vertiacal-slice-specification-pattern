namespace Application.Specifications.Product;

public sealed class ProductByIdSpec : Specification<Domain.Product>
{
    public ProductByIdSpec(System.Guid id)
    {
        Criteria = p => p.Id == id;
    }
}
