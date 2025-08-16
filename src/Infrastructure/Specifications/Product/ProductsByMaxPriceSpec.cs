namespace Application.Specifications.Product;

public sealed class ProductsByMaxPriceSpec : Specification<Domain.Product>
{
    public ProductsByMaxPriceSpec(decimal maxPrice)
    {
        Criteria = p => p.Price <= maxPrice && p.IsActive;
        ApplyOrderBy(p => p.Price);
    }
}
