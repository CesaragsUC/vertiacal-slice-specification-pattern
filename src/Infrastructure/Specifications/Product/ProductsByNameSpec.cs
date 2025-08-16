namespace Application.Specifications.Product;

public sealed class ProductsByNameSpec : Specification<Domain.Product>
{
    public ProductsByNameSpec(string name)
    {
        Criteria = p => p.Name == name;
        ApplyOrderByDescending(p => p.Price);
    }
}
